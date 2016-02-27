using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Infragistics.Win;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SystemAlign
{
    public partial class FormDlgLogin : Form
    {
        public event LoginCompliteEvent1 LoginComplite;

        //private FormDlgKeyboard _dlgKeyboard;
        
        public FormDlgLogin()
        {
            InitializeComponent();
        }

        private void eventing_VKeyboardEntered()
        {
            ubtnLogin.PerformClick();
        }

        private Process ps;// = new Process();

        private void FormDlgLogin_Load(object sender, EventArgs e)
        {
            Point formStartPoint = new Point(465, 232);
            this.Location = formStartPoint;
            //Trace.WriteLine("Logon - " + this.Location.X.ToString("000") + "   " + this.Location.Y.ToString("000"));

            //utxtNumber.Text = string.Empty;
            //utxtPassward.Text = string.Empty;
            //utxtNumber.Select();
            InitializeComponentAddOn();

            ps = new System.Diagnostics.Process();
            ps.StartInfo.FileName = "osk.exe";
            ps.Start();

            ubtnLogin.Focus();
        }


        //클래스 생성자를 호출 시 마지막 작업
        //멤버변수등을 초기화 한다.
        private void InitializeComponentAddOn()
        {
            //utxtNumber.Text = string.Empty;
            //utxtPassward.Text = string.Empty;
            //utxtNumber.Select();
            string tmpPass = GetReg("Software\\ShinJin M Tec\\LNS System\\System Lamination", "Eid");
            utxtNumber.Text = tmpPass;
            utxtPassward.Text = string.Empty;
            utxtPassward.Select();

#if(SYST_SIMUL)
            utxtPassward.Text = "lg";
#else
            
#endif
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

        public string GetReg(string strNodePath, string regName)
        {
            RegistryKey reg = Registry.CurrentUser;

            reg = reg.OpenSubKey(strNodePath, true);
            string regData = reg.GetValue(regName, "").ToString();
            reg.Close();
            return regData;
        }

        //로그인 버튼 선택시 : 입력데이터 확인 후
        //재입력 받거나 계속 진행한다.
        private void ubtnLogin_Click(object sender, EventArgs e)
        {
            Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
            if (this.TextBoxNullCheck() == false)
            {
                if (messageBox.MessageBox_Show("로그인 오류", "아이디 또는 패스워드 입력필요!.", "아이디 또는 패스워드를 입력하지 않았습니다.<br/><br/>로그온에 필요한 아이디와 패스워드를 입력하세요!", MessageBoxButtons.OK,MessageBoxIcon.Error) == DialogResult.OK) return;
            }

            GetSetInputID = utxtNumber.Text;
            GetSetInputPass = utxtPassward.Text;
            //bool inputDataCheck = LoginComplite(utxtNumber.Text, utxtPassward.Text);
            if (LoginComplite(utxtNumber.Text, utxtPassward.Text) == true)
            {
               this.DialogResult = DialogResult.OK;
            }
            else
            {
                if (messageBox.MessageBox_Show("로그인 오류", "아이디 또는 패스워드 오류!.", "아이디 또는 패스워드가 정확하지 않습니다.<br/><br/>확인 후 아이디와 패스워드를 다시 입력하세요!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) utxtNumber.Select();
            }
        }

        

        //취소 버튼 선택시
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

        private void Display_Cursor_Point()
        {
            if (_intNullPosition == 1) utxtNumber.Select();
            else if (_intNullPosition == 2) utxtPassward.Select();
            else if (_intNullPosition == 3) utxtNumber.Select();
        }

        [DllImport("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]

        public static extern bool ReleaseCapture();

        public readonly int WM_NLBUTTONDOWN = 0xA1;
        public readonly int HT_CAPTION = 0x2;

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

        private void FormDlgLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void FormDlgLogin_Paint(object sender, PaintEventArgs e)
        {
            ubtnLogin.Focus();
        }

        private void FormDlgLogin_Shown(object sender, EventArgs e)
        {
            ubtnLogin.Focus();
        }



        /*
        //로그오프 버튼 선택시 : 입력데이터 확인 후
        //재입력 받거나 계속 진행한다.
        private void ubtnLogoff_Click(object sender, EventArgs e)
        {
            if (this.TextBoxNullCheck() == false)
            {
                UltraMessageboxShow("로그인 오류", "저~~ 작업자님 여기서 이러시면 안됩니다.", "언능 사번과 비밀번호를 똑바로 입력하세요!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.loginCompilete(DialogResult.No);
        }

        //각 버튼으로 부터 클릭되었을 때 현재 입력받은 값을
        //전역 변수에 저장하고 대화상자 종료 이벤트 발생한다.
        private void loginCompilete(DialogResult dlgResult)
        {
            //this.resultData[0] = this.utxtNumber.Text;
            //this.resultData[1] = this.utxtPassward.Text;
            bool m_bLogResult = GetReg(this.utxtNumber.Text, this.utxtPassward.Text);
            
            if (m_bLogResult == true)
            {
                this.DialogResult = dlgResult;
            }
            else
            {
                this.utxtNumber.Text ="";
                this.utxtPassward.Text = "";
            }
        }

        public void setReg(string idData, string pwData)
        {
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Nexstar Technology\\NSIARMan\\NSIAR_CONFIG_ETC", RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue("ETC_ADMIN_ID", idData, RegistryValueKind.String);
            reg.SetValue("ETC_ADMIN_PW", pwData, RegistryValueKind.String);
            reg.Close();
        }

        public bool GetReg(string idData, string pwData)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey("Software\\Nexstar Technology\\NSIARMan\\NSIAR_CONFIG_ETC", true);
            if (reg == null)
            {
                reg.Close();
                setReg(this.utxtNumber.Text, this.utxtPassward.Text);
                return true;
            }
            else
            {
                string idStr = reg.GetValue("ETC_ADMIN_ID","").ToString();
                string pwStr = reg.GetValue("ETC_ADMIN_PW", "").ToString();

                if ((idData == idStr) && (pwData == pwStr))
                {
                    return true;
                }
                else
                    UltraMessageboxShow("로그인", "로그인 오류", "사원번호와 비밀번호를 확인 해 주십시요.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
            return false;
        }
        */
    }
}
