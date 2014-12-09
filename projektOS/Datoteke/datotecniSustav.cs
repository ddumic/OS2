using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektOS
{
    class datotecniSustav
    {
        public void kreirajDatoteku(string naziv, string sadrzaj)
        {
            string path = @"..\..\..\Datoteke\" + naziv + ".txt";
            if (!datotekaPostoji(path))
            { 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(sadrzaj);
                }
            }
        }
        public void zapisi(string naziv, byte[] sadrzaj)
        {
            string path = @"..\..\..\Datoteke\" + naziv + ".txt";

            using (BinaryWriter writer = new BinaryWriter(new StreamWriter(path, false).BaseStream, Encoding.UTF8))
            {
                writer.Write(sadrzaj);
            }
        }
        public string citrajDatoteku(string naziv)
        {
            string path = @"..\..\..\Datoteke\" + naziv + ".txt";
            if (datotekaPostoji(path))
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(path);
                string myString = myFile.ReadToEnd();
                myFile.Close();
                return myString;
            }
            return "false";
        }
        public byte[] citaj(string naziv)
        {
            string path = @"..\..\..\Datoteke\" + naziv + ".txt";

            using (BinaryReader writer = new BinaryReader(new StreamReader(path, false).BaseStream, Encoding.UTF8))
                return writer.ReadBytes((int)writer.BaseStream.Length);
        }
        public bool datotekaPostoji(string putanja)
        {
            if (!File.Exists(putanja))
            {
                return false;
            }
            return true;
        }
    }
}
