using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoLicence.Common
{
    public class LicenceHelper
    {
        /// <summary>
        /// 获取电脑信息，并生成文件
        /// </summary>
        public static string GetComputerInfoAndGenerateFile()
        {
            string computerKeyFile = string.Empty;
            try
            {
                var info = GetComputerInfo();
                if (info != null && info.Count > 0)
                {
                    //获取到电脑信息
                    var strInfo = new StringBuilder();
                    foreach (var computer in info)
                    {
                        strInfo.AppendLine($"{computer.Key}={computer.Value}");
                    }
                    RegistFileHelper.WriteComputerInfoFile(strInfo.ToString());
                    computerKeyFile = RegistFileHelper.ComputerInfofile;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            return computerKeyFile;
        }

        public static Dictionary<string,string> GetComputerInfo()
        {
            var info = ComputerHelper.GetComputerInfo();
            return info;
        }

        public static bool CheckLicenceKeyIsExists()
        {
            var keyFile = RegistFileHelper.RegistInfofile;
            if (File.Exists(keyFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetComputerInfo(string computerInfoFile)
        {
            return RegistFileHelper.ReadComputerInfoFile(computerInfoFile);
        }

        public static void GenerateLicenceKey(string info,string keyfile)
        {
            string encrypto = RSAHelper.Encrypto(info);
            string priKey = RSAHelper.PriKey;//公钥加密，私钥解密
            byte[] priKeyBytes = Encoding.ASCII.GetBytes(priKey);
            string priKeyBase64=Convert.ToBase64String(priKeyBytes);
            StringBuilder keyInfo= new StringBuilder();
            keyInfo.AppendLine($"prikey={priKeyBase64}");
            keyInfo.AppendLine($"encrypto={encrypto}");
            RegistFileHelper.WriteRegistFile(keyInfo.ToString(), keyfile);
        }

        public static string ReadLicenceKey(string keyfile)
        {
            var keyInfo = RegistFileHelper.ReadRegistFile(keyfile);
            if (keyInfo == null)
            {
                return string.Empty;
            }
            string[] keyInfoArr = keyInfo.Split("\r\n");
            var priKeyBase64 = keyInfoArr[0].Substring(keyInfoArr[0].IndexOf("=")+1);
            var encrypto = keyInfoArr[1].Substring(keyInfoArr[1].IndexOf("=")+1);
            var priKeyByte= Convert.FromBase64String(priKeyBase64);
            var priKey = Encoding.ASCII.GetString(priKeyByte);
            RSAHelper.PriKey= priKey;
            var info = RSAHelper.Decrypto(encrypto);
            return info;
        }

        public static string GetDefaultRegisterFileName()
        {
            return RegistFileHelper.RegistInfofile;
        }

        public static string GetDefaultComputerFileName()
        {
            return RegistFileHelper.ComputerInfofile;
        }
        
        public static string GetPublicKey()
        {
            if (string.IsNullOrEmpty(RSAHelper.PubKey))
            {
                RSAHelper.generateKey();
            }
            return RSAHelper.PubKey;
        }

        public static string GetPrivateKey()
        {
            if (string.IsNullOrEmpty(RSAHelper.PriKey))
            {
                RSAHelper.generateKey();
            }
            return RSAHelper.PriKey;
        }
    }
}
