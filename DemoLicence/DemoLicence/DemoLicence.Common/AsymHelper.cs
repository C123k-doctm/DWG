using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DemoLicence.Common
{

    public class DSAHelper
    {

        private static string keyContainerName = "star";

        private static string m_PriKey = string.Empty;

        private static string m_PubKey = string.Empty;


        public static string PriKey
        {
            get
            {
                return m_PriKey;
            }

            set
            {
                m_PriKey = value;
            }
        }

        public static string PubKey
        {
            get
            {
                return m_PubKey;
            }

            set
            {
                m_PubKey = value;
            }
        }

        public static void generateKey()
        {
            CspParameters m_CspParameters;
            m_CspParameters = new CspParameters();
            m_CspParameters.KeyContainerName = keyContainerName;
            DSACryptoServiceProvider asym = new DSACryptoServiceProvider();
            m_PriKey = asym.ToXmlString(true);
            m_PubKey = asym.ToXmlString(false);
            asym.PersistKeyInCsp = false;
            asym.Clear();
        }

        public static string Encrypto(string source)
        {
            if (string.IsNullOrEmpty(m_PubKey) && string.IsNullOrEmpty(m_PriKey))
            {
                generateKey();
            }
            return getEncryptoInfoByDSA(source);
        }

        /// <summary>
        /// 创建数字签名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string getEncryptoInfoByDSA(string source)
        {
            string dest = string.Empty;
            string ss = HashHelper.GetInfoBySHA1(source);
            byte[] btSource = Convert.FromBase64String(ss);
            DSACryptoServiceProvider asym = new DSACryptoServiceProvider();
            asym.FromXmlString(m_PriKey);
            DSASignatureFormatter dsaFormatter = new DSASignatureFormatter(asym);
            dsaFormatter.SetHashAlgorithm("SHA1");
            byte[] btDest = dsaFormatter.CreateSignature(btSource);
            dest = Convert.ToBase64String(btDest);
            return dest;
        }

        /// <summary>
        /// 验证数字签名
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool verifyInfo(string source, string dest)
        {
            string ss = HashHelper.GetInfoBySHA1(source);
            byte[] btnSource = Convert.FromBase64String(ss);
            byte[] btnDest = Convert.FromBase64String(dest);
            DSACryptoServiceProvider asym = new DSACryptoServiceProvider();
            asym.FromXmlString(m_PubKey);
            DSASignatureDeformatter dsaFormatter = new DSASignatureDeformatter(asym);
            dsaFormatter.SetHashAlgorithm("SHA1");
            return dsaFormatter.VerifySignature(btnSource, btnDest);
        }
    }

    public class RSAHelper
    {

        private static string keyContainerName = "star";

        private static string m_PriKey = string.Empty;

        private static string m_PubKey = string.Empty;


        public static string PriKey
        {
            get
            {
                return m_PriKey;
            }

            set
            {
                m_PriKey = value;
            }
        }

        public static string PubKey
        {
            get
            {
                return m_PubKey;
            }

            set
            {
                m_PubKey = value;
            }
        }

        public static string Encrypto(string source)
        {
            if (string.IsNullOrEmpty(m_PubKey) && string.IsNullOrEmpty(m_PriKey))
            {
                generateKey();
            }
            return getEncryptoInfoByRSA(source);
        }

        public static string Decrypto(string dest)
        {
            if (string.IsNullOrEmpty(m_PubKey) && string.IsNullOrEmpty(m_PriKey))
            {
                generateKey();
            }
            return getDecryptoInfoByRSA(dest);
        }

        public static void generateKey()
        {
            CspParameters m_CspParameters;
            m_CspParameters = new CspParameters();
            m_CspParameters.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider asym = new RSACryptoServiceProvider(m_CspParameters);
            m_PriKey = asym.ToXmlString(true);
            m_PubKey = asym.ToXmlString(false);
            asym.PersistKeyInCsp = false;
            asym.Clear();
        }

        private static string getEncryptoInfoByRSA(string source)
        {
            byte[] plainByte = Encoding.ASCII.GetBytes(source);
            //初始化参数
            RSACryptoServiceProvider asym = new RSACryptoServiceProvider();
            asym.FromXmlString(m_PubKey);
            int keySize = asym.KeySize / 8;//非对称加密，每次的长度不能太长，否则会报异常
            int bufferSize = keySize - 11;
            if (plainByte.Length > bufferSize)
            {
                throw new Exception("非对称加密最多支持【" + bufferSize + "】字节，实际长度【" + plainByte.Length + "】字节。");
            }
            byte[] cryptoByte = asym.Encrypt(plainByte, false);
            return Convert.ToBase64String(cryptoByte);
        }

        private static string getDecryptoInfoByRSA(string dest)
        {
            byte[] btDest = Convert.FromBase64String(dest);
            //初始化参数
            RSACryptoServiceProvider asym = new RSACryptoServiceProvider();
            asym.FromXmlString(m_PriKey);
            int keySize = asym.KeySize / 8;//非对称加密，每次的长度不能太长，否则会报异常
                                           //int bufferSize = keySize - 11;
            if (btDest.Length > keySize)
            {
                throw new Exception("非对称解密最多支持【" + keySize + "】字节，实际长度【" + btDest.Length + "】字节。");
            }
            byte[] cryptoByte = asym.Decrypt(btDest, false);
            return Encoding.ASCII.GetString(cryptoByte);
        }
    }



    public enum AsymType
    {
        DSA = 0,
        RSA = 1
    }

}