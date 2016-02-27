using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.Misc;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.UserInterface;
using FontStyle = System.Drawing.FontStyle;
using Resources = SystemAlign.Properties.Resources;

namespace SystemAlign
{
    internal class Control_Inspect_Gap
    {
        private readonly List<double> Gap_Data_Minus_Array = new List<double>();
        private readonly List<double> Gap_Data_Pluse_Array = new List<double>();
        private readonly int[] Threshold_01 = {150, 190};
        private readonly int[] Threshold_02 = {50, 100};
        private readonly IplImage _NowSavedImage = new IplImage(4096, 3072, BitDepth.U8, 3);
        private readonly List<CvPoint> cvPntLstImagePoint = new List<CvPoint>();
        private readonly Control_Excel excelFile = new Control_Excel();
        public readonly List<int> iCenterPointToROI = new List<int>();
        private readonly List<int> iLstNowDivNo = new List<int>();
        private readonly List<int> iLstNowLgtNo = new List<int>();
        private readonly List<int> iLstNowLstNo = new List<int>();
        private readonly List<int> iLstNowRoiNo = new List<int>();
        private readonly List<int> iLstNowRowNo = new List<int>();
        private readonly List<int> iLstNowSeqNo = new List<int>();
        private readonly List<int> iLstNowSidNo = new List<int>();
        private readonly Hough imageHougher = new Hough();
        private readonly Pen myArrowPen = new Pen(Color.LawnGreen, 1);
        private readonly Pen myLinePen = new Pen(Color.LawnGreen, 1);
        private readonly List<string> sLstNowDisNo = new List<string>();
        private readonly List<string> sLstNowPolNo = new List<string>();
        private readonly List<string> sLstNowTypNo = new List<string>();
        private readonly List<string> strLstDisplayData = new List<string>();
        private readonly string[] strResultNgOk = {string.Empty, string.Empty, string.Empty};
        private static IplImage tempImage = new IplImage(4096, 3072, BitDepth.U8, 3);
        
        private double Cal_Garo_Value = 0.0;
        private double Cal_Sero_Value = 0.0;// = 11.1577;

        private double Cal_Big_Left = 0.0;
        private double Cal_Big_Righ = 0.0;
        private double Cal_Mid_Left = 0.0;
        private double Cal_Mid_Righ = 0.0;
        private double Cal_Sml_Left = 0.0;
        private double Cal_Sml_Righ = 0.0;

        public string CallForm = "Main";
        public double Dist_Center_Garo;
        public double Dist_Center_Sero;
        private Thread Excel_Write_Thread;
        private Thread File_Save_Thread;
        private List<double> Gap_Data_Max_Array = new List<double>();
        private List<double> Gap_Data_Min_Array = new List<double>();
        private Thread History_All_Save_Thread;
        private Thread History_NG_Save_Thread;
        private int HoughRetryCount;
        public double Image_Center_Garo;
        public double Image_Center_Sero;
        private int NowCellDataCount = 3;
        private int NowCellGrabPos = 1;
        private int NowCellGrabber = 0;
        private int NowCellNumber;
        private int NowCellType = 0;
        public double NowCenter_Garo;
        public double NowCenter_Sero;
        private string NowExcelFolderSavePath;
        private uint NowFailNumber_BiCell;
        private uint NowFailNumber_Gap;
        private int NowGapNumber;
        private int NowGapTypeNumber;
        private string NowImageFolderSavePath;
        public string NowInspectResult = "OK"; //"NG"
        public double NowLength_Garo;
        public double NowLength_Sero;
        private double NowModel_ATypeX = 0.0;
        private double NowModel_ATypeY = 0.0;
        private int NowModel_Array = 0;
        private double NowModel_CTypeX = 0.0;
        private double NowModel_CTypeY = 0.0;
        private int NowModel_Cells;
        private uint NowProdectNumber_BiCell;
        private uint NowProdectNumber_Gap;
        private int NowROI_Number = -1;
        private int NowROI_Start;
        private uint NowTrigNumber_BiCell;
        private uint NowTrigNumber_Gap;
        private List<double> PLC_Model_Gap_Data = new List<double>();
        public double Real_Center_Garo;
        public double Real_Center_Sero;
        private double RectTheta = 0.0;
        private CvPoint RectTheta1 = new CvPoint(0, 0);
        private CvPoint RectTheta2 = new CvPoint(0, 0);
        private string Run_Mode = "Auto";
        private bool ThreadFirstFlag;
        private bool ZeroCheck_Big;
        private bool ZeroCheck_Middle;
        public bool ZeroCheck_Small = false;
        public DateTime _EndTime;
        private bool _FlagManual = true;
        private double _dCalibration_GaRo;
        private double _dCalibration_SeRo;
        private double _dNow_Image_Garo;
        private double _dNow_Image_Sero;
        public double[] _dSavedResultValue = new double[3];
        private double _dSaved_Center_Garo = 0.0;
        private double _dSaved_Center_Sero = 0.0;
        private double _dVision_Grabber_Center_Angle = 0.0;
        private double _dVision_Grabber_Center_X = 0.0;
        private double _dVision_Grabber_Center_Y = 0.0;
        private double _dVision_Offset_BiCell_Angle = 0.0;
        private double _dVision_Offset_BiCell_Y = 0.0;
        private double _dVision_Offset_ImageCenterX = 0.0;
        private double _dVision_Offset_ImageCenterY = 0.0;

        private double _dVision_Offset_Type_X = 0.0;
        private double _dVision_Offset_Type_Y = 0.0;
        private DateTime _dtImageSaveTime;
        private DateTime _dtInspDataSaveTime;
        private DateTime _dtInspLogFileTime;
        private DateTime _dtUmacSetTime = new DateTime();
        private double[] _fResultValueLeft = new double[3];
        private double[] _fResultValueRight = new double[3];
        private CInspection_Folding_Gap _gapSystem;
        private int _iAllHistoryViewNo;
        private int _iEdgeParam1;
        private int _iEdgeParam2;
        private int _iEdgeParam3;
        private int _iGrabImageGaro;
        private int _iGrabImageSero;
        private int _iHistoryViewNo = -1;
        private int _iImgCount = -1;
        private int _iLineParam1;
        private int _iLineParam2;
        private int _iLineParam3;
        private int _iManual_CellNo = -1;
        private int _iManual_CellType = -1;
        private int _iManual_GripNo = -1;
        private int _iNGHistoryViewNo;
        public int _iSavedCellNumber = 0;
        public int _iSavedCellType = 0;
        public int _iSavedGrabber = 0;
        public uint _iSavedTrigNumber = 0;
        private PictureBoxIpl _imageBoxIpl;
        private string _nowAccount = string.Empty;
        private static IplImage _nowIplImage = new IplImage(4096, 3072, BitDepth.U8, 3);
        private Control_PLC _plc;
        private PositionConvert _pointAnalize;
        private List<CvPoint> _savedCvPntLstImagePoint;
        public int _iSavedGap_Number = -1;
        public double _savedLength_Sepa = 0.0;
        public double _savedValue_Left = 0.0;
        public double _savedValue_Right = 0.0;
        private string _strHisImgName = string.Empty;
        public string _strSavedInspectResult = string.Empty;
        public string[] _strSavedNGOK = {string.Empty, string.Empty, string.Empty};
        private List<CvPoint> boxArrowPntLst = new List<CvPoint>();
        private AdjustableArrowCap cusCap;
        public List<CvPoint> cvPntLstCenterPoint = new List<CvPoint>();
        public List<CvPoint> cvPntLstCrossPoint = new List<CvPoint>();
        public List<CvPoint> cvPntLstThetaPoint = new List<CvPoint>();
        public List<CvPoint> cvPntLstWidthData = new List<CvPoint>();
        public CvPoint cvPntNow_Image_CentPnt = new CvPoint(0, 0);
        private double dRealCenterValue;
        private double dRealMaxValue;
        private double dRealMinValue;
        private double dRealMinusValue = 0.0;
        private double dRealPluseValue = 0.0;
        private double dResultValueLeft;
        private double dResultValueRight;
        public List<bool> drawOKList = new List<bool>();
        public List<int> drawROIList = new List<int>();
        private string failSaveData = string.Empty;
        private string failSaveImage = string.Empty;
        private Graphics gc;
        private int iImgPixResultData1;
        private int iImgPixResultData2;
        private int iStandardLong_A = 1700;
        private int iStandardLong_C = 1670;
        private int iStandardShort_A = 1280;
        private int iStandardShort_C = 1255;
        private int inspItemCount = 14;
        private double measureWidthResult = -1.0;
        private string passSaveData = string.Empty;
        private string passSaveImage = string.Empty;
        public Point pntCenterMarkInspBox = new Point(0, 0);
        private int shiftCenter = 5;
        private string strImgSaveTimeDelivery = string.Empty;

        private string strInspect_RunMode = "Manual";
        private UltraPanel uPanel_History_All;
        private UltraPanel uPanel_History_NG;

        //_gapSystem, _posConverter_Gap, fileSystem, gc, Inspect_Main01_IplBox, plc
        public Control_Inspect_Gap()
        {
            //Inspect_Gap_Class_Init();
        }


      

        public List<double> GetSet_NowModel_Gap_Data
        {
            get { return PLC_Model_Gap_Data; }
            set { PLC_Model_Gap_Data = value; }
        }

        public int GetSet_NowModel_Cells
        {
            get { return NowModel_Cells; }
#if(!SIM02)
            set { NowModel_Cells = 11; }
#else
            set { NowModel_Cells = value; }
#endif
        }

        public CInspection_Folding_Gap GetSet_GapSystem
        {
            get { return _gapSystem; }
            set { _gapSystem = value; }
        }

        public PositionConvert GetSet_Converter
        {
            get { return _pointAnalize; }
            set { _pointAnalize = value; }
        }

        public Control_Files GetSet_FileSystem { get; set; }

        public Graphics GetSet_Graphics
        {
            get { return gc; }
            set { gc = value; }
        }

        public PictureBoxIpl GetSet_ImageBoxIpl
        {
            get { return _imageBoxIpl; }
            set { _imageBoxIpl = value; }
        }

        public IplImage GetSet_NowIplImage
        {
            get { return _nowIplImage; }
            set { _nowIplImage = value; }
        }

        public Control_PLC GetSet_PLCSystem
        {
            get { return _plc; }
            set { _plc = value; }
        }

        public int GetSet_NowGapNumber
        {
            get { return NowGapNumber; }
            set { NowGapNumber = value; }
        }


        //자동을 시작했을 때 아래의 플래그가 트루이면
        //갭 값을 증가 시키지 않는다.

