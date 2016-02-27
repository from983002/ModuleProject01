using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.Misc;
using Matrox.MatroxImagingLibrary;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.UserInterface;
using FontStyle = System.Drawing.FontStyle;
using Resources = SystemAlign.Properties.Resources;

namespace SystemAlign
{
    internal class Control_Inspect_BiCell
    {
        private readonly string[] ImageDataArray = new string[7];
        private readonly List<string> NowModel_Array = new List<string>();
        private readonly List<double> NowModel_GapData = new List<double>();
        private readonly int[] Threshold_01 = {150, 190};
        private readonly int[] Threshold_02 = {50, 100};
        private readonly List<CvPoint> cvPntLstImagePoint = new List<CvPoint>();
        private readonly Control_Excel excelFile = new Control_Excel();
        private  string failSaveData = string.Empty;
        private  string failSaveImage = string.Empty;
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
        private readonly List<string> strLstCellArrayData = new List<string>();
        private readonly List<string> strLstDisplayData = new List<string>();
        private readonly IplImage tempImage = new IplImage(2448, 2050, BitDepth.U8, 3);
        private readonly Control_UltraMessageBox uMessageBox = new Control_UltraMessageBox();
        private Thread BiCell_Manual;
        private string CallForm = "Main_Form";
        public List<CvPoint> CvPntLstCrossPoint = new List<CvPoint>();
        public List<CvPoint> CvPntLstThetaPoint = new List<CvPoint>();
        public CvPoint CvPntNowImageCentPnt = new CvPoint(0, 0);
        public double[] DSavedResultValue = new double[3];
        public double DistCenterGaro;
        public double DistCenterSero;
        public DateTime EndTime;
        private Thread Excel_Write_Thread;
        public bool FailZeroCheck1 = false;
        private Thread File_Save_Thread;
        private Thread History_All_Save_Thread;
        private Thread History_NG_Save_Thread;
        private int HoughRetryCount;
        public double ImageCenterGaro;
        public double ImageCenterSero;
        private MIL_ID MilApplication = MIL.M_NULL;
        private MIL_ID MilDigitizer = MIL.M_NULL;
        private MIL_ID MilDisplay = MIL.M_NULL;
        private MIL_ID MilImage = MIL.M_NULL;
        private MIL_ID MilSystem = MIL.M_NULL;
        private int NowCellDataCount = 3;
        private int NowCellGrabPos = 1;
        private int NowCellGrabber = 0;
        private int NowCellNumber = 0;
        private int NowCellType = 0;
        public double NowCenterGaro;
        public double NowCenterSero;
        private string NowExcelFolderSavePath;
        private string NowImageFolderSavePath;
        public string[] NowInspecOKorNg = {string.Empty, string.Empty, string.Empty};
        public string NowInspectResult = "OK"; //"NG"
        public double NowLengthGaro;
        public double NowLengthSero;
        private int NowModelDataCount = 6;
        private double NowModel_ATypeX;
        private double NowModel_ATypeY;
        private double NowModel_CTypeX;
        private double NowModel_CTypeY;
        private int NowModel_Cells;
        private int NowROI_Number = -1;
        public IplImage NowSavedImage = new IplImage(2448, 2050, BitDepth.U8, 3);
        public double RealCenterGaro;
        public double RealCenterSero;
        private double RectTheta = 0.0;
        private CvPoint RectTheta1 = new CvPoint(0, 0);
        private CvPoint RectTheta2 = new CvPoint(0, 0);
        private string Run_Mode = "Auto";
        public int SavedCellNumber = 0;
        public int SavedCellType = 0;
        public int SavedGrabber = 0;
        public double SavedLengthGaro = 0.0;
        public double SavedLengthSepa = 0.0;
        public double SavedLengthSero = 0.0;
        public uint SavedTrigNumber = 0;
        public string[] StrSavedInspectOkorNg = {string.Empty, string.Empty, string.Empty};
        public string StrSavedInspectResult = string.Empty;
        private Thread UmacWrite_P3702;
        private bool VisionJobWorking = true;
        private bool _FlagManual = true;
        private string[] _SavedImageData = new string[8];
        private double _dCalibration_GaRo;
        private double _dCalibration_SeRo;
        private double _dNow_Image_Garo;
        private double _dNow_Image_Sero;
        private double _dSaved_Center_Garo = 0.0;
        private double _dSaved_Center_Sero = 0.0;
        private DateTime _dtImageSaveTime;
        //Control_UMAC umac = new Control_UMAC();
        //측정한 제품의 중심위치를 저장한다.
        private DateTime _dtInspDataSaveTime;
        private DateTime _dtInspLogFileTime;
        private DateTime _dtUmacSetTime = new DateTime();
        private double[] _fResultValueLeft = new double[3];
        private double[] _fResultValueRight = new double[3];
        private CInspection_Lamination _gapSystem;
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
        private PictureBoxIpl _imageBoxIpl;
        private double _measureWidthResult = -1.0;
        private IplImage _nowIplImage;
        private PositionConvert _pointAnalize;
        private string _strHisImgName = string.Empty;
        private List<CvPoint> boxArrowPntLst = new List<CvPoint>();
        private AdjustableArrowCap cusCap;
        public List<CvPoint> cvPntLstCenterPoint = new List<CvPoint>();
        public List<CvPoint> cvPntLstWidthData = new List<CvPoint>();
        public List<bool> drawOKList = new List<bool>();
        public List<int> drawROIList = new List<int>();
        private Graphics gc;
        private int iStandardLong_A = 1700;
        private int iStandardLong_C = 1670;
        private int iStandardShort_A = 1280;
        private int iStandardShort_C = 1255;
        private int inspItemCount = 14;
        private string passSaveData = string.Empty;
        private string passSaveImage = string.Empty;
        public Point pntCenterMarkInspBox = new Point(0, 0);
        private int shiftCenter = 20;
        private string strImgSaveTimeDelivery = string.Empty;

        private string strInspect_RunMode = "Manual";
        private UltraPanel uPanel_History_All;
        private UltraPanel uPanel_History_NG;
        private Control_UMAC umac;

        public Control_Inspect_BiCell()
        {
        }


        public CInspection_Lamination GetSet_GapSystem
        {
            get { return _gapSystem; }
            set { _gapSystem = value; }
        }

