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

namespace Stable_Diffusion_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();

        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;
        private static bool showConsole = true; //Or false if you don't want to see the console
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.MenuItem menuItem = new System.Windows.Forms.MenuItem();
        private System.ComponentModel.Container components = new System.ComponentModel.Container();
        private System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
        public MainWindow()
        {
            InitializeComponent();
            App.Current.MainWindow.Hide(); //hide the current window
            StartupWindow startupWindow = new StartupWindow();
            startupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startupWindow.Show();

            ControlsInvoker.startupWindow = startupWindow;
            //Console.Out.WriteLine(@"C:\stable-diffusion-ui\Start Stable Diffusion UI.cmd");
            ControlsInvoker.statusLabel(startupWindow.lbStatus);
            Task.Run(() => ScriptLauncher.RunScript("cmd.exe", @"C:\stable-diffusion-ui\Start Stable Diffusion UI.cmd"," ", true, false));


            //Notification Icon
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            //notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            notifyIcon.BalloonTipTitle = "Stable Diffusion UI";
            notifyIcon.Text = "Stable Diffusion UI";
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/sd.ico")).Stream;
            notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            ShowTrayIcon(true);
            //Context Menu for Notification Icon
            menuItem.Index = 0;
            menuItem.Text = "Show Console Window";
            menuItem.Click += MenuItem_Click;

            menuItem.Index = 1;
            menuItem.Text = "Exit";
            menuItem.Click += MenuItemExit_Click;

            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem });
            notifyIcon.ContextMenu = contextMenu;
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            
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

        private void Dispose ()
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
