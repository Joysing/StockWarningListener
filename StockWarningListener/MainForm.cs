using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Configuration;
using TCMS.DBUtility;
using System.Data;
using System.Net;

namespace StockWarningListener
{
    public partial class MainForm : Form
    {
        
        const int MOUSEEVENTF_MOVE = 0x0001;//移动鼠标 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;//模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;//模拟鼠标右键抬起 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;//模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;//模拟鼠标中键抬起 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;//标示是否采用绝对坐标 （左上角为[0,0]，右下角为[65535,65535]）
        


        //System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
        System.Timers.Timer CheckBuyTimer = new System.Timers.Timer(100);//0.1秒执行一次
        System.Timers.Timer CheckSellTimer = new System.Timers.Timer(500);//0.5秒执行一次
        System.Timers.Timer CheckClientTimer = new System.Timers.Timer(10000);//10秒检测一次
        int SendCount = 0;//已发送的行数
        int SendType = 0;//发送文字还是文件
        string FilePath = @"D:\\dzh365\\WarningTxt";
        string QQWindowName = "";
        IntPtr QQWindowHandle;//QQ窗口句柄
        string username = "";
        string password = "";
        string dzhPath = "";

        public MainForm()
        {
            InitializeComponent();
            comboBox_SendType.SelectedIndex = 0;
            FilePath = ConfigurationManager.AppSettings["FilePath"].ToString().Trim();
            textBox_FilePath.Text = AppSettingUtils.GetAppSettingsValue("FilePath");
            textBox_YHClientPath.Text = AppSettingUtils.GetAppSettingsValue("YHClientPath");

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetAllDesktopWindows();
            string en_US = "00000409"; //英文
            //string cn_ZH = "00000804";
            uint KLF_ACTIVATE = 1;
            PostMessage(0xffff, 0x00500, IntPtr.Zero, LoadKeyboardLayout(en_US, KLF_ACTIVATE));//屏蔽中文输入法

            CheckBuyTimer.Elapsed += delegate
            {
                Thread thread = new Thread(CheckBuy);
                thread.SetApartmentState(ApartmentState.STA);//要在线程里使用剪切板，必须设为STA模式
                thread.Start();
            };
            CheckBuyTimer.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            CheckSellTimer.Elapsed += delegate
            {
                Thread thread = new Thread(CheckSell);
                thread.SetApartmentState(ApartmentState.STA);//要在线程里使用剪切板，必须设为STA模式
                thread.Start();
            };
            CheckSellTimer.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            CheckSellTimer.Start();
            CheckClientTimer.Elapsed += delegate
            {
                Thread thread1 = new Thread(CheckClient);
                //thread1.SetApartmentState(ApartmentState.STA);
                thread1.Start();
            };
            CheckClientTimer.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            CheckClientTimer.Start();

            DataSet data = DBHelperSQLite.Query("select * from t_user WHERE clientType='YH'");
            if (data.Tables[0].Rows.Count > 0)
            {
                username= Convert.ToString(data.Tables[0].Rows[0]["Username"]);
                password= Convert.ToString(data.Tables[0].Rows[0]["Password"]);
                textBox_YHUserName.Text = username;
                textBox_YHPassword.Text = password;
            }
            DataSet sysData = DBHelperSQLite.Query("select * from t_sysconfig");
            if (sysData.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dataRow in sysData.Tables[0].Rows)
                {
                    switch(Convert.ToString(dataRow["configName"]))
                    {
                        case "dzhPath":
                            dzhPath = Convert.ToString(dataRow["configValue"]);
                            textBox_dzhPath.Text = dzhPath;
                            break;
                        default:
                            break;
                    }
                }
            }
            
            //getStockNowPrice(new List<string>(){ "sh601006","sh601007"});
            
        }

        private void button_TestSendMsg_Click(object sender, EventArgs e)
        {
            //getStockNowPrice(new List<string>(){ "sh601006","sh601007"});

        }



        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        /// <summary>
        /// 找到窗口
        /// </summary>
        /// <param name="lpClassName">窗口类名(例：Button)</param>
        /// <param name="lpWindowName">窗口标题</param>
        /// <returns>窗口句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
   
