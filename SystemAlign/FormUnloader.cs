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
    public partial class FormUnloader : Form
    {
        public FormUnloader()
        {
            InitializeComponent();
        }
        //FormMsgBox messageBox = new FormMsgBox();
        private void utxtMovePosRun_Click(object sender, EventArgs e)
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            messageBox.ShowMessage("저~~ 작업자님", "여기서 이러시면 안됩니다.", "언능 데이터 입력하세요");
            //messageBox.ShowDialog();
        }

        private void utxtMovePosBack_Click(object sender, EventArgs e)
        {

        }

        private void ubtnStartPosMove_Click(object sender, EventArgs e)
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            messageBox.ShowMessage("저~~ 작업자님", "여기서 이러시면 안됩니다.", "언능 데이터 입력하세요");
            //messageBox.ShowDialog();
        }
    }
}
