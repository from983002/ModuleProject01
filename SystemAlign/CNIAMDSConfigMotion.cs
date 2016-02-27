using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;

using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;


using Excel = Microsoft.Office.Interop.Excel;

namespace SystemAlign
{
    class CNIAMDSConfigMotion
    {
        int m_lAxisNo;
        public void dataUpdate()
        {
            uint duState1 = 0, duState2 = 0, duRetCode;

            //++ Z상 신호의 상태를 확인합니다.
            duRetCode = CAXM.AxmStatusReadMechanical(m_lAxisNo, ref duState1);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                duState1 = ((duState1 & (uint)AXT_MOTION_QIMECHANICAL_SIGNAL.QIMECHANICAL_ZPHASE_LEVEL) == 0) ? (uint)0 : (uint)1;
                //chkZPhase.Checked = Convert.ToBoolean(duState1);
            }
            //++ Servo Alarm 신호의 상태를 확인합니다.
            duRetCode = CAXM.AxmSignalReadServoAlarm(m_lAxisNo, ref duState1);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //chkAlarm.Checked = Convert.ToBoolean(duState1);
            }
            //++ Inposition(위치결정완료) 신호의 상태를 확인합니다.
            duRetCode = CAXM.AxmSignalReadInpos(m_lAxisNo, ref duState1);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //chkInp.Checked = Convert.ToBoolean(duState1);
            }
            //++ Emergency 신호의 상태를 확인합니다.
            duRetCode = CAXM.AxmSignalReadStop(m_lAxisNo, ref duState1);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //chkStopSig.Checked = Convert.ToBoolean(duState1);
            }
            //++ (+/-)End Limit신호의 상태를 확인합니다.
            duRetCode = CAXM.AxmSignalReadLimit(m_lAxisNo, ref duState1, ref duState2);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //chkElimitP.Checked = Convert.ToBoolean(duState1);
                //chkElimitN.Checked = Convert.ToBoolean(duState2);
            }
        }

        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public static int WM_QUIT = 0x12;
        public Excel.Application xls;
        public Excel.Worksheet xlsSheet;
        public Excel.Workbook xlsBook;
        private object opt = Missing.Value;

        private Infragistics.Win.UltraWinDataSource.UltraDataSource uDSConfigMotion;

        public CNIAMDSConfigMotion()
        {
            this.uDSConfigMotion = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
        }

        public UltraDataSource ConfigMotion
        {
            get { return this.uDSConfigMotion; }
        }

        public void ExcelFileOpen(string filePath)
        {
            string fileNamePath = filePath;

            xls = new Excel.Application();
            xls.Visible = false;
            xlsBook = (Excel.Workbook)xls.Workbooks.Open(fileNamePath, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt);
            xlsSheet = (Excel.Worksheet)xlsBook.ActiveSheet;
            Debug.Write("ExcelControl 종료 : " + System.DateTime.Now.ToString() + System.Environment.NewLine);
            ReadMotionSet_SaveFile(xlsSheet);
            ExclFileClose();
        }

        private void ExclFileClose()
        {
            if (xlsSheet != null) Marshal.ReleaseComObject(xlsSheet);
            if (xlsBook != null) xlsBook.Close(false, System.Type.Missing, System.Type.Missing);

            xls.UserControl = false;
            IntPtr hwnd = new IntPtr(xls.Hwnd);
            xls.Quit();
            Marshal.ReleaseComObject(xls);
            PostMessage(hwnd, WM_QUIT, 0, 0);
        }


        private void ReadMotionSet_SaveFile(Excel.Worksheet xlsSheet)
        {
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("AxName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("LimitPos");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("LimitNeg");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PosOrigin");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("OnOff");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PosStart");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Error");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Position");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Speed");
            ultraDataColumn1.DataType = typeof(uint);
            this.uDSConfigMotion.Band.Columns.AddRange(new object[] {
            ultraDataColumn1, ultraDataColumn2, ultraDataColumn3,
            ultraDataColumn4, ultraDataColumn5, ultraDataColumn6,
            ultraDataColumn7, ultraDataColumn8, ultraDataColumn9, ultraDataColumn10});

            try
            {
                this.uDSConfigMotion.Rows.Clear();
                string[] rowReadData = new string[9];
                UltraDataRow rowItem = null;
                int IONameReadCount = 1;
                int ColumnCount = 1;
                for (int row = 2; row < 100; row++) //
                {
                    Debug.Write("Row " + row.ToString() + " 입니다." + System.Environment.NewLine);
                    if (((Excel.Range)xlsSheet.Cells[row, 1]).Value2 == null) // null 이면 종료
                        break;

                    this.uDSConfigMotion.Rows.Add();
                    rowItem = this.uDSConfigMotion.Rows[row - 2];

                    for (int col = 1; col < 10; col++)
                    {
                        rowReadData[col - 1] = ((Excel.Range)xlsSheet.Cells[row, col]).Value2.ToString();//.Trim();
                    }

                    rowItem["No"] = this.uDSConfigMotion.Rows.Count;
                    rowItem["AxName"] = rowReadData[0];
                    rowItem["LimitPos"] = rowReadData[1];
                    rowItem["LimitNeg"] = rowReadData[2];
                    rowItem["PosOrigin"] = rowReadData[3];
                    rowItem["OnOff"] = rowReadData[4];
                    rowItem["PosStart"] = rowReadData[5];
                    rowItem["Error"] = rowReadData[6];
                    rowItem["Position"] = rowReadData[7];
                    rowItem["Speed"] = rowReadData[8];
                }
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();
                this.uDSConfigMotion.Rows.Add();

            }
            catch
            {
                //MessageBox.Show("ERROR !! READ TO EXCEL FILE !!");
            }
            finally
            {

            }
        }
    }
}
