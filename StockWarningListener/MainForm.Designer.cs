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
            System.Windows.Forms.Button button_YHClientPath;
            System.Windows.Forms.Button button_saveUser;
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
            this.checkBox_CallPhone = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_YHClientPath = new System.Windows.Forms.TextBox();
            this.textBox_YHUserName = new System.Windows.Forms.TextBox();
            this.textBox_YHPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button_getBalance = new System.Windows.Forms.Button();
            this.label_available = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label_balance = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView_warehouse = new System.Windows.Forms.DataGridView();
            this.stockCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.canSellQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.marketValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_selectDZHPath = new System.Windows.Forms.Button();
            this.textBox_dzhPath = new System.Windows.Forms.TextBox();
            button_YHClientPath = new System.Windows.Forms.Button();
            button_saveUser = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_warehouse)).BeginInit();
            this.SuspendLayout();
            // 
            // button_YHClientPath
            // 
            button_YHClientPath.Location = new System.Drawing.Point(889, 11);
            button_YHClientPath.Name = "button_YHClientPath";
            button_YHClientPath.Size = new System.Drawing.Size(78, 27);
            button_YHClientPath.TabIndex = 12;
            button_YHClientPath.Text = "浏览…";
            button_YHClientPath.UseVisualStyleBackColor = true;
            button_YHClientPath.Click += new System.EventHandler(this.button_YHClientPath_Click);
            // 
            // button_saveUser
            // 
            button_saveUser.Location = new System.Drawing.Point(889, 52);
            button_saveUser.Name = "button_saveUser";
            button_saveUser.Size = new System.Drawing.Size(87, 65);
            button_saveUser.TabIndex = 18;
            button_saveUser.Text = "保存账号";
            button_saveUser.UseVisualStyleBackColor = true;
            button_saveUser.Click += new System.EventHandler(this.button_saveUser_Click);
            // 
            // button_TestSendMsg
            // 
            this.button_TestSendMsg.Location = new System.Drawing.Point(142, 145);
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
            this.button_Start.Location = new System.Drawing.Point(284, 145);
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
            this.label3.Location = new System.Drawing.Point(533, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "大智慧目录";
            // 
            // checkBox_CallPhone
            // 
            this.checkBox_CallPhone.AutoSize = true;
            this.checkBox_CallPhone.Checked = true;
            this.checkBox_CallPhone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_CallPhone.Location = new System.Drawing.Point(261, 104);
            this.checkBox_CallPhone.Name = "checkBox_CallPhone";
            this.checkBox_CallPhone.Size = new System.Drawing.Size(135, 19);
            this.checkBox_CallPhone.TabIndex = 10;
            this.checkBox_CallPhone.Text = "同时拨打QQ电话";
            this.checkBox_CallPhone.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "银河证券路径";
            // 
            // textBox_YHClientPath
            // 
            this.textBox_YHClientPath.Enabled = false;
            this.textBox_YHClientPath.Location = new System.Drawing.Point(621, 13);
            this.textBox_YHClientPath.Multiline = true;
            this.textBox_YHClientPath.Name = "textBox_YHClientPath";
            this.textBox_YHClientPath.Size = new System.Drawing.Size(262, 25);
            this.textBox_YHClientPath.TabIndex = 11;
            // 
            // textBox_YHUserName
            // 
            this.textBox_YHUserName.Location = new System.Drawing.Point(621, 52);
            this.textBox_YHUserName.Multiline = true;
            this.textBox_YHUserName.Name = "textBox_YHUserName";
            this.textBox_YHUserName.Size = new System.Drawing.Size(262, 25);
            this.textBox_YHUserName.TabIndex = 14;
            // 
            // textBox_YHPassword
            // 
            this.textBox_YHPassword.Location = new System.Drawing.Point(621, 93);
            this.textBox_YHPassword.Multiline = true;
            this.textBox_YHPassword.Name = "textBox_YHPassword";
            this.textBox_YHPassword.Size = new System.Drawing.Size(262, 25);
            this.textBox_YHPassword.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(518, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "银河证券账号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(518, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "银河证券密码";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 180);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1059, 276);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_getBalance);
            this.tabPage1.Controls.Add(this.label_available);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label_balance);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.dataGridView_warehouse);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1051, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "资金股份";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_getBalance
            // 
            this.button_getBalance.Location = new System.Drawing.Point(938, 7);
            this.button_getBalance.Name = "button_getBalance";
            this.button_getBalance.Size = new System.Drawing.Size(106, 23);
            this.button_getBalance.TabIndex = 24;
            this.button_getBalance.Text = "刷新持仓";
            this.button_getBalance.UseVisualStyleBackColor = true;
            this.button_getBalance.Click += new System.EventHandler(this.button_getBalance_Click);
            // 
            // label_available
            // 
            this.label_available.ForeColor = System.Drawing.Color.Red;
            this.label_available.Location = new System.Drawing.Point(202, 17);
            this.label_available.Name = "label_available";
            this.label_available.Size = new System.Drawing.Size(100, 15);
            this.label_available.TabIndex = 23;
            this.label_available.Text = "0.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 15);
            this.label9.TabIndex = 22;
            this.label9.Text = "可用";
            // 
            // label_balance
            // 
            this.label_balance.ForeColor = System.Drawing.Color.Red;
            this.label_balance.Location = new System.Drawing.Point(53, 17);
            this.label_balance.Name = "label_balance";
            this.label_balance.Size = new System.Drawing.Size(100, 15);
            this.label_balance.TabIndex = 21;
            this.label_balance.Text = "0.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 20;
            this.label7.Text = "余额";
            // 
            // dataGridView_warehouse
            // 
            this.dataGridView_warehouse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_warehouse.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stockCode,
            this.stockName,
            this.qty,
            this.canSellQty,
            this.newPrice,
            this.marketValue,
            this.cost});
            this.dataGridView_warehouse.Location = new System.Drawing.Point(7, 35);
            this.dataGridView_warehouse.Name = "dataGridView_warehouse";
            this.dataGridView_warehouse.ReadOnly = true;
            this.dataGridView_warehouse.RowHeadersWidth = 51;
            this.dataGridView_warehouse.RowTemplate.Height = 27;
            this.dataGridView_warehouse.Size = new System.Drawing.Size(1038, 206);
            this.dataGridView_warehouse.TabIndex = 0;
            // 
            // stockCode
            // 
            this.stockCode.HeaderText = "证券代码";
            this.stockCode.MinimumWidth = 6;
            this.stockCode.Name = "stockCode";
            this.stockCode.ReadOnly = true;
            this.stockCode.Width = 125;
            // 
            // stockName
            // 
            this.stockName.HeaderText = "证券名称";
            this.stockName.MinimumWidth = 6;
            this.stockName.Name = "stockName";
            this.stockName.ReadOnly = true;
            this.stockName.Width = 125;
            // 
            // qty
            // 
            this.qty.HeaderText = "股票数量";
            this.qty.MinimumWidth = 6;
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 125;
            // 
            // canSellQty
            // 
            this.canSellQty.HeaderText = "可卖数量";
            this.canSellQty.MinimumWidth = 6;
            this.canSellQty.Name = "canSellQty";
            this.canSellQty.ReadOnly = true;
            this.canSellQty.Width = 125;
            // 
            // newPrice
            // 
            this.newPrice.HeaderText = "最新价";
            this.newPrice.MinimumWidth = 6;
            this.newPrice.Name = "newPrice";
            this.newPrice.ReadOnly = true;
            this.newPrice.Width = 125;
            // 
            // marketValue
            // 
            this.marketValue.HeaderText = "股票市值";
            this.marketValue.MinimumWidth = 6;
            this.marketValue.Name = "marketValue";
            this.marketValue.ReadOnly = true;
            this.marketValue.Width = 125;
            // 
            // cost
            // 
            this.cost.HeaderText = "盈亏/成本";
            this.cost.MinimumWidth = 6;
            this.cost.Name = "cost";
            this.cost.ReadOnly = true;
            this.cost.Width = 125;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1051, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_selectDZHPath
            // 
            this.button_selectDZHPath.Location = new System.Drawing.Point(889, 131);
            this.button_selectDZHPath.Name = "button_selectDZHPath";
            this.button_selectDZHPath.Size = new System.Drawing.Size(78, 27);
            this.button_selectDZHPath.TabIndex = 21;
            this.button_selectDZHPath.Text = "浏览…";
            this.button_selectDZHPath.UseVisualStyleBackColor = true;
            this.button_selectDZHPath.Click += new System.EventHandler(this.button_selectDZHPath_Click);
            // 
            // textBox_dzhPath
            // 
            this.textBox_dzhPath.Enabled = false;
            this.textBox_dzhPath.Location = new System.Drawing.Point(621, 133);
            this.textBox_dzhPath.Multiline = true;
            this.textBox_dzhPath.Name = "textBox_dzhPath";
            this.textBox_dzhPath.Size = new System.Drawing.Size(262, 25);
            this.textBox_dzhPath.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 468);
            this.Controls.Add(this.button_selectDZHPath);
            this.Controls.Add(this.textBox_dzhPath);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(button_saveUser);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_YHPassword);
            this.Controls.Add(this.textBox_YHUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(button_YHClientPath);
            this.Controls.Add(this.textBox_YHClientPath);
            this.Controls.Add(this.checkBox_CallPhone);
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
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_warehouse)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBox_CallPhone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_YHClientPath;
        private System.Windows.Forms.TextBox textBox_YHUserName;
        private System.Windows.Forms.TextBox textBox_YHPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn canSellQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn newPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn marketValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label_available;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label_balance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_getBalance;
        private System.Windows.Forms.Button button_selectDZHPath;
        private System.Windows.Forms.TextBox textBox_dzhPath;
    }
}

