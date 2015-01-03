using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace krometre
{
    public partial class aboutfr : Form
    {
        ThreadStart ths = null;
        Thread th1 = null;
        ThreadStart mths = null;
        Thread mth = null;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public aboutfr()
        {
            InitializeComponent();
        }

        private void resim_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

        }



        void musikcal()
        {
            try
            {
                if (File.Exists("jen.wma")) mp.URL ="jen.wma" ;
                else MessageBox.Show("Burlarda biyerde jen.wma dosyası olacaktı ne yaptın ona söyle.:) \nSana müzik çalacaktım ama şansını kaybettin.", "Hata!");
            }
            catch
            {
                
            }
           
        }


        private void goster() 
        {
            for (int j = 0; j <= 50; j++) 
            {
                this.Opacity = j * 0.02;
                th1.Join(50);
            
            }

            do
            {
                label1.Left++;
                label2.Top++;
                Thread.Sleep(30);
                label2.Left++;
                label1.Top++;
                Thread.Sleep(30);
                label2.Left--;
                label1.Left--;
                Thread.Sleep(30);
                label2.Top--;
                label1.Top--;




            } while (true);
        
        }

        private void gizle()
        {
            for (int j = 0; j <= 25; j++)
            {
                this.Opacity =this.Opacity -  0.04;
                th1.Join(50);
                

            }

            th1.Abort();
        }

        private void aboutfr_Shown(object sender, EventArgs e)
        {            
            mths = new ThreadStart(musikcal);
            mth = new Thread(mths);
            mth.Start();
            ths = new ThreadStart(goster);
            th1 = new Thread(ths);
            th1.Start();

        }

        private void aboutfr_FormClosing(object sender, FormClosingEventArgs e)
        {
            mth.Abort();
            mth.Join();
            th1.Abort();
            th1.Join();
            ths = new ThreadStart(gizle);
            th1 = new Thread(ths);
            th1.Start();
            th1.Join();
            
            
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void resim_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        private void aboutfr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27) this.Close();
        }
    }
}