        public Control_UMAC GetSet_UMACSystem
        {
            get { return umac; }
            set { umac = value; }
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

        public IplImage GetSet_NowImage
        {
            get { return _nowIplImage; }
            set { _nowIplImage = value; }
        }

        public Control_PLC GetSet_PLCSystem { get; set; }

        public IplImage GetSet_NowIplImage
        {
            get { return _nowIplImage; }
            set { _nowIplImage = value; }
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
        }//uPanel_History_NG

        public string GetSet_Calling_Form
        {
            get { return CallForm; }
            set { CallForm = value; }
        }

        public List<string> GridDisplayData
        {
            get { return strLstDisplayData; }
        }

        public event MyEventOneInsp_BiCell OperationEvent_BiCell;


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

            pntCenterMarkInspBox.X = _imageBoxIpl.Width/2;
            pntCenterMarkInspBox.Y = _imageBoxIpl.Height/2;

            //_dCalibration_GaRo = double.Parse(_gapSystem.StrListVisConData_BiCell[2]);
            //_dCalibration_SeRo = double.Parse(_gapSystem.StrListVisConData_BiCell[5]);

            _iEdgeParam1 = int.Parse(_gapSystem.StrListSysConData[30]);
            _iEdgeParam2 = int.Parse(_gapSystem.StrListSysConData[31]);
            _iEdgeParam3 = int.Parse(_gapSystem.StrListSysConData[32]);

            _iLineParam1 = int.Parse(_gapSystem.StrListSysConData[33]);
            _iLineParam2 = int.Parse(_gapSystem.StrListSysConData[34]);
            _iLineParam3 = int.Parse(_gapSystem.StrListSysConData[35]);

            _iGrabImageGaro = int.Parse(_gapSystem.StrListSysConData[36]);
            _iGrabImageSero = int.Parse(_gapSystem.StrListSysConData[37]);

            Threshold_01[0] = int.Parse(_gapSystem.StrListSysConData[30]);
            Threshold_01[1] = int.Parse(_gapSystem.StrListSysConData[31]);

            for (int i = 0; i < _gapSystem.RectListImageZone_BiCell.Count; i++)
            {
                iLstNowRowNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[4 + (i*inspItemCount)])); //Row
                iLstNowRoiNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[5 + (i*inspItemCount)])); //ROI
                sLstNowTypNo.Add(_gapSystem.StrLstRcpConInspData_BiCell[6 + (i*inspItemCount)]); //Type
                iLstNowSeqNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[7 + (i*inspItemCount)])); //Seq No
                iLstNowSidNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[8 + (i*inspItemCount)])); //Side No
                sLstNowPolNo.Add(_gapSystem.StrLstRcpConInspData_BiCell[9 + (i*inspItemCount)]); //극성
                iLstNowDivNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[10 + (i*inspItemCount)])); //분할
                sLstNowDisNo.Add(_gapSystem.StrLstRcpConInspData_BiCell[11 + (i*inspItemCount)]); //표시
                iLstNowLgtNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[12 + (i*inspItemCount)])); //밝기
                iLstNowLstNo.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[13 + (i*inspItemCount)])); //리스트 번호
                iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[5 + (i*inspItemCount)]));
//                 if (_gapSystem.StrLstRcpConInspData_BiCell[6 + (i*inspItemCount)] == "위치")
//                 {
//                     iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[5 + (i*inspItemCount)]));
//                 }
//                 else
//                 {
//                     iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[5 + (i*inspItemCount)]));
//                     iCenterPointToROI.Add(int.Parse(_gapSystem.StrLstRcpConInspData_BiCell[5 + (i*inspItemCount)]));
//                 }
            }
        }

        private void Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check()
        {
            _gapSystem.RectListImageZone_BiCell.Clear();

            for (int i = 0; i < _gapSystem.RectListRecipeBoxZone_BiCell.Count; i++)
            {
                var tempRect = new CvRect(0, 0, 0, 0);
                _pointAnalize.BoxToImage(_gapSystem.RectListRecipeBoxZone_BiCell[i], ref tempRect,
                    _gapSystem.GetSet_System_Status_Zoom_X_BiCell, _gapSystem.GetSet_System_Status_Zoom_Y_BiCell);
                _gapSystem.RectListImageZone_BiCell.Add(tempRect);
            }
        }

        public void Inspect_Get_ModelData_From_UMAC()
        {
            //인스펙션 로그 파일 기록 함수 호출 (201:현재 모델 정보 획득)

            if (_gapSystem.IsConnect_UMAC == false)
            {
                uMessageBox.SystemConnectStatus_UMAC("UMAC");
                return;
            }

            string strResponse = umac.Umac_Communicate_Command("P5001,5");
            string[] strArray = strResponse.Split('\r');

            for (int i = 0; i < NowModelDataCount; i++)
            {
                if (i == 0) NowModel_Cells = int.Parse(strArray[i]);
                else if (i == 1) NowModel_ATypeX = double.Parse(strArray[i]);
                else if (i == 2) NowModel_ATypeY = double.Parse(strArray[i]);
                else if (i == 3) NowModel_CTypeX = double.Parse(strArray[i]);
                else if (i == 4) NowModel_CTypeY = double.Parse(strArray[i]);
            }

            strResponse = umac.Umac_Communicate_Command("P5101, " + NowModel_Cells);
            string[] strArray2 = strResponse.Split('\r');
            for (int i = 0; i < NowModel_Cells; i++)
            {
                NowModel_Array.Add(strArray2[i].Trim());
            }


            strResponse = umac.Umac_Communicate_Command("P5200, " + NowModel_Cells);
            string[] floatArray = strResponse.Split('\r');
            for (int i = 0; i < NowModel_Cells; i++)
            {
                NowModel_GapData.Add(double.Parse(floatArray[i].Trim()));
            }
        }


        public void Inspect_Get_ModelData_Read_CellArray(int arrayData)
        {
            int uFlagHigh = 0;
            strLstCellArrayData.Clear();
            for (int nIndex = 0; nIndex < NowModel_Cells; nIndex++)
            {
                uFlagHigh = arrayData & 0x00000001;

                arrayData = arrayData >> 1;

                if (uFlagHigh == 1)
                    strLstCellArrayData.Add("C");
                else
                    strLstCellArrayData.Add("A");
            }
        }

        public void Inspect_BiCell_Initionalize()
        {
            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load();
        }

        private void Inspect_MIL_Initialize()
        {
            //MIL.MappAllocDefault(MIL.M_DEFAULT, ref MilApplication, ref MilSystem, ref MilDisplay, ref MilDigitizer, ref MilImage);
            //MIL.MdispSelectWindow(MilDisplay, MilImage, Inspect_Main01_IplBox.Handle);
        }

        public void Inspect_Auto_Image_Grabing(int grabNo)
        {
            SavedTrigNumber++;
            NowTrigNumber_BiCell = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "trigNoBiCell"));
            NowTrigNumber_BiCell++;
            SetReg(_gapSystem.RegPathGapStatus, "trigNoBiCell", NowTrigNumber_BiCell.ToString());

            _dtInspDataSaveTime = DateTime.Now;

            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load();

            ROI_Check_Result = Inspect_Manual_Run_Run();
            if (ROI_Check_Result == false)
            {
                NowTrigNumber_BiCell = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "trigNoBiCell"));
                NowTrigNumber_BiCell++;
                SetReg(_gapSystem.RegPathGapStatus, "trigNoBiCell", NowTrigNumber_BiCell.ToString());

                NowFailNumber_BiCell = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "NGCountBiCell"));
                NowFailNumber_BiCell++;
                SetReg(_gapSystem.RegPathGapStatus, "NGCountBiCell", NowFailNumber_BiCell.ToString());
            }

            OperationEvent_BiCell(NowProdectNumber_BiCell, NowFailNumber_BiCell);
            Inspect_Auto_Drawing_Result();
            OperationEvent_BiCell(NowProdectNumber_BiCell, NowFailNumber_BiCell);
        }

        private uint NowTrigNumber_BiCell = 0;
        private bool ROI_Check_Result = true;
        public string Inspect_Run_Ready_TrigNo_Reg_To_Data(string strNodePath, string regTitle)
        {
            return GetReg(strNodePath, regTitle);
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

        public void SetReg(string strNodePath, string strName, string strData)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(strNodePath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(strName, strData, RegistryValueKind.String);
            reg.Close();
        }

        private uint NowFailNumber_BiCell = 0;
        public void Inspect_Manual_Image_Grabing()
        {
            SavedTrigNumber++;
            NowTrigNumber_BiCell = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "trigNoBiCell"));
            NowTrigNumber_BiCell++;
            SetReg(_gapSystem.RegPathGapStatus, "trigNoBiCell", NowTrigNumber_BiCell.ToString());

            _dtInspDataSaveTime = DateTime.Now;

            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load();
