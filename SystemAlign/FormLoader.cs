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
    public partial class FormLoader : Form
    {
        public FormLoader()
        {
            InitializeComponent();
        }

        private void uexpBarProcess_Click(object sender, EventArgs e)
        {

        }

        private void uexpBarProcess_ItemDoubleClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            if (e.Item.Key == "PowerOn")
                //this.PowerOn_Select(sender, e);
                this.PowerOn_Select();
            else if (e.Item.Key == "PowerOff")
                this.PowerOff_Select();
            else if (e.Item.Key == "LoaderFront")
                this.LoaderFront_Select();
            else if (e.Item.Key == "LoaderRear")
                this.LoaderRear_Select();
            else if (e.Item.Key == "LoaderInit")
                this.LoaderInit_Select();
            else if (e.Item.Key == "LoaderStop")
                this.LoaderStop_Select();
            else if (e.Item.Key == "WorkPlay")
                this.WorkPlay_Select();
            else if (e.Item.Key == "WorkEnd")
                this.WorkEnd_Select();
        }

        //private void PowerOn_Select(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        private void PowerOn_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "전원 ON", "로더의 전원을 인간합니다.");
            //messageBox.ShowDialog();
            return;
        }

        private void PowerOff_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "전원 OFF", "로더의 전원을 단절합니다.");
            //messageBox.ShowDialog();
            return;
        }

        private void LoaderFront_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "전방 이동", "로더를 전방으로 이동합니다.");
            //messageBox.ShowDialog();
            return;
        }

        private void LoaderRear_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "후방 이동", "로더를 후방으로 이동합니다.");
            //messageBox.ShowDialog();
            return;
        }
        private void LoaderInit_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "시작 이동", "로더의 시작 위치로 이동합니다.");
            //messageBox.ShowDialog();
            return;
        }
        private void LoaderStop_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "이동 엄춤", "로더의 이동을 멈춤니다.");
            //messageBox.ShowDialog();
            return;
        }
        private void WorkPlay_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "작업 적용", "로더의 조정을 적용합니다.");
            //messageBox.ShowDialog();
            return;
        }
        private void WorkEnd_Select()
        {
            CNIAMMessageBoxView messageBox = new CNIAMMessageBoxView();
            //FormMsgBox messageBox = new FormMsgBox();
            messageBox.ShowMessage("로더 조정", "전원 완료", "로더의 조정을 완료합니다.");
            //messageBox.ShowDialog();
            return;
        }

        private void upnlBack_PaintClient(object sender, PaintEventArgs e)
        {

        }
    }
}
