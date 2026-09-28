using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DemoLicence
{
    public class HashHelper
    {
        #region 信息摘要

        /// <summary>
        /// MD5信息摘要
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetInfoByMd5(string source)
        {
            HashAlgorithm hash = getHashAlgorithm(HashType.MD5);
            return Encrypto(hash, source);
        }

        public static string GetInfoBySHA1(string source)
        {
            HashAlgorithm hash = getHashAlgorithm(HashType.SHA1);
            return Encrypto(hash, source);
        }

        public static string GetInfoBySHA256(string source)
        {
            HashAlgorithm hash = getHashAlgorithm(HashType.SHA256);
            return Encrypto(hash, source);
        }

        public static string GetInfoBySHA384(string source)
        {
            HashAlgorithm hash = getHashAlgorithm(HashType.SHA384);
            return Encrypto(hash, source);
        }

        public static string GetInfoBySHA512(string source)
        {
            HashAlgorithm hash = getHashAlgorithm(HashType.SHA512);
            return Encrypto(hash, source);
        }

        #endregion

        /// <summary>
        /// 根据类型获取摘要算法名称
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static HashAlgorithm getHashAlgorithm(HashType t)
        {
            HashAlgorithm hash;
            switch (t)
            {
                case HashType.MD5:
                    hash = new MD5CryptoServiceProvider();
                    break;
                case HashType.SHA1:
                    hash = new SHA1Managed();
                    break;
                case HashType.SHA256:
                    hash = new SHA256Managed();
                    break;
                case HashType.SHA384:
                    hash = new SHA384Managed();
                    break;
                case HashType.SHA512:
                    hash = new SHA512Managed();
                    break;
                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }
            return hash;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string Encrypto(HashAlgorithm hash, string source)
        {
            string dest = string.Empty;
            try
            {
                byte[] btSource = Encoding.Default.GetBytes(source);
                byte[] btDest = hash.ComputeHash(btSource);
                dest = Convert.ToBase64String(btDest, 0, btDest.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dest;
        }
    }
    public enum HashType
    {
        MD5 = 0,
        SHA1 = 1,
        SHA256 = 2,
        SHA384 = 3,
        SHA512 = 4
    }
}
