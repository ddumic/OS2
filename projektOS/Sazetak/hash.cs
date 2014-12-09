using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace projektOS
{
    class hash
    {
        public string sazetak(byte[] datoteka)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(datoteka);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
