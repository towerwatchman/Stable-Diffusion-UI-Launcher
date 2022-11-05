using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Stable_Diffusion_UI
{
    public static class ControlsInvoker
    {
        private static Label lbStatus;

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
    }
}
