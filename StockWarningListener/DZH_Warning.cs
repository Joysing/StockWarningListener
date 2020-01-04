using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StockWarningListener.WindowAPI;

namespace StockWarningListener
{
    /// <summary>
    /// 大智慧预警
    /// </summary>
    public class DZH_Warning
    {
        public int SendCount = 0;
        /// <summary>
        /// 从窗口获取预警数据
        /// </summary>
        public static void GetWarningDataFromWindow()
        {
            IntPtr HandleWarning = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "#32770", null);
            IntPtr HandleWarningList;

            while (true)
            {
                IntPtr HandleWarningParent = GetParent(HandleWarning);

                StringBuilder sb = new StringBuilder(512);
                GetWindowText(HandleWarning, sb, sb.Capacity);
                string WarningTitle = sb.ToString();
                GetWindowText(HandleWarningParent, sb, sb.Capacity);
                string WarningParentTitle = sb.ToString();

                if (WarningTitle.StartsWith("预警") && "大智慧".Equals(WarningParentTitle))//这是大智慧的预警窗口
                {
                    Console.WriteLine(WarningTitle);
                    HandleWarningList = FindWindowEx(HandleWarning, IntPtr.Zero, "SysListView32", "List2");
                    break;
                }
                else
                {
                    HandleWarning = FindWindowEx(IntPtr.Zero, HandleWarning, "#32770", null);
                }
            }
            if (HandleWarningList != IntPtr.Zero)
            {
                int vItemCount = SendMessage(HandleWarningList, WindowsMessage.LVM_GETITEMCOUNT, 0, "");
                int vProcessId;
                GetWindowThreadProcessId(HandleWarningList, out vProcessId);
                IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, vProcessId);
                IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
                Console.WriteLine(vItemCount);
                for (int i = 0; i < vItemCount; i++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[2];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = i;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));
                    vItem[1].mask = LVIF_TEXT;
                    vItem[1].iItem = i;
                    vItem[1].iSubItem = 1;
                    vItem[1].cchTextMax = vBuffer.Length;
                    vItem[1].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;

                    WriteProcessMemory(vProcess, vPointer,
                        Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(HandleWarningList, WindowsMessage.LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);

                    string vText = Marshal.PtrToStringUni(
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                    Console.WriteLine(vText);
                }
            }
            //

        }

        /// <summary>
        /// 从文件获取预警数据
        /// </summary>
        public string GetWarningDataFromTxt(string FilePath)
        {
            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)//0点时更新发送的行数
            {
                SendCount = 0;
            }
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
            }
            catch (FileNotFoundException)
            {
                return "";
            }

        }
    }
}
