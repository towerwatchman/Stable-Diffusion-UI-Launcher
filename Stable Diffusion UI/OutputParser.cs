using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stable_Diffusion_UI
{
    public class OutputParser
    {
        public void parseString(string str)
        {
            Task.Run(() => ControlsInvoker.UpdateUIconsole(str));
                        
            if (str.Contains("Stable Diffusion is ready!"))
            {
                Task.Run(() => ControlsInvoker.hideStartupWindow());
            }
            else
            {
                Task.Run(() => ControlsInvoker.updateStartup("Loading: " + str));
            }

        }
    }
}
