using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Stable_Diffusion_UI
{
    public static class ControlsInvoker
    {
        private static Label lbStatus;
        public static RichTextBox uiConsole;

        public static Window startupWindow;
        public static void statusLabel(Label label)
        {
            lbStatus = label;
        }

        public static void updateStartup(string content)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                lbStatus.Content = content;
            }));
        }

        public static void hideStartupWindow()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                startupWindow.Hide();
            }));
        }

        public static void UpdateUIconsole(string text)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var box = uiConsole;                
                box.AppendText(text + Environment.NewLine);
                uiConsole.ScrollToEnd();
            }));
        }


    }
}
