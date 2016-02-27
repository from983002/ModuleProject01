using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;


//using Excel = Microsoft.Office.Interop.Excel;

namespace SystemAlign
{
    class CNIAMDSConfigAxis
    {
        System.Collections.ArrayList m_ParameterList_Name = new System.Collections.ArrayList();
        System.Collections.ArrayList m_ParameterList_Data = new System.Collections.ArrayList();

        string regNodePath_Config_Axis = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_AXIS\\";

        public CNIAMDSConfigAxis()
        {
            this.regNodePath_Config_Axis = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_AXIS\\" + this.AssyOne_AxisName;
            if (this.Register_Make_Check() == true)
            {
                this.Registry_Write_Initialize();
            }

            for (int i = 0; i < 9; i++)
            {
                this.AxisStatusData[i] = new System.Collections.ArrayList();
            }            
        }

        public string RegNodePath_Config_Axis
        {
            get { return this.regNodePath_Config_Axis; }
            set { this.regNodePath_Config_Axis = value; }

        }
        string filePath;// = System.Windows.Forms.Application.StartupPath + @"\NIAMRearMotionParameter0.xlsx";
        string assyOne_AxisName = "AXIS00";


//         static public string getReg(string wdata)
//         {
//             RegistryKey reg = Registry.LocalMachine;
//             reg = reg.OpenSubKey("Software\\Nexstar Technology\\NSIARMan\\NSIAR_CONFIG_ETC\\ET", true);
//             if (reg == null)
//             {
//                 return "";
//             }
//             else
//             {
//                 return Convert.ToString(reg.GetValue(wdata));
//             }
//         }

        public string AssyOne_AxisName
        {
            get { return this.assyOne_AxisName; }
            set { this.assyOne_AxisName = value; }
        }

        public System.Collections.ArrayList ParameterList_Name
        {
            get { return this.m_ParameterList_Name; }
            set { this.m_ParameterList_Name = value; }
        }

        public System.Collections.ArrayList ParameterList_Data
        {
            get { return this.m_ParameterList_Data; }
            set { this.m_ParameterList_Data = value; }
        }

        public Infragistics.Win.UltraWinDataSource.UltraDataSource DSConfigMotion
        {
            get { return this.uDSConfigMotion; }
            set { this.uDSConfigMotion = value; }
        }  

        private Infragistics.Win.UltraWinDataSource.UltraDataSource uDSConfigMotion ;
       
