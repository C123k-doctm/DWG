using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLicence.Common
{
    public class RegistFileHelper
    {
        public static string ComputerInfofile = "ComputerInfo.key";
        public static string RegistInfofile = "Licence.key";
        public static void WriteRegistFile(string info, string keyFile)
        {
            string tmp = string.IsNullOrEmpty(keyFile) ? RegistInfofile : keyFile;
            WriteFile(info, tmp);
        }
        public static void WriteComputerInfoFile(string info)
        {
            WriteFile(info, ComputerInfofile);
        }
        public static string ReadRegistFile(string keyFile)
        {
            string tmp = string.IsNullOrEmpty(keyFile) ? RegistInfofile : keyFile;
            return ReadFile(tmp);
        }
        public static string ReadComputerInfoFile(string file)
        {
            string tmp = string.IsNullOrEmpty(file) ? ComputerInfofile : file;
            return ReadFile(tmp);
        }

        private static void WriteFile(string info, string fileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.Write(info);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private static string ReadFile(string fileName)
        {
            string info = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    info = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return info;
        }
    }
}
