using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace LlmUploadDeleteDataSet;

public partial class Form1 : Form
{

    private const string JsonConfig = "jsconfig.json";

    public Form1()
    {
        InitializeComponent();
        if (File.Exists(JsonConfig))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(JsonConfig)
                .Build();

            string? apiKeyValue = configuration.GetSection("apiKey").Value;
            if (apiKeyValue is not null) tbApiKey.Text = apiKeyValue;

            string? filePathValue = configuration.GetSection("filePath").Value;
            if (filePathValue is not null) tbFilePath.Text = filePathValue;

            string? baseUrlValue = configuration.GetSection("baseUrl").Value;
            if (baseUrlValue is not null) tbBaseUrl.Text = baseUrlValue;
        }
    }

    private void tbApiKey_Leave(object sender, EventArgs e)
    {
        var jsonConfig = new JObject
        {
            ["apiKey"] = tbApiKey.Text,
            ["filePath"] = tbFilePath.Text,
            ["baseUrl"] = tbBaseUrl.Text
        };
        File.WriteAllText(JsonConfig, jsonConfig.ToString());
    }

    private async void btnDeleteAllFiles_Click(object sender, EventArgs e)
    {
        await CommonMethodsForOpenWebUiApi.DeleteAllFiles(tbBaseUrl.Text, tbApiKey.Text, tbLog);
    }

    private async void btnAddFiles_Click(object sender, EventArgs e)
    {
        //await CommonMethodsForOpenWebUiApi.AddAllFilesFromPath(tbBaseUrl.Text, tbApiKey.Text, tbFilePath.Text, tbLog);
        await CommonMethodsForOpenWebUiApi.AddAllFilesFromPath(tbBaseUrl.Text
            , tbApiKey.Text
            , tbFilePath.Text
            , "TEST"
            , "test"
            , tbLog);
    }
}