using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace SystemAlign
{

// ReSharper disable once InconsistentNaming
    public class Control_PLC
    {

        //프로그램 기동시 True로 설정하고
        //프로그램 종료시 False로 설정해서
        //쓰레드를 종료시킨다.
        private bool PLC_Check_Loop_Flag = false;

        public Control_PLC()
        {
        }
       
        public Melsec melsec;/// = new Melsec();

        float fValue = 0.0f;
        int lValue = 0;
        uint dwValue = 0;
        ushort[] wValue = { 0, 0 };
        short[] sValue = { 0, 0 };
        byte[] byValue = { 0, 0, 0, 0 };
        sbyte[] chValue = { 0, 0, 0, 0 };
        BitArray bi = new BitArray(32, false);


        public bool Loop_Check_Flag
        {
            get { return PLC_Check_Loop_Flag; }
            set { PLC_Check_Loop_Flag = value; }
        }

        public Thread ConnectCheck_threading;
        public void PLC_Connecting_Check()
        {
            PLC_Check_Loop_Flag = true;
            ConnectCheck_threading = new Thread(Connect_Check_Threading);
            ConnectCheck_threading.Start();
        }

        public void Connect_Check_Threading()
        {
            int togleValue = 1;
            while (PLC_Check_Loop_Flag == true)
            {
                if (togleValue == 1) togleValue = 2;
                else if (togleValue == 2) togleValue = 1;

                PCL_WriteData_D3102(togleValue);

                Thread.Sleep(1900);
            }
        }

        public int PCL_WriteData_D4044(int togleValue)
        {
            int AlramDataLehgth = 10;
            byte[] byteData = new byte[4];
            if (togleValue == 0)
                byteData[0] = 48;
            else if (togleValue == 1)
                byteData[0] = 49;

            byteData[1] = 0;
            byteData[2] = 0;
            byteData[3] = 0;

            int wd = BitConverter.ToInt32(byteData, 0);
            int[] writeData = { wd };

            int isWrite = melsec.PLC_WriteData("D4044", 1, writeData);
            if (isWrite != 0) return -1;
            return 0;
        }

        public int PCL_WriteData_D3102(int togleValue)
        {
            int AlramDataLehgth = 10;
            byte[] byteData = new byte[4];
            if (togleValue == 1)
                byteData[0] = 48;
            else if (togleValue == 2)
                byteData[0] = 49;

            byteData[1] = 0;
            byteData[2] = 0;
            byteData[3] = 0;

            int wd = BitConverter.ToInt32(byteData, 0);
            int[] writeData = { wd };

            int isWrite = melsec.PLC_WriteData("D3102", 1, writeData);
            if (isWrite != 0) return -1;
            return 0;
        }

        public void PCL_WriteData_D3101(int RunStopValue)
        {
            int[] writeData = { RunStopValue };

            melsec.PLC_WriteData("D3101", 1, writeData);
        }

        public void PCL_WriteData_D4050(string add, int resultValue)
        {
            int[] writeData = { resultValue };

            melsec.PLC_WriteData(add, 1, writeData);
            //return 0;
        }

        public void PCL_WriteData_D4080(string add, int resultValue)
        {
            int[] writeData = { resultValue };

            melsec.PLC_WriteData(add, 1, writeData);
            //return 0;
        }
        
        public int PCL_WriteData_D4050(byte[] resultValue)
        {
            int wd1 = BitConverter.ToInt32(resultValue, 4);
            int wd2 = BitConverter.ToInt32(resultValue, 0);
            int[] writeData = { wd1, wd2 };

            melsec.PLC_WriteData("D4050", 2, writeData);
            return 0;
        }

        public int PCL_WriteData_D4080(byte[] resultValue)
        {
            int wd1 = BitConverter.ToInt32(resultValue, 4);
            int wd2 = BitConverter.ToInt32(resultValue, 0);
            int[] writeData = { wd1, wd2 };

            melsec.PLC_WriteData("D4080", 2, writeData);
            return 0;
        }

        /*
        public int PCL_WriteData_D4050(byte[] resultValue)
        {

            byte[] byteData = new byte[4];
            if (resultValue == 0)
                byteData[0] = 48;
            else if (resultValue == 1)
                byteData[0] = 49;
            else if (resultValue == 2)
                byteData[0] = 50;

            byteData[1] = 0;
            byteData[2] = 0;
            byteData[3] = 0;

            int wd = BitConverter.ToInt32(resultValue, 0);
            int[] writeData = {wd1, wd2};

            melsec.PLC_WriteData("D4050", 2, writeData);
            return 0;
        }
        */

        public string TypeMaxValue()
        {
            string valueData = "";
            valueData += "float : " + float.MinValue.ToString() + "\t\t to \t" + float.MaxValue.ToString() + "\r\n";
            valueData += "int   : " + int.MinValue.ToString() + "\t\t to \t" + int.MaxValue.ToString() + "\r\n";
            valueData += "uint  : " + uint.MinValue.ToString() + "\t\t to \t" + uint.MaxValue.ToString() + "\r\n";
            valueData += "ushort : " + ushort.MinValue.ToString() + "\t\t to \t" + ushort.MaxValue.ToString() + "\r\n";
            valueData += "byte : " + byte.MinValue.ToString() + "\t\t to \t" + byte.MaxValue.ToString() + "\r\n";
            valueData += "sbyte : " + sbyte.MinValue.ToString() + "\t\t to \t" + sbyte.MaxValue.ToString() + "\r\n";
            return valueData;
        }

        char[] Read_BarCode = new char[10];
        int Read_DataTrigger = 0;
        int Read_WelderPoint = 0;
        int Read_TogglePoint = 0;
        int Read_WelderStatus = 0;
        int Read_SideMode = 0;
        private int Read_AutoRun = 0;

        public int NowAutoRun
        {
            get { return this.Read_AutoRun; }
            set { this.Read_AutoRun = value; }
        }

        public char[] BarCode
        {
            get { return this.Read_BarCode; }
            set {this.Read_BarCode = value;}
        }

        public int DataTrigger
        {
            get { return this.Read_DataTrigger; }
            set { this.Read_DataTrigger = value; }
        }

        public int WelderPoint
        {
            get { return this.Read_WelderPoint; }
            set { this.Read_WelderPoint = value; }
        }
        public int TogglePoint
        {
            get { return this.Read_TogglePoint; }
            set { this.Read_TogglePoint = value; }
        }
        public int WelderStatus
        {
            get { return this.Read_WelderStatus; }
            set { this.Read_WelderStatus = value; }
        }

        public int SideType
        {
            get { return this.Read_SideMode; }
            set { this.Read_SideMode = value; }
        }

        int All_Reset = 0;
        public int AllReset
        {
            get { return this.All_Reset; }
            set { this.All_Reset = value; }
        }

        int Cell_TotalNumber = 0;
        public int CellTotalNumber
        {
            get { return this.Cell_TotalNumber; }
            set { this.Cell_TotalNumber = value; }
        }

        int Cell_TotalPointNumber = 0;
        public int CellTotalPointNumber
        {
            get { return this.Cell_TotalPointNumber; }
            set { this.Cell_TotalPointNumber = value; }
        }


        int Now_AutoFlow = 0;
        public int NowAutoFlow
        {
            get { return this.Now_AutoFlow; }
            set { this.Now_AutoFlow = value; }
        }

        int Horn_TypeSelect = 0;
        public int HornTypeSelect
        {
            get { return this.Horn_TypeSelect; }
            set { this.Horn_TypeSelect = value; }
        }

        int Horn_Counter1 = 0;
        public int HornCounter1
        {
            get { return this.Horn_Counter1; }
            set { this.Horn_Counter1 = value; }
        }

        int Horn_Counter2 = 0;
        public int HornCounter2
        {
            get { return this.Horn_Counter2; }
            set { this.Horn_Counter2 = value; }
        }

        int Envil_Counter1 = 0;
        public int EnvilCounter1
        {
            get { return this.Envil_Counter1; }
            set { this.Envil_Counter1 = value; }
        }

        int Envil_Counter2 = 0;
        public int EnvilCounter2
        {
            get { return this.Envil_Counter2; }
            set { this.Envil_Counter2 = value; }
        }

        int Envil_Counter32 = 0;
        public int EnvilCounter32
        {
            get { return this.Envil_Counter32; }
            set { this.Envil_Counter32 = value; }
        }

        int Horn_Counter32 = 0;
        public int HornCounter32
        {
            get { return this.Horn_Counter32; }
            set { this.Horn_Counter32 = value; }
        }

        public string GetBarCode
        {
            get 
            { 
                string sBarcode = new string(this.BarCode);
                return sBarcode;
            }
        }

        private int sD6000Set = 6000;
        public string PLC_Make_D6000(int iAddress)
        {
            return "D" + (this.sD6000Set + iAddress).ToString();
        }

        private int iNormarPointQuentity = 21;
        public int NormarPointQuentity
        {
            get { return this.iNormarPointQuentity; }
            set { this.iNormarPointQuentity = value; }
        }

        private int iAllSeqPoint = 0;
        public int PLC_All_WelderPoint(int iSeqNo)
        {
            if (Read_SideMode == 3)
                return Read_TogglePoint + iSeqNo;
            return iSeqNo;
        }
        public int PLC_All_WelderPoint()
        {
            if (this.WelderPoint == 0)
            {
                return 0;
            }
            if (Read_SideMode == 3)
                return Read_TogglePoint + this.WelderPoint;
            return this.WelderPoint;
        }

        private int iNowCapture = 0;
        public int NowCapture
        {
            get { return this.iNowCapture; }
            set { this.iNowCapture = value; }
        }

        private int _iModelNameLength = 26;
        private int _iRecipeNumberLength = 10;

        private int[] _intArrayReadToPLC;
        private byte[,] _byteArrayReadToPLC;
        /*
        public string[] PCL_ReadData_ModelName()
        {
            int _iModelNameLength = 200;
        int _iRecipeNumberLength = 200;
            string decodingModelName = string.Empty;
            string decodingRecipeNumber = string.Empty;
            string[] strReadData = { string.Empty, string.Empty };

            _intArrayReadToPLC = new int[_iModelNameLength];
            _byteArrayReadToPLC = new byte[_iModelNameLength, 4];

            int isRead = melsec.PLC_ReadData("D11000", _iModelNameLength, _intArrayReadToPLC);
            if (isRead != 0) return strReadData;

            for (int i = 0; i < _iModelNameLength; i++)
            {
                //아스키값이 들어온다.
                byte[] intToByte = BitConverter.GetBytes(_intArrayReadToPLC[i]);
                decodingModelName += ASCIIEncoding.ASCII.GetString(intToByte, 0, 1);
            }
            strReadData[0] = decodingModelName;

            _intArrayReadToPLC = new int[_iRecipeNumberLength];
            _byteArrayReadToPLC = new byte[_iRecipeNumberLength, 4];
            isRead = melsec.PLC_ReadData("D11200", _iRecipeNumberLength, _intArrayReadToPLC);

            if (isRead != 0) return strReadData;

            for (int i = 0; i < _iRecipeNumberLength; i++)
            {
                //바이트에 숫자로 넘어온다.
                byte[] intToByte = BitConverter.GetBytes(_intArrayReadToPLC[i]);
                //decodingRecipeNumber += ASCIIEncoding.ASCII.GetString(intToByte, 0, 1);
                decodingRecipeNumber += intToByte[0].ToString();
            }
            strReadData[1] = decodingRecipeNumber;

            return strReadData;
        }
        */

        public string[] PCL_ReadData_ModelName()
        {
            string[] modelInfo = new string[2];
            modelInfo[0] = PCL_ReadData_D11000();
            modelInfo[1] = PCL_ReadData_D11200();

            return modelInfo;
        }

        public string PCL_ReadData_D11000()
        {
            int m_iPlc_Read_D11000_Que = 200;
            int[] readToPlcIntArray = new int[m_iPlc_Read_D11000_Que];
            string decodingReadData = string.Empty;

            int isRead = melsec.PLC_ReadData("D11000", m_iPlc_Read_D11000_Que, readToPlcIntArray);
            if (isRead != 0) return decodingReadData;

            /////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < readToPlcIntArray.Length; i++)
            {
                //아스키값이 들어온다.
                byte[] intToByte = BitConverter.GetBytes(readToPlcIntArray[i]);
                for (int j = 0; j < intToByte.Length; j++)
                {
                    if (intToByte[j] != 0)
                        decodingReadData += ASCIIEncoding.ASCII.GetString(intToByte, j, 1);
                }
            }
            /////////////////////////////////////////////////////////////////////////////////

            return decodingReadData;
        }


        public string PCL_ReadData_D11200()
        {
            int m_iPlc_Read_D11200_Que = 200;
            int[] readToPlcIntArray = new int[m_iPlc_Read_D11200_Que];
            string decodingReadData = string.Empty;

            int isRead = melsec.PLC_ReadData("D12000", m_iPlc_Read_D11200_Que, readToPlcIntArray);
            if (isRead != 0) return decodingReadData;

            /////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < readToPlcIntArray.Length; i++)
            {
                //아스키값이 들어온다.
                byte[] intToByte = BitConverter.GetBytes(readToPlcIntArray[i]);
                for (int j = 0; j < intToByte.Length; j++)
                {
                    if (intToByte[j] != 0)
                        decodingReadData += ASCIIEncoding.ASCII.GetString(intToByte, j, 1);
                }
            }
            /////////////////////////////////////////////////////////////////////////////////

            return decodingReadData;
        }


        public string PCL_ReadData_D12000()
        {
            int m_iPlc_Read_D12000_Que = 10;
            int[] readToPlcIntArray = new int[m_iPlc_Read_D12000_Que];
            string decodingModelName = string.Empty;

            int isRead = melsec.PLC_ReadData("D12000", m_iPlc_Read_D12000_Que, readToPlcIntArray);
            if (isRead != 0) return decodingModelName;

            /////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < readToPlcIntArray.Length; i++)
            {
                //아스키값이 들어온다.
                byte[] intToByte = BitConverter.GetBytes(readToPlcIntArray[i]);
                for (int j = 0; j < intToByte.Length; j++)
                {
                    if (intToByte[j] != 0)
                        decodingModelName += ASCIIEncoding.ASCII.GetString(intToByte, j, 1);
                }
            }
            /////////////////////////////////////////////////////////////////////////////////

            return decodingModelName;
        }

        public int PCL_WriteData_D12000()
        {
            //byte[] byteData = {75, 76, 0, 0};
            //if(BitConverter.IsLittleEndian)
            //    Array.Reverse(byteData);
            //int i = BitConverter.ToInt32(byteData, 0);

            int AlramDataLehgth = 10;
            byte[] byteData = new byte[4];
            byteData[0] = 49;
            byteData[1] = 0;
            byteData[2] = 0;
            byteData[3] = 0;

            int wd = BitConverter.ToInt32(byteData, 0);
            int[] writeData = { wd };

            int isWrite = melsec.PLC_WriteData("D12000", 1, writeData);
            if (isWrite != 0) return -1;
            return 0;
        }


        private int PCL_WriteData_D10000()
        {
            //byte[] byteData = {75, 76, 0, 0};
            //if(BitConverter.IsLittleEndian)
            //    Array.Reverse(byteData);
            //int i = BitConverter.ToInt32(byteData, 0);

            int AlramDataLehgth = 10;
            byte[] byteData = new byte[4];
            byteData[0] = 90;
            byteData[1] = 89;
            byteData[2] = 0;
            byteData[3] = 0;

            int wd = BitConverter.ToInt32(byteData, 0);
            int[] writeData = {wd};

            int isWrite = melsec.PLC_WriteData("D10000", 1, writeData);
            if (isWrite != 0) return -1;
            return 0;
        }



        private int PCL_WriteData_D6000(int iWriteValue, int iWriteAddress)
        {
            string sSendData = PLC_Make_D6000(iWriteAddress);
            int[] writeData = { iWriteValue };
            int isWrite = melsec.PLC_WriteData(sSendData, 1, writeData);
            if (isWrite != 0) return -1;
            return 0;
        }


        private int m_iPLC_Add_NowAutoRun = 13;
        private int m_iPLC_Add_AllCellNo = 17;
        private int m_iPLC_Add_AllCellPointNo = 18;
        private int m_iPLC_Add_DataTrigger = 10;
        private int m_iPLC_Add_EnvilCounter1 = 28;
        private int m_iPLC_Add_EnvilCounter2 = 29;
        private int m_iPLC_Add_HornCounter1 = 26;
        private int m_iPLC_Add_HornCounter2 = 27;
        private int m_iPLC_Add_HornSelect = 25;
        private int m_iPLC_Add_NowAutoFlow = 14;
        private int m_iPLC_Add_NowWelStatus = 11;
        private int m_iPLC_Add_NowWelderPos = 15;
        private int m_iPLC_Add_ReadStart = 6000;
        private int m_iPLC_Add_Reset = 12;
        private int m_iPLC_Add_SideType = 16;
        private int m_iPLC_Add_TogglePos = 19;

        private int[] m_iPlc_Read_W500_IntArray;
        byte[,] m_byPlc_Read_W500_ByteArray;
        int m_iPlc_Read_W500_Que = 20;
        public int PCL_ReadData_M500()
        {
            m_iPlc_Read_W500_IntArray = new int[m_iPlc_Read_W500_Que];
            m_byPlc_Read_W500_ByteArray = new byte[m_iPlc_Read_W500_Que, 4];

            int isRead = melsec.PLC_ReadData("D500", m_iPlc_Read_W500_Que, m_iPlc_Read_W500_IntArray);
            if (isRead != 0) return -1;

            //Data Trigger를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iNowD506 = int.Parse(m_iPlc_Read_W500_IntArray[6].ToString());
            NowCapture = iNowD506;
            /////////////////////////////////////////////////////////////////////////////////

            return 0;
        }

        private string BarcodeData_Edit(char[] chBarCode)
        {
            string sBarString = "";
            for (int i = 0; i < chBarCode.Length; i++)
            {
                if (chBarCode[i] != 0x00)
                {
                    sBarString += chBarCode[i].ToString();
                }
            }
            return sBarString;
        }

        int m_iPlc_Read_Quentiry= 30;
        private int[] m_iPlc_Read_IntArray;
        private byte[,] m_byPlc_Read_ByteArray;
        private int PCL_ReadData_D6000()
        {
            m_iPlc_Read_IntArray = new int[m_iPlc_Read_Quentiry];
            m_byPlc_Read_ByteArray = new byte[m_iPlc_Read_Quentiry, 4];

            int isRead = melsec.PLC_ReadData("D6000", m_iPlc_Read_Quentiry, m_iPlc_Read_IntArray);
            if (isRead != 0) return -1;

            for (int i = 0; i < m_iPlc_Read_Quentiry; i++)
            {
                byte[] intTobyte = BitConverter.GetBytes(m_iPlc_Read_IntArray[i]);
                m_byPlc_Read_ByteArray[i, 0] = intTobyte[0];
                m_byPlc_Read_ByteArray[i, 1] = intTobyte[1];
                m_byPlc_Read_ByteArray[i, 2] = intTobyte[2];
                m_byPlc_Read_ByteArray[i, 3] = intTobyte[3];
            }

            byte[] IntToByte;

            //바코드 번호를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            var byBarCode = new byte[40];
            for (int i = 0; i < 9; i++)
            {
                IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[i]);
                int add1 = i * 4;
                byBarCode[add1] = IntToByte[0];
                byBarCode[add1 + 1] = IntToByte[1];
                byBarCode[add1 + 2] = IntToByte[2];
                byBarCode[add1 + 3] = IntToByte[3];
            }
            char[] chBarCode = Encoding.ASCII.GetString(byBarCode).ToCharArray();
           
            string strBarCode = BarcodeData_Edit(chBarCode);
            BarCode = chBarCode;


            //Data Trigger를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iNowTrigger = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_DataTrigger].ToString());
            DataTrigger = iNowTrigger;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_DataTrigger]);
            /////////////////////////////////////////////////////////////////////////////////


            //현재 용접상태(용접시작1/용접시작확인2/용접완료0)를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////            
            int iNowStatus = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_NowWelStatus].ToString());
            WelderStatus = iNowStatus;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_NowWelStatus]);
            /////////////////////////////////////////////////////////////////////////////////

            //초기화 진행 정보를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iReset = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_Reset].ToString());
            AllReset = iReset;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_Reset]);
            /////////////////////////////////////////////////////////////////////////////////

            //현재 오토 플로우를 읽어와서 저장한다. 이 값이 6이 되면 카메라로 캡처를 진행한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iNowFlow = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_NowAutoFlow].ToString());
            NowAutoFlow = iNowFlow;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_NowAutoFlow]);
            /////////////////////////////////////////////////////////////////////////////////


            //현재 오토 런인지 읽어와서 저장한다. 이 값이 0이면 진행하지 않는다.
            /////////////////////////////////////////////////////////////////////////////////
            int iNowAutoRun = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_NowAutoRun].ToString());
            NowAutoRun = iNowAutoRun;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_NowAutoRun]);
            /////////////////////////////////////////////////////////////////////////////////


            //현재 용접 포인트를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iNowWelder = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_NowWelderPos].ToString());
            WelderPoint = iNowWelder;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_NowWelderPos]);
            /////////////////////////////////////////////////////////////////////////////////

            //작업모듈(단방향1/단방향 반전2/양방향3)를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iNowSideMode = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_SideType].ToString());
            SideType = iNowSideMode;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_SideType]);
            /////////////////////////////////////////////////////////////////////////////////

            //모든 셀 수량을 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iCellAllNo = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_AllCellNo].ToString());
            CellTotalNumber = iCellAllNo;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_AllCellNo]);
            /////////////////////////////////////////////////////////////////////////////////

            //모든 셀  포인트 수량을 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iCellAllPointNo = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_AllCellPointNo].ToString());
            CellTotalPointNumber = iCellAllPointNo;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_AllCellPointNo]);
            /////////////////////////////////////////////////////////////////////////////////

            //반전 시작 포인트를 읽어와서 저장한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iTogglePos = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_TogglePos].ToString());
            TogglePoint = iTogglePos;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_TogglePos]);
            /////////////////////////////////////////////////////////////////////////////////

            //혼의 종류를 선택한다.
            /////////////////////////////////////////////////////////////////////////////////
            int iHornSelect = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_HornSelect].ToString());
            HornTypeSelect = iHornSelect;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_HornSelect]);
            /////////////////////////////////////////////////////////////////////////////////

            //혼의 사용횟수의 앞부분 저장소
            /////////////////////////////////////////////////////////////////////////////////
            int iHornCount1 = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_HornCounter1].ToString());
            HornCounter1 = iHornCount1;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_HornCounter1]);
            var byHornCount = new byte[4];
            byHornCount[0] = IntToByte[0];
            byHornCount[1] = IntToByte[1];
            /////////////////////////////////////////////////////////////////////////////////

            //혼의 사용횟수의 뒷부분 저장소
            /////////////////////////////////////////////////////////////////////////////////
            int iHornCount2 = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_HornCounter2].ToString());
            HornCounter2 = iHornCount2;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_HornCounter2]);
            byHornCount[2] = IntToByte[0];
            byHornCount[3] = IntToByte[1];
            int iHornCount = BitConverter.ToInt32(byHornCount, 0);
            HornCounter32 = iHornCount;
            //MessageBox.Show(melsecData.HornCounter32.ToString());
            /////////////////////////////////////////////////////////////////////////////////

            //엔빌의 사용횟수의 앞부분 저장소
            /////////////////////////////////////////////////////////////////////////////////
            int iEnvilCount1 = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_EnvilCounter1].ToString());
            EnvilCounter1 = iEnvilCount1;
            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_EnvilCounter1]);
            var byEnvilCount = new byte[4];
            byEnvilCount[0] = IntToByte[0];
            byEnvilCount[1] = IntToByte[1];
            /////////////////////////////////////////////////////////////////////////////////

            //혼의 사용횟수의 뒷부분 저장소
            /////////////////////////////////////////////////////////////////////////////////
            int iEnvilCount2 = int.Parse(m_iPlc_Read_IntArray[m_iPLC_Add_EnvilCounter2].ToString());
            EnvilCounter2 = iEnvilCount2;

            IntToByte = BitConverter.GetBytes(m_iPlc_Read_IntArray[m_iPLC_Add_EnvilCounter2]);
            byEnvilCount[2] = IntToByte[0];
            byEnvilCount[3] = IntToByte[1];
            int iEnvilCount = BitConverter.ToInt32(byEnvilCount, 0);
            EnvilCounter32 = iEnvilCount;
            /////////////////////////////////////////////////////////////////////////////////

            return 0;
        }


    }

    /*
    //BYTE4 32bit Type
    public struct MelsecDataFormat32
    {
        //float
        public float fValue;

        //long  -2,147,483,648 to 2,147,483,647      
        public int lValue;       
        
        //DWORD 0 to 4,294,967,295
        public uint dwValue;          
      
        //WORD  0 to 65,535 : 2개
        public ushort[] wValue;   
        
        //short -32,768 to 32,767 : 2개
        public short[] sValue;
        
        //BYTE  0 to 255 : 4개
        public byte[] byValue;

        //char  -128 to 127 : 4개
        public sbyte[] chValue;

        //unsigned 32bit
        BitArray bi;// = new BitArray(32, false);
    }
    */

}
