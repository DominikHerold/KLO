using System.Configuration;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

using Klo.Core;

namespace Klo.Tray
{
    public partial class Form1 : Form
    {
        private readonly System.Timers.Timer _timer;

        private readonly WebClientWrapper _client;

        private readonly Core.Klo _klo;
        
        public Form1()
        {
            var host = ConfigurationManager.AppSettings["host"];
            InitializeComponent();
            _client = new WebClientWrapper();
            _klo = new Core.Klo(_client, host);

            _timer = new System.Timers.Timer(5000) { AutoReset = false };
            _timer.Elapsed += OnTimeEvent;
            OnTimeEvent(null, null);
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                var isInUse = _klo.IsInUse();
                if (!isInUse.HasValue)
                {
                    notifyIcon1.Icon = new Icon("warning.ico");
                    notifyIcon1.Text = "Fehler";
                }
                else if (isInUse.GetValueOrDefault(true))
                {
                    notifyIcon1.Icon = new Icon("trafficlight_red_16.ico");
                    notifyIcon1.Text = "Besetzt";
                }
                else
                {
                    notifyIcon1.Icon = new Icon("trafficlight_green_16.ico");
                    notifyIcon1.Text = "Frei";
                }
            }
            finally
            {
                _timer.Enabled = true;
            }
        }
    }
}
