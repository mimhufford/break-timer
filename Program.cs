using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Break
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new BreakApp());
        }
    }

    public class BreakApp : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public BreakApp()
        {
            var notifier = new Thread(new ThreadStart(() =>
            {
                NotifyIcon toast = new NotifyIcon { Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location) };

                const int SHOW_TIME = 5 * 1000;
                const int FREQUENCY = 10 * 60 * 1000;

                while (true)
                {
                    toast.Visible = true;
                    toast.ShowBalloonTip(0, "Have a stretch", " ", ToolTipIcon.None);
                    Thread.Sleep(SHOW_TIME);
                    toast.Visible = false;
                    Thread.Sleep(FREQUENCY);
                }
            }));

            notifier.Start();

            trayIcon = new NotifyIcon()
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = true,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", (object sender, EventArgs e) => {
                        trayIcon.Visible = false;
                        notifier.Abort();
                        Application.Exit();
                    })
                }),
            };
        }
    }
}