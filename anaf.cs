using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Threading;
using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;







namespace krometre
{
    public partial class ana : Form
    {
        int yont=0;
        double debi = 0;
        int datasayisi = 0;
        string ilkgost = "00:00:00.00";
        string RxString = "";
        TimeSpan sure = new TimeSpan(0, 0, 0, 0, 0);
        double sure2 = 0;
        DateTime bas = new DateTime();
        DateTime drk = new DateTime();
        Thread th;
        double ymin = 0;
        double ymax = 0;

  
        
        public FontFamily ozelfont(string url)
        {
            PrivateFontCollection customs = new PrivateFontCollection();
            customs.AddFontFile(url);
            return customs.Families[0]; 
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

        public void baslat(int mod)
        {
            th = null;
            switch (mod)
            {
                case 1: th = new Thread(new ThreadStart(arader)); break;
                
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
          //  lbgost.Font = new Font(ozelfont(@"digital.TTF"), 20);
            //lbdurum.Font = new Font(ozelfont(@"digital.TTF"), 15);
            lbdurum.Text = "Hazır";
        }

        private void btbasla_Click(object sender, System.EventArgs e)

        {
            serialPort1.PortName = cmbPort.Text;
            serialPort1.BaudRate = 9600;
            serialPort1.DtrEnable = true; // leonardo için gerekli
            serialPort1.RtsEnable = true; // leonardo için gerekli

            serialPort1.Open();
          //  if (RxString != "")
         //   {
                if (sender != btstart) yont = 1;
                if (th == null) baslat(yont);
                else if ((th.ThreadState == ThreadState.Unstarted) || (th.ThreadState == ThreadState.AbortRequested) || (th.ThreadState == ThreadState.Stopped) || (th.ThreadState == ThreadState.Aborted) || (th.ThreadState == ThreadState.StopRequested))
                  
                    baslat(yont);
                    
                else
                    if (th.IsAlive) devamet();


                btpause.Enabled = true;
                btdurdur.Enabled = true;
                btzero.Enabled = true;
                btbasla.Enabled = false;
                btstart.Enabled = false;
                lbdurum.Text = "Baglandı ...";
                lbdebi.ForeColor = Color.Lime;
                label10.ForeColor = Color.Lime;
            }
            

       // }
        private void refreshSerialPorts()
        {
            var ports = SerialPort.GetPortNames();
            cmbPort.DataSource = ports;

         
        }
        private void ana_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
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
           
            btpause.Enabled = false;
            lbdurum.Text = "Bekliyor ...";
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
                if (th != null) lbdurum.Text = "Data Bekleniyor ...";
            }
            catch
            {
            
            }
        }

        private void btsifir_Click(object sender, EventArgs e)
        {
            grder.Rows.Clear();
            bas = DateTime.Now;
            RxString = "";
            debi = 0;
            sure2 = 0;
            datasayisi = 0;
            lbdebi.Text = ": 0";
            lbanlik.Text = ": 0";
            label9.Text = ": 0";
            chart1.Series.Clear();
            chart1.Series.Add("Anlık Debi (lt/dk)");
            chart1.Series["Anlık Debi (lt/dk)"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["Anlık Debi (lt/dk)"].Color = Color.Red;

        }

        private void btdurdur_Click(object sender, EventArgs e)
        {
            try
            {   
                
                btbasla.Enabled = true;
                btstart.Enabled = true;
               
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
                lbdurum.Text = "Durduruldu ..." ;
                lbdebi.ForeColor = Color.Red;
                label10.ForeColor = Color.Red;
            }
            catch (Exception ht)
            {
                lbgost.Text = ilkgost;
                MessageBox.Show(ht.ToString());

            }

            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
               
            }
        }

