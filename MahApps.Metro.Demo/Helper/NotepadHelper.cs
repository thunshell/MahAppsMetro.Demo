using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class NotepadHelper
    {
        #region [ API: 记事本 ]

        /// <summary>
        /// 传递消息给记事本
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        /// <summary>
        /// 查找句柄
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("User32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 记事本需要的常量
        /// </summary>
        public const uint WM_SETTEXT = 0x000C;

        #endregion

        public static void NewNotePad(string message)
        {
            #region [ 启动记事本 ]

            System.Diagnostics.Process Proc;

            try
            {
                // 启动记事本
                Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = "notepad.exe";
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardOutput = true;

                Proc.Start();
            }
            catch
            {
                Proc = null;
            }

            #endregion

            #region [ 传递数据给记事本 ]

            if (Proc != null)
            {
                // 调用 API, 传递数据
                while (Proc.MainWindowHandle == IntPtr.Zero)
                {
                    Proc.Refresh();
                }

                IntPtr vHandle = FindWindowEx(Proc.MainWindowHandle, IntPtr.Zero, "Edit", null);

                // 传递数据给记事本
                SendMessage(vHandle, WM_SETTEXT, 0, message);
            }

            #endregion

        }
    }
}
