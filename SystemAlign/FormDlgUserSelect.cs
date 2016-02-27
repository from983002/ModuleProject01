using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SystemAlign
{
    public partial class FormDlgUserSelect : Form
    {
        public event UserSelectEvent1 UserSelecting;

        private string _selectedAccount = null;

        private Control_UltraMessageBox messageBox;

        public FormDlgUserSelect()
        {
            InitializeComponent();
        }

        private void FormDlgUserSelect_Load(object sender, EventArgs e)
        {
            Trace.WriteLine("userselect - " + this.Location.X.ToString("000") + "   " + this.Location.Y.ToString("000"));
            Point startPoint = new Point(411, 232);
            Location = startPoint;

            messageBox = new Control_UltraMessageBox();
        }

        public string SelectedAccount
        {
            get { return _selectedAccount; }
            set { _selectedAccount = value; }
        }
        /*
        [DllImport("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]

        public static extern bool ReleaseCapture();

        public readonly int WM_NLBUTTONDOWN = 0xA1;
        public readonly int HT_CAPTION = 0x2;
        protected override void OnMouseDown(MouseEventArgs e)
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
        */

        static readonly string Account_Operator = "OPERATOR";
        static readonly string Account_Engineer = "ENGINEER";
        static readonly string Account_Maker = "MAKER";

        //static readonly string Account_Operator = "OP";
        //static readonly string Account_Engineer = "ENG";
        //static readonly string Account_Maker = "MK";
        
        static readonly string Account_Password = "PASSWORD EDIT";
        private void Select_uBtn_Operator_Click(object sender, EventArgs e)
        {
            //_selectedAccount = Select_uBtn_Operator.Text;
            SelectedAccount = Account_Operator;
            UserSelecting(Account_Operator);
        }

        private void Select_uBtn_Engineer_Click(object sender, EventArgs e)
        {
            //_selectedAccount = Select_uBtn_Engineer.Text;
            SelectedAccount = Account_Engineer;
            UserSelecting(Account_Engineer);
        }

        private void Select_uBtn_Maker_Click(object sender, EventArgs e)
        {
            //_selectedAccount = Select_uBtn_Maker.Text;
            SelectedAccount = Account_Maker;
            UserSelecting(Account_Maker);
        }

        private void Select_uBtn_Edit_Click(object sender, EventArgs e)
        {
            //_selectedAccount = Select_uBtn_Edit.Text;
            SelectedAccount = Account_Password;
            UserSelecting(Account_Password);
        }

       
    }
}