// #if (!SIM01)
//             Inspect_Get_ModelData_From_UMAC_SIM();
// #else
//     //차후 메인의 설정값으로 변경해야함.
//             Inspect_Get_ModelData_From_UMAC();
// #endif
            _FlagManual = true;

            ROI_Check_Result = Inspect_Manual_Run_Run();
           
            Inspect_Manual_Drawing_Result();
        }
        
        /*
         public void Inspect_Manual_Image_Grabing(int grabNo)
        {
            SavedTrigNumber++;

            _dtInspDataSaveTime = DateTime.Now;

            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load();
#if (!SIM01)
            Inspect_Get_ModelData_From_UMAC_SIM();
#else
            //차후 메인의 설정값으로 변경해야함.
            Inspect_Get_ModelData_From_UMAC();
#endif
            _FlagManual = true;
            

            FormDlgManualAlign manual = new FormDlgManualAlign();
            manual.ModelCellCount = NowModel_Cells;
            manual.GripNo = grabNo - 1;
            if (manual.ShowDialog() == DialogResult.OK)
            {
                _iManual_GripNo = manual.GripNo;
                _iManual_CellType = manual.CellType;
                _iManual_CellNo = manual.CellNo;

                NowCellGrabber = manual.GripNo;
                NowCellType = manual.CellType;
                NowCellNumber = manual.CellNo;

                bool ROI_Check_Result = Inspect_Manual_Run_Run();
                if (ROI_Check_Result == false) return;

                Inspect_Manual_Drawing_Result();
            }
        }
        */

        private void BiCell_Manual_Testing()
        {
            //로딩한 이미지의 ROI 처리를 진행한다.
            bool ROI_Check_Result = Inspect_Manual_Run_Run();
            if (ROI_Check_Result == false) return;

            Inspect_Manual_Drawing_Result();
        }

        public void Inspect_Get_ModelData_From_UMAC_SIM()
        {
            NowModel_ATypeX = 190.0;
            NowModel_ATypeY = 142.5;
            NowModel_CTypeX = 193.5;
            NowModel_CTypeY = 145.0;
            NowModel_Cells = 11;
            NowModel_Array.Add("A");
            NowModel_Array.Add("C");
            NowModel_Array.Add("C");
            NowModel_Array.Add("A");
            NowModel_Array.Add("A");
            NowModel_Array.Add("C");
            NowModel_Array.Add("C");
            NowModel_Array.Add("A");
            NowModel_Array.Add("A");
            NowModel_Array.Add("C");
            NowModel_Array.Add("C");
            //NowModel_Array = 409;
            NowModel_GapData.Add(54.50);
            NowModel_GapData.Add(149.10);
            NowModel_GapData.Add(2.20);
            NowModel_GapData.Add(4.20);
            NowModel_GapData.Add(5.80);
            NowModel_GapData.Add(4.80);
            NowModel_GapData.Add(3.60);
            NowModel_GapData.Add(5.50);
            NowModel_GapData.Add(7.00);
            NowModel_GapData.Add(5.80);
            NowModel_GapData.Add(5.20);
        }

        public bool Inspect_Manual_Run_Run()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            //if (_nowIplImage == null) return false;
            bool areaResult = true;
            var resultPoint = new CvPoint(0, 0);
            tempImage.ResetROI();
            cvPntLstWidthData.Clear();
            cvPntLstCenterPoint.Clear();
            //CvWindow.ShowImages(_nowIplImage);
            _nowIplImage.Copy(tempImage);
            for (int i = 0; i < _gapSystem.RectListImageZone_BiCell.Count; i++)
            {
                NowROI_Number = 0;
                var cuttingimage = new IplImage(_gapSystem.RectListImageZone_BiCell[i].Width,
                    _gapSystem.RectListImageZone_BiCell[i].Height, BitDepth.U8, 3);
                //var cuttingimage = new IplImage(_gapSystem.RectListImageZone[i].Width, _gapSystem.RectListImageZone[i].Height, BitDepth.U8, 1);
                Cv.SetImageROI(tempImage,
                    new CvRect(_gapSystem.RectListImageZone_BiCell[i].Location,
                        _gapSystem.RectListImageZone_BiCell[i].Size));
                Cv.Copy(tempImage, cuttingimage);

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

                    //imageHougher.HoughLines_Point(cuttingimage, _iEdgeParam1, _iEdgeParam2, _iLineParam1, _iLineParam2,_iLineParam3);
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


                        resultPoint = Inspect_Run_Run_02(cuttingimage, FindedPoints, iLstNowSidNo[i],
                            sLstNowPolNo[i], sLstNowTypNo[i]);
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

                //if (sLstNowTypNo[i] == "거리") 
                cvPntLstCenterPoint.Add(resultPoint);
            }
            return areaResult;
        }

        private CvPoint Inspect_Run_Run_02(IplImage zoneImage, List<CvPoint> findedPoints, int sideData, string polaData,
            string typeData)
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
                //CvSize CSize = new CvSize(2, 2);
                //Cv.DrawCircle(zoneImage, measurePoints[0], 2, CvColor.Red);
                //CvWindow.ShowImages(zoneImage);
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
            //int tmpData = (iGridRow*11) + 10;
            int BrightDeferentValue = 0;
            //bool result = int.TryParse(_gapSystem.StrLstRcpConGridData_BiCell[10], out BrightDeferentValue);
            bool result = int.TryParse(_gapSystem.StrLstRcpConGridData_BiCell[(iGridRow*11) + 10],
                out BrightDeferentValue);
            if (result == false) return false;

            //string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
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

        /*
        private bool Inspect_Run_Run_Finded_Lines_UsingLine_Check(IplImage zoneImage, CvPoint[] measPoint, string polaData, int sideData)
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

            int BrightDeferentValue = 0;
            bool result = int.TryParse(_gapSystem.StrLstRcpConGridData_BiCell[10], out BrightDeferentValue);
            if (result == false) return false;
            
            string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
            Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(zoneImage, measPoint, v1, v2);

            bool checkResult = false;
            if (intMethod == 0 && sideData == 0)
            {
                if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 1)
            {
                if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 2)
            {
                if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 3)
            {
                if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 0)
            {
                if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 1)
            {
                if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 2)
            {
                if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 3)
            {
                if (v1[0] < v2[0]) return true;
                return false;
            }
            return false;
        }
        */

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

        private void Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(IplImage cuttingimage, CvPoint[] measPoint)
        {
            //cuttingimage.Set2D(measPoint[0].Y, measPoint[0].X, 255);
            //cuttingimage.Set2D(measPoint[1].Y, measPoint[1].X, 255);
            cuttingimage.Circle(measPoint[0].X, measPoint[0].Y, 2, CvColor.Blue, 1, LineType.AntiAlias, 0);
            cuttingimage.Circle(measPoint[1].X, measPoint[1].Y, 2, CvColor.Blue, 1, LineType.AntiAlias, 0);

            var cvFont = new CvFont(FontFace.Italic, 0.5, 0.5);
            cuttingimage.PutText("1", measPoint[0], cvFont, 255);
            cuttingimage.PutText("2", measPoint[1], cvFont, 255);

            CvWindow.ShowImages(cuttingimage);
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

        public void Inspect_Manual_Drawing_Result()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_CenterPoint_To_ImagePoint();
            Inspect_Run_Run_Image_Inspecting();

            //저장 쓰레드와 분석 쓰레드가 독립적으로 구동하기 때문에
            //히스토리부분에서 싱크를 시키기 위해서 번호를 설정한다.
            _iHistoryViewNo = _iImgCount;

            //인스펙션 로그 파일 기록 함수 호출 (4:이미지 저장 진행)
            FormDlgInsp_Inspection_LogData_File_Data_Write(4);

            //쓰레드에서 아래 두개의 멤버에 접근하기 전에 메인 쓰레드에서
            //이미 값이 변경될 가능성이 있어 다른 메버에 복사하여 복사된
            //멤버의 값을 사용하여 저장 데이터를 만든다.
            _nowIplImage.Copy(NowSavedImage);
            _SavedImageData = ImageDataArray;
            if(ROI_Check_Result == false)
            Inspect_Run_Run_Result_Drawing_Manual_NG();
            else
            //검사 결과를 화면에 표시해 준다.
            Inspect_Run_Run_Result_Drawing();

            if (Run_Mode != "Manual")
            {
                Inspect_Auto_Data_Set();
                //엑셀 파일에 결과를 저장한다.
                _dtImageSaveTime = DateTime.Now;

                //엑셀에 데이터를 저장하는 메소드가 실행시간의 정유율을 높게 차지 하기
                //때문에 뒤의 두개의 함수와 같은 경우로 같이 쓰레드로 처리한다.
                //string strtmp = _alignSystem.StrListSysConTitle[0];
                //string strtmp1 = _alignSystem.StrListSysConData[1];
                if ((passSaveData == "ON") || (failSaveData == "ON"))
                {
                    Excel_Write_Thread = new Thread(FormDlgInsp_Inspection_Excel_Data_Write);
                    Excel_Write_Thread.Start();
                }

                if ((passSaveImage == "ON") || (failSaveImage == "ON"))
                {
                    File_Save_Thread = new Thread(Inspect_Run_Run_SaveImage_To_File);
                    File_Save_Thread.Start();
                }

                if (CallForm == "Inspect_Form")
                {
                    History_All_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_All);
                    History_All_Save_Thread.Start();

                    History_NG_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_NG);
                    History_NG_Save_Thread.Start();
                }
            }
        }

        private bool CenterFinded = false;

        public void Inspect_Auto_Drawing_Result()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            if (ROI_Check_Result == true)
                CenterFinded = Inspect_Run_Run_CenterPoint_To_ImagePoint();

            //쓰레드에서 아래 두개의 멤버에 접근하기 전에 메인 쓰레드에서
            //이미 값이 변경될 가능성이 있어 다른 메버에 복사하여 복사된
            //멤버의 값을 사용하여 저장 데이터를 만든다.
            _nowIplImage.Copy(NowSavedImage);
            _SavedImageData = ImageDataArray;

            //저장 쓰레드와 분석 쓰레드가 독립적으로 구동하기 때문에
            //히스토리부분에서 싱크를 시키기 위해서 번호를 설정한다.
            _iHistoryViewNo = _iImgCount;

            //인스펙션 로그 파일 기록 함수 호출 (4:이미지 저장 진행)
            FormDlgInsp_Inspection_LogData_File_Data_Write(4);

            if (ROI_Check_Result == false || CenterFinded == false)
            {

                //검사 결과를 화면에 표시해 준다.
                Inspect_Run_Run_Result_Drawing_Auto_NG();
            }
            else
            {
                NowTrigNumber_BiCell =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "trigNoBiCell"));
                NowTrigNumber_BiCell++;
                SetReg(_gapSystem.RegPathGapStatus, "trigNoBiCell", NowTrigNumber_BiCell.ToString());

                NowProdectNumber_BiCell =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(_gapSystem.RegPathGapStatus, "ProductNoBiCell"));
                NowProdectNumber_BiCell++;
                SetReg(_gapSystem.RegPathGapStatus, "ProductNoBiCell", NowProdectNumber_BiCell.ToString());

                Inspect_Run_Run_Image_Inspecting();

                //검사 결과를 화면에 표시해 준다.
                Inspect_Run_Run_Result_Drawing_Auto();
            }

            Inspect_Auto_Data_Set();

            _dtImageSaveTime = DateTime.Now;

            //엑셀에 데이터를 저장하는 메소드가 실행시간의 정유율을 높게 차지 하기
            //때문에 뒤의 두개의 함수와 같은 경우로 같이 쓰레드로 처리한다.
            Excel_Write_Thread = new Thread(FormDlgInsp_Inspection_Excel_Data_Write);
            Excel_Write_Thread.Start();

            //if ((ROI_Check_Result != false && CenterFinded != false) && (passSaveData == "ON"))
            //{
            //    Excel_Write_Thread = new Thread(FormDlgInsp_Inspection_Excel_Data_Write);
            //    Excel_Write_Thread.Start();
            //}
            //else if ((ROI_Check_Result == false || CenterFinded == false) && (failSaveData == "ON"))
            //{
            //    Excel_Write_Thread = new Thread(FormDlgInsp_Inspection_Excel_Data_Write);
            //    Excel_Write_Thread.Start();
            //}


            if ((ROI_Check_Result != false && CenterFinded != false) && (passSaveImage == "ON"))
            {
                File_Save_Thread = new Thread(Inspect_Run_Run_SaveImage_To_File);
                File_Save_Thread.Start();
            }
            else if ((ROI_Check_Result == false || CenterFinded == false) && (failSaveImage == "ON"))
            {
                File_Save_Thread = new Thread(Inspect_Run_Run_SaveImage_To_File);
                File_Save_Thread.Start();
            }


            History_All_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_All);
            History_All_Save_Thread.Start();


            if (ROI_Check_Result == false || CenterFinded == false)
            {
                History_NG_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_NG);
                History_NG_Save_Thread.Start();
            }
        }

        uint NowProdectNumber_BiCell = 0;
        /*
        public void Inspect_Auto_Drawing_Result()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            bool CenterFinded = Inspect_Run_Run_CenterPoint_To_ImagePoint();

            Inspect_Run_Run_Image_Inspecting();

            //저장 쓰레드와 분석 쓰레드가 독립적으로 구동하기 때문에
            //히스토리부분에서 싱크를 시키기 위해서 번호를 설정한다.
            _iHistoryViewNo = _iImgCount;

            //인스펙션 로그 파일 기록 함수 호출 (4:이미지 저장 진행)
            FormDlgInsp_Inspection_LogData_File_Data_Write(4);

            //쓰레드에서 아래 두개의 멤버에 접근하기 전에 메인 쓰레드에서
            //이미 값이 변경될 가능성이 있어 다른 메버에 복사하여 복사된
            //멤버의 값을 사용하여 저장 데이터를 만든다.
            _nowIplImage.Copy(NowSavedImage);
            _SavedImageData = ImageDataArray;


            //검사 결과를 화면에 표시해 준다.
            Inspect_Run_Run_Result_Drawing_Auto();


            Inspect_Auto_Data_Set();
            //엑셀 파일에 결과를 저장한다.
            _dtImageSaveTime = DateTime.Now;

            //엑셀에 데이터를 저장하는 메소드가 실행시간의 정유율을 높게 차지 하기
            //때문에 뒤의 두개의 함수와 같은 경우로 같이 쓰레드로 처리한다.
            //string strtmp = _alignSystem.StrListSysConTitle[0];
            //string strtmp1 = _alignSystem.StrListSysConData[1];
            if ((passSaveData == "ON") || (failSaveData == "ON"))
            {
                Excel_Write_Thread = new Thread(FormDlgInsp_Inspection_Excel_Data_Write);
                Excel_Write_Thread.Start();
            }

            if ((passSaveImage == "ON") || (failSaveImage == "ON"))
            {
                File_Save_Thread = new Thread(Inspect_Run_Run_SaveImage_To_File);
                File_Save_Thread.Start();
            }

            //if (CallForm == "Inspect")
            //{
            History_All_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_All);
            History_All_Save_Thread.Start();

            History_NG_Save_Thread = new Thread(Inspect_Run_Run_Drawing_Result_To_History_NG);
            History_NG_Save_Thread.Start();
            //}
        }
        */

        
        private void Inspect_Run_Run_SaveImage_To_File()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //string tmpstr1 = _alignSystem.StrListSysConTitle[0];
            //string tmpstr2 = _alignSystem.StrListSysConName[0];
            //string tmpstr3 = _alignSystem.StrListSysConData[0];

            string imageSaveFolderName = Inspect_Set_FolderName_ImageFile(_dtInspDataSaveTime);

