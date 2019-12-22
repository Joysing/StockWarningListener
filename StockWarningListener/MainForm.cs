using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Configuration;

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
        const int BM_CLICK = 0xF5;//单击


        //System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
        System.Timers.Timer TipTimer = new System.Timers.Timer(100);//0.1秒执行一次
        int SendCount = 0;//已发送的行数
        int SendType = 0;//发送文字还是文件
        string FilePath = @"D:\\Desktop\\预警输出股.txt";
        string QQWindowName = "";
        IntPtr QQWindowHandle;//QQ窗口句柄
        public MainForm()
        {
            InitializeComponent();
            comboBox_SendType.SelectedIndex = 0;
            FilePath = ConfigurationManager.AppSettings["FilePath"].ToString().Trim();
            textBox_FilePath.Text = AppSettingUtils.GetAppSettingsValue("FilePath");
            //dtFormat.ShortDatePattern = "yyyy-MM-dd";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetAllDesktopWindows();
            string en_US = "00000409"; //英文
            string cn_ZH = "00000804";
            uint KLF_ACTIVATE = 1;
            PostMessage(0xffff, 0x00500, IntPtr.Zero, LoadKeyboardLayout(en_US, KLF_ACTIVATE));//屏蔽中文输入法

            //timer.Elapsed += delegate
            //{
            //    Thread thread1 = new Thread(Timer_TimesUp);
            //    thread1.SetApartmentState(ApartmentState.STA);//要在线程里使用剪切板，必须设为STA模式
            //    thread1.Start();
            //};
            //timer.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            Buy("600356",0,100);
            //Console.WriteLine(GetBalance());
        }

        /// <summary>
        /// 获取资金状况
        /// </summary>
        private StringBuilder GetBalance()
        {
            IntPtr YHWindowHandle = FindWindow(null, "网上股票交易系统5.0");
            IntPtr Handle32770a = FindWindowEx(YHWindowHandle, 0, "#32770", null);
            IntPtr Handle32770b = FindWindowEx(YHWindowHandle, (int)Handle32770a, "#32770", null);
            IntPtr HandleAvailableFunds = GetChildrenWindowHandle(Handle32770b, "Static",null,3);
            IntPtr HandleMarketValue = GetChildrenWindowHandle(Handle32770b, "Static",null,7);
            IntPtr HandleAssets = GetChildrenWindowHandle(Handle32770b, "Static",null,9);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbTemp = new StringBuilder();
            SendMessage(HandleAvailableFunds, WindowsMessage.WM_GETTEXT, 128, sbTemp);
            sb.Append("可用资金：").Append(sbTemp.ToString());
            SendMessage(HandleMarketValue, WindowsMessage.WM_GETTEXT, 128, sbTemp);
            sb.Append("，市值：").Append(sbTemp.ToString());
            SendMessage(HandleAssets, WindowsMessage.WM_GETTEXT, 128, sbTemp);
            sb.Append("，总资产：").Append(sbTemp.ToString());
            return sb;
        }

        /// <summary>
        /// 获取第几个子控件
        /// </summary>
        /// <param name="ParentHandle">父窗口句柄</param>
        /// <param name="ClassName">控件类名</param>
        /// <param name="Title">控件标题</param>
        /// <param name="which">第几个</param>
        /// <returns></returns>
        private IntPtr GetChildrenWindowHandle(IntPtr ParentHandle,string ClassName,string Title,int which)
        {
            IntPtr ChildrenWindowHandle = FindWindowEx(ParentHandle, 0, ClassName, Title);
            if (which==1)
            {
                return ChildrenWindowHandle;
            }
            else if(which > 1)
            {
                for (int i = 1; i < which; i++)
                {
                    ChildrenWindowHandle = FindWindowEx(ParentHandle, (int)ChildrenWindowHandle, ClassName, Title);
                }
            }
            return ChildrenWindowHandle;
        }
        /// <summary>
        /// 买入
        /// </summary>
        /// <param name="StockCode">股票代码</param>
        /// <param name="Price">价格，输入0则以最新价买入</param>
        /// <param name="QTY">数量</param>
        private void Buy(string StockCode,double Price,int QTY)
        {
            IntPtr YHWindowHandle = FindWindow(null, "网上股票交易系统5.0");
            IntPtr AfxMDIFrame42sWindowHandle = FindWindowEx(YHWindowHandle, 0, "AfxMDIFrame42s", null);
            IntPtr Handle32770 = GetChildrenWindowHandle(AfxMDIFrame42sWindowHandle, "#32770", null, 4);
            IntPtr HandleStockCodeEdit = FindWindowEx(Handle32770, 0, "Edit", null);//证券代码编辑框
            IntPtr HandlePriceEdit = FindWindowEx(Handle32770, (int)HandleStockCodeEdit, "Edit", null);//买入价格编辑框
            IntPtr HandleQTYEdit = FindWindowEx(Handle32770, (int)HandlePriceEdit, "Edit", null);//买入数量编辑框
            IntPtr HandleBuyButton = FindWindowEx(Handle32770, 0, "Button", "买入[B]");//买入按钮
            IntPtr HandleStockName = FindWindowEx(Handle32770, 0, "Static", null);//股票名称
            IntPtr HandlePriceList = FindWindowEx(Handle32770, 0, "#32770", null);//买5卖5
            IntPtr HandlePriceSell5 = FindWindowEx(HandlePriceList, 0, "Static", null);//卖5
            IntPtr HandlePriceSell4 = FindWindowEx(HandlePriceList, (int)HandlePriceSell5, "Static", null);//卖4
            IntPtr HandlePriceSell3 = FindWindowEx(HandlePriceList, (int)HandlePriceSell4, "Static", null);//卖3
            IntPtr HandlePriceSell2 = FindWindowEx(HandlePriceList, (int)HandlePriceSell3, "Static", null);//卖2
            IntPtr HandlePriceSell1 = FindWindowEx(HandlePriceList, (int)HandlePriceSell2, "Static", null);//卖1
            IntPtr HandlePriceNew = FindWindowEx(HandlePriceList, (int)HandlePriceSell1, "Static", null);//最新价

            if (YHWindowHandle.ToString() != "0")
            {
                SetForegroundWindow(YHWindowHandle); //把窗体置于最前
                SendKeys.SendWait("{F1}"); //模拟键盘输入F1买入界面
                                           //SendKeys.Send("{F2}"); //模拟键盘输入F2卖出界面

                Clipboard.SetText(StockCode);
                SendMessage(HandleStockCodeEdit, WindowsMessage.WM_PASTE, 0, "");
                while (true)
                {
                    StringBuilder sbStockName = new StringBuilder();
                    SendMessage(HandleStockName, WindowsMessage.WM_GETTEXT, 128, sbStockName);//读取股票名称
                    if (sbStockName.Length > 0)
                    {
                        break;
                    }
                }
                if (Price > 0)
                {
                    Clipboard.SetText(Convert.ToString(Price));
                    SendMessage(HandlePriceEdit, WindowsMessage.WM_PASTE, 0, "");
                }
                else
                {
                    SendMessage(HandlePriceNew, WindowsMessage.WM_LBUTTONDOWN, 0, "");
                    SendMessage(HandlePriceNew, WindowsMessage.WM_LBUTTONUP, 0, "");
                }
                
                Clipboard.SetText(Convert.ToString(QTY));
                SendMessage(HandleQTYEdit, WindowsMessage.WM_PASTE, 0, "");
                SendMessage(HandleQTYEdit, WindowsMessage.WM_PASTE, 0, "");

                //SendMessage(HandleBuyButton, BM_CLICK, 1, "0");
                //SendMessage(HandleQTYEdit, WindowsMessage.WM_KEYDOWN, WindowsKeyCode.VK_CONTROL, "");
                //SendMessage(HandleQTYEdit, WindowsMessage.WM_CHAR, WindowsKeyCode.VK_B, "");
                //SendMessage(HandleQTYEdit, WindowsMessage.WM_KEYUP, WindowsKeyCode.VK_CONTROL, "");
                SendKeys.SendWait("^b");

                SendKeys.SendWait("{ENTER}"); //确认下单


                while (true)
                {
                    IntPtr WindowHandleTipForm = FindWindowEx(IntPtr.Zero, 0, "#32770", null);
                    IntPtr WindowHandleTipFormContent = FindWindowEx(WindowHandleTipForm, 0, "Static", null);
                    IntPtr WindowHandleTipFormTitle = FindWindowEx(WindowHandleTipForm, (int)WindowHandleTipFormContent, "Static", null);
                    StringBuilder TipTitle = new StringBuilder();
                    SendMessage(WindowHandleTipFormTitle, WindowsMessage.WM_GETTEXT, 10, TipTitle);
                    if (WindowHandleTipFormTitle.ToInt32() > 0 && "提示".Equals(TipTitle.ToString()))
                    {
                        StringBuilder TipContent = new StringBuilder();
                        SendMessage(WindowHandleTipFormContent, WindowsMessage.WM_GETTEXT, 1024, TipContent);
                        Console.WriteLine(TipContent);
                        SetForegroundWindow(WindowHandleTipForm); //把窗体置于最前
                        SendKeys.SendWait("{ENTER}");
                        break;
                    }
                    else if (WindowHandleTipFormTitle.ToInt32() == 0)
                    {
                        break;
                    }
                }
                //SendKeys.SendWait("{ENTER}");//Enter
                //SendKeys.Send("^{ENTER}");//Ctrl+Enter
            }
            else
            {
                MessageBox.Show("木有找到股票交易窗口");
            }
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
        /// <summary>
        /// 找到窗口
        /// </summary>
        /// <param name="hwndParent">父窗口句柄（如果为空，则为桌面窗口）</param>
        /// <param name="hwndChildAfter">子窗口句柄（从该子窗口之后查找）</param>
        /// <param name="lpszClass">窗口类名(例：Button</param>
        /// <param name="lpszWindow">窗口标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private extern static IntPtr FindWindowEx(IntPtr hwndParent, int hwndChildAfter, string lpszClass, string lpszWindow);
        //把窗体置于最前
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">消息接受窗口句柄</param>
        /// <param name="wMsg">消息</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, string lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll")]
        private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32")]
        public static extern int EnumWindows(CallBack x, int y);
        public delegate bool CallBack(int hwnd, int lParam);

        public static bool Report(int hwnd, int lParam)
        {
            Console.Write("Window handle is :");
            Console.WriteLine(hwnd);
            Console.Read();
            return true;
        }
        /// <summary>
        /// 获取预警数据
        /// </summary>
        public string GetWarningData()
        {
            try
            {
                StreamReader sr = new StreamReader(FilePath, Encoding.Default);
                string line;
                StringBuilder allLine = new StringBuilder();
                int lineCount = 0;
                DateTime dateTime;
                while ((line = sr.ReadLine()) != null)
                {
                    DateTime.TryParse(line.Split('\t')[2].Split(' ')[0], out dateTime);
                    int compNum = DateTime.Compare(dateTime, DateTime.Today);
                    // 今天输出的预警数据
                    if (compNum == 0)
                    {
                        lineCount++;
                        allLine.Append(line + "\n");
                    }
                }
                sr.Close();
                if (lineCount > SendCount)//有新数据时
                {
                    SendCount = lineCount;
                    allLine.Replace(DateTime.Now.ToString("yyyy-MM-dd"), "").Replace("\t", " ").Replace("  ", " ");
                    return allLine.ToString();
                }
                else
                {
                    return "";
                }
            }catch (FileNotFoundException)
            {
                return "";
            }
            
        }

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

        private void button_TestSendMsg_Click(object sender, EventArgs e)
        {
            if ("".Equals(QQWindowName))
            {
                MessageBox.Show("先选择QQ窗口");
                return;
            }
            Clipboard.SetText("测试消息");
            SendMessageByClipboard();
            if (checkBox_CallPhone.Checked)
            {
                CallQQPhone();
            }
            
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            switch (button_Start.Text)
            {
                case "开始检测":
                    if ("".Equals(QQWindowName))
                    {
                        MessageBox.Show("先选择QQ窗口");
                        return;
                    }
                    //timer.Start();
                    button_Start.Text = "停止检测";
                    break;
                case "停止检测":
                    //timer.Stop();
                    button_Start.Text = "开始检测";
                    break;
            }
            
        }

        private void Timer_TimesUp()
        {
            if(DateTime.Now.Hour==0&& DateTime.Now.Minute == 0)//0点时更新发送的行数
            {
                SendCount = 0;
            }
            string WarningData = GetWarningData();
            if (!"".Equals(WarningData))
            {
                if (SendType == 0)
                {
                    //发送手打文字
                    //SendMessage(QQWindowName, WarningData);

                    if (WarningData.Length < 4499) {
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

                    //int WarningDataSubStringLength = 4400;//每段长度，复制状态下，QQ一次最多只能发送4499字（中文）
                    //int WarningDataSubStringCount = WarningData.Length / WarningDataSubStringLength + 1;//WarningData可以分成多少段
                    //string[] WarningDataSubString = new string[WarningDataSubStringCount];
                    //for(int i=0;i< WarningDataSubStringCount; i++)
                    //{
                    //    System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();
                    //    int indexStart = i * WarningDataSubStringLength;
                    //    int indexEnd = WarningData.Length - indexStart > WarningDataSubStringLength ? WarningDataSubStringLength : WarningData.Length - indexStart;
                    //    Clipboard.SetDataObject(WarningData.Substring(indexStart, indexEnd),true);
                    //    SendMessageByClipboard(QQWindowName);
                    //    //WarningDataSubString[i] = WarningData.Substring(i* WarningDataSubStringLength, WarningDataSubStringLength);
                    //}

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
    }
}
