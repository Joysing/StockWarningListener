using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockWarningListener
{
    /// <summary>
    /// 银河证券客户端操作
    /// </summary>
    public class YH_Client
    {
        /// <summary>
        /// 获取资金状况
        /// </summary>
        private static StringBuilder GetBalance()
        {
            IntPtr YHWindowHandle = WindowAPI.FindWindow(null, "网上股票交易系统5.0");
            IntPtr Handle32770a = WindowAPI.FindWindowEx(YHWindowHandle, IntPtr.Zero, "#32770", null);
            IntPtr Handle32770b = WindowAPI.FindWindowEx(YHWindowHandle, Handle32770a, "#32770", null);
            IntPtr HandleAvailableFunds = WindowAPI.GetChildrenWindowHandle(Handle32770b, "Static", null, 3);
            IntPtr HandleMarketValue = WindowAPI.GetChildrenWindowHandle(Handle32770b, "Static", null, 7);
            IntPtr HandleAssets = WindowAPI.GetChildrenWindowHandle(Handle32770b, "Static", null, 9);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbTemp = new StringBuilder();
            WindowAPI.SendMessage(HandleAvailableFunds, WindowsMessage.WM_GETTEXT, 128, sbTemp);
            sb.Append("可用资金：").Append(sbTemp.ToString());
            WindowAPI.SendMessage(HandleMarketValue, WindowsMessage.WM_GETTEXT, 128, sbTemp);
            sb.Append("，市值：").Append(sbTemp.ToString());
            WindowAPI.SendMessage(HandleAssets, WindowsMessage.WM_GETTEXT, 128, sbTemp);
            sb.Append("，总资产：").Append(sbTemp.ToString());
            return sb;
        }

        /// <summary>
        /// 买入
        /// </summary>
        /// <param name="StockCode">股票代码</param>
        /// <param name="Price">价格，输入0则以最新价买入</param>
        /// <param name="QTY">数量</param>
        private static void Buy(string StockCode, double Price, int QTY)
        {
            IntPtr YHWindowHandle = WindowAPI.FindWindow(null, "网上股票交易系统5.0");
            IntPtr AfxMDIFrame42sWindowHandle = WindowAPI.FindWindowEx(YHWindowHandle, IntPtr.Zero, "AfxMDIFrame42s", null);
            IntPtr Handle32770 = WindowAPI.GetChildrenWindowHandle(AfxMDIFrame42sWindowHandle, "#32770", null, 4);
            IntPtr HandleStockCodeEdit = WindowAPI.FindWindowEx(Handle32770, IntPtr.Zero, "Edit", null);//证券代码编辑框
            IntPtr HandlePriceEdit = WindowAPI.FindWindowEx(Handle32770, HandleStockCodeEdit, "Edit", null);//买入价格编辑框
            IntPtr HandleQTYEdit = WindowAPI.FindWindowEx(Handle32770, HandlePriceEdit, "Edit", null);//买入数量编辑框
            IntPtr HandleBuyButton = WindowAPI.FindWindowEx(Handle32770, IntPtr.Zero, "Button", "买入[B]");//买入按钮
            IntPtr HandleStockName = WindowAPI.FindWindowEx(Handle32770, IntPtr.Zero, "Static", null);//股票名称
            IntPtr HandlePriceList = WindowAPI.FindWindowEx(Handle32770, IntPtr.Zero, "#32770", null);//买5卖5
            IntPtr HandlePriceSell5 = WindowAPI.FindWindowEx(HandlePriceList, IntPtr.Zero, "Static", null);//卖5
            IntPtr HandlePriceSell4 = WindowAPI.FindWindowEx(HandlePriceList, HandlePriceSell5, "Static", null);//卖4
            IntPtr HandlePriceSell3 = WindowAPI.FindWindowEx(HandlePriceList, HandlePriceSell4, "Static", null);//卖3
            IntPtr HandlePriceSell2 = WindowAPI.FindWindowEx(HandlePriceList, HandlePriceSell3, "Static", null);//卖2
            IntPtr HandlePriceSell1 = WindowAPI.FindWindowEx(HandlePriceList, HandlePriceSell2, "Static", null);//卖1
            IntPtr HandlePriceNew = WindowAPI.FindWindowEx(HandlePriceList, HandlePriceSell1, "Static", null);//最新价

            if (YHWindowHandle.ToString() != "0")
            {
                WindowAPI.SetForegroundWindow(YHWindowHandle); //把窗体置于最前
                SendKeys.SendWait("{F1}"); //模拟键盘输入F1买入界面
                                           //SendKeys.Send("{F2}"); //模拟键盘输入F2卖出界面

                Clipboard.SetText(StockCode);
                WindowAPI.SendMessage(HandleStockCodeEdit, WindowsMessage.WM_PASTE, 0, "");
                while (true)
                {
                    StringBuilder sbStockName = new StringBuilder();
                    WindowAPI.SendMessage(HandleStockName, WindowsMessage.WM_GETTEXT, 128, sbStockName);//读取股票名称
                    if (sbStockName.Length > 0)
                    {
                        break;
                    }
                }
                if (Price > 0)
                {
                    Clipboard.SetText(Convert.ToString(Price));
                    WindowAPI.SendMessage(HandlePriceEdit, WindowsMessage.WM_PASTE, 0, "");
                }
                else
                {
                    WindowAPI.SendMessage(HandlePriceNew, WindowsMessage.WM_LBUTTONDOWN, 0, "");
                    WindowAPI.SendMessage(HandlePriceNew, WindowsMessage.WM_LBUTTONUP, 0, "");
                }

                Clipboard.SetText(Convert.ToString(QTY));
                WindowAPI.SendMessage(HandleQTYEdit, WindowsMessage.WM_PASTE, 0, "");
                WindowAPI.SendMessage(HandleQTYEdit, WindowsMessage.WM_PASTE, 0, "");

                //SendMessage(HandleBuyButton, BM_CLICK, 1, "0");
                //SendMessage(HandleQTYEdit, WindowsMessage.WM_KEYDOWN, WindowsKeyCode.VK_CONTROL, "");
                //SendMessage(HandleQTYEdit, WindowsMessage.WM_CHAR, WindowsKeyCode.VK_B, "");
                //SendMessage(HandleQTYEdit, WindowsMessage.WM_KEYUP, WindowsKeyCode.VK_CONTROL, "");
                SendKeys.SendWait("^b");

                SendKeys.SendWait("{ENTER}"); //确认下单


                while (true)
                {
                    IntPtr WindowHandleTipForm = WindowAPI.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "#32770", null);
                    IntPtr WindowHandleTipFormContent = WindowAPI.FindWindowEx(WindowHandleTipForm, IntPtr.Zero, "Static", null);
                    IntPtr WindowHandleTipFormTitle = WindowAPI.FindWindowEx(WindowHandleTipForm, WindowHandleTipFormContent, "Static", null);
                    StringBuilder TipTitle = new StringBuilder();
                    WindowAPI.SendMessage(WindowHandleTipFormTitle, WindowsMessage.WM_GETTEXT, 10, TipTitle);
                    if (WindowHandleTipFormTitle.ToInt32() > 0 && "提示".Equals(TipTitle.ToString()))
                    {
                        StringBuilder TipContent = new StringBuilder();
                        WindowAPI.SendMessage(WindowHandleTipFormContent, WindowsMessage.WM_GETTEXT, 1024, TipContent);
                        Console.WriteLine(TipContent);
                        WindowAPI.SetForegroundWindow(WindowHandleTipForm); //把窗体置于最前
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
                Console.WriteLine("木有找到股票交易窗口");
            }
        }
        

    }
}
