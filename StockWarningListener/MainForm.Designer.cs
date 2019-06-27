namespace StockWarningListener
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_TestSendMsg = new System.Windows.Forms.Button();
            this.comboBox_QQWindows = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_SendType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_GetAllWindows = new System.Windows.Forms.Button();
            this.textBox_FilePath = new System.Windows.Forms.TextBox();
            this.button_selectFilePath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_TestSendMsg
            // 
            this.button_TestSendMsg.Location = new System.Drawing.Point(285, 199);
            this.button_TestSendMsg.Name = "button_TestSendMsg";
            this.button_TestSendMsg.Size = new System.Drawing.Size(136, 29);
            this.button_TestSendMsg.TabIndex = 0;
            this.button_TestSendMsg.Text = "测试发送消息";
            this.button_TestSendMsg.UseVisualStyleBackColor = true;
            this.button_TestSendMsg.Click += new System.EventHandler(this.button_TestSendMsg_Click);
            // 
            // comboBox_QQWindows
            // 
            this.comboBox_QQWindows.FormattingEnabled = true;
            this.comboBox_QQWindows.Location = new System.Drawing.Point(120, 59);
            this.comboBox_QQWindows.Name = "comboBox_QQWindows";
            this.comboBox_QQWindows.Size = new System.Drawing.Size(121, 23);
            this.comboBox_QQWindows.TabIndex = 1;
            this.comboBox_QQWindows.SelectedIndexChanged += new System.EventHandler(this.ComboBox_QQWindows_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "QQ窗口";
            // 
            // comboBox_SendType
            // 
            this.comboBox_SendType.FormattingEnabled = true;
            this.comboBox_SendType.Items.AddRange(new object[] {
            "发送文字",
            "发送txt文件"});
            this.comboBox_SendType.Location = new System.Drawing.Point(120, 100);
            this.comboBox_SendType.Name = "comboBox_SendType";
            this.comboBox_SendType.Size = new System.Drawing.Size(121, 23);
            this.comboBox_SendType.TabIndex = 3;
            this.comboBox_SendType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SendType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "发送方式";
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(285, 235);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(136, 28);
            this.button_Start.TabIndex = 5;
            this.button_Start.Text = "开始检测";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // button_GetAllWindows
            // 
            this.button_GetAllWindows.Location = new System.Drawing.Point(261, 54);
            this.button_GetAllWindows.Name = "button_GetAllWindows";
            this.button_GetAllWindows.Size = new System.Drawing.Size(121, 30);
            this.button_GetAllWindows.TabIndex = 6;
            this.button_GetAllWindows.Text = "获取QQ窗口";
            this.button_GetAllWindows.UseVisualStyleBackColor = true;
            this.button_GetAllWindows.Click += new System.EventHandler(this.Button_GetAllWindows_Click);
            // 
            // textBox_FilePath
            // 
            this.textBox_FilePath.Enabled = false;
            this.textBox_FilePath.Location = new System.Drawing.Point(120, 13);
            this.textBox_FilePath.Multiline = true;
            this.textBox_FilePath.Name = "textBox_FilePath";
            this.textBox_FilePath.Size = new System.Drawing.Size(262, 25);
            this.textBox_FilePath.TabIndex = 7;
            // 
            // button_selectFilePath
            // 
            this.button_selectFilePath.Location = new System.Drawing.Point(388, 11);
            this.button_selectFilePath.Name = "button_selectFilePath";
            this.button_selectFilePath.Size = new System.Drawing.Size(78, 27);
            this.button_selectFilePath.TabIndex = 8;
            this.button_selectFilePath.Text = "浏览…";
            this.button_selectFilePath.UseVisualStyleBackColor = true;
            this.button_selectFilePath.Click += new System.EventHandler(this.Button_selectFilePath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "监控文件";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 318);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_selectFilePath);
            this.Controls.Add(this.textBox_FilePath);
            this.Controls.Add(this.button_GetAllWindows);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_SendType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_QQWindows);
            this.Controls.Add(this.button_TestSendMsg);
            this.Name = "MainForm";
            this.Text = "股票预警检测程序";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_TestSendMsg;
        private System.Windows.Forms.ComboBox comboBox_QQWindows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_SendType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_GetAllWindows;
        private System.Windows.Forms.TextBox textBox_FilePath;
        private System.Windows.Forms.Button button_selectFilePath;
        private System.Windows.Forms.Label label3;
    }
}

