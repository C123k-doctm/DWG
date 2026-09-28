using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Teigha_Learn_Cad
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            // Legacy analysis writes intermediate files using relative paths.
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0 && args[0] == "--startup-check")
                return CheckStartup();
            Application.Run(new MainForms());
            return 0;
        }

        private static int CheckStartup()
        {
            string report = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup-check.txt");
            try
            {
                if (IntPtr.Size != 8)
                    throw new InvalidOperationException("The bundled Teigha runtime requires x64.");
                using (var image = new OpenCvSharp.Mat(2, 2, OpenCvSharp.MatType.CV_8UC1, new OpenCvSharp.Scalar(7)))
                {
                    if (OpenCvSharp.Cv2.CountNonZero(image) != 4)
                        throw new InvalidOperationException("OpenCV native runtime check failed.");
                }
                using (var buffer = new MemoryStream())
                {
                    var workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                    workbook.CreateSheet("Check").CreateRow(0).CreateCell(0).SetCellValue("OK");
                    workbook.Write(buffer);
                }
                using (var form = new MainForms())
                {
                    if (form.CAD == null || form.CAD.database == null || form.CAD.helperDevice == null)
                        throw new InvalidOperationException("Default CAD template or rendering device was not initialized.");
                    form.CAD.helperDevice.Update();
                    // Exercise the existing FormClosed cleanup instead of leaving native services to finalizers.
                    IntPtr windowHandle = form.Handle;
                    form.Close();
                }
                File.WriteAllText(report, "PASS: x64; OpenCV native; XLSX creation; default DWG template; CAD rendering device; clean shutdown.\r\n", Encoding.UTF8);
                return 0;
            }
            catch (Exception error)
            {
                File.WriteAllText(report, "FAIL\r\n" + error, Encoding.UTF8);
                return 1;
            }
        }
    }
}
