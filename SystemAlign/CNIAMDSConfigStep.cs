using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SystemAlign
{
    class CNIAMDSConfigStep
    {
        EziMotionPlusR m_EziMotion;// = new EziMotionPlusR();
        string regNodePath_Config_Step = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_STEP\\";
        public CNIAMDSConfigStep()
        {
            EziMotion_Connect();
            EziMotion_Check_Slave();
            //EziMotion_Get_SlaveInfo();

            this.regNodePath_Config_Step = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_STEP\\" + this.m_StepName;
            if (this.Register_Make_Check() == true)
            {
                this.Registry_Write_Initialize();
            }

            for (int i = 0; i < 2; i++)
            {
                this.m_StepStatusData[i] = new System.Collections.ArrayList();
            }
        }

        int nBuffSize = 256;
        int nRtn;
        byte nType;
        //char[] IpBuffer = new char[256];
        string IpBuffer;
        byte iSlaveNo = 0;
        byte nPortNo = 1;
        int uBaudRate = 115200;
        int upModuleID = 0;

        private void EziMotion_Connect()
        {
            EziMotionPlusR.FAS_Connect(nPortNo, uBaudRate);
        }

        private void EziMotion_Close()
        {
            EziMotionPlusR.FAS_Close(nPortNo);
        }

        private void EziMotion_Check_Slave()
        {
            if (EziMotionPlusR.FAS_IsSlaveExist(nPortNo, iSlaveNo) == false) return;
        }

        private void EziMotion_Get_SlaveInfo()
        {
            nRtn = EziMotionPlusR.FAS_GetSlaveInfo(nPortNo, iSlaveNo, nType, ref IpBuffer, nBuffSize);
            string IRtn = nRtn.ToString();
        }

        System.Collections.ArrayList m_ParameterList_Name = new System.Collections.ArrayList();
        System.Collections.ArrayList m_ParameterList_Data = new System.Collections.ArrayList();

        System.Collections.ArrayList[] m_StepStatusData = new System.Collections.ArrayList[2];
        public System.Collections.ArrayList[] StepStatusArray
        {
            get { return this.m_StepStatusData; }
            set { this.m_StepStatusData = value; }
        }

        byte iAlarmType;
        int dwOutPut;
        uint InStatus;
        uint OutStatus;
        uint AxisStatus;
        uint CmdPos;
        uint ActPos, ActVel;
        uint PosError;
        uint AtoVal;
        ushort PosItemNo;
        int LogicMask = 4;
        bool Level;
        int[] logicArray = new int[24];
        bool[] levelArray = new bool[24];
        int uFlagHigh, uFlagLow, uDataHigh, uDataLow;
        int m_stepNo = 0;
        byte b;
        byte LogicNo;
        byte mLevel;
        int itemN = 18;

      
        public void Step_Status_Read()
        {
            for (int i = 0; i < 2; i++)
            {
                m_StepStatusData[i].Clear();
                EziMotionPlusR.FAS_GetAxisStatus(nPortNo, (byte)i, ref  dwOutPut);
                int nIndex = 0;
                for (nIndex = 0; nIndex < 32; nIndex++)
                {
                    // Verify the last bit value of data read
                    uFlagHigh = dwOutPut & 0x00000001;

                    // Shift rightward by bit by bit
                    dwOutPut = dwOutPut >> 1;

                    // Updat bit value in control
                    if (uFlagHigh == 1)
                        m_StepStatusData[i].Add(true);
                    else
                        m_StepStatusData[i].Add(false);
                }
                EziMotionPlusR.FAS_GetCommandPos(nPortNo, (byte)i, ref CmdPos); //(nPortNo, (byte)i, ref  dwOutPut);
                m_StepStatusData[i].Add(CmdPos);
                EziMotionPlusR.FAS_GetActualVel(nPortNo, (byte)i, ref ActVel); //(nPortNo, (byte)i, ref  dwOutPut);
                m_StepStatusData[i].Add(ActVel);
            }           
        }

        string m_StepName = "STEP00";
        public string StepName
        {
            get { return this.m_StepName; }
            set { this.m_StepName = value; }
        }

       

        public bool Register_Make_Check()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey("Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_STEP\\STEP00", true);
            if (reg == null)
            {
                return true;
            }
            reg.Close();
            return false;
        }

        double[] m_Registry_Initialize_Value = { 80000.0, 100.0, 10000.0, 100.0, 80000.0, 100.0 };
        string[] m_Registry_Initialize_Name = { "MOVE_VELOCITY", "MOVE_ACCEL", "JOG_VELOCITY", "JOG_ACCEL", "HOME_VELOCITY", "HOME_ACCEL" };

        public void Registry_Write_Initialize()
        {
            for (int i = 0; i < 2; i++)
            {
                this.regNodePath_Config_Step = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_STEP\\STEP" + i.ToString("00");
                for (int j = 0; j < 6; j++)
                {
                    this.setReg(this.regNodePath_Config_Step, m_Registry_Initialize_Name[j], m_Registry_Initialize_Value[j].ToString());
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

        public string Regstry_Read_Config_Step(string strNodePath, string regName)
        {
            RegistryKey reg = Registry.CurrentUser;

            reg = reg.OpenSubKey(strNodePath, true);
            string regData = reg.GetValue(regName, "").ToString();
            reg.Close();
            return regData;
        }


        uint duRetCode;
        private void Step_Status_Signal_Set(IStepMotors Axis_On)
        {            
            //상대, 절재 위치 설정
            //duRetCode = CAXM.AxmMotSetAbsRelMode(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_INIT_ABSRELMODE);
            //리미트 시그널 설정
            //duRetCode = CAXM.AxmSignalSetLimit(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_STOP_SIGNAL_MODE, Axis_On.AXIS_CONF_POS_END_LIMIT, Axis_On.AXIS_CONF_NEG_END_LIMIT);
            //InPosition 사용 설정
            //duRetCode = CAXM.AxmSignalSetInpos(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_INPOSITION);
            //서보 알람 시그널 설정
            //duRetCode = CAXM.AxmSignalSetServoAlarm(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_ALARM);
            //서보 감속 정지 사용 설정
            //duRetCode = CAXM.AxmSignalSetStop(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_STOP_SIGNAL_MODE, Axis_On.AXIS_CONF_STOP_SIGNAL_LEVEL);
            //서보의 최소속도 설정
            //duRetCode = CAXM.AxmMotSetMinVel(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_MIN_VELOCITY);
            //서보의 펄스 메소드 설정
            //duRetCode = CAXM.AxmMotSetPulseOutMethod(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_PULSE_OUT_METHOD);
            //서보의 입력 메소드 설정
            //duRetCode = CAXM.AxmMotSetEncInputMethod(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_ENC_INPUT_METHOD);
            //서보 온 설정
            //duRetCode = CAXM.AxmSignalServoOn(Axis_On.AXIS_CONF_NO, Axis_On.AXIS_CONF_SVON_LEVEL);
        }



        public void Step_Configration_Set(IStepMotors InStep, int nStep)
        {
            //RM 에서 설정된 부분을 사용한다.
            //this.Step_Status_Signal_Set(InStep);

            //설정 단위가 1mm가 되도록 Unit을 설정한다.(10:10000 = 1pulse에 1mm)
            //duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(Axis_On.AXIS_CONF_NO, 10, 10000);
            bool ezRetCode = EziMotionPlusR.FAS_ServoEnable((byte)1, (byte)0, true);
        }

        public void Step_Parameter_Set(IStepMotors InStep, int StepNo)
        {
            int nStep = 3;
            string nodePath = "Software\\Nexstar Technology\\NIAMMan\\NIAM_CONFIG_STEP\\STEP" + StepNo.ToString("00");
            InStep.MOVE_VELOCITY = double.Parse(Regstry_Read_Config_Step(nodePath, m_Registry_Initialize_Name[0]));
            InStep.MOVE_ACCEL = double.Parse(Regstry_Read_Config_Step(nodePath, m_Registry_Initialize_Name[1]));
            InStep.JOG_VELOCITY = double.Parse(Regstry_Read_Config_Step(nodePath, m_Registry_Initialize_Name[2]));
            InStep.JOG_ACCEL = double.Parse(Regstry_Read_Config_Step(nodePath, m_Registry_Initialize_Name[3]));
            InStep.HOME_VELOCITY = double.Parse(Regstry_Read_Config_Step(nodePath, m_Registry_Initialize_Name[4]));
            InStep.HOME_ACCEL = double.Parse(Regstry_Read_Config_Step(nodePath, m_Registry_Initialize_Name[5]));

            InStep.PULSE_PER_REVOLUTION[2] = uint.Parse(this.uDSConfigStepMotor.Rows[0].GetCellValue(nStep).ToString());
            InStep.AXIS_MAX_SPEED[2] = double.Parse(this.uDSConfigStepMotor.Rows[1].GetCellValue(nStep).ToString());
            InStep.AXIS_START_SPEED[2] = double.Parse(this.uDSConfigStepMotor.Rows[2].GetCellValue(nStep).ToString());
            InStep.AXIS_ACC_TIME[2] = double.Parse(this.uDSConfigStepMotor.Rows[3].GetCellValue(nStep).ToString());
            InStep.AXIS_DEC_TIME[2] = double.Parse(this.uDSConfigStepMotor.Rows[4].GetCellValue(nStep).ToString());
            InStep.SPEED_OVERRIDE[2] = double.Parse(this.uDSConfigStepMotor.Rows[5].GetCellValue(nStep).ToString());
            InStep.JOG_SPEED[2] = double.Parse(this.uDSConfigStepMotor.Rows[6].GetCellValue(nStep).ToString());
            InStep.JOG_START_SPEED[2] = double.Parse(this.uDSConfigStepMotor.Rows[7].GetCellValue(nStep).ToString());
            InStep.JOG_ACC_DEC_TIME[2] = double.Parse(this.uDSConfigStepMotor.Rows[8].GetCellValue(nStep).ToString());
            InStep.ALARM_LOGIC[2] = uint.Parse(this.uDSConfigStepMotor.Rows[9].GetCellValue(nStep).ToString());
            InStep.RUNSTOP_LOGIC[2] = uint.Parse(this.uDSConfigStepMotor.Rows[10].GetCellValue(nStep).ToString());
            InStep.ALARM_RESET_LOGIC[2] = uint.Parse(this.uDSConfigStepMotor.Rows[11].GetCellValue(nStep).ToString());
            InStep.SW_LIMIT_PLUS_VALUE[2] = double.Parse(this.uDSConfigStepMotor.Rows[12].GetCellValue(nStep).ToString());
            InStep.SW_LIMIT_MINUS_VALUE[2] = double.Parse(this.uDSConfigStepMotor.Rows[13].GetCellValue(nStep).ToString());
            InStep.SW_LIMIT_STOP_METHOD[2] = uint.Parse(this.uDSConfigStepMotor.Rows[14].GetCellValue(nStep).ToString());
            InStep.HW_LIMIT_STOP_METHOD[2] = uint.Parse(this.uDSConfigStepMotor.Rows[15].GetCellValue(nStep).ToString());
            InStep.LIMIT_SENSOR_LOGIC[2] = uint.Parse(this.uDSConfigStepMotor.Rows[16].GetCellValue(nStep).ToString());
            InStep.ORIGIN_SPEED[2] = int.Parse(this.uDSConfigStepMotor.Rows[17].GetCellValue(nStep).ToString());
            InStep.ORIGIN_SEARCH_SPEED[2] = int.Parse(this.uDSConfigStepMotor.Rows[18].GetCellValue(nStep).ToString());
            InStep.ORIGIN_ACC_DEC_TIME[2] = double.Parse(this.uDSConfigStepMotor.Rows[19].GetCellValue(nStep).ToString());
            InStep.ORIGIN_METHOD[2] = uint.Parse(this.uDSConfigStepMotor.Rows[20].GetCellValue(nStep).ToString());
            InStep.ORIGIN_DIRECT[2] = uint.Parse(this.uDSConfigStepMotor.Rows[21].GetCellValue(nStep).ToString());
            InStep.ORIGIN_OFFSET[2] = double.Parse(this.uDSConfigStepMotor.Rows[22].GetCellValue(nStep).ToString());
            InStep.ORIGIN_POSITION_SET[2] = double.Parse(this.uDSConfigStepMotor.Rows[23].GetCellValue(nStep).ToString());
            InStep.ORIGIN_SENSOR_LOGIC[2] = uint.Parse(this.uDSConfigStepMotor.Rows[24].GetCellValue(nStep).ToString());
            InStep.STOP_CURRENT[2] = uint.Parse(this.uDSConfigStepMotor.Rows[25].GetCellValue(nStep).ToString());
            InStep.MOTION_DIRECT[2] = uint.Parse(this.uDSConfigStepMotor.Rows[26].GetCellValue(nStep).ToString());
            InStep.LIMIT_SENSOR_DIRECT[2] = uint.Parse(this.uDSConfigStepMotor.Rows[27].GetCellValue(nStep).ToString());
            InStep.ENCODER_MULTIPLY_VALUE[2] = uint.Parse(this.uDSConfigStepMotor.Rows[28].GetCellValue(nStep).ToString());
            InStep.ENCODER_DIRECT[2] = uint.Parse(this.uDSConfigStepMotor.Rows[29].GetCellValue(nStep).ToString());
        }

        public Infragistics.Win.UltraWinDataSource.UltraDataSource DSConfigStepMotor
        {
            get { return this.uDSConfigStepMotor; }
            set { this.uDSConfigStepMotor = value; }
        }

        private Infragistics.Win.UltraWinDataSource.UltraDataSource uDSConfigStepMotor;

        public void Read_StepMotor_Parameter_WordFile()
        {

            this.uDSConfigStepMotor = new Infragistics.Win.UltraWinDataSource.UltraDataSource();

            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item0");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item2");

            this.uDSConfigStepMotor.Band.Columns.AddRange(new object[] { ultraDataColumn1, ultraDataColumn2, ultraDataColumn3, ultraDataColumn4 });

            this.m_ColumnNo = 0;
            string filePath = System.Windows.Forms.Application.StartupPath + @"\EziSTEP_ParamDefault.ini";
            //string filePath = @"C:\Program Files (x86)\EzSoftware RM\EzSoftware\MotionDefault.mot";
            string strLine = "";
            System.IO.StreamReader srFile = new System.IO.StreamReader(filePath);

            while ((strLine = srFile.ReadLine()) != null)
            {
                StringParsing(strLine);
            }
            srFile.Close();

            this.m_ParamCount = this.uDSConfigStepMotor.Rows.Count / m_ColumnNo;
        }

        int m_ParamCount = 0;
        public int StepMotorParamCount
        {
            get { return this.m_ParamCount; }
            set { this.m_ParamCount = value; }
        }

        int m_ColumnNo = 0;
        private void StringParsing(string strLine)
        {
            string readNo = "", readName = "", readMin = "", readMax = "", readData = "";
            int indexNo = 0, indexStr = 0, indexEnd = 0;

            if (strLine.Length < 12 || strLine.IndexOf('=') > 3 || strLine.IndexOf('=') < 0) return;

            indexNo = int.Parse(strLine.Substring(0, 2));
            readNo = indexNo.ToString("00");
            if (readNo == "00") this.m_ColumnNo++;
            indexStr = strLine.IndexOf('=');
            indexEnd = strLine.IndexOf('|');
            readName = strLine.Substring(indexStr + 1, indexEnd - indexStr - 1);
            indexStr = strLine.IndexOf('|', indexEnd + 1);
            readMin = strLine.Substring(indexEnd + 1, indexStr - indexEnd - 1);
            indexEnd = strLine.IndexOf('|', indexStr + 1);
            readMin = strLine.Substring(indexStr + 1, indexEnd - indexStr - 1);
            indexStr = strLine.IndexOf('|', indexEnd + 1);
            readMax = strLine.Substring(indexEnd + 1, indexStr - indexEnd - 1);
            indexEnd = strLine.IndexOf('|', indexStr + 1);
            int temp = strLine.Length - indexStr - 1;
            readData = strLine.Substring(indexStr + 1, strLine.Length - indexStr - 1);

            Infragistics.Win.UltraWinDataSource.UltraDataRow ultraDataRow = null;
            this.uDSConfigStepMotor.Rows.Add();
            ultraDataRow = this.uDSConfigStepMotor.Rows[this.uDSConfigStepMotor.Rows.Count - 1];
            ultraDataRow[0] = readName;
            ultraDataRow[1] = readMin;
            ultraDataRow[2] = readMax;
            ultraDataRow[3] = readData;
        }
    }
}
