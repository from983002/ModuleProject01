using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SystemAlign
{
    public partial class FormPicker : Form
    {
        public FormPicker()
        {
            InitializeComponent();
            string tempString11 = "이동\r\n" + "위치\r\n" + "mm";
            this.ultraTextEditor11.Text = tempString11;
        }
    }
}
