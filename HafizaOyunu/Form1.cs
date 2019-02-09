using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HafizaOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[] sayiDizisi = new int[10];
        int[] buttonIkonNumara = new int[5];
        int[] buttonIkonNumara1 = new int[5];
        Button oncekiButon;
        int _puan;
        bool _timerControl = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();

            int gecici;
            bool sayiVarMi;
            int sayac = 0;
            do
            {
                gecici = rnd.Next(1, 11);
                sayiVarMi = sayiDizisi.Contains(gecici);
                if (!sayiVarMi)
                {
                    sayiDizisi[sayac] = gecici;
                    sayac++;
                }
            } while (sayac != 10);

            Array.Copy(sayiDizisi, buttonIkonNumara, 5);
            Array.Copy(sayiDizisi, 5, buttonIkonNumara1, 0, 5);

            this.Text = "3:00";

            for (int i = 0; i < 10; i++)
            {
                Button btn = new Button();
                btn.Width = 50;
                btn.Height = 50;

                int ikonSirasi = Array.IndexOf(buttonIkonNumara, i + 1);

                if (ikonSirasi == -1)
                {
                    ikonSirasi = Array.IndexOf(buttonIkonNumara1, i + 1);
                }

                btn.Tag = imageList1.Images[ikonSirasi];

                btn.Name = ikonSirasi.ToString();
                btn.Click += Btn_Click;
                btn.MouseDown += Btn_MouseDown;
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                flwButtons.Controls.Add(btn);
            }

            SureAzalt(this.Text);
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackgroundImage = (Image)btn.Tag;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (_timerControl)
            {
                timer1.Start();
                _timerControl = false;
            }

            Button btn = (Button)sender;
            Image img = (Image)btn.Tag;
            btn.BackgroundImage = img;

            if (oncekiButon == null)
            {
                oncekiButon = btn;
            }
            else
            {
                if (btn.Name == oncekiButon.Name)
                {
                    //flwButtons.Controls.Remove(btn);
                    //flwButtons.Controls.Remove(oncekiButon);
                    btn.BackgroundImage = null;
                    btn.BackColor = Color.Red;

                    oncekiButon.BackgroundImage = null;
                    oncekiButon.BackColor = Color.Red;

                    oncekiButon = null;

                    if (!string.IsNullOrEmpty(label1.Text))
                    {
                        _puan = Convert.ToInt32(label1.Text);
                        _puan += 100;
                        label1.Text = _puan.ToString();
                    }
                    else
                    {
                        label1.Text = "100";
                    }
                }
                else
                {
                    btn.BackgroundImage = null;
                    oncekiButon.BackgroundImage = null;

                    oncekiButon = null;
                }
            }

            bool oyunBittiMi = ButonKontrol();

            if (!oyunBittiMi)
            {
                OyunDurum("Süreniz bitti, oyunu kaybettiniz. Biraz ceviz yeyin :) Yeniden başlamak ister misiniz");
            }
        }

        public string SureAzalt(string deger)
        {
            string[] degerDizisi = deger.Split(':');
            int[] degerDizisi1 = { Convert.ToInt32(degerDizisi[0]), Convert.ToInt32(degerDizisi[1]) };

            if (degerDizisi1[0] == 3)
            {
                degerDizisi1[0] = degerDizisi1[0] - 1;
            }
            else if (degerDizisi1[0] == 2 && (degerDizisi1[1] == 0 || degerDizisi1[1] == 00))
            {
                degerDizisi1[0] = degerDizisi1[0] - 1;
            }
            else if (degerDizisi1[0] == 1 && (degerDizisi1[1] == 0 || degerDizisi1[1] == 00))
            {
                degerDizisi1[0] = degerDizisi1[0] - 1;
            }

            if (degerDizisi1[1] == 00 || degerDizisi1[1] == 0)
            {
                degerDizisi1[1] = 59;
            }
            else
            {
                degerDizisi1[1] = degerDizisi1[1] - 1;
            }

            return degerDizisi1[0] + ":" + degerDizisi1[1];
        }

        public bool ButonKontrol()
        {
            bool durum = false;
            foreach (Button item in flwButtons.Controls)
            {
                if (item.BackColor != Color.Red)
                {
                    durum = true;
                }
            }
            return durum;
        }

        public void OyunDurum(string mesaj)
        {
            DialogResult dialog = MessageBox.Show(mesaj, "YOU ARE LOSE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DialogResult.Yes == dialog)
            {
                Application.Restart();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = SureAzalt(this.Text);

            if (this.Text == "0:0")
            {
                timer1.Stop();                
            }
        }
    }
}
