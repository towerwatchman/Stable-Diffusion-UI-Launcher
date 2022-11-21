using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Stable_Diffusion_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.MenuItem menuItem1 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem2 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem3 = new System.Windows.Forms.MenuItem();
        private System.ComponentModel.Container components = new System.ComponentModel.Container();
        private System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
        private UI_Console uI_Console;
        public MainWindow()
        {
            InitializeComponent();
            App.Current.MainWindow.Hide(); //hide the current window
            StartupWindow startupWindow = new StartupWindow();
            startupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startupWindow.Show();

            uI_Console = new UI_Console();
            uI_Console.Show();
            ControlsInvoker.uiConsole = uI_Console.uiConsole;

            ControlsInvoker.startupWindow = startupWindow;
            //Console.Out.WriteLine(@"C:\stable-diffusion-ui\Start Stable Diffusion UI.cmd");
            ControlsInvoker.statusLabel(startupWindow.lbStatus);
            Task.Run(() => ScriptLauncher.RunScript("cmd.exe", @"C:\stable-diffusion-ui\Start Stable Diffusion UI.cmd", " ", true, false));
            App.Current.MainWindow.Hide(); //hide the current window

            //Notification Icon
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            //notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            notifyIcon.BalloonTipTitle = "Stable Diffusion UI";
            notifyIcon.Text = "Stable Diffusion UI";
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/sd.ico")).Stream;
            notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            ShowTrayIcon(true);
            //Context Menu for Notification Icon
            menuItem1.Index = 0;
            menuItem1.Text = "Show Console Window";
            menuItem1.Click += MenuItemConsole_Click;

            menuItem2.Index = 0;
            menuItem2.Text = "Show Web Interface";
            menuItem2.Click += MenuItemWeb_Click;

            menuItem3.Index = 1;
            menuItem3.Text = "Exit";
            menuItem3.Click += MenuItemExit_Click;

            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1 });
            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem2 });
            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem3 });

            notifyIcon.ContextMenu = contextMenu;
        }

        //http://localhost:9000/
        private void MenuItemWeb_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost:9000/");
        }
        private void MenuItemConsole_Click(object sender, EventArgs e)
        {
            //uI_Console = new UI_Console();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                uI_Console.Show();
            }));
        }
        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        void ShowTrayIcon(bool show)
        {
            if (notifyIcon != null)
                notifyIcon.Visible = show;
        }

        private void Dispose()
        {
            notifyIcon.Dispose();
            notifyIcon = null;
            ProcessManager.KillProcess(ScriptLauncher.p.Id);
            ScriptLauncher.p.Close();
            ScriptLauncher.p.Dispose();
            App.Current.Shutdown();
        }
    }
}
