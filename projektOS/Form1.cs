using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projektOS
{
    public partial class Form1 : Form
    {
        datotecniSustav datoteka = new datotecniSustav();
        kriptosustav kriptografija = new kriptosustav();
        RSA rsa = new RSA();
        hash sazetak = new hash();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                MessageBox.Show("Morate izabrati asimetricni ili simetricni algoritam");
            }
            else if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                MessageBox.Show("Odaberite samo jedan algoritam!");
            }
            else
            {
                string pomocna = datoteka.citrajDatoteku("vjezba");
                if (checkBox1.Checked == true)
                {
                    //simetricni algoritam
                    if (String.Compare(pomocna, "false") == 0)
                    {
                        MessageBox.Show("Datoteka ne postoji", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        richTextBox1.Text = pomocna;
                        byte[] aes = Encoding.UTF8.GetBytes(pomocna);
                        byte[] kriptiraniTekst = kriptografija.AESkriptiranje(aes);
                        string zapis = System.Convert.ToBase64String(kriptiraniTekst, 0, kriptiraniTekst.Length);
                        richTextBox2.Text = zapis;
                        datoteka.kreirajDatoteku("kriptirani_tekstAES", zapis);
                    }
                }
                else
                {
                    if (String.Compare(pomocna, "false") == 0)
                    {
                        MessageBox.Show("Datoteka ne postoji", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //asimetricni algoritam
                        richTextBox1.Text = pomocna;
                        byte[] rsaValue = Encoding.UTF8.GetBytes(pomocna);
                        byte[] kriptiraniTekst = rsa.kriptiranje(rsaValue);
                        string zapis = System.Convert.ToBase64String(kriptiraniTekst, 0, kriptiraniTekst.Length);
                        richTextBox2.Text = zapis;
                        datoteka.kreirajDatoteku("kriptirani_tekstRSA", zapis);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                MessageBox.Show("Morate izabrati asimetricni ili simetricni algoritam");
            }
            else if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                MessageBox.Show("Odaberite samo jedan algoritam!");
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    byte[] base64Array;
                    if (datoteka.datotekaPostoji(@"..\..\..\Datoteke\kriptirani_tekstAES.txt"))
                    {
                        base64Array = System.Convert.FromBase64String(datoteka.citrajDatoteku("kriptirani_tekstAES"));
                        byte[] dekriptiraniTekst = kriptografija.AESdekriptiranje(base64Array);
                        string zapis = System.Text.Encoding.UTF8.GetString(dekriptiraniTekst);
                        richTextBox2.Text = zapis;
                    }
                    else
                    {
                        base64Array = System.Convert.FromBase64String(richTextBox2.Text);
                        byte[] dekriptiraniTekst = kriptografija.AESdekriptiranje(base64Array);
                        string zapis = System.Text.Encoding.UTF8.GetString(dekriptiraniTekst);
                        richTextBox2.Text = zapis;
                    }
                }
                else
                {
                    if (datoteka.datotekaPostoji(@"..\..\..\Datoteke\kriptirani_tekstRSA.txt"))
                    {
                        byte[] base64RSA = System.Convert.FromBase64String(datoteka.citrajDatoteku("kriptirani_tekstRSA"));
                        byte[] dekriptiraniTekst = rsa.dekriptiranje(base64RSA);
                        string zapis = System.Text.Encoding.UTF8.GetString(dekriptiraniTekst);
                        richTextBox2.Text = zapis;
                    }
                    else
                    {
                        byte[] rsaValue = System.Convert.FromBase64String(richTextBox2.Text);
                        byte[] dekriptiraniTekst = rsa.dekriptiranje(rsaValue);
                        string zapis = System.Text.Encoding.UTF8.GetString(dekriptiraniTekst);
                        richTextBox2.Text = zapis;
                    }
                }
            }
        }

        private void Sazetak_Click(object sender, EventArgs e)
        {
            //sazetak
            string pomocna = datoteka.citrajDatoteku("vjezba");
            byte[] sazetakPoruke = Encoding.ASCII.GetBytes(pomocna);
            richTextBox3.Text = sazetak.sazetak(sazetakPoruke);
            datoteka.kreirajDatoteku("sazetak", richTextBox3.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            potpis potpis = new potpis(new SHA1Managed(), new RSACryptoServiceProvider());
            TextReader reader = new StreamReader(@"..\..\..\Datoteke\privatni_kljuc.xml");
            potpis.rsa.FromXmlString(reader.ReadToEnd());
            string sazetak = datoteka.citrajDatoteku("vjezba");
            byte[] sazetakByte = Encoding.UTF8.GetBytes(sazetak);
            byte[] returnValue = potpis.Potpis(potpis.sha1.ComputeHash(sazetakByte));
            string zapis = Encoding.UTF8.GetString(returnValue);
            MessageBox.Show("Digitalni potpis: " + Convert.ToBase64String(returnValue));

            //Debug.WriteLine("dat:" + sazetak);
            Debug.WriteLine("hash:" + Encoding.UTF8.GetString(potpis.sha1.ComputeHash(sazetakByte)));
            //Debug.WriteLine("signed:" + zapis);

            datoteka.zapisi("digitalniPotpis", returnValue);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            potpis p = new potpis(new SHA1Managed(), new RSACryptoServiceProvider());
            TextReader reader = new StreamReader(@"..\..\..\Datoteke\javni_kljuc.xml");
            string sazetak = datoteka.citrajDatoteku("vjezba");
            byte[] sazetakByte = Encoding.UTF8.GetBytes(sazetak);
            byte[] hash = p.sha1.ComputeHash(sazetakByte);
            p.rsa.FromXmlString(reader.ReadToEnd());
            //Debug.WriteLine("dat:" + sazetak);
            Debug.WriteLine("hash:" + Encoding.UTF8.GetString(hash));
            if (p.provjeriPotpis(hash, (datoteka.citaj("digitalniPotpis"))) == true)
            {
                MessageBox.Show("Potpis je valjan!", "Success");
            }
            else
            {
                MessageBox.Show("Potpis nije valjan!", "ERROR", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }
    }
}
