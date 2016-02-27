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
    public partial class FormDlgModelEdit : Form
    {
        public FormDlgModelEdit()
        {
            InitializeComponent();
            this.Text = CaptionString;
        }

        public static string CaptionString;

        string[] inputData;

        public string[] InputData
        {
            get { return this.inputData; }
            set { this.inputData = value; }
        }

        private string _strModelName = null;
        public string GetSet_Model_Name
        {
            get { return this._strModelName; }
            set { this._strModelName = value; }
        }

        private string _strModelNumber = null;
        public string GetSet_Model_Number
        {
            get { return this._strModelNumber; }
            set { this._strModelNumber = value; }
        }

        public void EditDataDisplay()
        {
            this.utxtNumber.Text = this.inputData[0];
            this.utxtPassword.Text = this.inputData[1];
        }

        private void ubtnOK_Click(object sender, EventArgs e)
        {
            if (this.TextBoxNullCheck() == false)
            {
                Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                if (messageBox.MessageBox_Show("모델 추가", "모델 추가 정보 오류.", "추가하고자 하는 모델의 이름과 모델 번호를 입력해주세요!<br/><br/> 모델 이름과 모델 번호는 반드시 입력해야 합니다.", MessageBoxButtons.OK,
                    MessageBoxIcon.Error) == DialogResult.OK)
                    Display_Cursor_Point();
                return;
            }

            if (int.Parse(utxtPassword.Text) < 1)
            {
                Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                if (messageBox.MessageBox_Show("모델 추가", "모델 추가 정보 오류.", "모델 번호를 잘못 입력했습니다 !<br/><br/>모델 번호는 0보다 커야 합니다.", MessageBoxButtons.OK,
                    MessageBoxIcon.Error) == DialogResult.OK)
                    utxtPassword.Select();
                return;
            }

            if (int.Parse(utxtPassword.Text) > 1000)
            {
                Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                if (messageBox.MessageBox_Show("모델 추가", "모델 추가 정보 오류.", "모델 번호를 잘못 입력했습니다 !<br/><br/>모델 번호는 1000보다 작아야 합니다.", MessageBoxButtons.OK,
                    MessageBoxIcon.Error) == DialogResult.OK)
                    utxtPassword.Select();
                return;
            }

            this.utxtNumber.Text = this.utxtNumber.Text.TrimStart();
            this.utxtNumber.Text = this.utxtNumber.Text.TrimEnd();

            this.utxtPassword.Text = this.utxtPassword.Text.TrimStart();
            this.utxtPassword.Text = this.utxtPassword.Text.TrimEnd();

            string[] inData = { this.utxtNumber.Text, this.utxtPassword.Text};
            this.InputData = inData;

            GetSet_Model_Name = utxtNumber.Text;
            GetSet_Model_Number = utxtPassword.Text;
            
            this.DialogResult = DialogResult.OK;
        }

        private void ubtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        //사번과 비밀번호에 데이터가 입력되어 있는지 검사한다.
        //리턴 false : 입력값 오류,  true : 입력값 정상
        private int _intNullPosition = 0;
        private bool TextBoxNullCheck()
        {
            _intNullPosition = 0;
            string tempstr = this.utxtNumber.Text;
            //if (this.utxtNumber.Text == "" || this.utxtPassward.Text == "") return false;
            if (this.utxtNumber.Text == "" && this.utxtPassword.Text != "")
            {
                _intNullPosition = 1;
                return false;
            }
            else if (this.utxtNumber.Text != "" && this.utxtPassword.Text == "")
            {
                _intNullPosition = 2;
                return false;
            }
            else if (this.utxtNumber.Text == "" && this.utxtPassword.Text == "")
            {
                _intNullPosition = 3;
                return false;
            }
            else return true;
        }

        private void Display_Cursor_Point()
        {
            if (_intNullPosition == 1) utxtNumber.Select();
            else if (_intNullPosition == 2) utxtPassword.Select();
            else if (_intNullPosition == 3) utxtNumber.Select();
        }

        private void utxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 46)// && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
            {
                e.Handled = true;
            }
        }

        private void utxtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (e.KeyChar == '-')// && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
            {
                e.Handled = true;
            }
        }
    }
}
