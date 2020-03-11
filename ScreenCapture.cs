using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.IO;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        string defaultPath;
        string preferenceFilePath;
        string exportFilePath;
        bool fullScreenFlag;
        string prefixText;


        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            var plan = context.PlanSetup;
            if (plan == null)
            {
                System.Windows.MessageBox.Show("No plan is loaded.");
                return;
            }

            defaultPath = @"\\172.16.10.181\va_transfer\MLC\--- ESAPI ---\ScreeCapturePreference\";
            preferenceFilePath = defaultPath + context.CurrentUser.Name + "_ScreeCapturePreference.txt";
            exportFilePath = @"C:\Users\vms\Desktop";
            fullScreenFlag = false;
            if (File.Exists(preferenceFilePath))
            {
                StreamReader sr = new StreamReader(preferenceFilePath, Encoding.GetEncoding("Shift_JIS"));
                string[] subString = sr.ReadLine().Split(',');
                sr.Close();

                if (subString.Length == 2)
                {
                    exportFilePath = subString[0];

                    if (subString[1] == "FullScreen")
                    {
                        fullScreenFlag = true;
                    }
                    else
                    {
                        fullScreenFlag = false;
                    }
                }
            }
            else
            {
                using (StreamWriter sr = new StreamWriter(preferenceFilePath, false))
                {
                    string captureWindowType;
                    if (fullScreenFlag == true)
                    {
                        captureWindowType = "FullScreen";
                    }
                    else
                    {
                        captureWindowType = "ActiveWindow";
                    }
                    sr.WriteLine(exportFilePath + "," + captureWindowType);
                    sr.Flush();
                }
            }

            prefixText = plan.Course.Patient.Id + "_" + plan.Course.Id + "_" + plan.Id;
            // TODO : Add here the code that is called when the script is launched from Eclipse.
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();

        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.Dll")]
        static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        extern static IntPtr GetForegroundWindow();
        private void ThreadTask()
        {

            // TODO : Add here the code that is called when the script is launched from Eclipse.
            Rectangle rect = new Rectangle();
            Thread.Sleep(500);
            if (fullScreenFlag == true)
            {
                rect = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                RECT r;
                IntPtr active = GetForegroundWindow();
                GetWindowRect(active, out r);
                rect = new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);
            }


            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }

            string filePath = exportFilePath + @"\SC_" + prefixText + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";

            bmp.Save(filePath, ImageFormat.Jpeg);
            System.Windows.MessageBox.Show("Save as " + filePath);


        }
    }
}
