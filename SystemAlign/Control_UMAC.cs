using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemAlign
{
    public class Umac_System
    {
        /*
        #define P_PC_ALIGN_VISION_CMD P351    			// PC VISION 보정 수동 명령 버퍼
        #define PC_ALIGN_VISION_CMD_Ready 0     		// 대기중
        #define PC_ALIGN_VISION_CMD_Processing 11       // 진행중
        #define PC_ALIGN_VISION_CMD_Error 99     		// 에러 상태
        #define PC_ALIGN_VISION_CMD_Stop 10     		// 수동 명령 진행 중지
        #define PC_ALIGN_VISION_CMD_Clear 12     		// 초기화
        #define PC_ALIGN_VISION_CMD_1_GpPos 1   		// Gripper 1 Grip 위치로(위의 GALIGN_1 명령에서 설정된 위치로 이동)
        #define PC_ALIGN_VISION_CMD_2_GpPos 2   		// Gripper 2 Grip 위치로(위의 GALIGN_2 명령에서 설정된 위치로 이동)
        #define PC_ALIGN_VISION_CMD_1_Grip 3      	    // Gripper 1 Grip
        #define PC_ALIGN_VISION_CMD_2_Grip 4      	    // Gripper 2 Grip
        #define PC_ALIGN_VISION_CMD_1_Cor 9       	    // VISION 신호 준 후, 500 msec 기다려, 데이터 받은 후 VISION값 기준 보정 진행
        #define PC_ALIGN_VISION_CMD_2_Cor 10       	    // VISION 신호 준 후, 500 msec 기다려, 데이터 받은 후 VISION값 기준 보정 진행
        #define PC_ALIGN_VISION_CMD_1_Ungp 7      	    // Gripper 1 UnGrip
        #define PC_ALIGN_VISION_CMD_2_Ungp 8      	    // Gripper 2 UnGrip
        #define PC_ALIGN_VISION_CMD_1_UCor 5      	    // VISION 신호 준 후, 500 msec 기다려, 데이터 받은 후 UMAC에서 보정 진행
        #define PC_ALIGN_VISION_CMD_2_UCor 6      	    // VISION 신호 준 후, 500 msec 기다려, 데이터 받은 후 UMAC에서 보정 진행
        */

        private string sAddress = "192.6.94.5";
        private UInt32 m_dwDevice;
        private Int32 m_bDriverOpen;

         private int iUmacInputQT = 8;
        private int iUmacOutputQT = 8;
        private List<bool> fLstUmacInput = new List<bool>();
        private List<bool> fLstUmacOutput = new List<bool>();
        private int iUmacAxisQT = 8;
        public string IPAddress
        {
            get { return sAddress; }
            set { sAddress = value; }
        }
        public int AxisQuentiry
        {
            get { return iUmacAxisQT; }
            set { iUmacAxisQT = value; }
        }

        public List<bool> InputStatus
        {
            get { return fLstUmacInput; }
            set { fLstUmacInput = value; }
        }

         public List<bool> OutputStatus
        {
            get { return fLstUmacOutput; }
            set { fLstUmacOutput = value; }
        }

        public int InputQuentiry
        {
            get { return iUmacInputQT; }
            set { iUmacInputQT = value; }
        }

         public int OutputQuentiry
        {
            get { return iUmacOutputQT; }
            set { iUmacOutputQT = value; }
        }

        public UInt32 DeviceNo
        {
            get { return m_dwDevice; }
            set { m_dwDevice = value; }
        }

        public Int32 DeviceOpen
        {
            get { return m_bDriverOpen; }
            set { m_bDriverOpen = value; }
        }

        public List<UmacAxisPosition> axisPositions;
        public List<UmacAxisStatus> axisStatus;

        public UmacAxisPosition axisPos;
        public UmacAxisStatus axisSta;
        public struct UmacAxisPosition
        {
            public int AxisNo;
            public double Command;
            public double Actual;
            public double Velicity;
        }

        public struct UmacAxisStatus
        {
            public int AxisNo;
            public bool AMP;
            public string HFLG;
            public string PLIM;
            public string MLIM;
            public string FAUL;
        }
    }


    public class Control_UMAC
    {
        public event MyEventOneCntUmac UmacCntEvent1;

        public Umac_System umacSystem;
       
        public Control_UMAC()
        {
            umacSystem = new Umac_System();
            umacSystem.DeviceNo = 0;
        }

        
        public string Umac_Open()
        {
            //umacSystem.DeviceNo = PMAC.PmacSelect(0);
            string strResponse = string.Empty;
            if (umacSystem.DeviceNo >= 0 && umacSystem.DeviceNo < 255)
            {
                umacSystem.DeviceOpen = PMAC.OpenPmacDevice(umacSystem.DeviceNo);

                Byte[] byCommand;
                Byte[] byResponse;

                byCommand = new Byte[255];
                byResponse = new Byte[255];
                byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("&1B11S");
                UmacCntEvent1("&1B11S", 1);
                
                PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
                strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
                UmacCntEvent1(strResponse, 2);
            }
            return strResponse;
        }


        public int UMAC_Connection()
        {
            umacSystem.DeviceNo = PMAC.PmacSelect(0);

            if (umacSystem.DeviceNo >= 0 && umacSystem.DeviceNo < 255)
            {
                umacSystem.DeviceOpen = PMAC.OpenPmacDevice(umacSystem.DeviceNo);
                //timerStatus.Enabled = true;

                Byte[] byCommand;
                Byte[] byResponse;

                byCommand = new Byte[255];
                byResponse = new Byte[255];
                byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("&1B11S");
                PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            }

            return umacSystem.DeviceOpen;
        }
        public int Umac_Connect()
        {
            if (umacSystem.DeviceNo >= 0 && umacSystem.DeviceNo < 10)
            {
                umacSystem.DeviceOpen = PMAC.OpenPmacDevice ( umacSystem.DeviceNo );
            }
            return umacSystem.DeviceOpen;
        }

        public string Umac_SetData_StringOne(int setAdd, int setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            String strValue = String.Format("P{0:d}={1:d}", setAdd, setData);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_SetData_StringOne(int setAdd, double setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            String strValue = String.Format("P{0:d}={1:F8}", setAdd, setData);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_SetData_StringOne(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(setData);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_GetData_StringOne(string getAddress)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strResponse;
            
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(getAddress);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_GetData_StringOne(int getAdd)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strAddress = string.Format("P{0:d},1", getAdd);

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strAddress);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            
            string strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            string[] strArray = strResponse.ToString().Split('\r');
            return strArray[0];
        }

        public string[] Umac_GetData_StringArray(string getAddress)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strResponse;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(getAddress);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            return strResponse.ToString().Split('\r');
        }

        public string Umac_GetData_P351()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P351,1");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);

            string strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            string[] strArray = strResponse.ToString().Split('\r');
            return strArray[0];
        }

        public string Umac_SetData_P351(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(setData);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }


        public string Umac_GetData_P5101()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P5102,1");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);

            string strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            string[] strArray = strResponse.ToString().Split('\r');
            return strArray[0];
        }

        public string Umac_SetData_P5102(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P5102=" + setData);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_SetData_P5101(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P5101=" + setData);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_SetData_M6924(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M6924=" + setData);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_SetData_M6923(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M6923=" + setData);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string Umac_SetData_P3702(string setData)
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P3702="+setData+"P3701=1");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            return System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
        }

        public string[] Umac_GetData_CellData()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strResponse;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P381,10");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            return strResponse.ToString().Split('\r');
        }

        public string[] Umac_GetData_CurrentData()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strResponse;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P371,10");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            return strResponse.ToString().Split('\r');
        }

        public void Eventing_Send(string sendData, int typData)
        {
            UmacCntEvent1(sendData, typData);
        }


        /// <summary>
        /// 유멕 시스템의 입력, 출력 신호 정보를 확인한다.
        /// </summary>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_InputOutput_Check()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            Byte[] byValue = new Byte[255];
            String[] strArray = new String[41];
            String strValue;
            String strResponse;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M0,20");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');

            for (int i = 0; i < umacSystem.InputQuentiry; i++)
            {
                umacSystem.InputStatus.Add(strArray[i].CompareTo("0") != 0 ? true : false);
            }

            for (int i = 0; i < umacSystem.OutputQuentiry; i++)
            {
                umacSystem.OutputStatus.Add(strArray[i+10].CompareTo("0") != 0 ? true : false);
            }
        }

        /// <summary>
        /// 유멕 시스템의 Axis 현재 위치 정보를 확인한다.
        /// </summary>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_AxisPosition_Check()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            Byte[] byValue = new Byte[255];
            String[] strArray = new String[41];
            String strValue;
            String strResponse;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P111,80");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');
            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[0]));

            umacSystem.axisPositions.Clear();

            for (int i = 0; i < umacSystem.AxisQuentiry; i++)
            {
                umacSystem.axisPos.AxisNo = i + 1;
                umacSystem.axisPos.Command = Convert.ToDouble(strArray[(i * 10) + 0]);
                umacSystem.axisPos.Actual = Convert.ToDouble(strArray[(i * 10) + 1]);
                umacSystem.axisPos.Velicity = Convert.ToDouble(strArray[(i * 10) + 2]);
                umacSystem.axisPositions.Add(umacSystem.axisPos);
            }
        }

        /// <summary>
        /// 유멕 시스템의 Axis 상태를 확인한다.
        /// </summary>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_AxisStatus_Check()
        {
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            Byte[] byValue = new Byte[255];
            String[] strArray = new String[41];
            String strValue;
            String strResponse;
            string tmpCommand = string.Empty;

            umacSystem.axisStatus.Clear();

            for (int i = 0; i < umacSystem.AxisQuentiry; i++)
            {
                tmpCommand = "M" + (i + 1).ToString() + "20,20";
                //byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M120,20");
                byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(tmpCommand);
                PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
                strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
                strArray = strResponse.ToString().Split('\r');

                umacSystem.axisSta.AxisNo = i + 1;
                umacSystem.axisSta.AMP = Convert.ToInt32(strArray[19]) == 1 ? true : false;
                umacSystem.axisSta.HFLG = strArray[0].ToString();
                umacSystem.axisSta.PLIM = strArray[1].ToString();
                umacSystem.axisSta.MLIM = strArray[2].ToString();
                umacSystem.axisSta.MLIM = strArray[3].ToString();

                umacSystem.axisStatus.Add(umacSystem.axisSta);
            }
        }

        /// <summary>
        /// 유멕 시스템의 Axis를 홈으로 이동시킨다.
        /// </summary>
        /// <param name="axisNo"> UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Home(int axisNo)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("{0:d}HM", axisNo);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 조그 이동을 진행한다.
        /// </summary>
        /// <param name="axisNo">  UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <param name="ampSet">  UMAC 시스템의 Amplifer 설정 값 - 예 : 0 or 1 </param>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_Amplifer(int axisNo, int ampSet)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M{0:d}39={1:d}", axisNo, ampSet);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 조그 이동을 진행한다.
        /// </summary>
        /// <param name="axisNo"> UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Jog_Front(int axisNo)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=100 #{1:d}J-", axisNo, axisNo);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 조그 이동을 진행한다.
        /// </summary>
        /// <param name="axisNo"> UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Jog_Rear(int axisNo)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=100 #{1:d}J+", axisNo, axisNo);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 조그 이동을 진행한다.
        /// </summary>
        /// <param name="axisNo"> UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Jog_Left(int axisNo)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=200 #{1:d}J-", axisNo, axisNo);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 조그 이동을 진행한다.
        /// </summary>
        /// <param name="axisNo"> UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Jog_Right(int axisNo)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=200 #{1:d}J+", axisNo, axisNo);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 조그 이동을 중지한다.
        /// </summary>
        /// <param name="axisNo"> UMAC 시스템의 Axis 번호 - 예 : 1~8 </param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Jog_Stop(int axisNo)
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("#{0:d}J/", axisNo);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 출력 신호값을 설정한다.
        /// </summary>
        /// <param name="outputNo"> UMAC 시스템의 Output 인터페이스 번호 예 : 1~8 </param>
        /// <param name="outputSet">UMAC 시스템의 Output 인터페이스 설정 예 : 0 or 1</param>
        /// <returns>
        /// 반환값 없음
        /// </returns>
        /// <exception cref="System.Exception">
        /// 이벤트 유형이 오류이면 Exception발생
        /// </exception> 
        public void Umac_Output_Set(int outputNo, int outputSet)
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M{0:d}={1:d}", outputNo, outputSet);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 Axis를 이동한다.
        /// </summary>
        /// <param name="moveSet"> UMAC 시스템의 Axis를 이동할 Axis 번호? </param>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_Axis_Move01(int moveSet)
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=21");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 Axis를 이동한다.
        /// </summary>
        /// <param name="moveSet"> UMAC 시스템의 Axis를 이동할 Axis 번호? </param>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_Axis_Move02(int moveSet)
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=22");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 Axis를 이동한다.
        /// </summary>
        /// <param name="moveSet"> UMAC 시스템의 Axis를 이동할 Axis 번호? </param>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_Axis_Move03(int moveSet)
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=23");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 Axis를 이동한다.
        /// </summary>
        /// <param name="moveSet"> UMAC 시스템의 Axis를 이동할 Axis 번호? </param>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_Axis_Move04(int moveSet)
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=24");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
        }

        /// <summary>
        /// 유멕 시스템의 파일을 다운로드한다.
        /// </summary>
        /// <param name="fileName"> 다운로드할 파일의 이름이다.? </param>
        /// <returns> 반환값 없음 </returns>
        /// FileDlg.Filter = "PMAC Files (*.pmc)|*.pmc|All Files (*.*)|*.*||";
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_File_DownLoad(string fileName)
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("&1A");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            System.Threading.Thread.Sleep(10);

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(fileName);
            if (PMAC.PmacDownloadA(umacSystem.DeviceNo, new PMAC.DOWNLOADMSGPROC(DownloadMsgProc), null, new PMAC.DOWNLOADPROGRESS(DownloadProgress), byCommand, 1, 1, 1, 1) == 0)
            {
                MessageBox.Show("다운로드 에러입니다.", "에러");
                return;
            }

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("Enable PLC1,10,11");
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            runEnable = true;
            stopEnable = false;
            progressEnable = false;
        }


        public static void DownloadMsgProc(String str, Int32 newlie)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }

        public static bool runEnable = false;
        public static bool stopEnable = false;
        public static Int32 progressValue = 0;
        public static bool progressEnable = false;
        public static void DownloadProgress(Int32 nPercent)
        {
            progressValue = nPercent;

            if (nPercent >= 100) progressEnable = true;
        }


        /// <summary>
        /// 현재 로딩되어 있는 실행파일 구동을 시작한다.
        /// </summary>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_File_Run()
        {
            if (umacSystem.DeviceOpen == 0)
                return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("&1B11R");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);

            runEnable = false;
            stopEnable = true;
        }

        /// <summary>
        /// 현재 로딩되어 있는 실행파일 구동을 정지한다.
        /// </summary>
        /// <returns> 반환값 없음 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public void Umac_File_Stop()
        {
            if (umacSystem.DeviceOpen == 0) return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("&1B11S");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);

            runEnable = true;
            stopEnable = false;
        }

        /// <summary>
        /// 유멕 시스템과 통신을 진행한다.
        /// </summary>
        /// <param name="sendStr"> 유멕 시스템에게 전송할 컴멘드이다. </param>
        /// <returns> 유멕 시스템에서 컴멘드를 처리한 결과 값 </returns>
        /// <exception cref="System.Exception"> 이벤트 유형이 오류이면 Exception발생 </exception> 
        public string Umac_Communicate_Command(string sendStr)
        {
            string strResponse = string.Empty;

            if (umacSystem.DeviceOpen == 0) return strResponse;

           
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(sendStr);
            PMAC.PmacGetResponseA(umacSystem.DeviceNo, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);

            return strResponse;
        }
    }
}
