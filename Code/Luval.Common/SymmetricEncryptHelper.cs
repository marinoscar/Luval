using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public class SymmetricEncryptHelper
    {

        public SymmetricEncryptHelper(SymmetricAlgorithm algorithm, string key, string iv)
        {
            Provider = algorithm;
            Key = key;
            IV = iv;
        }

        public SymmetricAlgorithm Provider { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }

    }
}
