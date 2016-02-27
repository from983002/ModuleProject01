using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraMessageBox;

namespace SystemAlign
{
    public class Control_UltraMessageBox
    {
        private Infragistics.Win.UltraMessageBox.UltraMessageBoxInfo _messageinfo;
        //메세지 박스 제어에 사용되는 리소스 제어 객체
        private ResourceManager rm = SystemAlign.Properties.Resources.ResourceManager;

        private Infragistics.Win.UltraMessageBox.UltraMessageBoxManager ultraMessageBoxManager1;

        
        string msg1 = string.Empty;
        string msg2 = string.Empty;
        string msg3 = string.Empty;
        MessageBoxButtons btn = new MessageBoxButtons();
        MessageBoxIcon icon = new MessageBoxIcon();

        public DialogResult DlgMain_Manu_Message(int manuNo)
        {
            if (manuNo == 1)
            {
                msg1 = "시스템 종료";
                msg2 = "시스템 종료 확인!";
                msg3 = "시스템을 종료 하시겠습니까?";
                btn = MessageBoxButtons.OKCancel;
                icon = MessageBoxIcon.Stop;
            }
            else if (manuNo == 2)
            {
                msg1 = "시스템 정보 2015.07.25 ver3.0301";
                msg2 = "시스템 정보 확인!";
                msg3 = "본 시스템에 대한 문의는 아래의 홈페이지를 참고하여 주십시요 !";
                btn = MessageBoxButtons.OK;
                icon = MessageBoxIcon.Information;
            }
            return MessageBox_Show(msg1, msg2, msg3, btn, icon);
        }

        public DialogResult SystemConnectStatus_UMAC(string msg)
        {
            string msg1 = "시스템 통신";
            string msg2 = "제어 시스템 연결 오류!";
            string msg3 = "제어 시스템 " + msg + "과 연결이 되지 않아서 종료합니다.\r\n관리자에게 문의 하여 주십시요!";
            MessageBoxButtons btn = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Stop;

            return MessageBox_Show(msg1, msg2, msg3, btn, icon);
        }

        public DialogResult SystemConnectStatus(List<string> msg, bool[] status)
        {
            string strmsg = string.Empty;
            int falseCount = 0;
            for(int i = 0; i < status.Count(); i++)
            {
                if (status[i] == false)
                {
                    if (falseCount != 0) strmsg += ", " + msg[i];
                    else strmsg += msg[i];
                    falseCount++;
                }
            }
            
            string msg1 = "시스템 통신";
            string msg2 = "제어 시스템 연결 오류 !";
            string msg3 = strmsg + " 시스템과 연결이 되지 않아서 시작을 할 수 없습니다.\r\n관리자에게 문의 하여 주십시요!";
            MessageBoxButtons btn = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Stop;

            return MessageBox_Show(msg1, msg2, msg3, btn, icon);
        }

        public UltraMessageBoxInfo GetSetMessageBoxInfo
        {
            get { return _messageinfo; }
            set { _messageinfo = value; }
        }

        public Control_UltraMessageBox()
        {
            ultraMessageBoxManager1 = new Infragistics.Win.UltraMessageBox.UltraMessageBoxManager();
            _messageinfo = new UltraMessageBoxInfo();
            MessageBox_Initionalize();
        }

