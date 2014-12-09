using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace projektOS
{
    class RSA
    {
        datotecniSustav datoteka = new datotecniSustav();
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
        public byte[] kriptiranje(byte[] cistiTekst)
        {
            if (datoteka.datotekaPostoji(@"..\..\..\Datoteke\javni_kljuc.xml") && datoteka.datotekaPostoji(@"..\..\..\Datoteke\privatni_kljuc.xml"))
            {
                MessageBox.Show("Datoteke javnog i privatnog kljuca vec postoje");
                TextReader reader = new StreamReader(@"..\..\..\Datoteke\javni_kljuc.xml");
                string publicKey = reader.ReadToEnd();
                reader.Close();
                rsa.FromXmlString(publicKey);
                byte[] rezultat = rsa.Encrypt(cistiTekst, false);
                return rezultat;
            }
            else
            {
                WriteRSAInfoToFile();
                TextReader reader = new StreamReader(@"..\..\..\Datoteke\javni_kljuc.xml");
                string publicKey = reader.ReadToEnd();
                reader.Close();
                rsa.FromXmlString(publicKey);
                byte[] rezultat = rsa.Encrypt(cistiTekst,false);
                string zapis = System.Convert.ToBase64String(rezultat, 0, rezultat.Length);
                datoteka.kreirajDatoteku("kriptirani_tekstRSA", zapis);
                return rezultat;
            }
        }
        public byte[] dekriptiranje(byte[] kriptiraniTekst)
        {
            try
            {
                TextReader reader = new StreamReader(@"..\..\..\Datoteke\privatni_kljuc.xml");
                string privateKey = reader.ReadToEnd();
                reader.Close();
                rsa.FromXmlString(privateKey);
                byte[] decryptedData = rsa.Decrypt(kriptiraniTekst,false);
                return decryptedData;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                byte[] a = new byte[1];
                return a;
            }
        }
        static void WriteRSAInfoToFile()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            TextWriter writer = new StreamWriter(@"..\..\..\Datoteke\javni_kljuc.xml");
            string publicKey = RSA.ToXmlString(false);
            writer.Write(publicKey);
            writer.Close();

            writer = new StreamWriter(@"..\..\..\Datoteke\privatni_kljuc.xml");
            string privateKey = RSA.ToXmlString(true);
            writer.Write(privateKey);
            writer.Close();
        }
    }
}
