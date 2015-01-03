using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Threading;
using System;
using System.IO;

/*
 * Coder & designer by : Kamil YEŞİL
 * Web: http://kyesil.com
 */
namespace krometre
{
    public partial class ana : Form
    {
        int yont=0;
        aboutfr hakk;
        string ilkgost = "00:00:00.00";
        TimeSpan sure = new TimeSpan(0, 0, 0, 0, 0);
        DateTime bas = new DateTime();
        DateTime drk = new DateTime();
        Thread th;
        public FontFamily ozelfont(string url)
        {
            PrivateFontCollection customs = new PrivateFontCollection();
            customs.AddFontFile(url);
            return customs.Families[0]; 
        }
        void bitti(int md)
        {
            MessageBox.Show("Süre Bitti!", "Geri sayım");
         /* Geri sayım aracı bittiğinde buraya gelicek*/
        }
        void yaz()
        {   string son=sure.ToString();
            if (yont==1)sure = DateTime.Now - bas; else sure =   bas - DateTime.Now;
            try
            {
            
                lbgost.Text = son.Remove(son.Length - 5, 5);
                lbgost.Refresh();
            }
            catch (Exception ) {
                
            }
        }
        public void arader()
        {
            while (true)
            {
                yaz();
            }
        }
        public void gerisay()
        {   bas = bas + sure; 
            while (bas>DateTime.Now)
            {
                yaz();
            }
            btzero.Enabled = false;
            btstart.Enabled = true;
            btpause.Enabled = false;
            bitti(0);
        
        }
        public void baslat(int mod)
        {
            th = null;
            switch (mod)
            {
                case 1: th = new Thread(new ThreadStart(arader)); break;
                case 2: th = new Thread(new ThreadStart(gerisay)); break;
                case 3: { } break;
               
            }
            bas = DateTime.Now;
            th.Start();
            while (!th.IsAlive) ;
            
        }
        public void devamet()
        {
            bas+=(DateTime.Now - drk);
            th.Resume(); 
              while (!th.IsAlive) ;
        }

        public ana()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            lbgost.Font = new Font(ozelfont(@"digital.TTF"), 40);
            lbdurum.Font = new Font(ozelfont(@"digital.TTF"), 15);
            lbdurum.Text = "Hazır";
        }

        private void btbasla_Click(object sender, System.EventArgs e)
        {

            if (sender != btstart) yont = 1;
                if (th == null) baslat(yont);
                else if ((th.ThreadState == ThreadState.Unstarted) || (th.ThreadState == ThreadState.AbortRequested) || (th.ThreadState == ThreadState.Stopped) || (th.ThreadState == ThreadState.Aborted) || (th.ThreadState == ThreadState.StopRequested))
                    baslat(yont);
                else
                    if (th.IsAlive) devamet();

            btdurakla.Enabled = true;
            btpause.Enabled = true;
            btdurdur.Enabled = true;
            btzero.Enabled = true;
            btbasla.Enabled = false;
            btstart.Enabled = false;

            //lbdurum.Text = "Calısıyor..";

        }

        private void ana_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            { 
                if (th.ThreadState >= ThreadState.SuspendRequested) devamet();

                th.Abort();
                th.Join();
            }
            catch (Exception)
            {
                Application.Exit();
            }
        }

        private void btdurakla_Click(object sender, EventArgs e)
        {   
            btbasla.Enabled = true;
            btstart.Enabled = true;
            btdurakla.Enabled = false;
            btpause.Enabled = false;
            //lbdurum.Text = "Beklıyor..";
            try
            {
               drk = DateTime.Now; th.Suspend();
            }
            catch (Exception) { }


        }

        private void zamanla_Tick(object sender, EventArgs e)
        {
            try
            {
                if (th != null) lbdurum.Text = th.ThreadState.ToString();
            }
            catch
            {
            
            }
        }

        private void btsifir_Click(object sender, EventArgs e)
        {
            bas = DateTime.Now;
            btarader.Enabled = true;btarader.Enabled = true;
        }

        private void btdurdur_Click(object sender, EventArgs e)
        {
            try
            {   
               // lbdurum.Text = "Durdu.." ;
                btbasla.Enabled = true;
                btstart.Enabled = true;
                btarader.Enabled = true;
                btdurakla.Enabled = false;
                btpause.Enabled = false;
                btturz.Enabled = true;
                btdurdur.Enabled = false;
                btzero.Enabled = false;
                if (th.ThreadState >= ThreadState.SuspendRequested) devamet();
                th.Abort();
                th.Join(100);
                while (th.IsAlive) ;
                lbgost.Text = ilkgost;
               // lbdurum.Text= th.ThreadState.ToString();
                
            }
            catch (Exception ht)
            {
                lbgost.Text = ilkgost;
                MessageBox.Show(ht.ToString());

            }
            
        }

        private void btarader_Click(object sender, EventArgs e)
        {
            btturz.Enabled = false;
            grder.RowCount++;
            grder.Rows[grder.RowCount - 2].Cells[0].Value = lbgost.Text;
            grder.Rows[grder.RowCount - 2].Cells[1].Value = DateTime.Now.ToString();

            
        }

        private void btturz_Click(object sender, EventArgs e)
        {
            btarader.Enabled = false;
            btarader_Click(btturz, EventArgs.Empty);
            btturz.Enabled = true;
            bas = DateTime.Now;
        }

        private void btaktar_Click(object sender, EventArgs e)
        {
            if (saveaktar.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter dsy = new StreamWriter(saveaktar.FileName);
                    for (int j = 0; j <= grder.RowCount - 2; j++)
                    {
                        string s = grder.Rows[j].Cells[0].Value.ToString(); 
                        if (!chder.Checked) s = s + " | " + grder.Rows[j].Cells[1].Value.ToString();
                        dsy.WriteLine(s);  
                    }
                    dsy.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString()+"Kaydedilirken bir hata ile karşılaşıldı. Yazılabilirbir ortam seçtiğinizden emin olup tekrar deneyin.", "Hata!");
                }
            }
        }

        private void btsil_Click(object sender, EventArgs e)
        {
            grder.Rows.Clear();
        }

        private void btstart_Click(object sender, EventArgs e)
        {
            yont = 2;
            sure = new TimeSpan((int)nugun.Value, (int)nusaat.Value, (int)nudak.Value, (int)nusan.Value);
            btbasla_Click(btstart, null);
           

        }

        private void pn_disp_DoubleClick(object sender, EventArgs e)
        {
           hakk= new aboutfr();
            hakk.Opacity = 0.0;
            hakk.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://kyesil.com");
        }





    }
}
