using System;
using System.Configuration;
using System.Drawing;
using System.Media;
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

        private bool _notificationWanted;
        private bool? _lastState = false;
        private DateTime _inUseSince = DateTime.Now;

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
            lock (this)
            {
                try
                {
                    _lastState = _klo.IsInUse();
                    if (!_lastState.HasValue)
                    {
                        notifyIcon1.Icon = new Icon("warning.ico");
                        notifyIcon1.Text = "Fehler";
                    }
                    else if (_lastState.GetValueOrDefault(true))
                    {
                        notifyIcon1.Icon = new Icon("trafficlight_red_16.ico");
                        notifyIcon1.Text = "Besetzt seit " + Math.Round((DateTime.Now - _inUseSince).TotalMinutes, 0) + " Minuten";
                    }
                    else
                    {
                        notifyIcon1.Icon = new Icon("trafficlight_green_16.ico");
                        notifyIcon1.Text = "Frei";
                        _inUseSince = DateTime.Now;

                        if (_notificationWanted)
                        {
                            _notificationWanted = false;
                            SendFreeNotification();
                        }
                    }
                }
                finally
                {
                    _timer.Enabled = true;
                }
            }
        }

        private void SendFreeNotification()
        {
            notifyIcon1.ShowBalloonTip(10000, "Erleichterung naht", "KLO ist frei! :-)", ToolTipIcon.Info);
            new SoundPlayer("spuelung.wav").Play();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            notifyWhenFreeToolStripMenuItem.Enabled = _lastState.GetValueOrDefault(false);
            notifyWhenFreeToolStripMenuItem.Checked = _notificationWanted;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnTimeEvent(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyWhenFreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_lastState.GetValueOrDefault())
                return;

            _notificationWanted = true;
            notifyWhenFreeToolStripMenuItem.Checked = true;
            notifyIcon1.ShowBalloonTip(2000, "Besetzt!", "Aber du wirst benachrichtigt, sobald KLO frei ist!", ToolTipIcon.Info);
        }
    }
}