        //把窗体置于最前
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //发送消息
        /// <summary>
        /// 发送QQ消息
        /// </summary>
        /// <param name="aioName">聊天窗口名</param>
        /// <param name="info">聊天内容</param>
        public void SendMessage(string info)
        {
            QQWindowHandle = FindWindow(null, QQWindowName);
            if (QQWindowHandle.ToString() != "0")
            {
                SetForegroundWindow(QQWindowHandle);             //把窗体置于最前
                SendKeys.SendWait(info);
                SendKeys.SendWait("{ENTER}");//Enter
                //SendKeys.Send("^{ENTER}");//Ctrl+Enter
            }
            else
            {
                MessageBox.Show("木有找到这个聊天窗口");
            }
        }

        /// <summary>
        /// 使用剪切板内容发送QQ消息
        /// </summary>
        public void SendMessageByClipboard()
        {
            QQWindowHandle = FindWindow(null, QQWindowName);
            if (QQWindowHandle.ToString() != "0")
            {
                SetForegroundWindow(QQWindowHandle); //把窗体置于最前
                SendKeys.SendWait("^V");//粘贴
                SendKeys.SendWait("{ENTER}");//Enter
                //SendKeys.Send("^{ENTER}");//Ctrl+Enter
            }
            else
            {
                MessageBox.Show("木有找到这个聊天窗口");
            }
        }

        

        private void Button_Start_Click(object sender, EventArgs e)
        {
            switch (button_Start.Text)
            {
                case "开始检测":
                    if(!YH_Client.AutoLogin(dataGridView_warehouse, username, password))
                    {
                        return;
                    }
                    if ("".Equals(dzhPath))
                    {
                        MessageBox.Show("请先选择大智慧安装目录！");
                        return;
                    }
                    CheckBuyTimer.Start();
                    button_Start.Text = "停止检测";
                    break;
                case "停止检测":
                    CheckBuyTimer.Stop();
                    button_Start.Text = "开始检测";
                    break;
            }
            
        }
        private void CheckClient()
        {
            YH_Client.AutoLogin(dataGridView_warehouse, username, password);
            
        }
        private void CheckBuy()
        {
            try
            {
                StreamReader sr = new StreamReader(dzhPath + @"\WarningTxt\" + DateTime.Now.Year+ DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + ".txt", Encoding.Default);
                string line;
                StringBuilder allLine = new StringBuilder();
                int lineCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineArray = line.Split('\t');
                    bool isBuy = true;
                    if (lineArray.Length > 0)
                    {
                        //300782	股票名称	2019-12-24 13:57	421.22	0.52%	6345	BUY
                        foreach (DataGridViewRow row in dataGridView_warehouse.Rows)
                        {
                            //预警时间在10分钟之前的股票不再买入
                            if (DateTime.Parse(lineArray[2]).AddMinutes(10) < DateTime.Now)
                            {
                                isBuy = false;
                                break;
                            }
                            //当天不能重复买入同一个股票
                            if (lineArray[0].Equals(row.Cells[0].Value)&& Convert.ToInt32(row.Cells[2].Value) > Convert.ToInt32(row.Cells[3].Value))
                            {
                                isBuy = false;
                                break;
                            }
                        }
                        if (isBuy)
                        {
                            YH_Client.Buy(lineArray[0], Convert.ToDouble(lineArray[3]), 100);
                            
                        }
                    }
                    else
                    {
                        return;
                    }
                    
                    lineCount++;
                    allLine.Append(line + "\n");
                    //todo 买入，写dat文件保存已买入的股票600519	股票名称	2019-12-24 13:56	1148.03	-0.11%	8283	BUY
                }
                sr.Close();
                if (lineCount > SendCount)//有新数据时
                {
                    SendCount = lineCount;
                    allLine.Replace(DateTime.Now.ToString("yyyy-MM-dd"), "").Replace("\t", " ").Replace("  ", " ");
                    allLine.ToString();
                }
                else
                {
                    return;
                }
            }
            catch (FileNotFoundException)
            {
                return;
            }
            
            
            
        }

