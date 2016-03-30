using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Modules_Device
{
    public interface IGrabber
    {
        Image GetSet_GrabImg { get; set; }
    }

    public class Grabber_Euresys : IGrabber
    {
        public Image GetSet_GrabImg { get; set; }

    }
}
