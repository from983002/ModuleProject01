using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infragistics.Win;
using Infragistics.Win.UltraMessageBox;

namespace SystemAlign
{
    class CNIAMMessageBoxView
    {
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
    }
}