        //울투라 메세지 박스 설정부 : ultraMessageBox_Show(캡션, 헤더, 메세지) 함수로 호출
        private void MessageBox_Initionalize()
        {
            //메세지 박스
            _messageinfo.Appearance.BackColor = Control.DefaultBackColor;
            _messageinfo.Appearance.FontData.SizeInPoints = 8;
            _messageinfo.Appearance.FontData.Name = "MS Reference Sans Serif";
            //메세지 박스 헤더
            _messageinfo.HeaderAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))),
                ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            _messageinfo.HeaderAppearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(117)))),
                ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            _messageinfo.HeaderAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            _messageinfo.HeaderAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))),
                ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            _messageinfo.HeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            _messageinfo.HeaderAppearance.ForeColor = System.Drawing.Color.White;
            _messageinfo.HeaderAppearance.FontData.SizeInPoints = 8;
            //메세지 박스 본문
            _messageinfo.ContentAreaAppearance.BackColor = Control.DefaultBackColor;
            _messageinfo.ContentAreaAppearance.ImageVAlign = VAlign.Middle;
            _messageinfo.ContentAreaAppearance.ImageHAlign = HAlign.Left;
            //메세지 박스 버튼 지역
            _messageinfo.ButtonAreaAppearance.BackColor = Control.DefaultBackColor;
            //메세지 박스 버튼
            _messageinfo.ButtonAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))),
                ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            _messageinfo.ButtonAppearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(117)))),
                ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            _messageinfo.ButtonAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            _messageinfo.ButtonAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))),
                ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            _messageinfo.ButtonAppearance.FontData.Name = "MS Reference Sans Serif";
            //메세지 박스 푸터
            _messageinfo.FooterAppearance.BackColor = System.Drawing.Color.LightSlateGray;
            _messageinfo.FooterAppearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))),
                ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            _messageinfo.FooterAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            _messageinfo.FooterAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))),
                ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            //messageinfo.FooterAppearance.FontData.Name = "MS Reference Sans Serif";
            _messageinfo.FooterFormatted = rm.GetString("FooterText");
            _messageinfo.FooterAppearance.Image = SystemAlign.Properties.Resources.info;
            _messageinfo.FooterAppearance.ImageVAlign = VAlign.Middle;
            _messageinfo.FooterAppearance.ImageHAlign = HAlign.Left;
        }

        public DialogResult MessageBox_Show(string strCaption, string strHeader, string strContents, MessageBoxButtons btnButtons, MessageBoxIcon msgIcon)
        {
            _messageinfo.Icon = msgIcon;
            _messageinfo.Buttons = btnButtons;
            _messageinfo.Caption = strCaption;
            _messageinfo.Header = strHeader;
            _messageinfo.TextFormatted = strContents;

            return ultraMessageBoxManager1.ShowMessageBox(_messageinfo);
        }

       
        //기존의 시스템에서 사용하던 메세지박스 클래스
        /*
        private Infragistics.Win.UltraMessageBox.UltraMessageBoxManager uMesManager;
        private Infragistics.Win.UltraWinForm.UltraFormManager ufmDlgMain;

        public CNIAMMessageBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {            
            this.uMesManager = new Infragistics.Win.UltraMessageBox.UltraMessageBoxManager();

            this.uMesManager.HeaderAppearance.BackColor = System.Drawing.Color.DimGray;
            this.uMesManager.HeaderAppearance.BackColor2 = System.Drawing.Color.DarkGray;
            this.uMesManager.HeaderAppearance.BackGradientAlignment = Infragistics.Win.GradientAlignment.Container;
            this.uMesManager.HeaderAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uMesManager.HeaderAppearance.FontData.BoldAsString = "True";
            this.uMesManager.HeaderAppearance.FontData.SizeInPoints = 9F;
            this.uMesManager.HeaderAppearance.ForeColor = System.Drawing.Color.White;
            this.uMesManager.HeaderAppearance.BorderColor = System.Drawing.Color.White;

            this.uMesManager.ContentAreaAppearance.BackColor = System.Drawing.Color.DimGray;
            this.uMesManager.ContentAreaAppearance.BackColor2 = System.Drawing.Color.DarkGray;
            //this.uMesManager.ContentAreaAppearance.BackGradientAlignment = Infragistics.Win.GradientAlignment.Container;
            this.uMesManager.ContentAreaAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uMesManager.ContentAreaAppearance.FontData.BoldAsString = "True";
            this.uMesManager.ContentAreaAppearance.ForeColor = System.Drawing.Color.White;
            this.uMesManager.ContentAreaAppearance.BorderColor = System.Drawing.Color.White;

            this.uMesManager.ButtonAppearance.BackColor = System.Drawing.Color.DimGray;
            this.uMesManager.ButtonAppearance.BackColor2 = System.Drawing.Color.DarkGray;
            this.uMesManager.ButtonAppearance.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.uMesManager.ButtonAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uMesManager.ButtonAppearance.FontData.BoldAsString = "True";
            this.uMesManager.ButtonAppearance.FontData.SizeInPoints = 9F;
            this.uMesManager.ButtonAppearance.ForeColor = System.Drawing.Color.White;

            this.uMesManager.ButtonAreaAppearance.BackColor = System.Drawing.Color.DimGray;
            //this.uMesManager.ButtonAreaAppearance.BackColor2 = System.Drawing.Color.DarkGray;
            //this.uMesManager.ButtonAreaAppearance.BackGradientAlignment = Infragistics.Win.GradientAlignment.Container;
            //this.uMesManager.ButtonAreaAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uMesManager.ButtonAreaAppearance.BorderColor = System.Drawing.Color.White;

            this.uMesManager.FooterAppearance.BackColor = System.Drawing.Color.DimGray;
            this.uMesManager.FooterAppearance.BackColor2 = System.Drawing.Color.DarkGray;
            this.uMesManager.FooterAppearance.BackGradientAlignment = Infragistics.Win.GradientAlignment.Container;
            this.uMesManager.FooterAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uMesManager.FooterAppearance.FontData.BoldAsString = "True";
            this.uMesManager.FooterAppearance.FontData.SizeInPoints = 9F;
            this.uMesManager.FooterAppearance.ForeColor = System.Drawing.Color.White;

            this.uMesManager.UseAppStyling = false;
            this.uMesManager.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.uMesManager.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

        }

        public void ShowMessage(string captionStr, string headerStr, string textStr)
        {
            using (UltraMessageBoxInfo ultraMessageBoxInfo = new UltraMessageBoxInfo())
            {
                ultraMessageBoxInfo.EnableSounds = Infragistics.Win.DefaultableBoolean.False;
                ultraMessageBoxInfo.HeaderAppearance.FontData.SizeInPoints = 9;

                ultraMessageBoxInfo.Caption = captionStr;
                ultraMessageBoxInfo.Header = headerStr;
                ultraMessageBoxInfo.Text = textStr;
                uMesManager.ShowMessageBox(ultraMessageBoxInfo);
            }
        }

        public void ShowMessage(string captionStr, string headerStr, string textStr, string footerStr)
        {
            using (UltraMessageBoxInfo ultraMessageBoxInfo = new UltraMessageBoxInfo())
            {
                ultraMessageBoxInfo.EnableSounds = Infragistics.Win.DefaultableBoolean.False;
                ultraMessageBoxInfo.HeaderAppearance.FontData.SizeInPoints = 9;

                ultraMessageBoxInfo.Caption = captionStr;
                ultraMessageBoxInfo.Header = headerStr;
                ultraMessageBoxInfo.Text = textStr;
                ultraMessageBoxInfo.Footer = footerStr;

                this.uMesManager.ShowMessageBox(ultraMessageBoxInfo);
            }
        }
        */
    }
}