//             string imageFileName = SavedTrigNumber.ToString("0000000000") + " "
//                 + SavedCellNumber.ToString("00") + " " + StrSavedInspectResult +
//                                    String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
//                                        _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
//                                        _dtInspDataSaveTime.Hour,
//                                        _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
//                                        _dtInspDataSaveTime.Millisecond);

            string imageFileName = NowTrigNumber_BiCell.ToString("00000000") + " " + " " + StrSavedInspectResult +
                                   String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                                       _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                                       _dtInspDataSaveTime.Hour,
                                       _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                                       _dtInspDataSaveTime.Millisecond);
           
            string imageFilePath = imageSaveFolderName + "\\" + imageFileName + ".jpg";

            _strHisImgName = imageFilePath;

            //이미지에 검사결과를 기록하기 전에 복사해 놓은 이미지를 저장한다.
            NowSavedImage.SaveImage(imageFilePath);
        }

        private string Inspect_Set_FolderName_ImageFile(DateTime checkTime)
        {
            var fileSystem = new Control_Files();

            string folderName = NowImageFolderSavePath + "\\Cell" +
                                String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            folderName = folderName +
                         String.Format("\\{0:00}년{1:00}월{2:00}일", checkTime.Year, checkTime.Month, checkTime.Day);


            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);

            return folderName;
        }


        private void Inspect_Run_Run_Drawing_Result_To_History_All()
        {
            if (uPanel_History_All.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_History_All del = Inspect_Run_Run_Drawing_Result_To_History_All;
                uPanel_History_All.Invoke(del);
            }
            else
            {
                _strHistoryViewNameAll[_iAllHistoryViewNo] = _strHisImgName;

                ((PictureBoxIpl) (uPanel_History_All.ClientArea.Controls[_iAllHistoryViewNo])).ImageIpl = NowSavedImage;
                uPanel_History_All.ClientArea.Controls[_iAllHistoryViewNo].Refresh();
                if (_iAllHistoryViewNo + 1 == 11) _iAllHistoryViewNo = 0;
                else _iAllHistoryViewNo++;

            }
        }
        
        public List<string> _strHistoryViewNameAll = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        public List<string> _strHistoryViewNameNG = new List<string>{"0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"};

        private void Inspect_Run_Run_Drawing_Result_To_History_NG()
        {
            if (uPanel_History_NG.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_History_NG del = Inspect_Run_Run_Drawing_Result_To_History_NG;
                uPanel_History_NG.Invoke(del);
            }
            else
            {
                _strHistoryViewNameNG[_iNGHistoryViewNo] = _strHisImgName;

                ((PictureBoxIpl) (uPanel_History_NG.ClientArea.Controls[_iNGHistoryViewNo])).ImageIpl = NowSavedImage;
                uPanel_History_NG.ClientArea.Controls[_iNGHistoryViewNo].Refresh();

                if (_iNGHistoryViewNo + 1 == 11) _iNGHistoryViewNo = 0;
                else _iNGHistoryViewNo++;
            }
        }


        public void Inspect_Auto_Data_Set()
        {
            NowImageFolderSavePath = _gapSystem.StrListSysConData[2];
            NowExcelFolderSavePath = _gapSystem.StrListSysConData[5];
            //_strSavedInspectResult = _strSavedNGOK[2];
            passSaveImage = _gapSystem.StrListSysConData[0];
            passSaveData = _gapSystem.StrListSysConData[3];

            failSaveImage = _gapSystem.StrListSysConData[1];
            failSaveData = _gapSystem.StrListSysConData[4];
        }

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

            if (passSave == "OFF" && StrSavedInspectResult == "OK") return;
            if (failSave == "OFF" && StrSavedInspectResult == "NG") return;

            string nowExcelFileName = Inspect_Set_FileName_ExcelFile(_dtInspDataSaveTime);
            excelFile.WriteExcelFile(nowExcelFileName, strLstDisplayData);
        }

        //이를 CSV파일로 변경한다.
        private string Inspect_Set_FileName_ExcelFile(DateTime checkTime)
        {
            //string folderName = NowExcelFolderSavePath + "\\Gap" + String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string folderName = NowExcelFolderSavePath + "\\Cell" +
                                String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}년{1:00}월{2:00}일 Gap Vision.csv", checkTime.Year, checkTime.Month,
                                  checkTime.Day);

            excelFile.Excel_Folder_Check_Or_Make(folderName);
            excelFile.Excel_File_Check_Or_Make_BiCell(fileName);

            return fileName;
        }

        private void Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check()
        {
            var tempFloats = new[] {0f, 0f};

            _pointAnalize.BoxVsImage(_imageBoxIpl, Resources.empty, ref tempFloats);
            _gapSystem.GetSet_System_Inspect_Zoom_X_BiCell = tempFloats[0];
            _gapSystem.GetSet_System_Inspect_Zoom_Y_BiCell = tempFloats[1];

            _gapSystem.RectListInspBoxZone_BiCell.Clear();
            for (int i = 0; i < _gapSystem.RectListImageZone_BiCell.Count; i++)
            {
                var tempRect = new Rectangle();
                _pointAnalize.ImageToBox(_gapSystem.RectListImageZone_BiCell[i], ref tempRect,
                    _gapSystem.GetSet_System_Inspect_Zoom_X_BiCell, _gapSystem.GetSet_System_Inspect_Zoom_Y_BiCell);
                _gapSystem.RectListInspBoxZone_BiCell.Add(tempRect);

                ////Trace.WriteLine(
                //    _gapSystem.RectListRecipeBoxZone[i].Width.ToString("000") + "  " + _gapSystem.RectListRecipeBoxZone[i].Height.ToString("000") + " : " +
                //    _gapSystem.RectListImageZone[i].Width.ToString("000") + "  " + _gapSystem.RectListImageZone[i].Height.ToString("000") + " : " + 
                //    _gapSystem.RectListInspBoxZone[i].Width.ToString("000") + "  " + _gapSystem.RectListInspBoxZone[i].Height.ToString("000"));
            }
        }

        private void Inspect_Run_Run_Result_Drawing_Auto()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_PictureBox_Reflash();

            var myPen = new Pen(Color.LawnGreen, 3f);
            int StanX = int.Parse(_gapSystem.StrListVisConData_BiCell[6]);
            int StanY = int.Parse(_gapSystem.StrListVisConData_BiCell[8]);
            var startCvPoint = new CvPoint(20, StanY);
            var endCvPoint = new CvPoint(_nowIplImage.Width - 20, StanY);
            CvPoint ResultPointStart = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, startCvPoint);
            CvPoint ResultPointEnd = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, endCvPoint);
            gc.DrawLine(myPen, ResultPointStart.X, ResultPointStart.Y, ResultPointEnd.X, ResultPointEnd.Y);
            
            Rectangle DrawRect = _gapSystem.RectListInspBoxZone_BiCell[0];
            List<CvPoint> BoxDrawPoint = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, cvPntLstImagePoint);
            CvPoint FindPoint = BoxDrawPoint[0];
            var SeroLine_PointStr = new Point(DrawRect.X, FindPoint.Y);
            var SeroLine_PointEnd = new Point(DrawRect.X + DrawRect.Width, FindPoint.Y);
            gc.DrawLine(myPen, SeroLine_PointStr, SeroLine_PointEnd);
            
            var _font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);
            string displayStr02 = "Milli Data : " + _SavedImageData[1];
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(370, 600));
            string displayStr03 = "Move : " + _SavedImageData[0];
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));
            string displayStr01 = "Pixel Data : " + _SavedImageData[3];
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));

            int pixValue = 0;
            bool result = int.TryParse(_SavedImageData[3], out pixValue);

            if (result == false)
            {
                MessageBox.Show("픽셀값 오류 입니다.");
                return;
            }

            

            //if (pixValue > -1) MessageBox.Show("픽셀값 : " + pixValue.ToString());
            //픽박스에 인스첵 존을 표시해 주는 함수
            //Inspect_Run_Run_DrawingZone_To_Box_Fail();

