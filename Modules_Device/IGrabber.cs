using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Modules_Device
{
    interface IGrabber
    {
        public Image GetSet_GrabImg { get; set; }
    }
}
