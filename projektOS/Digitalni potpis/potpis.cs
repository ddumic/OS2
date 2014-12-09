using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projektOS
{
    class potpis
    {
        public RSAPKCS1SignatureFormatter potpisi;
        public RSAPKCS1SignatureDeformatter RSAPK;
        public SHA1Managed sha1;
        public RSACryptoServiceProvider rsa;
        public RSACryptoServiceProvider RSAC;
        public potpis(SHA1Managed SHA1, RSACryptoServiceProvider RSA)
        {
            rsa = RSA;
            potpisi = new RSAPKCS1SignatureFormatter(rsa);
            sha1 = SHA1;
            RSAC = new RSACryptoServiceProvider();
            potpisi.SetHashAlgorithm("SHA1");
        }

        public byte[] Potpis(byte[] hash)
        {
            return potpisi.CreateSignature(hash);
        }

        public bool provjeriPotpis(byte[] hash, byte[] potpisanHash)
        {
            RSAParameters parametri = rsa.ExportParameters(false);
            RSAC.ImportParameters(parametri);
            RSAPK = new RSAPKCS1SignatureDeformatter(RSAC);
            RSAPK.SetHashAlgorithm("SHA1");
            try
            {
                if (RSAPK.VerifySignature(hash, potpisanHash) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}