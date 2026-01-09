using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LlmUploadDeleteDataSet;

public static class CommonMethodsForOpenWebUiApi
{
    public static async Task DeleteAllFiles(string baseUrl, string apiKey, TextBox tbLog)
    {
        tbLog.Clear();
        CancellationToken ct = CancellationToken.None;
        baseUrl = baseUrl.TrimEnd('/');

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var listUrl = $"{baseUrl}/api/v1/files/";
        var listOfFiles = await http.GetStringAsync(listUrl, ct);
        var jArrayListOfFiles = JArray.Parse(listOfFiles);
        foreach (JToken jToken in jArrayListOfFiles)
        {
            string? fileId = jToken["id"]?.ToString();
            if (fileId is not null)
            {
                var delUrl = $"{baseUrl}/api/v1/files/{Uri.EscapeDataString(fileId)}";
                var resp = await http.DeleteAsync(delUrl, ct);
                tbLog.AppendText($"{fileId}: {resp.IsSuccessStatusCode} {Environment.NewLine}");
            }
        }
        tbLog.AppendText($"Done DeleteAllFiles {Environment.NewLine}");
    }

    public static async Task AddAllFilesFromPath(string baseUrl, string apiKey, string filePath, TextBox tbLog)
    {
        tbLog.Clear();
        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        using var http = new HttpClient(handler);
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var listOfFiles = Directory.GetFiles(filePath);

        foreach (string llmFile in listOfFiles)
        {
            var fileId = await UploadFile(baseUrl, http, llmFile);
            var knowledgeId = await CreateKnowledgeBase(baseUrl, apiKey, llmFile, llmFile);
            await AddFileToKnowledge(baseUrl, http, knowledgeId, fileId);
            tbLog.AppendText($"{fileId}{Environment.NewLine}");
        }
        tbLog.AppendText($"Done AddAllFilesFromPath {Environment.NewLine}");
    }

    /// <summary>
    /// Creates ONE knowledge base (collection) and adds ALL files from the folder to it.
    /// Returns the created knowledgeId.
    /// </summary>
    public static async Task<string> AddAllFilesFromPath(
        string baseUrl,
        string apiKey,
        string folderPath,
        string knowledgeName,
        string knowledgeDescription,
        TextBox tbLog,
        CancellationToken ct = default)
    {
        tbLog.Clear();
        baseUrl = baseUrl.TrimEnd('/');

        // One shared HttpClient for all calls
        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        using var http = new HttpClient(handler);
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        // 1) Create ONE knowledge base for the whole folder
        var knowledgeId = await CreateKnowledgeBase(baseUrl, http, knowledgeName, knowledgeDescription, ct);
        tbLog.AppendText($"Knowledge created: {knowledgeId}{Environment.NewLine}");

        // 2) Upload and add every file into that knowledge base
        var files = Directory.GetFiles(folderPath);
        var listOfAddedFiles = new List<string>();
        foreach (var file in files)
        {
            if (CheckIfSameFileAlreadyAdded(listOfAddedFiles, file)) continue;

            try
            {
                var fileId = await UploadFile(baseUrl, http, file, ct);
                await AddFileToKnowledge(baseUrl, http, knowledgeId, fileId, ct);

                tbLog.AppendText($"OK  file={Path.GetFileName(file)}  fileId={fileId}{Environment.NewLine}");
                listOfAddedFiles.Add(file);

            }
            catch (Exception ex)
            {
                tbLog.AppendText($"ERR file={Path.GetFileName(file)}  {ex.Message}{Environment.NewLine}");
            }
        }

        tbLog.AppendText($"Done AddAllFilesFromPath. knowledgeId={knowledgeId}{Environment.NewLine}");
        return knowledgeId;
    }

    private static bool CheckIfSameFileAlreadyAdded(List<string> listOfAddedFiles, string originalFile)
    {
        return listOfAddedFiles.Any() && listOfAddedFiles.Any(addedFile => CompareTxtFiles(addedFile, originalFile));
    }

