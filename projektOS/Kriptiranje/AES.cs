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
    class kriptosustav
    {
        datotecniSustav datoteka = new datotecniSustav();
        public byte[] AESkriptiranje(byte[] cistiTekst)
        {
            RijndaelManaged AES = new RijndaelManaged();
            //provjera da li vec datoteke kljuceva postoje
            if (datoteka.datotekaPostoji(@"..\..\..\Datoteke\tajni_kljuc.txt"))
            {
                MessageBox.Show("AES tajni kljuc vec postoji!");
                //kriptiraj
                string kljuc = datoteka.citrajDatoteku("tajni_kljuc");
                byte[] base64Array = System.Convert.FromBase64String(kljuc);
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Mode = CipherMode.ECB;
                AES.Key = base64Array;
                using (MemoryStream mem = new MemoryStream())
                {
                    using (CryptoStream crypStream = new CryptoStream(mem, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypStream.Write(cistiTekst, 0, cistiTekst.Length);
                    }
                    return mem.ToArray();
                }
            }
            else
            {
                //ne postoji javni kljuc
                //generiraj javni kljuc
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Mode = CipherMode.ECB;
                byte[] tajniKljuc = AES.Key;
                string zapis = System.Convert.ToBase64String(tajniKljuc, 0, tajniKljuc.Length);
                //kreiraj datoteku s javnim kljucem
                datoteka.kreirajDatoteku("tajni_kljuc", zapis);
                //kriptiraj
                using (MemoryStream mem = new MemoryStream())
                {
                    using (CryptoStream crypStream = new CryptoStream(mem, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypStream.Write(cistiTekst, 0, cistiTekst.Length);
                    }
                    return mem.ToArray();
                }
            }   
        }

        public byte[] AESdekriptiranje(byte[] kriptiraniTekst)
        {
            try
            {

                RijndaelManaged AES = new RijndaelManaged();
                string kljuc = datoteka.citrajDatoteku("tajni_kljuc");
                byte[] base64Array = System.Convert.FromBase64String(kljuc);
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Mode = CipherMode.ECB;
                AES.Key = base64Array;
                using (MemoryStream mem = new MemoryStream())
                {
                    using (CryptoStream cryptStream = new CryptoStream(mem, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptStream.Write(kriptiraniTekst, 0, kriptiraniTekst.Length);
                    }
                    return mem.ToArray();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                byte[] b = new byte[1];
                return b;
            }
        }
    }
}
