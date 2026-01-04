namespace LlmUploadDeleteDataSet
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnDeleteAllFiles = new Button();
            btnAddFiles = new Button();
            panel1 = new Panel();
            tbApiKey = new TextBox();
            label1 = new Label();
            panel2 = new Panel();
            tbFilePath = new TextBox();
            label2 = new Label();
            tbLog = new TextBox();
            panel3 = new Panel();
            tbBaseUrl = new TextBox();
            label3 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // btnDeleteAllFiles
            // 
            btnDeleteAllFiles.Dock = DockStyle.Bottom;
            btnDeleteAllFiles.Location = new Point(0, 427);
            btnDeleteAllFiles.Name = "btnDeleteAllFiles";
            btnDeleteAllFiles.Size = new Size(800, 23);
            btnDeleteAllFiles.TabIndex = 0;
            btnDeleteAllFiles.Text = "Delete all files";
            btnDeleteAllFiles.UseVisualStyleBackColor = true;
            btnDeleteAllFiles.Click += btnDeleteAllFiles_Click;
            // 
            // btnAddFiles
            // 
            btnAddFiles.Dock = DockStyle.Bottom;
            btnAddFiles.Location = new Point(0, 404);
            btnAddFiles.Name = "btnAddFiles";
            btnAddFiles.Size = new Size(800, 23);
            btnAddFiles.TabIndex = 1;
            btnAddFiles.Text = "Add files";
            btnAddFiles.UseVisualStyleBackColor = true;
            btnAddFiles.Click += btnAddFiles_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(tbApiKey);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 30);
            panel1.TabIndex = 4;
            // 
            // tbApiKey
            // 
            tbApiKey.Dock = DockStyle.Top;
            tbApiKey.Location = new Point(49, 0);
            tbApiKey.Name = "tbApiKey";
            tbApiKey.Size = new Size(751, 23);
            tbApiKey.TabIndex = 5;
            tbApiKey.Leave += tbApiKey_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 4;
            label1.Text = "Api key:";
            // 
            // panel2
            // 
            panel2.Controls.Add(tbFilePath);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 30);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 30);
            panel2.TabIndex = 5;
            // 
            // tbFilePath
            // 
            tbFilePath.Dock = DockStyle.Top;
            tbFilePath.Location = new Point(55, 0);
            tbFilePath.Name = "tbFilePath";
            tbFilePath.Size = new Size(745, 23);
            tbFilePath.TabIndex = 5;
            tbFilePath.Leave += tbApiKey_Leave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 4;
            label2.Text = "File path:";
            // 
            // tbLog
            // 
            tbLog.Dock = DockStyle.Fill;
            tbLog.Location = new Point(0, 90);
            tbLog.Multiline = true;
            tbLog.Name = "tbLog";
            tbLog.Size = new Size(800, 314);
            tbLog.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(tbBaseUrl);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 60);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 30);
            panel3.TabIndex = 7;
            // 
            // tbBaseUrl
            // 
            tbBaseUrl.Dock = DockStyle.Top;
            tbBaseUrl.Location = new Point(52, 0);
            tbBaseUrl.Name = "tbBaseUrl";
            tbBaseUrl.Size = new Size(748, 23);
            tbBaseUrl.TabIndex = 5;
            tbBaseUrl.Leave += tbApiKey_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Left;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 4;
            label3.Text = "Base Url:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tbLog);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(btnAddFiles);
            Controls.Add(btnDeleteAllFiles);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDeleteAllFiles;
        private Button btnAddFiles;
        private Panel panel1;
        private TextBox tbApiKey;
        private Label label1;
        private Panel panel2;
        private TextBox tbFilePath;
        private Label label2;
        private TextBox tbLog;
        private Panel panel3;
        private TextBox tbBaseUrl;
        private Label label3;
    }
}