        public void Read_Motion_Parameter_WordFile()
        {

            this.uDSConfigMotion = new Infragistics.Win.UltraWinDataSource.UltraDataSource();

            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis0");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis3");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis4");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis5");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis6");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis7");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis8");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Axis9");

            this.uDSConfigMotion.Band.Columns.AddRange(new object[] { ultraDataColumn1, ultraDataColumn2, ultraDataColumn3, ultraDataColumn4, ultraDataColumn5,
                ultraDataColumn6, ultraDataColumn7, ultraDataColumn8, ultraDataColumn9, ultraDataColumn10, ultraDataColumn11 });

            this.m_ColumnNo = 0;
            string filePath = System.Windows.Forms.Application.StartupPath + @"\MotionDefault.mot";
            //string filePath = @"C:\Program Files (x86)\EzSoftware RM\EzSoftware\MotionDefault.mot";
            string strLine = "";
            StreamReader srFile = new StreamReader(filePath);

            while ((strLine = srFile.ReadLine()) != null)
            {
                StringParsing(strLine);
            }
            srFile.Close();
        }

        int m_ColumnNo = 1;
        private void StringParsing(string strLine)
        {

            string readNo = "", readName = "", readData = "";
            int indexNo = strLine.IndexOf('.');
            int indexLeng = strLine.Length;
            string Datastring = strLine.Substring(0, 2);
            if (Datastring == "##" || Datastring == "#=") return;

            readNo = strLine.Substring(0, 2);
            if (readNo == "00") this.m_ColumnNo++;
            indexNo = strLine.IndexOf('=');
            readName = strLine.Substring(3, indexNo - 3);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            Infragistics.Win.UltraWinDataSource.UltraDataRow ultraDataRow = null;
            if (this.uDSConfigMotion.Rows.Count < 40)
            {
                this.uDSConfigMotion.Rows.Add();
                ultraDataRow = this.uDSConfigMotion.Rows[this.uDSConfigMotion.Rows.Count - 1];
                ultraDataRow[0] = readName;
                ultraDataRow[1] = readData;
            }
            else
            {
                int cn = this.uDSConfigMotion.Band.Columns.Count;
                ultraDataRow = this.uDSConfigMotion.Rows[int.Parse(readNo)];
                ultraDataRow[this.m_ColumnNo] = readData;
            }
        }
 /*
        private void ReadMotionParameter0(Excel.Worksheet xlsSheet)
        {
            string axisNo = null;
            
            for (int nCol = 1; nCol < 1000; nCol++)
            {
                if (((Excel.Range)xlsSheet.Cells[1, nCol]).Value2 == null) // null 이면 종료
                    break;

                if (((Excel.Range)xlsSheet.Cells[1, nCol]).Value2 != null && ((Excel.Range)xlsSheet.Cells[1, nCol + 1]).Value2 != null) // null 이면 종료
                {
                    axisNo = ((Excel.Range)xlsSheet.Cells[1, nCol + 1]).Value2.ToString().Trim();
                }

                if (int.Parse(axisNo) != 0)
                {
                    nCol++;
                    continue;
                }

                for (int nRow = 1; nRow < 1000; nRow++)
                {
                    if (((Excel.Range)xlsSheet.Cells[nRow, nCol]).Value2 == null && ((Excel.Range)xlsSheet.Cells[nRow, nCol + 1]).Value2 == null) // null 이면 종료
                        break;
                    string emps = ((Excel.Range)xlsSheet.Cells[nRow, nCol]).Value2.ToString();
                    m_ParameterList_Name.Add(((Excel.Range)xlsSheet.Cells[nRow, nCol]).Value2.ToString()); 
                }
                break;
            }            
        }

       
        public void Regstry_Read_Config_Axis()
        {
            RegistryKey reg = Registry.CurrentUser;

            reg = reg.OpenSubKey(this.regNodePath_Config_Axis, true);
            for (int i = 0; i < m_ParameterList_Name.Count; i++)
            {
                string tmpd = m_ParameterList_Name[i].ToString();
                m_ParameterList_Data.Add(reg.GetValue(m_ParameterList_Name[i].ToString(), "").ToString());
            }
        }
        */

        public string Regstry_Read_Config_Axis(string strNodePath, string regName)
        {
            RegistryKey reg = Registry.CurrentUser;

            reg = reg.OpenSubKey(strNodePath, true);
            string regData = reg.GetValue(regName, "").ToString();
            reg.Close();
            return regData;
        }

        double[] m_Registry_Initialize_Value = { 500.0, 800.0, 100.0, 200.0, 100.0, 800.0 };
        string[] m_Registry_Initialize_Name = { "MOVE_VELOCITY", "MOVE_ACCEL", "JOG_VELOCITY", "JOG_ACCEL", "HOME_VELOCITY", "HOME_ACCEL" };

        public string[] Registry_Initialize_Name
        {
            get { return this.m_Registry_Initialize_Name; }
            set { m_Registry_Initialize_Name = value; }
        }

        public bool Register_Make_Check()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey("Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_AXIS\\AXIS00", true);
            if (reg == null)
            {                
                return true;
            }
            reg.Close();
            return false;
        }

        public void Registry_Write_Initialize()
        {
            for (int i = 0; i < 10;i++ )
            {
                this.regNodePath_Config_Axis = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_AXIS\\AXIS" + i.ToString("00");
                for (int j = 0; j < 6; j++ )
                {
                    this.setReg(this.regNodePath_Config_Axis, m_Registry_Initialize_Name[j], m_Registry_Initialize_Value[j].ToString());
                }
            }
        }
        
        public void setReg(string strNodePath, string strName, string strDAta)
        {
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey(strNodePath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(strName, strDAta, RegistryValueKind.String);
            reg.Close();
        }

        uint duRetCode;
        private void Axis_Status_Signal_Set(IServoMotors Axis_On)
        {
            //상대, 절재 위치 설정
            duRetCode = CAXM.AxmMotSetAbsRelMode(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_INIT_ABSRELMODE);
            //리미트 시그널 설정
            duRetCode = CAXM.AxmSignalSetLimit(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_STOP_SIGNAL_MODE, Axis_On.AXIS_CONF_POS_END_LIMIT, Axis_On.AXIS_CONF_NEG_END_LIMIT);
            //InPosition 사용 설정
            duRetCode = CAXM.AxmSignalSetInpos(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_INPOSITION);
            //서보 알람 시그널 설정
            duRetCode = CAXM.AxmSignalSetServoAlarm(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_ALARM);
            //서보 감속 정지 사용 설정
            duRetCode = CAXM.AxmSignalSetStop(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_STOP_SIGNAL_MODE, Axis_On.AXIS_CONF_STOP_SIGNAL_LEVEL);
            //서보의 최소속도 설정
            duRetCode = CAXM.AxmMotSetMinVel(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_MIN_VELOCITY);
            //서보의 펄스 메소드 설정
            duRetCode = CAXM.AxmMotSetPulseOutMethod(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_PULSE_OUT_METHOD);
            //서보의 입력 메소드 설정
            duRetCode = CAXM.AxmMotSetEncInputMethod(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_ENC_INPUT_METHOD);
            //서보 온 설정
            duRetCode = CAXM.AxmSignalServoOn(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_SVON_LEVEL);
        }

       

        public void Axis_Configration_Set(IServoMotors Axis_On, int nAxis)
        {
            //RM 에서 설정된 부분을 사용한다.
            this.Axis_Status_Signal_Set(Axis_On);

            //설정 단위가 1mm가 되도록 Unit을 설정한다.(10:10000 = 1pulse에 1mm)
            duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(Axis_On.AXIS_CONF_NO, 10, 10000);
        }

        public void Axis_Parameter_Set(IServoMotors Axis_On, int nAxis)
        {
            nAxis = nAxis + 1;
            string nodePath = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_AXIS\\AXIS" + nAxis.ToString("00");
            Axis_On.AXIS_MEAS_MOVE_VELOCITY = double.Parse(Regstry_Read_Config_Axis(nodePath, m_Registry_Initialize_Name[0]));
            Axis_On.AXIS_MEAS_MOVE_ACCEL    = double.Parse(Regstry_Read_Config_Axis(nodePath, m_Registry_Initialize_Name[1]));
            Axis_On.AXIS_MEAS_JOG_VELOCITY  = double.Parse(Regstry_Read_Config_Axis(nodePath, m_Registry_Initialize_Name[2]));
            Axis_On.AXIS_MEAS_JOG_ACCEL     = double.Parse(Regstry_Read_Config_Axis(nodePath, m_Registry_Initialize_Name[3]));
            Axis_On.AXIS_MEAS_HOME_VELOCITY = double.Parse(Regstry_Read_Config_Axis(nodePath, m_Registry_Initialize_Name[4]));
            Axis_On.AXIS_MEAS_HOME_ACCEL    = double.Parse(Regstry_Read_Config_Axis(nodePath, m_Registry_Initialize_Name[5]));

            Axis_On.AXIS_CONF_NO                    = int.Parse(this.uDSConfigMotion.Rows[0].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_PULSE_OUT_METHOD      = uint.Parse(this.uDSConfigMotion.Rows[1].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_ENC_INPUT_METHOD      = uint.Parse(this.uDSConfigMotion.Rows[2].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INPOSITION            = uint.Parse(this.uDSConfigMotion.Rows[3].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_ALARM                 = uint.Parse(this.uDSConfigMotion.Rows[4].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_NEG_END_LIMIT         = uint.Parse(this.uDSConfigMotion.Rows[5].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_POS_END_LIMIT         = uint.Parse(this.uDSConfigMotion.Rows[6].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_MIN_VELOCITY          = double.Parse(this.uDSConfigMotion.Rows[7].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_MAX_VELOCITY          = double.Parse(this.uDSConfigMotion.Rows[8].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_SIGNAL           = uint.Parse(this.uDSConfigMotion.Rows[9].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_LEVEL            = uint.Parse(this.uDSConfigMotion.Rows[10].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_DIR              = int.Parse(this.uDSConfigMotion.Rows[11].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_ZPHASE_LEVEL          = uint.Parse(this.uDSConfigMotion.Rows[12].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_ZPHASE_USE            = uint.Parse(this.uDSConfigMotion.Rows[13].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_STOP_SIGNAL_MODE      = uint.Parse(this.uDSConfigMotion.Rows[14].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_STOP_SIGNAL_LEVEL     = uint.Parse(this.uDSConfigMotion.Rows[15].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_FIRST_VELOCITY   = double.Parse(this.uDSConfigMotion.Rows[16].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_SECOND_VELOCITY  = double.Parse(this.uDSConfigMotion.Rows[17].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_THIRD_VELOCITY   = double.Parse(this.uDSConfigMotion.Rows[18].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_LAST_VELOCITY    = double.Parse(this.uDSConfigMotion.Rows[19].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_FIRST_ACCEL      = double.Parse(this.uDSConfigMotion.Rows[20].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_SECOND_ACCEL     = double.Parse(this.uDSConfigMotion.Rows[21].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_END_CLEAR_TIME   = double.Parse(this.uDSConfigMotion.Rows[22].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_HOME_END_OFFSET       = double.Parse(this.uDSConfigMotion.Rows[23].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_NEG_SOFT_LIMIT        = double.Parse(this.uDSConfigMotion.Rows[24].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_POS_SOFT_LIMIT        = double.Parse(this.uDSConfigMotion.Rows[25].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_MOVE_PULSE            = double.Parse(this.uDSConfigMotion.Rows[26].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_MOVE_UNIT             = double.Parse(this.uDSConfigMotion.Rows[27].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INIT_POSITION         = double.Parse(this.uDSConfigMotion.Rows[28].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INIT_VELOCITY         = double.Parse(this.uDSConfigMotion.Rows[29].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INIT_ACCEL            = double.Parse(this.uDSConfigMotion.Rows[30].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INIT_DECEL            = double.Parse(this.uDSConfigMotion.Rows[31].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INIT_ABSRELMODE       = uint.Parse(this.uDSConfigMotion.Rows[32].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_INIT_PROFILEMODE      = uint.Parse(this.uDSConfigMotion.Rows[33].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_SVON_LEVEL            = uint.Parse(this.uDSConfigMotion.Rows[34].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_ALARM_RESET_LEVEL     = uint.Parse(this.uDSConfigMotion.Rows[35].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_ENCODER_TYPE          = uint.Parse(this.uDSConfigMotion.Rows[36].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_SOFT_LIMIT_SEL        = uint.Parse(this.uDSConfigMotion.Rows[37].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_SOFT_LIMIT_STOP_MODE  = uint.Parse(this.uDSConfigMotion.Rows[38].GetCellValue(nAxis).ToString());
            Axis_On.AXIS_CONF_SOFT_LIMIT_ENABLE     = uint.Parse(this.uDSConfigMotion.Rows[39].GetCellValue(nAxis).ToString());
        }


        System.Collections.ArrayList[] AxisStatusData = new System.Collections.ArrayList[9];

        public System.Collections.ArrayList[] AxisStatusArray
        {
            get { return this.AxisStatusData; }
            set { this.AxisStatusData = value; }
        }
        //Assy1에 있는 모션 상태 그리드 표시를 업데이트한다.
        public void Axis_Status_Read()
        {
            uint duStatus = 0, duRetCode = 0;
            bool SignalOutput = false;

            for (int i = 0; i < 9; i++)
            {
                this.AxisStatusData[i].Clear();
                //Column 0
                this.AxisStatusData[i].Add(i.ToString("00"));

                duRetCode = CAXM.AxmSignalIsServoOn(i, ref duStatus);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) SignalOutput = (Convert.ToBoolean(duStatus));
                //Column 1
                this.AxisStatusData[i].Add(SignalOutput);

                duRetCode = CAXM.AxmSignalReadServoAlarm(i, ref duStatus);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) SignalOutput = (Convert.ToBoolean(duStatus));
                //Column 2
                this.AxisStatusData[i].Add(SignalOutput);

                duRetCode = CAXM.AxmStatusReadInMotion(i, ref duStatus);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) SignalOutput = (Convert.ToBoolean(duStatus));
                //Column 3
                this.AxisStatusData[i].Add(SignalOutput);

                duRetCode = CAXM.AxmHomeReadSignal(i, ref duStatus);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) SignalOutput = (Convert.ToBoolean(duStatus));
                //Column 4
                this.AxisStatusData[i].Add(SignalOutput);

                uint uPositiveStatus = 0, uNegativeStatus = 0;
                CAXM.AxmSignalReadLimit(i, ref uPositiveStatus, ref uNegativeStatus);
                //Column 5
                this.AxisStatusData[i].Add(uPositiveStatus);
                //Column 6
                this.AxisStatusData[i].Add(uNegativeStatus);

                double locationData = 0;
                duRetCode = CAXM.AxmStatusGetActPos(i, ref locationData);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    //Column 7
                    this.AxisStatusData[i].Add(locationData.ToString());

                double speedData = 0;
                duRetCode = CAXM.AxmStatusReadVel(i, ref speedData);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    //Column 8
                    this.AxisStatusData[i].Add(speedData.ToString("0.000"));
            }
        }

        double m_CurrntLocation;
        public double CurrentLocation
        {
            get { return this.m_CurrntLocation; }
            set { this.m_CurrntLocation = value; }
        }

        public void GetActPos(IServoMotors InAxis)
        {
            duRetCode = CAXM.AxmStatusGetActPos(InAxis.AXIS_CONF_NO , ref this.m_CurrntLocation);
        }
        
    }
}
