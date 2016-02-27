using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SystemAlign
{
    public partial class FormDlgPassEdit : Form
    {
        public event PasswordEditEvent1 PasswordEditing;

        //private FormDlgKeyboard _dlgKeyboard;

        public FormDlgPassEdit()
        {
            InitializeComponent();

        }

        private void FormDlgPassEdit_Load(object sender, EventArgs e)
        {
            var formStartPoint = new Point(465, 232);
            this.Location = formStartPoint;
            //Trace.WriteLine("PassEdit - " + this.Location.X.ToString("000") + "   " + this.Location.Y.ToString("000"));

            utxtNumber.Text = null;
            utxtPassward.Text = null;
            utxtNumber.Select();

            //_dlgKeyboard = new FormDlgKeyboard();
            //_dlgKeyboard.VKeyboardEntered += new VertualKeyboardEvent1(eventing_VKeyboardEntered);
            //_dlgKeyboard.Show();
        }

        private void eventing_VKeyboardEntered()
        {
            ubtnLogin.PerformClick();
        }

        //사용자 입력 데이터를 저장하는 변수
        private string _strInputID = null;
        private string _strInputPass = null;

        //다른 클래스에서 위의 결과 데이터를 저장하거나 리턴하는 함수
        public string GetSetInputID
        {
            get { return this._strInputID; }
            set { this._strInputID = value; }
        }


        //다른 클래스에서 위의 결과 데이터를 저장하거나 리턴하는 함수
        public string GetSetInputPass
        {
            get { return this._strInputPass; }
            set { this._strInputPass = value; }
        }


        //사번과 비밀번호에 데이터가 입력되어 있는지 검사한다.
        //리턴 false : 입력값 오류,  true : 입력값 정상
        //입력값 오류일 때 커서의 위치를 알기위해서 널포인트를 설정해준다.
        //아이디 일때 1, 패스워드 일때 2, 둘다 일때 3

        private int _intNullPosition = 0;
        private bool TextBoxNullCheck()
        {
            _intNullPosition = 0;
            string tempstr = this.utxtNumber.Text;
            //if (this.utxtNumber.Text == "" || this.utxtPassward.Text == "") return false;
            if (this.utxtNumber.Text == "" && this.utxtPassward.Text != "")
            {
                _intNullPosition = 1;
                return false;
            }
            else if (this.utxtNumber.Text != "" && this.utxtPassward.Text == "")
            {
                 _intNullPosition = 2;
                return false;
            }
            else if (this.utxtNumber.Text == "" && this.utxtPassward.Text == "")
            {
                _intNullPosition = 3;
                return false;
            }
            else return true;
        }

        [DllImport("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]

        public static extern bool ReleaseCapture();

        public readonly int WM_NLBUTTONDOWN = 0xA1;
        public readonly int HT_CAPTION = 0x2;

        private void Display_Cursor_Point()
        {
            if (_intNullPosition == 1) utxtNumber.Select();
            else if (_intNullPosition == 2) utxtPassward.Select();
            else if (_intNullPosition == 3) utxtNumber.Select(); 
        }

        private void ubtnLogin_Click(object sender, EventArgs e)
        {
            var messageBox = new Control_UltraMessageBox();
            if (this.TextBoxNullCheck() == false)
            {
                //_dlgKeyboard.TopMost = false;
                //_dlgKeyboard.Refresh();
                
                if (messageBox.MessageBox_Show("로그인 오류", "아이디 또는 패스워드 입력필요!.",
                    "아이디 또는 패스워드를 입력하지 않았습니다.<br/><br/>로그온에 필요한 아이디와 패스워드를 입력하세요!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error) == DialogResult.OK)
                    //_dlgKeyboard.TopMost = true;
                //_dlgKeyboard.Refresh();
                Display_Cursor_Point();
                return;
            }

            //_dlgKeyboard.TopMost = false;
            if (messageBox.MessageBox_Show("로그인 정보", "아이디, 패스워드 변경!.",
                    "아이디와 패스워드를 정말로 변경하시겠습니까? <br/><br/>아이디와 패스워드의 분실 및 노출되지 않도록 주의해 주십시요!", MessageBoxButtons.OK,
                    MessageBoxIcon.Question) != DialogResult.OK) return;
            //_dlgKeyboard.TopMost = true;
            GetSetInputID = utxtNumber.Text;
            GetSetInputPass = utxtPassward.Text;

            if (PasswordEditing(utxtNumber.Text, utxtPassward.Text) == true)
                this.DialogResult = DialogResult.OK;
        }

        private void upnlBack_MouseDownClient(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 다른 컨트롤에 묶여있을 수 있을 수 있으므로 마우스캡쳐 해제
                ReleaseCapture();
                // 타이틀 바의 다운 이벤트처럼 보냄
                SendMessage(this.Handle, WM_NLBUTTONDOWN, HT_CAPTION, 0);
            }
            base.OnMouseDown(e);
        }

        private void FormDlgPassEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (_dlgKeyboard.Enabled == true)
            //{
            //    _dlgKeyboard.Dispose();
            //}
        }

        private void ubtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