//             string gridTime = String.Format(" {0:00}.{1:00}.{2:00} {3:00}시{4:00}분{5:00}.{6:000}초",
//                                        _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
//                                        _dtInspDataSaveTime.Hour,
//                                        _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
//                                        _dtInspDataSaveTime.Millisecond);

            string gridDate = String.Format("{0:00}.{1:00}.{2:00}", _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month,
                _dtInspDataSaveTime.Day);

            string gridTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초", _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute,
                _dtInspDataSaveTime.Second, _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(gridDate);
            strLstDisplayData.Add(gridTime);
            strLstDisplayData.Add(NowTrigNumber_BiCell.ToString("00000000"));
            strLstDisplayData.Add(_SavedImageData[0]);
            strLstDisplayData.Add(_SavedImageData[1]);
            strLstDisplayData.Add(_SavedImageData[2]);
            strLstDisplayData.Add(_SavedImageData[3]);
            strLstDisplayData.Add(_SavedImageData[4]);
            strLstDisplayData.Add(_SavedImageData[5]);
            strLstDisplayData.Add(_SavedImageData[6]);
        }

        private void Inspect_Run_Run_Result_Drawing_Auto_NG()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_PictureBox_Reflash();

            var myPen = new Pen(Color.LawnGreen, 3f);
            int StanX = int.Parse(_gapSystem.StrListVisConData_BiCell[6]);
            int StanY = int.Parse(_gapSystem.StrListVisConData_BiCell[8]);
            var startCvPoint = new CvPoint(20, StanY);
            var endCvPoint = new CvPoint(_nowIplImage.Width - 20, StanY);
            CvPoint ResultPointStart = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, startCvPoint);
            CvPoint ResultPointEnd = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, endCvPoint);
            gc.DrawLine(myPen, ResultPointStart.X, ResultPointStart.Y, ResultPointEnd.X, ResultPointEnd.Y);

            //Rectangle DrawRect = _gapSystem.RectListInspBoxZone_BiCell[0];
            //List<CvPoint> BoxDrawPoint = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, cvPntLstImagePoint);
            //CvPoint FindPoint = BoxDrawPoint[0];
            //var SeroLine_PointStr = new Point(DrawRect.X, FindPoint.Y);
            //var SeroLine_PointEnd = new Point(DrawRect.X + DrawRect.Width, FindPoint.Y);
            //gc.DrawLine(myPen, SeroLine_PointStr, SeroLine_PointEnd);

            var _font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);
            string displayStr02 = "Milli Data : NG";
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(420, 600));
            string displayStr03 = "Move : NG" ;
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));
            string displayStr01 = "Pixel Data : NG" ;
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));