        public bool GetSet_ThreadFirstFlag
        {
            get { return ThreadFirstFlag; }
            set { ThreadFirstFlag = value; }
        }

        public List<double> GetSet_Gap_Min_Data_Array
        {
            get { return Gap_Data_Min_Array; }
            set { Gap_Data_Min_Array = value; }
        }

        public List<double> GetSet_Gap_Max_Data_Array
        {
            get { return Gap_Data_Max_Array; }
            set { Gap_Data_Max_Array = value; }
        }

        public UltraPanel GetSet_HistoryAll
        {
            get { return uPanel_History_All; }
            set { uPanel_History_All = value; }
        }

        public UltraPanel GetSet_HistoryNG
        {
            get { return uPanel_History_NG; }
            set { uPanel_History_NG = value; }
        }

        public string GetSet_Calling_Form
        {
            get { return CallForm; }
            set { CallForm = value; }
        }

        public List<string> GridDisplayData
        {
            get { return strLstDisplayData; }
        }

        public event MyEventOneInsp_Gap1 OperationEvent_Gap1;
        public event MyEventOneInsp_Gap2 OperationEvent_Gap2;

        private void Inspect_Gap_Class_Init()
        {
            Inspect_Gap_Initionalize();
        }

        private void Inspect_Ready_Run_MyArrowPen_Make()
        {
            cusCap = new AdjustableArrowCap(5, 5, false);
            myArrowPen.StartCap = LineCap.Custom;
            myArrowPen.EndCap = LineCap.Custom;
            myArrowPen.CustomStartCap = cusCap;
        }