        private void btarader_Click(object sender, EventArgs e)
        {
           
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
          // provider.NumberGroupSizes = new int[] { 3 };
            datasayisi = datasayisi+1;
            btturz.Enabled = false;
            grder.RowCount++;
            grder.Rows[grder.RowCount - 2].Cells[0].Value = datasayisi.ToString();
            grder.Rows[grder.RowCount - 2].Cells[1].Value = DateTime.Now.ToString("HH:mm:ss");
            grder.Rows[grder.RowCount - 2].Cells[2].Value = lbgost.Text;
            grder.Rows[grder.RowCount - 2].Cells[3].Value = Convert.ToDouble(RxString, provider).ToString();
            grder.Rows[grder.RowCount - 2].Cells[4].Value = (Convert.ToDouble(RxString, provider) * Convert.ToDouble(sure.TotalSeconds, provider) / 60).ToString("0.######");
            debi = debi + Convert.ToDouble(RxString, provider) * Convert.ToDouble(sure.TotalSeconds, provider) / 60; // 60 e bölüp lt'ye cevirildi.
            lbdebi.Text = ": " + debi.ToString("0.###");
            lbanlik.Text = ": " +  Convert.ToDouble(RxString, provider).ToString(); 
            sure2 = sure2 + sure.TotalSeconds;
            label9.Text = ": " + sure2.ToString("0.###");
            chart1.Series["Anlık Debi (lt/dk)"].Points.AddXY(datasayisi, Convert.ToDouble(RxString, provider));
            if (Convert.ToDouble(RxString, provider) > ymax & Convert.ToDouble(RxString, provider)>ymin)
            {
                ymax=Convert.ToDouble(RxString, provider) + 0.1;
                ymin = Convert.ToDouble(RxString, provider) - 0.1;
            }
            if (Convert.ToDouble(RxString, provider) > ymax )
            {
                ymax = Convert.ToDouble(RxString, provider) + 0.1;
            }
            if (Convert.ToDouble(RxString, provider) < ymin)
            {
               ymin = Convert.ToDouble(RxString, provider) - 0.1;
            }
            
            chart1.ChartAreas[0].AxisY.Maximum = ymax;
            chart1.ChartAreas[0].AxisY.Minimum = ymin;
            lbdurum.Text = "Data Aliniyor ...";
        }

        private void btturz_Click(object sender, EventArgs e)
        {
           
            btarader_Click(btturz, EventArgs.Empty);
            btturz.Enabled = true;
            bas = DateTime.Now;
            grder.FirstDisplayedCell = grder.Rows[grder.Rows.Count - 1].Cells[0];
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
                        string s = grder.Rows[j].Cells[0].Value.ToString() + " | " + grder.Rows[j].Cells[1].Value.ToString() + " | " + grder.Rows[j].Cells[2].Value.ToString() + " | " + grder.Rows[j].Cells[3].Value.ToString() + " | " + grder.Rows[j].Cells[4].Value.ToString(); 
                     
                        dsy.WriteLine(s);  
                    }

                    dsy.WriteLine("-------------------------------------------------");
                    dsy.WriteLine("Toplam Geçen Gaz (lt)" + lbdebi.Text);
                    dsy.WriteLine("Toplam Süre (sn)" + label9.Text);  
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
            bas = DateTime.Now;
            RxString = "";
            debi = 0;
            sure2 = 0;
            datasayisi = 0;
            lbdebi.Text = ": 0";
            lbanlik.Text = ": 0";
            label9.Text = ": 0";
        }

        private void btstart_Click(object sender, EventArgs e)
        {
            yont = 2;
            sure = new TimeSpan((int)nugun.Value, (int)nusaat.Value, (int)nudak.Value, (int)nusan.Value);
            btbasla_Click(btstart, null);
           

        }

        private void chder_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadLine();
            this.Invoke(new EventHandler(DisplayText));
        }

        private void DisplayText(object sender, EventArgs e)
        {
            
            // textBox2.AppendText(RxString.Substring(0, RxString.IndexOf(",") + 0) + Environment.NewLine);

            RxString= (RxString.Remove(RxString.IndexOf(",")));
            btarader_Click(btturz, EventArgs.Empty);
            btturz.Enabled = true;
            bas = DateTime.Now;
            grder.FirstDisplayedCell = grder.Rows[grder.Rows.Count - 1].Cells[0];
           
        }

        private void grder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ana_Load(object sender, EventArgs e)
        {
            refreshSerialPorts();
            chart1.Series.Clear();
            chart1.Series.Add("Anlık Debi (lt/dk)");
            chart1.Series["Anlık Debi (lt/dk)"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["Anlık Debi (lt/dk)"].Color = Color.Red;
            
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

 


    }
}
