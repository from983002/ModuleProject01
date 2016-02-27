using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.Win;
using Infragistics.Win.UltraActivityIndicator;
using Infragistics.Win.UltraWinGrid;

namespace SystemAlign
{
    class Control_UltraGrid
    {
        /*
        private void Run_Ready_Recipe_GridComputeSize()
        {
            int iGridRowQt = m_clRecipe.GridEnableXy[0];

            iPosIndY = new int[iGridRowQt];

            uDS_Recipe1.Rows.Clear();
            for (int i = 0; i < iGridRowQt; i++)
            {
                uDS_Recipe1.Rows.Add(true, new Object[] { "None", "None" });
            }

            uDS_Recipe2.Rows.Clear();
            for (int i = 0; i < iGridRowQt; i++)
            {
                uDS_Recipe2.Rows.Add(true, new Object[] { "None", "None" });
            }

            int RowHeight = (658 - 26) / (int.Parse(m_sGridAxisY));
            int HeightAther = (658 - 26) % (int.Parse(m_sGridAxisY));

            Size newSizeGrid = uGrd_Recipe1.Size;
            Size newSizePanel = uPnl_StatusPanel1.Size;

            if (HeightAther == 0)
            {
                newSizeGrid.Height = 660;
                newSizePanel.Height = 664;
            }
            else
            {
                newSizeGrid.Height = 658;
                newSizePanel.Height = 662;
            }

            uGrd_Recipe1.Size = newSizeGrid;
            uGrd_Recipe2.Size = newSizeGrid;
            uPnl_StatusPanel1.Size = newSizePanel;
            uPnl_StatusPanel2.Size = newSizePanel;

            uGrd_Recipe1.DisplayLayout.Override.DefaultRowHeight = RowHeight + 1;
            uGrd_Recipe2.DisplayLayout.Override.DefaultRowHeight = RowHeight + 1;

            szIndiSize.Height = RowHeight - 1;

            uGrd_Recipe1.Refresh();
            uGrd_Recipe2.Refresh();
        }

        private void Run_Ready_Recipe_Cell_ColorSet(int gridNo, int rowNo, int colNo, int colorNo)
        {
            UltraGrid colorGrid = null;

            if (gridNo == 1) colorGrid = uGrd_Recipe1;
            else colorGrid = uGrd_Recipe2;

            if (colorNo == 1)
            {
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor = Color.Gold; // Orange;Chartreuse
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor2 = Color.Yellow; //Gold;LimeGreen
            }
            else if (colorNo == 2)
            {
                if (WorkStatusFlagArray[m_iNowWorkPoint - 1].Complite == false)
                {
                    colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor = Color.Chartreuse;
                    colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor2 = Color.LawnGreen;
                }

            }
            else if (colorNo == 3)
            {
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor = Color.Orange;
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor2 = Color.DarkOrange;
            }
            else if (colorNo == 4)
            {
                if (m_iNowWorkPoint > 0) WorkStatusFlagArray[m_iNowWorkPoint - 1].Complite = true;
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor = Color.Red;
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor2 = Color.Firebrick;

            }
            else if (colorNo == 0)
            {
                if (WorkStatusFlagArray[m_iNowWorkPoint - 1].Complite == false)
                {
                    colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor = Color.Transparent;
                    colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackColor2 = Color.Transparent;
                }
            }



            if (m_fLamping)
            {
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackGradientAlignment =
                    GradientAlignment.Element;
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackGradientStyle =
                    GradientStyle.VerticalBump;
            }
            else
            {
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackGradientAlignment =
                    GradientAlignment.Client;
                colorGrid.DisplayLayout.Rows[rowNo].Cells[colNo].Appearance.BackGradientStyle = GradientStyle.Vertical;
            }
            uPnl_StatusPanel1.ClientArea.ResumeLayout(false);
            uPnl_StatusPanel1.ResumeLayout(false);
            uPnl_StatusPanel2.ClientArea.ResumeLayout(false);
            uPnl_StatusPanel2.ResumeLayout(false);
            ResumeLayout(false);
            Refresh();
        }


        private void Run_Ready_Recipe_GridSet()
        {
            BandsCollection uGrdBands1;
            uGrdBands1 = uGrd_Recipe1.DisplayLayout.Bands;
            uGrdBands1[0].Columns["Left"].Width = 98;
            uGrdBands1[0].Columns["Right"].Width = 98;

            uGrdBands1[0].Columns["Left"].Header.Caption = "LEFT";
            uGrdBands1[0].Columns["Right"].Header.Caption = "RIGHT";

            BandsCollection uGrdBands2;
            uGrdBands2 = uGrd_Recipe2.DisplayLayout.Bands;
            uGrdBands2[0].Columns["Left"].Width = 98;
            uGrdBands2[0].Columns["Right"].Width = 98;

            uGrdBands2[0].Columns["Left"].Header.Caption = "LEFT";
            uGrdBands2[0].Columns["Right"].Header.Caption = "RIGHT";

            uDS_ChartGrid1.Rows.Clear();
            uDS_ChartGrid2.Rows.Clear();
        }


        private void Run_Ready_Recipe_Title_Make()
        {
            rKey = Registry.CurrentUser.OpenSubKey(@"ShinJinMTec\Visteon IK");

            m_sSideMode = rKey.GetValue("UsedSideMode").ToString();
            m_sSideStartNo = rKey.GetValue("UsedSideStartNo").ToString();
            m_sToggleMode = rKey.GetValue("UsedToggleMode").ToString();
            m_sToggleStartNo = rKey.GetValue("UsedToggleStartNo").ToString();
            rKey.Close();

            BandsCollection uGrdBands1;
            uGrdBands1 = uGrd_Recipe1.DisplayLayout.Bands;
            uGrdBands1[0].Columns["Left"].Header.Caption = "LEFT";
            uGrdBands1[0].Columns["Right"].Header.Caption = "RIGHT";


            BandsCollection uGrdBands2;
            uGrdBands2 = uGrd_Recipe2.DisplayLayout.Bands;
            uGrdBands2[0].Columns["Left"].Header.Caption = "LEFT";
            uGrdBands2[0].Columns["Right"].Header.Caption = "RIGHT";

            if (m_sSideMode == "True")
            {
                uLbl_StatusTitleBar1.Text = "L E F T";
                uLbl_StatusTitleBar2.Text = "R I G H T";
            }
            else if (m_sToggleMode == "True")
            {
                uLbl_StatusTitleBar1.Text = "N O R M A L";
                uLbl_StatusTitleBar2.Text = "R E V E R S E";
            }

        }

        private void uGrd_Recipe1_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            if (WorkStatusFlagArray == null || uGrd_Recipe1.DisplayLayout.Rows.Count < 1) return;
            if (!(e.Element is CellUIElement))
            { return; }
            UltraGridCell cell = e.Element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
            if (cell != null)
            {
                string sCellData = cell.Text;
                string sDisplayStr = "";
                for (int j = 0; j < m_clRecipe.WeldingPointQt; j++)
                {
                    if (sCellData == m_clRecipe.WeldingPointList[j].sSequence)
                    {
                        if (WorkStatusFlagArray[j].Complite == true)
                        {
                            sDisplayStr += "Capture : ";
                            sDisplayStr += uGrid_Recipe_Popup_String(WorkStatusFlagArray[j].CamData) + "\r\n";
                            sDisplayStr += "Welding : ";
                            sDisplayStr += uGrid_Recipe_Popup_String(WorkStatusFlagArray[j].WelData) + "\r\n";
                            sDisplayStr += "Datalog : ";
                            sDisplayStr += uGrid_Recipe_Popup_String(WorkStatusFlagArray[j].SaveData) + "\r\n";

                            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo tipInfo = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo(sDisplayStr, Infragistics.Win.ToolTipImage.Default, "", Infragistics.Win.DefaultableBoolean.True);
                            tipInfo.Appearance.BackColor = Color.FromArgb(((((224)))), ((((224)))), ((((224)))));
                            tipInfo.Appearance.BackColor2 = Color.DarkGray;

                            uTT_FailPop1.SetUltraToolTip(uGrd_Recipe1, tipInfo);
                            uTT_FailPop1.ShowToolTip(uGrd_Recipe1);
                        }
                        return;
                    }
                }
            }
            else
            { uTT_FailPop1.HideToolTip(); }
        }
        */
    }

