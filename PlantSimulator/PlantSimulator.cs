using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Globalization;

namespace PlantSimulator
{
    public partial class PlantSimulator : Form
    {

        private System.ComponentModel.BackgroundWorker _backgroundWorker;

        public PlantSimulator()
        {
            InitializeComponent();
            InitFormControls();

        }

        bool _doSimulate;
        private void InitFormControls()
        {
            //add log format values to combobox and set default
            cmbxLogFormat.Items.Add("Kaco1");
            cmbxLogFormat.Items.Add("Kaco2");
            cmbxLogFormat.Items.Add("Generisch");

            for (int i = 1; i <= 5; i++)
            {
                cmbxInverterCount.Items.Add(i.ToString());
            }

            //preselect comboboxes
            cmbxLogFormat.SelectedItem = cmbxLogFormat.Items[0];
            cmbxInverterCount.SelectedItem = cmbxInverterCount.Items[0];

            //initialize trackbars
            tb_temperature.SetRange(-10, 120);
            tb_temperature.TickFrequency = 5;
            tb_wattage.SetRange(0, 15000);
            tb_wattage.TickFrequency = 1000;

            //disalbe stop button
            btnStop.Enabled = false;

        }

        private void toggleStartStopButton()
        {
            btnStop.Enabled = !btnStop.Enabled;
            btnStart.Enabled = !btnStart.Enabled;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _doSimulate = true; //set simulation active

            _backgroundWorker = new BackgroundWorker();

            _backgroundWorker.DoWork += new DoWorkEventHandler(StartLogging);
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.RunWorkerAsync();

            toggleStartStopButton();
        }

        void StartLogging(object sender, DoWorkEventArgs e)
        {
          Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            while (_doSimulate)
            {
                string url = "";
                int inverterCount = 1;
                string logFormat = "Kaco1";
                int plantId = 0;
                string password = "";
                int temperature = 50;
                int wattage = 1000;
                this.Invoke((MethodInvoker)delegate
                {
                    url = this.tbxUrl.Text;
                    inverterCount = Convert.ToInt32(this.cmbxInverterCount.SelectedItem);
                    logFormat = cmbxLogFormat.SelectedItem.ToString();
                    plantId = Convert.ToInt32(tbxPlantId.Text);
                    password = tbxPassword.Text;
                    wattage = tb_wattage.Value;
                    temperature = tb_temperature.Value;
                });

                for (int i = 1; i <= inverterCount; i++)
                {
                    LogMeasure(logFormat, i, url, plantId, password, wattage, temperature);
                }

                Thread.Sleep(2000);
            }

        }

        private void LogMeasure(string logFormat, int inverter, string url, int plantId,
                                string password, int wattage, int temperature)
        {
            // build logging url

            StringBuilder builder = new StringBuilder();
            switch (logFormat)
            {
                case "Kaco1":
                    builder.Append("Kaco1");
                    builder.AppendFormat("?data=26.12.2009;23:53:00;5;158.0;3.20;134;229.6;1.34;{0};{1}", wattage, temperature);
                    builder.Append("&inverter=" + inverter);
                    builder.Append("&plant=" + plantId);
                    builder.Append("&pw=" + password);

                    break;
                case "Kaco2":
                    builder.Append("Kaco2");
                    builder.AppendFormat("?data=*0{0}0;4;378.2;3.96;1498;228.9;6.55;{1};{2};5000;", inverter, wattage, temperature);
                    builder.Append("&plant=" + plantId);
                    builder.Append("&pw=" + password);

                    break;
                case "Generisch":
                    builder.Append("Generic");
                    builder.AppendFormat("?generatoramperage=" + 1.5);
                    builder.AppendFormat("&generatorvoltage=" + 220.1);
                    builder.AppendFormat("&generatorwattage=" + (wattage + 200));
                    builder.AppendFormat("&gridamperage=" + 1.9);
                    builder.AppendFormat("&gridvoltage=" + 240);
                    builder.AppendFormat("&feedinwattage=" + wattage);
                    builder.AppendFormat("&plant=" + plantId);
                    builder.AppendFormat("&inverter=" + inverter);
                    builder.Append("&pw=" + password);
                    builder.AppendFormat("&systemstatus=" + 5);
                    builder.AppendFormat("&temperature=" + temperature);
                    builder.AppendFormat("&timestamp=" + ((DateTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / TimeSpan.TicksPerSecond));
                    break;
            }

            //append query to url
            url += builder.ToString();

            // output to log textarea
            AppendToLogArea("URL: " + url);

            //call generated logging url
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                AppendToLogArea(reader.ReadToEnd() + Environment.NewLine);

            }
            catch (Exception ex)
            {
                AppendToLogArea(ex.Message);
            }
        }

        private void AppendToLogArea(string p)
        {
            // runs on UI thread
            this.Invoke((MethodInvoker)delegate
            {
                this.tbxLog.AppendText(DateTime.Now.ToLongTimeString());
                this.tbxLog.AppendText("\t");
                this.tbxLog.AppendText(p);
                this.tbxLog.AppendText(Environment.NewLine);
            });
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _doSimulate = false;
            _backgroundWorker.CancelAsync();
            _backgroundWorker.Dispose();

            toggleStartStopButton();
        }

        private void tb_temperature_ValueChanged(object sender, EventArgs e)
        {
            this.tbx_temperature.Text = this.tb_temperature.Value + " °C";
        }

        private void tb_wattage_ValueChanged(object sender, EventArgs e)
        {
            this.tbx_currentWattage.Text = this.tb_wattage.Value+ " W";
        }

    }
}
