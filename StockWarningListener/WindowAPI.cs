using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StockWarningListener
{
    public class WindowAPI
    {
        public static int PROCESS_VM_OPERATION = 0x0008;
        public static int PROCESS_VM_READ = 0x0010;
        public static int PROCESS_VM_WRITE = 0x0020;
        public static int LVIF_TEXT = 0x0001;

        /// <summary>
        /// 获取控件图片
        /// </summary>
        /// <param name="handle">图片控件句柄</param>
        /// <param name="dwRop">光栅运算代码</param>
        /// <returns></returns>
        public static Bitmap CaptureWindow(IntPtr handle, int dwRop)

        {
            // get te hDC of the target window
            IntPtr hdcSrc = GetWindowDC(handle);
            // get the size
            RECT windowRect = new RECT();
            GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = SelectObject(hdcDest, hBitmap);
            // bitblt over
            BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, dwRop);
            // restore selection
            SelectObject(hdcDest, hOld);
            // clean up
            DeleteDC(hdcDest);
            ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Bitmap img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            DeleteObject(hBitmap);
            return img;
        }
        /// <summary>
        /// 获取第几个子控件
        /// </summary>
        /// <param name="ParentHandle">父窗口句柄</param>
        /// <param name="ClassName">控件类名</param>
        /// <param name="Title">控件标题</param>
        /// <param name="which">第几个</param>
        /// <returns></returns>
        public static IntPtr GetChildrenWindowHandle(IntPtr ParentHandle, string ClassName, string Title, int which)
        {
            IntPtr ChildrenWindowHandle = FindWindowEx(ParentHandle, IntPtr.Zero, ClassName, Title);
            if (which == 1)
            {
                return ChildrenWindowHandle;
            }
            else if (which > 1)
            {
                for (int i = 1; i < which; i++)
                {
                    ChildrenWindowHandle = FindWindowEx(ParentHandle, ChildrenWindowHandle, ClassName, Title);
                }
            }
            return ChildrenWindowHandle;
        }

        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public IntPtr pszText; // string 
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
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
        public extern static IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 取父窗口句柄
        /// </summary>
        /// <param name="hwndChild">当前窗口句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetParent")]
        public extern static IntPtr GetParent(IntPtr hwndChild);

        /// <summary>
        /// 取窗口标题
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpString">标题返回</param>
        /// <param name="nMaxCount">标题最大长度</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 把窗体置于最前
        /// </summary>
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
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, string lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll")]
        public static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        public static int MEM_COMMIT = 0x1000;
        public static int MEM_RELEASE = 0x8000;
        public static int MEM_RESERVE = 0x2000;
        public static int PAGE_READWRITE = 4;

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int flAllocationType, int flProtect);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
        int nWidth, int nHeight, IntPtr hObjectSource,
        int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
        int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }
}