    //사용방법
    // color_Indicator.DrawFilter = new UltraActivityIndicatorTextDrawFilter(iCellText.ToString(), m_Font);
    public class UltraActivityIndicatorTextDrawFilter : IUIElementDrawFilter
    {
        #region Members

        private readonly Font font;
        private readonly string text = string.Empty;
        private SizeF textSize;

        #endregion //Members

        #region Constructor

        public UltraActivityIndicatorTextDrawFilter(string text, Font font)
        {
            this.text = text;
            this.font = font;
        }

        #endregion //Constructor

        #region IUIElementDrawFilter Members

        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            var marqueeElement = drawParams.Element as MarqueeIndicatorUIElement;
            if (marqueeElement != null &&
                text != String.Empty)
            {
                if (textSize.IsEmpty)
                    textSize = drawParams.Graphics.MeasureString(text, font);
                Rectangle workRect = marqueeElement.RectInsideBorders;
                drawParams.Graphics.DrawString(text, font, drawParams.TextBrush,
                                               new PointF(workRect.Right - (workRect.Width + textSize.Width) / 2,
                                                          workRect.Bottom - (workRect.Height + textSize.Height) / 2));
            }
            return false;
        }

        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            if (drawParams.Element is MarqueeIndicatorUIElement)
                return DrawPhase.AfterDrawElement;
            else
                return DrawPhase.None;
        }

        #endregion
    }
}
