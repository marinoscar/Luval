using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public class SymmetricEncryptHelper
    {

        #region Constructors

        public SymmetricEncryptHelper(string key, string iv) : this("RijndaelManaged", key, iv)
        {
            
        }

        public SymmetricEncryptHelper(string algorithmName, string key, string iv)
        {
            ProviderName = algorithmName;
            Key = key;
            IV = iv;
        } 

        #endregion

        #region Property Implementation

        public string ProviderName { get; }
        public string Key { get; }
        public string IV { get; } 

        #endregion

        #region Methods
        public byte[] Encrypt(byte[] data)
        {
            using (var sOut = new MemoryStream())
            {
                using (var provider = SymmetricAlgorithm.Create(ProviderName))
                {
                    using (var enc = new CryptoStream(sOut, provider.CreateEncryptor(ToArray(Key), ToArray(IV)), CryptoStreamMode.Write))
                    {
                        enc.Write(data, 0, data.Length);
                        return enc.ReadToEnd();
                    }
                }
            }
        }

        public string Encrypt(string data, Encoding encoding)
        {
            return encoding.GetString(Encrypt(encoding.GetBytes(data)));
        }

        public string Encrpyt(string data)
        {
            return Encrypt(data, Encoding.UTF8);
        }

        public byte[] Decrypt(byte[] data)
        {
            using (var sIn = new MemoryStream())
            {
                using (var provider = SymmetricAlgorithm.Create(ProviderName))
                {
                    using (var dec = new CryptoStream(sIn, provider.CreateDecryptor(ToArray(Key), ToArray(IV)), CryptoStreamMode.Read))
                    {
                        return dec.ReadToEnd();
                    }
                }
            }
        }

        public string Decrypt(string data, Encoding encoding)
        {
            return encoding.GetString(Decrypt(encoding.GetBytes(data)));
        }

        public string Decrypt(string data)
        {
            return Decrypt(data, Encoding.UTF8);
        }

        private byte[] ToArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        } 

        #endregion
    }
}