        private void CheckSell()
        {
            DataGridViewRowCollection dataRow = dataGridView_warehouse.Rows;
            List<string> stockCode = new List<string>();
            int index = 0;
            foreach(DataGridViewRow row in dataRow)
            {
                string marketCode="";
                switch(Convert.ToString(row.Cells[7].Value)){
                    case "深A":
                        marketCode = "sz";
                        break;
                    case "上A":
                        marketCode = "sh";
                        break;
                    default:
                        break;
                }
                if (marketCode.Length > 0)
                {
                    stockCode.Add(marketCode + Convert.ToString(row.Cells[0].Value));
                }
                
            }
            if (stockCode.Count > 0)
            {
                getStockNowPrice(stockCode);
            }
        }
        private void SendDataToQQ(string WarningData)
        {

            if (!"".Equals(WarningData))
            {
                if (SendType == 0)
                {
                    //发送手打文字
                    //SendMessage(QQWindowName, WarningData);

                    if (WarningData.Length < 4499)
                    {
                        //发送粘贴文字
                        Clipboard.SetDataObject(WarningData);
                    }
                    else
                    {
                        //粘贴文件
                        System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();
                        strcoll.Add(FilePath);
                        Clipboard.SetFileDropList(strcoll);
                    }
                    SendMessageByClipboard();



                }
                else if (SendType == 1)
                {
                    //粘贴文件
                    System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();
                    strcoll.Add(FilePath);
                    Clipboard.SetFileDropList(strcoll);
                    SendMessageByClipboard();
                }
                if (checkBox_CallPhone.Checked)
                {
                    CallQQPhone();
                }
            }
        }
        private void ComboBox_SendType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendType = comboBox_SendType.SelectedIndex;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; //最左坐标
            public int Top; //最上坐标
            public int Right; //最右坐标
            public int Bottom; //最下坐标
        }

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam); //IntPtr hWnd用int也可以
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        //获取窗口Text 
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        //获取窗口类名 
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        //窗口是否可视
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        //自定义一个类，用来保存句柄信息，在遍历的时候，随便也用空上句柄来获取些信息，呵呵 
        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }

        public WindowInfo[] GetAllDesktopWindows()
        {
            //用来保存窗口对象 列表
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows 
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);

                //get hwnd 
                wnd.hWnd = hWnd;

                //get window name  
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();

                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();

                //如果是可视的QQ窗口，就加入列表
                if (IsWindowVisible(hWnd) && "TXGuiFoundation".Equals(wnd.szClassName)) { 
                    wndList.Add(wnd);
                    //下拉列表添加一项
                    comboBox_QQWindows.Items.Add(wnd.szWindowName);
                }
                return true;
            }, 0);

            return wndList.ToArray();
        }

        private void ComboBox_QQWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            QQWindowName = comboBox_QQWindows.Text;
            QQWindowHandle = FindWindow(null, QQWindowName);
        }

        private void Button_GetAllWindows_Click(object sender, EventArgs e)
        {
            comboBox_QQWindows.Items.Clear();
            GetAllDesktopWindows();
        }

        private void Button_selectFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
    
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "TXT文件|*.txt;*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox_FilePath.Text = dialog.FileName;
                FilePath = dialog.FileName;
                AppSettingUtils.UpdateAppSettings("FilePath", dialog.FileName);
            }
        }
        private void CallQQPhone()
        {
            QQWindowHandle = FindWindow(null, QQWindowName);
            SetForegroundWindow(QQWindowHandle); //把窗体置于最前
            RECT WindowFx = new RECT();
            GetWindowRect(QQWindowHandle, ref WindowFx);//h为窗口句柄
            int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;//电脑屏幕宽度
            int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;//电脑屏幕高度

            //挂掉上一次的电话
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, (WindowFx.Right - 20) * 65535 / ScreenWidth, (WindowFx.Top + 283) * 65535 / ScreenHeight, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, (WindowFx.Right - 20) * 65535 / ScreenWidth, (WindowFx.Top + 283) * 65535 / ScreenHeight, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, (WindowFx.Right - 20) * 65535 / ScreenWidth, (WindowFx.Top + 283) * 65535 / ScreenHeight, 0, 0);

            Thread.Sleep(1000);
            //通过坐标点击电话按钮
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, (WindowFx.Right - 128) * 65535 / ScreenWidth, (WindowFx.Top + 68) * 65535 / ScreenHeight, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, (WindowFx.Right - 128) * 65535 / ScreenWidth, (WindowFx.Top + 68) * 65535 / ScreenHeight, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, (WindowFx.Right - 128) * 65535 / ScreenWidth, (WindowFx.Top + 68) * 65535 / ScreenHeight, 0, 0);

            Thread.Sleep(1000);
            //通过坐标点击【QQ电话】
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, (WindowFx.Right - 128) * 65535 / ScreenWidth, (WindowFx.Top + 106) * 65535 / ScreenHeight, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, (WindowFx.Right - 128) * 65535 / ScreenWidth, (WindowFx.Top + 106) * 65535 / ScreenHeight, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, (WindowFx.Right - 128) * 65535 / ScreenWidth, (WindowFx.Top + 106) * 65535 / ScreenHeight, 0, 0);
        }

        private void button_YHClientPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "EXE文件|*.exe;*.exe";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox_YHClientPath.Text = dialog.FileName;
                FilePath = dialog.FileName;
                AppSettingUtils.UpdateAppSettings("YHClientPath", dialog.FileName);
            }
        }

        private void button_saveUser_Click(object sender, EventArgs e)
        {
            string UserName = textBox_YHUserName.Text;
            string Password = textBox_YHPassword.Text;
            DBHelperSQLite.ExecuteSql("update t_User set UserName='"+ UserName + "'"+ ",Password = '" + Password + "' WHERE clientType='YH'");
        }

        private void button_getBalance_Click(object sender, EventArgs e)
        {
            YH_Client.GetBalance(dataGridView_warehouse);
        }

        public void dataGridViewAddNewRow(string stockCode,string stockName,int qty,int availabelQty,double newPrice,double marketValue,double costPrice)
        {
            int index = dataGridView_warehouse.Rows.Add();
            dataGridView_warehouse.Rows[index].Cells[0].Value = stockCode;
            dataGridView_warehouse.Rows[index].Cells[1].Value = stockName;
            dataGridView_warehouse.Rows[index].Cells[2].Value = qty;
            dataGridView_warehouse.Rows[index].Cells[3].Value = availabelQty;
            dataGridView_warehouse.Rows[index].Cells[4].Value = newPrice;
            dataGridView_warehouse.Rows[index].Cells[5].Value = marketValue;
            dataGridView_warehouse.Rows[index].Cells[6].Value = costPrice;
        }

        private void button_selectDZHPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择大智慧安装目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    return;
                }
                textBox_dzhPath.Text = dialog.SelectedPath;
                dzhPath = dialog.SelectedPath;
                DBHelperSQLite.ExecuteSql("update t_sysConfig set configValue='" + dzhPath + "' WHERE configName='dzhPath'");
            }
        }
		
        /// <summary>
        /// 通过API接口取股票最新价
        /// </summary>
        /// <returns></returns>
		public List<double> getStockNowPrice(List<string> stockList){
            //http ://hq.sinajs.cn/list=sh601006,sh601007,
            string url = "http://hq.sinajs.cn/list=";
            foreach (string stockCode in stockList)
            {
                url += stockCode + ",";
            }

			WebRequest myWebRequest = WebRequest.Create(url);
            myWebRequest.ContentType = "text/html;charset=utf-8";
            WebHeaderCollection Headers = new WebHeaderCollection();
            WebResponse myWebResponse = myWebRequest.GetResponse();
            Stream ReceiveStream = myWebResponse.GetResponseStream();

            string responseStr = "";
            List<double> resultPrice =new List<double>();
            if (ReceiveStream != null)
            {
                StreamReader reader = new StreamReader(ReceiveStream, Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            string[] allStockLine = responseStr.Split('\n');
            for (int i=0;i< allStockLine.Length;i++)
            {
                string line = allStockLine[i];
                if (line.Length > 0)
                {
                    string marketCode = line.Substring(11, 2);
                    string stockCode = line.Substring(13, 6);
                    string lineValue = line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('"') - line.IndexOf('"'));
                    string stockName = lineValue.Split(',')[0];
                    double newPrice = Convert.ToDouble(lineValue.Split(',')[3]);
                    resultPrice.Add(newPrice);
                }
            }
            myWebResponse.Close();
            return resultPrice;
		}
    }
}