        private void Inspect_Ready_Run_RecipeGrid_Data_Load()
        {
            ////Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            string tmp1 = _gapSystem.StrListSysConTitle[10];
            string tmp2 = _gapSystem.StrListVisConTitle_Gap[7];

            pntCenterMarkInspBox.X = _imageBoxIpl.Width/2;
            pntCenterMarkInspBox.Y = _imageBoxIpl.Height/2;

            _dCalibration_GaRo = double.Parse(_gapSystem.StrListVisConData_Gap[0]);
            _dCalibration_SeRo = double.Parse(_gapSystem.StrListVisConData_Gap[1]);

            _iEdgeParam1 = int.Parse(_gapSystem.StrListSysConData[11]);
            _iEdgeParam2 = int.Parse(_gapSystem.StrListSysConData[12]);
            _iEdgeParam3 = int.Parse(_gapSystem.StrListSysConData[13]);

            _iLineParam1 = int.Parse(_gapSystem.StrListSysConData[14]);
            _iLineParam2 = int.Parse(_gapSystem.StrListSysConData[15]);
            _iLineParam3 = int.Parse(_gapSystem.StrListSysConData[16]);

            _iGrabImageGaro = int.Parse(_gapSystem.StrListSysConData[17]);
            _iGrabImageSero = int.Parse(_gapSystem.StrListSysConData[18]);

            Threshold_01[0] = int.Parse(_gapSystem.StrListSysConData[11]);
            Threshold_01[1] = int.Parse(_gapSystem.StrListSysConData[12]);

            for (int i = 0; i < _gapSystem.RectListImageZone_Gap.Count; i++)
            {
                iLstNowRowNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[4 + (i*inspItemCount)])); //Row
                iLstNowRoiNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[5 + (i*inspItemCount)])); //ROI
                sLstNowTypNo.Add(_gapSystem.StrLstRcpConInspData_Gap[6 + (i*inspItemCount)]); //Type
                iLstNowSeqNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[7 + (i*inspItemCount)])); //Seq No
                iLstNowSidNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[8 + (i*inspItemCount)])); //Side No
                sLstNowPolNo.Add(_gapSystem.StrLstRcpConInspData_Gap[9 + (i*inspItemCount)]); //극성
                iLstNowDivNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[10 + (i*inspItemCount)])); //분할
                sLstNowDisNo.Add(_gapSystem.StrLstRcpConInspData_Gap[11 + (i*inspItemCount)]); //표시
                iLstNowLgtNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[12 + (i*inspItemCount)])); //밝기
                iLstNowLstNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[13 + (i*inspItemCount)])); //리스트 번호
                if (_gapSystem.StrLstRcpConInspData_Gap[6 + (i*inspItemCount)] == "거리")
                {
                    iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[5 + (i*inspItemCount)]));
                }
                else
                {
                    iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[5 + (i*inspItemCount)]));
                    iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_Gap[5 + (i*inspItemCount)]));
                }
            }
        }

        private void Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check()
        {
            _gapSystem.RectListImageZone_Gap.Clear();

            for (int i = 0; i < _gapSystem.RectListRecipeBoxZone_Gap.Count; i++)
            {
                var tempRect = new CvRect(0, 0, 0, 0);
                _pointAnalize.BoxToImage(_gapSystem.RectListRecipeBoxZone_Gap[i], ref tempRect,
                    _gapSystem.GetSet_System_Status_Zoom_X_Gap, _gapSystem.GetSet_System_Status_Zoom_Y_Gap);
                _gapSystem.RectListImageZone_Gap.Add(tempRect);
            }
        }

        public void Gap_Data_Mixing_Make()
        {
            string GapData = _gapSystem.StrListVisConGridData_Uper[1];

            Gap_Data_Max_Array.Clear();
            Gap_Data_Min_Array.Clear();
            Gap_Data_Pluse_Array.Clear();
            Gap_Data_Minus_Array.Clear();

            for (int i = 0; i < NowModel_Cells; i++)
            {
                double dNumberPluse = 0.0;
                bool resultPluse = double.TryParse(_gapSystem.StrListVisConGridData_Uper[4*i + 2], out dNumberPluse);
                Gap_Data_Pluse_Array.Add(dNumberPluse);

                double dNumberMinus = 0.0;
                bool resultMinus = double.TryParse(_gapSystem.StrListVisConGridData_Uper[4*i + 3], out dNumberMinus);
                Gap_Data_Minus_Array.Add(dNumberMinus);

                Gap_Data_Max_Array.Add(PLC_Model_Gap_Data[i] + Gap_Data_Pluse_Array[i]);
                Gap_Data_Min_Array.Add(PLC_Model_Gap_Data[i] - Gap_Data_Minus_Array[i]);
            }
        }

        public void Inspect_Gap_Initionalize()
        {
            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load();

            //측정값 계산에 사용되는 켈값을 로딩한다.
            Inspect_Gap_Offset_Load_To_System();
        }

        public void Inspect_Auto_Image_Grabing()
        {
            Tack_Time_Watch_Gap.Reset();
            Tack_Time_Watch_Gap.Start();

            _dtInspDataSaveTime = DateTime.Now;

            //레지스트리로 부터 갭번호를 읽어온다.
            NowGapNumber = Inspect_Run_Run_Get_Reg_NowGapNo();
            
            //쓰레드가 처음 시작된 시점인지를 판단하는 플래그를
            //검사해서 처음이면 갭번호를 증가하지 않는다.
            if (ThreadFirstFlag==true) ThreadFirstFlag = false;
            else
            {
                //트리거 입력되어 갭번호를 증가한다.
                if (NowGapNumber < (NowModel_Cells - 1)) NowGapNumber++;
                Inspect_Run_Run_Set_Reg_NowGapNo(NowGapNumber);
            }

            //로딩한 이미지의 ROI 처리를 진행한다.
            Inspect_Run_Run_Image_Scan();
          
            Inspect_Run_Run_Drawing_Result();
            OperationEvent_Gap1(NowGapNumber, NowProdectNumber_Gap);
        }

        /*
        public void Inspect_Auto_Image_Grabing()
        {
            _dtInspDataSaveTime = DateTime.Now;

            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load();

            //레지스트리로 부터 갭번호를 읽어온다.
            NowGapNumber = Inspect_Run_Run_Get_Reg_NowGapNo();
            //쓰레드가 처음 시작된 시점인지를 판단하는 플래그를
            //검사해서 처음이면 갭번호를 증가하지 않는다.
            if (ThreadFirstFlag) ThreadFirstFlag = false;
            else
            {
                //트리거 입력되어 갭번호를 증가한다.
                if (NowGapNumber < (NowModel_Cells - 1)) NowGapNumber++;
                else NowGapNumber = NowModel_Cells - 1;

                Inspect_Run_Run_Set_Reg_NowGapNo(NowGapNumber);
            }

            OperationEvent_Gap(NowGapNumber, NowProdectNumber_Gap);
            Trace.WriteLine(NowGapNumber.ToString("00"));
            //로딩한 이미지의 ROI 처리를 진행한다.
            Inspect_Manual_Run_Run();
            Inspect_Run_Run_Drawing_Result();
        }
        */
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


        //1번 파라미터의 경로 아래에 있는  
        //2번 파라미터와 같은 이름의 레지스터 항목을
        //3번 파라미터 값으로 저장한다.
        public void SetReg(string strNodePath, string strName, string strData)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(strNodePath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(strName, strData, RegistryValueKind.String);
            reg.Close();
        }

        public string Inspect_Run_Ready_Get_RegData_Gap(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }

        public void Inspect_Run_Run_Read_Reg_NgCount_Gap()
        {
            NowFailNumber_Gap = uint.Parse(Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "NGCountGap"));

            if (NowFailNumber_Gap < uint.MaxValue)
                NowFailNumber_Gap++;
            else
                NowFailNumber_Gap = 1;
            SetReg(_gapSystem.RegPathGapStatus, "NGCountGap", NowFailNumber_Gap.ToString());
        }

        public void Inspect_Run_Run_Read_Reg_NgCount_BiCell()
        {
            NowFailNumber_BiCell =
                uint.Parse(Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "NGCountBiCell"));

            if (NowFailNumber_BiCell < uint.MaxValue)
                NowFailNumber_BiCell++;
            else
                NowFailNumber_BiCell = 1;
            SetReg(_gapSystem.RegPathGapStatus, "NGCountBiCell", NowFailNumber_BiCell.ToString());
        }

        public int Inspect_Run_Run_Get_Reg_NowGapNo()
        {
            string strGapNo = GetReg(_gapSystem.RegPathGapStatus, "NowGapNo");
            int iNumber = -1;
            bool result = int.TryParse(strGapNo, out iNumber);
            return iNumber;
        }

        public void Inspect_Run_Run_Set_Reg_NowGapNo(int gapNo)
        {
            SetReg(_gapSystem.RegPathGapStatus, "NowGapNo", gapNo.ToString());
        }

        /*
        
        public void Inspect_Run_Run_Read_Reg_NowGapNo()
        {
            NowGapNumber = int.Parse(Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "NowGapNo"));
                NowGapNumber++;
            if (NowGapNumber > (NowModel_Cells-1))
            {
                NowGapNumber = (NowModel_Cells - 1);
            }
            SetReg(_gapSystem.RegPathGapStatus, "NowGapNo", NowGapNumber.ToString());
        }
        public void Inspect_Run_Run_Read_Reg_NowCellNo()
        {
            NowCellNumber = int.Parse(Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "NowCellNo"));

            if (NowCellNumber < int.MaxValue)
                NowCellNumber++;
            else
                NowCellNumber = 1;
            SetReg(_gapSystem.RegPathGapStatus, "NowCellNo", NowCellNumber.ToString());
        }
        public void Inspect_Run_Run_Read_Reg_TrigNo_Gap()
        {
            NowTrigNumber_Gap = uint.Parse(Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "trigNoGap"));

            if (NowTrigNumber_Gap < uint.MaxValue)
                NowTrigNumber_Gap++;
            else
                NowTrigNumber_Gap = 1;
            SetReg(_gapSystem.RegPathGapStatus, "trigNoGap", NowTrigNumber_Gap.ToString());
        }

        private uint TrigNoBiCell = 0;
        public void Inspect_Run_Run_Read_Reg_TrigNo_BiCell()
        {
            TrigNoBiCell = uint.Parse(Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "trigNoBiCell"));

            if (TrigNoBiCell < uint.MaxValue)
                TrigNoBiCell++;
            else
                TrigNoBiCell = 1;
            SetReg(_gapSystem.RegPathGapStatus, "trigNoBiCell", TrigNoBiCell.ToString());
        }
        public void Inspect_Run_Run_Read_Reg_NowAccount()
        {
            _nowAccount = Inspect_Run_Ready_Get_RegData_Gap(_gapSystem.RegPathGapStatus, "nowAccount");

            SetReg(_gapSystem.RegPathGapStatus, "nowAccount", _nowAccount);
        }
        */

        public void GapBiCell_Status_Set_To_Reg()
        {
            NowTrigNumber_Gap = uint.Parse(GetReg(_gapSystem.RegPathGapStatus, "trigNoGap"));
            NowFailNumber_Gap = uint.Parse(GetReg(_gapSystem.RegPathGapStatus, "NGCountGap"));
            NowProdectNumber_Gap = uint.Parse(GetReg(_gapSystem.RegPathGapStatus, "ProductNoGap"));

            NowTrigNumber_BiCell = uint.Parse(GetReg(_gapSystem.RegPathGapStatus, "trigNoBiCell"));
            NowFailNumber_BiCell = uint.Parse(GetReg(_gapSystem.RegPathGapStatus, "NGCountBiCell"));
            NowProdectNumber_BiCell = uint.Parse(GetReg(_gapSystem.RegPathGapStatus, "ProductNoBiCell"));
        }

        public uint GetSet_NowTrigger_No
        {
            get { return NowTrigNumber_Gap; }
            set { NowTrigNumber_Gap = value; }
        }

        private void Inspect_Get_NowCell_Data()
        {
            //_plc.PCL_WriteData_D4045(0);

            if (NowCellNumber == 1)
                NowROI_Start = 4;
            else if (NowCellNumber == 2)
                NowROI_Start = 8;
            else
                NowROI_Start = 0;
            NowCellNumber++;
        }

        public void Inspect_Manual_Image_Grabing(int grabNo)
        {
            _dtInspDataSaveTime = DateTime.Now;
            //레지스트리로 부터 갭번호를 읽어온다.
            NowGapNumber = Inspect_Run_Run_Get_Reg_NowGapNo();
            //로딩한 이미지의 ROI 처리를 진행한다.
            Inspect_Run_Run_Image_Scan();
            Inspect_Run_Run_Drawing_Result();
        }

        public bool Inspect_Run_Run_Image_Scan()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            bool areaResult = true;
            var resultPoint = new CvPoint(0, 0);
            cvPntLstWidthData.Clear();
            cvPntLstCenterPoint.Clear();
            //CvWindow.ShowImages(_nowIplImage);

            tempImage.ResetROI();
            _nowIplImage.Copy(tempImage);


            //for (int i = NowROI_Start; i < NowROI_Start + 4; i++)
            for (int i = 0; i < _gapSystem.RectListImageZone_Gap.Count; i++)
            {
                NowROI_Number = i;
                var cuttingimage = new IplImage(_gapSystem.RectListImageZone_Gap[i].Width, _gapSystem.RectListImageZone_Gap[i].Height, BitDepth.U8, 3);
                Cv.SetImageROI(tempImage, new CvRect(_gapSystem.RectListImageZone_Gap[i].Location, _gapSystem.RectListImageZone_Gap[i].Size));
                
                tempImage.Copy(cuttingimage);

                //CvWindow.ShowImages(cuttingimage);

                bool houghLineCheck = false;
                bool lineCenterCheck = false;
                _iEdgeParam1 = Threshold_01[0];
                _iEdgeParam2 = Threshold_01[1];
                HoughRetryCount = 0;
                resultPoint.X = 0;
                resultPoint.Y = 0;
                //현재 검출을 진행하고 있는 영역의 이미지를 표시해 주는 함수이다.
                //테스트 진행시 디버깅용으로사용되는 함수임.
                //Inspect_Run_Run_Test_DisplayZone_Gray(cuttingimage, _iCany1, _ICany2);
                do
                {
                    houghLineCheck = false;
                    lineCenterCheck = false;

                    List<CvPoint> FindedPoints = imageHougher.HoughLines_Point(cuttingimage, _iEdgeParam1, _iEdgeParam2, _iLineParam1, _iLineParam2, _iLineParam3);
                    HoughRetryCount++;
                    if (HoughRetryCount == 1)
                    {
                        _iEdgeParam1 = Threshold_02[0];
                        _iEdgeParam2 = Threshold_02[1];
                    }
                    else
                    {
                        _iEdgeParam1 = _iEdgeParam1 - _iEdgeParam3;
                        _iEdgeParam2 = _iEdgeParam2 - _iEdgeParam3;
                    }

                    //허프변환 후 찾은 포이트가 없고 재시도 횟수가 5보다 작으면 참.
                    if ((FindedPoints.Count == 0) && (HoughRetryCount < 5)) houghLineCheck = true;
                    else
                    {
                        //여러번 반복해서도 값을 찾지 못했을 때 실패을 리턴한다.
                        //재시도가 5이고 허프에서 포인트를 찾지 못했다면 실패을 리턴한다.
                        //if ((HoughRetryCount > 4) && (imageHougher.GetSetFindedPoints.Count == 0))
                        //    return false;

                        if ((HoughRetryCount > 4) && (FindedPoints.Count == 0))
                        {
                            areaResult = false;
                            break;
                        }


                        resultPoint = Inspect_Run_Run_02(cuttingimage, FindedPoints, iLstNowSidNo[i], sLstNowPolNo[i], sLstNowTypNo[i]);
                        if (resultPoint.X == 0 && resultPoint.Y == 0) lineCenterCheck = true;

                        //여러번 반복해서도 값을 찾지 못했을 때 거짓을 리턴한다.
                        //재시도가 5이고 포인트도 찾지 못했다면 거짓을 리턴한다.
                        //if ((HoughRetryCount > 4) && lineCenterCheck)
                        //    return false;

                        if ((HoughRetryCount > 4) && lineCenterCheck)
                        {
                            areaResult = false;
                            break;
                        }
                    }
                } while (houghLineCheck || lineCenterCheck);

                if (sLstNowTypNo[i] == "거리") cvPntLstCenterPoint.Add(resultPoint);
            }

            return areaResult;
        }

        private CvPoint Inspect_Run_Run_02(IplImage zoneImage, List<CvPoint> findedPoints, int sideData, string polaData, string typeData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var resurtPoint = new CvPoint(0, 0);

            for (int i = 0; i < findedPoints.Count/2; i++)
            {
                CvPoint startPoint = findedPoints[i*2];
                CvPoint endPoint = findedPoints[i*2 + 1];

                CvPoint centerPoint = Inspect_Run_Run_Finded_Lines_CenterPoint_Check(startPoint, endPoint);
                if (centerPoint.X < 0 || centerPoint.Y < 0)
                {
                    return resurtPoint;
                }


                CvPoint[] measurePoints = Inspect_Run_Run_Finded_Lines_MeasurePoint_Check(centerPoint, sideData);
                if (measurePoints[0].X >= zoneImage.Width || measurePoints[1].X >= zoneImage.Width ||
                    measurePoints[0].Y >= zoneImage.Height || measurePoints[1].Y >= zoneImage.Height ||
                    measurePoints[0].X < 0 || measurePoints[1].X < 0 || measurePoints[0].Y < 0 || measurePoints[1].Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    return resurtPoint;
                }


                bool usedLineCheck = Inspect_Run_Run_Finded_Lines_UsingLine_Check(zoneImage, measurePoints, polaData,
                    sideData);
                if (usedLineCheck)
                {
                    if (typeData == "넓이")
                    {
                        //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                        cvPntLstWidthData.Add(startPoint);
                        cvPntLstWidthData.Add(endPoint);
                        cvPntLstCenterPoint.Add(startPoint);
                        cvPntLstCenterPoint.Add(endPoint);
                    }

                    return centerPoint;
                }
            }


            return resurtPoint;
        }

        private void Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(IplImage cuttingimage, CvPoint[] measPoint,
            CvScalar scOne, CvScalar scTwo)
        {
            cuttingimage.Circle(measPoint[0].X, measPoint[0].Y, 2, CvColor.Blue, 1, LineType.AntiAlias, 0);
            cuttingimage.Circle(measPoint[1].X, measPoint[1].Y, 2, CvColor.Blue, 1, LineType.AntiAlias, 0);

            var cvFont = new CvFont(FontFace.Italic, 0.5, 0.5);
            cuttingimage.PutText("1" + scOne[0], measPoint[0], cvFont, 255);
            cuttingimage.PutText("2" + scTwo[0], measPoint[1], cvFont, 255);

            CvWindow.ShowImages(cuttingimage);
        }

        //허프에서 발경한 3개의 라인 중에서 설정 조건에 
        //1.거리, 넓이, 2,극성 의 설정 조건에 해당하는 //라인을 찾는다.
        //만약 라인이 없다면 어떻게 할 것인지를 결정해야한다.
        //다시 허프조건을 변경해서 찾을 것인지 판다해서
        //재 검색 모듈을 제작해야한다.
        private bool Inspect_Run_Run_Finded_Lines_UsingLine_Check(IplImage zoneImage, CvPoint[] measPoint,
            string polaData, int sideData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            int intMethod = -1;
            if (polaData == "흑백") intMethod = 0;
            else if (polaData == "백흑") intMethod = 1;

            //만약 측정좌표가 이미지의 크기에서 벗어났는지를 검사한다.
            //벗어났으면 False를 리턴한다. 이때 어떻게 할것이지 결졍해서 추가 해야함.
            if (measPoint[0].X >= zoneImage.Width || measPoint[1].X >= zoneImage.Width ||
                measPoint[0].Y >= zoneImage.Height || measPoint[1].Y >= zoneImage.Height ||
                measPoint[0].X < 0 || measPoint[1].X < 0 || measPoint[0].Y < 0 || measPoint[1].Y < 0)
                return false;

            CvScalar v1 = zoneImage.Get2D(measPoint[0].Y, measPoint[0].X);
            CvScalar v2 = zoneImage.Get2D(measPoint[1].Y, measPoint[1].X);
            int iGridRow = iLstNowRowNo[NowROI_Number];
            int BrightDeferentValue = 0;
            bool result = int.TryParse(_gapSystem.StrLstRcpConGridData_Gap[(iGridRow*11) + 10], out BrightDeferentValue);
            if (result == false) return false;

            //string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
            //if (NowROI_Number == 2)
            //Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(zoneImage, measPoint, v1, v2);

            bool checkResult = false;
            if (intMethod == 0 && sideData == 0)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 1)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 2)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 3)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 0)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 1)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 2)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 3)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            return false;
        }

       

        private CvPoint[] Inspect_Run_Run_Finded_Lines_MeasurePoint_Check(CvPoint centerPoint, int sideData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);


            var measPoint1 = new CvPoint(0, 0);
            var measPoint2 = new CvPoint(0, 0);

            //sideData = 0, 2이면 받은 이미지를 Y측으로 분할 해서 분할된 이미지의 밝기를 비교한다.
            //sideData = 1, 3이면 받은 이미지를 X측으로 분할 해서 분할된 이미지의 밝기를 비교한다.
            //측정 위치는 X,Y측의 중간값의 중심으로 각각 5씩 이동한 곳에서 측정한다.

            switch (sideData)
            {
                    //위쪽을 향하도록 설정 시
                case 0:
                    measPoint1.X = centerPoint.X;
                    measPoint1.Y = centerPoint.Y - shiftCenter;

                    measPoint2.X = centerPoint.X;
                    measPoint2.Y = centerPoint.Y + shiftCenter;
                    break;
                case 1:
                    measPoint1.X = centerPoint.X - shiftCenter;
                    measPoint1.Y = centerPoint.Y;

                    measPoint2.X = centerPoint.X + shiftCenter;
                    measPoint2.Y = centerPoint.Y;
                    break;
                case 2:
                    measPoint1.X = centerPoint.X;
                    measPoint1.Y = centerPoint.Y - shiftCenter;

                    measPoint2.X = centerPoint.X;
                    measPoint2.Y = centerPoint.Y + shiftCenter;
                    break;
                case 3:
                    measPoint1.X = centerPoint.X - shiftCenter;
                    measPoint1.Y = centerPoint.Y;

                    measPoint2.X = centerPoint.X + shiftCenter;
                    measPoint2.Y = centerPoint.Y;
                    break;
            }
            var measPoints = new CvPoint[2];
            measPoints[0] = measPoint1;
            measPoints[1] = measPoint2;

            //Trace.WriteLine(centerPoint.X.ToString() + ":" + centerPoint.Y.ToString() + "   " + measPoint1.X.ToString() + ":" + measPoint1.Y.ToString() + "   " + measPoint2.X.ToString() + ":" + measPoint2.Y.ToString());

            return measPoints;
        }

        private CvPoint Inspect_Run_Run_Finded_Lines_CenterPoint_Check(CvPoint pt1, CvPoint pt2)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var pt = new CvPoint(0, 0);

            float a;
            if (pt1.X == pt2.X)
            {
                pt.X = pt2.X;
                if (pt1.Y > pt2.Y)
                    pt.Y = (pt1.Y - pt2.Y)/2 + pt2.Y;
                else
                    pt.Y = (pt2.Y - pt1.Y)/2 + pt1.Y;
                return pt;
            }
            if (pt1.Y == pt2.Y)
            {
                if (pt1.X > pt2.X)
                    pt.X = (pt1.X - pt2.X)/2 + pt2.X;
                else
                    pt.X = (pt2.X - pt1.X)/2 + pt1.X;
                pt.Y = pt2.Y;
                return pt;
            }
            a = (pt2.Y - pt1.Y)/(float) (pt2.X - pt1.X);

            if (a < 1)
            {
                if (pt1.X > pt2.X)
                    pt.X = (pt1.X - pt2.X)/2 + pt2.X;
                else
                    pt.X = (pt2.X - pt1.X)/2 + pt1.X;
                pt.Y = pt2.Y;
                return pt;
            }
            float b = pt2.Y - (a*pt2.X);
            if (b < 1)
            {
                pt.X = pt2.X;
                if (pt1.Y > pt2.Y)
                    pt.Y = (pt1.Y - pt2.Y)/2 + pt2.Y;
                else
                    pt.Y = (pt2.Y - pt1.Y)/2 + pt1.Y;
                return pt;
            }

            double d = Math.Sqrt(Math.Pow((pt1.X - pt2.X), 2) + Math.Pow((pt1.Y - pt2.Y), 2));
            int xdist = (int) d/2;

            float A = a*a + 1;
            float B = 2*(-pt1.X + a*b - a*pt1.Y);
            float C = pt1.X*pt1.X + b*b - 2*b*pt1.Y*pt1.Y - (xdist*xdist);

            if (pt1.X < pt2.X)
            {
                string strfloat1 = ((float) (-B + Math.Sqrt(B*B - 4*A*C))/(2*A)).ToString();
                pt.X = (int) ((-B + Math.Sqrt(B*B - 4*A*C))/(2*A));
            }
            else
            {
                string strfloat2 = ((float) (-B - Math.Sqrt(B*B - 4*A*C))/(2*A)).ToString();
                pt.X = (int) ((-B - Math.Sqrt(B*B - 4*A*C))/(2*A));
            }

            string strfloat3 = ((int) (a*pt.X + b)).ToString();
            pt.Y = (int) (a*pt.X + b);
            return pt;
        }

        public void Inspect_Run_Run_Drawing_Result()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_ROI_CenterPoint_Find();

            if (Run_Mode == "Auto") 
                Inspect_Run_Run_FindData_Inspection();
            else 
                Inspect_Run_Run_FindData_Inspection_Manual();

            //검출이 정상적으로 진행되었을 땡때이이
            if (strResultNgOk[2] == "OK")
            {
                //_plc.PCL_WriteData_D4045(0);
               
            }
                
            //스펙에서 에러일때
            else if (strResultNgOk[2] == "NG")
            {
                //_plc.PCL_WriteData_D4045(1);
                //OperationEvent_Gap2(NowGapNumber, NowProdectNumber_Gap);
               
            }


            //저장 쓰레드와 분석 쓰레드가 독립적으로 구동하기 때문에
            //히스토리부분에서 싱크를 시키기 위해서 번호를 설정한다.
            _iHistoryViewNo = _iImgCount;

            //쓰레드에서 아래 두개의 멤버에 접근하기 전에 메인 쓰레드에서
            //이미 값이 변경될 가능성이 있어 다른 메버에 복사하여 복사된
            //멤버의 값을 사용하여 저장 데이터를 만든다.
            FormDlgInsp_Inspection_Save_Data_Copy();

            Tack_Time_Watch_Gap.Stop();
            //MessageBox.Show(Tack_Time_Watch_Gap.ElapsedMilliseconds.ToString());

            if (strResultNgOk[2] == "NONE")
                Inspect_Run_Run_Inspect_None_Display();
            else
                //검사 결과를 화면에 표시해 준다.
                Inspect_Run_Run_Inspect_Result_Display();

            if (Run_Mode != "Manual")
                Inspect_Run_Run_Data_Save();
        }

        public string GetSet_RunMode
        {
            get { return Run_Mode; }
            set { Run_Mode = value; }
        }

        Stopwatch Tack_Time_Watch_Gap = new Stopwatch();

        private void Inspect_Run_Run_Data_Save()
        {
            Inspect_Auto_Data_Set();

            
            //엑셀 파일에 결과를 저장한다.
            _dtImageSaveTime = DateTime.Now;

            //갭토탈에 사용되는 결과값 기록
            if (strLstDisplayData[2] == "NG")
                SetReg(_gapSystem.RegPathGapTotal, "ResultData", strLstDisplayData[2]);

            //갭토탈에 사용되는 측정값 기록
            string RegTitleL = "GapL" + NowGapNumber.ToString("0");
            string RegTitleR = "GapR" + NowGapNumber.ToString("0");
            SetReg(_gapSystem.RegPathGapTotal, RegTitleL, strLstDisplayData[3]);
            SetReg(_gapSystem.RegPathGapTotal, RegTitleR, strLstDisplayData[5]);

            GapTotal_Write_Flag = true;

            //엑셀에 데이터를 저장하는 메소드가 실행시간의 정유율을 높게 차지 하기
            //때문에 뒤의 두개의 함수와 같은 경우로 같이 쓰레드로 처리한다.
            //string strtmp = _alignSystem.StrListSysConTitle[0];
            //string strtmp1 = _alignSystem.StrListSysConData[1];
            if ((passSaveData == "ON" && _strSavedInspectResult == "OK") ||
                (failSaveData == "ON" && _strSavedInspectResult == "NG") ||
                (failSaveData == "ON" && _strSavedInspectResult == "NONE"))
            {
                Excel_Write_Thread = new Thread(FormDlgInsp_Inspection_Excel_Data_Write);
                Excel_Write_Thread.Start();
            }

            if ((passSaveImage == "ON" && _strSavedInspectResult == "OK") ||
                (failSaveImage == "ON" && _strSavedInspectResult == "NG") ||
                (failSaveImage == "ON" && _strSavedInspectResult == "NONE"))
            {
                File_Save_Thread = new Thread(Inspect_Run_Run_SaveImage_To_File);
                File_Save_Thread.Start();
            }


            History_All_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_All);
            History_All_Save_Thread.Start();

            if (_strSavedInspectResult == "NG" || _strSavedInspectResult == "NONE")
            {
                History_NG_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_NG);
                History_NG_Save_Thread.Start();
            }
        }

        public void Inspect_Auto_Data_Set()
        {
            NowImageFolderSavePath = _gapSystem.StrListSysConData[2];
            NowExcelFolderSavePath = _gapSystem.StrListSysConData[5];
            _strSavedInspectResult = _strSavedNGOK[2];
            passSaveImage = _gapSystem.StrListSysConData[0];
            failSaveImage = _gapSystem.StrListSysConData[1];
            passSaveData = _gapSystem.StrListSysConData[3];
            failSaveData = _gapSystem.StrListSysConData[4];
        }

        //기존의 히스토리 픽처박스 번호가 처리용 프로세스에서 이미 다음 것을 진행하기
        //때문에 번호가 씽크되지 않아서 _iImgCount 대신에 표시부 쓰레드 시작하기 전에
        //_iImgCount 를 _iHistoryViewNo에 넘져준다. 
        private void Inspect_Run_Run_Drawing_Result_To_History_All()
        {
            if (uPanel_History_All.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_All;
                uPanel_History_All.Invoke(del);
            }
            else
            {
                //Inspect_uTab_ImageList22.SelectedTab = Inspect_uTab_ImageList22.Tabs["Inspect_ImageAll"];
#if (!TIME_TEST)
                ((PictureBoxIpl) (uPanel_History_All.ClientArea.Controls[_iAllHistoryViewNo])).ImageIpl = _NowSavedImage;
                uPanel_History_All.ClientArea.Controls[_iAllHistoryViewNo].Refresh();
#endif
                if (_iAllHistoryViewNo + 1 == 11) _iAllHistoryViewNo = 0;
                else _iAllHistoryViewNo++;
            }
        }

        private void Inspect_Run_Run_Drawing_Result_To_History_NG()
        {
            if (uPanel_History_NG.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_NG;
                uPanel_History_NG.Invoke(del);
            }
            else
            {
                if (ZeroCheck_Small || _strSavedInspectResult == "NG")
                {
                    //Inspect_uTab_ImageList22.SelectedTab = Inspect_uTab_ImageList22.Tabs["Inspect_ImageFail"];

                    ((PictureBoxIpl) (uPanel_History_NG.ClientArea.Controls[_iNGHistoryViewNo])).ImageIpl =
                        _NowSavedImage;
                    uPanel_History_NG.ClientArea.Controls[_iNGHistoryViewNo].Refresh();

                    if (_iNGHistoryViewNo + 1 == 11) _iNGHistoryViewNo = 0;
                    else _iNGHistoryViewNo++;
                }
            }
        }


        private void Inspect_Run_Run_SaveImage_To_File()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //string tmpstr1 = _alignSystem.StrListSysConTitle[0];
            //string tmpstr2 = _alignSystem.StrListSysConName[0];
            //string tmpstr3 = _alignSystem.StrListSysConData[0];

            string imageSaveFolderName = Inspect_Set_FolderName_ImageFile(_dtInspDataSaveTime);

            string imageFileName = _iSavedTrigNumber.ToString("0000000000") + " "
                                   + _iSavedGap_Number.ToString("00") + " " + _strSavedInspectResult +
                                   String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                                       _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                                       _dtInspDataSaveTime.Hour,
                                       _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                                       _dtInspDataSaveTime.Millisecond);

            string imageFilePath = imageSaveFolderName + "\\" + imageFileName + ".jpg";

            _strHisImgName = imageFilePath;

            //이미지에 검사결과를 기록하기 전에 복사해 놓은 이미지를 저장한다.
            _NowSavedImage.SaveImage(imageFilePath);
        }

        private string Inspect_Set_FolderName_ImageFile(DateTime checkTime)
        {
            var fileSystem = new Control_Files();

            string folderName = NowImageFolderSavePath + "\\Gap" +
                                String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            folderName = folderName +
                         String.Format("\\{0:00}년{1:00}월{2:00}일", checkTime.Year, checkTime.Month, checkTime.Day);


            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);

            return folderName;
        }
        /*
        
        public void FormDlgInsp_Inspection_Excel_Data_Write()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);


            _dtInspDataSaveTime = DateTime.Now;
            var tmpDateTime = new DateTime();
            while (_dtInspDataSaveTime.Date == tmpDateTime.Date)
            {
                _dtInspDataSaveTime = DateTime.Now;
            }


            string passSave = _gapSystem.StrListSysConData[3];
            string failSave = _gapSystem.StrListSysConData[4];

            if (passSave == "OFF" && _strSavedInspectResult == "OK") return;
            if (failSave == "OFF" && _strSavedInspectResult == "NG") return;

            string nowExcelFileName = Inspect_Set_FileName_ExcelFile(_dtInspDataSaveTime);
            excelFile.WriteExcelFile(nowExcelFileName, strLstDisplayData);
            //2.판정3Left.5Right
            //NowGapNumber
            
            Gap_NgOk_Array.Add(strLstDisplayData[2]);
            
            string nowExcelFileName_Total = Inspect_Set_FileName_ExcelFile_Total(_dtInspDataSaveTime);
            if (NowGapNumber == 0)
            {
                string gridTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                _dtInspDataSaveTime.Hour,_dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

                Total_Data = gridTime + "," + strLstDisplayData[3] + "," + strLstDisplayData[5] + ",";
                excelFile.WriteExcelFile(nowExcelFileName_Total, Total_Data, false);
                return;
            }
            else if (NowGapNumber == 10)
            {
                //생산 수량을 업데이트한다.
                GetSet_NowProduct_Number();

                foreach (string NgOk in Gap_NgOk_Array)
                {
                    if (NgOk == "NG")
                    {
                        Total_Data = strLstDisplayData[3] + "," + strLstDisplayData[5]  + "," + "NG,";
                        excelFile.WriteExcelFile(nowExcelFileName_Total, Total_Data, true);
                        Gap_NgOk_Array.Clear();
                        return;
                    }
                }

                Total_Data = strLstDisplayData[3] + "," + strLstDisplayData[5] + "," + "OK,";
                excelFile.WriteExcelFile(nowExcelFileName_Total, Total_Data, true);

                
                Gap_NgOk_Array.Clear();
                
                return;
            }
            else
            {
                Total_Data = strLstDisplayData[3] + "," + strLstDisplayData[5]+",";
                excelFile.WriteExcelFile(nowExcelFileName_Total, Total_Data, false);
                return;
            }
        }
        */

        public void FormDlgInsp_Inspection_Excel_Data_Write()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);


            _dtInspDataSaveTime = DateTime.Now;
            var tmpDateTime = new DateTime();
            while (_dtInspDataSaveTime.Date == tmpDateTime.Date)
            {
                _dtInspDataSaveTime = DateTime.Now;
            }


            string passSave = _gapSystem.StrListSysConData[3];
            string failSave = _gapSystem.StrListSysConData[4];

            if (passSave == "OFF" && _strSavedInspectResult == "OK") return;
            if (failSave == "OFF" && _strSavedInspectResult == "NG") return;

            string nowExcelFileName = Inspect_Set_FileName_ExcelFile(_dtInspDataSaveTime);
            excelFile.WriteExcelFile(nowExcelFileName, strLstDisplayData);
        }

        //갭데이터를 레지에 저장했는지를 알리는 플래그
        //false이면 갭 번호가 0이어도 파이에 저장하지 않느다.
        private bool GapTotal_Write_Flag = false;
         
        /*
        public void FormDlgInsp_Inspection_Excel_GapTotal_Write()
        {
            gap_Total_Data.Clear();
            string TimeData = GetReg(_gapSystem.RegPathGapTotal, "TimeData");
            gap_Total_Data.Append(TimeData);
            gap_Total_Data.Append(",");
            //gap_Total_Data
            string CountCell = GetReg(_gapSystem.RegPathGapTotal, "CellCount");
            int CntNo = int.Parse(CountCell);

            for (int i = 0; i < CntNo; i++)
            {
                string tmpStrL = GetReg(_gapSystem.RegPathGapTotal, "GapL"+i.ToString("0"));
                gap_Total_Data.Append(tmpStrL);
                gap_Total_Data.Append(",");
                string tmpStrR = GetReg(_gapSystem.RegPathGapTotal, "GapR" + i.ToString("0"));
                gap_Total_Data.Append(tmpStrR);
                gap_Total_Data.Append(",");
            }

            string ResultData = GetReg(_gapSystem.RegPathGapTotal, "ResultData");
            gap_Total_Data.Append(ResultData);
            gap_Total_Data.Append(",");

            string nowExcelFileName_Total = Inspect_Set_FileName_ExcelFile_Total(_dtInspDataSaveTime);
            excelFile.WriteExcelFile(nowExcelFileName_Total, gap_Total_Data);
            
            //생산 수량을 업데이트한다.
            GetSet_NowProduct_Number();

            string saveTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                _dtInspDataSaveTime.Hour, _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

            SetReg(_gapSystem.RegPathGapTotal, "TimeData", saveTime);
            SetReg(_gapSystem.RegPathGapTotal, "CellCount", NowModel_Cells.ToString("0"));
            SetReg(_gapSystem.RegPathGapTotal, "ResultData", "OK");
            for (int i = 0; i < CntNo; i++)
            {
                string tmpStrL = "GapL" + i.ToString("0");
                SetReg(_gapSystem.RegPathGapTotal, tmpStrL, "0");
                string tmpStrR = "GapR" + i.ToString("0");
                SetReg(_gapSystem.RegPathGapTotal, tmpStrR, "0");
            }
        }
        */

        StringBuilder gap_Total_Data = new StringBuilder();
        //public List<string> StrLstGapTotalTitle = new List<string>();
        //public List<string> StrLstGapTotalData = new List<string>();
        //public string RegPathGapTotal = "Software\\ShinJin M Tec\\LNS System\\System Gap\\GapTotalData";
        public void GetSet_NowProduct_Number()
        {
            if (NowGapNumber == (NowModel_Cells-1))
            {
                NowProdectNumber_Gap = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "ProductNoGap"));
                if (NowProdectNumber_Gap < uint.MaxValue)
                    NowProdectNumber_Gap++;
                else
                    NowProdectNumber_Gap = 1;
                SetReg(_gapSystem.RegPathGapStatus, "ProductNoGap", NowProdectNumber_Gap.ToString());
            }
        }
        public string Inspect_Run_Ready_TrigNo_Reg_To_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
        }
        private string Total_Data = string.Empty;
        List<string> Gap_NgOk_Array = new List<string>();
        //이를 CSV파일로 변경한다.
        private string Inspect_Set_FileName_ExcelFile(DateTime checkTime)
        {
            string folderName = NowExcelFolderSavePath + "\\Gap" +
                                String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}년{1:00}월{2:00}일 Gap Vision.csv", checkTime.Year, checkTime.Month,
                                  checkTime.Day);

            excelFile.Excel_Folder_Check_Or_Make(folderName);
            excelFile.Excel_File_Check_Or_Make_Gap(fileName);


            return fileName;
        }

        private string Inspect_Set_FileName_ExcelFile_Total(DateTime checkTime)
        {
            string folderName = NowExcelFolderSavePath + "\\Gap" + String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string fileName = folderName + String.Format("\\{0:00}년{1:00}월{2:00}일 Gap Total.csv", checkTime.Year, checkTime.Month, checkTime.Day);

            excelFile.Excel_Folder_Check_Or_Make(folderName);
            excelFile.Excel_File_Check_Or_Make_Gap_Total(fileName);

            return fileName;
        }

        private void Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check()
        {
            var tempFloats = new[] {0f, 0f};

            _pointAnalize.BoxVsImage(_imageBoxIpl, Resources.empty, ref tempFloats);
            _gapSystem.GetSet_System_Inspect_Zoom_X_Gap = tempFloats[0];
            _gapSystem.GetSet_System_Inspect_Zoom_Y_Gap = tempFloats[1];

            _gapSystem.RectListInspBoxZone_Gap.Clear();
            for (int i = 0; i < _gapSystem.RectListImageZone_Gap.Count; i++)
            {
                var tempRect = new Rectangle();
                _pointAnalize.ImageToBox(_gapSystem.RectListImageZone_Gap[i], ref tempRect,
                    _gapSystem.GetSet_System_Inspect_Zoom_X_Gap, _gapSystem.GetSet_System_Inspect_Zoom_Y_Gap);
                _gapSystem.RectListInspBoxZone_Gap.Add(tempRect);

                ////Trace.WriteLine(
                //    _gapSystem.RectListRecipeBoxZone[i].Width.ToString("000") + "  " + _gapSystem.RectListRecipeBoxZone[i].Height.ToString("000") + " : " +
                //    _gapSystem.RectListImageZone[i].Width.ToString("000") + "  " + _gapSystem.RectListImageZone[i].Height.ToString("000") + " : " + 
                //    _gapSystem.RectListInspBoxZone[i].Width.ToString("000") + "  " + _gapSystem.RectListInspBoxZone[i].Height.ToString("000"));
            }
        }

        public void Inspect_Gap_Offset_Load_To_System()
        {

            Cal_Garo_Value = double.Parse(_gapSystem.StrListVisConData_Gap[0]);
            Cal_Sero_Value = double.Parse(_gapSystem.StrListVisConData_Gap[1]);
            Cal_Mid_Left = double.Parse(_gapSystem.StrListVisConData_Gap[2]);
            Cal_Mid_Righ = double.Parse(_gapSystem.StrListVisConData_Gap[3]);
            Cal_Big_Left = double.Parse(_gapSystem.StrListVisConData_Gap[4]);
            Cal_Big_Righ = double.Parse(_gapSystem.StrListVisConData_Gap[5]);
            Cal_Sml_Left = double.Parse(_gapSystem.StrListVisConData_Gap[6]);
            Cal_Sml_Righ = double.Parse(_gapSystem.StrListVisConData_Gap[7]);
        }

        private void Inspect_Run_Run_Inspect_Result_Display()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //ROI찾기 에서 중간에 종료 되었을때가 있는지 확인해야 한다.
            //중간에 종료가 되었다면 12개를 0으로 채워서 진행하도록 한다.
            if (_savedCvPntLstImagePoint.Count < 12)
            {
                MessageBox.Show("검출을 다 못찾아서 ImagePoint의 수량이 작다.");
                return;
            }
            Inspect_Run_Run_PictureBox_Reflash();

            List<CvPoint> BoxDrawPoint = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, _savedCvPntLstImagePoint);


            if (NowGapTypeNumber == 2)
            {
                //가로 라인을 그린다.
                //gc.DrawLine(myLinePen, BoxDrawPoint[4].X, BoxDrawPoint[4].Y, BoxDrawPoint[6].X, BoxDrawPoint[6].Y);
                //gc.DrawLine(myLinePen, BoxDrawPoint[5].X, BoxDrawPoint[5].Y, BoxDrawPoint[7].X, BoxDrawPoint[7].Y);
            }
            else if (NowGapTypeNumber == 3)
            {
                //가로 라인을 그린다.
                //gc.DrawLine(myLinePen, BoxDrawPoint[8].X, BoxDrawPoint[8].Y, BoxDrawPoint[10].X, BoxDrawPoint[10].Y);
                //gc.DrawLine(myLinePen, BoxDrawPoint[9].X, BoxDrawPoint[9].Y, BoxDrawPoint[11].X, BoxDrawPoint[11].Y);
            }
            else if (NowGapTypeNumber == 1)
            {
                //가로 라인을 그린다.
                //gc.DrawLine(myLinePen, BoxDrawPoint[0].X, BoxDrawPoint[0].Y, BoxDrawPoint[2].X, BoxDrawPoint[2].Y);
                //gc.DrawLine(myLinePen, BoxDrawPoint[1].X, BoxDrawPoint[1].Y, BoxDrawPoint[3].X, BoxDrawPoint[3].Y);
            }

            var _font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);



            string displayStr01 = "MAX : " + dRealMaxValue.ToString("0.000000");
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));
            string displayStr02 = "CENTER : " + dRealCenterValue.ToString("0.000000");
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(410, 600));
            string displayStr03 = "MIN : " + dRealMinValue.ToString("0.000000");
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));

            string displayStr04 = "GAP LEFT : " + _savedValue_Left.ToString("0.000000") + " " + _strSavedNGOK[0];
            gc.DrawString(displayStr04, _font, Brushes.LawnGreen, new PointF(200, 620));
            string displayStr05 = "GAP RIGHT : " + _savedValue_Right.ToString("0.000000") + " " + _strSavedNGOK[1];
            gc.DrawString(displayStr05, _font, Brushes.LawnGreen, new PointF(410, 620));
            string displayStr06 = "RESULT : " + _strSavedNGOK[2];
            gc.DrawString(displayStr06, _font, Brushes.LawnGreen, new PointF(620, 620));

            //픽박스에 인스첵 존을 표시해 주는 함수
            Inspect_Run_Run_Display_Arrow_ZoneBox();

            string gridTime = String.Format(" {0:00}.{1:00}.{2:00} {3:00}시{4:00}분{5:00}.{6:000}초",
                _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(gridTime);
            strLstDisplayData.Add(NowGapNumber.ToString("00"));
            strLstDisplayData.Add(_strSavedNGOK[2]);
            strLstDisplayData.Add(_savedValue_Left.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[0]);
            strLstDisplayData.Add(_savedValue_Right.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[1]);
            strLstDisplayData.Add(dRealCenterValue.ToString("0.0000"));
            //strLstDisplayData.Add(dRealPluseValue.ToString("0.0000"));
            //strLstDisplayData.Add(dRealMinusValue.ToString("0.0000"));
            strLstDisplayData.Add(dRealMaxValue.ToString("0.0000"));
            strLstDisplayData.Add(dRealMinValue.ToString("0.0000"));
        }

        private void Inspect_Run_Run_Inspect_None_Display()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_PictureBox_Reflash();
            _strSavedNGOK[0] = "NG";
            _strSavedNGOK[1] = "NG";
            _strSavedNGOK[2] = "NONE";
            myLinePen.Color = Color.LawnGreen;

            var _font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);

            string displayStr01 = "MAX : " + dRealMaxValue.ToString("0.000000");
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));
            string displayStr02 = "CENTER : " + dRealCenterValue.ToString("0.000000");
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(410, 600));
            string displayStr03 = "MIN : " + dRealMinValue.ToString("0.000000");
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));

            string displayStr04 = "GAP LEFT : " + _savedValue_Left.ToString("0.000000") + " " + _strSavedNGOK[0];
            gc.DrawString(displayStr04, _font, Brushes.LawnGreen, new PointF(200, 620));
            string displayStr05 = "GAP RIGHT : " + _savedValue_Right.ToString("0.000000") + " " + _strSavedNGOK[1];
            gc.DrawString(displayStr05, _font, Brushes.LawnGreen, new PointF(410, 620));
            string displayStr06 = "RESULT : " + _strSavedNGOK[2];
            gc.DrawString(displayStr06, _font, Brushes.LawnGreen, new PointF(620, 620));

            //픽박스에 인스첵 존을 표시해 주는 함수
            //Inspect_Run_Run_Display_Arrow_ZoneBox();

            string gridTime = String.Format(" {0:00}.{1:00}.{2:00} {3:00}시{4:00}분{5:00}.{6:000}초",
                _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(gridTime);
            strLstDisplayData.Add(NowGapNumber.ToString("00"));
            strLstDisplayData.Add(_strSavedNGOK[2]);
            strLstDisplayData.Add(_savedValue_Left.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[0]);
            strLstDisplayData.Add(_savedValue_Right.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[1]);
            strLstDisplayData.Add(dRealCenterValue.ToString("0.0000"));
            strLstDisplayData.Add(dRealMaxValue.ToString("0.0000"));
            strLstDisplayData.Add(dRealMinValue.ToString("0.0000"));

        }

        private void Inspect_Run_Run_Display_Arrow_ZoneBox()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //이미지의 단위를 픽처박스의 단위로 변환해 준다.

            myLinePen.Color = Color.LawnGreen;
            myLinePen.Width = 2;
            myArrowPen.Width = 2;
            int pointCount = 0;
            int nowROI_Number = -1;

            if (NowGapTypeNumber == 2) nowROI_Number = 4;
            else if (NowGapTypeNumber == 3) nowROI_Number = 8;
            else nowROI_Number = 0;

            boxArrowPntLst = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, _savedCvPntLstImagePoint);
            for (int i = nowROI_Number; i < nowROI_Number + 4; i++)
            {
                var startPoint = new CvPoint(0, 0);
                var endPoint = new CvPoint(0, 0);

                startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X, boxArrowPntLst[i].Y);
                endPoint =
                    new CvPoint(
                        _gapSystem.RectListInspBoxZone_Gap[i].X + (_gapSystem.RectListInspBoxZone_Gap[i].Width),
                        boxArrowPntLst[i].Y);
                gc.DrawLine(myLinePen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);

                int tmpWidth = _gapSystem.RectListInspBoxZone_Gap[i].Width/4;
                if (pointCount == 0 || pointCount == 2)
                {
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*1), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*1),
                        boxArrowPntLst[i].Y - 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*2), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*2),
                        boxArrowPntLst[i].Y - 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*3), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*3),
                        boxArrowPntLst[i].Y - 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                }
                else if (pointCount == 1 || pointCount == 3)
                {
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*1), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*1),
                        boxArrowPntLst[i].Y + 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*2), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*2),
                        boxArrowPntLst[i].Y + 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*3), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth*3),
                        boxArrowPntLst[i].Y + 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                }
                pointCount++;
            }
        }


        private void Inspect_Run_Run_PictureBox_Reflash()
        {
            if (_imageBoxIpl.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_Inspection_Display del = Inspect_Run_Run_PictureBox_Reflash;
                _imageBoxIpl.Invoke(del);
            }
            else
            {
                _imageBoxIpl.ImageIpl = _nowIplImage;
                _imageBoxIpl.Refresh();
            }
        }

        
        public void FormDlgInsp_Inspection_Save_Data_Copy()
        {
            _nowIplImage.Copy(_NowSavedImage);

            if (strResultNgOk[2] != "NONE")
            {
                _iSavedGap_Number = NowGapNumber;
                _iSavedTrigNumber = NowTrigNumber_Gap;
                _savedValue_Left = dResultValueLeft;
                _savedValue_Right = dResultValueRight;
                _strSavedNGOK = strResultNgOk;
                _savedCvPntLstImagePoint = cvPntLstImagePoint;
            }
            else
            {
                //MessageBox.Show("엔지 발생함.");
                _savedValue_Left = 0.0;
                _savedValue_Right = 0.0;
                _strSavedNGOK = strResultNgOk;
                var zeroPoint = new CvPoint(0, 0);
                _savedCvPntLstImagePoint = new List<CvPoint>();
                for (int i = 0; i < 12; i++)
                {
                    _savedCvPntLstImagePoint.Add(zeroPoint);
                }
            }
        }


        public void Inspect_Run_Run_FindData_Inspection()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            
            ZeroCheck_Small = false;
            for (int i = 0; i < 4; i++)
            {
                if (cvPntLstImagePoint[i].X == 0 || cvPntLstImagePoint[i].Y == 0)
                {
                    ZeroCheck_Small = true;
                    break;
                }
            }

            ZeroCheck_Middle = false;
            for (int i = 4; i < 8; i++)
            {
                if (cvPntLstImagePoint[i].X == 0 || cvPntLstImagePoint[i].Y == 0)
                {
                    ZeroCheck_Middle = true;
                    break;
                }
            }

            ZeroCheck_Big = false;
            for (int i = 8; i < 12; i++)
            {
                if (cvPntLstImagePoint[i].X == 0 || cvPntLstImagePoint[i].Y == 0)
                {
                    ZeroCheck_Big = true;
                    break;
                }
            }

            //결과 값을 초기화 한다. 0:왼쪽, 1:오른쪽, 2:양쪽(최종) 결과값을 가진다.
            strResultNgOk[0] = "NG";
            strResultNgOk[1] = "NG";
            strResultNgOk[2] = "NG";

            //중,대, 소 순서대로 찾은 포인트를 확인해서 셋중 찾지 못한 포인트가 모두 
            //있다면 검출 실패로 판단한다.
            //PLC에 결과 값으로 0:OK, 1:NG, 2:None 으로 넘겨준다. 이 상황에서는 2를 넘겨준다.
            if (ZeroCheck_Small && ZeroCheck_Middle && ZeroCheck_Big)
            {
                //검출 실패 루틴을 진행하면 된다.
                strResultNgOk[2] = "NONE";
                dResultValueLeft = 0.0;
                dResultValueRight = 0.0;
                return;
            }


            if (ZeroCheck_Middle == false)
            {
                //갭 종류 번호를 초기화 한다. 이 길이의 갭은 0번으로 설정되어져 있다.
                NowGapTypeNumber = 2;

                //찾은 포인트의 거리를 구한다.
                iImgPixResultData1 = cvPntLstImagePoint[5].Y - cvPntLstImagePoint[4].Y;
                iImgPixResultData2 = cvPntLstImagePoint[7].Y - cvPntLstImagePoint[6].Y;

                //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                dResultValueLeft = (double) iImgPixResultData1 / Cal_Mid_Left;
                dResultValueRight = (double) iImgPixResultData2 / Cal_Mid_Righ;


                //////////////////////////////////////////////////////////////////////////////////////////
                //현재 측정된 값이 약 40 ~ 50 사이의 값이라면 현재 갭 번호를 0으로 설정하도록 한다.
                bool GapZeroCheck1 = false;
                bool GapZeroCheck2 = false;
                bool GapZeroCheck3 = false;
                if ((PLC_Model_Gap_Data[0] - 10.0) <= dResultValueLeft &&
                    (PLC_Model_Gap_Data[0] + 10) >= dResultValueLeft) GapZeroCheck1 = true;
                if ((PLC_Model_Gap_Data[0] - 10.0) <= dResultValueRight &&
                    (PLC_Model_Gap_Data[0] + 10) >= dResultValueRight) GapZeroCheck2 = true;
                if (GapZeroCheck1 && GapZeroCheck2) GapZeroCheck3 = true;

                if (GapZeroCheck3)
                {
                    //Inspect_Gap_Gap_Zero_Setting();
                    //OperationEvent_Gap1(NowGapNumber, NowProdectNumber_Gap);
                }
                //////////////////////////////////////////////////////////////////////////////////////////


                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                dRealCenterValue = PLC_Model_Gap_Data[NowGapNumber];

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = Gap_Data_Max_Array[NowGapNumber];
                dRealMinValue = Gap_Data_Min_Array[NowGapNumber];


                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK")
                {
                    strResultNgOk[2] = "OK";
                }

                return;
            }

            if (ZeroCheck_Big == false && NowGapNumber == 1)
            {
                //갭 종류 번호를 초기화 한다. 이 길이의 갭은 1번으로 설정되어져 있다.
                NowGapTypeNumber = 3;

                //찾은 포인트의 거리를 구한다.
                iImgPixResultData1 = cvPntLstImagePoint[9].Y - cvPntLstImagePoint[8].Y;
                iImgPixResultData2 = cvPntLstImagePoint[11].Y - cvPntLstImagePoint[10].Y;

                //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                dResultValueLeft = iImgPixResultData1/Cal_Big_Left;
                dResultValueRight = iImgPixResultData2/Cal_Big_Righ;

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                dRealCenterValue = PLC_Model_Gap_Data[NowGapNumber];

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = Gap_Data_Max_Array[NowGapNumber];
                dRealMinValue = Gap_Data_Min_Array[NowGapNumber];


                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";
                return;
            }


            //소폭 값이 정상적으로 검출되었을 때를 진행한다.
            if (ZeroCheck_Small == false && NowGapNumber != 1 && NowGapNumber != 0)
            {
                //갭 종류 번호를 초기화 한다. 이 길이의 갭은 0번으로 설정되어져 있다.
                NowGapTypeNumber = 1;

                //찾은 포인트의 거리를 구한다.
                iImgPixResultData1 = cvPntLstImagePoint[1].Y - cvPntLstImagePoint[0].Y;
                iImgPixResultData2 = cvPntLstImagePoint[3].Y - cvPntLstImagePoint[2].Y;

                //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                dResultValueLeft = iImgPixResultData1/Cal_Sml_Left;
                dResultValueRight = iImgPixResultData2/Cal_Sml_Righ;

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                dRealCenterValue = PLC_Model_Gap_Data[NowGapNumber];

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = Gap_Data_Max_Array[NowGapNumber];
                dRealMinValue = Gap_Data_Min_Array[NowGapNumber];

                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";
            }
        }

        private Thread Gap_Zero_Setting;
        /*
        public void Inspect_Gap_Gap_Zero_Setting()
        {
            if (GapTotal_Write_Flag == true)
            {
                Gap_Zero_Setting = new Thread(FormDlgInsp_Inspection_Excel_GapTotal_Write);
                Gap_Zero_Setting.Start();
            }

            NowGapNumber = 0;
            SetReg(_gapSystem.RegPathGapStatus, "NowGapNo", NowGapNumber.ToString());

            if (NowProdectNumber_Gap < uint.MaxValue)
                NowProdectNumber_Gap++;
            else
                NowProdectNumber_Gap = 1;
            SetReg(_gapSystem.RegPathGapStatus, "ProductNoGap", NowProdectNumber_Gap.ToString());
        }
        */
        public void Inspect_Run_Run_FindData_Inspection_Manual()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
           
            //Cal_Garo_Value = double.Parse(_gapSystem.StrListVisConData_Gap[0]);
            //Cal_Sero_Value = double.Parse(_gapSystem.StrListVisConData_Gap[1]);
            //Cal_Mid_Left = double.Parse(_gapSystem.StrListVisConData_Gap[2]);
            //Cal_Mid_Righ = double.Parse(_gapSystem.StrListVisConData_Gap[3]);
            //Cal_Big_Left = double.Parse(_gapSystem.StrListVisConData_Gap[4]);
            //Cal_Big_Righ = double.Parse(_gapSystem.StrListVisConData_Gap[5]);
            //Cal_Sml_Left = double.Parse(_gapSystem.StrListVisConData_Gap[6]);
            //Cal_Sml_Righ = double.Parse(_gapSystem.StrListVisConData_Gap[7]);

            ZeroCheck_Small = false;
            for (int i = 0; i < 4; i++)
            {
                if (cvPntLstImagePoint[i].X == 0 || cvPntLstImagePoint[i].Y == 0)
                {
                    ZeroCheck_Small = true;
                    break;
                }
            }

            ZeroCheck_Middle = false;
            for (int i = 4; i < 8; i++)
            {
                if (cvPntLstImagePoint[i].X == 0 || cvPntLstImagePoint[i].Y == 0)
                {
                    ZeroCheck_Middle = true;
                    break;
                }
            }

            ZeroCheck_Big = false;
            for (int i = 8; i < 12; i++)
            {
                if (cvPntLstImagePoint[i].X == 0 || cvPntLstImagePoint[i].Y == 0)
                {
                    ZeroCheck_Big = true;
                    break;
                }
            }

            //결과 값을 초기화 한다. 0:왼쪽, 1:오른쪽, 2:양쪽(최종) 결과값을 가진다.
            strResultNgOk[0] = "NG";
            strResultNgOk[1] = "NG";
            strResultNgOk[2] = "NG";

            //중,대, 소 순서대로 찾은 포인트를 확인해서 셋중 찾지 못한 포인트가 모두 
            //있다면 검출 실패로 판단한다.
            //PLC에 결과 값으로 0:OK, 1:NG, 2:None 으로 넘겨준다. 이 상황에서는 2를 넘겨준다.
            if (ZeroCheck_Small && ZeroCheck_Middle && ZeroCheck_Big)
            {
                //검출 실패 루틴을 진행하면 된다.
                strResultNgOk[2] = "NONE";
                dResultValueLeft = 0.0;
                dResultValueRight = 0.0;
                return;
            }


            if (ZeroCheck_Middle == false)
            {
                //갭 종류 번호를 초기화 한다. 이 길이의 갭은 0번으로 설정되어져 있다.
                NowGapTypeNumber = 2;

                //찾은 포인트의 거리를 구한다.
                iImgPixResultData1 = cvPntLstImagePoint[5].Y - cvPntLstImagePoint[4].Y;
                iImgPixResultData2 = cvPntLstImagePoint[7].Y - cvPntLstImagePoint[6].Y;

                //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                dResultValueLeft = iImgPixResultData1 / Cal_Mid_Left;
                dResultValueRight = iImgPixResultData2 / Cal_Mid_Righ;

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(_gapSystem.StrListVisConGridData_Uper[1], out dNumberCenter);
                dRealCenterValue = dNumberCenter;

                double dNumberPluse = 0.0;
                bool resultPluse = double.TryParse(_gapSystem.StrListVisConGridData_Uper[2], out dNumberPluse);

                double dNumberMinus = 0.0;
                bool resultMinus = double.TryParse(_gapSystem.StrListVisConGridData_Uper[3], out dNumberMinus);

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = dRealCenterValue + dNumberPluse;
                dRealMinValue = dRealCenterValue - dNumberMinus;


                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK")
                {
                    strResultNgOk[2] = "OK";
                }
                return;
            }

            if (ZeroCheck_Big == false && NowGapNumber == 1)
            {
                //갭 종류 번호를 초기화 한다. 이 길이의 갭은 1번으로 설정되어져 있다.
                NowGapTypeNumber = 3;

                //찾은 포인트의 거리를 구한다.
                iImgPixResultData1 = cvPntLstImagePoint[9].Y - cvPntLstImagePoint[8].Y;
                iImgPixResultData2 = cvPntLstImagePoint[11].Y - cvPntLstImagePoint[10].Y;

                //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                dResultValueLeft = iImgPixResultData1/Cal_Big_Left;
                dResultValueRight = iImgPixResultData2/Cal_Big_Righ;


                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(_gapSystem.StrListVisConGridData_Uper[5], out dNumberCenter);
                dRealCenterValue = dNumberCenter;

                double dNumberPluse = 0.0;
                bool resultPluse = double.TryParse(_gapSystem.StrListVisConGridData_Uper[6], out dNumberPluse);

                double dNumberMinus = 0.0;
                bool resultMinus = double.TryParse(_gapSystem.StrListVisConGridData_Uper[7], out dNumberMinus);

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = dRealCenterValue + dNumberPluse;
                dRealMinValue = dRealCenterValue - dNumberMinus;


                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";
                return;
            }


            //소폭 값이 정상적으로 검출되었을 때를 진행한다.
            if (ZeroCheck_Small == false && NowGapNumber != 1 && NowGapNumber != 0)
            {
                //갭 종류 번호를 초기화 한다. 이 길이의 갭은 0번으로 설정되어져 있다.
                NowGapTypeNumber = 1;

                //찾은 포인트의 거리를 구한다.
                iImgPixResultData1 = cvPntLstImagePoint[1].Y - cvPntLstImagePoint[0].Y;
                iImgPixResultData2 = cvPntLstImagePoint[3].Y - cvPntLstImagePoint[2].Y;

                //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                dResultValueLeft = iImgPixResultData1/Cal_Sml_Left;
                dResultValueRight = iImgPixResultData2/Cal_Sml_Righ;


//                 double dNumberCenter = 0.0;
//                 bool resultCenter = double.TryParse(_gapSystem.StrListVisConGridData_Uper[1], out dNumberCenter);
//                 dRealCenterValue = dNumberCenter;
// 
//                 double dNumberPluse = 0.0;
//                 bool resultPluse = double.TryParse(_gapSystem.StrListVisConGridData_Uper[4 * i + 2], out dNumberPluse);
//                 Gap_Data_Pluse_Array.Add(dNumberPluse);
// 
//                 double dNumberMinus = 0.0;
//                 bool resultMinus = double.TryParse(_gapSystem.StrListVisConGridData_Uper[4 * i + 3], out dNumberMinus);
//                 Gap_Data_Minus_Array.Add(dNumberMinus);

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(_gapSystem.StrListVisConGridData_Uper[(4*NowGapNumber) + 1],
                    out dNumberCenter);
                dRealCenterValue = dNumberCenter;

                double dNumberPluse = 0.0;
                bool resultPluse = double.TryParse(_gapSystem.StrListVisConGridData_Uper[(4*NowGapNumber) + 2],
                    out dNumberPluse);

                double dNumberMinus = 0.0;
                bool resultMinus = double.TryParse(_gapSystem.StrListVisConGridData_Uper[(4*NowGapNumber) + 3],
                    out dNumberMinus);

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = dRealCenterValue + dNumberPluse;
                dRealMinValue = dRealCenterValue - dNumberMinus;

                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";
            }
        }


        private void FormDlgInsp_Inspection_Write_LogFile(int msgNo)
        {
            var fileSystem = new Control_Files();

            DateTime logTime = DateTime.Now;
            string folderName = Environment.CurrentDirectory + "\\Log\\System\\" +
                                String.Format("{0:00}년{1:00}월", logTime.Year, logTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}-{1:00}-{2:00} Align Insp.log", logTime.Year, logTime.Month,
                                  logTime.Day);

            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);
            //해당 경로에 파일이 있는지 검사해서 없으면 생성한다.
            //Param 1 : 조사할 경로, Param 2 : 파일이 종류(1:login, 2:Inspect, 3:Excel)
            fileSystem.File_IO_Text_File_Check_Or_Make_Inspect(fileName);

            _dtInspLogFileTime = DateTime.Now;
            string logData = String.Format("[{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}] ",
                _dtInspLogFileTime.Year, _dtInspLogFileTime.Month, _dtInspLogFileTime.Day, _dtInspLogFileTime.Hour,
                _dtInspLogFileTime.Minute, _dtInspLogFileTime.Second, _dtInspLogFileTime.Millisecond);

            logData += FormDlgInsp_Inspection_LogData_Message_Make(msgNo);
            fileSystem.File_IO_Text_File_Write_Inspect(fileName, logData);
        }

        public string FormDlgInsp_Inspection_LogData_Message_Make(int msgNo)
        {
            string msgData = string.Empty;

            //1~100번 내부 함수 진행
            if (msgNo == 1) msgData = "검출 프로그램 기동 시작";
            else if (msgNo == 2) msgData = "검출 프로그램 기동 정지";
            else if (msgNo == 3) msgData = "이미지 분석 진행";
            else if (msgNo == 4) msgData = "이미지 저장 진행";

                //101~200번 부터는 그랩보드와 관련함수들
            else if (msgNo == 101) msgData = "트리거 신호 입력";
            else if (msgNo == 102) msgData = "이미지 그랩 진행";

                //201~300번 부터는 유멕과 통신을 진행하는 함수들
            else if (msgNo == 201) msgData = "현재 모델 정보 취득";
            else if (msgNo == 202) msgData = "현재 셀의 정보 취득";
            else if (msgNo == 203) msgData = "분석 결과값 전송 : 검출 OK";
            else if (msgNo == 204) msgData = "분석 결과값 전송 : 검출 NG";

            //301~400번 부터는 피엘씨와 통신을 진행하는 함수들

            //401~500번 부터는 엘브이에스와 통신을 진행하는 함수들

            return " " + msgData + "\r\n";
        }


        public void Inspect_Run_Run_Drawing_Result_Arrow_Make_V(CvPoint pt1, CvPoint pt2, ref CvPoint pt1R)
        {
            if (pt1.X == pt2.X)
            {
                pt1R.X = pt1.X;
                pt1R.Y = pt1R.Y;
                return;
            }

            double x = 0.0;
            double y = 0.0;

            double a = (double) (pt2.Y - pt1.Y)/(pt2.X - pt1.X);
            double b = pt2.Y - (a*pt2.X);
            y = pt1R.Y;
            x = (y - b)/a;
            pt1R.X = (int) x;
            pt1R.Y = (int) y;
        }

        public void Inspect_Run_Run_Drawing_Result_Arrow_Make_H(CvPoint pt1, CvPoint pt2, ref CvPoint pt1R)
        {
            if (pt1.Y == pt2.Y)
            {
                pt1R.X = pt1R.X;
                pt1R.Y = pt1.Y;
                return;
            }

            double x = 0.0;
            double y = 0.0;

            double a = (double) (pt2.Y - pt1.Y)/(pt2.X - pt1.X);
            double b = pt2.Y - (a*pt2.X);
            x = pt1R.X;
            y = a*x + b;
            pt1R.X = (int) x;
            pt1R.Y = (int) y;
        }

        public void Inspect_Run_Run_ROI_CenterPoint_Find()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            //타입별 시작 번호가 달라서 찾은 센터 포인트 주소를 만들어주어 카운트 한다.
            int cntPntAdd = 0;
            cvPntLstImagePoint.Clear();
            for (int i = 0; i < _gapSystem.RectListImageZone_Gap.Count; i++)
            {
                string tmpTypeNo = sLstNowTypNo[i];
                int tmpSeqNo = iLstNowSeqNo[i];
                int tmpRoiNo = iLstNowRoiNo[i];
                CvRect tmpRect = _gapSystem.RectListImageZone_Gap[tmpRoiNo];

                if (cvPntLstCenterPoint[cntPntAdd].X == 0 && cvPntLstCenterPoint[cntPntAdd].Y == 0)
                {
                    var zeroCvPoint = new CvPoint(0, 0);
                    cvPntLstImagePoint.Add(zeroCvPoint);

                    //디버깅 용으로 사용하는 함수 이다.
                    //검출 구역의 포인트를 표시해준다.
                    //Inspect_Run_Run_Drawing_Result_Location_Point(zeroCvPoint, i);
                }
                else
                {
                    CvPoint tmpPnt = cvPntLstCenterPoint[cntPntAdd];
#if(!DEBUG)
                    Trace.WriteLine(tmpRect.X.ToString("000  ") + tmpRect.Y.ToString("000  ") + tmpPnt.X.ToString("000  ") + tmpPnt.Y.ToString("000  "));
#endif
                    var rCvPoint = new CvPoint(tmpRect.X + tmpPnt.X, tmpRect.Y + tmpPnt.Y);
                    cvPntLstImagePoint.Add(rCvPoint);

                    //디버깅 용으로 사용하는 함수 이다.
                    //검출 구역의 포인트를 표시해준다.
                    //Inspect_Run_Run_Drawing_Result_Location_Point(rCvPoint, i);
                }
                cntPntAdd++;
            }

            //CvWindow.ShowImages(_nowIplImage);
        }

        private delegate void Delegate_Run_Run_Drawing_Result_Inspection_Display();

        private delegate void Delegate_Run_Run_Drawing_Result_To_File_Display();
    }
}