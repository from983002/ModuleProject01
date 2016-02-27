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
    class CNIAMDSConfigIO
    {
        //public System.Windows.Forms.Timer m_tmIOTick;
        public CNIAMDSConfigIO()
        {
            //m_tmIOTick = new System.Windows.Forms.Timer();
            //this.m_tmIOTick.Tick += new System.EventHandler(this.m_tmIOTick_Tick);
            //m_tmIOTick.Enabled = false;
            //m_tmIOTick.Interval = 100;

            //this.uDSConfigInp = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
            //this.uDSConfigOtp = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
        }

        int m_Inp_Module_Quentity = 0;
        public int Inp_Module_Quentity
        {
            get { return this.m_Inp_Module_Quentity; }
            set { this.m_Inp_Module_Quentity = value; }
        }

        int m_Otp_Module_Quentity = 0;
        public int Otp_Module_Quentity
        {
            get { return this.m_Otp_Module_Quentity; }
            set { this.m_Otp_Module_Quentity = value; }
        }

        //설정에서 저장한 입력 맵 데이터가 저장되는 배열
        bool[] m_abReadedInpMapUse;
        public bool[] ReadedInpMapUse
        {
            get { return this.m_abReadedInpMapUse; }
            set { this.m_abReadedInpMapUse = value; }
        }

        //장비에서 읽어온 입력 맵 데이터가 저장되는 배열
        bool[] m_abReadedInpMapData;
        public bool[] ReadedInpMapData
        {
            get { return this.m_abReadedInpMapData; }
            set { this.m_abReadedInpMapData = value; }
        }

        //설정에서 저장한 출력 맵 데이터가 저장되는 배열
        bool[] m_abReadedOtpMapUse;
        public bool[] ReadedOtpMapUse
        {
            get { return this.m_abReadedOtpMapUse; }
            set { this.m_abReadedOtpMapUse = value; }
        }

        //장비에서 읽어온 출력 맵 데이터가 저장되는 배열
        bool[] m_abReadedOtpMapData;
        public bool[] ReadedOtpMapData
        {
            get { return this.m_abReadedOtpMapData; }
            set { this.m_abReadedOtpMapData = value; }
        }


        //델리게이트를 진행할 함수를 지정해 준다.
        public delegate void MyEventOne(bool[] disData);
        //MyEventOne : 델리게이트 명.
        //PortInData : 이벤트 전송시 전달할 데이터.

        //이벤트를 진행할 함수를 지정해 준다.
        public event MyEventOne DisplayInpStatus;
        //MyEventOne : 델리게이트 명.
        //CompleteInput : Base Class의 멤버함수명. 실질적인 Behavior이 진행되는 함수.

        //델리게이트를 진행할 함수를 지정해 준다.
        public delegate void MyEventTwo(bool[] disData);
        //MyEventOne : 델리게이트 명.
        //PortInData : 이벤트 전송시 전달할 데이터.

        //이벤트를 진행할 함수를 지정해 준다.
        public event MyEventTwo DisplayOtpStatus;
        //MyEventOne : 델리게이트 명.
        //CompleteInput : Base Class의 멤버함수명. 실질적인 Behavior이 진행되는 함수.

        private void m_tmIOTick_Tick(object sender, EventArgs e)
        {
            //this.m_abReadedInpMapData = readInput_Map();
            //this.m_abReadedOtpMapData = readOutput_Map();
            //DisplayInpStatus(this.m_abReadedInpMapData);
            //DisplayOtpStatus(this.m_abReadedOtpMapData);
        }

        public bool[] readInput_Map()
        {
            short nIndex = 0;
            uint uDataHigh = 0, uDataLow = 0, uFlagHigh = 0, uFlagLow = 0, uModuleID = 0;
            int nBoardNo = 0, nModulePos = 0; 

            int boolArrayQuentity = this.m_Inp_Module_Quentity * 32;

            bool[] m_bReadResultIOMap = new bool[boolArrayQuentity];

            for (int moduleNo = 0; moduleNo < this.m_Inp_Module_Quentity; moduleNo++)
            {
                CAXD.AxdInfoGetModule(moduleNo, ref nBoardNo, ref nModulePos, ref uModuleID);
                CAXD.AxdiReadInportWord(moduleNo, 0, ref uDataHigh);
                CAXD.AxdiReadInportWord(moduleNo, 1, ref uDataLow);

                for (nIndex = 0; nIndex < 16; nIndex++)
                {
                    int wIndex = nIndex + 32*moduleNo;
                    // Verify the last bit value of data read
                    uFlagHigh = uDataHigh & 0x0001;
                    uFlagLow = uDataLow & 0x0001;

                    // Shift rightward by bit by bit
                    uDataHigh = uDataHigh >> 1;
                    uDataLow = uDataLow >> 1;

                    // Updat bit value in control
                    if (uFlagHigh == 1)
                        m_bReadResultIOMap[wIndex] = true;
                    else
                        m_bReadResultIOMap[wIndex] = false;

                    if (uFlagLow == 1)
                        m_bReadResultIOMap[wIndex + 16] = true;
                    else
                        m_bReadResultIOMap[wIndex + 16] = false;
                }
            }    
            return m_bReadResultIOMap;
        }

        public bool[] readOutput_Map()
        {
            short nIndex = 0;
            uint uDataHigh = 0, uDataLow = 0, uFlagHigh = 0, uFlagLow = 0,uModuleID = 0;
            int nBoardNo = 0, nModulePos = 0; 

            int boolArrayQuentity = this.m_Otp_Module_Quentity * 32;

            bool[] m_bReadResultIOMap = new bool[boolArrayQuentity];

            for (int moduleNo = 0; moduleNo < this.m_Otp_Module_Quentity; moduleNo++)
            {
                CAXD.AxdInfoGetModule(moduleNo + 3, ref nBoardNo, ref nModulePos, ref uModuleID);
                CAXD.AxdoReadOutportWord(moduleNo + 3, 0, ref uDataHigh);                
                CAXD.AxdoReadOutportWord(moduleNo + 3, 1, ref uDataLow);

                for (nIndex = 0; nIndex < 16; nIndex++)
                {
                    int wIndex = nIndex + 32 * moduleNo;
                    // Verify the last bit value of data read
                    uFlagHigh = uDataHigh & 0x0001;
                    uFlagLow = uDataLow & 0x0001;

                    // Shift rightward by bit by bit
                    uDataHigh = uDataHigh >> 1;
                    uDataLow = uDataLow >> 1;

                    // Updat bit value in control
                    if (uFlagHigh == 1)
                        m_bReadResultIOMap[wIndex] = true;
                    else
                        m_bReadResultIOMap[wIndex] = false;

                    if (uFlagLow == 1)
                        m_bReadResultIOMap[wIndex + 16] = true;
                    else
                        m_bReadResultIOMap[wIndex + 16] = false;
                }
            }
            return m_bReadResultIOMap;
        }
            

        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public static int WM_QUIT = 0x12;
        public Excel.Application xls;
        public Excel.Worksheet xlsSheet;
        public Excel.Workbook xlsBook;
        private object opt = Missing.Value;

        private Infragistics.Win.UltraWinDataSource.UltraDataSource uDSConfigInp;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource uDSConfigOtp;

        string excelFilePath;
        public UltraDataSource ConfigInpMap
        {           
            get { return this.uDSConfigInp; }
        }
        public UltraDataSource ConfigOtpMap
        {
            get { return this.uDSConfigOtp; }
        }

        public void ExcelFileOpen(string filePath)
        {
            string fileNamePath = filePath;
            
            xls = new Excel.Application();
            xls.Visible = false;
            xlsBook = (Excel.Workbook)xls.Workbooks.Open(fileNamePath, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt);
            xlsSheet = (Excel.Worksheet)xlsBook.ActiveSheet;
            Debug.Write("ExcelControl 종료 : " + System.DateTime.Now.ToString() + System.Environment.NewLine);
            ReadInp_SaveFile(xlsSheet);
            ReadOtp_SaveFile(xlsSheet);
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


        private void ReadInp_SaveFile(Excel.Worksheet xlsSheet)
        {
            this.uDSConfigInp = new Infragistics.Win.UltraWinDataSource.UltraDataSource();

            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnNo1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnName1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnStatus1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnNo2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnName2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnStatus2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnNo3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No3");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnName3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name3");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn uDataColumnStatus3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status3");
            //ultraDataColumn1.DataType = typeof(uint);
            this.uDSConfigInp.Band.Columns.AddRange(new object[] {
            uDataColumnNo1, uDataColumnName1, uDataColumnStatus1,
            uDataColumnNo2, uDataColumnName2, uDataColumnStatus2,
            uDataColumnNo3, uDataColumnName3, uDataColumnStatus3,});

            int boolArrayQuentity = this.m_Inp_Module_Quentity * 32;
            this.m_abReadedInpMapUse = new bool[boolArrayQuentity];
            try
            {
                this.uDSConfigInp.Rows.Clear();
                string rowReadData;
                UltraDataRow rowItem=null;
                int IONameReadCount = 1;
                int ColumnCount = 1;
                int IOMapAddressCount = 0;
                for (int row = 4; row < 100; row++) //
                {
                    
                    Debug.Write("Row " + row.ToString() + " 입니다." + System.Environment.NewLine);
                    if (((Excel.Range)xlsSheet.Cells[row, 5]).Value2 == null) // null 이면 종료
                        break;

                    if (((Excel.Range)xlsSheet.Cells[row, 6]).Value2 == null)
                    {
                        m_abReadedInpMapUse[IOMapAddressCount] = false;
                        IOMapAddressCount++;
                        continue;
                    }
                    else
                    {
                        m_abReadedInpMapUse[IOMapAddressCount] = true;
                        IOMapAddressCount++;
                    }
                    

                    if (ColumnCount == 1)
                    {
                        this.uDSConfigInp.Rows.Add();
                        rowItem = this.uDSConfigInp.Rows[this.uDSConfigInp.Rows.Count - 1];
                    }

                    //if (((Excel.Range)xlsSheet.Cells[row, 6]).Value2 == null)
                    //    continue;

                    rowReadData = ((Excel.Range)xlsSheet.Cells[row, 6]).Value2.ToString().Trim();

                    switch (ColumnCount)
                    {
                        case 1:
                            rowItem["NO1"] = IONameReadCount.ToString("00");
                            rowItem["Name1"] = rowReadData;
                            rowItem["Status1"] = (IOMapAddressCount - 1).ToString("00");

                            break;
                        case 2:
                            rowItem["NO2"] = IONameReadCount.ToString("00");
                            rowItem["Name2"] = rowReadData;
                            rowItem["Status2"] = (IOMapAddressCount - 1).ToString("00");
                            break;
                        case 3:
                            rowItem["NO3"] = IONameReadCount.ToString("00");
                            rowItem["Name3"] = rowReadData;
                            rowItem["Status3"] = (IOMapAddressCount - 1).ToString("00");
                            break;
                    }

                    if (ColumnCount == 3) ColumnCount = 1;
                    else ColumnCount++;

                    IONameReadCount++;
                }
                int irc = this.uDSConfigInp.Rows.Count;
                for (int i = 0; i < (27 - irc); i++) this.uDSConfigInp.Rows.Add();
            }
            catch
            {
                //MessageBox.Show("ERROR !! READ TO EXCEL FILE !!");
            }
            finally
            {

            }
        }

        private void ReadOtp_SaveFile(Excel.Worksheet xlsSheet)
        {
            this.uDSConfigOtp = new Infragistics.Win.UltraWinDataSource.UltraDataSource();

            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("No3");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name3");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status3");
            ultraDataColumn1.DataType = typeof(uint);
            this.uDSConfigOtp.Band.Columns.AddRange(new object[] {
            ultraDataColumn1, ultraDataColumn2, ultraDataColumn3,
            ultraDataColumn4, ultraDataColumn5, ultraDataColumn6,
            ultraDataColumn7, ultraDataColumn8, ultraDataColumn9,});
            
            int boolArrayQuentity = this.m_Otp_Module_Quentity * 32;
            this.m_abReadedOtpMapUse = new bool[boolArrayQuentity];

            try
            {
                this.uDSConfigOtp.Rows.Clear();
                string rowReadData;
                string rowStatusDAta;
                UltraDataRow rowItem = null;
                int IONameReadCount = 1;
                int ColumnCount = 1;
                int IOMapAddressCount = 0;
                for (int row = 100; row < 200; row++) //
                {
                    Debug.Write("Row " + row.ToString() + " 입니다." + System.Environment.NewLine);
                    if (((Excel.Range)xlsSheet.Cells[row, 5]).Value2 == null) // null 이면 종료
                        break;

                    if (((Excel.Range)xlsSheet.Cells[row, 6]).Value2 == null)
                    {
                        m_abReadedOtpMapUse[IOMapAddressCount] = false;
                        IOMapAddressCount++;
                        continue;
                    }
                    else
                    {
                        m_abReadedOtpMapUse[IOMapAddressCount] = true;
                        IOMapAddressCount++;
                    }


                    if (ColumnCount == 1)
                    {
                        this.uDSConfigOtp.Rows.Add();
                        rowItem = this.uDSConfigOtp.Rows[this.uDSConfigOtp.Rows.Count - 1];
                    }


                    rowReadData = ((Excel.Range)xlsSheet.Cells[row, 6]).Value2.ToString().Trim();
                    rowStatusDAta = ((Excel.Range)xlsSheet.Cells[row, 7]).Value2.ToString().Trim();
                    
                    switch (ColumnCount)
                    {
                        case 1:
                            rowItem["NO1"] = IONameReadCount.ToString("00");
                            rowItem["Name1"] = rowReadData;
                            rowItem["Status1"] = (IOMapAddressCount-1).ToString("00");
                            break;
                        case 2:
                            rowItem["NO2"] = IONameReadCount.ToString("00");
                            rowItem["Name2"] = rowReadData;
                            rowItem["Status2"] = (IOMapAddressCount - 1).ToString("00");
                            break;
                        case 3:
                            rowItem["NO3"] = IONameReadCount.ToString("00");
                            rowItem["Name3"] = rowReadData;
                            rowItem["Status3"] = (IOMapAddressCount - 1).ToString("00");
                            break;
                    }

                    if (ColumnCount == 3) ColumnCount = 1;
                    else ColumnCount++;

                    IONameReadCount++;
                }
                int orc = uDSConfigOtp.Rows.Count;
                for (int i = 0; i < (12 - orc); i++) this.uDSConfigOtp.Rows.Add();
                //this.uDSConfigOtp.Rows.Add();
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