//             int pixValue = 0;
//             bool result = int.TryParse(_SavedImageData[3], out pixValue);
// 
//             if (result == false)
//             {
//                 MessageBox.Show("픽셀값 오류 입니다.");
//                 return;
//             }



            //if (pixValue > -1) MessageBox.Show("픽셀값 : " + pixValue.ToString());
            //픽박스에 인스첵 존을 표시해 주는 함수
            //Inspect_Run_Run_DrawingZone_To_Box_Fail();

            //             string gridTime = String.Format(" {0:00}.{1:00}.{2:00} {3:00}시{4:00}분{5:00}.{6:000}초",
            //                                        _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
            //                                        _dtInspDataSaveTime.Hour,
            //                                        _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
            //                                        _dtInspDataSaveTime.Millisecond);

            string gridDate = String.Format("{0:00}.{1:00}.{2:00}", _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month,
                _dtInspDataSaveTime.Day);

            string gridTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초", _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute,
                _dtInspDataSaveTime.Second, _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(gridDate);
            strLstDisplayData.Add(gridTime);
            strLstDisplayData.Add(NowTrigNumber_BiCell.ToString("00000000"));
            //strLstDisplayData.Add(_SavedImageData[0]);
            //strLstDisplayData.Add(_SavedImageData[1]);
            //strLstDisplayData.Add(_SavedImageData[2]);
            //strLstDisplayData.Add(_SavedImageData[3]);
            //strLstDisplayData.Add(_SavedImageData[4]);
            //strLstDisplayData.Add(_SavedImageData[5]);
            //strLstDisplayData.Add(_SavedImageData[6]);
            strLstDisplayData.Add("0");
            strLstDisplayData.Add("0");
            strLstDisplayData.Add("0");
            strLstDisplayData.Add("0");
            strLstDisplayData.Add("0");
            strLstDisplayData.Add("0");
            strLstDisplayData.Add("0");
        }


        private void Inspect_Run_Run_Result_Drawing_Manual_NG()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_PictureBox_Reflash();

            var myPen = new Pen(Color.LawnGreen, 3f);
            int StanX = int.Parse(_gapSystem.StrListVisConData_BiCell[6]);
            int StanY = int.Parse(_gapSystem.StrListVisConData_BiCell[8]);
            var startCvPoint = new CvPoint(20, StanY);
            var endCvPoint = new CvPoint(_nowIplImage.Width - 20, StanY);
            CvPoint ResultPointStart = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, startCvPoint);
            CvPoint ResultPointEnd = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, endCvPoint);
            gc.DrawLine(myPen, ResultPointStart.X, ResultPointStart.Y, ResultPointEnd.X, ResultPointEnd.Y);

            var _font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);
            string displayStr02 = "Milli Data : NG";
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(420, 600));
            string displayStr03 = "Move : NG";
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));
            string displayStr01 = "Pixel Data : NG";
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));

            string gridDate = String.Format("{0:00}.{1:00}.{2:00}", _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month,
                _dtInspDataSaveTime.Day);

            string gridTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초", _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute,
                _dtInspDataSaveTime.Second, _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(gridDate);
            strLstDisplayData.Add(gridTime);
            strLstDisplayData.Add(NowTrigNumber_BiCell.ToString("00000000"));
            strLstDisplayData.Add(_SavedImageData[0]);
            strLstDisplayData.Add(_SavedImageData[1]);
            strLstDisplayData.Add(_SavedImageData[2]);
            strLstDisplayData.Add(_SavedImageData[3]);
            strLstDisplayData.Add(_SavedImageData[4]);
            strLstDisplayData.Add(_SavedImageData[5]);
            strLstDisplayData.Add(_SavedImageData[6]);
        }


        private void Inspect_Run_Run_Display_Arrow_ZoneBox()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //이미지의 단위를 픽처박스의 단위로 변환해 준다.

            //var myPen = new Pen(Color.LawnGreen);
            var myPen = new Pen(Color.Red);
            myPen.Width = 2;
            Rectangle DrawRect = _gapSystem.RectListInspBoxZone_BiCell[0];

            List<CvPoint> BoxDrawPoint = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, cvPntLstImagePoint);
            CvPoint FindPoint = BoxDrawPoint[0];
            var SeroLine_PointStr = new Point((DrawRect.X/2)-20, FindPoint.Y);
            var SeroLine_PointEnd = new Point((DrawRect.X / 2) + 20, FindPoint.Y);
            gc.DrawLine(myPen, SeroLine_PointStr, SeroLine_PointEnd);


            /*
            startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
            endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1),
                boxArrowPntLst[i].Y - 30);
            gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
            endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2),
                boxArrowPntLst[i].Y - 30);
            gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
            endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3),
                boxArrowPntLst[i].Y - 30);
            gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);



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

                int tmpWidth = _gapSystem.RectListInspBoxZone_Gap[i].Width / 4;
                if (pointCount == 0 || pointCount == 2)
                {
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1),
                        boxArrowPntLst[i].Y - 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2),
                        boxArrowPntLst[i].Y - 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3),
                        boxArrowPntLst[i].Y - 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                }
                else if (pointCount == 1 || pointCount == 3)
                {
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1),
                        boxArrowPntLst[i].Y + 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2),
                        boxArrowPntLst[i].Y + 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    startPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(_gapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3),
                        boxArrowPntLst[i].Y + 30);
                    gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                }
                pointCount++;
            }
            */
        }

        private void Inspect_Run_Run_Result_Drawing()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_PictureBox_Reflash();
            var myPen = new Pen(Color.LawnGreen);
            myPen.Width = 2;
            int StanX = int.Parse(_gapSystem.StrListVisConData_BiCell[6]);
            int StanY = int.Parse(_gapSystem.StrListVisConData_BiCell[8]);
            var startCvPoint = new CvPoint(20, StanY);
            var endCvPoint = new CvPoint(_nowIplImage.Width - 20, StanY);
            CvPoint ResultPointStart = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, startCvPoint);
            CvPoint ResultPointEnd = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, endCvPoint);
            gc.DrawLine(myPen, ResultPointStart.X, ResultPointStart.Y, ResultPointEnd.X, ResultPointEnd.Y);

            Rectangle DrawRect = new Rectangle();

            if(CallForm == "FormDlgMain") DrawRect = _gapSystem.RectListRecipeBoxZone_BiCell[0];
            else DrawRect = _gapSystem.RectListInspBoxZone_BiCell[0];

            List<CvPoint> BoxDrawPoint = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, cvPntLstImagePoint);
            CvPoint FindPoint = BoxDrawPoint[0];
            var SeroLine_PointStr = new Point(DrawRect.X, FindPoint.Y);
            var SeroLine_PointEnd = new Point(DrawRect.X + DrawRect.Width, FindPoint.Y);
            gc.DrawLine(myPen, SeroLine_PointStr, SeroLine_PointEnd);

            var _font = new Font(new FontFamily("Arial"), 10,FontStyle.Bold);

            string displayStr02 = "Milli Data : " + _SavedImageData[1];
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(370, 600));
            string displayStr03 = "Move : " + _SavedImageData[0];
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));
            string displayStr01 = "Pixel Data : " + _SavedImageData[3];
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));

            

            string gridDate = String.Format("{0:00}.{1:00}.{2:00}", _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month,
                _dtInspDataSaveTime.Day);

            string gridTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초", _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute,
                _dtInspDataSaveTime.Second, _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(gridDate);
            strLstDisplayData.Add(gridTime);
            strLstDisplayData.Add(NowTrigNumber_BiCell.ToString("00000000"));
            strLstDisplayData.Add(_SavedImageData[0]);
            strLstDisplayData.Add(_SavedImageData[1]);
            strLstDisplayData.Add(_SavedImageData[2]);
            strLstDisplayData.Add(_SavedImageData[3]);
            strLstDisplayData.Add(_SavedImageData[4]);
            strLstDisplayData.Add(_SavedImageData[5]);
            strLstDisplayData.Add(_SavedImageData[6]);
        }

        private void Inspect_Run_Run_Result_Drawing_Manual()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_PictureBox_Reflash();
            var myPen = new Pen(Color.LawnGreen);
            myPen.Width = 2;
            int StanX = int.Parse(_gapSystem.StrListVisConData_BiCell[6]);
            int StanY = int.Parse(_gapSystem.StrListVisConData_BiCell[8]);
            var startCvPoint = new CvPoint(20, StanY);
            var endCvPoint = new CvPoint(_nowIplImage.Width - 20, StanY);
            CvPoint ResultPointStart = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, startCvPoint);
            CvPoint ResultPointEnd = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, endCvPoint);
            gc.DrawLine(myPen, ResultPointStart.X, ResultPointStart.Y, ResultPointEnd.X, ResultPointEnd.Y);

            Rectangle DrawRect = _gapSystem.RectListInspBoxZone_BiCell[0];
            List<CvPoint> BoxDrawPoint = _pointAnalize.ImageToBox(_nowIplImage, _imageBoxIpl, cvPntLstImagePoint);
            CvPoint FindPoint = BoxDrawPoint[0];
            var SeroLine_PointStr = new Point(DrawRect.X, FindPoint.Y);
            var SeroLine_PointEnd = new Point(DrawRect.X + DrawRect.Width, FindPoint.Y);
            gc.DrawLine(myPen, SeroLine_PointStr, SeroLine_PointEnd);

            var _font = new Font(new FontFamily("Arial"), 10,
                FontStyle.Bold);

            string displayStr02 = "Milli Data : " + _SavedImageData[1];
            gc.DrawString(displayStr02, _font, Brushes.LawnGreen, new PointF(370, 600));
            string displayStr03 = "Move : " + _SavedImageData[0];
            gc.DrawString(displayStr03, _font, Brushes.LawnGreen, new PointF(620, 600));
            string displayStr01 = "Pixel Data : " + _SavedImageData[3];
            gc.DrawString(displayStr01, _font, Brushes.LawnGreen, new PointF(200, 600));


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

        private void Inspect_Run_Run_Image_Inspecting()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //////////////////////////////////////////////////////////////

            //string tmpstr = _gapSystem.StrListVisConTitle_BiCell[0];

            double Resol_Raro = double.Parse(_gapSystem.StrListVisConData_BiCell[2]);
            double Resol_Sero = double.Parse(_gapSystem.StrListVisConData_BiCell[5]);

            double Basic_Offset = double.Parse(_gapSystem.StrListVisConData_BiCell[9]);

            int Basic_Garo = int.Parse(_gapSystem.StrListVisConData_BiCell[6]);
            int Basic_Sero = int.Parse(_gapSystem.StrListVisConData_BiCell[8]);

            int PixelData = cvPntLstImagePoint[0].Y - Basic_Sero;

            double MiliData = PixelData/Resol_Sero;
            //double tmpCal = (double) (PixelData/10.784);

            double ResultMiliData = MiliData - Basic_Offset;
            var CSize = new CvSize(2, 2);
            Cv.DrawCircle(_nowIplImage, cvPntLstImagePoint[0], 2, CvColor.Red);
            //CvWindow.ShowImages(_nowIplImage);

            ImageDataArray[0] = ResultMiliData.ToString();
            ImageDataArray[1] = MiliData.ToString();
            ImageDataArray[2] = Basic_Offset.ToString();
            ImageDataArray[3] = PixelData.ToString();
            ImageDataArray[4] = Resol_Sero.ToString();
            ImageDataArray[5] = cvPntLstImagePoint[0].Y.ToString();
            ImageDataArray[6] = Basic_Sero.ToString();
            //////////////////////////////////////////////////////////////
            UmacWrite_P3702 = new Thread(Write_To_UMAC_P3702);
            UmacWrite_P3702.Start();
        }

        private void Write_To_UMAC_P3702()
        {
            umac.Umac_SetData_P3702(ImageDataArray[0]);
        }

        private void FormDlgInsp_Inspection_LogData_File_Data_Write(int msgNo)
        {
            var fileSystem = new Control_Files();

            DateTime logTime = DateTime.Now;
            string folderName = Environment.CurrentDirectory + "\\Log\\System\\" +
                                String.Format("{0:00}년{1:00}월", logTime.Year, logTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}-{1:00}-{2:00} Align Insp.log", logTime.Year, logTime.Month,
                                  logTime.Day);
            //fileSystem.File_IO_Text_File_Check_Or_Make_Inspect(folderName);
            //if (fileSystem.GetSet_FileNameInspect != fileName)
            //{
            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);
            //해당 경로에 파일이 있는지 검사해서 없으면 생성한다.
            //Param 1 : 조사할 경로, Param 2 : 파일이 종류(1:login, 2:Inspect, 3:Excel)
            //fileSystem.File_IO_Text_File_Check_Or_Make(fileName,2);
            fileSystem.File_IO_Text_File_Check_Or_Make_Inspect(fileName);
            //}

            //StreamWriter strLoginFile = new StreamWriter(fileName, true, Encoding.Default);
            //strLoginFile.Flush();
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


        public bool Inspect_Run_Run_CenterPoint_To_ImagePoint()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            bool CenterResult = true;
            //여기에서 타입이 넙이미면 한번에 두개의 포인트를 작업하도록
            //만들어야 한다.
            cvPntLstImagePoint.Clear();
            for (int i = 0; i < _gapSystem.RectListImageZone_BiCell.Count; i++)
            {
                string tmpTypeNo = sLstNowTypNo[i];
                int tmpSeqNo = iLstNowSeqNo[i];
                int tmpRoiNo = iLstNowRoiNo[i];
                CvRect tmpRect = _gapSystem.RectListImageZone_BiCell[tmpRoiNo];

                if (cvPntLstCenterPoint[i].X == 0 && cvPntLstCenterPoint[i].Y == 0)
                {
                    var zeroCvPoint = new CvPoint(0, 0);
                    cvPntLstImagePoint.Add(zeroCvPoint);
                    CenterResult = false;
                }
                else
                {
                    CvPoint tmpPnt = cvPntLstCenterPoint[i];
#if(!DEBUG)
                    Trace.WriteLine(tmpRect.X.ToString("000  ") + tmpRect.Y.ToString("000  ") + tmpPnt.X.ToString("000  ") + tmpPnt.Y.ToString("000  "));
#endif
                    var rCvPoint = new CvPoint(tmpRect.X + tmpPnt.X, tmpRect.Y + tmpPnt.Y);
                    cvPntLstImagePoint.Add(rCvPoint);
                }


                //디버깅 용으로 사용하는 함수 이다.
                //검출 구역의 포인트를 표시해준다.
                //Inspect_Run_Run_Drawing_Result_Location_Point(rCvPoint, i);
            }
            return CenterResult;
        }

        private delegate void Delegate_Run_Run_Drawing_Result_Inspection_Display();

        private delegate void Delegate_Run_Run_Drawing_Result_To_File_Display();

        private delegate void Delegate_Run_Run_Drawing_Result_To_History_All();

        private delegate void Delegate_Run_Run_Drawing_Result_To_History_NG();
    }
}