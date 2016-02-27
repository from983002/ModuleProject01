//using AbstractFactory;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ClosedXML.Excel;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinChart;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Matrox.MatroxImagingLibrary;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.UserInterface;
using FontStyle = System.Drawing.FontStyle;
using Resources = SystemAlign.Properties.Resources;

namespace SystemAlign
{
    public partial class FormDlgInsp : Form
    {
        private readonly List<PictureBoxIpl> HistoryAllBox = new List<PictureBoxIpl>();
        private  string _NowExcelFolderSavePath_Uper = string.Empty;
        private  string _NowImageFolderSavePath_Uper = string.Empty;
        private string _NowExcelFolderSavePath_Down = string.Empty;
        private string _NowImageFolderSavePath_Down = string.Empty;
        private readonly string[] NowInspecOKorNG = new string[3];
        private readonly List<string> NowModel_CellArray = new List<string>();
        private readonly List<string> Now_Model_Cell_Type_Array = new List<string>();
        private readonly List<double> Now_Model_Data_Array_Uper = new List<double>();
        private readonly List<double> Now_Model_Data_Array_Down = new List<double>();
        private readonly Stopwatch Tack_Time_Watch_BiCell = new Stopwatch();
        private readonly Stopwatch Tack_Time_Watch_Gap = new Stopwatch();
        private readonly int[] Threshold_01 = {150, 190};
        private readonly int[] Threshold_02 = {50, 100};
        private readonly Stopwatch _SeqStopwatch = new Stopwatch();
        private readonly double[] _dResultValueLeft = new double[3];
        private readonly double[] _dResultValueRight = new double[3];
        private readonly double[] _dSavedResultValue = new double[3];
        private readonly double[] _fResultValueLeft = new double[3];
        private readonly double[] _fResultValueRight = new double[3];
        private PositionConvert _posConverter_Down;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly List<string> _strLstTestImageNames_Down = new List<string>();
        private readonly List<string> _strLstTestImageNames_Uper = new List<string>();

        private List<CvPoint> cvPntLstImagePoint_Uper;// = new List<CvPoint>();
        private List<CvPoint> cvPntLstImagePoint_Down;// = new List<CvPoint>();
        private readonly List<CvPoint> cvPntLstWidthData = new List<CvPoint>();
        private readonly List<bool> drawOKList = new List<bool>();
        private readonly List<int> drawROIList = new List<int>();
        private readonly List<int> iCenterPointToROI_Uper = new List<int>();
        private readonly List<int> iLstNowDivNo_Uper = new List<int>();
        private readonly List<int> iLstNowLgtNo_Uper = new List<int>();
        private readonly List<int> iLstNowLstNo_Uper = new List<int>();
        private readonly List<int> iLstNowRoiNo_Uper = new List<int>();
        private readonly List<int> iLstNowRowNo_Uper = new List<int>();
        private readonly List<int> iLstNowSeqNo_Uper = new List<int>();
        private readonly List<int> iLstNowSidNo_Uper = new List<int>();

        private readonly List<int> iCenterPointToROI_Down = new List<int>();
        private readonly List<int> iLstNowDivNo_Down = new List<int>();
        private readonly List<int> iLstNowLgtNo_Down = new List<int>();
        private readonly List<int> iLstNowLstNo_Down = new List<int>();
        private readonly List<int> iLstNowRoiNo_Down = new List<int>();
        private readonly List<int> iLstNowRowNo_Down = new List<int>();
        private readonly List<int> iLstNowSeqNo_Down = new List<int>();
        private readonly List<int> iLstNowSidNo_Down = new List<int>();

        private readonly Hough imageHougher = new Hough();
        //private readonly byte[,] imgBuf = new byte[4096, 3072];
        private static byte[,] imgBuf_Down = new byte[4096, 3072];

        
        private static byte[,] imgBuf_Uper = new byte[4096, 3072];
        
        //private Control_Inspect_BiCell inspection_BiCell = new Control_Inspect_BiCell();
        //private Control_Inspect_Gap inspection_Gap = new Control_Inspect_Gap();

        //private readonly CvMat matImg_BiCell = new CvMat(3072, 4096, MatrixType.U8C1);
        private readonly CvMat CVMatImg_Uper = new CvMat(3072, 4096, MatrixType.U8C1);
        private readonly CvMat CVMatImg_Down = new CvMat(3072, 4096, MatrixType.U8C1);

        //이벤트를 진행할 함수를 지정해 준다.
        private readonly Pen myArrowPen = new Pen(Color.LawnGreen, 1);
        private readonly Pen myLinePen = new Pen(Color.LawnGreen, 1);
        private readonly List<string> sLstNowDisNo_Uper = new List<string>();
        private readonly List<string> sLstNowPolNo_Uper = new List<string>();
        private readonly List<string> sLstNowTypNo_Uper = new List<string>();
        private readonly List<string> sLstNowUsed_Uper = new List<string>();
        private readonly List<string> sLstNowUsed_Down = new List<string>();
        private readonly List<string> sLstNowDisNo_Down = new List<string>();
        private readonly List<string> sLstNowPolNo_Down = new List<string>();
        private readonly List<string> sLstNowTypNo_Down = new List<string>();
        private static IplImage SrcIplImage_Down = new IplImage(4096, 3072, BitDepth.U8, 3);
        private static IplImage SrcIplImage_Uper = new IplImage(4096, 3072, BitDepth.U8, 3);
        private string A1GrabStatus = "7";
        private string A2GrabStatus = "8";
        private double Dist_Center_Garo;
        private double Dist_Center_Sero;
        private Thread Excel_Write_Thread;
        private bool FailZeroCheck1;
        private bool FailZeroCheck2;
        private Thread File_Save_Thread_Uper;
        private Thread File_Save_Thread_Down;
        private CInspection_Lamination LamiSystem;
        private Thread History_All_Save_Thread_Uper;
        private Thread History_NG_Save_Thread_Uper;
        private Thread History_All_Save_Thread_Down;
        private Thread History_NG_Save_Thread_Down;
        private int HoughRetryCount;
        private Color IONoColor;
        private double Image_Center_Garo;
        private double Image_Center_Sero;
        private UltraGrid[] InspProducts;
        private Thread Inspect_Stop_threading;
        private Thread InspectionThread;
        public Thread InspectionThread2;
        public Thread InspectionThread3;
        private string LogExcelData = string.Empty;
        private List<CvRect> LstTempCvRect = new List<CvRect>();
        private Thread MIL_Grab_Threading_Down;
        private Thread MIL_Grab_Threading_Uper;
        private int MIL_ImageCount = 1;
        private bool MIL_Trigger_Close = true;
        private MIL_ID MilApplication; // = MIL.M_NULL;
        private MIL_ID MilDigitizer_Down; // = MIL.M_NULL;
        private MIL_ID MilDigitizer_Uper; // = MIL.M_NULL;
        private MIL_ID MilDisplay_Down; // = MIL.M_NULL;
        private MIL_ID MilDisplay_Uper; // = MIL.M_NULL;
        private MIL_ID MilImage_Down; // = MIL.M_NULL;
        private MIL_ID MilImage_Uper; // = MIL.M_NULL;
        private MIL_ID MilSystem_Uper; // = MIL.M_NULL;
        private MIL_ID MilSystem_Down; // = MIL.M_NULL;
        private int NowCellDataCount = 3;
        private int NowCellGrabPos = 1;
        private int NowCellGrabber;
        private int NowCellNumber;
        private int NowCellType;
        private double NowCenter_Garo;
        private double NowCenter_Sero;
        private uint NowFailNumber_Down;
        private uint NowFailNumber_Uper;
        private uint NowFailNumber_Both;
        private int NowGapNumber;
        private string NowInspectResult = "OK"; //"NG"
        private double NowLength_Garo;
        private double NowLength_Sero;
        private double NowLength_Spea;
        private int NowModelDataCount = 6;
        private double NowModel_ATypeX;
        private double NowModel_ATypeY;
        private int NowModel_Array = 0;
        private double NowModel_CTypeX;
        private double NowModel_CTypeY;
        private int NowModel_Cells;
        private uint NowProdectNumber;
        private uint NowFailNumber;
        private uint NowTrigNumber;
        private uint NowPassNumber_Uper;
        private uint NowPassNumber_Down;
        private string NowUmacData = string.Empty;
        private double Real_Center_Garo;
        private double Real_Center_Sero;
        private double RectTheta;
        private CvPoint RectTheta1 = new CvPoint(0, 0);
        private CvPoint RectTheta2 = new CvPoint(0, 0);
        private List<string> RowTypeList = new List<string>();
        private List<string> RowsNoList = new List<string>();
        private List<string> SequNoList = new List<string>();
        private bool Timeout_Seq_Loop = true;
        private Thread Timeout_Sequence_Thread;
        //private bool VisionJobWorking = true;
        private bool VisionJobWorking = true;
        private List<string> ZoneNoList = new List<string>();
        public bool _CycleCompleteFlag_Down = true;
        public bool _CycleCompleteFlag_Uper = true;
        private DateTime _EndTime;
        private bool _FlagManual;
        private bool _Flag_Seq_TimeOut_End;
        //private IplImage _NowSavedImage = new IplImage();
        private DateTime _StrTime;
        private CvCapture _camera;

        private double _dCalibration_GaRo_Uper;
        private double _dCalibration_SeRo_Uper;
        private double _dCalibration_GaRo_Down;
        private double _dCalibration_SeRo_Down;
        private double _dNow_Image_Garo;
        private double _dNow_Image_Sero;
        private double _dVision_Grabber_Center_Angle;
        private double _dVision_Grabber_Center_X;
        private double _dVision_Grabber_Center_Y;
        private double _dVision_Offset_BiCell_Angle;
        private double _dVision_Offset_BiCell_Y;
        private double _dVision_Offset_ImageCenterX;
        private double _dVision_Offset_ImageCenterY;

        private double _dVision_Offset_Type_X;
        private double _dVision_Offset_Type_Y;
        private DateTime _dateTime_Eventing = new DateTime();

        private ControlDrawArea _drawArea_BiCell;
        private ControlDrawArea _drawArea_Gap;
        private DateTime _dtImageSaveTimeAfter_BiCell;
        private DateTime _dtImageSaveTimeAfter_Gap;
        private DateTime _dtImageSaveTimeBefore;
        private DateTime _dtInspDataSaveTime;
        private DateTime _dtInspLogFileTime;
        private DateTime _dtSaveCopyTime = new DateTime();
        private DateTime _dtUmacSetTime;
        private int _iAllHistoryViewNo_Uper;
        private int _iAllHistoryViewNo_Down;
        private int _iCamGrabCnt = 0;
        private int _iEdgeParam1_Uper;
        private int _iEdgeParam2_Uper;
        private int _iEdgeParam3_Uper;
        private int _iEdgeParam1_Down;
        private int _iEdgeParam2_Down;
        private int _iEdgeParam3_Down;
        private int _iExcelFileRowNo = 1;
        private int _iGrabImageGaro_Uper;
        private int _iGrabImageSero_Uper;
        private int _iGrabImageGaro_Down;
        private int _iGrabImageSero_Down;
        private int _iHistoryViewNo = -1;
        private int _iImgCount = -1;
        private int _iLineParam1_Uper;
        private int _iLineParam2_Uper;
        private int _iLineParam3_Uper;
        private int _iLineParam1_Down;
        private int _iLineParam2_Down;
        private int _iLineParam3_Down;
        private int _iManual_CellNo = -1;
        private int _iManual_CellType = -1;
        private int _iManual_GripNo = -1;
        private int _iNGHistoryViewNo_Uper;
        private int _iNGHistoryViewNo_Down;
        private int _iSavedCellNumber;
        private int _iSavedCellType;
        private int _iSavedGrabber;
        private uint _iSavedTrigNumber;

        private int _iTimeOutImagingValue;
        private int _iTimeOutStepValue;

        private int _intTestImgCount;
        private IplImage _nowIplImage_Down = new IplImage(4096, 3072, BitDepth.U8, 3); // = new IplImage();
        private IplImage _nowIplImage_Uper = new IplImage(4096, 3072, BitDepth.U8, 3);
        private PositionConvert _posConverter_Uper;
        private double _savedLength_Garo;
        private double _savedLength_Sepa;
        private double _savedLength_Sero;
        private List<string> _strHisImgName_Uper = new List<string>();
        private List<string> _strHisImgName_Down = new List<string>();
        //private string _strHisImgName_Down = string.Empty;
        private string _strNowExcelFileName = string.Empty;
        private string _strNowExcelRowNo = string.Empty;
        private string _strNowModel_Name = string.Empty;
        private string _strNowModel_Number = string.Empty;
        private string _strOldInspLogFileName = string.Empty;
        private string _strOldInspSaveFileName = string.Empty;
        private string _strResultData = string.Empty;
        private string[] _strSavedInspectOkorNG = {string.Empty, string.Empty, string.Empty};
        private string _strSavedInspectResult_Uper = string.Empty;
        private string _strSavedInspectResult_Down = string.Empty;
        private string _strTimeOutImagingOn;
        private string _strTimeOutStepOn;
        private StreamWriter _swInspLogFile;
        private List<CvPoint> boxArrowPntLst = new List<CvPoint>();
        private CvCapture cap;
        private AdjustableArrowCap cusCap;
        private CvPoint cvPntCentMarkImage = new CvPoint(0, 0);
        private List<CvPoint> cvPntLstCrossPntNew = new List<CvPoint>();
        public List<CvPoint> cvPntLstCrossPoint = new List<CvPoint>();
        public List<PointF> cvPntLstCrossPointF = new List<PointF>();
        public List<CvPoint> cvPntLstThetaPoint = new List<CvPoint>();
        private PointF cvPntNow_Image_CentPntF = new PointF(0, 0);
        //CvPoint cvPntNow_Image_CentPnt = new CvPoint(0,0);
        private Control_UltraMessageBox dlgInspMessageBox; // = new CUltraMessageBox();
        private int endflagcheck = 0;
        private Control_Excel excelFile;
        private Control_Files fileSystem;
        private Graphics gc_BiCell;
        private Graphics gc_Gap;
        private IplImage gray;
        private int iStandardLong_A = 1700;
        private int iStandardLong_C = 1670;
        private int iStandardShort_A = 1280;
        private int iStandardShort_C = 1255;
        private int inspItemCount = 14;
        private int lineQuentity = 3;
        private int m_InsertCarrierCount = 0;
        private int m_RailCount = 0;
        private Color m_Status_OFF_Gray = SystemColors.Control;
        private Color m_Status_ON_Green = Color.Lime;
        private Color m_Status_ON_Red = Color.Tomato;
        private double measureWidthResult = -1.0;
        private IplImage monoIplImage_Down = new IplImage(4096, 3072, BitDepth.U8, 1);
        private IplImage monoIplImage_Uper = new IplImage(4096, 3072, BitDepth.U8, 1);
        private IplImage nowInspectImage;
        private Control_PLC plc;
        private Point pntCenterMarkInspBox = new Point(0, 0);
        private IplImage result;
        private int searchSide = 1;
        private int shiftCenter = 5;
        private StreamWriter srExcelFile;
        private IplImage src;
        private List<double> strLstGapArrayData = new List<double>();
        private int traceNo = 1;
        private Control_UltraMessageBox uMessageBox = new Control_UltraMessageBox();
        private UltraDataSource[] udsInsCarrierDatas;
        private Control_UMAC umac;
        private static bool constructFlag = true;

        public FormDlgInsp()
        {
            InitializeComponent();
            constructFlag = true;
            Chart_Array_Make();
           
            for (int i = 0; i < 10; i++)
            {
                UperData_Charts[i].Axis.X.Labels.ItemFormat = AxisItemLabelFormat.None;
                UperData_Charts[i].Axis.X.Extent = 3;
                UperData_Charts[i].Axis.Y.Extent = 20;

                DownData_Charts[i].Axis.X.Labels.ItemFormat = AxisItemLabelFormat.None;
                DownData_Charts[i].Axis.X.Extent = 3;
                DownData_Charts[i].Axis.Y.Extent = 20;
            }
        }

       
        
        public IplImage GetSet_NowIplImage
        {
            get { return SrcIplImage_Uper; }
            set { SrcIplImage_Uper = value; }
        }
        
        public List<string> GetSet_GridDisplayData_Uper
        {
            get { return strLstDisplayData_Uper; }
        }

        public List<string> GetSet_GridDisplayData_Down
        {
            get { return strLstDisplayData_Down; }
        }

        public int GetSet_NowGapNomber
        {
            get { return NowGapNumber; }
            set { NowGapNumber = value; }
        }


        private Graphics gc_Uper;

        public Graphics GetSet_Draw_GC_Uper
        {
            get { return gc_Uper; }
            set { gc_Uper = value; }
        }

        private Graphics gc_Down;

        public Graphics GetSet_Draw_GC_Down
        {
            get { return gc_Down; }
            set { gc_Down = value; }
        }

        private Graphics gc_List;

        public Graphics GetSet_Draw_GC_List
        {
            get { return gc_List; }
            set { gc_List = value; }
        }

        public Control_Files GetSet_FileSystem
        {
            get { return fileSystem; }
            set { fileSystem = value; }
        }

        public ControlDrawArea GetSet_DrawArea_Gap
        {
            get { return _drawArea_Gap; }
            set { _drawArea_Gap = value; }
        }

        public ControlDrawArea GetSet_DrawArea_BiCell
        {
            get { return _drawArea_BiCell; }
            set { _drawArea_BiCell = value; }
        }

        public string GetSet_NowModel_Name
        {
            get { return _strNowModel_Name; }
            set { _strNowModel_Name = value; }
        }

        public string GetSet_NowModel_Number
        {
            get { return _strNowModel_Number; }
            set { _strNowModel_Number = value; }
        }

        public Control_Excel GetSet_ExcelFile
        {
            get { return excelFile; }
            set { excelFile = value; }
        }

        public Control_UMAC GetSet_UMAC_System
        {
            get { return umac; }
            set { umac = value; }
        }

        public Control_PLC GetSet_PLC_System
        { 
            get { return plc; }
            set { plc = value; }
        }

        public Control_PN2212 GetSet_LVS_System { get; set; }

        public PositionConvert GetSet_Converter_Uper
        {
            get { return _posConverter_Uper; }
            set { _posConverter_Uper = value; }
        }

        public PositionConvert GetSet_Converter_Down
        {
            get { return _posConverter_Down; }
            set { _posConverter_Down = value; }
        }

        public CInspection_Lamination GetSet_LamiSystem
        {
            get { return LamiSystem; }
            set
            {
                LamiSystem = value;
            }
        }

        public Control_UltraMessageBox UMessageBox
        {
            get { return uMessageBox; }
            set { uMessageBox = value; }
        }
        //Control_Inspect_Gap inspection_Gap = new Control_Inspect_Gap();

        public event MyEventInsp_Pix01 Pix_Loading;

        public event MyEventOneInsp OperationEvent;
//         public event MyEventTwoInsp TestStartEvent;
//         public event MyEventMeaData1 MeaDataEvent1;
//         public event MyEventMeaData2 MeaDataEvent2;
//         public event MyEventMeaData3 MeaDataEvent3;
//         public event MyEventMeaData4 MeaDataEvent4;

        private Process myProcess;

        public bool GetSet_PixLoad { get; set; }
        private void FormDlgInsp_Load(object sender, EventArgs e)
        {
            ////Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            /// 
            
            StartPosition = FormStartPosition.Manual;   //968, 706
            Location = new Point(0, 3);
            timer1.Enabled = true;
            fileSystem = new Control_Files();
            excelFile = new Control_Excel();

            Now_PictureBox = Inspect_Main01_IplBox;
            Now_Used_Form = "Inspect";
            
#if(SYST_SIMUL)
            Inspect_Ready_Run();
#else
            
#endif
            Inspect_DataSet_Initialize_Fron_Upper();
            Inspect_DataSet_Initialize_Rear_Upper();

            Measurement_Grid_Resize(uGrd_Inspect_Measure_Uper);
            Measurement_Grid_Resize(uGrd_Inspect_Measure_Down);

            FormDlgInsp_Init_View();

            Inspect_Initionalize();

            Inspect_Ready_Ready();


           
            //Inspect_Allocate_Memory();

            //Measurement_Register_To_Grid_Uper();
            //Measurement_Register_To_Grid_Down();

            Measurement_List_To_GraphList_Uper();
            Measurement_List_To_GraphList_Down();

            Measure_Grid_Making_Uper();
            Measure_Grid_Making_Down();

            Chart_Making_Uper();
            Chart_Making_Down();

            //20150305 WKB 209
            Inspect_Allocate_Memory();

            MeasureData_Loading();

            Product_Data_Init();
        }


        public void Inspect_DataSet_Initialize_Fron_Upper()
        {
            if (dataSet1.Tables.Count != 0) return;

            dataSet1.Tables.Add("Meas");
            dataSet1.Tables["Meas"].Columns.Add("NO");
            dataSet1.Tables["Meas"].Columns.Add("항목");
            dataSet1.Tables["Meas"].Columns.Add("중심");
            dataSet1.Tables["Meas"].Columns.Add("상한");
            dataSet1.Tables["Meas"].Columns.Add("하한");
            dataSet1.Tables["Meas"].Columns.Add("측정");
            dataSet1.Tables["Meas"].Columns.Add("판정");
            dataSet1.Tables["Meas"].Columns.Add("NG");
            dataSet1.Tables["Meas"].Columns.Add("수율");
            dataSet1.Tables["Meas"].Columns.Add("평균");
            dataSet1.Tables["Meas"].Columns.Add("편차");
            dataSet1.Tables["Meas"].Columns.Add("최소");
            dataSet1.Tables["Meas"].Columns.Add("최대");
            dataSet1.Tables["Meas"].Columns.Add("CP");
            dataSet1.Tables["Meas"].Columns.Add("CPK");
            
            //dataSet1.Tables["Meas"].Columns.Add("OkCount");
            //dataSet1.Tables["Meas"].Columns.Add("SumValue");
            //dataSet1.Tables["Meas"].Columns.Add("SeqValue");
            //dataSet1.Tables["Meas"].Columns.Add("ProductOK");

        }


        public void Inspect_DataSet_Initialize_Rear_Upper()
        {
            if (dataSet2.Tables.Count != 0) return;

            dataSet2.Tables.Add("Meas");
            dataSet2.Tables["Meas"].Columns.Add("NO");
            dataSet2.Tables["Meas"].Columns.Add("항목");
            dataSet2.Tables["Meas"].Columns.Add("중심");
            dataSet2.Tables["Meas"].Columns.Add("상한");
            dataSet2.Tables["Meas"].Columns.Add("하한");
            dataSet2.Tables["Meas"].Columns.Add("측정");
            dataSet2.Tables["Meas"].Columns.Add("판정");
            dataSet2.Tables["Meas"].Columns.Add("NG");
            dataSet2.Tables["Meas"].Columns.Add("수율");
            dataSet2.Tables["Meas"].Columns.Add("평균");
            dataSet2.Tables["Meas"].Columns.Add("편차");
            dataSet2.Tables["Meas"].Columns.Add("최소");
            dataSet2.Tables["Meas"].Columns.Add("최대");
            dataSet2.Tables["Meas"].Columns.Add("CP");
            dataSet2.Tables["Meas"].Columns.Add("CPK");
            
        }

        public void Product_Data_Init()
        {
            Inspect_Count_RegData_Reset();

            //Measure_Grid_Making_Uper();
            //Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);

            //Measure_Grid_Making_Down();
            //Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Down, uGrd_Inspect_Measure_Down);

            Chart_Making_Uper();
            Chart_Making_Down();

            inspect_Run_Run_Display_NG_OK_Count();
        }

        public void Measurement_TempList_To_Register_Uper()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Uper, true);
            if (reg != null)
            {
                reg.Close();
                Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathMeasure_Uper, false);
            }

            for (int i = 0; i < LamiSystem.StrLstMeasureData_TempUper.Count; i++)
            {

                this.SetReg(LamiSystem.RegPathMeasure_Uper, i.ToString("000"), LamiSystem.StrLstMeasureData_TempUper[i]);
            }
        }

        public void Measurement_TempList_To_Register_Down()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Down, true);
            if (reg != null)
            {
                reg.Close();
                Registry.CurrentUser.DeleteSubKey(LamiSystem.RegPathMeasure_Down, false);
            }

            for (int i = 0; i < LamiSystem.StrLstMeasureData_TempDown.Count; i++)
            {
                this.SetReg(LamiSystem.RegPathMeasure_Down, i.ToString("000"), LamiSystem.StrLstMeasureData_TempDown[i]);
            }
        }

        public struct TempList_MeasureData
        {
            public int RegAddress;
            public int DataCount;
            public RegistryKey reg;
            public List<string> TempRegList;
        }

        public void Measurement_Register_To_TempList_Uper()
        {
            TempList_MeasureData Struct_Temp = new TempList_MeasureData();
            Struct_Temp.reg = Registry.CurrentUser;
            Struct_Temp.reg = Struct_Temp.reg.OpenSubKey(LamiSystem.RegPathMeasure_Uper, true);
            if (Struct_Temp.reg == null)
             {
                 //Measure_Grid_Making_Uper();
                 return;
             }

            Struct_Temp.DataCount = Struct_Temp.reg.ValueCount;
            Struct_Temp.RegAddress = 0;

            LamiSystem.StrLstMeasureData_TempUper.Clear();

            for (int i = 0; i < Struct_Temp.DataCount; i++)
            {
                LamiSystem.StrLstMeasureData_TempUper.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper, i.ToString("000")));
            }
        }

        public void Measurement_Register_To_TempList_Down()
        {
            TempList_MeasureData Struct_Temp = new TempList_MeasureData();
            Struct_Temp.reg = Registry.CurrentUser;
            Struct_Temp.reg = Struct_Temp.reg.OpenSubKey(LamiSystem.RegPathMeasure_Down, true);
            if (Struct_Temp.reg == null)
            {
                //Measure_Grid_Making_Uper();
                return;
            }

            Struct_Temp.DataCount = Struct_Temp.reg.ValueCount;
            Struct_Temp.RegAddress = 0;

            LamiSystem.StrLstMeasureData_TempDown.Clear();

            for (int i = 0; i < Struct_Temp.DataCount; i++)
            {
                LamiSystem.StrLstMeasureData_TempDown.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, i.ToString("000")));
            }
        }

        public struct MeasureData_Loading_Struct
        {
            public RegistryKey reg;
            public int regCount;
            public int ColCount;
            public int RowCount;
            public string regData;
            public string[] tmpreg;
            public int GraphNum;// = int.Parse(tmpreg[i]) / 400;
            public int RowsNum;// = (int.Parse(tmpreg[i]) - (GraphNum * 400)) / 100;
            public int ColNum;// = (int.Parse(tmpreg[i]) - (GraphNum * 400)) - (RowsNum * 100);
            public double tmpregData;// = double.Parse(regData);
        }

        public void MeasureData_Loading()
        {
            if (LamiSystem.GetSet_Model_Changed_Flag == true)
            {
                LamiSystem.GetSet_Model_Changed_Flag = false;
                return;
            }

            MeasureData_Loading_Struct Data_Struct = new MeasureData_Loading_Struct();
            Data_Struct.reg = Registry.CurrentUser;
            Data_Struct.reg = Data_Struct.reg.OpenSubKey(LamiSystem.RegPathMeasureGrid_Buf_Uper, true);
            if (Data_Struct.reg != null)
            {
                Data_Struct.regCount = Data_Struct.reg.ValueCount;
                Data_Struct.ColCount = 19;

                Data_Struct.RowCount = Data_Struct.regCount / Data_Struct.ColCount;

                for (int i = 0; i < Data_Struct.RowCount; i++)
                {
                    for (int j = 0; j < Data_Struct.ColCount; j++)
                    {
                        Data_Struct.regData = this.GetReg(LamiSystem.RegPathMeasureGrid_Buf_Uper, (i * Data_Struct.ColCount + j).ToString("000"));
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue(j, Data_Struct.regData);
                    }
                }
            }

            Data_Struct.reg = Registry.CurrentUser;
            Data_Struct.reg = Data_Struct.reg.OpenSubKey(LamiSystem.RegPathMeasureGrid_Buf_Down, true);
            if (Data_Struct.reg != null)
            {
                Data_Struct.regCount = Data_Struct.reg.ValueCount;
                Data_Struct.ColCount = 19;

                Data_Struct.RowCount = Data_Struct.regCount / Data_Struct.ColCount;

                for (int i = 0; i < Data_Struct.RowCount; i++)
                {
                    for (int j = 0; j < Data_Struct.ColCount; j++)
                    {
                        Data_Struct.regData = this.GetReg(LamiSystem.RegPathMeasureGrid_Buf_Down, (i * Data_Struct.ColCount + j).ToString("000"));
                        uDS_Inspect_Measure_Down.Rows[i].SetCellValue(j, Data_Struct.regData);
                    }
                }
            }

            Data_Struct.reg = Registry.CurrentUser;
            Data_Struct.reg = Data_Struct.reg.OpenSubKey(LamiSystem.RegPathMeasureChart_Buf_Uper, true);

            if (Data_Struct.reg != null)
            {
                Data_Struct.tmpreg = Data_Struct.reg.GetValueNames();
                Data_Struct.regCount = Data_Struct.reg.ValueCount;

                for (int i = 0; i < Data_Struct.regCount; i++)
                {
                    Data_Struct.regData = this.GetReg(LamiSystem.RegPathMeasureChart_Buf_Uper, Data_Struct.tmpreg[i]);
                    Data_Struct.GraphNum = int.Parse(Data_Struct.tmpreg[i]) / 400;
                    Data_Struct.RowsNum = (int.Parse(Data_Struct.tmpreg[i]) - (Data_Struct.GraphNum * 400)) / 100;
                    Data_Struct.ColNum = (int.Parse(Data_Struct.tmpreg[i]) - (Data_Struct.GraphNum * 400)) - (Data_Struct.RowsNum * 100);
                    Data_Struct.tmpregData = double.Parse(Data_Struct.regData);
                    Uper_MeasureTables[Data_Struct.GraphNum].Rows[Data_Struct.RowsNum][Data_Struct.ColNum] = double.Parse(Data_Struct.regData);
                }
            }

            Data_Struct.reg = Registry.CurrentUser;
            Data_Struct.reg = Data_Struct.reg.OpenSubKey(LamiSystem.RegPathMeasureChart_Buf_Down, true);
            if (Data_Struct.reg != null)
            {
                Data_Struct.tmpreg = Data_Struct.reg.GetValueNames();
                Data_Struct.regCount = Data_Struct.reg.ValueCount;

                for (int i = 0; i < Data_Struct.regCount; i++)
                {
                    Data_Struct.regData = this.GetReg(LamiSystem.RegPathMeasureChart_Buf_Down, Data_Struct.tmpreg[i]);
                    Data_Struct.GraphNum = int.Parse(Data_Struct.tmpreg[i]) / 400;
                    Data_Struct.RowsNum = (int.Parse(Data_Struct.tmpreg[i]) - (Data_Struct.GraphNum * 400)) / 100;
                    Data_Struct.ColNum = (int.Parse(Data_Struct.tmpreg[i]) - (Data_Struct.GraphNum * 400)) - (Data_Struct.RowsNum * 100);
                    Data_Struct.tmpregData = double.Parse(Data_Struct.regData);
                    Down_MeasureTables[Data_Struct.GraphNum].Rows[Data_Struct.RowsNum][Data_Struct.ColNum] = double.Parse(Data_Struct.regData);
                }
            }
        }

        /*
        
        public void MeasureData_Loading()
        {
            if (LamiSystem.GetSet_Model_Changed_Flag == true)
            {
                LamiSystem.GetSet_Model_Changed_Flag = false;
                return;
            }

            MeasureData_Loading_Struct Data_Struct = new MeasureData_Loading_Struct();
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureGrid_Buf_Uper, true);
            if (reg != null)
            {
                int regCount = reg.ValueCount;
                int ColCount = 19;

                int RowCount = regCount/ColCount;

                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        string regData = this.GetReg(LamiSystem.RegPathMeasureGrid_Buf_Uper,(i*ColCount + j).ToString("000"));
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue(j, regData);
                    }
                }
            }

            reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureGrid_Buf_Down, true);
            if (reg != null)
            {
                int regCount = reg.ValueCount;
                int ColCount = 19;

                int RowCount = regCount / ColCount;

                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        string regData = this.GetReg(LamiSystem.RegPathMeasureGrid_Buf_Down, (i * ColCount + j).ToString("000"));
                        uDS_Inspect_Measure_Down.Rows[i].SetCellValue(j, regData);
                    }
                }
            }

            reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureChart_Buf_Uper, true);
            
            if (reg != null)
            {
                string[] tmpreg = reg.GetValueNames();
                int regCount = reg.ValueCount;

                for (int i = 0; i < regCount; i++)
                {
                    string regData = this.GetReg(LamiSystem.RegPathMeasureChart_Buf_Uper, tmpreg[i]);
                    int GraphNum = int.Parse(tmpreg[i]) / 400;
                    int RowsNum = (int.Parse(tmpreg[i]) - (GraphNum * 400)) / 100;
                    int ColNum = (int.Parse(tmpreg[i]) - (GraphNum * 400)) - (RowsNum * 100);
                    double tmpregData = double.Parse(regData);
                    Uper_MeasureTables[GraphNum].Rows[RowsNum][ColNum] = double.Parse(regData);
                }
            }
            
            reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasureChart_Buf_Down, true);
            if (reg != null)
            {
                string[] tmpreg = reg.GetValueNames();
                int regCount = reg.ValueCount;

                for (int i = 0; i < regCount; i++)
                {
                    string regData = this.GetReg(LamiSystem.RegPathMeasureChart_Buf_Down, tmpreg[i]);
                    int GraphNum = int.Parse(tmpreg[i]) / 400;
                    int RowsNum = (int.Parse(tmpreg[i]) - (GraphNum * 400)) / 100;
                    int ColNum = (int.Parse(tmpreg[i]) - (GraphNum * 400)) - (RowsNum * 100);
                    double tmpregData = double.Parse(regData);
                    Down_MeasureTables[GraphNum].Rows[RowsNum][ColNum] = double.Parse(regData);
                }
            }
        }

        */

        //List<string> Graph_No_Array = new List<string>();
        private string[] Graph_No_Now_Uper;
        
        public void Measurement_List_To_GraphList_Uper()
        {
            Graph_No_Now_Uper = new string[40];
            int rowCount = LamiSystem.StrLstRcpConGridData_Uper.Count/11;

            for (int i = 0; i < rowCount; i++)
            {
                int CellData = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[(i * 11) + 1]);
                for (int j = 0; j < 4; j++)
                {
                    if (Graph_No_Now_Uper[(CellData - 1)*4 + j] == null)
                    {
                        Graph_No_Now_Uper[(CellData - 1) * 4 + j] = LamiSystem.StrLstRcpConGridData_Uper[i * 11];
                        break;
                    }
                }
            }
        }

        private string[] Graph_No_Now_Down;
        public void Measurement_List_To_GraphList_Down()
        {
            Graph_No_Now_Down = new string[40];
            int rowCount = LamiSystem.StrLstRcpConGridData_Down.Count / 11;

            for (int i = 0; i < rowCount; i++)
            {
                int CellData = int.Parse(LamiSystem.StrLstRcpConGridData_Down[(i * 11) + 1]);
                for (int j = 0; j < 4; j++)
                {
                    if (Graph_No_Now_Down[(CellData - 1) * 4 + j] == null)
                    {
                        Graph_No_Now_Down[(CellData - 1) * 4 + j] = LamiSystem.StrLstRcpConGridData_Down[i * 11];
                        break;
                    }
                }
            }
        }
        /*
         private void FormDlgInsp_Load(object sender, EventArgs e)
        {
            ////Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            StartPosition = FormStartPosition.Manual;   //968, 706
            Location = new Point(0, 3);
            timer1.Enabled = true;
            fileSystem = new Control_Files();
            excelFile = new Control_Excel();

            Now_PictureBox = Inspect_Main01_IplBox;
            Now_Used_Form = "Inspect";

            FormDlgInsp_Init_View();

            //if (Inspect_Get_ModelData_From_UMAC() == false)
            //{
            //    if (Inspect_Get_ModelData_From_UMAC() == false)
            //    {
            //        MessageBox.Show("유멕과 통신에서 오류가 발생했습니다. 프로그램을 종료합니다.");
            //        return;
            //    }
            //}

            Default_Number_Set_Uper();
            //Default_Number_Set_Down();
           
            Data_Mixing_Make_Uper();
            Inspect_Initionalize_Uper();
            
            //Inspection_BiCell_Object_Setting();
            //inspection_BiCell.Inspect_BiCell_Initionalize();
            //inspection_BiCell.OperationEvent_BiCell += myEvent_Inspect_BiCell_01;
            
            Inspect_Ready_Ready();
            Inspect_Ready_Run();

            Inspect_Allocate_Memory();
            Inspect_uButton_Manual20.PerformClick();

            Chart_Making_Uper();
            Chart_Making_Down();
        }
        
        public void Default_Number_Set_Uper()
        {
            //uTxt_Gap_No
            switch (NowModel_Cells)
            {
                case 11:
                    uTxt_Gap_No.Text = "8";
                    break;
                case 13:
                    uTxt_Gap_No.Text = "9";
                    break;
                case 15:
                    uTxt_Gap_No.Text = "0";
                    break;
                case 17:
                    uTxt_Gap_No.Text = "13";
                    break;
                case 19:
                    uTxt_Gap_No.Text = "11";
                    break;
                case 21:
                    uTxt_Gap_No.Text = "8";
                    break;
            }
        }
*/
        static IplImage srcImgGray = new IplImage(4096, 3072, BitDepth.U8, 1);

        private void Inspect_Allocate_Memory()
        {
            try
            {
                NowGapNumber = 0;
                string tmpCount = NowModel_Cells.ToString();
                Run_Mode = "Manual";
                
                Inspect_Main01_IplBox.ImageIpl = IplImage.FromBitmap(Properties.Resources.BiCell_Top);
                Inspect_Main02_IplBox.ImageIpl = IplImage.FromBitmap(Properties.Resources.BiCell_Bot);

                bool Serarch_Result_Uper = Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(Inspect_Main01_IplBox.ImageIpl);
                bool Serarch_Result_Lwer = Inspect_Run_Run_ROI_EdgeLine_Centering_Down(Inspect_Main02_IplBox.ImageIpl);

                //Measure_Grid_Making_Uper();
                //Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);
                //Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
                //Inspect_Run_Run_FindData_Inspection_Uper();

                //Measure_Grid_Making_Down();
                //Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Down, uGrd_Inspect_Measure_Down);
                //Inspect_Run_Run_ROI_CenterPoint_Find_Down();
                //Inspect_Run_Run_FindData_Inspection_Down();
                
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        private void Inspect_Allocate_Memory()
        {
            try
            {
                NowGapNumber = 0;
                string tmpCount = NowModel_Cells.ToString();
                Run_Mode = "Manual";
                
                Inspect_Main01_IplBox.ImageIpl = IplImage.FromBitmap(Properties.Resources.BiCell_Top);
                Inspect_Main02_IplBox.ImageIpl = IplImage.FromBitmap(Properties.Resources.BiCell_Bot);

                bool Serarch_Result_Uper = Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(Inspect_Main01_IplBox.ImageIpl);
                bool Serarch_Result_Lwer = Inspect_Run_Run_ROI_EdgeLine_Centering_Down(Inspect_Main02_IplBox.ImageIpl);

                Measure_Grid_Making_Uper();
                Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
                Inspect_Run_Run_FindData_Inspection_Uper();
                

                Measure_Grid_Making_Down();
                Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Down, uGrd_Inspect_Measure_Down);
                Inspect_Run_Run_ROI_CenterPoint_Find_Down();
                Inspect_Run_Run_FindData_Inspection_Down();
                
                
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();
            }
            catch (Exception)
            {

                throw;
            }
        }
        */
        /*
        private void FormDlgInsp_Load(object sender, EventArgs e)
        {
            ////Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            StartPosition = FormStartPosition.Manual;   //968, 706
            Location = new Point(0, 3);
            timer1.Enabled = true;
            fileSystem = new Control_Files();
            excelFile = new Control_Excel();
            
            
            GapBiCell_Status_Loading_To_Reg();

            FormDlgInsp_Init_View();

            if (Inspect_Get_ModelData_From_UMAC() == false)
            {
                if (Inspect_Get_ModelData_From_UMAC() == false)
                {
                    MessageBox.Show("유멕과 통신에서 오류가 발생했습니다. 프로그램을 종료합니다.");
                    return;
                }
            }

            
            Inspection_Gap_Object_Setting();
            inspection_Gap.Gap_Data_Mixing_Make();
            inspection_Gap.Inspect_Gap_Initionalize();
            inspection_Gap.OperationEvent_Gap1 += myEvent_Inspect_Gap_01;
            inspection_Gap.OperationEvent_Gap2 += myEvent_Inspect_Gap_02;

            
            Inspection_BiCell_Object_Setting();
            inspection_BiCell.Inspect_BiCell_Initionalize();
            inspection_BiCell.OperationEvent_BiCell += myEvent_Inspect_BiCell_01;
            
            Inspect_Ready_Ready();
            Inspect_Ready_Run();

            Inspect_uButton_Manual20.PerformClick();
        }
 
        */
        private readonly List<double> Uper_Data_Minus_Array = new List<double>();
        private readonly List<double> Uper_Data_Pluse_Array = new List<double>();
        public void Data_Mixing_Make_Uper()
        {
            
            string NameData = LamiSystem.StrListVisConGridData_Uper[0];

            Uper_Data_Max_Array.Clear();
            Uper_Data_Min_Array.Clear();
            Uper_Data_Pluse_Array.Clear();
            Uper_Data_Minus_Array.Clear();

            int InsCount = LamiSystem.StrLstRcpConGridData_Uper.Count / 11;
            for (int i = 0; i < InsCount; i++)
            {
                string RecipeName = LamiSystem.StrLstRcpConGridData_Uper[i*11];
                int VisCount = LamiSystem.StrListVisConGridData_Uper.Count/8;
                for (int j = 0; j < VisCount; j++)
                {
                    string VisionName = LamiSystem.StrListVisConGridData_Uper[i*8];
                    if (RecipeName == VisionName)
                    {
                        double dNumberPluse = 0.0;
                        bool resultPluse = double.TryParse(LamiSystem.StrListVisConGridData_Uper[8 * i + 2], out dNumberPluse);
                        Uper_Data_Pluse_Array.Add(dNumberPluse);

                        double dNumberMinus = 0.0;
                        bool resultMinus = double.TryParse(LamiSystem.StrListVisConGridData_Uper[8 * i + 3], out dNumberMinus);
                        Uper_Data_Minus_Array.Add(dNumberMinus);

                        break;
                    }
                }

                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(LamiSystem.StrListVisConGridData_Uper[8 * i + 1], out dNumberCenter);
                Now_Model_Data_Array_Uper.Add(dNumberCenter);

                Uper_Data_Max_Array.Add(Now_Model_Data_Array_Uper[i] + Uper_Data_Pluse_Array[i]);
                Uper_Data_Min_Array.Add(Now_Model_Data_Array_Uper[i] - Uper_Data_Minus_Array[i]);
            }
        }


        private List<double> Uper_Data_Max_Array = new List<double>();
        private List<double> Uper_Data_Min_Array = new List<double>();
        private List<double> Down_Data_Max_Array = new List<double>();
        private List<double> Down_Data_Min_Array = new List<double>();
        private readonly List<double> Down_Data_Minus_Array = new List<double>();
        private readonly List<double> Down_Data_Pluse_Array = new List<double>();
        public void Data_Mixing_Make_Down()
        {

            string NameData = LamiSystem.StrListVisConGridData_Down[0];

            Down_Data_Max_Array.Clear();
            Down_Data_Min_Array.Clear();
            Down_Data_Pluse_Array.Clear();
            Down_Data_Minus_Array.Clear();

            int InsCount = LamiSystem.StrLstRcpConGridData_Down.Count / 11;
            for (int i = 0; i < InsCount; i++)
            {
                string RecipeName = LamiSystem.StrLstRcpConGridData_Down[i * 11];
                int VisCount = LamiSystem.StrListVisConGridData_Down.Count / 8;
                for (int j = 0; j < VisCount; j++)
                {
                    string VisionName = LamiSystem.StrListVisConGridData_Down[i * 8];
                    if (RecipeName == VisionName)
                    {
                        double dNumberPluse = 0.0;
                        bool resultPluse = double.TryParse(LamiSystem.StrListVisConGridData_Down[8 * i + 2], out dNumberPluse);
                        Down_Data_Pluse_Array.Add(dNumberPluse);

                        double dNumberMinus = 0.0;
                        bool resultMinus = double.TryParse(LamiSystem.StrListVisConGridData_Down[8 * i + 3], out dNumberMinus);
                        Down_Data_Minus_Array.Add(dNumberMinus);

                        break;
                    }
                }

                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(LamiSystem.StrListVisConGridData_Down[8 * i + 1], out dNumberCenter);
                Now_Model_Data_Array_Down.Add(dNumberCenter);

                Down_Data_Max_Array.Add(Now_Model_Data_Array_Down[i] + Down_Data_Pluse_Array[i]);
                Down_Data_Min_Array.Add(Now_Model_Data_Array_Down[i] - Down_Data_Minus_Array[i]);
            }
        }

        public void Now_Status_Loading_To_Reg()
        {
            NowPassNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Uper"));
            NowPassNumber_Down = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Down"));
            NowTrigNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Trigger"));
            NowFailNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Both"));
            NowFailNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Uper"));
            NowFailNumber_Down = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Down"));
            NowProdectNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Product"));
        }

        private void FormDlgInsp_Init_View()
        {
            //uTxt_Gap_No.Text = NowGapNumber.ToString();
            uTxt_Gap_No.Text = "8";

            NowProdectNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Product"));
           
            //Inspect_Count_RegData_Reset();
            
            Inspect_uLabel_Assy05.Text = NowFailNumber_Uper.ToString("0") + " / " + NowProdectNumber.ToString();
            Inspect_uLabel_Assy06.Text = NowFailNumber_Down.ToString("0") + " / " + NowProdectNumber.ToString();
            Inspect_uLabel_Assy07.Text = NowFailNumber.ToString("0") + " / " + NowProdectNumber.ToString();
            //갭 현재 모델 정보를 나타냄
            Inspect_uLabel_Assy04.Text = _strNowModel_Name + " - " + _strNowModel_Number;
            

            //현재 사용자 모드를 나타낸다.
            Inspect_uLabel_System10.Text = Inspect_User_Account_Check(CInspection_Folding_Align.Account_Operator);

            //사용자 로그 시간을 나타낸다.
            Inspect_uLabel_System11.Text = Inspect_Login_Time_Check(LamiSystem.GetSet_Now_Login_Time);


            //하드 디스크 사용량 나타냄 DiskUsed
            //string dirveName = GapSystem.StrListSysConData[2].Substring(0, 1);
#if(SYST_SIMUL)
#else
            Inspect_uLabel_System13.Text = Inspect_DiskSpace_Check(LamiSystem.StrListSysConData[2].Substring(0, 1));
#endif
            Inspect_Main01_IplBox.ImageIpl = null;
            Inspect_Main02_IplBox.ImageIpl = null;

            Inspect_uButton_Manual18.Enabled = false;
            Inspect_uButton_Manual15.Enabled = true;
            Inspect_uButton_Manual20.Enabled = true;
            Inspect_uButton_Manual21.Enabled = true;

            Inspect_Main01_IplBox.Visible = true;
            Inspect_Main02_IplBox.Visible = false;
            uPanel_Uper.Visible = false;
            uPanel_Down.Visible = false;
        }


        public bool Inspect_Get_ModelData_From_UMAC()
        {
            //인스펙션 로그 파일 기록 함수 호출 (201:현재 모델 정보 획득)
            //FormDlgInsp_Inspection_Write_LogFile(201);

            //if (GapSystem.IsConnect_UMAC == false)
            //{
            //    uMessageBox.SystemConnectStatus_UMAC("UMAC");
            //    return false;
            //}

            string strResponse = umac.Umac_Communicate_Command("P5001,5");
            string[] strArray = strResponse.Split('\r');

            int iNumber = 0;
            double dNumber = 0.0;
            bool result = false;
            for (int i = 0; i < 5; i++)
            {
                iNumber = 0;
                dNumber = 0.0;
                result = false;

                switch (i)
                {
                    case 0:
                        
#if(BiCellTest)
                        if (checkBox1.Checked == true) NowModel_Cells = 19;
                        else NowModel_Cells = 11 ;
                        break;
#else
                        result = int.TryParse(strArray[i], out iNumber);
                        if (result) NowModel_Cells = iNumber;
                         else return false;
                        break;
#endif
                    case 1:
                        result = double.TryParse(strArray[i], out dNumber);
                        if (result) NowModel_ATypeX = dNumber;
                        else return false;
                        break;

                    case 2:
                        result = double.TryParse(strArray[i], out dNumber);
                        if (result) NowModel_ATypeY = dNumber;
                        else return false;
                        break;

                    case 3:
                        result = double.TryParse(strArray[i], out dNumber);
                        if (result) NowModel_CTypeX = dNumber;
                        else return false;
                        break;

                    case 4:
                        result = double.TryParse(strArray[i], out dNumber);
                        if (result) NowModel_CTypeY = dNumber;
                        else return false;
                        break;
                }
            }

            //현재 모델의 셋타입 배열을 가져온다.
            strResponse = umac.Umac_Communicate_Command("P5101, " + NowModel_Cells);
            string[] strArray2 = strResponse.Split('\r');

            if (strArray2.Length < NowModel_Cells) return false;

            string tmpStr = string.Empty;
            Now_Model_Cell_Type_Array.Clear();
            for (int i = 0; i < NowModel_Cells; i++)
            {
                if (strArray2[i].Trim() == "1") tmpStr = "A";
                else if (strArray2[i].Trim() == "2") tmpStr = "C";
                else return false;
                Now_Model_Cell_Type_Array.Add(tmpStr);
            }

            strResponse = umac.Umac_Communicate_Command("P5200, " + NowModel_Cells);
            string[] floatArray = strResponse.Split('\r');

            if (floatArray.Length < NowModel_Cells) return false;

            Now_Model_Data_Array_Uper.Clear();
            for (int i = 0; i < NowModel_Cells; i++)
            {
                result = double.TryParse(floatArray[i].Trim(), out dNumber);
                if (result == false) return false;
                Now_Model_Data_Array_Uper.Add(dNumber);
            }

            return true;
        }

        /*
        private void Inspection_Gap_Object_Setting()
        {
            //초기화 작업에서는 이미지와 호출 객체를 설정하지 않고
            //사용시에 설정하도록 한다.
            //inspection_Gap.GetSet_NowIplImage = Inspect_Main01_IplBox.ImageIpl;
            //inspection_Gap.GetSet_Calling_Form = this.Name;
            //갭 인스펙션의 셀수량 변수에 값을 할당한다.
            //Inspection_Gap_Object_Setting();
            inspection_Gap.GetSet_NowModel_Cells = NowModel_Cells;
            inspection_Gap.GetSet_NowModel_Gap_Data = Now_Model_Gap_Data_Array;

            gc_Gap = Inspect_Main01_IplBox.CreateGraphics();
            inspection_Gap.GetSet_GapSystem = GapSystem;
            inspection_Gap.GetSet_Converter = _posConverter_Gap;
            inspection_Gap.GetSet_FileSystem = fileSystem;
            inspection_Gap.GetSet_Graphics = gc_Gap;
            inspection_Gap.GetSet_ImageBoxIpl = Inspect_Main01_IplBox;
            inspection_Gap.GetSet_PLCSystem = plc;
        }
        */

        /*
        public void Inspect_Gap_Status_Display()
        {
            myEvent_Inspect_Gap_01(1,1);
        }

        public void myEvent_Inspect_Gap_01(int GapNo, uint ProductNo)
        {
            if (InvokeRequired)
            {
                Delegate_myEvent_Inspect_Gap_01 del = myEvent_Inspect_Gap_01;
                Invoke(del, GapNo, ProductNo);
            }
            else
            {
                //NowGapNumber, NowProdectNumber_Gap
                uTxt_Gap_No.Text = NowGapNumber.ToString();
                uTxt_Gap_No.Refresh();

                //NowProdectNumber_Gap = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus, "Count_Trigger"));

                //Inspect_uLabel_Assy05.Text = NowProdectNumber_Gap.ToString();
                Inspect_uLabel_Assy05.Text = NowProdectNumber.ToString();//Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus, "Count_Trigger");
                Inspect_uLabel_Assy05.Refresh();

                Inspect_uLabel_Assy06.Text = NowFailNumber_Uper.ToString("0") + " / " + NowTrigNumber;
                Inspect_uLabel_Assy06.Refresh();

                Inspect_uLabel_Assy07.Text = NowGapNumber.ToString();
                Inspect_uLabel_Assy07.Refresh();
            }
        }
        */
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
                uTxt_Gap_No.Text = NowGapNumber.ToString();
                uTxt_Gap_No.Refresh();

                //NowProdectNumber_Gap = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus, "Count_Trigger"));

                //Inspect_uLabel_Assy05.Text = NowProdectNumber_Gap.ToString();
                Inspect_uLabel_Assy05.Text = Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus,"Count_Trigger");
                Inspect_uLabel_Assy05.Refresh();

                Inspect_uLabel_Assy06.Text = NowFailNumber_Gap.ToString("0") + " / " + NowTrigNumber_Gap;
                Inspect_uLabel_Assy06.Refresh();

                Inspect_uLabel_Assy07.Text = GapNo.ToString();
                Inspect_uLabel_Assy07.Refresh();
            }
        }
*/

        /*
        public void myEvent_Inspect_Gap_02(int GapNo, uint ProductNo)
        {
            if (InvokeRequired)
            {
                Delegate_myEvent_Inspect_Gap_02 del = myEvent_Inspect_Gap_02;
                Invoke(del, GapNo, ProductNo);
            }
            else
            {
                Inspect_Run_Run_Make_Fail_Check();

                uTxt_Gap_No.Text = GapNo.ToString();
                uTxt_Gap_No.Refresh();

                NowProdectNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Product"));

                Inspect_uLabel_Assy05.Text = NowProdectNumber.ToString();
                Inspect_uLabel_Assy05.Refresh();

                Inspect_uLabel_Assy06.Text = NowFailNumber_Uper.ToString("0") + " / " + NowTrigNumber;
                Inspect_uLabel_Assy06.Refresh();

                Inspect_uLabel_Assy07.Text = GapNo.ToString();
                Inspect_uLabel_Assy07.Refresh();
            }
        }
        */

        /*
        private void Inspection_BiCell_Object_Setting()
        {
            //초기화 작업에서는 이미지와 호출 객체를 설정하지 않고
            //사용시에 설정하도록 한다.
            //inspection_BiCell.GetSet_NowIplImage = Inspect_Main01_IplBox.ImageIpl;
            //inspection_BiCell.GetSet_Calling_Form = this.Name;

            gc_BiCell = Inspect_Main02_IplBox.CreateGraphics();
            inspection_BiCell.GetSet_GapSystem = GapSystem;
            inspection_BiCell.GetSet_Converter = _posConverter_Gap;
            inspection_BiCell.GetSet_FileSystem = fileSystem;
            inspection_BiCell.GetSet_Graphics = gc_BiCell;
            inspection_BiCell.GetSet_ImageBoxIpl = Inspect_Main02_IplBox;
            inspection_BiCell.GetSet_PLCSystem = plc;
            inspection_BiCell.GetSet_UMACSystem = umac;
        }
        */
       
        public void myEvent_Inspect_BiCell_01(uint ProductNo, uint NgProductNo)
        {
            if (InvokeRequired)
            {
                Delegate_myEvent_Inspect_BiCell_01 del = myEvent_Inspect_BiCell_01;
                Invoke(del, ProductNo, NgProductNo);
            }
            else
            {
                NowTrigNumber =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Trigger"));

                NowProdectNumber =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Product"));

                NowFailNumber =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Both"));

                NowFailNumber_Uper =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Uper"));

                NowFailNumber_Down =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Down"));
            }
        }


        private void FormDlgInsp_Inspection_Write_LogFile(int msgNo)
        {
            //Control_Files fileSystem = new Control_Files();

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


        /*
        //////////////////////////////////////////////////////////
//진행 모델 정보///
//////////////////////////////////////////////////////////
P381 > 148  = A 타입 X축 길이

P382 > 122 = A 타입 Y축 길이

P383 > 146 = C 타입 X축 길이

P384 > 120 = C 타입 Y축 길이

P385 > 11 = 모델 셀 수량

P386 > 00110011001 = 모델 셀 배열 : 0 > A 타입, 1 > C 타입.역순으로 배열됨
        */

        public void Inspect_Get_ModelData_Read_CellType_Array(List<string> Model_Array)
        {
            int uFlagHigh = 0;
            NowModel_CellArray.Clear();
            for (int i = 0; i < NowModel_Cells; i++)
            {
                if (Model_Array[i] == "1")
                    NowModel_CellArray.Add("A");
                else
                    NowModel_CellArray.Add("C");
            }
        }


        public void Inspect_Get_ModelData_Read_CellArray(int arrayData)
        {
            int uFlagHigh = 0;
            NowModel_CellArray.Clear();
            for (int nIndex = 0; nIndex < NowModel_Cells; nIndex++)
            {
                uFlagHigh = arrayData & 0x00000001;

                arrayData = arrayData >> 1;

                if (uFlagHigh == 1)
                    NowModel_CellArray.Add("C");
                else
                    NowModel_CellArray.Add("A");
            }
        }

        public void Inspect_MIL_Initialize(MIL_ID app, MIL_ID sys, MIL_ID dis_Gap, MIL_ID dig_Gap, MIL_ID img_Gap,MIL_ID dis_BiCell, MIL_ID dig_BiCell, MIL_ID img_BiCell)
        {
            MilApplication = app;
            MilSystem_Uper = sys;
            MilDisplay_Uper = dis_Gap;
            MilDigitizer_Uper = dig_Gap;
            MilImage_Uper = img_Gap;

            MilDisplay_Down = dis_BiCell;
            MilDigitizer_Down = dig_BiCell;
            MilImage_Down = img_BiCell;
        }


        private void Inspect_Ready_Run()
        {
            //Inspection_BiCell_Object_Setting();

            //string imageFolder_BiCell = @"D:\image\2014-03-22\Cell";
            //Inspect_Run_Ready_Test_ImageFolder_Load_BiCell(imageFolder_BiCell);
            string imageFolder_Gap = @"C:\N21S";
            //string imageFolder_Gap = @"E:\N21S";
            //string imageFolder_Gap = @"D:\Test Images\N21AU";
            //string imageFolder_Gap = @"C:\TestImage\CUper";
            //string imageFolder_Gap = @"D:\AutoManual";
            //string imageFolder_Gap = @"D:\monodown";
            //string imageFolder_Gap = @"D:\monostng";
            //string imageFolder_Gap = @"D:\TestImage\Debug";
            //string imageFolder_Gap = @"D:\monodownng";
            //string imageFolder_Gap = @"D:\monongtest";
            //string imageFolder_Gap = @"D:\LamiOne\Image_Fail";
            //string imageFolder_Gap = @"D:\LamiOne\Image_Half";
            //string imageFolder_Gap = @"D:\LamiOne\Image_Uper";
            //string imageFolder_Gap = @"D:\LamiPoint\Image_Test";
            //string imageFolder_Gap = @"D:\LamiPoint\Image_BiCell_Uper";
            //string imageFolder_Gap = @"D:\LamiPoint\Image_HalfCell_Uper";
            Inspect_Run_Ready_Test_ImageFolder_Load_Uper(imageFolder_Gap);

            string imageFolder_Down = @"C:\N21S";
            //string imageFolder_Down = @"E:\N21SL";
            //string imageFolder_Down = @"D:\Test Images\N21AU";
            Inspect_Run_Ready_Test_ImageFolder_Load_Down(imageFolder_Down);

            gc_Uper = Inspect_Main01_IplBox.CreateGraphics();
            gc_Down = Inspect_Main02_IplBox.CreateGraphics();
            gc_List = Inspect_ImageList_IplBox.CreateGraphics();
        }

        /*
        private void Inspect_Ready_Run()
        {
            //Inspection_BiCell_Object_Setting();

            //string imageFolder_BiCell = @"D:\image\2014-03-22\Cell";
            //Inspect_Run_Ready_Test_ImageFolder_Load_BiCell(imageFolder_BiCell);

            string imageFolder_Gap = @"D:\TestImage\Uper";
            //string imageFolder_Gap = @"D:\AutoManual";
            //string imageFolder_Gap = @"D:\monodown";
            //string imageFolder_Gap = @"D:\monostng";
            //string imageFolder_Gap = @"D:\GapNew1";
            //string imageFolder_Gap = @"D:\monodownng";
            //string imageFolder_Gap = @"D:\monongtest";
            //string imageFolder_Gap = @"D:\LamiOne\Image_Fail";
            //string imageFolder_Gap = @"D:\LamiOne\Image_Half";
            //string imageFolder_Gap = @"D:\LamiOne\Image_Uper";
            //string imageFolder_Gap = @"D:\LamiPoint\Image_Test";
            //string imageFolder_Gap = @"D:\LamiPoint\Image_BiCell_Uper";
            //string imageFolder_Gap = @"D:\LamiPoint\Image_HalfCell_Uper";
            Inspect_Run_Ready_Test_ImageFolder_Load_Uper(imageFolder_Gap);

            Inspect_Run_Ready_Test_ImageFolder_Load_Uper(imageFolder_Gap);

            gc_Uper = Inspect_Main01_IplBox.CreateGraphics();
            gc_Down = Inspect_Main02_IplBox.CreateGraphics();
        }
        */
        //static Bitmap btmap = Resources.BiCell;
        static Bitmap btmap_Gap = Resources.GapDefault;
        

        public void Inspect_Ready_Ready()
        {
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check_Uper();
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check_Down();
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check_Uper();
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check_Down();
        }


        private void Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check_Uper()
        {
            var tempFloats = new[] {0f, 0f};

            _posConverter_Uper.BoxVsImage(Inspect_Main01_IplBox, Resources.empty, ref tempFloats);
            LamiSystem.GetSet_System_Inspect_Zoom_X_Uper = tempFloats[0];
            LamiSystem.GetSet_System_Inspect_Zoom_Y_Uper = tempFloats[1];

            LamiSystem.RectListInspBoxZone_Uper.Clear();
            for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
            {
                var tempRect = new Rectangle();
                _posConverter_Uper.ImageToBox(LamiSystem.RectListImageZone_Uper[i], ref tempRect, LamiSystem.GetSet_System_Inspect_Zoom_X_Uper, LamiSystem.GetSet_System_Inspect_Zoom_Y_Uper);
                LamiSystem.RectListInspBoxZone_Uper.Add(tempRect);

                ////Trace.WriteLine(
                //    _alignSystem.RectListRecipeBoxZone[i].Width.ToString("000") + "  " + _alignSystem.RectListRecipeBoxZone[i].Height.ToString("000") + " : " +
                //    _alignSystem.RectListImageZone[i].Width.ToString("000") + "  " + _alignSystem.RectListImageZone[i].Height.ToString("000") + " : " + 
                //    _alignSystem.RectListInspBoxZone[i].Width.ToString("000") + "  " + _alignSystem.RectListInspBoxZone[i].Height.ToString("000"));
            }
        }

        private void Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check_Down()
        {
            var tempFloats = new[] { 0f, 0f };

            _posConverter_Uper.BoxVsImage(Inspect_Main02_IplBox, Resources.empty, ref tempFloats);
            LamiSystem.GetSet_System_Inspect_Zoom_X_Down = tempFloats[0];
            LamiSystem.GetSet_System_Inspect_Zoom_Y_Down = tempFloats[1];

            LamiSystem.RectListInspBoxZone_Down.Clear();
            for (int i = 0; i < LamiSystem.RectListImageZone_Down.Count; i++)
            {
                var tempRect = new Rectangle();
                _posConverter_Uper.ImageToBox(LamiSystem.RectListImageZone_Down[i], ref tempRect, LamiSystem.GetSet_System_Inspect_Zoom_X_Down, LamiSystem.GetSet_System_Inspect_Zoom_Y_Down);
                LamiSystem.RectListInspBoxZone_Down.Add(tempRect);

                ////Trace.WriteLine(
                //    _alignSystem.RectListRecipeBoxZone[i].Width.ToString("000") + "  " + _alignSystem.RectListRecipeBoxZone[i].Height.ToString("000") + " : " +
                //    _alignSystem.RectListImageZone[i].Width.ToString("000") + "  " + _alignSystem.RectListImageZone[i].Height.ToString("000") + " : " + 
                //    _alignSystem.RectListInspBoxZone[i].Width.ToString("000") + "  " + _alignSystem.RectListInspBoxZone[i].Height.ToString("000"));
            }
        }

        private void Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check_Uper()
        {
            ROI_Zone_Image_Uper = new List<IplImage>();
            ROI_Zone_Rect_Uper = new List<CvRect>();

            LamiSystem.RectListImageZone_Uper.Clear();

            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Uper.Count; i++)
            {
                var tempRect = new CvRect(0, 0, 0, 0);
                _posConverter_Uper.BoxToImage(LamiSystem.RectListRecipeBoxZone_Uper[i], ref tempRect, LamiSystem.GetSet_System_Status_Zoom_X_Uper, LamiSystem.GetSet_System_Status_Zoom_Y_Uper);
                LamiSystem.RectListImageZone_Uper.Add(tempRect);

                ROI_Zone_Image_Uper.Add(new IplImage(LamiSystem.RectListImageZone_Uper[i].Width, LamiSystem.RectListImageZone_Uper[i].Height, BitDepth.U8, 3));
                ROI_Zone_Rect_Uper.Add(new CvRect(LamiSystem.RectListImageZone_Uper[i].Location, LamiSystem.RectListImageZone_Uper[i].Size));
            }
        }

        private void Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check_Down()
        {
            ROI_Zone_Image_Down = new List<IplImage>();
            ROI_Zone_Rect_Down = new List<CvRect>();

            LamiSystem.RectListImageZone_Down.Clear();

            for (int i = 0; i < LamiSystem.RectListRecipeBoxZone_Down.Count; i++)
            {
                var tempRect = new CvRect(0, 0, 0, 0);
                _posConverter_Uper.BoxToImage(LamiSystem.RectListRecipeBoxZone_Down[i], ref tempRect, LamiSystem.GetSet_System_Status_Zoom_X_Down, LamiSystem.GetSet_System_Status_Zoom_Y_Down);
                LamiSystem.RectListImageZone_Down.Add(tempRect);

                ROI_Zone_Image_Down.Add(new IplImage(LamiSystem.RectListImageZone_Down[i].Width, LamiSystem.RectListImageZone_Down[i].Height, BitDepth.U8, 3));
                ROI_Zone_Rect_Down.Add(new CvRect(LamiSystem.RectListImageZone_Down[i].Location, LamiSystem.RectListImageZone_Down[i].Size));
            }
        }


        private string Inspect_Login_Time_Check(DateTime loginTime)
        {
            return loginTime.ToString("yyyy-MM-dd") + "  " + loginTime.ToString("HH:mm:ss");
        }

        private string Inspect_User_Account_Check(string userAccount)
        {
            if (LamiSystem.GetSet_Now_User_Account == CInspection_Folding_Align.Account_Operator)
                return "OPERATOR";
            if (LamiSystem.GetSet_Now_User_Account == CInspection_Folding_Align.Account_Engineer)
                return "ENGINEER";
            return "MAKER";
        }

        //실패 : -1, 성공 : 0 이상
        public int String_To_Integer(string srcData)
        {
            bool Con_Result = false;
            int Con_Value = -1;
            Con_Result = int.TryParse(srcData, out Con_Value);
            return Con_Value;
        }

        private double DiskUsed = 0.0d;
        private string Inspect_DiskSpace_Check(string diskName)
        {
            
            DiskUsed = 0.0d;
            var ioControl = new CWKBIOControl();
            string DisplayData = ioControl.GetDiskSpace(diskName);
            DiskUsed = double.Parse(ioControl.GetSet_UsedSize) / double.Parse(ioControl.GetSet_DiskSize) * 100;
            return DisplayData;

            //return ioControl.GetDiskSpace(diskName);
        }


        private bool ImageDelete_Flag = false;
        private int DiskPer = 0;

        private bool DiskUsed_SizeChecking()
        {
#if(SYST_SIMUL)
            return ImageDelete_Flag;
#endif
            string SystemTitle = LamiSystem.StrListSysConTitle[10].ToString();
            string SystemNames = LamiSystem.StrListSysConName[10].ToString();

            DiskPer = String_To_Integer(LamiSystem.StrListSysConData[41]);
            if (DiskPer < 0) DiskPer = 90;

            ImageDelete_Flag = false;

            
            Inspect_DiskSpace_Check(LamiSystem.StrListSysConData[2].Substring(0, 1));
            if (DiskUsed > DiskPer) ImageDelete_Flag = true;

            return ImageDelete_Flag;
        }

        


        private void Inspect_uButton_TestRunStop02_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor; //  커서를 모래시계로 만들었다가..

            if (Inspect_uButton_TestRunStop02.Text == "시 작")
            {
                //검사 시작전에 디스크의 데이터 사용량을 확인한다.
                if (DiskUsed_SizeChecking() == true)
                {
                    dlgInspMessageBox = new Control_UltraMessageBox();
                    dlgInspMessageBox.MessageBox_Show("Inspection", "데이터 용량 초과", "현재 저장되어 있는 데이터의 크기가 디스크 용량의 " + DiskPer.ToString() + "%를 초과 했습니다.<br><br/>데이터를 삭제 한 후 다시 시작하여 주십시요!",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                //엑셀 데이터 기록에 사용되는 시간임.
                _dtImageSaveTimeBefore = DateTime.Now;

                //인스펙션 로그 파일 기록 함수 호출 (1:검출 프로그램 기동 시작)
                FormDlgInsp_Inspection_Write_LogFile(1);

                NowGapNumber = int.Parse(uTxt_Gap_No.Text.Trim());
                ThreadFirstFlag = true;
                //Loading_Image_Name.Visible = false;
                uTxt_Gap_No.ReadOnly = true;

                TrigWatch_Uper.Reset();
                TrigWatch_Down.Reset();

                //plc.PCL_WriteData_D3101(1);
                if (LamiSystem.SystemStatusFlag[2] == true)
                    umac.Umac_SetData_M6923("1");

                //2015-01-07 시작 수정함.
                //Measurement_Register_To_Grid_Uper();
                //Measurement_Register_To_Grid_Down();

                Measure_Grid_Making_Uper();
                Measure_Grid_Making_Down();
                //2015-01-07 종료 수정함.

                //PLC와 모델명을 싱크하기 위해서 이곳에서 메인 함수를 이벤트 호출한다.
                //호출한 이벤트에서 모델명을 비교후 아래의 함수
                //Inspect_Thread_Starting 함수를 호출해서 검사를 진행한다.
                Inspect_Thread_Starting();
                
            }
            else
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                
                //인스펙션 로그 파일 기록 함수 호출 (2:검출 프로그램 기동 정지)
                FormDlgInsp_Inspection_Write_LogFile(2);
                Inspect_Stop_View();
                uTxt_Gap_No.ReadOnly = false;
                _iPaint_Uper_Flag = 0;
                _iPaint_Down_Flag = 0;

                //이미지 저장까지의 프로세스를 모두 마친 후에 스레드를 정지한다.
                Inspect_Stop_threading = new Thread(Inspect_Stop_Ready);
                Inspect_Stop_threading.Start();
                //plc.PCL_WriteData_D3101(2);

                if (LamiSystem.SystemStatusFlag[2] == true)
                    umac.Umac_SetData_M6923("2");
                //Thread.Sleep(600);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        public void Inspect_Thread_Starting()
        {

#if(SYST_SIMUL)
            Thread[] milFileTriging_Gap = {new Thread(Inspect_Run_Run_Threading_File),};
            foreach (Thread milTriger_Uper in milFileTriging_Gap)
            {
                milTriger_Uper.Start();
            }
            return;

            //InspectionThread = new Thread(Inspect_Run_Run_Threading_File);
            //InspectionThread.Start();
#else

#endif

#if(UMAC_SIMUL)

#else
            if (Inspect_Get_ModelData_From_UMAC() == false)
            {
                if (Inspect_Get_ModelData_From_UMAC() == false)
                {
                    MessageBox.Show("유멕과 통신에서 오류가 발생했습니다. 확인하여 주십시요!.");
                    return;
                }
            }
#endif
            MIL_Trigger_Close = false;
            VisionJobWorking = true;
            MilApplication = MIL.M_NULL;
            MilSystem_Uper = MIL.M_NULL;
            MilDisplay_Uper = MIL.M_NULL;
            MilDisplay_Down = MIL.M_NULL;
            MilDigitizer_Uper = MIL.M_NULL;
            MilDigitizer_Down = MIL.M_NULL;
            MilImage_Uper = MIL.M_NULL;
            MilImage_Down = MIL.M_NULL;

            MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication); // Application 할당

            MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 0, MIL.M_DEFAULT, ref MilSystem_Uper); // 프레임그레버 할당
            MIL.MdigAlloc(MilSystem_Uper, MIL.M_DEV0, @"C:\Visionsystem\Data\solfcl_mil9_CSC12M25BMP19_4tap_8bit_t0.dcf",
                MIL.M_DEFAULT, ref MilDigitizer_Uper);
            MIL.MdispAlloc(MilSystem_Uper, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Uper);
            MIL.MbufAlloc2d(MilSystem_Uper, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP,
                ref MilImage_Uper);
            MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기

            MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 1, MIL.M_DEFAULT, ref MilSystem_Down); // 프레임그레버 할당
            MIL.MdigAlloc(MilSystem_Down, MIL.M_DEV0, @"C:\Visionsystem\Data\solfcl_mil9_CSC12M25BMP19_4tap_8bit_t1.dcf",
                MIL.M_DEFAULT, ref MilDigitizer_Down);
            MIL.MdispAlloc(MilSystem_Down, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Down);
            MIL.MbufAlloc2d(MilSystem_Down, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP,
                ref MilImage_Down);
            MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기



            Inspect_Run_Componet_set();
            Inspect_Run_Ready();

            _CycleCompleteFlag_Uper = true;
            _CycleCompleteFlag_Down = true;

            Inspect_Run_Run();
        }

        private string tmpFilename = string.Empty;
        public void Inspect_Run_Run_ImageLoading_To_Box_Gap(int imgCount)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                //얼라인 테스트 결과 약 50msec정도 소요된다.
                //이는 픽셀버퍼 2352 * 1728 를 리딩할 때
                //소요시간인 50msec와 비슷한 시간이 소요된다.

                //모노셀 테스트 결과 약 150msec정도 소요된다.
                //모노셀 레졸루션 : 4096 * 3072 
                Stopwatch mwatch = new Stopwatch();
                mwatch.Start();
                tmpFilename = _strLstTestImageNames_Uper[imgCount];
                _nowIplImage_Uper = new IplImage(_strLstTestImageNames_Uper[imgCount]);
                _nowIplImage_Down = new IplImage(_strLstTestImageNames_Down[imgCount]);
                mwatch.Stop();
                string timeData = mwatch.ElapsedMilliseconds.ToString("000");
                Trace.WriteLine("이미지 로딩 시간 : " + timeData);
                //Inspect_Run_Run_ImageLoading_To_Box_Display_Gap(imgCount);
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        /*
        public void Inspect_Run_Run_ImageLoading_To_Box_Gap(int imgCount)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                //얼라인 테스트 결과 약 50msec정도 소요된다.
                //이는 픽셀버퍼 2352 * 1728 를 리딩할 때
                //소요시간인 50msec와 비슷한 시간이 소요된다.

                //모노셀 테스트 결과 약 150msec정도 소요된다.
                //모노셀 레졸루션 : 4096 * 3072 
                Stopwatch mwatch = new Stopwatch();
                mwatch.Start();
                tmpFilename = _strLstTestImageNames_Uper[imgCount];
                _nowIplImage_Uper = new IplImage(_strLstTestImageNames_Uper[imgCount]);
                _nowIplImage_Down = new IplImage(_strLstTestImageNames_Uper[imgCount]);
                mwatch.Stop();
                string timeData = mwatch.ElapsedMilliseconds.ToString("000");
                Trace.WriteLine("이미지 로딩 시간 : " + timeData);
                //Inspect_Run_Run_ImageLoading_To_Box_Display_Gap(imgCount);
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */
        private void Inspect_Run_Run_Threading_File()
        {
            //NowFailNumber = 0;
            myProcess = Process.GetCurrentProcess();
            myProcess.PriorityClass = ProcessPriorityClass.RealTime;
            myProcess.Threads[0].PriorityLevel = ThreadPriorityLevel.TimeCritical;

            //syst_
            _iImgCount = 0;
            //for (int j = 0; j < 10; j++)
            for (int j = 0; j < 1; j++)
            {
                //for (int i = 0; i < _strLstTestImageNames_Uper.Count; i++)
                    for (int i = 0; i < 10; i++)
                {
                    _iImgCount = i;

                    //Inspect_Run_Run_TackTime_TextBox_Reflash();

                    Inspect_Run_Run_ImageLoading_To_Box_Gap(i);

                    if (Inspect_Run_Run_TrigTime_Check_Uper() == false)
                    {
                        //MessageBox.Show(MethodBase.GetCurrentMethod().Name + " Complete Falg Uper");// + e.Message);
                        //_stopwatch.Stop();
                        //_stopwatch.Reset();
                        Trace.WriteLine("이중 트리거 입력" + " : " + _stopwatch.ElapsedMilliseconds.ToString());
                        continue;
                        //return;

                    }
                    //Grab_Thread_Uper = new Thread(Inspect_Run_Run_Threading_File_Load_Uper);
                    //Grab_Thread_Uper.Start();
                    Inspect_Run_Run_Threading_File_Load_Uper();
                    Thread.Sleep(10);
                }
            }
        }

        /*
        private void Inspect_Run_Run_Threading_File()
        {
            //NowFailNumber = 0;
            myProcess = Process.GetCurrentProcess();
            myProcess.PriorityClass = ProcessPriorityClass.RealTime;
            myProcess.Threads[0].PriorityLevel = ThreadPriorityLevel.TimeCritical;
           
            Tack_Time_Watch_Gap.Reset();
            Tack_Time_Watch_Gap.Start();

            _iImgCount = 0;
            for (int j = 0; j < 30; j++)
            {
                //for (int i = 0; i < _strLstTestImageNames_Gap.Count; i++)
                for (int i = 0; i < 1; i++)
                {
                    _iImgCount = i;

                    Tack_Time_Watch_Gap.Stop();
                    //Inspect_Run_Run_TackTime_TextBox_Reflash();
                    if(i ==0&& j==0)
                    Inspect_Run_Run_ImageLoading_To_Box_Gap(i);

                    
                    Tack_Time_Watch_Gap.Reset();
                    Tack_Time_Watch_Gap.Start();

                    Grab_Thread = new Thread(Inspect_Run_Run_Threading_File_Load_Gap);
                    Grab_Thread.Start();
                    

                    Thread.Sleep(330);
                   
                }
            }
        }
        */

        Stopwatch tsw01= new Stopwatch();
        private void Inspect_Run_Run_Threading_File_Load_Uper()
        {
            //tsw01.Stop();
            //string doTime = tsw01.ElapsedMilliseconds.ToString();
            //Trace.WriteLine("파일 루프 시간 : "+doTime);
            //tsw01.Reset();
            //tsw01.Start();
            Inspect_Run_Run_GetSet_Triger_Number();
            //string imageFilePath = "D:\\Top01.jpg";
            //_nowIplImage_Uper.SaveImage(imageFilePath);
            //Cv.Copy(IplImage.FromFile(imageFilePath), _nowIplImage_Uper);
            //로딩한 이미지의 ROI 처리를 진행한다.
            
            
          
            ROI_Search_Result_Uper = Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(_nowIplImage_Uper);
            _CycleCompleteFlag_Uper = true;
           
            Inspect_Run_Run_Drawing_Result_Uper();

            int tmpCount = LamiSystem.StrLstRcpConGridData_Down.Count;
            if (tmpCount != 0)
            {
                if (downFileCount == 8)
                    downFileCount = downFileCount;
                //string imageFilePath_Down = "D:\\Bot01.jpg";
                //_nowIplImage_Down.SaveImage(imageFilePath_Down);
                //Cv.Copy(IplImage.FromFile(imageFilePath_Down), _nowIplImage_Down);
                ROI_Search_Result_Down = Inspect_Run_Run_ROI_EdgeLine_Centering_Down(_nowIplImage_Down);
                _CycleCompleteFlag_Down = true;

                Inspect_Run_Run_Drawing_Result_Down();
                downFileCount++;
            }

            UMAC_Data_Communication();

            Disk_Used_Spece_Check();

            Thread.Sleep(3);

            return;
        }

        private int downFileCount = 0;
        private void Disk_Used_Spece_Check()
        {

#if(SYST_SIMUL)
            return;
#endif

            Stopwatch dsc = new Stopwatch();
            dsc.Reset();
            dsc.Start();
            Inspect_DiskSpace_Check(LamiSystem.StrListSysConData[2].Substring(0, 1));
            dsc.Stop();
            string tmpElapse = dsc.ElapsedMilliseconds.ToString();
            
            if (DiskUsed > 98)
            {
                dlgInspMessageBox = new Control_UltraMessageBox();
                dlgInspMessageBox.MessageBox_Show("Inspection", "데이터 용량 초과", "현재 저장되어 있는 데이터 크기가 디스크 용량의 98%를 초과하여<br/><br/>자동검사를 정지합니다.<br/><br/>데이터를 정리한 후 재 시작하여 주십시요! ", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Inspect_Disk_Used_Stop_Eventing();
            }
        }

        private delegate void delegate_Disk_Used_Stop_Eventing();
        private void Inspect_Disk_Used_Stop_Eventing()
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_Disk_Used_Stop_Eventing del = Inspect_Disk_Used_Stop_Eventing;
                    Invoke(del);
                }
                else
                {
                    Inspect_uButton_TestRunStop02.PerformClick();
                }
            }
            catch (Exception)
            {
                Inspect_uButton_TestRunStop02.PerformClick();
            }
        }


        private void Inspect_Run_Run()
        {
            try
            {
                ROI_Search_Result_Uper = false;
                ROI_Search_Result_Down = false;

//                 Thread[] milGrabTriging_Uper = {new Thread(Inspect_Run_Run_Threading_Grab_Uper_Loop)};
//                 foreach (Thread milTriger_Uper in milGrabTriging_Uper)
//                 {
//                     milTriger_Uper.Start();
//                 }
// 
//                 Thread[] milGrabTriging_Down = {new Thread(Inspect_Run_Run_Threading_Grab_Down_Loop)};
//                 foreach (Thread milTriger_Down in milGrabTriging_Down)
//                 {
//                     milTriger_Down.Start();
//                 }

                
                 MIL_Grab_Threading_Uper = new Thread(Inspect_Run_Run_Threading_Grab_Uper_Loop);
                 MIL_Grab_Threading_Uper.Priority = ThreadPriority.Highest;
                 MIL_Grab_Threading_Uper.Start();
 
                 MIL_Grab_Threading_Down = new Thread(Inspect_Run_Run_Threading_Grab_Down_Loop);
                 MIL_Grab_Threading_Down.Priority = ThreadPriority.Highest;
                 MIL_Grab_Threading_Down.Start();

                VisionJobWorking = true;
            }
            catch (ThreadAbortException)
            {
            }
            catch (ThreadInterruptedException)
            {
            }
            catch (ThreadStartException)
            {
            }
            catch (ThreadStateException)
            {
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;  // 커서를 원래 모양으로 만들었습니다.
        }

        private bool Uper_Grab_Doing_Flag = false;
        private int testCount = 0;
        private void Inspect_Run_Run_Threading_Grab_Uper_Loop()
        {
            //Trace.WriteLine((_traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
           
            Process myProcess = Process.GetCurrentProcess();
            myProcess.PriorityClass = ProcessPriorityClass.RealTime;
            myProcess.Threads[0].PriorityLevel = ThreadPriorityLevel.TimeCritical;

            while (VisionJobWorking)
            {
                
                MIL.MdigGrab(MilDigitizer_Uper, MilImage_Uper);
                

                if (Inspect_Run_Run_TrigTime_Check_Uper() == false)
                {
                    //MessageBox.Show(MethodBase.GetCurrentMethod().Name + " Complete Falg Uper");// + e.Message);
                    //_stopwatch.Stop();
                    //_stopwatch.Reset();
                    Trace.WriteLine("이중 트리거 입력" + " : " + _stopwatch.ElapsedMilliseconds.ToString());
                    continue;
                    //return;

                }

                //타임킬링 진행하고 있어 함수 임시로 접음.
                //새로운 제품이 감지되었음을 기록하는 함수와 시간변수설정
                Inspect_Run_Run_GetSet_Triger_Number();
                //_SeqStopwatch.Reset();
                //_SeqStopwatch.Start();
                //_dtInspDataSaveTime = DateTime.Now;
                
                Array.Clear(imgBuf_Uper, 0, imgBuf_Uper.Length);
                _CycleCompleteFlag_Uper = false;
                MIL.MbufGet2d(MilImage_Uper, 0, 0, 4096, 3072, imgBuf_Uper);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Uper, 0);
                CVMatImg_Uper.Data = bufPtr;
                Cv.Merge(CVMatImg_Uper, CVMatImg_Uper, CVMatImg_Uper, null, _nowIplImage_Uper);


                Grab_Thread_Uper = new Thread(Inspect_Run_Run_Grab_Running_Uper);
                Grab_Thread_Uper.Start();

                
                Thread.Sleep(3);
            }
        }

        Stopwatch TrigWatch_Uper = new Stopwatch();
        public bool Inspect_Run_Run_TrigTime_Check_Uper()
        {
            TrigWatch_Uper.Stop();
            //1 int tmpElapsed = int.Parse(TrigWatch_Uper.ElapsedMilliseconds.ToString());

            //1 
            string strElapsed = TrigWatch_Uper.ElapsedMilliseconds.ToString();
            //Inspect_Run_Run_Data_Monitor("Uper : " + strElapsed);
            Trace.WriteLine((NowTrigNumber).ToString("00000") + "Uper : " + strElapsed);
            int tmpElapsed = int.Parse(strElapsed);
            //1

            if (tmpElapsed == 0)
            {
                TrigWatch_Uper.Reset();
                TrigWatch_Uper.Start();
                return true;
            }

            if (tmpElapsed < _iTriggerDeleay_Uper)
            {
                TrigWatch_Uper.Start();
                Trace.WriteLine((NowTrigNumber).ToString("00000") + " 간섭 트리거 입력 상부 : Method Name : " + MethodBase.GetCurrentMethod().Name);
               
                return false;
            }

            TrigWatch_Uper.Reset();
            TrigWatch_Uper.Start();

            Trace.WriteLine((NowTrigNumber).ToString("00000") + " 정상 트리거 입력 상부 : Method Name : " + MethodBase.GetCurrentMethod().Name);
            
            return true;
        }

        /*
         private void Inspect_Run_Run_Threading_Grab_Uper_Loop()
        {
            //Trace.WriteLine((_traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
           
            Process myProcess = Process.GetCurrentProcess();
            myProcess.PriorityClass = ProcessPriorityClass.RealTime;
            myProcess.Threads[0].PriorityLevel = ThreadPriorityLevel.TimeCritical;

            while (VisionJobWorking)
            {
                
                //_stopwatch.Start();
                MIL.MdigGrab(MilDigitizer_Uper, MilImage_Uper);
                //_stopwatch.Stop();
                //_stopwatch.Reset();
                

                if (_CycleCompleteFlag_Uper == false)
                {
                    //MessageBox.Show(MethodBase.GetCurrentMethod().Name + " Complete Falg Uper");// + e.Message);
                    //_stopwatch.Stop();
                    //_stopwatch.Reset();
                    Trace.WriteLine("이중 트리거 입력" + " : " + _stopwatch.ElapsedMilliseconds.ToString());
                    continue;
                    //return;
                }

                //타임킬링 진행하고 있어 함수 임시로 접음.
                //새로운 제품이 감지되었음을 기록하는 함수와 시간변수설정
                Inspect_Run_Run_GetSet_Triger_Number();
                //_SeqStopwatch.Reset();
                //_SeqStopwatch.Start();
                //_dtInspDataSaveTime = DateTime.Now;
                
                Array.Clear(imgBuf_Uper, 0, imgBuf_Uper.Length);
                _CycleCompleteFlag_Uper = false;
                MIL.MbufGet2d(MilImage_Uper, 0, 0, 4096, 3072, imgBuf_Uper);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Uper, 0);
                CVMatImg_Uper.Data = bufPtr;
                Cv.Merge(CVMatImg_Uper, CVMatImg_Uper, CVMatImg_Uper, null, _nowIplImage_Uper);


                Grab_Thread_Uper = new Thread(Inspect_Run_Run_Grab_Running_Uper);
                Grab_Thread_Uper.Start();

                
                Thread.Sleep(3);
            }
        }
         */

        private Thread Grab_Thread_Uper;

        private void Inspect_Run_Run_Threading_Grab_Down_Loop()
        {
            //Trace.WriteLine((_traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Process myProcess = Process.GetCurrentProcess();
            myProcess.PriorityClass = ProcessPriorityClass.RealTime;
            myProcess.Threads[0].PriorityLevel = ThreadPriorityLevel.TimeCritical;

            while (VisionJobWorking)
            {
                MIL.MdigGrab(MilDigitizer_Down, MilImage_Down);

//                 if (_CycleCompleteFlag_Down == false)
//                 {
//                     Trace.WriteLine("이중 트리거 입력" + " : " + _stopwatch.ElapsedMilliseconds.ToString());
//                     continue;
//                 }

                if (Inspect_Run_Run_TrigTime_Check_Down() == false)
                {
                    Trace.WriteLine("이중 트리거 입력" + " : " + _stopwatch.ElapsedMilliseconds.ToString());
                    continue;
                }

                Array.Clear(imgBuf_Down, 0, imgBuf_Down.Length);
                _CycleCompleteFlag_Down = false;
                MIL.MbufGet2d(MilImage_Down, 0, 0, 4096, 3072, imgBuf_Down);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Down, 0);
                CVMatImg_Down.Data = bufPtr;
                Cv.Merge(CVMatImg_Down, CVMatImg_Down, CVMatImg_Down, null, _nowIplImage_Down);
                
                Grab_Thread_Down = new Thread(Inspect_Run_Run_Grab_Running_Down);
                Grab_Thread_Down.Start();
               
                //_CycleCompleteFlag_Down = true;
                Thread.Sleep(3);
            }
        }

        Stopwatch TrigWatch_Down = new Stopwatch();
        public bool Inspect_Run_Run_TrigTime_Check_Down()
        {
            TrigWatch_Down.Stop();
            //int tmpElapsed = int.Parse(TrigWatch_Down.ElapsedMilliseconds.ToString());

            //1 
            string strElapsed = TrigWatch_Down.ElapsedMilliseconds.ToString();
            //Inspect_Run_Run_Data_Monitor("Down : " + strElapsed);
            Trace.WriteLine((NowTrigNumber).ToString("00000") + "Down : " + strElapsed);
            int tmpElapsed = int.Parse(strElapsed);
            //1

            if (tmpElapsed == 0)
            {
                TrigWatch_Down.Reset();
                TrigWatch_Down.Start();
                return true;
            }

            if (tmpElapsed < _iTriggerDeleay_Down)
            {
                TrigWatch_Down.Start();
                Trace.WriteLine((NowTrigNumber).ToString("00000") + " 간섭 트리거 입력 하부 : Method Name : " + MethodBase.GetCurrentMethod().Name);
                return false;
            }

            TrigWatch_Down.Reset();
            TrigWatch_Down.Start();

            Trace.WriteLine((NowTrigNumber).ToString("00000") + " 정상 트리거 입력 하부 : Method Name : " + MethodBase.GetCurrentMethod().Name);
            return true;
        }
        private Thread Grab_Thread_Down;


        /*
        private void Inspect_Run_Run_Grab_Running_Gap()
        {
            //트리거 일렬번호를 레지로 부터 읽은다음
            //1을 증가하고 다시 저장한다.


            Inspect_Run_Run_GetSet_TrigerNo_Gap();

            _dtInspDataSaveTime = DateTime.Now;

            //쓰레드가 처음 시작된 시점인지를 판단하는 플래그를
            //검사해서 처음이면 갭번호를 증가하지 않는다.
            if (ThreadFirstFlag == true)
            {
                ThreadFirstFlag = false;
            }
            else
            {
                //트리거 입력되어 갭번호를 증가한다.
                if (NowGapNumber < (NowModel_Cells - 1)) 
                    NowGapNumber++;
                //else{}
                Inspect_Run_Run_Set_Reg_NowGapNo(NowGapNumber);
            }

            //로딩한 이미지의 ROI 처리를 진행한다.
            bool Serarch_Result = Inspect_Run_Run_ROI_EdgeLine_Centering(false);
            
            if (Serarch_Result == false)
            {
                _iPLC_Result_Code = 2;
                PLC_WriteData_Threading();

                Result_Display_Thread = new Thread(Inspect_Run_Run_Inspect_None_Display);
                Result_Display_Thread.Start();
                dResultValueLeft = 0.0;
                dResultValueRight = 0.0;
                _savedValue_Left = 0.0;
                _savedValue_Right = 0.0;
                _strSavedNGOK[2] = "NG";

                Inspect_Run_Run_Data_Save();
                Tack_Time_Watch_Gap.Stop();
                Inspect_Run_Run_TackTime_TextBox_Reflash();
                _CycleCompleteFlag_Gap = true;
                Thread.Sleep(3);
                return;
            }

            Inspect_Run_Run_Drawing_Result();
            _CycleCompleteFlag_Gap = true;
            Thread.Sleep(3);
            return;
        }
        */


        private void Inspect_Run_Run_Grab_Running_Uper_Test01()
        {
        }

        

        private delegate void delegate_IplBoxImage_Image_Loading_Uper(IplImage disIplImage);
        private void Inspect_Run_Run_IplBoxImage_Loading_Uper(IplImage disIplImage)
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_IplBoxImage_Image_Loading_Uper del = Inspect_Run_Run_IplBoxImage_Loading_Uper;
                    Invoke(del, disIplImage);
                }
                else
                {
                    Inspect_Main01_IplBox.ImageIpl = disIplImage;
                    Inspect_Main01_IplBox.Refresh();
                }
            }
            catch (Exception)
            {
                Inspect_Run_Run_IplBoxImage_Loading_Uper(disIplImage);
            }
        }

        private delegate void delegate_IplBoxImage_Image_Loading_Down(IplImage disIplImage);
        private void Inspect_Run_Run_IplBoxImage_Loading_Down(IplImage disIplImage)
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_IplBoxImage_Image_Loading_Down del = Inspect_Run_Run_IplBoxImage_Loading_Down;
                    Invoke(del, disIplImage);
                }
                else
                {
                    Inspect_Main02_IplBox.ImageIpl = disIplImage;
                    Inspect_Main02_IplBox.Refresh();
                }
            }
            catch (Exception)
            {
                Inspect_Run_Run_IplBoxImage_Loading_Down(disIplImage);
            }
        }



        private delegate void delegate_Run_Run_Button_PerformClick(UltraButton performButton, int ButtonNo);
        private void Inspect_Run_Run_Button_PerformClick(UltraButton performButton, int ButtonNo)
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_Run_Run_Button_PerformClick del = Inspect_Run_Run_Button_PerformClick;
                    Invoke(del, performButton, ButtonNo);
                }
                else
                {
                    if (ButtonNo == 1)
                    {
                        Inspect_Main01_IplBox.Visible = true;
                        Inspect_Main02_IplBox.Visible = false;
                        uPanel_Uper.Visible = false;
                        uPanel_Down.Visible = false;
                        Inspect_Main01_IplBox.Refresh();
                    }
                    else
                    {
                        Inspect_Main01_IplBox.Visible = false;
                        Inspect_Main02_IplBox.Visible = true;
                        uPanel_Uper.Visible = false;
                        uPanel_Down.Visible = false;
                        Inspect_Main02_IplBox.Refresh();
                    }

                }
            }
            catch (Exception)
            {
                Inspect_Run_Run_Button_PerformClick(performButton, ButtonNo);
            }

        }

        /*
        private void Inspect_Run_Run_Grab_Running_Down()
        {
            //트리거 일렬번호를 레지로 부터 읽은다음
            //1을 증가하고 다시 저장한다.
            try
            {
                Trace.WriteLine(" : Method Name : 1 start " + MethodBase.GetCurrentMethod().Name);
                string imageFilePath = "M:\\NowImage_Down.jpg";
                _nowIplImage_Down.SaveImage(imageFilePath);
                Trace.WriteLine(" : Method Name : 1 end " + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 2 start " + MethodBase.GetCurrentMethod().Name);
                Cv.Copy(IplImage.FromFile(imageFilePath), SrcIplImage_Down);
                Trace.WriteLine(" : Method Name : 2 end " + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 3 start " + MethodBase.GetCurrentMethod().Name);
                Inspect_Run_Run_IplBoxImage_Loading_Down();
                Trace.WriteLine(" : Method Name : 3 end " + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 4 start " + MethodBase.GetCurrentMethod().Name);
                Inspect_Run_Run_GetSet_Triger_Number();
                Trace.WriteLine(" : Method Name : 4 end " + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 5 start " + MethodBase.GetCurrentMethod().Name);
                _dtInspDataSaveTime = DateTime.Now;
                Trace.WriteLine(" : Method Name : 5 end " + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 6 start " + MethodBase.GetCurrentMethod().Name);
                //로딩한 이미지의 ROI 처리를 진행한다.
                ROI_Search_Result_Down = Inspect_Run_Run_ROI_EdgeLine_Centering_Down(SrcIplImage_Down);
                Trace.WriteLine(" : Method Name : 6 end " + MethodBase.GetCurrentMethod().Name);


                if (ROI_Search_Result_Uper == false || ROI_Search_Result_Down == false)
                {
                    _strSavedNGOK[0] = "NG";
                    _strSavedNGOK[1] = "NG";
                    _strSavedNGOK[2] = "NG";
                    dResultValue_Uper = 0.0;
                    dResultValue_Down = 0.0;
                    _savedValue_Uper = 0.0;
                    _savedValue_Down = 0.0;

                    FormDlgInsp_Inspection_Save_Data_Copy();

                    _iPLC_Result_Code = 2;
                    //PLC_WriteData_Threading();
                    
                    Result_Display_Thread = new Thread(Inspect_Run_Run_None_Point_Search_Display);
                    Result_Display_Thread.Start();

                    Inspect_Run_Run_Data_Save();
                    _CycleCompleteFlag_Uper = true;
                    Thread.Sleep(3);
                    return;
                }
                Trace.WriteLine(" : Method Name : 7 end " + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 8 start " + MethodBase.GetCurrentMethod().Name);
                Inspect_Run_Run_Drawing_Result();
                Trace.WriteLine(" : Method Name : 8 end" + MethodBase.GetCurrentMethod().Name);

                Trace.WriteLine(" : Method Name : 9 start " + MethodBase.GetCurrentMethod().Name);
                _CycleCompleteFlag_Down = true;
                Trace.WriteLine(" : Method Name : 9 end " + MethodBase.GetCurrentMethod().Name);

               

                Thread.Sleep(3);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }

        }
         * 
         * private void Inspect_Run_Run_Grab_Running_Down()
        {
            //트리거 일렬번호를 레지로 부터 읽은다음 1을 증가하고 다시 저장한다.
            try
            {
                string imageFilePath = "M:\\NowImage_Down.jpg";
                _nowIplImage_Down.SaveImage(imageFilePath);
                Cv.Copy(IplImage.FromFile(imageFilePath), SrcIplImage_Down);

                Inspect_Run_Run_IplBoxImage_Loading_Down();

               

                //로딩한 이미지의 ROI 처리를 진행한다.
                ROI_Search_Result_Down = Inspect_Run_Run_ROI_EdgeLine_Centering_Down(SrcIplImage_Down);
                ROI_Search_Result_Uper = true;
                ROI_Search_Result_Down = true;

                if (ROI_Search_Result_Uper == false || ROI_Search_Result_Down == false)
                {
                    _strSavedNGOK[0] = "NG";
                    _strSavedNGOK[1] = "NG";
                    _strSavedNGOK[2] = "NG";
                    //dResultValue_Uper = 0.0;
                    //dResultValue_Down = 0.0;
                    //_savedValue_Uper = 0.0;
                    //_savedValue_Down = 0.0;

                    FormDlgInsp_Inspection_Save_Data_Copy();

                    _iPLC_Result_Code = 2;
                    //PLC_WriteData_Threading();

                    Result_Display_Thread = new Thread(Inspect_Run_Run_None_Point_Search_Display);
                    Result_Display_Thread.Start();

                    Inspect_Run_Run_Data_Save();
                    _CycleCompleteFlag_Uper = true;
                    Thread.Sleep(3);
                    return;
                }
                Inspect_Run_Run_Drawing_Result();
                _CycleCompleteFlag_Uper = true;
                _CycleCompleteFlag_Down = true;

                Thread.Sleep(3);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }
        */

        private int tmpintData = 1;
        private bool ROI_Search_Result_Uper = false;
        private bool ROI_Search_Result_Down = false;
        private void Inspect_Run_Run_Grab_Running_Uper()
        {
            try
            {
                //로딩한 이미지의 ROI 처리를 진행한다.
                ROI_Search_Result_Uper = Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(_nowIplImage_Uper);
                _CycleCompleteFlag_Uper = true;
                Inspect_Run_Run_Drawing_Result_Uper();
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }

        private void Inspect_Run_Run_Grab_Running_Down()
        {
            //트리거 일렬번호를 레지로 부터 읽은다음 1을 증가하고 다시 저장한다.
            try
            {
                //로딩한 이미지의 ROI 처리를 진행한다.
                ROI_Search_Result_Down = Inspect_Run_Run_ROI_EdgeLine_Centering_Down(_nowIplImage_Down);
                _CycleCompleteFlag_Down = true;
                Inspect_Run_Run_Drawing_Result_Down();

                UMAC_Data_Communication();
               
                Thread.Sleep(3);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }

        //private bool Uper_Measure_Result = false;
        //private bool Down_Measure_Result = false;

        private bool Uper_Measure_Result = true;
        private bool Down_Measure_Result = true;

        private void UMAC_Data_Communication()
        {
            //제품 생산 수량을 카운트하는 레지스터와 변수
            Inspect_Run_Run_GetSet_Product_Count();

            string Inspect_Result = "0";

            if (Uper_Measure_Result == false)
            {
                Inspect_Result = "1";
                //상부 엔지 수량을 카운트하는 레지스터와 변수
                Inspect_Run_Run_GetSet_Ng_Uper();
            }

            if (Down_Measure_Result == false)
            {
                Inspect_Result = "1";
                //하부 엔지 수량을 카운트하는 레지스터와 변수
                Inspect_Run_Run_GetSet_Ng_Down();
            }

            if (Uper_Measure_Result == false || Down_Measure_Result == false)
            {
                //제품 엔지 수량을 카운트하는 레지스터와 변수
                Inspect_Run_Run_GetSet_Ng_Both();
                inspect_Run_Run_NG_Alram_Count();
            }
            else
            {
                Inspect_Result = "2";
                NG_Alram_Count = 0;
            }

            //string Umac_Staus = "0";
            //Umac_Staus = umac.Umac_GetData_P5102();
            //if (Umac_Staus == "12")
            umac.Umac_SetData_P5102(Inspect_Result);
            //_SeqStopwatch.Stop();
            //Trace.WriteLine(" : UMAC 전송 시간 : " + _SeqStopwatch.ElapsedMilliseconds.ToString());
            
            inspect_Run_Run_Display_NG_OK_Count();
        }

        int NG_Alram_Count = 0;

        private void inspect_Run_Run_NG_Alram_Count()
        {
            string tmpOnOff = LamiSystem.StrListSysConData[38];
            if (tmpOnOff == "OFF") return;

            NG_Alram_Count = NG_Alram_Count + 1;

            string tmpValue = LamiSystem.StrListSysConData[39];
            int SetValue = int.Parse(tmpValue);

            if (SetValue <= NG_Alram_Count)
            {
                //UMAC에게 값을 전달한다.
                NG_Alram_Count = 0;
            }
        }


        private delegate void delegate_Run_Run_Display_NG_OK_Count();
        public void inspect_Run_Run_Display_NG_OK_Count()
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_Run_Run_Display_NG_OK_Count del = inspect_Run_Run_Display_NG_OK_Count;
                    Invoke(del);
                }
                else
                {
                    NowFailNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Uper"));
                    NowFailNumber_Down = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Down"));
                    NowFailNumber_Both = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Both"));
                    NowProdectNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Product"));
                    NowPassNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Uper"));
                    NowPassNumber_Down = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Down"));

                    float tmpValue = (float)(((float)NowProdectNumber - (float)NowFailNumber_Uper) / (float)NowProdectNumber) * 100f;
                    Inspect_uLabel_Assy05.Text = tmpValue.ToString("0.00") + " % (" + NowFailNumber_Uper.ToString() + " / " + NowProdectNumber.ToString() + ")";
                    Inspect_uLabel_Assy05.Refresh();
                    
                    tmpValue = (float)(((float)NowProdectNumber - (float)NowFailNumber_Down) / (float)NowProdectNumber) * 100f;
                    Inspect_uLabel_Assy06.Text = tmpValue.ToString("0.00") + " % (" + NowFailNumber_Down.ToString() + " / " + NowProdectNumber.ToString() + ")";
                    Inspect_uLabel_Assy06.Refresh();
                    
                    tmpValue = (float)(((float)NowProdectNumber - (float)NowFailNumber_Both) / (float)NowProdectNumber) * 100f;
                    Inspect_uLabel_Assy07.Text = tmpValue.ToString("0.00") + " % (" + NowFailNumber_Both.ToString() + " / " + NowProdectNumber.ToString() + ")";
                    Inspect_uLabel_Assy07.Refresh();
                }
            }
            catch
            {
                
            }
        }


        /*
         private delegate void delegate_Run_Run_Image_Loading();
        private void Inspect_Run_Run_Image_Loading()
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_Run_Run_Image_Loading del = Inspect_Run_Run_Image_Loading;
                    Invoke(del);
                }
                else
                {
        */

        private void Inspect_Run_Run_GetSet_Product_Count()
        {
            try
            {
                NowProdectNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Product"));
                if (NowProdectNumber < uint.MaxValue)
                {
                    NowProdectNumber++;
                    SetReg(LamiSystem.RegPathGapStatus, "Count_Product", NowProdectNumber.ToString());
                }
                else
                {
                    Inspect_Count_RegData_Reset();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }



        private void Inspect_Run_Run_GetSet_Ng_Both()
        {
            try
            {
                //private uint NowFailNumber_Down;
                //private uint NowFailNumber_Uper;
                //해당 날자의 폴더를 생성할 때마다 이미지 저장에 사용되는 트리거 번호를 초기화한다.
                NowFailNumber_Both = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Both"));
                if (NowFailNumber_Both < uint.MaxValue)
                    NowFailNumber_Both++;
                else
                    NowFailNumber_Both = 0;
                SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Both", NowFailNumber_Both.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        private void Inspect_Run_Run_GetSet_Ng_Uper()
        {
            try
            {
                //private uint NowFailNumber_Down;
                //private uint NowFailNumber_Uper;
                //해당 날자의 폴더를 생성할 때마다 이미지 저장에 사용되는 트리거 번호를 초기화한다.
                NowFailNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Uper"));
                if (NowFailNumber_Uper < uint.MaxValue)
                    NowFailNumber_Uper++;
                else
                    NowFailNumber_Uper = 0;
                SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Uper", NowFailNumber_Uper.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        private void Inspect_Run_Run_GetSet_Ng_Down()
        {
            try
            {
                //private uint NowFailNumber_Down;
                //private uint NowFailNumber_Uper;
                //해당 날자의 폴더를 생성할 때마다 이미지 저장에 사용되는 트리거 번호를 초기화한다.
                NowFailNumber_Down = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Down"));
                if (NowFailNumber_Down < uint.MaxValue)
                    NowFailNumber_Down++;
                else
                    NowFailNumber_Down = 0;
                SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Down", NowFailNumber_Down.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        /*
        private void UMAC_Data_Communication_Test()
        {
            //Inspect_Result_Down
            _strSavedInspectResult_Uper = "OK";
            _strSavedInspectResult_Down = "OK";

            string Inspect_Result = "0";
            if (_strSavedInspectResult_Uper == "NG" || _strSavedInspectResult_Down == "NG") Inspect_Result = "1";
            else Inspect_Result = "2";

            string Umac_Staus = "0";
            Umac_Staus = umac.Umac_GetData_P5101();
            //if (Umac_Staus == "11")
                umac.Umac_SetData_P5101(Inspect_Result);

            _strSavedInspectResult_Uper = "OK";
            _strSavedInspectResult_Down = "NG";

            Inspect_Result = "0";
            if (_strSavedInspectResult_Uper == "NG" || _strSavedInspectResult_Down == "NG") Inspect_Result = "1";
            else Inspect_Result = "2";

            Umac_Staus = "0";
            Umac_Staus = umac.Umac_GetData_P5101();
            //if (Umac_Staus == "11")
                umac.Umac_SetData_P5101(Inspect_Result);

            _strSavedInspectResult_Uper = "NG";
            _strSavedInspectResult_Down = "OK";

            Inspect_Result = "0";
            if (_strSavedInspectResult_Uper == "NG" || _strSavedInspectResult_Down == "NG") Inspect_Result = "1";
            else Inspect_Result = "2";

            Umac_Staus = "0";
            Umac_Staus = umac.Umac_GetData_P5101();
            if (Umac_Staus == "11")
                umac.Umac_SetData_P5101(Inspect_Result);

            _strSavedInspectResult_Uper = "NG";
            _strSavedInspectResult_Down = "NG";

            Inspect_Result = "0";
            if (_strSavedInspectResult_Uper == "NG" || _strSavedInspectResult_Down == "NG") Inspect_Result = "1";
            else Inspect_Result = "2";

            Umac_Staus = "0";
            Umac_Staus = umac.Umac_GetData_P5101();
            //if (Umac_Staus == "11")
                umac.Umac_SetData_P5101(Inspect_Result);
        }
        */
        private delegate void delegate_Run_Run_Image_Loading();
        private void Inspect_Run_Run_Image_Loading()
        {
            try
            {
                if (InvokeRequired)
                {
                    delegate_Run_Run_Image_Loading del = Inspect_Run_Run_Image_Loading;
                    Invoke(del);
                }
                else
                {
                    //파일을 저장한다.
                    //Tack_Time_Watch_Gap.Reset();
                    //Tack_Time_Watch_Gap.Start();
                    string imageFilePath = "M:\\NowImage" + NowGapNumber.ToString("00") + ".jpg";
                    _nowIplImage_Uper.SaveImage(imageFilePath);
                    //Tack_Time_Watch_Gap.Stop();
                    //Inspect_Run_Run_TackTime_TextBox_Reflash();

                    //Tack_Time_Watch_Gap.Reset();
                    //Tack_Time_Watch_Gap.Start();
                    Cv.Copy(IplImage.FromFile(imageFilePath), SrcIplImage_Uper);
                    //Tack_Time_Watch_Gap.Stop();
                    //Inspect_Run_Run_TackTime_TextBox_Reflash();
                }
            }
            catch (Exception)
            {
                Inspect_Run_Run_Image_Loading();
            }
           
        }


        private delegate void Delegate_Run_Run_StopWatch_Result_Inspection_Display();
        private void Inspect_Run_Run_TextBox_Reflash()
        {
            if (Debug01.InvokeRequired)
            {
                Delegate_Run_Run_StopWatch_Result_Inspection_Display del = Inspect_Run_Run_TextBox_Reflash;
                Debug01.Invoke(del);
            }
            else
            {
                Debug01.Visible = true;
                Debug01.Text += "크리거입력 : " + _SeqStopwatch.ElapsedMilliseconds.ToString() + "\r\n";
                Debug01.Refresh();
            }
        }


        private delegate void Delegate_Run_Run_StopWatch_Result_Inspection_Display2(string distring);
        private void Inspect_Run_Run_Data_Monitor(string distring)
        {
            if (Debug01.InvokeRequired)
            {
                Delegate_Run_Run_StopWatch_Result_Inspection_Display2 del = Inspect_Run_Run_Data_Monitor;
                Debug01.Invoke(del, distring);
            }
            else
            {
                Debug01.Visible = true;
                Debug01.Text += distring + "\r\n";
                Debug01.Refresh();
            }
        }

        /*
        private void Inspect_Run_Run_Threading_Grab_BiCell()
        {
            //Process myProcess = Process.GetCurrentProcess();
            //myProcess.PriorityClass = ProcessPriorityClass.RealTime;
            //myProcess.Threads[0].PriorityLevel = ThreadPriorityLevel.TimeCritical;


            while (VisionJobWorking)
            {
                MIL.MdigGrab(MilDigitizer_BiCell, MilImage_BiCell);

                if (_CycleCompleteFlag_BiCell == false)
                {
                    return;
                }

                _CycleCompleteFlag_BiCell = false;

                Tack_Time_Watch_BiCell.Reset();
                Tack_Time_Watch_BiCell.Start();

                MIL.MbufGet2d(MilImage_BiCell, 0, 0, 4096, 3072, imgBuf_BiCell);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_BiCell, 0);
                matImg_BiCell.Data = bufPtr;
                monoIplImage_BiCell = Cv.GetImage(matImg_BiCell);
                Cv.CvtColor(monoIplImage_BiCell, srcIplImage_BiCell, ColorConversion.GrayToBgr);
                Inspect_Run_Run_Grab_Running_BiCell();
            }
        }
        */


        private void Inspect_Run_Run_GetSet_Triger_Number()
        {
            try
            {
                //해당 날자의 폴더를 생성할 때마다 이미지 저장에 사용되는 트리거 번호를 초기화한다.
                NowTrigNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Trigger"));
                if (NowTrigNumber < uint.MaxValue)
                    NowTrigNumber++;
                else
                    NowTrigNumber = 0;
                SetReg(LamiSystem.RegPathGapStatus, "Count_Trigger", NowTrigNumber.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }


        public void Inspect_Run_Run_Grab_Manual()
        {
            _dtInspDataSaveTime = DateTime.Now;

            Inspect_Get_ModelData_From_UMAC();

            //Data_Mixing_Make_Uper();
            //Data_Mixing_Make_Down();

            Inspect_Initionalize();

            //로딩한 이미지의 ROI 처리를 진행한다.
            bool Serarch_Result = Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(SrcIplImage_Uper);

            if (Serarch_Result == false) Inspect_Run_Run_None_Point_Search_Display();

            Inspect_Run_Run_ROI_CenterPoint_Find_Uper();

            Inspect_Run_Run_FindData_Inspection_Uper();
            //Inspect_Run_Run_FindData_Inspecting_Uper();

            Inspect_Save_Data_Copy_Uper();

            //검사 결과를 화면에 표시해 준다.
            Inspect_Run_Run_Inspect_Result_Display_Uper();
        }

        /*
        private void Inspect_Manual_Image_Grabing()
        {
            _dtInspDataSaveTime = DateTime.Now;
            
            //로딩한 이미지의 ROI 처리를 진행한다.
            //로딩한 이미지의 ROI 처리를 진행한다.
            bool Serarch_Result = Inspect_Run_Run_ROI_EdgeLine_Centering(false);

            if (Serarch_Result == false) Inspect_Run_Run_Inspect_None_Display();
            Inspect_Run_Run_Drawing_Result();

            _CycleCompleteFlag_Gap = true;
        }

        */
        string[] GapTotal_Data = new string[50];
        private string GapTotal_Time;

        /*
        private void Gep_Total_Write_Time()
        {
            if(NowGapNumber ==0)
            {
                GapTotal_Time = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                //string saveTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                _dtInspDataSaveTime.Hour, _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

                
                excelFile.WriteExcelFile(GapTotal_Filename, GapTotal_Time, true,GapTotal_Result, NowModel_Cells);
                //GapTotal_Data[0] = saveTime;
            }
        }
        */

        private delegate void Delegate_Run_Run_TackTime_Result_Display();
        private void Inspect_Run_Run_Value_TextBox_Reflash()
        {
            if (Debug01.InvokeRequired)
            {
                Delegate_Run_Run_TackTime_Result_Display del = Inspect_Run_Run_Value_TextBox_Reflash;
                Debug01.Invoke(del);
            }
            else
            {
                Debug01.Visible = true;
                Debug01.Text += "측정 결과 : " + dResultValue_Uper.ToString() + " " + dResultValue_Down.ToString() + "\r\n";
                //Debug01.SelectionStart = Debug01.TextLength - 1;
                Debug01.Refresh();
            }
        }

        private delegate void Delegate_Run_Run_TackTime_Result_Inspection_Display();
        private void Inspect_Run_Run_TackTime_TextBox_Reflash()
        {
            if (Debug01.InvokeRequired)
            {
                Delegate_Run_Run_TackTime_Result_Inspection_Display del = Inspect_Run_Run_TackTime_TextBox_Reflash;
                Debug01.Invoke(del);
            }
            else
            {
                Debug01.Visible = true;
                Debug01.Text += "텍타임 결과 : " + Tack_Time_Watch_Gap.ElapsedMilliseconds.ToString() + "\r\n";
                Debug01.SelectionStart = Debug01.TextLength - 1;
                Debug01.Refresh();
            }
        }

        public void PLC_WriteData_Threading()
        {
            Thread[] PLC_Data_Writing_Gap = { new Thread(PLC_Write_Data_Send) };
            foreach (Thread PLC_Writing in PLC_Data_Writing_Gap)
            {
                PLC_Writing.Start();
            }
        }

        private int _iPLC_Result_Code = 0;


        
        private void PLC_Write_Data_Send()
        {
            if (_iPLC_Result_Code != 0)
            {
                if (_iPLC_Result_Code == 1)
                {
                    //plc.PCL_WriteData_D4045(1);
                }
                else if (_iPLC_Result_Code == 2)
                {
                    //plc.PCL_WriteData_D4045(2);
                }
            }

            int tmpIntLeft = (int) (_savedValue_Uper*100);
            string tmpStrLeft = "D" + (4051 + _iSaved_Uper_Number).ToString();
            //plc.PCL_WriteData_D4050(tmpStrLeft, tmpIntLeft);


            int tmpIntRigh = (int)(_savedValue_Down * 100);
            string tmpStrRigh = "D" + (4081 + _iSaved_Uper_Number).ToString();
            //plc.PCL_WriteData_D4080(tmpStrRigh, tmpIntRigh);
        }

        /*
         * private void PLC_Write_Data_Send()
        {
            if (_iPLC_Result_Code == 0)
                plc.PCL_WriteData_D4045(0);
            else if (_iPLC_Result_Code == 1)
                plc.PCL_WriteData_D4045(1);
            else if (_iPLC_Result_Code == 2)
                plc.PCL_WriteData_D4045(2);

            int tmpIntLeft = (int) (_savedValue_Left*100);
            string tmpStrLeft = "D" + (4051 + _iSavedGap_Number).ToString();
            plc.PCL_WriteData_D4050(tmpStrLeft, tmpIntLeft);


            int tmpIntRigh = (int)(_savedValue_Right * 100);
            string tmpStrRigh = "D" + (4081 + _iSavedGap_Number).ToString();
            plc.PCL_WriteData_D4080(tmpStrRigh, tmpIntRigh);
        }
         */
        Font _font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);
        private bool ThreadFirstFlag = true;


        public struct Result_Display_Struct
        {
            public List<string> itemResult;
            public string displayStr01;
            public string displayStr02;
            public string displayStr03;
            public string displayStr04;
            public string displayStr05;
            public string displayStr06;
            public string gridTime;
            public Font _font;
        }

        private int _iPaint_Down_Flag = 0;
        private int NowGapTypeNumber;
        private void Inspect_Run_Run_Inspect_Result_Display_Uper()
        {
            try
            {
                if (Inspect_Main01_IplBox.Visible != true) return;

                _iPaint_Uper_Flag = 1;

                Result_Display_Struct Struct_Data = new Result_Display_Struct();
                Struct_Data.itemResult = new List<string>();
                Struct_Data.itemResult = itemResult_Uper;
                GetSet_Draw_GC_Uper = Inspect_Main01_IplBox.CreateGraphics();

//                 //Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);
//                 int mesColCount = 19;
//                 for (int i = 0; i < LamiSystem.RectListInspBoxZone_Uper.Count; i++)
//                 {
//                     string checkResult = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i/2 * mesColCount + 6).ToString("000"));
//                     
//                     if (checkResult == "NG") myLinePen.Color = Color.Red;
//                     else myLinePen.Color = Color.LawnGreen;
// 
//                     gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i]);
//                 }

                for (int i = 0; i < Struct_Data.itemResult.Count; i++)
                {
                    if (Struct_Data.itemResult[i] == "NG") myLinePen.Color = Color.Red;
                    else myLinePen.Color = Color.LawnGreen;

                    //gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2]);
                    //gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2 + 1]);

                    //iLstNowRoiNo_Uper
                    gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[iLstNowRoiNo_Uper[i * 2]]);
                    gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[iLstNowRoiNo_Uper[i * 2 + 1]]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        private void Inspect_Run_Run_Inspect_Result_Display_Down()
        {
            try
            {
                if (Inspect_Main02_IplBox.Visible != true) return;

                //폼의 페인트 함수에서 사용되어지는 플래그이다.
                //보기를 변경 했다가 다시 원래대로 돌아올 때 ROI를 다시 보여주기 위한 함수이다.
                _iPaint_Down_Flag = 1;

                Result_Display_Struct Struct_Data = new Result_Display_Struct();
                Struct_Data.itemResult = new List<string>();
                Struct_Data.itemResult = itemResult_Down;
                GetSet_Draw_GC_Down = Inspect_Main02_IplBox.CreateGraphics();

                //Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);
//                 int mesColCount = 19;
//                 for (int i = 0; i < LamiSystem.RectListInspBoxZone_Down.Count; i++)
//                 {
//                     string checkResult = this.GetReg(LamiSystem.RegPathMeasure_Down, (i / 2 * mesColCount + 6).ToString("000"));
// 
//                     if (checkResult == "NG") myLinePen.Color = Color.Red;
//                     else myLinePen.Color = Color.LawnGreen;
// 
//                     gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i]);
//                 }

                for (int i = 0; i < Struct_Data.itemResult.Count; i++)
                {
                    if (Struct_Data.itemResult[i] == "NG") myLinePen.Color = Color.Red;
                    else myLinePen.Color = Color.LawnGreen;

                    //gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i * 2]);
                    //gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i * 2 + 1]);

                    //iLstNowRoiNo_Uper
                    gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[iLstNowRoiNo_Down[i * 2]]);
                    gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[iLstNowRoiNo_Down[i * 2 + 1]]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        /*
         * 
        private void Inspect_Run_Run_Inspect_Result_Display()
        {
            try
            {
                Result_Display_Struct Struct_Data = new Result_Display_Struct();

                Inspect_Run_Run_PictureBox_Reflash();

                Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);

                Struct_Data.displayStr04 = "GAP LEFT : " + _savedValue_Left.ToString("0.000000") + " " + _strSavedNGOK[0];
                //gc.DrawString(Struct_Data.displayStr04, Struct_Data._font, Brushes.LawnGreen, new PointF(130, 600));
                Struct_Data.displayStr05 = "GAP RIGHT : " + _savedValue_Right.ToString("0.000000") + " " + _strSavedNGOK[1];
                //gc.DrawString(Struct_Data.displayStr05, Struct_Data._font, Brushes.LawnGreen, new PointF(410, 600));
                Struct_Data.displayStr06 = "RESULT : " + _strSavedNGOK[2];
                //gc.DrawString(Struct_Data.displayStr06, Struct_Data._font, Brushes.LawnGreen, new PointF(720, 600));

                //픽박스에 인스첵 존을 표시해 주는 함수
                Inspect_Run_Run_Display_Arrow_ZoneBox();

                 Struct_Data.gridTime = String.Format(" {0:00}.{1:00}.{2:00} {3:00}시{4:00}분{5:00}.{6:000}초",
                     _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                     _dtInspDataSaveTime.Hour,
                     _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                     _dtInspDataSaveTime.Millisecond);

                 strLstDisplayData.Clear();
                strLstDisplayData.Add(Struct_Data.gridTime);
                strLstDisplayData.Add(_iSavedGap_Number.ToString("00"));
                strLstDisplayData.Add(_strSavedNGOK[2]);
                strLstDisplayData.Add(_savedValue_Left.ToString("0.000000"));
                strLstDisplayData.Add(_strSavedNGOK[0]);
                strLstDisplayData.Add(_savedValue_Right.ToString("0.000000"));
                strLstDisplayData.Add(_strSavedNGOK[1]);
                strLstDisplayData.Add(_iSavedCenter.ToString("0.0000"));
                strLstDisplayData.Add(_iSavedMax.ToString("0.0000"));
                strLstDisplayData.Add(_iSavedMin.ToString("0.0000"));
            }
            catch (Exception)
            {
                
                //throw;
            }
            
        }
         * 
        private void Inspect_Run_Run_Inspect_Result_Display()
        {


            Result_Display_Struct Struct_Data = new Result_Display_Struct();

            Inspect_Run_Run_PictureBox_Reflash();

            Struct_Data._font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);

            //Struct_Data.displayStr01 = "MAX : " + dRealMaxValue.ToString("0.000000");
            //gc.DrawString(Struct_Data.displayStr01, Struct_Data._font, Brushes.LawnGreen, new PointF(200, 600));
            //Struct_Data.displayStr02 = "CENTER : " + dRealCenterValue.ToString("0.000000");
            //gc.DrawString(Struct_Data.displayStr02, Struct_Data._font, Brushes.LawnGreen, new PointF(410, 600));
            //Struct_Data.displayStr03 = "MIN : " + dRealMinValue.ToString("0.000000");
            //gc.DrawString(Struct_Data.displayStr03, Struct_Data._font, Brushes.LawnGreen, new PointF(620, 600));

            Struct_Data.displayStr04 = "GAP LEFT : " + _savedValue_Left.ToString("0.000000") + " " + _strSavedNGOK[0];
            gc.DrawString(Struct_Data.displayStr04, Struct_Data._font, Brushes.LawnGreen, new PointF(200, 620));
            Struct_Data.displayStr05 = "GAP RIGHT : " + _savedValue_Right.ToString("0.000000") + " " + _strSavedNGOK[1];
            gc.DrawString(Struct_Data.displayStr05, Struct_Data._font, Brushes.LawnGreen, new PointF(410, 620));
            Struct_Data.displayStr06 = "RESULT : " + _strSavedNGOK[2];
            gc.DrawString(Struct_Data.displayStr06, Struct_Data._font, Brushes.LawnGreen, new PointF(620, 620));

            //픽박스에 인스첵 존을 표시해 주는 함수
            Inspect_Run_Run_Display_Arrow_ZoneBox();

            Struct_Data.gridTime = String.Format(" {0:00}.{1:00}.{2:00} {3:00}시{4:00}분{5:00}.{6:000}초",
                _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                _dtInspDataSaveTime.Hour,
                _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

            strLstDisplayData.Clear();
            strLstDisplayData.Add(Struct_Data.gridTime);
            strLstDisplayData.Add(_iSavedGap_Number.ToString("00"));
            strLstDisplayData.Add(_strSavedNGOK[2]);
            strLstDisplayData.Add(_savedValue_Left.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[0]);
            strLstDisplayData.Add(_savedValue_Right.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[1]);
            strLstDisplayData.Add(_iSavedCenter.ToString("0.0000"));
            strLstDisplayData.Add(_iSavedMax.ToString("0.0000"));
            strLstDisplayData.Add(_iSavedMin.ToString("0.0000"));
        }
        */
        private readonly List<string> strLstDisplayData_Uper = new List<string>();
        private readonly List<string> strLstDisplayData_Down = new List<string>();
        private void Inspect_Run_Run_None_Point_Search_Display()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Result_Display_Struct Struct_Data = new Result_Display_Struct();

            //Inspect_Run_Run_PictureBox_Reflash();
           
            myLinePen.Color = Color.LawnGreen;

            Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);

            Struct_Data.displayStr04 = "GAP LEFT : " + _savedValue_Uper.ToString("0.000000") + " " + _strSavedNGOK[0];
            gc_Uper.DrawString(Struct_Data.displayStr04, Struct_Data._font, Brushes.LawnGreen, new PointF(130, 600));
            Struct_Data.displayStr05 = "GAP RIGHT : " + _savedValue_Down.ToString("0.000000") + " " + _strSavedNGOK[1];
            gc_Uper.DrawString(Struct_Data.displayStr05, Struct_Data._font, Brushes.LawnGreen, new PointF(410, 600));
            Struct_Data.displayStr06 = "RESULT : " + _strSavedNGOK[2];
            gc_Uper.DrawString(Struct_Data.displayStr06, Struct_Data._font, Brushes.LawnGreen, new PointF(720, 600));

            //gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i]);
            //픽박스에 인스첵 존을 표시해 주는 함수
            //Run_Run_Display_Arrow_ZoneBox_Uper();
        }
        
        /*
        private void Inspect_Run_Run_Inspect_Result_Display()
        {
            Result_Display_Struct Struct_Data = new Result_Display_Struct();
            Inspect_Run_Run_PictureBox_Reflash();

            //List<CvPoint> BoxDrawPoint = _posConverter_Gap.ImageToBox(srcIplImage_Gap, Inspect_Main01_IplBox, _savedCvPntLstImagePoint);
            
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
            strLstDisplayData.Add(_iSavedGap_Number.ToString("00"));
            strLstDisplayData.Add(_strSavedNGOK[2]);
            strLstDisplayData.Add(_savedValue_Left.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[0]);
            strLstDisplayData.Add(_savedValue_Right.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[1]);
            strLstDisplayData.Add(_iSavedCenter.ToString("0.0000"));
            strLstDisplayData.Add(_iSavedMax.ToString("0.0000"));
            strLstDisplayData.Add(_iSavedMin.ToString("0.0000"));
        }

        private readonly List<string> strLstDisplayData = new List<string>();
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
            strLstDisplayData.Add(_iSavedGap_Number.ToString("00"));
            strLstDisplayData.Add(_strSavedNGOK[2]);
            strLstDisplayData.Add(_savedValue_Left.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[0]);
            strLstDisplayData.Add(_savedValue_Right.ToString("0.000000"));
            strLstDisplayData.Add(_strSavedNGOK[1]);
            strLstDisplayData.Add(_iSavedCenter.ToString("0.0000"));
            strLstDisplayData.Add(_iSavedMax.ToString("0.0000"));
            strLstDisplayData.Add(_iSavedMin.ToString("0.0000"));
        }
        */
        private PictureBoxIpl Now_PictureBox;
        public PictureBoxIpl GetSet_Display_PictureBox
        {
            get { return Now_PictureBox; }
            set { Now_PictureBox = value; }
        }

        private string Now_Used_Form;

        public string GetSet_Used_Form
        {
            get { return Now_Used_Form; }
            set { Now_Used_Form = value; }
        }


        public struct ArrowZoneBox_Struct
        {
            public int pointCount;
            public int nowROI_Number;
            public CvPoint startPoint;
            public CvPoint endPoint;
            public int tmpWidth;
        }
        private void Run_Run_Display_Arrow_ZoneBox_Uper() 
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //이미지의 단위를 픽처박스의 단위로 변환해 준다.
            ArrowZoneBox_Struct Struct_Data = new ArrowZoneBox_Struct();
            //gc = Inspect_Main01_IplBox.CreateGraphics();

            myLinePen.Color = Color.LawnGreen;
            myLinePen.Width = 2;
            myArrowPen.Width = 2;
            Struct_Data.pointCount = 0;
            Struct_Data.nowROI_Number = -1;

            if (NowGapTypeNumber == 2) Struct_Data.nowROI_Number = 4;
            else if (NowGapTypeNumber == 3) Struct_Data.nowROI_Number = 8;
            else Struct_Data.nowROI_Number = 0;

            //boxArrowPntLst = _posConverter_Gap.ImageToBox(SrcIplImageGap, Now_PictureBox, _savedCvPntLstImagePoint);
            boxArrowPntLst = _posConverter_Uper.ImageToBox(NowSavedImage_Uper, Now_PictureBox, _savedCvPntLstImagePoint_Uper);
            boxArrowPntLst[4] = boxArrowPntLst[0];
            boxArrowPntLst[8] = boxArrowPntLst[0];
            boxArrowPntLst[6] = boxArrowPntLst[2];
            boxArrowPntLst[10] = boxArrowPntLst[2];

            for (int i = Struct_Data.nowROI_Number; i < Struct_Data.nowROI_Number + 4; i++)
            {
                Struct_Data.startPoint = new CvPoint(0, 0);
                Struct_Data.endPoint = new CvPoint(0, 0);
                if (Now_Used_Form == "Main")
                {
                   Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X, boxArrowPntLst[i].Y);
                   Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (LamiSystem.RectListRecipeBoxZone_Uper[i].Width), boxArrowPntLst[i].Y);
                }
                else if (Now_Used_Form == "Inspect")
                {
                   
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X, boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (LamiSystem.RectListInspBoxZone_Uper[i].Width), boxArrowPntLst[i].Y);
                    
                }
                GetSet_Draw_GC_Uper = Inspect_Main01_IplBox.CreateGraphics();
                gc_Uper.DrawRectangle(myLinePen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);

                Struct_Data.tmpWidth = LamiSystem.RectListInspBoxZone_Uper[i].Width / 4;
                if (Struct_Data.pointCount == 0 || Struct_Data.pointCount == 2)
                {
                   
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                   
                }
                else if (Struct_Data.pointCount == 1 || Struct_Data.pointCount == 3)
                {
                   
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                   
                }
                Struct_Data.pointCount++;
            }
        }


        /*
         * 
         private void Run_Run_Display_Arrow_ZoneBox_Uper() 
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //이미지의 단위를 픽처박스의 단위로 변환해 준다.
            ArrowZoneBox_Struct Struct_Data = new ArrowZoneBox_Struct();
            //gc = Inspect_Main01_IplBox.CreateGraphics();

            myLinePen.Color = Color.LawnGreen;
            myLinePen.Width = 2;
            myArrowPen.Width = 2;
            Struct_Data.pointCount = 0;
            Struct_Data.nowROI_Number = -1;

            if (NowGapTypeNumber == 2) Struct_Data.nowROI_Number = 4;
            else if (NowGapTypeNumber == 3) Struct_Data.nowROI_Number = 8;
            else Struct_Data.nowROI_Number = 0;

            //boxArrowPntLst = _posConverter_Gap.ImageToBox(SrcIplImageGap, Now_PictureBox, _savedCvPntLstImagePoint);
            boxArrowPntLst = _posConverter.ImageToBox(NowSavedImage_Uper, Now_PictureBox, _savedCvPntLstImagePoint_Uper);
            boxArrowPntLst[4] = boxArrowPntLst[0];
            boxArrowPntLst[8] = boxArrowPntLst[0];
            boxArrowPntLst[6] = boxArrowPntLst[2];
            boxArrowPntLst[10] = boxArrowPntLst[2];

            for (int i = Struct_Data.nowROI_Number; i < Struct_Data.nowROI_Number + 4; i++)
            {
                Struct_Data.startPoint = new CvPoint(0, 0);
                Struct_Data.endPoint = new CvPoint(0, 0);
                if (Now_Used_Form == "Main")
                {
                   Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X, boxArrowPntLst[i].Y);
                   Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (LamiSystem.RectListRecipeBoxZone_Uper[i].Width), boxArrowPntLst[i].Y);
                }
                else if (Now_Used_Form == "Inspect")
                {
                   
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X, boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (LamiSystem.RectListInspBoxZone_Uper[i].Width), boxArrowPntLst[i].Y);
                    
                }
                GetSet_Draw_GC_Uper = Inspect_Main01_IplBox.CreateGraphics();
                gc_Uper.DrawLine(myLinePen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);

                Struct_Data.tmpWidth = LamiSystem.RectListInspBoxZone_Uper[i].Width / 4;
                if (Struct_Data.pointCount == 0 || Struct_Data.pointCount == 2)
                {
                    if (Now_Used_Form == "Main")
                    {
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                    }
                    else if (Now_Used_Form == "Inspect")
                    {
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y - 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                    }
                }
                else if (Struct_Data.pointCount == 1 || Struct_Data.pointCount == 3)
                {
                    if (Now_Used_Form == "Main")
                    {
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListRecipeBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                    }
                    else if (Now_Used_Form == "Inspect")
                    {
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 1), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 2), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                        Struct_Data.startPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y);
                        Struct_Data.endPoint = new CvPoint(LamiSystem.RectListInspBoxZone_Uper[i].X + (Struct_Data.tmpWidth * 3), boxArrowPntLst[i].Y + 30);
                        gc_Uper.DrawLine(myArrowPen, Struct_Data.startPoint.X, Struct_Data.startPoint.Y, Struct_Data.endPoint.X, Struct_Data.endPoint.Y);
                    }
                }
                Struct_Data.pointCount++;
            }
        }

         * 
         * 
         private void Inspect_Run_Run_Display_Arrow_ZoneBox()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            //이미지의 단위를 픽처박스의 단위로 변환해 준다.
            ArrowZoneBox_Struct Struct_Data = new ArrowZoneBox_Struct();

            myLinePen.Color = Color.LawnGreen;
            myLinePen.Width = 2;
            myArrowPen.Width = 2;
            int pointCount = 0;
            int nowROI_Number = -1;

            if (NowGapTypeNumber == 2) nowROI_Number = 4;
            else if (NowGapTypeNumber == 3) nowROI_Number = 8;
            else nowROI_Number = 0;

            boxArrowPntLst = _posConverter_Gap.ImageToBox(SrcIplImageGap, Now_PictureBox, _savedCvPntLstImagePoint);
            for (int i = nowROI_Number; i < nowROI_Number + 4; i++)
            {
                var startPoint = new CvPoint(0, 0);
                var endPoint = new CvPoint(0, 0);
                if (Now_Used_Form == "Main")
                {
                    startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X, boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (GapSystem.RectListRecipeBoxZone_Gap[i].Width), boxArrowPntLst[i].Y);
                }
                else if (Now_Used_Form == "Inspect")
                {
                    startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X, boxArrowPntLst[i].Y);
                    endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (GapSystem.RectListInspBoxZone_Gap[i].Width), boxArrowPntLst[i].Y);
                }
                
                gc.DrawLine(myLinePen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);

                int tmpWidth = GapSystem.RectListInspBoxZone_Gap[i].Width / 4;
                if (pointCount == 0 || pointCount == 2)
                {
                    if (Now_Used_Form == "Main")
                    {
                        startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y - 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y - 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y - 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    }
                    else if (Now_Used_Form == "Inspect")
                    {
                        startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y - 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y - 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y - 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    }
                }
                else if (pointCount == 1 || pointCount == 3)
                {
                    if (Now_Used_Form == "Main")
                    {
                        startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y + 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y + 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListRecipeBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y + 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    }
                    else if (Now_Used_Form == "Inspect")
                    {
                        startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 1), boxArrowPntLst[i].Y + 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 2), boxArrowPntLst[i].Y + 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        startPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y);
                        endPoint = new CvPoint(GapSystem.RectListInspBoxZone_Gap[i].X + (tmpWidth * 3), boxArrowPntLst[i].Y + 30);
                        gc.DrawLine(myArrowPen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    }
                }
                pointCount++;
            }
        }
        */

        /*
        private void Inspect_Run_Run_PictureBox_Reflash()
        {
            try
            {
                if (Inspect_Main01_IplBox.InvokeRequired)
                {
                    Delegate_Run_Run_Drawing_Result_Inspection_Display del = Inspect_Run_Run_PictureBox_Reflash;
                    Inspect_Main01_IplBox.Invoke(del);
                }
                else
                {
                    //NowGapNumber, NowProdectNumber_Gap
                    uTxt_Gap_No.Text = NowGapNumber.ToString();
                    //uTxt_Gap_No.Refresh();

                    //NowProdectNumber_Gap = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus, "Count_Trigger"));

                    //Inspect_uLabel_Assy05.Text = NowProdectNumber_Gap.ToString();
                    Inspect_uLabel_Assy05.Text = NowProdectNumber.ToString();//Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus, "Count_Trigger");
                    //Inspect_uLabel_Assy05.Refresh();

                    Inspect_uLabel_Assy06.Text = NowFailNumber_Uper.ToString("0") + " / " + NowTrigNumber;
                    //Inspect_uLabel_Assy06.Refresh();

                    Inspect_uLabel_Assy07.Text = NowGapNumber.ToString();
                    //Inspect_uLabel_Assy07.Refresh();

                    //Inspect_Main01_IplBox.ImageIpl = SrcIplImageGap;//.Clone(); 
                    Inspect_Main01_IplBox.ImageIpl = NowSavedImage_Uper; 
                    Inspect_Main01_IplBox.Refresh();
                }
            }
            catch (Exception e)
            {
                string ErrorMessage = e.Message;
                //throw;
            }
            
        }
        */


        /*
         private void Inspect_Run_Run_Result_Write_Reg()
        {
            //_strSavedNGOK
            //갭토탈에 사용되는 결과값 기록
            if (strLstDisplayData[2] == "NG")
                SetReg(GapSystem.RegPathGapTotal, "ResultData", strLstDisplayData[2]);

            //갭토탈에 사용되는 측정값 기록
            string RegTitleL = "GapL" + NowGapNumber.ToString("0");
            string RegTitleR = "GapR" + NowGapNumber.ToString("0");
            SetReg(GapSystem.RegPathGapTotal, RegTitleL, strLstDisplayData[3]);
            SetReg(GapSystem.RegPathGapTotal, RegTitleR, strLstDisplayData[5]);

            GapTotal_Write_Flag = true;
        }
        */


        /*
        private void Inspect_Run_Run_Data_Save()
        {
            _strSavedInspectResult = _strSavedNGOK[2];

            //엑셀 파일에 결과를 저장한다.
            _dtImageSaveTime = DateTime.Now;

            if (GapTotal_Filename != null)
            {
                Gap_Zero_Setting1 = new Thread(WriteExcelFileData);
                Gap_Zero_Setting1.Start();
            }
        

            TotalGap_Write_Thread = new Thread(Inspect_Run_Run_Result_Write_GapTotal);
            TotalGap_Write_Thread.Start();
            
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
        */

        private static string _passSaveData_Uper = string.Empty;
        private static string _passSaveImage_Uper = string.Empty;
        private static string _failSaveData_Uper = string.Empty;
        private static string _failSaveImage_Uper = string.Empty;

        private static string _passSaveData_Down = string.Empty;
        private static string _passSaveImage_Down = string.Empty;
        private static string _failSaveData_Down = string.Empty;
        private static string _failSaveImage_Down = string.Empty;

        private Thread TotalGap_Write_Thread;

        //20150327 WKB 301
        private void Inspect_Result_Data_Save_Uper()
        {
            try
            {
                if ((_passSaveImage_Uper == "ON" && Uper_Measure_Result == true) || (_failSaveImage_Uper == "ON" && Uper_Measure_Result == false) )
                {
                    Inspect_Result_Image_To_File_Uper();
                    Inspect_Run_Run_Drawing_Result_To_History_NG_Uper();
                    if (_failSaveImage_Uper == "ON" && Uper_Measure_Result == false)
                    {
                        Inspect_Run_Run_Drawing_Result_To_History_All_Uper();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        //20150327 WKB 209
        /*
        private void Inspect_Result_Data_Save_Uper()
        {
            try
            {
                if ((_passSaveImage_Uper == "ON" && Uper_Measure_Result == true) || (_failSaveImage_Uper == "ON" && Uper_Measure_Result == false) )
                {
                    File_Save_Thread_Uper = new Thread(Inspect_Result_Image_To_File_Uper);
                    File_Save_Thread_Uper.Start();

                    History_All_Save_Thread_Uper = new Thread(Inspect_Run_Run_Drawing_Result_To_History_All_Uper);
                    History_All_Save_Thread_Uper.Start();


                    if (_failSaveImage_Uper == "ON" && Uper_Measure_Result == false)
                    {
                        History_NG_Save_Thread_Uper = new Thread(Inspect_Run_Run_Drawing_Result_To_History_NG_Uper);
                        History_NG_Save_Thread_Uper.Start();
                    }                   
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */

        private void Inspect_Result_Data_Save_Down()
        {
            try
            {
                if ((_passSaveImage_Down == "ON" && Down_Measure_Result == true) || (_failSaveImage_Down == "ON" && Down_Measure_Result == false))
                {
                    Inspect_Result_Image_To_File_Down();
                    Inspect_Run_Run_Drawing_Result_To_History_All_Down();
                    if (_failSaveImage_Down == "ON" && Down_Measure_Result == false)
                    {
                        Inspect_Run_Run_Drawing_Result_To_History_NG_Down();
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }



        public string Inspect_Run_Run_GapTotal_Zero_Writing()
        {
            string tmpStr = "OK,\r\n";

            for (int i = 0; i < NowModel_Cells; i++)
            {
                if (GapTotal_Result_NGOK[i] == "NG")
                {
                    tmpStr = "NG,\r\n";
                    break;
                }
            }

            string saveTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                    DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);

            tmpStr += saveTime + ",";


            return tmpStr;
        }


        private string[] GapTotal_Result_NGOK =
        {
            "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG",
            "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG", "NG"
        };

        StringBuilder gap_Total_Data = new StringBuilder();


        private string Inspect_Set_FileName_ExcelFile_Uper(DateTime checkTime)
        {
            //string folderName = NowExcelFolderSavePath + "\\Gap" + String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string folderName = _NowExcelFolderSavePath_Uper + String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string fileName = folderName + String.Format("\\{0:00}년{1:00}월{2:00}일 상부 비전.csv", checkTime.Year, checkTime.Month, checkTime.Day);
            List<string> itemNames = Inspect_ExcelFile_ItemNames_Make_Uper();
            excelFile.Excel_Folder_Check_Or_Make(folderName);
            excelFile.Excel_File_Check_Or_Make_Uper(fileName, itemNames);

            return fileName;
        }

        public List<string> Inspect_ExcelFile_ItemNames_Make_Uper()
        {
            List<string> nameList = new List<string>();
            int RecipeRows = LamiSystem.StrLstRcpConGridData_Uper.Count/11;
            for (int i = 0; i < RecipeRows; i++)
            {
                string tmpData = LamiSystem.StrLstRcpConGridData_Uper[i*11];
                nameList.Add(LamiSystem.StrLstRcpConGridData_Uper[i*11]);
            }
            return nameList;
        }

        private string Inspect_Set_FileName_ExcelFile_Down(DateTime checkTime)
        {
            string folderName = _NowExcelFolderSavePath_Uper + String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string fileName = folderName + String.Format("\\{0:00}년{1:00}월{2:00}일 하부 비전.csv", checkTime.Year, checkTime.Month, checkTime.Day);
            List<string> itemNames = Inspect_ExcelFile_ItemNames_Make_Down();
            excelFile.Excel_Folder_Check_Or_Make(folderName);
            excelFile.Excel_File_Check_Or_Make_Down(fileName, itemNames);

            return fileName;
        }

        public List<string> Inspect_ExcelFile_ItemNames_Make_Down()
        {
            List<string> nameList = new List<string>();
            int RecipeRows = LamiSystem.StrLstRcpConGridData_Down.Count / 11;
            for (int i = 0; i < RecipeRows; i++)
            {
                //string tmpData = LamiSystem.StrLstRcpConGridData_Down[i * 11];
                nameList.Add(LamiSystem.StrLstRcpConGridData_Down[i * 11]);
            }
            return nameList;
        }

        public string[] _strSavedNGOK = { string.Empty, string.Empty, string.Empty };


        public struct Struct_Inspection_Excel_Data
        {
            public List<string> itemResult_List;// = itemResult_Uper;
            public List<string> MeasureData_List;// = MeasureData_Uper;
            public List<string> ExcelWriteData;// = new List<string>();
            public string SaveTriggerNum;// = NowTrigNumber.ToString("0000000000");
            public string writeTime;
            public string nowExcelFileName;
        }

        public void FormDlgInsp_Inspection_Excel_Data_Write_Uper()
        {
            try
            {
                //Stopwatch excelwritewatch = new Stopwatch();
                //excelwritewatch.Reset();
                //excelwritewatch.Start();

                Struct_Inspection_Excel_Data struct_Excel_Uper = new Struct_Inspection_Excel_Data();
                struct_Excel_Uper.itemResult_List = itemResult_Uper;
                struct_Excel_Uper.MeasureData_List = MeasureData_Uper;
                
                Uper_Measure_Result = true;

                struct_Excel_Uper.ExcelWriteData = new List<string>();
                _dtInspDataSaveTime = DateTime.Now;

                if (_passSaveData_Uper == "OFF" && _strSavedInspectResult_Uper == "OK") return;
                if (_failSaveData_Uper == "OFF" && _strSavedInspectResult_Uper == "NG") return;

                struct_Excel_Uper.ExcelWriteData.Add(GetSet_NowModel_Name);

                //2015.02.10 WKB 207
                //struct_Excel_Uper.SaveTriggerNum = NowTrigNumber.ToString("0000000000");

                //2015.02.10 WKB 208
                struct_Excel_Uper.SaveTriggerNum = _iSavedTrigNumber.ToString("0000000000");
                //Trace.WriteLine("iSavedTriggerNumber : " + _iSavedTrigNumber);

                struct_Excel_Uper.ExcelWriteData.Add(struct_Excel_Uper.SaveTriggerNum);

                struct_Excel_Uper.writeTime = String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                    _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                    _dtInspDataSaveTime.Hour,
                    _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                    _dtInspDataSaveTime.Millisecond);
                struct_Excel_Uper.ExcelWriteData.Add(struct_Excel_Uper.writeTime);

                if (_strSavedInspectResult_Uper == "OK") Uper_Measure_Result = true;
                else Uper_Measure_Result = false;



                struct_Excel_Uper.ExcelWriteData.Add(_strSavedInspectResult_Uper);

                for (int i = 0; i < struct_Excel_Uper.MeasureData_List.Count; i++)
                {
                    struct_Excel_Uper.ExcelWriteData.Add(struct_Excel_Uper.MeasureData_List[i]);
                }

                struct_Excel_Uper.nowExcelFileName = Inspect_Set_FileName_ExcelFile_Uper(_dtInspDataSaveTime);
                excelFile.WriteExcelFile(struct_Excel_Uper.nowExcelFileName, struct_Excel_Uper.ExcelWriteData);

                //excelwritewatch.Stop();
                //Trace.WriteLine("엑셀 저장 : "+excelwritewatch.ElapsedMilliseconds.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }

        }


        public void FormDlgInsp_Inspection_Excel_Data_Write_Down()
        {
            try
            {
                Struct_Inspection_Excel_Data struct_Excel_Down = new Struct_Inspection_Excel_Data();
                struct_Excel_Down.itemResult_List = itemResult_Down;
                struct_Excel_Down.MeasureData_List = MeasureData_Down;

                Down_Measure_Result = true;

                struct_Excel_Down.ExcelWriteData = new List<string>();
                
                if (_passSaveData_Down == "OFF" && _strSavedInspectResult_Down == "OK") return;
                if (_failSaveData_Down == "OFF" && _strSavedInspectResult_Down == "NG") return;

                struct_Excel_Down.ExcelWriteData.Add(GetSet_NowModel_Name);

                struct_Excel_Down.SaveTriggerNum = NowTrigNumber.ToString("0000000000");
                struct_Excel_Down.ExcelWriteData.Add(struct_Excel_Down.SaveTriggerNum);

                struct_Excel_Down.writeTime = String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초", _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                    _dtInspDataSaveTime.Hour,_dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,_dtInspDataSaveTime.Millisecond);
                struct_Excel_Down.ExcelWriteData.Add(struct_Excel_Down.writeTime);

                if (_strSavedInspectResult_Down == "OK") Down_Measure_Result = true;
                else Down_Measure_Result = false;

                struct_Excel_Down.ExcelWriteData.Add(_strSavedInspectResult_Down);

                for (int i = 0; i < struct_Excel_Down.MeasureData_List.Count; i++)
                {
                    struct_Excel_Down.ExcelWriteData.Add(struct_Excel_Down.MeasureData_List[i]);
                }

                string nowExcelFileName = Inspect_Set_FileName_ExcelFile_Down(_dtInspDataSaveTime);
                excelFile.WriteExcelFile(nowExcelFileName, struct_Excel_Down.ExcelWriteData);
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }


        /*
        
        public void FormDlgInsp_Inspection_Excel_Data_Write_Uper()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                List<string> itemResult_List = itemResult_Uper;
                List<string> MeasureData_List = MeasureData_Uper;
                
                Uper_Measure_Result = true;

                List<string> ExcelWriteData = new List<string>();
                _dtInspDataSaveTime = DateTime.Now;

                if (_passSaveData_Uper == "OFF" && _strSavedInspectResult_Uper == "OK") return;
                if (_failSaveData_Uper == "OFF" && _strSavedInspectResult_Uper == "NG") return;

                ExcelWriteData.Add(GetSet_NowModel_Name);

                string SaveTriggerNum = NowTrigNumber.ToString("0000000000");
                ExcelWriteData.Add(SaveTriggerNum);

                string writeTime = String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                    _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                    _dtInspDataSaveTime.Hour,
                    _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                    _dtInspDataSaveTime.Millisecond);
                ExcelWriteData.Add(writeTime);

//                 bool Uper_Result = true;
//                 string InspData = "OK";
// 
//                 for (int i = 0; i < itemResult_List.Count; i++)
//                 {
//                     if (itemResult_List[i] == "NG")
//                     {
//                         Uper_Result = false;
//                         InspData = "NG";
//                         break;
//                     }
//                 }
//                Uper_Measure_Result = Uper_Result;

                if (_strSavedInspectResult_Uper == "OK") Uper_Measure_Result = true;
                else Uper_Measure_Result = false;



                ExcelWriteData.Add(_strSavedInspectResult_Uper);

                for (int i = 0; i < MeasureData_List.Count; i++)
                {
                    ExcelWriteData.Add(MeasureData_List[i]);
                }

                string nowExcelFileName = Inspect_Set_FileName_ExcelFile_Uper(_dtInspDataSaveTime);
                excelFile.WriteExcelFile(nowExcelFileName, ExcelWriteData);
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }

        }


        public void FormDlgInsp_Inspection_Excel_Data_Write_Down()
        {
            try
            {
                List<string> itemResult_List = itemResult_Down;
                List<string> MeasureData_List = MeasureData_Down;

                Down_Measure_Result = true;

                List<string> ExcelWriteData = new List<string>();
                
                if (_passSaveData_Down == "OFF" && _strSavedInspectResult_Down == "OK") return;
                if (_failSaveData_Down == "OFF" && _strSavedInspectResult_Down == "NG") return;

                ExcelWriteData.Add(GetSet_NowModel_Name);

                string SaveTriggerNum = NowTrigNumber.ToString("0000000000");
                ExcelWriteData.Add(SaveTriggerNum);

                string writeTime = String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",_dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                    _dtInspDataSaveTime.Hour,_dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,_dtInspDataSaveTime.Millisecond);
                ExcelWriteData.Add(writeTime);


//                 int mesColCount = 19;
//                 int RecipeColumn = 11;
//                 bool Down_Result = true;
//                 string InspData = "OK";
//                 for (int i = 0; i < itemResult_List.Count; i++)
//                 {
//                     if (itemResult_List[i] == "NG")
//                     {
//                         Down_Result = false;
//                         InspData = "NG";
//                         break;
//                     }
//                 }
// 
//                 Down_Measure_Result = Down_Result;
//                 ExcelWriteData.Add(InspData);

                if (_strSavedInspectResult_Down == "OK") Down_Measure_Result = true;
                else Down_Measure_Result = false;

                ExcelWriteData.Add(_strSavedInspectResult_Down);

                for (int i = 0; i < MeasureData_List.Count; i++)
                {
                    ExcelWriteData.Add(MeasureData_List[i]);
                }

                string nowExcelFileName = Inspect_Set_FileName_ExcelFile_Down(_dtInspDataSaveTime);
                excelFile.WriteExcelFile(nowExcelFileName, ExcelWriteData);
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */
        //갭데이터를 레지에 저장했는지를 알리는 플래그
        //false이면 갭 번호가 0이어도 파이에 저장하지 않느다.
        private bool GapTotal_Write_Flag = false;

        /*
        public void FormDlgInsp_Inspection_Excel_Data_Write_Uper()
         {
             //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
             try
             {
                 Uper_Measure_Result = true;

                 List<string> ExcelWriteData = new List<string>();
                 //var tmpDateTime = new DateTime();
                 //while (_dtInspDataSaveTime.Date == tmpDateTime.Date) _dtInspDataSaveTime = DateTime.Now;

                 _dtInspDataSaveTime = DateTime.Now;

                 if (_passSaveData_Uper == "OFF" && _strSavedInspectResult_Uper == "OK") return;
                 if (_failSaveData_Uper == "OFF" && _strSavedInspectResult_Uper == "NG") return;

                 //string modelName = GetSet_NowModel_Name;
                 ExcelWriteData.Add(GetSet_NowModel_Name);

                 string SaveTriggerNum = NowTrigNumber.ToString("0000000000");
                 ExcelWriteData.Add(SaveTriggerNum);


                 //string writeDate = _dtInspDataSaveTime.ToString("yyyy-MM-dd");
                 //ExcelWriteData.Add(writeDate);

                 string writeTime = String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                     _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                     _dtInspDataSaveTime.Hour,
                     _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                     _dtInspDataSaveTime.Millisecond);
                 ExcelWriteData.Add(writeTime);

                 int mesColCount = 19;
                 int RecipeColumn = 11;
                 bool Uper_Result = true;
                 bool Down_Result = true;
                 string InspData = "OK";
                 for (int i = 0; i < uGrd_Inspect_Measure_Uper.Rows.Count; i++)
                 {
                     string InspResult = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 6).ToString("000"));

                     if (InspResult != "NG") continue;

                     string NgItem = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 1).ToString("000"));
                     int RcpRows = LamiSystem.StrLstRcpConGridData_Uper.Count/11;

                     for (int j = 0; j < RcpRows; j++)
                     {
                         string RcpItem = LamiSystem.StrLstRcpConGridData_Uper[j*RecipeColumn + 0];
                         if (NgItem == RcpItem)
                         {
                             string Checked = LamiSystem.StrLstRcpConGridData_Uper[j*RecipeColumn + 9];
                             if (Checked == "True")
                             {
                                 Uper_Result = false;
                                 InspData = "NG";
                                 break;
                             }
                         }
                     }
                 }

                 Uper_Measure_Result = Uper_Result;

                 ExcelWriteData.Add(InspData);

 //                 string InspData = string.Empty;
 // 
 //                 for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
 //                 {
 //                     if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value.ToString() == "NG")
 //                     {
 //                         InspData = "NG";
 //                         break;
 //                     }
 //                     InspData = "OK";
 //                 }
 //                 ExcelWriteData.Add(InspData);

                 for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
                 {
                     string WriteMeasData = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value.ToString();
                     if (WriteMeasData != "")
                         ExcelWriteData.Add(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value.ToString());
                 }


                 string nowExcelFileName = Inspect_Set_FileName_ExcelFile_Uper(_dtInspDataSaveTime);
                 excelFile.WriteExcelFile(nowExcelFileName, ExcelWriteData);
             }
             catch (Exception e)
             {
                 MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
             }

         }
        */
        /*
        public void FormDlgInsp_Inspection_Excel_GapTotal_Write()
        {
            gap_Total_Data.Clear();
            string TimeData = GetReg(GapSystem.RegPathGapTotal, "TimeData");
            gap_Total_Data.Append(TimeData);
            gap_Total_Data.Append(",");
            //gap_Total_Data
            string CountCell = GetReg(GapSystem.RegPathGapTotal, "CellCount");
            int CntNo = int.Parse(CountCell);
            bool NoneFlag = false;

            for (int i = 0; i < CntNo; i++)
            {
                string tmpStrL = GetReg(GapSystem.RegPathGapTotal, "GapL" + i.ToString("0"));

                //읽은 값이 "0"이면 측정 불가이므로 플래그를 세운다.
                if (tmpStrL == "0" && NoneFlag == false) NoneFlag = true;
                gap_Total_Data.Append(tmpStrL);
                gap_Total_Data.Append(",");

                string tmpStrR = GetReg(GapSystem.RegPathGapTotal, "GapR" + i.ToString("0"));

                //읽은 값이 "0"이면 측정 불가이므로 플래그를 세운다.
                if (tmpStrR == "0" && NoneFlag == false) NoneFlag = true;
                gap_Total_Data.Append(tmpStrR);
                gap_Total_Data.Append(",");
            }

            string ResultData = GetReg(GapSystem.RegPathGapTotal, "ResultData");
            if (NoneFlag == true) ResultData = "NG";

            gap_Total_Data.Append(ResultData);
            gap_Total_Data.Append(",");

            //string nowExcelFileName_Total = Inspect_Set_FileName_ExcelFile_Total(_dtInspDataSaveTime);
            //excelFile.WriteExcelFile(nowExcelFileName_Total, gap_Total_Data);
            GapTotal_Filename = Inspect_Set_FileName_ExcelFile_Total(_dtInspDataSaveTime);
            excelFile.WriteExcelFile(GapTotal_Filename, gap_Total_Data);

            //생산 수량을 업데이트한다.
            //GetSet_NowProduct_Number();

            string saveTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                _dtInspDataSaveTime.Hour, _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                _dtInspDataSaveTime.Millisecond);

            SetReg(GapSystem.RegPathGapTotal, "TimeData", saveTime);
            SetReg(GapSystem.RegPathGapTotal, "CellCount", NowModel_Cells.ToString("0"));
            SetReg(GapSystem.RegPathGapTotal, "ResultData", "OK");
            for (int i = 0; i < CntNo; i++)
            {
                string tmpStrL = "GapL" + i.ToString("0");
                SetReg(GapSystem.RegPathGapTotal, tmpStrL, "0");
                string tmpStrR = "GapR" + i.ToString("0");
                SetReg(GapSystem.RegPathGapTotal, tmpStrR, "0");
            }
        }
        */



        public void GetSet_NowProduct_Number()
        {
            //if (NowGapNumber == (NowModel_Cells - 1))
            //{
                NowProdectNumber = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_Trigger"));
                if (NowProdectNumber < uint.MaxValue)
                    NowProdectNumber++;
                else
                    NowProdectNumber = 1;
                SetReg(LamiSystem.RegPathGapStatus, "Count_Trigger", NowProdectNumber.ToString());
            //}
        }

        //기존의 히스토리 픽처박스 번호가 처리용 프로세스에서 이미 다음 것을 진행하기
        //때문에 번호가 씽크되지 않아서 _iImgCount 대신에 표시부 쓰레드 시작하기 전에
        //_iImgCount 를 _iHistoryViewNo에 넘져준다. 
        private void Inspect_Run_Run_Drawing_Result_To_History_All_Uper()
        {
            if (uPnl_History_All_Uper.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_All_Uper;
                uPnl_History_All_Uper.Invoke(del);
            }
            else
            {
                if (_strHisImgName_Uper.Count == 0) return;
                if (string.IsNullOrEmpty(_strHisImgName_Uper[_strHisImgName_Uper.Count - 1]) == true) return;

                _strHistoryViewNameAll_Uper[_iAllHistoryViewNo_Uper] = _strHisImgName_Uper[_strHisImgName_Uper.Count-1];
                _strHisImgName_Uper.Clear();
                
                ((PictureBoxIpl) (uPnl_History_All_Uper.ClientArea.Controls[_iAllHistoryViewNo_Uper])).ImageIpl =
                    NowSavedImage_Uper;
                uPnl_History_All_Uper.ClientArea.Controls[_iAllHistoryViewNo_Uper].Refresh();

                Inspect_Run_Run_History_Grid_All_Uper();

                if (_iAllHistoryViewNo_Uper + 1 == 20) _iAllHistoryViewNo_Uper = 0;
                else _iAllHistoryViewNo_Uper++;
            }
        }

        private void Inspect_Run_Run_History_Grid_All_Uper()
        {
            UltraGridRow row;

            if (uGrd_History_All_Uper.DisplayLayout.Rows.Count < 20)
            {
                row = uGrd_History_All_Uper.DisplayLayout.Bands[0].AddNew();
            }
            else
            {
                row = uGrd_History_All_Uper.DisplayLayout.Rows[_iAllHistoryViewNo_Uper];
            }

            for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
            {
                string WriteMeasData =
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                if (WriteMeasData != "")
                {
                    string ColName = (i + 1).ToString("0");
                    row.Cells[ColName].Value =
                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                }
            }
        }

        /*
         private void Inspect_Run_Run_Drawing_Result_To_History_All_Uper()
        {
            if (uPnl_History_All_Uper.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_All_Uper;
                uPnl_History_All_Uper.Invoke(del);
            }
            else
            {
                if (NowModel_Cells == uPnl_History_All_Uper.ClientArea.Controls.Count)
                {
                    _strHistoryViewNameAll_Uper[_iSaved_Uper_Number] = _strHisImgName_Uper;
                    ((PictureBoxIpl)(uPnl_History_All_Uper.ClientArea.Controls[_iSaved_Uper_Number])).ImageIpl = NowSavedImage_Uper;
                    uPnl_History_All_Uper.ClientArea.Controls[_iSaved_Uper_Number].Refresh();
                }
                else
                {
                    _strHistoryViewNameAll_Uper[_iAllHistoryViewNo_Uper] = _strHisImgName_Uper;
                    ((PictureBoxIpl)(uPnl_History_All_Uper.ClientArea.Controls[_iAllHistoryViewNo_Uper])).ImageIpl = NowSavedImage_Uper;
                    uPnl_History_All_Uper.ClientArea.Controls[_iAllHistoryViewNo_Uper].Refresh();

                    if (_iAllHistoryViewNo_Uper + 1 == 20) _iAllHistoryViewNo_Uper = 0;
                    else _iAllHistoryViewNo_Uper++;
                }

                Inspect_Run_Run_History_Grid_All_Uper();
            }
        }
        */

        


        //_iImgCount 를 _iHistoryViewNo에 넘져준다. 
        private void Inspect_Run_Run_Drawing_Result_To_History_All_Down()
        {
            if (uPnl_History_All_Down.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_All_Down;
                uPnl_History_All_Down.Invoke(del);
            }
            else
            {
                if (_strHisImgName_Down.Count == 0) return;
                if (string.IsNullOrEmpty(_strHisImgName_Down[_strHisImgName_Down.Count - 1]) == true) return;

                _strHistoryViewNameAll_Down[_iAllHistoryViewNo_Down] = _strHisImgName_Down[_strHisImgName_Down.Count - 1];
                ((PictureBoxIpl) (uPnl_History_All_Down.ClientArea.Controls[_iAllHistoryViewNo_Down])).ImageIpl =
                    NowSavedImage_Down;
                uPnl_History_All_Down.ClientArea.Controls[_iAllHistoryViewNo_Down].Refresh();

                Inspect_Run_Run_History_Grid_All_Down();

                if (_iAllHistoryViewNo_Down + 1 == 20) _iAllHistoryViewNo_Down = 0;
                else _iAllHistoryViewNo_Down++;
            }
        }

        private void Inspect_Run_Run_History_Grid_All_Down()
        {
            UltraGridRow row;

            if (uGrd_History_All_Down.DisplayLayout.Rows.Count < 20)
            {
                row = uGrd_History_All_Down.DisplayLayout.Bands[0].AddNew();
            }
            else
            {
                row = uGrd_History_All_Down.DisplayLayout.Rows[_iAllHistoryViewNo_Down];
            }

            for (int i = 0; i < uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count; i++)
            {
                string WriteMeasData =
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                if (WriteMeasData != "")
                {
                    string ColName = (i + 1).ToString("0");
                    row.Cells[ColName].Value = uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                }
            }
        }


        //_strHisImgName
        private void Inspect_Run_Run_Drawing_Result_To_History_NG_Uper()
        {
            if (uPnl_History_NG_Uper.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_NG_Uper;
                uPnl_History_NG_Uper.Invoke(del);
            }
            else
            {
                //20150327 WKB 301
                if (_strHisImgName_Down.Count == 0) return;
                if (string.IsNullOrEmpty(_strHisImgName_Down[_strHisImgName_Down.Count - 1]) == true) return;

                _strHistoryViewNameNG_Uper[_iAllHistoryViewNo_Uper] = _strHisImgName_Uper[_strHisImgName_Uper.Count - 1];
                

                //20150327 WKB 209
                //_strHistoryViewNameNG_Uper[_iNGHistoryViewNo_Uper] = _strHisImgName_Uper;

                ((PictureBoxIpl) (uPnl_History_NG_Uper.ClientArea.Controls[_iNGHistoryViewNo_Uper])).ImageIpl = NowSavedImage_Uper;
                uPnl_History_NG_Uper.ClientArea.Controls[_iNGHistoryViewNo_Uper].Refresh();

                Inspect_Run_Run_History_Grid_NG_Uper();

                if (_iNGHistoryViewNo_Uper + 1 == 20) _iNGHistoryViewNo_Uper = 0;
                else _iNGHistoryViewNo_Uper++;

                
            }
        }

        private void Inspect_Run_Run_History_Grid_NG_Uper()
        {
            UltraGridRow row;

            if (uGrd_History_NG_Uper.DisplayLayout.Rows.Count < 20)
            {
                row = uGrd_History_NG_Uper.DisplayLayout.Bands[0].AddNew();
            }
            else
            {
                row = uGrd_History_NG_Uper.DisplayLayout.Rows[_iNGHistoryViewNo_Uper];
            }

            for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
            {
                string WriteMeasData =
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                if (WriteMeasData != "")
                {
                    string ColName = (i + 1).ToString("0");
                    row.Cells[ColName].Value =
                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                }
            }
        }

        private void Inspect_Run_Run_Drawing_Result_To_History_NG_Down()
        {
            if (uPnl_History_NG_Down.InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_NG_Down;
                uPnl_History_NG_Down.Invoke(del);
            }
            else
            {
                _strHistoryViewNameNG_Down[_iNGHistoryViewNo_Down] = _strHisImgName_Down[_strHisImgName_Down.Count - 1];

                ((PictureBoxIpl)(uPnl_History_NG_Down.ClientArea.Controls[_iNGHistoryViewNo_Down])).ImageIpl = NowSavedImage_Down;
                uPnl_History_NG_Down.ClientArea.Controls[_iNGHistoryViewNo_Down].Refresh();

                Inspect_Run_Run_History_Grid_NG_Down();

                if (_iNGHistoryViewNo_Down + 1 == 20) _iNGHistoryViewNo_Down = 0;
                else _iNGHistoryViewNo_Down++;
            }
        }

        private void Inspect_Run_Run_History_Grid_NG_Down()
        {
            UltraGridRow row;

            if (uGrd_History_NG_Down.DisplayLayout.Rows.Count < 20)
            {
                row = uGrd_History_NG_Down.DisplayLayout.Bands[0].AddNew();
            }
            else
            {
                row = uGrd_History_NG_Down.DisplayLayout.Rows[_iNGHistoryViewNo_Down];
            }

            for (int i = 0; i < uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count; i++)
            {
                string WriteMeasData =
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                if (WriteMeasData != "")
                {
                    string ColName = (i + 1).ToString("0");
                    row.Cells[ColName].Value =
                        uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정"].Value.ToString();
                }
            }
        }


        List<string> _strHistoryViewNameAll_Uper = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        List<string> _strHistoryViewNameNG_Uper = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };

        List<string> _strHistoryViewNameAll_Down = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        List<string> _strHistoryViewNameNG_Down = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };

        //private bool Uper_Measure_Result = false;
        //private bool Down_Measure_Result = false;

        public int _iSaved_Uper_Number = -1;
        public int _iSaved_Down_Number = -1;

        private void Inspect_Result_Image_To_File_Uper()
        {
            Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            
            string imageSaveFolderName_Uper = Inspect_Set_FolderName_ImageFile(_dtInspDataSaveTime, "상부");
            string disText = string.Empty;
            if (Uper_Measure_Result == true) disText = "OK";
            else disText = "NG";

            string imageFileName_Uper = _iSavedTrigNumber.ToString("0000000000") + " " + disText + //_strSavedInspectResult_Uper +
                                   String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                                       _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                                       _dtInspDataSaveTime.Hour,
                                       _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                                       _dtInspDataSaveTime.Millisecond);

            //_strHisImgName_Uper = imageSaveFolderName_Uper + "\\" + imageFileName_Uper + ".jpg";
            _strHisImgName_Uper.Add(imageSaveFolderName_Uper + "\\" + imageFileName_Uper + ".jpg");
            string imageFilePath_Uper = imageSaveFolderName_Uper + "\\" + imageFileName_Uper + ".jpg";

            //Stopwatch imagesavewatch = new Stopwatch();
            //imagesavewatch.Start();
           //이미지에 검사결과를 기록하기 전에 복사해 놓은 이미지를 저장한다.
            NowSavedImage_Uper.SaveImage(imageFilePath_Uper);

            //imagesavewatch.Stop();
            //Trace.WriteLine("이미지 저장 시간 : "+imagesavewatch.ElapsedMilliseconds.ToString());
        }


        private void Inspect_Result_Image_To_File_Down()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            string imageSaveFolderName_Down = Inspect_Set_FolderName_ImageFile(_dtInspDataSaveTime, "하부");

            string disText = string.Empty;

            if (Down_Measure_Result == true) disText = "OK";
            else disText = "NG";

            string imageFileName_Down = _iSavedTrigNumber.ToString("0000000000") + " " + disText + //_strSavedInspectResult_Down +
                                   String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                                       _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                                       _dtInspDataSaveTime.Hour,
                                       _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                                       _dtInspDataSaveTime.Millisecond);

            //_strHisImgName_Down = imageSaveFolderName_Down + "\\" + imageFileName_Down + ".jpg";
            _strHisImgName_Down.Add(imageSaveFolderName_Down + "\\" + imageFileName_Down + ".jpg");
            string imageFilePath_Down = imageSaveFolderName_Down + "\\" + imageFileName_Down + ".jpg";

            //이미지에 검사결과를 기록하기 전에 복사해 놓은 이미지를 저장한다.
            NowSavedImage_Down.SaveImage(imageFilePath_Down);
        }

        /*
        private void Inspect_Run_Run_SaveImage_To_File()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            
            string imageSaveFolderName = Inspect_Set_FolderName_ImageFile(_dtInspDataSaveTime);

            //string SourceFileName = "NowGrabImage";
            //string SourceFilePath = imageSaveFolderName + "\\" + SourceFileName + ".jpg";
            string SourceFilePath = "M:\\NowGrabImage.jpg";
            string imageFileName = _iSavedTrigNumber.ToString("0000000000") + " "
                                   + _iSavedGap_Number.ToString("00") + " " + _strSavedInspectResult +
                                   String.Format(" {0:00}년{1:00}월{2:00}일 {3:00}시{4:00}분{5:00}.{6:000}초",
                                       _dtInspDataSaveTime.Year, _dtInspDataSaveTime.Month, _dtInspDataSaveTime.Day,
                                       _dtInspDataSaveTime.Hour,
                                       _dtInspDataSaveTime.Minute, _dtInspDataSaveTime.Second,
                                       _dtInspDataSaveTime.Millisecond);

            _strHisImgName = imageSaveFolderName + "\\" + imageFileName + ".jpg";
            string imageFilePath = imageSaveFolderName + "\\" + imageFileName + ".jpg";

           //이미지에 검사결과를 기록하기 전에 복사해 놓은 이미지를 저장한다.
            NowSavedImage.SaveImage(imageFilePath);
            //MoveFile(SourceFilePath, imageFilePath);
        }
        */
        public static void MoveFile(string source, string target)
        {
            FileInfo file = new FileInfo(source);
            if (file.Exists)
            {
                file.CopyTo(target, true);
            }
        }
        private string Inspect_Set_FolderName_ImageFile(DateTime checkTime, string partName)
        {
            var fileSystem = new Control_Files();

            string folderName = _NowImageFolderSavePath_Uper + "\\" + partName +
                                String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            folderName = folderName +
                         String.Format("\\{0:00}년{1:00}월{2:00}일", checkTime.Year, checkTime.Month, checkTime.Day);


            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);

            return folderName;
        }

        private string Inspect_Set_FolderName_ImageFile(DateTime checkTime)
        {
            var fileSystem = new Control_Files();

            string folderName = _NowImageFolderSavePath_Uper + "\\Gap" +
                                String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            folderName = folderName +
                         String.Format("\\{0:00}년{1:00}월{2:00}일", checkTime.Year, checkTime.Month, checkTime.Day);


            //해당 경로에 폴더가 있는지 검사해서 없으면 생성한다.
            //Param1 : 검사할 디렉토리 경로
            fileSystem.File_IO_Folder_Check_Or_Make(folderName);

            return folderName;
        }

        public void Inspect_Run_Run_Set_Reg_NowGapNo(int gapNo)
        {
            SetReg(LamiSystem.RegPathGapStatus, "NowGapNo", gapNo.ToString());
        }
        private static  string Run_Mode = "Auto";



        /*
        
        public void Inspect_Run_Run_Drawing_Result_Uper()
        {
            try
            {
                
                Inspect_Run_Run_IplBoxImage_Loading_Uper(_nowIplImage_Uper);
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();

                Inspection_Data_List inspect_Data = Inspect_Run_Run_FindData_Inspecting_Uper();
                Measure_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Uper));
                Measure_Display_Thread_Uper.Start(inspect_Data);

                FormDlgInsp_Inspection_Excel_Data_Write_Uper();
                Graph_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Uper));
                Graph_Display_Thread_Uper.Start(inspect_Data);

                Inspect_Save_Data_Copy_Uper();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                Result_Display_Thread_Uper = new Thread(Inspect_Run_Run_Inspect_Result_Display_Uper);
                Result_Display_Thread_Uper.Start();
                Inspect_Result_Data_Save_Uper();
                

                /*
                Stopwatch testModule = new Stopwatch();

                testModule.Reset(); testModule.Start();
                Inspect_Run_Run_IplBoxImage_Loading_Uper(_nowIplImage_Uper);
                testModule.Stop(); Trace.WriteLine("1 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
                testModule.Stop(); Trace.WriteLine("2 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                Inspection_Data_List inspect_Data = Inspect_Run_Run_FindData_Inspecting_Uper();
                Measure_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Uper));
                Measure_Display_Thread_Uper.Start(inspect_Data);
                testModule.Stop(); Trace.WriteLine("3 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                FormDlgInsp_Inspection_Excel_Data_Write_Uper();
                testModule.Stop(); Trace.WriteLine("4 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                Graph_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Uper));
                Graph_Display_Thread_Uper.Start(inspect_Data);
                //Inspect_Run_Run_Measure_Graph_Uper();
                testModule.Stop(); Trace.WriteLine("5 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                Inspect_Save_Data_Copy_Uper();
                testModule.Stop(); Trace.WriteLine("6 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();
                testModule.Stop(); Trace.WriteLine("7 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                Result_Display_Thread_Uper = new Thread(Inspect_Run_Run_Inspect_Result_Display_Uper);
                Result_Display_Thread_Uper.Start();
                testModule.Stop(); Trace.WriteLine("8 : " + testModule.ElapsedMilliseconds.ToString());

                testModule.Reset(); testModule.Start();
                Inspect_Result_Data_Save_Uper();
                testModule.Stop(); Trace.WriteLine("9 : " + testModule.ElapsedMilliseconds.ToString());
               
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

         */

        private Thread Measure_Display_Thread_Uper;
        private Thread Graph_Display_Thread_Uper;

        //20150327 WKB 301
        public void Inspect_Run_Run_Drawing_Result_Uper()
        {
            try
            {
                Inspect_Run_Run_IplBoxImage_Loading_Uper(_nowIplImage_Uper);
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
               
                //Inspection_Data_List inspect_Data = Inspect_Run_Run_FindData_Inspecting_Uper();
                
                Inspection_Grid_Data[] StructData = Inspect_Run_Run_FindData_Inspecting_Uper(); 

                Measure_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Uper));
                Measure_Display_Thread_Uper.Start(StructData);

                FormDlgInsp_Inspection_Excel_Data_Write_Uper();

                Graph_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Uper));
                Graph_Display_Thread_Uper.Start(StructData);

                Inspect_Save_Data_Copy_Uper();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                //Result_Display_Thread_Uper = new Thread(Inspect_Run_Run_Inspect_Result_Display_Uper);
                //Result_Display_Thread_Uper.Start();

                Result_Listing_Thread_Uper = new Thread(Inspect_Result_Data_Save_Uper);
                Result_Listing_Thread_Uper.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        //20150725 WKB
        /*
        public void Inspect_Run_Run_Drawing_Result_Uper()
        {
            try
            {
                Inspect_Run_Run_IplBoxImage_Loading_Uper(_nowIplImage_Uper);
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();
               
                //Inspection_Data_List inspect_Data = Inspect_Run_Run_FindData_Inspecting_Uper();
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();
                Inspection_Data_List inspect_Data = Inspect_Run_Run_FindData_Inspecting_Uper(ref StructData); 

                Measure_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Uper));
                Measure_Display_Thread_Uper.Start(inspect_Data);

                FormDlgInsp_Inspection_Excel_Data_Write_Uper();

//                 Stopwatch downExcel = new Stopwatch();
//                 downExcel.Reset();
//                 downExcel.Start();
//                 FormDlgInsp_Inspection_Excel_Data_Write_Uper();
//                 downExcel.Stop();
//                 Trace.WriteLine("Uper Write : " + downExcel.ElapsedMilliseconds.ToString());

                Graph_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Uper));
                Graph_Display_Thread_Uper.Start(inspect_Data);

                Inspect_Save_Data_Copy_Uper();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                Result_Display_Thread_Uper = new Thread(Inspect_Run_Run_Inspect_Result_Display_Uper);
                Result_Display_Thread_Uper.Start();

                Result_Listing_Thread_Uper = new Thread(Inspect_Result_Data_Save_Uper);
                Result_Listing_Thread_Uper.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */

        //20150327 WKB 209
        /*
           public void Inspect_Run_Run_Drawing_Result_Uper()
        {
            try
            {
                Inspect_Run_Run_IplBoxImage_Loading_Uper(_nowIplImage_Uper);
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();

                Inspection_Data_List inspect_Data = Inspect_Run_Run_FindData_Inspecting_Uper(); 

                Measure_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Uper));
                Measure_Display_Thread_Uper.Start(inspect_Data);

                FormDlgInsp_Inspection_Excel_Data_Write_Uper();

//                 Stopwatch downExcel = new Stopwatch();
//                 downExcel.Reset();
//                 downExcel.Start();
//                 FormDlgInsp_Inspection_Excel_Data_Write_Uper();
//                 downExcel.Stop();
//                 Trace.WriteLine("Uper Write : " + downExcel.ElapsedMilliseconds.ToString());

                Graph_Display_Thread_Uper = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Uper));
                Graph_Display_Thread_Uper.Start(inspect_Data);

                Inspect_Save_Data_Copy_Uper();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                Result_Display_Thread_Uper = new Thread(Inspect_Run_Run_Inspect_Result_Display_Uper);
                Result_Display_Thread_Uper.Start();

                Inspect_Result_Data_Save_Uper();
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */

        private Thread Result_Listing_Thread_Uper;
        private Thread Result_Listing_Thread_Down;

        private Thread Result_Display_Thread_Uper;
        private Thread Result_Display_Thread_Down;

        private Thread Measure_Display_Thread_Down;
        private Thread Graph_Display_Thread_Down;

        //20150327 WKB 2301
        public void Inspect_Run_Run_Drawing_Result_Down()
        {
            try
            {
                //로딩한 이미지를 표시해준다.
                Inspect_Run_Run_IplBoxImage_Loading_Down(_nowIplImage_Down);
                Inspect_Run_Run_ROI_CenterPoint_Find_Down();

                Inspection_Grid_Data[] StructData = Inspect_Run_Run_FindData_Inspecting_Down();
                Measure_Display_Thread_Down = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Down));
                Measure_Display_Thread_Down.Start(StructData);

                FormDlgInsp_Inspection_Excel_Data_Write_Down();

                Graph_Display_Thread_Down = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Down));
                Graph_Display_Thread_Down.Start(StructData);

                Inspect_Save_Data_Copy_Down();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                Result_Display_Thread_Down = new Thread(Inspect_Run_Run_Inspect_Result_Display_Down);
                Result_Display_Thread_Down.Start();

                Result_Listing_Thread_Down = new Thread(Inspect_Result_Data_Save_Down);
                Result_Listing_Thread_Down.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }


        //20150327 WKB 209
        /*
        public void Inspect_Run_Run_Drawing_Result_Down()
        {
            try
            {
                //로딩한 이미지를 표시해준다.
                Inspect_Run_Run_IplBoxImage_Loading_Down(_nowIplImage_Down);
                Inspect_Run_Run_ROI_CenterPoint_Find_Down();
                
                Inspection_Data_List inspect_Data_Down = Inspect_Run_Run_FindData_Inspecting_Down();
                Measure_Display_Thread_Down = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_FindData_Display_Down));
                Measure_Display_Thread_Down.Start(inspect_Data_Down);

                FormDlgInsp_Inspection_Excel_Data_Write_Down();

//                 Stopwatch downExcel = new Stopwatch();
//                 downExcel.Reset();
//                 downExcel.Start();
//                 FormDlgInsp_Inspection_Excel_Data_Write_Down();
//                 downExcel.Stop();
//                 Trace.WriteLine("Down Write : "+downExcel.ElapsedMilliseconds.ToString());


                Graph_Display_Thread_Down = new Thread(new ParameterizedThreadStart(Inspect_Run_Run_Measure_Graph_Down));
                Graph_Display_Thread_Down.Start(inspect_Data_Down);

                Inspect_Save_Data_Copy_Down();
                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                Result_Display_Thread_Down = new Thread(Inspect_Run_Run_Inspect_Result_Display_Down);
                Result_Display_Thread_Down.Start();
                Inspect_Result_Data_Save_Down();
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }        
        */

        /*
        public void Inspect_Run_Run_Drawing_Result_Uper()
        {
            try
            {
                Inspect_Run_Run_ROI_CenterPoint_Find_Uper();

                Inspect_Run_Run_ROI_CenterPoint_Find_Down();

                Inspect_Run_Run_FindData_Inspection_Uper();

                Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);

                FormDlgInsp_Inspection_Excel_Data_Write_Uper();

                Inspect_Run_Run_FindData_Inspection_Down();

                Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Down, uGrd_Inspect_Measure_Down);

                FormDlgInsp_Inspection_Excel_Data_Write_Down();

                Inspect_Run_Run_Measure_Graph_Uper();

                Inspect_Run_Run_Measure_Graph_Down();
                
                /////////////////////////////////////
                //상부 인스펙션 필요 검출 포인트 찾기 실패
                /////////////////////////////////////
                Inspect_None_Flag_Uper = false;
                if (Inspect_None_Flag_Uper == true)
                {
                    _strSavedNGOK[0] = "NG";
                    _strSavedNGOK[1] = "NG";
                    _strSavedNGOK[2] = "NG";

                    Inspect_Save_Data_Copy_Uper();

                    _iPLC_Result_Code = 2;
                    //PLC_WriteData_Threading();

                    Result_Display_Thread = new Thread(Inspect_Run_Run_None_Point_Search_Display);
                    Result_Display_Thread.Start();

                    Inspect_Result_Data_Save_Uper();

                    _CycleCompleteFlag_Uper = true;
                    Thread.Sleep(3);
                    return;
                }

                Inspect_Save_Data_Copy_Uper();

                strResultNgOk[2] = "OK";
                //검출이 정상적으로 진행되었을 
                if (strResultNgOk[2] == "OK")
                {
                    //정상일때는 데이터를 송부하지 않기로 협의됨
                    //2014.07.24 유영민 사원
                    //_iPLC_Result_Code = 0;
                    //PLC_WriteData_Threading();
                }

                //스펙에서 에러일때
                else if (strResultNgOk[2] == "NG")
                {
                    _savedValue_Uper = dResultValue_Uper;
                    _savedValue_Down = dResultValue_Down;

                    _iPLC_Result_Code = 1;
                    //PLC_WriteData_Threading();
                    //Inspect_Run_Run_Make_Fail_Check();
                }

                System.GC.Collect(0, GCCollectionMode.Forced);
                System.GC.WaitForFullGCComplete();

                Result_Display_Thread = new Thread(Inspect_Run_Run_Inspect_Result_Display);
                Result_Display_Thread.Start();

                if (Run_Mode != "Manual") Inspect_Result_Data_Save_Uper();
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 4 end " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 3 end " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 2 end " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 1 end " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 1 start " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 2 start " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 3 start " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 4 start " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 5 start " + MethodBase.GetCurrentMethod().Name);
//             Trace.WriteLine(" : Method Name : 5 end " + MethodBase.GetCurrentMethod().Name);
            
            
        }

        */
        /*
        
        public void Inspect_Run_Run_Measure_Graph_Uper()
        {
            try
            {
                ChartRemovedChart_Uper = -1;

                for (int i = 0; i < uGrd_Inspect_Measure_Uper.Rows.Count; i++)
                {
                    //그래프 테이블 데이터 //Graph_No_Now
                    string GrdData = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString();
                    if (GrdData == "") continue;

                    string UsedFlag = LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 9];
                    if (UsedFlag == "False") continue;

                    double MeaValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value.ToString());

                    int GraphRow_Count = -1;
                    int GraphNum_Count = -1;
                    for (int j = 0; j < 40; j++)
                    {
                        string RcpData = Graph_No_Now_Uper[j];
                        if (GrdData == RcpData)
                        {
                            GraphNum_Count = j / 4;
                            GraphRow_Count = j % 4;
                        }
                    }
                    Inspect_MeasureData_Graph_Dis_Uper(GraphNum_Count, GraphRow_Count, MeaValue);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        */
        public void Inspect_Run_Run_Measure_Graph_Uper(object inspect_Data)
        {
            try
            {
                Inspection_Grid_Data[] grid_Struct = (Inspection_Grid_Data[]) inspect_Data;
                ChartRemovedChart_Uper = -1;
                //int Mesaure_Col_Count = 13;
                //int Measure_Rows = inspectData.Inspection_Data.Count/Mesaure_Col_Count;
                for (int i = 0; i < grid_Struct.Count(); i++)
                {
                    //그래프 테이블 데이터 //Graph_No_Now
                    string GrdData = grid_Struct[i].readName;
                    if (GrdData == "") continue;

                    //string UsedFlag = LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 9];
                    //if (UsedFlag == "False") continue;

                    double MeaValue = double.Parse(grid_Struct[i].dResultValueStr);

                    int GraphRow_Count = -1;
                    int GraphNum_Count = -1;
                    for (int j = 0; j < 40; j++)
                    {
                        string RcpData = Graph_No_Now_Uper[j];
                        if (GrdData == RcpData)
                        {
                            GraphNum_Count = j / 4;
                            GraphRow_Count = j % 4;
                            break;
                        }
                    }
                    Inspect_MeasureData_Graph_Dis_Uper(GraphNum_Count, GraphRow_Count, MeaValue);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        private int ChartRemovedChart_Uper = -1;
        public void Inspect_MeasureData_Graph_Dis_Uper(int Graph_Count, int Row_Count, double MeaValue)
        {
            if (Row_Count == 0)
            {
                Uper_MeasureTables[Graph_Count].Columns.RemoveAt(0);

                var measureColumn = new DataColumn();
                measureColumn.DataType = Type.GetType("System.Double");
                measureColumn.AllowDBNull = false;
                measureColumn.DefaultValue = 0d;
                Uper_MeasureTables[Graph_Count].Columns.Add(measureColumn);
            }
                
            Uper_MeasureTables[Graph_Count].Rows[Row_Count][99] = MeaValue;
        }



        public void Inspect_Run_Run_Measure_Graph_Down(object inspect_Data)
        {
            try
            {
                Inspection_Grid_Data[] grid_Struct = (Inspection_Grid_Data[])inspect_Data;
                ChartRemovedChart_Down = -1;
                //int Mesaure_Col_Count = 13;
                //int Measure_Rows = inspectData.Inspection_Data.Count / Mesaure_Col_Count;
                for (int i = 0; i < grid_Struct.Count(); i++)
                {
                    //그래프 테이블 데이터 //Graph_No_Now
                    string GrdData = grid_Struct[i].readName;
                    if (GrdData == "") continue;

                    //string UsedFlag = LamiSystem.StrLstRcpConGridData_Down[i * 11 + 9];
                    //if (UsedFlag == "False") continue;

                    double MeaValue = double.Parse(grid_Struct[i].dResultValueStr);

                    int GraphRow_Count = -1;
                    int GraphNum_Count = -1;
                    for (int j = 0; j < 40; j++)
                    {
                        string RcpData = Graph_No_Now_Down[j];
                        if (GrdData == RcpData)
                        {
                            GraphNum_Count = j / 4;
                            GraphRow_Count = j % 4;
                        }
                    }
                    Inspect_MeasureData_Graph_Dis_Down(GraphNum_Count, GraphRow_Count, MeaValue);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

       
        
        private int ChartRemovedChart_Down = -1;
        public void Inspect_MeasureData_Graph_Dis_Down(int Graph_Count, int Row_Count, double MeaValue)
        {
            if (Row_Count == 0)
            {
                Down_MeasureTables[Graph_Count].Columns.RemoveAt(0);

                var measureColumn = new DataColumn();
                measureColumn.DataType = Type.GetType("System.Double");
                measureColumn.AllowDBNull = false;
                measureColumn.DefaultValue = 0d;
                Down_MeasureTables[Graph_Count].Columns.Add(measureColumn);
            }
            Down_MeasureTables[Graph_Count].Rows[Row_Count][99] = MeaValue;
        }
        /*
        */
        /*
         public void Inspect_Run_Run_Drawing_Result()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_Run_Run_ROI_CenterPoint_Find();
            
            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();

            Inspect_Run_Run_FindData_Inspection();

            if (Inspect_None_Flag == true)
            {
                _strSavedNGOK[0] = "NG";
                _strSavedNGOK[1] = "NG";
                _strSavedNGOK[2] = "NG";
                dResultValue_Uper = 0.0;
                dResultValue_Down = 0.0;
                _savedValue_Left = 0.0;
                _savedValue_Right = 0.0;

                Inspect_Run_Run_Result_Write_GapTotal();

                FormDlgInsp_Inspection_Save_Data_Copy();



                _iPLC_Result_Code = 2;
                PLC_WriteData_Threading();

                //Tack_Time_Watch_Gap.Stop();
                //Inspect_Run_Run_TackTime_TextBox_Reflash();


                //Inspect_Run_Run_Inspect_None_Display();
                Result_Display_Thread = new Thread(Inspect_Run_Run_Inspect_None_Display);
                Result_Display_Thread.Start();

                Inspect_Run_Run_Data_Save();

                _CycleCompleteFlag_Uper = true;
                Thread.Sleep(3);
                return;
            }

            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();

            //갭 토달 파일에 정보를 기록한다.
            Inspect_Run_Run_Result_Write_GapTotal();
            
            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();

            //쓰레드에서 아래 두개의 멤버에 접근하기 전에 메인 쓰레드에서
            //이미 값이 변경될 가능성이 있어 다른 메버에 복사하여 복사된
            //멤버의 값을 사용하여 저장 데이터를 만든다.
            FormDlgInsp_Inspection_Save_Data_Copy();
            
            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();

            strResultNgOk[2] = "OK";
            //검출이 정상적으로 진행되었을 
            if (strResultNgOk[2] == "OK")
            {
                //정상일때는 데이터를 송부하지 않기로 협의됨
                //2014.07.24 유영민 사원
                _iPLC_Result_Code = 0;
                PLC_WriteData_Threading();
            }

            //스펙에서 에러일때
            else if (strResultNgOk[2] == "NG")
            {
                _savedValue_Left = dResultValue_Uper;
                _savedValue_Right = dResultValue_Down;

                _iPLC_Result_Code = 1;
                PLC_WriteData_Threading();
                
                //_savedValue_Left = dResultValueLeft;
                //_savedValue_Right = dResultValueRight;
                Inspect_Run_Run_Make_Fail_Check();
            }

            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();

            System.GC.Collect(0, GCCollectionMode.Forced);
            System.GC.WaitForFullGCComplete();

            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();
            
            //디스플레이에 만은 시간이 소요되어 쓰레드 작업을 
            //진행하도록 한다.
            //Inspect_Run_Run_Inspect_Result_Display();
            Result_Display_Thread = new Thread(Inspect_Run_Run_Inspect_Result_Display);
            Result_Display_Thread.Start();

            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();

            if (Run_Mode != "Manual")
                Inspect_Run_Run_Data_Save();

            //Tack_Time_Watch_Gap.Stop();
            //Inspect_Run_Run_TackTime_TextBox_Reflash();
        }
        */
        private Thread Status_Display_Thread;
        public void Inspect_Initionalize()
        {
            Inspect_Ready_Run_MyArrowPen_Make();

            //RectListImageZone 리스트를 작성하는 함수
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check_Uper();
            Inspect_Ready_Ready_RecipeBoxZone_To_ImageZone_Check_Down();

            //이미지 구역을 박스 구역으로 바꾸는 함수.
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check_Uper();
            Inspect_Ready_Ready_ImageZone_To_InspectBoxZone_Check_Down();

            //시스템의 설정 값을 적용한는 함수.
            Inspect_Ready_Run_System_Data_Load();

            //RectListImageZone 리스트를 이용하는 함수
            Inspect_Ready_Run_RecipeGrid_Data_Load_Uper();
            Inspect_Ready_Run_RecipeGrid_Data_Load_Down();

            //측정값 계산에 사용되는 켈값을 로딩한다.
            Inspect_Offset_Load_To_System();
        }
        private void Inspect_Ready_Run_MyArrowPen_Make()
        {
            cusCap = new AdjustableArrowCap(5, 5, false);
            myArrowPen.StartCap = LineCap.Custom;
            myArrowPen.EndCap = LineCap.Custom;
            myArrowPen.CustomStartCap = cusCap;
        }

        public void Inspect_Ready_Run_System_Data_Load()
        {
            ////Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            string tmp1 = LamiSystem.StrListSysConTitle[10];
            string tmp2 = LamiSystem.StrListVisConTitle[7];

            _NowImageFolderSavePath_Uper = LamiSystem.StrListSysConData[2];
            _NowExcelFolderSavePath_Uper = LamiSystem.StrListSysConData[5];
            _passSaveImage_Uper = LamiSystem.StrListSysConData[0];
            _failSaveImage_Uper = LamiSystem.StrListSysConData[1];
            _passSaveData_Uper = LamiSystem.StrListSysConData[3];
            _failSaveData_Uper = LamiSystem.StrListSysConData[4];

            _iEdgeParam1_Uper = int.Parse(LamiSystem.StrListSysConData[11]);
            _iEdgeParam2_Uper = int.Parse(LamiSystem.StrListSysConData[12]);
            _iEdgeParam3_Uper = int.Parse(LamiSystem.StrListSysConData[13]);

            _iLineParam1_Uper = int.Parse(LamiSystem.StrListSysConData[14]);
            _iLineParam2_Uper = int.Parse(LamiSystem.StrListSysConData[15]);
            _iLineParam3_Uper = int.Parse(LamiSystem.StrListSysConData[16]);

            _iGrabImageGaro_Uper = int.Parse(LamiSystem.StrListSysConData[17]);
            _iGrabImageSero_Uper = int.Parse(LamiSystem.StrListSysConData[18]);

            _dCalibration_GaRo_Uper = double.Parse(LamiSystem.StrListVisConData[2]);
            _dCalibration_SeRo_Uper = double.Parse(LamiSystem.StrListVisConData[5]);

            _NowImageFolderSavePath_Down = LamiSystem.StrListSysConData[21];
            _NowExcelFolderSavePath_Down = LamiSystem.StrListSysConData[24];
            _passSaveImage_Down = LamiSystem.StrListSysConData[19];
            _failSaveImage_Down = LamiSystem.StrListSysConData[20];
            _passSaveData_Down = LamiSystem.StrListSysConData[22];
            _failSaveData_Down = LamiSystem.StrListSysConData[23];

            _dCalibration_GaRo_Down = double.Parse(LamiSystem.StrListVisConData[8]);
            _dCalibration_SeRo_Down = double.Parse(LamiSystem.StrListVisConData[11]);

            _iEdgeParam1_Down = int.Parse(LamiSystem.StrListSysConData[30]);
            _iEdgeParam2_Down = int.Parse(LamiSystem.StrListSysConData[31]);
            _iEdgeParam3_Down = int.Parse(LamiSystem.StrListSysConData[32]);

            _iLineParam1_Down = int.Parse(LamiSystem.StrListSysConData[33]);
            _iLineParam2_Down = int.Parse(LamiSystem.StrListSysConData[34]);
            _iLineParam3_Down = int.Parse(LamiSystem.StrListSysConData[35]);
            
            _iGrabImageGaro_Down = int.Parse(LamiSystem.StrListSysConData[36]);
            _iGrabImageSero_Down = int.Parse(LamiSystem.StrListSysConData[37]);

            _iTriggerDeleay_Uper = int.Parse(LamiSystem.StrListSysConData[44]);
            _iTriggerDeleay_Down = int.Parse(LamiSystem.StrListSysConData[45]);
        }

        private int _iTriggerDeleay_Uper = 500;
        private int _iTriggerDeleay_Down = 500;

        //20150304 WKB 209
        public void Inspect_Ready_Run_RecipeGrid_Data_Load_Uper()
        {
            Inspect_Ready_Run_RecipeGrid_ListArray_Clear_Uper();
            //for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
            for (int i = 0; i < LamiSystem.StrLstRcpConInspData_Uper.Count / inspItemCount; i++)
            {
                iLstNowRowNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * inspItemCount)])); //Row

                int tmpROI = int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i*inspItemCount)]);
                iLstNowRoiNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)])); //ROI
                sLstNowTypNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[6 + (i * inspItemCount)]); //Type

                //iLstNowSeqNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[7 + (i * inspItemCount)])); //Seq No
                string SeqNo = (LamiSystem.StrLstRcpConInspData_Uper[7 + (i*inspItemCount)] == "1차") ? "1" : "2";
                iLstNowSeqNo_Uper.Add(int.Parse(SeqNo)); //Seq No

                iLstNowSidNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[8 + (i * inspItemCount)])); //Side No
                sLstNowPolNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[9 + (i * inspItemCount)]); //극성
                iLstNowDivNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[10 + (i * inspItemCount)])); //분할
                sLstNowDisNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[11 + (i * inspItemCount)]); //표시
                iLstNowLgtNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[12 + (i * inspItemCount)])); //밝기
                iLstNowLstNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[13 + (i * inspItemCount)])); //리스트 번호

                iCenterPointToROI_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
            }
        }
        //20150304 WKB 208
        /*
        public void Inspect_Ready_Run_RecipeGrid_Data_Load_Uper()
        {
            Inspect_Ready_Run_RecipeGrid_ListArray_Clear_Uper();
            for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
            //for (int i = 0; i < LamiSystem.StrLstRcpConInspData_Uper.Count / inspItemCount; i++)
            {
                iLstNowRowNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * inspItemCount)])); //Row
                iLstNowRoiNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)])); //ROI
                sLstNowTypNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[6 + (i * inspItemCount)]); //Type

                //iLstNowSeqNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[7 + (i * inspItemCount)])); //Seq No
                string SeqNo = (LamiSystem.StrLstRcpConInspData_Uper[7 + (i*inspItemCount)] == "1차") ? "1" : "2";
                iLstNowSeqNo_Uper.Add(int.Parse(SeqNo)); //Seq No

                iLstNowSidNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[8 + (i * inspItemCount)])); //Side No
                sLstNowPolNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[9 + (i * inspItemCount)]); //극성
                iLstNowDivNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[10 + (i * inspItemCount)])); //분할
                sLstNowDisNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[11 + (i * inspItemCount)]); //표시
                iLstNowLgtNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[12 + (i * inspItemCount)])); //밝기
                iLstNowLstNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[13 + (i * inspItemCount)])); //리스트 번호

                iCenterPointToROI_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
            }
        }         
        */
        private void Inspect_Ready_Run_RecipeGrid_ListArray_Clear_Uper()
        {
            iLstNowRowNo_Uper.Clear();
            //.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * inspItemCount)])); //Row
            iLstNowRoiNo_Uper.Clear();//.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)])); //ROI
            sLstNowTypNo_Uper.Clear();//.Add(LamiSystem.StrLstRcpConInspData_Uper[6 + (i * inspItemCount)]); //Type
            iLstNowSeqNo_Uper.Clear();
            //.Add(int.Parse(SeqNo)); //Seq No

            iLstNowSidNo_Uper.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[8 + (i * inspItemCount)])); //Side No
            sLstNowPolNo_Uper.Clear(); //Add(LamiSystem.StrLstRcpConInspData_Uper[9 + (i * inspItemCount)]); //극성
            iLstNowDivNo_Uper.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[10 + (i * inspItemCount)])); //분할
            sLstNowDisNo_Uper.Clear(); //Add(LamiSystem.StrLstRcpConInspData_Uper[11 + (i * inspItemCount)]); //표시
            iLstNowLgtNo_Uper.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[12 + (i * inspItemCount)])); //밝기
            iLstNowLstNo_Uper.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[13 + (i * inspItemCount)])); //리스트 번호

            iCenterPointToROI_Uper.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
        }
        public void Inspect_Ready_Run_RecipeGrid_Data_Load_Down()
        {
            Inspect_Ready_Run_RecipeGrid_ListArray_Clear_Down();

            for (int i = 0; i < LamiSystem.RectListImageZone_Down.Count; i++)
            {
                iLstNowRowNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[4 + (i * inspItemCount)])); //Row
                iLstNowRoiNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[5 + (i * inspItemCount)])); //ROI
                sLstNowTypNo_Down.Add(LamiSystem.StrLstRcpConInspData_Down[6 + (i * inspItemCount)]); //Type
                
                //iLstNowSeqNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[7 + (i * inspItemCount)])); //Seq No
                string SeqNo = (LamiSystem.StrLstRcpConInspData_Down[7 + (i * inspItemCount)] == "1차") ? "1" : "2";
                iLstNowSeqNo_Down.Add(int.Parse(SeqNo)); //Seq No

                iLstNowSidNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[8 + (i * inspItemCount)])); //Side No
                sLstNowPolNo_Down.Add(LamiSystem.StrLstRcpConInspData_Down[9 + (i * inspItemCount)]); //극성
                iLstNowDivNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[10 + (i * inspItemCount)])); //분할
                sLstNowDisNo_Down.Add(LamiSystem.StrLstRcpConInspData_Down[11 + (i * inspItemCount)]); //표시
                iLstNowLgtNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[12 + (i * inspItemCount)])); //밝기
                iLstNowLstNo_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[13 + (i * inspItemCount)])); //리스트 번호
                
                iCenterPointToROI_Down.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Down[5 + (i * inspItemCount)]));
            }
        }
        private void Inspect_Ready_Run_RecipeGrid_ListArray_Clear_Down()
        {
            iLstNowRowNo_Down.Clear();
            iLstNowRoiNo_Down.Clear();//.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)])); //ROI
            sLstNowTypNo_Down.Clear();//.Add(LamiSystem.StrLstRcpConInspData_Uper[6 + (i * inspItemCount)]); //Type
            iLstNowSeqNo_Down.Clear();
            iLstNowSidNo_Down.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[8 + (i * inspItemCount)])); //Side No
            sLstNowPolNo_Down.Clear(); //Add(LamiSystem.StrLstRcpConInspData_Uper[9 + (i * inspItemCount)]); //극성
            iLstNowDivNo_Down.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[10 + (i * inspItemCount)])); //분할
            sLstNowDisNo_Down.Clear(); //Add(LamiSystem.StrLstRcpConInspData_Uper[11 + (i * inspItemCount)]); //표시
            iLstNowLgtNo_Down.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[12 + (i * inspItemCount)])); //밝기
            iLstNowLstNo_Down.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[13 + (i * inspItemCount)])); //리스트 번호
            iCenterPointToROI_Down.Clear(); //Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
        }
        /*
        private void Inspect_Ready_Run_RecipeGrid_Data_Load_Uper()
        {
            ////Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            string tmp1 = LamiSystem.StrListSysConTitle[10];
            string tmp2 = LamiSystem.StrListVisConTitle_Gap[7];

            NowImageFolderSavePath = LamiSystem.StrListSysConData[2];
            NowExcelFolderSavePath = LamiSystem.StrListSysConData[5];
            //_strSavedInspectResult = _strSavedNGOK[2];
            passSaveImage = LamiSystem.StrListSysConData[0];
            failSaveImage = LamiSystem.StrListSysConData[1];
            passSaveData = LamiSystem.StrListSysConData[3];
            failSaveData = LamiSystem.StrListSysConData[4];

            pntCenterMarkInspBox.X = Inspect_Main01_IplBox.Width / 2;
            pntCenterMarkInspBox.Y = Inspect_Main01_IplBox.Height / 2;

            _dCalibration_GaRo = double.Parse(LamiSystem.StrListVisConData_Gap[0]);
            _dCalibration_SeRo = double.Parse(LamiSystem.StrListVisConData_Gap[1]);

            _iEdgeParam1 = int.Parse(LamiSystem.StrListSysConData[11]);
            _iEdgeParam2 = int.Parse(LamiSystem.StrListSysConData[12]);
            _iEdgeParam3 = int.Parse(LamiSystem.StrListSysConData[13]);

            _iLineParam1 = int.Parse(LamiSystem.StrListSysConData[14]);
            _iLineParam2 = int.Parse(LamiSystem.StrListSysConData[15]);
            _iLineParam3 = int.Parse(LamiSystem.StrListSysConData[16]);

            _iGrabImageGaro = int.Parse(LamiSystem.StrListSysConData[17]);
            _iGrabImageSero = int.Parse(LamiSystem.StrListSysConData[18]);

            Threshold_01[0] = int.Parse(LamiSystem.StrListSysConData[11]);
            Threshold_01[1] = int.Parse(LamiSystem.StrListSysConData[12]);

            for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
            {
                iLstNowRowNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[4 + (i * inspItemCount)])); //Row
                iLstNowRoiNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)])); //ROI
                sLstNowTypNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[6 + (i * inspItemCount)]); //Type
                iLstNowSeqNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[7 + (i * inspItemCount)])); //Seq No
                iLstNowSidNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[8 + (i * inspItemCount)])); //Side No
                sLstNowPolNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[9 + (i * inspItemCount)]); //극성
                iLstNowDivNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[10 + (i * inspItemCount)])); //분할
                sLstNowDisNo_Uper.Add(LamiSystem.StrLstRcpConInspData_Uper[11 + (i * inspItemCount)]); //표시
                iLstNowLgtNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[12 + (i * inspItemCount)])); //밝기
                iLstNowLstNo_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[13 + (i * inspItemCount)])); //리스트 번호
                if (LamiSystem.StrLstRcpConInspData_Uper[6 + (i * inspItemCount)] == "거리")
                {
                    iCenterPointToROI_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
                }
                else
                {
                    iCenterPointToROI_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
                    iCenterPointToROI_Uper.Add(int.Parse(LamiSystem.StrLstRcpConInspData_Uper[5 + (i * inspItemCount)]));
                }
            }
        }
        */


        List<double> Cal_Value_Uper = new List<double>();
        List<double> Cal_Value_Down = new List<double>(); 
        public void Inspect_Offset_Load_To_System()
        {
            _iAllHistoryViewNo_Uper = 0;
            _iNGHistoryViewNo_Uper = 0;
            _iAllHistoryViewNo_Down = 0;
            _iNGHistoryViewNo_Down = 0;

            Cal_Garo_Uper = double.Parse(LamiSystem.StrListVisConData[2]);
            Cal_Sero_Uper = double.Parse(LamiSystem.StrListVisConData[5]);
            Cal_Garo_Down = double.Parse(LamiSystem.StrListVisConData[8]);
            Cal_Sero_Down = double.Parse(LamiSystem.StrListVisConData[11]);

            Cal_Value_Uper.Clear();
            Cal_Value_Down.Clear();

            int VisRowCount_Uper = LamiSystem.StrListVisConGridData_Uper.Count/8;
            for (int i = 0; i < VisRowCount_Uper; i++)
            {
                string tmpData = LamiSystem.StrListVisConGridData_Uper[i*8 + 6].ToString();
                Cal_Value_Uper.Add(double.Parse(LamiSystem.StrListVisConGridData_Uper[i*8 + 6].ToString()));
            }

            int VisRowCount_Down = LamiSystem.StrListVisConGridData_Down.Count / 8;
            for (int i = 0; i < VisRowCount_Down; i++)
            {
                string tmpData = LamiSystem.StrListVisConGridData_Down[i * 8 + 6].ToString();
                Cal_Value_Down.Add(double.Parse(LamiSystem.StrListVisConGridData_Down[i * 8 + 6].ToString()));
            }
            //Cal_Big_Left = double.Parse(LamiSystem.StrListVisConData[4]);
            //Cal_Big_Righ = double.Parse(LamiSystem.StrListVisConData[5]);
            //Cal_Sml_Left = double.Parse(LamiSystem.StrListVisConData[6]);
            //Cal_Sml_Righ = double.Parse(LamiSystem.StrListVisConData[7]);
        }

        private bool CPK_First_Flag = true;

        private static string[] strResultNgOk = { string.Empty, string.Empty, string.Empty };
        private static IplImage tempImage = new IplImage(4096, 3072, BitDepth.U8, 3);
        private int GraphSeries_Uper = -1;
        private int GraphSeries_Down = -1;

        private double Cal_Garo_Uper = 0.0;
        private double Cal_Sero_Uper = 0.0;// = 11.1577;
        double dNumber = 0.0;
        private bool ParseResult = false;
        private static double Cal_Big_Left = 0.0;
        private static double Cal_Big_Righ = 0.0;
        private static double Cal_Garo_Down = 0.0;
        private static double Cal_Sero_Down = 0.0;
        private static double Cal_Sml_Left = 0.0;
        private static double Cal_Sml_Righ = 0.0;
        private static int iImgPixResultData_Uper;
        private static int iImgPixResultData_Down;
        private bool Inspect_None_Flag_Uper = false;
        private bool Inspect_None_Flag_Down = false;
        private bool ZeroCheck_Start = false;

        private string Inspect_Result_Uper = "NG";

        public double Inspect_RunRun_MeasureData_Editing(double MeasureData, string CenterValue, string MaxValue, string MinValue)
        {
            double editResult = 0d;
            double dNumber = 0.0;
            bool ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
            //보상 비율이 정상적이 값인지 검사한다.
            if (ParseResult == true)
            {
                if (dNumber > 100) dNumber = 100.0;
                Gap_Maker_Per = dNumber;

                //string strCanValue = this.GetReg(LamiSystem.RegPathMeasure_Uper, (RowCount * 17 + 2).ToString("0.000")); //"규격 중심"

                double NowCenValue = double.Parse(CenterValue);
                double NowMaxValue = double.Parse(MaxValue);
                double NowMinValue = double.Parse(MinValue);
                editResult = Inspect_Gap_Data_Create(MeasureData, Gap_Maker_Per, NowCenValue, NowMaxValue, NowMinValue);
            }
            return editResult;
        }

        /*
        public double Inspect_RunRun_MeasureData_Editing(double MeaData, int RowCount)
        {
            double editResult = 0d;
            double dNumber = 0.0;
            bool ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
            //보상 비율이 정상적이 값인지 검사한다.
            if (ParseResult == true)
            {
                if (dNumber > 100) dNumber = 100.0;
                Gap_Maker_Per = dNumber;
                string strCanValue = this.GetReg(LamiSystem.RegPathMeasure_Uper, (RowCount * 17 + 2).ToString("0.000")); //"규격 중심"

                double NowCenValue = double.Parse(strCanValue);
                editResult = Inspect_Gap_Data_Create(MeaData, NowCenValue, Gap_Maker_Per);
            }
            return editResult;
        }
        */

        public void Inspect_Run_Run_FindData_ZeroData_Uper(UltraGridRow row, int RowCount, int NgCount)
        {
            row.Cells["판정 결과"].Value = "NG";
            row.Cells["NgCount"].Value = (NgCount++).ToString("0");
            row.Cells["측정 값"].Value = "0.000";
        }


        /*
        private delegate void Delegate_Test_Log_Write(string writeData, int mode, TextBox tbox);
        public void Inspect_Test_Log_Write(string writeData, int mode, TextBox tbox)
        {
            if (InvokeRequired)
            {
                Delegate_Test_Log_Write del = Inspect_Test_Log_Write;
                Invoke(del, writeData, mode, tbox);
            }
        */

        private delegate void Delegate_Run_Run_FindData_Inspection_Uper();
        private delegate void Delegate_Run_Run_FindData_Inspection_Down();

        public struct Inspection_Grid_Data
        {
            public int RecipeColumn;// = 11;
            public int VisionColumn;// = 8;
            public int RcpGrdRows;// = LamiSystem.StrLstRcpConGridData_Uper.Count / RecipeColumn;
            public UltraGridRow row;// = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();
            public string readName;// = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn];
            public string CenValue;// = LamiSystem.StrListVisConGridData_Uper[i * VisionColumn + 1];
            public string MaxValue;// = LamiSystem.StrListVisConGridData_Uper[i * VisionColumn + 2];
            public string MinValue;// = LamiSystem.StrListVisConGridData_Uper[i * VisionColumn + 3];
            public double MaxData;//
            public double MinData;//

            public int RowNum;// = 0;//row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
            public double maxCal;// = 0d;
            public double minCal;//
            public int SideNo;// = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 3]);
            public int iImgPixResultData1;// = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].Y - cvPntLstImagePoint_Uper[i*2].Y);
            public string dResultValueStr;
            public double dResultValue;// = ((double) iImgPixResultData1*(double) Cal_Sero_Uper);

            public string itemResult;

            public string strOkCount;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 14).ToString("000"));
            public int nowOkCount;// = int.Parse(strOkCount);

            public string strNgCount;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 15).ToString("000"));
            public int nowNgCount;// = int.Parse(strNgCount);

            public string uperMaxValuestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 3).ToString("000"));
            public double uperMaxValue;// = double.Parse(uperMaxValuestr);

            public string uperMinValuestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 4).ToString("000"));
            public double uperMinValue;// = double.Parse(uperMinValuestr);

            public float VisionItem_CalValue;// = float.Parse(LamiSystem.StrListVisConGridData_Uper[(i*8) + 6]);
            public bool VisionItem_CalUsed;// = (LamiSystem.StrListVisConGridData_Uper[(i*8) + 7] == "True")

            public float SuYul;//

            public float OkData;// = (float)nowOkCount;
            public float NgData;// = (float)nowNgCount;

            public float NowAvr;// = 0f;
            public float OldValue;//= 0f;
            public float OldSquare;//= 0f;

            public string OldValuestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 16).ToString("000"));
            public string OldSquarestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 17).ToString("000"));

            public float NowStdDev;// = 0f;
            public float[] StdDivArray;// = new float[2];

            public string OldStdDevstr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 9).ToString("000"));
            public float OldStdDev;// = float.Parse(OldStdDevstr);

            public string OldMinstr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 10).ToString("000"));
            public float OldMin;// = float.Parse(OldMinstr);
            public float NowMin;// = Control_CPKData.Min((float) dResultValue_Uper, OldMin);

            public string OldMaxstr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 11).ToString("000"));
            public float OldMax;// = float.Parse(OldMaxstr);
            public float NowMax;// = Control_CPKData.Max((float) dResultValue_Uper, OldMax);
 
            public float NowCP;// = 0f;

            public float NowCPKU;// = 0f; // Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
            public float NowCPKL;// = 0f; // Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
            public float NowCPK;//= 0f; // Control_CPKData.Cpk(NowCPKU, NowCPKL);

            public string strProductOK;
            public float ProductOK;
        }

        public struct List_Grid_Data
        {
            public int RecipeColumn;// = 11;
            public int VisionColumn;// = 8;
            public int RcpGrdRows;// = LamiSystem.StrLstRcpConGridData_Uper.Count / RecipeColumn;
            public UltraGridRow row;// = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();
            public List<string> readName;// = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn];
            public List<string> CenValue;// = LamiSystem.StrListVisConGridData_Uper[i * VisionColumn + 1];
            public List<string> MaxValue;// = LamiSystem.StrListVisConGridData_Uper[i * VisionColumn + 2];
            public List<string> MinValue;// = LamiSystem.StrListVisConGridData_Uper[i * VisionColumn + 3];
            public List<double> MaxData;//
            public List<double> MinData;//

            public int RowNum;// = 0;//row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
            public double maxCal;// = 0d;
            public double minCal;//
            public List<int> SideNo;// = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 3]);
            public List<int> iImgPixResultData1;// = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].Y - cvPntLstImagePoint_Uper[i*2].Y);
            public List<string> dResultValueStr;
            public List<double> dResultValue;// = ((double) iImgPixResultData1*(double) Cal_Sero_Uper);

            public List<string> strOkCount;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 14).ToString("000"));
            public List<int> nowOkCount;// = int.Parse(strOkCount);

            public List<string> strNgCount;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 15).ToString("000"));
            public List<int> nowNgCount;// = int.Parse(strNgCount);

            public List<string> uperMaxValuestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 3).ToString("000"));
            public List<double> uperMaxValue;// = double.Parse(uperMaxValuestr);

            public List<string> uperMinValuestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * 17 + 4).ToString("000"));
            public List<double> uperMinValue;// = double.Parse(uperMinValuestr);

            public float VisionItem_CalValue;// = float.Parse(LamiSystem.StrListVisConGridData_Uper[(i*8) + 6]);
            public bool VisionItem_CalUsed;// = (LamiSystem.StrListVisConGridData_Uper[(i*8) + 7] == "True")

            public List<float> SuYul;//

            public List<float> OkData;// = (float)nowOkCount;
            public List<float> NgData;// = (float)nowNgCount;

            public List<float> NowAvr;// = 0f;
            public List<float> OldValue;//= 0f;
            public List<float> OldSquare;//= 0f;

            public List<string> OldValuestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 16).ToString("000"));
            public List<string> OldSquarestr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 17).ToString("000"));

            public List<float> NowStdDev;// = 0f;
            public float[][] StdDivArray;// = new float[2];

            public List<string> OldStdDevstr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 9).ToString("000"));
            public List<float> OldStdDev;// = float.Parse(OldStdDevstr);

            public List<string> OldMinstr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 10).ToString("000"));
            public List<float> OldMin;// = float.Parse(OldMinstr);
            public List<float> NowMin;// = Control_CPKData.Min((float) dResultValue_Uper, OldMin);

            public List<string> OldMaxstr;// = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*17 + 11).ToString("000"));
            public List<float> OldMax;// = float.Parse(OldMaxstr);
            public List<float> NowMax;// = Control_CPKData.Max((float) dResultValue_Uper, OldMax);

            public List<float> NowCP;// = 0f;

            public List<float> NowCPKU;// = 0f; // Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
            public List<float> NowCPKL;// = 0f; // Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
            public List<float> NowCPK;//= 0f; // Control_CPKData.Cpk(NowCPKU, NowCPKL);

            public List<string> strProductOK;
            public List<float> ProductOK;
        }

        private delegate void Delegate_Manual_FindData_Inspection_Uper();
        private delegate void Delegate_Manual_FindData_Inspection_Down();
        public void Inspect_Manual_FindData_Inspection_Uper()
        {
            if (InvokeRequired)
            {
                Delegate_Manual_FindData_Inspection_Uper del = Inspect_Manual_FindData_Inspection_Uper;
                Invoke(del);
            }
            else
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();

                string pointData = string.Empty;

                int mesColCount = 19;
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
                //Test Function
                //Inspect_Test_Log_Write(pointData, 1, textBox15);
                Random r = new Random();
                
                try
                {
                    strLstDisplayData_Uper.Clear();
                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    StructData.RecipeColumn = 11;
                    StructData.VisionColumn = 8;
                    StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / StructData.RecipeColumn;

                    for (int i = 0; i < StructData.RcpGrdRows; i++)
                    {
                        //StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                        StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i * StructData.RecipeColumn];
                        int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / StructData.VisionColumn;

                        for (int j = 0; j < VisGrdRows; j++)
                        {
                            string VisName = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn];
                            if (StructData.readName == VisName)
                            {
                                StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 1];
                                StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 2];
                                StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 3];

                                //StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                                //strLstDisplayData_Uper.Add(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0"));
                                
                                strLstDisplayData_Uper.Add((i+1).ToString("0"));
                                
                                //StructData.row.Cells["검사 항목"].Value = StructData.readName;
                                StructData.readName = StructData.readName;
                                strLstDisplayData_Uper.Add(StructData.readName);

                                //StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                                strLstDisplayData_Uper.Add(StructData.CenValue);

                                StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                                StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);
                                
                                //StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                                strLstDisplayData_Uper.Add(StructData.MaxData.ToString("0.00"));

                                //StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");
                                strLstDisplayData_Uper.Add(StructData.MinData.ToString("0.00"));

                                //비전에서 설정한 아이템의 켈값을 적용한다.
                                StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 6]);
                                StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                                break;
                            }
                        }

                        string NowUsed = sLstNowDisNo_Uper[i * 2];
                        if (NowUsed == "False")
                        {
                            strLstDisplayData_Uper.Add("0.00");
                            strLstDisplayData_Uper.Add("NO");
                            continue;
                        }

                        //20150217 WKB 207
                        //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                        //20150217 WKB 208
                        //float tmpValue1 = (float)(r.Next(-100, 100) / 10000f);
                        //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                        //20150226 WKB 208
                        float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                        StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 3]);
                        if (StructData.SideNo % 2 == 0)
                        {
                            iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].Y - cvPntLstImagePoint_Uper[i * 2].Y);
                            
                            //20150217 WKB 207
                            //dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper) + tmpValue;

                            //20150217 WKB 208
                            dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper);
                        }
                        //가로 일때 진행됨
                        else
                        {
                            iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].X - cvPntLstImagePoint_Uper[i * 2].X);

                            //20150217 WKB 207
                            //dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper) + tmpValue;

                            //20150217 WKB 208
                            dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper);
                        }

                        StructData.uperMaxValue = StructData.MaxData;
                        StructData.uperMinValue = StructData.MinData;

                        if (cvPntLstImagePoint_Uper[i * 2].X == 0 || cvPntLstImagePoint_Uper[i * 2].Y == 0 || cvPntLstImagePoint_Uper[i * 2 + 1].X == 0 || cvPntLstImagePoint_Uper[i * 2 + 1].Y == 0)
                        {
                            //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                            //StructData.row.Cells["측정 값"].Value = "0.000";
                            StructData.dResultValue = 0;
                            StructData.itemResult = "NG";
                            //20150226 WKB 207
                            //strLstDisplayData_Uper.Add("0.000");

                            //20150226 WKB 208
                            strLstDisplayData_Uper.Add("0.00");

                            //StructData.row.Cells["판정 결과"].Value = "NG";
                            
                            strLstDisplayData_Uper.Add("NG");
                        }
                        else
                        {
                            //비전에서 설정한 아이템의 켈값을 적용한다.
                            //StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(i * 8) + 6]);
                            //StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(i * 8) + 7] == "True") ? true : false;
                            if (StructData.VisionItem_CalUsed == true) dResultValue_Uper = dResultValue_Uper * StructData.VisionItem_CalValue;

                            //측정값을 보상을 적용한다.
                            if (LamiSystem.StrListSysConData[28] == "ON")
                            {
//                                 dResultValue_Uper = Inspect_RunRun_MeasureData_Editing(dResultValue_Uper,
//                                     StructData.row.Cells["규격 중심"].Value.ToString(),
//                                     StructData.row.Cells["규격 상한"].Value.ToString(),
//                                     StructData.row.Cells["규격 하한"].Value.ToString());

                                dResultValue_Uper = Inspect_RunRun_MeasureData_Editing(dResultValue_Uper,
                                    StructData.CenValue.ToString(),
                                    StructData.MaxValue.ToString(),
                                    StructData.MinValue.ToString());
                            }

                            StructData.dResultValueStr = dResultValue_Uper.ToString("0.000");
                            
                            //20150226 WKB 207
                            //strLstDisplayData_Uper.Add(dResultValue_Uper.ToString("0.000"));
                            
                            //20150226 WKB 208
                            strLstDisplayData_Uper.Add(dResultValue_Uper.ToString("0.00"));

                            //현재 측정값이 양품인지 검사한다. 
                            if (StructData.uperMaxValue > dResultValue_Uper && StructData.uperMinValue < dResultValue_Uper)
                            {
                                StructData.itemResult = "OK";
                                strLstDisplayData_Uper.Add("OK");
                            }
                            else
                            {
                                //StructData.row.Cells["판정 결과"].Value = "NG";
                                StructData.itemResult = "NG";
                                strLstDisplayData_Uper.Add("NG");
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public void Inspect_Manual_FindData_Inspection_Down()
        {
            if (InvokeRequired)
            {
                Delegate_Manual_FindData_Inspection_Down del = Inspect_Manual_FindData_Inspection_Down;
                Invoke(del);
            }
            else
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();

                string pointData = string.Empty;

                int mesColCount = 19;
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
                //Test Function
                //Inspect_Test_Log_Write(pointData, 1, textBox15);
                Random r = new Random();


                try
                {
                    strLstDisplayData_Down.Clear();
                    uDS_Inspect_Measure_Down.Rows.Clear();
                    StructData.RecipeColumn = 11;
                    StructData.VisionColumn = 8;
                    StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Down.Count / StructData.RecipeColumn;

                    for (int i = 0; i < StructData.RcpGrdRows; i++)
                    {
                        //StructData.row = this.uGrd_Inspect_Measure_Down.DisplayLayout.Bands[0].AddNew();

                        StructData.readName = LamiSystem.StrLstRcpConGridData_Down[i * StructData.RecipeColumn];

                        int VisGrdRows = LamiSystem.StrListVisConGridData_Down.Count / StructData.VisionColumn;
                        for (int j = 0; j < VisGrdRows; j++)
                        {
                            string VisName = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn];
                            if (StructData.readName == VisName)
                            {
                                StructData.CenValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 1];
                                StructData.MaxValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 2];
                                StructData.MinValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 3];

                                //StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0");
                                strLstDisplayData_Down.Add((i+1).ToString("0"));

                                //StructData.row.Cells["검사 항목"].Value = StructData.readName;
                                strLstDisplayData_Down.Add(StructData.readName);

                                //StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                                strLstDisplayData_Down.Add(StructData.CenValue);

                                StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                                StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                                //StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                                strLstDisplayData_Down.Add(StructData.MaxData.ToString("0.00"));

                                //StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");
                                strLstDisplayData_Down.Add(StructData.MinData.ToString("0.00"));

                                //비전에서 설정한 아이템의 켈값을 적용한다.
                                StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Down[(j * StructData.VisionColumn) + 6]);
                                StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Down[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                                break;
                            }
                        }

                        string NowUsed = sLstNowDisNo_Down[i * 2];
                        if (NowUsed == "False")
                        {
                            strLstDisplayData_Down.Add("0.00");
                            strLstDisplayData_Down.Add("NO");
                            continue;
                        }
                        
                        //20150217 WKB 207
                        //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                        //20150217 WKB 208
                        //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                        //20150226 WKB 208
                        float tmpValue = (float)(r.Next(-99, 99) / 100000f);
                        tmpValue = tmpValue;
                        StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Down[i * 11 + 3]);
                        if (StructData.SideNo % 2 == 0)
                        {
                            iImgPixResultData_Down = Math.Abs(cvPntLstImagePoint_Down[i * 2 + 1].Y - cvPntLstImagePoint_Down[i * 2].Y);

                            //20150226 WKB 207
                            //dResultValue_Down = ((double)iImgPixResultData_Down * (double)Cal_Sero_Down) + tmpValue;

                            //20150226 WKB 208
                            dResultValue_Down = ((double)iImgPixResultData_Down * (double)Cal_Sero_Down);
                        }
                        //가로 일때 진행됨
                        else
                        {
                            iImgPixResultData_Down = Math.Abs(cvPntLstImagePoint_Down[i * 2 + 1].X - cvPntLstImagePoint_Down[i * 2].X);

                            //20150226 WKB 207
                            //dResultValue_Down = ((double)iImgPixResultData_Down * (double)Cal_Garo_Down) + tmpValue;

                            //20150226 WKB 208
                            dResultValue_Down = ((double)iImgPixResultData_Down * (double)Cal_Garo_Down);
                        }


                        StructData.uperMaxValue = StructData.MaxData;
                        StructData.uperMinValue = StructData.MinData;


                        if (cvPntLstImagePoint_Down[i * 2].X == 0 || cvPntLstImagePoint_Down[i * 2].Y == 0 || cvPntLstImagePoint_Down[i * 2 + 1].X == 0 || cvPntLstImagePoint_Down[i * 2 + 1].Y == 0)
                        {
                            //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                            //StructData.row.Cells["측정 값"].Value = "0.000";
                            //StructData.dResultValue = 0;
                            //20150226 WKB 207
                            //strLstDisplayData_Down.Add("0.000");

                            //20150226 WKB 208
                            strLstDisplayData_Down.Add("0.00");

                            //StructData.row.Cells["판정 결과"].Value = "NG";
                            //StructData.itemResult = "NG";
                            strLstDisplayData_Down.Add("NG");
                        }
                        else
                        {
                            
                            if (StructData.VisionItem_CalUsed == true) dResultValue_Down = dResultValue_Down * StructData.VisionItem_CalValue;

                            //측정값을 보상을 적용한다.
                            if (LamiSystem.StrListSysConData[28] == "ON")
                            {
                                //dResultValue_Down = Inspect_RunRun_MeasureData_Editing(dResultValue_Down,
                                //    StructData.row.Cells["규격 중심"].Value.ToString(),
                                //    StructData.row.Cells["규격 상한"].Value.ToString(),
                                //    StructData.row.Cells["규격 하한"].Value.ToString());

                                dResultValue_Down = Inspect_RunRun_MeasureData_Editing(dResultValue_Down,
                                    StructData.CenValue,
                                    StructData.MaxValue,
                                    StructData.MinValue);
                            }

                            //StructData.dResultValue = dResultValue_Down;

                            //20150226 WKB 207
                            //strLstDisplayData_Down.Add(dResultValue_Down.ToString("0.000"));

                            //20150226 WKB 208
                            strLstDisplayData_Down.Add(dResultValue_Down.ToString("0.00"));

                            //현재 측정값이 양품인지 검사한다. 
                            if (StructData.uperMaxValue > dResultValue_Down && StructData.uperMinValue < dResultValue_Down)
                            {
                                //StructData.row.Cells["판정 결과"].Value = "OK";
                                strLstDisplayData_Down.Add("OK");
                            }
                            else
                            {
                                //StructData.row.Cells["판정 결과"].Value = "NG";
                                strLstDisplayData_Down.Add("NG");
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        
        
        public void Inspect_Run_Run_FindData_Inspection_Uper()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Inspection_Uper del = Inspect_Run_Run_FindData_Inspection_Uper;
                Invoke(del);
            }
            else
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();

                string pointData = string.Empty;

                int mesColCount = 19;
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
                //Test Function
                //Inspect_Test_Log_Write(pointData, 1, textBox15);
                Random r = new Random();


                try
                {
                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    StructData.RecipeColumn = 11;
                    StructData.VisionColumn = 8;
                    StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / StructData.RecipeColumn;

                    for (int i = 0; i < StructData.RcpGrdRows; i++)
                    {
                        StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                        StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i * StructData.RecipeColumn];

                        int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / StructData.VisionColumn;
                        for (int j = 0; j < VisGrdRows; j++)
                        {
                            string VisName = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn];
                            if (StructData.readName == VisName)
                            {
                                StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 1];
                                StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 2];
                                StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 3];

                                StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                                StructData.row.Cells["검사 항목"].Value = StructData.readName;
                                StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                                StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                                StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);
                                StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                                StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");

                                //비전에서 설정한 아이템의 켈값을 적용한다.
                                StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 6]);
                                StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                                break;
                            }
                        }

                        string NowUsed = sLstNowDisNo_Uper[i * 2];
                        if (NowUsed == "False")
                        {
                            StructData.row.Cells["측정 값"].Value = "0.00";
                            StructData.row.Cells["판정 결과"].Value = "NO";
                            StructData.row.Cells["수율"].Value = "0.000";
                            StructData.row.Cells["평균"].Value = "0.000";
                            StructData.row.Cells["표준 편차"].Value = "0.000";
                            StructData.row.Cells["최소 값"].Value = "0.000";
                            StructData.row.Cells["최대 값"].Value = "0.000";
                            StructData.row.Cells["CP 값"].Value = "0.000";
                            StructData.row.Cells["CPK 값"].Value = "0.000";
                            StructData.row.Cells["OkCount"].Value = "0";
                            StructData.row.Cells["NgCount"].Value = "0";
                            StructData.row.Cells["SumValue"].Value = "0.0";
                            StructData.row.Cells["SquValue"].Value = "0.0";
                            continue;
                        }
                        
                        //20150216 WKB 207
                        //float tmpValue = (float)(r.Next(-100,100) / 10000f);

                        //20150226 WKB 208
                        float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                        StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 3]);
                        if (StructData.SideNo % 2 == 0)
                        {
                            iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].Y - cvPntLstImagePoint_Uper[i * 2].Y);
                            dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper) + tmpValue;
                        }
                            //가로 일때 진행됨
                        else
                        {
                            iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].X - cvPntLstImagePoint_Uper[i*2].X);
                            dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper) + tmpValue;
                        }


                        StructData.strOkCount = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"));
                        StructData.nowOkCount = int.Parse(StructData.strOkCount);

                        StructData.strNgCount = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"));
                        StructData.nowNgCount = int.Parse(StructData.strNgCount);

                        StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 3).ToString("000"));
                        StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                        StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 4).ToString("000"));
                        StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                        if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 || cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                        {
                            //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                            _strSavedInspectResult_Uper = "NG";
                            StructData.row.Cells["판정 결과"].Value = "NG";
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "NG");
                            StructData.row.Cells["NgCount"].Value = StructData.nowNgCount.ToString("0");

                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 5).ToString("000"), "0.000");
                            StructData.row.Cells["측정 값"].Value = "0.000";

                            StructData.row.Appearance.BackColor = Color.OrangeRed;
                        }
                        else
                        {
                            
                            if (StructData.VisionItem_CalUsed == true) dResultValue_Uper = dResultValue_Uper * StructData.VisionItem_CalValue;

                            //측정값을 보상을 적용한다.
                            if (LamiSystem.StrListSysConData[28] == "ON")
                            {
                                dResultValue_Uper = Inspect_RunRun_MeasureData_Editing(dResultValue_Uper,
                                    StructData.row.Cells["규격 중심"].Value.ToString(),
                                    StructData.row.Cells["규격 상한"].Value.ToString(),
                                    StructData.row.Cells["규격 하한"].Value.ToString());
                                //dResultValue_Uper = Inspect_RunRun_MeasureData_Editing(dResultValue_Uper, i, StructData.row.Cells["규격 중심"].Value.ToString());
                            }

                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 5).ToString("000"), dResultValue_Uper.ToString("0.000"));
                            StructData.row.Cells["측정 값"].Value = dResultValue_Uper.ToString("0.000");

                            //현재 측정값이 양품인지 검사한다. 
                            if (StructData.uperMaxValue > dResultValue_Uper && StructData.uperMinValue < dResultValue_Uper)
                            {
                                _strSavedInspectResult_Uper = "OK";
                                StructData.row.Cells["판정 결과"].Value = "OK";
                                StructData.nowOkCount = StructData.nowOkCount + 1;
                                //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "OK");
                                //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"), StructData.nowOkCount.ToString("0"));
                                StructData.row.Cells["OkCount"].Value = (StructData.nowOkCount).ToString("0");
                            }
                            else
                            {
                                _strSavedInspectResult_Uper = "NG";
                                StructData.row.Cells["판정 결과"].Value = "NG";
                                //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "NG");
                                StructData.nowNgCount = StructData.nowNgCount + 1;
                                //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                                StructData.row.Cells["NgCount"].Value = (StructData.nowNgCount).ToString("0");

                                StructData.row.Appearance.BackColor = Color.OrangeRed;
                            }
                        }


                        StructData.SuYul = 0f;
                        //현재 수율을 적용한다.
                        if (StructData.nowOkCount == 0) StructData.row.Cells["수율"].Value = "0.000";
                        else
                        {
                            StructData.OkData = (float)StructData.nowOkCount;
                            StructData.NgData = (float)StructData.nowNgCount;
                            StructData.SuYul = (float)(StructData.OkData / (StructData.OkData + StructData.NgData)) * 100;
                            StructData.row.Cells["수율"].Value = StructData.SuYul.ToString("0.000");
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 7).ToString("000"), StructData.SuYul.ToString("0.000"));
                        }

                        //현재 평균 값을 적용합니다.
                        StructData.NowAvr = 0f;
                        StructData.OldValue = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount == 1)
                        {
                            StructData.NowAvr = (float)dResultValue_Uper;
                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            StructData.OldValue = (float) dResultValue_Uper;
                            StructData.OldSquare = (float) dResultValue_Uper*(float) dResultValue_Uper;

                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        } 
                        else if (StructData.nowOkCount + StructData.nowNgCount > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            StructData.OkData = (float)StructData.nowOkCount;
                            StructData.NgData = (float)StructData.nowNgCount;
                            StructData.NowAvr = (float)((StructData.OldValue + (float)dResultValue_Uper) / (StructData.OkData + StructData.NgData));

                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            //StructData.OldSquare = (StructData.OldValue * StructData.OldValue) + ((float)dResultValue_Uper * (float)dResultValue_Uper);
                            StructData.OldSquare = (StructData.OldSquare) + ((float)dResultValue_Uper * (float)dResultValue_Uper);
                            StructData.OldValue = (StructData.OldValue + (float) dResultValue_Uper);

                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        }


                        //현재 표준편차을 적용하니다. nowOkCount
                        StructData.OldStdDev = 0f;
                        StructData.NowStdDev = 0f;
                        StructData.StdDivArray = new float[2];
                        if (StructData.nowOkCount + StructData.nowNgCount > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            int nCount = StructData.nowOkCount + StructData.nowNgCount;

                            StructData.NowStdDev =(float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                            StructData.row.Cells["표준 편차"].Value = StructData.NowStdDev.ToString("0.000");
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000"), StructData.NowStdDev.ToString("0.000"));
                        }

                        //현재 최소 값을 적용하니다.
                        StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"));
                        StructData.OldMin = float.Parse(StructData.OldMinstr);
                        StructData.NowMin = Control_CPKData.Min((float)dResultValue_Uper, StructData.OldMin);
                        StructData.row.Cells["최소 값"].Value = StructData.NowMin.ToString("0.000");
                        //시작 106 : 레지로 수정된 코드
                        //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                        //종료 106 : 레지로 수정된 코드

                        //현재 최대 값을 적용합니다.
                        StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"));
                        StructData.OldMax = float.Parse(StructData.OldMaxstr);
                        StructData.NowMax = Control_CPKData.Max((float)dResultValue_Uper, StructData.OldMax);
                        StructData.row.Cells["최대 값"].Value = StructData.NowMax.ToString("0.000");

                        //시작 107 : 레지로 수정된 코드
                        //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                        //종료 107 : 레지로 수정된 코드

                        //현재 CP 값을 적용하니다. 
                        StructData.NowCP = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                        {
                            StructData.NowCP = Control_CPKData.Cp((float)StructData.uperMaxValue, (float)StructData.uperMinValue, StructData.NowStdDev);
                            StructData.row.Cells["CP 값"].Value = StructData.NowCP.ToString("0.000");
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000"), StructData.NowCP.ToString("0.000"));
                        }

                        //현재 CPK 값을 적용합니다.
                        StructData.NowCPKU = 0f; // Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                        StructData.NowCPKL = 0f; // Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                        StructData.NowCPK = 0f; // Control_CPKData.Cpk(NowCPKU, NowCPKL);
                        if (StructData.nowOkCount + StructData.nowNgCount >= 2 )
                        {
                            StructData.NowCPKU = Control_CPKData.CpkU((float)StructData.uperMaxValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPKL = Control_CPKData.CpkL((float)StructData.uperMinValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                            StructData.row.Cells["CPK 값"].Value = StructData.NowCPK.ToString("0.000");
                            //this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                        }

                    }

                    //그리드 공란을 채운다.
                    if (StructData.RcpGrdRows < 11)
                    {
                        for (int i = 0; i < 10 - StructData.RcpGrdRows; i++)
                        {
                            StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();
                            StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                        }
                    }
                }
                catch (Exception e)
                {
                    //Inspect_Test_Log_Write(pointData, 2, textBox16);
                    //MessageBox.Show(MethodBase.GetCurrentMethod().Name + _iImgCount.ToString("0") + " " + e.Message);
                    throw;
                }
            }
        }

        /*
         public void Inspect_Run_Run_FindData_Inspection_Uper()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Inspection_Uper del = Inspect_Run_Run_FindData_Inspection_Uper;
                Invoke(del);
            }
            else
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();

                string pointData = string.Empty;

                int mesColCount = 19;
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
                //Test Function
                //Inspect_Test_Log_Write(pointData, 1, textBox15);
                Random r = new Random();


                try
                {
                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    StructData.RecipeColumn = 11;
                    StructData.VisionColumn = 8;
                    StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / StructData.RecipeColumn;

                    for (int i = 0; i < StructData.RcpGrdRows; i++)
                    {
                        StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                        StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i * StructData.RecipeColumn];

                        int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / StructData.VisionColumn;
                        for (int j = 0; j < VisGrdRows; j++)
                        {
                            string VisName = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn];
                            if (StructData.readName == VisName)
                            {
                                StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 1];
                                StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 2];
                                StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 3];

                                StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                                StructData.row.Cells["검사 항목"].Value = StructData.readName;
                                StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                                StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                                StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);
                                StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                                StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");

                                //비전에서 설정한 아이템의 켈값을 적용한다.
                                StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 6]);
                                StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                                break;
                            }
                        }

                        string NowUsed = sLstNowDisNo_Uper[i * 2];
                        if (NowUsed == "False")
                        {
                            StructData.row.Cells["측정 값"].Value = "0.00";
                            StructData.row.Cells["판정 결과"].Value = "NO";
                            StructData.row.Cells["수율"].Value = "0.000";
                            StructData.row.Cells["평균"].Value = "0.000";
                            StructData.row.Cells["표준 편차"].Value = "0.000";
                            StructData.row.Cells["최소 값"].Value = "0.000";
                            StructData.row.Cells["최대 값"].Value = "0.000";
                            StructData.row.Cells["CP 값"].Value = "0.000";
                            StructData.row.Cells["CPK 값"].Value = "0.000";
                            StructData.row.Cells["OkCount"].Value = "0";
                            StructData.row.Cells["NgCount"].Value = "0";
                            StructData.row.Cells["SumValue"].Value = "0.0";
                            StructData.row.Cells["SquValue"].Value = "0.0";
                            continue;
                        }

                        float tmpValue = (float)(r.Next(-100,100) / 10000f);
                        StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i * 11 + 3]);
                        if (StructData.SideNo % 2 == 0)
                        {
                            iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].Y - cvPntLstImagePoint_Uper[i * 2].Y);
                            dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper) + tmpValue;
                        }
                            //가로 일때 진행됨
                        else
                        {
                            iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].X - cvPntLstImagePoint_Uper[i*2].X);
                            dResultValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper) + tmpValue;
                        }


                        StructData.strOkCount = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"));
                        StructData.nowOkCount = int.Parse(StructData.strOkCount);

                        StructData.strNgCount = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"));
                        StructData.nowNgCount = int.Parse(StructData.strNgCount);

                        StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 3).ToString("000"));
                        StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                        StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 4).ToString("000"));
                        StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                        if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 || cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                        {
                            //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                            _strSavedInspectResult_Uper = "NG";
                            StructData.row.Cells["판정 결과"].Value = "NG";
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "NG");
                            StructData.row.Cells["NgCount"].Value = StructData.nowNgCount.ToString("0");

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 5).ToString("000"), "0.000");
                            StructData.row.Cells["측정 값"].Value = "0.000";

                            StructData.row.Appearance.BackColor = Color.OrangeRed;
                        }
                        else
                        {
                            
                            if (StructData.VisionItem_CalUsed == true) dResultValue_Uper = dResultValue_Uper * StructData.VisionItem_CalValue;

                            //측정값을 보상을 적용한다.
                            if (LamiSystem.StrListSysConData[28] == "ON")
                            {
                                dResultValue_Uper = Inspect_RunRun_MeasureData_Editing(dResultValue_Uper,
                                    StructData.row.Cells["규격 중심"].Value.ToString(),
                                    StructData.row.Cells["규격 상한"].Value.ToString(),
                                    StructData.row.Cells["규격 하한"].Value.ToString());
                                //dResultValue_Uper = Inspect_RunRun_MeasureData_Editing(dResultValue_Uper, i, StructData.row.Cells["규격 중심"].Value.ToString());
                            }

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 5).ToString("000"), dResultValue_Uper.ToString("0.000"));
                            StructData.row.Cells["측정 값"].Value = dResultValue_Uper.ToString("0.000");

                            //현재 측정값이 양품인지 검사한다. 
                            if (StructData.uperMaxValue > dResultValue_Uper && StructData.uperMinValue < dResultValue_Uper)
                            {
                                _strSavedInspectResult_Uper = "OK";
                                StructData.row.Cells["판정 결과"].Value = "OK";
                                StructData.nowOkCount = StructData.nowOkCount + 1;
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "OK");
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"), StructData.nowOkCount.ToString("0"));
                                StructData.row.Cells["OkCount"].Value = (StructData.nowOkCount).ToString("0");
                            }
                            else
                            {
                                _strSavedInspectResult_Uper = "NG";
                                StructData.row.Cells["판정 결과"].Value = "NG";
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "NG");
                                StructData.nowNgCount = StructData.nowNgCount + 1;
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                                StructData.row.Cells["NgCount"].Value = (StructData.nowNgCount).ToString("0");

                                StructData.row.Appearance.BackColor = Color.OrangeRed;
                            }
                        }


                        StructData.SuYul = 0f;
                        //현재 수율을 적용한다.
                        if (StructData.nowOkCount == 0) StructData.row.Cells["수율"].Value = "0.000";
                        else
                        {
                            StructData.OkData = (float)StructData.nowOkCount;
                            StructData.NgData = (float)StructData.nowNgCount;
                            StructData.SuYul = (float)(StructData.OkData / (StructData.OkData + StructData.NgData)) * 100;
                            StructData.row.Cells["수율"].Value = StructData.SuYul.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 7).ToString("000"), StructData.SuYul.ToString("0.000"));
                        }

                        //현재 평균 값을 적용합니다.
                        StructData.NowAvr = 0f;
                        StructData.OldValue = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount == 1)
                        {
                            StructData.NowAvr = (float)dResultValue_Uper;
                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            StructData.OldValue = (float) dResultValue_Uper;
                            StructData.OldSquare = (float) dResultValue_Uper*(float) dResultValue_Uper;

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        } 
                        else if (StructData.nowOkCount + StructData.nowNgCount > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            StructData.OkData = (float)StructData.nowOkCount;
                            StructData.NgData = (float)StructData.nowNgCount;
                            StructData.NowAvr = (float)((StructData.OldValue + (float)dResultValue_Uper) / (StructData.OkData + StructData.NgData));

                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            //StructData.OldSquare = (StructData.OldValue * StructData.OldValue) + ((float)dResultValue_Uper * (float)dResultValue_Uper);
                            StructData.OldSquare = (StructData.OldSquare) + ((float)dResultValue_Uper * (float)dResultValue_Uper);
                            StructData.OldValue = (StructData.OldValue + (float) dResultValue_Uper);

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        }


                        //현재 표준편차을 적용하니다. nowOkCount
                        StructData.OldStdDev = 0f;
                        StructData.NowStdDev = 0f;
                        StructData.StdDivArray = new float[2];
                        if (StructData.nowOkCount + StructData.nowNgCount > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            int nCount = StructData.nowOkCount + StructData.nowNgCount;

                            StructData.NowStdDev =(float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                            StructData.row.Cells["표준 편차"].Value = StructData.NowStdDev.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000"), StructData.NowStdDev.ToString("0.000"));
                        }

                        //현재 최소 값을 적용하니다.
                        StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"));
                        StructData.OldMin = float.Parse(StructData.OldMinstr);
                        StructData.NowMin = Control_CPKData.Min((float)dResultValue_Uper, StructData.OldMin);
                        StructData.row.Cells["최소 값"].Value = StructData.NowMin.ToString("0.000");
                        //시작 106 : 레지로 수정된 코드
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                        //종료 106 : 레지로 수정된 코드

                        //현재 최대 값을 적용합니다.
                        StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"));
                        StructData.OldMax = float.Parse(StructData.OldMaxstr);
                        StructData.NowMax = Control_CPKData.Max((float)dResultValue_Uper, StructData.OldMax);
                        StructData.row.Cells["최대 값"].Value = StructData.NowMax.ToString("0.000");

                        //시작 107 : 레지로 수정된 코드
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                        //종료 107 : 레지로 수정된 코드

                        //현재 CP 값을 적용하니다. 
                        StructData.NowCP = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                        {
                            StructData.NowCP = Control_CPKData.Cp((float)StructData.uperMaxValue, (float)StructData.uperMinValue, StructData.NowStdDev);
                            StructData.row.Cells["CP 값"].Value = StructData.NowCP.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000"), StructData.NowCP.ToString("0.000"));
                        }

                        //현재 CPK 값을 적용합니다.
                        StructData.NowCPKU = 0f; // Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                        StructData.NowCPKL = 0f; // Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                        StructData.NowCPK = 0f; // Control_CPKData.Cpk(NowCPKU, NowCPKL);
                        if (StructData.nowOkCount + StructData.nowNgCount >= 2 )
                        {
                            StructData.NowCPKU = Control_CPKData.CpkU((float)StructData.uperMaxValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPKL = Control_CPKData.CpkL((float)StructData.uperMinValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                            StructData.row.Cells["CPK 값"].Value = StructData.NowCPK.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                        }

                    }

                    //그리드 공란을 채운다.
                    if (StructData.RcpGrdRows < 11)
                    {
                        for (int i = 0; i < 10 - StructData.RcpGrdRows; i++)
                        {
                            StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();
                            StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                        }
                    }
                }
                catch (Exception e)
                {
                    //Inspect_Test_Log_Write(pointData, 2, textBox16);
                    //MessageBox.Show(MethodBase.GetCurrentMethod().Name + _iImgCount.ToString("0") + " " + e.Message);
                    throw;
                }
            }
        }

        */


        //20150217 WKB 208-
        public bool Inspect_Run_Run_FindData_OkNg_Uper(Inspection_Grid_Data StructData)
        {
            bool OkNg_Result = true;
            //Inspection_Grid_Data StructData = new Inspection_Grid_Data();

            int mesColCount = 19;
            Random r = new Random();

            double MeasureValue_Uper = 0d;

            StructData.RecipeColumn = 11;
            StructData.VisionColumn = 8;
            StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count/StructData.RecipeColumn;

            MeasureData_Uper.Clear();
            itemResult_Uper.Clear();

            for (int i = 0; i < StructData.RcpGrdRows; i++)
            {
                StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i*StructData.RecipeColumn];
                MeasureValue_Uper = 0d;

                int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count/StructData.VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn];
                    if (StructData.readName == VisName)
                    {
                        StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 1];
                        StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 2];
                        StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 3];

                        StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                        StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                        //비전에서 설정한 아이템의 켈값을 적용한다.
                        StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 6]);
                        StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 7] == "True") ? true : false;

                        break;
                    }
                }

                //레시피 그리드이 판별이 체크가 되어 있지 않은 경우
                string NowUsed = sLstNowDisNo_Uper[i*2];
                if (NowUsed == "False")
                {

                }

                //20150217 WKB 207
                //float tmpValue = (float) (r.Next(-100, 100)/10000f);

                //20150217 WKB 208
                //float tmpValue1 = (float)(r.Next(-100, 100) / 10000f);
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i*11 + 3]);
                if (StructData.SideNo%2 == 0)
                {
                    iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].Y - cvPntLstImagePoint_Uper[i*2].Y);
                    MeasureValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper) + tmpValue;
                }
                    //가로 일때 진행됨
                else
                {
                    iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].X - cvPntLstImagePoint_Uper[i*2].X);
                    MeasureValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper) + tmpValue;
                }


                //StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 3).ToString("000"));
                //StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                //StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 4).ToString("000"));
                //StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                StructData.uperMaxValue = double.Parse(LamiSystem.StrListVisConGridData_Uper[i * StructData.VisionColumn + 2]);
                StructData.uperMinValue = double.Parse(LamiSystem.StrListVisConGridData_Uper[i * StructData.VisionColumn + 3]);

                //측정 포인트를 찾지 못했을 경우
                if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 || cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                {
                    itemResult_Uper.Add("NG");

                    //20150226 WKB 207
                    //MeasureData_Uper.Add("0.000");

                    //20150226 WKB 208
                    MeasureData_Uper.Add("0.00000");

                    if (NowUsed != "False") OkNg_Result = false;
                }
                else
                {

                    if (StructData.VisionItem_CalUsed == true)
                        MeasureValue_Uper = MeasureValue_Uper * StructData.VisionItem_CalValue;

                    //측정값을 보상을 적용한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        MeasureValue_Uper = Inspect_RunRun_MeasureData_Editing(MeasureValue_Uper, StructData.CenValue, StructData.MaxValue, StructData.MinValue);
                    }

                    //현재 측정값이 양품인지 검사한다. 
                    //if (StructData.uperMaxValue > MeasureValue_Uper && StructData.uperMinValue < MeasureValue_Uper)
                    if (StructData.MaxData> MeasureValue_Uper && StructData.MinData < MeasureValue_Uper)
                    {
                        itemResult_Uper.Add("OK");
                        
                        //20150226 WKB 207
                        //MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.000"));

                        //20150226 WKB 208
                        MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.00000"));

                    }
                    else
                    {
                        itemResult_Uper.Add("NG");
                        
                        //20150226 WKB 207
                        //MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.000"));

                        //20150226 WKB 208
                        MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.00000"));


                        if (NowUsed != "False") OkNg_Result = false;
                    }
                }
            }

            if (OkNg_Result == true)
            {
                NowPassNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Uper"));
                NowPassNumber_Uper = NowPassNumber_Uper + 1;
                
                this.SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Uper", NowPassNumber_Uper.ToString("0"));
            }

            return OkNg_Result;
        }

        //20150725 WKB
        /*
         public bool Inspect_Run_Run_FindData_OkNg_Uper()
        {
            bool OkNg_Result = true;
            Inspection_Grid_Data StructData = new Inspection_Grid_Data();

            int mesColCount = 19;
            Random r = new Random();

            double MeasureValue_Uper = 0d;

            StructData.RecipeColumn = 11;
            StructData.VisionColumn = 8;
            StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count/StructData.RecipeColumn;

            MeasureData_Uper.Clear();
            itemResult_Uper.Clear();

            for (int i = 0; i < StructData.RcpGrdRows; i++)
            {
                StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i*StructData.RecipeColumn];
                MeasureValue_Uper = 0d;

                int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count/StructData.VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn];
                    if (StructData.readName == VisName)
                    {
                        StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 1];
                        StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 2];
                        StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 3];

                        StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                        StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                        //비전에서 설정한 아이템의 켈값을 적용한다.
                        StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 6]);
                        StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 7] == "True") ? true : false;

                        break;
                    }
                }

                //레시피 그리드이 판별이 체크가 되어 있지 않은 경우
                string NowUsed = sLstNowDisNo_Uper[i*2];
                if (NowUsed == "False")
                {

                }

                //20150217 WKB 207
                //float tmpValue = (float) (r.Next(-100, 100)/10000f);

                //20150217 WKB 208
                //float tmpValue1 = (float)(r.Next(-100, 100) / 10000f);
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i*11 + 3]);
                if (StructData.SideNo%2 == 0)
                {
                    iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].Y - cvPntLstImagePoint_Uper[i*2].Y);
                    MeasureValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper) + tmpValue;
                }
                    //가로 일때 진행됨
                else
                {
                    iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].X - cvPntLstImagePoint_Uper[i*2].X);
                    MeasureValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper) + tmpValue;
                }


                //StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 3).ToString("000"));
                //StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                //StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 4).ToString("000"));
                //StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                StructData.uperMaxValue = double.Parse(LamiSystem.StrListVisConGridData_Uper[i * StructData.VisionColumn + 2]);
                StructData.uperMinValue = double.Parse(LamiSystem.StrListVisConGridData_Uper[i * StructData.VisionColumn + 3]);

                //측정 포인트를 찾지 못했을 경우
                if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 || cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                {
                    itemResult_Uper.Add("NG");

                    //20150226 WKB 207
                    //MeasureData_Uper.Add("0.000");

                    //20150226 WKB 208
                    MeasureData_Uper.Add("0.00000");

                    if (NowUsed != "False") OkNg_Result = false;
                }
                else
                {

                    if (StructData.VisionItem_CalUsed == true)
                        MeasureValue_Uper = MeasureValue_Uper * StructData.VisionItem_CalValue;

                    //측정값을 보상을 적용한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        MeasureValue_Uper = Inspect_RunRun_MeasureData_Editing(MeasureValue_Uper, StructData.CenValue, StructData.MaxValue, StructData.MinValue);
                    }

                    //현재 측정값이 양품인지 검사한다. 
                    //if (StructData.uperMaxValue > MeasureValue_Uper && StructData.uperMinValue < MeasureValue_Uper)
                    if (StructData.MaxData> MeasureValue_Uper && StructData.MinData < MeasureValue_Uper)
                    {
                        itemResult_Uper.Add("OK");
                        
                        //20150226 WKB 207
                        //MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.000"));

                        //20150226 WKB 208
                        MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.00000"));

                    }
                    else
                    {
                        itemResult_Uper.Add("NG");
                        
                        //20150226 WKB 207
                        //MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.000"));

                        //20150226 WKB 208
                        MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.00000"));


                        if (NowUsed != "False") OkNg_Result = false;
                    }
                }
            }

            if (OkNg_Result == true)
            {
                NowPassNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Uper"));
                NowPassNumber_Uper = NowPassNumber_Uper + 1;
                
                this.SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Uper", NowPassNumber_Uper.ToString("0"));
            }

            return OkNg_Result;
        }
         */
        //20150217 WKB 207
        /*
        public bool Inspect_Run_Run_FindData_OkNg_Uper()
        {
            bool OkNg_Result = true;
            Inspection_Grid_Data StructData = new Inspection_Grid_Data();

            int mesColCount = 19;
            Random r = new Random();

            double MeasureValue_Uper = 0d;

            StructData.RecipeColumn = 11;
            StructData.VisionColumn = 8;
            StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count/StructData.RecipeColumn;

            MeasureData_Uper.Clear();
            itemResult_Uper.Clear();

            for (int i = 0; i < StructData.RcpGrdRows; i++)
            {
                StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i*StructData.RecipeColumn];
                MeasureValue_Uper = 0d;

                int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count/StructData.VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn];
                    if (StructData.readName == VisName)
                    {
                        StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 1];
                        StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 2];
                        StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 3];

                        StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                        StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                        //비전에서 설정한 아이템의 켈값을 적용한다.
                        StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 6]);
                        StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 7] == "True") ? true : false;

                        break;
                    }
                }

                //레시피 그리드이 판별이 체크가 되어 있지 않은 경우
                string NowUsed = sLstNowDisNo_Uper[i*2];
                if (NowUsed == "False")
                {
//                     itemResult_Uper.Add("NO");
//                     MeasureData_Uper.Add("0.000");
//                     continue;
                }

                //20150217 WKB 207
                //float tmpValue = (float) (r.Next(-100, 100)/10000f);

                

                StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i*11 + 3]);
                if (StructData.SideNo%2 == 0)
                {
                    iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].Y - cvPntLstImagePoint_Uper[i*2].Y);
                    MeasureValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Sero_Uper) + tmpValue;
                }
                    //가로 일때 진행됨
                else
                {
                    iImgPixResultData_Uper = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].X - cvPntLstImagePoint_Uper[i*2].X);
                    MeasureValue_Uper = ((double)iImgPixResultData_Uper * (double)Cal_Garo_Uper) + tmpValue;
                }


                StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 3).ToString("000"));
                StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 4).ToString("000"));
                StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                //측정 포인트를 찾지 못했을 경우
                if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 || cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                {
                    itemResult_Uper.Add("NG");
                    MeasureData_Uper.Add("0.000");
                    if (NowUsed != "False") OkNg_Result = false;
                }
                else
                {

                    if (StructData.VisionItem_CalUsed == true)
                        MeasureValue_Uper = MeasureValue_Uper * StructData.VisionItem_CalValue;

                    //측정값을 보상을 적용한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        MeasureValue_Uper = Inspect_RunRun_MeasureData_Editing(MeasureValue_Uper, StructData.CenValue, StructData.MaxValue, StructData.MinValue);
                    }

                    //현재 측정값이 양품인지 검사한다. 
                    if (StructData.uperMaxValue > MeasureValue_Uper && StructData.uperMinValue < MeasureValue_Uper)
                    {
                        itemResult_Uper.Add("OK");
                        MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.000"));
                    }
                    else
                    {
                        itemResult_Uper.Add("NG");
                        MeasureData_Uper.Add(MeasureValue_Uper.ToString("0.000"));

                        if (NowUsed != "False") OkNg_Result = false;
                    }
                }
            }

            if (OkNg_Result == true)
            {
                NowPassNumber_Uper = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Uper"));
                NowPassNumber_Uper = NowPassNumber_Uper + 1;
                
                this.SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Uper", NowPassNumber_Uper.ToString("0"));
            }

            return OkNg_Result;
        }
        */

        List<string> itemResult_Uper = new List<string>();
        List<string> MeasureData_Uper = new List<string>();
        /*
        public void Inspect_Run_Run_FindData_Inspection_Uper()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Inspection_Uper del = Inspect_Run_Run_FindData_Inspection_Uper;
                Invoke(del);
            }
            else
            {
            Inspection_Grid_Data StructData = new Inspection_Grid_Data();

            string pointData = string.Empty;

            int mesColCount = 19;
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
            //Test Function
            //Inspect_Test_Log_Write(pointData, 1, textBox15);
            Random r = new Random();


            try
            {
                //Stopwatch testFunc = new Stopwatch();
                //testFunc.Reset();
                //testFunc.Start();
                bool OkNg_Data = Inspect_Run_Run_FindData_OkNg_Uper();
                //testFunc.Stop();
                //MessageBox.Show(testFunc.ElapsedMilliseconds.ToString());

                uDS_Inspect_Measure_Uper.Rows.Clear();
                StructData.RecipeColumn = 11;
                StructData.VisionColumn = 8;
                StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count/StructData.RecipeColumn;

                for (int i = 0; i < StructData.RcpGrdRows; i++)
                {
                    //UltraDataRow dataRow = uDS_Inspect_Measure_Uper.Rows.Add();
                    StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                    StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i*StructData.RecipeColumn];

                    int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count/StructData.VisionColumn;
                    for (int j = 0; j < VisGrdRows; j++)
                    {
                        string VisName = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn];
                        if (StructData.readName == VisName)
                        {
                            StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 1];
                            StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 2];
                            StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 3];

                            StructData.row.Cells["NO"].Value =uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                            StructData.row.Cells["검사 항목"].Value = StructData.readName;
                            StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                            StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                            StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);
                            StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                            StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");

                            //비전에서 설정한 아이템의 켈값을 적용한다.
                            StructData.VisionItem_CalValue =float.Parse(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 6]);
                            StructData.VisionItem_CalUsed =(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 7] == "True")? true: false;

                            //측정된 값을 할당한다.
                            dResultValue_Uper = double.Parse(MeasureData_Uper[i]);
                            
                            //레지스트리에 있는 값들을 읽어와서 형변환을 거친후 할당한다.
                            StructData.nowOkCount =int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 14).ToString("000")));

                            StructData.nowNgCount =int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 15).ToString("000")));

                            StructData.uperMaxValue =double.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 3).ToString("000")));

                            StructData.uperMinValue =double.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 4).ToString("000")));
                            
                            break;
                        }
                    }
                    
                    //판별이 활성화로 설정이 안되어 있는 경우에는 판별을 진향하지 않는다.
                    string NowUsed = sLstNowDisNo_Uper[i*2];
                    if (NowUsed == "False")
                    {
                        StructData.row.Cells["측정 값"].Value = "0.00";
                        StructData.row.Cells["판정 결과"].Value = "NO";
                        StructData.row.Cells["수율"].Value = "0.000";
                        StructData.row.Cells["평균"].Value = "0.000";
                        StructData.row.Cells["표준 편차"].Value = "0.000";
                        StructData.row.Cells["최소 값"].Value = "0.000";
                        StructData.row.Cells["최대 값"].Value = "0.000";
                        StructData.row.Cells["CP 값"].Value = "0.000";
                        StructData.row.Cells["CPK 값"].Value = "0.000";
                        StructData.row.Cells["OkCount"].Value = "0";
                        StructData.row.Cells["NgCount"].Value = "0";
                        StructData.row.Cells["SumValue"].Value = "0.0";
                        StructData.row.Cells["SquValue"].Value = "0.0";
                        StructData.row.Cells["ProductOK"].Value = "0.0";
                        continue;
                    }


                    if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 ||
                        cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                    {
                        //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                        _strSavedInspectResult_Uper = "NG";
                        StructData.row.Cells["판정 결과"].Value = "NG";
                        StructData.nowNgCount = StructData.nowNgCount + 1;
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 6).ToString("000"), "NG");
                        StructData.row.Cells["NgCount"].Value = StructData.nowNgCount.ToString("0");

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 5).ToString("000"), "0.000");
                        StructData.row.Cells["측정 값"].Value = "0.000";

                        StructData.row.Appearance.BackColor = Color.OrangeRed;
                    }
                    else
                    {
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 5).ToString("000"),dResultValue_Uper.ToString("0.000"));
                        StructData.row.Cells["측정 값"].Value = dResultValue_Uper.ToString("0.000");

                        //현재 측정값이 양품인지 검사한다. itemResult_Uper
                        //if (StructData.uperMaxValue > dResultValue_Uper && StructData.uperMinValue < dResultValue_Uper)
                        if (itemResult_Uper[i] == "OK")
                        {
                            _strSavedInspectResult_Uper = "OK";
                            StructData.row.Cells["판정 결과"].Value = "OK";
                            StructData.nowOkCount = StructData.nowOkCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 6).ToString("000"), "OK");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 14).ToString("000"),StructData.nowOkCount.ToString("0"));

                            StructData.row.Cells["OkCount"].Value = (StructData.nowOkCount).ToString("0");
                        }
                        else
                        {
                            _strSavedInspectResult_Uper = "NG";
                            StructData.row.Cells["판정 결과"].Value = "NG";
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 6).ToString("000"), "NG");
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                            StructData.row.Cells["NgCount"].Value = (StructData.nowNgCount).ToString("0");

                            StructData.row.Appearance.BackColor = Color.OrangeRed;
                        }
                    }


                    StructData.SuYul = 0f;
                    //현재 수율을 적용한다.
                    if (StructData.nowOkCount == 0) StructData.row.Cells["수율"].Value = "0.000";
                    else
                    {
                        StructData.OkData = (float) StructData.nowOkCount;
                        StructData.NgData = (float) StructData.nowNgCount;
                        StructData.SuYul = (float) (StructData.OkData/(StructData.OkData + StructData.NgData))*100;
                        StructData.row.Cells["수율"].Value = StructData.SuYul.ToString("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 7).ToString("000"),StructData.SuYul.ToString("0.000"));
                    }

                    if (OkNg_Data == false)
                    {
                        //평균값 기존데이터로 처리
                        StructData.row.Cells["평균"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 8).ToString("000"));
                        //표준편차 기존데이터로 처리
                        StructData.row.Cells["표준 편차"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 9).ToString("000"));
                        //최소 값 기존데이터로 처리
                        StructData.row.Cells["최소 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 10).ToString("000"));
                        //최대 값 기존데이터로 처리
                        StructData.row.Cells["최대 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 11).ToString("000"));
                        //CP 값 기존데이터로 처리
                        StructData.row.Cells["CP 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 12).ToString("000"));
                        //CPK 값 기존데이터로 처리
                        StructData.row.Cells["CPK 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 13).ToString("000"));
                        continue;
                    }

                    //현재 평균 값을 적용합니다.
                    StructData.NowAvr = 0f;
                    StructData.OldValue = 0f;
                    if (NowPassNumber_Uper == 1)
                    {
                        StructData.NowAvr = (float) dResultValue_Uper;
                        StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"),StructData.NowAvr.ToString("0.000"));

                        StructData.OldValue = (float) dResultValue_Uper;
                        StructData.OldSquare = (float) dResultValue_Uper*(float) dResultValue_Uper;

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"),StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"),StructData.OldSquare.ToString("0.000"));
                    }
                    else if (NowPassNumber_Uper > 1)
                    {
                        StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 16).ToString("000"));
                        StructData.OldValue = float.Parse(StructData.OldValuestr);

                        StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 17).ToString("000"));
                        StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                        StructData.NowAvr =(float) ((StructData.OldValue + (float) dResultValue_Uper)/(float) NowPassNumber_Uper);
                        StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"),StructData.NowAvr.ToString("0.000"));

                        StructData.OldSquare = (StructData.OldSquare) +((float) dResultValue_Uper*(float) dResultValue_Uper);
                        StructData.OldValue = (StructData.OldValue + (float) dResultValue_Uper);

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"),StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"),StructData.OldSquare.ToString("0.000"));
                    }


                    //현재 표준편차을 적용하니다. nowOkCount
                    StructData.OldStdDev = 0f;
                    StructData.NowStdDev = 0f;
                    StructData.StdDivArray = new float[2];
                    if (StructData.nowOkCount <= 1) StructData.row.Cells["표준 편차"].Value = "0.000";
                    else if (StructData.nowOkCount > 1)
                    {
                        StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 16).ToString("000"));
                        StructData.OldValue = float.Parse(StructData.OldValuestr);

                        StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 17).ToString("000"));
                        StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                        //int nCount = StructData.nowOkCount; //+ StructData.nowNgCount;
                        int nCount = (int)NowPassNumber_Uper;

                        StructData.NowStdDev =(float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                        StructData.row.Cells["표준 편차"].Value = StructData.NowStdDev.ToString("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 9).ToString("000"),StructData.NowStdDev.ToString("0.000"));
                    }

                    //현재 최소 값을 적용하니다.
                    StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 10).ToString("000"));
                    StructData.OldMin = float.Parse(StructData.OldMinstr);
                    StructData.NowMin = Control_CPKData.Min((float) dResultValue_Uper, StructData.OldMin);
                    StructData.row.Cells["최소 값"].Value = StructData.NowMin.ToString("0.000");
                    //시작 106 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 10).ToString("000"),StructData.NowMin.ToString("0.000"));
                    //종료 106 : 레지로 수정된 코드

                    //현재 최대 값을 적용합니다.
                    StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 11).ToString("000"));
                    StructData.OldMax = float.Parse(StructData.OldMaxstr);
                    StructData.NowMax = Control_CPKData.Max((float) dResultValue_Uper, StructData.OldMax);
                    StructData.row.Cells["최대 값"].Value = StructData.NowMax.ToString("0.000");

                    //시작 107 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 11).ToString("000"),StructData.NowMax.ToString("0.000"));
                    //종료 107 : 레지로 수정된 코드

                    //현재 CP 값을 적용하니다. 
                    StructData.NowCP = 0f;
                    if (StructData.nowOkCount + StructData.nowNgCount < 2) StructData.row.Cells["CP 값"].Value = "0.000";
                    else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                    {
                        StructData.NowCP = Control_CPKData.Cp((float) StructData.uperMaxValue,(float) StructData.uperMinValue, StructData.NowStdDev);
                        StructData.row.Cells["CP 값"].Value = StructData.NowCP.ToString("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 12).ToString("000"),StructData.NowCP.ToString("0.000"));
                    }

                    //현재 CPK 값을 적용합니다.
                    StructData.NowCPKU = 0f; // Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                    StructData.NowCPKL = 0f; // Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                    StructData.NowCPK = 0f; // Control_CPKData.Cpk(NowCPKU, NowCPKL);
                    if (StructData.nowOkCount + StructData.nowNgCount < 2) StructData.row.Cells["CPK 값"].Value = "0.000";
                    else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                    {
                        StructData.NowCPKU = Control_CPKData.CpkU((float) StructData.uperMaxValue, StructData.NowAvr,StructData.NowStdDev);
                        StructData.NowCPKL = Control_CPKData.CpkL((float) StructData.uperMinValue, StructData.NowAvr,StructData.NowStdDev);
                        StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                        StructData.row.Cells["CPK 값"].Value = StructData.NowCPK.ToString("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 13).ToString("000"),StructData.NowCPK.ToString("0.000"));
                    }
                }

                //그리드 공란을 채운다.
                if (StructData.RcpGrdRows < 11)
                {
                    for (int i = 0; i < 10 - StructData.RcpGrdRows; i++)
                    {
                        StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();
                        StructData.row.Cells["NO"].Value =uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + e.Message);
                throw;
            }
            }
        }
        */
        public struct Inspection_Data_List
        {
            public List<string> Inspection_Data;
        }

        public Inspection_Grid_Data[] Inspect_Run_Run_FindData_Inspecting_Uper()
        {
            
            Inspection_Grid_Data StructData = new Inspection_Grid_Data();
            StructData.RecipeColumn = 11;
            StructData.VisionColumn = 8;
            StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / StructData.RecipeColumn;
            Inspection_Grid_Data[] Inspect_Grid_Data = new Inspection_Grid_Data[StructData.RcpGrdRows];

            try
            {
                Inspection_Data_List InspectData = new Inspection_Data_List();
                InspectData.Inspection_Data = new List<string>();
                
                int mesColCount = 19;
                bool OkNg_Data = Inspect_Run_Run_FindData_OkNg_Uper(StructData);
                
                if (OkNg_Data == true) _strSavedInspectResult_Uper = "OK";
                else _strSavedInspectResult_Uper = "NG";

                

                for (int i = 0; i < StructData.RcpGrdRows; i++)
                {
                    StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i*StructData.RecipeColumn];
                    StructData.dResultValueStr = MeasureData_Uper[i];
                    StructData.itemResult = itemResult_Uper[i];

                    int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count/StructData.VisionColumn;
                    for (int j = 0; j < VisGrdRows; j++)
                    {
                        string VisName = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn];
                        if (StructData.readName == VisName)
                        {
                            StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 1];
                            StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 2];
                            StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 3];

                            StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                            StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                            InspectData.Inspection_Data.Add(StructData.readName);
                            InspectData.Inspection_Data.Add(StructData.CenValue);
                            InspectData.Inspection_Data.Add(StructData.MaxData.ToString("0.00"));
                            InspectData.Inspection_Data.Add(StructData.MinData.ToString("0.00"));

                            //비전에서 설정한 아이템의 켈값을 적용한다.
                            StructData.VisionItem_CalValue =float.Parse(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 6]);
                            StructData.VisionItem_CalUsed =(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 7] == "True")? true: false;

                            //측정된 값을 할당한다.
                            dResultValue_Uper = double.Parse(MeasureData_Uper[i]);

                            //레지스트리에 있는 값들을 읽어와서 형변환을 거친후 할당한다.

                            //string tmpstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 14).ToString("000"));
                            StructData.nowOkCount =int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 14).ToString("000")));
                            StructData.nowNgCount =int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 15).ToString("000")));
                            //StructData.uperMaxValue =double.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 3).ToString("000")));
                            //StructData.uperMinValue =double.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 4).ToString("000")));
                            break;
                        }
                    }


                    if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 ||
                        cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                    {
                        //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                        InspectData.Inspection_Data.Add("0.000");
                        //_strSavedInspectResult_Uper = "NG";
                        InspectData.Inspection_Data.Add("NG");

                        StructData.nowNgCount = StructData.nowNgCount + 1;
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                    }
                    else
                    {
                        InspectData.Inspection_Data.Add(dResultValue_Uper.ToString("0.000"));

                        //현재 측정값이 양품인지 검사한다. itemResult_Uper
                        if (itemResult_Uper[i] == "OK")
                        {
                            //_strSavedInspectResult_Uper = "OK";
                            InspectData.Inspection_Data.Add("OK");
                            StructData.nowOkCount = StructData.nowOkCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 14).ToString("000"),StructData.nowOkCount.ToString("0"));
                        }
                        else
                        {
                            //_strSavedInspectResult_Uper = "NG";
                            InspectData.Inspection_Data.Add("NG");
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                        }
                    }

                    StructData.SuYul = 0f;
                    //현재 수율을 적용한다.
                    if (StructData.nowOkCount == 0)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 7).ToString("000"), "0.000");
                    }
                    else
                    {
                        StructData.OkData = (float) StructData.nowOkCount;
                        StructData.NgData = (float) StructData.nowNgCount;
                        StructData.SuYul = (float) (StructData.OkData/(StructData.OkData + StructData.NgData))*100;
                        InspectData.Inspection_Data.Add(StructData.SuYul.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 7).ToString("000"), StructData.SuYul.ToString("0.000"));
                    }

                    if (i == 7)
                        i = i;
                    if (OkNg_Data == false)
                    {
                        //평균값 기존데이터로 처리
                        string tmpAvr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"));
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 8).ToString("000")));
                        //표준편차 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 9).ToString("000")));
                        //최소 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 10).ToString("000")));
                        //최대 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 11).ToString("000")));
                        //CP 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 12).ToString("000")));
                        //CPK 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 13).ToString("000")));

                        //편균
                        StructData.NowAvr = float.Parse( this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000")));
                        StructData.NowStdDev = float.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 9).ToString("000")));

                        StructData.NowMin = float.Parse(GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000")));
                        StructData.NowMax  = float.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000")));

                        StructData.NowCP = float.Parse(GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000")));
                        StructData.NowCPK = float.Parse(GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000")));
                        
                        Inspect_Grid_Data[i] = StructData;

                        continue;
                    }

                    //현재 평균 값을 적용합니다.
                    StructData.NowAvr = 0f;
                    StructData.OldValue = 0f;
                    if (NowPassNumber_Uper == 1)
                    {
                        StructData.NowAvr = (float) dResultValue_Uper;
                        InspectData.Inspection_Data.Add(StructData.NowAvr.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                        StructData.OldValue = (float) dResultValue_Uper;
                        StructData.OldSquare = (float) dResultValue_Uper*(float) dResultValue_Uper;

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                    }
                    else if (NowPassNumber_Uper > 1)
                    {
                        StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"));
                        StructData.OldValue = float.Parse(StructData.OldValuestr);

                        StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"));
                        StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                        StructData.NowAvr = (float) ((StructData.OldValue + (float) dResultValue_Uper)/(float) NowPassNumber_Uper);
                        InspectData.Inspection_Data.Add(StructData.NowAvr.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                        StructData.OldSquare = (StructData.OldSquare) + ((float) dResultValue_Uper*(float) dResultValue_Uper);
                        StructData.OldValue = (StructData.OldValue + (float) dResultValue_Uper);

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                    }

                    //현재 표준편차을 적용하니다. nowOkCount
                    StructData.OldStdDev = 0f;
                    StructData.NowStdDev = 0f;
                    StructData.StdDivArray = new float[2];

                    if (StructData.nowOkCount <= 1)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 9).ToString("000"), "0.000");
                    }
                    else if (StructData.nowOkCount > 1)
                    {
                        int nCount = (int) NowPassNumber_Uper;
                        StructData.NowStdDev =(float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                        InspectData.Inspection_Data.Add(StructData.NowStdDev.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 9).ToString("000"), StructData.NowStdDev.ToString("0.000"));
                    }

                    //현재 최소 값을 적용하니다.

                    
                    StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 10).ToString("000"));
                    StructData.OldMin = float.Parse(StructData.OldMinstr);
                    StructData.NowMin = Control_CPKData.Min((float) dResultValue_Uper, StructData.OldMin);

                    
                    if (StructData.OldMin < 0)
                        MessageBox.Show("기존값 : " + dResultValue_Uper.ToString());
                    if (dResultValue_Uper < 0)
                        MessageBox.Show("측정값 : " + dResultValue_Uper.ToString());
                    if (StructData.NowMin < 0) 
                        MessageBox.Show("현재값 : " + StructData.NowMin.ToString());
                    

                    InspectData.Inspection_Data.Add(StructData.NowMin.ToString("0.000"));

                    //시작 106 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                    //종료 106 : 레지로 수정된 코드

                    //현재 최대 값을 적용합니다.
                    StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 11).ToString("000"));
                    StructData.OldMax = float.Parse(StructData.OldMaxstr);
                    StructData.NowMax = Control_CPKData.Max((float) dResultValue_Uper, StructData.OldMax);
                    InspectData.Inspection_Data.Add(StructData.NowMax.ToString("0.000"));

                    //시작 107 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                    //종료 107 : 레지로 수정된 코드

                    //현재 CP 값을 적용하니다. 
                    StructData.NowCP = 0f;
                    if (StructData.nowOkCount + StructData.nowNgCount < 2)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 12).ToString("000"), "0.000");
                    }
                    else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                    {
                        StructData.NowCP = Control_CPKData.Cp((float)StructData.MaxData, (float)StructData.MinData, StructData.NowStdDev);
                        InspectData.Inspection_Data.Add(StructData.NowCP.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 12).ToString("000"),StructData.NowCP.ToString("0.000"));
                    }

                    //현재 CPK 값을 적용합니다.
                    StructData.NowCPKU = 0f;
                    StructData.NowCPKL = 0f;
                    StructData.NowCPK = 0f;
                    if (StructData.nowOkCount + StructData.nowNgCount < 2)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 13).ToString("000"), "0.000");
                    }
                    else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                    {
                        StructData.NowCPKU = Control_CPKData.CpkU((float)StructData.MaxData, StructData.NowAvr, StructData.NowStdDev);
                        StructData.NowCPKL = Control_CPKData.CpkL((float)StructData.MinData, StructData.NowAvr, StructData.NowStdDev);
                        StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                        InspectData.Inspection_Data.Add(StructData.NowCPK.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                    }

                    Inspect_Grid_Data[i] = StructData;
                }

                return Inspect_Grid_Data;
                //return InspectData;
            }
            catch (Exception e)
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + e.Message);
                return Inspect_Grid_Data;
            }
        }

        //20150725 WKB
        /*
        public Inspection_Data_List Inspect_Run_Run_FindData_Inspecting_Uper(  )
        {
            Inspection_Data_List InspectData = new Inspection_Data_List();
            try
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();
                InspectData.Inspection_Data = new List<string>();

                int mesColCount = 19;
                bool OkNg_Data = Inspect_Run_Run_FindData_OkNg_Uper( );
                
                if (OkNg_Data == true) _strSavedInspectResult_Uper = "OK";
                else _strSavedInspectResult_Uper = "NG";

                StructData.RecipeColumn = 11;
                StructData.VisionColumn = 8;
                StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count/StructData.RecipeColumn;

                for (int i = 0; i < StructData.RcpGrdRows; i++)
                {
                    StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i*StructData.RecipeColumn];

                    int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count/StructData.VisionColumn;
                    for (int j = 0; j < VisGrdRows; j++)
                    {
                        string VisName = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn];
                        if (StructData.readName == VisName)
                        {
                            StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 1];
                            StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 2];
                            StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j*StructData.VisionColumn + 3];

                            StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                            StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                            InspectData.Inspection_Data.Add(StructData.readName);
                            InspectData.Inspection_Data.Add(StructData.CenValue);
                            InspectData.Inspection_Data.Add(StructData.MaxData.ToString("0.00"));
                            InspectData.Inspection_Data.Add(StructData.MinData.ToString("0.00"));

                            //비전에서 설정한 아이템의 켈값을 적용한다.
                            StructData.VisionItem_CalValue =float.Parse(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 6]);
                            StructData.VisionItem_CalUsed =(LamiSystem.StrListVisConGridData_Uper[(j*StructData.VisionColumn) + 7] == "True")? true: false;

                            //측정된 값을 할당한다.
                            dResultValue_Uper = double.Parse(MeasureData_Uper[i]);

                            //레지스트리에 있는 값들을 읽어와서 형변환을 거친후 할당한다.

                            //string tmpstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 14).ToString("000"));
                            StructData.nowOkCount =int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 14).ToString("000")));
                            StructData.nowNgCount =int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 15).ToString("000")));
                            StructData.uperMaxValue =double.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 3).ToString("000")));
                            StructData.uperMinValue =double.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 4).ToString("000")));
                            break;
                        }
                    }


                    if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 ||
                        cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                    {
                        //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                        InspectData.Inspection_Data.Add("0.000");
                        //_strSavedInspectResult_Uper = "NG";
                        InspectData.Inspection_Data.Add("NG");

                        StructData.nowNgCount = StructData.nowNgCount + 1;
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                    }
                    else
                    {
                        InspectData.Inspection_Data.Add(dResultValue_Uper.ToString("0.000"));

                        //현재 측정값이 양품인지 검사한다. itemResult_Uper
                        if (itemResult_Uper[i] == "OK")
                        {
                            //_strSavedInspectResult_Uper = "OK";
                            InspectData.Inspection_Data.Add("OK");
                            StructData.nowOkCount = StructData.nowOkCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 14).ToString("000"),StructData.nowOkCount.ToString("0"));
                        }
                        else
                        {
                            //_strSavedInspectResult_Uper = "NG";
                            InspectData.Inspection_Data.Add("NG");
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                        }
                    }

                    StructData.SuYul = 0f;
                    //현재 수율을 적용한다.
                    if (StructData.nowOkCount == 0)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 7).ToString("000"), "0.000");
                    }
                    else
                    {
                        StructData.OkData = (float) StructData.nowOkCount;
                        StructData.NgData = (float) StructData.nowNgCount;
                        StructData.SuYul = (float) (StructData.OkData/(StructData.OkData + StructData.NgData))*100;
                        InspectData.Inspection_Data.Add(StructData.SuYul.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 7).ToString("000"), StructData.SuYul.ToString("0.000"));
                    }

                    if (OkNg_Data == false)
                    {
                        //평균값 기존데이터로 처리
                        string tmpAvr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"));
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 8).ToString("000")));
                        //표준편차 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 9).ToString("000")));
                        //최소 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 10).ToString("000")));
                        //최대 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 11).ToString("000")));
                        //CP 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 12).ToString("000")));
                        //CPK 값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Uper,(i*mesColCount + 13).ToString("000")));
                        continue;
                    }

                    //현재 평균 값을 적용합니다.
                    StructData.NowAvr = 0f;
                    StructData.OldValue = 0f;
                    if (NowPassNumber_Uper == 1)
                    {
                        StructData.NowAvr = (float) dResultValue_Uper;
                        InspectData.Inspection_Data.Add(StructData.NowAvr.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                        StructData.OldValue = (float) dResultValue_Uper;
                        StructData.OldSquare = (float) dResultValue_Uper*(float) dResultValue_Uper;

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                    }
                    else if (NowPassNumber_Uper > 1)
                    {
                        StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"));
                        StructData.OldValue = float.Parse(StructData.OldValuestr);

                        StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"));
                        StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                        StructData.NowAvr = (float) ((StructData.OldValue + (float) dResultValue_Uper)/(float) NowPassNumber_Uper);
                        InspectData.Inspection_Data.Add(StructData.NowAvr.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                        StructData.OldSquare = (StructData.OldSquare) + ((float) dResultValue_Uper*(float) dResultValue_Uper);
                        StructData.OldValue = (StructData.OldValue + (float) dResultValue_Uper);

                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                    }

                    //현재 표준편차을 적용하니다. nowOkCount
                    StructData.OldStdDev = 0f;
                    StructData.NowStdDev = 0f;
                    StructData.StdDivArray = new float[2];

                    if (StructData.nowOkCount <= 1)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 9).ToString("000"), "0.000");
                    }
                    else if (StructData.nowOkCount > 1)
                    {
                        int nCount = (int) NowPassNumber_Uper;
                        StructData.NowStdDev =(float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                        InspectData.Inspection_Data.Add(StructData.NowStdDev.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 9).ToString("000"), StructData.NowStdDev.ToString("0.000"));
                    }

                    //현재 최소 값을 적용하니다.

                    
                    StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 10).ToString("000"));
                    StructData.OldMin = float.Parse(StructData.OldMinstr);
                    StructData.NowMin = Control_CPKData.Min((float) dResultValue_Uper, StructData.OldMin);

                    
                    if (StructData.OldMin < 0)
                        MessageBox.Show("기존값 : " + dResultValue_Uper.ToString());
                    if (dResultValue_Uper < 0)
                        MessageBox.Show("측정값 : " + dResultValue_Uper.ToString());
                    if (StructData.NowMin < 0) 
                        MessageBox.Show("현재값 : " + StructData.NowMin.ToString());
                    

                    InspectData.Inspection_Data.Add(StructData.NowMin.ToString("0.000"));

                    //시작 106 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                    //종료 106 : 레지로 수정된 코드

                    //현재 최대 값을 적용합니다.
                    StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 11).ToString("000"));
                    StructData.OldMax = float.Parse(StructData.OldMaxstr);
                    StructData.NowMax = Control_CPKData.Max((float) dResultValue_Uper, StructData.OldMax);
                    InspectData.Inspection_Data.Add(StructData.NowMax.ToString("0.000"));

                    //시작 107 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                    //종료 107 : 레지로 수정된 코드

                    //현재 CP 값을 적용하니다. 
                    StructData.NowCP = 0f;
                    if (StructData.nowOkCount + StructData.nowNgCount < 2)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 12).ToString("000"), "0.000");
                    }
                    else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                    {
                        StructData.NowCP = Control_CPKData.Cp((float) StructData.uperMaxValue,(float) StructData.uperMinValue, StructData.NowStdDev);
                        InspectData.Inspection_Data.Add(StructData.NowCP.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 12).ToString("000"),StructData.NowCP.ToString("0.000"));
                    }

                    //현재 CPK 값을 적용합니다.
                    StructData.NowCPKU = 0f;
                    StructData.NowCPKL = 0f;
                    StructData.NowCPK = 0f;
                    if (StructData.nowOkCount + StructData.nowNgCount < 2)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 13).ToString("000"), "0.000");
                    }
                    else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                    {
                        StructData.NowCPKU = Control_CPKData.CpkU((float) StructData.uperMaxValue, StructData.NowAvr, StructData.NowStdDev);
                        StructData.NowCPKL = Control_CPKData.CpkL((float) StructData.uperMinValue, StructData.NowAvr, StructData.NowStdDev);
                        StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                        InspectData.Inspection_Data.Add(StructData.NowCPK.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i*mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                    }
                }

                return InspectData;
            }
            catch (Exception e)
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + e.Message);
                return InspectData;
            }
        }
        */

        public struct Display_FindData_Struct
        {
            public Inspection_Data_List InspectData;
            public int MeasureGrid_Col_Count;// = 13;
            public int MeasureGrid_Rows;// = InspectData.Inspection_Data.Count / MeasureGrid_Col_Count;
            public UltraDataRow dataRow01;
            public UltraDataRow dataRow02;
        }

        public delegate void Delegate_Run_Run_FindData_Display_Uper(object Grid_Struct_Data);


        public void Inspect_Run_Run_FindData_Display_Uper(object Grid_Struct_Data)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Display_Uper del = Inspect_Run_Run_FindData_Display_Uper;
                Invoke(del, Grid_Struct_Data);
            }
            else
            {
                try
                {
                    Inspection_Grid_Data[] grid_Struct = (Inspection_Grid_Data[])Grid_Struct_Data;

                    Display_FindData_Struct struct_DataDisp_Uper = new Display_FindData_Struct();
                    dataSet1.Tables["Meas"].Rows.Clear();
                    int mesColCount = 19;
                    
                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    //그리드 공란을 채운다.
                    //struct_DataDisp_Uper.MeasureGrid_Col_Count = 13;
                    //struct_DataDisp_Uper.MeasureGrid_Rows = struct_DataDisp_Uper.InspectData.Inspection_Data.Count / struct_DataDisp_Uper.MeasureGrid_Col_Count;
                    for (int i = 0; i < grid_Struct.Count(); i++)
                    {
                        DataRow dr = dataSet1.Tables["Meas"].NewRow();

                        dr["NO"] = (i+1).ToString("0");
                        dr["항목"] = grid_Struct[i].readName;
                        dr["중심"] = grid_Struct[i].CenValue;
                        dr["상한"] = grid_Struct[i].MaxValue;
                        dr["하한"] = grid_Struct[i].MinValue;
                        dr["측정"] = grid_Struct[i].dResultValueStr;
                        dr["판정"] = grid_Struct[i].itemResult;
                        dr["NG"] = grid_Struct[i].nowNgCount.ToString("0");
                        dr["수율"] = grid_Struct[i].SuYul.ToString("0.00");
                        dr["평균"] = grid_Struct[i].NowAvr.ToString("0.00");
                        dr["편차"] = grid_Struct[i].NowStdDev.ToString("0.00");
                        dr["최소"] = grid_Struct[i].NowMin.ToString("0.00");
                        dr["최대"] = grid_Struct[i].NowMax.ToString("0.00");
                        dr["CP"] = grid_Struct[i].NowCP.ToString("0.00");
                        dr["CPK"] = grid_Struct[i].NowCPK.ToString("0.00");
                        

//                         dr["NO"] = i.ToString("0");
//                         dr["검사 항목"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 0];
//                         dr["규격 중심"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 1];
//                         dr["규격 상한"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 2];
//                         dr["규격 하한"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 3];
//                         dr["측정 값"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 4];
//                         dr["판정 결과"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 5];
//                         dr["수율"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 6];
//                         dr["평균"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 7];
//                         dr["표준 편차"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 8];
//                         dr["최소 값"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 9];
//                         dr["최대 값"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 10];
//                         dr["CP 값"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 11];
//                         dr["CPK 값"] = struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 12];
//                         dr["NgCount"] = (int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"))).ToString("0000"));
                        dataSet1.Tables["Meas"].Rows.Add(dr);

                        if (grid_Struct[i].itemResult == "NG")
                        {
                            if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor != Color.OrangeRed)
                                uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                        }
                        else
                        {
                            if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor != Color.White)
                                uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;
                        }
                    }
                    Measurement_Grid_Resize(uGrd_Inspect_Measure_Uper);
                    
                    /*
                    int mesColCount = 19;
                    struct_DataDisp_Uper.InspectData = (Inspection_Data_List)Inspect_Data;
                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    //그리드 공란을 채운다.
                    struct_DataDisp_Uper.MeasureGrid_Col_Count = 13;
                    struct_DataDisp_Uper.MeasureGrid_Rows = struct_DataDisp_Uper.InspectData.Inspection_Data.Count / struct_DataDisp_Uper.MeasureGrid_Col_Count;
                    for (int i = 0; i < struct_DataDisp_Uper.MeasureGrid_Rows; i++)
                    {
                        struct_DataDisp_Uper.dataRow01 = uDS_Inspect_Measure_Uper.Rows.Add();

                        struct_DataDisp_Uper.dataRow01.SetCellValue("NO", uDS_Inspect_Measure_Uper.Rows.Count.ToString("0"));
                        struct_DataDisp_Uper.dataRow01.SetCellValue("검사 항목", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 0]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("규격 중심", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 1]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("규격 상한", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 2]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("규격 하한", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 3]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("측정 값", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 4]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("판정 결과", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 5]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("수율", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 6]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("평균", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 7]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("표준 편차", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 8]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("최소 값", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 9]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("최대 값", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 10]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("CP 값", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 11]);
                        struct_DataDisp_Uper.dataRow01.SetCellValue("CPK 값", struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 12]);


                        //엔지 카운트에서 진행 해야할 라인이다.
                        struct_DataDisp_Uper.dataRow01.SetCellValue("NgCount", (int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"))).ToString("0000")));

                        if (struct_DataDisp_Uper.InspectData.Inspection_Data[i * struct_DataDisp_Uper.MeasureGrid_Col_Count + 5] == "NG")
                        {
                            uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                        }
                    }

                    if (struct_DataDisp_Uper.MeasureGrid_Rows < 11)
                    {
                        for (int i = 0; i < 10 - struct_DataDisp_Uper.MeasureGrid_Rows; i++)
                        {
                            struct_DataDisp_Uper.dataRow02 = uDS_Inspect_Measure_Uper.Rows.Add();
                            struct_DataDisp_Uper.dataRow02.SetCellValue("NO", uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0"));
                        }
                    }
                    */
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    throw;
                }
            }
        }



        public delegate void Delegate_Run_Run_FindData_Display_Down(object Grid_Struct_Data);
        public void Inspect_Run_Run_FindData_Display_Down(object Grid_Struct_Data)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Display_Down del = Inspect_Run_Run_FindData_Display_Down;
                Invoke(del, Grid_Struct_Data);
            }
            else
            {
                try
                {
                    Inspection_Grid_Data[] grid_Struct = (Inspection_Grid_Data[])Grid_Struct_Data;

                    dataSet2.Tables["Meas"].Rows.Clear();
                    //int mesColCount = 19;

                    //uDS_Inspect_Measure_Uper.Rows.Clear();
                    //그리드 공란을 채운다.
                    //struct_DataDisp_Uper.MeasureGrid_Col_Count = 13;
                    //struct_DataDisp_Uper.MeasureGrid_Rows = struct_DataDisp_Uper.InspectData.Inspection_Data.Count / struct_DataDisp_Uper.MeasureGrid_Col_Count;
                    for (int i = 0; i < grid_Struct.Count(); i++)
                    {
                        DataRow dr = dataSet2.Tables["Meas"].NewRow();

                        dr["NO"] = (i + 1).ToString("0");
                        dr["항목"] = grid_Struct[i].readName;
                        dr["중심"] = grid_Struct[i].CenValue;
                        dr["상한"] = grid_Struct[i].MaxValue;
                        dr["하한"] = grid_Struct[i].MinValue;
                        dr["측정"] = grid_Struct[i].dResultValueStr;
                        dr["판정"] = grid_Struct[i].itemResult;
                        dr["NG"] = grid_Struct[i].nowNgCount.ToString("0");
                        dr["수율"] = grid_Struct[i].SuYul.ToString("0.00");
                        dr["평균"] = grid_Struct[i].NowAvr.ToString("0.00");
                        dr["편차"] = grid_Struct[i].NowStdDev.ToString("0.00");
                        dr["최소"] = grid_Struct[i].NowMin.ToString("0.00");
                        dr["최대"] = grid_Struct[i].NowMax.ToString("0.00");
                        dr["CP"] = grid_Struct[i].NowCP.ToString("0.00");
                        dr["CPK"] = grid_Struct[i].NowCPK.ToString("0.00");
                        
                        dataSet2.Tables["Meas"].Rows.Add(dr);

                        if (grid_Struct[i].itemResult == "NG")
                        {
                            if (uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor != Color.OrangeRed)
                                uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                        }
                        else
                        {
                            if (uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor != Color.White)
                                uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;
                        }
                    }
                    Measurement_Grid_Resize(uGrd_Inspect_Measure_Down);
                    /*
                    Display_FindData_Struct struct_DataDisp_Down = new Display_FindData_Struct();
                    int mesColCount = 19;
                    struct_DataDisp_Down.InspectData = (Inspection_Data_List)Inspect_Data;
                    uDS_Inspect_Measure_Down.Rows.Clear();
                    //그리드 공란을 채운다.
                    struct_DataDisp_Down.MeasureGrid_Col_Count = 13;
                    struct_DataDisp_Down.MeasureGrid_Rows = struct_DataDisp_Down.InspectData.Inspection_Data.Count / struct_DataDisp_Down.MeasureGrid_Col_Count;
                    for (int i = 0; i < struct_DataDisp_Down.MeasureGrid_Rows; i++)
                    {
                        struct_DataDisp_Down.dataRow01 = uDS_Inspect_Measure_Down.Rows.Add();

                        struct_DataDisp_Down.dataRow01.SetCellValue("NO", uDS_Inspect_Measure_Down.Rows.Count.ToString("0"));
                        struct_DataDisp_Down.dataRow01.SetCellValue("검사 항목", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 0]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("규격 중심", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 1]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("규격 상한", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 2]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("규격 하한", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 3]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("측정 값", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 4]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("판정 결과", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 5]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("수율", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 6]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("평균", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 7]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("표준 편차", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 8]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("최소 값", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 9]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("최대 값", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 10]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("CP 값", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 11]);
                        struct_DataDisp_Down.dataRow01.SetCellValue("CPK 값", struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 12]);

                        struct_DataDisp_Down.dataRow01.SetCellValue("NgCount", (int.Parse(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 15).ToString("000"))).ToString("0000")));

                        if (struct_DataDisp_Down.InspectData.Inspection_Data[i * struct_DataDisp_Down.MeasureGrid_Col_Count + 5] == "NG")
                        {
                            uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                        }
                    }

                    if (struct_DataDisp_Down.MeasureGrid_Rows < 11)
                    {
                        for (int i = 0; i < 10 - struct_DataDisp_Down.MeasureGrid_Rows; i++)
                        {
                            struct_DataDisp_Down.dataRow02 = uDS_Inspect_Measure_Down.Rows.Add();
                            struct_DataDisp_Down.dataRow02.SetCellValue("NO", uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0"));
                        }
                    }
                    */

                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    throw;
                }
            }
        }

        /*

        public void Inspect_Run_Run_FindData_Display_Uper(object Inspect_Data)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Display_Uper del = Inspect_Run_Run_FindData_Display_Uper;
                Invoke(del, Inspect_Data);
            }
            else
            {
                try
                {
                    Inspection_Data_List InspectData = (Inspection_Data_List) Inspect_Data;
                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    //그리드 공란을 채운다.
                    int MeasureGrid_Col_Count = 13;
                    int MeasureGrid_Rows = InspectData.Inspection_Data.Count/MeasureGrid_Col_Count;
                    for (int i = 0; i < MeasureGrid_Rows; i++)
                    {
                        UltraDataRow dataRow = uDS_Inspect_Measure_Uper.Rows.Add();

                        dataRow.SetCellValue("NO", uDS_Inspect_Measure_Uper.Rows.Count.ToString("0"));
                        dataRow.SetCellValue("검사 항목", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 0]);
                        dataRow.SetCellValue("규격 중심", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 1]);
                        dataRow.SetCellValue("규격 상한", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 2]);
                        dataRow.SetCellValue("규격 하한", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 3]);
                        dataRow.SetCellValue("측정 값", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 4]);
                        dataRow.SetCellValue("판정 결과", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 5]);
                        dataRow.SetCellValue("수율", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 6]);
                        dataRow.SetCellValue("평균", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 7]);
                        dataRow.SetCellValue("표준 편차", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 8]);
                        dataRow.SetCellValue("최소 값", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 9]);
                        dataRow.SetCellValue("최대 값", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 10]);
                        dataRow.SetCellValue("CP 값", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 11]);
                        dataRow.SetCellValue("CPK 값", InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 12]);

                        if (InspectData.Inspection_Data[i*MeasureGrid_Col_Count + 5] == "NG")
                        {
                            uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                        }
                    }

                    if (MeasureGrid_Rows < 11)
                    {
                        for (int i = 0; i < 10 - MeasureGrid_Rows; i++)
                        {
                            UltraDataRow dataRow = uDS_Inspect_Measure_Uper.Rows.Add();
                            dataRow.SetCellValue("NO", uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0"));
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    throw;
                }
            }
        }
        
        public void Inspect_Run_Run_FindData_Inspection_Uper()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Inspection_Uper del = Inspect_Run_Run_FindData_Inspection_Uper;
                Invoke(del);
            }
            else
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();

                string pointData = string.Empty;

                int mesColCount = 19;
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
                //Test Function
                //Inspect_Test_Log_Write(pointData, 1, textBox15);
                Random r = new Random();


                try
                {
                    Stopwatch testFunc = new Stopwatch();
                    testFunc.Reset();
                    testFunc.Start();
                    bool OkNg_Data = Inspect_Run_Run_FindData_OkNg_Uper();
                    testFunc.Stop();
                    MessageBox.Show(testFunc.ElapsedMilliseconds.ToString());

                    uDS_Inspect_Measure_Uper.Rows.Clear();
                    StructData.RecipeColumn = 11;
                    StructData.VisionColumn = 8;
                    StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / StructData.RecipeColumn;

                    for (int i = 0; i < StructData.RcpGrdRows; i++)
                    {
                        StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                        StructData.readName = LamiSystem.StrLstRcpConGridData_Uper[i * StructData.RecipeColumn];

                        int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / StructData.VisionColumn;
                        for (int j = 0; j < VisGrdRows; j++)
                        {
                            string VisName = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn];
                            if (StructData.readName == VisName)
                            {
                                StructData.CenValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 1];
                                StructData.MaxValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 2];
                                StructData.MinValue = LamiSystem.StrListVisConGridData_Uper[j * StructData.VisionColumn + 3];

                                StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                                StructData.row.Cells["검사 항목"].Value = StructData.readName;
                                StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                                //StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                                //StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);
                                StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                                StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");

                                //비전에서 설정한 아이템의 켈값을 적용한다.
                                //StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 6]);
                                //StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                                //MeasureData_Uper
                                dResultValue_Uper = double.Parse(MeasureData_Uper[i]);

                                StructData.strOkCount = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"));
                                StructData.nowOkCount = int.Parse(StructData.strOkCount);

                                StructData.strNgCount = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"));
                                StructData.nowNgCount = int.Parse(StructData.strNgCount);

                                StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 3).ToString("000"));
                                StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                                StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 4).ToString("000"));
                                StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);
                               
                                break;
                            }
                        }

                        string NowUsed = sLstNowDisNo_Uper[i * 2];
                        if (NowUsed == "False")
                        {
                            StructData.row.Cells["측정 값"].Value = "0.00";
                            StructData.row.Cells["판정 결과"].Value = "NO";
                            StructData.row.Cells["수율"].Value = "0.000";
                            StructData.row.Cells["평균"].Value = "0.000";
                            StructData.row.Cells["표준 편차"].Value = "0.000";
                            StructData.row.Cells["최소 값"].Value = "0.000";
                            StructData.row.Cells["최대 값"].Value = "0.000";
                            StructData.row.Cells["CP 값"].Value = "0.000";
                            StructData.row.Cells["CPK 값"].Value = "0.000";
                            StructData.row.Cells["OkCount"].Value = "0";
                            StructData.row.Cells["NgCount"].Value = "0";
                            StructData.row.Cells["SumValue"].Value = "0.0";
                            StructData.row.Cells["SquValue"].Value = "0.0";
                            StructData.row.Cells["ProductOK"].Value = "0.0";
                            continue;
                        }


                        if (cvPntLstImagePoint_Uper[i*2].X == 0 || cvPntLstImagePoint_Uper[i*2].Y == 0 || cvPntLstImagePoint_Uper[i*2 + 1].X == 0 || cvPntLstImagePoint_Uper[i*2 + 1].Y == 0)
                        {
                            //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                            _strSavedInspectResult_Uper = "NG";
                            StructData.row.Cells["판정 결과"].Value = "NG";
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "NG");
                            StructData.row.Cells["NgCount"].Value = StructData.nowNgCount.ToString("0");

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 5).ToString("000"), "0.000");
                            StructData.row.Cells["측정 값"].Value = "0.000";

                            StructData.row.Appearance.BackColor = Color.OrangeRed;
                        }
                        else
                        {
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 5).ToString("000"), dResultValue_Uper.ToString("0.000"));
                            StructData.row.Cells["측정 값"].Value = dResultValue_Uper.ToString("0.000");

                            //현재 측정값이 양품인지 검사한다. itemResult_Uper
                            //if (StructData.uperMaxValue > dResultValue_Uper && StructData.uperMinValue < dResultValue_Uper)
                            if (itemResult_Uper[i] == "OK")
                            {
                                _strSavedInspectResult_Uper = "OK";
                                StructData.row.Cells["판정 결과"].Value = "OK";
                                StructData.nowOkCount = StructData.nowOkCount + 1;
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "OK");
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"), StructData.nowOkCount.ToString("0"));
                                
                                StructData.row.Cells["OkCount"].Value = (StructData.nowOkCount).ToString("0");
                            }
                            else
                            {
                                _strSavedInspectResult_Uper = "NG";
                                StructData.row.Cells["판정 결과"].Value = "NG";
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 6).ToString("000"), "NG");
                                StructData.nowNgCount = StructData.nowNgCount + 1;
                                this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                                StructData.row.Cells["NgCount"].Value = (StructData.nowNgCount).ToString("0");

                                StructData.row.Appearance.BackColor = Color.OrangeRed;
                            }
                        }


                        StructData.SuYul = 0f;
                        //현재 수율을 적용한다.
                        if (StructData.nowOkCount == 0) StructData.row.Cells["수율"].Value = "0.000";
                        else
                        {
                            StructData.OkData = (float)StructData.nowOkCount;
                            StructData.NgData = (float)StructData.nowNgCount;
                            StructData.SuYul = (float)(StructData.OkData / (StructData.OkData + StructData.NgData)) * 100;
                            StructData.row.Cells["수율"].Value = StructData.SuYul.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 7).ToString("000"), StructData.SuYul.ToString("0.000"));
                        }

                        if (OkNg_Data == false)
                        {
                            //평균값 기존데이터로 처리
                            StructData.row.Cells["평균"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"));
                            //표준편차 기존데이터로 처리
                            StructData.row.Cells["표준 편차"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000"));
                            //최소 값 기존데이터로 처리
                            StructData.row.Cells["최소 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"));
                            //최대 값 기존데이터로 처리
                            StructData.row.Cells["최대 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"));
                            //CP 값 기존데이터로 처리
                            StructData.row.Cells["CP 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000"));
                            //CPK 값 기존데이터로 처리
                            StructData.row.Cells["CPK 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000"));
                            continue;
                        }

                        //현재 평균 값을 적용합니다.
                        StructData.NowAvr = 0f;
                        StructData.OldValue = 0f;
                        if (NowPassNumber_Uper == 1)
                        {
                            StructData.NowAvr = (float)dResultValue_Uper;
                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            StructData.OldValue = (float) dResultValue_Uper;
                            StructData.OldSquare = (float) dResultValue_Uper*(float) dResultValue_Uper;

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        }
                        else if (NowPassNumber_Uper > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            StructData.NowAvr = (float)((StructData.OldValue + (float)dResultValue_Uper) / (float)NowPassNumber_Uper);
                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            StructData.OldSquare = (StructData.OldSquare) + ((float)dResultValue_Uper * (float)dResultValue_Uper);
                            StructData.OldValue = (StructData.OldValue + (float) dResultValue_Uper);

                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        }


                        //현재 표준편차을 적용하니다. nowOkCount
                        StructData.OldStdDev = 0f;
                        StructData.NowStdDev = 0f;
                        StructData.StdDivArray = new float[2];
                        if (StructData.nowOkCount <= 1) StructData.row.Cells["표준 편차"].Value = "0.000";
                        else if (StructData.nowOkCount > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);
                            
                            int nCount = StructData.nowOkCount;//+ StructData.nowNgCount;

                            StructData.NowStdDev =(float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                            StructData.row.Cells["표준 편차"].Value = StructData.NowStdDev.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000"), StructData.NowStdDev.ToString("0.000"));
                        }

                        //현재 최소 값을 적용하니다.
                        StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"));
                        StructData.OldMin = float.Parse(StructData.OldMinstr);
                        StructData.NowMin = Control_CPKData.Min((float)dResultValue_Uper, StructData.OldMin);
                        StructData.row.Cells["최소 값"].Value = StructData.NowMin.ToString("0.000");
                        //시작 106 : 레지로 수정된 코드
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                        //종료 106 : 레지로 수정된 코드

                        //현재 최대 값을 적용합니다.
                        StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"));
                        StructData.OldMax = float.Parse(StructData.OldMaxstr);
                        StructData.NowMax = Control_CPKData.Max((float)dResultValue_Uper, StructData.OldMax);
                        StructData.row.Cells["최대 값"].Value = StructData.NowMax.ToString("0.000");

                        //시작 107 : 레지로 수정된 코드
                        this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                        //종료 107 : 레지로 수정된 코드

                        //현재 CP 값을 적용하니다. 
                        StructData.NowCP = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount < 2) StructData.row.Cells["CP 값"].Value = "0.000";
                        else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                        {
                            StructData.NowCP = Control_CPKData.Cp((float)StructData.uperMaxValue, (float)StructData.uperMinValue, StructData.NowStdDev);
                            StructData.row.Cells["CP 값"].Value = StructData.NowCP.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000"), StructData.NowCP.ToString("0.000"));
                        }

                        //현재 CPK 값을 적용합니다.
                        StructData.NowCPKU = 0f; // Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                        StructData.NowCPKL = 0f; // Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                        StructData.NowCPK = 0f; // Control_CPKData.Cpk(NowCPKU, NowCPKL);
                        if (StructData.nowOkCount + StructData.nowNgCount < 2) StructData.row.Cells["CPK 값"].Value = "0.000";
                        else if (StructData.nowOkCount + StructData.nowNgCount >= 2 )
                        {
                            StructData.NowCPKU = Control_CPKData.CpkU((float)StructData.uperMaxValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPKL = Control_CPKData.CpkL((float)StructData.uperMinValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                            StructData.row.Cells["CPK 값"].Value = StructData.NowCPK.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                        }

                    }

                    //그리드 공란을 채운다.
                    if (StructData.RcpGrdRows < 11)
                    {
                        for (int i = 0; i < 10 - StructData.RcpGrdRows; i++)
                        {
                            StructData.row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();
                            StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                        }
                    }
                }
                catch (Exception e)
                {
                    //Inspect_Test_Log_Write(pointData, 2, textBox16);
                    //MessageBox.Show(MethodBase.GetCurrentMethod().Name + _iImgCount.ToString("0") + " " + e.Message);
                    throw;
                }
            }
        }

        Display_FindData_Struct struct_DataDisp_Uper = new Display_FindData_Struct();
        

        public delegate void Delegate_Run_Run_FindData_Display_Down(object Inspect_Data);
        public void Inspect_Run_Run_FindData_Display_Down(object Inspect_Data)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Display_Down del = Inspect_Run_Run_FindData_Display_Down;
                Invoke(del, Inspect_Data);
            }
            else
            {
                try
                {
                    Inspection_Data_List InspectData = (Inspection_Data_List)Inspect_Data;
                    uDS_Inspect_Measure_Down.Rows.Clear();
                    //그리드 공란을 채운다.
                    int MeasureGrid_Col_Count = 13;
                    int MeasureGrid_Rows = InspectData.Inspection_Data.Count / MeasureGrid_Col_Count;
                    for (int i = 0; i < MeasureGrid_Rows; i++)
                    {
                        UltraDataRow dataRow = uDS_Inspect_Measure_Down.Rows.Add();

                        dataRow.SetCellValue("NO", uDS_Inspect_Measure_Down.Rows.Count.ToString("0"));
                        dataRow.SetCellValue("검사 항목", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 0]);
                        dataRow.SetCellValue("규격 중심", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 1]);
                        dataRow.SetCellValue("규격 상한", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 2]);
                        dataRow.SetCellValue("규격 하한", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 3]);
                        dataRow.SetCellValue("측정 값", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 4]);
                        dataRow.SetCellValue("판정 결과", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 5]);
                        dataRow.SetCellValue("수율", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 6]);
                        dataRow.SetCellValue("평균", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 7]);
                        dataRow.SetCellValue("표준 편차", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 8]);
                        dataRow.SetCellValue("최소 값", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 9]);
                        dataRow.SetCellValue("최대 값", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 10]);
                        dataRow.SetCellValue("CP 값", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 11]);
                        dataRow.SetCellValue("CPK 값", InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 12]);

                        if (InspectData.Inspection_Data[i * MeasureGrid_Col_Count + 5] == "NG")
                        {
                            uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                        }
                    }

                    if (MeasureGrid_Rows < 11)
                    {
                        for (int i = 0; i < 10 - MeasureGrid_Rows; i++)
                        {
                            UltraDataRow dataRow = uDS_Inspect_Measure_Down.Rows.Add();
                            dataRow.SetCellValue("NO", uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0"));
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    throw;
                }
            }
        }
        */

        public bool Inspect_Run_Run_FindData_OkNg_Down()
        {
            bool OkNg_Result = true;
            Inspection_Grid_Data StructData = new Inspection_Grid_Data();

            int mesColCount = 19;
            Random r = new Random();

            double MeasureValue_Down = 0d;

            StructData.RecipeColumn = 11;
            StructData.VisionColumn = 8;
            StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Down.Count / StructData.RecipeColumn;

            MeasureData_Down.Clear();
            itemResult_Down.Clear();

            for (int i = 0; i < StructData.RcpGrdRows; i++)
            {
                StructData.readName = LamiSystem.StrLstRcpConGridData_Down[i * StructData.RecipeColumn];
                MeasureValue_Down = 0d;

                int VisGrdRows = LamiSystem.StrListVisConGridData_Down.Count / StructData.VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn];
                    if (StructData.readName == VisName)
                    {
                        StructData.CenValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 1];
                        StructData.MaxValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 2];
                        StructData.MinValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 3];

                        StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                        StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                        //비전에서 설정한 아이템의 켈값을 적용한다.
                        StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Down[(j * StructData.VisionColumn) + 6]);
                        StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Down[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                        break;
                    }
                }

                //레시피 그리드이 판별이 체크가 되어 있지 않은 경우
                string NowUsed = sLstNowDisNo_Down[i * 2];
                //if (NowUsed == "False")
                //{
                //    itemResult_Down.Add("NO");
                //    MeasureData_Down.Add("0.000");
                //    continue;
                //}

                //20150217 WKB 207
                //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                //20150217 WKB 208
                //float tmpValue1 = (float)(r.Next(-100, 100) / 10000f);
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);
                
                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                StructData.SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Down[i * 11 + 3]);
                if (StructData.SideNo % 2 == 0)
                {
                    iImgPixResultData_Down = Math.Abs(cvPntLstImagePoint_Down[i * 2 + 1].Y - cvPntLstImagePoint_Down[i * 2].Y);
                    MeasureValue_Down = ((double)iImgPixResultData_Down * (double)Cal_Sero_Down) + tmpValue;
                }
                //가로 일때 진행됨
                else
                {
                    iImgPixResultData_Down = Math.Abs(cvPntLstImagePoint_Down[i * 2 + 1].X - cvPntLstImagePoint_Down[i * 2].X);
                    MeasureValue_Down = ((double)iImgPixResultData_Down * (double)Cal_Garo_Down) + tmpValue;
                }


                StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 3).ToString("000"));
                StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 4).ToString("000"));
                StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                //측정 포인트를 찾지 못했을 경우
                if (cvPntLstImagePoint_Down[i * 2].X == 0 || cvPntLstImagePoint_Down[i * 2].Y == 0 || cvPntLstImagePoint_Down[i * 2 + 1].X == 0 || cvPntLstImagePoint_Down[i * 2 + 1].Y == 0)
                {
                    itemResult_Down.Add("NG");
                    
                    //20150226 WKB 207
                    //MeasureData_Down.Add("0.000");

                    //20150226 WKB 208
                    MeasureData_Down.Add("0.00000");

                    if (NowUsed != "False") OkNg_Result = false;
                }
                else
                {

                    if (StructData.VisionItem_CalUsed == true)
                        MeasureValue_Down = MeasureValue_Down * StructData.VisionItem_CalValue;

                    //측정값을 보상을 적용한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        MeasureValue_Down = Inspect_RunRun_MeasureData_Editing(MeasureValue_Down, StructData.CenValue, StructData.MaxValue, StructData.MinValue);
                    }

                    //현재 측정값이 양품인지 검사한다. 
                    if (StructData.MaxData > MeasureValue_Down && StructData.MinData < MeasureValue_Down)
                    {
                        itemResult_Down.Add("OK");

                        //20150226 WKB 207
                        //MeasureData_Down.Add(MeasureValue_Down.ToString("0.000"));

                        //20150226 WKB 208
                        MeasureData_Down.Add(MeasureValue_Down.ToString("0.00000"));
                    }
                    else
                    {
                        itemResult_Down.Add("NG");

                        //20150226 WKB 207
                        //MeasureData_Down.Add(MeasureValue_Down.ToString("0.000"));

                        //20150226 WKB 208
                        MeasureData_Down.Add(MeasureValue_Down.ToString("0.00000"));

                        if (NowUsed != "False") OkNg_Result = false;
                    }
                }
            }

            if (OkNg_Result == true)
            {
                NowPassNumber_Down = uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_OK_Down"));
                NowPassNumber_Down = NowPassNumber_Down + 1;

                this.SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Down", NowPassNumber_Down.ToString("0"));
            }
            return OkNg_Result;
        }




        List<string> itemResult_Down = new List<string>();
        List<string> MeasureData_Down = new List<string>();

        public void Inspect_Run_Run_FindData_Inspection_Down()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_FindData_Inspection_Down del = Inspect_Run_Run_FindData_Inspection_Down;
                Invoke(del);
            }
            else
            {
                Inspection_Grid_Data StructData = new Inspection_Grid_Data();

                string pointData = string.Empty;

                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
                //Test Function
                //Inspect_Test_Log_Write(pointData, 1, textBox15);
                Random r = new Random();
                int mesColCount = 19;
                try
                {
                    bool OkNg_Data = Inspect_Run_Run_FindData_OkNg_Down();

                    if (OkNg_Data == true) _strSavedInspectResult_Down = "OK";
                    else _strSavedInspectResult_Down = "NG";

                    uDS_Inspect_Measure_Down.Rows.Clear();
                    StructData.RecipeColumn = 11;
                    StructData.VisionColumn = 8;
                    StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Down.Count / StructData.RecipeColumn;

                    for (int i = 0; i < StructData.RcpGrdRows; i++)
                    {
                        StructData.row = this.uGrd_Inspect_Measure_Down.DisplayLayout.Bands[0].AddNew();

                        StructData.readName = LamiSystem.StrLstRcpConGridData_Down[i * StructData.RecipeColumn];

                        int VisGrdRows = LamiSystem.StrListVisConGridData_Down.Count / StructData.VisionColumn;
                        for (int j = 0; j < VisGrdRows; j++)
                        {
                            string VisName = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn];
                            if (StructData.readName == VisName)
                            {
                                StructData.CenValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 1];
                                StructData.MaxValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 2];
                                StructData.MinValue = LamiSystem.StrListVisConGridData_Down[j * StructData.VisionColumn + 3];

                                StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0");
                                strLstDisplayData_Down.Add(uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0"));

                                StructData.row.Cells["검사 항목"].Value = StructData.readName;
                                strLstDisplayData_Down.Add(StructData.readName);

                                StructData.row.Cells["규격 중심"].Value = StructData.CenValue;
                                strLstDisplayData_Down.Add(StructData.CenValue);

                                StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                                StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                                StructData.row.Cells["규격 상한"].Value = StructData.MaxData.ToString("0.00");
                                strLstDisplayData_Down.Add(StructData.MaxData.ToString("0.00"));

                                StructData.row.Cells["규격 하한"].Value = StructData.MinData.ToString("0.00");
                                strLstDisplayData_Down.Add(StructData.MinData.ToString("0.00"));

                                //비전에서 설정한 아이템의 켈값을 적용한다.
                                StructData.VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Down[(j * StructData.VisionColumn) + 6]);
                                StructData.VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Down[(j * StructData.VisionColumn) + 7] == "True") ? true : false;

                                dResultValue_Down = double.Parse(MeasureData_Down[i]);

                                StructData.strOkCount = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 14).ToString("000"));
                                StructData.nowOkCount = int.Parse(StructData.strOkCount);

                                StructData.strNgCount = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 15).ToString("000"));
                                StructData.nowNgCount = int.Parse(StructData.strNgCount);

                                StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 3).ToString("000"));
                                StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                                StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 4).ToString("000"));
                                StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);
                                
                                break;
                            }
                        }

                        string NowUsed = sLstNowDisNo_Down[i*2];
//                         if (NowUsed == "False")
//                         {
//                             StructData.row.Cells["측정 값"].Value = "0.00";
//                             StructData.row.Cells["판정 결과"].Value = "NO";
//                             StructData.row.Cells["수율"].Value = "0.000";
//                             StructData.row.Cells["평균"].Value = "0.000";
//                             StructData.row.Cells["표준 편차"].Value = "0.000";
//                             StructData.row.Cells["최소 값"].Value = "0.000";
//                             StructData.row.Cells["최대 값"].Value = "0.000";
//                             StructData.row.Cells["CP 값"].Value = "0.000";
//                             StructData.row.Cells["CPK 값"].Value = "0.000";
//                             StructData.row.Cells["OkCount"].Value = "0";
//                             StructData.row.Cells["NgCount"].Value = "0";
//                             StructData.row.Cells["SumValue"].Value = "0.0";
//                             StructData.row.Cells["SquValue"].Value = "0.0";
//                             StructData.row.Cells["ProductOK"].Value = "0.0";
//                             continue;
//                         }
                        
                        if (cvPntLstImagePoint_Down[i * 2].X == 0 || cvPntLstImagePoint_Down[i * 2].Y == 0 || cvPntLstImagePoint_Down[i * 2 + 1].X == 0 || cvPntLstImagePoint_Down[i * 2 + 1].Y == 0)
                        {
                            //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                            StructData.row.Cells["측정 값"].Value = "0.000";

                            //_strSavedInspectResult_Down = "NG";
                            StructData.row.Cells["판정 결과"].Value = "NG";

                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            StructData.row.Cells["NgCount"].Value = StructData.nowNgCount.ToString("0");
                           
                            //측정 값
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 5).ToString("000"), "0.000");
                            //측정 결과
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 6).ToString("000"), "NG");
                            //NgCount
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));

                            StructData.row.Appearance.BackColor = Color.OrangeRed;
                        }
                        else
                        {
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 5).ToString("000"), dResultValue_Down.ToString("0.000"));
                            StructData.row.Cells["측정 값"].Value = dResultValue_Down.ToString("0.000");

                            //현재 측정값이 양품인지 검사한다. 
                            if (StructData.uperMaxValue > dResultValue_Down && StructData.uperMinValue < dResultValue_Down)
                            {
                                //_strSavedInspectResult_Down = "OK";
                                StructData.row.Cells["판정 결과"].Value = "OK";
                                this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 6).ToString("000"), "OK");
                                StructData.nowOkCount = StructData.nowOkCount + 1;
                                this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 14).ToString("000"), StructData.nowOkCount.ToString("0"));
                                StructData.row.Cells["OkCount"].Value = (StructData.nowOkCount).ToString("0");

                            }
                            else
                            {
                                //_strSavedInspectResult_Down = "NG";
                                StructData.row.Cells["판정 결과"].Value = "NG";
                                this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 6).ToString("000"), "NG");
                                StructData.nowNgCount = StructData.nowNgCount + 1;
                                this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 15).ToString("000"), StructData.nowNgCount.ToString("0"));
                                StructData.row.Cells["NgCount"].Value = (StructData.nowNgCount).ToString("0");

                                StructData.row.Appearance.BackColor = Color.OrangeRed;
                            }
                        }

                        StructData.SuYul = 0f;
                        //현재 수율을 적용한다.
                        if (StructData.nowOkCount == 0)
                        {
                            StructData.row.Cells["수율"].Value = "0.000";
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 7).ToString("000"), "0.000");
                        }
                        else
                        {
                            StructData.OkData = (float)StructData.nowOkCount;
                            StructData.NgData = (float)StructData.nowNgCount;
                            StructData.SuYul = (float)(StructData.OkData / (StructData.OkData + StructData.NgData)) * 100;
                            StructData.row.Cells["수율"].Value = StructData.SuYul.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 7).ToString("000"), StructData.SuYul.ToString("0.000"));
                        }

                        if (OkNg_Data == false)
                        {
                            //평균값 기존데이터로 처리
                            StructData.row.Cells["평균"].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 8).ToString("000"));
                            //표준편차 기존데이터로 처리
                            StructData.row.Cells["표준 편차"].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 9).ToString("000"));
                            //최소 값 기존데이터로 처리
                            StructData.row.Cells["최소 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 10).ToString("000"));
                            //최대 값 기존데이터로 처리
                            StructData.row.Cells["최대 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 11).ToString("000"));
                            //CP 값 기존데이터로 처리
                            StructData.row.Cells["CP 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 12).ToString("000"));
                            //CPK 값 기존데이터로 처리
                            StructData.row.Cells["CPK 값"].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 13).ToString("000"));
                            continue;
                        }

                        //현재 평균 값을 적용합니다.
                        StructData.NowAvr = 0f;
                        StructData.OldValue = 0f;
                        if (NowPassNumber_Down == 1)
                        {
                            StructData.NowAvr = (float)dResultValue_Down;
                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            StructData.OldValue = (float)dResultValue_Down;
                            StructData.OldSquare = (float)dResultValue_Down * (float)dResultValue_Down;

                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        }
                        else if (NowPassNumber_Down > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            //StructData.OkData = (float)StructData.nowOkCount;
                            //StructData.NgData = (float)StructData.nowNgCount;
                            StructData.NowAvr = (float)((StructData.OldValue + (float)dResultValue_Down) / (float)NowPassNumber_Down);

                            StructData.row.Cells["평균"].Value = StructData.NowAvr.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 8).ToString("000"), StructData.NowAvr.ToString("0.000"));

                            StructData.OldSquare = (StructData.OldSquare) + ((float)dResultValue_Down * (float)dResultValue_Down);
                            StructData.OldValue = (StructData.OldValue + (float)dResultValue_Down);

                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 16).ToString("000"), StructData.OldValue.ToString("0.000"));
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 17).ToString("000"), StructData.OldSquare.ToString("0.000"));
                        }

                        //현재 표준편차을 적용하니다. nowOkCount
                        StructData.OldStdDev = 0f;
                        StructData.NowStdDev = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount <= 1) StructData.row.Cells["표준 편차"].Value = "0.000";
                        else if (StructData.nowOkCount + StructData.nowNgCount > 1)
                        {
                            StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 16).ToString("000"));
                            StructData.OldValue = float.Parse(StructData.OldValuestr);

                            StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 17).ToString("000"));
                            StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                            //int nCount = StructData.nowOkCount + StructData.nowNgCount;
                            //int nCount = StructData.nowOkCount;
                            int nCount = (int)NowPassNumber_Down;

                            StructData.NowStdDev = (float)Control_CPKData.StDev(nCount, (double)StructData.OldValue, (double)StructData.OldSquare);
                            StructData.row.Cells["표준 편차"].Value = StructData.NowStdDev.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 9).ToString("000"), StructData.NowStdDev.ToString("0.000"));
                        }

                        //현재 최소 값을 적용하니다.
                        StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 10).ToString("000"));
                        StructData.OldMin = float.Parse(StructData.OldMinstr);
                        StructData.NowMin = Control_CPKData.Min((float)dResultValue_Down, StructData.OldMin);
                        StructData.row.Cells["최소 값"].Value = StructData.NowMin.ToString("0.000");
                        //시작 106 : 레지로 수정된 코드
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                        //종료 106 : 레지로 수정된 코드

                        StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 11).ToString("000"));
                        StructData.OldMax = float.Parse(StructData.OldMaxstr);
                        StructData.NowMax = Control_CPKData.Max((float)dResultValue_Down, StructData.OldMax);
                        StructData.row.Cells["최대 값"].Value = StructData.NowMax.ToString("0.000");
                        //시작 107 : 레지로 수정된 코드
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                        //종료 107 : 레지로 수정된 코드

                        //현재 CP 값을 적용하니다. 
                        StructData.NowCP = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount < 2) StructData.row.Cells["CP 값"].Value = "0.000";
                        else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                        {
                            StructData.NowCP = Control_CPKData.Cp((float)StructData.uperMaxValue, (float)StructData.uperMinValue, StructData.NowStdDev);
                            StructData.row.Cells["CP 값"].Value = StructData.NowCP.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 12).ToString("000"), StructData.NowCP.ToString("0.000"));
                        }

                        //현재 CPK 값을 적용합니다.
                        StructData.NowCPKU = 0f; 
                        StructData.NowCPKL = 0f; 
                        StructData.NowCPK = 0f;
                        if (StructData.nowOkCount + StructData.nowNgCount < 2) StructData.row.Cells["CPK 값"].Value = "0.000";
                        else if (StructData.nowOkCount + StructData.nowNgCount >= 2)
                        {
                            StructData.NowCPKU = Control_CPKData.CpkU((float)StructData.uperMaxValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPKL = Control_CPKData.CpkL((float)StructData.uperMinValue, StructData.NowAvr, StructData.NowStdDev);
                            StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                            StructData.row.Cells["CPK 값"].Value = StructData.NowCPK.ToString("0.000");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                        }
                    }

                    //그리드 공란을 채운다.
                    if (StructData.RcpGrdRows < 11)
                    {
                        for (int i = 0; i < 10 - StructData.RcpGrdRows; i++)
                        {
                            StructData.row = this.uGrd_Inspect_Measure_Down.DisplayLayout.Bands[0].AddNew();
                            StructData.row.Cells["NO"].Value = uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0");
                        }
                    }
                }
                catch (Exception e)
                {
                    //Inspect_Test_Log_Write(pointData, 2, textBox16);
                    //MessageBox.Show(MethodBase.GetCurrentMethod().Name + _iImgCount.ToString("0") + " " + e.Message);
                    throw;
                }
            }
        }

        public Inspection_Grid_Data[] Inspect_Run_Run_FindData_Inspecting_Down()
        {

            Inspection_Grid_Data StructData = new Inspection_Grid_Data();
            StructData.RecipeColumn = 11;
            StructData.VisionColumn = 8;
            StructData.RcpGrdRows = LamiSystem.StrLstRcpConGridData_Down.Count / StructData.RecipeColumn;
            Inspection_Grid_Data[] Inspect_Grid_Data = new Inspection_Grid_Data[StructData.RcpGrdRows];

            try
            {
                Inspection_Data_List InspectData = new Inspection_Data_List();
                InspectData.Inspection_Data = new List<string>();

                int mesColCount = 19;
                bool OkNg_Data = Inspect_Run_Run_FindData_OkNg_Down();

                if (OkNg_Data == true) _strSavedInspectResult_Down = "OK";
                else _strSavedInspectResult_Down = "NG";

                for (int i = 0; i < StructData.RcpGrdRows; i++)
                {
                    StructData.readName = LamiSystem.StrLstRcpConGridData_Down[i*StructData.RecipeColumn];
                    StructData.dResultValueStr = MeasureData_Down[i];
                    StructData.itemResult = itemResult_Down[i];

                    int VisGrdRows = LamiSystem.StrListVisConGridData_Down.Count/StructData.VisionColumn;
                    for (int j = 0; j < VisGrdRows; j++)
                    {
                        string VisName = LamiSystem.StrListVisConGridData_Down[j*StructData.VisionColumn];
                        if (StructData.readName == VisName)
                        {
                            StructData.CenValue = LamiSystem.StrListVisConGridData_Down[j*StructData.VisionColumn + 1];
                            StructData.MaxValue = LamiSystem.StrListVisConGridData_Down[j*StructData.VisionColumn + 2];
                            StructData.MinValue = LamiSystem.StrListVisConGridData_Down[j*StructData.VisionColumn + 3];

                            StructData.MaxData = double.Parse(StructData.CenValue) + double.Parse(StructData.MaxValue);
                            StructData.MinData = double.Parse(StructData.CenValue) - double.Parse(StructData.MinValue);

                            InspectData.Inspection_Data.Add(StructData.readName);
                            InspectData.Inspection_Data.Add(StructData.CenValue);
                            InspectData.Inspection_Data.Add(StructData.MaxData.ToString("0.00"));
                            InspectData.Inspection_Data.Add(StructData.MinData.ToString("0.00"));


                            //비전에서 설정한 아이템의 켈값을 적용한다.
                            StructData.VisionItem_CalValue =
                                float.Parse(LamiSystem.StrListVisConGridData_Down[(j*StructData.VisionColumn) + 6]);
                            StructData.VisionItem_CalUsed =(LamiSystem.StrListVisConGridData_Down[(j*StructData.VisionColumn) + 7] == "True")? true: false;

                            dResultValue_Down = double.Parse(MeasureData_Down[i]);

                            StructData.strOkCount = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 14).ToString("000"));
                            StructData.nowOkCount = int.Parse(StructData.strOkCount);

                            StructData.strNgCount = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 15).ToString("000"));
                            StructData.nowNgCount = int.Parse(StructData.strNgCount);

                            StructData.uperMaxValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 3).ToString("000"));
                            StructData.uperMaxValue = double.Parse(StructData.uperMaxValuestr);

                            StructData.uperMinValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 4).ToString("000"));
                            StructData.uperMinValue = double.Parse(StructData.uperMinValuestr);

                            //Inspect_Grid_Data[i] = StructData;
                            break;
                        }
                    }

//                     string NowUsed = sLstNowDisNo_Down[i*2];
//                     if (NowUsed == "False")
//                     {
//                         InspectData.Inspection_Data.Add("0.000");       //측정값
//                         InspectData.Inspection_Data.Add("NO");       //판정 결과
//                         InspectData.Inspection_Data.Add("0.000");       //수율
//                         InspectData.Inspection_Data.Add("0.000");       //평균
//                         InspectData.Inspection_Data.Add("0.000");       //표준 편차
//                         InspectData.Inspection_Data.Add("0.000");       //최소 값
//                         InspectData.Inspection_Data.Add("0.000");       //최대 값
//                         InspectData.Inspection_Data.Add("0.000");       //CP 값
//                         InspectData.Inspection_Data.Add("0.000");       //CPK 값
// 
//                         continue;
//                     }

                    if (cvPntLstImagePoint_Down[i*2].X == 0 || cvPntLstImagePoint_Down[i*2].Y == 0 ||
                        cvPntLstImagePoint_Down[i*2 + 1].X == 0 || cvPntLstImagePoint_Down[i*2 + 1].Y == 0)
                    {
                        //측정 포인트를 찾지 못했을 경우에 진행하는 함수
                        //_strSavedInspectResult_Down = "NG";
                        InspectData.Inspection_Data.Add("0.000");       
                        InspectData.Inspection_Data.Add("NG");       
                        StructData.nowNgCount = StructData.nowNgCount + 1;

                        //측정 값
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 5).ToString("000"), "0.000");//측정 결과
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 6).ToString("000"), "NG");//NgCount
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));

                    }
                    else
                    {
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 5).ToString("000"),dResultValue_Down.ToString("0.000"));
                        InspectData.Inspection_Data.Add(dResultValue_Down.ToString("0.000"));

                        //현재 측정값이 양품인지 검사한다. 
                        if (StructData.MaxData > dResultValue_Down && StructData.MinData < dResultValue_Down)
                        {
                            //_strSavedInspectResult_Down = "OK";
                            InspectData.Inspection_Data.Add("OK");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 6).ToString("000"), "OK");
                            StructData.nowOkCount = StructData.nowOkCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 14).ToString("000"),StructData.nowOkCount.ToString("0"));
                        }
                        else
                        {
                            //_strSavedInspectResult_Down = "NG";
                            InspectData.Inspection_Data.Add("NG");
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 6).ToString("000"), "NG");
                            StructData.nowNgCount = StructData.nowNgCount + 1;
                            this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 15).ToString("000"),StructData.nowNgCount.ToString("0"));
                        }
                    }

                    StructData.SuYul = 0f;
                    //현재 수율을 적용한다.
                    if (StructData.nowOkCount == 0)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 7).ToString("000"), "0.000");
                    }
                    else
                    {
                        StructData.OkData = (float) StructData.nowOkCount;
                        StructData.NgData = (float) StructData.nowNgCount;
                        StructData.SuYul = (float) (StructData.OkData/(StructData.OkData + StructData.NgData))*100;
                        InspectData.Inspection_Data.Add(StructData.SuYul.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 7).ToString("000"),StructData.SuYul.ToString("0.000"));
                    }

                    if (OkNg_Data == false)
                    {
                        //평균값 기존데이터로 처리
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 8).ToString("000")));    //평균
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 9).ToString("000")));    //표준 편차
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 10).ToString("000")));   //최소 값
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 11).ToString("000")));   //최대 값
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 12).ToString("000")));   //CP 값
                        InspectData.Inspection_Data.Add(this.GetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 13).ToString("000")));   //CPK 값

                        //편균
                        StructData.NowAvr = float.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000")));
                        StructData.NowStdDev = float.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000")));

                        StructData.NowMin = float.Parse(GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000")));
                        StructData.NowMax = float.Parse(this.GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000")));

                        StructData.NowCP = float.Parse(GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000")));
                        StructData.NowCPK = float.Parse(GetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000")));

                        Inspect_Grid_Data[i] = StructData;
                        continue;
                    }

                    //현재 평균 값을 적용합니다.
                    StructData.NowAvr = 0f;
                    StructData.OldValue = 0f;
                    if (NowPassNumber_Down == 1)
                    {
                        StructData.NowAvr = (float) dResultValue_Down;
                        InspectData.Inspection_Data.Add(StructData.NowAvr.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 8).ToString("000"),StructData.NowAvr.ToString("0.000"));

                        StructData.OldValue = (float) dResultValue_Down;
                        StructData.OldSquare = (float) dResultValue_Down*(float) dResultValue_Down;

                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 16).ToString("000"),StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 17).ToString("000"),StructData.OldSquare.ToString("0.000"));
                    }
                    else if (NowPassNumber_Down > 1)
                    {
                        StructData.OldValuestr = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 16).ToString("000"));
                        StructData.OldValue = float.Parse(StructData.OldValuestr);

                        StructData.OldSquarestr = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 17).ToString("000"));
                        StructData.OldSquare = float.Parse(StructData.OldSquarestr);

                        StructData.NowAvr =(float) ((StructData.OldValue + (float) dResultValue_Down)/(float) NowPassNumber_Down);

                        InspectData.Inspection_Data.Add(StructData.NowAvr.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 8).ToString("000"),StructData.NowAvr.ToString("0.000"));

                        StructData.OldSquare = (StructData.OldSquare) +((float) dResultValue_Down*(float) dResultValue_Down);
                        StructData.OldValue = (StructData.OldValue + (float) dResultValue_Down);

                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 16).ToString("000"),StructData.OldValue.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 17).ToString("000"),StructData.OldSquare.ToString("0.000"));
                    }

                    //현재 표준편차을 적용하니다. nowOkCount
                    StructData.OldStdDev = 0f;
                    StructData.NowStdDev = 0f;
                    if (NowPassNumber_Down <= 1)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 9).ToString("000"), "0.000");
                    }
                    else if (NowPassNumber_Down > 1)
                    {
                        int nCount = (int) NowPassNumber_Down;
                        StructData.NowStdDev = (float)Control_CPKData.StDev(nCount, (double) StructData.OldValue,(double) StructData.OldSquare);
                        InspectData.Inspection_Data.Add(StructData.NowStdDev.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 9).ToString("000"),StructData.NowStdDev.ToString("0.000"));
                    }

                    //현재 최소 값을 적용하니다.
                    StructData.OldMinstr = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 10).ToString("000"));
                    StructData.OldMin = float.Parse(StructData.OldMinstr);
                    StructData.NowMin = Control_CPKData.Min((float) dResultValue_Down, StructData.OldMin);
                    InspectData.Inspection_Data.Add(StructData.NowMin.ToString("0.000"));
                    //시작 106 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 10).ToString("000"), StructData.NowMin.ToString("0.000"));
                    //종료 106 : 레지로 수정된 코드

                    StructData.OldMaxstr = this.GetReg(LamiSystem.RegPathMeasure_Down,(i*mesColCount + 11).ToString("000"));
                    StructData.OldMax = float.Parse(StructData.OldMaxstr);
                    StructData.NowMax = Control_CPKData.Max((float) dResultValue_Down, StructData.OldMax);
                    InspectData.Inspection_Data.Add(StructData.NowMax.ToString("0.000"));
                    //시작 107 : 레지로 수정된 코드
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 11).ToString("000"), StructData.NowMax.ToString("0.000"));
                    //종료 107 : 레지로 수정된 코드

                    //현재 CP 값을 적용하니다. 
                    StructData.NowCP = 0f;
                    if (NowPassNumber_Down < 2)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 12).ToString("000"), "0.000");
                    }
                    else if (NowPassNumber_Down >= 2)
                    {
                        StructData.NowCP = Control_CPKData.Cp((float)StructData.MaxData, (float)StructData.MinData, StructData.NowStdDev);
                        InspectData.Inspection_Data.Add(StructData.NowCP.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 12).ToString("000"), StructData.NowCP.ToString("0.000"));
                    }

                    //현재 CPK 값을 적용합니다.
                    StructData.NowCPKU = 0f;
                    StructData.NowCPKL = 0f;
                    StructData.NowCPK = 0f;
                    if (NowPassNumber_Down < 2)
                    {
                        InspectData.Inspection_Data.Add("0.000");
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 13).ToString("000"), "0.000");
                    }
                    else if (NowPassNumber_Down >= 2)
                    {
                        StructData.NowCPKU = Control_CPKData.CpkU((float)StructData.MaxData, StructData.NowAvr, StructData.NowStdDev);
                        StructData.NowCPKL = Control_CPKData.CpkL((float)StructData.MinData, StructData.NowAvr, StructData.NowStdDev);
                        StructData.NowCPK = Control_CPKData.Cpk(StructData.NowCPKU, StructData.NowCPKL);
                        InspectData.Inspection_Data.Add(StructData.NowCPK.ToString("0.000"));
                        this.SetReg(LamiSystem.RegPathMeasure_Down, (i*mesColCount + 13).ToString("000"), StructData.NowCPK.ToString("0.000"));
                    }

                    Inspect_Grid_Data[i] = StructData;
                }
                return Inspect_Grid_Data;
            }
            catch (Exception e)
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + e.Message);
                return Inspect_Grid_Data;
                //throw;
            }

        }


        /*
        
        public void Inspect_Run_Run_FindData_Inspection_Uper()
        {
            string pointData = string.Empty;

            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 1 start ");
            //Test Function
            //Inspect_Test_Log_Write(pointData, 1, textBox15);
            
             try
             {
                 ////////////////////////////////////////////////////////////////////////////////////////////////
                 ////////////////////////////////////////////////////////////////////////////////////////////////
                for (int i = 0; i < cvPntLstImagePoint_Uper.Count/2; i++)
                {
                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 2 start " + i.ToString());
                    int SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i*11 + 3]);

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 3 start " + i.ToString());
                    //세로 일때 진행됨
                    if (SideNo%2 == 0)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 4 start " + i.ToString());
                        iImgPixResultData1 = Math.Abs(cvPntLstImagePoint_Uper[i*2 + 1].Y - cvPntLstImagePoint_Uper[i*2].Y);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 5 start " + i.ToString());
                        dResultValue_Uper = (double)((double)iImgPixResultData1 * (double)Cal_Sero_Uper);
                    }
                        //가로 일때 진행됨
                    else
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 6 start " + i.ToString());
                        iImgPixResultData1 = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].X - cvPntLstImagePoint_Uper[i * 2].X);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 7 start " + i.ToString());
                        dResultValue_Uper = (double)((double)iImgPixResultData1 * (double)Cal_Garo_Uper);
                    }

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 8 start " + i.ToString());
                    string strOkCount = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("OkCount");
                    int nowOkCount = int.Parse(strOkCount);
                    //int nowOkCount = int.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["OkCount"].Value.ToString());
                    
                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 9 start " + i.ToString());
                    string strNgCount = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("NgCount");
                    int nowNgCount = int.Parse(strNgCount);
                    //int nowNgCount = int.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["NgCount"].Value.ToString());

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 10 start " + i.ToString());
                    string uperMaxValuestr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("규격 상한");
                    double uperMaxValue = double.Parse(uperMaxValuestr);
                    //double uperMaxValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                    
                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 11 start " + i.ToString());
                    string uperMinValuestr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("규격 하한");
                    double uperMinValue = double.Parse(uperMinValuestr);
                    //double uperMinValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 12 start " + i.ToString());
                    //측정 포인트를 찾지 못했을 경우 진행하는 함수.
                    if (cvPntLstImagePoint_Uper[i * 2].X == 0 || cvPntLstImagePoint_Uper[i * 2].Y == 0 || cvPntLstImagePoint_Uper[i * 2 + 1].X == 0 || cvPntLstImagePoint_Uper[i * 2 + 1].Y == 0)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 13 start " + i.ToString());
                        //측정 포인트를 찾지 못했을 경우에 진행하는 함수.
                        Inspect_Run_Run_FindData_ZeroData_Uper(i);
                    }
                    else
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 14 start " + i.ToString());
                        //비전에서 설정한 아이템의 켈값을 적용한다.
                        float VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(i * 8) + 6]);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 15 start " + i.ToString());
                        bool VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(i * 8) + 7] == "True") ? true : false;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 16 start " + i.ToString());
                        if (VisionItem_CalUsed == true) dResultValue_Uper = dResultValue_Uper * VisionItem_CalValue;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 17 start " + i.ToString());
                        //보상을 설정했는지 검사한다.
                        string tmpstr = LamiSystem.StrListSysConName[28];

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 18 start " + i.ToString());
                        if (LamiSystem.StrListSysConData[28] == "ON")
                        {
                            dNumber = 0.0;

                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 19 start " + i.ToString());
                            ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);

                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 20 start " + i.ToString());
                            //보상 비율이 정상적이 값인지 검사한다.
                            if (ParseResult == true)
                            {
                                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 21 start " + i.ToString());
                                if (dNumber > 100) dNumber = 100.0;

                                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 22 start " + i.ToString());
                                Gap_Maker_Per = dNumber;

                                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 23 start " + i.ToString());
                                string strCanValue =(string) uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("규격 중심");
                                double NowCenValue = double.Parse(strCanValue);
                                //uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("규격 중심");
                                //double NowCenValue =double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 중심"].Value.ToString());

                                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 24 start " + i.ToString());
                                dResultValue_Uper = Inspect_Gap_Data_Create(dResultValue_Uper, NowCenValue, Gap_Maker_Per);
                            }
                        }

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 25 start " + i.ToString());
                        //현재 측정값이 양품인지 검사한다. 
                        if (uperMaxValue > dResultValue_Uper && uperMinValue < dResultValue_Uper)
                        {
                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 26 start " + i.ToString());
                            _strSavedInspectResult_Uper = "OK";

                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 27 start " + i.ToString());
                            uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("판정 결과", "OK");
                            uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                            //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";

                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 28 start " + i.ToString());
                            string nowOkCountstr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("OkCount");
                            nowOkCount = int.Parse(nowOkCountstr.Trim());
                            uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("OkCount", (nowOkCount = nowOkCount + 1).ToString(""));
                            uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                            //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["OkCount"].Value = (nowOkCount = nowOkCount + 1).ToString("");
                        }
                        else
                        {
                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 29 start " + i.ToString());
                            _strSavedInspectResult_Uper = "NG";

                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 30 start " + i.ToString());
                            uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("판정 결과", "NG");
                            uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                            //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";

                            Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 31 start " + i.ToString());
                            uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("NgCount", (nowNgCount = nowNgCount + 1).ToString(""));
                            uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                            //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["NgCount"].Value = (nowNgCount = nowNgCount + 1).ToString("");
                        }

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 32 start " + i.ToString());
                        //현재 측정한 값을 적용한다.
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("측정 값", dResultValue_Uper.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value = dResultValue_Uper.ToString("0.000");
                    }

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 33 start " + i.ToString());
                    Inspect_Result_Uper = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("판정 결과");
                    uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                    //Inspect_Result_Uper = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value.ToString();

                    float SuYul = 0f;
                    //현재 수율을 적용한다.
                    if (nowOkCount != 0)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 34 start " + i.ToString());
                        SuYul = ((float)nowOkCount / ((float)nowOkCount + (float)nowNgCount)) * 100f;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 35 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("수율", SuYul.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["수율"].Value = SuYul.ToString("0.000");
                    }
                    
                    
                    //현재 평균 값을 적용하니다.
                    float NowAvr = 0f;
                    float OldValue = 0f;
                    if (nowOkCount + nowNgCount != 0)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 36 start " + i.ToString());
                        string OldValuestr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("SumValue");
                        OldValue = float.Parse(OldValuestr);
                        //OldValue = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["SumValue"].Value.ToString());

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 37 start " + i.ToString());
                        NowAvr = (float)(OldValue + (float)dResultValue_Uper) / (float)(nowOkCount + nowNgCount);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 38 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("SumValue", ((OldValue + (float)dResultValue_Uper)).ToString("0"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["SumValue"].Value = ((OldValue + (float)dResultValue_Uper)).ToString("0");

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 39 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("평균", NowAvr.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["평균"].Value = NowAvr.ToString("0.000");
                    }

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 40 start " + i.ToString());
                    //현재 표준편차을 적용하니다. nowOkCount
                    float NowStdDev = 0f;

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 41 start " + i.ToString());
                    float[] StdDivArray=new float[2];

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 42 start " + i.ToString());
                    if (nowOkCount + nowNgCount == 1)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 43 start " + i.ToString());
                        OldStdDev = (float) dResultValue_Uper;
                    }
                    else if (nowOkCount + nowNgCount == 2)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 44 start " + i.ToString());
                        StdDivArray[0] = OldStdDev;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 45 start " + i.ToString());
                        StdDivArray[1] = (float) dResultValue_Uper;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 46 start " + i.ToString());
                        NowStdDev = Control_CPKData.StDev(StdDivArray);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 47 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("표준 편차", NowStdDev.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["표준 편차"].Value = NowStdDev.ToString("0.000");
                    }
                    else if (nowOkCount + nowNgCount != 0)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 48 start " + i.ToString());
                        string OldStdDevstr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("표준 편차");
                        OldStdDev = float.Parse(OldStdDevstr);
                        //OldStdDev = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["표준 편차"].Value.ToString());

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 49 start " + i.ToString());
                        StdDivArray[0] = OldStdDev;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 50 start " + i.ToString());
                        StdDivArray[1] = (float)dResultValue_Uper;

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 51 start " + i.ToString());
                        NowStdDev = Control_CPKData.StDev(StdDivArray);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 52 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("표준 편차", NowStdDev.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["표준 편차"].Value = NowStdDev.ToString("0.000");
                    }

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 53 start " + i.ToString());
                    //현재 최소 값을 적용하니다.
                    string OldMinstr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("최소 값");
                    float OldMin = float.Parse(OldMinstr);
                    //float OldMin = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최소 값"].Value.ToString());

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 54 start " + i.ToString());
                    float NowMin = Control_CPKData.Min((float)dResultValue_Uper, OldMin);

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 55 start " + i.ToString());
                    uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("최소 값", NowMin.ToString("0.000"));
                    uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                    //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최소 값"].Value = NowMin.ToString("0.000");
                    
                    //현재 최대 값을 적용하니다.
                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 56 start " + i.ToString());
                    string OldMaxstr = (string)uDS_Inspect_Measure_Uper.Rows[i].GetCellValue("최대 값");
                    float OldMax = float.Parse(OldMaxstr);
                    //float OldMax = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최대 값"].Value.ToString());

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 57 start " + i.ToString());
                    float NowMax = Control_CPKData.Max((float)dResultValue_Uper, OldMax);

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 58 start " + i.ToString());
                    uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("최대 값", NowMax.ToString("0.000"));
                    uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                    //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최대 값"].Value = NowMax.ToString("0.000");

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 59 start " + i.ToString());
                    //현재 CP 값을 적용하니다. 
                    float NowCP = 0f;  
                    if (nowOkCount + nowNgCount >= 2)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 60 start " + i.ToString());
                        NowCP = Control_CPKData.Cp((float)uperMaxValue, (float)uperMinValue, NowStdDev);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 61 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("CP 값", NowCP.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["CP 값"].Value = NowCP.ToString("0.000");
                    }

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 62 start " + i.ToString());
                    //현재 CPK 값을 적용하니다.
                    float NowCPKU = 0f;// Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                    float NowCPKL = 0f;// Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                    float NowCPK = 0f;// Control_CPKData.Cpk(NowCPKU, NowCPKL);
                    if (nowOkCount + nowNgCount >= 2)
                    {
                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 63 start " + i.ToString());
                        NowCPKU = Control_CPKData.CpkU((float)uperMaxValue, NowAvr, NowStdDev);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 64 start " + i.ToString());
                        NowCPKL = Control_CPKData.CpkL((float)uperMinValue, NowAvr, NowStdDev);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 65 start " + i.ToString());
                        NowCPK = Control_CPKData.Cpk(NowCPKU, NowCPKL);

                        Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 66 start " + i.ToString());
                        uDS_Inspect_Measure_Uper.Rows[i].SetCellValue("CPK 값", NowCPK.ToString("0.000"));
                        uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                        //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["CPK 값"].Value = NowCPK.ToString("0.000");
                    }

                    Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 67 start " + i.ToString());
                    uGrd_Inspect_Measure_Uper.DataSource = uDS_Inspect_Measure_Uper;
                    //Test Function
                    Inspect_Test_Log_Write(pointData, 2, textBox15);
                }

                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + " 68 start ");
                //Measurement_Grid_Resize(uGrd_Inspect_Measure_Uper);
               
            }
            catch (Exception e)
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name + _iImgCount.ToString("0") + " " + " 69 start ");
                Inspect_Test_Log_Write(pointData, 2, textBox16);
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + _iImgCount.ToString("0") + " " + e.Message);
                throw;
            }
        }
        */

        /*
        public void Inspect_Run_Run_FindData_Inspection_Uper()
        {
            try
            {
                for (int i = 0; i < cvPntLstImagePoint_Uper.Count/2; i++)
                {

                    int SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Uper[i*11 + 3]);

                    //세로 일때 진행됨
                    if (SideNo % 2 == 0) iImgPixResultData1 = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].Y - cvPntLstImagePoint_Uper[i * 2].Y);
                    //가로 일때 진행됨
                    else iImgPixResultData1 = Math.Abs(cvPntLstImagePoint_Uper[i * 2 + 1].X - cvPntLstImagePoint_Uper[i * 2].X);

                    int nowOkCount = int.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["OkCount"].Value.ToString());
                    int nowNgCount = int.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["NgCount"].Value.ToString());
                    double UperMaxValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                    double UperMinValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                    //측정 포인트를 찾았을 경우에 진행하는 함수.
                    if (iImgPixResultData1 != 0)
                    {
                        dResultValue_Uper = (double) ((double) iImgPixResultData1/(double) Cal_Garo_Uper);

                        //비전에서 설정한 아이템의 켈값을 적용한다.
                        float VisionItem_CalValue = float.Parse(LamiSystem.StrListVisConGridData_Uper[(i*8) + 6]);
                        bool VisionItem_CalUsed = (LamiSystem.StrListVisConGridData_Uper[(i*8) + 7] == "True")
                            ? true
                            : false;
                        if (VisionItem_CalUsed == true) dResultValue_Uper = dResultValue_Uper*VisionItem_CalValue;

                       
                        
                        //보상을 설정했는지 검사한다.
                        string tmpstr = LamiSystem.StrListSysConName[28];
                        if (LamiSystem.StrListSysConData[28] == "ON")
                        {
                            dNumber = 0.0;
                            ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
                            //보상 비율이 정상적이 값인지 검사한다.
                            if (ParseResult == true)
                            {
                                if (dNumber > 100) dNumber = 100.0;
                                Gap_Maker_Per = dNumber;
                                double NowCenValue =
                                    double.Parse(
                                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 중심"].Value.ToString());
                                dResultValue_Uper = Inspect_Gap_Data_Create(dResultValue_Uper, NowCenValue, Gap_Maker_Per);
                            }
                        }

                        //현재 측정값이 양품인지 검사한다. 
                        if (UperMaxValue > dResultValue_Uper && UperMinValue < dResultValue_Uper)
                        {
                            uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";
                            uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["OkCount"].Value = (nowOkCount++).ToString("");
                        }

                        else
                        {
                            uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";
                            uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["NgCount"].Value = (nowNgCount++).ToString("");
                        }

                        //현재 측정한 값을 적용한다.
                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value =
                            dResultValue_Uper.ToString("0.000");
                    }
                    //측정 포인트를 찾지 못했을 경우에 진행하는 함수.
                    else Inspect_Run_Run_FindData_ZeroData_Uper(i);

                    float SuYul = 0f;
                    //현재 수율을 적용한다.
                    if (nowOkCount != 0) SuYul = (float)(nowOkCount / (nowOkCount + nowNgCount)) * 100f;
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["수율"].Value = SuYul.ToString("0.000");

                    //현재 평균 값을 적용하니다.
                    float NowAvr = 0f;
                    float OldAvar = 0f;
                    if (nowOkCount + nowNgCount != 0)
                    {
                        OldAvar = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["평균"].Value.ToString());
                        NowAvr = (float)(OldAvar + (float)dResultValue_Uper) / (float)(nowOkCount + nowNgCount);
                    }
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["평균"].Value = NowAvr.ToString("0.000");

                    //현재 표준편차을 적용하니다. nowOkCount
                    float NowStdDev = 0f;
                    float[] StdDivArray=new float[2];
                    if (nowOkCount + nowNgCount == 1)
                    {
                        OldStdDev = (float) dResultValue_Uper;
                    }
                    else if (nowOkCount + nowNgCount == 2)
                    {
                        StdDivArray[0] = OldStdDev;
                        StdDivArray[1] = (float) dResultValue_Uper;

                        NowStdDev = Control_CPKData.StDev(StdDivArray);
                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["표준 편차"].Value = NowStdDev.ToString("0.000");
                    }
                    else
                    {
                        OldStdDev = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["표준 편차"].Value.ToString());
                        StdDivArray[0] = OldStdDev;
                        StdDivArray[1] = (float)dResultValue_Uper;

                        NowStdDev = Control_CPKData.StDev(StdDivArray);
                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["표준 편차"].Value = NowStdDev.ToString("0.000");
                    }
                    

                    //현재 최소 값을 적용하니다.
                    float OldMin = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최소 값"].Value.ToString());
                    //float[] MinArray = new[] { (float)dResultValue_Uper, OldMin };
                    //float NowMin = Control_CPKData.Min(MinArray);
                    float NowMin = Control_CPKData.Min((float)dResultValue_Uper, OldMin);
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최소 값"].Value = NowMin.ToString("0.000");

                    //현재 최대 값을 적용하니다.
                    float OldMax = float.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최대 값"].Value.ToString());
                    //float[] MaxArray = new[] { (float)dResultValue_Uper, OldMax };
                    //float NowMax = Control_CPKData.Max(MaxArray);
                    float NowMax = Control_CPKData.Max((float)dResultValue_Uper, OldMax);
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["최대 값"].Value = NowMax.ToString("0.000");


                    //현재 CP 값을 적용하니다. 
                    float NowCP = 0f;  
                    if (nowOkCount + nowNgCount >= 2)
                    {
                        NowCP = Control_CPKData.Cp((float)UperMaxValue, (float)UperMinValue, NowStdDev);
                        uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["CP 값"].Value = NowCP.ToString("0.000");
                    }

                    //현재 CPK 값을 적용하니다.
                    float NowCPKU = 0f;// Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                    float NowCPKL = 0f;// Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                    float NowCPK = 0f;// Control_CPKData.Cpk(NowCPKU, NowCPKL);
                    if (nowOkCount + nowNgCount >= 2)
                    {
                        NowCPKU = Control_CPKData.CpkU((float)UperMaxValue, NowAvr, NowStdDev);
                        NowCPKL = Control_CPKData.CpkL((float)UperMinValue, NowAvr, NowStdDev);
                        NowCPK = Control_CPKData.Cpk(NowCPKU, NowCPKL);
                    }                  
                   
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["CPK 값"].Value = NowCPK.ToString("0.000");
                }
                Measurement_Grid_Resize(uGrd_Inspect_Measure_Uper);
                
                return;

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }
        */

        private float OldCPK = 0f;
        private float OldCP = 0f;
        private float OldStdDev = 0f;

        public void Measurement_Grid_To_Register(string strNodePath, Infragistics.Win.UltraWinGrid.UltraGrid doGrid)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                reg = reg.OpenSubKey(strNodePath, true);
                if (reg != null)
                {
                    reg.Close();
                    Registry.CurrentUser.DeleteSubKey(strNodePath,false);
                }

                int titleNo = 0;
                for (int i = 0; i < doGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < 19; j++)
                    {
                        string wData = doGrid.Rows[i].Cells[j].Value.ToString();
                        this.SetReg(strNodePath, titleNo.ToString("000"), wData);
                        titleNo++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        

        public void Measurement_Register_To_Grid_Uper()
        {
            uDS_Inspect_Measure_Uper.Rows.Clear();
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Uper, true);
            if (reg == null)
            {
                Measure_Grid_Making_Uper();
                return;
            }

            int DataCount = reg.ValueCount;
            //이전 레시피의 Row 수량을 저장한다.
            int iMeasureRows = DataCount / 17;
            string strCellData = string.Empty;

            int RegAddress = 0;

            for (int i = 0; i < iMeasureRows; i++)
            {
                for (int j = 0; j < _iMeasurementGridColumnCount; j++)
                {
                    if (j == 0) uDS_Inspect_Measure_Uper.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "","","" });
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells[j].Value = this.GetReg(LamiSystem.RegPathMeasure_Uper, RegAddress.ToString("000"));
                    RegAddress++;
                }
            }
        }

        private int _iMeasurementGridColumnCount = 19;
        public void Measurement_Register_To_Grid_Down()
        {

            uDS_Inspect_Measure_Down.Rows.Clear();
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Down, true);
            if (reg == null)
            {
                Measure_Grid_Making_Down();
                return;
            }

            int DataCount = reg.ValueCount;
            //이전 레시피의 Row 수량을 저장한다.
            int iMeasureRows = DataCount / 17;
            string strCellData = string.Empty;

            int RegAddress = 0;

            for (int i = 0; i < iMeasureRows; i++)
            {
                for (int j = 0; j < _iMeasurementGridColumnCount; j++)
                {
                    if (j == 0) uDS_Inspect_Measure_Down.Rows.Add(true, new Object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "","" });
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells[j].Value = this.GetReg(LamiSystem.RegPathMeasure_Down, RegAddress.ToString("000"));
                    RegAddress++;
                }
            }
        }

        public void Inspect_Run_Run_FindData_ZeroData_Down(int rowNumber)
        {
            int nowNgCount = int.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[rowNumber].Cells["NgCount"].Value.ToString());
            uGrd_Inspect_Measure_Down.DisplayLayout.Rows[rowNumber].Cells["판정 결과"].Value = "NG";
            uGrd_Inspect_Measure_Down.DisplayLayout.Rows[rowNumber].Cells["NgCount"].Value = (nowNgCount = nowNgCount + 1).ToString("");
            uGrd_Inspect_Measure_Down.DisplayLayout.Rows[rowNumber].Cells["측정 값"].Value = "0.000";
        }

        private string Inspect_Result_Down = "NG";

        private delegate void Delegate_Test_Log_Write(string writeData, int mode, TextBox tbox);
        public void Inspect_Test_Log_Write(string writeData, int mode, TextBox tbox)
        {
            if (InvokeRequired)
            {
                Delegate_Test_Log_Write del = Inspect_Test_Log_Write;
                Invoke(del, writeData, mode, tbox);
            }
            else
            {
                if(mode == 1) tbox.Text += writeData;
                else if(mode == 2) tbox.Text = writeData;
            }
            
        }

        /*
        public void Inspect_Run_Run_FindData_Inspection_Down()
        {
            try
            {
                for (int i = 0; i < cvPntLstImagePoint_Down.Count / 2; i++)
                {
                    string UsedFlag = LamiSystem.StrLstRcpConGridData_Down[i*11 + 9];
                    if(UsedFlag == "False" ) continue;

                    int SideNo = int.Parse(LamiSystem.StrLstRcpConGridData_Down[i * 11 + 3]);

                    //세로 일때 진행됨
                    if (SideNo % 2 == 0)
                    {
                        iImgPixResultData1 = Math.Abs(cvPntLstImagePoint_Down[i * 2 + 1].Y - cvPntLstImagePoint_Down[i * 2].Y);
                        dResultValue_Down = (double)((double)iImgPixResultData1 / (double)Cal_Sero_Down);
                    }
                    //가로 일때 진행됨
                    else
                    {
                        iImgPixResultData1 = Math.Abs(cvPntLstImagePoint_Down[i * 2 + 1].X - cvPntLstImagePoint_Down[i * 2].X);
                        dResultValue_Down = (double)((double)iImgPixResultData1 / (double)Cal_Garo_Down);
                    }

                    //비전에서 설정한 아이템의 켈값을 적용한다.
                    float VisionItem_CalValue = float.Parse(LamiSystem.StrLstVisConGridData_Down[(i * 8) + 6]);
                    bool VisionItem_CalUsed = (LamiSystem.StrLstVisConGridData_Down[(i * 8) + 7] == "True") ? true : false;
                    if (VisionItem_CalUsed == true) dResultValue_Down = dResultValue_Down * VisionItem_CalValue;

                    double DownMaxValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                    double DownMinValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                    //보상을 설정했는지 검사한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        dNumber = 0.0;
                        ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
                        //보상 비율이 정상적이 값인지 검사한다.
                        if (ParseResult == true)
                        {
                            if (dNumber > 100) dNumber = 100.0;
                            Gap_Maker_Per = dNumber;
                            double NowCenValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 중심"].Value.ToString());
                            dResultValue_Down = Inspect_Gap_Data_Create(dResultValue_Down, NowCenValue, Gap_Maker_Per);
                        }
                    }

                    int nowOkCount = int.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["OkCount"].Value.ToString());
                    int nowNgCount = int.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["NgCount"].Value.ToString());

                    //현재 측정값이 양품인지 검사한다. 
                    if (DownMaxValue > dResultValue_Down && DownMinValue < dResultValue_Down)
                    {
                        uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";
                        uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["OkCount"].Value = (nowOkCount++).ToString("");
                    }

                    else
                    {
                        uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";
                        uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["NgCount"].Value = (nowNgCount++).ToString("");
                    }


                    //현재 측정한 값을 적용한다.
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정 값"].Value = dResultValue_Down.ToString("0.000");

                    //현재 수율을 적용한다.
                    float SuYul = (float)(nowOkCount / (nowOkCount + nowNgCount)) * 100f;
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["수율"].Value = SuYul.ToString("0.000");

                    //현재 평균 값을 적용하니다.
                    float OldAvar = float.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["평균"].Value.ToString());
                    float[] AvrArray = new[] { (float)dResultValue_Down, OldAvar };
                    float NowAvr = Control_CPKData.Avage(AvrArray);
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["평균"].Value = NowAvr.ToString("0.000");


                    //현재 표준편차을 적용하니다.
                    float OldStdDev = float.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["표준 편차"].Value.ToString());
                    float[] StdDivArray = new[] { (float)dResultValue_Down, OldStdDev };
                    float NowStdDev = Control_CPKData.StDev(StdDivArray);
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["표준 편차"].Value = NowStdDev.ToString("0.000");

                    //현재 최소 값을 적용하니다.
                    float OldMin = float.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["최소 값"].Value.ToString());
                    //float[] MinArray = new[] { (float)dResultValue_Down, OldMin };
                    //float NowMin = Control_CPKData.Min(MinArray);
                    float NowMin = Control_CPKData.Min((float)dResultValue_Down, OldMin);
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["최소 값"].Value = NowMin.ToString("0.000");

                    //현재 최대 값을 적용하니다.
                    float OldMax = float.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["최대 값"].Value.ToString());
                    //float[] MaxArray = new[] { (float)dResultValue_Down, OldMax };
                    //float NowMax = Control_CPKData.Max(MaxArray);
                    float NowMax = Control_CPKData.Max((float)dResultValue_Down, OldMax);
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["최대 값"].Value = NowMax.ToString("0.000");

                    //현재 CP 값을 적용하니다.
                    float NowCP = Control_CPKData.Cp((float)DownMaxValue, (float)DownMinValue, NowStdDev);
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["CP 값"].Value = NowCP.ToString("0.000");

                    //현재 CPK 값을 적용하니다.
                    float NowCPKU = Control_CPKData.CpkU((float)DownMaxValue, NowAvr, NowStdDev);
                    float NowCPKL = Control_CPKData.CpkL((float)DownMinValue, NowAvr, NowStdDev);
                    float NowCPK = Control_CPKData.Cpk(NowCPKU, NowCPKL);
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["CPK 값"].Value = NowCPK.ToString("0.000");
                }
                Measurement_Grid_Resize(uGrd_Inspect_Measure_Down);
                return;
                GetSet_NowProduct_Number();

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }
        */
        /*
         * 
         public void Inspect_Run_Run_FindData_Inspection()
        {
            try
            {
                //한쪽 시작 점을 찾지 못했을때 를 대비해서 복사를 진행한다.
                if (cvPntLstImagePoint[0].Y == 0 && cvPntLstImagePoint[2].Y != 0)
                {
                    CvPoint tmPoint = new CvPoint(cvPntLstImagePoint[0].X, cvPntLstImagePoint[2].Y);
                    cvPntLstImagePoint[0] = tmPoint;
                }
                else if (cvPntLstImagePoint[2].Y == 0 && cvPntLstImagePoint[0].Y != 0)
                {
                    CvPoint tmPoint = new CvPoint(cvPntLstImagePoint[2].X, cvPntLstImagePoint[0].Y);
                    cvPntLstImagePoint[2] = tmPoint;
                }

                //if (cvPntLstImagePoint[0].X == 0 || cvPntLstImagePoint[0].Y == 0 || cvPntLstImagePoint[2].X == 0 || cvPntLstImagePoint[2].Y == 0)
                if (cvPntLstImagePoint[0].Y == 0 || cvPntLstImagePoint[2].Y == 0)
                    ZeroCheck_Start = true;
                else
                    ZeroCheck_Start = false;
                

                if (cvPntLstImagePoint[1].X == 0 || cvPntLstImagePoint[1].Y == 0 || cvPntLstImagePoint[3].X == 0 || cvPntLstImagePoint[3].Y == 0)
                    ZeroCheck_Small = true;
                else
                    ZeroCheck_Small = false;


                if (cvPntLstImagePoint[5].X == 0 || cvPntLstImagePoint[5].Y == 0 || cvPntLstImagePoint[7].X == 0 || cvPntLstImagePoint[7].Y == 0)
                    ZeroCheck_Middle = true;
                else
                    ZeroCheck_Middle = false;


                if (cvPntLstImagePoint[9].X == 0 || cvPntLstImagePoint[9].Y == 0 || cvPntLstImagePoint[11].X == 0 || cvPntLstImagePoint[11].Y == 0)
                    ZeroCheck_Big = true;
                else
                    ZeroCheck_Big = false;


                //결과 값을 초기화 한다. 0:왼쪽, 1:오른쪽, 2:양쪽(최종) 결과값을 가진다.
                strResultNgOk[0] = "NG";
                strResultNgOk[1] = "NG";
                strResultNgOk[2] = "NG";

                

                

                Inspect_None_Flag = false;
                if (ZeroCheck_Start)
                {
                    //MessageBox.Show(tmpFilename);
                    Inspect_None_Flag = true;
                    return;
                }

                //중,대, 소 순서대로 찾은 포인트를 확인해서 셋중 찾지 못한 포인트가 모두 있다면 검출 실패로 판단한다.
                //PLC에 결과 값으로 0:OK, 1:NG, 2:None 으로 넘겨준다. 이 상황에서는 2를 넘겨준다.
                if (ZeroCheck_Small && ZeroCheck_Middle && ZeroCheck_Big)
                {
                    //MessageBox.Show(tmpFilename);
                    Inspect_None_Flag = true;
                    return;
                }

               

                if (ZeroCheck_Middle == false)
                {
                    //갭 종류 번호를 초기화 한다. 이 길이의 갭은 0번으로 설정되어져 있다.
                    NowGapTypeNumber = 2;

                    //찾은 포인트의 거리를 구한다.
                    //iImgPixResultData1 = cvPntLstImagePoint[5].Y - cvPntLstImagePoint[4].Y;
                    //iImgPixResultData2 = cvPntLstImagePoint[7].Y - cvPntLstImagePoint[6].Y;

                    iImgPixResultData1 = cvPntLstImagePoint[5].Y - cvPntLstImagePoint[0].Y;
                    iImgPixResultData2 = cvPntLstImagePoint[7].Y - cvPntLstImagePoint[2].Y;

                    //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                    //dResultValueLeft = (double)(iImgPixResultData1 / Cal_Mid_Left);
                    //dResultValueRight = (double)(iImgPixResultData2 / Cal_Mid_Righ);

                    dResultValueLeft = (double)((double)iImgPixResultData1 / (double)Cal_Mid_Left);
                    dResultValueRight = (double)((double)iImgPixResultData2 / (double)Cal_Mid_Righ);

                    //////////////////////////////////////////////////////////////////////////////////////////
                    //현재 측정된 값이 약 40 ~ 50 사이의 값이라면 현재 갭 번호를 0으로 설정하도록 한다.
                    bool GapZeroCheck1 = false;
                    bool GapZeroCheck2 = false;
                    bool GapZeroCheck3 = false;
                    if ((Now_Model_Data_Array_Uper[0] - 15.0) <= dResultValueLeft &&
                        (Now_Model_Data_Array_Uper[0] + 15.0) >= dResultValueLeft) GapZeroCheck1 = true;
                    if ((Now_Model_Data_Array_Uper[0] - 15.0) <= dResultValueRight &&
                        (Now_Model_Data_Array_Uper[0] + 15.0) >= dResultValueRight) GapZeroCheck2 = true;
                    if (GapZeroCheck1 && GapZeroCheck2) GapZeroCheck3 = true;

                    if (GapZeroCheck3)
                    {
                        NowGapNumber = 0;
                        GetSet_NowProduct_Number();
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////


                    //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                    dRealCenterValue = Now_Model_Data_Array_Uper[NowGapNumber];

                    //판단 최대 길이와 최소 길이를 계산한다.
                    dRealMaxValue = Uper_Data_Max_Array[NowGapNumber];
                    dRealMinValue = Uper_Data_Min_Array[NowGapNumber];

                    //string sysName = GapSystem.StrListSysConData[28];

                    //보상을 설정했는지 검사한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        //현재 측정값이 양품인지 검사한다. dResultValueLeft
                        //if (dRealMaxValue > dResultValueLeft && dRealMinValue < dResultValueLeft && dRealMaxValue > dResultValueRight && dRealMinValue < dResultValueRight)
                        //if (strResultNgOk[2] == "OK")
                        //{
                            dNumber = 0.0;
                            ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
                            //보상 비율이 정상적이 값인지 검사한다.
                            if (ParseResult == true)
                            {
                                if (dNumber > 100) dNumber = 100.0;
                                Gap_Maker_Per = dNumber;
                                Inspect_Gap_Data_Create(dRealCenterValue, dRealMaxValue, dRealMinValue);
                            }
                        //}
                    }

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
                    //iImgPixResultData1 = cvPntLstImagePoint[9].Y - cvPntLstImagePoint[8].Y;
                    //iImgPixResultData2 = cvPntLstImagePoint[11].Y - cvPntLstImagePoint[10].Y;

                    iImgPixResultData1 = cvPntLstImagePoint[9].Y - cvPntLstImagePoint[0].Y;
                    iImgPixResultData2 = cvPntLstImagePoint[11].Y - cvPntLstImagePoint[2].Y;

                    //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                    //dResultValueLeft = iImgPixResultData1 / Cal_Big_Left;
                    //dResultValueRight = iImgPixResultData2 / Cal_Big_Righ;

                    dResultValueLeft = (double)((double)iImgPixResultData1 / (double)Cal_Big_Left);
                    dResultValueRight = (double)((double)iImgPixResultData2 / (double)Cal_Big_Righ);

                    //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                    dRealCenterValue = Now_Model_Data_Array_Uper[NowGapNumber];

                    //판단 최대 길이와 최소 길이를 계산한다.
                    dRealMaxValue = Uper_Data_Max_Array[NowGapNumber];
                    dRealMinValue = Uper_Data_Min_Array[NowGapNumber];

                    //string sysName = GapSystem.StrListSysConData[28];

                    //보상을 설정했는지 검사한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        //현재 측정값이 양품인지 검사한다.
                        //if (dRealMaxValue + 0.2 > dResultValueLeft && dRealMinValue - 0.2 < dResultValueLeft && dRealMaxValue + 0.2 > dResultValueRight && dRealMinValue - 0.2 < dResultValueRight)
                        //if (strResultNgOk[2] == "OK")
                        //{
                            dNumber = 0.0;
                            ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
                            //보상 비율이 정상적이 값인지 검사한다.
                            if (ParseResult == true)
                            {
                                if (dNumber > 100) dNumber = 100.0;
                                Gap_Maker_Per = dNumber;
                                Inspect_Gap_Data_Create(dRealCenterValue, dRealMaxValue, dRealMinValue);
                            }
                        //}
                    }

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


                    double tmpSmall_1 = (double)((double)cvPntLstImagePoint[1].Y / (double)Cal_Sml_Left);
                    double tmpSmall_2 = (double)((double)cvPntLstImagePoint[0].Y / (double)Cal_Sml_Left);
                    dResultValueLeft = tmpSmall_1 - tmpSmall_2;

                    double tmpSmall_3 = (double)((double)cvPntLstImagePoint[3].Y / (double)Cal_Sml_Righ);
                    double tmpSmall_4 = (double)((double)cvPntLstImagePoint[2].Y / (double)Cal_Sml_Righ);
                    dResultValueRight = tmpSmall_3 - tmpSmall_4;

                    //찾은 포인트의 거리를 구한다.
                    //iImgPixResultData1 = cvPntLstImagePoint[1].Y - cvPntLstImagePoint[0].Y;
                    //iImgPixResultData2 = cvPntLstImagePoint[3].Y - cvPntLstImagePoint[2].Y;

                    //픽셀 거리에서 해상도를 계산해서 실질적인 미리 거리로 변환한다.
                    //dResultValueLeft = (double)((double)iImgPixResultData1 / (double)Cal_Sml_Left);
                    //dResultValueRight = (double)((double)iImgPixResultData2 / (double)Cal_Sml_Righ);

                    //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                    dRealCenterValue = Now_Model_Data_Array_Uper[NowGapNumber];

                    //판단 최대 길이와 최소 길이를 계산한다.
                    dRealMaxValue = Uper_Data_Max_Array[NowGapNumber];
                    dRealMinValue = Uper_Data_Min_Array[NowGapNumber];

                    //보상을 설정했는지 검사한다.
                    if (LamiSystem.StrListSysConData[28] == "ON")
                    {
                        //현재 측정값이 양품인지 검사한다.
                        //if (dRealMaxValue + 0.2 > dResultValueLeft && dRealMinValue - 0.2 < dResultValueLeft && dRealMaxValue + 0.2 > dResultValueRight && dRealMinValue - 0.2 < dResultValueRight)
                        //if (strResultNgOk[2] == "OK")
                        //{
                            dNumber = 0.0;
                            ParseResult = double.TryParse(LamiSystem.StrListSysConData[29], out dNumber);
                            //보상 비율이 정상적이 값인지 검사한다.
                            if (ParseResult == true)
                            {
                                if (dNumber > 100) dNumber = 100.0;
                                Gap_Maker_Per = dNumber;
                                Inspect_Gap_Data_Create(dRealCenterValue, dRealMaxValue, dRealMinValue);
                            }
                        //}
                    }

                    //왼쪽이 스펙에 들어가는지 검사한다.
                    if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                    //오른쪽이 스펙에 들어가는지 검사한다.
                    if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                    //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                    if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }
         * 
         * 
        public void Inspect_Run_Run_FindData_Inspection()
        {
            Trace.WriteLine("NowGapNumber : " + NowGapNumber.ToString());
            //Debug01.Text += NowGapNumber.ToString() + "\r\n";

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
                //strResultNgOk[2] = "NONE";

                _iPLC_Result_Code = 2;
                PLC_WriteData_Threading();

                Inspect_Run_Run_Inspect_None_Display();

                dResultValueLeft = 0.0;
                dResultValueRight = 0.0;
                _savedValue_Left = 0.0;
                _savedValue_Right = 0.0;
                _strSavedNGOK[2] = "NG";
                //Gap_Zero_Setting4 = new Thread(WriteExcelFileData);
                //Gap_Zero_Setting4.Start();
                //TotalGap_Write_Thread = new Thread(Inspect_Run_Run_Result_Write_GapTotal);
                //TotalGap_Write_Thread.Start();

                //Inspect_Run_Run_Data_Save();
                //_CycleCompleteFlag_Gap = true;
                //Thread.Sleep(3);
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
                //dResultValueLeft = (double)(iImgPixResultData1 / Cal_Mid_Left);
                //dResultValueRight = (double)(iImgPixResultData2 / Cal_Mid_Righ);

                dResultValueLeft = (double)((double)iImgPixResultData1 /(double) Cal_Mid_Left);
                dResultValueRight = (double)((double)iImgPixResultData2 / (double)Cal_Mid_Righ);

                //////////////////////////////////////////////////////////////////////////////////////////
                //현재 측정된 값이 약 40 ~ 50 사이의 값이라면 현재 갭 번호를 0으로 설정하도록 한다.
                bool GapZeroCheck1 = false;
                bool GapZeroCheck2 = false;
                bool GapZeroCheck3 = false;
                if ((Now_Model_Gap_Data_Array[0] - 15.0) <= dResultValueLeft &&
                    (Now_Model_Gap_Data_Array[0] + 15.0) >= dResultValueLeft) GapZeroCheck1 = true;
                if ((Now_Model_Gap_Data_Array[0] - 15.0) <= dResultValueRight &&
                    (Now_Model_Gap_Data_Array[0] + 15.0) >= dResultValueRight) GapZeroCheck2 = true;
                if (GapZeroCheck1 && GapZeroCheck2) GapZeroCheck3 = true;

                if (GapZeroCheck3)
                {
                    NowGapNumber = 0;
                    //Gep_Total_Write_Time();
                    //Inspect_Gap_Gap_Zero_Setting();
                    //myEvent_Inspect_Gap_01(NowGapNumber, NowProdectNumber_Gap);
                    //OperationEvent_Gap1(NowGapNumber, NowProdectNumber_Gap);
                }
                //////////////////////////////////////////////////////////////////////////////////////////


                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                dRealCenterValue = Now_Model_Gap_Data_Array[NowGapNumber];

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

//                 if (GapTotal_Filename != null)
//                 {
//                     Gap_Zero_Setting1 = new Thread(WriteExcelFileData);
//                     Gap_Zero_Setting1.Start();
//                 }
        
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
                //dResultValueLeft = iImgPixResultData1 / Cal_Big_Left;
                //dResultValueRight = iImgPixResultData2 / Cal_Big_Righ;

                dResultValueLeft =(double) ((double)iImgPixResultData1 /(double) Cal_Big_Left);
                dResultValueRight =(double)((double) iImgPixResultData2 / (double) Cal_Big_Righ);

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                dRealCenterValue = Now_Model_Gap_Data_Array[NowGapNumber];

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = Gap_Data_Max_Array[NowGapNumber];
                dRealMinValue = Gap_Data_Min_Array[NowGapNumber];


                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";

//                 if (GapTotal_Filename != null)
//                 {
//                     Gap_Zero_Setting2 = new Thread(WriteExcelFileData);
//                     Gap_Zero_Setting2.Start();
//                 }
        
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
                //dResultValueLeft = iImgPixResultData1/Cal_Sml_Left;
                //dResultValueRight = iImgPixResultData2/Cal_Sml_Righ;

                dResultValueLeft = (double)((double)iImgPixResultData1 / (double)Cal_Sml_Left);
                dResultValueRight = (double)((double)iImgPixResultData2 / (double)Cal_Sml_Righ);

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                dRealCenterValue = Now_Model_Gap_Data_Array[NowGapNumber];

                //판단 최대 길이와 최소 길이를 계산한다.
                dRealMaxValue = Gap_Data_Max_Array[NowGapNumber];
                dRealMinValue = Gap_Data_Min_Array[NowGapNumber];

                string sysName = GapSystem.StrListSysConData[28];
                if (sysName == "OK") Gap_Maker_Per = double.Parse(GapSystem.StrListSysConData[29]);
                //if (strResultNgOk[2] == "OK") 
                //Inspect_Gap_Data_Create(dRealCenterValue, dResultValueLeft, dResultValueRight,dRealMaxValue, dRealMinValue);


                //왼쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueLeft && dRealMaxValue >= dResultValueLeft) strResultNgOk[0] = "OK";

                //오른쪽이 스펙에 들어가는지 검사한다.
                if (dRealMinValue <= dResultValueRight && dRealMaxValue >= dResultValueRight) strResultNgOk[1] = "OK";

                //양쪽 모두 스펙에 들아기는지 검사해서 최종 결과를 기록한다.
                if (strResultNgOk[0] == "OK" && strResultNgOk[1] == "OK") strResultNgOk[2] = "OK";

//                 if (GapTotal_Filename != null)
//                 {
//                    
//                     Gap_Zero_Setting3 = new Thread(WriteExcelFileData);
//                     Gap_Zero_Setting3.Start();
//                 }
        
            }

           
        }
        */
        /*
        public void WriteExcelFileData()
        {
            string[] tmpStrings = { dResultValueLeft.ToString(), dResultValueRight.ToString() };
            string tmpStr = string.Empty;

            if (constructFlag == true)
            {
                constructFlag = false;
                if (NowGapNumber > 0)
                {
                    string saveTime = String.Format("{0:00}시{1:00}분{2:00}.{3:000}초",
                    DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);

                    tmpStr = saveTime + ",";

                    int AddCount = 0;
                    do
                    {
                        tmpStr += "0,0,";
                        AddCount++;
                    } while (AddCount < NowGapNumber);

                    excelFile.WriteExcelFile(GapTotal_Filename, tmpStr);
                }
            }
            
            //excelFile.WriteExcelFile(GapTotal_Filename, tmpStrings);
            GapTotal_Result[NowGapNumber] = strResultNgOk[2];
        }
         * 
         * */
        private static double Gap_Maker_Per = 0.0;
        public struct GapData_Struct
        {
            public double A;// 기존 참값= dResultValueLeft; // 기존 참값
            public double P;// 보상 비율= 30.0; // 보상 비율
            public double B;// 보상 적용값= 0.0; // 보상 적용값
            public double T;// 보상 목표값= dRealCenterValue; // 보상 목표값
            public double R;// 양품 레인지 = dRealMaxValue - dRealMinValue; // 양품 레인지
        }


        //Inspect_Gap_Data_Create(MeasureData, Gap_Maker_Per, NowCenValue, NowMaxValue, NowMinValue);


        private double Inspect_Gap_Data_Create(double MeasureData, double Gap_Maker_Per, double NowCenValue, double NowMaxValue, double NowMinValue)
        {
            GapData_Struct Gap_Maker = new GapData_Struct();
           
            Gap_Maker.P = Gap_Maker_Per;//30.0; // 보상 비율
            Gap_Maker.B = 0.0; // 보상 적용값
            Gap_Maker.T = NowCenValue; // 보상 목표값
            Gap_Maker.R = NowMaxValue - NowMinValue; // 양품 레인지

            Gap_Maker.A = MeasureData; // 기존 참값
            Gap_Maker.B = Gap_Maker.T + (Gap_Maker.P / 100) * (Gap_Maker.A - Gap_Maker.T);
            //MeasureData = Gap_Maker.B;

            //Gap_Maker.A = dResultValue_Down; // 기존 참값
            //Gap_Maker.B = Gap_Maker.T + (Gap_Maker.P / 100) * (Gap_Maker.A - Gap_Maker.T);
            //dResultValueRight = Gap_Maker.B;
            return Gap_Maker.B;
        }


        /*
        
        private double Inspect_Gap_Data_Create(double dRealCenterValue, double dRealMaxValue, double dRealMinValue )
        {
            GapData_Struct Gap_Maker = new GapData_Struct();
           
            Gap_Maker.P = Gap_Maker_Per;//30.0; // 보상 비율
            Gap_Maker.B = 0.0; // 보상 적용값
            Gap_Maker.T = dRealCenterValue; // 보상 목표값
            Gap_Maker.R = dRealMaxValue - dRealMinValue; // 양품 레인지

            Gap_Maker.A = dResultValue_Uper; // 기존 참값
            Gap_Maker.B = Gap_Maker.T + (Gap_Maker.P / 100) * (Gap_Maker.A - Gap_Maker.T);
            dResultValue_Uper = Gap_Maker.B;

            Gap_Maker.A = dResultValue_Down; // 기존 참값
            Gap_Maker.B = Gap_Maker.T + (Gap_Maker.P / 100) * (Gap_Maker.A - Gap_Maker.T);
            //dResultValueRight = Gap_Maker.B;
            return Gap_Maker.B;
        }
        */
        private Thread Gap_Zero_Setting;
        private Thread Gap_Zero_Setting1;
        private Thread Gap_Zero_Setting2;
        private Thread Gap_Zero_Setting3; 
        private Thread Gap_Zero_Setting4;
        private Thread Gap_Zero_Setting5;
        
        /*
        public void Inspect_Gap_Gap_Zero_Setting()
        {
            if (GapTotal_Write_Flag == true)
            {
                Gap_Zero_Setting = new Thread(FormDlgInsp_Inspection_Excel_GapTotal_Write);
                Gap_Zero_Setting.Start();
            }
           
            NowGapNumber = 0;
            SetReg(GapSystem.RegPathGapStatus, "NowGapNo", NowGapNumber.ToString());

            if (NowProdectNumber_Gap < uint.MaxValue)
                NowProdectNumber_Gap++;
            else
                NowProdectNumber_Gap = 1;
            SetReg(GapSystem.RegPathGapStatus, "Count_Trigger", NowProdectNumber_Gap.ToString());
        }
        */

        private static bool ZeroCheck_Big = false;
        private static bool ZeroCheck_Middle = false;
        private static bool ZeroCheck_Small = false;


        /*
        public void Inspect_Run_Run_FindData_Inspection_Manual()
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
                dResultValueLeft = iImgPixResultData1/Cal_Mid_Left;
                dResultValueRight = iImgPixResultData2/Cal_Mid_Righ;

                 double dNumberCenter = 0.0;
                 bool resultCenter = double.TryParse(GapSystem.StrListVisConGridData_Uper[1], out dNumberCenter);
                 dRealCenterValue = dNumberCenter;
 
                 double dNumberPluse = 0.0;
                 bool resultPluse = double.TryParse(GapSystem.StrListVisConGridData_Uper[2], out dNumberPluse);
 
                 double dNumberMinus = 0.0;
                 bool resultMinus = double.TryParse(GapSystem.StrListVisConGridData_Uper[3], out dNumberMinus);
             
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
                dResultValueLeft = iImgPixResultData1 / Cal_Big_Left;
                dResultValueRight = iImgPixResultData2 / Cal_Big_Righ;


                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(GapSystem.StrListVisConGridData_Uper[5], out dNumberCenter);
                dRealCenterValue = dNumberCenter;

                double dNumberPluse = 0.0;
                bool resultPluse = double.TryParse(GapSystem.StrListVisConGridData_Uper[6], out dNumberPluse);

                double dNumberMinus = 0.0;
                bool resultMinus = double.TryParse(GapSystem.StrListVisConGridData_Uper[7], out dNumberMinus);

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
                dResultValueLeft = iImgPixResultData1 / Cal_Sml_Left;
                dResultValueRight = iImgPixResultData2 / Cal_Sml_Righ;


                //                 double dNumberCenter = 0.0;
                //                 bool resultCenter = double.TryParse(GapSystem.StrListVisConGridData_Uper[1], out dNumberCenter);
                //                 dRealCenterValue = dNumberCenter;
                // 
                //                 double dNumberPluse = 0.0;
                //                 bool resultPluse = double.TryParse(GapSystem.StrListVisConGridData_Uper[4 * i + 2], out dNumberPluse);
                //                 Gap_Data_Pluse_Array.Add(dNumberPluse);
                // 
                //                 double dNumberMinus = 0.0;
                //                 bool resultMinus = double.TryParse(GapSystem.StrListVisConGridData_Uper[4 * i + 3], out dNumberMinus);
                //                 Gap_Data_Minus_Array.Add(dNumberMinus);

                //비전 설정에서 입력받은 검출 상하한 값을 가져온다.
                double dNumberCenter = 0.0;
                bool resultCenter = double.TryParse(GapSystem.StrListVisConGridData_Uper[(4 * NowGapNumber) + 1],
                    out dNumberCenter);
                dRealCenterValue = dNumberCenter;

                double dNumberPluse = 0.0;
                bool resultPluse = double.TryParse(GapSystem.StrListVisConGridData_Uper[(4 * NowGapNumber) + 2],
                    out dNumberPluse);

                double dNumberMinus = 0.0;
                bool resultMinus = double.TryParse(GapSystem.StrListVisConGridData_Uper[(4 * NowGapNumber) + 3],
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
        */

        private static double dRealCenterValue;
        private static double dRealMaxValue;
        private static double dRealMinValue;
        private static double dRealMinusValue = 0.0;
        private static double dRealPluseValue = 0.0;
        private static double dResultValue_Uper;
        private static double dResultValue_Down;
        public static double _savedValue_Uper = 0.0;
        public static double _savedValue_Down = 0.0;
        private static List<CvPoint> _savedCvPntLstImagePoint_Uper;
        private static List<CvPoint> _savedCvPntLstImagePoint_Down;
        public static double _iSavedCenter = 0.0;
        public static double _iSavedMax = 0.0;
        public static double _iSavedMin = 0.0;
        private static IplImage NowSavedImage_Uper = new IplImage(4096, 3072, BitDepth.U8, 3);
        private static IplImage NowSavedImage_Down = new IplImage(4096, 3072, BitDepth.U8, 3);

        public void Inspect_Save_Data_Copy_Uper()
        {
            Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                Cv.Copy(_nowIplImage_Uper, NowSavedImage_Uper);
                _iSavedTrigNumber = NowTrigNumber;
                _savedValue_Uper = dResultValue_Uper;
                _strSavedNGOK = strResultNgOk;
                _savedCvPntLstImagePoint_Uper = cvPntLstImagePoint_Uper;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        public void Inspect_Save_Data_Copy_Down()
        {
            try
            {
                Cv.Copy(_nowIplImage_Down, NowSavedImage_Down);
                _savedValue_Down = dResultValue_Down;
                _strSavedNGOK = strResultNgOk;
                _savedCvPntLstImagePoint_Down = cvPntLstImagePoint_Down;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        /*
        public void FormDlgInsp_Inspection_Save_Data_Copy()
        {
            SrcIplImageGap.Copy(NowSavedImage);

            _iSavedGap_Number = NowGapNumber;
            _iSavedTrigNumber = NowTrigNumber_Gap;
            _savedValue_Left = dResultValueLeft;
            _savedValue_Right = dResultValueRight;
            _strSavedNGOK = strResultNgOk;
            _savedCvPntLstImagePoint = cvPntLstImagePoint;
            _iSavedCenter = dRealCenterValue;
            _iSavedMax = dRealMaxValue;
            _iSavedMin = dRealMinValue;
        }
        */

        /*
        public void FormDlgInsp_Inspection_Save_Data_Copy()
        {
            SrcIplImageGap.Copy(NowSavedImage);

            if (strResultNgOk[2] != "NONE")
            {
                _iSavedGap_Number = NowGapNumber;
                _iSavedTrigNumber = NowTrigNumber_Gap;
                _savedValue_Left = dResultValueLeft;
                _savedValue_Right = dResultValueRight;
                _strSavedNGOK = strResultNgOk;
                _savedCvPntLstImagePoint = cvPntLstImagePoint;
                _iSavedCenter = dRealCenterValue;
                _iSavedMax = dRealMaxValue;
                _iSavedMin = dRealMinValue;
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
        */


        public int Inspect_Run_Run_Get_Reg_NowGapNo()
        {
            string strGapNo = GetReg(LamiSystem.RegPathGapStatus, "NowGapNo");
            int iNumber = -1;
            bool result = int.TryParse(strGapNo, out iNumber);
            return iNumber;
        }

       

        private CvRect tempRect01 = new CvRect(0, 0, 0, 0);


        public static List<IplImage> ROI_Zone_Image_Uper; // = new List<IplImage>();
        public static List<CvRect> ROI_Zone_Rect_Uper; // = new List<CvRect>();

        public static List<IplImage> ROI_Zone_Image_Down; // = new List<IplImage>();
        public static List<CvRect> ROI_Zone_Rect_Down; // = new List<CvRect>();

        private int _nowRoiNumber_Uper = -1;
        private int _nowRoiNumber_Down = -1;
        private static readonly int[] Threshold01 = { 150, 190 };
        private static readonly int[] Threshold02 = { 50, 100 };
        private readonly Hough _imageHougher_Uper = new Hough();
        private readonly Hough _imageHougher_Down = new Hough();
        //List<bool> Lst_ROI_Search_Result = new List<bool>();
        private readonly List<CvPoint> _cvPntLstCenterPoint_Uper = new List<CvPoint>();
        private readonly List<CvPoint> _cvPntLstCenterPoint_Down = new List<CvPoint>();
        //private readonly List<CvPoint> _cvPntLstImagePoint = new List<CvPoint>();
        //private static bool ROI_Search_Result = false;
        //CuttingZone_To_Image Struct_CuttingImage = new CuttingZone_To_Image();
        public struct CuttingZone_To_Image
        {
            public bool areaResult;
            public IplImage cuttingimage;
            public bool houghLineCheck;
            public bool lineCenterCheck;
            public CvPoint resultPoint;
            public bool point_Find_Result;
            public int iSearchCount;
            public int EdgeParam1;
            public int EdgeParam2;
        }

        /*
        
        //우기백 2014.07.24
        //쓰레쉬 홀드 파라메타가 초기화 되지 않아서
        //이 부분을 수정함.
        public bool Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(IplImage WorkImage)
        {
            try
            {
                CuttingZone_To_Image Struct_CutImg = new CuttingZone_To_Image();
                _cvPntLstCenterPoint_Uper.Clear();
                //CvWindow.ShowImages(WorkImage);

                for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
                {
                    string NowUsed = sLstNowDisNo_Uper[i];
                    if (NowUsed == "False")
                    {
                        //Struct_CutImg.resultPoint = new CvPoint(0, 0);
                        //_cvPntLstCenterPoint_Uper.Add(Struct_CutImg.resultPoint);
                        //continue;
                    }

                    Struct_CutImg.point_Find_Result = false;
                    Struct_CutImg.iSearchCount = 0;
                    Struct_CutImg.EdgeParam1 = 0;
                    Struct_CutImg.EdgeParam2 = 0;

                    Cv.SetImageROI(WorkImage, ROI_Zone_Rect_Uper[i]);
                    Cv.Copy(WorkImage, ROI_Zone_Image_Uper[i]);
                    //CvWindow.ShowImages(ROI_Zone_Image_Uper[i]);
                    //ROI_Zone_Image_Uper[i].SaveImage("D:\\Z2\\" + i.ToString("00") + ".jpg");
                    int tmpint = iLstNowRoiNo_Uper[i];
                    ROI_Zone_Image_Uper[iLstNowRoiNo_Uper[i]].SaveImage("D:\\Z1\\" + i.ToString("00") + ".jpg");
                    do
                    {
                        Struct_CutImg.resultPoint.X = 0;
                        Struct_CutImg.resultPoint.Y = 0;
                        switch (Struct_CutImg.iSearchCount)
                        {
                            case 0:
                                //Struct_CutImg.EdgeParam1 = _iEdgeParam1;
                                //Struct_CutImg.EdgeParam2 = _iEdgeParam2;
                                //_iEdgeParam1_Uper = 150;
                                //_iEdgeParam2_Uper = 210;
                                Struct_CutImg.EdgeParam1 = _iEdgeParam1_Uper;
                                Struct_CutImg.EdgeParam2 = _iEdgeParam2_Uper;
                                break;
                            case 1:
                                Struct_CutImg.EdgeParam1 = Threshold02[0];
                                Struct_CutImg.EdgeParam2 = Threshold02[1];
                                break;
                            case 2:
                                Struct_CutImg.EdgeParam1 = (Threshold02[0] < _iEdgeParam3_Uper) ? 0 : Threshold02[0] - _iEdgeParam3_Uper;
                                Struct_CutImg.EdgeParam2 = (Threshold02[1] < _iEdgeParam3_Uper) ? 0 : Threshold02[1] - _iEdgeParam3_Uper;                                
                                break;
                        }
                        
                        _nowRoiNumber_Uper = i;
                        _imageHougher_Uper.GetSet_NowROI = i;

                        //Cv.SetImageROI(WorkImage, ROI_Zone_Rect_Uper[i]);
                        //Cv.Copy(WorkImage, ROI_Zone_Image_Uper[i]);

                        //_imageHougher.HoughLines_Point(ROI_Zone_Image[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1, _iLineParam2, _iLineParam3);
                        List<CvPoint> FindedPoints = _imageHougher_Uper.HoughLines_Point(ROI_Zone_Image_Uper[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1_Uper, _iLineParam2_Uper, _iLineParam3_Uper);

                        if (FindedPoints.Count < 2)
                        {
                            Struct_CutImg.iSearchCount++;
                            continue;
                        }

                         Struct_CutImg.resultPoint = Inspect_Run_Run_Finded_Lines_Uper(ROI_Zone_Image_Uper[i], FindedPoints, iLstNowSidNo_Uper[i], sLstNowPolNo_Uper[i], sLstNowTypNo_Uper[i]);

                        if (Struct_CutImg.resultPoint.X == 0 && Struct_CutImg.resultPoint.Y == 0)
                        {
                            Struct_CutImg.iSearchCount++;
                        }
                        else
                        {
                            Struct_CutImg.point_Find_Result = true;
                        }

                    } while (Struct_CutImg.point_Find_Result == false && Struct_CutImg.iSearchCount < 3);

                    //string imageSavePath = "ROIImages\\" + NowTrigNumber_Gap.ToString("000000") + " "+ NowGapNumber.ToString("00") + " " + i.ToString("00") +  ".jpg";
                    //ROI_Zone_Image[i].SaveImage(imageSavePath);

                    _cvPntLstCenterPoint_Uper.Add(Struct_CutImg.resultPoint);
                    //WorkImage.DrawCircle(Struct_CutImg.resultPoint, 4, CvColor.Red);
                    //CvWindow.ShowImages(WorkImage);
                    WorkImage.ResetROI();
                }//end for                    
                return true;
            } //end try
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }//end function

         * 
        //우기백 2014.07.24
        //쓰레쉬 홀드 파라메타가 초기화 되지 않아서
        //이 부분을 수정함.
        public bool Inspect_Run_Run_ROI_EdgeLine_Centering_Down(IplImage WorkImage)
        {
            try
            {
                CuttingZone_To_Image Struct_CutImg = new CuttingZone_To_Image();
                _cvPntLstCenterPoint_Down.Clear();
                //CvWindow.ShowImages(WorkImage);

                for (int i = 0; i < LamiSystem.RectListImageZone_Down.Count; i++)
                {
                    //string NowUsed = sLstNowDisNo_Down[i];
                    //if (NowUsed == "False")
                    //{
                    //    Struct_CutImg.resultPoint = new CvPoint(0, 0);
                    //    _cvPntLstCenterPoint_Down.Add(Struct_CutImg.resultPoint);
                    //    continue;
                    //}

                    Struct_CutImg.point_Find_Result = false;
                    Struct_CutImg.iSearchCount = 0;
                    Struct_CutImg.EdgeParam1 = 0;
                    Struct_CutImg.EdgeParam2 = 0;
                    do
                    {
                        Struct_CutImg.resultPoint.X = 0;
                        Struct_CutImg.resultPoint.Y = 0;
                        switch (Struct_CutImg.iSearchCount)
                        {
                            case 0:
                                //Struct_CutImg.EdgeParam1 = _iEdgeParam1;
                                //Struct_CutImg.EdgeParam2 = _iEdgeParam2;
                                //_iEdgeParam1_Down = 150;
                                //_iEdgeParam2_Down = 210;
                                Struct_CutImg.EdgeParam1 = _iEdgeParam1_Down;
                                Struct_CutImg.EdgeParam2 = _iEdgeParam2_Down;
                                break;
                            case 1:
                                Struct_CutImg.EdgeParam1 = Threshold02[0];
                                Struct_CutImg.EdgeParam2 = Threshold02[1];
                                break;
                            case 2:
                                Struct_CutImg.EdgeParam1 = (Threshold02[0] < _iEdgeParam3_Down) ? 0 : Threshold02[0] - _iEdgeParam3_Down;
                                Struct_CutImg.EdgeParam2 = (Threshold02[1] < _iEdgeParam3_Down) ? 0 : Threshold02[1] - _iEdgeParam3_Down;
                                break;
                        }

                        _nowRoiNumber_Down = i;
                        _imageHougher_Down.GetSet_NowROI = i;

                        Cv.SetImageROI(WorkImage, ROI_Zone_Rect_Down[i]);
                        Cv.Copy(WorkImage, ROI_Zone_Image_Down[i]);

                        
                        //List<CvPoint> FindedPoints = _imageHougher_Down.HoughLines_Point(ROI_Zone_Image_Down[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1_Down, _iLineParam2_Down, _iLineParam3_Down);
                        List<CvPoint> FindedPoints = _imageHougher_Down.HoughLines_Point(ROI_Zone_Image_Down[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1_Down, _iLineParam2_Down, _iLineParam3_Down);

                        //                          string tmpStr = string.Empty;
                        //                          for (int j = 0; j < FindedPoints.Count; j++)
                        //                          {
                        //                              tmpStr += " P" + j.ToString() + " " + FindedPoints[j].X.ToString() + " " + FindedPoints[j].Y.ToString() + "\t";
                        //                          }
                        // 
                        //                          FormDlgInsp_Inspection_Write_LogFile(NowGapNumber.ToString("00") + "\t" + i.ToString("00") + "\t" + Struct_CutImg.iSearchCount.ToString() +
                        //                                  "\t" + Struct_CutImg.EdgeParam1.ToString("000") + "\t" + Struct_CutImg.EdgeParam2.ToString("000") + "\t" + _iEdgeParam3.ToString("00") +
                        //                                  "\t" + _iLineParam1.ToString("00") + "\t" + _iLineParam2.ToString("000") + "\t" + _iLineParam3.ToString("000") +
                        //                                  "\t" + GapSystem.RectListImageZone_Gap[i].Width.ToString("000") + "\t" + GapSystem.RectListImageZone_Gap[i].Height.ToString("000") +
                        //                                  "\t" + GapSystem.RectListImageZone_Gap[i].Location.X.ToString("0000") + "\t" + GapSystem.RectListImageZone_Gap[i].Location.Y.ToString("0000") +
                        //                                  "\t" + tmpStr + "\r\n");

                        if (FindedPoints.Count < 2)
                        {
                            Struct_CutImg.iSearchCount++;
                            continue;
                        }

                        Struct_CutImg.resultPoint = Inspect_Run_Run_Finded_Lines_Down(ROI_Zone_Image_Down[i], FindedPoints, iLstNowSidNo_Down[i], sLstNowPolNo_Down[i], sLstNowTypNo_Down[i]);

                        if (Struct_CutImg.resultPoint.X == 0 && Struct_CutImg.resultPoint.Y == 0)
                        {
                            Struct_CutImg.iSearchCount++;
                        }
                        else
                        {
                            Struct_CutImg.point_Find_Result = true;
                        }

                    } while (Struct_CutImg.point_Find_Result == false && Struct_CutImg.iSearchCount < 3);

                    //string imageSavePath = "ROIImages\\" + NowTrigNumber_Gap.ToString("000000") + " "+ NowGapNumber.ToString("00") + " " + i.ToString("00") +  ".jpg";
                    //ROI_Zone_Image[i].SaveImage(imageSavePath);

                    _cvPntLstCenterPoint_Down.Add(Struct_CutImg.resultPoint);
                    WorkImage.ResetROI();
                }//end for                    
                return true;
            } //end try
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }//end function

        */

        ///2015-01-09 Start
        //쓰레쉬 홀드 파라메타가 초기화 되지 않아서
        //이 부분을 수정함.
        public bool Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(IplImage WorkImage)
        {
            try
            {
                CuttingZone_To_Image Struct_CutImg = new CuttingZone_To_Image();
                _cvPntLstCenterPoint_Uper.Clear();
                //CvWindow.ShowImages(WorkImage);

                for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
                {
                    string NowUsed = sLstNowDisNo_Uper[i];
                    if (NowUsed == "False")
                    {
                        //Struct_CutImg.resultPoint = new CvPoint(0, 0);
                        //_cvPntLstCenterPoint_Uper.Add(Struct_CutImg.resultPoint);
                        //continue;
                    }

                    Struct_CutImg.point_Find_Result = false;
                    Struct_CutImg.iSearchCount = 0;
                    Struct_CutImg.EdgeParam1 = 0;
                    Struct_CutImg.EdgeParam2 = 0;

                    Cv.SetImageROI(WorkImage, ROI_Zone_Rect_Uper[iLstNowRoiNo_Uper[i]]);
                    Cv.Copy(WorkImage, ROI_Zone_Image_Uper[iLstNowRoiNo_Uper[i]]);
                    //CvWindow.ShowImages(ROI_Zone_Image_Uper[i]);
                    //ROI_Zone_Image_Uper[i].SaveImage("D:\\Z1\\" + i.ToString("00") + ".jpg");
                    //int tmpint = iLstNowRoiNo_Uper[i];
                    //ROI_Zone_Image_Uper[iLstNowRoiNo_Uper[i]].SaveImage("D:\\Z1\\" + i.ToString("00") + ".jpg");
                    do
                    {
                        Struct_CutImg.resultPoint.X = 0;
                        Struct_CutImg.resultPoint.Y = 0;
                        switch (Struct_CutImg.iSearchCount)
                        {
                            case 0:
                                //Struct_CutImg.EdgeParam1 = _iEdgeParam1;
                                //Struct_CutImg.EdgeParam2 = _iEdgeParam2;
                                //_iEdgeParam1_Uper = 150;
                                //_iEdgeParam2_Uper = 210;
                                Struct_CutImg.EdgeParam1 = _iEdgeParam1_Uper;
                                Struct_CutImg.EdgeParam2 = _iEdgeParam2_Uper;
                                break;
                            case 1:
                                Struct_CutImg.EdgeParam1 = Threshold02[0];
                                Struct_CutImg.EdgeParam2 = Threshold02[1];
                                break;
                            case 2:
                                Struct_CutImg.EdgeParam1 = (Threshold02[0] < _iEdgeParam3_Uper) ? 0 : Threshold02[0] - _iEdgeParam3_Uper;
                                Struct_CutImg.EdgeParam2 = (Threshold02[1] < _iEdgeParam3_Uper) ? 0 : Threshold02[1] - _iEdgeParam3_Uper;                                
                                break;
                        }
                        
                        _nowRoiNumber_Uper = i;
                        _imageHougher_Uper.GetSet_NowROI = i;

                        //Cv.SetImageROI(WorkImage, ROI_Zone_Rect_Uper[i]);
                        //Cv.Copy(WorkImage, ROI_Zone_Image_Uper[i]);

                        //_imageHougher.HoughLines_Point(ROI_Zone_Image[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1, _iLineParam2, _iLineParam3);
                        List<CvPoint> FindedPoints = _imageHougher_Uper.HoughLines_Point(ROI_Zone_Image_Uper[iLstNowRoiNo_Uper[i]], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1_Uper, _iLineParam2_Uper, _iLineParam3_Uper);

                        if (FindedPoints.Count < 2)
                        {
                            Struct_CutImg.iSearchCount++;
                            continue;
                        }

                        Struct_CutImg.resultPoint = Inspect_Run_Run_Finded_Lines_Uper(ROI_Zone_Image_Uper[iLstNowRoiNo_Uper[i]], FindedPoints, iLstNowSidNo_Uper[i], sLstNowPolNo_Uper[i], sLstNowTypNo_Uper[i]);

                        if (Struct_CutImg.resultPoint.X == 0 && Struct_CutImg.resultPoint.Y == 0)
                        {
                            Struct_CutImg.iSearchCount++;
                        }
                        else
                        {
                            Struct_CutImg.point_Find_Result = true;
                        }

                    } while (Struct_CutImg.point_Find_Result == false && Struct_CutImg.iSearchCount < 3);

                    //string imageSavePath = "ROIImages\\" + NowTrigNumber_Gap.ToString("000000") + " "+ NowGapNumber.ToString("00") + " " + i.ToString("00") +  ".jpg";
                    //ROI_Zone_Image[i].SaveImage(imageSavePath);

                    _cvPntLstCenterPoint_Uper.Add(Struct_CutImg.resultPoint);
                    //WorkImage.DrawCircle(Struct_CutImg.resultPoint, 4, CvColor.Red);
                    //CvWindow.ShowImages(WorkImage);
                    WorkImage.ResetROI();
                }//end for                    
                return true;
            } //end try
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }//end function
        ///2015-01-09 Finish

        //우기백 2014.07.24
        //쓰레쉬 홀드 파라메타가 초기화 되지 않아서
        //이 부분을 수정함.
        public bool Inspect_Run_Run_ROI_EdgeLine_Centering_Down(IplImage WorkImage)
        {
            try
            {
                CuttingZone_To_Image Struct_CutImg = new CuttingZone_To_Image();
                _cvPntLstCenterPoint_Down.Clear();
                //CvWindow.ShowImages(WorkImage);

                for (int i = 0; i < LamiSystem.RectListImageZone_Down.Count; i++)
                {
                    //string NowUsed = sLstNowDisNo_Down[i];
                    //if (NowUsed == "False")
                    //{
                    //    Struct_CutImg.resultPoint = new CvPoint(0, 0);
                    //    _cvPntLstCenterPoint_Down.Add(Struct_CutImg.resultPoint);
                    //    continue;
                    //}

                    Struct_CutImg.point_Find_Result = false;
                    Struct_CutImg.iSearchCount = 0;
                    Struct_CutImg.EdgeParam1 = 0;
                    Struct_CutImg.EdgeParam2 = 0;
                    do
                    {
                        Struct_CutImg.resultPoint.X = 0;
                        Struct_CutImg.resultPoint.Y = 0;
                        switch (Struct_CutImg.iSearchCount)
                        {
                            case 0:
                                //Struct_CutImg.EdgeParam1 = _iEdgeParam1;
                                //Struct_CutImg.EdgeParam2 = _iEdgeParam2;
                                //_iEdgeParam1_Down = 150;
                                //_iEdgeParam2_Down = 210;
                                Struct_CutImg.EdgeParam1 = _iEdgeParam1_Down;
                                Struct_CutImg.EdgeParam2 = _iEdgeParam2_Down;
                                break;
                            case 1:
                                Struct_CutImg.EdgeParam1 = Threshold02[0];
                                Struct_CutImg.EdgeParam2 = Threshold02[1];
                                break;
                            case 2:
                                Struct_CutImg.EdgeParam1 = (Threshold02[0] < _iEdgeParam3_Down) ? 0 : Threshold02[0] - _iEdgeParam3_Down;
                                Struct_CutImg.EdgeParam2 = (Threshold02[1] < _iEdgeParam3_Down) ? 0 : Threshold02[1] - _iEdgeParam3_Down;
                                break;
                        }

                        _nowRoiNumber_Down = i;
                        _imageHougher_Down.GetSet_NowROI = i;
                        //iLstNowRoiNo_Uper
                        Cv.SetImageROI(WorkImage, ROI_Zone_Rect_Down[iLstNowRoiNo_Down[i]]);
                        Cv.Copy(WorkImage, ROI_Zone_Image_Down[iLstNowRoiNo_Down[i]]);

                        
                        //List<CvPoint> FindedPoints = _imageHougher_Down.HoughLines_Point(ROI_Zone_Image_Down[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1_Down, _iLineParam2_Down, _iLineParam3_Down);
                        List<CvPoint> FindedPoints = _imageHougher_Down.HoughLines_Point(ROI_Zone_Image_Down[iLstNowRoiNo_Down[i]], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1_Down, _iLineParam2_Down, _iLineParam3_Down);

                        //                          string tmpStr = string.Empty;
                        //                          for (int j = 0; j < FindedPoints.Count; j++)
                        //                          {
                        //                              tmpStr += " P" + j.ToString() + " " + FindedPoints[j].X.ToString() + " " + FindedPoints[j].Y.ToString() + "\t";
                        //                          }
                        // 
                        //                          FormDlgInsp_Inspection_Write_LogFile(NowGapNumber.ToString("00") + "\t" + i.ToString("00") + "\t" + Struct_CutImg.iSearchCount.ToString() +
                        //                                  "\t" + Struct_CutImg.EdgeParam1.ToString("000") + "\t" + Struct_CutImg.EdgeParam2.ToString("000") + "\t" + _iEdgeParam3.ToString("00") +
                        //                                  "\t" + _iLineParam1.ToString("00") + "\t" + _iLineParam2.ToString("000") + "\t" + _iLineParam3.ToString("000") +
                        //                                  "\t" + GapSystem.RectListImageZone_Gap[i].Width.ToString("000") + "\t" + GapSystem.RectListImageZone_Gap[i].Height.ToString("000") +
                        //                                  "\t" + GapSystem.RectListImageZone_Gap[i].Location.X.ToString("0000") + "\t" + GapSystem.RectListImageZone_Gap[i].Location.Y.ToString("0000") +
                        //                                  "\t" + tmpStr + "\r\n");

                        if (FindedPoints.Count < 2)
                        {
                            Struct_CutImg.iSearchCount++;
                            continue;
                        }

                        Struct_CutImg.resultPoint = Inspect_Run_Run_Finded_Lines_Down(ROI_Zone_Image_Down[iLstNowRoiNo_Down[i]], FindedPoints, iLstNowSidNo_Down[i], sLstNowPolNo_Down[i], sLstNowTypNo_Down[i]);

                        if (Struct_CutImg.resultPoint.X == 0 && Struct_CutImg.resultPoint.Y == 0)
                        {
                            Struct_CutImg.iSearchCount++;
                        }
                        else
                        {
                            Struct_CutImg.point_Find_Result = true;
                        }

                    } while (Struct_CutImg.point_Find_Result == false && Struct_CutImg.iSearchCount < 3);

                    //string imageSavePath = "ROIImages\\" + NowTrigNumber_Gap.ToString("000000") + " "+ NowGapNumber.ToString("00") + " " + i.ToString("00") +  ".jpg";
                    //ROI_Zone_Image[i].SaveImage(imageSavePath);

                    _cvPntLstCenterPoint_Down.Add(Struct_CutImg.resultPoint);
                    WorkImage.ResetROI();
                }//end for                    
                return true;
            } //end try
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }//end function

        /*
        //우기백 2014.07.24
        //쓰레쉬 홀드 파라메타가 초기화 되지 않아서
        //이 부분을 수정함.
        public bool Inspect_Run_Run_ROI_EdgeLine_Centering(bool MeasMode)
        {
            try
            {
                CuttingZone_To_Image Struct_CutImg = new CuttingZone_To_Image();

                //var Struct_CuttingImage = new CuttingZone_To_Image();
                //Struct_CutImg.resultPoint.X = 0;
                //Struct_CutImg.resultPoint.Y = 0;
                cvPntLstCenterPoint = new List<CvPoint>();
//                 Struct_CutImg.point_Find_Result = false;
//                 Struct_CutImg.iSearchCount = 0;
//                 Struct_CutImg.EdgeParam1 = 0;
//                 Struct_CutImg.EdgeParam2 = 0;

                //인스펙션 로그 파일 기록 함수 호출 (3:이미지 분석 진행)
                //FormDlgInsp_Inspection_LogData_File_Data_Write(3);
                //Inspect_Run_Run_CuttingZone_To_Image_Display();

                //cvPntLstCenterPoint.Clear();
                //Lst_ROI_Search_Result.Clear();

                //CvWindow.ShowImages(SrcIplImageGap);

                for (int i = 0; i < GapSystem.RectListImageZone_Gap.Count; i++)
                {
                    if (i == 4 || i == 6 || i == 8 || i == 10)
                    {
                        CvPoint zeroPoint = new CvPoint(0,0);
                        cvPntLstCenterPoint.Add(zeroPoint);
                        continue;
                    }

                    Struct_CutImg.point_Find_Result = false;
                    Struct_CutImg.iSearchCount = 0;
                    Struct_CutImg.EdgeParam1 = 0;
                    Struct_CutImg.EdgeParam2 = 0;
                    do
                    {
                        Struct_CutImg.resultPoint.X = 0;
                        Struct_CutImg.resultPoint.Y = 0;
                        switch (Struct_CutImg.iSearchCount)
                        {
                            case 0:
                                Struct_CutImg.EdgeParam1 = _iEdgeParam1;
                                Struct_CutImg.EdgeParam2 = _iEdgeParam2;
                                break;
                            case 1:
                                Struct_CutImg.EdgeParam1 = _threshold02[0];
                                Struct_CutImg.EdgeParam2 = _threshold02[1];
                                break;
                            case 2:
                                Struct_CutImg.EdgeParam1 = (_threshold02[0] < _iEdgeParam3) ? 0 : _threshold02[0] - _iEdgeParam3;
                                Struct_CutImg.EdgeParam2 = (_threshold02[1] < _iEdgeParam3) ? 0 : _threshold02[1] - _iEdgeParam3;                                
                                break;
                        }


                        NowROI_Number = i;
                        _imageHougher.GetSet_NowROI = i;

                        IplImage ROIImgStd = new IplImage(SrcIplImageGap.ROI.Size, BitDepth.U8, 3);
                        Cv.Copy(SrcIplImageGap, ROIImgStd);
                        Cv.SetImageROI(ROIImgStd, ROI_Zone_Rect[i]);
                        Cv.Copy(ROIImgStd, ROI_Zone_Image[i]);

                        //Cv.SetImageROI(SrcIplImageGap, ROI_Zone_Rect[i]);
                        //Cv.Copy(SrcIplImageGap, ROI_Zone_Image[i]);
                        //Thread.Sleep(0);
                        //CvWindow.ShowImages(ROI_Zone_Image[i]);

                        
                        
                        _imageHougher.HoughLines_Point(ROI_Zone_Image[i], Struct_CutImg.EdgeParam1, Struct_CutImg.EdgeParam2, _iLineParam1, _iLineParam2, _iLineParam3);
                        
                        if (_imageHougher.GetSetFindedPoints.Count < 2)
                        {
                            Struct_CutImg.iSearchCount++;
                            continue;
                        }
                        List<CvPoint> FindedPoints = _imageHougher.GetSetFindedPoints;

                        Struct_CutImg.resultPoint = Inspect_Run_Run_Finded_Lines(ROI_Zone_Image[i], FindedPoints, iLstNowSidNo[i], sLstNowPolNo[i], sLstNowTypNo[i]);

                        if (Struct_CutImg.resultPoint.X == 0 && Struct_CutImg.resultPoint.Y == 0)
                        {
                            //point_Find_Result = false;
                            Struct_CutImg.iSearchCount++;
                        }
                        else
                        {
                            Struct_CutImg.point_Find_Result = true;
                            FormDlgInsp_Inspection_Write_LogFile(NowGapNumber.ToString("00") + "\t" + i.ToString("00") + "\t" + Struct_CutImg.iSearchCount.ToString() + "\t" +Struct_CutImg.EdgeParam1.ToString("000") + "\t" + Struct_CutImg.EdgeParam2.ToString("000") + "\t" + _iEdgeParam3.ToString("00") + "\t" + _iLineParam1.ToString("00") + "\t" +_iLineParam2.ToString("000") + "\t" +_iLineParam3.ToString("000") + "\r\n");
                        }

                    } while (Struct_CutImg.point_Find_Result == false && Struct_CutImg.iSearchCount < 3);

                    cvPntLstCenterPoint.Add(Struct_CutImg.resultPoint);
                    SrcIplImageGap.ResetROI();
                }//end for
                return true;
            } //end try
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
            
        }//end function

        */

        /*
         public bool Inspect_Run_Run_ROI_EdgeLine_Centering(bool MeasMode)
        {
            try
            {
                //var Struct_CuttingImage = new CuttingZone_To_Image();
                Struct_CuttingImage.resultPoint = new CvPoint(0, 0);

                bool point_Find_Result = false;

                //인스펙션 로그 파일 기록 함수 호출 (3:이미지 분석 진행)
                //FormDlgInsp_Inspection_LogData_File_Data_Write(3);
                //Inspect_Run_Run_CuttingZone_To_Image_Display();

                cvPntLstCenterPoint.Clear();
                Lst_ROI_Search_Result.Clear();

                //CvWindow.ShowImages(SrcIplImageGap);

                for (int i = 0; i < GapSystem.RectListImageZone_Gap.Count; i++)
                {
                    int iSearchCount = 0;
                    do
                    {
                        switch (iSearchCount)
                        {
                            case 0:
                                //_iEdgeParam1 = _threshold01[0]; _iEdgeParam2 = _threshold01[1];
                                break;
                            case 1:
                                _iEdgeParam1 = _threshold02[0]; _iEdgeParam2 = _threshold02[1];
                                break;
                            case 2:
                                _iEdgeParam1 = (_threshold02[0] < _iEdgeParam3) ? 0 : _threshold02[0] - _iEdgeParam3;
                                _iEdgeParam2 = (_threshold02[1] < _iEdgeParam3) ? 0 : _threshold02[1] - _iEdgeParam3;
                                break;
                        }


                        NowROI_Number = i;
                        _imageHougher.GetSet_NowROI = i;

                        Cv.SetImageROI(SrcIplImageGap, ROI_Zone_Rect[i]);
                        Cv.Copy(SrcIplImageGap, ROI_Zone_Image[i]);

                        //CvWindow.ShowImages(ROI_Zone_Image[i]);

                        Struct_CuttingImage.resultPoint.X = 0;
                        Struct_CuttingImage.resultPoint.Y = 0;

                        _imageHougher.HoughLines_Point(ROI_Zone_Image[i], _iEdgeParam1, _iEdgeParam2, _iLineParam1, _iLineParam2, _iLineParam3);
                        if (_imageHougher.GetSetFindedPoints.Count < 2)
                        {
                            iSearchCount++;
                            continue;
                        }

                        Struct_CuttingImage.resultPoint = Inspect_Run_Run_Finded_Lines(ROI_Zone_Image[i], _imageHougher.GetSetFindedPoints, iLstNowSidNo[i], sLstNowPolNo[i], sLstNowTypNo[i]);
                        //Struct_CuttingImage.resultPoint = Inspect_Run_Run_Finded_Lines(ROI_Zone_Image[i], HoughLines_Points[i], _iLstNowSidNo[i], _sLstNowPolNo[i], _sLstNowTypNo[i]);

                        if (Struct_CuttingImage.resultPoint.X == 0 && Struct_CuttingImage.resultPoint.Y == 0) point_Find_Result = false;
                        else point_Find_Result = true;

                        iSearchCount++;

                    } while (point_Find_Result == false && iSearchCount < 3);

                    cvPntLstCenterPoint.Add(Struct_CuttingImage.resultPoint);
                    SrcIplImageGap.ResetROI();

                    if (point_Find_Result == true) Lst_ROI_Search_Result.Add(true);
                    else Lst_ROI_Search_Result.Add(false);

                }//end for
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            
        }//end function

        */
        /*
         public bool Inspect_Run_Run_ROI_EdgeLine_Centering(bool MeasMode)
        {
            var Struct_CuttingImage = new CuttingZone_To_Image();
            Struct_CuttingImage.resultPoint = new CvPoint(0, 0);

            bool point_Find_Result = false;

            //인스펙션 로그 파일 기록 함수 호출 (3:이미지 분석 진행)
            //FormDlgInsp_Inspection_LogData_File_Data_Write(3);
            //Inspect_Run_Run_CuttingZone_To_Image_Display();

            cvPntLstCenterPoint.Clear();
            Lst_ROI_Search_Result.Clear();

            //CvWindow.ShowImages(SrcIplImageGap);

            for (int i = 0; i < GapSystem.RectListImageZone_Gap.Count; i++)
            {
                int iSearchCount = 0;
                do
                {
                    switch (iSearchCount)
                    {
                        case 0:
                            //_iEdgeParam1 = _threshold01[0]; _iEdgeParam2 = _threshold01[1];
                            break;
                        case 1:
                            _iEdgeParam1 = _threshold02[0]; _iEdgeParam2 = _threshold02[1];
                            break;
                        case 2:
                            _iEdgeParam1 = (_threshold02[0] < _iEdgeParam3) ? 0 : _threshold02[0] - _iEdgeParam3;
                            _iEdgeParam2 = (_threshold02[1] < _iEdgeParam3) ? 0 : _threshold02[1] - _iEdgeParam3;
                            break;
                    }


                    NowROI_Number = i;
                    _imageHougher.GetSet_NowROI = i;

                    Cv.SetImageROI(SrcIplImageGap, ROI_Zone_Rect[i]);
                    Cv.Copy(SrcIplImageGap, ROI_Zone_Image[i]);

                    //CvWindow.ShowImages(ROI_Zone_Image[i]);

                    Struct_CuttingImage.resultPoint.X = 0;
                    Struct_CuttingImage.resultPoint.Y = 0;

                    _imageHougher.HoughLines_Point(ROI_Zone_Image[i], _iEdgeParam1, _iEdgeParam2, _iLineParam1, _iLineParam2, _iLineParam3);
                    if (_imageHougher.GetSetFindedPoints.Count < 2)
                    {
                        iSearchCount++;
                        continue;
                    }

                    Struct_CuttingImage.resultPoint = Inspect_Run_Run_Finded_Lines(ROI_Zone_Image[i], _imageHougher.GetSetFindedPoints, iLstNowSidNo[i], sLstNowPolNo[i], sLstNowTypNo[i]);
                    //Struct_CuttingImage.resultPoint = Inspect_Run_Run_Finded_Lines(ROI_Zone_Image[i], HoughLines_Points[i], _iLstNowSidNo[i], _sLstNowPolNo[i], _sLstNowTypNo[i]);

                    if (Struct_CuttingImage.resultPoint.X == 0 && Struct_CuttingImage.resultPoint.Y == 0) point_Find_Result = false;
                    else point_Find_Result = true;

                    iSearchCount++;

                } while (point_Find_Result == false && iSearchCount < 3);

                cvPntLstCenterPoint.Add(Struct_CuttingImage.resultPoint);
                SrcIplImageGap.ResetROI();
                
                if (point_Find_Result == true) Lst_ROI_Search_Result.Add(true);
                else Lst_ROI_Search_Result.Add(false);

            }//end for
            return true;
        }//end function
        */
        public struct CntFind_Struct
        {
            public int cntPntAdd;
            public string tmpTypeNo;
            public int tmpSeqNo;
            public int tmpRoiNo ;
            public CvRect tmpRect;
            public CvPoint zeroCvPoint;
            public CvPoint tmpPnt;
            public CvPoint rCvPoint;
        }

//         public struct CenterPoint_Struct
//         {
//             public  List<CvPoint> cvPntLstImagePoint;// = new List<CvPoint>();
//         }
// 
//         public CenterPoint_Struct CntPoint_List = new CenterPoint_Struct();

        //우기백 2014.07.23
        //센터 포인트 저장 배열이 초기화 되지 않아서 
        //선언 후 함수내부에서 객체를 생성하도록 수정함.
        public void Inspect_Run_Run_ROI_CenterPoint_Find_Uper()
        {
            try
            {

                CntFind_Struct Struct_CntFind = new CntFind_Struct();
                Struct_CntFind.zeroCvPoint = new CvPoint(0, 0);
                //타입별 시작 번호가 달라서 찾은 센터 포인트 주소를 만들어주어 카운트 한다.
                Struct_CntFind.cntPntAdd = 0;
                //CntPoint_List.cvPntLstImagePoint = new List<CvPoint>();
                cvPntLstImagePoint_Uper = new List<CvPoint>();
                for (int i = 0; i < LamiSystem.RectListImageZone_Uper.Count; i++)
                {
                    Struct_CntFind.tmpTypeNo = sLstNowTypNo_Uper[i];
                    Struct_CntFind.tmpSeqNo = iLstNowSeqNo_Uper[i];
                    Struct_CntFind.tmpRoiNo = iLstNowRoiNo_Uper[i];
                    Struct_CntFind.tmpRect = LamiSystem.RectListImageZone_Uper[Struct_CntFind.tmpRoiNo];

                    if (_cvPntLstCenterPoint_Uper[Struct_CntFind.cntPntAdd].X == 0 && _cvPntLstCenterPoint_Uper[Struct_CntFind.cntPntAdd].Y == 0)
                    {
                        //Struct_CntFind.zeroCvPoint = new CvPoint(0, 0);
                        cvPntLstImagePoint_Uper.Add(Struct_CntFind.zeroCvPoint);

                        //디버깅 용으로 사용하는 함수 이다.
                        //검출 구역의 포인트를 표시해준다.
                        //Inspect_Run_Run_Drawing_Result_Location_Point(zeroCvPoint, i);
                    }
                    else
                    {
                        Struct_CntFind.tmpPnt = _cvPntLstCenterPoint_Uper[Struct_CntFind.cntPntAdd];
                        Struct_CntFind.rCvPoint = new CvPoint(Struct_CntFind.tmpRect.X + Struct_CntFind.tmpPnt.X, Struct_CntFind.tmpRect.Y + Struct_CntFind.tmpPnt.Y);
                        cvPntLstImagePoint_Uper.Add(Struct_CntFind.rCvPoint);

                        //디버깅 용으로 사용하는 함수 이다.
                        //검출 구역의 포인트를 표시해준다.
                        //Inspect_Run_Run_Drawing_Result_Location_Point(rCvPoint, i);
                    }
                    Struct_CntFind.cntPntAdd++;
                }
                //CvWindow.ShowImages(_nowIplImage);
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }


        public void Inspect_Run_Run_ROI_CenterPoint_Find_Down()
        {
            try
            {
                CntFind_Struct Struct_CntFind = new CntFind_Struct();
                Struct_CntFind.zeroCvPoint = new CvPoint(0, 0);

                Struct_CntFind.cntPntAdd = 0;
                //CntPoint_List.cvPntLstImagePoint = new List<CvPoint>();
                cvPntLstImagePoint_Down = new List<CvPoint>();
                for (int i = 0; i < LamiSystem.RectListImageZone_Down.Count; i++)
                {
                    //string NowUsed = sLstNowDisNo_Down[i];
                    //if (NowUsed == "False") continue;

                    Struct_CntFind.tmpTypeNo = sLstNowTypNo_Down[i];
                    Struct_CntFind.tmpSeqNo = iLstNowSeqNo_Down[i];
                    Struct_CntFind.tmpRoiNo = iLstNowRoiNo_Down[i];
                    Struct_CntFind.tmpRect = LamiSystem.RectListImageZone_Down[Struct_CntFind.tmpRoiNo];

                    if (_cvPntLstCenterPoint_Down[Struct_CntFind.cntPntAdd].X == 0 && _cvPntLstCenterPoint_Down[Struct_CntFind.cntPntAdd].Y == 0)
                    {
                        //Struct_CntFind.zeroCvPoint = new CvPoint(0, 0);
                        cvPntLstImagePoint_Down.Add(Struct_CntFind.zeroCvPoint);

                        //디버깅 용으로 사용하는 함수 이다.
                        //검출 구역의 포인트를 표시해준다.
                        //Inspect_Run_Run_Drawing_Result_Location_Point(zeroCvPoint, i);
                    }
                    else
                    {
                        Struct_CntFind.tmpPnt = _cvPntLstCenterPoint_Down[Struct_CntFind.cntPntAdd];
                        Struct_CntFind.rCvPoint = new CvPoint(Struct_CntFind.tmpRect.X + Struct_CntFind.tmpPnt.X, Struct_CntFind.tmpRect.Y + Struct_CntFind.tmpPnt.Y);
                        cvPntLstImagePoint_Down.Add(Struct_CntFind.rCvPoint);

                        //디버깅 용으로 사용하는 함수 이다.
                        //검출 구역의 포인트를 표시해준다.
                        //Inspect_Run_Run_Drawing_Result_Location_Point(rCvPoint, i);
                    }
                    Struct_CntFind.cntPntAdd++;
                }
                //CvWindow.ShowImages(_nowIplImage);

                if (GetSet_PixLoad == true)
                {
                    int Garo = Math.Abs(cvPntLstImagePoint_Down[0].X - cvPntLstImagePoint_Down[1].X);
                    int Sero = Math.Abs(cvPntLstImagePoint_Down[2].Y - cvPntLstImagePoint_Down[3].Y);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        /*
        public void Inspect_Run_Run_ROI_CenterPoint_Find()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            CntFind_Struct Struct_CntFind = new CntFind_Struct();
            //타입별 시작 번호가 달라서 찾은 센터 포인트 주소를 만들어주어 카운트 한다.
            Struct_CntFind. cntPntAdd = 0;
            cvPntLstImagePoint.Clear();
            for (int i = 0; i < GapSystem.RectListImageZone_Gap.Count; i++)
            {
                Struct_CntFind.tmpTypeNo = sLstNowTypNo[i];
                Struct_CntFind.tmpSeqNo = iLstNowSeqNo[i];
                Struct_CntFind.tmpRoiNo = iLstNowRoiNo[i];
                Struct_CntFind.tmpRect = GapSystem.RectListImageZone_Gap[Struct_CntFind.tmpRoiNo];

                if (cvPntLstCenterPoint[Struct_CntFind.cntPntAdd].X == 0 && cvPntLstCenterPoint[Struct_CntFind.cntPntAdd].Y == 0)
                {
                    Struct_CntFind.zeroCvPoint = new CvPoint(0, 0);
                    cvPntLstImagePoint.Add(Struct_CntFind.zeroCvPoint);

                    //디버깅 용으로 사용하는 함수 이다.
                    //검출 구역의 포인트를 표시해준다.
                    //Inspect_Run_Run_Drawing_Result_Location_Point(zeroCvPoint, i);
                }
                else
                {
                    Struct_CntFind.tmpPnt = cvPntLstCenterPoint[Struct_CntFind.cntPntAdd];
                    Struct_CntFind.rCvPoint = new CvPoint(Struct_CntFind.tmpRect.X + Struct_CntFind.tmpPnt.X, Struct_CntFind.tmpRect.Y + Struct_CntFind.tmpPnt.Y);
                    cvPntLstImagePoint.Add(Struct_CntFind.rCvPoint);

                    //디버깅 용으로 사용하는 함수 이다.
                    //검출 구역의 포인트를 표시해준다.
                    //Inspect_Run_Run_Drawing_Result_Location_Point(rCvPoint, i);
                }
                Struct_CntFind.cntPntAdd++;
            }

            //CvWindow.ShowImages(_nowIplImage);
        }
        */
        /*
        
        public void Inspect_Run_Run_ROI_CenterPoint_Find()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            //타입별 시작 번호가 달라서 찾은 센터 포인트 주소를 만들어주어 카운트 한다.
            int cntPntAdd = 0;
            cvPntLstImagePoint.Clear();
            for (int i = 0; i < GapSystem.RectListImageZone_Gap.Count; i++)
            {
                string tmpTypeNo = sLstNowTypNo[i];
                int tmpSeqNo = iLstNowSeqNo[i];
                int tmpRoiNo = iLstNowRoiNo[i];
                CvRect tmpRect = GapSystem.RectListImageZone_Gap[tmpRoiNo];

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

        */

      

        private CvPoint Inspect_Run_Run_Finded_Lines_Uper(IplImage zoneImage, CvPoint[] findedPoints, int sideData, string polaData, string typeData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Finded_Lines Struct_FindedLines = new Finded_Lines();

            Struct_FindedLines.resultPoint = new CvPoint(0, 0);

            for (int i = 0; i < findedPoints.Length / 2; i++)
            {
                Struct_FindedLines.startPoint = findedPoints[i * 2];
                Struct_FindedLines.endPoint = findedPoints[i * 2 + 1];

                Struct_FindedLines.centerPoint =
                    Inspect_Run_Run_Finded_Lines_CenterPoint_Check(Struct_FindedLines.startPoint,
                        Struct_FindedLines.endPoint);
                if (Struct_FindedLines.centerPoint.X < 0 || Struct_FindedLines.centerPoint.Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    return Struct_FindedLines.resultPoint;
                }


                Struct_FindedLines.measurePoints =
                    Inspect_Run_Run_Finded_Lines_MeasurePoint_Check(Struct_FindedLines.centerPoint, sideData);
                if (Struct_FindedLines.measurePoints[0].X >= zoneImage.Width ||
                    Struct_FindedLines.measurePoints[1].X >= zoneImage.Width ||
                    Struct_FindedLines.measurePoints[0].Y >= zoneImage.Height ||
                    Struct_FindedLines.measurePoints[1].Y >= zoneImage.Height ||
                    Struct_FindedLines.measurePoints[0].X < 0 || Struct_FindedLines.measurePoints[1].X < 0 ||
                    Struct_FindedLines.measurePoints[0].Y < 0 || Struct_FindedLines.measurePoints[1].Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    return Struct_FindedLines.resultPoint;
                }


                bool usedLineCheck = Inspect_Run_Run_Finded_Lines_UsingLine_Check_Uper(zoneImage, Struct_FindedLines.measurePoints, polaData, sideData);

                if (usedLineCheck)
                {
                    return Struct_FindedLines.centerPoint;
                }
            }
            return Struct_FindedLines.resultPoint;
        }

        public struct Struct_UsingLine_Check
        {
            public int intMethod;
            public CvScalar v1;// = zoneImage.Get2D(measPoint[0].Y, measPoint[0].X);
            public CvScalar v2;// = zoneImage.Get2D(measPoint[1].Y, measPoint[1].X);

            public int iGridRow;// = iLstNowRowNo_Uper[_nowRoiNumber_Uper];
            public int BrightDeferentValue;//e = 0;
            public bool result;// = int.TryParse(LamiSystem.StrLstRcpConGridData_Uper[(iGridRow * 11) + 10], out BrightDeferentValue);
            public bool checkResult;//
        }

        //허프에서 발경한 3개의 라인 중에서 설정 조건에 
        //1.거리, 넓이, 2,극성 의 설정 조건에 해당하는 //라인을 찾는다.
        //만약 라인이 없다면 어떻게 할 것인지를 결정해야한다.
        //다시 허프조건을 변경해서 찾을 것인지 판다해서
        //재 검색 모듈을 제작해야한다.
        private bool Inspect_Run_Run_Finded_Lines_UsingLine_Check_Uper(IplImage zoneImage, CvPoint[] measPoint, string polaData, int sideData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
             Trace.WriteLine("_nowRoiNumber_Uper : " + _nowRoiNumber_Uper.ToString("00"));
            Trace.WriteLine("zoneImage : " + zoneImage.Width.ToString("0000") +" " + zoneImage.Height.ToString("0000"));
            Trace.WriteLine("polaData : " + polaData);
            Trace.WriteLine("polaData : " + sideData);

            Struct_UsingLine_Check UsingLine = new Struct_UsingLine_Check();

            UsingLine.intMethod = -1;
            if (polaData == "흑백") UsingLine.intMethod = 0;
            else if (polaData == "백흑") UsingLine.intMethod = 1;

            //만약 측정좌표가 이미지의 크기에서 벗어났는지를 검사한다.
            //벗어났으면 False를 리턴한다. 이때 어떻게 할것이지 결졍해서 추가 해야함.
            if (measPoint[0].X >= zoneImage.Width || measPoint[1].X >= zoneImage.Width ||
                measPoint[0].Y >= zoneImage.Height || measPoint[1].Y >= zoneImage.Height ||
                measPoint[0].X < 0 || measPoint[1].X < 0 || measPoint[0].Y < 0 || measPoint[1].Y < 0)
                return false;

            UsingLine.v1 = zoneImage.Get2D(measPoint[0].Y, measPoint[0].X);
            UsingLine.v2 = zoneImage.Get2D(measPoint[1].Y, measPoint[1].X);

            UsingLine.iGridRow = iLstNowRowNo_Uper[_nowRoiNumber_Uper];
            Trace.WriteLine("UsingLine.iGridRow : " + UsingLine.iGridRow.ToString("00"));

            UsingLine.BrightDeferentValue = 0;
            UsingLine.result = int.TryParse(LamiSystem.StrLstRcpConGridData_Uper[(UsingLine.iGridRow * 11) + 10], out UsingLine.BrightDeferentValue);
            if (UsingLine.result == false) return false;

            //string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
            //if (NowROI_Number == 2)
            //Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(zoneImage, measPoint, v1[0], v2[0]);

            UsingLine.checkResult = false;
            if (UsingLine.intMethod == 0 && sideData == 0)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                return false;
            }
            if (UsingLine.intMethod == 0 && sideData == 1)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 0 && sideData == 2)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 0 && sideData == 3)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 0)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 1)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 2)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 3)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue)) 
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            return false;
        }


        private bool Inspect_Run_Run_Finded_Lines_UsingLine_Check_Down(IplImage zoneImage, CvPoint[] measPoint, string polaData, int sideData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            Struct_UsingLine_Check UsingLine = new Struct_UsingLine_Check();

            UsingLine.intMethod = -1;
            if (polaData == "흑백") UsingLine.intMethod = 0;
            else if (polaData == "백흑") UsingLine.intMethod = 1;

            //만약 측정좌표가 이미지의 크기에서 벗어났는지를 검사한다.
            //벗어났으면 False를 리턴한다. 이때 어떻게 할것이지 결졍해서 추가 해야함.
            if (measPoint[0].X >= zoneImage.Width || measPoint[1].X >= zoneImage.Width ||
                measPoint[0].Y >= zoneImage.Height || measPoint[1].Y >= zoneImage.Height ||
                measPoint[0].X < 0 || measPoint[1].X < 0 || measPoint[0].Y < 0 || measPoint[1].Y < 0)
                return false;

            UsingLine.v1 = zoneImage.Get2D(measPoint[0].Y, measPoint[0].X);
            UsingLine.v2 = zoneImage.Get2D(measPoint[1].Y, measPoint[1].X);

            UsingLine.iGridRow = iLstNowRowNo_Down[_nowRoiNumber_Down];
            UsingLine.BrightDeferentValue = 0;
            UsingLine.result = int.TryParse(LamiSystem.StrLstRcpConGridData_Down[(UsingLine.iGridRow * 11) + 10], out UsingLine.BrightDeferentValue);
            if (UsingLine.result == false) return false;

            //string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
            //if (NowROI_Number == 2)
            //Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(zoneImage, measPoint, v1[0], v2[0]);

            UsingLine.checkResult = false;
            if (UsingLine.intMethod == 0 && sideData == 0)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue))
                    return true;
                return false;
            }
            if (UsingLine.intMethod == 0 && sideData == 1)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 0 && sideData == 2)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 0 && sideData == 3)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 0)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 1)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 2)
            {
                if ((UsingLine.v1[0] > UsingLine.v2[0]) && (UsingLine.v1[0] - UsingLine.v2[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (UsingLine.intMethod == 1 && sideData == 3)
            {
                if ((UsingLine.v1[0] < UsingLine.v2[0]) && (UsingLine.v2[0] - UsingLine.v1[0] > UsingLine.BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            return false;
        }

        /*
        
        private bool Inspect_Run_Run_Finded_Lines_UsingLine_Check_Uper(IplImage zoneImage, CvPoint[] measPoint, string polaData, int sideData)
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
           
            int iGridRow = iLstNowRowNo_Uper[_nowRoiNumber_Uper];
            int BrightDeferentValue = 0;
            bool result = int.TryParse(LamiSystem.StrLstRcpConGridData_Uper[(iGridRow * 11) + 10], out BrightDeferentValue);
            if (result == false) return false;

            //string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
            //if (NowROI_Number == 2)
            //Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(zoneImage, measPoint, v1[0], v2[0]);

            bool checkResult = false;
            if (intMethod == 0 && sideData == 0)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) 
                    return true;
                return false;
            }
            if (intMethod == 0 && sideData == 1)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) 
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 2)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) 
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 3)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) 
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 0)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 1)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) 
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 2)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue)) 
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 3)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue)) 
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            return false;
        }


        private bool Inspect_Run_Run_Finded_Lines_UsingLine_Check_Down(IplImage zoneImage, CvPoint[] measPoint, string polaData, int sideData)
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

            int iGridRow = iLstNowRowNo_Down[_nowRoiNumber_Down];
            int BrightDeferentValue = 0;
            bool result = int.TryParse(LamiSystem.StrLstRcpConGridData_Down[(iGridRow * 11) + 10], out BrightDeferentValue);
            if (result == false) return false;

            //string tmpstr = _gapSystem.StrLstRcpConGridData_BiCell[10];
            //테스트용을 위해서 측정 포인트를 표시해 주는 함수.
            //if (NowROI_Number == 2)
            //Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(zoneImage, measPoint, v1[0], v2[0]);

            bool checkResult = false;
            if (intMethod == 0 && sideData == 0)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue))
                    return true;
                return false;
            }
            if (intMethod == 0 && sideData == 1)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 2)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 0 && sideData == 3)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 0)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 1)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 2)
            {
                if ((v1[0] > v2[0]) && (v1[0] - v2[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] > v2[0]) return true;
                return false;
            }
            if (intMethod == 1 && sideData == 3)
            {
                if ((v1[0] < v2[0]) && (v2[0] - v1[0] > BrightDeferentValue))
                    return true;
                //if (v1[0] < v2[0]) return true;
                return false;
            }
            return false;
        }

        */

        public void Inspect_Run_Run_Finded_Lines_MeasurePoint_Show(IplImage zoneImage, CvPoint[] measPoint, double v1, double v2)
        {
            zoneImage.Circle(measPoint[0],2,CvColor.Orange);
            //zoneImage.PutText(v1.ToString(), measPoint[0] , CvColor.Orange);
            zoneImage.Circle(measPoint[1], 2, CvColor.Orange);
            //CvWindow.ShowImages(zoneImage);
        }

        public struct Finded_Lines
        {
            public CvPoint centerPoint;
            public CvPoint endPoint;
            public CvPoint[] measurePoints;
            public CvPoint resultPoint;
            public CvPoint startPoint;
        }

        //private Finded_Lines Struct_FindedLines;

       
        private CvPoint Inspect_Run_Run_Finded_Lines_Uper(IplImage zoneImage, List<CvPoint> findedPoints, int sideData, string polaData, string typeData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            Finded_Lines Struct_FindedLines = new Finded_Lines();

            Struct_FindedLines.resultPoint = new CvPoint(0, 0);

            for (int i = 0; i < findedPoints.Count / 2; i++)
            {
                Struct_FindedLines.startPoint = findedPoints[i * 2];
                Struct_FindedLines.endPoint = findedPoints[i * 2 + 1];

                Struct_FindedLines.centerPoint = Inspect_Run_Run_Finded_Lines_CenterPoint_Check(Struct_FindedLines.startPoint,Struct_FindedLines.endPoint);
                if (Struct_FindedLines.centerPoint.X < 0 || Struct_FindedLines.centerPoint.Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    //MessageBox.Show("Inspect_Run_Run_Finded_Lines : Inspect_Run_Run_Finded_Lines_CenterPoint_Check \r\n" +"에서 측정값이 0보다 적게 계산되었습니다.");
                    return Struct_FindedLines.resultPoint;
                }

                Struct_FindedLines.measurePoints = Inspect_Run_Run_Finded_Lines_MeasurePoint_Check(Struct_FindedLines.centerPoint, sideData);
                if (Struct_FindedLines.measurePoints[0].X >= zoneImage.Width ||
                    Struct_FindedLines.measurePoints[1].X >= zoneImage.Width ||
                    Struct_FindedLines.measurePoints[0].Y >= zoneImage.Height ||
                    Struct_FindedLines.measurePoints[1].Y >= zoneImage.Height ||
                    Struct_FindedLines.measurePoints[0].X < 0 || Struct_FindedLines.measurePoints[1].X < 0 ||
                    Struct_FindedLines.measurePoints[0].Y < 0 || Struct_FindedLines.measurePoints[1].Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    //MessageBox.Show("Inspect_Run_Run_Finded_Lines : Inspect_Run_Run_Finded_Lines_MeasurePoint_Check \r\n" +"에서 측정값이 ROI 범위를 벗어 났습니다.");

                    return Struct_FindedLines.resultPoint;
                }


                bool usedLineCheck = Inspect_Run_Run_Finded_Lines_UsingLine_Check_Uper(zoneImage, Struct_FindedLines.measurePoints, polaData, sideData);
                if (usedLineCheck)
                {
                    return Struct_FindedLines.centerPoint;
                }
            }
            return Struct_FindedLines.resultPoint;
        }

        private CvPoint Inspect_Run_Run_Finded_Lines_Down(IplImage zoneImage, List<CvPoint> findedPoints, int sideData, string polaData, string typeData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            Finded_Lines Struct_FindedLines = new Finded_Lines();

            Struct_FindedLines.resultPoint = new CvPoint(0, 0);

            for (int i = 0; i < findedPoints.Count / 2; i++)
            {
                Struct_FindedLines.startPoint = findedPoints[i * 2];
                Struct_FindedLines.endPoint = findedPoints[i * 2 + 1];

                Struct_FindedLines.centerPoint = Inspect_Run_Run_Finded_Lines_CenterPoint_Check(Struct_FindedLines.startPoint, Struct_FindedLines.endPoint);
                if (Struct_FindedLines.centerPoint.X < 0 || Struct_FindedLines.centerPoint.Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    //MessageBox.Show("Inspect_Run_Run_Finded_Lines : Inspect_Run_Run_Finded_Lines_CenterPoint_Check \r\n" +"에서 측정값이 0보다 적게 계산되었습니다.");
                    return Struct_FindedLines.resultPoint;
                }

                Struct_FindedLines.measurePoints = Inspect_Run_Run_Finded_Lines_MeasurePoint_Check(Struct_FindedLines.centerPoint, sideData);
                if (Struct_FindedLines.measurePoints[0].X >= zoneImage.Width ||
                    Struct_FindedLines.measurePoints[1].X >= zoneImage.Width ||
                    Struct_FindedLines.measurePoints[0].Y >= zoneImage.Height ||
                    Struct_FindedLines.measurePoints[1].Y >= zoneImage.Height ||
                    Struct_FindedLines.measurePoints[0].X < 0 || Struct_FindedLines.measurePoints[1].X < 0 ||
                    Struct_FindedLines.measurePoints[0].Y < 0 || Struct_FindedLines.measurePoints[1].Y < 0)
                {
                    //Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(zoneImage, startPoint, endPoint);
                    //MessageBox.Show("Inspect_Run_Run_Finded_Lines : Inspect_Run_Run_Finded_Lines_MeasurePoint_Check \r\n" +"에서 측정값이 ROI 범위를 벗어 났습니다.");

                    return Struct_FindedLines.resultPoint;
                }


                bool usedLineCheck = Inspect_Run_Run_Finded_Lines_UsingLine_Check_Down(zoneImage, Struct_FindedLines.measurePoints, polaData, sideData);
                if (usedLineCheck)
                {
                    return Struct_FindedLines.centerPoint;
                }
            }
            return Struct_FindedLines.resultPoint;
        }


        public struct Struck_Lines_MeasurePoint
        {
            public CvPoint measPoint1;
            public CvPoint measPoint2;
            public CvPoint[] measPoints;
        }

        private CvPoint[] Inspect_Run_Run_Finded_Lines_MeasurePoint_Check(CvPoint centerPoint, int sideData)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            Struck_Lines_MeasurePoint MeasurePoint = new Struck_Lines_MeasurePoint();

            MeasurePoint.measPoint1 = new CvPoint(0, 0);
            MeasurePoint.measPoint2 = new CvPoint(0, 0);

            //sideData = 0, 2이면 받은 이미지를 Y측으로 분할 해서 분할된 이미지의 밝기를 비교한다.
            //sideData = 1, 3이면 받은 이미지를 X측으로 분할 해서 분할된 이미지의 밝기를 비교한다.
            //측정 위치는 X,Y측의 중간값의 중심으로 각각 5씩 이동한 곳에서 측정한다.

            switch (sideData)
            {
                //위쪽을 향하도록 설정 시
                case 0:
                    MeasurePoint.measPoint1.X = centerPoint.X;
                    MeasurePoint.measPoint1.Y = centerPoint.Y - shiftCenter;

                    MeasurePoint.measPoint2.X = centerPoint.X;
                    MeasurePoint.measPoint2.Y = centerPoint.Y + shiftCenter;
                    break;
                case 1:
                    MeasurePoint.measPoint1.X = centerPoint.X - shiftCenter;
                    MeasurePoint.measPoint1.Y = centerPoint.Y;

                    MeasurePoint.measPoint2.X = centerPoint.X + shiftCenter;
                    MeasurePoint.measPoint2.Y = centerPoint.Y;
                    break;
                case 2:
                    MeasurePoint.measPoint1.X = centerPoint.X;
                    MeasurePoint.measPoint1.Y = centerPoint.Y - shiftCenter;

                    MeasurePoint.measPoint2.X = centerPoint.X;
                    MeasurePoint.measPoint2.Y = centerPoint.Y + shiftCenter;
                    break;
                case 3:
                    MeasurePoint.measPoint1.X = centerPoint.X - shiftCenter;
                    MeasurePoint.measPoint1.Y = centerPoint.Y;

                    MeasurePoint.measPoint2.X = centerPoint.X + shiftCenter;
                    MeasurePoint.measPoint2.Y = centerPoint.Y;
                    break;
            }
            MeasurePoint.measPoints = new CvPoint[2];
            MeasurePoint.measPoints[0] = MeasurePoint.measPoint1;
            MeasurePoint.measPoints[1] = MeasurePoint.measPoint2;

            //Trace.WriteLine(centerPoint.X.ToString() + ":" + centerPoint.Y.ToString() + "   " + measPoint1.X.ToString() + ":" + measPoint1.Y.ToString() + "   " + measPoint2.X.ToString() + ":" + measPoint2.Y.ToString());

            return MeasurePoint.measPoints;
        }

        /*
        
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

        */
        public struct Struck_Lines_CenterPoint
        {
            public CvPoint pt;
            public float a;
            public float b;
            public double d;
            public int xdist;

            public float A;// = a * a + 1;
            public float B;// = 2 * (-pt1.X + a * b - a * pt1.Y);
            public float C;// = pt1.X * pt1.X + b * b - 2 * b * pt1.Y * pt1.Y - (xdist * xdist);
            public string strfloat1;
            public string strfloat2;
            public string strfloat3;
        }

        private CvPoint Inspect_Run_Run_Finded_Lines_CenterPoint_Check(CvPoint pt1, CvPoint pt2)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            Struck_Lines_CenterPoint CenterPoint = new Struck_Lines_CenterPoint();
            CenterPoint.pt = new CvPoint(0, 0);

            //CenterPoint.a;
            if (pt1.X == pt2.X)
            {
                CenterPoint.pt.X = pt2.X;
                if (pt1.Y > pt2.Y)
                    CenterPoint.pt.Y = (pt1.Y - pt2.Y) / 2 + pt2.Y;
                else
                    CenterPoint.pt.Y = (pt2.Y - pt1.Y) / 2 + pt1.Y;
                return CenterPoint.pt;
            }
            if (pt1.Y == pt2.Y)
            {
                if (pt1.X > pt2.X)
                    CenterPoint.pt.X = (pt1.X - pt2.X) / 2 + pt2.X;
                else
                    CenterPoint.pt.X = (pt2.X - pt1.X) / 2 + pt1.X;
                CenterPoint.pt.Y = pt2.Y;
                return CenterPoint.pt;
            }
            CenterPoint.a = (pt2.Y - pt1.Y) / (float)(pt2.X - pt1.X);

            if (CenterPoint.a < 1)
            {
                if (pt1.X > pt2.X)
                    CenterPoint.pt.X = (pt1.X - pt2.X) / 2 + pt2.X;
                else
                    CenterPoint.pt.X = (pt2.X - pt1.X) / 2 + pt1.X;
                CenterPoint.pt.Y = pt2.Y;
                return CenterPoint.pt;
            }
            CenterPoint.b = pt2.Y - (CenterPoint.a * pt2.X);
            if (CenterPoint.b < 1)
            {
                CenterPoint.pt.X = pt2.X;
                if (pt1.Y > pt2.Y)
                    CenterPoint.pt.Y = (pt1.Y - pt2.Y) / 2 + pt2.Y;
                else
                    CenterPoint.pt.Y = (pt2.Y - pt1.Y) / 2 + pt1.Y;
                return CenterPoint.pt;
            }

            CenterPoint.d = Math.Sqrt(Math.Pow((pt1.X - pt2.X), 2) + Math.Pow((pt1.Y - pt2.Y), 2));
            CenterPoint.xdist = (int)CenterPoint.d / 2;

            float A = CenterPoint.a * CenterPoint.a + 1;
            float B = 2 * (-pt1.X + CenterPoint.a * CenterPoint.b - CenterPoint.a * pt1.Y);
            float C = pt1.X * pt1.X + CenterPoint.b * CenterPoint.b - 2 * CenterPoint.b * pt1.Y * pt1.Y - (CenterPoint.xdist * CenterPoint.xdist);

            if (pt1.X < pt2.X)
            {
                string strfloat1 = ((float)(-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A)).ToString();
                CenterPoint.pt.X = (int)((-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A));
            }
            else
            {
                string strfloat2 = ((float)(-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A)).ToString();
                CenterPoint.pt.X = (int)((-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A));
            }

            string strfloat3 = ((int)(CenterPoint.a * CenterPoint.pt.X + CenterPoint.b)).ToString();
            CenterPoint.pt.Y = (int)(CenterPoint.a * CenterPoint.pt.X + CenterPoint.b);
            return CenterPoint.pt;
        }

        /*
        
        private CvPoint Inspect_Run_Run_Finded_Lines_CenterPoint_Check(CvPoint pt1, CvPoint pt2)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var pt = new CvPoint(0, 0);

            float a;
            if (pt1.X == pt2.X)
            {
                pt.X = pt2.X;
                if (pt1.Y > pt2.Y)
                    pt.Y = (pt1.Y - pt2.Y) / 2 + pt2.Y;
                else
                    pt.Y = (pt2.Y - pt1.Y) / 2 + pt1.Y;
                return pt;
            }
            if (pt1.Y == pt2.Y)
            {
                if (pt1.X > pt2.X)
                    pt.X = (pt1.X - pt2.X) / 2 + pt2.X;
                else
                    pt.X = (pt2.X - pt1.X) / 2 + pt1.X;
                pt.Y = pt2.Y;
                return pt;
            }
            a = (pt2.Y - pt1.Y) / (float)(pt2.X - pt1.X);

            if (a < 1)
            {
                if (pt1.X > pt2.X)
                    pt.X = (pt1.X - pt2.X) / 2 + pt2.X;
                else
                    pt.X = (pt2.X - pt1.X) / 2 + pt1.X;
                pt.Y = pt2.Y;
                return pt;
            }
            float b = pt2.Y - (a * pt2.X);
            if (b < 1)
            {
                pt.X = pt2.X;
                if (pt1.Y > pt2.Y)
                    pt.Y = (pt1.Y - pt2.Y) / 2 + pt2.Y;
                else
                    pt.Y = (pt2.Y - pt1.Y) / 2 + pt1.Y;
                return pt;
            }

            double d = Math.Sqrt(Math.Pow((pt1.X - pt2.X), 2) + Math.Pow((pt1.Y - pt2.Y), 2));
            int xdist = (int)d / 2;

            float A = a * a + 1;
            float B = 2 * (-pt1.X + a * b - a * pt1.Y);
            float C = pt1.X * pt1.X + b * b - 2 * b * pt1.Y * pt1.Y - (xdist * xdist);

            if (pt1.X < pt2.X)
            {
                string strfloat1 = ((float)(-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A)).ToString();
                pt.X = (int)((-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A));
            }
            else
            {
                string strfloat2 = ((float)(-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A)).ToString();
                pt.X = (int)((-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A));
            }

            string strfloat3 = ((int)(a * pt.X + b)).ToString();
            pt.Y = (int)(a * pt.X + b);
            return pt;
        }

        */


        /*
        private void Inspect_Run_Run_Grab_Running_BiCell()
        {
            //트리거 일렬번호를 레지로 부터 읽은다음
            //1을 증가하고 다시 저장한다.
            Inspect_Run_Run_GetSet_TrigerNo_BiCell();
            inspection_BiCell.GetSet_NowIplImage = srcIplImage_BiCell;
            inspection_BiCell.GetSet_HistoryAll = ultraPanel6;
            inspection_BiCell.GetSet_HistoryNG = ultraPanel7;
            inspection_BiCell.Inspect_Auto_Image_Grabing(1);
            _CycleCompleteFlag_BiCell = true;
        }
        */

        /*
         * //////////////////////////////////////////////////////////
//진행 셀 정보///
//////////////////////////////////////////////////////////
P371 > 1 or 2  = 현재 진행 그립퍼의 번호를 나타낸다. 1 > Left, 2 > Right 를 바타낸다.
P372 > 1 or 2 or 3 ~ 11  = 현재 진행 셀의 번호를 나타낸다.
P373 > 1 or 2  = 현재 진행 셀의 타입을 나타낸다. 1 > A타입, 2 > C 타입. 
        */


        private void Inspect_Stop_Ready()
        {
            //이미지 저장까지 끝나면 _CycleCompleteFla 변수가 True로 셋팅된다.
            //이때 까지 기다렸다가 루프를 종료하고 정지 프로세스를 진행한다.
            while (_CycleCompleteFlag_Uper == false)
            {
                Thread.Sleep(1);
                //endflagcheck++;
                ////Trace.WriteLine("정지 버튼 눌린 후 대기 카운트 : " + endflagcheck.ToString("000000"));
            }

            Inspect_Stop_Run();
        }

        private void Inspect_Stop_Run()
        {
            if (VisionJobWorking)
            {
                VisionJobWorking = false;
            }

            MIL_Grab_Thread_Stoping();
        }

        private void MIL_Grab_Thread_Stoping()
        {
            if (InvokeRequired)
            {
                Delegate_MIL_Grab_Thread_Stoping del = MIL_Grab_Thread_Stoping;
                Invoke(del);
            }
            else
            {
#if(SIM)
                return;
#endif
                if (MIL_Grab_Threading_Uper.IsAlive) MIL_Grab_Threading_Uper.Abort();
                if (MIL_Grab_Threading_Down.IsAlive) MIL_Grab_Threading_Down.Abort();

                MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MdigHalt(MilDigitizer_Uper);
                MIL.MbufFree(MilImage_Uper); //이미지 버퍼
                MIL.MdispFree(MilDisplay_Uper); // 모니터링
                MIL.MdigFree(MilDigitizer_Uper); // 프레임그래버

                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MdigHalt(MilDigitizer_Down);
                MIL.MbufFree(MilImage_Down); //이미지 버퍼
                MIL.MdispFree(MilDisplay_Down); // 모니터링
                MIL.MdigFree(MilDigitizer_Down); // 프레임그래버

                MIL.MsysFree(MilSystem_Uper); // 그래버 추가
                MIL.MsysFree(MilSystem_Down); // 그래버 추가

                MIL.MappFree(MilApplication); //
                MIL_Trigger_Close = true;
            }
        }


        private void Inspect_MessageBox_Display(int disNo)
        {
            dlgInspMessageBox = new Control_UltraMessageBox();
            if (disNo == 1)
                dlgInspMessageBox.MessageBox_Show(
                    "Inspection", "이미지 검출 에러", "이미지 분석을 진행하는 중 설정 조건에 해당하는<br/>결과를 찾지 못했습니다. 확인하여 주십시요!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Inspect_Run_Run_Threading_Display()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Threading_Display del = Inspect_Run_Run_Threading_Display;
                Invoke(del);
            }
            else
            {
#if(SIM)
                Inspect_uLabel_Assy07.Text = _iSavedCellNumber.ToString("00") + " / " + _stopwatch.ElapsedMilliseconds.ToString("0000" + "ms");
#else
                Inspect_uLabel_Assy07.Text = _iSavedCellNumber.ToString("00");
#endif
                Inspect_uLabel_Assy07.Refresh();
            }
        }

        public void Inspect_Run_Run_Make_Fail_Check()
        {
            NowFailNumber_Uper =
                uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(LamiSystem.RegPathGapStatus, "Count_NG_Both"));
            if (NowFailNumber_Uper < uint.MaxValue)
                NowFailNumber_Uper++;
            else
                NowFailNumber_Uper = 1;
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Both", NowFailNumber_Uper.ToString());
        }


        /*
        //기존의 히스토리 픽처박스 번호가 처리용 프로세스에서 이미 다음 것을 진행하기
        //때문에 번호가 씽크되지 않아서 _iImgCount 대신에 표시부 쓰레드 시작하기 전에
        //_iImgCount 를 _iHistoryViewNo에 넘져준다. 
        private void Inspect_Run_Run_Drawing_Result_To_History_All()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_All;
                Invoke(del);
            }
            else
            {
                //파일을 새로읽어 버려서 이미지가 중간에 바뀜.
                //원본을 유지하기 위해서 이미지에 결과를 표시하기 전의 이미지를 복사한 이미지를 
                //사용하도록 한다. 이미지 이름이 _NowSavedImage 이다.

                //Inspect_uTab_ImageList22.SelectedTab = Inspect_uTab_ImageList22.Tabs["Inspect_ImageAll"];

                ((PictureBoxIpl) (uPanel_History_All_Uper.ClientArea.Controls[_iAllHistoryViewNo])).ImageIpl = _NowSavedImage;
                uPanel_History_All_Uper.ClientArea.Controls[_iAllHistoryViewNo].Refresh();

                if (_iAllHistoryViewNo + 1 == 11) _iAllHistoryViewNo = 0;
                else _iAllHistoryViewNo++;
            }
        }

        private void Inspect_Run_Run_Drawing_Result_To_History_NG()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_To_File_Display del = Inspect_Run_Run_Drawing_Result_To_History_NG;
                Invoke(del);
            }
            else
            {
                //파일을 새로읽어 버려서 이미지가 중간에 바뀜.
                //원본을 유지하기 위해서 이미지에 결과를 표시하기 전의 이미지를 복사한 이미지를 
                //사용하도록 한다. 이미지 이름이 _NowSavedImage 이다.

                if (FailZeroCheck1 || FailZeroCheck2 || _strSavedInspectResult == "NG")
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
        */

 /*
        private void Inspect_Run_Run_Image_Inspection_Drawing_Display()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Drawing_Result_Inspection_Display del =
                    Inspect_Run_Run_Image_Inspection_Drawing_Display;
                Invoke(del);
            }
            else
            {
                Inspect_Main01_IplBox.ImageIpl = SrcIplImageGap;
                Inspect_Main01_IplBox.Refresh();
                NowProdectNumber_Gap =
                    uint.Parse(Inspect_Run_Ready_TrigNo_Reg_To_Data(GapSystem.RegPathGapStatus, "Count_Trigger"));

                Inspect_uLabel_Assy05.Text = NowProdectNumber_Gap.ToString();
                Inspect_uLabel_Assy06.Text = NowFailNumber_Gap.ToString("0") + " / " + NowTrigNumber_Gap;
#if(SIM)
    //Inspect_uLabel_Assy06.Text = NowFailNumber.ToString("00000");
#endif
            }
        }

       
        private void Inspect_Run_Run_Display_PictureBox()
        {
            if (InvokeRequired)
            {
                Deligate_Run_Run_Display_PictureBox del = Inspect_Run_Run_Display_PictureBox;
                Invoke(del);
            }
            else
            {
                Inspect_Main02_IplBox.ImageIpl = _nowIplImage_BiCell;
                Inspect_Main01_IplBox.Refresh();
            }
        }
        */

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


        private void Inspect_Run_Run_CuttingZone_To_Image_Display()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_CuttingZone_To_Image_Display del = Inspect_Run_Run_CuttingZone_To_Image_Display;
                Invoke(del);
            }
        }


        private void Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(IplImage inspImage, CvPoint sPoint,
            CvPoint ePoint)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay del =
                    Inspect_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay;
                Invoke(del, inspImage, sPoint, ePoint);
            }
            else
            {
                picBox01.Visible = true;
                inspImage.DrawLine(sPoint, ePoint, CvColor.Red, 3);
                picBox01.ImageIpl = inspImage;
                picBox01.Refresh();
            }
        }


        private void Inspect_Run_Run_Test_DisplayZone_Gray_Display()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Test_DisplayZone_Gray_Display del = Inspect_Run_Run_Test_DisplayZone_Gray_Display;
                Invoke(del);
            }
            else
            {
                picBox02.Visible = true;
                picBox02.Refresh();
            }
        }

        //찾고하 하는 콘트롤 콜렉션을 받아서 콘트롤을 배열에 저장한다.
        private void getControls(Control.ControlCollection Ocontrol, ref ArrayList Space)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            for (int i = 0; i < Ocontrol.Count; i++)
            {
                Space.Add(Ocontrol[i]);
                if (Ocontrol[i].Controls.Count > 0)
                {
                    getControls(Ocontrol[i].Controls, ref Space);
                }
            }
        }

        private void Inspect_Run_Run_ImageLoading_To_Box_Display_BiCell(int imgCount) //, int HisPicNo)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_ImageLoading_To_Box_Display_BiCell del =
                    Inspect_Run_Run_ImageLoading_To_Box_Display_BiCell;
                Invoke(del, imgCount); //,HisPicNo);
            }
            else
            {
                //InspectBiCell_uLabel_Assy06.Text = traceNo.ToString("0000 :") + " " + _strLstTestImageNames_BiCell[imgCount];

                Inspect_Main01_IplBox.ImageIpl = SrcIplImage_Uper;
            }
        }

        /*
        private void Inspect_Run_Run_ImageLoading_To_Box_Display_Gap(int imgCount) //, int HisPicNo)
        {
            try
            {
                if (InvokeRequired)
                {
                    Delegate_Run_Run_ImageLoading_To_Box_Display_Gap del = Inspect_Run_Run_ImageLoading_To_Box_Display_Gap;
                    Invoke(del, imgCount); //,HisPicNo);
                }
                else
                {
                    //Inspect_uLabel_Assy06.Text = NowFailNumber.ToString("0") + " / " + NowTrigNumber;
                    //Inspect_uLabel_Assy06.Text = traceNo.ToString("0000 :") + " " + _strLstTestImageNames_Gap[imgCount];
                    //Inspect_Main01_IplBox.ImageIpl = SrcIplImageGap;
                }
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                //throw;
            }
            
        }
        */

        private void Inspect_Run_Run_Drawing_Arrows_To_Width(Rectangle inspRect, int iNowDivNo, int iNowRowNo,
            int iNowSidNo, int iNowSeqNo)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var startPoint = new Point(inspRect.X, inspRect.Y);
            var endPoint = new Point(inspRect.X + inspRect.Width, inspRect.Y + inspRect.Height);

            var TopPoint = new Point(inspRect.X, inspRect.Y);
            var ButtomPoint = new Point(inspRect.X, inspRect.Y + inspRect.Height);

            int ArrowLength = 30;
            float Axis_X = 0f;
            float Axis_Y = 0f;
            float Axis_Step = 0;

            var WidthStartPoint = new CvPoint(0, 0);
            var WidthEndPoint = new CvPoint(0, 0);

            var ArrowStartPoint = new CvPoint(0, 0);
            var ArrowEndPoint = new CvPoint(0, 0);

            var ThetaStartPoint = new CvPoint(0, 0);
            var ThetaEndPoint = new CvPoint(0, 0);

            //gc = Inspect_Main01_IplBox.CreateGraphics();
            myLinePen.Color = Color.LawnGreen;

            if (iNowSeqNo == 0)
            {
                ThetaStartPoint = boxArrowPntLst[8];
                ThetaEndPoint = boxArrowPntLst[9];

                WidthStartPoint.Y = TopPoint.Y;
                Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref WidthStartPoint);

                WidthEndPoint.Y = ButtomPoint.Y;
                Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref WidthEndPoint);

                gc_Gap.DrawLine(myLinePen, WidthStartPoint.X, WidthStartPoint.Y, WidthEndPoint.X, WidthEndPoint.Y);
            }
            else
            {
                ThetaStartPoint = boxArrowPntLst[10];
                ThetaEndPoint = boxArrowPntLst[11];

                WidthStartPoint.Y = TopPoint.Y;
                Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref WidthStartPoint);

                WidthEndPoint.Y = ButtomPoint.Y;
                Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref WidthEndPoint);

                gc_Gap.DrawLine(myLinePen, WidthStartPoint.X, WidthStartPoint.Y, WidthEndPoint.X, WidthEndPoint.Y);
            }


            if (iNowSidNo == 3 || iNowSidNo == 1)
            {
                Axis_Y = endPoint.Y - startPoint.Y;
                Axis_Step = Axis_Y/(iNowDivNo + 1);
                for (int i = 0; i < iNowDivNo; i++)
                {
                    ArrowStartPoint.Y = startPoint.Y + (int) (Axis_Step*(i + 1));
                    Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref ArrowStartPoint);
                    ArrowEndPoint.Y = startPoint.Y + (int) (Axis_Step*(i + 1));
                    ;
                    if (iNowSidNo == 1)
                    {
                        ArrowStartPoint.X = ArrowStartPoint.X + 3;
                        ArrowEndPoint.X = ArrowStartPoint.X + ArrowLength;
                    }
                    else
                    {
                        ArrowStartPoint.X = ArrowStartPoint.X - 4;
                        ArrowEndPoint.X = ArrowStartPoint.X - ArrowLength;
                    }
                    gc_Gap.DrawLine(myArrowPen, ArrowStartPoint.X, ArrowStartPoint.Y, ArrowEndPoint.X, ArrowEndPoint.Y);
                }
            }
        }

        private void Inspect_Run_Run_Drawing_Arrows_To_InspBox(Rectangle inspRect, int iNowDivNo, int iNowRowNo,
            int iNowSidNo)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var startPoint = new Point(inspRect.X, inspRect.Y);
            var endPoint = new Point(inspRect.X + inspRect.Width, inspRect.Y + inspRect.Height);

            int ArrowLength = 30;
            float Axis_X = 0f;
            float Axis_Y = 0f;
            float Axis_Step = 0;

            var ArrowStartPoint = new CvPoint(0, 0);
            var ArrowEndPoint = new CvPoint(0, 0);

            var ThetaStartPoint = new CvPoint(0, 0);
            var ThetaEndPoint = new CvPoint(0, 0);

            if (iNowRowNo == 0)
            {
                ThetaStartPoint = boxArrowPntLst[0];
                ThetaEndPoint = boxArrowPntLst[1];
            }
            else if (iNowRowNo == 1)
            {
                ThetaStartPoint = boxArrowPntLst[2];
                ThetaEndPoint = boxArrowPntLst[3];
            }
            else if (iNowRowNo == 2)
            {
                ThetaStartPoint = boxArrowPntLst[4];
                ThetaEndPoint = boxArrowPntLst[5];
            }
            else if (iNowRowNo == 3)
            {
                ThetaStartPoint = boxArrowPntLst[6];
                ThetaEndPoint = boxArrowPntLst[7];
            }

            //gc = Inspect_Main01_IplBox.CreateGraphics();
            myLinePen.Color = Color.LawnGreen;

            if (iNowSidNo == 0)
            {
                Axis_X = endPoint.X - startPoint.X;
                Axis_Step = Axis_X/(iNowDivNo + 1);
                for (int i = 0; i < iNowDivNo; i++)
                {
                    ArrowStartPoint.X = startPoint.X + (int) (Axis_Step*(i + 1));
                    Inspect_Run_Run_Drawing_Result_Arrow_Make_H(ThetaStartPoint, ThetaEndPoint, ref ArrowStartPoint);
                    ArrowStartPoint.Y = ArrowStartPoint.Y + 1;

                    ArrowEndPoint.X = startPoint.X + (int) (Axis_Step*(i + 1));
                    ArrowEndPoint.Y = ArrowStartPoint.Y + ArrowLength;

                    gc_Gap.DrawLine(myArrowPen, ArrowStartPoint.X, ArrowStartPoint.Y, ArrowEndPoint.X, ArrowEndPoint.Y);
                }
            }
            else if (iNowSidNo == 2)
            {
                Axis_X = endPoint.X - startPoint.X;
                Axis_Step = Axis_X/(iNowDivNo + 1);
                for (int i = 0; i < iNowDivNo; i++)
                {
                    ArrowStartPoint.X = startPoint.X + (int) (Axis_Step*(i + 1));
                    Inspect_Run_Run_Drawing_Result_Arrow_Make_H(ThetaStartPoint, ThetaEndPoint, ref ArrowStartPoint);
                    ArrowStartPoint.Y = ArrowStartPoint.Y - 3;

                    ArrowEndPoint.X = startPoint.X + (int) (Axis_Step*(i + 1));
                    ArrowEndPoint.Y = ArrowStartPoint.Y - ArrowLength;

                    gc_Gap.DrawLine(myArrowPen, ArrowStartPoint.X, ArrowStartPoint.Y, ArrowEndPoint.X, ArrowEndPoint.Y);
                }
            }
            else if (iNowSidNo == 3)
            {
                Axis_Y = endPoint.Y - startPoint.Y;
                Axis_Step = Axis_Y/(iNowDivNo + 1);
                for (int i = 0; i < iNowDivNo; i++)
                {
                    ArrowStartPoint.Y = startPoint.Y + (int) (Axis_Step*(i + 1));
                    Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref ArrowStartPoint);
                    ArrowStartPoint.X = ArrowStartPoint.X + 3;

                    ArrowEndPoint.X = ArrowStartPoint.X + ArrowLength;
                    ArrowEndPoint.Y = startPoint.Y + (int) (Axis_Step*(i + 1));
                    ;

                    gc_Gap.DrawLine(myArrowPen, ArrowStartPoint.X, ArrowStartPoint.Y, ArrowEndPoint.X, ArrowEndPoint.Y);
                }
            }
            else if (iNowSidNo == 1)
            {
                Axis_Y = endPoint.Y - startPoint.Y;
                Axis_Step = Axis_Y/(iNowDivNo + 1);
                for (int i = 0; i < iNowDivNo; i++)
                {
                    ArrowStartPoint.Y = startPoint.Y + (int) (Axis_Step*(i + 1));
                    Inspect_Run_Run_Drawing_Result_Arrow_Make_V(ThetaStartPoint, ThetaEndPoint, ref ArrowStartPoint);
                    ArrowStartPoint.X = ArrowStartPoint.X - 4;

                    ArrowEndPoint.X = ArrowStartPoint.X - ArrowLength;
                    ArrowEndPoint.Y = startPoint.Y + (int) (Axis_Step*(i + 1));

                    gc_Gap.DrawLine(myArrowPen, ArrowStartPoint.X, ArrowStartPoint.Y, ArrowEndPoint.X, ArrowEndPoint.Y);
                }
            }
        }

        //D:\alignTestImage
        private static string Excel_Uper_Filename;
        private static string Excel_Down_Filename;
        private static string[] GapTotal_Result = new string[30];
        private void Inspect_Run_Ready()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            int iNumber = 0;
            bool result = int.TryParse(uTxt_Gap_No.Text.Trim(), out iNumber);
            if (result) SetReg(LamiSystem.RegPathGapStatus, "NowGapNo", iNumber.ToString());

            //이값을 트루로 설정해줘야 트리거가 입력됬을 시
            //갭 번호가 증가 하지 않는다. 처음만 증가하지
            //않도록 하면되는 플레그이다.
            ThreadFirstFlag = true;
            //inspection_Gap.GetSet_NowModel_Gap_Data = Now_Model_Gap_Data_Array;
            Run_Mode = "Auto";
            //Data_Mixing_Make_Uper();
            //Data_Mixing_Make_Down();
            //현재(사용자가 검사 시작버튼 클릭) 시각을 기준으로
            //이에 해당하는 날자의 엑셀 파일이 있는지 검사해서
            //없다면 파일을 생성한다.
            //Inspect_Set_FileName_ExcelFile(DateTime.Now);
            DateTime ExcelMakeTime = DateTime.Now;
            Excel_Uper_Filename = Inspect_Set_FileName_ExcelFile_Uper(ExcelMakeTime);
            Excel_Down_Filename = Inspect_Set_FileName_ExcelFile_Down(ExcelMakeTime);
            /*
            int iNumber = 0;
            bool result = int.TryParse(uTxt_Gap_No.Text.Trim(), out iNumber);
            if (result) SetReg(GapSystem.RegPathGapStatus, "NowGapNo", iNumber.ToString());

            //이값을 트루로 설정해줘야 트리거가 입력됬을 시
            //갭 번호가 증가 하지 않는다. 처음만 증가하지
            //않도록 하면되는 플레그이다.
            inspection_Gap.GetSet_ThreadFirstFlag = true;
            inspection_Gap.GetSet_NowModel_Gap_Data = Now_Model_Gap_Data_Array;
            inspection_Gap.GetSet_RunMode = "Auto";
            inspection_Gap.GetSet_HistoryAll = uPanel_History_All_Uper;
            inspection_Gap.GetSet_HistoryNG = uPanel_History_NG;
            inspection_Gap.Gap_Data_Mixing_Make();
            
            //현재(사용자가 검사 시작버튼 클릭) 시각을 기준으로
            //이에 해당하는 날자의 엑셀 파일이 있는지 검사해서
            //없다면 파일을 생성한다.
            Inspect_Set_FileName_ExcelFile(DateTime.Now);
            */
        }

        

        


        //2014.05.30.01 WKB
        //엑셀 파일에 저장하는 것이 프로세스 분석 결과 가장 많은 시간을 차지하는 것으로 나타나
        //이를 CSV파일로 변경한다.
        private string Inspect_Set_FileName_ExcelFile(DateTime checkTime)
        {
            string folderName = _NowExcelFolderSavePath_Uper + "\\Gap" +
                               String.Format("\\{0:00}년{1:00}월", checkTime.Year, checkTime.Month);
            string fileName = folderName +
                              String.Format("\\{0:00}년{1:00}월{2:00}일 Gap Vision.csv", checkTime.Year, checkTime.Month,
                                  checkTime.Day);

            excelFile.Excel_Folder_Check_Or_Make(folderName);
            excelFile.Excel_File_Check_Or_Make_Gap(fileName);

            return fileName;
        }

        //레지스트리의 데이터 값을 읽와서 리턴한다.
        //1번 파람 : 레지스트리의 경로
        //2번 파람 : 레지스트리의 이름
        //리턴 파람: 레지스트리에서 읽은 값
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


        //1번 파라미터의 경로 아래에 있는  
        //2번 파라미터와 같은 이름의 레지스터 항목을
        //3번 파라미터 값으로 저장한다.
        public void SetReg(string strNodePath, string strName, string strData)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(strNodePath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(strName, strData, RegistryValueKind.String);
            reg.Close();
        }


        private void Inspect_Run_Ready_Test_ImageFolder_Load_BiCell(string folderPath)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var di = new DirectoryInfo(folderPath);
            foreach (FileInfo f in di.GetFiles())
            {
                _strLstTestImageNames_Down.Add(f.FullName);
            }
        }

        private void Inspect_Run_Ready_Test_ImageFolder_Load_Uper(string folderPath)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var di = new DirectoryInfo(folderPath);
            foreach (FileInfo f in di.GetFiles())
            {
                _strLstTestImageNames_Uper.Add(f.FullName);
            }
        }


        private void Inspect_Run_Ready_Test_ImageFolder_Load_Down(string folderPath)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            var di = new DirectoryInfo(folderPath);
            foreach (FileInfo f in di.GetFiles())
            {
                _strLstTestImageNames_Down.Add(f.FullName);
            }
        }
    

        private void Inspect_Run_Componet_set()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_uButton_TestRunStop02.Appearance.Image = Resources.stop2;
            Inspect_uButton_TestRunStop02.Text = "정 지";
            Inspect_uButton_TestRunStop02.Focus();

            Inspect_uButton_TestRunStop03.Enabled = false;
            //Inspect_uTab_ImageList22.Enabled = false;

            string tmp1 = LamiSystem.StrListSysConTitle[10];

//             _iLineParam1 = int.Parse(textBox4.Text);
//             _iEdgeParam1 = int.Parse(textBox5.Text);
//             _iEdgeParam2 = int.Parse(textBox6.Text);
//             Threshold_01[0] = int.Parse(textBox5.Text);
//             Threshold_01[1] = int.Parse(textBox6.Text);
        }

        private void Inspect_Stop_View()
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            Inspect_uButton_TestRunStop02.Appearance.Image = Resources.start;
            Inspect_uButton_TestRunStop02.Text = "시 작";

            Inspect_uButton_TestRunStop03.Enabled = true;
            Inspect_uTab_ImageList22.Enabled = true;

            //시퀀스 타임아웃을 체크하는 쓰레드 루프의 조건 플래그
            //false이면 루프를 나오면서 Timeout_Sequence_Thread.Abort()실행한다.
            Timeout_Seq_Loop = false;
        }

        //기준 위치 설정
        private void Inspect_uButton_Assy08_Click(object sender, EventArgs e)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);

            //if (Inspect_uButton_TestRunStop02.Text == "정 지") return;

            //TestImageLoad();
            Inspect_uButton_TestRunStop02.PerformClick();
        }


        private void Inspect_uButton_Assy09_Click(object sender, EventArgs e)
        {
            //HistoryTable_Initialize(Down_HistoryTables_All);
            //float[] testarray = new float[] { 4.0154f, 4.0234f, 4.0164f};
            //float testresult = (float)Control_CPKData.StDev(testarray);
            //float testcpk = (float) Control_CPKData.Cp(3.9f, 2.1f, testresult);
            //UMAC_Data_Communication();
            //Inspect_Run_Run_Grab_Running_Uper_Test01();
            //Measure_Grid_Making_Uper();
            //Measure_Grid_Making_Down();
            //return;
            
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;

            Inspect_Count_RegData_Reset();

            Measure_Grid_Making_Uper();
            Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);

            Measure_Grid_Making_Down();
            Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Down, uGrd_Inspect_Measure_Down);
            
            Chart_Making_Uper();
            Chart_Making_Down();

            inspect_Run_Run_Display_NG_OK_Count();

            return;
        }

        private void Inspect_Count_RegData_Reset()
        {
            SetReg(LamiSystem.RegPathGapStatus, "Count_Product", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Both", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Uper", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Down", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Uper", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_OK_Down", "0");

            NowProdectNumber = 0;
            NowFailNumber_Both = 0;
            NowFailNumber_Uper = 0;
            NowFailNumber_Down = 0;
            NowPassNumber_Uper = 0;
            NowPassNumber_Down = 0;
            //WKB 20150725
            
            //RegistryKey reg = Registry.CurrentUser;
            //reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Uper, true);
            int regCount = 0;
            int mesColCount = 19;
            //if (reg != null)
            //{
                //regCount = reg.ValueCount;
                //int Grid_Rows = regCount / mesColCount;
                for (int i = 0; i < 10; i++)
                {
                    //평균
                    this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 8).ToString("000"), "0.000");
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
            //}

            //reg = reg.OpenSubKey(LamiSystem.RegPathMeasure_Down, true);
            //if (reg != null)
            //{
                //regCount = reg.ValueCount;
                //int Grid_Rows = regCount / mesColCount;
                for (int i = 0; i < 10; i++)
                {
                    //평균
                    this.SetReg(LamiSystem.RegPathMeasure_Down, (i * mesColCount + 8).ToString("000"), "0.000");
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
            //}
            
        }


        private void Inspect_uButton_Manual14_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor; //  커서를 모래시계로 만들었다가..

            Thread.Sleep(10);
            //Inspect_uButton_Manual20.PerformClick();

            Inspect_Run_Run_Manual_Grab_Gap();
            Inspect_Main01_IplBox.ImageIpl = SrcIplImage_Uper;
            //Loading_Image_Name.Visible = false;

            this.Cursor = System.Windows.Forms.Cursors.Default;  // 커서를 원래 모양으로 만들었습니다.
        }

        public void Inspect_Run_Run_Manual_Grab_Gap()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Manual_Grab_Gap del = Inspect_Run_Run_Manual_Grab_Gap;
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
                MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, MIL.M_DEFAULT, MIL.M_DEFAULT, ref MilSystem_Uper); // 프레임그레버 할당
                MIL.MdigAlloc(MilSystem_Uper, MIL.M_DEV0, @"C:\Visionsystem\Data\solxcl_mil9_G60FV11CL_c_8bit_2tap_P2.dcf",
                    MIL.M_DEFAULT, ref MilDigitizer_Uper);
                MIL.MdispAlloc(MilSystem_Uper, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Uper);
                MIL.MbufAlloc2d(MilSystem_Uper, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP,
                    ref MilImage_Uper);
                MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기
                MIL.MdigGrab(MilDigitizer_Uper, MilImage_Uper);

                MIL.MbufGet2d(MilImage_Uper, 0, 0, 4096, 3072, imgBuf_Uper);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Uper, 0);
                CVMatImg_Uper.Data = bufPtr;
                monoIplImage_Uper = Cv.GetImage(CVMatImg_Uper);
                Cv.CvtColor(monoIplImage_Uper, SrcIplImage_Uper, ColorConversion.GrayToBgr);

                MIL.MdigControl(MilDigitizer_Uper, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MappFreeDefault(MilApplication, MilSystem_Uper, MilDisplay_Uper, MilDigitizer_Uper, MilImage_Uper);
            }
        }


        public void Inspect_Run_Run_Manual_Grab_Down()
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Manual_Grab_Down del = Inspect_Run_Run_Manual_Grab_Down;
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
                MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, 1, MIL.M_DEFAULT, ref MilSystem_Uper); // 프레임그레버 할당
                MIL.MdigAlloc(MilSystem_Uper, MIL.M_DEV1, @"C:\Visionsystem\Data\solxcl_mil9_G60FV11CL_c_8bit_2tap_P2.dcf",MIL.M_DEFAULT, ref MilDigitizer_Down);
                MIL.MdispAlloc(MilSystem_Uper, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay_Down);
                MIL.MbufAlloc2d(MilSystem_Uper, 4096, 3072, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP,ref MilImage_Down);
                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE); // 트리거 타임아웃 무한대기
                MIL.MdigGrab(MilDigitizer_Down, MilImage_Down);

                MIL.MbufGet2d(MilImage_Down, 0, 0, 4096, 3072, imgBuf_Down);
                IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(imgBuf_Down, 0);
                CVMatImg_Down.Data = bufPtr;
                monoIplImage_Down = Cv.GetImage(CVMatImg_Down);
                Cv.CvtColor(monoIplImage_Down, SrcIplImage_Down, ColorConversion.GrayToBgr);

                MIL.MdigControl(MilDigitizer_Down, MIL.M_GRAB_ABORT, MIL.M_DEFAULT);
                MIL.MappFreeDefault(MilApplication, MilSystem_Uper, MilDisplay_Down, MilDigitizer_Down, MilImage_Down);
            }
        }

        //하부 이미지 버튼
        private void Inspect_uButton_Manual15_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.Enabled = true;
            Inspect_uButton_Manual15.Enabled = false;
            Inspect_uButton_Manual20.Enabled = true;
            Inspect_uButton_Manual21.Enabled = true;

            Inspect_Main01_IplBox.Visible = false;
            Inspect_Main02_IplBox.Visible = true;
            uPanel_Uper.Visible = false;
            uPanel_Down.Visible = false;

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private DataTable Uper_HistoryTables_NG;
        private DataTable Uper_HistoryTables_All;
        private DataTable Down_HistoryTables_NG;
        private DataTable Down_HistoryTables_All;

        private void HistoryTable_Initialize(DataTable doDataTable)
        {
//             for (int i = 0; i < UperChart_Count; i++)
//             {
//                 ultraChartSetup(this.UperData_Charts[i]);
// 
//                
//                 this.UperData_Charts[i].DataSource = initializeDataTable(Uper_MeasureTables[i]);
// 
//                 this.UperData_Charts[i].Location = Uper_chartPoints[i];
//                 this.UperData_Charts[i].Visible = true;
// 
//                 float CenValue = float.Parse(Chart_Title_No_Uper[i][2]);
//                 float MaxValue = float.Parse(Chart_Title_No_Uper[i][2]) + float.Parse(Chart_Title_No_Uper[i][3]) * 2;
//                 float MinValue = float.Parse(Chart_Title_No_Uper[i][2]) - float.Parse(Chart_Title_No_Uper[i][4]) * 2;
//                 this.UperData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
//                 this.UperData_Charts[i].Axis.Y.RangeMax = MaxValue;
//                 this.UperData_Charts[i].Axis.Y.RangeMin = MinValue;
//             }

//             if (Row_Count == 0)
//             {
//                 Down_MeasureTables[Graph_Count].Columns.RemoveAt(0);
// 
//                 var measureColumn = new DataColumn();
//                 measureColumn.DataType = Type.GetType("System.Double");
//                 measureColumn.AllowDBNull = false;
//                 measureColumn.DefaultValue = 0d;
//                 Down_MeasureTables[Graph_Count].Columns.Add(measureColumn);
//             }
//             Down_MeasureTables[Graph_Count].Rows[Row_Count][99] = MeaValue;

            doDataTable = new DataTable();
            doDataTable.Columns.Clear();
            doDataTable.Rows.Clear();

            for (int i = 0; i < 20; i++)
            {
                var TrigColumn = new DataColumn();
                TrigColumn.DataType = Type.GetType("System.String");
                TrigColumn.AllowDBNull = false;
                TrigColumn.ColumnName = "TrigNum";
                TrigColumn.DefaultValue = 0;
                doDataTable.Columns.Add(TrigColumn);

                for (int j = 0; j < uGrd_Inspect_Measure_Uper.Rows.Count; j++)
                {
                    if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[j].Cells[1].Value.ToString() == "") return;
                    var measureColumn = new DataColumn();
                    measureColumn.DataType = Type.GetType("System.Double");
                    measureColumn.AllowDBNull = false;
                    measureColumn.ColumnName = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[j].Cells[1].Value.ToString();
                    measureColumn.DefaultValue = 0;
                    doDataTable.Columns.Add(measureColumn);
                }
            }

           
        }

        
       

        private void Inspect_Manual_Image_Grabing()
        {
            _dtInspDataSaveTime = DateTime.Now;
            GetSet_Used_Form = "Inspect";
            GetSet_Display_PictureBox = Inspect_Main01_IplBox;

            //SrcIplImageGap
            ///rcImgGray = new IplImage(Inspect_Main01_IplBox.ImageIpl.Size, BitDepth.U8, 1);
            ///v.CvtColor(Inspect_Main01_IplBox.ImageIpl, srcImgGray, ColorConversion.BgrToGray);
            ///etSet_NowIplImage = srcImgGray;
            //GetSet_NowIplImage = Inspect_Main01_IplBox.ImageIpl;
            
            GetSet_Draw_GC_Uper = Inspect_Main01_IplBox.CreateGraphics();

            Inspect_Get_ModelData_From_UMAC();

            //Data_Mixing_Make_Uper();
            //Data_Mixing_Make_Down();

            Inspect_Initionalize();

            //로딩한 이미지의 ROI 처리를 진행한다.
            bool Serarch_Result = Inspect_Run_Run_ROI_EdgeLine_Centering_Uper(Inspect_Main01_IplBox.ImageIpl);

            if (Serarch_Result == false) Inspect_Run_Run_None_Point_Search_Display();

            Inspect_Run_Run_ROI_CenterPoint_Find_Uper();

            Inspect_Run_Run_FindData_Inspection_Uper();

            Inspect_Save_Data_Copy_Uper();

            //검사 결과를 화면에 표시해 준다.
            Inspect_Run_Run_Inspect_Result_Display_Uper();

            _CycleCompleteFlag_Uper = true;
        }

        /*
        private void Inspect_Manual_Image_Grabing()
        {
            _dtInspDataSaveTime = DateTime.Now;
            
            //로딩한 이미지의 ROI 처리를 진행한다.
            //로딩한 이미지의 ROI 처리를 진행한다.
            bool Serarch_Result = Inspect_Run_Run_ROI_EdgeLine_Centering(false);

            if (Serarch_Result == false) Inspect_Run_Run_Inspect_None_Display();
            Inspect_Run_Run_Drawing_Result();

            _CycleCompleteFlag_Gap = true;
        }
        */


        //갭 수동 테스트 버튼 인스펙션의 버튼
        private void Inspect_uButton_Manual16_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;

            if (Inspect_Main01_IplBox.ImageIpl == null) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor; //  커서를 모래시계로 만들었다가..

            Thread.Sleep(10);
            //Inspect_uButton_Manual20.PerformClick();

            Graphics gc = Inspect_Main01_IplBox.CreateGraphics();

            int iManual_GapNo = -1;
            bool result = int.TryParse(uTxt_Gap_No.Text, out iManual_GapNo);
            if (result == true)
                NowGapNumber = iManual_GapNo;

            Run_Mode = "Manual";
            
            SrcIplImage_Uper = Inspect_Main01_IplBox.ImageIpl;
            Inspect_Manual_Image_Grabing();

            this.Cursor = System.Windows.Forms.Cursors.Default; 
        }

        private void Inspect_uButton_Manual17_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor; //  커서를 모래시계로 만들었다가..
            
            //Inspect_uButton_Manual21.PerformClick();
            Thread.Sleep(10);
            Inspect_Run_Run_Manual_Grab_Down();
            Inspect_Main02_IplBox.ImageIpl = SrcIplImage_Down;
            //Loading_Image_Name.Visible = false;

            this.Cursor = System.Windows.Forms.Cursors.Default;  // 커서를 원래 모양으로 만들었습니다.
        }



        private void Inspect_Run_Run_Threading2_Display(TextBox tBox, int dataNo)
        {
            if (InvokeRequired)
            {
                Delegate_Run_Run_Threading2_Display del =
                    Inspect_Run_Run_Threading2_Display;
                Invoke(del, tBox, dataNo);
            }
            else
            {
                tBox.Text = dataNo.ToString("0");
                tBox.Refresh();
            }
        }

        //상부 이미지 버튼
        private void Inspect_uButton_Manual18_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            _iReviewFlag_Uper = 1;

            Inspect_uButton_Manual18.Enabled = false;
            Inspect_uButton_Manual15.Enabled = true;
            Inspect_uButton_Manual20.Enabled = true;
            Inspect_uButton_Manual21.Enabled = true;

            Inspect_Main01_IplBox.Visible = true;
            Inspect_Main02_IplBox.Visible = false;
            uPanel_Uper.Visible = false;
            uPanel_Down.Visible = false;

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        /*
         private void Inspect_uButton_Manual18_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            //if (Inspect_uLabel_MainTitle01.Text == "BICELL VISION") return;

            Thread.Sleep(10);
            Inspect_uButton_Manual20.PerformClick();

            var pFileDlg = new OpenFileDialog
            {
                DefaultExt = "*.jpg;*.png;*.gif;*.tga",
                Filter = @"이미지 파일 (*.jpg, *.png, *.gif, *.tga) |*.jpg;*.png;*.gif;*.tga|모든 파일(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Image File Open"
            };
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strFullPathFile = pFileDlg.FileName;
                //Bitmap bitImage = new Bitmap(strFullPathFile);
                //IplImage bitmapImage = IplImage.FromBitmap(bitImage);
                //Inspect_Main01_IplBox.ImageIpl = IplImage.FromBitmap(bitImage);
                Inspect_Main01_IplBox.ImageIpl = IplImage.FromFile(strFullPathFile);
                
                //IplImage srcOrigin = new IplImage([파일이름], [로드모드]); //+초기화
                //CvMat srcMat;
                //초기화
                //srcMat = Cv.GetMat(srcOrigin);
                //[예시2]
                //선언
                //CvMat srcOrigin = new CvMat([파일이름], [로드모드]); //초기화
                //IplImage srcImg;
                //초기화
                //srcImg = Cv.GetImage(srcOrigin);
                //CvMat srcMat = Cv.GetMat(IplImage.FromFile(strFullPathFile));
                
                Loading_Image_Name.Text = strFullPathFile;
                Loading_Image_Name.Visible = true;
            }
        }
        */

        private void Inspect_uButton_Manual19_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            
            if (Inspect_Main02_IplBox.ImageIpl == null) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            Thread.Sleep(10);
            //Inspect_uButton_Manual21.PerformClick();
            Graphics gc = Inspect_Main02_IplBox.CreateGraphics();

            //var inspection_BiCell = new Control_Inspect_BiCell(GapSystem, _posConverter_BiCell, fileSystem, gc,Inspect_Main02_IplBox, Inspect_Main02_IplBox.ImageIpl, plc, umac);
            //var inspection_BiCell = new Control_Inspect_BiCell();
            //inspection_BiCell.GetSet_GapSystem = GapSystem;
            //inspection_BiCell.GetSet_Calling_Form = this.Name;
            //inspection_BiCell.GetSet_NowIplImage = Inspect_Main02_IplBox.ImageIpl;
            //inspection_BiCell.Inspect_Manual_Image_Grabing();

            //List<string> resultArray = inspection_BiCell.GridDisplayData;
            //Manual_Inspect_ResultData_To_Viewer_Grid_BiCell(resultArray);

            _CycleCompleteFlag_Down = true;

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

       
        //상부 그래프 버튼 클릭
        private void Inspect_uButton_Manual20_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.Enabled = true;
            Inspect_uButton_Manual15.Enabled = true;
            Inspect_uButton_Manual20.Enabled = false;
            Inspect_uButton_Manual21.Enabled = true;

            Test_Graph.Enabled = true;

            Inspect_Main01_IplBox.Visible = false;
            Inspect_Main02_IplBox.Visible = false;
            uPanel_Uper.Visible = true;
            uPanel_Down.Visible = false;

            
            //Chart_Making_Uper();

            //Chart_DataWrite_Uper();
            
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        /*
        
        public void Measure_Grid_Making_Uper()
        {
            //chart_Array_Uper.Clear();
            int VisionColumn = 8;
            int RecipeColumn = 11;
            int RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / RecipeColumn;

            uDS_Inspect_Measure_Uper.Rows.Clear();

            for (int i = 0; i < RcpGrdRows; i++)
            {
                //string readNo = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn + 1];
                string readName = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn];

                int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn];
                    if (readName == VisName)
                    {
                        UltraGridRow row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                        string CenValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 1];
                        string MaxValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 2];
                        string MinValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 3];
                        row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                        row.Cells["검사 항목"].Value = readName;
                        row.Cells["규격 중심"].Value = CenValue;
                        row.Cells["규격 상한"].Value = (double.Parse(CenValue) + double.Parse(MaxValue)).ToString("0.00");
                        row.Cells["규격 하한"].Value = (double.Parse(CenValue) - double.Parse(MinValue)).ToString("0.00"); 
                        row.Cells["측정 값"].Value = "0.00";
                        row.Cells["판정 결과"].Value = "NO";
                        row.Cells["수율"].Value = "0.000";
                        row.Cells["평균"].Value = "0.000";
                        row.Cells["표준 편차"].Value = "0.000";
                        row.Cells["최소 값"].Value = "0.000";
                        row.Cells["최대 값"].Value = "0.000";
                        row.Cells["CP 값"].Value = "0.000";
                        row.Cells["CPK 값"].Value = "0.000";
                        row.Cells["OkCount"].Value = "0";
                        row.Cells["NgCount"].Value = "0";
                        row.Cells["SumValue"].Value = "0";
                        row.Cells["SquValue"].Value = "0";
                        row.Cells["ProductOK"].Value = "0";
                        break;
                    }
                }
            }

            //그리드 공란을 채운다.
            for (int i = 0; i < 10-RcpGrdRows; i++)
            {
                UltraGridRow row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
            }
            
            int mesColCount = 19;

            for (int i = 0; i < 0;i++)//this.uGrd_Inspect_Measure_Uper.Rows.Count; i++)
            {
                //StructData.strOkCount = this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"));
                //StructData.nowOkCount = int.Parse(StructData.strOkCount);

                //StructData.strNgCount = this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"));
                //StructData.nowNgCount = int.Parse(StructData.strNgCount);
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
            

            Measurement_Grid_Resize(uGrd_Inspect_Measure_Uper);
            //Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);
        }

        */


        public void Measure_Grid_Making_Uper()
        {
            return;
            //chart_Array_Uper.Clear();
            int VisionColumn = 8;
            int RecipeColumn = 11;
            int RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / RecipeColumn;

            uDS_Inspect_Measure_Uper.Rows.Clear();

            for (int i = 0; i < RcpGrdRows; i++)
            {
                //string readNo = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn + 1];
                string readName = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn];

                int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn];
                    if (readName == VisName)
                    {
                        UltraGridRow row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                        string CenValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 1];
                        string MaxValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 2];
                        string MinValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 3];
                        row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
                        row.Cells["검사 항목"].Value = readName;
                        row.Cells["규격 중심"].Value = CenValue;
                        row.Cells["규격 상한"].Value = (double.Parse(CenValue) + double.Parse(MaxValue)).ToString("0.00");
                        row.Cells["규격 하한"].Value = (double.Parse(CenValue) - double.Parse(MinValue)).ToString("0.00"); 
                        row.Cells["측정 값"].Value = "0.00";
                        row.Cells["판정 결과"].Value = "NO";
                        row.Cells["수율"].Value = "0.000";
                        row.Cells["평균"].Value = "0.000";
                        row.Cells["표준 편차"].Value = "0.000";
                        row.Cells["최소 값"].Value = "0.000";
                        row.Cells["최대 값"].Value = "0.000";
                        row.Cells["CP 값"].Value = "0.000";
                        row.Cells["CPK 값"].Value = "0.000";
                        row.Cells["OkCount"].Value = "0";
                        row.Cells["NgCount"].Value = "0000";
                        row.Cells["SumValue"].Value = "0";
                        row.Cells["SquValue"].Value = "0";
                        row.Cells["ProductOK"].Value = "0";
                        break;
                    }
                }
            }

            //그리드 공란을 채운다.
            for (int i = 0; i < 10-RcpGrdRows; i++)
            {
                UltraGridRow row = this.uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].AddNew();

                row.Cells["NO"].Value = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count.ToString("0");
            }
            
//             int mesColCount = 19;
// 
//             for (int i = 0; i < 0;i++)//this.uGrd_Inspect_Measure_Uper.Rows.Count; i++)
//             {
//                 //StructData.strOkCount = this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"));
//                 //StructData.nowOkCount = int.Parse(StructData.strOkCount);
// 
//                 //StructData.strNgCount = this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"));
//                 //StructData.nowNgCount = int.Parse(StructData.strNgCount);
//                 //표준편차
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 9).ToString("000"), "0.000");
// 
//                 //최소
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 10).ToString("000"), "0.000");
// 
//                 //최대
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 11).ToString("000"), "0.000");
// 
//                 //CP
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 12).ToString("000"), "0.000");
// 
//                 //CPK
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 13).ToString("000"), "0.000");
//                 //OK count
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 14).ToString("000"), "0");
//                 //NG Count
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 15).ToString("000"), "0");
//                 //Value
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 16).ToString("000"), "0");
//                 //SqValue
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 17).ToString("000"), "0");
//                 //ProductOK
//                 this.SetReg(LamiSystem.RegPathMeasure_Uper, (i * mesColCount + 18).ToString("000"), "0");
// 
//             }
//             

            Measurement_Grid_Resize(uGrd_Inspect_Measure_Uper);
            //Measurement_Grid_To_Register(LamiSystem.RegPathMeasure_Uper, uGrd_Inspect_Measure_Uper);
        }


        public void Measure_Grid_Making_Down()
        {
            return;
            //chart_Array_Uper.Clear();
            int VisionColumn = 8;
            int RecipeColumn = 11;
            int RcpGrdRows = LamiSystem.StrLstRcpConGridData_Down.Count / RecipeColumn;

            uDS_Inspect_Measure_Down.Rows.Clear();

            for (int i = 0; i < RcpGrdRows; i++)
            {
                string readName = LamiSystem.StrLstRcpConGridData_Down[i * RecipeColumn];

                int VisGrdRows = LamiSystem.StrListVisConGridData_Down.Count / VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Down[j * VisionColumn];
                    if (readName == VisName)
                    {
                        UltraGridRow row = this.uGrd_Inspect_Measure_Down.DisplayLayout.Bands[0].AddNew();

                        string CenValue = LamiSystem.StrListVisConGridData_Down[j * VisionColumn + 1];
                        string MaxValue = LamiSystem.StrListVisConGridData_Down[j * VisionColumn + 2];
                        string MinValue = LamiSystem.StrListVisConGridData_Down[j * VisionColumn + 3];

                        row.Cells["NO"].Value = uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0");
                        row.Cells["검사 항목"].Value = readName;
                        row.Cells["규격 중심"].Value = CenValue;
                        row.Cells["규격 상한"].Value = (double.Parse(CenValue) + double.Parse(MaxValue)).ToString("0.00");
                        row.Cells["규격 하한"].Value = (double.Parse(CenValue) - double.Parse(MinValue)).ToString("0.00");
                        row.Cells["측정 값"].Value = "0.00";
                        row.Cells["판정 결과"].Value = "NO";
                        row.Cells["수율"].Value = "0.000";
                        row.Cells["평균"].Value = "0.000";
                        row.Cells["표준 편차"].Value = "0.000";
                        row.Cells["최소 값"].Value = "0.000";
                        row.Cells["최대 값"].Value = "0.000";
                        row.Cells["CP 값"].Value = "0.000";
                        row.Cells["CPK 값"].Value = "0.000";
                        row.Cells["OkCount"].Value = "0";
                        row.Cells["NgCount"].Value = "0000";
                        row.Cells["SumValue"].Value = "0";
                        row.Cells["SquValue"].Value = "0";
                        row.Cells["ProductOK"].Value = "0";
                        break;
                    }
                }
            }

            for (int i = 0; i < 10 - RcpGrdRows; i++)
            {
                UltraGridRow row = this.uGrd_Inspect_Measure_Down.DisplayLayout.Bands[0].AddNew();

                row.Cells["NO"].Value = uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count.ToString("0");
            }

            int mesColCount = 19;

            for (int i = 0; i < 0; i++)//this.uGrd_Inspect_Measure_Uper.Rows.Count; i++)
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
                //OK Count
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

            Measurement_Grid_Resize(uGrd_Inspect_Measure_Down);
        }

        public void Measurement_Grid_Resize(Infragistics.Win.UltraWinGrid.UltraGrid doGrid)
        {
           if (doGrid.Rows.Count < 11)
            {
//                 if(doGrid.DisplayLayout.Bands[0].Columns[0].Width == 37) return;
//                 doGrid.DisplayLayout.Bands[0].Columns[0].Width = 37;
//                 doGrid.DisplayLayout.Bands[0].Columns[1].Width = 58;
//                 doGrid.DisplayLayout.Bands[0].Columns[2].Width = 45;
//                 doGrid.DisplayLayout.Bands[0].Columns[3].Width = 45;
//                 doGrid.DisplayLayout.Bands[0].Columns[4].Width = 45;
//                 doGrid.DisplayLayout.Bands[0].Columns[5].Width = 45;
//                 doGrid.DisplayLayout.Bands[0].Columns[6].Width = 36;
//                 doGrid.DisplayLayout.Bands[0].Columns[7].Width = 45;
//                 doGrid.DisplayLayout.Bands[0].Columns[8].Width = 44;
//                 doGrid.DisplayLayout.Bands[0].Columns[9].Width = 44;
//                 doGrid.DisplayLayout.Bands[0].Columns[10].Width = 44;
//                 doGrid.DisplayLayout.Bands[0].Columns[11].Width = 44;
//                 doGrid.DisplayLayout.Bands[0].Columns[12].Width = 44;
//                 doGrid.DisplayLayout.Bands[0].Columns[13].Width = 44;

                //if (doGrid.DisplayLayout.Bands[0].Columns[0].Width == 30) return;
                doGrid.DisplayLayout.Bands[0].Columns[0].Width = 30;//37
                doGrid.DisplayLayout.Bands[0].Columns[1].Width = 49;//58
                doGrid.DisplayLayout.Bands[0].Columns[2].Width = 43;//45
                doGrid.DisplayLayout.Bands[0].Columns[3].Width = 43;//45
                doGrid.DisplayLayout.Bands[0].Columns[4].Width = 43;//45
                doGrid.DisplayLayout.Bands[0].Columns[5].Width = 43;//45
                doGrid.DisplayLayout.Bands[0].Columns[6].Width = 30;//36
                doGrid.DisplayLayout.Bands[0].Columns[7].Width = 43;//45
                doGrid.DisplayLayout.Bands[0].Columns[8].Width = 43;//44
                doGrid.DisplayLayout.Bands[0].Columns[9].Width = 43;//44
                doGrid.DisplayLayout.Bands[0].Columns[10].Width = 43;//44
                doGrid.DisplayLayout.Bands[0].Columns[11].Width = 43;//44
                doGrid.DisplayLayout.Bands[0].Columns[12].Width = 43;//44
                doGrid.DisplayLayout.Bands[0].Columns[13].Width = 43;//44
                doGrid.DisplayLayout.Bands[0].Columns[14].Width = 38;//44
            }
            else
            {
                if (doGrid.DisplayLayout.Bands[0].Columns[3].Width == 41) return;
                doGrid.DisplayLayout.Bands[0].Columns[0].Width = 30;
                doGrid.DisplayLayout.Bands[0].Columns[1].Width = 49;
                doGrid.DisplayLayout.Bands[0].Columns[2].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[3].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[4].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[5].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[6].Width = 30;
                doGrid.DisplayLayout.Bands[0].Columns[7].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[8].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[9].Width = 41;
                doGrid.DisplayLayout.Bands[0].Columns[10].Width = 42;
                doGrid.DisplayLayout.Bands[0].Columns[11].Width = 42;
                doGrid.DisplayLayout.Bands[0].Columns[12].Width = 42;
                doGrid.DisplayLayout.Bands[0].Columns[13].Width = 42;
                doGrid.DisplayLayout.Bands[0].Columns[14].Width = 39;
                if (doGrid.Rows.Count > 0) doGrid.ActiveRow = doGrid.Rows[0];
            }


        }


        List<string> Chart_No_Array_Uper_01 = new List<string>();
        List<string> Chart_No_Array_Uper_02 = new List<string>();
        List<string> Chart_No_Array_Uper_03 = new List<string>();
        List<string> Chart_No_Array_Uper_04 = new List<string>();
        List<string> Chart_No_Array_Uper_05 = new List<string>();
        List<string> Chart_No_Array_Uper_06 = new List<string>();
        List<string> Chart_No_Array_Uper_07 = new List<string>();
        List<string> Chart_No_Array_Uper_08 = new List<string>();
        List<string> Chart_No_Array_Uper_09 = new List<string>();
        List<string> Chart_No_Array_Uper_10 = new List<string>(); 
        List<string>[] Chart_Title_No_Uper = new List<string>[10];

        public void Chart_Uper_Title_Init()
        {
            for (int i = 0; i < Chart_Title_No_Uper.Count(); i++)
            {
                Chart_Title_No_Uper[i].Clear();
            }
        }
        public void Chart_Making_Uper()
        {
            Chart_Uper_Title_Init();
            chart_Array_Uper.Clear();
            int VisionColumn = 8;
            int RecipeColumn = 11;
            int RcpGrdRows = LamiSystem.StrLstRcpConGridData_Uper.Count / RecipeColumn;

            for (int i = 0; i < RcpGrdRows; i++)
            {
                string readNo = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn + 1];
                string readName = LamiSystem.StrLstRcpConGridData_Uper[i * RecipeColumn];
                Chart_Make_Array_Uper(readNo);
                Chart_Title_No_Uper[int.Parse(readNo) - 1].Add(i.ToString());
                Chart_Title_No_Uper[int.Parse(readNo) - 1].Add(readName);

                int VisGrdRows = LamiSystem.StrListVisConGridData_Uper.Count / VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn];
                    if (readName == VisName)
                    {
                        string CenValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 1];
                        string MaxValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 2];
                        string MinValue = LamiSystem.StrListVisConGridData_Uper[j * VisionColumn + 3];
                        Chart_Title_No_Uper[int.Parse(readNo) - 1].Add(CenValue);
                        Chart_Title_No_Uper[int.Parse(readNo) - 1].Add(MaxValue);
                        Chart_Title_No_Uper[int.Parse(readNo) - 1].Add(MinValue);
                        break;
                    }
                }
            }

            //UperChart_Count = chart_Array_Uper.Count;
            UperChart_Count = 10;

            Uper_chartPoints = chart_Locattion_Make_Uper(UperChart_Count);
            Chart_Initialize_Uper();
        }

        public void Chart_Making_Down()
        {
            Chart_Down_Title_Init();
            chart_Array_Down.Clear();

            int VisionColumn = 8;
            int RecipeColumn = 11;
            int RcpGrdRows = LamiSystem.StrLstRcpConGridData_Down.Count / RecipeColumn;

            for (int i = 0; i < RcpGrdRows; i++)
            {
                string readNo = LamiSystem.StrLstRcpConGridData_Down[i * RecipeColumn + 1];
                string readName = LamiSystem.StrLstRcpConGridData_Down[i * RecipeColumn];
                Chart_Make_Array_Down(readNo);
                Chart_Title_No_Down[int.Parse(readNo) - 1].Add(i.ToString());
                Chart_Title_No_Down[int.Parse(readNo) - 1].Add(readName);

                int VisGrdRows = LamiSystem.StrListVisConGridData_Down.Count / VisionColumn;
                for (int j = 0; j < VisGrdRows; j++)
                {
                    string VisName = LamiSystem.StrListVisConGridData_Down[j * VisionColumn];
                    if (readName == VisName)
                    {
                        string CenValue = LamiSystem.StrListVisConGridData_Down[j * VisionColumn + 1];
                        string MaxValue = LamiSystem.StrListVisConGridData_Down[j * VisionColumn + 2];
                        string MinValue = LamiSystem.StrListVisConGridData_Down[j * VisionColumn + 3];
                        Chart_Title_No_Down[int.Parse(readNo) - 1].Add(CenValue);
                        Chart_Title_No_Down[int.Parse(readNo) - 1].Add(MaxValue);
                        Chart_Title_No_Down[int.Parse(readNo) - 1].Add(MinValue);
                        break;
                    }
                }
            }

            //DownChart_Count = chart_Array_Down.Count;
            DownChart_Count = 10;
            Down_chartPoints = chart_Locattion_Make_Down(DownChart_Count);
            Chart_Initialize_Down();
        }

        List<string> chart_Array_Uper = new List<string>(); 
        public void Chart_Make_Array_Uper(string chartNo)
        {
            
            foreach (var ReadNo in chart_Array_Uper)
            {
                if (chartNo == ReadNo) return;
            }
            chart_Array_Uper.Add(chartNo);
        }

        //하부 그래프 버튼 클릭
        private void Inspect_uButton_Manual21_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.Enabled = true;
            Inspect_uButton_Manual15.Enabled = true;
            Inspect_uButton_Manual20.Enabled = true;
            Inspect_uButton_Manual21.Enabled = false;

            Inspect_Main01_IplBox.Visible = false;
            Inspect_Main02_IplBox.Visible = false;
            uPanel_Uper.Visible = false;
            uPanel_Down.Visible = true;

            //Chart_Making_Down();
            //Chart_DataWrite_Down();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public void Chart_DataWrite_Down()
        {
            for (int i = 0; i < 100; i++)
            {
                Down_MeasureTables[0].Rows[0][i] = i + 1;
                Down_MeasureTables[0].Rows[1][i] = 100 - i;
            }
        }

        List<string> Chart_No_Array_Down_01 = new List<string>();
        List<string> Chart_No_Array_Down_02 = new List<string>();
        List<string> Chart_No_Array_Down_03 = new List<string>();
        List<string> Chart_No_Array_Down_04 = new List<string>();
        List<string> Chart_No_Array_Down_05 = new List<string>();
        List<string> Chart_No_Array_Down_06 = new List<string>();
        List<string> Chart_No_Array_Down_07 = new List<string>();
        List<string> Chart_No_Array_Down_08 = new List<string>();
        List<string> Chart_No_Array_Down_09 = new List<string>();
        List<string> Chart_No_Array_Down_10 = new List<string>();
        List<string>[] Chart_Title_No_Down = new List<string>[10];

        public void Chart_Down_Title_Init()
        {
            for (int i = 0; i < Chart_Title_No_Down.Count(); i++)
            {
                Chart_Title_No_Down[i].Clear();
            }
        }

        

        List<string> chart_Array_Down = new List<string>();
        public void Chart_Make_Array_Down(string chartNo)
        {
            foreach (var ReadNo in chart_Array_Down)
            {
                if (chartNo == ReadNo) return;
            }
            chart_Array_Down.Add(chartNo);
        }

        //20150302 WKB 209

        private void Chart_Initialize_Down()
        {
            for (int i = 0; i < 10; i++)
            {
                this.DownData_Charts[i].Visible = false;
            }

            for (int i = 0; i < DownChart_Count; i++)
            {
                //20150302 WKB 208
                //ultraChartSetup(this.DownData_Charts[i]);

                //20150302 WKB 209
                //업과 다운이 함께 사용하는데 함수내에 업속성을 사용하므로
                //에러가 발생할 수 있어서 이를 분리하여 어퍼와 다운으로 분리함.
                ultraChartSetup_Down(this.DownData_Charts[i]);

                this.Down_MeasureTables[i] = new DataTable();
                this.DownData_Charts[i].DataSource = initializeDataTable(Down_MeasureTables[i]);

                this.DownData_Charts[i].Location = Down_chartPoints[i];
                //this.DownData_Charts[i].Visible = true;

                if (Chart_Title_No_Down[i].Count() == 0)
                {
                    this.DownData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                    this.DownData_Charts[i].Axis.Y.RangeMax = 2.0d;
                    this.DownData_Charts[i].Axis.Y.RangeMin = 0.0d;
                    continue;
                }

                this.DownData_Charts[i].Visible = true;
                //float CenValue = float.Parse(Chart_Title_No_Down[i][2]);
                float MaxValue = float.Parse(Chart_Title_No_Down[i][2]) + float.Parse(Chart_Title_No_Down[i][3]) * 2;
                float MinValue = float.Parse(Chart_Title_No_Down[i][2]) - float.Parse(Chart_Title_No_Down[i][4]) * 2;
                this.DownData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                this.DownData_Charts[i].Axis.Y.RangeMax = MaxValue;
                this.DownData_Charts[i].Axis.Y.RangeMin = MinValue;
            }
        }
        /*
        private void Chart_Initialize_Uper()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    this.UperData_Charts[i].Visible = false;
            //}

            for (int i = 0; i < UperChart_Count; i++)
            {
                ultraChartSetup(this.UperData_Charts[i]);

                this.Uper_MeasureTables[i] = new DataTable();
                this.UperData_Charts[i].DataSource = initializeDataTable(Uper_MeasureTables[i]);

                this.UperData_Charts[i].Location = Uper_chartPoints[i];
                this.UperData_Charts[i].Visible = true;

                if (Chart_Title_No_Uper[i].Count() == 0)
                {
                    this.UperData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                    this.UperData_Charts[i].Axis.Y.RangeMax = 2.0d;
                    this.UperData_Charts[i].Axis.Y.RangeMin = 0.0d;
                    continue;
                }

                //float CenValue = float.Parse(Chart_Title_No_Uper[i][2]);
                float MaxValue = float.Parse(Chart_Title_No_Uper[i][2]) + float.Parse(Chart_Title_No_Uper[i][3]) * 2;
                float MinValue = float.Parse(Chart_Title_No_Uper[i][2]) - float.Parse(Chart_Title_No_Uper[i][4])*2;
                this.UperData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                this.UperData_Charts[i].Axis.Y.RangeMax = MaxValue;
                this.UperData_Charts[i].Axis.Y.RangeMin = MinValue;
            }
        }
        */
        //20150302 WKB 207
        /*
        private void Chart_Initialize_Down()
        {
            for (int i = 0; i < 10; i++)
            {
                this.DownData_Charts[i].Visible = false;
            }

            for (int i = 0; i < DownChart_Count; i++)
            {
                ultraChartSetup(this.DownData_Charts[i]);

                this.Down_MeasureTables[i] = new DataTable();
                this.DownData_Charts[i].DataSource = initializeDataTable(Down_MeasureTables[i]);

                this.DownData_Charts[i].Location = Down_chartPoints[i];
                this.DownData_Charts[i].Visible = true;

                float CenValue = float.Parse(Chart_Title_No_Down[i][2]);
                float MaxValue = float.Parse(Chart_Title_No_Down[i][2]) + float.Parse(Chart_Title_No_Down[i][3]) * 2;
                float MinValue = float.Parse(Chart_Title_No_Down[i][2]) - float.Parse(Chart_Title_No_Down[i][4]) * 2;
                this.DownData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                this.DownData_Charts[i].Axis.Y.RangeMax = MaxValue;
                this.DownData_Charts[i].Axis.Y.RangeMin = MinValue;
            }
        }
        */
        



        private void timer1_Tick(object sender, EventArgs e)
        {
            Inspect_uLabel_System12.Text = DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private int graph_Col_Count = 0;
        public void Test_Graph_DataTable()
        {
            Double wData = 3.0d;
            Uper_MeasureTables[0].Columns.RemoveAt(0);
            var measureColumn = new DataColumn();
            string colName = (99 + 1).ToString();
            measureColumn.DataType = Type.GetType("System.Double");
            measureColumn.AllowDBNull = false;
            measureColumn.DefaultValue = 0;
            Uper_MeasureTables[0].Columns.Add(measureColumn);
            Double tmpdouble = 0d;
            if (graph_Col_Count%2 == 0) tmpdouble = (double)wData - 0.9d;
            else tmpdouble = (double)wData + 0.9d;

            if (graph_Col_Count == 5) tmpdouble = (double)2d;

            Uper_MeasureTables[0].Rows[0][99] = tmpdouble;

            //Uper_MeasureTables[0].Rows[0][graph_Col_Count] = graph_Col_Count/10 + 1;
            if (graph_Col_Count >= 99) graph_Col_Count = 0;
            else graph_Col_Count++;
        }
        private void Inspect_uLabel_MainTitle01_Click(object sender, EventArgs e)
        {
        }

        private void FormDlgInsp_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TestStartEvent();
            Measurement_DataSet_To_Register(LamiSystem.RegPathMeasureGrid_Buf_Uper, uDS_Inspect_Measure_Uper);
            Measurement_DataSet_To_Register(LamiSystem.RegPathMeasureGrid_Buf_Down, uDS_Inspect_Measure_Down);
            Measurement_DataTable_To_Register(LamiSystem.RegPathMeasureChart_Buf_Uper, Uper_MeasureTables, "Uper");
            Measurement_DataTable_To_Register(LamiSystem.RegPathMeasureChart_Buf_Down, Down_MeasureTables, "Down");

            if (MIL_Grab_Threading_Uper == null) return;

            if (MIL_Grab_Threading_Uper.IsAlive == true)
            {
                Inspect_Stop_threading = new Thread(Inspect_Stop_Ready);
                Inspect_Stop_threading.Start();
            }
        }

        public void Measurement_DataSet_To_Register(string strNodePath, Infragistics.Win.UltraWinDataSource.UltraDataSource doDataSet)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                reg = reg.OpenSubKey(strNodePath, true);
                if (reg != null)
                {
                    reg.Close();
                    Registry.CurrentUser.DeleteSubKey(strNodePath, false);
                }
                int ColCount = uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].Columns.Count;
                int titleNo = 0;
                for (int i = 0; i < doDataSet.Rows.Count; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        string wData = doDataSet.Rows[i].GetCellValue(j).ToString();
                        this.SetReg(strNodePath, titleNo.ToString("000"), wData);
                        titleNo++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }


        public void Measurement_DataTable_To_Register(string strNodePath, DataTable[] MeasureTables, string SideData)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                reg = reg.OpenSubKey(strNodePath, true);
                if (reg != null)
                {
                    reg.Close();
                    Registry.CurrentUser.DeleteSubKey(strNodePath, false);
                }

                int GraphItem_Count = 0;
                int ColCount = uGrd_Inspect_Measure_Uper.DisplayLayout.Bands[0].Columns.Count;
                int titleNo = 0;
                for (int i = 0; i < MeasureTables.Count(); i++)
                {
                    if (MeasureTables[i] == null) 
                        continue;
                    if (SideData == "Uper") GraphItem_Count = Chart_Title_No_Uper[i].Count / 5;
                    else GraphItem_Count = Chart_Title_No_Down[i].Count / 5;

                    for (int j = 0; j < GraphItem_Count; j++)
                    {
                        for (int k = 0; k < 100; k++)
                        {
                            if (k == 0) 
                                titleNo = (i*400) + (j*100);
                            string wData = MeasureTables[i].Rows[j][k].ToString();
                            this.SetReg(strNodePath, titleNo.ToString("0000"), wData);
                            titleNo++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }

        }
        //Uper_MeasureTables[Graph_Count].Rows[Row_Count][99] = MeaValue;

        private void upnlInspDlgBack_Click(object sender, EventArgs e)
        {
        }

        private void FormDlgInsp_Click(object sender, EventArgs e)
        {
        }

        private void Inspect_uButton_TestRunStop02_MouseHover(object sender, EventArgs e)
        {
        }

        private void Inspect_uButton_TestRunStop03_Click(object sender, EventArgs e)
        {
           
        }

        private void InspectBiCell_uButton_Assy09_Click(object sender, EventArgs e)
        {
            SetReg(LamiSystem.RegPathGapStatus, "Count_Trigger", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_Product", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Both", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Uper", "0");
            SetReg(LamiSystem.RegPathGapStatus, "Count_NG_Down", "0");

            NowProdectNumber = 0;
            NowTrigNumber = 0;
            NowFailNumber = 0;
            NowFailNumber_Down = 0;
            NowFailNumber_Uper = 0;

            //InspectBiCell_uLabel_Assy04.Text = NowTrigNumber_BiCell.ToString("0");
            //InspectBiCell_uLabel_Assy06.Text = NowProdectNumber_BiCell.ToString("0");
            //InspectBiCell_uLabel_Assy08.Text = NowFailNumber_BiCell.ToString("0");
        }

        private delegate void Delegate_MIL_Grab_Thread_Stoping();

        private delegate void Delegate_Run_Run_CuttingZone_To_Image_Display();

        private delegate void Delegate_Run_Run_Drawing_Result_Inspection_Display();

        private delegate void Delegate_Run_Run_Drawing_Result_To_File_Display(); //string fileName);

        private delegate void Delegate_Run_Run_Finded_Lines_CenterPoint_Check_FailDisplay(
            IplImage inspImage, CvPoint sPoint, CvPoint ePoint);

        private delegate void Delegate_Run_Run_ImageLoading_To_Box_Display_BiCell(int imgCount);

        private delegate void Delegate_Run_Run_ImageLoading_To_Box_Display_Gap(int imgCount);

        private delegate void Delegate_Run_Run_Manual_Grab_Down();

        private delegate void Delegate_Run_Run_Manual_Grab_Gap();

        private delegate void Delegate_Run_Run_Test_DisplayZone_Gray_Display();

        private delegate void Delegate_Run_Run_Threading2_Display(TextBox tBox, int dataNo);

        private delegate void Delegate_Run_Run_Threading_Display();

        private delegate void Delegate_myEvent_Inspect_BiCell_01(uint gapNo, uint productNo);

        private delegate void Delegate_myEvent_Inspect_Gap_01(int gapNo, uint productNo);

        private delegate void Delegate_myEvent_Inspect_Gap_02(int gapNo, uint productNo);

        private delegate void Deligate_Run_Run_Display_PictureBox();

        private delegate IplImage WritetextDelegate();


        private string Select_Inspect_Image_Name = string.Empty;

        //20150327 WKB 301
        public void Inspect_get_Control_Image_All_Uper(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.PerformClick();

            string ClickName = ((Control) sender).Name.ToString();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("uPnl_History_All_Uper", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control) al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl) (uPnl_History_All_Uper.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameAll_Uper[i - 2];

                            if (Inspect_ImageList_IplBox.Visible == false)
                            {
                                Inspect_ImageList_IplBox.Visible = true;
                            }

                            if (_strHistoryViewNameAll_Uper[i - 2] == null) return;

                            Inspect_ImageList_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameAll_Uper[i - 2]);
                            Inspect_ImageList_IplBox.Refresh();

                            //기존의 분석한 측정값을 불러오는 함수
                            History_Read_DataTable_All_Uper(i-2);
                            
                            History_Read_ROI_Draw_Uper();

                            //이미지를 다시 분석해서 측정값을 찾아내는 함수
                            //Inspect_getHistory_Image_Uper();
                        }
                        break;
                    }
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        //20150327 WKB 209
        /*
        public void Inspect_get_Control_Image_All_Uper(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.PerformClick();

            string ClickName = ((Control) sender).Name.ToString();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("uPnl_History_All_Uper", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control) al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl) (uPnl_History_All_Uper.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameAll_Uper[i - 2];
                            Inspect_Main01_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameAll_Uper[i - 2]);
                            Inspect_Main01_IplBox.Refresh();
                            
                            //기존의 분석한 측정값을 불러오는 함수
                            History_Read_DataTable_All_Uper(i-2);
                            
                            History_Read_ROI_Draw_Uper();

                            //이미지를 다시 분석해서 측정값을 찾아내는 함수
                            //Inspect_getHistory_Image_Uper();
                        }
                        break;
                    }
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */


        private void History_Read_DataTable_All_Uper(int rowNum)
        {
            UltraGridRow row = uGrd_History_All_Uper.DisplayLayout.Rows[rowNum];
            for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
            {
                if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value = row.Cells[(i + 1).ToString("0")].Value;
                double NowValue = double.Parse(row.Cells[(i + 1).ToString("0")].Value.ToString());

                Random r = new Random();

                //20150217 WKB 207
                //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                //20150217 WKB 208
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                NowValue = NowValue + (double)tmpValue;

                uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value = NowValue.ToString("0.000");

                double MaxValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                double MinValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                if (MaxValue > NowValue && MinValue < NowValue)
                {
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;

                }
                    
                else
                {
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                }
            }
        }

        //20150327 WKB 301
        public void Inspect_get_Control_Image_NG_Uper(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.PerformClick();

            string ClickName = ((Control)sender).Name.ToString();
            ArrayList al = new ArrayList();
            
            getControls(this.Controls.Find("uPnl_History_NG_Uper", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control)al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl)(uPnl_History_NG_Uper.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameNG_Uper[i - 2];
                            Inspect_ImageList_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameNG_Uper[i - 2]);
                            Inspect_ImageList_IplBox.Refresh();

                            //기존에 측정한 값을 읽어오는 함수
                            History_Read_DataTable_NG_Uper(i - 2);
                            History_Read_ROI_Draw_Uper();

                            //이미지를 분석해서 측정값을 구하는 함수
                            //Inspect_getHistory_Image_Uper();
                        }
                        break;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        //20150327 WKB 209
        /*
        public void Inspect_get_Control_Image_NG_Uper(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual18.PerformClick();

            string ClickName = ((Control)sender).Name.ToString();
            ArrayList al = new ArrayList();

            //uPnl_History_All_Uper
            getControls(this.Controls.Find("uPnl_History_NG_Uper", true), ref al);
            
            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control)al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl)(uPnl_History_NG_Uper.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameNG_Uper[i - 2];
                            Inspect_Main01_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameNG_Uper[i - 2]);
                            Inspect_Main01_IplBox.Refresh();

                            //기존에 측정한 값을 읽어오는 함수
                            History_Read_DataTable_NG_Uper(i - 2);
                            History_Read_ROI_Draw_Uper();

                            //이미지를 분석해서 측정값을 구하는 함수
                            //Inspect_getHistory_Image_Uper();
                        }
                        break;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */

        private void History_Read_DataTable_NG_Uper(int rowNum)
        {
            UltraGridRow row = uGrd_History_NG_Uper.DisplayLayout.Rows[rowNum];
            for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
            {
                if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                //uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value = row.Cells[(i + 1).ToString("0")].Value;
                double NowValue = double.Parse(row.Cells[(i + 1).ToString("0")].Value.ToString());

                Random r = new Random();
                //20150217 WKB 207
                //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                //20150217 WKB 208
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                NowValue = NowValue + (double)tmpValue;

                uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["측정 값"].Value = NowValue.ToString("0.000");

                double MaxValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                double MinValue = double.Parse(uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                if (MaxValue > NowValue && MinValue < NowValue)
                {
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;
                }
                else
                {
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";
                    uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                }
            }
        }


        //히스토리에서 선택한 그림판의 이미지를 읽어온다.
        public void Inspect_get_Control_Image_All_Down(object sender)
        {

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual15.PerformClick();

            string ClickName = ((Control)sender).Name.ToString();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("uPnl_History_All_Down", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control)al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl)(uPnl_History_All_Down.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameAll_Down[i - 2];
                            Inspect_ImageList_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameAll_Down[i - 2]);
                            Inspect_ImageList_IplBox.Refresh();

                            //기존의 분석한 측정값을 불러오는 함수
                            History_Read_DataTable_All_Down(i - 2);
                            History_Read_ROI_Draw_Down();

                            //이미지를 다시 분석해서 측정값을 찾아내는 함수
                            //Inspect_getHistory_Image_Down();
                        }
                        break;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        //히스토리에서 선택한 그림판의 측정 값을 읽어와 표시한다.
        private void History_Read_DataTable_All_Down(int rowNum)
        {
            UltraGridRow row = uGrd_History_All_Down.DisplayLayout.Rows[rowNum];
            for (int i = 0; i < uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count; i++)
            {
                if (uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                //uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정 값"].Value = row.Cells[(i + 1).ToString("0")].Value;
                double NowValue = double.Parse(row.Cells[(i + 1).ToString("0")].Value.ToString());

                Random r = new Random();
                //20150217 WKB 207
                //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                //20150217 WKB 208
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                NowValue = NowValue + (double)tmpValue;
                uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정 값"].Value = NowValue.ToString("0.000");

                double MaxValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                double MinValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                if (MaxValue > NowValue && MinValue < NowValue)
                {
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;
                }
                else
                {
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                }
            }
        }

        private Graphics Box_Uper;
        private Graphics Box_Down;

        //20150327 WKB 301
        private void History_Read_ROI_Draw_Uper()
        {
            try
            {
                if (Inspect_Main01_IplBox.Visible != true) return;

                _iPaint_Uper_Flag = 2;

                Result_Display_Struct Struct_Data = new Result_Display_Struct();
                
              
                GetSet_Draw_GC_Uper = Inspect_Main01_IplBox.CreateGraphics();

                
                for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
                {
                    if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                    string checkResult = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value.ToString();

                    if (checkResult == "NG") myLinePen.Color = Color.Red;
                    else myLinePen.Color = Color.LawnGreen;

                    gc_List.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2]);
                    gc_List.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2 + 1]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        //20150327 WKB 209
        /*
        private void History_Read_ROI_Draw_Uper()
        {
            try
            {
                //Inspect_uButton_Manual18.PerformClick();

                if (Inspect_Main01_IplBox.Visible != true) return;

                _iPaint_Uper_Flag = 2;

                Result_Display_Struct Struct_Data = new Result_Display_Struct();
                
                //1
                GetSet_Draw_GC_Uper = Inspect_Main01_IplBox.CreateGraphics();

                //1
                //Box_Uper = Inspect_Main01_IplBox.CreateGraphics();

                //Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);
                
                for (int i = 0; i < uGrd_Inspect_Measure_Uper.DisplayLayout.Rows.Count; i++)
                {
                    if (uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                    string checkResult = uGrd_Inspect_Measure_Uper.DisplayLayout.Rows[i].Cells["판정 결과"].Value.ToString();

                    if (checkResult == "NG") myLinePen.Color = Color.Red;
                    else myLinePen.Color = Color.LawnGreen;

                    //2
                    //Box_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2]);
                    //Box_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2 + 1]);

                    //2
                    gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2]);
                    gc_Uper.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Uper[i * 2 + 1]);
                    //2
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */

        //20150327 WKB 301
        public void Inspect_get_Control_Image_NG_Down(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual15.PerformClick();

            string ClickName = ((Control)sender).Name.ToString();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("uPnl_History_NG_Down", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control)al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl)(uPnl_History_NG_Down.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameNG_Down[i - 2];
                            Inspect_ImageList_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameNG_Down[i - 2]);
                            Inspect_ImageList_IplBox.Refresh();

                            //기존의 분석한 측정값을 불러오는 함수
                            History_Read_DataTable_NG_Down(i - 2);
                            History_Read_ROI_Draw_Down();

                            //이미지를 다시 분석해서 측정값을 찾아내는 함수
                            //Inspect_getHistory_Image_Down();
                        }
                        break;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        //20150327 WKB 209
        /*
        public void Inspect_get_Control_Image_NG_Down(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual15.PerformClick();

            string ClickName = ((Control)sender).Name.ToString();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("uPnl_History_NG_Down", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control)al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl)(uPnl_History_NG_Down.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameNG_Down[i - 2];
                            Inspect_Main02_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameNG_Down[i - 2]);
                            Inspect_Main02_IplBox.Refresh();

                            //기존의 분석한 측정값을 불러오는 함수
                            History_Read_DataTable_NG_Down(i - 2);
                            History_Read_ROI_Draw_Down();

                            //이미지를 다시 분석해서 측정값을 찾아내는 함수
                            //Inspect_getHistory_Image_Down();
                        }
                        break;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */
        /*
        public void Inspect_get_Control_Image_NG_Down(object sender)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            Inspect_uButton_Manual15.PerformClick();

            string ClickName = ((Control)sender).Name.ToString();
            ArrayList al = new ArrayList();

            getControls(this.Controls.Find("uPnl_History_NG_Down", true), ref al);

            for (int i = 0; i < al.Count; i++)
            {
                if (al[i] is PictureBoxIpl)
                {
                    string tmpName = ((Control)al[i]).Name.ToString();
                    if (ClickName == tmpName)
                    {
                        if (((PictureBoxIpl)(uPnl_History_NG_Down.ClientArea.Controls[i - 2])).ImageIpl != null)
                        {
                            Select_Inspect_Image_Name = _strHistoryViewNameNG_Down[i - 2];
                            Inspect_Main02_IplBox.ImageIpl = IplImage.FromFile(_strHistoryViewNameNG_Down[i - 2]);
                            Inspect_Main02_IplBox.Refresh();

                            //기존의 분석한 측정값을 불러오는 함수
                            History_Read_DataTable_NG_Down(i - 2);
                            History_Read_ROI_Draw_Down();

                            //이미지를 다시 분석해서 측정값을 찾아내는 함수
                            //Inspect_getHistory_Image_Down();
                        }
                        break;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        */
        private void History_Read_DataTable_NG_Down(int rowNum)
        {
            UltraGridRow row = uGrd_History_NG_Down.DisplayLayout.Rows[rowNum];
            for (int i = 0; i < uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count; i++)
            {
                if (uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                //uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정 값"].Value = row.Cells[(i + 1).ToString("0")].Value;
                double NowValue = double.Parse(row.Cells[(i + 1).ToString("0")].Value.ToString());
                Random r = new Random();
                
                //20150217 WKB 207
                //float tmpValue = (float)(r.Next(-100, 100) / 10000f);

                //20150217 WKB 208
                //float tmpValue = (float)(r.Next(-99, 99) / 10000f);

                //20150226 WKB 208
                float tmpValue = (float)(r.Next(-99, 99) / 100000f);

                NowValue = NowValue + (double)tmpValue;
                uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["측정 값"].Value = NowValue.ToString("0.000");

                double MaxValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 상한"].Value.ToString());
                double MinValue = double.Parse(uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["규격 하한"].Value.ToString());

                if (MaxValue > NowValue && MinValue < NowValue)
                {
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "OK";
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;
                }
                else
                {
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value = "NG";
                    uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Appearance.BackColor = Color.OrangeRed;
                }
            }
        }

        //20150327 WKB 301
        private void History_Read_ROI_Draw_Down()
        {
            try
            {
                //Inspect_uButton_Manual15.PerformClick();
                if (Inspect_Main02_IplBox.Visible != true) return;

                _iPaint_Down_Flag = 2;

                Result_Display_Struct Struct_Data = new Result_Display_Struct();
                GetSet_Draw_GC_Down = Inspect_Main02_IplBox.CreateGraphics();

                //Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);

                for (int i = 0; i < uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count; i++)
                {
                    if (uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                    string checkResult = uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value.ToString();

                    if (checkResult == "NG") myLinePen.Color = Color.Red;
                    else myLinePen.Color = Color.LawnGreen;

                    gc_List.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i * 2]);
                    gc_List.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i * 2 + 1]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }

        //20150327 WKB 209
        /*
        private void History_Read_ROI_Draw_Down()
        {
            try
            {
                //Inspect_uButton_Manual15.PerformClick();
                if (Inspect_Main02_IplBox.Visible != true) return;

                _iPaint_Down_Flag = 2;

                Result_Display_Struct Struct_Data = new Result_Display_Struct();
                GetSet_Draw_GC_Down = Inspect_Main02_IplBox.CreateGraphics();

                //Struct_Data._font = new Font(new FontFamily("Arial"), 15, FontStyle.Bold);

                for (int i = 0; i < uGrd_Inspect_Measure_Down.DisplayLayout.Rows.Count; i++)
                {
                    if (uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["검사 항목"].Value.ToString() == "") return;

                    string checkResult = uGrd_Inspect_Measure_Down.DisplayLayout.Rows[i].Cells["판정 결과"].Value.ToString();

                    if (checkResult == "NG") myLinePen.Color = Color.Red;
                    else myLinePen.Color = Color.LawnGreen;

                    gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i * 2]);
                    gc_Down.DrawRectangle(myLinePen, LamiSystem.RectListInspBoxZone_Down[i * 2 + 1]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
            }
        }
        */

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

        private void Inspect_getHistory_Image_Uper()
        {
            int startNo = Select_Inspect_Image_Name.LastIndexOf("\\");
            int startNo2 = Select_Inspect_Image_Name.IndexOf(" ", startNo + 1);
            //string strNo = Select_Inspect_Image_Name.Substring(startNo2 + 1, 2);
            string strNo = Select_Inspect_Image_Name.Substring(startNo + 1, startNo2 - startNo - 1);

            int readGapNumber = -1;
            bool result = int.TryParse(Select_Inspect_Image_Name.Substring(startNo2 + 1, 2), out readGapNumber);
            if (result == true) NowGapNumber = readGapNumber;

            Graphics gc = Inspect_Main01_IplBox.CreateGraphics();
            Run_Mode = "Manual";
            SrcIplImage_Uper = Inspect_Main01_IplBox.ImageIpl;
            Inspect_Manual_Image_Grabing();
        }

        /*
        private void Inspect_getHistory_Image_Uper()
        {
            //Loading_Image_Name.Text = Select_Inspect_Image_Name;
            //if (Loading_Image_Name.Visible == false) Loading_Image_Name.Visible = true;

            int startNo = Select_Inspect_Image_Name.LastIndexOf("\\");
            int startNo2 = Select_Inspect_Image_Name.IndexOf(" ", startNo + 1);
            string strNo = Select_Inspect_Image_Name.Substring(startNo2 + 1, 2);
            int readGapNumber = -1;
            bool result = int.TryParse(Select_Inspect_Image_Name.Substring(startNo2 + 1, 2), out readGapNumber);
            if (result == true) NowGapNumber = readGapNumber;

            Graphics gc = Inspect_Main01_IplBox.CreateGraphics();
            Run_Mode = "Manual";
            SrcIplImage_Uper = Inspect_Main01_IplBox.ImageIpl;
            Inspect_Manual_Image_Grabing();
        }
        */

        private void Inspect_getHistory_Image_Down()
        {
            int startNo = Select_Inspect_Image_Name.LastIndexOf("\\");
            int startNo2 = Select_Inspect_Image_Name.IndexOf(" ", startNo + 1);
            string strNo = Select_Inspect_Image_Name.Substring(startNo2 + 1, 2);
            int readGapNumber = -1;
            bool result = int.TryParse(Select_Inspect_Image_Name.Substring(startNo2 + 1, 2), out readGapNumber);
            if (result == true) NowGapNumber = readGapNumber;

            Graphics gc = Inspect_Main02_IplBox.CreateGraphics();
            Run_Mode = "Manual";
            SrcIplImage_Uper = Inspect_Main02_IplBox.ImageIpl;
            Inspect_Manual_Image_Grabing();
        }
        

        //ALL 0번 갭 클릭
        private void Inspect_cvPBox_HistoryAll11_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        //ALL 1번 갭 클릭
        private void Inspect_cvPBox_HistoryAll10_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll09_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll08_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll07_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll06_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll05_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll01_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll04_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll03_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll02_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void pictureBoxIpl23_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl24_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl25_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl26_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl27_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl28_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl29_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl30_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl31_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl32_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl33_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl1_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl2_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl3_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl4_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl5_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl6_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl7_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl8_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl9_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl10_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl11_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl12_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl13_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl14_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl15_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl16_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl17_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl18_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl19_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl20_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl21_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl22_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        public void Chart_Array_Make()
        {
            UperData_Charts = new UltraChart[10];
            DownData_Charts = new UltraChart[10];
            Uper_MeasureTables = new DataTable[10];
            Down_MeasureTables = new DataTable[10];

            UperData_Charts[0] = this.uChart_Uper_U01;
            UperData_Charts[1] = this.uChart_Uper_U02;
            UperData_Charts[2] = this.uChart_Uper_U03;
            UperData_Charts[3] = this.uChart_Uper_U04;
            UperData_Charts[4] = this.uChart_Uper_U05;
            UperData_Charts[5] = this.uChart_Uper_U06;
            UperData_Charts[6] = this.uChart_Uper_U07;
            UperData_Charts[7] = this.uChart_Uper_U08;
            UperData_Charts[8] = this.uChart_Uper_U09;
            UperData_Charts[9] = this.uChart_Uper_U10;

            DownData_Charts[0] = this.uChart_Down_U01;
            DownData_Charts[1] = this.uChart_Down_U02;
            DownData_Charts[2] = this.uChart_Down_U03;
            DownData_Charts[3] = this.uChart_Down_U04;
            DownData_Charts[4] = this.uChart_Down_U05;
            DownData_Charts[5] = this.uChart_Down_U06;
            DownData_Charts[6] = this.uChart_Down_U07;
            DownData_Charts[7] = this.uChart_Down_U08;
            DownData_Charts[8] = this.uChart_Down_U09;
            DownData_Charts[9] = this.uChart_Down_U10;

            Chart_Title_No_Uper[0] = Chart_No_Array_Uper_01;
            Chart_Title_No_Uper[1] = Chart_No_Array_Uper_02;
            Chart_Title_No_Uper[2] = Chart_No_Array_Uper_03;
            Chart_Title_No_Uper[3] = Chart_No_Array_Uper_04;
            Chart_Title_No_Uper[4] = Chart_No_Array_Uper_05;
            Chart_Title_No_Uper[5] = Chart_No_Array_Uper_06;
            Chart_Title_No_Uper[6] = Chart_No_Array_Uper_07;
            Chart_Title_No_Uper[7] = Chart_No_Array_Uper_08;
            Chart_Title_No_Uper[8] = Chart_No_Array_Uper_09;
            Chart_Title_No_Uper[9] = Chart_No_Array_Uper_10;

            Chart_Title_No_Down[0] = Chart_No_Array_Down_01;
            Chart_Title_No_Down[1] = Chart_No_Array_Down_02;
            Chart_Title_No_Down[2] = Chart_No_Array_Down_03;
            Chart_Title_No_Down[3] = Chart_No_Array_Down_04;
            Chart_Title_No_Down[4] = Chart_No_Array_Down_05;
            Chart_Title_No_Down[5] = Chart_No_Array_Down_06;
            Chart_Title_No_Down[6] = Chart_No_Array_Down_07;
            Chart_Title_No_Down[7] = Chart_No_Array_Down_08;
            Chart_Title_No_Down[8] = Chart_No_Array_Down_09;
            Chart_Title_No_Down[9] = Chart_No_Array_Down_10;
        }

        Infragistics.Win.Misc.UltraLabel[] UperChart_Title = new UltraLabel[20];


        public Infragistics.Win.UltraWinChart.UltraChart[] DownData_Charts;
        public Infragistics.Win.UltraWinChart.UltraChart[] UperData_Charts;
        public DataTable[] Uper_MeasureTables;
        public DataTable[] Down_MeasureTables;

        private int UperChart_Count = 0;
        private int DownChart_Count = 0;

        /// <summary>
        /// Ultra Chart 의 데이터 값의 표시 구성항목을 설정한다.
        /// </summary>
        private void Chart_Initialize_Uper()
        {
            for (int i = 0; i < 10; i++)
            {
                this.UperData_Charts[i].Visible = false;
            }

            for (int i = 0; i < UperChart_Count; i++)
            {
                ultraChartSetup_Uper(this.UperData_Charts[i]);

                this.Uper_MeasureTables[i] = new DataTable();
                this.UperData_Charts[i].DataSource = initializeDataTable(Uper_MeasureTables[i]);

                this.UperData_Charts[i].Location = Uper_chartPoints[i];
                //this.UperData_Charts[i].Visible = true;

                if (Chart_Title_No_Uper[i].Count() == 0)
                {
                    this.UperData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                    this.UperData_Charts[i].Axis.Y.RangeMax = 2.0d;
                    this.UperData_Charts[i].Axis.Y.RangeMin = 0.0d;
                    continue;
                }
                this.UperData_Charts[i].Visible = true;
                //float CenValue = float.Parse(Chart_Title_No_Uper[i][2]);
                float MaxValue = float.Parse(Chart_Title_No_Uper[i][2]) + float.Parse(Chart_Title_No_Uper[i][3]) * 2;
                float MinValue = float.Parse(Chart_Title_No_Uper[i][2]) - float.Parse(Chart_Title_No_Uper[i][4])*2;
                this.UperData_Charts[i].Axis.Y.RangeType = AxisRangeType.Custom;
                this.UperData_Charts[i].Axis.Y.RangeMax = MaxValue;
                this.UperData_Charts[i].Axis.Y.RangeMin = MinValue;
            }
        }

       

        private Point[] Uper_chartPoints;
       
        private Point[] chart_Locattion_Make_Uper(int chart_Count)
        {
            Point[] chartPoints = new Point[chart_Count];

            int backGaro = uPanel_Uper.Width;
            int backSero = uPanel_Uper.Height;
            int lengthGap = 3;

            int RowCount = chart_Count / 2;
            if (chart_Count % 2 != 0) RowCount++;

            //그래프 별 간격을 3으로 주었을때 계산되는 값임.
            int chart_Garo = (backGaro - 9) / 2;
            int chart_Sero = (backSero - 18) / 5;



            for (int i = 0; i < UperChart_Count; i++)
            {
                this.UperData_Charts[i].Width = chart_Garo;
                this.UperData_Charts[i].Height = chart_Sero;

                //if (i / 5 == 0) chartPoints[i].X = 10;
                //else chartPoints[i].X = chart_Garo + 20;
                //chartPoints[i].Y = (i % 5) * (backSero / 5) + 10;

                if (i/5 == 0) chartPoints[i].X = 3;
                else chartPoints[i].X = chart_Garo + 6;

                chartPoints[i].Y = (i%5)*(backSero/5) + 3;
            }
            return chartPoints;
        }

        private Point[] Down_chartPoints;

        private Point[] chart_Locattion_Make_Down(int chart_Count)
        {
            Point[] chartPoints = new Point[chart_Count];

            int backGaro = uPanel_Down.Width;
            int backSero = uPanel_Down.Height;
            int lengthGap = 3;

            int RowCount = chart_Count / 2;
            if (chart_Count % 2 != 0) RowCount++;

            //그래프 별 간격을 3으로 주었을때 계산되는 값임.
            int chart_Garo = (backGaro - 9) / 2;
            int chart_Sero = (backSero - 18) / 5;



            for (int i = 0; i < DownChart_Count; i++)
            {
                this.DownData_Charts[i].Width = chart_Garo;
                this.DownData_Charts[i].Height = chart_Sero;

                if (i / 5 == 0) chartPoints[i].X = 3;
                else chartPoints[i].X = chart_Garo + 6;

                chartPoints[i].Y = (i % 5) * (backSero / 5) + 3;
            }
            return chartPoints;
        }

        int[] mesaureData = new int[100];
        public void Chart_DataWrite_Uper()
        {
            for (int i = 0; i < 100; i++)
            {
                Uper_MeasureTables[0].Rows[0][i] = 2d;
                Uper_MeasureTables[0].Rows[1][i] = 3d;
            }
        }

        //20150302 WKB 209
        public struct initDataTable
        {
            public DataColumn measureColumn;
            public string colName;
            public DataRow measureRow_01;
            public DataRow measureRow_02;
        }
        //20150302 WKB 209
        private DataTable initializeDataTable(DataTable measureTable)
        {
            measureTable.Columns.Clear();
            measureTable.Rows.Clear();

            initDataTable struct_initData = new initDataTable();

            for (int i = 0; i < 100; i++)
            {
                //var measureColumn = new DataColumn();
                //string colName = (i + 1).ToString();
                struct_initData.measureColumn = new DataColumn();
                struct_initData.colName = (i + 1).ToString();

                struct_initData.measureColumn.DataType = Type.GetType("System.Decimal");
                struct_initData.measureColumn.AllowDBNull = false;
                struct_initData.measureColumn.Caption = struct_initData.colName;
                struct_initData.measureColumn.ColumnName = struct_initData.colName;
                struct_initData.measureColumn.DefaultValue = 0;
                measureTable.Columns.Add(struct_initData.measureColumn);
            }

            //DataRow measureRow_01 = measureTable.NewRow();
            struct_initData. measureRow_01 = measureTable.NewRow();
            measureTable.Rows.Add(struct_initData.measureRow_01);

            //DataRow measureRow_02 = measureTable.NewRow();
            struct_initData.measureRow_02 = measureTable.NewRow();
            measureTable.Rows.Add(struct_initData.measureRow_02);

            return measureTable;
        }

        //20150302 WKB 207
        /*
        private DataRow measureRow_01;
        private DataRow measureRow_02;
        /// <summary>
        /// Ultra Chart 에 사용되어지는 DataTable을 초기화 한다.
        /// </summary>
        private DataTable initializeDataTable(DataTable measureTable)
        {
            measureTable.Columns.Clear();
            measureTable.Rows.Clear();

            for (int i = 0; i < 100; i++)
            {
                //measureTable.Columns.Add(new DataColumn(i.ToString(), Type.GetType("System.Decimal")));
                //measureTable.Columns[i].AllowDBNull = false;

                var measureColumn = new DataColumn();
                string colName = (i+1).ToString();
                measureColumn.DataType = Type.GetType("System.Decimal");
                measureColumn.AllowDBNull = false;
                measureColumn.Caption = colName;
                measureColumn.ColumnName = colName;
                measureColumn.DefaultValue = 0;
                measureTable.Columns.Add(measureColumn);
            }

            measureRow_01 = measureTable.NewRow();
            measureTable.Rows.Add(measureRow_01);

            measureRow_02 = measureTable.NewRow();
            measureTable.Rows.Add(measureRow_02);

            return measureTable;
        }
        */
        private void ultraChartSetup(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            iChart.Legend.Visible = false;

            //iChart.Axis.Y.RangeMax = 10;
            //iChart.Axis.X.LineThickness = 2;
            //iChart.Axis.Y.LineThickness = 2;
            iChart.ColorModel.CustomPalette = ChartColors;

            for (int i = 0; i < Chart_Title_No_Uper.Count(); i++)
            {
                if (Chart_Title_No_Uper[i].Count != 0)
                {
                    iChart.Axis.Y.RangeType = AxisRangeType.Custom;
                    iChart.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
                    iChart.Axis.Y.TickmarkInterval = float.Parse(Chart_Title_No_Uper[i][3]);
                    iChart.Axis.Y.Labels.ItemFormat = AxisItemLabelFormat.DataValue;
                    //iChart.Axis.Y.Labels.ItemFormatString = "<ITEM_LABEL>";
                }

            }
            //iChart.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
            //iChart.Axis.X.TickmarkInterval = (double)10;
            //iChart.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.DataValue;
            //iChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";

            iChart.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
            if (iChart.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                //iChart.Tooltips.FormatString = "<ITEM_LABEL> : <DATA_VALUE:0.###>";
                iChart.Tooltips.FormatString = "<DATA_VALUE:0.###>";
            }
        }

        Color[] ChartColors = new Color[] { Color.DimGray, Color.Orange, Color.Blue, Color.Green };
        private void ultraChartSetup_Uper(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            iChart.Legend.Visible = false;

            iChart.ColorModel.CustomPalette = ChartColors;

            for (int i = 0; i < Chart_Title_No_Uper.Count(); i++)
            {
                if (Chart_Title_No_Uper[i].Count != 0)
                {
                    iChart.Axis.Y.RangeType = AxisRangeType.Custom;
                    iChart.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
                    iChart.Axis.Y.TickmarkInterval = float.Parse(Chart_Title_No_Uper[i][3]);
                    iChart.Axis.Y.Labels.ItemFormat = AxisItemLabelFormat.DataValue;
                    //iChart.Axis.Y.Labels.ItemFormatString = "<ITEM_LABEL>";
                }
                
            }

            iChart.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
            if (iChart.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                iChart.Tooltips.FormatString = "<DATA_VALUE:0.###>";
            }
        }

        private void ultraChartSetup_Down(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            iChart.Legend.Visible = false;

            iChart.ColorModel.CustomPalette = ChartColors;

            for (int i = 0; i < Chart_Title_No_Down.Count(); i++)
            {
                if (Chart_Title_No_Down[i].Count != 0)
                {
                    iChart.Axis.Y.RangeType = AxisRangeType.Custom;
                    iChart.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
                    iChart.Axis.Y.TickmarkInterval = float.Parse(Chart_Title_No_Down[i][3]);
                    iChart.Axis.Y.Labels.ItemFormat = AxisItemLabelFormat.DataValue;
                }
            }
            
            iChart.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
            if (iChart.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                iChart.Tooltips.FormatString = "<DATA_VALUE:0.###>";
            }
        }

        private void uChart_Uper_U01_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 0);
        }

        public void uChart_Uper_FillSceneGraph(object sender,Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e, int GraphNo)
        {
            IAdvanceAxis x = (IAdvanceAxis)e.Grid["X"];
            IAdvanceAxis y = (IAdvanceAxis)e.Grid["Y"];

            if (x != null)
            {
                if (Chart_Title_No_Uper[GraphNo].Count == 0)
                {
                    //uChart_Uper_Fill_None_Graph(sender, e, GraphNo);
                    UperData_Charts[GraphNo].Visible = false;
                    return;
                }

                //중앙 라인 그리기
                float CenTarget = float.Parse(Chart_Title_No_Uper[GraphNo][2]);
                int yVal = (int)y.Map(CenTarget);
                int xStart = (int)x.Map(0);
                int xEnd = (int)x.Map(99);

                Line l = new Line(new Point((int)xStart, (int)yVal), new Point((int)xEnd, (int)yVal));
                l.PE.Stroke = Color.Blue;
                l.PE.StrokeWidth = 2;
                l.lineStyle.DrawStyle = LineDrawStyle.Dash;
                e.SceneGraph.Add(l);

                //중앙 값 그리기
                string NameItemCent = CenTarget.ToString("0.00");
                Point ptextCompNameCent = new Point(7, yVal);
                Infragistics.UltraChart.Core.Primitives.Text textCompNameCent = new Text(ptextCompNameCent, NameItemCent);
                textCompNameCent.labelStyle.Font = new Font("Bookman Old Style", 6F, System.Drawing.FontStyle.Bold);
                textCompNameCent.labelStyle.FontColor = ChartColors[0];
                e.SceneGraph.Add(textCompNameCent);

                //상단 라인 그리기
                float MaxTarget = float.Parse(Chart_Title_No_Uper[GraphNo][2]) +
                                  float.Parse(Chart_Title_No_Uper[GraphNo][3]);
                 yVal = (int) y.Map(MaxTarget);
                 xStart = (int) x.Map(0);
                 xEnd = (int) x.Map(99);

                 l = new Line(new Point((int) xStart, (int) yVal), new Point((int) xEnd, (int) yVal));
                l.PE.Stroke = Color.Green;
                l.PE.StrokeWidth = 2;
                l.lineStyle.DrawStyle = LineDrawStyle.Dash;
                e.SceneGraph.Add(l);

                //상단 값 그리기
                string NameItemMax = MaxTarget.ToString("0.00");
                Point ptextCompNameMax = new Point(7, yVal);
                Infragistics.UltraChart.Core.Primitives.Text textCompNameMax = new Text(ptextCompNameMax, NameItemMax);
                textCompNameMax.labelStyle.Font = new Font("Bookman Old Style", 6F, System.Drawing.FontStyle.Bold);
                textCompNameMax.labelStyle.FontColor = ChartColors[0];
                e.SceneGraph.Add(textCompNameMax);

                //하단 라인 그리기
                float MinTarget = float.Parse(Chart_Title_No_Uper[GraphNo][2]) -
                                  float.Parse(Chart_Title_No_Uper[GraphNo][4]);
                yVal = (int) y.Map(MinTarget);
                xStart = (int) x.Map(0);
                xEnd = (int) x.Map(99);

                l = new Line(new Point((int) xStart, (int) yVal), new Point((int) xEnd, (int) yVal));
                l.PE.Stroke = Color.Green;
                l.PE.StrokeWidth = 2;
                l.lineStyle.DrawStyle = LineDrawStyle.Dash;
                e.SceneGraph.Add(l);

                //하단 값 그리기
                string NameItemMin = MinTarget.ToString("0.00");
                Point ptextCompNameMin = new Point(7, yVal);
                Infragistics.UltraChart.Core.Primitives.Text textCompNameMin = new Text(ptextCompNameMin, NameItemMin);
                textCompNameMin.labelStyle.Font = new Font("Bookman Old Style", 6F, System.Drawing.FontStyle.Bold);
                textCompNameMin.labelStyle.FontColor = ChartColors[0];
                e.SceneGraph.Add(textCompNameMin);

                if (Chart_Title_No_Uper[GraphNo].Count/5 == 1)
                {
                    string NameItem = Chart_Title_No_Uper[GraphNo][1];
                    int pointX = (UperData_Charts[GraphNo].Width / 2);
                    int pointY = (UperData_Charts[GraphNo].Height - 7); 
                    Point ptextCompName = new Point(pointX, pointY);
                    
                    Infragistics.UltraChart.Core.Primitives.Text textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = ChartColors[0];
                    e.SceneGraph.Add(textCompName);
                }

                if (Chart_Title_No_Uper[GraphNo].Count / 5 == 2)
                {
                    string NameItem = Chart_Title_No_Uper[GraphNo][1];
                    int pointX = (UperData_Charts[GraphNo].Width/2) - (NameItem.Length*15) - 10;// + UperData_Charts[GraphNo].Location.X;
                    int pointY = (UperData_Charts[GraphNo].Height - 7); 
                    Point ptextCompName = new Point(pointX, pointY);
                    
                    Infragistics.UltraChart.Core.Primitives.Text textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = ChartColors[0];
                    e.SceneGraph.Add(textCompName);

                    NameItem = "+";
                    pointX = (DownData_Charts[GraphNo].Width / 2) + (NameItem.Length * 15) - 6;
                    ptextCompName.X = pointX;

                    textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    e.SceneGraph.Add(textCompName);

                    NameItem = Chart_Title_No_Uper[GraphNo][6];
                    pointX = (UperData_Charts[GraphNo].Width/2) + (NameItem.Length*15) + 10;// + Uper_chartPoints[GraphNo].X;
                    ptextCompName.X = pointX;
                    
                    textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = ChartColors[1];
                    e.SceneGraph.Add(textCompName);
                }
            }
        }

        private void uChart_Uper_U01_ChartDrawItem(object sender, Infragistics.UltraChart.Shared.Events.ChartDrawItemEventArgs e)
        {
            
        }

        private void uChart_Uper_U02_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 1);
        }

        private void uChart_Uper_U03_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 2);
        }

        private void uChart_Uper_U04_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 3);
        }

        private void uChart_Uper_U05_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 4);
        }

        private void uChart_Uper_U06_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 5);
        }

        private void uChart_Uper_U07_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 6);
        }

        private void uChart_Uper_U08_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 7);
        }

        private void uChart_Uper_U09_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 8);
        }

        private void uChart_Uper_U10_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Uper_FillSceneGraph(sender, e, 9);
        }


        public void uChart_Down_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e, int GraphNo)
        {
            IAdvanceAxis x = (IAdvanceAxis)e.Grid["X"];
            IAdvanceAxis y = (IAdvanceAxis)e.Grid["Y"];

            if (x != null)
            {
                //중앙 라인 그리기
                float CenTarget = float.Parse(Chart_Title_No_Down[GraphNo][2]);
                int yVal = (int)y.Map(CenTarget);
                int xStart = (int)x.Map(0);
                int xEnd = (int)x.Map(99);

                Line l = new Line(new Point((int)xStart, (int)yVal), new Point((int)xEnd, (int)yVal));
                l.PE.Stroke = Color.Blue;
                l.PE.StrokeWidth = 2;
                l.lineStyle.DrawStyle = LineDrawStyle.Dash;
                e.SceneGraph.Add(l);

                //중앙 값 그리기
                string NameItemCent = CenTarget.ToString("0.00");
                Point ptextCompNameCent = new Point(7, yVal);
                Infragistics.UltraChart.Core.Primitives.Text textCompNameCent = new Text(ptextCompNameCent, NameItemCent);
                textCompNameCent.labelStyle.Font = new Font("Bookman Old Style", 6F, System.Drawing.FontStyle.Bold);
                textCompNameCent.labelStyle.FontColor = ChartColors[0];
                e.SceneGraph.Add(textCompNameCent);

                //상단 라인 그리기
                float MaxTarget = float.Parse(Chart_Title_No_Down[GraphNo][2]) +
                                  float.Parse(Chart_Title_No_Down[GraphNo][3]);
                 yVal = (int)y.Map(MaxTarget);
                 xStart = (int)x.Map(0);
                 xEnd = (int)x.Map(99);

                 l = new Line(new Point((int)xStart, (int)yVal), new Point((int)xEnd, (int)yVal));
                l.PE.Stroke = Color.Green;
                l.PE.StrokeWidth = 2;
                l.lineStyle.DrawStyle = LineDrawStyle.Dash;
                e.SceneGraph.Add(l);

                //상단 값 그리기
                string NameItemMax = MaxTarget.ToString("0.00");
                Point ptextCompNameMax = new Point(7, yVal);
                Infragistics.UltraChart.Core.Primitives.Text textCompNameMax = new Text(ptextCompNameMax, NameItemMax);
                textCompNameMax.labelStyle.Font = new Font("Bookman Old Style", 6F, System.Drawing.FontStyle.Bold);
                textCompNameMax.labelStyle.FontColor = ChartColors[0];
                e.SceneGraph.Add(textCompNameMax);

            
                //하단 라인 그리기
                float MinTarget = float.Parse(Chart_Title_No_Down[GraphNo][2]) -
                                  float.Parse(Chart_Title_No_Down[GraphNo][4]);
                yVal = (int)y.Map(MinTarget);
                xStart = (int)x.Map(0);
                xEnd = (int)x.Map(99);

                l = new Line(new Point((int)xStart, (int)yVal), new Point((int)xEnd, (int)yVal));
                l.PE.Stroke = Color.Green;
                l.PE.StrokeWidth = 2;
                l.lineStyle.DrawStyle = LineDrawStyle.Dash;
                e.SceneGraph.Add(l);


                //하단 값 그리기
                string NameItemMin = MinTarget.ToString("0.00");
                Point ptextCompNameMin = new Point(7, yVal);
                Infragistics.UltraChart.Core.Primitives.Text textCompNameMin = new Text(ptextCompNameMin, NameItemMin);
                textCompNameMin.labelStyle.Font = new Font("Bookman Old Style", 6F, System.Drawing.FontStyle.Bold);
                textCompNameMin.labelStyle.FontColor = ChartColors[0];
                e.SceneGraph.Add(textCompNameMin);

               


                if (Chart_Title_No_Down[GraphNo].Count / 5 == 1)
                {
                    string NameItem = Chart_Title_No_Down[GraphNo][1];
                    int pointX = (DownData_Charts[GraphNo].Width / 2);
                    int pointY = (DownData_Charts[GraphNo].Height - 7);
                    Point ptextCompName = new Point(pointX, pointY);

                    Infragistics.UltraChart.Core.Primitives.Text textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = ChartColors[0];
                    e.SceneGraph.Add(textCompName);
                }

                if (Chart_Title_No_Down[GraphNo].Count / 5 == 2)
                {

                    string NameItem = Chart_Title_No_Down[GraphNo][1] ;
                    int pointX = (DownData_Charts[GraphNo].Width / 2) - (NameItem.Length * 15) - 10;
                    int pointY = (DownData_Charts[GraphNo].Height -7);
                    Point ptextCompName = new Point(pointX, pointY);

                    Infragistics.UltraChart.Core.Primitives.Text textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = ChartColors[0];
                    e.SceneGraph.Add(textCompName);

                    NameItem = "+";
                    pointX = (DownData_Charts[GraphNo].Width / 2) + (NameItem.Length * 15)-6;
                    ptextCompName.X = pointX;

                    textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    e.SceneGraph.Add(textCompName);

                    NameItem = Chart_Title_No_Down[GraphNo][6];
                    pointX = (DownData_Charts[GraphNo].Width / 2) + (NameItem.Length * 15) + 10;
                    ptextCompName.X = pointX;

                    textCompName = new Text(ptextCompName, NameItem);
                    textCompName.labelStyle.Font = new Font("Bookman Old Style", 11F, System.Drawing.FontStyle.Bold);
                    textCompName.labelStyle.FontColor = ChartColors[1];
                    e.SceneGraph.Add(textCompName);
                }
            }
        }
        private void uChart_Down_U01_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 0);
        }

        private void uChart_Down_U02_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 1);
        }

        private void uChart_Down_U03_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 2);
        }

        private void uChart_Down_U04_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 3);
        }

        private void uChart_Down_U05_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 4);
        }

        private void uChart_Down_U06_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 5);
        }

        private void uChart_Down_U07_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 6);
        }

        private void uChart_Down_U08_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 7);
        }

        private void uChart_Down_U09_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 8);
        }

        private void uChart_Down_U10_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            uChart_Down_FillSceneGraph(sender, e, 9);
        }

        //1초에 한번씩 진행하는 그래프 테스트용 데이터 발생함수 호출기
        private void Test_Graph_Tick(object sender, EventArgs e)
        {
            //Test_Graph_DataTable();
        }

        private void uChart_Down_U01_ChartDataClicked(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {

        }

        private void pictureBoxIpl34_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl35_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl36_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl37_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl38_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl39_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl40_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl41_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void pictureBoxIpl42_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll20_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll19_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll18_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll17_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll16_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll15_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll14_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll13_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll12_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void Inspect_cvPBox_HistoryAll02_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Uper(sender);
        }

        private void pictureBoxIpl1_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl2_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl3_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl4_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl5_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl6_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl7_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl8_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl9_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl10_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl11_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl12_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl13_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl14_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl15_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl16_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl17_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl18_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl19_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl20_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_All_Down(sender);
        }

        private void pictureBoxIpl21_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl22_Click_1(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl43_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl44_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl45_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl46_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl47_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl48_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl49_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl50_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl51_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl52_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl53_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl54_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl55_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl56_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl57_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl58_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl59_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private void pictureBoxIpl60_Click(object sender, EventArgs e)
        {
            if (Inspect_uButton_TestRunStop02.Text == "정 지") return;
            Inspect_get_Control_Image_NG_Down(sender);
        }

        private int _iReviewFlag_Uper = 0;
        private int _iPaint_Uper_Flag = 0;


        private delegate void delegate_Main01_IplBox_Reflashing();

        private void Inspect_Main01_IplBox_Reflashing()
        {
            if (InvokeRequired)
            {
                delegate_Main01_IplBox_Reflashing del = Inspect_Main01_IplBox_Reflashing;
                Invoke(del);
            }
            else
            {
                Inspect_Main01_IplBox.Update();
                Inspect_Main01_IplBox.Refresh();
            }
        }

        private void FormDlgInsp_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void Inspect_uTab_ImageList22_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (Inspect_uTab_ImageList22.ActiveTab == null) return;
            if (Inspect_uTab_ImageList22.ActiveTab.Key == null) return;

            switch (this.Inspect_uTab_ImageList22.ActiveTab.Key)
            {
                case "Inspect_CPK":
                    if (Inspect_ImageList_IplBox.Visible == true) Inspect_ImageList_IplBox.Visible = false;
                    break;
                case "Inspect_ImageAll_Uper":
                    break;
                case "Inspect_ImageFail_Uper":
                    break;
                case "Inspect_ImageAll_Down":
                    break;
                case "Inspect_ImageFail_Down":
                    break;
            }
        }

        private void Chart_DoubleClicking(Infragistics.Win.UltraWinChart.UltraChart doUChart)
        {
            //panel.BringToFront(); panel.SendToBack();
            if (doUChart.Dock.ToString() != "Fill")
            {
                doUChart.Dock = DockStyle.Fill;
                doUChart.BringToFront();

            }
            else doUChart.Dock = DockStyle.None;
        }

        private void uChart_Uper_U01_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U01);
        }

        private void uChart_Uper_U02_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U02);
        }

        private void uChart_Uper_U03_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U03);
        }

        private void uChart_Uper_U04_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U04);
        }

        private void uChart_Uper_U05_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U05);
        }

        private void uChart_Uper_U06_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U06);
        }

        private void uChart_Uper_U07_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U07);
        }

        private void uChart_Uper_U08_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U08);
        }

        private void uChart_Uper_U09_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U09);
        }

        private void uChart_Uper_U10_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Uper_U10);
        }

        private void uChart_Down_U01_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U01);
        }

        private void uChart_Down_U02_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U02);
        }

        private void uChart_Down_U03_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U03);
        }

        private void uChart_Down_U04_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U04);
        }

        private void uChart_Down_U05_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U05);
        }

        private void uChart_Down_U06_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U06);
        }

        private void uChart_Down_U07_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U07);
        }

        private void uChart_Down_U08_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U08);
        }

        private void uChart_Down_U09_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U09);
        }

        private void uChart_Down_U10_DoubleClick(object sender, EventArgs e)
        {
            Chart_DoubleClicking(uChart_Down_U10);
        }

        private void uDS_Inspect_Measure_Uper_CellDataRequested(object sender, CellDataRequestedEventArgs e)
        {

        }

    }
}