    static bool CompareTxtFiles(string file1, string file2)
    {
        string text1 = File.ReadAllText(file1);
        string text2 = File.ReadAllText(file2);

        text1 = Regex.Replace(text1, @"\s+", "");
        text2 = Regex.Replace(text2, @"\s+", "");

        return string.Equals(text1, text2, StringComparison.OrdinalIgnoreCase);
    }

    private static async Task AddFileToKnowledge(
        string baseUrl,
        HttpClient http,
        string knowledgeId,
        string fileId,
        CancellationToken ct)
    {
        baseUrl = baseUrl.TrimEnd('/');

        var payload = JsonSerializer.Serialize(new { file_id = fileId });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var resp = await http.PostAsync($"{baseUrl}/api/v1/knowledge/{knowledgeId}/file/add", content, ct);
        resp.EnsureSuccessStatusCode();
    }

    private static async Task<string> UploadFile(
        string baseUrl,
        HttpClient http,
        string filePath,
        CancellationToken ct)
    {
        baseUrl = baseUrl.TrimEnd('/');

        using var form = new MultipartFormDataContent();
        await using var fs = File.OpenRead(filePath);

        var fileContent = new StreamContent(fs);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        form.Add(fileContent, "file", Path.GetFileName(filePath));

        using var resp = await http.PostAsync($"{baseUrl}/api/v1/files/", form, ct);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync(ct);

        using var doc = JsonDocument.Parse(json);
        var id = doc.RootElement.GetProperty("id").GetString();

        if (string.IsNullOrWhiteSpace(id))
            throw new Exception("Upload succeeded but no file id returned.");

        return id;
    }

    private static async Task<string> CreateKnowledgeBase(
        string baseUrl,
        HttpClient http,
        string name,
        string description,
        CancellationToken ct)
    {
        baseUrl = baseUrl.TrimEnd('/');

        var payload = JsonSerializer.Serialize(new { name, description });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var resp = await http.PostAsync($"{baseUrl}/api/v1/knowledge/create", content, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        resp.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("id").GetString()
               ?? throw new Exception("Create succeeded but no 'id' returned.");
    }

    static async Task<string> CreateKnowledgeBase(string baseUrl, string apiKey, string name, string description)
    {
        using var http = new HttpClient();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var payload = JsonSerializer.Serialize(new { name, description });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var resp = await http.PostAsync($"{baseUrl}/api/v1/knowledge/create", content);
        var body = await resp.Content.ReadAsStringAsync();

        resp.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("id").GetString()
               ?? throw new Exception("Create succeeded but no 'id' returned.");
    }

    static async Task AddFileToKnowledge(string baseUrl, HttpClient http, string knowledgeId, string fileId)
    {
        var payload = JsonSerializer.Serialize(new { file_id = fileId });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var resp = await http.PostAsync($"{baseUrl}/api/v1/knowledge/{knowledgeId}/file/add", content);
        resp.EnsureSuccessStatusCode();
    }

    static async Task<string> UploadFile(string baseUrl, HttpClient http, string filePath)
    {
        using var form = new MultipartFormDataContent();
        await using var fs = File.OpenRead(filePath);
        baseUrl = baseUrl.TrimEnd('/');

        var fileContent = new StreamContent(fs);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        form.Add(fileContent, "file", Path.GetFileName(filePath));

        var requestUri = $"{baseUrl}/api/v1/files/";
        var resp = await http.PostAsync(requestUri, form);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();

        // Open WebUI returns a JSON object containing the uploaded file metadata including id.
        using var doc = JsonDocument.Parse(json);
        var id = doc.RootElement.GetProperty("id").GetString();

        if (string.IsNullOrWhiteSpace(id))
            throw new Exception("Upload succeeded but no file id returned.");

        return id;
    }
}