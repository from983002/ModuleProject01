using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraMessageBox;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Matrox.MatroxImagingLibrary;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using OpenCvSharp;
using PerCederberg.Grammatica.Runtime;
using ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using Point = System.Drawing.Point;


namespace SystemAlign
{
    using DrawList = List<DrawObject>;

    //MyEventOne : 델리게이트 명.
    //PortInData : 이벤트 전송시 전달할 데이터.
    //델리게이트를 진행할 함수를 지정해 준다.
    public delegate void MyEventOneInsp(int OpNo);
    public delegate void MyEventTwoInsp();
    public delegate void MyEventMeaData1();
    public delegate void MyEventMeaData2();
    public delegate void MyEventMeaData3();
    public delegate void MyEventMeaData4();
    public delegate void MyEventOneInsp_Gap1(int GapNo, uint ProductNo);
    public delegate void MyEventOneInsp_Gap2(int GapNo, uint ProductNo);

    public delegate void MyEventInsp_Pix01(int Garo, int Sero);

    public delegate void MyEventOneInsp_BiCell(uint GapNo, uint ProductNo);
    //패스워드 수정에서 적용 시 발생하는 이벤트
    public delegate bool PasswordEditEvent1(string inputID, string inputPass);

    //가상 키보드에서 엔터를 클릭했을때 발생하는 이벤트
    //이 이벤트로 각 창의 적용 버튼을 클릭핸들로 이동한다.
    public delegate void VertualKeyboardEvent1();

    public delegate void UserSelectEvent1(string selectedAccount);

    public delegate bool LoginCompliteEvent1(string inputID, string inputPass);

    //모델 변경 참에서 모델을 추가했을 때 발생시키는 이벤트이다.
    //이때 현재 리스트 배열의 정보를 텍스트 파일에 저장한다.
    public delegate bool ModelRegEvent1(string modelName, string modelNumber);

    //모델 변경 창에서 모델을 변경 했을 때 발생시키는 이벤트이다.
    //이때 현재 리스트 배열의 정보를 텍스트 파일의 정보로 변경한다.
    public delegate bool ModelRegEvent2();

    //레시피 탭의 DrawArea의 객체에서 발생 시키는 이벤트
    //inspectZone 선택시 발생하는 이벤트임.
    public delegate void RecipeEvent2(int selectRect);

    //상부 Recipe inspectZone 선택시 발생하는 이벤트임.
    public delegate void RecipeEvent3(int selectRect);
    //하부 Recipe inspectZone 선택시 발생하는 이벤트임.
    public delegate void RecipeEvent4(int selectRect);

    //CControl_UMAC 클래스에서 데이터를 umac으로 전송한
    //데이터와 받은 데이터를 받는 이벤트이다.
    public delegate void MyEventOneCntUmac(string logData, int type);

    public partial class FormDlgMain : Form
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// 얼라인 메인폼을 운용하고 제어함.///////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region 얼라인 메인 폼

        /// 메임폼의 멤버스.

        //폼의 포커스를 얻기 위해서 시스템 API 함수를 사용한다.
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        //private const int WM_SHOWNOACTIVATE = 4;

        #region 메인폼 : 멤버

        //얼라인 환경 적용된 내용을 가지는 얼라인 시스템 환경 클래스
        //CInspection_Folding_Align AlignSystem = new CInspection_Folding_Align();
        CInspection_Lamination LamiSystem = new CInspection_Lamination();
        //메세지 박스 제어에 사용되는 리소스 제어 객체
        private ResourceManager rm = SystemAlign.Properties.Resources.ResourceManager;

        static readonly string Account_Operator = "OPERATOR";
        static readonly string Account_Engineer = "ENGINEER";
        static readonly string Account_Maker = "MAKER";
//         static readonly string Account_Operator = "OP";
//         static readonly string Account_Engineer = "ENG";
//         static readonly string Account_Maker = "MK";
        static readonly string Account_Password = "PASSWORD EDIT";
        //private string _strNowId = null;
        //private string _strNowPass = null;

        private FormDlgInsp dlgInspect = new FormDlgInsp();
        private FormDlgLogin dlgLogin;
        private FormDlgUserSelect dlgUserSelect;
        private FormDlgPassEdit dlgPasswordEdit;

        private bool _boolUserSelected_TabChange = false;
        #endregion

        /// 메임폼의 프로퍼티.

        #region 메인폼 : 프로퍼티


        public string NowUserAccount
        {
            get { return _strNowAccount; }
            set { _strNowAccount = value; }
        }

        public string NowUserId
        {
            get { return LamiSystem.GetSet_Now_User_ID; }
            set { LamiSystem.GetSet_Now_User_ID = value; }
        }

        public string NowUserPass
        {
            get { return LamiSystem.GetSet_Now_User_Pass; }
            set { LamiSystem.GetSet_Now_User_Pass = value; }
        }

        #endregion

        /// 메임폼의 생성자, 폼의 로딩함수, 초기화 함수.

        #region 메인폼 : 생성자, 초기화

        /// <summary>
        /// 메임폼의 생성자이다.
        /// </summary>
        public FormDlgMain()
        {
            InitializeComponent();
            _Uper_Control_DrawArea = new ControlDrawArea(RecipeGap_drawArea1, this);
            _Down_Control_DrawArea = new ControlDrawArea(RecipeGap_drawArea2, this);
           
            Custom_Event_Connect();
        }

        private Control_UMAC umac;
        private Control_PLC plc;
        private Control_PN2212 lvs;
        private void Custom_Event_Connect()
        {
            lvs = new Control_PN2212(sPort1);
            string[] portsNames = lvs.LVS_GetPort();

            if(portsNames.Length > 0)
                sPort1.PortName = portsNames[portsNames.Length - 1];

            //plc = new Control_PLC();
            //PLC_Initialize();
            //PLC_Connection();

            umac = new Control_UMAC();
            umac.UmacCntEvent1 += new MyEventOneCntUmac(umac_UmacCntEvent1);
        }

        /*
        public void PLC_Initialize()
        {
            //PC쪽 설정 내용 "192.168.0.25";
            this.m_MelsecTCP.ActSourceNetworkNumber = 4;
            this.m_MelsecTCP.ActSourceStationNumber = 2;

            //PLC쪽 설정 내용
            //this.m_MelsecUDP.ActCpuType = 146; //Q06UDEHCPU=34, Q06HCPU=35;(폴딩)
            //this.m_MelsecUDP.ActHostAddress = "192.168.172.233";
            this.m_MelsecTCP.ActCpuType = 34;
            this.m_MelsecTCP.ActHostAddress = "100.100.100.2";
            this.m_MelsecTCP.ActNetworkNumber = 4;
            this.m_MelsecTCP.ActStationNumber = 1;

            this.m_MelsecTCP.ActTimeOut = 5000;
        }

        public int PLC_Connection()
        {
            int IsOpen = this.m_MelsecTCP.Open();
            return IsOpen;
        }
        */

        private string LogSend = "Send : ";
        private string LogResp = "Resp : ";
        void umac_UmacCntEvent1(string logData, int typeData)
        {
             if (typeData == 1) Equipment_Umac07_uTxt.Text += LogSend;
             else Equipment_Umac07_uTxt.Text += LogResp;
             
             Equipment_Umac07_uTxt.Text += logData+ "\r\n";
        }

        private bool eventing_PasswordEditing(string inputId, string inputPass)
        {
            NowUserId = dlgPasswordEdit.GetSetInputID;
            NowUserPass = dlgPasswordEdit.GetSetInputPass;

            return true;
        }

        //static readonly string Account_Operator = "OPERATOR";
        //static readonly string Account_Engineer = "ENGINEER";
        //static readonly string Account_Maker = "MAKER";

        private bool eventing_LoginComplite(string inputId, string inputPass)
        {
            string Now_Account = LamiSystem.GetSet_Now_User_Account;
            if (Now_Account == Account_Engineer)
            {
                NowUserId = this.GetReg(LamiSystem.RegPathGapStatus, "Eid");
                NowUserPass = this.GetReg(LamiSystem.RegPathGapStatus, "Epass");
            }
            else if (Now_Account == Account_Maker)
            {
                NowUserId = this.GetReg(LamiSystem.RegPathGapStatus, "Mid");
                NowUserPass = this.GetReg(LamiSystem.RegPathGapStatus, "Mpass");
            }

            if (NowUserId == inputId && NowUserPass == inputPass)
            {
                string processName = "osk";
                System.Diagnostics.Process[] tProcess = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (System.Diagnostics.Process process in tProcess)
                {
                    if (process.ProcessName == "osk")
                    {
                        process.Kill();
                    }
                }
                return true;
            }
            else return false;
        }

        private string _strNowAccount = null;
        /// <summary>
        /// 메임폼 로딩에 대한 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDlgMain_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Size = new Size(1280, 984);

            fileSystem = new Control_Files();

            dlgInspect.OperationEvent += new MyEventOneInsp(Get_PLC_Now_ModelName_RecipeNo);
            dlgInspect.Pix_Loading += new MyEventInsp_Pix01(eventing_PixLoad);
            

//             dlgInspect.TestStartEvent += new MyEventTwoInsp(Measurement_Data_Save);
//             dlgInspect.MeaDataEvent1 += new MyEventMeaData1(Measurement_Data_Copy1);
//             dlgInspect.MeaDataEvent2 += new MyEventMeaData2(Measurement_Data_Copy2);
//             dlgInspect.MeaDataEvent3 += new MyEventMeaData3(Measurement_Data_Copy3);
//             dlgInspect.MeaDataEvent4 += new MyEventMeaData4(Measurement_Data_Copy4);
        }

//         RegistryKey reg = Registry.CurrentUser;
//                 reg = reg.OpenSubKey(strNodePath, true);
//                 if (reg != null)
//                 {
//                     reg.Close();
//                     Registry.CurrentUser.DeleteSubKey(strNodePath, false);
//                 }

//          Measurement_DataSet_To_Register(LamiSystem.RegPathMeasureGrid_Buf_Uper, uDS_Inspect_Measure_Uper);
//             Measurement_DataSet_To_Register(LamiSystem.RegPathMeasureGrid_Buf_Down, uDS_Inspect_Measure_Down);
//             Measurement_DataTable_To_Register(LamiSystem.RegPathMeasureChart_Buf_Uper, Uper_MeasureTables);
//             Measurement_DataTable_To_Register(LamiSystem.RegPathMeasureChart_Buf_Down, Down_MeasureTables);
        public void Initional_Measure_Buffer_Reg()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureGrid_Buf_Uper, true);
            if (reg != null)
            {
                reg.Close();
                Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathMeasureGrid_Buf_Uper, false);
            }

            reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureGrid_Buf_Down, true);
            if (reg != null)
            {
                reg.Close();
                Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathMeasureGrid_Buf_Down, false);
            }

            reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureChart_Buf_Uper, true);
            if (reg != null)
            {
                reg.Close();
                Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathMeasureChart_Buf_Uper, false);
            }

            reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureChart_Buf_Down, true);
            if (reg != null)
            {
                reg.Close();
                Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathMeasureChart_Buf_Down, false);
            }

        }

// 
//         private UltraDataSource Inspect_MeaData_Uper;
//         private UltraDataSource Inspect_MeaData_Down;
//         private System.Data.DataTable[] Inspect_MeaChart_Uper;
//         private System.Data.DataTable[] Inspect_MeaChart_Down;
// 
//         public void Measurement_Data_Save()
//         {
//             string tmpstr = dlgInspect.uDS_Inspect_Measure_Uper.Rows[0].GetCellValue("CPK 값").ToString();
//             Inspect_MeaData_Uper = dlgInspect.uDS_Inspect_Measure_Uper;
//             Inspect_MeaData_Down = dlgInspect.uDS_Inspect_Measure_Down;
//             Inspect_MeaChart_Uper = dlgInspect.Uper_MeasureTables;
//             Inspect_MeaChart_Down = dlgInspect.Down_MeasureTables;
// 
//             string tmpstr2 = Inspect_MeaData_Uper.Rows[0].GetCellValue("CPK 값").ToString();
//         }
// 
// 
//         public void Measurement_Data_Copy1()
//         {
//             string tmpstr1 = Inspect_MeaData_Uper.Rows[0].GetCellValue("CPK 값").ToString();
//             dlgInspect.uDS_Inspect_Measure_Uper = Inspect_MeaData_Uper;
//             string tmpstr2 = dlgInspect.uDS_Inspect_Measure_Uper.Rows[0].GetCellValue("CPK 값").ToString();
//         }
// 
//         public void Measurement_Data_Copy2()
//         {
//             string tmpstr1 = Inspect_MeaData_Down.Rows[0].GetCellValue("CPK 값").ToString();
//             dlgInspect.uDS_Inspect_Measure_Down = Inspect_MeaData_Down;
//             string tmpstr2 = dlgInspect.uDS_Inspect_Measure_Down.Rows[0].GetCellValue("CPK 값").ToString();
//         }
// 
//         public void Measurement_Data_Copy3()
//         {
//             dlgInspect.Uper_MeasureTables = Inspect_MeaChart_Uper;
//         }
// 
//         public void Measurement_Data_Copy4()
//         {
//             dlgInspect.Down_MeasureTables = Inspect_MeaChart_Down;
//         }

        public void FormDlgMain_Initionalize()
        {
            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 10);

            Inspect_MIL_Initialize_Gap();
            Inspect_MIL_Initialize_BiCell();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 30);

            Inspect_CalData_Loading();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 40);

            SystemLami_Config_Loading();
            VisionLami_Config_Loading();

            RecipeLami_Config_Loading();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 50);

            Recipe_Config_ListData_To_UperGrid();
            Recipe_Config_ListData_To_DownGrid();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 60);
            
            //20150307 WKB 209
            //Recipe_Uper_Config_DropDownList_Setup();
            //Recipe_Down_Config_DropDownList_Setup();

            Recipe_Config_UperGrid_Output();
            Recipe_Config_DownGrid_Output();

            //Recipe_Config_Inspect_Output_Uper();
            //Recipe_Config_Inspect_Output_Down();

            //SetReg(LamiSystem.RegPathRcpCon, "상단 항목", uGrd_Recipe_UperData.Rows.Count.ToString("0"));
            //SetReg(LamiSystem.RegPathRcpCon, "하단 항목", uGrd_Recipe_DownData.Rows.Count.ToString("0"));

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 70);

            Equipment_Config_Loading();
            System_Status_Check();
            
            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 80);
            //생산 수량, 검사 수량, NG 수량, 유저 어카운 값을 가지는 레지스터를
            //확인 한 후 없다면 디폴트 값을 기록해준다.
            Align_Status_File_To_Register();

            //프로그램 기동시 유저를 선택하지 않고 바로 오피로 기본 로그온 하도록
            //수정한다. 여기에서 오피로 설정하고 이에 해당하는 폼을 나타낸다.

            LamiSystem.GetSet_Now_Login_Time = DateTime.Now;

            LamiSystem.GetSet_Now_User_Account = Account_Operator;

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 85);

            System_Status_NowAccount_View_To_Reg();

            User_NowAccount_Viewer_Set(LamiSystem.GetSet_Now_User_Account);
            ubtnToolbarUser.Text = LamiSystem.GetSet_Now_User_Account;

            _Uper_Control_DrawArea.SeclectingRect += new RecipeEvent3(Recipe_Config_Display_Select_Rect_Row_Uper);
            _Down_Control_DrawArea.SeclectingRect += new RecipeEvent3(Recipe_Config_Display_Select_Rect_Row_Down);

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 90);

            User_Login_Data_Write_To_File(1);

            _strNow_Model_Name = GetReg(LamiSystem.RegPathSystemStatus, "모델 이름");
            _strNow_Model_Number = GetReg(LamiSystem.RegPathSystemStatus, "모델 번호");
            LamiSystem.GetSet_Now_Model_Name = _strNow_Model_Name;
            LamiSystem.GetSet_Now_Model_Number = _strNow_Model_Number;

            LamiSystem.StrListRcpConData.Clear();
            LamiSystem.StrListRcpConData.Add(_strNow_Model_Name);
            LamiSystem.StrListRcpConData.Add(_strNow_Model_Number);

            ubtnToolbarModel.Text = "MODEL  " + _strNow_Model_Name + " - " + _strNow_Model_Number;
            ubtnToolbarModel.Refresh();

            Initional_Measure_Buffer_Reg();

            //인스펙션 폼에서 사용하는 양산 수량 정보를 초기화한다.
            //레지스트리의 데이터를 초기화 하는 함수.
            Inspect_Count_RegData_Reset();

            //조명 제어기를 설정하는 함수를 호출한다.
            //시리얼 포트를 통해서 조명 제어기를 설정한다.
            System_BackLight_Initialize();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 95);
        }

        private void Inspect_Count_RegData_Reset()
        {
            SetReg(LamiSystem.RegPathGapStatus, "Count_Product", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Both", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Uper", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Down", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Uper", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Down", "0");

            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Uper, true);
            int regCount = 0;
            int mesColCount = 19;
            if (reg != null)
            {
                regCount = reg.ValueCount;
                int Grid_Rows = regCount / mesColCount;
                for (int i = 0; i < Grid_Rows; i++)
                {
                    //표준편차
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000"), "0.000");
                    //최소
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"), "0.000");
                    //최대
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"), "0.000");
                    //CP
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000"), "0.000");
                    //CPK
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000"), "0.000");
                    //OK count
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"), "0");
                    //NG Count
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), "0");
                    //Value
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), "0");
                    //SqValue
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), "0");
                    //ProductOK
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 18).ToString("000"), "0");
                }
            }
            
           
            //reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Down, true);
            if (reg != null)
            {
                reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Down, true);
                if (reg == null) return;

                regCount = reg.ValueCount;
                int Grid_Rows = regCount / mesColCount;
                for (int i = 0; i < Grid_Rows; i++)
                {
                    //표준편차
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 9).ToString("000"), "0.000");
                    //최소
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 10).ToString("000"), "0.000");
                    //최대
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 11).ToString("000"), "0.000");
                    //CP
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 12).ToString("000"), "0.000");
                    //CPK
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 13).ToString("000"), "0.000");
                    //OK count
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 14).ToString("000"), "0");
                    //NG Count
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 15).ToString("000"), "0");
                    //Value
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 16).ToString("000"), "0");
                    //SqValue
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 17).ToString("000"), "0");
                    //ProductOK
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 18).ToString("000"), "0");
                }
            }
            
        }

        /*
        private void FormDlgMain_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Size = new Size(1280, 984);

             fileSystem = new Control_Files();
            
            //RecipeGap_drawArea1.pictureBox1.ImageIpl = IplImage.FromBitmap(SystemAlign.Properties.Resources.GapSetImage);
            //RecipeBiCell_drawArea1.pictureBox1.ImageIpl = IplImage.FromBitmap(SystemAlign.Properties.Resources.BiCell);
            

            dlgInspect.OperationEvent += new MyEventOneInsp(Get_PLC_Now_ModelName_RecipeNo);
            this.MouseWheel += new MouseEventHandler(FormDlgMain_MouseWheel);

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 10);

            Inspect_MIL_Initialize_Gap();
            Inspect_MIL_Initialize_BiCell();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 30);

            Inspect_CalData_Loading();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 40);

            SystemLami_Config_Loading();
            VisionLami_Config_Loading();
            RecipeLami_Config_Loading();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 50);

            Recipe_Config_ListData_To_UperGrid();
            Recipe_Config_ListData_To_DownGrid();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 60);
            //Recipe_Uper_Config_DropDownList_Setup();
            //Recipe_Down_Config_DropDownList_Setup();

            Recipe_Config_UperGrid_Output();
            Recipe_Config_DownGrid_Output();

            //SetReg(LamiSystem.RegPathRcpCon, "상단 항목", uGrd_Recipe_UperData.Rows.Count.ToString("0"));
            //SetReg(LamiSystem.RegPathRcpCon, "하단 항목", uGrd_Recipe_DownData.Rows.Count.ToString("0"));

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 70);

            Equipment_Config_Loading();
            System_Status_Check();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 80);
            //생산 수량, 검사 수량, NG 수량, 유저 어카운 값을 가지는 레지스터를
            //확인 한 후 없다면 디폴트 값을 기록해준다.
            Align_Status_File_To_Register();

            //프로그램 기동시 유저를 선택하지 않고 바로 오피로 기본 로그온 하도록
            //수정한다. 여기에서 오피로 설정하고 이에 해당하는 폼을 나타낸다.

            LamiSystem.GetSet_Now_Login_Time = DateTime.Now;

            LamiSystem.GetSet_Now_User_Account = Account_Operator;

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 85);

            System_Status_NowAccount_View_To_Reg();

            User_NowAccount_Viewer_Set(LamiSystem.GetSet_Now_User_Account);
            ubtnToolbarUser.Text = LamiSystem.GetSet_Now_User_Account;

            _Uper_Control_DrawArea.SeclectingRect += new RecipeEvent3(Recipe_Config_Display_Select_Rect_Row_Uper);
            _Down_Control_DrawArea.SeclectingRect += new RecipeEvent3(Recipe_Config_Display_Select_Rect_Row_Down);

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 90);

            User_Login_Data_Write_To_File(1);

            _strNow_Model_Name = GetReg(LamiSystem.RegPathSystemStatus, "모델 이름");
            _strNow_Model_Number = GetReg(LamiSystem.RegPathSystemStatus, "모델 번호");
            LamiSystem.GetSet_Now_Model_Name = _strNow_Model_Name;
            LamiSystem.GetSet_Now_Model_Number = _strNow_Model_Number;

            LamiSystem.StrListRcpConData.Clear();
            LamiSystem.StrListRcpConData.Add(_strNow_Model_Name);
            LamiSystem.StrListRcpConData.Add(_strNow_Model_Number);

            ubtnToolbarModel.Text = "MODEL  " + _strNow_Model_Name + " - " + _strNow_Model_Number;
            ubtnToolbarModel.Refresh();

            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalizing !", 100);
        }
        */

        

        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void Align_Status_File_To_Register()
        {
            LamiSystem.StrLstGapStatusData.Clear();
            LamiSystem.StrLstGapStatusTitle.Clear();

            byte[] resourceObject = SystemAlign.Properties.Resources.AlignStatusDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Align_Status_StringParsing_Inspect(strLine);
                }
            }
            ioStream.Close();

            Align_Status_RegData_Check();
        }


        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Align_Status_StringParsing_Inspect(string fileData)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strTitleName = null;
            intStartIndex = 0;
            intEndIndex = fileData.IndexOf("\t", intStartIndex);
            strTitleName = fileData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrLstGapStatusTitle.Add(strTitleName);

            intStartIndex = intEndIndex + 1;
            intEndIndex = fileData.IndexOf("\t", intStartIndex);
            strTitleName = fileData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrLstGapStatusData.Add(strTitleName);
        }

        private void Align_Status_RegData_Check()
        {
            for (int i = 0; i < LamiSystem.StrLstGapStatusData.Count; i++)
            {
                string regData = GetReg(LamiSystem.RegPathGapStatus, LamiSystem.StrLstGapStatusTitle[i]);
                if (regData == "")
                    SetReg(LamiSystem.RegPathGapStatus, LamiSystem.StrLstGapStatusTitle[i], LamiSystem.StrLstGapStatusData[i]);
            }
        }


        private void User_NowAccount_Viewer_Set(string account)
        {
            if (account == Account_Operator)
            {
                ubtnToolbarUser.Text = Account_Operator;

            }
            else if (account == Account_Engineer)
            {

                ubtnToolbarUser.Text = Account_Engineer;
            }
            else if (account == Account_Maker)
            {
                ubtnToolbarUser.Text = Account_Maker;
            }
            this.Select();
        }


        bool VisionJobWorking = true;


        //2014.06.11 WKB
        //현재 상태에서 카메라 연결, 조명 장치 연결을 확인하지 않는다.
        //카메라와 조명을 확인 할 필요가 없어져 바로 상태값을 True로
        //설정해서 입력해 준다.
        bool[] _systemStatusFlag = new bool[4];
        private void System_Status_Check()
        {
#if(SIMMIL)
            //MIL에서 작업을 진행하기 때문에 연결 확인을 진행하지 않는다.
            _systemStatusFlag[0] = true;
#endif

#if(SIMPLC)
            if (PLC_Connect_Check() != 0) _systemStatusFlag[1] = false;
            else _systemStatusFlag[1] = true;
#endif

#if(!SIMUMAC)
            if (UMAC_Connect_Check() != 0) _systemStatusFlag[2] = false;
            else
            {
                _systemStatusFlag[2] = true;
                Check_Commu.Enabled = true;
            }
#endif

#if(SIMLVS)
            //LVS를 시스템에서 직접 제어를 하지 않아서 연결 상태를 확인하지 않는다.
            _systemStatusFlag[3] = true;
            LamiSystem.IsConnect_LVS = true;
#endif
            _systemStatusFlag[0] = true;
            _systemStatusFlag[1] = true;
            _systemStatusFlag[3] = true;
            LamiSystem.SystemStatusFlag = _systemStatusFlag;
        }

        /*
        private int PLC_Connect_Check()
        {
            string sMessage = "PLC와 연결 할 수 없습니다." + "\r\n" + "확인 후 다시 시도하십시요!";
            
            int Plc_isConnect = PLC_Connect_Ready();
            if (Plc_isConnect != 0)
            {
                LamiSystem.IsConnect_PLC = false;
                MessageBox.Show(sMessage);
            }
            else
            {
                //메인에서 연결 콘트롤을 가져오면서 추가됨.
                plc = new Control_PLC(this.m_MelsecTCP);
                
                LamiSystem.IsConnect_PLC = true;
                plc.PLC_Connecting_Check();
            }

            return Plc_isConnect;
        }

        private int m_iPLCIsOpen = 0;

        private int PLC_Connect_Ready()
        {
            PLC_Initialize();

            int isOpen = PLC_Connection();
            m_iPLCIsOpen = isOpen;
            if (isOpen != 0) return -1;
            else return 0;
        }
        */

        Control_Metrox brabber = new Control_Metrox();
        private int CAM_Connect_Check()
        {
            int Cam_isConnect = CAM_Connect_Ready();
            if (Cam_isConnect != 0)
                MessageBox.Show("CAM과 연결을 할 수 없습니다." + "\r\n" + "관리자에게 확인 하십시요!");
            return Cam_isConnect;
        }



        
        private int UMAC_Connect_Check()
        {
            int UMAC_isConnect = UMAC_Connect_Ready();
            if (UMAC_isConnect != 0)
            {
                LamiSystem.IsConnect_UMAC = false;
                MessageBox.Show("UMAC 장치와 연결을 할 수 없습니다." + "\r\n" + "관리자에게 확인 하십시요!");
            }
            else LamiSystem.IsConnect_UMAC = true;

            return UMAC_isConnect;
        }

        
        private int LVS_Connect_Check()
        {
            int LVS_isConnect = LVS_Connect_Ready();
            if (LVS_isConnect != 0)
            {
                LamiSystem.IsConnect_LVS = false;
                MessageBox.Show("조명 장치와 연결을 할 수 없습니다." + "\r\n" + "관리자에게 확인 하십시요!");
            }
            else LamiSystem.IsConnect_LVS = true;
            return LVS_isConnect;
        }

        
        

        /*
         private int PLC_Connect_Ready()
        {
             plc.melsec.PLC_Initialize();
            
             int isOpen = plc.melsec.PLC_Connection();
             m_iPLCIsOpen = isOpen;
             if (isOpen != 0) return -1;
             else return 0;


        }
        */


        private int CAM_Connect_Ready()
        {
            bool isOpen = CAM_Initialize();
            if (isOpen == false) return -1;
            return 0;
        }


        private CvCapture capture;
        private bool CAM_Initialize()
        {
            try
            {
                capture = CvCapture.FromCamera(CaptureDevice.MIL, 0);
                capture.SetCaptureProperty(CaptureProperty.FrameWidth, 2352);
                capture.SetCaptureProperty(CaptureProperty.FrameHeight, 1728);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        

        private int UMAC_Connect_Ready()
        {
            int iUMACIsOpen = umac.Umac_Connect();
            if (iUMACIsOpen > 0) return 0;
            else return -1;
        }

        private int LVS_Connect_Ready()
        {
            //int iUMACIsOpen = TCP_Client_Open();
            //if (iUMACIsOpen != 0) return -1;
            return 0;
        }

        public void Inspect_CalData_Loading()
        {
            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (Inspect_CalData_Register_Empty_Check() == true)
            {
                Inspect_CalData_File_To_Register();
            }

            Inspect_CalData_Register_To_Lists();
        }

       
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void Inspect_CalData_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.CalibrationConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Inspcet_CalData_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Inspcet_CalData_StringParsing(string dataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intCalibrationLineCount = 0;
            string strCalTitle = "";
            string strCalData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (dataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intCalibrationLineCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = dataLine.IndexOf("\t", intStartIndex);
                    strCalTitle = dataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                    intCalibrationLineCount++;
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = dataLine.IndexOf("\t", intStartIndex);
                    strCalData = dataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                }
                
            }
            SetReg(LamiSystem.RegPathCal, strCalTitle, strCalData);
        }

        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool Inspect_CalData_Register_Empty_Check()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathCal, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }

        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void Inspect_CalData_Register_To_Lists()
        {
            Register_To_StringList(LamiSystem.RegPathCal, ref LamiSystem.StrLstCalTitle);
            for (int i = 0; i < LamiSystem.StrLstCalTitle.Count; i++)
            {
                string strRegCalData = Inspect_CalData_Reg_To_List(LamiSystem.RegPathCal, LamiSystem.StrLstCalTitle[i]);
                LamiSystem.StrLstCalData.Add(strRegCalData);
            }
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string Inspect_CalData_Reg_To_List(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        /*
        public void RecipeBiCell_Config_Display_Select_Rect_Row(int selectRect)
        {
            int ItemCount = 14;
            int listAddress = (_BiCell_Control_DrawArea.GetSetGraphicListCount - selectRect) - 1;

            string rowType = LamiSystem.StrLstRcpConInspData_BiCell[6 + (listAddress * ItemCount)];
            string selectSeqNo = LamiSystem.StrLstRcpConInspData_BiCell[7 + (listAddress * ItemCount)];
            int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_BiCell[4 + (listAddress * ItemCount)]);

            RecipeBiCell_Config_Display_Select_Rect_Button(intGridRowNo, selectSeqNo, rowType);
        }

        public void RecipeBiCell_Config_Display_Select_Rect_Button(int rowNo, string strSeqNo, string rowType)
        {
            RecipeBiCell_Config_Display_Select_Grid_Button_Init();

            uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;

            if (int.Parse(strSeqNo) == 0 || rowType == "넓이")
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;

            if (int.Parse(strSeqNo) == 1 || rowType == "넓이")
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
        }

        public void RecipeBiCell_Config_Display_Select_Grid_Button_Init()
        {
            for (int i = 0; i < uGrd_RecipeBiCell_Data.DisplayLayout.Rows.Count; i++)
            {
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[i].Cells[0].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[i].Cells[2].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[i].Cells[5].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            }
            return;
        }
        */

        public void Recipe_Config_Display_Select_Rect_Row_Uper(int selectRect)
        {
            int Grid_Rows = -1;
            int ROI_No = uGrd_Recipe_Inspect_Uper.Rows.Count - 1 - selectRect;
            string rowType = string.Empty;
            string selectSeqNo = string.Empty;

            //int DS_Rows = uGrd_Recipe_Inspect_Uper.Rows.Count - 1 - selectRect;
            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                int readROI = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                if (readROI == ROI_No)
                {
                    Grid_Rows = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[13].Value.ToString())/2;
                    rowType = uGrd_Recipe_Inspect_Uper.Rows[Grid_Rows].Cells[6].Value.ToString();
                    //selectSeqNo = uGrd_Recipe_Inspect_Uper.Rows[Grid_Rows].Cells[7].Value.ToString();
                    selectSeqNo = uGrd_Recipe_Inspect_Uper.Rows[i].Cells[7].Value.ToString();
                    break;
                }
            }
            //int Grid_Rows = DS_Rows/2;
            //if (colNo == 2) DS_Rows = (2 * e.Cell.Row.Index);
            //else DS_Rows = (2 * e.Cell.Row.Index) + 1;

            //string rowType = uDS_Inspect_Uper.Rows[Grid_Rows].GetCellValue(6).ToString();
            //string selectSeqNo = uDS_Inspect_Uper.Rows[Grid_Rows].GetCellValue(7).ToString();


            Recipe_Config_Display_Select_Rect_Button_Uper(Grid_Rows, selectSeqNo, rowType);

            /*
            if (colNo == 2) DS_Rows = (2*e.Cell.Row.Index);
            else DS_Rows = (2*e.Cell.Row.Index) + 1;

            int nowSelectZoneNo = int.Parse(uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(5).ToString());
            string selectSeqNo = uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(5).ToString();
            int intGridRowNo = e.Cell.Row.Index;
            string rowType = uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(6).ToString();
            
            Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
            Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
            */
        }

        public void Recipe_Config_Display_Select_Rect_Button_Uper(int rowNo, string strSeqNo, string rowType)
        {
            Recipe_Config_Display_Select_UperGrid_Button_Init();

            uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;

            int tmpInde = strSeqNo.IndexOf("2");
            //if (strSeqNo == "1차")
            if (tmpInde < 0)
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;
            else
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
        }

        /*
        public void Recipe_Config_Display_Select_Rect_Button_Uper(int rowNo, string strSeqNo, string rowType)
        {
            Recipe_Config_Display_Select_UperGrid_Button_Init();

            uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;

            if (int.Parse(strSeqNo) == 0 || rowType == "넓이")
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;

            if (int.Parse(strSeqNo) == 1 || rowType == "넓이")
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
        }
        */

        //2015. WKB. 207
        //그리드 개체를 제어하던 것을 그리드의 데이터소스를 제어하는 방법으로 변경함.
        //uDS_Inspect_Down
        public void Recipe_Config_Display_Select_Rect_Row_Down(int selectRect)
        {
            //int Grid_Rows1 = -1;
            int Grid_Rows = -1;
            //int ROI_No1 = uGrd_Recipe_Inspect_Down.Rows.Count - 1 - selectRect;
            int ROI_No = uDS_Inspect_Down.Rows.Count - 1 - selectRect;//uGrd_Recipe_Inspect_Down.Rows.Count - 1 - selectRect;
            //string rowType1 = string.Empty;
            //string selectSeqNo1 = string.Empty;
            string rowType = string.Empty;
            string selectSeqNo = string.Empty;

            //for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
            for (int i = 0; i < uDS_Inspect_Down.Rows.Count; i++)
            {
                int readROI1 = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[5].Value.ToString());
                int readROI = int.Parse(uDS_Inspect_Down.Rows[i].GetCellValue(5).ToString());

                if (readROI == ROI_No)
                {
                    //Grid_Rows1 = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[13].Value.ToString()) / 2;
                    Grid_Rows = int.Parse(uDS_Inspect_Down.Rows[i].GetCellValue(13).ToString()) / 2;
                    //rowType1 = uGrd_Recipe_Inspect_Down.Rows[Grid_Rows].Cells[6].Value.ToString();
                    rowType = uDS_Inspect_Down.Rows[Grid_Rows].GetCellValue(6).ToString();
                    //selectSeqNo1 = uGrd_Recipe_Inspect_Down.Rows[i].Cells[7].Value.ToString();
                    selectSeqNo = uDS_Inspect_Down.Rows[i].GetCellValue(7).ToString();
                }
            }
            Recipe_Config_Display_Select_Rect_Button_Down(Grid_Rows, selectSeqNo, rowType);
        }

        //2015. WKB. 207
        /*
        public void Recipe_Config_Display_Select_Rect_Row_Down(int selectRect)
        {
            int Grid_Rows = -1;
            int ROI_No = uGrd_Recipe_Inspect_Down.Rows.Count - 1 - selectRect;
            string rowType = string.Empty;
            string selectSeqNo = string.Empty;

            for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
            {
                int readROI = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[5].Value.ToString());
                if (readROI == ROI_No)
                {
                    Grid_Rows = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[13].Value.ToString()) / 2;
                    rowType = uGrd_Recipe_Inspect_Down.Rows[Grid_Rows].Cells[6].Value.ToString();
                    selectSeqNo = uGrd_Recipe_Inspect_Down.Rows[i].Cells[7].Value.ToString();
                }
            }
            Recipe_Config_Display_Select_Rect_Button_Down(Grid_Rows, selectSeqNo, rowType);
        }
        */

        /*
        public void Recipe_Config_Display_Select_Rect_Row_Down(int selectRect)
        {
//             int ItemCount = 14;
//             int listAddress = (_Down_Control_DrawArea.GetSetGraphicListCount - selectRect) - 1;
// 
//             string rowType = LamiSystem.StrLstRcpConInspData_Down[6 + (listAddress * ItemCount)];
//             string selectSeqNo = LamiSystem.StrLstRcpConInspData_Down[7 + (listAddress * ItemCount)];
//             int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_Down[4 + (listAddress * ItemCount)]);
// 
//             Recipe_Config_Display_Select_Rect_Button_Down(intGridRowNo, selectSeqNo, rowType);
            int DS_Rows = LamiSystem.RectListRecipeBoxZone_Down.Count - 1 - selectRect;
            int Grid_Rows = DS_Rows / 2;

            string rowType = uDS_Inspect_Down.Rows[DS_Rows].GetCellValue(6).ToString();
            string selectSeqNo = uDS_Inspect_Down.Rows[DS_Rows].GetCellValue(7).ToString();


            Recipe_Config_Display_Select_Rect_Button_Down(Grid_Rows, selectSeqNo, rowType);
        }
        */

        public void Recipe_Config_Display_Select_Rect_Button_Down(int rowNo, string strSeqNo, string rowType)
        {
//             Recipe_Config_Display_Select_DownGrid_Button_Init();
// 
//             uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;
// 
//             if (int.Parse(strSeqNo) == 0 || rowType == "넓이") uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;
// 
//             if (int.Parse(strSeqNo) == 1 || rowType == "넓이") uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
            Recipe_Config_Display_Select_DownGrid_Button_Init();

            uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;

            int tmpInde = strSeqNo.IndexOf("2");
            //if (strSeqNo == "1차")
            if (tmpInde < 0)
                uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;
            else
                uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
        }

        static readonly string System_Status_NowAccount_Title = "nowAccount";
        private string System_Status_NowAccount_Reg_To_View()
        {
            return GetReg(LamiSystem.RegPathSystemStatus, System_Status_NowAccount_Title);
        }

        //public void SetReg(string strNodePath, string strName, string strData)

        private void System_Status_NowAccount_View_To_Reg()
        {
            SetReg(LamiSystem.RegPathSystemStatus, System_Status_NowAccount_Title, LamiSystem.GetSet_Now_User_Account);//NowUserAccount);
        }

        private void SystemLami_Config_Loading()
        {

            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (System_Config_Register_Empty_Check() == true)
            {
                System_Config_File_To_Register();
            }

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //타이틀, 콘트롤명, 데이터 리스트 배열에 저장한다. 
            System_Config_Register_To_Lists();
        }

        public void System_BackLight_Initialize()
        {
            Port_Initialize("2");
            //Port_Open();
            string SetValue = "";

            //string ndatasys = LamiSystem.StrListSysConData[25];
            //string datasys = LamiSystem.StrListSysConData[26];

            if (LamiSystem.StrListSysConData[25] == "ON")
                SetValue = "200,";
            else
                SetValue = "000,";


            if (LamiSystem.StrListSysConData[26] == "ON")
                SetValue += "200";
            else
                SetValue += "000";



            //LVS_Set_BackLight(SetValue);

            //Port_Close();
        }
        private void VisionLami_Config_Loading()
        {
            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (VisionLami_Config_Register_Empty_Check(LamiSystem.RegPathVisCon_Lami) == true)
            {
                VisionLami_Config_File_To_Register();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (VisionLami_Config_Register_Empty_Check(LamiSystem.RegPathVisConGrid_Uper) == true)
            {
                VisionLami_Config_File_To_Register_UperGrid();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (VisionLami_Config_Register_Empty_Check(LamiSystem.RegPathVisConGrid_Down) == true)
            {
                VisionLami_Config_File_To_Register_DownGrid();
            }
            
            Vision_uGrd_Uper.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 타이틀, 콘트롤명, 데이터 리스트 배열에 저장한다. 
            VisionLami_Config_Register_To_Lists();

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 상부 그리드 데이터 리스트 배열에 저장한다. 
            VisionLami_Config_Register_To_Lists_UperGrid();

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 하부 그리드 데이터 리스트 배열에 저장한다. 
            VisionLami_Config_Register_To_Lists_DownGrid();

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            VisionLami_Config_ListData_To_UperGrid();
            VisionLami_Config_ListData_To_DownGrid();
        }

        /*
        private void VisionBiCell_Config_Loading()
        {
            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (VisionBiCell_Config_Register_Empty_Check(LamiSystem.RegPathVisCon_BiCell) == true)
            {
                VisionBiCell_Config_File_To_Register();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (VisionBiCell_Config_Register_Empty_Check(LamiSystem.RegPathVisConGrid_BiCell) == true)
            {
                VisionBiCell_Config_File_To_Register_Grid();
            }

            VisionBiCell_uGrd_Offset.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //타이틀, 콘트롤명, 데이터 리스트 배열에 저장한다. 
            VisionBiCell_Config_Register_To_Lists();

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //그리드 타이틀, 데이터 리스트 배열에 저장한다. 
            VisionBiCell_Config_Register_To_Lists_Grid();
        }
        */

        /*
        private void RecipeBiCell_Config_Loading()
        {
            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (RecipeBiCell_Config_Register_Empty_Check(LamiSystem.RegPathRcpCon_BiCell) == true)
            {
                RecipeBiCell_Config_File_To_Register();
            }

            if (RecipeBiCell_Config_Register_Empty_Check(LamiSystem.RegPathRcpConGrid_BiCell) == true)
            {
                RecipeBiCell_Config_File_To_Register_Grid();
            }

            if (RecipeBiCell_Config_Register_Empty_Check(LamiSystem.RegPathRcpConInsp_BiCell) == true)
            {
                RecipeBiCell_Config_File_To_Register_Inspect();
            }

            RecipeBiCell_Config_Register_To_Lists();

            RecipeBiCell_Config_Register_To_Lists_Grid();

            RecipeBiCell_Config_Register_To_Lists_Inspect();

            RecipeBiCell_Config_Box_To_Image_Sum();
        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서 각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void RecipeBiCell_Config_Register_To_Lists_Inspect()
        {
            RecipeBiCell_Config_Register_To_List_Inspect();
            LamiSystem.StrLstRcpConInspData_BiCell.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_BiCell.Count; i++)
            {
                string strRegisterGridData = RecipeBiCell_Config_Register_To_List_Inspect(LamiSystem.RegPathRcpConInsp_BiCell, LamiSystem.StrLstRcpConInspTitle_BiCell[i]);
                LamiSystem.StrLstRcpConInspData_BiCell.Add(strRegisterGridData);
            }

            RecipeBiCell_Config_Inspect_Output();
        }


        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void RecipeBiCell_Config_Register_To_Lists_Grid()
        {
            RecipeBiCell_Config_Register_To_List_Grid();
            LamiSystem.StrLstRcpConGridData_BiCell.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConGridTitle_BiCell.Count; i++)
            {
                string strRegisterGridData = RecipeBiCell_Config_Register_To_List_Data(LamiSystem.RegPathRcpConGrid_BiCell, LamiSystem.StrLstRcpConGridTitle_BiCell[i]);
                LamiSystem.StrLstRcpConGridData_BiCell.Add(strRegisterGridData);
            }
        }

        //레지스트리의 데이터 값을 읽와서 리스트 배열에 저장한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름을 가지는 문자열 배열
        //3번 파람 : 레지스트리에서 읽은 값을 저장할 문자열 배열
        public void RecipeBiCell_Config_Register_To_List_Data(string strNodePath, List<string> regTitle, List<string> regData)
        {
            regData.Clear();

            for (int i = 0; i < regTitle.Count; i++)
            {
                regData.Add(this.GetReg(strNodePath, regTitle[i]));
            }
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void RecipeBiCell_Config_Register_To_List_Grid()
        {
            Register_To_StringList(LamiSystem.RegPathRcpConGrid_BiCell, ref LamiSystem.StrLstRcpConGridTitle_BiCell);
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string RecipeBiCell_Config_Register_To_List_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void RecipeBiCell_Config_Register_To_List_Title()
        {
            Register_To_StringList(LamiSystem.RegPathRcpCon_Gap, ref LamiSystem.StrListRcpConTitle_BiCell);
        }

        //레지스터에 저장되어져 있는 네임과 데이터를 읽어와서 네임, 데이터 리스트배열에 저장한다.
        private void RecipeBiCell_Config_Register_To_List_NameNData(string readData)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strTitleName = null;
            intStartIndex = 0;
            intEndIndex = readData.IndexOf("\t", intStartIndex);
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrLstRcpConName_BiCell.Add(strTitleName);

            intStartIndex = intEndIndex + 1;
            intEndIndex = readData.Length;
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrListRcpConData_BiCell.Add(strTitleName);
        }

        private string RecipeBiCell_Config_Register_To_Var(string title, string analStr)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strModelData = null;
            intStartIndex = 0;
            intEndIndex = analStr.IndexOf("\t", intStartIndex);
            //strModelData = analStr.Substring(intStartIndex, intEndIndex - intStartIndex);

            intStartIndex = intEndIndex + 1;
            intEndIndex = analStr.Length;
            strModelData = analStr.Substring(intStartIndex, intEndIndex - intStartIndex);

            return strModelData;
        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void RecipeBiCell_Config_Register_To_Lists()
        {
            RecipeBiCell_Config_Register_To_List_Title();
            for (int i = 0; i < LamiSystem.StrListRcpConTitle_BiCell.Count; i++)
            {
                string strRegisterNameNData = RecipeBiCell_Config_Register_To_List_Data(LamiSystem.RegPathRcpCon_BiCell, LamiSystem.StrListRcpConTitle_BiCell[i]);
                RecipeBiCell_Config_Register_To_List_NameNData(strRegisterNameNData);

                if (LamiSystem.StrListRcpConTitle_BiCell[i] == "모델 이름")
                    LamiSystem.GetSet_Now_Model_Name = RecipeBiCell_Config_Register_To_Var(LamiSystem.StrListRcpConTitle_BiCell[i], strRegisterNameNData);
                else if (LamiSystem.StrListRcpConTitle_Gap[i] == "모델 번호")
                    LamiSystem.GetSet_Now_Model_Number = RecipeBiCell_Config_Register_To_Var(LamiSystem.StrListRcpConTitle_BiCell[i], strRegisterNameNData);
            }
        }

        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intRecipeBiCellRectCellCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void RecipeBiCell_Config_File_To_Register_Inspect()
        {
            _intRecipeBiCellRectCellCount = 0;
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeBiCellInspectAreaDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    RecipeBiCell_Config_StringParsing_Inspect(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void RecipeBiCell_Config_StringParsing_Inspect(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathRcpConInsp_BiCell, _intRecipeBiCellRectCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intRecipeBiCellRectCellCount++;
            }
        }

        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intRecipeBiCellGridCellCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void RecipeBiCell_Config_File_To_Register_Grid()
        {
            _intRecipeBiCellGridCellCount = 0;
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeBiCellGridDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    RecipeBiCell_Config_StringParsing_Grid(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void RecipeBiCell_Config_StringParsing_Grid(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathRcpConGrid_BiCell, _intRecipeBiCellGridCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intRecipeBiCellGridCellCount++;
            }
        }
        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool RecipeBiCell_Config_Register_Empty_Check(string checkingRegNodePath)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(checkingRegNodePath, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void RecipeBiCell_Config_StringParsing(string strLine)
        {
            string readTitle = null, readData = null;
            int indexNo = strLine.IndexOf("\t");
            readTitle = strLine.Substring(0, indexNo);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            SetReg(LamiSystem.RegPathRcpCon_BiCell, readTitle, readData);
        }

        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void RecipeBiCell_Config_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeBiCellConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    RecipeBiCell_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }
        */

        private void RecipeLami_Config_Loading()
        {
            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (RecipeGap_Config_Register_Empty_Check(LamiSystem.RegPathRcpCon) == true)
            {
                RecipeGap_Config_File_To_Register();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (RecipeGap_Config_Register_Empty_Check(LamiSystem.RegPathRcpConGrid_Uper) == true)
            {
                //string gridRowsCount = GetReg(LamiSystem.RegPathRcpCon, "상단 항목");
                //if (gridRowsCount != "0") Recipe_Config_File_To_Register_UperGrid();
                Recipe_Config_File_To_Register_UperGrid();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (RecipeGap_Config_Register_Empty_Check(LamiSystem.RegPathRcpConGrid_Down) == true)
            {
                //SetReg(LamiSystem.RegPathRcpCon, "상단 항목", uGrd_Recipe_UperData.Rows.Count.ToString("0"));
                //SetReg(LamiSystem.RegPathRcpCon, "하단 항목", uGrd_Recipe_DownData.Rows.Count.ToString("0"));
                //string gridRowsCount = GetReg(LamiSystem.RegPathRcpCon, "하단 항목");
                //if(gridRowsCount != "0") Recipe_Config_File_To_Register_DownGrid();
                Recipe_Config_File_To_Register_DownGrid();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (RecipeGap_Config_Register_Empty_Check(LamiSystem.RegPathRcpConInsp_Uper) == true)
            {
                //string gridRowsCount = GetReg(LamiSystem.RegPathRcpCon, "상단 항목");
                //if (gridRowsCount != "0") Recipe_Config_File_To_Register_Inspect_Uper();
                Recipe_Config_File_To_Register_Inspect_Uper();
            }

            //레지스테에 그리드(바이셀 옵셋) 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (RecipeGap_Config_Register_Empty_Check(LamiSystem.RegPathRcpConInsp_Down) == true)
            {
                //string gridRowsCount = GetReg(LamiSystem.RegPathRcpCon, "하단 항목");
                //if (gridRowsCount != "0") Recipe_Config_File_To_Register_Inspect_Down();
                Recipe_Config_File_To_Register_Inspect_Down();
            }

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //타이틀, 콘트롤명, 데이터 리스트 배열에 저장한다. 
            RecipeGap_Config_Register_To_Lists();

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //그리드 타이틀, 데이터 리스트 배열에 저장한다. 
            Recipe_Config_Register_To_UperGrid();
            Recipe_Config_Register_To_DownGrid();

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //그리드 타이틀, 데이터 리스트 배열에 저장한다. 
            Recipe_Config_Register_To_Lists_Inspect_Uper();
            Recipe_Config_Register_To_Lists_Inspect_Down();
            
            RecipeGap_Config_Box_To_Image_Sum();
        }

        private void Equipment_Config_Loading()
        {
            //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
            if (Equipment_Config_Register_Empty_Check(LamiSystem.RegPathEquip) == true)
            {
                Equipment_Config_File_To_Register();
            }

            //레지스트리에 저장되어져 있는 데이터를 읽어와서 
            //타이틀, 콘트롤명, 데이터 리스트 배열에 저장한다. 
            Equipment_Config_Register_To_Lists();
        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void Equipment_Config_Register_To_Lists()
        {
            Equipment_Config_Register_To_List_Title();
            for (int i = 0; i < LamiSystem.strLstEquipTitle.Count; i++)
            {
                string strRegisterNameNData =
                    Equipment_Config_Register_To_List_Data(LamiSystem.RegPathEquip, LamiSystem.strLstEquipTitle[i]);

                Equipment_Config_Register_To_List_NameNData(strRegisterNameNData);

                if (LamiSystem.StrListRcpConTitle.Count != 0)
                {
                    if (LamiSystem.StrListRcpConTitle[i] == "연결 주소")
                        Equipment_Umac02_uTxt.Text = Equipment_Config_Register_To_Var(LamiSystem.strLstEquipTitle[i], strRegisterNameNData);
                    else if (LamiSystem.StrListRcpConTitle[i] == "장치 번호")
                        Equipment_Umac04_uCom.Text = Equipment_Config_Register_To_Var(LamiSystem.strLstEquipTitle[i], strRegisterNameNData);
                }

            }
        }

        //레지스터에 저장되어져 있는 네임과 데이터를 읽어와서 네임, 데이터 리스트배열에 저장한다.
        private void Equipment_Config_Register_To_List_NameNData(string readData)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strTitleName = null;
            intStartIndex = 0;
            intEndIndex = readData.IndexOf("\t", intStartIndex);
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.strLstEquipName.Add(strTitleName);

            intStartIndex = intEndIndex + 1;
            intEndIndex = readData.Length;
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.strLstEquipData.Add(strTitleName);
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string Equipment_Config_Register_To_List_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void Equipment_Config_Register_To_List_Title()
        {
            Register_To_StringList(LamiSystem.RegPathEquip, ref LamiSystem.strLstEquipTitle);
        }

        private string Equipment_Config_Register_To_Var(string title, string analStr)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strModelData = null;
            intStartIndex = 0;
            intEndIndex = analStr.IndexOf("\t", intStartIndex);
            //strModelData = analStr.Substring(intStartIndex, intEndIndex - intStartIndex);

            intStartIndex = intEndIndex + 1;
            intEndIndex = analStr.Length;
            strModelData = analStr.Substring(intStartIndex, intEndIndex - intStartIndex);

            return strModelData;
        }

        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void Equipment_Config_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.EquipmentConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Equipment_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Equipment_Config_StringParsing(string strLine)
        {
            string readTitle = null, readData = null;
            int indexNo = strLine.IndexOf("\t");
            readTitle = strLine.Substring(0, indexNo);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            SetReg(LamiSystem.RegPathEquip, readTitle, readData);
        }

        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool Equipment_Config_Register_Empty_Check(string checkingRegNodePath)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(checkingRegNodePath, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }

        //_strListRecipeConfigInspectArea
        #endregion

        private PositionConvert _point_Converter_Uper = PositionConvert.InstanceConvert;
        private PositionConvert _point_Converter_Down = PositionConvert.InstanceConvert;

        private void RecipeGap_Config_Box_To_Image_Sum()
        {
            float[] tempFloats_Uper = new float[] {0f, 0f};
            //if (RecipeGap_drawArea1.pictureBox1.ImageIpl == null)
            //if (RecipeGap_drawArea1.pictureBox1.Image == null)
                _point_Converter_Uper.BoxVsImage(RecipeGap_drawArea1.pictureBox1, SystemAlign.Properties.Resources.BiCell_Top, ref tempFloats_Uper);
            //else if (RecipeGap_drawArea1.pictureBox1.Image.Width != 4096)
                //_point_Converter_Uper.BoxVsImage(RecipeGap_drawArea1.pictureBox1, RecipeGap_drawArea1.pictureBox1.ImageIpl, ref tempFloats_Uper);
                //_point_Converter_Uper.BoxVsImage(RecipeGap_drawArea1.pictureBox1, RecipeGap_drawArea1.pictureBox1.Image, ref tempFloats_Uper);

            LamiSystem.GetSet_System_Status_Zoom_X_Uper = tempFloats_Uper[0];
            LamiSystem.GetSet_System_Status_Zoom_Y_Uper = tempFloats_Uper[1];

            float[] tempFloats_Down = new float[] { 0f, 0f };
            //if (RecipeGap_drawArea2.pictureBox1.ImageIpl == null)
            //if (RecipeGap_drawArea2.pictureBox1.Image == null)
                _point_Converter_Uper.BoxVsImage(RecipeGap_drawArea2.pictureBox1, SystemAlign.Properties.Resources.BiCell_Bot, ref tempFloats_Down);
            //else
                //_point_Converter_Uper.BoxVsImage(RecipeGap_drawArea2.pictureBox1, RecipeGap_drawArea2.pictureBox1.ImageIpl, ref tempFloats_Down);
                //_point_Converter_Uper.BoxVsImage(RecipeGap_drawArea2.pictureBox1, RecipeGap_drawArea2.pictureBox1.Image, ref tempFloats_Down);

            LamiSystem.GetSet_System_Status_Zoom_X_Down = tempFloats_Down[0];
            LamiSystem.GetSet_System_Status_Zoom_Y_Down = tempFloats_Down[1];
        }

        /*

        private void RecipeBiCell_Config_Box_To_Image_Sum()
        {
            float[] tempFloats = new float[] { 0f, 0f };
            if (RecipeBiCell_drawArea1.pictureBox1.ImageIpl == null)
                _point_Converter_BiCell.BoxVsImage(RecipeBiCell_drawArea1.pictureBox1, SystemAlign.Properties.Resources.empty, ref tempFloats);
            else
                _point_Converter_BiCell.BoxVsImage(RecipeBiCell_drawArea1.pictureBox1, RecipeBiCell_drawArea1.pictureBox1.ImageIpl, ref tempFloats);

            LamiSystem.GetSet_System_Status_Zoom_X_BiCell = tempFloats[0];
            LamiSystem.GetSet_System_Status_Zoom_Y_BiCell = tempFloats[1];
        }
        */

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void Recipe_Config_Register_To_Lists_Inspect_Uper()
        {
            Recipe_Config_Register_To_List_Inspect_Uper();
            LamiSystem.StrLstRcpConInspData_Uper.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_Uper.Count; i++)
            {
                string strRegisterGridData = Recipe_Config_Register_To_List_Inspect_Uper(LamiSystem.RegPathRcpConInsp_Uper, LamiSystem.StrLstRcpConInspTitle_Uper[i]);
                LamiSystem.StrLstRcpConInspData_Uper.Add(strRegisterGridData);
            }

            Recipe_Config_Inspect_Output_Uper();
        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void Recipe_Config_Register_To_Lists_Inspect_Down()
        {
            Recipe_Config_Register_To_List_Inspect_Down();
            LamiSystem.StrLstRcpConInspData_Down.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_Down.Count; i++)
            {
                string strRegisterGridData = Recipe_Config_Register_To_List_Inspect_Down(LamiSystem.RegPathRcpConInsp_Down, LamiSystem.StrLstRcpConInspTitle_Down[i]);
                LamiSystem.StrLstRcpConInspData_Down.Add(strRegisterGridData);
            }

            Recipe_Config_Inspect_Output_Down();
        }

        /*
        /// <summary>
        /// 기존에 저장되어져 있는 문자열 2차원 배열에서 데이터를 읽어와서
        /// 레시피 설정 그리드에 데이터를 표시한다.
        /// 레시피셋업그리드 사이즈 : 420, 170
        /// </summary>
        private void RecipeBiCell_Config_Inspect_Output()
        {
            int ItemCount = 14;

            int rectCount = LamiSystem.StrLstRcpConInspTitle_BiCell.Count / ItemCount;
            _BiCell_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_BiCell.Clear();

            System.Drawing.Rectangle tempRectOld = new System.Drawing.Rectangle(0, 0, 0, 0);
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < rectCount; i++)
            {
                tempRectNew.X = int.Parse(LamiSystem.StrLstRcpConInspData_BiCell[(i * ItemCount) + 0]);
                tempRectNew.Y = int.Parse(LamiSystem.StrLstRcpConInspData_BiCell[(i * ItemCount) + 1]);
                tempRectNew.Width = int.Parse(LamiSystem.StrLstRcpConInspData_BiCell[(i * ItemCount) + 2]);
                tempRectNew.Height = int.Parse(LamiSystem.StrLstRcpConInspData_BiCell[(i * ItemCount) + 3]);

                LamiSystem.RectListRecipeBoxZone_BiCell.Add(tempRectNew);

                if (tempRectOld != tempRectNew)
                {
                    _BiCell_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_BiCell[i]);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_BiCell[i].Left);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_BiCell[i].Right);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_BiCell[i].Width);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_BiCell[i].Height);
                    tempRectOld = tempRectNew;
                }
            }
        }
        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string RecipeBiCell_Config_Register_To_List_Inspect(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void RecipeBiCell_Config_Register_To_List_Inspect()
        {
            Register_To_StringList(LamiSystem.RegPathRcpConInsp_BiCell, ref LamiSystem.StrLstRcpConInspTitle_BiCell);
        }
        */

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void Recipe_Config_Register_To_List_Inspect_Uper()
        {
            Register_To_StringList(LamiSystem.RegPathRcpConInsp_Uper, ref LamiSystem.StrLstRcpConInspTitle_Uper);
        }
        private void Recipe_Config_Register_To_List_Inspect_Down()
        {
            Register_To_StringList(LamiSystem.RegPathRcpConInsp_Down, ref LamiSystem.StrLstRcpConInspTitle_Down);
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string Recipe_Config_Register_To_List_Inspect_Uper(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }
        public string Recipe_Config_Register_To_List_Inspect_Down(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intRecipeUperRectCellCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void Recipe_Config_File_To_Register_Inspect_Uper()
        {
            _intRecipeUperRectCellCount = 0;
            //Rources에 들록되어 있는 파일을 사용한다.
            //byte[] resourceObject = SystemAlign.Properties.Resources.RecipeGapInspectAreaDefault;
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeInspectAreaUper;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Recipe_Config_StringParsing_Inspect_Uper(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Recipe_Config_StringParsing_Inspect_Uper(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathRcpConInsp_Uper, _intRecipeUperRectCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intRecipeUperRectCellCount++;
            }
        }


        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intRecipeDownRectCellCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void Recipe_Config_File_To_Register_Inspect_Down()
        {
            _intRecipeDownRectCellCount = 0;
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeInspectAreaDown;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Recipe_Config_StringParsing_Inspect_Down(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Recipe_Config_StringParsing_Inspect_Down(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathRcpConInsp_Down, _intRecipeDownRectCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intRecipeDownRectCellCount++;
            }
        }

        /// 메임폼에서 사용하는 이벤트 핸들러.

        #region 메인폼 : 이벤트 핸들러

        /// <summary>
        /// 툴바의 오픈 버튼 클릭에 해한 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnToolbarOpen_Click(object sender, EventArgs e)
        {
            FormDlgModelReg dlgModelReg = new FormDlgModelReg();
            dlgModelReg.GetSet_LamiSystem = LamiSystem;
            dlgModelReg.ModelAdding += new ModelRegEvent1(eventing_ModelAdding);

            dlgModelReg.GetSetModelName = LamiSystem.GetSet_Now_Model_Name;
            dlgModelReg.GetSetModelNumber = LamiSystem.GetSet_Now_Model_Number;

            if (dlgModelReg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                string OldName = LamiSystem.GetSet_Now_Model_Name + " " + LamiSystem.GetSet_Now_Model_Number;
                string NowName = dlgModelReg.GetSetModelName + " " + dlgModelReg.GetSetModelNumber;

                if (OldName != NowName) LamiSystem.GetSet_Model_Changed_Flag = true;
                else LamiSystem.GetSet_Model_Changed_Flag = false;

                LamiSystem.GetSet_Now_Model_Name = dlgModelReg.GetSetModelName;
                LamiSystem.GetSet_Now_Model_Number = dlgModelReg.GetSetModelNumber;

                Model_Change_Apply();

                //모델을 변경하면 세이브 기능도 구현하다록 해준다.
                Model_Change_Data_Save();


                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            if (dlgModelReg.IsDisposed == false) dlgModelReg.Dispose();
            this.Activate();
            this.Focus();
        }

        private Thread ModelChang_threading;
        private void Model_Changing_Display()
        {
            ModelChang_threading = new Thread(Display_Model_Changing_ProgressBar);
            ModelChang_threading.Start();
        }

        private void Display_Model_Changing_ProgressBar()
        {
            int progValue = 2;
            while (progValue > 0)
            {
                Thread.Sleep(10);
                progValue += 2;
                if (progValue > 100) progValue = 2;
                //Display_Model_Changing_Progress(progValue);
            }
        }

        private delegate void Delegate_Model_Changing_Progress(string iValue);

        private void Display_Model_Changing_Progress(string iValue)
        {
            if (InvokeRequired)
            {
                Delegate_Model_Changing_Progress del = Display_Model_Changing_Progress;
                Invoke(del, iValue);
            }
            else
            {
                //picBox01.Visible = true;
                ustbar.Text  = iValue;
                //picBox01.ImageIpl = inspImage;
                ustbar.Refresh();
            }
        }

        //20150304 WKB 209
        /// <summary>
        /// 툴바의 저장 버튼 클릭에 대한 밴들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnToolbarSave_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 10);
            //시스템 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            System_Config_ListData_To_Register();

            //비전부 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            Vision_Config_ListData_To_Register();
            Vision_Config_UperGrid_To_Register();
            Vision_Config_DownGrid_To_Register();
            
            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 20);

            //레시피 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Recipe_Config_ListData_To_Register();
            Recipe_Config_UperGrid_To_Register();
            Recipe_Config_DownGrid_To_Register();

            //레시피 설정 탭에서 적용되어져 있는 ROI 정보를 레지스터에 기록한다.
            Recipe_Config_UperInspect_To_Register();
            Recipe_Config_DownInspect_To_Register();

            //환경 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Equipment_Config_ListData_To_Register();
            
            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 30);

            //현재 적용되어져 있는 리스트 배열의 값을 현재 모델의 파일에 저장한다.
            Model_Config_Add_List_To_File(LamiSystem.GetSet_Now_Model_Name, LamiSystem.GetSet_Now_Model_Number);

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        //20150304 WKB 208
        /*
        private void ubtnToolbarSave_Click(object sender, EventArgs e)
        {
            //var messageBox = new Control_UltraMessageBox(); 
            //DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("MainToolbarSaveCaption"),
            //    rm.GetString("MainToolbarSaveHeader"), rm.GetString("MainToolbarSaveContent"),
            //    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (dlgResult == DialogResult.Cancel) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 10);
            //시스템 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            System_Config_ListData_To_Register();

            //비전부 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            Vision_Config_ListData_To_Register();
            Vision_Config_UperGrid_To_Register();
            Vision_Config_DownGrid_To_Register();
            
            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 20);

            //레시피 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Recipe_Config_ListData_To_Register();
            Recipe_Config_UperGrid_To_Register();
            Recipe_Config_DownGrid_To_Register();

            //레시피 설정 탭에서 적용되어져 있는 ROI 정보를 레지스터에 기록한다.
            Recipe_Config_UperInspect_To_Register();
            Recipe_Config_DownInspect_To_Register();

            //환경 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Equipment_Config_ListData_To_Register();
            
            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 30);

            //현재 적용되어져 있는 리스트 배열의 값을 현재 모델의 파일에 저장한다.
            Model_Config_Add_List_To_File(LamiSystem.GetSet_Now_Model_Name, LamiSystem.GetSet_Now_Model_Number);

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */
        /// <summary>
        /// 
        /// </summary>
        Control_UltraMessageBox uMessageBox = new Control_UltraMessageBox();
        /// <summary>
        /// 메임 폼의 메인 탭을 선택 했을 때의 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static readonly string TabName_System = "System";
        static readonly string TabName_Vision = "Vision";
        static readonly string TabName_Recipe = "Recipe";
        static readonly string TabName_Log = "LogData";
        static readonly string TabName_Camera = "Camera";
        static readonly string TabName_Teach = "Teach";

        //20150305 WKB 209
        private void utabDlgMain_SelectedTabChanged(object sender,Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (_boolUserSelected_TabChange == true) return;
            if (utabDlgMain.ActiveTab == null) return;
            if (utabDlgMain.ActiveTab.Key == null) return;

            switch (this.utabDlgMain.ActiveTab.Key)
            {
                case "System":
                    //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    System_Config_Initionalize();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MainForm_ProgracessBar_Display_01("Model System Data Load Complite !", 100);
                    break;

                case "Vision":
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    VisionLami_Config_Initionalize();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MainForm_ProgracessBar_Display_01("Model Vision Data Load Complite !", 100);
                    break;

                case "Recipe":
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    //선택한 레시피 항목중 중복된 것을 찾는다.
                    //Selected_Recipe_GridCheck();

                    RecipeGap_Config_Initionalize();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MainForm_ProgracessBar_Display_01("Model Recipe Data Load Complite !", 100);
                    break;

                case "Config":
                    break;

                case "LogData":
                    break;
                    
                case "Camera":
                    break;
            }
        }


        //20150305 WKB 208
        /*
        ly string TabName_Teach = "Teach";
        private void utabDlgMain_SelectedTabChanged(object sender,Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (_boolUserSelected_TabChange == true) return;
            if (utabDlgMain.ActiveTab == null) return;
            if (utabDlgMain.ActiveTab.Key == null) return;

            switch (this.utabDlgMain.ActiveTab.Key)
            {
                case "System":

                    //2015-01-05-01 wkb
//                     RecipeGrid_Data_Saving();
//                     if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false 
//                         || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false) return;

                    //2015-01-05-01 Start wkb
//                     RecipeGrid_Data_Saving();
// 
//                     if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false
//                         || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false)
//                     {
//                         if (utabDlgMain.Tabs["Recipe"].Visible == true)
//                             this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Recipe"];
//                         return;
//                     }
                    //2015-01-05-01 Finish wkb

                    //레지스테에 설정값이 없다면 파일의 데이터를 레지스터에 저장한다.
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    System_Config_Initionalize();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MainForm_ProgracessBar_Display_01("Model System Data Load Complite !", 100);
                    break;

                case "Vision":

                    //2015-01-05-01 wkb
//                     RecipeGrid_Data_Saving();
//                     if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false
//                         || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false) return;

                    //2015-01-05-01 Start wkb
//                     RecipeGrid_Data_Saving();
// 
//                     if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false
//                         || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false)
//                     {
//                         if (utabDlgMain.Tabs["Recipe"].Visible == true)
//                             this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Recipe"];
//                         return;
//                     }
                    //2015-01-05-01 Finish wkb

                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    VisionLami_Config_Initionalize();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MainForm_ProgracessBar_Display_01("Model Vision Data Load Complite !", 100);
                    break;

                case "Recipe":
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    //선택한 레시피 항목중 중복된 것을 찾는다.
                    Selected_Recipe_GridCheck();

                    RecipeGap_Config_Initionalize();
                    //Recipe_Config_Inspect_Output_Uper();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MainForm_ProgracessBar_Display_01("Model Recipe Data Load Complite !", 100);
                    break;

                case "Config":
                    break;

                case "LogData":
                    break;
                    
                case "Camera":
                    break;
            }
        }
        */

        public void RecipeGrid_Data_Saving()
        {
            if (RecipeGrid_Changed == true)
            {
                Recipe_uBtn_ParamApply.PerformClick();
                RecipeGrid_Changed = false;
            }
        }

//         private void Recipe_Config_Register_To_UperROI()
//         {
//             _intRecipeUperRectCellCount = 0;
//             //Rources에 들록되어 있는 파일을 사용한다.
//             //byte[] resourceObject = SystemAlign.Properties.Resources.RecipeGapInspectAreaDefault;
//             byte[] resourceObject = SystemAlign.Properties.Resources.RecipeInspectAreaUper;
//             System.IO.Stream ioStream = new MemoryStream(resourceObject);
//             using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
//             {
//                 string strLine;
//                 while ((strLine = srFile.ReadLine()) != null)
//                 {
//                     Recipe_Config_StringParsing_Inspect_Uper(strLine);
//                 }
//             }
//             ioStream.Close();
//         }

        private void ubtnToolbarUser_TextChanged(object sender, EventArgs e)
        {
            _boolUserSelected_TabChange = true;

            LamiSystem.GetSet_Now_User_Account = ubtnToolbarUser.Text;
            //NowUserAccount = ubtnToolbarUser.Text;
            //string strNowData = ubtnToolbarUser.Text;
            if (ubtnToolbarUser.Text == Account_Operator)
            {
                utabDlgMain.Tabs["System"].Visible = false;
                utabDlgMain.Tabs["Vision"].Visible = false;
                utabDlgMain.Tabs["Recipe"].Visible = false;
                utabDlgMain.Tabs["Config"].Visible = false;
                utabDlgMain.Tabs["Camera"].Visible = false;
                utabDlgMain.Tabs["LogData"].Visible = true;

                ubtnToolbarOpen.Enabled = false;
                ubtnToolbarSave.Enabled = false;
                ubtnToolbarApply.Enabled = false;
            }
            else if (ubtnToolbarUser.Text == Account_Engineer)
            {
                utabDlgMain.Tabs["System"].Visible = true;
                utabDlgMain.Tabs["Vision"].Visible = true;
                utabDlgMain.Tabs["Recipe"].Visible = true;
                utabDlgMain.Tabs["Config"].Visible = false;
                utabDlgMain.Tabs["Camera"].Visible = false;
                utabDlgMain.Tabs["LogData"].Visible = true;
                
                ubtnToolbarOpen.Enabled = true;
                ubtnToolbarSave.Enabled = true;
                ubtnToolbarApply.Enabled = true;
            }
            else if (ubtnToolbarUser.Text == Account_Maker)
            {
                utabDlgMain.Tabs["System"].Visible = true;
                utabDlgMain.Tabs["Vision"].Visible = true;
                utabDlgMain.Tabs["Recipe"].Visible = true;
                utabDlgMain.Tabs["Config"].Visible = true;
                utabDlgMain.Tabs["Camera"].Visible = true;
                utabDlgMain.Tabs["LogData"].Visible = true;
                
                ubtnToolbarOpen.Enabled = true;
                ubtnToolbarSave.Enabled = true;
                ubtnToolbarApply.Enabled = true;
            }

            _boolUserSelected_TabChange = false;
        }
        
        private void ubtnToolbarUser_Click(object sender, EventArgs e)
        {
            dlgUserSelect = new FormDlgUserSelect();
            dlgUserSelect.UserSelecting += new UserSelectEvent1(eventing_UserSelecting);

            if (dlgUserSelect.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                LamiSystem.GetSet_Now_Login_Time = DateTime.Now;

                LamiSystem.GetSet_Now_User_Account = dlgUserSelect.SelectedAccount;
                UserSelect_NowAccount_Check(LamiSystem.GetSet_Now_User_Account);
                System_Status_NowAccount_View_To_Reg();

                if (LamiSystem.GetSet_Now_User_Account == Account_Engineer || LamiSystem.GetSet_Now_User_Account == Account_Maker)
                    this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["System"];

                User_Login_Data_Write_To_File(1);
                this.Select();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private Control_Files fileSystem;// = new Control_Files();
        //[2011-11-19 10:17:22.56] [-01] [3-NOR] Operator로 Login 되었습니다.   (TitleForm.cpp, 339)
        public void User_Login_Data_Write_To_File(int writeType)
        {
            

            DateTime logTime = DateTime.Now;

            string folderName = Environment.CurrentDirectory + "\\Log\\Login\\" +
                                String.Format("{0:00}년{1:00}월", logTime.Year, logTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}-{1:00}-{2:00} Align Login.log", logTime.Year, logTime.Month,
                                  logTime.Day);

            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);
            //해당 경로에 파일이 있는지 검사해서 없으면 생성한다.
            //Param 1 : 조사할 경로, Param 2 : 파일이 종류(1:login, 2:Inspect, 3:Excel)
            //fileSystem.File_IO_Text_File_Check_Or_Make(fileName, 1);
            fileSystem.File_IO_Text_File_Check_Or_Make_Login(fileName);

            string logData = string.Empty;

            if (writeType == 1)
            {
                logData = String.Format("[{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}] ", logTime.Year,
                    logTime.Month, logTime.Day, logTime.Hour, logTime.Minute, logTime.Second, logTime.Millisecond);
                logData += LamiSystem.GetSet_Now_User_Account + " 계정으로 로그인 되었습니다.\r\n";
            }
            //fileSystem.File_IO_Text_File_Write(logData,1);
            //fileSystem.File_IO_Text_File_Write(fileName, logData, 1);
            fileSystem.File_IO_Text_File_Write_Login(fileName, logData);
            utxtSysLog001.Text += logData;
        }

        /*
         * 
        public void User_Login_Data_Write_To_File(int writeType)
        {
            Control_Files fileSystem = new Control_Files();

            DateTime logTime = DateTime.Now;

            string folderName = Environment.CurrentDirectory + "\\Log\\Login\\" +
                                String.Format("{0:00}년{1:00}월", logTime.Year, logTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}-{1:00}-{2:00} Align Login.log", logTime.Year, logTime.Month,
                                  logTime.Day);

            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);
            //해당 경로에 파일이 있는지 검사해서 없으면 생성한다.
            //Param 1 : 조사할 경로, Param 2 : 파일이 종류(1:login, 2:Inspect, 3:Excel)
            fileSystem.File_IO_Text_File_Check_Or_Make(fileName);

            StreamWriter strLoginFile = new StreamWriter(fileName, true, Encoding.Default);
            strLoginFile.Flush();

            string logData = string.Empty;

            if (writeType == 1)
            {
                logData = String.Format("[{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}] ", logTime.Year,
                    logTime.Month, logTime.Day, logTime.Hour, logTime.Minute, logTime.Second, logTime.Millisecond);
                logData += AlignSystem.GetSet_Now_User_Account + " 계정으로 로그인 되었습니다.";
            }
            fileSystem.File_IO_Text_File_Write(logData);
            strLoginFile.WriteLine(logData);
            utxtSysLog001.Text += logData + "\r\n";
            strLoginFile.Close();
        }
        //[2011-11-19 10:17:22.56] [-01] [3-NOR] Operator로 Login 되었습니다.   (TitleForm.cpp, 339)
        public void User_Login_Data_Write_To_File(int writeType)
        {
            DateTime _strTime = DateTime.Now;
            string strLogData = String.Format("{0:00}-{1:00}-{2:00}-Login.log", _strTime.Year, _strTime.Month, _strTime.Day);
            string addModelFileName = Environment.CurrentDirectory + "\\Log\\Login\\" + strLogData;
            StreamWriter strLoginFile = new StreamWriter(addModelFileName, true, Encoding.Default);
            strLoginFile.Flush();


            if (writeType == 1)
            {
                strLogData = String.Format("[{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}] ", _strTime.Year,
                _strTime.Month, _strTime.Day, _strTime.Hour, _strTime.Minute, _strTime.Second, _strTime.Millisecond);
                strLogData += AlignSystem.GetSet_Now_User_Account + " 계정으로 로그인 되었습니다.";
            }
            strLoginFile.WriteLine(strLogData);
            utxtSysLog001.Text += strLogData + "\r\n";
            strLoginFile.Close();
        }
        */

        public void System_Operation_Data_Write_To_File(int writeType)
        {
            DateTime _strTime = DateTime.Now;
            string strLogData = String.Format("{0:00}-{1:00}-{2:00}-System.log", _strTime.Year, _strTime.Month, _strTime.Day);
            string addModelFileName = Environment.CurrentDirectory + "\\Log\\System\\" + strLogData;
            StreamWriter strLoginFile = new StreamWriter(addModelFileName, true, Encoding.Default);
            strLoginFile.Flush();

            strLogData = String.Format("[{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}] ", _strTime.Year,
               _strTime.Month, _strTime.Day, _strTime.Hour, _strTime.Minute, _strTime.Second, _strTime.Millisecond);
            
            strLogData += System_Operation_Data_Make_Message(writeType);

            strLoginFile.WriteLine(strLogData);
            utxtSysLog001.Text += strLogData + "\r\n";
            strLoginFile.Close();
            strLoginFile.Dispose();
        }

        /*
         * [03/01 10:21:11.450] =====프로그램 시작========
[03/01 10:21:33.056] 촬영신호출력
[03/01 10:21:33.103] ----------- ALign 촬영완료 -----------------------
[03/01 10:21:33.103] 검사시작
[03/01 10:21:33.103] Bi-Cell 정보 2
1

[03/01 10:21:33.103] (Trigger Pos=1)Gripper ID A2
[03/01 10:21:33.103] 좌측변 찾는위치 => 상부
[03/01 10:21:33.103] nCam=0 검사완료
[03/01 10:21:33.103] 위치 UMac CMD 시작 : P375=0.00000P376=0.00000P377=-180.000000P378=0.000000P379=0.000000P1165=0.000000P1166=0.000000P1171=0.000000P1172=0.000000P371=1000.00000P372=1000.00000P373=1000.000000P374=0.000000P391=1'
[03/01 10:21:33.103] 위치 UMac 보정값 전송완료(2.08 sec) xy(1000.0000 1000.0000) 1000.00000'
[03/01 10:21:33.134] SaveImage D:\image\Cam0\2012-03-01/ ( Trigger 00001 = Pos(01) ) 2012년03월01일 10시21분33.134초.jpg
        */

        public string System_Operation_Data_Make_Message(int TypeNo)
        {
            string msgData = string.Empty;

            if (TypeNo == 1) msgData = "검출 프로그램 기동 시작";
            else if (TypeNo == 2) msgData =  "트리거 신호 입력";
            else if (TypeNo == 3) msgData =  "이미지 그랩 진행";
            else if (TypeNo == 3) msgData =  "이미지 정보 취합";
            else if (TypeNo == 4) msgData =  "이미지 분석 진행";
            else if (TypeNo == 5) msgData =  "분석 결과값 전송";
            else if (TypeNo == 6) msgData =  "이미지 저장 진행";

            return "\t" + msgData + "\r\n";
        }

        //20150304 WKB 209
        private void ubtnToolbarInspect_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //20150304 WKB 209
            //2015-01-05-01 Start wkb
            RecipeGrid_Data_Saving();

            bool ComStatus = true;
            List<string> NGCom = new List<string>();
            NGCom.Add("Camera");
            NGCom.Add("PLC");
            NGCom.Add("UMAC");
            NGCom.Add("LVS");

            for (int i = 0; i < LamiSystem.SystemStatusFlag.Length; i++)
            {
                if (LamiSystem.SystemStatusFlag[i] == false)
                {
                    ComStatus = false;
                    break;
                }
            }

            if (ComStatus == false)
            {
                uMessageBox.SystemConnectStatus(NGCom, LamiSystem.SystemStatusFlag);
                //return;
            }

            //dlgInspect = new FormDlgInsp();//LamiSystem, _point_Converter_Gap, _Gap_Control_DrawArea, _point_Converter_BiCell, _BiCell_Control_DrawArea);
            //dlgInspect.OperationEvent += new MyEventOneInsp(Get_PLC_Now_ModelName_RecipeNo);

            dlgInspect_Object_Setting();

            if (Pix_Load.Checked == true) dlgInspect.GetSet_PixLoad = true;
            else dlgInspect.GetSet_PixLoad = false;

            dlgInspect.ShowDialog();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        //20150304 WKB 208
        /*
        private void ubtnToolbarInspect_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //20150304 WKB 209
            //2015-01-05-01 Start wkb
            //RecipeGrid_Data_Saving();

            if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false
                || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false)
            {
                if (utabDlgMain.Tabs["Recipe"].Visible == true)
                    this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Recipe"];
                return;
            }
            //2015-01-05-01 Finish wkb

            bool ComStatus = true;
            List<string> NGCom = new List<string>();
            NGCom.Add("Camera");
            NGCom.Add("PLC");
            NGCom.Add("UMAC");
            NGCom.Add("LVS");

            for (int i = 0; i < LamiSystem.SystemStatusFlag.Length; i++)
            {
                if (LamiSystem.SystemStatusFlag[i] == false)
                {
                    ComStatus = false;
                    break;
                }
            }

            if (ComStatus == false)
            {
                uMessageBox.SystemConnectStatus(NGCom, LamiSystem.SystemStatusFlag);
                //return;
            }

           

            //dlgInspect = new FormDlgInsp();//LamiSystem, _point_Converter_Gap, _Gap_Control_DrawArea, _point_Converter_BiCell, _BiCell_Control_DrawArea);
            //dlgInspect.OperationEvent += new MyEventOneInsp(Get_PLC_Now_ModelName_RecipeNo);

            dlgInspect_Object_Setting();
            dlgInspect.ShowDialog();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */
         //Inspect_MeaData_Uper = dlgInspect.uDS_Inspect_Measure_Uper;
            //Inspect_MeaData_Down = dlgInspect.uDS_Inspect_Measure_Down;
            //Inspect_MeaChart_Uper = dlgInspect.Uper_MeasureTables;
            //Inspect_MeaChart_Down = dlgInspect.Down_MeasureTables;

        public void dlgInspect_Object_Setting()
        {
            dlgInspect.GetSet_FileSystem = fileSystem;
            dlgInspect.GetSet_NowModel_Name = _strNow_Model_Name;
            dlgInspect.GetSet_NowModel_Number = _strNow_Model_Number;
            dlgInspect.GetSet_UMAC_System = umac;
            dlgInspect.GetSet_PLC_System = plc;
            dlgInspect.GetSet_LVS_System = lvs;
            dlgInspect.GetSet_LamiSystem = LamiSystem;
            dlgInspect.GetSet_Converter_Uper = _point_Converter_Uper;
            dlgInspect.GetSet_Converter_Down = _point_Converter_Down;
            dlgInspect.Inspect_MIL_Initialize(MilApplication, MilSystem_Uper, MilDisplay_Uper, MilDigitizer_Uper, MilImage_Uper, MilDisplay_Down, MilDigitizer_Down, MilImage_Down);
        }

        #endregion

        public void eventing_PixLoad(int Garo, int Sero)
        {
            Trace.WriteLine(Garo.ToString() + " " + Sero.ToString());
        }
        private void Get_PLC_Now_ModelName_RecipeNo(int opNo)
        {
            Get_PLC_Model_Name();

            //PLC로 부터 읽은 모델명을 시스템의 모델명과 비교해서 다르다면 
            //시스템에서 모델 파일을 찾아보고 있다면 이를 적용한다.
            //만약 모델 파일에 읽은 모델명이 없다면 메세지 표시 후 종료한다.

#if(SIMPLC)
            //NowPLCReadModelName[0] = "T 80";   //"P 1.0"
            //NowPLCReadModelName[1] = "10";       //"1"
            NowPLCReadModelName[0] = LamiSystem.GetSet_Now_Model_Name;
            NowPLCReadModelName[1] = LamiSystem.GetSet_Now_Model_Number;
#endif        
            if ((LamiSystem.GetSet_Now_Model_Name != NowPLCReadModelName[0]) || (LamiSystem.GetSet_Now_Model_Number != NowPLCReadModelName[1]))
            {
                string modelName = NowPLCReadModelName[0] + "-" + NowPLCReadModelName[1] + ".rcp";
                string strModelFolderName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelName;
                //string strModelFolderName = Environment.CurrentDirectory + "\\ModelData\\" + modelName;

                var logFileInfo = new FileInfo(strModelFolderName);
                if (logFileInfo.Exists == false)
                {
                    //NowPlcModelAlram 결과 같이 저장된다. 0:대기상태, 1:알람상태
                    //알람영역을 읽어와서 이미 1로 되어 있는지 검사한다.
                    Get_PLC_Model_Alram();
                    if (NowPlcModelAlram == "0")
                    {
                        //NowPlcWriteResponse 변수에 쓰기 결과 같이 저장된다. 
                        //0:정상, 나머지:오류
                        Set_PLC_Model_Alram();
                    }
                    else if (NowPlcModelAlram == "1")
                    {
                        //이미 알람이 셋 되어 있는 경우
                    }


                    Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                    messageBox.MessageBox_Show("레시피 로딩", "모델 레시피 오류 !", "PLC에서 설정한 모델 " + NowPLCReadModelName[0] + "-" + NowPLCReadModelName[1] +
                    "은 등록되지 않은 모델입니다.\r\n관리자에게 확인하여 주십시요 !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LamiSystem.GetSet_Now_Model_Name = NowPLCReadModelName[0];
                LamiSystem.GetSet_Now_Model_Number = NowPLCReadModelName[1];

                Model_Change_Apply();

                //세이브 버튼과 같은 함수를 호출하도록 수정함.
                Model_Change_Data_Save();

                Display_Model_Changing_Progress("Model Change Complite !");
            }

            //인스펙션의 검사를 시작한다.
            //이곳에서 다시 인스펙션 클래스로 돌아간다.
            //현재 모델 정보를 나타냄
            dlgInspect.Inspect_uLabel_Assy04.Text = LamiSystem.GetSet_Now_Model_Name + " - " +
                                         LamiSystem.GetSet_Now_Model_Number;
            dlgInspect.Inspect_Thread_Starting();
        }

        private void Set_PLC_Alram_Data()
        {
            //plc.
        }

        private void Model_Change_Data_Save()
        {
            //시스템 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            System_Config_ListData_To_Register();

            //비전부 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            Vision_Config_ListData_To_Register();
            Vision_Config_UperGrid_To_Register();
            
            //20150315 WKB 209
            Vision_Config_DownGrid_To_Register();

            //레시피 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            Recipe_Config_ListData_To_Register();
            
            Recipe_Config_UperGrid_To_Register();
            Recipe_Config_DownGrid_To_Register();

            Recipe_Config_UperInspect_To_Register();
            Recipe_Config_DownInspect_To_Register();

            //환경 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Equipment_Config_ListData_To_Register();

            //현재 적용되어져 있는 리스트 배열의 값을 현재 모델의 파일에 저장한다.
            Model_Config_Add_List_To_File(LamiSystem.GetSet_Now_Model_Name, LamiSystem.GetSet_Now_Model_Number);
        }

        //PCL_ReadData_D12000
        private string NowPlcModelAlram = string.Empty;
        private int NowPlcWriteResponse = -1;
        private string NowPlcReadData = string.Empty;
        private void Get_PLC_Model_Alram()
        {
#if(SIMPLC)
            NowPlcReadData = plc.PCL_ReadData_D12000();
            if (NowPlcReadData != string.Empty && NowPlcReadData.Length > 0)
                NowPlcModelAlram = NowPlcReadData.Substring(0, 1);
            else PLC_Error_MessageBox_Show(MethodBase.GetCurrentMethod().Name);

#else
            NowPlcModelAlram = "0";
#endif
        }

        private void PLC_Error_MessageBox_Show(string msgNo)
        {
            string strMsgOne = string.Empty;
            string strMsgTwo = string.Empty;
            string strMsgThe = string.Empty;

            if (msgNo == "Get_PLC_Model_Alram")
            {
                strMsgOne = "PLC 통신 오류";
                strMsgTwo = "오류 발생 함수 : " + msgNo;
                strMsgThe = "PLC로 부터 알람 상태 메모리 영역에 엑세스 하지 못했습니다!\r\n관리자에게 확인하여 주십시요 !";
            }
            var messageBox = new Control_UltraMessageBox();
            messageBox.MessageBox_Show(strMsgOne, strMsgTwo, strMsgThe, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void Set_PLC_Model_Alram()
        {
#if(SIMPLC)
            NowPlcWriteResponse = plc.PCL_WriteData_D12000();
#else
            NowPlcWriteResponse = 0;
#endif
        }

        private string[] NowPLCReadModelName = {string.Empty, string.Empty};
        private void Get_PLC_Model_Name()
        {
            //NowPLCReadModelName = plc.PCL_ReadData_ModelName();
        }
        /// 대화상자를 사용하는 메소드가 주임.

        #region 메인폼 : 공용 메소스

        /// <summary>
        /// 폴더 선택 대화상자를 활성화한다.
        /// </summary>
        /// <returns>
        /// 대화상자에서 선택한 폴더의 경로를 반환한다.
        /// </returns>
        private string FolderBrowser_Open()
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            return dialog.SelectedPath;
        }

        /// <summary>
        /// 파일 선택 대화상자를 활성화한다.
        /// </summary>
        /// <returns>
        /// 선택한 파일의 전체 경로를 반환다.
        /// </returns>
        private string FileDialog_Open()
        {
            String strFullPathFile = null;
            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                strFullPathFile = pFileDlg.FileName;
                this.Select();
            }
            return strFullPathFile;
        }

        //프로그레스 바의 상태값을 나타낸다.
        private void PrograssBar_Display(int nowData, int TotalData)
        {
            this.uprog.Value = (int) (((float) nowData/(float) TotalData)*100);
        }


        //1번 파라미터의 경로 아래에 있는  
        //2번 파라미터와 같은 이름의 레지스터 항목을
        //3번 파라미터 값으로 저장한다.
        public void SetReg(string strNodePath, string strName, string strData)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(strNodePath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(strName, strData, RegistryValueKind.String);
            reg.Close();
        }

        //1번 파라미터의 경로 아래에 있는  
        //2번 파라미터와 같은 이름의 레지스터 항목의 값을 리턴한다.
        public string GetReg(string strNodePath, string regName)
        {
            RegistryKey reg = Registry.CurrentUser;

            reg = reg.OpenSubKey(strNodePath, true);
            string regData = reg.GetValue(regName, "").ToString();
            reg.Close();
            return regData;
        }


        //1번 파라미터의 경로 아래에 있는 서브 레지스트리 항목 값을 
        //2번 파라미터의 리스트에 저장한다.
        private void Register_To_StringList(string regNodePath, ref List<string> stringList)
        {
            stringList.Clear();

            // 서브키를 얻어온다.
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(regNodePath, true);
            // 없으면 함수 종료
            if (rk == null) return;
            foreach (string s in rk.GetValueNames())
            {
                stringList.Add(s);
            }
            rk.Close();
        }

        private void UserSelect_NowAccount_Check(string account)
        {
            dlgLogin = new FormDlgLogin();
            dlgLogin.LoginComplite += new LoginCompliteEvent1(eventing_LoginComplite);

            if (account == Account_Operator)
            {
                ubtnToolbarUser.Text = Account_Operator;

            }
            else if (account == Account_Engineer)
            {
                if (dlgLogin.ShowDialog() == DialogResult.OK)
                {
                    NowUserId = dlgLogin.GetSetInputID;
                    NowUserPass = dlgLogin.GetSetInputPass;
                    ubtnToolbarUser.Text = Account_Engineer;
                }
            }
            else if (account == Account_Maker)
            {
                if (dlgLogin.ShowDialog() == DialogResult.OK)
                {
                    NowUserId = dlgLogin.GetSetInputID;
                    NowUserPass = dlgLogin.GetSetInputPass;
                    ubtnToolbarUser.Text = Account_Maker;
                }
                else return;
            }
            else if (account == Account_Password)
            {
                if (dlgLogin.ShowDialog() == DialogResult.OK)
                {
                    dlgPasswordEdit = new FormDlgPassEdit();
                    dlgPasswordEdit.PasswordEditing += new PasswordEditEvent1(eventing_PasswordEditing);
                    dlgPasswordEdit.ShowDialog();
                }
                else return;
            }
            this.Select();
        }


        private void eventing_UserSelecting(string selectedAccount)
        {
            LamiSystem.GetSet_Now_User_Account = selectedAccount;
        }


        #endregion

        /// 메임폼에서 사용하는 기타 함수.

        #region 메인폼 : 기타 함수

        #endregion

        #endregion
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// 시스템 설정 탭을 운용하고 제어함.//////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region 시스템 설정 탭

        /// 시스템 설정 탭의 멤버스.

        #region 시스템 설정 탭 : Members
        #endregion

        /// 시스템 설정 탭의 프로퍼티.

        #region 시스템 설정 탭 : 프로퍼티

        #endregion

        /// 시스템 설정 탭의 생성자, 초기화.

        #region 시스템 설정 탭 : 생성자, 초기화

        /// 콘피그 저장 레지스트리가 비어 있는지 확인해서 비어 있다면
        /// 텍스트파일에서 디폴트 설정값을 읽어와서 레지스트리에 저장한다.
        private void System_Config_Initionalize()
        {
            MainForm_ProgracessBar_Display_01("System Config Loading !", 50);
            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            System_Config_ListData_To_Viewer();
        }

        #endregion

        /// 시스템 설정 탭의 이벤트 핸들러.

        #region 시스템 설정 탭 : 이벤트 핸들러


        //이미지 저장 버튼의 클릭 이벤트 핸들러
        private void System_uBtn_ImagePath_Click(object sender, EventArgs e)
        {
            System_uTxt_ImagePath_Gap.Text = FolderBrowser_Open();
        }

        //측정값 저장 버튼의 클릭 이벤트 핸들러
        private void System_uBtn_MeasPath_Click(object sender, EventArgs e)
        {
            System_uTxt_MeasPath_Gap.Text = FolderBrowser_Open();
        }

        //설정값 초기화 버튼의 클릭 이벤트 핸들러
        private void System_uBtn_SetupInit_Click(object sender, EventArgs e)
        {
            Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
            DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("SystemMessage01Catipion"),
                rm.GetString("SystemMessage01Header"), rm.GetString("SystemMessage01Content"),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dlgResult == DialogResult.Cancel) return;

            //디폴트 시스템 설정 정보 파일을 읽어서 리스트배열에 저장한다.
            System_Config_File_To_Lists();

            //리스트배열의 값을 읽어와서 표시해준다.
            System_Config_ListData_To_Viewer();
        }

        //설정값 적용 버튼의 클릭 이벤트 핸들러
        private void System_uBtn_SetupApply_Click(object sender, EventArgs e)
        {
            //Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
            //DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("SystemMessage02Catipion"),rm.GetString("SystemMessage02Header"), rm.GetString("SystemMessage02Content"),MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            //if (dlgResult == DialogResult.Cancel) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            System_Config_Viewer_To_List_Data();

            //시리얼 포트를 통해서 조명 제어기를 설정한다.
#if(SYST_SIMUL)

#else
            System_BackLight_Setup();
#endif
            //텍스트 파일에 타이틀과 설정값을 저장하는 곳이지만
            //최초에만 사용하는 기능으로 남겨놓는다.

            #region System_Config_Save_To_File()

            //System_Config_Save_To_File();

            #endregion

            ubtnToolbarSave.PerformClick();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        
        public void System_BackLight_Setup()
        {
            Port_Initialize("2");
            Port_Open();
            string SetValue1 = "";
            string SetValue2 = "";

            if (System_uCombo_EtcItem01.Text == "ON")
                SetValue1 = "200,";
            else
                SetValue1 = "000,";

            if (System_uCombo_EtcItem02.Text == "ON")
                SetValue2 = "200";
            else
                SetValue2 = "000";

            LVS_Set_BackLight(SetValue1, SetValue2);

            Port_Close();
        }
        

        
        public void LVS_Set_BackLight_CH3_OP(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x33;      //채널>> 3
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            sPort1.Write(setZero, 0, setZero.Length);
        }

        public void LVS_Set_BackLight_CH5_JB(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x35;      //채널>> 3
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            sPort1.Write(setZero, 0, setZero.Length);
        }

        public void Port_Initialize(string portNo)
        {
            sPort1.BaudRate = 19200;
            sPort1.DataBits = 8;
            sPort1.StopBits = StopBits.One;
            sPort1.Parity = Parity.None;
            sPort1.PortName = "COM" + portNo;
        }

        public void Port_Open()
        {
            if (sPort1.IsOpen == false)
                sPort1.Open();
        }

        public void Port_Close()
        {
            if (sPort1.IsOpen == true)
                sPort1.Close();
        }

        public void LVS_Set_BackLight(string setValue1, string setValue2)
        {
            byte[] ascii1 = Encoding.ASCII.GetBytes(setValue1);
            byte[] ascii2 = Encoding.ASCII.GetBytes(setValue2);
            //Encoding.ASCII.GetString(ascii);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[25];
            setZero[23] = 0x0D;      //종료>> CR
            setZero[24] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x33;      //채널>> 3

            setZero[3] = ascii1[0];      //값1 >> 0
            setZero[4] = ascii1[1];      //값2 >> 0
            setZero[5] = ascii1[2];      //값3 >> 0
            setZero[6] = 0x2C;      //값1 >> 0

            setZero[7] = 0x30;      //값2 >> 0
            setZero[8] = 0x30;      //값3 >> 0
            setZero[9] = 0x30;      //값1 >> 0
            setZero[10] = 0x2C;      //값1 >> 0

            setZero[11] = ascii2[0];      //값2 >> 0
            setZero[12] = ascii2[1];      //값3 >> 0
            setZero[13] = ascii2[2];      //값1 >> 0
            setZero[14] = 0x2C;      //값1 >> 0

            setZero[15] = 0x34;      //값2 >> 0
            setZero[16] = 0x30;      //값3 >> 0
            setZero[17] = 0x30;      //값1 >> 0
            setZero[18] = 0x2C;      //값1 >> 0

            setZero[19] = 0x34;      //값2 >> 0
            setZero[20] = 0x30;      //값3 >> 0
            setZero[21] = 0x30;      //값1 >> 0
            setZero[22] = 0x2C;      //값1 >> 0

            sPort1.Write(setZero, 0, setZero.Length);
        }

        public void LVS_Set_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //Encoding.ASCII.GetString(ascii);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[12];
            setZero[10] = 0x0D;      //종료>> CR
            setZero[11] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x35;      //채널>> 3
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0
            setZero[6] = ascii[3];      //값1 >> 0
            setZero[7] = ascii[4];      //값2 >> 0
            setZero[8] = ascii[5];      //값3 >> 0
            setZero[9] = ascii[6];      //값1 >> 0

            sPort1.Write(setZero, 0, setZero.Length);
        }

        public void LVS_Set_InSide_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //Encoding.ASCII.GetString(ascii);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x34;      //채널>> 4
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            sPort1.Write(setZero,0,setZero.Length);
        }

        public void LVS_Set_OutSide_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x33;      //채널>> 3
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            sPort1.Write(setZero, 0, setZero.Length);
        }

        /// 시스템 설정 탭의 기타 함수.

        #region 시스템 설정 탭 : 기타 함수

        private void System_Config_File_To_Lists()
        {
//             string filePath = System.Windows.Forms.Application.StartupPath + @"\Data\ConfigFiles\SystemConfigDefault.cfg";
//             string strLine = "";
//             //StreamReader srFile = new StreamReader( filePath, Encoding.GetEncoding( "ks_c_5601-1987" ), true );
//             StreamReader srFile = new StreamReader(filePath, Encoding.Default, true);
//             while ((strLine = srFile.ReadLine()) != null)
//             {
//                 System_Config_File_To_List_TitleNameData(strLine);
//             }
//             srFile.Close();

            byte[] resourceObject = SystemAlign.Properties.Resources.SystemConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    System_Config_File_To_List_TitleNameData(strLine);
                }
            }
            ioStream.Close();
        }
        /*
        //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.SystemConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    System_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        
        //디폴트 설정값 파일을 읽어서 해당 리스트 배열에 저장한다.
        //파일을 한 라인씩 읽어서 파싱하는 함수를 호출한다.
        private void System_Config_File_To_Lists()
        {
            string filePath = System.Windows.Forms.Application.StartupPath + @"\Data\ConfigFiles\SystemConfigDefault.cfg";
            string strLine = "";
            //StreamReader srFile = new StreamReader( filePath, Encoding.GetEncoding( "ks_c_5601-1987" ), true );
            StreamReader srFile = new StreamReader(filePath, Encoding.Default, true);
            while ((strLine = srFile.ReadLine()) != null)
            {
                System_Config_File_To_List_TitleNameData(strLine);
            }
            srFile.Close();

        }
*/
        //문자열 데이터를 탭 문자로 구분하여 3개의 문자열로 분리해서
        //타이틀, 네임, 데이터 리스트에 저장한다.
        private void System_Config_File_To_List_TitleNameData(string readData)
        {
            try
            {
                int intStartIndex = 0;
                int intEndIndex = 0;
                int intTitleCount = 0;
                string strTitleName = null;
                intStartIndex = 0;
                intEndIndex = readData.IndexOf("\t", intStartIndex);
                strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
                LamiSystem.StrListSysConTitle.Add(strTitleName);

                intStartIndex = intEndIndex + 1;
                intEndIndex = readData.IndexOf("\t", intStartIndex);
                strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
                LamiSystem.StrListSysConName.Add(strTitleName);

                intStartIndex = intEndIndex + 1;
                intEndIndex = readData.Length;
                strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
                LamiSystem.StrListSysConData.Add(strTitleName);
            }
            catch (Exception e)
            {
                Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                messageBox.MessageBox_Show("기본 설정 파일 에러", "파일의 형식이 잘못되었습니다.<br/>올바른 설정 파일인지 확인해 주십시요!", e.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void System_Config_Viewer_To_List_Data()
        {
            LamiSystem.StrListSysConData.Clear();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("SystemConfig_Tab_BackPanel", true), ref al);

            for (int i = 0; i < LamiSystem.StrListSysConName.Count; i++)
            {
                for (int j = 0; j < al.Count; j++)
                {
                    string tmpName = ((Control) al[j]).Name.ToString();
                    string tmpList = LamiSystem.StrListSysConName[i];
                    if (((Control)al[j]).Name.ToString() == LamiSystem.StrListSysConName[i])
                    {
                        if (al[j] is UltraCheckEditor)
                        {
                            LamiSystem.StrListSysConData.Add(((UltraCheckEditor)al[j]).Checked.ToString());
                        }
                        else
                        {
                            LamiSystem.StrListSysConData.Add(((Control)al[j]).Text);
                        }
                        break;
                    }
                }
            }
        }

        //찾고하 하는 콘트롤 콜렉션을 받아서 콘트롤을 배열에 저장한다.
        private void getControls(Control.ControlCollection Ocontrol, ref ArrayList Space)
        {
            for (int i = 0; i < Ocontrol.Count; i++)
            {
                Space.Add(Ocontrol[i]);
                if (Ocontrol[i].Controls.Count > 0)
                {
                    getControls(Ocontrol[i].Controls, ref Space);
                }
            }
        }


        //찾고자 하는 콘트롤 배열을 받아서 콘트롤을 배열에 저장한다.
        private void getControls(Control[] Ocontrol, ref ArrayList Space)
        {
            for (int i = 0; i < Ocontrol.Length; i++)
            {
                Space.Add(Ocontrol[i]);
                if (Ocontrol[i].Controls.Count > 0)
                {
                    getControls(Ocontrol[i].Controls, ref Space);
                }
            }
        }


        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void System_Config_Register_To_Lists()
        {
            System_Config_Register_To_List_Title();
            for (int i = 0; i < LamiSystem.StrListSysConTitle.Count; i++)
            {
                string strRegisterNameNData = System_Config_Register_To_List_Data(LamiSystem.RegPathSysCon,
                    LamiSystem.StrListSysConTitle[i]);
                System_Config_Register_To_List_NameNData(strRegisterNameNData);
            }
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void System_Config_Register_To_List_Title()
        {
            Register_To_StringList(LamiSystem.RegPathSysCon, ref LamiSystem.StrListSysConTitle);
        }


        //레지스터에 저장되어져 있는 네임과 데이터를 읽어와서 네임, 데이터 리스트배열에 저장한다.
        private void System_Config_Register_To_List_NameNData(string readData)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strTitleName = null;
            intStartIndex = 0;
            intEndIndex = readData.IndexOf("\t", intStartIndex);
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrListSysConName.Add(strTitleName);

            intStartIndex = intEndIndex + 1;
            intEndIndex = readData.Length;
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrListSysConData.Add(strTitleName);
        }

        //리스트 배열의 값을 파일에 저장한다.
        private void System_Config_Save_To_File()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\WriteLines2.txt");
            file.Flush();
            for (int i = 0; i < LamiSystem.StrListSysConTitle.Count; i++)
            {
                file.WriteLine(LamiSystem.StrListSysConTitle[i] + "\t" + LamiSystem.StrListSysConName[i] + "\t" +
                               LamiSystem.StrListSysConData[i]);
            }
            file.Close();
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void System_Config_Lists_To_Register(string strNodePath, List<string> regTitle, List<string> regControl, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                //string temptitle = regTitle[i];
                //string tempControl = regControl[i];
                //string tempData = regData[i];
                this.SetReg(strNodePath, regTitle[i], regControl[i] + "\t" + regData[i]);
            }
        }

        //레지스트리의 데이터 값을 읽와서 리스트 배열에 저장한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름을 가지는 문자열 배열
        //3번 파람: 레지스트리에서 읽은 값을 저장할 문자열 배열
        public void System_Config_Register_To_List_Data(string strNodePath, List<string> regTitle, List<string> regData)
        {
            regData.Clear();

            for (int i = 0; i < regTitle.Count; i++)
            {
                regData.Add(this.GetReg(strNodePath, regTitle[i]));
            }
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string System_Config_Register_To_List_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }


        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void System_Config_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.SystemConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    System_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }


        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void System_Config_StringParsing(string strLine)
        {
            string readTitle = null, readData = null;
            int indexNo = strLine.IndexOf("\t");
            readTitle = strLine.Substring(0, indexNo);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            SetReg(LamiSystem.RegPathSysCon, readTitle, readData);
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void System_Config_ListData_To_Register()
        {
            System_Config_Lists_To_Register(LamiSystem.RegPathSysCon, LamiSystem.StrListSysConTitle, LamiSystem.StrListSysConName,LamiSystem.StrListSysConData);
        }

        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool System_Config_Register_Empty_Check()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathSysCon, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }

        }

        /// strListConfigSystemData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void System_Config_ListData_To_Viewer()
        {
            for (int i = 0; i < LamiSystem.StrListSysConName.Count; i++)
            {
                ArrayList al = new ArrayList();
                getControls(this.Controls.Find(LamiSystem.StrListSysConName[i], true), ref al);

                if (al[0] is UltraCheckEditor)
                {
                    if (LamiSystem.StrListSysConData[i] == "True")
                        ((UltraCheckEditor) al[0]).Checked = true;
                    else
                        ((UltraCheckEditor) al[0]).Checked = false;
                }
                else if (al[0] is UltraComboEditor)
                {
                    if (LamiSystem.StrListSysConData[i] == "ON")
                        ((UltraComboEditor) al[0]).SelectedIndex = 0;
                    else
                        ((UltraComboEditor) al[0]).SelectedIndex = 1;
                }
                else if (al[0] is UltraTextEditor)
                {
                    string dataTemp = LamiSystem.StrListSysConData[i];
                    ((UltraTextEditor)al[0]).Text = LamiSystem.StrListSysConData[i];
                }
            }
            MainForm_ProgracessBar_Display_01("System Config Loading !", 80);
        }


        #endregion

        #endregion
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// 비전부 설정 탭을 운용하고 제어함.//////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region 비전부 설정 탭

        /// 시스템 조정 탭의 메버스.

        #region 비전부 설정 탭 : 멤버스

        #endregion

        /// 시스템 조정 탭의 프로퍼티.

        #region 비전부 설정 탭 : 프로퍼티

        #endregion

        /// 시스템 조정 탭의 생성자, 초기화.

        #region 비전부 설정 탭 : 생성자, 초기화

        /// 콘피그 저장 레지스트리가 비어 있는지 확인해서 비어 있다면
        /// 텍스트파일에서 디폴트 설정값을 읽어와서 레지스트리에 저장한다.
        private void VisionLami_Config_Initionalize()
        {
            MainForm_ProgracessBar_Display_01("Model Vision Data Loading !", 10);

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            VisionLami_Config_ListData_To_Viewer();

            MainForm_ProgracessBar_Display_01("Model Vision Data Loading !", 30);

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            VisionLami_Config_ListData_To_UperGrid();

            
            MainForm_ProgracessBar_Display_01("Model Vision Data Loading !", 50);
            Thread.Sleep(1000);
            VisionLami_Config_ListData_To_DownGrid();

            MainForm_ProgracessBar_Display_01("Model Vision Data Loading !", 70);
            //바이셀 옵셋 그리드를 크기를 조정한다.
            Vision_Uper_Grid_Resize();

            MainForm_ProgracessBar_Display_01("Model Vision Data Loading !", 90);
            Vision_Down_Grid_Resize();
        }

        /*
        /// 콘피그 저장 레지스트리가 비어 있는지 확인해서 비어 있다면
        /// 텍스트파일에서 디폴트 설정값을 읽어와서 레지스트리에 저장한다.
        private void VisionBiCell_Config_Initionalize()
        {
            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            VisionBiCell_Config_ListData_To_Viewer();

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            VisionBiCell_Config_ListData_To_Viewer_Grid();

            //바이셀 옵셋 그리드를 크기를 조정한다.
            VisionBiCell_BiCell_Grid_Resize();
        }
        */

        #endregion

        // 시스템 조정 탭의 이벤트 핸들러.//////////////////////////////////////////////////
        #region 비전부 설정 탭 : 이벤트 핸들러

        /// <summary>
        /// 시스템 조정 탭의 이미지 읽기 버튼 클릭 이벤트 핸들러
        /// </summary>
        private void Calibration_uBtn_ImageRead_Click(object sender, EventArgs e)
        {
            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            pFileDlg.InitialDirectory = Environment.CurrentDirectory+"\\DefaultImage";
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strFullPathFile = pFileDlg.FileName;
                strLoad_Image_Name = strFullPathFile;

                Vision_Config_IplBox1.ImageIpl = IplImage.FromFile(strFullPathFile);
                switch (this.ultraTabControl1.ActiveTab.Key)
                {
                    case "RecipeUper":
                        Vision_Config_IplBox1.ImageIpl = IplImage.FromFile(strFullPathFile);
                        break;
                    case "RecipeDown":
                        Vision_Config_IplBox2.ImageIpl = IplImage.FromFile(strFullPathFile);
                        break;
                }
            }
        }

        private void Vision_uBtn_ParamApply_Click(object sender, EventArgs e)
        {
            //Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
            //DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("VisionMessage01Caption"),rm.GetString("VisionMessage01Header"), rm.GetString("VisionMessage01Content"),MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (dlgResult == DialogResult.Cancel) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            //레시피 설정값이 정상적인지를 확인하는 함수를 호출한다.
            if (uDs_Vision_Applying_Check() == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            //20150305 WKB 209
            VisionDownStatus_Enable();

            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            VisionLami_Config_Viewer_To_List_Data();
            VisionLami_Config_UperGrid_To_List();
            VisionLami_Config_DownGrid_To_List();

            ubtnToolbarSave.PerformClick();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public bool uDs_Vision_Applying_Check()
        {
            for (int i = 0; i < uDS_Offset_Uper.Rows.Count; i++)
            {
                string Grid_Name = uDS_Offset_Uper.Rows[i].GetCellValue(1).ToString();
                if (Grid_Name == "")
                {
                    uMessageBox.MessageBox_Show("비전부 설정", "상부 비전 설정", "설정한 검사 항목 중에서 " + (i + 1).ToString() + " 번째 검사 항목의 이름을 입력하세요.<br/><br/>상부 비전부 항목을 다시 설정 하십시요!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //20150304 WKB 209
                    VisionEditStatus_Desiable(0); 
                    this.ultraTabControl1.SelectedTab = this.ultraTabControl1.Tabs[0];
                    return false;
                }
            }

            for (int i = 0; i < uDS_Offset_Down.Rows.Count; i++)
            {
                string Grid_Name = uDS_Offset_Down.Rows[i].GetCellValue(1).ToString();

                if (Grid_Name == "")
                {
                    uMessageBox.MessageBox_Show("비전부 설정", "하부 비전 설정", "설정한 검사 항목 중에서 " + (i + 1).ToString() + " 번째 검사 항목의 이름을 입력하세요.<br/><br/>하부 비전부 항목을 다시 설정 하십시요!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //20150304 WKB 209
                    VisionEditStatus_Desiable(1);
                    this.ultraTabControl1.SelectedTab = this.ultraTabControl1.Tabs[1];
                    return false;
                }
            }

            return true;
        }

        private void Vision_uGrd_Offset_MouseDown(object sender, MouseEventArgs e)
        {
//             UltraGridRow row;
//             UIElement element;
//             element = Vision_uGrd_Uper.DisplayLayout.UIElement.ElementFromPoint(e.Location);
//             row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
//             if (row != null && row.IsDataRow)
//             {
//                 _iVisionBiCellGridRowNo = row.Index;
//                 Vision_uGrd_Uper.Rows[_iVisionBiCellGridRowNo].Appearance.BackColor = Color.Silver;
//             }
        }

        private void Vision_uGrd_Offset_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {

        }



        private void Vision_uBtn_02_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    bool CheckUsedUper = Vision_Uper_Recipe_Used_Check();
                    if (CheckUsedUper == true)
                    {
                        Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                        messageBox.MessageBox_Show("비전부 설정", "삭제 오류 !", "선택 한 항목은 현재 레시피에서 사용하고 있어 삭제할 수 없습니다. <br/><br/>" +
                                                                        "레시피에서 먼저 해당 항목을 삭제하십시요!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show("현재 레시피에서 사용하고 있어 삭제할 수 없습니다..\r\n레시피에서 먼저 해당 항목을 삭제하십시요!");
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                    Vision_Uper_DataSource_Delete();
                    Vision_Uper_Grid_Resize();

                    //20150304 WKB 209
                    VisionEditStatus_Desiable(0);

                    break;
                case "RecipeDown":
                    bool CheckUsedDown = Vision_Down_Recipe_Used_Check();
                    if (CheckUsedDown == true)
                    {
                        Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
                        messageBox.MessageBox_Show("비전부 설정", "삭제 오류 !", "선택 한 항목은 현재 레시피에서 사용하고 있어 삭제할 수 없습니다. <br/><br/>" +
                                                                        "레시피에서 먼저 해당 항목을 삭제하십시요!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show("현재 레시피에서 사용하고 있어 삭제할 수 없습니다..\r\n레시피에서 먼저 해당 항목을 삭제하십시요!");
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                    Vision_Down_DataSource_Delete();
                    Vision_Down_Grid_Resize();

                    //20150304 WKB 209
                    VisionEditStatus_Desiable(1);

                    break;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private bool Vision_Uper_Recipe_Used_Check()
        {
            if (_iVisionUperGridRowNo < 0) return false;
            string selectItem = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[1].Value.ToString();
            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                string RecipeItem = uGrd_Recipe_UperData.Rows[i].Cells[0].Value.ToString();
                if(selectItem == RecipeItem) 
                    return true;
            }
            return false;
        }

        private bool Vision_Down_Recipe_Used_Check()
        {
            if (_iVisionDownGridRowNo < 0) return false;
            string selectItem = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[1].Value.ToString();
            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                string RecipeItem = uGrd_Recipe_DownData.Rows[i].Cells[0].Value.ToString();
                if (selectItem == RecipeItem)
                    return true;
            }
            return false;
        }
        //편집에서 이동 버튼이 하부와 상부의 항목을 편집하는데 사용되므로
        //현재 탭 콘트롤에서 선택된 탭의 키를 가지고 활성화 도었는 탭을
        //판별하여 이에 해당하는 작업을 진행한다.
        private void Vision_uBtn_BiCell03_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    if (_iVisionUperGridRowNo < 0) break;
                    if (_iVisionUperGridRowNo == 0) break;
                    
                //20150304 WKB 209
                    VisionEditStatus_Desiable(0);

                    Vision_Uper_DataSource_Move(_iVisionUperGridRowNo - 1);
                    _iVisionUperGridRowNo = _iVisionUperGridRowNo - 1;
                    Vision_Uper_Grid_Resize();
                    break;
                case "RecipeDown":
                    if (_iVisionDownGridRowNo < 0) break;
                    if (_iVisionDownGridRowNo == 0) break;

                    //20150304 WKB 209
                    VisionEditStatus_Desiable(1);

                    Vision_Down_DataSource_Move(_iVisionDownGridRowNo - 1);
                    _iVisionDownGridRowNo = _iVisionDownGridRowNo - 1;
                    Vision_Down_Grid_Resize();
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void Vision_uBtn_BiCell04_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    if (_iVisionUperGridRowNo < 0) break;
                    if (_iVisionUperGridRowNo == Vision_uGrd_Uper.Rows.Count - 1) break;
                    Vision_Uper_DataSource_Move(_iVisionUperGridRowNo + 1);
                    _iVisionUperGridRowNo = _iVisionUperGridRowNo + 1;
                    Vision_Uper_Grid_Resize();
                    break;
                case "RecipeDown":
                    if (_iVisionDownGridRowNo < 0) break;
                    if (_iVisionDownGridRowNo == Vision_uGrd_Down.Rows.Count - 1) break;
                    Vision_Down_DataSource_Move(_iVisionDownGridRowNo + 1);
                    _iVisionDownGridRowNo = _iVisionDownGridRowNo + 1;
                    Vision_Down_Grid_Resize();
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void Vision_uBtn_BiCell01_Click(object sender, EventArgs e)
        {
            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    break;

                case "RecipeDown":
                    break;
            }
        }

        public void Vision_Uper_Grid_Resize()
        {
            if (Vision_uGrd_Uper.Rows.Count > 22)
            {
                if (Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[1].Width == 58) return;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[0].Width = 31;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[1].Width = 58;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[2].Width = 59;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[3].Width = 58;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[4].Width = 58;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[5].Width = 53;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[6].Width = 53;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[7].Width = 73;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[8].Width = 31;
            }
            else
            {
                if (Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[1].Width == 61) return;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[0].Width = 31;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[1].Width = 61;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[2].Width = 61;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[3].Width = 60;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[4].Width = 60;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[5].Width = 55;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[6].Width = 55;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[7].Width = 78;
                Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns[8].Width = 31;
                if (Vision_uGrd_Uper.Rows.Count > 0)
                    Vision_uGrd_Uper.ActiveRow = Vision_uGrd_Uper.Rows[0];
            }
        }

        public void Vision_Down_Grid_Resize()
        {
            if (Vision_uGrd_Down.Rows.Count > 22)
            {
                if (Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[1].Width == 58) return;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[0].Width = 31;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[1].Width = 58;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[2].Width = 59;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[3].Width = 58;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[4].Width = 58;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[5].Width = 53;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[6].Width = 53;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[7].Width = 75;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[8].Width = 31;
            }
            else
            {
                if (Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[1].Width == 61) return;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[0].Width = 31;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[1].Width = 61;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[2].Width = 61;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[3].Width = 60;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[4].Width = 60;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[5].Width = 55;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[6].Width = 55;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[7].Width = 80;
                Vision_uGrd_Down.DisplayLayout.Bands[0].Columns[8].Width = 31;
                if (Vision_uGrd_Down.Rows.Count > 0) 
                    Vision_uGrd_Down.ActiveRow = Vision_uGrd_Down.Rows[0];
            }
        }
        /*
        public void VisionBiCell_BiCell_Grid_Resize()
        {
            if (VisionBiCell_uGrd_Offset.Rows.Count > 23)
            {
                VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns[0].Width = 43;
                VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns[1].Width = 160;
                VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns[2].Width = 198;
            }
            else
            {
                VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns[0].Width = 43;
                VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns[1].Width = 160;
                VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns[2].Width = 216;
                VisionBiCell_uGrd_Offset.ActiveRow = VisionBiCell_uGrd_Offset.Rows[0];
            }
        }
        */
        private int _iVisionUperGridRowNo = -1;
        public void Vision_Uper_DataSource_Insert()
        {
            UltraGridRow row = this.Vision_uGrd_Uper.DisplayLayout.Bands[0].AddNew();
            if (Vision_uGrd_Uper.Rows.Count < 2)
            {
                row.Cells[0].Value = "01";

                //20150305 WKB 209
                row.Cells[1].Value = "";
                
                //20150305 WKB 208
                //row.Cells[1].Value = "E";
                row.Cells[2].Value = "2.00";
                row.Cells[3].Value = "3.00";
                row.Cells[4].Value = "1.00";
                row.Cells[5].Value = "100.00";
                row.Cells[6].Value = "1.0000";
                row.Cells[7].Value = "110.00";
                row.Cells[8].Value = "True";
            }
            else
            {
                int iRowNO = 0;
                bool ParseResult = int.TryParse(Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[0].Value.ToString(), out iRowNO);
                if (ParseResult == true) row.Cells[0].Value = (iRowNO + 1).ToString("00");

                //그리드에서 추가할 Row를 선택하지 않았을 때 _iVisionUperGridRowNo의 값이 -1이 된다.
                //이때에는 마지막 Row의 데이터를 복사하도록 한다.
                if (_iVisionUperGridRowNo == -1) _iVisionUperGridRowNo = Vision_uGrd_Uper.Rows.Count - 2;

                //20150305 WKB 209
                row.Cells[1].Value = "";

                //20150305 WKB 208
                //row.Cells[1].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[1].Value + "'";

                row.Cells[2].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[2].Value;
                row.Cells[3].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[3].Value;
                row.Cells[4].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[4].Value;
                row.Cells[5].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[5].Value;
                row.Cells[6].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[6].Value;
                row.Cells[7].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[7].Value;
                row.Cells[8].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Cells[8].Value;
            }
           
                Vision_uGrd_Uper.Rows.Move(row, Vision_uGrd_Uper.Rows.Count - 1);
        }

        /*
        private int _iVisionUperGridRowNo = -1;
        public void Vision_Uper_DataSource_Insert()
        {
            UltraGridRow row = this.Vision_uGrd_Uper.DisplayLayout.Bands[0].AddNew();
            if (Vision_uGrd_Uper.Rows.Count < 2)
            {
                row.Cells[0].Value = "01";
                row.Cells[1].Value = "E";
                row.Cells[2].Value = "2.00";
                row.Cells[3].Value = "3.00";
                row.Cells[4].Value = "1.00";
                row.Cells[5].Value = "100.00";
                row.Cells[6].Value = "1.0000";
                row.Cells[7].Value = "110.00";
                row.Cells[8].Value = "True";
            }
            else
            {

                if (_iVisionUperGridRowNo < 0)
                {
                    int iRowNO = 0;
                    bool ParseResult = int.TryParse(Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[0].Value.ToString(), out iRowNO);
                    if (ParseResult == true) row.Cells[0].Value = (iRowNO + 1).ToString("00");

                    row.Cells[1].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[1].Value;
                    row.Cells[2].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[2].Value;
                    row.Cells[3].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[3].Value;
                    row.Cells[4].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[4].Value;
                    row.Cells[5].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[5].Value;
                    row.Cells[6].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[6].Value;
                    row.Cells[7].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[7].Value;
                    row.Cells[8].Value = Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[8].Value;
                }
                else
                {
                    int iRowNO = 0;
                    bool ParseResult = int.TryParse(Vision_uGrd_Uper.Rows[Vision_uGrd_Uper.Rows.Count - 2].Cells[0].Value.ToString(),out iRowNO);
                    if (ParseResult == true) row.Cells[0].Value = (iRowNO + 1).ToString("00");

                    row.Cells[1].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[1].Value;
                    row.Cells[2].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[2].Value;
                    row.Cells[3].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[3].Value;
                    row.Cells[4].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[4].Value;
                    row.Cells[5].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[5].Value;
                    row.Cells[6].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[6].Value;
                    row.Cells[7].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[7].Value;
                    row.Cells[8].Value = Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo - 1].Cells[8].Value;
                }
            }

            //Vision_Uper_Inspect_Insert();

            if (_iVisionUperGridRowNo < 0)
                Vision_uGrd_Uper.Rows.Move(row, Vision_uGrd_Uper.Rows.Count - 1);
            else
                Vision_uGrd_Uper.Rows.Move(row, _iVisionUperGridRowNo);
        } 
        */
       

        private int _iVisionDownGridRowNo = -1;

        public void Vision_Down_DataSource_Insert()
        {
            UltraGridRow row = this.Vision_uGrd_Down.DisplayLayout.Bands[0].AddNew();
            if (Vision_uGrd_Down.Rows.Count < 2)
            {
                
                row.Cells[0].Value = "01";

                //20150305 WKB 209
                row.Cells[1].Value = "";

                //20150305 WKB 208
                //row.Cells[1].Value = "E";

                row.Cells[2].Value = "2.00";
                row.Cells[3].Value = "3.00";
                row.Cells[4].Value = "1.00";
                row.Cells[5].Value = "100.00";
                row.Cells[6].Value = "1.0000";
                row.Cells[7].Value = "110.00";
                row.Cells[8].Value = "True";
            }
            else
            {
                int iRowNO = 0;
                bool ParseResult = int.TryParse(Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[0].Value.ToString(), out iRowNO);
                if (ParseResult == true) row.Cells[0].Value = (iRowNO + 1).ToString("00");

                //그리드에서 추가할 Row를 선택하지 않았을 때 _iVisionUperGridRowNo의 값이 -1이 된다.
                //이때에는 마지막 Row의 데이터를 복사하도록 한다.
                if (_iVisionDownGridRowNo == -1) _iVisionDownGridRowNo = Vision_uGrd_Down.Rows.Count - 2;

                //20150305 WKB 209
                row.Cells[1].Value = "";

                //20150305 WKB 208
                //row.Cells[1].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[1].Value + "'";
                row.Cells[2].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[2].Value;
                row.Cells[3].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[3].Value;
                row.Cells[4].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[4].Value;
                row.Cells[5].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[5].Value;
                row.Cells[6].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[6].Value;
                row.Cells[7].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[7].Value;
                row.Cells[8].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Cells[8].Value;
            }

                Vision_uGrd_Down.Rows.Move(row, Vision_uGrd_Down.Rows.Count - 1);
        }

        /*
        public void Vision_Down_DataSource_Insert()
        {
            UltraGridRow row = this.Vision_uGrd_Down.DisplayLayout.Bands[0].AddNew();
            if (Vision_uGrd_Down.Rows.Count < 2)
            {
                row.Cells[0].Value = "01";
                row.Cells[1].Value = "E";
                row.Cells[2].Value = "2.00";
                row.Cells[3].Value = "3.00";
                row.Cells[4].Value = "1.00";
                row.Cells[5].Value = "100.00";
                row.Cells[6].Value = "1.0000";
                row.Cells[7].Value = "110.00";
                row.Cells[8].Value = "True";
            }
            else
            {
                if (_iVisionDownGridRowNo < 0)
                {
                    int iRowNO = 0;
                    bool ParseResult = int.TryParse(Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[0].Value.ToString(), out iRowNO);
                    if (ParseResult == true) row.Cells[0].Value = (iRowNO + 1).ToString("00");

                    row.Cells[1].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[1].Value;
                    row.Cells[2].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[2].Value;
                    row.Cells[3].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[3].Value;
                    row.Cells[4].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[4].Value;
                    row.Cells[5].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[5].Value;
                    row.Cells[6].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[6].Value;
                    row.Cells[7].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[7].Value;
                    row.Cells[8].Value = Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[8].Value;
                }
                else
                {
                    int iRowNO = 0;
                    bool ParseResult = int.TryParse(Vision_uGrd_Down.Rows[Vision_uGrd_Down.Rows.Count - 2].Cells[0].Value.ToString(), out iRowNO);
                    if (ParseResult == true) row.Cells[0].Value = (iRowNO + 1).ToString("00");

                    row.Cells[1].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[1].Value;
                    row.Cells[2].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[2].Value;
                    row.Cells[3].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[3].Value;
                    row.Cells[4].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[4].Value;
                    row.Cells[5].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[5].Value;
                    row.Cells[6].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[6].Value;
                    row.Cells[7].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[7].Value;
                    row.Cells[8].Value = Vision_uGrd_Down.Rows[_iVisionDownGridRowNo - 1].Cells[8].Value;
                }
            }


            if (_iVisionDownGridRowNo < 0)
                Vision_uGrd_Down.Rows.Move(row, Vision_uGrd_Down.Rows.Count - 1);
            else
                Vision_uGrd_Down.Rows.Move(row, _iVisionDownGridRowNo);
        }
        */

        public void Vision_Uper_DataSource_Delete()
        {
            if (_iVisionUperGridRowNo < 0)
            {
                MessageBox.Show("삭제를 원하는 항목을 선택하여 주십시요!");
            }
            else
            {
                Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Delete(false);
                _iVisionUperGridRowNo = -1;
            }
        }

        public void Vision_Down_DataSource_Delete()
        {
            if (_iVisionDownGridRowNo < 0)
            {
                MessageBox.Show("삭제를 원하는 항목을 선택하여 주십시요!");
            }
            else
            {
                Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Delete(false);
                _iVisionDownGridRowNo = -1;
            }
        }


        public void Vision_Uper_DataSource_Move(int rowNo)
        {
            UltraGridRow row = this.Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo];
            Vision_uGrd_Uper.Rows.Move(row, rowNo);
        }

        public void Vision_Down_DataSource_Move(int rowNo)
        {
            UltraGridRow row = this.Vision_uGrd_Down.Rows[_iVisionDownGridRowNo];
            Vision_uGrd_Down.Rows.Move(row, rowNo);
        }


        #endregion
        
        /// 시스템 조정 탭의 기타 함수.

        #region 비전부 설정 탭 : 기타 함수

        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool VisionLami_Config_Register_Empty_Check()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathVisCon_Lami, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }
        

        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void VisionLami_Config_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.VisionLamiConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    VisionLami_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }

       
        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void VisionLami_Config_StringParsing(string strLine)
        {
            string readTitle = null, readData = null;
            int indexNo = strLine.IndexOf("\t");
            readTitle = strLine.Substring(0, indexNo);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            SetReg(LamiSystem.RegPathVisCon_Lami, readTitle, readData);
        }

        /*
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void VisionBiCell_Config_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.VisionBiCellConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    VisionBiCell_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }
        */

        /*
        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void VisionBiCell_Config_StringParsing(string strLine)
        {
            string readTitle = null, readData = null;
            int indexNo = strLine.IndexOf("\t");
            readTitle = strLine.Substring(0, indexNo);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            SetReg(LamiSystem.RegPathVisCon_BiCell, readTitle, readData);
        }
        */

        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool VisionLami_Config_Register_Empty_Check(string checkingRegNodePath)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(checkingRegNodePath, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }

        /*
        public bool VisionBiCell_Config_Register_Empty_Check(string checkingRegNodePath)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(checkingRegNodePath, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }
        */

        /*
        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intVisionBiCellGridCellCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void VisionBiCell_Config_File_To_Register_Grid()
        {
            _intVisionBiCellGridCellCount = 0;
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.VisionBiCellGridDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    VisionBiCell_Config_StringParsing_Grid(strLine);
                }
            }
            ioStream.Close();
        }
        */

        /*
        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void VisionBiCell_Config_StringParsing_Grid(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathVisConGrid_BiCell, _intVisionBiCellGridCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intVisionBiCellGridCellCount++;
            }
        }
        */

        //_intVisionGridUperCount
        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intVisionGridUperCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void VisionLami_Config_File_To_Register_UperGrid()
        {
            _intVisionGridUperCount = 0;
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.VisionGridUperDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    VisionLami_Config_StringParsing_UperGrid(strLine);
                }
            }
            ioStream.Close();
        }

        
        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void VisionLami_Config_StringParsing_UperGrid(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathVisConGrid_Uper, _intVisionGridUperCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intVisionGridUperCount++;
            }
        }

        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intVisionGridDownCount = 0;
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void VisionLami_Config_File_To_Register_DownGrid()
        {
            _intVisionGridDownCount = 0;
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.VisionGridDownDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    VisionLami_Config_StringParsing_DownGrid(strLine);
                }
            }
            ioStream.Close();
        }

        private void VisionLami_Config_StringParsing_DownGrid(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathVisConGrid_Down, _intVisionGridDownCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intVisionGridDownCount++;
            }
        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void VisionLami_Config_Register_To_Lists()
        {
            VisionLami_Config_Register_To_List_Title();
            for (int i = 0; i < LamiSystem.StrListVisConTitle.Count; i++)
            {
                string strRegisterNameNData = VisionLami_Config_Register_To_List_Data(LamiSystem.RegPathVisCon_Lami,
                    LamiSystem.StrListVisConTitle[i]);
                VisionLami_Config_Register_To_List_NameNData(strRegisterNameNData);
            }
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void VisionLami_Config_Register_To_List_Title()
        {
            Register_To_StringList(LamiSystem.RegPathVisCon_Lami, ref LamiSystem.StrListVisConTitle);
        }


        //레지스터에 저장되어져 있는 네임과 데이터를 읽어와서 네임, 데이터 리스트배열에 저장한다.
        private void VisionLami_Config_Register_To_List_NameNData(string readData)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strTitleName = null;
            intStartIndex = 0;
            intEndIndex = readData.IndexOf("\t", intStartIndex);
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrListVisConName.Add(strTitleName);

            intStartIndex = intEndIndex + 1;
            intEndIndex = readData.Length;
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrListVisConData.Add(strTitleName);
        }




        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void VisionLami_Config_Register_To_Lists_UperGrid()
        {
            LamiSystem.StrListVisConGridTitle_Uper.Clear();
            VisionLami_Config_Register_To_List_UperGrid();
            LamiSystem.StrListVisConGridData_Uper.Clear();

            for (int i = 0; i < LamiSystem.StrListVisConGridTitle_Uper.Count; i++)
            {
                string strRegisterGridData = VisionLami_Config_Register_To_List_Data(LamiSystem.RegPathVisConGrid_Uper, LamiSystem.StrListVisConGridTitle_Uper[i]);
                LamiSystem.StrListVisConGridData_Uper.Add(strRegisterGridData);
            }
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void VisionLami_Config_Register_To_List_UperGrid()
        {
            Register_To_StringList(LamiSystem.RegPathVisConGrid_Uper, ref LamiSystem.StrListVisConGridTitle_Uper);
        }


        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void VisionLami_Config_Register_To_Lists_DownGrid()
        {
            LamiSystem.StrListVisConGridTitle_Down.Clear();
            VisionLami_Config_Register_To_List_DownGrid();
            LamiSystem.StrListVisConGridData_Down.Clear();

            for (int i = 0; i < LamiSystem.StrListVisConGridTitle_Down.Count; i++)
            {
                string strRegisterGridData = VisionLami_Config_Register_To_List_Data(LamiSystem.RegPathVisConGrid_Down, LamiSystem.StrListVisConGridTitle_Down[i]);
                LamiSystem.StrListVisConGridData_Down.Add(strRegisterGridData);
            }
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void VisionLami_Config_Register_To_List_DownGrid()
        {
            Register_To_StringList(LamiSystem.RegPathVisConGrid_Down, ref LamiSystem.StrListVisConGridTitle_Down);
        }


        /// strListConfigVisionData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void VisionLami_Config_ListData_To_Viewer()
        {
            for (int i = 0; i < LamiSystem.StrListVisConName.Count; i++)
            {
                ArrayList al = new ArrayList();

                getControls(this.Controls.Find(LamiSystem.StrListVisConName[i], true), ref al);

                if (al[0] is UltraCheckEditor)
                {
                    if (LamiSystem.StrListVisConData[i] == "True")
                        ((UltraCheckEditor) al[0]).Checked = true;
                    else
                        ((UltraCheckEditor) al[0]).Checked = false;
                }
                else if (al[0] is UltraComboEditor)
                {
                    if (LamiSystem.StrListVisConData[i] == "ON")
                        ((UltraComboEditor) al[0]).SelectedIndex = 0;
                    else
                        ((UltraComboEditor) al[0]).SelectedIndex = 1;
                }
                else if (al[0] is UltraTextEditor)
                {
                    string dataTemp = LamiSystem.StrListVisConData[i];
                    ((UltraTextEditor)al[0]).Text = LamiSystem.StrListVisConData[i];
                }
            }
        }

        /*
        /// strListConfigVisionData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void VisionBiCell_Config_ListData_To_Viewer()
        {
            for (int i = 0; i < LamiSystem.StrListVisConName_BiCell.Count; i++)
            {
                ArrayList al = new ArrayList();

                getControls(this.Controls.Find(LamiSystem.StrListVisConName_BiCell[i], true), ref al);

                if (al[0] is UltraCheckEditor)
                {
                    if (LamiSystem.StrListVisConData_BiCell[i] == "True")
                        ((UltraCheckEditor)al[0]).Checked = true;
                    else
                        ((UltraCheckEditor)al[0]).Checked = false;
                }
                else if (al[0] is UltraComboEditor)
                {
                    if (LamiSystem.StrListVisConData_BiCell[i] == "ON")
                        ((UltraComboEditor)al[0]).SelectedIndex = 0;
                    else
                        ((UltraComboEditor)al[0]).SelectedIndex = 1;
                }
                else if (al[0] is UltraTextEditor)
                {
                    string dataTemp = LamiSystem.StrListVisConData_BiCell[i];
                    ((UltraTextEditor)al[0]).Text = LamiSystem.StrListVisConData_BiCell[i];
                }
            }
        }
        */

        /// _alignSystem._strListVisionConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void VisionLami_Config_ListData_To_UperGrid()
        {
            uDS_Offset_Uper.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrListVisConGridData_Uper.Count; i++)
            {
                int cellCount = i%(Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns.Count - 1);
                if (cellCount == 0)
                {
                    uDS_Offset_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "" });
                    Vision_uGrd_Uper.DisplayLayout.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString("00");
                }

                string TempXY = rowCount.ToString("00") + "  " + (cellCount + 1).ToString("00");
                Vision_uGrd_Uper.DisplayLayout.Rows[rowCount].Cells[cellCount + 1].Value = LamiSystem.StrListVisConGridData_Uper[i];
                //if (cellCount == 3) rowCount++;
                if (cellCount == Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns.Count - 2) rowCount++;
            }
        }

        /// _alignSystem._strListVisionConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void VisionLami_Config_ListData_To_DownGrid()
        {
            uDS_Offset_Down.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrListVisConGridData_Down.Count; i++)
            {
                int cellCount = i % (Vision_uGrd_Down.DisplayLayout.Bands[0].Columns.Count - 1);
                if (cellCount == 0)
                {
                    uDS_Offset_Down.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "" });
                    //Vision_uGrd_Down.DisplayLayout.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString("00");
                    uDS_Offset_Down.Rows[rowCount].SetCellValue(0, (rowCount + 1).ToString("00"));
                }

                string TempXY = rowCount.ToString("00") + "  " + (cellCount + 1).ToString("00");
                //Vision_uGrd_Down.DisplayLayout.Rows[rowCount].Cells[cellCount + 1].Value = LamiSystem.StrListVisConGridData_Down[i];
                uDS_Offset_Down.Rows[rowCount].SetCellValue(cellCount + 1, LamiSystem.StrListVisConGridData_Down[i]);
                if (cellCount == Vision_uGrd_Down.DisplayLayout.Bands[0].Columns.Count - 2) rowCount++;
            }
        }

        // uDS_Inspect_Measure_Uper.Rows[i].SetCellValue(j, Data_Struct.regData);

        /*
        private void VisionLami_Config_ListData_To_UperGrid()
        {
            uDS_Offset_Uper.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrListVisConGridData_Uper.Count; i++)
            {
                int cellCount = i%(Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns.Count - 1);
                if (cellCount == 0)
                {
                    uDS_Offset_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "" });
                    Vision_uGrd_Uper.DisplayLayout.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString("00");
                }

                string TempXY = rowCount.ToString("00") + "  " + (cellCount + 1).ToString("00");
                Vision_uGrd_Uper.DisplayLayout.Rows[rowCount].Cells[cellCount + 1].Value = LamiSystem.StrListVisConGridData_Uper[i];
                //if (cellCount == 3) rowCount++;
                if (cellCount == Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns.Count - 2) rowCount++;
            }
        }
         * 
         private void VisionLami_Config_ListData_To_DownGrid()
        {
            uDS_Offset_Down.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrListVisConGridData_Down.Count; i++)
            {
                int cellCount = i % (Vision_uGrd_Down.DisplayLayout.Bands[0].Columns.Count - 1);
                if (cellCount == 0)
                {
                    uDS_Offset_Down.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "" });
                    Vision_uGrd_Down.DisplayLayout.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString("00");
                }

                string TempXY = rowCount.ToString("00") + "  " + (cellCount + 1).ToString("00");
                Vision_uGrd_Down.DisplayLayout.Rows[rowCount].Cells[cellCount + 1].Value = LamiSystem.StrListVisConGridData_Down[i];
                if (cellCount == Vision_uGrd_Down.DisplayLayout.Bands[0].Columns.Count - 2) rowCount++;
            }
        }
         * 
        /// _alignSystem._strListVisionConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void VisionBiCell_Config_ListData_To_Viewer_Grid()
        {
            uDS_Offset_BiCell.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrListVisConGridData_Down.Count; i++)
            {
                //int cellCount = i%4;
                int cellCount = i % (VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns.Count - 1);
                if (cellCount == 0)
                {
                    uDS_Offset_BiCell.Rows.Add(true, new Object[] { "", "","" });
                    VisionBiCell_uGrd_Offset.DisplayLayout.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString("00");
                }

                string TempXY = rowCount.ToString("00") + "  " + (cellCount + 1).ToString("00");
                VisionBiCell_uGrd_Offset.DisplayLayout.Rows[rowCount].Cells[cellCount + 1].Value = LamiSystem.StrListVisConGridData_Down[i];
                //if (cellCount == 3) rowCount++;
                if (cellCount == VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns.Count - 2) rowCount++;
            }
        }
        */

        //레지스트리의 데이터 값을 읽와서 리스트 배열에 저장한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름을 가지는 문자열 배열
        //3번 파람: 레지스트리에서 읽은 값을 저장할 문자열 배열
        public void VisionLami_Config_Register_To_List_Data(string strNodePath, List<string> regTitle, List<string> regData)
        {
            regData.Clear();

            for (int i = 0; i < regTitle.Count; i++)
            {
                regData.Add(this.GetReg(strNodePath, regTitle[i]));
            }
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string VisionLami_Config_Register_To_List_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string VisionBiCell_Config_Register_To_List_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }


        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void VisionLami_Config_Viewer_To_List_Data()
        {
            LamiSystem.StrListVisConData.Clear();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("VisionGapConfig_Tab_BackPanel", true), ref al);

            for (int i = 0; i < LamiSystem.StrListVisConName.Count; i++)
            {
                for (int j = 0; j < al.Count; j++)
                {
                    if (((Control)al[j]).Name.ToString() == LamiSystem.StrListVisConName[i])
                    {
                        if (al[j] is UltraCheckEditor)
                        {
                            LamiSystem.StrListVisConData.Add(((UltraCheckEditor)al[j]).Checked.ToString());
                        }
                        else
                        {
                            LamiSystem.StrListVisConData.Add(((Control)al[j]).Text);
                        }
                        break;
                    }
                }
            }
        }

        /*
        private void VisionBiCell_Config_Viewer_To_List_Data()
        {
            LamiSystem.StrListVisConData_BiCell.Clear();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("VisionBiCellConfig_Tab_BackPanel", true), ref al);

            for (int i = 0; i < LamiSystem.StrListVisConName_BiCell.Count; i++)
            {
                for (int j = 0; j < al.Count; j++)
                {
                    if (((Control)al[j]).Name.ToString() == LamiSystem.StrListVisConName_BiCell[i])
                    {
                        if (al[j] is UltraCheckEditor)
                        {
                            LamiSystem.StrListVisConData_BiCell.Add(((UltraCheckEditor)al[j]).Checked.ToString());
                        }
                        else
                        {
                            LamiSystem.StrListVisConData_BiCell.Add(((Control)al[j]).Text);
                        }
                        break;
                    }
                }
            }
        }
        */


        //20150305 WKB 209

        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void VisionLami_Config_UperGrid_To_List()
        {
            LamiSystem.StrListVisConGridData_Uper.Clear();
            LamiSystem.StrListVisConGridTitle_Uper.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Offset_Uper.Rows.Count; i++)
            {
                for (int j = 1; j < uDS_Offset_Uper.Band.Columns.Count; j++)
                {
                    string strCellData1 = Vision_uGrd_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    string strCellData = uDS_Offset_Uper.Rows[i].GetCellValue(j).ToString();
                    LamiSystem.StrListVisConGridData_Uper.Add(strCellData);
                    LamiSystem.StrListVisConGridTitle_Uper.Add(regTitle++.ToString("000"));
                }
            }
        }

        private void VisionLami_Config_DownGrid_To_List()
        {
            LamiSystem.StrListVisConGridData_Down.Clear();
            LamiSystem.StrListVisConGridTitle_Down.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Offset_Down.Rows.Count; i++)
            {
                for (int j = 1; j < Vision_uGrd_Down.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = Vision_uGrd_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrListVisConGridData_Down.Add(strCellData);
                    LamiSystem.StrListVisConGridTitle_Down.Add(regTitle++.ToString("000"));
                }
            }
        }

        //20150305 WKB 208
        /*
        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void VisionLami_Config_UperGrid_To_List()
        {
            LamiSystem.StrListVisConGridData_Uper.Clear();
            LamiSystem.StrListVisConGridTitle_Uper.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Offset_Uper.Rows.Count; i++)
            {
                for (int j = 1; j < Vision_uGrd_Uper.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = Vision_uGrd_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrListVisConGridData_Uper.Add(strCellData);
                    LamiSystem.StrListVisConGridTitle_Uper.Add(regTitle++.ToString("000"));
                }
            }
        }

        private void VisionLami_Config_DownGrid_To_List()
        {
            LamiSystem.StrListVisConGridData_Down.Clear();
            LamiSystem.StrListVisConGridTitle_Down.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Offset_Down.Rows.Count; i++)
            {
                for (int j = 1; j < Vision_uGrd_Down.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = Vision_uGrd_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrListVisConGridData_Down.Add(strCellData);
                    LamiSystem.StrListVisConGridTitle_Down.Add(regTitle++.ToString("000"));
                }
            }
        }
        */

        /*
        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void VisionBiCell_Config_Viewer_To_List_Grid()
        {
            //int tempcount = _strListVisionConfigGridTitle.Count;
            LamiSystem.StrListVisConGridData_Down.Clear();
            LamiSystem.StrListVisConGridTitle_Down.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Offset_BiCell.Rows.Count; i++)
            {
                for (int j = 1; j < VisionBiCell_uGrd_Offset.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = VisionBiCell_uGrd_Offset.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrListVisConGridData_Down.Add(strCellData);
                    LamiSystem.StrListVisConGridTitle_Down.Add(regTitle++.ToString("000"));
                }
            }
        }
        */


        /*
        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void Vision_Config_Viewer_To_List_Grid()
        {
            //int tempcount = _strListVisionConfigGridTitle.Count;
            AlignSystem.StrListVisConGridData.Clear();

            for (int i = 0; i < uDS_Offset_Uper.Rows.Count; i++)
            {
                for (int j = 1; j < Vision_uGrd_Offset.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = Vision_uGrd_Offset.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    AlignSystem.StrListVisConGridData.Add(strCellData);
                }
            }
        }
        */

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void Vision_Config_ListData_To_Register()
        {
            VisionLami_Config_Lists_To_Register(LamiSystem.RegPathVisCon_Lami, LamiSystem.StrListVisConTitle, LamiSystem.StrListVisConName,
                LamiSystem.StrListVisConData);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void VisionLami_Config_Lists_To_Register(string strNodePath, List<string> regTitle, List<string> regControl,List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                //string temptitle = regTitle[i];
                //string tempControl = regControl[i];
                //string tempData = regData[i];
                this.SetReg(strNodePath, regTitle[i], regControl[i] + "\t" + regData[i]);
            }
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void Vision_Config_UperGrid_To_Register()
        {
            Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathVisConGrid_Uper, false);

            VisionLami_Config_Lists_To_Register_Grid(LamiSystem.RegPathVisConGrid_Uper, LamiSystem.StrListVisConGridTitle_Uper,
                LamiSystem.StrListVisConGridData_Uper);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void VisionLami_Config_Lists_To_Register_Grid(string strNodePath, List<string> regTitle, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                Trace.WriteLine("Grid Title : " + regTitle[i] + "    Grid Data :" + regData[i]);
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }

        private void Vision_Config_DownGrid_To_Register()
        {
            Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathVisConGrid_Down, false);

            VisionLami_Config_Lists_To_Register_Grid(LamiSystem.RegPathVisConGrid_Down, LamiSystem.StrListVisConGridTitle_Down,
                LamiSystem.StrListVisConGridData_Down);
        }


        /*
        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void VisionBiCell_Config_ListData_To_Register()
        {
            VisionBiCell_Config_Lists_To_Register(LamiSystem.RegPathVisCon_BiCell, LamiSystem.StrListVisConTitle_BiCell, LamiSystem.StrListVisConName_BiCell,
                LamiSystem.StrListVisConData_BiCell);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void VisionBiCell_Config_Lists_To_Register(string strNodePath, List<string> regTitle, List<string> regControl,
            List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                //string temptitle = regTitle[i];
                //string tempControl = regControl[i];
                //string tempData = regData[i];
                this.SetReg(strNodePath, regTitle[i], regControl[i] + "\t" + regData[i]);
            }
        }
       


        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void VisionBiCell_Config_ListData_To_Register_Grid()
        {
            VisionLami_Config_Lists_To_Register_Grid(LamiSystem.RegPathVisConGrid_BiCell, LamiSystem.StrListVisConGridTitle_Down,
                LamiSystem.StrListVisConGridData_Down);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void VisionBiCell_Config_Lists_To_Register_Grid(string strNodePath, List<string> regTitle, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                Trace.WriteLine("Grid Title : " + regTitle[i] + "    Grid Data :" + regData[i]);
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }
 */
        #endregion

        #endregion
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// 모델 설정부를 운용하고 제어함./////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region 모델 설정 진행

        private static string _strNow_Model_Name = string.Empty;////"P 1.7";//null;
        private static string _strNow_Model_Number = string.Empty;//"2";//null;

        private void Model_Change_Apply()
        {
            MainForm_ProgracessBar_Display_01("Model Changing !!", 10);
            if (string.IsNullOrEmpty(LamiSystem.GetSet_Now_Model_Name) == false)
            {
                ubtnToolbarModel.Text = "MODEL  " + LamiSystem.GetSet_Now_Model_Name + " - " + LamiSystem.GetSet_Now_Model_Number;
                _strNow_Model_Name = LamiSystem.GetSet_Now_Model_Name;
                _strNow_Model_Number = LamiSystem.GetSet_Now_Model_Number;
            }

            string modelName = LamiSystem.GetSet_Now_Model_Name + "-" + LamiSystem.GetSet_Now_Model_Number + ".rcp";
            _strModelFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelName;
            //_strModelFileName = Environment.CurrentDirectory + "\\ModelData\\" + modelName;

            Model_Config_Apply_File_To_List(_strModelFileName);

            MainForm_ProgracessBar_Display_01("Model Changing !!", 30);
            LamiSystem.StrListRcpConData.Add(LamiSystem.GetSet_Now_Model_Name);
            LamiSystem.StrListRcpConData.Add(LamiSystem.GetSet_Now_Model_Number);
            //LamiSystem.StrListRcpConData[0] = LamiSystem.GetSet_Now_Model_Name;
            //LamiSystem.StrListRcpConData[1] = LamiSystem.GetSet_Now_Model_Number;            
            System_Config_Initionalize();
            MainForm_ProgracessBar_Display_01("Model Changing !!", 40);
            
            VisionLami_Config_Initionalize();
            MainForm_ProgracessBar_Display_01("Model Changing !!", 70);
            
            RecipeGap_Config_Initionalize();
            MainForm_ProgracessBar_Display_01("Model Changing !!", 80);

            Recipe_Config_Inspect_Output_Uper();
            MainForm_ProgracessBar_Display_01("Model Changing !!", 90);

            Recipe_Config_Inspect_Output_Down();
            MainForm_ProgracessBar_Display_01("Model Change Complite !", 100);

            //RecipeBiCell_Config_Initionalize();
            //uprog.Value = 90;
            //RecipeBiCell_Config_Inspect_Output();
            //uprog.Value = 100;
        }

        private void Model_Config_Apply_List_Clear()
        {
            LamiSystem.StrLstRcpConInspTitle_Uper.Clear();
            LamiSystem.StrLstRcpConInspData_Uper.Clear();

            LamiSystem.StrLstRcpConInspTitle_Down.Clear();
            LamiSystem.StrLstRcpConInspData_Down.Clear();
            
            LamiSystem.StrLstRcpConGridTitle_Uper.Clear();
            LamiSystem.StrLstRcpConGridData_Uper.Clear();

            LamiSystem.StrLstRcpConGridTitle_Down.Clear();
            LamiSystem.StrLstRcpConGridData_Down.Clear();

            LamiSystem.StrListRcpConTitle.Clear();
            LamiSystem.StrLstRcpConName.Clear();
            LamiSystem.StrListRcpConData.Clear();

            LamiSystem.StrListVisConGridTitle_Uper.Clear();
            LamiSystem.StrListVisConGridData_Uper.Clear();

            LamiSystem.StrListVisConGridTitle_Down.Clear();
            LamiSystem.StrListVisConGridData_Down.Clear();

            LamiSystem.StrListVisConTitle.Clear();
            LamiSystem.StrListVisConName.Clear();
            LamiSystem.StrListVisConData.Clear();

            LamiSystem.StrListSysConTitle.Clear();
            LamiSystem.StrListSysConName.Clear();
            LamiSystem.StrListSysConData.Clear();
        }
        private void Model_Config_Apply_File_To_List(string modelFilePath)
        {
            Model_Config_Apply_List_Clear();
            int configNumber = 0;
            using (StreamReader srFile = new StreamReader(modelFilePath, Encoding.Default, true))
            {
                int linecount = 0;
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    if (strLine == "")
                    {
                        configNumber++;
                        linecount = 0;
                        continue;
                    }
                    switch (configNumber)
                    {
                        case 0:
                            Model_Config_Apply_System_To_List(strLine, linecount);
                            break;
                        case 1:
                            Model_Config_Apply_Vision_To_List_Gap(strLine, linecount);
                            break;
                        case 2:
                            Model_Config_Apply_Vision_UperGrid_To_List(strLine, linecount);
                            break;
                        case 3:
                            Model_Config_Apply_Vision_DownGrid_To_List(strLine, linecount);
                            break;
                        
                        case 4:
                            Model_Config_Apply_RecipeGrid_To_List_Uper(strLine, linecount);
                            break;
                        case 5:
                            Model_Config_Apply_RecipeGrid_To_List_Down(strLine, linecount);
                            break;
                        case 6:
                            Model_Config_Apply_Recipe_UperInspect_To_List(strLine, linecount);
                            break;
                        case 7:
                            Model_Config_Apply_Recipe_DownInspect_To_List(strLine, linecount);
                            break;
                    }
                    linecount++;
                }
                Trace.WriteLine(linecount.ToString("000"));
            }
        }
/*
        private void Model_Config_Apply_RecipeInspect_To_List_BiCell(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrLstRcpConInspTitle_BiCell.Add(inputData[0]);
            LamiSystem.StrLstRcpConInspData_BiCell.Add(inputData[1]);
        }

        private void Model_Config_Apply_RecipeGrid_To_List_BiCell(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrLstRcpConGridTitle_BiCell.Add(inputData[0]);
            LamiSystem.StrLstRcpConGridData_BiCell.Add(inputData[1]);
        }

        private void Model_Config_Apply_Recipe_To_List_BiCell(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListRcpConTitle_BiCell.Add(inputData[0]);
            LamiSystem.StrLstRcpConName_BiCell.Add(inputData[1]);
            LamiSystem.StrListRcpConData_BiCell.Add(inputData[2]);
        }

        private void Model_Config_Apply_VisionGrid_To_List_BiCell(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListVisConGridTitle_Down.Add(inputData[0]);
            LamiSystem.StrListVisConGridData_Down.Add(inputData[1]);
        }

        
        private void Model_Config_Apply_Vision_To_List_BiCell(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListVisConTitle_BiCell.Add(inputData[0]);
            LamiSystem.StrListVisConName_BiCell.Add(inputData[1]);
            LamiSystem.StrListVisConData_BiCell.Add(inputData[2]);
        }
        */

        private void Model_Config_Apply_Recipe_UperInspect_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrLstRcpConInspTitle_Uper.Add(inputData[0]);
            LamiSystem.StrLstRcpConInspData_Uper.Add(inputData[1]);
        }

        private void Model_Config_Apply_Recipe_DownInspect_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrLstRcpConInspTitle_Down.Add(inputData[0]);
            LamiSystem.StrLstRcpConInspData_Down.Add(inputData[1]);
        }

        private void Model_Config_Apply_RecipeGrid_To_List_Uper(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrLstRcpConGridTitle_Uper.Add(inputData[0]);
            LamiSystem.StrLstRcpConGridData_Uper.Add(inputData[1]);
        }

        private void Model_Config_Apply_RecipeGrid_To_List_Down(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrLstRcpConGridTitle_Down.Add(inputData[0]);
            LamiSystem.StrLstRcpConGridData_Down.Add(inputData[1]);
        }

        private void Model_Config_Apply_Recipe_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListRcpConTitle.Add(inputData[0]);
            LamiSystem.StrLstRcpConName.Add(inputData[1]);
            LamiSystem.StrListRcpConData.Add(inputData[2]);
        }

        private void Model_Config_Apply_Vision_UperGrid_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListVisConGridTitle_Uper.Add(inputData[0]);
            LamiSystem.StrListVisConGridData_Uper.Add(inputData[1]);
        }

        private void Model_Config_Apply_Vision_DownGrid_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListVisConGridTitle_Down.Add(inputData[0]);
            LamiSystem.StrListVisConGridData_Down.Add(inputData[1]);
        }

        private void Model_Config_Apply_Vision_To_List_Gap(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListVisConTitle.Add(inputData[0]);
            LamiSystem.StrListVisConName.Add(inputData[1]);
            LamiSystem.StrListVisConData.Add(inputData[2]);
        }

        private int _intModel_System_Count = 0;
        private void Model_Config_Apply_System_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            LamiSystem.StrListSysConTitle.Add(inputData[0]);
            LamiSystem.StrListSysConName.Add(inputData[1]);
            LamiSystem.StrListSysConData.Add(inputData[2]);
        }


        /*
        private void Model_Config_Apply_File_To_List(string modelFilePath)
        {
            int configNumber = 0;
            using (StreamReader srFile = new StreamReader(modelFilePath, Encoding.Default, true))
            {
                int linecount = 0;
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    if (strLine == "")
                    {
                        configNumber++;
                        linecount = 0;
                        continue;
                    }
                    switch (configNumber)
                    {
                        case 0:
                            Model_Config_Apply_System_To_List(strLine, linecount);
                            break;
                        case 1:
                            Model_Config_Apply_Vision_To_List(strLine, linecount);
                            break;
                        case 2:
                            Model_Config_Apply_VisionGrid_To_List(strLine, linecount);
                            break;
                        case 3:
                            Model_Config_Apply_Recipe_To_List(strLine, linecount);
                            break;
                        case 4:
                            Model_Config_Apply_RecipeGrid_To_List(strLine, linecount);
                            break;
                        case 5:
                            Model_Config_Apply_RecipeInspect_To_List(strLine, linecount);
                            break;
                    }
                    linecount++;
                }
                Trace.WriteLine(linecount.ToString("000"));
            }
        }

        private void Model_Config_Apply_RecipeInspect_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            AlignSystem.StrLstRcpConInspTitle[lineCount] = inputData[0];
            AlignSystem.StrLstRcpConInspData[lineCount] = inputData[1];
        }

        private void Model_Config_Apply_RecipeGrid_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            AlignSystem.StrLstRcpConGridTitle[lineCount] = inputData[0];
            AlignSystem.StrLstRcpConGridData[lineCount] = inputData[1];
        }

        private void Model_Config_Apply_Recipe_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            AlignSystem.StrListRcpConTitle[lineCount] = inputData[0];
            AlignSystem.StrLstRcpConName_Gap[lineCount] = inputData[1];
            AlignSystem.StrListRcpConData_Gap[lineCount] = inputData[2];
        }

        private void Model_Config_Apply_VisionGrid_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            AlignSystem.StrListVisConGridTitle[lineCount] = inputData[0];
            AlignSystem.StrListVisConGridData[lineCount] = inputData[1];
        }

        private void Model_Config_Apply_Vision_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            AlignSystem.StrListVisConTitle[lineCount] = inputData[0];
            AlignSystem.StrListVisConName[lineCount] = inputData[1];
            AlignSystem.StrListVisConData[lineCount] = inputData[2];
        }

        private int _intModel_System_Count = 0;
        private void Model_Config_Apply_System_To_List(string configData, int lineCount)
        {
            List<string> inputData = Model_Config_Apply_Data_Pasing(configData);
            AlignSystem.StrListSysConTitle[lineCount] = inputData[0];
            AlignSystem.StrListSysConName[lineCount] = inputData[1];
            AlignSystem.StrListSysConData[lineCount] = inputData[2];
        }
        */
        private List<string> Model_Config_Apply_Data_Pasing(string configData)
        {
            List<string> readList = new List<string>();
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (configData.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = configData.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = configData.IndexOf("\t", intStartIndex);
                }

                strCellData = configData.Substring(intStartIndex, intEndIndex - intStartIndex);
                readList.Add(strCellData);
                //SetReg(RegNodePathVisionConfigGrid, _intVisionGridCellCount.ToString("00"), strCellData);
                intGridCellCount++;
                //_intVisionGridCellCount++;
            }
            return readList;
        }

        private delegate void Delegate_ProgracessBar_Display_01(string strMessage, int ProgBarValue);
        
        public void MainForm_ProgracessBar_Display_01(string strMessage, int ProgBarValue)
        {
            if (InvokeRequired)
            {
                Delegate_ProgracessBar_Display_01 del = MainForm_ProgracessBar_Display_01;
                Invoke(del, strMessage, ProgBarValue);
            }
            else
            {
                ustbar.Text = strMessage;
                ustbar.Refresh();

                uprog.Value = ProgBarValue;
                uprog.Refresh();
            }
        }


        /*
        
        private void Display_Model_Changing_ProgressBar()
        {
            int progValue = 2;
            while (progValue > 0)
            {
                Thread.Sleep(10);
                progValue += 2;
                if (progValue > 100) progValue = 2;
                //Display_Model_Changing_Progress(progValue);
            }
        }

        private delegate void Delegate_Model_Changing_Progress(string iValue);

        private void Display_Model_Changing_Progress(string iValue)
        {
            if (InvokeRequired)
            {
                Delegate_Model_Changing_Progress del = Display_Model_Changing_Progress;
                Invoke(del, iValue);
            }
            else
            {
                //picBox01.Visible = true;
                ustbar.Text  = iValue;
                //picBox01.ImageIpl = inspImage;
                ustbar.Refresh();
            }
        }
        */

        private bool eventing_ModelAdding(string addName, string addNumber)
        {
            return Model_Config_Add_List_To_File(addName, addNumber);
        }

        private bool _iSaveCompliteFlag = false;
        private string _strModelFileName = null;
        private bool Model_Config_Add_List_To_File(string addName, string addNumber)
        {
            try
            {
                bool WriteResult = false;

                //세이브를 모두 진행 했느지를 가지는 플래그 멤버
                _iSaveCompliteFlag = false;

                string modelName = addName + "-" + addNumber + ".rcp";
                string addModelFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelName;
                //string addModelFileName = Environment.CurrentDirectory + "\\ModelData\\" + modelName;
                System.IO.StreamWriter file = new System.IO.StreamWriter(addModelFileName, false, Encoding.Default);
                file.Flush();
                for (int i = 0; i < LamiSystem.StrListSysConTitle.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrListSysConTitle[i] + "\t" + LamiSystem.StrListSysConName[i] + "\t" + LamiSystem.StrListSysConData[i] + "\t");
                }
                file.WriteLine();

                MainForm_ProgracessBar_Display_01("Model Changing !!", 40);

                for (int i = 0; i < LamiSystem.StrListVisConTitle.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrListVisConTitle[i] + "\t" + LamiSystem.StrListVisConName[i] + "\t" + LamiSystem.StrListVisConData[i] + "\t");
                }
                file.WriteLine();

                MainForm_ProgracessBar_Display_01("Model Changing !!", 50);

                for (int i = 0; i < LamiSystem.StrListVisConGridTitle_Uper.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrListVisConGridTitle_Uper[i] + "\t" + LamiSystem.StrListVisConGridData_Uper[i] + "\t");
                }
                file.WriteLine();

                for (int i = 0; i < LamiSystem.StrListVisConGridTitle_Down.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrListVisConGridTitle_Down[i] + "\t" + LamiSystem.StrListVisConGridData_Down[i] + "\t");
                }
                file.WriteLine();

                MainForm_ProgracessBar_Display_01("Model Changing !!", 60);

                //             for (int i = 0; i < LamiSystem.StrListRcpConTitle.Count; i++)
                //             {
                //                 file.WriteLine(LamiSystem.StrListRcpConTitle[i] + "\t" + LamiSystem.StrLstRcpConName[i] + "\t" + LamiSystem.StrListRcpConData[i] + "\t");
                //             }
                //             file.WriteLine();

                for (int i = 0; i < LamiSystem.StrLstRcpConGridTitle_Uper.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrLstRcpConGridTitle_Uper[i] + "\t" + LamiSystem.StrLstRcpConGridData_Uper[i] + "\t");
                }
                file.WriteLine();

                MainForm_ProgracessBar_Display_01("Model Changing !!", 70);

                for (int i = 0; i < LamiSystem.StrLstRcpConGridTitle_Down.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrLstRcpConGridTitle_Down[i] + "\t" + LamiSystem.StrLstRcpConGridData_Down[i] + "\t");
                }
                file.WriteLine();

                MainForm_ProgracessBar_Display_01("Model Changing !!", 80);

                for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_Uper.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrLstRcpConInspTitle_Uper[i] + "\t" + LamiSystem.StrLstRcpConInspData_Uper[i] + "\t");
                }
                file.WriteLine();

                MainForm_ProgracessBar_Display_01("Model Changing !!", 90);

                for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_Down.Count; i++)
                {
                    file.WriteLine(LamiSystem.StrLstRcpConInspTitle_Down[i] + "\t" + LamiSystem.StrLstRcpConInspData_Down[i] + "\t");
                }
                //file.WriteLine();
                file.Close();

                MainForm_ProgracessBar_Display_01("Model Changing Complite !", 100);

                _iSaveCompliteFlag = true;

                WriteResult = true;
                return WriteResult;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
       
        }
        #endregion
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// 레시피 설정 탭을 운용하고 제어함.//////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region 레시피 설정 탭

        #region 레시피 설정 : 멤버



        //사용자 정의 함수 DrawArea를 제어하는 클래스
        private readonly ControlDrawArea _Uper_Control_DrawArea;
        private readonly ControlDrawArea _Down_Control_DrawArea;

        //private Infragistics.Win.ValueList _vListMeasMethod = new Infragistics.Win.ValueList();
        //private Infragistics.Win.ValueList _vListMeasDivid = new Infragistics.Win.ValueList();
        //private Infragistics.Win.ValueList _imgSideList = new Infragistics.Win.ValueList();

        #endregion


        #region 레시피 설정 탭 : 프로퍼티

        #endregion


        #region 레시피 설정 탭 : 생성자, 초기화
        /// 콘피그 저장 레지스트리가 비어 있는지 확인해서 비어 있다면
        /// 텍스트파일에서 디폴트 설정값을 읽어와서 레지스트리에 저장한다.
        /*
        private void RecipeBiCell_Config_Initionalize()
        {
            //티칭 탭을 구성하고 있는 클래스 구성용소이다.
            //DrawArea사용자 정의 콘트롤을 이용하는 클래스이다.
            _BiCell_Control_DrawArea.Initialize_Control();

            RecipeBiCell_drawArea1.pictureBox1.ImageIpl =
                IplImage.FromBitmap(SystemAlign.Properties.Resources.BiCell);

            //LamiSystem.GetSet_Now_Model_Name = dlgModelReg.GetSetModelName;
            //LamiSystem.GetSet_Now_Model_Number = dlgModelReg.GetSetModelNumber;

            
            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            RecipeBiCell_Config_ListData_To_Viewer();

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            RecipeBiCell_Config_ListData_To_Viewer_Grid();
            

            RecipeBiCell_Config_DropDownList_Setup();


            RecipeBiCell_Config_Grid_Output();

            RecipeBiCell_ImageBox_Line_Drawing();

            RecipeBiCell_drawArea1.GetSetGraphicsList.UnselectAll();

            if (Recipe_uTxt_ModelName.Text != string.Empty)
            {
                RecipeBiCell_uTxt_ModelName.Text = Recipe_uTxt_ModelName.Text;
                RecipeBiCell_uTxt_RecipeNo.Text = Recipe_uTxt_ModelNumber.Text;
            }

            else
            {
                RecipeBiCell_uTxt_ModelName.Text = LamiSystem.GetSet_Now_Model_Name;
                RecipeBiCell_uTxt_RecipeNo.Text = LamiSystem.GetSet_Now_Model_Number;
            }
            
            //RecipeGap_Config_Inspect_Output();
        }
        */

        /*
        /// <summary>
        /// 데이터 소스에 알맞게 각각의 셀의 스타일과 데이터를 설정한다.
        /// 드롭다운 리스트의 내용(텍스트, 이미지)과 버튼의 유,무 등을 설정한다.
        /// </summary>
        private void RecipeBiCell_Config_DropDownList_Setup()
        {
            //클리어 하지 않으면 탭을 열때마다 생성된다.
            LamiSystem._vListMeasMethod_BiCell.ValueListItems.Clear();
            LamiSystem._imgSideList_BiCell.ValueListItems.Clear();
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Clear();

            LamiSystem._vListMeasMethod_BiCell.ValueListItems.Add("위치");
            LamiSystem._vListMeasMethod_BiCell.ValueListItems.Add("거리");
            //LamiSystem._vListMeasMethod_BiCell.ValueListItems.Add("넓이");

            LamiSystem._vListMeasPola_BiCell.ValueListItems.Add("흑백");
            LamiSystem._vListMeasPola_BiCell.ValueListItems.Add("백흑");

            LamiSystem._imgSideList_BiCell.ValueListItems.Add(0, "0");
            LamiSystem._imgSideList_BiCell.ValueListItems.Add(1, "1");
            LamiSystem._imgSideList_BiCell.ValueListItems.Add(2, "2");
            LamiSystem._imgSideList_BiCell.ValueListItems.Add(3, "3");
            LamiSystem._imgSideList_BiCell.ValueListItems[0].Appearance.Image = global::SystemAlign.Properties.Resources._19;
            LamiSystem._imgSideList_BiCell.ValueListItems[0].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList_BiCell.ValueListItems[0].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList_BiCell.ValueListItems[1].Appearance.Image = global::SystemAlign.Properties.Resources._23;
            LamiSystem._imgSideList_BiCell.ValueListItems[1].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList_BiCell.ValueListItems[1].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList_BiCell.ValueListItems[2].Appearance.Image = global::SystemAlign.Properties.Resources._24;
            LamiSystem._imgSideList_BiCell.ValueListItems[2].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList_BiCell.ValueListItems[2].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList_BiCell.ValueListItems[3].Appearance.Image = global::SystemAlign.Properties.Resources._28;
            LamiSystem._imgSideList_BiCell.ValueListItems[3].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList_BiCell.ValueListItems[3].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList_BiCell.DisplayStyle = ValueListDisplayStyle.Picture;

            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("1");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("2");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("3");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("4");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("5");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("6");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("7");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("8");
            LamiSystem._vListMeasDivid_BiCell.ValueListItems.Add("9");

            uGrd_Recipe_Test_Gap.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
        }
        */

        //20150307 WKB 209
        /// 콘피그 저장 레지스트리가 비어 있는지 확인해서 비어 있다면
        /// 텍스트파일에서 디폴트 설정값을 읽어와서 레지스트리에 저장한다.
        private void RecipeGap_Config_Initionalize()
        {
            RecipeGap_drawArea1.pictureBox1.Image = Properties.Resources.BiCell_Top;
            _Uper_Control_DrawArea.Initialize_Control_Uper();

            RecipeGap_drawArea2.pictureBox1.Image = Properties.Resources.BiCell_Bot;
            _Down_Control_DrawArea.Initialize_Control_Down();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 10);

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            Recipe_Config_ListData_To_UperGrid();
            Recipe_Config_ListData_To_DownGrid();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 30);

            Recipe_Uper_Config_DropDownList_Setup();
            Recipe_Down_Config_DropDownList_Setup();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 50);

            Recipe_Config_UperGrid_Output();
            Recipe_Config_DownGrid_Output();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 60);

            Recipe_Config_Inspect_Output_Uper();
            Recipe_Config_Inspect_Output_Down();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 70);

            Recipe_Uper_Insert_Inspect();
            Recipe_Down_Insert_Inspect();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 90);

            RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            RecipeGap_drawArea2.GetSetGraphicsList.UnselectAll();
        }

        //20150307 WKB 208
        /*
        /// 콘피그 저장 레지스트리가 비어 있는지 확인해서 비어 있다면
        /// 텍스트파일에서 디폴트 설정값을 읽어와서 레지스트리에 저장한다.
        private void RecipeGap_Config_Initionalize()
        {
            RecipeGap_drawArea1.pictureBox1.Image = Properties.Resources.BiCell_Top;
            _Uper_Control_DrawArea.Initialize_Control_Uper();

            RecipeGap_drawArea2.pictureBox1.Image = Properties.Resources.BiCell_Bot;
            _Down_Control_DrawArea.Initialize_Control_Down();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 10);

            //리스트 배열에 저장되어져 있는 값을 표시해준다.
            Recipe_Config_ListData_To_UperGrid();
            Recipe_Config_ListData_To_DownGrid();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 30);

            Recipe_Uper_Config_DropDownList_Setup();
            Recipe_Down_Config_DropDownList_Setup();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 50);

            Recipe_Config_UperGrid_Output();
            Recipe_Config_DownGrid_Output();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 60);

            Recipe_Config_Inspect_Output_Uper();
            Recipe_Config_Inspect_Output_Down();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 70);

            Recipe_Uper_Insert_Inspect();
            Recipe_Down_Insert_Inspect();

            MainForm_ProgracessBar_Display_01("Model Recipe Data Loading !", 90);

            RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            RecipeGap_drawArea2.GetSetGraphicsList.UnselectAll();
        }
        */

        /*
        /// strListConfigRecipeData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void RecipeGap_Config_ListData_To_Viewer()
        {
            for (int i = 0; i < LamiSystem.StrLstRcpConName_Gap.Count; i++)
            {
                ArrayList al = new ArrayList();

                getControls(this.Controls.Find(LamiSystem.StrLstRcpConName_Gap[i], true), ref al);

                if (al[0] is UltraCheckEditor)
                {
                    if (LamiSystem.StrListRcpConData_Gap[i] == "True")
                        ((UltraCheckEditor)al[0]).Checked = true;
                    else
                        ((UltraCheckEditor)al[0]).Checked = false;
                }
                else if (al[0] is UltraComboEditor)
                {
                    if (LamiSystem.StrListRcpConData_Gap[i] == "ON")
                        ((UltraComboEditor)al[0]).SelectedIndex = 0;
                    else
                        ((UltraComboEditor)al[0]).SelectedIndex = 1;
                }
                else if (al[0] is UltraTextEditor)
                {
                    string dataTemp = LamiSystem.StrListRcpConData_Gap[i];
                    ((UltraTextEditor)al[0]).Text = LamiSystem.StrListRcpConData_Gap[i];
                }
            }
        }
        */

        
        /// _strListRecipeConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void Recipe_Config_ListData_To_UperGrid()
        {
            uDS_Recipe_Uper.Rows.Clear();

            //이전 레시피의 Row 수량을 저장한다.
            int iRecipeRows = LamiSystem.StrLstRcpConGridData_Uper.Count/11;

            string strCellData = string.Empty;
            int rowCount = 0;
            for (int i = 0; i < iRecipeRows; i++)
            {
                for (int j = 0; j < uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    if (j == 0) uDS_Recipe_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "", "", "" });

                    //20150307 WKB 208
                    //uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[j].Value = LamiSystem.StrLstRcpConGridData_Uper[(i * 11) + j];
                    
                    //20150307 WKB 209
                    uDS_Recipe_Uper.Rows[i].SetCellValue(j, LamiSystem.StrLstRcpConGridData_Uper[(i * 11) + j]);
                }
            }
        }

        private void Recipe_Config_ListData_To_DownGrid()
        {
            uDS_Recipe_Down.Rows.Clear();
           
            //이전 레시피의 Row 수량을 저장한다.
            int iRecipeRows = LamiSystem.StrLstRcpConGridData_Down.Count / 11;

            string strCellData = string.Empty;
            int rowCount = 0;
            for (int i = 0; i < iRecipeRows; i++)
            {
                for (int j = 0; j < uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    if (j == 0) uDS_Recipe_Down.Rows.Add(true, new Object[] {"", "", "", "", "", "", "", "", "", "", ""});
                    uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[j].Value = LamiSystem.StrLstRcpConGridData_Down[(i*11) + j];
                }
            }
        }
        
        /*
        private void Recipe_Config_ListData_To_UperGrid()
        {            
            uDS_Recipe_Uper.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrLstRcpConGridData_Uper.Count; i++)
            {
                int cellCount = i % (uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count);
                if (cellCount == 0)
                {
                    uDS_Recipe_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "", "", "" });
                }

                uGrd_Recipe_UperData.DisplayLayout.Rows[rowCount].Cells[cellCount].Value = LamiSystem.StrLstRcpConGridData_Uper[i];
                if (cellCount == uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count - 1) rowCount++;
            }
        }
       

        /// _strListRecipeConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void Recipe_Config_ListData_To_DownGrid()
        {
            uDS_Recipe_Down.Rows.Clear();
            int rowCount = 0;
            for (int i = 0; i < LamiSystem.StrLstRcpConGridData_Down.Count; i++)
            {
                
                int cellCount = i % (uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count);
                if (cellCount == 0)
                {
                    uDS_Recipe_Down.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "", "", "" });
                }

                uGrd_Recipe_DownData.DisplayLayout.Rows[rowCount].Cells[cellCount].Value = LamiSystem.StrLstRcpConGridData_Down[i];
                if (cellCount == uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count - 1) rowCount++;
            }
        }
 */
        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void Recipe_Config_Register_To_UperGrid()
        {
            Recipe_Config_Register_To_List_UperGrid();
            LamiSystem.StrLstRcpConGridData_Uper.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConGridTitle_Uper.Count; i++)
            {
                string strRegisterGridData = RecipeGap_Config_Register_To_List_Data(LamiSystem.RegPathRcpConGrid_Uper,LamiSystem.StrLstRcpConGridTitle_Uper[i]);
                LamiSystem.StrLstRcpConGridData_Uper.Add(strRegisterGridData);
            }
        }

        private void Recipe_Config_Register_To_DownGrid()
        {
            Recipe_Config_Register_To_List_DownGrid();
            LamiSystem.StrLstRcpConGridData_Down.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConGridTitle_Down.Count; i++)
            {
                string strRegisterGridData = RecipeGap_Config_Register_To_List_Data(LamiSystem.RegPathRcpConGrid_Down, LamiSystem.StrLstRcpConGridTitle_Down[i]);
                LamiSystem.StrLstRcpConGridData_Down.Add(strRegisterGridData);
            }
        }

        //레지스트리의 데이터 값을 읽와서 리스트 배열에 저장한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름을 가지는 문자열 배열
        //3번 파람: 레지스트리에서 읽은 값을 저장할 문자열 배열
        public void RecipeGap_Config_Register_To_List_Data(string strNodePath, List<string> regTitle, List<string> regData)
        {
            regData.Clear();

            for (int i = 0; i < regTitle.Count; i++)
            {
                regData.Add(this.GetReg(strNodePath, regTitle[i]));
            }
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void Recipe_Config_Register_To_List_UperGrid()
        {
            Register_To_StringList(LamiSystem.RegPathRcpConGrid_Uper, ref LamiSystem.StrLstRcpConGridTitle_Uper);
        }

        private void Recipe_Config_Register_To_List_DownGrid()
        {
            Register_To_StringList(LamiSystem.RegPathRcpConGrid_Down, ref LamiSystem.StrLstRcpConGridTitle_Down);
        }

        //레지스터에 저장되어져 있는 타이틀, 네임, 데이터 값을 읽어와서
        //각각의 타이틀 리스트, 네임 리스트, 데이터 리스트에 저장한다.
        private void RecipeGap_Config_Register_To_Lists()
        {
            RecipeGap_Config_Register_To_List_Title();
            for (int i = 0; i < LamiSystem.StrListRcpConTitle.Count; i++)
            {
                string strRegisterNameNData = RecipeGap_Config_Register_To_List_Data(LamiSystem.RegPathRcpCon, LamiSystem.StrListRcpConTitle[i]);
                if (strRegisterNameNData.Length > 2) RecipeGap_Config_Register_To_List_NameNData(strRegisterNameNData);

                if (LamiSystem.StrListRcpConTitle[i] == "모델 이름")
                    LamiSystem.GetSet_Now_Model_Name = RecipeGap_Config_Register_To_Var(LamiSystem.StrListRcpConTitle[i], strRegisterNameNData);
                else if (LamiSystem.StrListRcpConTitle[i] == "모델 번호")
                    LamiSystem.GetSet_Now_Model_Number = RecipeGap_Config_Register_To_Var(LamiSystem.StrListRcpConTitle[i], strRegisterNameNData);
            }

            if (string.IsNullOrEmpty(LamiSystem.GetSet_Now_Model_Name) == false)
            {
                ubtnToolbarModel.Text = "MODEL  " + LamiSystem.GetSet_Now_Model_Name + " - " + LamiSystem.GetSet_Now_Model_Number;
                _strNow_Model_Name = LamiSystem.GetSet_Now_Model_Name;
                _strNow_Model_Number = LamiSystem.GetSet_Now_Model_Number;
                
            }
        }

        private string RecipeGap_Config_Register_To_Var(string title, string analStr)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strModelData = null;
            intStartIndex = 0;
            intEndIndex = analStr.IndexOf("\t", intStartIndex);
            //strModelData = analStr.Substring(intStartIndex, intEndIndex - intStartIndex);

            intStartIndex = intEndIndex + 1;
            intEndIndex = analStr.Length;
            strModelData = analStr.Substring(intStartIndex, intEndIndex - intStartIndex);
           
            return strModelData;
        }

        //레지스터에 저장되어져 있는 네임과 데이터를 읽어와서 네임, 데이터 리스트배열에 저장한다.
        private void RecipeGap_Config_Register_To_List_NameNData(string readData)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intTitleCount = 0;
            string strTitleName = null;
            intStartIndex = 0;
            intEndIndex = readData.IndexOf("\t", intStartIndex);
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrLstRcpConName.Add(strTitleName);

            intStartIndex = intEndIndex + 1;
            intEndIndex = readData.Length;
            strTitleName = readData.Substring(intStartIndex, intEndIndex - intStartIndex);
            LamiSystem.StrListRcpConData.Add(strTitleName);
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
        public string RecipeGap_Config_Register_To_List_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        //레지스터에 저장되어져 있는 타이틀값을 읽어와서 타이틀 리스트배열에 저장한다.
        private void RecipeGap_Config_Register_To_List_Title()
        {
            Register_To_StringList(LamiSystem.RegPathRcpCon, ref LamiSystem.StrListRcpConTitle);
        }

        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        private int _intRecipeUperGridCellCount = 0;
        
        private void Recipe_Config_File_To_Register_UperGrid()
        {
            _intRecipeUperGridCellCount = 0;
            //byte[] resourceObject = SystemAlign.Properties.Resources.RecipeUperGridDefault;
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeGridDefaultUper;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Recipe_Config_StringParsing_UperGrid(strLine);
                }
            }
            ioStream.Close();
        }

        //비전 그리드 설정 파일을 열때 0으로 초기화 해주어야 한다.
        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private int _intRecipeDownGridCellCount = 0;
        private void Recipe_Config_File_To_Register_DownGrid()
        {
            _intRecipeDownGridCellCount = 0;
            //byte[] resourceObject = SystemAlign.Properties.Resources.RecipeDownGridDefault;
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeGridDefaultDown;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    Recipe_Config_StringParsing_DownGrid(strLine);
                }
            }
            ioStream.Close();
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Recipe_Config_StringParsing_UperGrid(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathRcpConGrid_Uper, _intRecipeUperGridCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intRecipeUperGridCellCount++;
            }
        }

        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void Recipe_Config_StringParsing_DownGrid(string gridDataLine)
        {
            int intStartIndex = 0;
            int intEndIndex = 0;
            int intGridCellCount = 0;
            string strCellData = "";

            //이 반복 모듈은 텍스트 파일에 라인 끝에도 탭을 추가 했을 때 사용하는 것임.
            while (gridDataLine.IndexOf("\t", intEndIndex + 1) != -1)
            {
                if (intGridCellCount == 0)
                {
                    intStartIndex = 0;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }
                else
                {
                    intStartIndex = intEndIndex + 1;
                    intEndIndex = gridDataLine.IndexOf("\t", intStartIndex);
                }

                strCellData = gridDataLine.Substring(intStartIndex, intEndIndex - intStartIndex);
                SetReg(LamiSystem.RegPathRcpConGrid_Down, _intRecipeDownGridCellCount.ToString("000"), strCellData);
                intGridCellCount++;
                _intRecipeDownGridCellCount++;
            }
        }


        //파일에서 일어온 정보를 타이틀 과 콘트롤&데이터로 분리하여 레지스트리에
        //타이틀(타이틀 리스트 정보)과 값(콘트롤 + 데이터 리스트)으로 저장한다.
        private void RecipeGap_Config_StringParsing(string strLine)
        {
            string readTitle = null, readData = null;
            int indexNo = strLine.IndexOf("\t");
            readTitle = strLine.Substring(0, indexNo);
            readData = strLine.Substring(indexNo + 1, strLine.Length - indexNo - 1);

            SetReg(LamiSystem.RegPathRcpCon, readTitle, readData);
        }

        //파일의 정보를 읽어와서 레지스트리에 저장하는 함수 
        //디폴트 설정 파일을 한줄씩 일어와서 파싱함수를 호출한다.
        private void RecipeGap_Config_File_To_Register()
        {
            //Rources에 들록되어 있는 파일을 사용한다.
            byte[] resourceObject = SystemAlign.Properties.Resources.RecipeConfigDefault;
            System.IO.Stream ioStream = new MemoryStream(resourceObject);
            //using (var srFile = new StreamReader(ioStream, Encoding.GetEncoding("ks_c_5601-1987"), true))
            using (var srFile = new StreamReader(ioStream, Encoding.Default, true))
            {
                string strLine;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    RecipeGap_Config_StringParsing(strLine);
                }
            }
            ioStream.Close();
        }


        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool RecipeGap_Config_Register_Empty_Check(string checkingRegNodePath)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(checkingRegNodePath, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }

        // SetReg(LamiSystem.RegPathRcpCon, "상단 항목", uGrd_Recipe_UperData.Rows.Count.ToString("0"));
        // SetReg(LamiSystem.RegPathRcpCon, "하단 항목", uGrd_Recipe_DownData.Rows.Count.ToString("0"));
        //시스템 콘피그 설정값이 저장되는 레지스터의 내용이 존재하는지를 
        //검사하여 그 결과를 리턴한다. 없다면 true, 있다면 false 을 리턴한다.
        public bool Recipe_Uper_Grid_Register_Empty_Check(string checkingRegNodePath)
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(checkingRegNodePath, true);
            if (reg == null)
            {
                return true;
            }
            else
            {
                reg.Close();
                return false;
            }
        }


        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void RecipeGap_Config_Viewer_To_List_Data()
        {
            LamiSystem.StrListRcpConData.Clear();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("RecipeConfig_Tab_BackPanel_Gap", true), ref al);

            for (int i = 0; i < LamiSystem.StrLstRcpConName.Count; i++)
            {
                for (int j = 0; j < al.Count; j++)
                {
                    if (((Control)al[j]).Name.ToString() == LamiSystem.StrLstRcpConName[i])
                    {
                        if (al[j] is UltraCheckEditor)
                        {
                            LamiSystem.StrListRcpConData.Add(((UltraCheckEditor)al[j]).Checked.ToString());
                        }
                        else
                        {
                            LamiSystem.StrListRcpConData.Add(((Control)al[j]).Text);
                        }
                        break;
                    }
                }
            }
        }

        /*
        private void RecipeBiCell_Config_Viewer_To_List_Data()
        {
            LamiSystem.StrListRcpConData_BiCell.Clear();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("RecipeConfig_Tab_BackPanel_BiCell", true), ref al);

            for (int i = 0; i < LamiSystem.StrLstRcpConName_BiCell.Count; i++)
            {
                for (int j = 0; j < al.Count; j++)
                {
                    if (((Control)al[j]).Name.ToString() == LamiSystem.StrLstRcpConName_BiCell[i])
                    {
                        if (al[j] is UltraCheckEditor)
                        {
                            LamiSystem.StrListRcpConData_BiCell.Add(((UltraCheckEditor)al[j]).Checked.ToString());
                        }
                        else
                        {
                            LamiSystem.StrListRcpConData_BiCell.Add(((Control)al[j]).Text);
                        }
                        break;
                    }
                }
            }
        }
        */

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void Recipe_Config_ListData_To_Register()
        {
            LamiSystem.StrListRcpConData.Clear();
            LamiSystem.StrListRcpConData.Add(_strNow_Model_Name);
            LamiSystem.StrListRcpConData.Add(_strNow_Model_Number);
            LamiSystem.StrListRcpConData.Add(uGrd_Recipe_UperData.Rows.Count.ToString("0"));
            LamiSystem.StrListRcpConData.Add(uGrd_Recipe_DownData.Rows.Count.ToString("0"));
            Recipe_Config_Lists_To_Register(LamiSystem.RegPathSystemStatus, LamiSystem.StrListRcpConTitle, LamiSystem.StrLstRcpConName, LamiSystem.StrListRcpConData);
            SetReg(LamiSystem.RegPathRcpCon, "상단 항목", uGrd_Recipe_UperData.Rows.Count.ToString("0"));
            SetReg(LamiSystem.RegPathRcpCon, "하단 항목", uGrd_Recipe_DownData.Rows.Count.ToString("0"));
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void Recipe_Config_Lists_To_Register(string strNodePath, List<string> regTitle, List<string> regControl,List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                //this.SetReg(strNodePath, regTitle[i], regControl[i] + "\t" + regData[i]);
                //System Lamination에 저장되어지는 값이 덱스트박스명과 함께 저장되지 않도록 수정함.
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void Recipe_Config_UperGrid_To_Register()
        {
            Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathRcpConGrid_Uper, false);
            Registry.CurrentUser.CreateSubKey(LamiSystem.RegPathRcpConGrid_Uper, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Recipe_Config_Grid_To_Register(LamiSystem.RegPathRcpConGrid_Uper, LamiSystem.StrLstRcpConGridTitle_Uper, LamiSystem.StrLstRcpConGridData_Uper);
        }

        private void Recipe_Config_DownGrid_To_Register()
        {
            Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathRcpConGrid_Down, false);
            Registry.CurrentUser.CreateSubKey(LamiSystem.RegPathRcpConGrid_Down, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Recipe_Config_Grid_To_Register(LamiSystem.RegPathRcpConGrid_Down , LamiSystem.StrLstRcpConGridTitle_Down, LamiSystem.StrLstRcpConGridData_Down);
        }


        /*
        private void Recipe_Config_ListData_To_Register_Grid()
        {
            Recipe_Config_Lists_To_Register_Grid(CInspection_Folding_Align.RegPathRcpConGrid, AlignSystem.StrLstRcpConGridTitle,
                AlignSystem.StrLstRcpConGridData);
        }
        */

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void Recipe_Config_Grid_To_Register(string strNodePath, List<string> regTitle, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                Trace.WriteLine("Recipe Grid Title : " + regTitle[i] + "    Grid Data :" + regData[i]);
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void Recipe_Config_UperInspect_To_Register()
        {
            Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathRcpConInsp_Uper, false);
            Registry.CurrentUser.CreateSubKey(LamiSystem.RegPathRcpConInsp_Uper, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Recipe_Config_Lists_To_Register_Inspect(LamiSystem.RegPathRcpConInsp_Uper, LamiSystem.StrLstRcpConInspTitle_Uper, LamiSystem.StrLstRcpConInspData_Uper);
        }

        private void Recipe_Config_DownInspect_To_Register()
        {
            Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathRcpConInsp_Down, false);
            Registry.CurrentUser.CreateSubKey(LamiSystem.RegPathRcpConInsp_Down, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Recipe_Config_Lists_To_Register_Inspect(LamiSystem.RegPathRcpConInsp_Down, LamiSystem.StrLstRcpConInspTitle_Down, LamiSystem.StrLstRcpConInspData_Down);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void Recipe_Config_Lists_To_Register_Inspect(string strNodePath, List<string> regTitle, List<string> regData)
        {
            for (int i = 0; i < regData.Count; i++)
            {
                Trace.WriteLine("Recipe Grid Title : " + regTitle[i] + "    Grid Data :" + regData[i]);
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }

        /*
        ////////////////////////////////////////////////////////////////////////////////////
        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void RecipeBiCell_Config_ListData_To_Register()
        {
            RecipeBiCell_Config_Lists_To_Register(LamiSystem.RegPathRcpCon_BiCell, LamiSystem.StrListRcpConTitle_BiCell, LamiSystem.StrLstRcpConName_BiCell,
                LamiSystem.StrListRcpConData_BiCell);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void RecipeBiCell_Config_Lists_To_Register(string strNodePath, List<string> regTitle, List<string> regControl, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                this.SetReg(strNodePath, regTitle[i], regControl[i] + "\t" + regData[i]);
            }
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void RecipeBiCell_Config_ListData_To_Register_Grid()
        {
            RecipeBiCell_Config_Lists_To_Register_Grid(LamiSystem.RegPathRcpConGrid_BiCell, LamiSystem.StrLstRcpConGridTitle_Down,
                LamiSystem.StrLstRcpConGridData_Down);
        }

      
        //리스트 배열의 값을 레지스트리에 저장한다.
        public void RecipeBiCell_Config_Lists_To_Register_Grid(string strNodePath, List<string> regTitle, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                Trace.WriteLine("Recipe Grid Title : " + regTitle[i] + "    Grid Data :" + regData[i]);
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void RecipeBiCell_Config_ListData_To_Register_Inspect()
        {
            RecipeBiCell_Config_Lists_To_Register_Inspect(LamiSystem.RegPathRcpConInsp_BiCell, LamiSystem.StrLstRcpConInspTitle_Down,
                LamiSystem.StrLstRcpConInspData_Down);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void RecipeBiCell_Config_Lists_To_Register_Inspect(string strNodePath, List<string> regTitle, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                Trace.WriteLine("Recipe Grid Title : " + regTitle[i] + "    Grid Data :" + regData[i]);
                this.SetReg(strNodePath, regTitle[i], regData[i]);
            }
        }
        */ 
        
        //라미네이션으로 넘어가면서 비전설정 탭에서 항목을 추가했을 때 이를 적용하기 위해서
        //수정을 진행함.
        private Graphics Gc_Roi_Uper;// = _Uper_Control_DrawArea._drawArea1.pictureBox1.CreateGraphics();

        //20150309 WKB 209
        private void Recipe_Config_Inspect_Output_Uper()
        {
            int ItemCount = 14;
            int rectCount = LamiSystem.StrLstRcpConInspTitle_Uper.Count / ItemCount;
            _Uper_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();
            System.Drawing.Rectangle[] Rects = new System.Drawing.Rectangle[rectCount];
            System.Drawing.Rectangle tempRectOld = new System.Drawing.Rectangle(0, 0, 0, 0);
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < rectCount; i++)
            {
                tempRectNew.X = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 0]);
                tempRectNew.Y = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 1]);
                tempRectNew.Width = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 2]);
                tempRectNew.Height = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 3]);


                LamiSystem.RectListRecipeBoxZone_Uper.Add(tempRectNew);

                if (tempRectOld != tempRectNew)
                {
                    Rects[i] = LamiSystem.RectListRecipeBoxZone_Uper[i];
                    _Uper_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Uper[i]);
                    tempRectOld = tempRectNew;
                }
            }
            Gc_Roi_Uper = RecipeGap_drawArea1.pictureBox1.CreateGraphics();
        }


        //20150309 WKB 208
        /*
         private void Recipe_Config_Inspect_Output_Uper()
        {
            int ItemCount = 14;
            int rectCount = LamiSystem.StrLstRcpConInspTitle_Uper.Count / ItemCount;
            _Uper_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();
            System.Drawing.Rectangle[] Rects = new System.Drawing.Rectangle[rectCount];
            System.Drawing.Rectangle tempRectOld = new System.Drawing.Rectangle(0, 0, 0, 0);
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < rectCount; i++)
            {
                tempRectNew.X = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 0]);
                tempRectNew.Y = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 1]);
                tempRectNew.Width = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 2]);
                tempRectNew.Height = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 3]);


                LamiSystem.RectListRecipeBoxZone_Uper.Add(tempRectNew);

                if (tempRectOld != tempRectNew)
                {
                    Rects[i] = LamiSystem.RectListRecipeBoxZone_Uper[i];
                    _Uper_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Uper[i]);
                    tempRectOld = tempRectNew;
                }
            }

            //for (int i = 0; i < rectCount; i++)
            //{
            Gc_Roi_Uper = RecipeGap_drawArea1.pictureBox1.CreateGraphics();
                //Gc_Roi_Uper = _Uper_Control_DrawArea._drawArea1.pictureBox1.CreateGraphics();
                //Gc_Roi_Uper.DrawRectangles(myPen, Rects);
            //}

        }
        */
        /*
        /// <summary>
        /// 기존에 저장되어져 있는 문자열 2차원 배열에서 데이터를 읽어와서
        /// 레시피 설정 그리드에 데이터를 표시한다.
        /// 레시피셋업그리드 사이즈 : 420, 170
        /// </summary>
        private void Recipe_Config_Inspect_Output_Uper()
        {
            int ItemCount = 14;
            //RegPathRcpConInsp_Uper
            int testItemCount = Vision_uGrd_Uper.Rows.Count;

            int rectCount = LamiSystem.StrLstRcpConInspTitle_Uper.Count / ItemCount;
            _Uper_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            System.Drawing.Rectangle tempRectOld = new System.Drawing.Rectangle(0, 0, 0, 0);
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < rectCount; i++)
            {
                tempRectNew.X = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 0]);
                tempRectNew.Y = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 1]);
                tempRectNew.Width = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 2]);
                tempRectNew.Height = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 3]);

                LamiSystem.RectListRecipeBoxZone_Uper.Add(tempRectNew);

                if (tempRectOld != tempRectNew)
                {
                    _Uper_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Uper[i]);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[i].Left);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[i].Right);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[i].Width);
                    Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[i].Height);
                    tempRectOld = tempRectNew;
                }
            }
        }
        */
        /// <summary>
        /// 기존에 저장되어져 있는 문자열 2차원 배열에서 데이터를 읽어와서
        /// 레시피 설정 그리드에 데이터를 표시한다.
        /// 레시피셋업그리드 사이즈 : 420, 170
        /// </summary>
        private void Recipe_Config_Inspect_Output_Down()
        {
            int ItemCount = 14;

            int rectCount = LamiSystem.StrLstRcpConInspTitle_Down.Count / ItemCount;
            _Down_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Down.Clear();

            System.Drawing.Rectangle tempRectOld = new System.Drawing.Rectangle(0, 0, 0, 0);
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < rectCount; i++)
            {
                tempRectNew.X = int.Parse(LamiSystem.StrLstRcpConInspData_Down[(i * ItemCount) + 0]);
                tempRectNew.Y = int.Parse(LamiSystem.StrLstRcpConInspData_Down[(i * ItemCount) + 1]);
                tempRectNew.Width = int.Parse(LamiSystem.StrLstRcpConInspData_Down[(i * ItemCount) + 2]);
                tempRectNew.Height = int.Parse(LamiSystem.StrLstRcpConInspData_Down[(i * ItemCount) + 3]);

                LamiSystem.RectListRecipeBoxZone_Down.Add(tempRectNew);

                if (tempRectOld != tempRectNew)
                {
                    _Down_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Down[i]);
                    tempRectOld = tempRectNew;
                }
            }
        }


        /// <summary>
        /// 데이터 소스에 알맞게 각각의 셀의 스타일과 데이터를 설정한다.
        /// 드롭다운 리스트의 내용(텍스트, 이미지)과 버튼의 유,무 등을 설정한다.
        /// </summary>
        private void Recipe_Uper_Config_DropDownList_Setup()
        {
            //클리어 하지 않으면 탭을 열때마다 생성된다.
            LamiSystem._vListItemName_Uper.ValueListItems.Clear();
            LamiSystem._vListMeasMethod.ValueListItems.Clear();
            LamiSystem._imgSideList.ValueListItems.Clear();
            LamiSystem._vListMeasDivid.ValueListItems.Clear();
            LamiSystem._vListMeasPola.ValueListItems.Clear();

            for (int i = 0; i < Vision_uGrd_Uper.Rows.Count; i++)
            {
                string TmpStr = Vision_uGrd_Uper.Rows[i].Cells[1].Value.ToString();
                LamiSystem._vListItemName_Uper.ValueListItems.Add(TmpStr);
            }
            

            LamiSystem._vListMeasMethod.ValueListItems.Add("1");
            LamiSystem._vListMeasMethod.ValueListItems.Add("2");
            LamiSystem._vListMeasMethod.ValueListItems.Add("3");
            LamiSystem._vListMeasMethod.ValueListItems.Add("4");
            LamiSystem._vListMeasMethod.ValueListItems.Add("5");
            LamiSystem._vListMeasMethod.ValueListItems.Add("6");
            LamiSystem._vListMeasMethod.ValueListItems.Add("7");
            LamiSystem._vListMeasMethod.ValueListItems.Add("8");
            LamiSystem._vListMeasMethod.ValueListItems.Add("9");
            LamiSystem._vListMeasMethod.ValueListItems.Add("10");

            LamiSystem._vListMeasPola.ValueListItems.Add("흑백");
            LamiSystem._vListMeasPola.ValueListItems.Add("백흑");

            LamiSystem._imgSideList.ValueListItems.Add(0, "0");
            LamiSystem._imgSideList.ValueListItems.Add(1, "1");
            LamiSystem._imgSideList.ValueListItems.Add(2, "2");
            LamiSystem._imgSideList.ValueListItems.Add(3, "3");
            LamiSystem._imgSideList.ValueListItems[0].Appearance.Image = global::SystemAlign.Properties.Resources._19;
            LamiSystem._imgSideList.ValueListItems[0].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[0].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.ValueListItems[1].Appearance.Image = global::SystemAlign.Properties.Resources._23;
            LamiSystem._imgSideList.ValueListItems[1].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[1].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.ValueListItems[2].Appearance.Image = global::SystemAlign.Properties.Resources._24;
            LamiSystem._imgSideList.ValueListItems[2].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[2].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.ValueListItems[3].Appearance.Image = global::SystemAlign.Properties.Resources._28;
            LamiSystem._imgSideList.ValueListItems[3].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[3].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.DisplayStyle = ValueListDisplayStyle.Picture;

            LamiSystem._vListMeasDivid.ValueListItems.Add("1");
            LamiSystem._vListMeasDivid.ValueListItems.Add("2");
            LamiSystem._vListMeasDivid.ValueListItems.Add("3");
            LamiSystem._vListMeasDivid.ValueListItems.Add("4");
            LamiSystem._vListMeasDivid.ValueListItems.Add("5");
            LamiSystem._vListMeasDivid.ValueListItems.Add("6");
            LamiSystem._vListMeasDivid.ValueListItems.Add("7");
            LamiSystem._vListMeasDivid.ValueListItems.Add("8");
            LamiSystem._vListMeasDivid.ValueListItems.Add("9");

            uGrd_Recipe_Test_Uper.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
        }

        private void Recipe_Down_Config_DropDownList_Setup()
        {
            //클리어 하지 않으면 탭을 열때마다 생성된다.
            LamiSystem._vListItemName_Down.ValueListItems.Clear();
            //LamiSystem._vListMeasMethod.ValueListItems.Clear();
            //LamiSystem._imgSideList.ValueListItems.Clear();
            //LamiSystem._vListMeasDivid.ValueListItems.Clear();

            for (int i = 0; i < Vision_uGrd_Down.Rows.Count; i++)
            {
                string TmpStr = Vision_uGrd_Down.Rows[i].Cells[1].Value.ToString();
                LamiSystem._vListItemName_Down.ValueListItems.Add(TmpStr);
            }

            /*
            LamiSystem._vListMeasMethod.ValueListItems.Add("1");
            LamiSystem._vListMeasMethod.ValueListItems.Add("2");
            LamiSystem._vListMeasMethod.ValueListItems.Add("3");
            LamiSystem._vListMeasMethod.ValueListItems.Add("4");
            LamiSystem._vListMeasMethod.ValueListItems.Add("5");
            LamiSystem._vListMeasMethod.ValueListItems.Add("6");
            LamiSystem._vListMeasMethod.ValueListItems.Add("7");
            LamiSystem._vListMeasMethod.ValueListItems.Add("8");
            LamiSystem._vListMeasMethod.ValueListItems.Add("9");
            LamiSystem._vListMeasMethod.ValueListItems.Add("10");
            LamiSystem._vListMeasMethod.ValueListItems.Add("11");
            LamiSystem._vListMeasMethod.ValueListItems.Add("12");

            LamiSystem._vListMeasPola.ValueListItems.Add("흑백");
            LamiSystem._vListMeasPola.ValueListItems.Add("백흑");

            LamiSystem._imgSideList.ValueListItems.Add(0, "0");
            LamiSystem._imgSideList.ValueListItems.Add(1, "1");
            LamiSystem._imgSideList.ValueListItems.Add(2, "2");
            LamiSystem._imgSideList.ValueListItems.Add(3, "3");
            LamiSystem._imgSideList.ValueListItems[0].Appearance.Image = global::SystemAlign.Properties.Resources._19;
            LamiSystem._imgSideList.ValueListItems[0].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[0].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.ValueListItems[1].Appearance.Image = global::SystemAlign.Properties.Resources._23;
            LamiSystem._imgSideList.ValueListItems[1].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[1].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.ValueListItems[2].Appearance.Image = global::SystemAlign.Properties.Resources._24;
            LamiSystem._imgSideList.ValueListItems[2].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[2].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.ValueListItems[3].Appearance.Image = global::SystemAlign.Properties.Resources._28;
            LamiSystem._imgSideList.ValueListItems[3].Appearance.ImageHAlign = HAlign.Center;
            LamiSystem._imgSideList.ValueListItems[3].Appearance.ImageVAlign = VAlign.Middle;
            LamiSystem._imgSideList.DisplayStyle = ValueListDisplayStyle.Picture;

            LamiSystem._vListMeasDivid.ValueListItems.Add("1");
            LamiSystem._vListMeasDivid.ValueListItems.Add("2");
            LamiSystem._vListMeasDivid.ValueListItems.Add("3");
            LamiSystem._vListMeasDivid.ValueListItems.Add("4");
            LamiSystem._vListMeasDivid.ValueListItems.Add("5");
            LamiSystem._vListMeasDivid.ValueListItems.Add("6");
            LamiSystem._vListMeasDivid.ValueListItems.Add("7");
            LamiSystem._vListMeasDivid.ValueListItems.Add("8");
            LamiSystem._vListMeasDivid.ValueListItems.Add("9");

            uGrd_Recipe_Test_Gap.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            */
        }

        /*
        private void RecipeBiCell_Config_Grid_Output()
        {
            for (int i = 0; i < uDS_Recipe_BiCell.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_RecipeBiCell_Data.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    RecipeBiCell_Config_Grid_Output_Style(i, j, strCellData);
                }
            }
        }

        private void RecipeBiCell_Config_Grid_Output_Style(int rowNo, int columnNo, string cellData)
        {
            switch (columnNo)
            {
                case 0:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 1:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasMethod_BiCell;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 2:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = "1차";
                    break;

                case 3:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._imgSideList_BiCell;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 4:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasPola_BiCell;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 5:
                    if (uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[1].Value.ToString() == "위치")
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = string.Empty;
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                    }
                    else
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = "2차";
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = ColumnStyle.Button;
                    }
                    break;
                case 6:
                    if (uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[1].Value.ToString() == "위치")
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = string.Empty;
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = ColumnStyle.Default;
                    }
                    else
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList =
                            LamiSystem._imgSideList_BiCell;
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    }
                    break;

                case 7:
                    if (uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[1].Value.ToString() == "위치")
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = string.Empty;
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = ColumnStyle.Default;
                    }
                    else
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList =
                            LamiSystem._vListMeasPola_BiCell;
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    }
                    break;

                case 8:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasDivid_BiCell;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 9:
                    if (cellData == "True")
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = true;
                    }
                    else
                    {
                        uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = false;
                    }
                    break;

                case 10:
                    uGrd_RecipeBiCell_Data.DisplayLayout.Bands[0].Columns[columnNo].PromptChar = ' ';
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = ColumnStyle.Integer;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].Activation = Activation.AllowEdit;
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[rowNo].Cells[columnNo].ActiveAppearance.ForeColor =
                        Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    break;
            }
        }
        */

        /// <summary>
        /// 기존에 저장되어져 있는 문자열 2차원 배열에서 데이터를 읽어와서
        /// 레시피 설정 그리드에 데이터를 표시한다.
        /// 레시피셋업그리드 사이즈 : 420, 170
        /// </summary>
        /*
        private void Recipe_Config_UperGrid_Output()
        {
            //LamiSystem.StrListVisConGridData_Uper[i];
            //uDS_Offset_Uper.Rows.Clear();
            int iItemCount = (LamiSystem.StrListVisConGridData_Uper.Count)/8;

            for (int i = 0; i < iItemCount; i++)
            {
                for (int j = 0; j < uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = string.Empty;
                    if (j == 0)
                    {
                        //uDS_Offset_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "", "", "" });
                        strCellData = LamiSystem.StrListVisConGridData_Uper[i*8];
                    }
                    else
                    {
                        strCellData = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    }
                    Recipe_Config_UperGrid_Output_Style(i, j, strCellData);
                }
            }
        }
        */
       
        private void Recipe_Config_UperGrid_Output()
        {
            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    Recipe_Config_UperGrid_Output_Style(i, j, strCellData);
                }
            }
        }
        

        private void Recipe_Config_UperGrid_Output_Style(int rowNo, int columnNo, string cellData)
        {
            switch (columnNo)
            {
                case 0:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListItemName_Uper;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 1:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasMethod;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 2:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = "1차";
                    break;

                case 3:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._imgSideList;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 4:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasPola;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 5:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = "2차";
                    break;

                case 6:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._imgSideList;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 7:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasPola;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 8:
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasDivid;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 9:
                    if (cellData == "True")
                    {
                        uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = true;
                    }
                    else
                    {
                        uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = false;
                    }
                    break;

                case 10:
                    uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[columnNo].PromptChar = ' ';
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = ColumnStyle.Integer;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].Activation = Activation.AllowEdit;
                    uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[columnNo].ActiveAppearance.ForeColor =
                        Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    break;
            }
        }

        private void Recipe_Config_DownGrid_Output()
        {
            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    Recipe_Config_DownGrid_Output_Style(i, j, strCellData);
                }
            }
        }

        /*
        private void Recipe_Config_DownGrid_Output()
        {
            int iItemCount = (LamiSystem.StrListVisConGridData_Down.Count) / 8;

            for (int i = 0; i < iItemCount; i++)
            {
                for (int j = 0; j < uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = string.Empty;
                    if (j == 0) strCellData = LamiSystem.StrListVisConGridData_Uper[i * 8];
                    else strCellData = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    string strCellData = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    Recipe_Config_DownGrid_Output_Style(i, j, strCellData);
                }
            }
        }
        */

        private void Recipe_Config_DownGrid_Output_Style(int rowNo, int columnNo, string cellData)
        {
            switch (columnNo)
            {
                case 0:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListItemName_Down;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 1:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasMethod;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 2:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = "1차";
                    break;

                case 3:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._imgSideList;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 4:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasPola;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 5:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = "2차";
                    break;

                case 6:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._imgSideList;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 7:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasPola;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 8:
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ValueList = LamiSystem._vListMeasDivid;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    break;

                case 9:
                    if (cellData == "True")
                    {
                        uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = true;
                    }
                    else
                    {
                        uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = false;
                    }
                    break;

                case 10:
                    uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[columnNo].PromptChar = ' ';
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Style = ColumnStyle.Integer;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Value = cellData;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].Activation = Activation.AllowEdit;
                    uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[columnNo].ActiveAppearance.ForeColor =
                        Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    break;
            }
        }


        #endregion


        #region 레시피 설정 탭 : 이벤트 핸들러

        private void Recipe_uBtn_ModelChange_Click(object sender, EventArgs e)
        {
            FormDlgModelReg dlgModelReg = new FormDlgModelReg();
            dlgModelReg.ModelAdding += new ModelRegEvent1(eventing_ModelAdding);
            //LamiSystem.GetSet_Now_Model_Name = Recipe_uTxt_ModelName.Text;
            //LamiSystem.GetSet_Now_Model_Number = Recipe_uTxt_ModelNumber.Text;

            dlgModelReg.GetSetModelName = LamiSystem.GetSet_Now_Model_Name;
            dlgModelReg.GetSetModelNumber = LamiSystem.GetSet_Now_Model_Number;

            if (dlgModelReg.ShowDialog() == DialogResult.OK)
            {
                LamiSystem.GetSet_Now_Model_Name = dlgModelReg.GetSetModelName;
                LamiSystem.GetSet_Now_Model_Number = dlgModelReg.GetSetModelNumber;
                Model_Change_Apply();
            }
        }

        /// <summary>
        /// 임시로 만들어 놓은 체크박스이다.
        /// 여기에서 드로우에어리어 클래스에서 사작형을 만들것인지 조정할것인지를 판다하게함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MakeModeFlaging();
        }

        public bool uDs_Uper_Recipe_Applying_Check()
        {
            for (int i = 0; i < uDS_Recipe_Uper.Rows.Count; i++)
            {
                string Grid_Name = uDS_Recipe_Uper.Rows[i].GetCellValue(0).ToString();
                if (Grid_Name == "")
                {
                    uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "설정한 레시피 중에서 " + (i + 1).ToString() + " 번째 항목의 이름을 선택하세요.<br/><br/>상부 레시피 항목을 다시 설정 하십시요!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    RecipeEditStatus_Desiable(0);
                    this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[0];
                    return false;
                }
            }

            for (int i = 0; i < uDS_Recipe_Uper.Rows.Count; i++)
            {
                string Grid_Name = uDS_Recipe_Uper.Rows[i].GetCellValue(1).ToString();

                if (Grid_Name == "0")
                {
                    uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "설정한 레시피 중에서 " + (i + 1).ToString() + " 번째 항목의 챠트을 선택하세요.<br/><br/>상부 레시피 항목을 다시 설정 하십시요!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    RecipeEditStatus_Desiable(0);
                    this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[0];
                    return false;
                }
            }

            return true;
        }

        public bool uDs_Down_Recipe_Applying_Check()
        {
            for (int i = 0; i < uDS_Recipe_Down.Rows.Count; i++)
            {
                string Grid_Name = uDS_Recipe_Down.Rows[i].GetCellValue(0).ToString();
                if (Grid_Name == "")
                {
                    uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "설정한 레시피 중에서 " + (i + 1).ToString() + " 번째 항목의 이름을 선택하세요.<br/><br/>하부 레시피 항목을 다시 설정 하십시요!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    RecipeEditStatus_Desiable(1);
                    this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[1];
                    return false;
                }
            }

            for (int i = 0; i < uDS_Recipe_Down.Rows.Count; i++)
            {
                string Grid_Name = uDS_Recipe_Down.Rows[i].GetCellValue(1).ToString();

                if (Grid_Name == "0")
                {
                    uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "설정한 레시피 중에서 " + (i + 1).ToString() + " 번째 항목의 챠트을 선택하세요.<br/><br/>하부 레시피 항목을 다시 설정 하십시요!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    RecipeEditStatus_Desiable(1);
                    this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[1];
                    return false;
                }
            }

            return true;
        }



        private void Recipe_uBtn_ParamApply_Click(object sender, EventArgs e)
        {
            
            //Control_UltraMessageBox messageBox = new Control_UltraMessageBox();

            //DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("RecipeMessage01Caption"), rm.GetString("RecipeMessage01Header"), rm.GetString("RecipeMessage01Content"),
            //    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (dlgResult == DialogResult.Cancel) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //레시피 설정값이 정상적인지를 확인하는 함수를 호출한다.
            if (Recipe_Grid_Value_Check() == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            //20150304 WKB 209
            RecipeDownStatus_Enable();

            //그림상자의 이미지와 설정한 구역의 배율을 계산하고 저장한다.
            RecipeGap_Config_Box_To_Image_Sum();
            
            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            RecipeGap_Config_Viewer_To_List_Data();
            
            //설정한 화면의 그리드 값들을 리스트 배열에 저장한다.
            Recipe_Config_Viewer_To_UperGrid();
            Recipe_Config_Viewer_To_DownGrid();
            
            //설정한 화면의 검출 영역을 배열에 저장한다.
            Recipe_Config_Viewer_To_List_Inspect_Uper();
            Recipe_Config_Viewer_To_List_Inspect_Down();

            //20150309 WKB 209
            Recipe_Config_Inspect_Output_Uper();
            Recipe_Config_Inspect_Output_Down();
            Recipe_Uper_Insert_Inspect();
            Recipe_Down_Insert_Inspect();

            ubtnToolbarSave.PerformClick();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private bool Recipe_Grid_Value_Check()
        {
            //상부 항목 추가시 항목이 공백, 챠트가 0번인지 확인한다.
            if (uDs_Uper_Recipe_Applying_Check() == false)
            {
                return false;
            }

            //하부 항목 추가시 항목이 공백, 챠트가 0번인지 확인한다.
            if (uDs_Down_Recipe_Applying_Check() == false)
            {
                return false;
            }

            //아이템이 중복으로 선택되어 있는지 확인한다.
            if (uGrd_Recipe_Applying_ItemName_Check() == false)
            {
                return false;
            }

            //챠트번호가 중복으로 선택되어 있는지 확인한다.
            if (uGrd_Recipe_Applying_GraphNum_Check() == false)
            {
                return false;
            }

            return true;
        }
        private void Recipe_Config_Register_To_Lists_Inspect_Uper_Temp()
        {
            Recipe_Config_Register_To_List_Inspect_Uper();
            LamiSystem.StrLstRcpConInspData_Uper.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_Uper.Count; i++)
            {
                string strRegisterGridData = Recipe_Config_Register_To_List_Inspect_Uper(LamiSystem.RegPathRcpConInsp_Uper, LamiSystem.StrLstRcpConInspTitle_Uper[i]);
                LamiSystem.StrLstRcpConInspData_Uper.Add(strRegisterGridData);
            }
        }

        private void Recipe_Config_Register_To_Lists_Inspect_Down_Temp()
        {
            Recipe_Config_Register_To_List_Inspect_Down();
            LamiSystem.StrLstRcpConInspData_Down.Clear();

            for (int i = 0; i < LamiSystem.StrLstRcpConInspTitle_Down.Count; i++)
            {
                string strRegisterGridData = Recipe_Config_Register_To_List_Inspect_Down(LamiSystem.RegPathRcpConInsp_Down, LamiSystem.StrLstRcpConInspTitle_Down[i]);
                LamiSystem.StrLstRcpConInspData_Down.Add(strRegisterGridData);
            }
        }

        public bool uGrd_Recipe_Applying_GraphNum_Check()
        {
            //Recipe_CheckedGraph_Result_Uper = true;
            //Recipe_CheckedGraph_Result_Down = true;
            int GraphCount = 0;

            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                GraphCount = 0;
                string Grid_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[1].Value.ToString();
                for (int j = 0; j < uGrd_Recipe_UperData.Rows.Count; j++)
                {
                    if (i == j) continue;

                    string Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[j].Cells[1].Value.ToString();
                    if (Select_Name == Grid_Name)
                    {
                        GraphCount = GraphCount + 1;

                        if (GraphCount >= 2)
                        {
                            uMessageBox.MessageBox_Show("레시피 설정", "그래프 설정", "설정한 " + Select_Name + " 번 그래프는 2회이상 중복으로 설정했습니다.<br/><br/>상부 레시피 그래프를 다시 설정 하여 주십시요!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //Recipe_CheckedGraph_Result_Uper = false;
                            return false;
                        }
                    }
                }
            }

            
            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                GraphCount = 0;
                string Grid_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[1].Value.ToString();
                for (int j = 0; j < uGrd_Recipe_DownData.Rows.Count; j++)
                {
                    if (i == j) continue;

                    string Select_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[j].Cells[1].Value.ToString();
                    if (Select_Name == Grid_Name)
                    {
                        GraphCount = GraphCount + 1;

                        if (GraphCount >= 2)
                        {
                            uMessageBox.MessageBox_Show("레시피 설정", "그래프 설정", "설정한 " + Select_Name + " 번 그래프는 2회이상 중복으로 설정했습니다.<br/><br/>하부 레시피 그래프를 다시 설정하여 주십시요!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //Recipe_CheckedGraph_Result_Down = false;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool Recipe_CheckedName_Result_Uper = true;
        private bool Recipe_CheckedName_Result_Down = true;
        private bool Recipe_CheckedGraph_Result_Uper = true;
        private bool Recipe_CheckedGraph_Result_Down = true;

        public bool uGrd_Recipe_Applying_ItemName_Check()
        {
            //Recipe_CheckedName_Result_Uper = true;
            //Recipe_CheckedName_Result_Down = true;

            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                string Grid_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[0].Value.ToString();
                for (int j = 0; j < uGrd_Recipe_UperData.Rows.Count; j++)
                {
                    if (i == j) continue;

                    string Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[j].Cells[0].Value.ToString();
                    if (Select_Name == Grid_Name)
                    {
                        uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "설정한 레시피 중에서 " + Select_Name + " 항목을 중복으로 설정했습니다.<br/><br/>상부 레시피 항목을 다시 설정 하십시요!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Recipe_CheckedName_Result_Uper = false;
                        return false;
                    }
                }
            }

            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                string Grid_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[0].Value.ToString();
                for (int j = 0; j < uGrd_Recipe_DownData.Rows.Count; j++)
                {
                    if (i == j) continue;

                    string Select_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[j].Cells[0].Value.ToString();
                    if (Select_Name == Grid_Name)
                    {
                        uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "설정한 레시피 중에서 " + Select_Name + " 항목을 중복으로 설정했습니다.<br/><br/>하부 레시피 항목을 다시 설정 하십시요!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Recipe_CheckedName_Result_Down = false;
                        return false;
                    }
                }
            }

            return true;
        }
/*
        //2015.02.07 WKB 208
        private void Recipe_Config_Viewer_To_List_Inspect_Uper()
        {
            LamiSystem.StrLstRcpConInspData_Uper.Clear();
            LamiSystem.StrLstRcpConInspTitle_Uper.Clear();

            DrawList zoneList = _Uper_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Uper.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            //for (int i = 0; i < uDS_Inspect_Uper.Rows.Count; i++)
            {
                int ROI_No = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                //int ROI_No = int.Parse(uDS_Inspect_Uper.Rows[i].GetCellValue(5).ToString());

                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Height.ToString());      //3
                    else if (j == 4)
                    {
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(uDS_Inspect_Uper.Rows[i].GetCellValue(j).ToString());
                    }
                    else if (j == 5)
                    {
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(uDS_Inspect_Uper.Rows[i].GetCellValue(j).ToString());
                    }
                    else if (j == 6)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[1].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(1).ToString();
                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);

                    }
                    else if (j == 7)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[2].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(2).ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[5].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(5).ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        
                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);
                    }
                    else if (j == 8)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[3].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(3).ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[6].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(6).ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);
                    }
                    else if (j == 9)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[4].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(4).ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[7].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(7).ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);
                    }
                    else if (j == 10)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[8].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(8).ToString();
                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);
                    }
                    else if (j == 11)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[9].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(9).ToString();
                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);
                    }
                    else if (j == 12)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[10].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Uper.Rows[i / 2].GetCellValue(10).ToString();
                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(tmpSeqNo);
                    }
                    else if (j == 13)
                    {
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = i.ToString("0");
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //uDS_Inspect_Uper.Rows[i].SetCellValue(j, i.ToString("0"));
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(i.ToString("0"));
                    }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Uper.Count * ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Uper.Add(i.ToString("000"));
            }
        }
        */
        //2015.03.09 WKB 209
        private void Recipe_Config_Viewer_To_List_Inspect_Uper()
        {
            LamiSystem.StrLstRcpConInspData_Uper.Clear();
            LamiSystem.StrLstRcpConInspTitle_Uper.Clear();

            DrawList zoneList = _Uper_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Uper.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                /*
                int ROI_No = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Height.ToString()); //3
                */

                //int ROI_No = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].Height.ToString()); //3

                    else if (j == 4)
                    {
                        //uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = (i/2).ToString("0");

                        //20150307 WKB 209
                        LamiSystem.StrLstRcpConInspData_Uper.Add((i / 2).ToString("0"));

                        //20150307 WKB 208
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 5)
                    {
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 6)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[1].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 7)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[2].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[5].Value.ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 8)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[3].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[6].Value.ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 9)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[4].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[7].Value.ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 10)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[8].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 11)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[9].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 12)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[10].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 13)
                    {
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = i.ToString("0");
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    //                     else
                    //                     {
                    //                         LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    //                     }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Uper.Count * ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Uper.Add(i.ToString("000"));
            }
        }
        /*
        //2015.02.07 WKB 207
          private void Recipe_Config_Viewer_To_List_Inspect_Uper()
        {
            LamiSystem.StrLstRcpConInspData_Uper.Clear();
            LamiSystem.StrLstRcpConInspTitle_Uper.Clear();

            DrawList zoneList = _Uper_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Uper.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                int ROI_No = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                for (int j = 0; j < ItemCount; j++)
                {
                    if(j ==0)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].X.ToString());        //0
                    else if(j==1)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Y.ToString());        //1
                    else if(j==2)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Height.ToString()); //3
                    else if (j == 4)
                    {
                        //uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = (i/2).ToString("0");

                        //20150307 WKB 209
                        LamiSystem.StrLstRcpConInspData_Uper.Add((i / 2).ToString("0"));

                        //20150307 WKB 208
                        //LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 5)
                    {
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 6)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[1].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 7)
                    {
                        string tmpSeqNo = string.Empty;
                        if(i%2 ==0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[2].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[5].Value.ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 8)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[3].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[6].Value.ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 9)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[4].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[7].Value.ToString();

                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 10)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[8].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 11)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[9].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 12)
                    {
                        string tmpSeqNo = uGrd_Recipe_UperData.DisplayLayout.Rows[i/2].Cells[10].Value.ToString();
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 13)
                    {
                        uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value = i.ToString("0");
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
//                     else
//                     {
//                         LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
//                     }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Uper.Count*ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Uper.Add(i.ToString("000"));
            }
        }
        */

        /*
        private void Recipe_Config_Viewer_To_List_Inspect_Uper()
        {
            LamiSystem.StrLstRcpConInspData_Uper.Clear();
            LamiSystem.StrLstRcpConInspTitle_Uper.Clear();

            DrawList zoneList = _Uper_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Uper.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                int ROI_No = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[ROI_No].Height.ToString()); //3
                    else
                    {
                        LamiSystem.StrLstRcpConInspData_Uper.Add(uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Uper.Count * ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Uper.Add(i.ToString("000"));
            }
        }
        */

        private void Recipe_Down_Insert_Inspect()
        {
            int tmpData = uGrd_Recipe_Inspect_Down.Rows.Count;

            List<string> RoiArray = new List<string>();

            for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
            {
                string strTmpData = uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[5].Value.ToString();
                RoiArray.Add(strTmpData);
            }
            uDS_Inspect_Down.Rows.Clear();

            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Down.Count; i++)
            {
                //데이터 : X 좌표
                string liststring1 = LamiSystem.RectListRecipeBoxZone_Down[i].X.ToString();
                //데이터 : Y 좌표
                string liststring2 = LamiSystem.RectListRecipeBoxZone_Down[i].Y.ToString();
                //데이터 : 넓이
                string liststring3 = LamiSystem.RectListRecipeBoxZone_Down[i].Width.ToString();
                //데이터 : 높이
                string liststring4 = LamiSystem.RectListRecipeBoxZone_Down[i].Height.ToString();
                //데이터 : Row 번호
                string liststring5 = (i / 2).ToString("0");
                
                //20150306 WKB 209
                string liststring6 = string.Empty;
                if (tmpData == 0)
                    liststring6 = LamiSystem.StrLstRcpConInspData_Down[5 + (i * 14)];
                else if (i < tmpData)
                    liststring6 = LamiSystem.StrLstRcpConInspData_Down[5 + (i * 14)];
                else
                    liststring6 = i.ToString("0");

                //데이터 : 그래프번호
                string liststring7 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 1];
                //데이터 : 시퀀스 번호
                string liststring8 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 2 + (i % 2 * 3)];
                //데이터 : 방향 번혼
                string liststring9 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 3 + (i % 2 * 3)];
                //데이터 : 극성 
                string liststring10 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 4 + (i % 2 * 3)];
                //데이터 : 분할 수량
                string liststring11 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 8];
                //데이터 : 표시 여부
                string liststring12 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 9];
                //데이터 : 밝기 수량
                string liststring13 = LamiSystem.StrLstRcpConGridData_Down[(i / 2 * 11) + 10];
                //데이터 : 리스트 번호
                string liststring14 = i.ToString("0");

                uDS_Inspect_Down.Rows.Add(true, new Object[] { liststring1, liststring2, liststring3, liststring4, liststring5, 
                    liststring6, liststring7, liststring8,liststring9,liststring10,liststring11,liststring12,liststring13,liststring14});
            }
        }

        private void Recipe_Uper_Insert_Inspect()
        {
            int tmpData = uGrd_Recipe_Inspect_Uper.Rows.Count;

            List<string> RoiArray = new List<string>();

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                string strTmpData = uGrd_Recipe_Inspect_Uper.DisplayLayout.Rows[i].Cells[5].Value.ToString();
                RoiArray.Add(strTmpData);   
            }
            uDS_Inspect_Uper.Rows.Clear();

           
            
            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Uper.Count; i++)
            {
                //데이터 : X 좌표
                string liststring1 = LamiSystem.RectListRecipeBoxZone_Uper[i].X.ToString();
                //데이터 : Y 좌표
                string liststring2 = LamiSystem.RectListRecipeBoxZone_Uper[i].Y.ToString();
                //데이터 : 넓이
                string liststring3 = LamiSystem.RectListRecipeBoxZone_Uper[i].Width.ToString();
                //데이터 : 높이
                string liststring4 = LamiSystem.RectListRecipeBoxZone_Uper[i].Height.ToString();
                
                //데이터 : Row 번호
                string liststring5 = (i / 2).ToString("0");


                
                
                //데이터 : ROI 번호
                string liststring6 = string.Empty;
                if (tmpData == 0)
                    liststring6 = LamiSystem.StrLstRcpConInspData_Uper[5 + (i * 14)];
                else if (i < tmpData) 
                    //liststring6 = RoiArray[i];
                    liststring6 = LamiSystem.StrLstRcpConInspData_Uper[5 + (i * 14)];
                else 
                    liststring6 = i.ToString("0");

                //데이터 : 그래프번호
                string liststring7 = LamiSystem.StrLstRcpConGridData_Uper[(i/2 * 11) + 1];
                //데이터 : 시퀀스 번호
                string liststring8 = LamiSystem.StrLstRcpConGridData_Uper[(i/2 * 11) + 2 + (i%2*3)];
                //데이터 : 방향 번혼
                string liststring9 = LamiSystem.StrLstRcpConGridData_Uper[(i / 2 * 11) + 3 + (i % 2 * 3)];
                //데이터 : 극성 
                string liststring10 = LamiSystem.StrLstRcpConGridData_Uper[(i / 2 * 11) + 4 + (i % 2 * 3)];
                //데이터 : 분할 수량
                string liststring11 = LamiSystem.StrLstRcpConGridData_Uper[(i / 2 * 11) + 8];
                //데이터 : 표시 여부
                string liststring12 = LamiSystem.StrLstRcpConGridData_Uper[(i / 2 * 11) + 9];
                //데이터 : 밝기 수량
                string liststring13 = LamiSystem.StrLstRcpConGridData_Uper[(i / 2 * 11) + 10];
                //데이터 : 리스트 번호
                string liststring14 = i.ToString("0");

                uDS_Inspect_Uper.Rows.Add(true, new Object[] { liststring1, liststring2, liststring3, liststring4, liststring5, 
                    liststring6, liststring7, liststring8,liststring9,liststring10,liststring11,liststring12,liststring13,liststring14});
            }
        }

        /*
        private void Recipe_Uper_Insert_Inspect()
        {
            int ItemCount = 14;
            int rectCount = LamiSystem.StrLstRcpConInspTitle_Uper.Count / ItemCount;
            _Uper_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            System.Drawing.Rectangle tempRectOld = new System.Drawing.Rectangle(0, 0, 0, 0);
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < rectCount; i++)
            {
                tempRectNew.X = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 0]);
                tempRectNew.Y = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 1]);
                tempRectNew.Width = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 2]);
                tempRectNew.Height = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[(i * ItemCount) + 3]);

                LamiSystem.RectListRecipeBoxZone_Uper.Add(tempRectNew);
                _Uper_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Uper[i]);
            }

            uDS_Inspect_Uper.Rows.Clear();
            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Uper.Count; i++)
            {
                //데이터 : X 좌표
                string liststring1 = LamiSystem.RectListRecipeBoxZone_Uper[i].X.ToString();
                //데이터 : Y 좌표
                string liststring2 = LamiSystem.RectListRecipeBoxZone_Uper[i].Y.ToString();
                //데이터 : 넓이
                string liststring3 = LamiSystem.RectListRecipeBoxZone_Uper[i].Width.ToString();
                //데이터 : 높이
                string liststring4 = LamiSystem.RectListRecipeBoxZone_Uper[i].Height.ToString();
                //데이터 : Row 번호
                string liststring5 = (i / 2).ToString("0");
                //데이터 : ROI 번호
                string liststring6 = i.ToString("0");
                //데이터 : 그래프번호
                string liststring7 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[1].Value.ToString();
                //데이터 : 시퀀스 번호
                string liststring8 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[((i % 2) * 3) + 2].Value.ToString();
                //데이터 : 방향 번혼
                string liststring9 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[((i % 2) * 3) + 3].Value.ToString();
                //데이터 : 극성 
                string liststring10 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[((i % 2) * 3) + 4].Value.ToString();
                //데이터 : 분할 수량
                string liststring11 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[8].Value.ToString();
                //데이터 : 표시 여부
                string liststring12 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[9].Value.ToString();
                //데이터 : 밝기 수량
                string liststring13 = uGrd_Recipe_UperData.DisplayLayout.Rows[i / 2].Cells[10].Value.ToString();
                //데이터 : 리스트 번호
                string liststring14 = i.ToString("0");

                uDS_Inspect_Uper.Rows.Add(true, new Object[] { liststring1, liststring2, liststring3, liststring4, liststring5, 
                    liststring6, liststring7, liststring8,liststring9,liststring10,liststring11,liststring12,liststring13,liststring14});
            }
        }
        */

        

        /*
        private void Recipe_Config_Viewer_To_List_Inspect_Uper()
        {
            int ItemCount = 14;
            List<string> tempList = new List<string>();
            int loopCount = LamiSystem.RectListRecipeBoxZone_Uper.Count;
            for (int i = 0; i < loopCount; i++)
            {
                tempList.Add(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * ItemCount)]);    //0 : Row
                tempList.Add(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * ItemCount)]);    //1 : ROI
                tempList.Add(LamiSystem.StrLstRcpConInspData_Uper[7 + (i * ItemCount)]);    //2 : SeqNo
                tempList.Add(LamiSystem.StrLstRcpConInspData_Uper[13 + (i * ItemCount)]);   //3 : LstNo

            }
           
            //레시피 데이터 인스펙트 진행
            //기존의 데이터를 보존해야 하는 부분과 새로 만들어야 하는 부분이 있다.
            //아래의 수량이 먼저 문제가 된다.
            //ROI박스와 레시피 박스를 분리해야한다.
            DrawList zoneList = _Uper_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Uper.Clear();

            int tmpItemCount = 4;
            for (int i = 0; i < loopCount; i++)
            {
                int RoiNo = int.Parse(tempList[1 + (i * tmpItemCount)]);
                LamiSystem.RectListRecipeBoxZone_Uper.Add(zoneList[zoneList.Count -1 -RoiNo].GetSetRectangle());
            }

            LamiSystem.StrLstRcpConInspData_Uper.Clear();
            LamiSystem.StrLstRcpConInspTitle_Uper.Clear();
            int inspectDataCount = 0;
            string liststring = "";
            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Uper.Count; i++)
            {
                int tmpRowNo = int.Parse(tempList[i * tmpItemCount + 0]);
                int tmpSeqNo = int.Parse(tempList[i * tmpItemCount + 2]) * 3;

                string liststring1 = LamiSystem.RectListRecipeBoxZone_Uper[i].X.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].X.ToString());        //0
                string liststring2 = LamiSystem.RectListRecipeBoxZone_Uper[i].Y.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].Y.ToString());        //1
                string liststring3 = LamiSystem.RectListRecipeBoxZone_Uper[i].Width.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].Width.ToString());    //2
                string liststring4 = LamiSystem.RectListRecipeBoxZone_Uper[i].Height.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(LamiSystem.RectListRecipeBoxZone_Uper[i].Height.ToString());   //3

                string liststring5 = tempList[i*tmpItemCount + 0];
                LamiSystem.StrLstRcpConInspData_Uper.Add(tempList[i * tmpItemCount + 0]);                           //4 : Grid Row
                string liststring6 = tempList[i * tmpItemCount + 1];
                LamiSystem.StrLstRcpConInspData_Uper.Add(tempList[i * tmpItemCount + 1]);                           //5 : ROI No
                
                string liststring7 = uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[1].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(
                    uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[1].Value.ToString());                        //6 : Type

                string liststring8 = tempList[i * tmpItemCount + 2];
                LamiSystem.StrLstRcpConInspData_Uper.Add(tempList[i * tmpItemCount + 2]);                           //7 : Seq No

                string liststring9 = uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 3].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(
                uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 3].Value.ToString());             //8 : 방향

                string liststring10 = uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 4].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(
                uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 4].Value.ToString());             //9 : 극성
                
                string liststring11 = uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[8].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(
                uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[8].Value.ToString());             //10 : 분할
                
                string liststring12 = uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[9].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(
                uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[9].Value.ToString());             //11 : 표시

                string liststring13 = uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[10].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Uper.Add(
                uGrd_Recipe_UperData.DisplayLayout.Rows[tmpRowNo].Cells[10].Value.ToString());             //12 : 밝기

                string liststring14 = tempList[i * tmpItemCount + 3];
                LamiSystem.StrLstRcpConInspData_Uper.Add(tempList[i * tmpItemCount + 3]);                           //13 : List No
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Uper.Count*ItemCount;
            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Uper.Add(i.ToString("000"));
            }
        }
        */


        //2015.02.07 WKB 208
        /*
        private void Recipe_Config_Viewer_To_List_Inspect_Down()
        {
            LamiSystem.StrLstRcpConInspData_Down.Clear();
            LamiSystem.StrLstRcpConInspTitle_Down.Clear();

            DrawList zoneList = _Down_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Down.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Down.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
            //for (int i = 0; i < uDS_Inspect_Down.Rows.Count; i++)
            {
                int ROI_No = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[5].Value.ToString());
                //int ROI_No = int.Parse(uDS_Inspect_Down.Rows[i].GetCellValue(5).ToString());
                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].Height.ToString());   //3
                    else if (j == 4)
                    {
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        //string tmpstr = uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString());
                    }
                    else if (j == 5)
                    {
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        
                        //string tmpstr = uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                        //string tmpstr11 = uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString());
                    }


                    else if (j == 6)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[1].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(1).ToString();
                        //uDS_Inspect_Down.Rows[i].SetCellValue(j,tmpSeqNo);
                        //string tmpstr = uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 7)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[2].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(2).ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[5].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(5).ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 8)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[3].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(3).ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[6].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(6).ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 9)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[4].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(4).ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[7].Value.ToString();
                            //tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(7).ToString();
                        
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, tmpSeqNo);
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 10)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[8].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(8).ToString();
                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, tmpSeqNo);
                        //string tmpstr = uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 11)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[9].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(9).ToString();
                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, tmpSeqNo);
                        //string tmpstr = uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 12)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[10].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //string tmpSeqNo = uDS_Recipe_Down.Rows[i / 2].GetCellValue(10).ToString();
                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, tmpSeqNo);
                        //string tmpstr = uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(tmpSeqNo);
                    }
                    else if (j == 13)
                    {
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = i.ToString("0");
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());

                        //uDS_Inspect_Down.Rows[i].SetCellValue(j, i.ToString("0"));
                        //string tmpstr = uDS_Inspect_Down.Rows[i].GetCellValue(j).ToString();
                        //LamiSystem.StrLstRcpConInspData_Down.Add(i.ToString("0"));
                    }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Down.Count * ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Down.Add(i.ToString("000"));
            }
        }
        */
        //2015.03.09 WKB 209
        private void Recipe_Config_Viewer_To_List_Inspect_Down()
        {
            LamiSystem.StrLstRcpConInspData_Down.Clear();
            LamiSystem.StrLstRcpConInspTitle_Down.Clear();

            DrawList zoneList = _Down_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Down.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Down.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
            {
                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].Height.ToString());   //3
                    else if (j == 4)
                    {
                        LamiSystem.StrLstRcpConInspData_Down.Add((i / 2).ToString("0"));
                    }
                    else if (j == 5)
                    {
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }

                    else if (j == 6)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[1].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 7)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[2].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[5].Value.ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 8)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[3].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[6].Value.ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 9)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[4].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[7].Value.ToString();
                        
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 10)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[8].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 11)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[9].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 12)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[10].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 13)
                    {
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = i.ToString("0");
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Down.Count * ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Down.Add(i.ToString("000"));
            }
        }

        /*
        //2015.02.07 WKB 207
        private void Recipe_Config_Viewer_To_List_Inspect_Down()
        {
            LamiSystem.StrLstRcpConInspData_Down.Clear();
            LamiSystem.StrLstRcpConInspTitle_Down.Clear();

            DrawList zoneList = _Down_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Down.Clear();

            for (int i = 0; i < zoneList.Count; i++)
            {
                LamiSystem.RectListRecipeBoxZone_Down.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
            }

            int ItemCount = 14;

            for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
            {
                int ROI_No = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[5].Value.ToString());
                for (int j = 0; j < ItemCount; j++)
                {
                    if (j == 0)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].X.ToString());        //0
                    else if (j == 1)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].Y.ToString());        //1
                    else if (j == 2)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].Width.ToString());    //2
                    else if (j == 3)
                        LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[ROI_No].Height.ToString());   //3
                    else if (j == 4)
                    {
                        //uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[4].Value = (i / 2).ToString("0");
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 5)
                    {
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 6)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[1].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 7)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[2].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[5].Value.ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 8)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[3].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[6].Value.ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 9)
                    {
                        string tmpSeqNo = string.Empty;
                        if (i % 2 == 0)
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[4].Value.ToString();
                        else
                            tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[7].Value.ToString();

                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 10)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[8].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 11)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[9].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 12)
                    {
                        string tmpSeqNo = uGrd_Recipe_DownData.DisplayLayout.Rows[i / 2].Cells[10].Value.ToString();
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = tmpSeqNo;
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                    else if (j == 13)
                    {
                        uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value = i.ToString("0");
                        LamiSystem.StrLstRcpConInspData_Down.Add(uGrd_Recipe_Inspect_Down.DisplayLayout.Rows[i].Cells[j].Value.ToString());
                    }
                }
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Down.Count * ItemCount;

            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Down.Add(i.ToString("000"));
            }
        }
        */
        /*
         private void Recipe_Config_Viewer_To_List_Inspect_Down()
        {          

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Uper.Count * ItemCount;
            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Uper.Add(i.ToString("000"));
            }

            int ItemCount = 14;
            List<string> tempList = new List<string>();
            int loopCount = LamiSystem.RectListRecipeBoxZone_Down.Count;
            for (int i = 0; i < loopCount; i++)
            {
                tempList.Add(LamiSystem.StrLstRcpConInspData_Down[4 + (i * ItemCount)]);    //0 : Row
                tempList.Add(LamiSystem.StrLstRcpConInspData_Down[5 + (i * ItemCount)]);    //1 : ROI
                tempList.Add(LamiSystem.StrLstRcpConInspData_Down[7 + (i * ItemCount)]);    //2 : SeqNo
                tempList.Add(LamiSystem.StrLstRcpConInspData_Down[13 + (i * ItemCount)]);   //3 : LstNo
            }

            //레시피 데이터 인스펙트 진행
            //기존의 데이터를 보존해야 하는 부분과 새로 만들어야 하는 부분이 있다.
            //아래의 수량이 먼저 문제가 된다.
            //ROI박스와 레시피 박스를 분리해야한다.
            DrawList zoneList = _Down_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_Down.Clear();

            int tmpItemCount = 4;
            for (int i = 0; i < loopCount; i++)
            {
                int RoiNo = int.Parse(tempList[1 + (i * tmpItemCount)]);
                LamiSystem.RectListRecipeBoxZone_Down.Add(zoneList[zoneList.Count - 1 - RoiNo].GetSetRectangle());
            }

            LamiSystem.StrLstRcpConInspData_Down.Clear();
            LamiSystem.StrLstRcpConInspTitle_Down.Clear();
            int inspectDataCount = 0;
            string liststring = "";
            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Down.Count; i++)
            {
                int tmpRowNo = int.Parse(tempList[i * tmpItemCount + 0]);
                int tmpSeqNo = int.Parse(tempList[i * tmpItemCount + 2]) * 3;

                string liststring1 = LamiSystem.RectListRecipeBoxZone_Down[i].X.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].X.ToString());        //0
                string liststring2 = LamiSystem.RectListRecipeBoxZone_Down[i].Y.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].Y.ToString());        //1
                string liststring3 = LamiSystem.RectListRecipeBoxZone_Down[i].Width.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].Width.ToString());    //2
                string liststring4 = LamiSystem.RectListRecipeBoxZone_Down[i].Height.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(LamiSystem.RectListRecipeBoxZone_Down[i].Height.ToString());   //3

                string liststring5 = tempList[i * tmpItemCount + 0];
                LamiSystem.StrLstRcpConInspData_Down.Add(tempList[i * tmpItemCount + 0]);                           //4 : Grid Row
                string liststring6 = tempList[i * tmpItemCount + 1];
                LamiSystem.StrLstRcpConInspData_Down.Add(tempList[i * tmpItemCount + 1]);                           //5 : ROI No

                string liststring7 = uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[1].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(
                    uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[1].Value.ToString());                        //6 : Type

                string liststring8 = tempList[i * tmpItemCount + 2];
                LamiSystem.StrLstRcpConInspData_Down.Add(tempList[i * tmpItemCount + 2]);                           //7 : Seq No

                string liststring9 = uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 3].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(
                uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 3].Value.ToString());             //8 : 방향

                string liststring10 = uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 4].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(
                uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 4].Value.ToString());             //9 : 극성

                string liststring11 = uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[8].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(
                uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[8].Value.ToString());             //10 : 분할

                string liststring12 = uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[9].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(
                uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[9].Value.ToString());             //11 : 표시

                string liststring13 = uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[10].Value.ToString();
                LamiSystem.StrLstRcpConInspData_Down.Add(
                uGrd_Recipe_DownData.DisplayLayout.Rows[tmpRowNo].Cells[10].Value.ToString());             //12 : 밝기

                string liststring14 = tempList[i * tmpItemCount + 3];
                LamiSystem.StrLstRcpConInspData_Down.Add(tempList[i * tmpItemCount + 3]);                           //13 : List No
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_Down.Count * ItemCount;
            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_Down.Add(i.ToString("000"));
            }
        }
        */
        /*
        private void RecipeBiCell_Config_Viewer_To_List_Inspect()
        {
            int ItemCount = 14;
            List<string> tempList = new List<string>();
            int loopCount = LamiSystem.RectListRecipeBoxZone_BiCell.Count;
            for (int i = 0; i < loopCount; i++)
            {
                tempList.Add(LamiSystem.StrLstRcpConInspData_BiCell[4 + (i * ItemCount)]);    //0 : Row
                tempList.Add(LamiSystem.StrLstRcpConInspData_BiCell[5 + (i * ItemCount)]);    //1 : ROI
                tempList.Add(LamiSystem.StrLstRcpConInspData_BiCell[7 + (i * ItemCount)]);    //2 : SeqNo
                tempList.Add(LamiSystem.StrLstRcpConInspData_BiCell[13 + (i * ItemCount)]);   //3 : LstNo

            }
            
            //string strCellData = uGrd_Recipe_Data.DisplayLayout.Rows[i].Cells[j].Value.ToString();
            
//             tempRectNew.Width =    int.Parse(AlignSystem.StrLstRcpConInspData[(i*ItemCount) + 2]);
//                 tempRectNew.Height =   int.Parse(AlignSystem.StrLstRcpConInspData[(i*ItemCount) + 3]);
// 
//                 AlignSystem.RectListRecipeBoxZone.Add(tempRectNew);
//                 if (tempRectOld != tempRectNew)
//                 {
//                     _Cls_Control_DrawArea.AddListObject(AlignSystem.RectListRecipeBoxZone_Gap[i]);
//                     tempRectOld = tempRectNew;
//                 }
             

            //레시피 데이터 인스펙트 진행
            //기존의 데이터를 보존해야 하는 부분과 새로 만들어야 하는 부분이 있다.
            //아래의 수량이 먼저 문제가 된다.
            //ROI박스와 레시피 박스를 분리해야한다.
            DrawList zoneList = _BiCell_Control_DrawArea.GetSetSelectZone;
            LamiSystem.RectListRecipeBoxZone_BiCell.Clear();

            int tmpItemCount = 4;
            for (int i = 0; i < loopCount; i++)
            {
                int RoiNo = int.Parse(tempList[1 + (i * tmpItemCount)]);
                //AlignSystem.RectListRecipeBoxZone.Add(zoneList[zoneList.Count-1-i].NowRectangle());
                //AlignSystem.RectListRecipeBoxZone.Add(zoneList[zoneList.Count - 1 - i].GetSetRectangle());
                LamiSystem.RectListRecipeBoxZone_BiCell.Add(zoneList[zoneList.Count - 1 - RoiNo].GetSetRectangle());

            }

            LamiSystem.StrLstRcpConInspData_BiCell.Clear();
            LamiSystem.StrLstRcpConInspTitle_BiCell.Clear();
            int inspectDataCount = 0;
            string liststring = "";
            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_BiCell.Count; i++)
            {
                int tmpRowNo = int.Parse(tempList[i * tmpItemCount + 0]);
                int tmpSeqNo = int.Parse(tempList[i * tmpItemCount + 2]) * 3;

                string liststring1 = LamiSystem.RectListRecipeBoxZone_BiCell[i].X.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(LamiSystem.RectListRecipeBoxZone_BiCell[i].X.ToString());        //0
                string liststring2 = LamiSystem.RectListRecipeBoxZone_BiCell[i].Y.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(LamiSystem.RectListRecipeBoxZone_BiCell[i].Y.ToString());        //1
                string liststring3 = LamiSystem.RectListRecipeBoxZone_BiCell[i].Width.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(LamiSystem.RectListRecipeBoxZone_BiCell[i].Width.ToString());    //2
                string liststring4 = LamiSystem.RectListRecipeBoxZone_BiCell[i].Height.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(LamiSystem.RectListRecipeBoxZone_BiCell[i].Height.ToString());   //3

                string liststring5 = tempList[i * tmpItemCount + 0];
                LamiSystem.StrLstRcpConInspData_BiCell.Add(tempList[i * tmpItemCount + 0]);                           //4 : Grid Row
                string liststring6 = tempList[i * tmpItemCount + 1];
                LamiSystem.StrLstRcpConInspData_BiCell.Add(tempList[i * tmpItemCount + 1]);                           //5 : ROI No

                string liststring7 = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[1].Value.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(
                    uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[1].Value.ToString());                        //6 : Type

                string liststring8 = tempList[i * tmpItemCount + 2];
                LamiSystem.StrLstRcpConInspData_BiCell.Add(tempList[i * tmpItemCount + 2]);                           //7 : Seq No

                string liststring9 = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 3].Value.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 3].Value.ToString());             //8 : 방향

                string liststring10 = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 4].Value.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[tmpSeqNo + 4].Value.ToString());             //9 : 극성

                string liststring11 = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[8].Value.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[8].Value.ToString());             //10 : 분할

                string liststring12 = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[9].Value.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[9].Value.ToString());             //11 : 표시

                string liststring13 = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[10].Value.ToString();
                LamiSystem.StrLstRcpConInspData_BiCell.Add(
                uGrd_RecipeBiCell_Data.DisplayLayout.Rows[tmpRowNo].Cells[10].Value.ToString());             //12 : 밝기

                string liststring14 = tempList[i * tmpItemCount + 3];
                LamiSystem.StrLstRcpConInspData_BiCell.Add(tempList[i * tmpItemCount + 3]);                           //13 : List No
            }

            int tmpTitleCount = LamiSystem.RectListRecipeBoxZone_BiCell.Count * ItemCount;
            for (int i = 0; i < tmpTitleCount; i++)
            {
                LamiSystem.StrLstRcpConInspTitle_BiCell.Add(i.ToString("000"));
            }
        }
        */

        private void FormDlgMain_MessageBox_Displaying()
        {
            
        }


        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        void Recipe_Config_Viewer_To_UperGrid()
        {
            LamiSystem.StrLstRcpConGridData_Uper.Clear();
            LamiSystem.StrLstRcpConGridTitle_Uper.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Recipe_Uper.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = string.Empty;
                    strCellData = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    if (strCellData == "1차" || strCellData == "2차")
                    {
                        strCellData = (strCellData == "1차") ? "1" : "2";
                    }
                    //StrLstRcpConInspData_Uper
                    LamiSystem.StrLstRcpConGridData_Uper.Add(strCellData);
                    LamiSystem.StrLstRcpConGridTitle_Uper.Add(regTitle++.ToString("000"));
                }
            }
        }

        void Recipe_Config_Viewer_To_DownGrid()
        {
            LamiSystem.StrLstRcpConGridData_Down.Clear();
            LamiSystem.StrLstRcpConGridTitle_Down.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Recipe_Down.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = string.Empty;
                    strCellData = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    if (strCellData == "1차" || strCellData == "2차")
                    {
                        strCellData = (strCellData == "1차") ? "1" : "2";
                    }

                    LamiSystem.StrLstRcpConGridData_Down.Add(strCellData);
                    LamiSystem.StrLstRcpConGridTitle_Down.Add(regTitle++.ToString("000"));
                    //string strCellData = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    //LamiSystem.StrLstRcpConGridData_Down.Add(strCellData);
                    //LamiSystem.StrLstRcpConGridTitle_Down.Add(regTitle++.ToString("000"));
                }
            }
        }

        /*
        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        void RecipeBiCell_Config_Viewer_To_List_Grid()
        {
            LamiSystem.StrLstRcpConGridData_BiCell.Clear();
            LamiSystem.StrLstRcpConGridTitle_BiCell.Clear();
            int regTitle = 0;

            for (int i = 0; i < uDS_Recipe_BiCell.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_RecipeBiCell_Data.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string strCellData = uGrd_RecipeBiCell_Data.DisplayLayout.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrLstRcpConGridData_BiCell.Add(strCellData);
                    LamiSystem.StrLstRcpConGridTitle_BiCell.Add(regTitle++.ToString("000"));
                }
            }
        }
        */

        /// <summary>
        /// 레시피 설정 데이터 그리드의 드롭다운 리스트 박스에서 데이터 선택 시 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uGrd_Recipe_Data_CellListSelect(object sender, CellEventArgs e)
        {
            Trace.WriteLine("uGrd_Recipe_Data_CellListSelect");
            int nowcell = e.Cell.Column.Index;
            Trace.WriteLine(nowcell.ToString("00"));
            int nowItem = e.Cell.ValueList.SelectedItemIndex;
            Trace.WriteLine(nowItem.ToString("00"));
        }

        private void uGrd_Recipe_Data_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Index == 0)
            {
                Recipe_Config_Display_Select_UperGrid_Row(e);
            }
            else if (e.Cell.Column.Index == 2)
            {
                Recipe_Config_Display_Select_UperGrid_Seq(e,2);
            }
            else if (e.Cell.Column.Index == 5)
            {
                Recipe_Config_Display_Select_UperGrid_Seq(e,5);
            }
           
            return;
        }

        //2015.02.07 WKB 208
        public void Recipe_Config_Display_Select_UperGrid_Seq(CellEventArgs e, int colNo)
        {

            RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_UperGrid_Button_Init();

            int ItemCount = 14;
            int intSeqNo = -1;
            if (colNo == 2) intSeqNo = 0;
            else if (colNo == 5) intSeqNo = 1;
            int DS_Rows = 0;
            if (colNo == 2) DS_Rows = (2 * e.Cell.Row.Index);
            else DS_Rows = (2 * e.Cell.Row.Index) + 1;

            ///2015-01-10 Start
            //if (uGrd_Recipe_Inspect_Uper.Rows.Count <= DS_Rows)
            //{
            //    return;
            //}

            if (uDS_Inspect_Uper.Rows.Count <= DS_Rows)
            {
                return;
            }

            //////2015-01-10 Finish
            
            string tmpZoneNo = uGrd_Recipe_Inspect_Uper.Rows[DS_Rows].Cells[5].Value.ToString();
            int nowSelectZoneNo = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[DS_Rows].Cells[5].Value.ToString());
            string rowType = uGrd_Recipe_Inspect_Uper.Rows[DS_Rows].Cells[6].Value.ToString();

            //int nowSelectZoneNo = int.Parse(uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(5).ToString());
            //string rowType = uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(6).ToString();

            Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
            Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
        }


        //2015.02.07 WKB 207
        /*
        public void Recipe_Config_Display_Select_UperGrid_Seq(CellEventArgs e, int colNo)
        {

            RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_UperGrid_Button_Init();

            int ItemCount = 14;
            int intSeqNo = -1;
            if (colNo == 2) intSeqNo = 0;
            else if (colNo == 5) intSeqNo = 1;
            int DS_Rows = 0;
            if (colNo == 2) DS_Rows = (2*e.Cell.Row.Index);
            else DS_Rows = (2*e.Cell.Row.Index) + 1;

            ///2015-01-10 Start
            if (uGrd_Recipe_Inspect_Uper.Rows.Count <= DS_Rows)
            {
                return;
            }

            //////2015-01-10 Finish
            string tmpZoneNo = uGrd_Recipe_Inspect_Uper.Rows[DS_Rows].Cells[5].Value.ToString();
            int nowSelectZoneNo = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[DS_Rows].Cells[5].Value.ToString());

            string rowType = uGrd_Recipe_Inspect_Uper.Rows[DS_Rows].Cells[6].ToString();
            
            Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
            Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);

//             for (int i = 0; i < _Uper_Control_DrawArea.GetSetGraphicListCount; i++)
//             {
//                 int nowSelectZoneNo = int.Parse(uDS_Inspect_Uper.row[5 + (i * ItemCount)]);
//                 string selectSeqNo = LamiSystem.StrLstRcpConInspData_Uper[7 + (i * ItemCount)];
//                 int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * ItemCount)]);
//                 string rowType = LamiSystem.StrLstRcpConInspData_Uper[6 + (i * ItemCount)];
// 
//                 if ((e.Cell.Row.Index == intGridRowNo && rowType == "넓이") || (e.Cell.Row.Index == intGridRowNo && intSeqNo == int.Parse(selectSeqNo)))
//                 {
//                     Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
//                     Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
//                     return;
//                 }
//             }
        }
        */       
       
        /*
        public void Recipe_Config_Display_Select_UperGrid_Seq(CellEventArgs e, int colNo)
        {
            RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_UperGrid_Button_Init();
            int ItemCount = 14;
            int intSeqNo = -1;
            if (colNo == 2) intSeqNo = 0;
            else if (colNo == 5) intSeqNo = 1;

            for (int i = 0; i < _Uper_Control_DrawArea.GetSetGraphicListCount; i++)
            {
                int nowSelectZoneNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * ItemCount)]);
                string selectSeqNo = LamiSystem.StrLstRcpConInspData_Uper[7 + (i * ItemCount)];
                int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * ItemCount)]);
                string rowType = LamiSystem.StrLstRcpConInspData_Uper[6 + (i * ItemCount)];

                if ((e.Cell.Row.Index == intGridRowNo && rowType == "넓이") || (e.Cell.Row.Index == intGridRowNo && intSeqNo == int.Parse(selectSeqNo)))
                {
                    Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
                    Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
                    return;
                }
            }
        }
        */
        /*       
        RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_UperGrid_Button_Init();

            int ItemCount = 14;
            int intSeqNo = -1;
            if (colNo == 2) intSeqNo = 0;
            else if (colNo == 5) intSeqNo = 1;
            int DS_Rows = 0;
            if (colNo == 2) DS_Rows = (2*e.Cell.Row.Index);
            else DS_Rows = (2*e.Cell.Row.Index) + 1;

            int nowSelectZoneNo = int.Parse(uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(5).ToString());
            string selectSeqNo = uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(7).ToString();
            int intGridRowNo = e.Cell.Row.Index;
            string rowType = uDS_Inspect_Uper.Rows[DS_Rows].GetCellValue(6).ToString();
            
            Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
            Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
        */

        public void Recipe_Config_Display_Select_DownGrid_Seq(CellEventArgs e, int colNo)
        {
            RecipeGap_drawArea2.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_DownGrid_Button_Init();

            int ItemCount = 14;
            int intSeqNo = -1;
            if (colNo == 2) intSeqNo = 0;
            else if (colNo == 5) intSeqNo = 1;
            int DS_Rows = 0;
            if (colNo == 2) DS_Rows = (2 * e.Cell.Row.Index);
            else DS_Rows = (2 * e.Cell.Row.Index) + 1;

            int nowSelectZoneNo = int.Parse(uGrd_Recipe_Inspect_Down.Rows[DS_Rows].Cells[5].Value.ToString());
            string rowType = uGrd_Recipe_Inspect_Down.Rows[DS_Rows].Cells[6].ToString();

            Recipe_Config_Display_Select_DownGrid_To_Rect(nowSelectZoneNo);
            Recipe_Config_Display_Select_DownGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
        }

        /*
        public void Recipe_Config_Display_Select_DownGrid_Seq(CellEventArgs e, int colNo)
        {
            RecipeGap_drawArea2.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_DownGrid_Button_Init();
            int ItemCount = 14;
            int intSeqNo = -1;
            if (colNo == 2) intSeqNo = 0;
            else if (colNo == 5) intSeqNo = 1;

            for (int i = 0; i < _Uper_Control_DrawArea.GetSetGraphicListCount; i++)
            {
                int nowSelectZoneNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * ItemCount)]);
                string selectSeqNo = LamiSystem.StrLstRcpConInspData_Uper[7 + (i * ItemCount)];
                int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * ItemCount)]);
                string rowType = LamiSystem.StrLstRcpConInspData_Uper[6 + (i * ItemCount)];

                if ((e.Cell.Row.Index == intGridRowNo && rowType == "넓이") ||
                    (e.Cell.Row.Index == intGridRowNo && intSeqNo == int.Parse(selectSeqNo)))
                {
                    Recipe_Config_Display_Select_DownGrid_To_Rect(nowSelectZoneNo);
                    Recipe_Config_Display_Select_DownGrid_Row_To_Button(e.Cell.Row.Index, colNo, rowType);
                    return;
                }
            }
        }
        */

        public void Recipe_Config_Display_Select_UperGrid_Row_To_Button(int rowNo, int colNo, string selectType)
        {
            uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;
            if (selectType == "넓이")
            {
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
            }
            else
                uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[colNo].ButtonAppearance.ForeColor = Color.White;
            return;
        }


        public void Recipe_Config_Display_Select_DownGrid_Row_To_Button(int rowNo, int colNo, string selectType)
        {
            uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;
            if (selectType == "넓이")
            {
                uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[2].ButtonAppearance.ForeColor = Color.White;
                uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[5].ButtonAppearance.ForeColor = Color.White;
            }
            else
                uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[colNo].ButtonAppearance.ForeColor = Color.White;
            return;
        }


        public void Recipe_Config_Display_Select_UperGrid_Row(CellEventArgs e)
        {
            int ItemCount = 14;
            RecipeGap_drawArea1.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_UperGrid_Button_Init();

            for (int i = 0; i < _Uper_Control_DrawArea.GetSetGraphicListCount; i++)
            {
                int nowSelectZoneNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * ItemCount)]);
                string selectSeqNo = LamiSystem.StrLstRcpConInspData_Uper[7 + (i * ItemCount)];
                int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * ItemCount)]);


                if (e.Cell.Row.Index == intGridRowNo)
                {
                    Recipe_Config_Display_Select_UperGrid_To_Rect(nowSelectZoneNo);
                    Recipe_Config_Display_Select_UperGrid_Row_To_Button(e.Cell.Row.Index);
                }
            }
            return;
        }

        public void Recipe_Config_Display_Select_DownGrid_Row(CellEventArgs e)
        {
            int ItemCount = 14;
            RecipeGap_drawArea2.GetSetGraphicsList.UnselectAll();
            Recipe_Config_Display_Select_DownGrid_Button_Init();

            for (int i = 0; i < _Down_Control_DrawArea.GetSetGraphicListCount; i++)
            {
                int nowSelectZoneNo = int.Parse(LamiSystem.StrLstRcpConInspData_Down[5 + (i * ItemCount)]);
                string selectSeqNo = LamiSystem.StrLstRcpConInspData_Down[7 + (i * ItemCount)];
                int intGridRowNo = int.Parse(LamiSystem.StrLstRcpConInspData_Down[4 + (i * ItemCount)]);


                if (e.Cell.Row.Index == intGridRowNo)
                {
                    Recipe_Config_Display_Select_DownGrid_To_Rect(nowSelectZoneNo);
                    Recipe_Config_Display_Select_DownGrid_Row_To_Button(e.Cell.Row.Index);
                }
            }
            return;
        }

        public void Recipe_Config_Display_Select_UperGrid_To_Rect(int selectRow)
        {
            int rectAddress = (_Uper_Control_DrawArea.GetSetGraphicListCount - selectRow) - 1;

            RecipeGap_drawArea1.GetSetGraphicsList[rectAddress].Selected = true;
            RecipeGap_drawArea1.pictureBox1.Refresh();
            return;
        }

        public void Recipe_Config_Display_Select_UperGrid_Row_To_Button(int rowNo)
        {
            uGrd_Recipe_UperData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;
            return;
        }

        public void Recipe_Config_Display_Select_DownGrid_To_Rect(int selectRow)
        {
            int rectAddress = (_Down_Control_DrawArea.GetSetGraphicListCount - selectRow) - 1;

            RecipeGap_drawArea2.GetSetGraphicsList[rectAddress].Selected = true;
            RecipeGap_drawArea2.pictureBox1.Refresh();
            return;
        }

        public void Recipe_Config_Display_Select_DownGrid_Row_To_Button(int rowNo)
        {
            uGrd_Recipe_DownData.DisplayLayout.Rows[rowNo].Cells[0].ButtonAppearance.ForeColor = Color.White;
            return;
        }

        public void Recipe_Config_Display_Select_UperGrid_Button_Init()
        {
            for (int i = 0; i < uGrd_Recipe_UperData.DisplayLayout.Rows.Count; i++)
            {
                uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[0].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[2].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[5].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            }
            return;
        }


        public void Recipe_Config_Display_Select_DownGrid_Button_Init()
        {
            for (int i = 0; i < uGrd_Recipe_DownData.DisplayLayout.Rows.Count; i++)
            {
                uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[0].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[2].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[5].ButtonAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            }
            return;
        }
        #endregion


        #region 레시피 설정 탭 : 기타 메소드



        private void MakeModeFlaging()
        {
//             if (checkBox1.Checked == true)
//                 _Gap_Control_DrawArea.ModeCheckFlag = true;
//             else
//             {
//                 _Gap_Control_DrawArea.ModeCheckFlag = false;
//             }
            _Uper_Control_DrawArea.ModeChecking();
        }

        #endregion

        /// <summary>
        /// OpenCV 설정 방법
        /// 1.C:\opencv 2.4.5 위치 시킴(32bit)
        /// 2.bin\debug\에 core,imgproc등 dll값은 버전을 위치 시켜야 한다. 만약 다르면
        ///   C++ Dll 임포트시 Dll을 찾을 수없다는 에러 발생 시킴.
        /// 3.결론은 시스템에서 시스템 패스로 지정한 오픈씨브이 버전과 디버그에 위치하는
        ///   버전의 파일이 같은지 확인해야한다. 다르다면 DLL을 찾을 수 없다는 에러발생함.
        /// 4.현재 시스템에 32비트 2.4.5 버전이 사용된다.
        /// </summary>
        /// <param name="imgPath"></param>
       [DllImport("VisionLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Searching_Line(string pathImage);
        private void button1_Click(object sender, EventArgs e)
        {
            //CUltraMessageBox messageBox = new CUltraMessageBox();
            //messageBox.MessageBox_Show("메세지 박스", "메세지 박스 헤더의 내용입니다.","메세지 박스의 메세지 내용입니다.\r\n전달하고자 하는 메세지를 전송한다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            //Control_UMAC umac = new Control_UMAC();
            //umac.Umac_Open();
        }

        #endregion

        private void ubtnToolbarApply_Click(object sender, EventArgs e)
        {

        }

        private void Vision_uBtn_ImageSave_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    Image_File_Save(Vision_Config_IplBox1.Image);
                    break;
                case "RecipeDown":
                    Image_File_Save(Vision_Config_IplBox2.Image);
                    break;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        /*
         * 
        private void Vision_uBtn_ImageSave_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    Image_File_Save(Vision_Config_IplBox1.ImageIpl);
                    break;
                case "RecipeDown":
                    Image_File_Save(Vision_Config_IplBox2.ImageIpl);
                    break;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
        
        private void Image_File_Save(object imageFile)
        {
            string fileName = string.Empty;

            var saveFile = new SaveFileDialog();
            // 다이얼 로그가 Open되었을 때 최초의 경로 설정
            //string filePath = System.Windows.Forms.Application.StartupPath + @"\Data\\ConfigFiles\SystemConfigDefault.cfg";
            saveFile.InitialDirectory = @"D:\AlignImage";

            // 다이얼 로그의 제목
            saveFile.Title = "이미지 저장 위치 지정";

            // 기본 확장자
            saveFile.DefaultExt = "jpg";

            // 파일 목록 필터링
            saveFile.Filter = "JPG files(*.jpg)|*.jpg";

            // OK버튼을 눌렀을때의 동작
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                // 경로와 파일명을 fileName에 저장
                fileName = saveFile.FileName.ToString();
                ((IplImage)(imageFile)).SaveImage(fileName);
                this.Select();
            }
        }
        */


        private void Image_File_Save(Image imageFile)
        {
            string fileName = string.Empty;

            var saveFile = new SaveFileDialog();
            // 다이얼 로그가 Open되었을 때 최초의 경로 설정
            //string filePath = System.Windows.Forms.Application.StartupPath + @"\Data\\ConfigFiles\SystemConfigDefault.cfg";
            saveFile.InitialDirectory = @"D:\AlignImage";

            // 다이얼 로그의 제목
            saveFile.Title = "이미지 저장 위치 지정";

            // 기본 확장자
            saveFile.DefaultExt = "jpg";

            // 파일 목록 필터링
            saveFile.Filter = "JPG files(*.jpg)|*.jpg";

            // OK버튼을 눌렀을때의 동작
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                // 경로와 파일명을 fileName에 저장
                fileName = saveFile.FileName.ToString();

                //이미지 일기에서 읽은 이미지와 저장하는 이미지가 같은 
                //이미지 일때 에러가 발생하기 때문에 이를 방지하기 위한
                //조건문이다.
                //strLoad_Image_Name = 이미지 읽기에서 저장하는 파일명
                if (fileName == strLoad_Image_Name) return;

                imageFile.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                this.Select();
            }
        }

        private Image Image_File_Open()
        {
            string strOpenFilePath = FileDialog_Open();
            Image OpenImage = null;
            if (strOpenFilePath != null)
            {
                //Vision_Config_IplBox1.ImageIpl = IplImage.FromFile(strOpenFilePath);
                OpenImage = Image.FromFile(strOpenFilePath);
            }
            return OpenImage;
        }

        /*
        string strOpenFilePath = FileDialog_Open();
            if (strOpenFilePath != null)
            {
                //Vision_Config_IplBox1.ImageIpl = IplImage.FromFile(strOpenFilePath);
                Image firstImage = Image.FromFile(strOpenFilePath);
                Vision_Config_IplBox1.Image = firstImage;
                Vision_Config_IplBox1.Refresh();
            }
        */

        private void Recipe_uBtn_ImageSave_Click(object sender, EventArgs e)
        {
            Image_File_Save(RecipeGap_drawArea1.pictureBox1.Image);
        }

        private string strLoad_Image_Name = string.Empty;
        private string strOld_File_Directory = string.Empty;

        //20150302 WKB 209
        private void Recipe_uBtn_ImageRead_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeGap_drawArea1.GetSet_ImageLoadPass = true;
                RecipeGap_drawArea2.GetSet_ImageLoadPass = true;
                //RecipeGap_drawArea1.pictureBox1.Image = Image_File_Open();
                var pFileDlg = new OpenFileDialog
                {
                    DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                    //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                    Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                    FilterIndex = 1,
                    Title = @"Image File Open"
                };

                if (strOld_File_Directory == "")
                {
                    pFileDlg.InitialDirectory = Environment.CurrentDirectory + "\\DefaultImage";
                }
                else
                {
                    pFileDlg.InitialDirectory = strOld_File_Directory;
                }
                
                if (pFileDlg.ShowDialog() == DialogResult.OK)
                {
                    string strFullPathFile = pFileDlg.FileName;

                    strLoad_Image_Name = strFullPathFile;

                    strOld_File_Directory = pFileDlg.FileName;
                    switch (this.ultraTabControl2.ActiveTab.Key)
                    {
                        case "UperRecipe":
                            RecipeGap_drawArea1.pictureBox1.Image = Image.FromFile(strFullPathFile);
                            break;
                        case "DownRecipe":
                            RecipeGap_drawArea2.pictureBox1.Image = Image.FromFile(strFullPathFile);
                            break;
                    }
                }

                RecipeGap_drawArea1.GetSet_ImageLoadPass = false;
                RecipeGap_drawArea2.GetSet_ImageLoadPass = false;
            }

            catch (Exception exception)
            {
                var pFileDlg = new OpenFileDialog
                {
                    DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                    //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                    Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                    FilterIndex = 1,
                    Title = @"Image File Open"
                };

                pFileDlg.InitialDirectory = Environment.CurrentDirectory + "\\DefaultImage";

                if (pFileDlg.ShowDialog() == DialogResult.OK)
                {
                    string strFullPathFile = pFileDlg.FileName;

                    strLoad_Image_Name = strFullPathFile;

                    strOld_File_Directory = pFileDlg.FileName;
                    switch (this.ultraTabControl2.ActiveTab.Key)
                    {
                        case "UperRecipe":
                            RecipeGap_drawArea1.pictureBox1.Image = Image.FromFile(strFullPathFile);
                            break;
                        case "DownRecipe":
                            RecipeGap_drawArea2.pictureBox1.Image = Image.FromFile(strFullPathFile);
                            break;
                    }
                }
            }
           
        }

        //20150302 WKB 208
        /*
        
        private void Recipe_uBtn_ImageRead_Click(object sender, EventArgs e)
        {
            //RecipeGap_drawArea1.pictureBox1.Image = Image_File_Open();
            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            pFileDlg.InitialDirectory = Environment.CurrentDirectory+"\\DefaultImage";
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strFullPathFile = pFileDlg.FileName;

                strLoad_Image_Name = strFullPathFile;

                switch (this.ultraTabControl2.ActiveTab.Key)
                {
                    case "UperRecipe":
                        RecipeGap_drawArea1.pictureBox1.Image = Image.FromFile(strFullPathFile);
                        break;
                    case "DownRecipe":
                        RecipeGap_drawArea2.pictureBox1.Image = Image.FromFile(strFullPathFile);
                        break;
                }
                
                
            }
        }

        */

        /*        
        private void Recipe_uBtn_ImageSave_Click(object sender, EventArgs e)
        {
            Image_File_Save(RecipeGap_drawArea1.pictureBox1.ImageIpl);
        }
         
        private void Recipe_uBtn_ImageRead_Click(object sender, EventArgs e)
        {
            //RecipeGap_drawArea1.pictureBox1.Image = Image_File_Open();
            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            pFileDlg.InitialDirectory = Environment.CurrentDirectory+"\\DefaultImage";
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strFullPathFile = pFileDlg.FileName;
                switch (this.ultraTabControl2.ActiveTab.Key)
                {
                    case "UperRecipe":
                        RecipeGap_drawArea1.pictureBox1.ImageIpl = IplImage.FromFile(strFullPathFile);
                        break;
                    case "DownRecipe":
                        RecipeGap_drawArea2.pictureBox1.ImageIpl = IplImage.FromFile(strFullPathFile);
                        break;
                }
                
                
            }
        }
        */

        private void ubtnToolbarExit_Click(object sender, EventArgs e)
        {
            //Param : 1=종료 확인 메세지
            if(uMessageBox.DlgMain_Manu_Message(1) == DialogResult.OK) this.Close();
        }

        private void FormDlgMain_Paint(object sender, PaintEventArgs e)
        {
            this.Focus();
        }

        private void Vision_uBtn_TabMeas_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    MainDlg_Image_Grab_MIL_Uper();
                    Vision_Config_IplBox1.ImageIpl = srcIplImage_Uper;
                    Vision_Config_IplBox1.Refresh();
                    break;
                case "RecipeDown":
                    MainDlg_Image_Grab_MIL_Down();
                    Vision_Config_IplBox2.ImageIpl = srcIplImage_Down;
                    Vision_Config_IplBox2.Refresh();
                    break;
            }

            //MainDlg_Image_Grab_MIL_Uper();
            //Vision_Config_IplBox1.ImageIpl = srcIplImage_Uper;
            //Vision_Config_IplBox1.Refresh();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            string returnData = umac.Umac_Open();
        }
        
        private void Equipment_uBtn_SetupApply_Click(object sender, EventArgs e)
        {
            Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
            DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("SystemMessage02Catipion"),
                rm.GetString("SystemMessage02Header"), rm.GetString("SystemMessage02Content"),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dlgResult == DialogResult.Cancel) return;

            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            Equipment_Config_Viewer_To_List_Data();
        }

        //해당 탭의 콘트롤 콜렉션을 지정해주면 콘트롤배열(Control[])이 되어서 
        //위의 해당 함수가 두가지로 오버로딩되었다.
        private void Equipment_Config_Viewer_To_List_Data()
        {
            LamiSystem.strLstEquipData.Clear();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("EquipmentConfig_Tab_BackPanel", true), ref al);

            for (int i = 0; i < LamiSystem.strLstEquipName.Count; i++)
            {
                for (int j = 0; j < al.Count; j++)
                {
                    if (((Control) al[j]).Name.ToString() == LamiSystem.strLstEquipName[i])
                    {
                        LamiSystem.strLstEquipData.Add(((Control) al[j]).Text);
                        break;
                    }
                }
            }
        }

        //데이터 리스트의 데이터를 레지스터에 저장한다.
        private void Equipment_Config_ListData_To_Register()
        {
            Equipment_Config_Lists_To_Register(LamiSystem.RegPathEquip, LamiSystem.strLstEquipTitle, LamiSystem.strLstEquipName, LamiSystem.strLstEquipData);
        }

        //리스트 배열의 값을 레지스트리에 저장한다.
        public void Equipment_Config_Lists_To_Register(string strNodePath, List<string> regTitle, List<string> regControl, List<string> regData)
        {
            for (int i = 0; i < regTitle.Count; i++)
            {
                this.SetReg(strNodePath, regTitle[i], regControl[i] + "\t" + regData[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Equipment_Umac06_uTxt_Click(object sender, EventArgs e)
        {
            //string strRec =  umac.Umac_Communicate_Command(Equipment_Umac05_uTxt.Text);
            //umac.Umac_GetData("P371,20");
            //Equipment_Umac07_uTxt.Text += strRec + "\r\n";
        }

        //연결 버튼 클릭 이벤트 핸들러
        private void Equipment_Umac08_uTxt_Click(object sender, EventArgs e)
        {
            Equipment_Umac07_uTxt.Text += umac.Umac_Open() + "\r\n";
        }

        private void EquipmentConfig_Tab_BackPanel_PaintClient(object sender, PaintEventArgs e)
        {

        }

        private void ultraLabel18_Click(object sender, EventArgs e)
        {
        }

        private void ultraLabel19_Click(object sender, EventArgs e)
        {
           
        }

        private void ultraLabel20_Click(object sender, EventArgs e)
        {
            
        }

        private void ultraTextEditor69_ValueChanged(object sender, EventArgs e)
        {
        }

        private void ultraLabel14_Click(object sender, EventArgs e)
        {
            if (lvs == null) lvs = new Control_PN2212(sPort1);
            string[] names = lvs.LVS_GetPort();
            for (int i = 0; i < names.Length; i++)
            {
                Commu_uCom04_Port.Items.Add(names[i]);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (lvs == null) lvs = new Control_PN2212(sPort1);
            lvs.LVS_Connect();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (lvs != null) lvs.LVS_Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lvs != null) LVS_Duty_Set();
        }

        public void LVS_Duty_Set()
        {
            byte[] sendData = new byte[7];
            sendData[0] = Convert.ToByte('L');
            sendData[1] = 0x31;
            sendData[2] = 0x30;
            sendData[3] = 0x32;
            sendData[4] = 0x30;
            sendData[5] = 0x0D;
            sendData[6] = 0x0A;
            sPort1.Write(sendData, 0, sendData.Length);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lvs != null) LVS_OFF_Set();
        }

        public void LVS_OFF_Set()
        {
            byte[] sendData = new byte[4];
            sendData[0] = 0x45;//E
            sendData[1] = 0x31;
            sendData[2] = 0x0D;
            sendData[3] = 0x0A;
            sPort1.Write(sendData, 0, sendData.Length);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lvs != null) lvs.LVS_ON_Set();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (lvs != null) lvs.LVS_Now_Set();
        }

        private void sPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (sPort1.IsOpen)
            {
                this.Invoke(new EventHandler(SerialRecived));
            }
        }

        private void SerialRecived(object sender, EventArgs e)
        {
            int iInputLengh = sPort1.BytesToRead;

            if (iInputLengh > 0)
            {
                string tmpStr = "";

                char[] inputArray = new char[iInputLengh];
                sPort1.Read(inputArray, 0, iInputLengh);
                foreach (var a in inputArray)
                {
                    tmpStr += a;
                }

                Equipment_Umac07_uTxt.AppendText(tmpStr);
                Equipment_Umac07_uTxt.SelectionStart = Equipment_Umac07_uTxt.Text.Length;
                Equipment_Umac07_uTxt.ScrollToCaret();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            umac.Umac_SetData_StringOne("P386=9876");
            umac.Umac_SetData_StringOne("P385=12211221122");
            string tmpread1 = umac.Umac_GetData_StringOne(385);
            string tmpread2 = umac.Umac_GetData_StringOne(386);
        }

        Matrix mat = new Matrix();
        //private double PI = 3.1415926535 8979323846 2643383279;
        private double PI = 3.1415926535;
        float[] rp = new float[6];

        System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Chartreuse);
        private Graphics g;// = pnlCanvas.CreateGraphics();
        Point cp = new Point(350, 350);
        Point p1 = new Point(600, 350);
        private Point p2 = new Point(0, 0);
        SolidBrush mBrush = new SolidBrush(Color.White);

        private void button9_Click(object sender, EventArgs e)
        {
            g = pnlCanvas.CreateGraphics();
            g.Transform = mat;

            System.Drawing.Rectangle mr1 = new System.Drawing.Rectangle(cp.X - 2, cp.Y - 2, 4, 4);


            g.Clear(pnlCanvas.BackColor);
            g.DrawEllipse(myPen, mr1);

            System.Drawing.Rectangle mr2 = new System.Drawing.Rectangle(p1.X - 2, p1.Y - 2, 4, 4);
            g.DrawEllipse(myPen, mr2);

            g.DrawLine(myPen, cp, p1);

            g.DrawString(cp.X.ToString() + "/" + cp.Y.ToString(), SystemFonts.DefaultFont, mBrush, cp.X - 23, cp.Y - 20);
            g.DrawString(p1.X.ToString() + "/" + p1.Y.ToString(), SystemFonts.DefaultFont, mBrush, p1.X - 23, p1.Y - 20);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mat.Translate(p1.X, p1.Y);
            mat.RotateAt(270f, cp, MatrixOrder.Append);
            rp = mat.Elements;

            string p2x = GetFormattedString(rp[4], 0);
            string p2y = GetFormattedString(rp[5], 0);
            p2.X = Convert.ToInt32(p2x);
            p2.Y = Convert.ToInt32(p2y);
            g.DrawLine(myPen, cp, p2);

            System.Drawing.Rectangle mr1 = new System.Drawing.Rectangle(p2.X - 2, p2.Y - 2, 4, 4);
            g.DrawEllipse(myPen, mr1);

            g.DrawString(p2.X.ToString() + "/" + p2.Y.ToString(), SystemFonts.DefaultFont, mBrush, p2.X - 23, p2.Y + 20);
        }
        /// <summary>
        /// 반올림 함수
        /// </summary>
        /// <param name="obj">반올림하고자 하는 값</param>
        /// <param name="dgt">표시하고자 하는 소숫점 자리수</param>
        /// <returns></returns>
        public static string GetFormattedString(object obj, int dgt)
        {
            double cv = 0;
            string dumStr = string.Empty;
            int totLen = 2 + dgt;

            for (int i = 0; i < dgt; i++)
                dumStr += "0";

            if (obj == null)
                return "0." + dumStr;

            if (obj is string)
            {
                if (Convert.ToString(obj as string).Trim() == string.Empty)
                {
                    return "0." + dumStr;
                }
                cv = double.Parse(Convert.ToString(obj as string).Trim());
            }
            else if (obj is double)
            {
                cv = (double)obj;
            }
            else if (obj is int)
            {
                cv = (int)obj;
            }
            else if (obj is float)
            {
                cv = (float)obj;
            }

            // Gets a NumberFormatInfo associated with the en-US culture.
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            // Displays the same value with four decimal digits.
            nfi.NumberDecimalDigits = dgt;

            return cv.ToString("N", nfi);
        }


        private void Vision_uBtn_TestManual_Click(object sender, EventArgs e)
        {
            //CaptureDevice milDevice = new CaptureDevice();
            //milDevice = CaptureDevice.MIL;
           

            //MIL.MdigGrab(MilDigitizer, MilImage);       // 트리거 신호 대기후 Grab
            //int band = MIL.MbufInquire(MilImage, MIL.M_SIZE_BAND, MIL.M_NULL);
            //IplImage cvImage = Cv.CreateImage(new CvSize(2352, 1728), BitDepth.U8, band);
            //IntPtr cvImagePtr = cvImage.ImageData;




            /*
            //MIL.MbufClear(MilBuffer, MIL.M_DEFAULT);
            //MIL.MbufLoad(fname, MilImage);
            //MIL.MbufGet2d(MilImage, 0, 0, 1, 1, cvImage.ImageDataOrigin);
            */
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["System"];
        }

        private void FormDlgMain_Paint_1(object sender, PaintEventArgs e)
        {
            //ustbar.Text = this.Width.ToString() + " " + this.Height.ToString() + "  X:" + this.Location.X.ToString() + " Y:" + this.Location.Y.ToString();
        }

        private void 감시창ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["LogData"];
        }

        private void 수동모드ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["System"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["System"];
        }

        private void 비전부설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["Vision"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Vision"];
        }

        private void 환경설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["Config"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Config"];
        }

        private void 레시피설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["Recipe"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Recipe"];
        }

        private void 시스템ToolStripMenuItem1_DropDownOpened(object sender, EventArgs e)
        {
            if (LamiSystem.GetSet_Now_User_Account == Account_Operator)
            {
                레시피열기ToolStripMenuItem.Enabled = false;
                레시피저장ToolStripMenuItem.Enabled = false;
                레시피적용ToolStripMenuItem.Enabled = false;
            }
            else
            {
                레시피열기ToolStripMenuItem.Enabled = true;
                레시피저장ToolStripMenuItem.Enabled = true;
                레시피적용ToolStripMenuItem.Enabled = true;
            }
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubtnToolbarExit.PerformClick();
        }

        private void logOnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 사용자변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubtnToolbarUser.PerformClick();
        }

        private void 검사진행ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubtnToolbarInspect.PerformClick();
        }

        private void 설정ToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (LamiSystem.GetSet_Now_User_Account == Account_Operator)
            {
                수동모드ToolStripMenuItem1.Enabled = false;
                비전부설정ToolStripMenuItem.Enabled = false;
                레시피설정ToolStripMenuItem.Enabled = false;
                환경설정ToolStripMenuItem.Enabled = false;
            }
            else if (LamiSystem.GetSet_Now_User_Account == Account_Engineer)
            {
                수동모드ToolStripMenuItem1.Enabled = true;
                비전부설정ToolStripMenuItem.Enabled = true;
                레시피설정ToolStripMenuItem.Enabled = true;
                환경설정ToolStripMenuItem.Enabled = false;
            }
            else if (LamiSystem.GetSet_Now_User_Account == Account_Maker)
            {
                수동모드ToolStripMenuItem1.Enabled = true;
                비전부설정ToolStripMenuItem.Enabled = true;
                레시피설정ToolStripMenuItem.Enabled = true;
                환경설정ToolStripMenuItem.Enabled = true;
            }
        }

        private void 레시피열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubtnToolbarOpen.PerformClick();
        }

        private void 레시피저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubtnToolbarSave.PerformClick();
        }

        private void 레시피적용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubtnToolbarApply.PerformClick();
        }

        private void ultraButton13_Click(object sender, EventArgs e)
        {

        }

        private void System_uBtn_ImagePath_BiCell_Click(object sender, EventArgs e)
        {
            System_uTxt_ImagePath_BiCell.Text = FolderBrowser_Open();
        }

        private void System_uBtn_MeasPath_BiCell_Click(object sender, EventArgs e)
        {
            System_uTxt_MeasPath_BiCell.Text = FolderBrowser_Open();
        }

        private void System_uBtn_DCF_File_Gap_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            if (fileDlg.ShowDialog() == DialogResult.OK)
                System_uTxt_DCF_Path_Gap.Text = fileDlg.FileName;
        }

        private void System_uBtn_DCF_File_BiCell_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            if (fileDlg.ShowDialog() == DialogResult.OK)
                System_uTxt_DCF_Path_BiCell.Text = fileDlg.FileName;
        }

        private void ultraButton19_Click(object sender, EventArgs e)
        {
            /*
            Control_UltraMessageBox messageBox = new Control_UltraMessageBox();

            DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("RecipeMessage01Caption"), rm.GetString("RecipeMessage01Header"), rm.GetString("RecipeMessage01Content"),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dlgResult == DialogResult.Cancel) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //그림상자의 이미지와 설정한 구역의 배율을 계산하고 저장한다.
            RecipeBiCell_Config_Box_To_Image_Sum();
            
            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            RecipeBiCell_Config_Viewer_To_List_Data();

            //설정한 화면의 그리드 값들을 리스트 배열에 저장한다.
            RecipeBiCell_Config_Viewer_To_List_Grid();
            
            //설정한 화면의 검출 영역을 배열에 저장한다.
            RecipeBiCell_Config_Viewer_To_List_Inspect();

            this.Cursor = System.Windows.Forms.Cursors.Default;
            */
        }

        private void VisionBiCell_uBtn_ParamApply_Click(object sender, EventArgs e)
        {
            //Control_UltraMessageBox messageBox = new Control_UltraMessageBox();
            //DialogResult dlgResult = messageBox.MessageBox_Show(rm.GetString("VisionMessage01Caption"),rm.GetString("VisionMessage01Header"), rm.GetString("VisionMessage01Content"),MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (dlgResult == DialogResult.Cancel) return;

            //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //VisionBiCell_Config_Viewer_To_List_Data();
            //VisionBiCell_Config_Viewer_To_List_Grid();

            //this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void ultraButton17_Click(object sender, EventArgs e)
        {
            //Recipe_uBtn_ModelChange.PerformClick();
        }

        private void Recipe_uBtn_Tab_Meas_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    MainDlg_Image_Grab_MIL_Uper();
                    RecipeGap_drawArea1.pictureBox1.Image = BitmapConverter.ToBitmap(srcIplImage_Uper);
                    RecipeGap_drawArea1.pictureBox1.Refresh();
                    break;
                case "DownRecipe":
                    MainDlg_Image_Grab_MIL_Down();
                    RecipeGap_drawArea2.pictureBox1.Image = BitmapConverter.ToBitmap(srcIplImage_Down);;
                    RecipeGap_drawArea2.pictureBox1.Refresh();
                    break;
            }

            //MainDlg_Image_Grab_MIL_Gap();
            //RecipeGap_drawArea1.pictureBox1.ImageIpl = srcIplImage_Uper;
            //RecipeGap_drawArea1.pictureBox1.Refresh();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        /*
        private void Recipe_uBtn_Tab_Meas_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    MainDlg_Image_Grab_MIL_Uper();
                    RecipeGap_drawArea1.pictureBox1.ImageIpl = BitmapConverter.ToBitmap(srcIplImage_Uper);
                    RecipeGap_drawArea1.pictureBox1.Refresh();
                    break;
                case "DownRecipe":
                    MainDlg_Image_Grab_MIL_Down();
                    RecipeGap_drawArea2.pictureBox1.ImageIpl = srcIplImage_Down;
                    RecipeGap_drawArea2.pictureBox1.Refresh();
                    break;
            }

            //MainDlg_Image_Grab_MIL_Gap();
            //RecipeGap_drawArea1.pictureBox1.ImageIpl = srcIplImage_Uper;
            //RecipeGap_drawArea1.pictureBox1.Refresh();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        */
        private void Inspect_MIL_Initialize_Gap()
        {
            OpenCV_Open_Uper();
        }

        private void Inspect_MIL_Initialize_BiCell()
        {
            OpenCV_Open_BiCell();
        }

        MIL_ID MilApplication = MIL.M_NULL;         // Application identifier.
        MIL_ID MilSystem_Uper = MIL.M_NULL;              // System identifier.
        MIL_ID MilSystem_Down = MIL.M_NULL;              // System identifier.

        MIL_ID MilDisplay_Uper = MIL.M_NULL;             // Display identifier.
        MIL_ID MilDigitizer_Uper = MIL.M_NULL;           // Digitizer identifier.
        MIL_ID MilImage_Uper = MIL.M_NULL;               // Image buffer identifier.

        MIL_ID MilDisplay_Down = MIL.M_NULL;             // Display identifier.
        MIL_ID MilDigitizer_Down = MIL.M_NULL;           // Digitizer identifier.
        MIL_ID MilImage_Down = MIL.M_NULL;               // Image buffer identifier.

        private CvMat matImg_Uper = new CvMat(4096, 3072, MatrixType.U8C1);
        private byte[,] imgBuf_Uper;// = new byte[_iGrabImageSero, _iGrabImageGaro];
        private CvSize cvSize_Uper;// = new CvSize(_iGrabImageGaro, _iGrabImageSero);
        private IplImage srcIplImage_Uper;// = Cv.CreateImageHeader(cvSize, BitDepth.U8, 1);
        private int _iGrabImageGaro_Uper;// = AlignSystem.GrabImageSizeGaro;
        private int _iGrabImageSero_Uper;// = AlignSystem.GrabImageSizeSero;

        private CvMat matImg_Down = new CvMat(4096, 3072, MatrixType.U8C1);
        private byte[,] imgBuf_Down;// = new byte[_iGrabImageSero, _iGrabImageGaro];
        private CvSize cvSize_Down;// = new CvSize(_iGrabImageGaro, _iGrabImageSero);
        private IplImage srcIplImage_Down;// = Cv.CreateImageHeader(cvSize, BitDepth.U8, 1);
        private int _iGrabImageGaro_Down;// = AlignSystem.GrabImageSizeGaro;
        private int _iGrabImageSero_Down;// = AlignSystem.GrabImageSizeSero;

        public void OpenCV_Open_Uper()
        {
            _iGrabImageGaro_Uper = LamiSystem.GrabImageSizeGaro_Gap;
            _iGrabImageSero_Uper = LamiSystem.GrabImageSizeSero_Gap;

            matImg_Uper = new CvMat(_iGrabImageSero_Uper, _iGrabImageGaro_Uper, MatrixType.U8C1);
            imgBuf_Uper = new byte[_iGrabImageSero_Uper, _iGrabImageGaro_Uper];
            cvSize_Uper = new CvSize(_iGrabImageGaro_Uper, _iGrabImageSero_Uper);
            //srcIplImage = Cv.CreateImageHeader(cvSize, BitDepth.U8, 3);
            srcIplImage_Uper = new IplImage(cvSize_Uper, BitDepth.U8, 3);
        }

        public void OpenCV_Open_BiCell()
        {
            _iGrabImageGaro_Down = LamiSystem.GrabImageSizeGaro_BiCell;
            _iGrabImageSero_Down = LamiSystem.GrabImageSizeSero_BiCell;

            matImg_Down = new CvMat(_iGrabImageSero_Down, _iGrabImageGaro_Down, MatrixType.U8C1);
            imgBuf_Down = new byte[_iGrabImageSero_Down, _iGrabImageGaro_Down];
            cvSize_Down = new CvSize(_iGrabImageGaro_Down, _iGrabImageSero_Down);
            //srcIplImage = Cv.CreateImageHeader(cvSize, BitDepth.U8, 3);
            srcIplImage_Down = new IplImage(cvSize_Down, BitDepth.U8, 3);
        }

        private string MIL_Status_Uper = "Close";
        private string MIL_Status_Down = "Close";
        public void MIL_Open_Uper()
        {
            MIL_Status_Uper = "Open";
            MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 0, MIL.M_DEFAULT, ref MilSystem_Uper); // 프레임그레버 할당
            MIL.MdigAlloc(MilSystem_Uper, MIL.M_DEV0, @"C:\Visionsystem\Data\solfcl_mil9_CSC12M25BMP19_4tap_8bit_c.dcf", MIL.M_DEFAULT, ref MilDigitizer_Uper);
            MIL.MdispAlloc(MilSystem_Uper, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Uper);
            MIL.MbufAlloc2d(MilSystem_Uper, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref MilImage_Uper);
        }

        public void MIL_Open_Down()
        {
            MIL_Status_Down = "Open";
            MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 1, MIL.M_DEFAULT, ref MilSystem_Down); // 프레임그레버 할당
            MIL.MdigAlloc(MilSystem_Down, MIL.M_DEV0, @"C:\Visionsystem\Data\solfcl_mi9_CSC12M25BMP19_4tap_8bit_c.dcf", MIL.M_DEFAULT, ref MilDigitizer_Down);
            MIL.MdispAlloc(MilSystem_Down, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Down);
            MIL.MbufAlloc2d(MilSystem_Down, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref MilImage_Down);
        }

        public void MIL_Close_Uper()
        {
            MIL_Status_Uper = "Close";
            MIL.MdigHalt(MilDigitizer_Uper);
            MIL.MbufFree(MilImage_Uper);         //이미지 버퍼
            MIL.MdispFree(MilDisplay_Uper);      // 모니터링
            MIL.MdigFree(MilDigitizer_Uper);      // 프레임그래버
            MIL.MsysFree(MilSystem_Uper);    // 그래버 추가
            //MIL.MappFree(MilApplication);   //
        }

        public void MIL_Close_Down()
        {
            MIL_Status_Down = "Close";
            MIL.MdigHalt(MilDigitizer_Down);
            MIL.MbufFree(MilImage_Down);         //이미지 버퍼
            MIL.MdispFree(MilDisplay_Down);      // 모니터링
            MIL.MdigFree(MilDigitizer_Down);      // 프레임그래버
            MIL.MsysFree(MilSystem_Down);    // 그래버 추가
            //MIL.MappFree(MilApplication);   //
        }

        public void MIL_Initionalize_Uper()
        {
            MilApplication = MIL.M_NULL;
            MilSystem_Uper = MIL.M_NULL;
            MilDisplay_Uper = MIL.M_NULL;
            MilDigitizer_Uper = MIL.M_NULL;
            MilImage_Uper = MIL.M_NULL;
        }

        public void MIL_Initionalize_Down()
        {
            MilApplication = MIL.M_NULL;                // Application identifier.
            MilSystem_Down = MIL.M_NULL;                     // System identifier.
            MilDisplay_Down = MIL.M_NULL;             // Display identifier.
            MilDigitizer_Down = MIL.M_NULL;           // Digitizer identifier.
            MilImage_Down = MIL.M_NULL;               // Image buffer identifier.
        }

        private delegate void Delegate_Image_Grab_MIL_Down();
        private delegate void Delegate_Image_Grab_MIL_Uper();
        public void MainDlg_Image_Grab_MIL_Uper()
        {
            if (InvokeRequired)
            {
                Delegate_Image_Grab_MIL_Uper del = MainDlg_Image_Grab_MIL_Uper;
                Invoke(del);
            }
            else
            {
                MilApplication = MIL.M_NULL;
                MilSystem_Uper = MIL.M_NULL;
                MilDisplay_Uper = MIL.M_NULL;
                MilDigitizer_Uper = MIL.M_NULL;
                MilImage_Uper = MIL.M_NULL;

                MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication); // Application 할당
                MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 0, MIL.M_DEFAULT, ref MilSystem_Uper); // 프레임그레버 할당
                MIL.MdigAlloc(MilSystem_Uper, MIL.M_DEV0, @"C:\Visionsystem\Data\solfcl_mil9_CSC12M25BMP19_4tap_8bit_C0.dcf", MIL.M_DEFAULT, ref MilDigitizer_Uper);
                MIL.MdispAlloc(MilSystem_Uper, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Uper);
                MIL.MbufAlloc2d(MilSystem_Uper, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref MilImage_Uper);
                MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기
                
                MIL.MdigGrab(MilDigitizer_Uper, MilImage_Uper);
//                 Stopwatch grabwatch = new Stopwatch();
//                 grabwatch.Reset();
//                 grabwatch.Start();
                MIL.MbufGet2d(MilImage_Uper, 0, 0, 4096, 3072, imgBuf_Uper);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Uper, 0);
                matImg_Uper.Data = bufPtr;
                monoIplImage_Uper = Cv.GetImage(matImg_Uper);
                Cv.CvtColor(monoIplImage_Uper, srcIplImage_Uper, ColorConversion.GrayToBgr);
//                 grabwatch.Stop();
//                 MessageBox.Show(grabwatch.ElapsedMilliseconds.ToString("0"));
                MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MappFreeDefault(MilApplication, MilSystem_Uper, MilDisplay_Uper, MilDigitizer_Uper, MilImage_Uper);
            }
        }

        public void MainDlg_Image_Grab_MIL_Down()
        {
            if (InvokeRequired)
            {
                Delegate_Image_Grab_MIL_Down del = MainDlg_Image_Grab_MIL_Down;
                Invoke(del);
            }
            else
            {
                MilApplication = MIL.M_NULL;
                MilSystem_Uper = MIL.M_NULL;
                MilDisplay_Down = MIL.M_NULL;
                MilDigitizer_Down = MIL.M_NULL;
                MilImage_Down = MIL.M_NULL;

                MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication); // Application 할당
                MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 1, MIL.M_DEFAULT, ref MilSystem_Down); // 프레임그레버 할당
                MIL.MdigAlloc(MilSystem_Down, MIL.M_DEV0, @"C:\Visionsystem\Data\solfcl_mil9_CSC12M25BMP19_4tap_8bit_C1.dcf", MIL.M_DEFAULT, ref MilDigitizer_Down);
                MIL.MdispAlloc(MilSystem_Down, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Down);
                MIL.MbufAlloc2d(MilSystem_Down, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref MilImage_Down);
                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기

                MIL.MdigGrab(MilDigitizer_Down, MilImage_Down);
                MIL.MbufGet2d(MilImage_Down, 0, 0, 4096, 3072, imgBuf_Down);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Down, 0);
                matImg_Down.Data = bufPtr;
                monoIplImage_Down = Cv.GetImage(matImg_Down);
                Cv.CvtColor(monoIplImage_Down, srcIplImage_Down, ColorConversion.GrayToBgr);
                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MappFreeDefault(MilApplication, MilSystem_Down, MilDisplay_Down, MilDigitizer_Down, MilImage_Down);
            }
        }
        IplImage monoIplImage_Uper = new IplImage(4096, 3072, BitDepth.U8, 1);
        IplImage monoIplImage_Down = new IplImage(4096, 3072, BitDepth.U8, 1);
        public void MainDlg_Image_Grab_MIL_BiCell()
        {
            /*
            if (LamiSystem.GetSet_Grab_Auto_Flag_BiCell == true)
            {
                LamiSystem.GetSet_Grab_Auto_Flag_BiCell = false;
                MIL.MdigControl(MilDigitizer_BiCell, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MappFreeDefault(MilApplication, MilSystem, MilDisplay_BiCell, MilDigitizer_BiCell, MilImage_BiCell);
                MIL_Initionalize_BiCell();
                MIL_Open_BiCell();
                Thread.Sleep(100);
            }
            else
            {
                MIL_Open_BiCell();
            }

            //umac.Umac_SetData_P351("13");

            MIL.MdigGrab(MilDigitizer_BiCell, MilImage_BiCell);
            
            MIL.MbufGet2d(MilImage_BiCell, 0, 0, _iGrabImageGaro_BiCell, _iGrabImageSero_BiCell, imgBuf_BiCell);
            IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_BiCell, 0);
            matImg_BiCell.Data = bufPtr;

            monoIplImage_BiCell = Cv.GetImage(matImg_BiCell);
            Cv.CvtColor(monoIplImage_BiCell, srcIplImage_BiCell, ColorConversion.GrayToBgr);

            MIL_Close_BiCell();

            if (utabDlgMain.ActiveTab.Key == "VisionBiCell")
            {
                VisionBiCell_Config_IplBox1.ImageIpl = srcIplImage_BiCell;
                VisionBiCell_Config_IplBox1.Refresh();
            }
            else if (utabDlgMain.ActiveTab.Key == "RecipeBiCell")
            {
                RecipeBiCell_drawArea1.pictureBox1.ImageIpl = srcIplImage_BiCell;
                RecipeBiCell_drawArea1.pictureBox1.Refresh();
            }
            GrayImage_View_BiCell(srcIplImage_BiCell);
            */

            //private delegate void Delegate_Run_Run_Threading2_Display(TextBox tBox, int dataNo);
            if (InvokeRequired)
            {
                Delegate_Image_Grab_MIL_Down del = MainDlg_Image_Grab_MIL_BiCell;
                Invoke(del);
            }
            else
            {
                MilApplication = MIL.M_NULL;
                MilSystem_Uper = MIL.M_NULL;
                MilDisplay_Down = MIL.M_NULL;
                MilDigitizer_Down = MIL.M_NULL;
                MilImage_Down = MIL.M_NULL;

                MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication); // Application 할당
                MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, MIL.M_DEFAULT, MIL.M_DEFAULT, ref MilSystem_Uper); // 프레임그레버 할당
                MIL.MdigAlloc(MilSystem_Uper, MIL.M_DEV1, @"C:\Visionsystem\Data\solxcl_mil9_G60FV11CL_c_8bit_2tap_P2.dcf", MIL.M_DEFAULT, ref MilDigitizer_Down);
                MIL.MdispAlloc(MilSystem_Uper, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Down);
                MIL.MbufAlloc2d(MilSystem_Uper, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref MilImage_Down);
                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기
                MIL.MdigGrab(MilDigitizer_Down, MilImage_Down);

                MIL.MbufGet2d(MilImage_Down, 0, 0, 4096, 3072, imgBuf_Down);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Down, 0);
                matImg_Down.Data = bufPtr;
                monoIplImage_Down = Cv.GetImage(matImg_Down);
                Cv.CvtColor(monoIplImage_Down, srcIplImage_Down, ColorConversion.GrayToBgr);

                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MappFreeDefault(MilApplication, MilSystem_Uper, MilDisplay_Down, MilDigitizer_Down, MilImage_Down);
            }
        }


        public void GrayImage_View_Gap(IplImage viewImage)
        {
            string tmpstr = LamiSystem.StrListSysConTitle[33];
            int _iEdgeParam1 = int.Parse(LamiSystem.StrListSysConData[11]);
            int _iEdgeParam2 = int.Parse(LamiSystem.StrListSysConData[12]);
            int _iEdgeParam3 = int.Parse(LamiSystem.StrListSysConData[13]);

            IplImage srcImgStd = viewImage.Clone();
            IplImage srcImgGray = new IplImage(viewImage.Size, BitDepth.U8, 1);
            Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);
            Cv.Canny(srcImgGray, srcImgGray, _iEdgeParam1, _iEdgeParam2, ApertureSize.Size3);

            //CvWindow.ShowImages(srcImgGray);
        }

        public void GrayImage_View_BiCell(IplImage viewImage)
        {
            int _iEdgeParam1 = int.Parse(LamiSystem.StrListSysConData[30]);
            int _iEdgeParam2 = int.Parse(LamiSystem.StrListSysConData[31]);
            int _iEdgeParam3 = int.Parse(LamiSystem.StrListSysConData[32]);

            IplImage srcImgStd = viewImage.Clone();
            IplImage srcImgGray = new IplImage(viewImage.Size, BitDepth.U8, 1);
            Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);
            Cv.Canny(srcImgGray, srcImgGray, _iEdgeParam1, _iEdgeParam2, ApertureSize.Size3);

            //CvWindow.ShowImages(srcImgGray);
        }

        private void FormDlgMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Initional_Measure_Buffer_Reg();
            //PLC 연결 확인 토글 플래그 전송
            //if (LamiSystem.IsConnect_PLC == true) plc.Loop_Check_Flag = false;
            //Thread.Sleep(2000);
            // 밀 리소스 해제
            if (LamiSystem.GetSet_Grab_Auto_Flag_Gap == true)
            {
                LamiSystem.GetSet_Grab_Auto_Flag_Gap = false;
                MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
            }
            //MIL.MappFreeDefault(MilApplication, MilSystem, MilDisplay_Gap, MilDigitizer_Gap, MilImage_Gap);
            if (MIL_Status_Uper == "Open") MIL_Close_Uper();

            if (LamiSystem.GetSet_Grab_Auto_Flag_Down == true)
            {
                LamiSystem.GetSet_Grab_Auto_Flag_Down = false;
                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
            }
            if (MIL_Status_Down == "Open") MIL_Close_Down();

            /* Inspect_MIL_Close_System();*/
        }

        /*
        private void VisionBiCell_uBtn_ImageGrab_Click(object sender, EventArgs e)
        {
            MainDlg_Image_Grab_MIL_BiCell();
            VisionBiCell_Config_IplBox1.ImageIpl = srcIplImage_BiCell;
        }

        private void RecipeBiCell_uBtn_ImageGrab_Click(object sender, EventArgs e)
        {
            MainDlg_Image_Grab_MIL_BiCell();
            RecipeBiCell_drawArea1.pictureBox1.ImageIpl = srcIplImage_BiCell;
        }
        */

        private Control_Inspect_Gap inspection_Gap = new Control_Inspect_Gap();

        private void Recipe_uBtn_GrabNone_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //2015-01-05-01 wkb
            RecipeGrid_Data_Saving();

            if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false)
            {
                if (utabDlgMain.Tabs["Recipe"].Visible == true) 
                    this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Recipe"];
                return;
            }

            dlgInspect.GetSet_LamiSystem = LamiSystem;
            dlgInspect.GetSet_Converter_Uper = _point_Converter_Uper;
            dlgInspect.GetSet_Converter_Down = _point_Converter_Down;

            dlgInspect.Inspect_Ready_Ready();

            //시스템의 설정 값을 적용한는 함수.
            dlgInspect.Inspect_Ready_Run_System_Data_Load();

            //RectListImageZone 리스트를 이용하는 함수
            dlgInspect.Inspect_Ready_Run_RecipeGrid_Data_Load_Uper();
            dlgInspect.Inspect_Ready_Run_RecipeGrid_Data_Load_Down();

            dlgInspect.Inspect_Offset_Load_To_System();

            bool ROI_Search_Result_Uper = false;
            bool ROI_Search_Result_Down = false;

            //Recipe_uBtn_ImageRead.PerformClick();

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    //srcIplImage_Uper = RecipeGap_drawArea1.pictureBox1.ImageIpl;
                    srcIplImage_Uper = IplImage.FromBitmap((Bitmap)RecipeGap_drawArea1.pictureBox1.Image);
                    break;

                case "DownRecipe":
                    //srcIplImage_Down = RecipeGap_drawArea2.pictureBox1.ImageIpl;
                    srcIplImage_Down = IplImage.FromBitmap((Bitmap)RecipeGap_drawArea2.pictureBox1.Image);
                    break;
            }

            
            //이미지를 다시 분석해서 측정값을 찾아내는 함수
            //Inspect_getHistory_Image_Uper();

            //Graphics gc = Inspect_Main01_IplBox.CreateGraphics();
            //Run_Mode = "Manual";
            //SrcIplImage_Uper = Inspect_Main01_IplBox.ImageIpl;
            //Inspect_Manual_Image_Grabing();
            
            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    //CvWindow.ShowImages(srcIplImage_Uper);
                    ROI_Search_Result_Uper = dlgInspect.Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(srcIplImage_Uper);
                    dlgInspect._CycleCompleteFlag_Uper = true;
                    dlgInspect.Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
                    dlgInspect.Inspect_Manual_FindData_Inspection_Uper();
                    List<string> resultArray_Uper = dlgInspect.GetSet_GridDisplayData_Uper;
                    Manual_Inspect_ResultData_To_Viewer_Grid_Uper(resultArray_Uper);
                    break;

                case "DownRecipe":
                    ROI_Search_Result_Down = dlgInspect.Inspect_Run_Run_ROI_EdgeLine_Centering_Down(srcIplImage_Down);
                    dlgInspect._CycleCompleteFlag_Down = true;
                    dlgInspect.Inspect_Run_Run_ROI_CenterPoint_Find_Down();
                    dlgInspect.Inspect_Manual_FindData_Inspection_Down();
                    List<string> resultArray_Down = dlgInspect.GetSet_GridDisplayData_Down;
                    Manual_Inspect_ResultData_To_Viewer_Grid_Down(resultArray_Down);
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        /*
        private void Recipe_uBtn_GrabNone_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //2015-01-05-01 wkb
            RecipeGrid_Data_Saving();

            if (Recipe_CheckedName_Result_Uper == false || Recipe_CheckedGraph_Result_Uper == false
                || Recipe_CheckedName_Result_Down == false || Recipe_CheckedGraph_Result_Down == false)
            {
                if (utabDlgMain.Tabs["Recipe"].Visible == true)
                    this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["Recipe"];
                return;
            }

            dlgInspect.GetSet_LamiSystem = LamiSystem;
            dlgInspect.GetSet_Converter_Uper = _point_Converter_Uper;
            dlgInspect.GetSet_Converter_Down = _point_Converter_Down;

            dlgInspect.Inspect_Ready_Ready();

            //시스템의 설정 값을 적용한는 함수.
            dlgInspect.Inspect_Ready_Run_System_Data_Load();

            //RectListImageZone 리스트를 이용하는 함수
            dlgInspect.Inspect_Ready_Run_RecipeGrid_Data_Load_Uper();
            dlgInspect.Inspect_Ready_Run_RecipeGrid_Data_Load_Down();

            dlgInspect.Inspect_Offset_Load_To_System();

            bool ROI_Search_Result_Uper = false;
            bool ROI_Search_Result_Down = false;

#if (SYST_SIMUL)
            Recipe_uBtn_ImageRead.PerformClick();

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    //srcIplImage_Uper = RecipeGap_drawArea1.pictureBox1.ImageIpl;
                    srcIplImage_Uper = IplImage.FromBitmap((Bitmap) RecipeGap_drawArea1.pictureBox1.Image);
                    break;

                case "DownRecipe":
                    //srcIplImage_Down = RecipeGap_drawArea2.pictureBox1.ImageIpl;
                    srcIplImage_Down = IplImage.FromBitmap((Bitmap) RecipeGap_drawArea2.pictureBox1.Image);
                    break;
            }
#else
            Recipe_uBtn_Tab_Meas.PerformClick();
            Thread.Sleep(100);
#endif
            //이미지를 다시 분석해서 측정값을 찾아내는 함수
            //Inspect_getHistory_Image_Uper();
            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    //CvWindow.ShowImages(srcIplImage_Uper);
                    ROI_Search_Result_Uper = dlgInspect.Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(srcIplImage_Uper);
                    dlgInspect._CycleCompleteFlag_Uper = true;
                    dlgInspect.Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
                    dlgInspect.Inspect_Manual_FindData_Inspection_Uper();
                    List<string> resultArray_Uper = dlgInspect.GetSet_GridDisplayData_Uper;
                    Manual_Inspect_ResultData_To_Viewer_Grid_Uper(resultArray_Uper);
                    break;

                case "DownRecipe":
                    ROI_Search_Result_Down = dlgInspect.Inspect_Run_Run_ROI_EdgeLine_Centering_Down(srcIplImage_Down);
                    dlgInspect._CycleCompleteFlag_Down = true;
                    dlgInspect.Inspect_Run_Run_ROI_CenterPoint_Find_Down();
                    dlgInspect.Inspect_Manual_FindData_Inspection_Down();
                    List<string> resultArray_Down = dlgInspect.GetSet_GridDisplayData_Down;
                    Manual_Inspect_ResultData_To_Viewer_Grid_Down(resultArray_Down);
                    break;
            }
            
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */

    

        /// _alignSystem._strListVisionConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void Manual_Inspect_ResultData_To_Viewer_Grid_Uper(List<string> resultArray)
        {
            uDS_Test_Uper.Rows.Clear();
            int grdRowsNum = 7;
            for (int i = 0; i < resultArray.Count; i++)
            {
                if (i % grdRowsNum == 0) uDS_Test_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "" });
                uGrd_Recipe_Test_Uper.DisplayLayout.Rows[i / grdRowsNum].Cells[i % grdRowsNum].Value = resultArray[i];
            }
            Recipe_Manual_Grid_Uper_Resize();
        }

        private void Manual_Inspect_ResultData_To_Viewer_Grid_Down(List<string> resultArray)
        {
            uDS_Test_Down.Rows.Clear();
            int grdRowsNum = 7;
            for (int i = 0; i < resultArray.Count; i++)
            {
                if (i % grdRowsNum == 0) uDS_Test_Down.Rows.Add(true, new Object[] { "", "", "", "", "", "", "" });
                uGrd_Recipe_Test_Down.DisplayLayout.Rows[i / grdRowsNum].Cells[i % grdRowsNum].Value = resultArray[i];
            }
            Recipe_Manual_Grid_Down_Resize();
        }


        public void Recipe_Manual_Grid_Uper_Resize()
        {
            for (int i = 0; i < uGrd_Recipe_Test_Uper.Rows.Count; i++)
            {
                string CheckResult = uGrd_Recipe_Test_Uper.DisplayLayout.Rows[i].Cells[6].Value.ToString();
                if (CheckResult == "NG")
                    uGrd_Recipe_Test_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
            }

            if (uGrd_Recipe_Test_Uper.Rows.Count > 8)
            {
                if (uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[0].Width == 40) return;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[0].Width = 40;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[1].Width = 60;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[2].Width = 53;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[3].Width = 53;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[4].Width = 53;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[5].Width = 53;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[6].Width = 51;
            }
            else
            {
                if (uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[0].Width == 42) return;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[0].Width = 42;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[1].Width = 68;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[2].Width = 55;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[3].Width = 55;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[4].Width = 55;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[5].Width = 55;
                uGrd_Recipe_Test_Uper.DisplayLayout.Bands[0].Columns[6].Width = 51;
                if (uGrd_Recipe_Test_Uper.Rows.Count > 0)
                    uGrd_Recipe_Test_Uper.ActiveRow = uGrd_Recipe_Test_Uper.Rows[0];
            }
        }


        public void Recipe_Manual_Grid_Down_Resize()
        {
            for (int i = 0; i < uGrd_Recipe_Test_Down.Rows.Count; i++)
            {
                string CheckResult = uGrd_Recipe_Test_Down.DisplayLayout.Rows[i].Cells[6].Value.ToString();
                if (CheckResult == "NG") uGrd_Recipe_Test_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
            }

            if (uGrd_Recipe_Test_Down.Rows.Count > 8)
            {
                if (uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[0].Width == 40) return;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[0].Width = 40;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[1].Width = 60;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[2].Width = 53;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[3].Width = 53;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[4].Width = 53;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[5].Width = 53;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[6].Width = 51;
            }
            else
            {
                if (uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[0].Width == 42) return;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[0].Width = 42;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[1].Width = 68;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[2].Width = 55;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[3].Width = 55;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[4].Width = 55;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[5].Width = 55;
                uGrd_Recipe_Test_Down.DisplayLayout.Bands[0].Columns[6].Width = 51;
                if (uGrd_Recipe_Test_Down.Rows.Count > 0)
                    uGrd_Recipe_Test_Down.ActiveRow = uGrd_Recipe_Test_Down.Rows[0];
            }
        }

        /*
        private void RecipeBiCell_uBtn_ImageRead_Click(object sender, EventArgs e)
        {
            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strFullPathFile = pFileDlg.FileName;
                RecipeBiCell_drawArea1.pictureBox1.ImageIpl = IplImage.FromFile(strFullPathFile);
            }
        }

        private void VisionBiCell_uBtn_ImageRead_Click(object sender, EventArgs e)
        {
            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                //Filter = "이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strFullPathFile = pFileDlg.FileName;
                VisionBiCell_Config_IplBox1.ImageIpl = IplImage.FromFile(strFullPathFile);
            }
        }

        private void VisionBiCell_uBtn_ImageSave_Click(object sender, EventArgs e)
        {
            Image_File_Save(VisionBiCell_Config_IplBox1.ImageIpl);
        }

        private void RecipeBiCell_uBtn_ImageSave_Click(object sender, EventArgs e)
        {
            Image_File_Save(RecipeBiCell_drawArea1.pictureBox1.ImageIpl);
        }
        */

        private void RecipeBiCell_uBtn_ManualTest_Click(object sender, EventArgs e)
        {
            /*
            Graphics gc = RecipeBiCell_drawArea1.pictureBox1.CreateGraphics();

            //Control_Inspect_BiCell inspection_BiCell = new Control_Inspect_BiCell(LamiSystem, _point_Converter_Gap, fileSystem, gc, RecipeBiCell_drawArea1.pictureBox1, RecipeBiCell_drawArea1.pictureBox1.ImageIpl, plc, umac);
            Control_Inspect_BiCell inspection_BiCell = new Control_Inspect_BiCell();
            inspection_BiCell.GetSet_GapSystem = LamiSystem;
            inspection_BiCell.GetSet_Converter = _point_Converter_BiCell;
            inspection_BiCell.GetSet_FileSystem = fileSystem;
            inspection_BiCell.GetSet_Graphics = gc;
            inspection_BiCell.GetSet_ImageBoxIpl = RecipeBiCell_drawArea1.pictureBox1;
            inspection_BiCell.GetSet_NowImage = RecipeBiCell_drawArea1.pictureBox1.ImageIpl;
            inspection_BiCell.GetSet_PLCSystem = plc;
            inspection_BiCell.GetSet_UMACSystem = umac;
            inspection_BiCell.GetSet_Calling_Form = this.Name;
            inspection_BiCell.Inspect_Manual_Image_Grabing();

            List<string> resultArray = inspection_BiCell.GridDisplayData;
            Manual_Inspect_ResultData_To_Viewer_Grid_BiCell(resultArray);
            */
        }

        /*
        /// _alignSystem._strListVisionConfigGridData 리스트 배열의 값을 읽어와서 
        /// 저장되어져 있는 데이터에 해당하는 콘트롤 값을 설정해준다.
        private void Manual_Inspect_ResultData_To_Viewer_Grid_BiCell(List<string> resultArray)
        {
            uDS_Test_BiCell.Rows.Add(true, new Object[] { "", "", "", "", "", "", "" });

            for (int i = 0; i < resultArray.Count; i++)
            {
                uGrd_Recipe_Test_BiCell.DisplayLayout.Rows[uDS_Test_BiCell.Rows.Count - 1].Cells[i].Value = resultArray[i];
            }
        }
        */


        private delegate void Delegate_myEvent_Inspect_Gap_01(int gapNo, uint productNo);

        private delegate void Delegate_myEvent_Inspect_Gap_02(int gapNo, uint productNo);

        /*
        public void myEvent_Inspect_Gap_01(int GapNo, uint ProductNo)
        {
            if (InvokeRequired)
            {
                Delegate_myEvent_Inspect_Gap_01 del = myEvent_Inspect_Gap_01;
                Invoke(del, GapNo, ProductNo);
            }
            else
            {
//                 uTxt_Gap_No.Text = GapNo.ToString();
//                 uTxt_Gap_No.Refresh();
// 
// 
//                 NowProdectNumber_Gap =
//                     uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "ProductNoGap"));
//                 //Inspect_uLabel_Assy05.Text = ProductNo.ToString() + " / " + NowProdectNumber_Gap.ToString();
//                 Inspect_uLabel_Assy05.Text = NowProdectNumber_Gap.ToString();
//                 Inspect_uLabel_Assy05.Refresh();
// 
//                 Inspect_uLabel_Assy06.Text = NowFailNumber_Gap.ToString("0") + " / " + NowTrigNumber_Gap;
//                 Inspect_uLabel_Assy06.Refresh();
// 
//                 Inspect_uLabel_Assy07.Text = GapNo.ToString();
//                 Inspect_uLabel_Assy07.Refresh();
            }
        }

        public void myEvent_Inspect_Gap_02(int GapNo, uint ProductNo)
        {
            if (InvokeRequired)
            {
                Delegate_myEvent_Inspect_Gap_02 del = myEvent_Inspect_Gap_02;
                Invoke(del, GapNo, ProductNo);
            }
            else
            {
//                 Inspect_Run_Run_Make_Fail_Check();
// 
//                 uTxt_Gap_No.Text = GapNo.ToString();
//                 uTxt_Gap_No.Refresh();
// 
//                 NowProdectNumber_Gap =
//                     uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "ProductNoGap"));
// 
//                 //Inspect_uLabel_Assy05.Text = ProductNo.ToString() + " / " + NowProdectNumber_Gap.ToString();
//                 Inspect_uLabel_Assy05.Text = NowProdectNumber_Gap.ToString();
//                 Inspect_uLabel_Assy05.Refresh();
// 
//                 Inspect_uLabel_Assy06.Text = NowFailNumber_Gap.ToString("0") + " / " + NowTrigNumber_Gap;
//                 Inspect_uLabel_Assy06.Refresh();
// 
//                 Inspect_uLabel_Assy07.Text = GapNo.ToString();
//                 Inspect_uLabel_Assy07.Refresh();
            }
        }
        */

        //갭비전 비전부 설정
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //utabDlgMain.Tabs["System"].Visible = false;
            //utabDlgMain.Tabs["VisionGap"].Visible = false;
            //utabDlgMain.Tabs["VisionBiCell"].Visible = false;
            //utabDlgMain.Tabs["RecipeGap"].Visible = false;
            //utabDlgMain.Tabs["RecipeBiCell"].Visible = false;
            //utabDlgMain.Tabs["Config"].Visible = false;
            //utabDlgMain.Tabs["Camera"].Visible = false;
            //utabDlgMain.Tabs["LogData"].Visible = true;
            if (utabDlgMain.Tabs["VisionGap"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["VisionGap"];
        }

        //시스템 설정
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["System"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["System"];
        }

        //갭비전 레시피 설정
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["RecipeGap"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["RecipeGap"];
        }

        //바이셀 비전부 설정
        private void 바이셀비전부설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["VisionBiCell"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["VisionBiCell"];
        }

        //바이셀 레시피 설정 
        private void 바이셀레시피설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["RecipeBiCell"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["RecipeBiCell"];
        }

        //보기 시스템 로그
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (utabDlgMain.Tabs["LogData"].Visible == true)
                this.utabDlgMain.SelectedTab = this.utabDlgMain.Tabs["LogData"];
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ubtnToolbarOpen.PerformClick();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ubtnToolbarSave.PerformClick();
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            ubtnToolbarUser.PerformClick();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            ubtnToolbarInspect.PerformClick();
        }

        private void 제품문의ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uMessageBox.DlgMain_Manu_Message(2);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ubtnToolbarExit.PerformClick();
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //Vision_Config_IplBox1
            if (ultraTabControl1.ActiveTab == null) return;
            if (ultraTabControl1.ActiveTab.Key == null) return;

            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    Vision_Config_IplBox1.Visible = true;
                    Vision_Config_IplBox2.Visible = false;
                    break;
                case "RecipeDown":
                    Vision_Config_IplBox1.Visible = false;
                    Vision_Config_IplBox2.Visible = true;
                    break;
            }
        }

        private void uGrd_Recipe_UperData_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Index == 0)
            {
                Recipe_Config_Display_Select_UperGrid_Row(e);
            }
            else if (e.Cell.Column.Index == 2)
            {
                Recipe_Config_Display_Select_UperGrid_Seq(e, 2);
            }
            else if (e.Cell.Column.Index == 5)
            {
                Recipe_Config_Display_Select_UperGrid_Seq(e, 5);
            }

            return;
        }

        private void uGrd_Recipe_DownData_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Index == 0)
            {
                Recipe_Config_Display_Select_DownGrid_Row(e);
            }
            else if (e.Cell.Column.Index == 2)
            {
                Recipe_Config_Display_Select_DownGrid_Seq(e, 2);
            }
            else if (e.Cell.Column.Index == 5)
            {
                Recipe_Config_Display_Select_DownGrid_Seq(e, 5);
            }

            return;
        }

        private void ultraTabControl2_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (ultraTabControl2.ActiveTab == null) return;
            if (ultraTabControl2.ActiveTab.Key == null) return;

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    //RecipeGap_drawArea1.pictureBox1.ImageIpl = IplImage.FromBitmap(Properties.Resources.BiCell_Top);
                    RecipeGap_drawArea1.pictureBox1.Image = Properties.Resources.BiCell_Top;
                    RecipeGap_drawArea1.Visible = true;
                    RecipeGap_drawArea2.Visible = false;
                    VisionLami_Config_ListData_To_UperGrid();
                    break;
                case "DownRecipe":
                    RecipeGap_drawArea1.Visible = false;
                    RecipeGap_drawArea2.Visible = true;
                    //RecipeGap_drawArea2.pictureBox1.ImageIpl = IplImage.FromBitmap(Properties.Resources.BiCell_Bot);
                    RecipeGap_drawArea2.pictureBox1.Image = Properties.Resources.BiCell_Bot;
                    VisionLami_Config_ListData_To_DownGrid();
                    break;
            }
        }

        private void Vision_uGrd_Uper_MouseDown(object sender, MouseEventArgs e)
        {
            UltraGridRow row;
            UIElement element;
            element = Vision_uGrd_Uper.DisplayLayout.UIElement.ElementFromPoint(e.Location);
            row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
            
            if (row != null && row.IsDataRow)
            {
                _iVisionUperGridRowNo = row.Index;
                Vision_uGrd_Uper.Rows[_iVisionUperGridRowNo].Appearance.BackColor = Color.Silver;
            }
        }

        private void Vision_uBtn_01_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            switch (this.ultraTabControl1.ActiveTab.Key)
            {
                case "RecipeUper":
                    //20150304 WKB 209
                    VisionEditStatus_Desiable(0);
                    
                    Vision_Uper_DataSource_Insert();
                    Vision_Uper_Grid_Resize();

                    //20150304 WKB 209
                    Vision_Uper_Insert_Edit();
                    break;
                case "RecipeDown":
                    //20150304 WKB 209
                    VisionEditStatus_Desiable(1);

                    Vision_Down_DataSource_Insert();
                    Vision_Down_Grid_Resize();

                    //20150304 WKB 209
                    Vision_Down_Insert_Edit();
                    break;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void Vision_Uper_Insert_Edit()
        {
            uDS_Offset_Uper.Rows[uDS_Offset_Uper.Rows.Count - 1].SetCellValue(0, "");
        }

        private void Vision_Down_Insert_Edit()
        {
            uDS_Offset_Down.Rows[uDS_Offset_Down.Rows.Count - 1].SetCellValue(0, "");
        }



        private void VisionEditStatus_Desiable(int NowTabIndex)
        {
            ubtnToolbarInspect.Enabled = false;
            ubtnToolbarUser.Enabled = false;
            ubtnToolbarOpen.Enabled = false;
            ubtnToolbarSave.Enabled = false;
            //ubtnToolbarExit.Enabled = false;

            VisionGap_uBtn_GrabNone.Enabled = false;

            for (int i = 0; i < utabDlgMain.Tabs.Count; i++)
            {
                utabDlgMain.Tabs[i].Enabled = false;
            }

            for (int i = 0; i < ultraTabControl1.Tabs.Count; i++)
            {
                ultraTabControl1.Tabs[i].Enabled = false;
            }

            utabDlgMain.Tabs["Vision"].Enabled = true;

            if (NowTabIndex == 0)
            {
                ultraTabControl1.Tabs[0].Enabled = true;
            }
            else if (NowTabIndex == 1)
            {
                ultraTabControl1.Tabs[1].Enabled = true;
            }
        }

        private void VisionDownStatus_Enable()
        {
            ubtnToolbarInspect.Enabled = true;
            ubtnToolbarUser.Enabled = true;
            ubtnToolbarOpen.Enabled = true;
            ubtnToolbarSave.Enabled = true;
            ubtnToolbarExit.Enabled = true;

            VisionGap_uBtn_GrabNone.Enabled = true;

            for (int i = 0; i < utabDlgMain.Tabs.Count; i++)
            {
                utabDlgMain.Tabs[i].Enabled = true;
            }

            for (int i = 0; i < ultraTabControl1.Tabs.Count; i++)
            {
                ultraTabControl1.Tabs[i].Enabled = true;
            }
        }

        private void Vision_uGrd_Down_MouseDown(object sender, MouseEventArgs e)
        {
            UltraGridRow row;
            UIElement element;
            element = Vision_uGrd_Down.DisplayLayout.UIElement.ElementFromPoint(e.Location);
            row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;

            if (row != null && row.IsDataRow)
            {
                _iVisionDownGridRowNo = row.Index;
                Vision_uGrd_Down.Rows[_iVisionDownGridRowNo].Appearance.BackColor = Color.Silver;
            }
        }

        /*
        //2015.02.12 WKB 207
        private void Vision_uGrd_Uper_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                //int ClickRowsNo = e.Cell.Row.Index;
                //double Value1 = double.Parse(Vision_uGrd_Uper.DisplayLayout.Rows[ClickRowsNo].Cells[5].Value.ToString());
                //double Value2 = double.Parse(Vision_uGrd_Uper.DisplayLayout.Rows[ClickRowsNo].Cells[6].Value.ToString());
                
                
                //double CalValue = Value1 / Value2;
                //Vision_uGrd_Uper.DisplayLayout.Rows[ClickRowsNo].Cells[7].Value = CalValue.ToString("0.0000");

                //2015.02.12 WKB 208
                //double OldValue = Value1/uDS_Inspect_Uper.Rows[ClickRowsNo].GetCellValue(7);
                int ClickRowsNo = e.Cell.Row.Index;
                bool ParseResult = false;

                double MeasValue = 0;
                ParseResult = double.TryParse(uDS_Offset_Uper.Rows[ClickRowsNo].GetCellValue(5).ToString(), out MeasValue);
                if (ParseResult == false) return;

                double RealValue = 0;
                ParseResult = double.TryParse(uDS_Offset_Uper.Rows[ClickRowsNo].GetCellValue(6).ToString(), out RealValue);
                if (ParseResult == false) return;

                double OldCal = 0;
                ParseResult = double.TryParse(uDS_Offset_Uper.Rows[ClickRowsNo].GetCellValue(7).ToString(), out OldCal);
                if (ParseResult == false) return;
                
                double BasicValue = MeasValue / OldCal;

                double NewCal = RealValue/BasicValue;
                uDS_Offset_Uper.Rows[ClickRowsNo].SetCellValue(7, NewCal.ToString("0.0000"));
                Vision_uGrd_Uper.DisplayLayout.Rows[ClickRowsNo].Cells[7].Value = NewCal.ToString("0.0000");

                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
         * 
        private void Vision_uGrd_Down_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                int ClickRowsNo = e.Cell.Row.Index;
                double Value1 = double.Parse(Vision_uGrd_Down.DisplayLayout.Rows[ClickRowsNo].Cells[5].Value.ToString());
                double Value2 = double.Parse(Vision_uGrd_Down.DisplayLayout.Rows[ClickRowsNo].Cells[6].Value.ToString());
                double CalValue = Value1 / Value2;
                Vision_uGrd_Down.DisplayLayout.Rows[ClickRowsNo].Cells[7].Value = CalValue.ToString("0.0000");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        */

        public struct ItemCalibration
        {
            public int ClickRowsNo;
            public bool ParseResult;
            public double MeasValue;
            public double RealValue;
            public double OldCal;
            public double BasicValue;
            public double NewCal;
        }
        //2015.02.12 WKB 208
        //가. 측정 / 비율 = A
        //나. 실측 / A = 비율
        //Formula 변경함.
        private void Vision_uGrd_Uper_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                ItemCalibration ICal = new ItemCalibration();

                ICal.ClickRowsNo = e.Cell.Row.Index;
                ICal.ParseResult = false;

                ICal.MeasValue = 0;
                ICal.ParseResult = double.TryParse(uDS_Offset_Uper.Rows[ICal.ClickRowsNo].GetCellValue(5).ToString(), out ICal.MeasValue);
                if (ICal.ParseResult == false) return;

                ICal.RealValue = 0;
                ICal.ParseResult = double.TryParse(uDS_Offset_Uper.Rows[ICal.ClickRowsNo].GetCellValue(6).ToString(), out ICal.RealValue);
                if (ICal.ParseResult == false) return;

                ICal.OldCal = 0;
                ICal.ParseResult = double.TryParse(uDS_Offset_Uper.Rows[ICal.ClickRowsNo].GetCellValue(7).ToString(), out ICal.OldCal);
                if (ICal.ParseResult == false) return;


                //20150325 WKB 209
                //일반적인 켈 값 추출로 변경 요청으로 변경함.
                //ICal.BasicValue = ICal.MeasValue / ICal.OldCal;
                //ICal.NewCal = ICal.RealValue / ICal.BasicValue;

                ICal.NewCal = ICal.RealValue / ICal.MeasValue;

                //uDS_Offset_Uper.Rows[ICal.ClickRowsNo].SetCellValue(7, ICal.NewCal.ToString("0.0000"));
                Vision_uGrd_Uper.DisplayLayout.Rows[ICal.ClickRowsNo].Cells[7].Value = ICal.NewCal.ToString("0.0000");

                return;
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message);
            }
        }
        
        private void Vision_uGrd_Down_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                ItemCalibration ICal = new ItemCalibration();

                ICal.ClickRowsNo = e.Cell.Row.Index;
                ICal.ParseResult = false;

                ICal.MeasValue = 0;
                ICal.ParseResult = double.TryParse(uDS_Offset_Down.Rows[ICal.ClickRowsNo].GetCellValue(5).ToString(), out ICal.MeasValue);
                if (ICal.ParseResult == false) return;

                ICal.RealValue = 0;
                ICal.ParseResult = double.TryParse(uDS_Offset_Down.Rows[ICal.ClickRowsNo].GetCellValue(6).ToString(), out ICal.RealValue);
                if (ICal.ParseResult == false) return;

                ICal.OldCal = 0;
                ICal.ParseResult = double.TryParse(uDS_Offset_Down.Rows[ICal.ClickRowsNo].GetCellValue(7).ToString(), out ICal.OldCal);
                if (ICal.ParseResult == false) return;

                //ICal.BasicValue = ICal.MeasValue / ICal.OldCal;
                //ICal.NewCal = ICal.RealValue / ICal.BasicValue;

                //20150325 WKB 209
                //일반적인 켈 값 추출로 변경 요청으로 변경함.
                //ICal.BasicValue = ICal.MeasValue / ICal.OldCal;
                //ICal.NewCal = ICal.RealValue / ICal.BasicValue;

                ICal.NewCal = ICal.RealValue / ICal.MeasValue;

                //uDS_Offset_Down.Rows[ICal.ClickRowsNo].SetCellValue(7, ICal.NewCal.ToString("0.0000"));
                Vision_uGrd_Down.DisplayLayout.Rows[ICal.ClickRowsNo].Cells[7].Value = ICal.NewCal.ToString("0.0000");

                return;
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message);
            }
        }

        //상부 해상도 가로 설정
        private void ultraButton9_Click(object sender, EventArgs e)
        {
            try
            {
                double Value1 = double.Parse(uTxt_Pix1_01_Lami.Text);
                double Value2 = double.Parse(uTxt_Pix1_02_Lami.Text);
                double CalValue = (Value2 / Value1);
                uTxt_Pix1_03_Lami.Text = CalValue.ToString("0.000000");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //상부 해상도 세로 설정
        private void ultraButton10_Click(object sender, EventArgs e)
        {
            try
            {
                double Value1 = double.Parse(uTxt_Pix1_04_Lami.Text);
                double Value2 = double.Parse(uTxt_Pix1_05_Lami.Text);
                double CalValue = (Value2 / Value1);
                uTxt_Pix1_06_Lami.Text = CalValue.ToString("0.0000");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ultraButton12_Click(object sender, EventArgs e)
        {
            try
            {
                double Value1 = double.Parse(uTxt_Pix2_01_Lami.Text);
                double Value2 = double.Parse(uTxt_Pix2_02_Lami.Text);
                double CalValue = (Value2 / Value1);
                uTxt_Pix2_03_Lami.Text = CalValue.ToString("0.0000");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ultraButton11_Click(object sender, EventArgs e)
        {
            try
            {
                double Value1 = double.Parse(uTxt_Pix2_04_Lami.Text);
                double Value2 = double.Parse(uTxt_Pix2_05_Lami.Text);
                double CalValue = (Value2 / Value1);
                uTxt_Pix2_06_Lami.Text = CalValue.ToString("0.0000");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RecipeEditStatus_Desiable(int NowTabIndex)
        {
            ubtnToolbarInspect.Enabled = false;
            ubtnToolbarUser.Enabled = false;
            ubtnToolbarOpen.Enabled = false;
            ubtnToolbarSave.Enabled = false;
            //ubtnToolbarExit.Enabled = false;

            Recipe_uBtn_GrabNone.Enabled = false;

            for (int i = 0; i < utabDlgMain.Tabs.Count; i++)
            {
                utabDlgMain.Tabs[i].Enabled = false;
            }

            for (int i = 0; i < ultraTabControl2.Tabs.Count; i++)
            {
                ultraTabControl2.Tabs[i].Enabled = false;
            }

            utabDlgMain.Tabs["Recipe"].Enabled = true;
            if (NowTabIndex == 0)
            {
                ultraTabControl2.Tabs["UperRecipe"].Enabled = true;
            }
            else if (NowTabIndex == 1)
            {
                ultraTabControl2.Tabs["DownRecipe"].Enabled = true;
            }
        }

        private void RecipeDownStatus_Enable()
        {
            ubtnToolbarInspect.Enabled = true;
            ubtnToolbarUser.Enabled = true;
            ubtnToolbarOpen.Enabled = true;
            ubtnToolbarSave.Enabled = true;
            ubtnToolbarExit.Enabled = true;

            Recipe_uBtn_GrabNone.Enabled = true;

            for (int i = 0; i < utabDlgMain.Tabs.Count; i++)
            {
                utabDlgMain.Tabs[i].Enabled = true;
            }
            
            for (int i = 0; i < ultraTabControl2.Tabs.Count; i++)
            {
                ultraTabControl2.Tabs[i].Enabled = true;
            }
        }

        private bool RecipeGrid_Changed = false;

        //20150304 WKB 209
        //레시피 항목 추가 버튼
        private void Recipe_uBtn_01_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            RecipeGrid_Changed = true;
            
            

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(0);

                    Recipe_Uper_Insert_Grid();
                    Recipe_Uper_Grid_Resize();
                    Recipe_Config_UperGrid_Output();
                    Recipe_Uper_Insert_ROI();
                    Recipe_Uper_Insert_Inspect();

                    //20150304 WKB 209
                    Recipe_Uper_Insert_Edit();
                    break;
                case "DownRecipe":
                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(1);

                    Recipe_Down_Insert_Grid();
                    Recipe_Down_Grid_Resize();
                    Recipe_Config_DownGrid_Output();
                    Recipe_Down_Insert_ROI();
                    Recipe_Down_Insert_Inspect();

                    //20150304 WKB 209
                    Recipe_Down_Insert_Edit();
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void Recipe_Uper_Insert_Edit()
        {
            uDS_Recipe_Uper.Rows[uDS_Recipe_Uper.Rows.Count-1].SetCellValue(0,"");
            uDS_Recipe_Uper.Rows[uDS_Recipe_Uper.Rows.Count - 1].SetCellValue(1, "0");
        }

        private void Recipe_Down_Insert_Edit()
        {
            uDS_Recipe_Down.Rows[uDS_Recipe_Down.Rows.Count - 1].SetCellValue(0, "");
            uDS_Recipe_Down.Rows[uDS_Recipe_Down.Rows.Count - 1].SetCellValue(1, "0");
        }

        //20150304 WKB 208
        /*
        //레시피 항목 추가 버튼
        private void Recipe_uBtn_01_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            RecipeGrid_Changed = true;
            MainButtonTab_Desiable();

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    Recipe_Uper_Insert_Grid();
                    Recipe_Uper_Grid_Resize();
                    Recipe_Config_UperGrid_Output();
                    Recipe_Uper_Insert_ROI();
                    Recipe_Uper_Insert_Inspect();
                    break;
                case "DownRecipe":
                    Recipe_Down_Insert_Grid();
                    Recipe_Down_Grid_Resize();
                    Recipe_Config_DownGrid_Output();
                    Recipe_Down_Insert_ROI();
                    Recipe_Down_Insert_Inspect();
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */
        private int _iRecipeUperGridRowNo = -1;
        
        public void Recipe_Uper_Insert_Grid()
        {
            UltraGridRow row = this.uGrd_Recipe_UperData.DisplayLayout.Bands[0].AddNew();
            if (uGrd_Recipe_UperData.Rows.Count < 2)
            {
                row.Cells[0].Value = "E";
                row.Cells[1].Value = "1";
                row.Cells[2].Value = "1차";
                row.Cells[3].Value = "0";
                row.Cells[4].Value = "흑백";
                row.Cells[5].Value = "2차";
                row.Cells[6].Value = "2";
                row.Cells[7].Value = "흑백";
                row.Cells[8].Value = "3";
                row.Cells[9].Value = "True";
                row.Cells[10].Value = "22";
            }
            else
            {
                //그리드에서 추가할 Row를 선택하지 않았을 때 _iVisionUperGridRowNo의 값이 -1이 된다.
                //이때에는 마지막 Row의 데이터를 복사하도록 한다.
                if (_iRecipeUperGridRowNo == -1) _iRecipeUperGridRowNo = uGrd_Recipe_UperData.Rows.Count - 2;

                row.Cells[0].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[0].Value;
                row.Cells[1].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[1].Value;
                row.Cells[2].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[2].Value;
                row.Cells[3].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[3].Value;
                row.Cells[4].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[4].Value;
                row.Cells[5].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[5].Value;
                row.Cells[6].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[6].Value;
                row.Cells[7].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[7].Value;
                row.Cells[8].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[8].Value;
                row.Cells[9].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[9].Value;
                row.Cells[10].Value = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Cells[10].Value;
            }

            uGrd_Recipe_UperData.Rows.Move(row, uGrd_Recipe_UperData.Rows.Count - 1);
            _iRecipeUperGridRowNo = -1;

            LamiSystem.StrLstRcpConGridData_Uper.Clear();
            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string tmpCellData = uGrd_Recipe_UperData.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrLstRcpConGridData_Uper.Add(tmpCellData);
                }
            }
            
        }

        public void Recipe_Uper_Insert_ROI()
        {
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                tempRectNew.X = (uGrd_Recipe_UperData.Rows.Count*2 + i - 1)*10;
                tempRectNew.Y = (uGrd_Recipe_UperData.Rows.Count*2 + i - 1)*10;
                tempRectNew.Width = 30;
                tempRectNew.Height = 30;

                LamiSystem.RectListRecipeBoxZone_Uper.Add(tempRectNew);

                _Uper_Control_DrawArea.AddListObject(tempRectNew);

//                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[LamiSystem.RectListRecipeBoxZone_Uper.Count-1].Left);
//                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[LamiSystem.RectListRecipeBoxZone_Uper.Count - 1].Right);
//                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[LamiSystem.RectListRecipeBoxZone_Uper.Count - 1].Width);
//                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Uper[LamiSystem.RectListRecipeBoxZone_Uper.Count - 1].Height);
            }
        }

        public void Recipe_Down_Insert_ROI()
        {
            System.Drawing.Rectangle tempRectNew = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                tempRectNew.X = (uGrd_Recipe_DownData.Rows.Count * 2 + i - 1) * 10;
                tempRectNew.Y = (uGrd_Recipe_DownData.Rows.Count * 2 + i - 1) * 10;
                tempRectNew.Width = 30;
                tempRectNew.Height = 30;

                LamiSystem.RectListRecipeBoxZone_Down.Add(tempRectNew);

                _Down_Control_DrawArea.AddListObject(tempRectNew);
                //                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Down[LamiSystem.RectListRecipeBoxZone_Down.Count-1].Left);
                //                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Down[LamiSystem.RectListRecipeBoxZone_Down.Count - 1].Right);
                //                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Down[LamiSystem.RectListRecipeBoxZone_Down.Count - 1].Width);
                //                 Trace.WriteLine(LamiSystem.RectListRecipeBoxZone_Down[LamiSystem.RectListRecipeBoxZone_Down.Count - 1].Height);
            }
        }

        public void Recipe_Uper_Grid_Resize()
        {
            if (uGrd_Recipe_UperData.Rows.Count > 21)
            {
                if (uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[0].Width == 54) return;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[0].Width = 54;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[1].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[2].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[3].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[4].Width = 39;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[5].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[6].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[7].Width = 39;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[8].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[9].Width = 35;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[10].Width = 35;
            }
            else
            {
                if (uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[0].Width == 60) return;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[0].Width = 60;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[1].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[2].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[3].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[4].Width = 41;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[5].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[6].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[7].Width = 41;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[8].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[9].Width = 36;
                uGrd_Recipe_UperData.DisplayLayout.Bands[0].Columns[10].Width = 36;
                if (uGrd_Recipe_UperData.Rows.Count > 0) uGrd_Recipe_UperData.ActiveRow = uGrd_Recipe_UperData.Rows[0];
            }
        }



        private int _iRecipeDownGridRowNo = -1;
        public void Recipe_Down_Insert_Grid()
        {
            UltraGridRow row = this.uGrd_Recipe_DownData.DisplayLayout.Bands[0].AddNew();
            if (uGrd_Recipe_DownData.Rows.Count < 2)
            {
                row.Cells[0].Value = "TH-A";
                row.Cells[1].Value = "1";
                row.Cells[2].Value = "1차";
                row.Cells[3].Value = "0";
                row.Cells[4].Value = "흑백";
                row.Cells[5].Value = "2차";
                row.Cells[6].Value = "2";
                row.Cells[7].Value = "흑백";
                row.Cells[8].Value = "3";
                row.Cells[9].Value = "True";
                row.Cells[10].Value = "22";
            }
            else
            {
                

                //그리드에서 추가할 Row를 선택하지 않았을 때 _iVisionUperGridRowNo의 값이 -1이 된다.
                //이때에는 마지막 Row의 데이터를 복사하도록 한다.
                if (_iRecipeDownGridRowNo == -1) _iRecipeDownGridRowNo = uGrd_Recipe_DownData.Rows.Count - 2;

                row.Cells[0].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[0].Value;
                row.Cells[1].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[1].Value;
                row.Cells[2].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[2].Value;
                row.Cells[3].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[3].Value;
                row.Cells[4].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[4].Value;
                row.Cells[5].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[5].Value;
                row.Cells[6].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[6].Value;
                row.Cells[7].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[7].Value;
                row.Cells[8].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[8].Value;
                row.Cells[9].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[9].Value;
                row.Cells[10].Value = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Cells[10].Value;
            }

            uGrd_Recipe_DownData.Rows.Move(row, uGrd_Recipe_DownData.Rows.Count - 1);
            _iRecipeDownGridRowNo = -1;

            LamiSystem.StrLstRcpConGridData_Down.Clear();
            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                for (int j = 0; j < uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    string tmpCellData = uGrd_Recipe_DownData.Rows[i].Cells[j].Value.ToString();
                    LamiSystem.StrLstRcpConGridData_Down.Add(tmpCellData);
                }
            }
        }

        public void Recipe_Down_Grid_Resize()
        {
            if (uGrd_Recipe_DownData.Rows.Count > 21)
            {
                if (uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[0].Width ==  54) return;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[0].Width =  54;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[1].Width =  35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[2].Width =  35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[3].Width =  35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[4].Width =  39;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[5].Width =  35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[6].Width =  35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[7].Width =  39;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[8].Width =  35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[9].Width = 35;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[10].Width = 35;
            }                                                                  
            else                                                               
            {
                if (uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[0].Width == 60) return;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[0].Width = 60;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[1].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[2].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[3].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[4].Width = 41;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[5].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[6].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[7].Width = 41;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[8].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[9].Width = 36;
                uGrd_Recipe_DownData.DisplayLayout.Bands[0].Columns[10].Width = 36;
                if (uGrd_Recipe_DownData.Rows.Count > 0) uGrd_Recipe_DownData.ActiveRow = uGrd_Recipe_DownData.Rows[0];
            }
        }

        private void uGrd_Recipe_UperData_MouseDown(object sender, MouseEventArgs e)
        {
            UltraGridRow row;
            UIElement element;
            element = uGrd_Recipe_UperData.DisplayLayout.UIElement.ElementFromPoint(e.Location);
            row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;

            if (row != null && row.IsDataRow)
            {
                _iRecipeUperGridRowNo = row.Index;
                uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Appearance.BackColor = Color.Silver;
            }

            //20150309 WKB 209
            if(row != null) row.ExpandAncestors();
        }

        private void ultraTabControl2_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void uGrd_Recipe_DownData_MouseDown(object sender, MouseEventArgs e)
        {
            UltraGridRow row;
            UIElement element;
            element = uGrd_Recipe_DownData.DisplayLayout.UIElement.ElementFromPoint(e.Location);
            row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;

            if (row != null && row.IsDataRow)
            {
                _iRecipeDownGridRowNo = row.Index;
                uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Appearance.BackColor = Color.Silver;
            }

            //20150309 WKB 209
            if (row != null) row.ExpandAncestors();
        }

        public void Recipe_Uper_Grid_Move(int rowNo)
        {
            UltraGridRow row = uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo];
            uGrd_Recipe_UperData.Rows.Move(row, rowNo);
            if (row != null) row.ExpandAncestors();
        }

        //2015.02.07 WKB 208
        public void Recipe_Uper_Inspect_Move_Up(int rowNo)
        {
            if ((_iRecipeUperGridRowNo * 2) >= uGrd_Recipe_Inspect_Uper.Rows.Count) return;

            UltraGridRow row = uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2];
            uGrd_Recipe_Inspect_Uper.Rows.Move(row, rowNo * 2);
            row = uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2 + 1];
            uGrd_Recipe_Inspect_Uper.Rows.Move(row, rowNo * 2 + 1);
            if (row != null) row.ExpandAncestors();

            for (int i = 0; i < uDS_Inspect_Uper.Rows.Count; i++)
            {
                uGrd_Recipe_Inspect_Uper.Rows[i].Cells[13].Value = i.ToString();
            }
        }

        //2015.02.07 WKB 207
        /*
        public void Recipe_Uper_Inspect_Move_Up(int rowNo)
        {
            if ((_iRecipeUperGridRowNo*2) >= uGrd_Recipe_Inspect_Uper.Rows.Count) return;

            UltraGridRow row = uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo*2];
            uGrd_Recipe_Inspect_Uper.Rows.Move(row, rowNo*2);
            row = uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo*2 + 1];
            uGrd_Recipe_Inspect_Uper.Rows.Move(row, rowNo*2 + 1);

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                uGrd_Recipe_Inspect_Uper.Rows[i].Cells[13].Value = i.ToString();
            }
            uGrd_Recipe_Inspect_Uper.Refresh();
        }
        */

        public void Recipe_Down_Inspect_Move_Up(int rowNo)
        {
            if ((_iRecipeDownGridRowNo * 2) >= uGrd_Recipe_Inspect_Down.Rows.Count) return;

            UltraGridRow row = uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo * 2];
            uGrd_Recipe_Inspect_Down.Rows.Move(row, rowNo * 2);
            row = uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo * 2 + 1];
            uGrd_Recipe_Inspect_Down.Rows.Move(row, rowNo * 2 + 1);
            if (row != null) row.ExpandAncestors();

            for (int i = 0; i < uDS_Inspect_Down.Rows.Count; i++)
            {
                uGrd_Recipe_Inspect_Down.Rows[i].Cells[13].Value = i.ToString();
            }
        }

        public void Recipe_Uper_Inspect_Move_Down(int rowNo)
        {
            UltraGridRow row = uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo*2];
            uGrd_Recipe_Inspect_Uper.Rows.Move(row, rowNo*2+1);
            uGrd_Recipe_Inspect_Uper.Refresh();
            row = uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2];
            uGrd_Recipe_Inspect_Uper.Rows.Move(row, rowNo * 2+1);
            if (row != null) row.ExpandAncestors();

            for (int i = 0; i < uDS_Inspect_Uper.Rows.Count; i++)
            {
                uGrd_Recipe_Inspect_Uper.Rows[i].Cells[13].Value = i.ToString();
            }

            uGrd_Recipe_Inspect_Uper.DataSource = uDS_Inspect_Uper;
        }

        public void Recipe_Down_Inspect_Move_Down(int rowNo)
        {
            UltraGridRow row = uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo * 2];
            uGrd_Recipe_Inspect_Down.Rows.Move(row, rowNo * 2 + 1);
            uGrd_Recipe_Inspect_Down.Refresh();
            row = uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo * 2];
            uGrd_Recipe_Inspect_Down.Rows.Move(row, rowNo * 2 + 1);
            if (row != null) row.ExpandAncestors();

            for (int i = 0; i < uDS_Inspect_Down.Rows.Count; i++)
            {
                uGrd_Recipe_Inspect_Down.Rows[i].Cells[13].Value = i.ToString();
            }
        }

        public void Recipe_Down_Grid_Move(int rowNo)
        {
            UltraGridRow row = uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo];
            uGrd_Recipe_DownData.Rows.Move(row, rowNo);

            //20150309 WKB 209
            if (row != null) row.ExpandAncestors();
        }
        
        private void Recipe_uBtn_03_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            RecipeGrid_Changed = true;

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    if (_iRecipeUperGridRowNo < 0) break;
                    if (_iRecipeUperGridRowNo == 0) break;

                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(0);

                    Recipe_Uper_Grid_Move(_iRecipeUperGridRowNo - 1);
                    Recipe_Uper_Inspect_Move_Up(_iRecipeUperGridRowNo - 1);
                    _iRecipeUperGridRowNo = _iRecipeUperGridRowNo - 1;
                    Recipe_Uper_Grid_Resize();
                    break;
                case "DownRecipe":
                    if (_iRecipeDownGridRowNo < 0) break;
                    if (_iRecipeDownGridRowNo == 0) break;

                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(1);

                    Recipe_Down_Grid_Move(_iRecipeDownGridRowNo - 1);
                    Recipe_Down_Inspect_Move_Up(_iRecipeDownGridRowNo - 1);
                    _iRecipeDownGridRowNo = _iRecipeDownGridRowNo - 1;
                    Recipe_Down_Grid_Resize();
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void Recipe_uBtn_04_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            RecipeGrid_Changed = true;

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    if (_iRecipeUperGridRowNo < 0) break;
                    if (_iRecipeUperGridRowNo == uGrd_Recipe_UperData.Rows.Count - 1) break;

                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(0);

                    Recipe_Uper_Grid_Move(_iRecipeUperGridRowNo + 1);
                    Recipe_Uper_Inspect_Move_Down(_iRecipeUperGridRowNo + 1);
                    _iRecipeUperGridRowNo = _iRecipeUperGridRowNo + 1;
                    Recipe_Uper_Grid_Resize();
                    break;

                case "DownRecipe":
                    if (_iRecipeDownGridRowNo < 0) break;
                    if (_iRecipeDownGridRowNo == uGrd_Recipe_DownData.Rows.Count - 1) break;

                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(1);

                    Recipe_Down_Grid_Move(_iRecipeDownGridRowNo + 1);
                    Recipe_Down_Inspect_Move_Down(_iRecipeDownGridRowNo + 1);
                    _iRecipeDownGridRowNo = _iRecipeDownGridRowNo + 1;
                    Recipe_Down_Grid_Resize();
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void Recipe_uBtn_02_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            RecipeGrid_Changed = true;

            switch (this.ultraTabControl2.ActiveTab.Key)
            {
                case "UperRecipe":
                    if (_iRecipeUperGridRowNo < 0)
                    {
                        MessageBox.Show("삭제를 원하는 항목을 선택하여 주십시요!");
                        break;
                    }
                    Recipe_Uper_Grid_Delete();
                    Recipe_Uper_Grid_Resize();
                    Recipe_Uper_Inspect_Delete();
                    Recipe_Uper_ROI_Delete();
                    _iRecipeUperGridRowNo = -1;

                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(0);
                    break;

                case "DownRecipe":
                    if (_iRecipeDownGridRowNo < 0)
                    {
                        MessageBox.Show("삭제를 원하는 항목을 선택하여 주십시요!");
                        break;
                    }
                    Recipe_Down_Grid_Delete();
                    Recipe_Down_Grid_Resize();
                    Recipe_Down_Inspect_Delete();
                    Recipe_Down_ROI_Delete();
                    _iRecipeDownGridRowNo = -1;

                    //20150304 WKB 209
                    RecipeEditStatus_Desiable(1);
                    break;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public void Recipe_Uper_ROI_Delete()
        {
            if (deleteROI_Uper_Inspect < 0) return;
            _Uper_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Uper.RemoveAt(deleteROI_Uper_Inspect);
            LamiSystem.RectListRecipeBoxZone_Uper.RemoveAt(deleteROI_Uper_Inspect);

            if (LamiSystem.RectListRecipeBoxZone_Uper.Count == 0)
            {
                _Uper_Control_DrawArea._drawArea1.pictureBox1.Refresh();
                return;
            }

            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Uper.Count; i++)
            {
                _Uper_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Uper[i]);
            }
        }

        public void Recipe_Down_ROI_Delete()
        {
            if (deleteROI_Down_Inspect < 0) return;
            _Down_Control_DrawArea.ClearListObject();
            LamiSystem.RectListRecipeBoxZone_Down.RemoveAt(deleteROI_Down_Inspect);
            LamiSystem.RectListRecipeBoxZone_Down.RemoveAt(deleteROI_Down_Inspect);

            if (LamiSystem.RectListRecipeBoxZone_Down.Count == 0)
            {
                _Down_Control_DrawArea._drawArea1.pictureBox1.Refresh();
                return;
            }

            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Down.Count; i++)
            {
                _Down_Control_DrawArea.AddListObject(LamiSystem.RectListRecipeBoxZone_Down[i]);
            }
        }


        public void Recipe_Uper_Grid_Delete()
        {
            
                uGrd_Recipe_UperData.Rows[_iRecipeUperGridRowNo].Delete(false);
        }

        private int deleteROI_Uper_Inspect = -1;

        //2015.02.07 WKB 208
        public void Recipe_Uper_Inspect_Delete()
        {
            deleteROI_Uper_Inspect = -1;

            //deleteROI_Uper_Inspect = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].Cells[5].Value.ToString());
            deleteROI_Uper_Inspect = int.Parse(uDS_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].GetCellValue(5).ToString());

            uDS_Inspect_Uper.Rows.RemoveAt(_iRecipeUperGridRowNo * 2);
            uDS_Inspect_Uper.Rows.RemoveAt(_iRecipeUperGridRowNo * 2);

            //uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].Delete(false);
            //uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].Delete(false);


            for (int i = 0; i < uDS_Inspect_Uper.Rows.Count; i++)
            {
                //int readROINo = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                //if (readROINo > deleteROI_Uper_Inspect) uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value = (readROINo - 2).ToString();

                int readROINo = int.Parse(uDS_Inspect_Uper.Rows[i].GetCellValue(5).ToString());
                if (readROINo > deleteROI_Uper_Inspect) uDS_Inspect_Uper.Rows[i].SetCellValue(5,(readROINo - 2).ToString());

                //uGrd_Recipe_Inspect_Uper.Rows[i].Cells[4].Value = (i / 2).ToString("0");
                //uGrd_Recipe_Inspect_Uper.Rows[i].Cells[13].Value = i.ToString("0");

                uDS_Inspect_Uper.Rows[i].SetCellValue(4, (i / 2).ToString("0"));
                uDS_Inspect_Uper.Rows[i].SetCellValue(13, i.ToString("0")); 
            }
        }

        //2015.02.07 WKB 207
        /*
        public void Recipe_Uper_Inspect_Delete()
        {
            deleteROI_Uper_Inspect = -1;
           
                deleteROI_Uper_Inspect = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].Cells[5].Value.ToString());
                
                uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].Delete(false);
                uGrd_Recipe_Inspect_Uper.Rows[_iRecipeUperGridRowNo * 2].Delete(false);
           

            for (int i = 0; i < uGrd_Recipe_Inspect_Uper.Rows.Count; i++)
            {
                int readROINo = int.Parse(uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value.ToString());
                if (readROINo > deleteROI_Uper_Inspect) uGrd_Recipe_Inspect_Uper.Rows[i].Cells[5].Value = (readROINo - 2).ToString();

                uGrd_Recipe_Inspect_Uper.Rows[i].Cells[4].Value = (i/2).ToString("0");
                uGrd_Recipe_Inspect_Uper.Rows[i].Cells[13].Value = i.ToString("0");
            }
        }
        */

        public void Recipe_Down_Grid_Delete()
        {
                uGrd_Recipe_DownData.Rows[_iRecipeDownGridRowNo].Delete(false);
        }

        private int deleteROI_Down_Inspect = -1;
        public void Recipe_Down_Inspect_Delete()
        {
            try
            {
                deleteROI_Down_Inspect = -1;

                deleteROI_Down_Inspect = int.Parse(uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo*2].Cells[5].Value.ToString());

                uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo*2].Delete(false);
                uGrd_Recipe_Inspect_Down.Rows[_iRecipeDownGridRowNo*2].Delete(false);


                for (int i = 0; i < uGrd_Recipe_Inspect_Down.Rows.Count; i++)
                {
                    int readROINo = int.Parse(uGrd_Recipe_Inspect_Down.Rows[i].Cells[5].Value.ToString());
                    if (readROINo > deleteROI_Down_Inspect) uGrd_Recipe_Inspect_Down.Rows[i].Cells[5].Value = (readROINo - 2).ToString();

                    uGrd_Recipe_Inspect_Down.Rows[i].Cells[4].Value = (i/2).ToString("0");
                    uGrd_Recipe_Inspect_Down.Rows[i].Cells[13].Value = i.ToString("0");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " : " + e.Message);
                throw;
            }
        }

        private void uGrd_Recipe_UperData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void uGrd_Recipe_Inspect_Down_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
           
        }

        private void uGrd_Recipe_Inspect_Uper_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            
        }

        private void uGrd_Recipe_UperData_CellListSelect(object sender, CellEventArgs e)
        {
            Trace.WriteLine("uGrd_Recipe_Data_CellListSelect");
            int nowcell = e.Cell.Column.Index;
            Trace.WriteLine(nowcell.ToString("00"));
            int nowItem = e.Cell.ValueList.SelectedItemIndex;
            Trace.WriteLine(nowItem.ToString("00"));
        }

        private void uTxt_Pix1_03_Lami_ValueChanged(object sender, EventArgs e)
        {

        }

        private int Check_Commu_Flag = 1;
        private void Check_Commu_Tick(object sender, EventArgs e)
        {
            if (Check_Commu_Flag == 1)
            {
                umac.Umac_SetData_M6924(Check_Commu_Flag.ToString("0"));
                Check_Commu_Flag = 2;
            }
            else
            {
                umac.Umac_SetData_M6924(Check_Commu_Flag.ToString("0"));
                Check_Commu_Flag = 1;
            }
        }

        private void System_uCombo_EtcItem02_ValueChanged(object sender, EventArgs e)
        {

        }

        private void System_uCombo_EtcItem01_ValueChanged(object sender, EventArgs e)
        {

        }


        private void ultraLabel3_Click(object sender, EventArgs e)
        {

        }

        //메인 폼이 먼저 그려진 후에 데이터 초기화를 진행하기 위하여
        //Load()함수와 분리를 진행 했다.
        private void FormDlgMain_Shown(object sender, EventArgs e)
        {
            FormDlgMain_Initionalize();
            MainForm_ProgracessBar_Display_01("Vision Inspect System Initionalize Complite !", 100);
        }

        private void FormDlgMain_Activated(object sender, EventArgs e)
        {
            this.Activate();
            this.Focus();
        }

        private void uDS_Offset_BiCell_CellDataRequested(object sender, CellDataRequestedEventArgs e)
        {

        }

        private void uGrd_Recipe_UperData_CellListSelect_1(object sender, CellEventArgs e)
        {
            int nowCol = e.Cell.Column.Index;
            if (nowCol == 0) uGrd_Recipe_UperData_ItemName_Check(sender, e);
            if (nowCol == 1) uGrd_Recipe_UperData_Graph_Check(sender, e);
        }

        public void uGrd_Recipe_UperData_Graph_Check(object sender, CellEventArgs e)
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedGraph_Result_Uper = true;
            Struct_Check.GraphCount = 0;
            Struct_Check.nowCol = e.Cell.Column.Index;
            Struct_Check.nowRow = e.Cell.Row.Index;
            Struct_Check.Select_Name = e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();

            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                if (i == Struct_Check.nowRow) continue;

                string Grid_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[Struct_Check.nowCol].Value.ToString();
                if (Struct_Check.Select_Name == Grid_Name)
                {
                    Struct_Check.GraphCount = Struct_Check.GraphCount + 1;
                    if (Struct_Check.GraphCount >= 2)
                    {
                        uMessageBox.MessageBox_Show("레시피 설정", "그래프 설정", "선택한 그래프는 이미 사용하고 있습니다.<br/><br/>그래프를 다시 선택하여 주십시요.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Recipe_CheckedGraph_Result_Uper = false;

                        //20150304 WKB 209
                        ultraTabControl2.Focus();
                        uDS_Recipe_Uper.Rows[e.Cell.Row.Index].SetCellValue(1, "0");
                        RecipeEditStatus_Desiable(0);
                        this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[0];

                        break;
                    }
                }
            }
        }

        public void uGrd_Recipe_UperData_ItemName_Check(object sender, CellEventArgs e)
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedName_Result_Uper = true;
            Struct_Check.nowCol = e.Cell.Column.Index;
            Struct_Check.nowRow = e.Cell.Row.Index;
            Struct_Check.Select_Name = e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();

            for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
            {
                if (i == Struct_Check.nowRow) continue;

                string Grid_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[Struct_Check.nowCol].Value.ToString();
                if (Struct_Check.Select_Name == Grid_Name)
                {
                    uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "선택한 항목은 이미 사용하고 있습니다.<br/><br/>항목을 다시 선택하여 주십시요.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Recipe_CheckedName_Result_Uper = false;
                    
                    //20150304 WKB 209
                    ultraTabControl2.Focus();
                    uDS_Recipe_Uper.Rows[e.Cell.Row.Index].SetCellValue(0,"");
                    RecipeEditStatus_Desiable(0);
                    this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[0];

                    break;
                }
            }
        }

        private void uGrd_Recipe_DownData_CellListSelect(object sender, CellEventArgs e)
        {
            int nowCol = e.Cell.Column.Index;
            if (nowCol == 0) uGrd_Recipe_DownData_ItemName_Check(sender, e);
            if (nowCol == 1) uGrd_Recipe_DownData_Graph_Check(sender, e);
        }

        public void uGrd_Recipe_DownData_Graph_Check(object sender, CellEventArgs e)
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedGraph_Result_Down = true;
            Struct_Check.GraphCount = 0;
            Struct_Check.nowCol = e.Cell.Column.Index;
            Struct_Check.nowRow = e.Cell.Row.Index;
            Struct_Check.Select_Name = e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();

            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                if (i == Struct_Check.nowRow) continue;

                Struct_Check.Grid_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[Struct_Check.nowCol].Value.ToString();
                if (Struct_Check.Select_Name == Struct_Check.Grid_Name)
                {
                    Struct_Check.GraphCount = Struct_Check.GraphCount + 1;
                    if (Struct_Check.GraphCount >= 2)
                    {
                        uMessageBox.MessageBox_Show("레시피 설정", "그래프 설정", "선택한 그래프는 이미 사용하고 있습니다.<br/><br/>그래프를 다시 선택하여 주십시요.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Recipe_CheckedGraph_Result_Uper = false;

                        //20150304 WKB 209
                        ultraTabControl2.Focus();
                        uDS_Recipe_Down.Rows[e.Cell.Row.Index].SetCellValue(1, "0");
                        RecipeEditStatus_Desiable(1);
                        this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[1];

                        break;
                    }
                }
            }
        }

        public void uGrd_Recipe_DownData_ItemName_Check(object sender, CellEventArgs e)
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedName_Result_Down = true;
            Struct_Check.nowCol = e.Cell.Column.Index;
            Struct_Check.nowRow = e.Cell.Row.Index;
            Struct_Check.Select_Name = e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();

            for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
            {
                if (i == Struct_Check.nowRow) continue;

                Struct_Check.Grid_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[Struct_Check.nowCol].Value.ToString();
                if (Struct_Check.Select_Name == Struct_Check.Grid_Name)
                {
                    uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "선택한 항목은 이미 사용하고 있습니다.<br/><br/>항목을 다시 선택하여 주십시요.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Recipe_CheckedName_Result_Down = false;

                    //20150304 WKB 209
                    ultraTabControl2.Focus();
                    uDS_Recipe_Down.Rows[e.Cell.Row.Index].SetCellValue(0, "");
                    RecipeEditStatus_Desiable(1);
                    this.ultraTabControl2.SelectedTab = this.ultraTabControl2.Tabs[1];

                    break;
                }
            }
        }


        public void Selected_Recipe_GridCheck()
        {
            Recipe_UperData_ItemName_Check();
            Recipe_DownData_ItemName_Check();

            Recipe_UperData_Graph_Check();
            Recipe_DownData_Graph_Check();
        }

        public void Recipe_UperData_ItemName_Check()
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedName_Result_Uper = true;
            //int nowCol = 0;//e.Cell.Column.Index;
            //int nowRow = 0;//e.Cell.Row.Index;
            //string Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[nowCol].Value.ToString(); //e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();
            for (int j = 0; j < uGrd_Recipe_UperData.Rows.Count; j++)
            {
                Struct_Check.Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[j].Cells[0].Value.ToString();
                for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
                {
                    if (i == j) continue;

                    Struct_Check.Grid_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[0].Value.ToString();
                    if (Struct_Check.Select_Name == Struct_Check.Grid_Name)
                    {
                        uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "선택한 항목은 이미 사용하고 있습니다.<br/><br/>항목을 다시 선택하여 주십시요.",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Recipe_CheckedName_Result_Uper = false;
                        return;
                    }
                }
            }
        }

        public void Recipe_DownData_ItemName_Check()
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedName_Result_Down = true;
            //int nowCol = 0;//e.Cell.Column.Index;
            //int nowRow = 0;//e.Cell.Row.Index;
            //string Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[nowCol].Value.ToString(); //e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();
            for (int j = 0; j < uGrd_Recipe_DownData.Rows.Count; j++)
            {
                Struct_Check.Select_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[j].Cells[0].Value.ToString();

                for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
                {
                    if (i == j) continue;

                    Struct_Check.Grid_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[0].Value.ToString();
                    if (Struct_Check.Select_Name == Struct_Check.Grid_Name)
                    {
                        uMessageBox.MessageBox_Show("레시피 설정", "항목 설정", "선택한 항목은 이미 사용하고 있습니다.<br/><br/>항목을 다시 선택하여 주십시요.",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Recipe_CheckedName_Result_Down = false;
                        return;
                    }
                }
            }
        }

        public void Recipe_UperData_Graph_Check()
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedGraph_Result_Uper = true;
            
            //int nowCol = 0;//e.Cell.Column.Index;
            //int nowRow = 0;//e.Cell.Row.Index;
            //string Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[nowCol].Value.ToString(); //e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();
            for (int j = 0; j < uGrd_Recipe_UperData.Rows.Count; j++)
            {
                Struct_Check.GraphCount = 0;
                Struct_Check.Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[j].Cells[1].Value.ToString();

                for (int i = 0; i < uGrd_Recipe_UperData.Rows.Count; i++)
                {
                    if (i == j) continue;

                    string Grid_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[1].Value.ToString();
                    if (Struct_Check.Select_Name == Struct_Check.Grid_Name)
                    {
                        Struct_Check.GraphCount = Struct_Check.GraphCount + 1;
                        if (Struct_Check.GraphCount >= 2)
                        {
                            uMessageBox.MessageBox_Show("레시피 설정", "그래프 설정", "선택한 그래프는 이미 사용하고 있습니다.<br/><br/>그래프를 다시 선택하여 주십시요.",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Recipe_CheckedGraph_Result_Uper = false;
                            return;
                        }
                    }
                }
            }
        }

        public void Recipe_DownData_Graph_Check()
        {
            Recipe_Double_Check Struct_Check = new Recipe_Double_Check();

            Recipe_CheckedGraph_Result_Down = true;
            
            //int nowCol = 0;//e.Cell.Column.Index;
            //int nowRow = 0;//e.Cell.Row.Index;
            //string Select_Name = uGrd_Recipe_UperData.DisplayLayout.Rows[i].Cells[nowCol].Value.ToString(); //e.Cell.ValueList.GetValue(e.Cell.ValueList.SelectedItemIndex).ToString();
            for (int j = 0; j < uGrd_Recipe_DownData.Rows.Count; j++)
            {
                Struct_Check.GraphCount = 0;
                Struct_Check.Select_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[j].Cells[1].Value.ToString();

                for (int i = 0; i < uGrd_Recipe_DownData.Rows.Count; i++)
                {
                    if (i == j) continue;

                    Struct_Check.Grid_Name = uGrd_Recipe_DownData.DisplayLayout.Rows[i].Cells[1].Value.ToString();
                    if (Struct_Check.Select_Name == Struct_Check.Grid_Name)
                    {
                        Struct_Check.GraphCount = Struct_Check.GraphCount + 1;
                        if (Struct_Check.GraphCount >= 2)
                        {
                            uMessageBox.MessageBox_Show("레시피 설정", "그래프 설정", "선택한 그래프는 이미 사용하고 있습니다.<br/><br/>그래프를 다시 선택하여 주십시요.",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Recipe_CheckedGraph_Result_Uper = false;
                            return;
                        }
                    }
                }
            }
        }

        public struct Recipe_Double_Check
        {
            public int GraphCount;
            public string Select_Name;
            public string Grid_Name;

            public int nowCol;// = e.Cell.Column.Index;
            public int nowRow;// = e.Cell.Row.Index;
        }

        private void utabDlgMain_ActiveTabChanging(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventArgs e)
        {
            return;

            if (utabDlgMain.ActiveTab == null) return;
            if (ubtnToolbarUser.Text == "OPERATOR") return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            switch (utabDlgMain.ActiveTab.Key)
            {
                case "System":
                    System_Config_Saving();
                    break;

                case "Vision":
                    Vision_Config_Saving();
                    break;

                case "Recipe":
                    Recipe_Config_Saving();
                    break;
            }
            All_Config_Saving();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public void System_Config_Saving()
        {
            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            System_Config_Viewer_To_List_Data();

#if(SYST_SIMUL)

#else
            //시리얼 포트를 통해서 조명 제어기를 설정한다.
            System_BackLight_Setup();
#endif
        }

        public void Vision_Config_Saving()
        {
            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            VisionLami_Config_Viewer_To_List_Data();
            VisionLami_Config_UperGrid_To_List();
            VisionLami_Config_DownGrid_To_List();
        }

        public void Recipe_Config_Saving()
        {
            if (uGrd_Recipe_Applying_ItemName_Check() == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }
            if (uGrd_Recipe_Applying_GraphNum_Check() == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            //그림상자의 이미지와 설정한 구역의 배율을 계산하고 저장한다.
            RecipeGap_Config_Box_To_Image_Sum();

            //설정한 화면의 값들을 리스트 배열에 저장한다. 
            RecipeGap_Config_Viewer_To_List_Data();


            //설정한 화면의 그리드 값들을 리스트 배열에 저장한다.
            Recipe_Config_Viewer_To_UperGrid();
            Recipe_Config_Viewer_To_DownGrid();

            //설정한 화면의 검출 영역을 배열에 저장한다.
            Recipe_Config_Viewer_To_List_Inspect_Uper();
            Recipe_Config_Viewer_To_List_Inspect_Down();
        }

        public void All_Config_Saving()
        {
            

            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 10);
            //시스템 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            System_Config_ListData_To_Register();

            //비전부 설정 탭에서 적용되어져 있는 List의 항목을 레지스터에 기록한다.
            Vision_Config_ListData_To_Register();
            Vision_Config_UperGrid_To_Register();
            Vision_Config_DownGrid_To_Register();
            
            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 20);

            //Recipe_Config_Register_To_Lists_Inspect_Uper();
            //Recipe_Config_Register_To_Lists_Inspect_Down();

            //레시피 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Recipe_Config_ListData_To_Register();
            Recipe_Config_UperGrid_To_Register();
            Recipe_Config_DownGrid_To_Register();

            Recipe_Config_Register_To_Lists_Inspect_Uper();
            Recipe_Config_Register_To_Lists_Inspect_Down();


            //레시피 설정 탭에서 적용되어져 있는 ROI 정보를 레지스터에 기록한다.
            Recipe_Config_UperInspect_To_Register();
            Recipe_Config_DownInspect_To_Register();

            //환경 설정 탭에서 적용되어져 있는 List 항목을 레지스터에 기록한다.
            Equipment_Config_ListData_To_Register();
            
            MainForm_ProgracessBar_Display_01("Model Recipe Saving !!", 30);

            //현재 적용되어져 있는 리스트 배열의 값을 현재 모델의 파일에 저장한다.
            Model_Config_Add_List_To_File(LamiSystem.GetSet_Now_Model_Name, LamiSystem.GetSet_Now_Model_Number);

            
        }

        private void VisionGap_uBtn_GrabNone_Click(object sender, EventArgs e)
        {

        }
    }
}
