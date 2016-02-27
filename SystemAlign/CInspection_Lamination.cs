using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Infragistics.Win.UltraWinDataSource;
using OpenCvSharp;
using System.Data;

namespace SystemAlign
{
    public class CInspection_Lamination
    {
        #region 클래스 멤버

        private bool Model_Changed_Flag = false;

        public bool GetSet_Model_Changed_Flag
        {
            get { return Model_Changed_Flag; }
            set { Model_Changed_Flag = value; }
        }

        public Infragistics.Win.UltraWinDataSource.UltraDataSource uDS_Inspect_MeasureData_Uper;

        public Infragistics.Win.UltraWinDataSource.UltraDataSource GetSet_uDS_Inspect_Measure_Uper
        {
            get
            {
                return uDS_Inspect_MeasureData_Uper;
            }
            set
            {
                uDS_Inspect_MeasureData_Uper = value;
            }
        }

        public Infragistics.Win.UltraWinDataSource.UltraDataSource uDS_Inspect_MeasureData_Down;

        public Infragistics.Win.UltraWinDataSource.UltraDataSource GetSet_uDS_Inspect_Measure_Down
        {
            get { return uDS_Inspect_MeasureData_Down; }
            set { uDS_Inspect_MeasureData_Down = value; }
        }

        public DataTable[] Uper_MeasureTables;

        public DataTable[] GetSet_Uper_MeasureTables
        {
            get { return Uper_MeasureTables; }
            set { Uper_MeasureTables = value; }
        }

        public DataTable[] Down_MeasureTables;
        public DataTable[] GetSet_Down_MeasureTables
        {
            get { return Down_MeasureTables; }
            set { Down_MeasureTables = value; }
        }


        public List<string> StrBufMeasureGridData_Uper = new List<string>();
        public List<string> StrBufMeasureGridName_Uper = new List<string>();
        public string RegPathMeasureGrid_Buf_Uper = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Measurement Data\\MeasureUper_Grid_Buf";

        public List<string> StrBufMeasureGridData_Down = new List<string>();
        public List<string> StrBufMeasureGridName_Down = new List<string>();
        public string RegPathMeasureGrid_Buf_Down = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Measurement Data\\MeasureDown_Grid_Buf";

        public List<string> StrBufMeasureChartData_Uper = new List<string>();
        public List<string> StrBufMeasureChartName_Uper = new List<string>();
        public string RegPathMeasureChart_Buf_Uper = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Measurement Data\\MeasureUper_Chart_Buf";

        public List<string> StrBufMeasureChartData_Down = new List<string>();
        public List<string> StrBufMeasureChartName_Down = new List<string>();
        public string RegPathMeasureChart_Buf_Down = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Measurement Data\\MeasureDown_Chart_Buf";

        public string DCF_File_Path_Gap = string.Empty;
        public string DCF_File_Path_BiCell = string.Empty;

        private bool Grab_Auto_Falg_Gap = false;

        public bool GetSet_Grab_Auto_Flag_Gap
        {
            get { return Grab_Auto_Falg_Gap; }
            set { Grab_Auto_Falg_Gap = value; }
        }

        private bool Grab_Auto_Falg_BiCell = false;

        public bool GetSet_Grab_Auto_Flag_Down
        {
            get { return Grab_Auto_Falg_BiCell; }
            set { Grab_Auto_Falg_BiCell = value; }
        }

        /// <summary>
        /// Calibration Data
        /// </summary>

        public List<string> StrLstMeasureData_Uper = new List<string>();
        public List<string> StrLstMeasureName_Uper = new List<string>();
        public string RegPathMeasure_Uper = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Measurement Data\\MeasureUper";

        public List<string> StrLstMeasureData_TempDown = new List<string>();
        public List<string> StrLstMeasureData_TempUper = new List<string>();

        public List<string> StrLstMeasureData_Down = new List<string>();
        public List<string> StrLstMeasureName_Down = new List<string>();
        public string RegPathMeasure_Down = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Measurement Data\\MeasureDown";

        public List<string> StrLstGapTotalTitle = new List<string>();
        public List<string> StrLstGapTotalData = new List<string>();
        public string RegPathGapTotal = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\GapTotalData";

        public List<string> StrLstCalTitle = new List<string>();
        public List<string> StrLstCalData = new List<string>();
        public string RegPathCal = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Calibration Lami";

        public string[] SystemTimeoutStep = new string[2];
        public string[] SystemTimeoutImaging = new string[2];

        private DateTime _dateUserLoginTime = new DateTime();

        public DateTime GetSet_Now_Login_Time
        {
            get { return _dateUserLoginTime; }
            set { _dateUserLoginTime = value; }
        }

        public bool[] SystemStatusFlag = { false, false, false, false };
        public bool IsConnect_UMAC = false;
        public bool IsConnect_LVS = false;
        public bool IsConnect_PLC = false;


        public int GrabImageSizeGaro_Gap = 4096;
        public int GrabImageSizeSero_Gap = 3072;

        public string[] SystemEdgeParam_Gap = new string[3];
        public string[] SystemLineParam_Gap = new string[3];

        public int GrabImageSizeGaro_BiCell = 4096;
        public int GrabImageSizeSero_BiCell = 3072;

        public string[] SystemEdgeParam_BiCell = new string[3];
        public string[] SystemLineParam_BiCell = new string[3];

        /// <summary>
        /// Equipment Config Data
        /// </summary>
        public List<string> strLstEquipTitle = new List<string>();
        public List<string> strLstEquipName = new List<string>();
        public List<string> strLstEquipData = new List<string>();
        public string RegPathEquip = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Equip Config Lami";

        public static readonly string Account_Operator = "OPERATOR";
        public static readonly string Account_Engineer = "ENGINEER";
        public static readonly string Account_Maker = "MAKER";

        //         public static readonly string Account_Operator = "OP";
        //         public static readonly string Account_Engineer = "ENG";
        //         public static readonly string Account_Maker = "MK";
        public static readonly string Account_Password = "PASSWORD EDIT";

        /// <summary>
        /// Align Status
        /// </summary>RegPathGapStatus
        public string RegPathGapStatus = "Software\\ShinJin M Tec\\LNS System\\System Lamination";
        public List<string> StrLstGapStatusTitle = new List<string>();
        public List<string> StrLstGapStatusData = new List<string>();

        /// <summary>
        /// 시스템 사용 멤버스
        /// </summary>
        public List<string> StrListSysConData = new List<string>();
        public List<string> StrListSysConTitle = new List<string>();
        public List<string> StrListSysConName = new List<string>();
        public string RegPathSysCon = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\System Config Lami";

        /// <summary>
        /// 비전부 사용 멤버스
        /// </summary>
        public List<string> StrListVisConData = new List<string>();
        public List<string> StrListVisConTitle = new List<string>();
        public List<string> StrListVisConName = new List<string>();
        public List<string> StrListVisConGridData_Uper = new List<string>();
        public List<string> StrListVisConGridTitle_Uper = new List<string>();
        public List<string> StrListVisConGridData_Down = new List<string>();
        public List<string> StrListVisConGridTitle_Down = new List<string>();
        public string RegPathVisCon_Lami = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Vision Config Lami";
        public string RegPathVisConGrid_Uper = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Vision Config Lami\\Vision Grid Uper";
        public string RegPathVisConGrid_Down = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Vision Config Lami\\Vision Grid Down";
       
        /// <summary>
        /// 레시피 사용 멤버스  _vListMeasPolarity
        /// </summary>
        public Infragistics.Win.ValueList _vListItemName_Uper = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListItemName_Down = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasMethod = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasDivid = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _imgSideList = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasPola = new Infragistics.Win.ValueList();

        public List<string> StrListRcpConData = new List<string>();
        public List<string> StrListRcpConTitle = new List<string>();
        public List<string> StrLstRcpConName = new List<string>();
        public List<string> StrLstRcpConGridData_Uper = new List<string>();
        public List<string> StrLstRcpConGridTitle_Uper = new List<string>();
        public List<string> StrLstRcpConGridData_Down = new List<string>();
        public List<string> StrLstRcpConGridTitle_Down = new List<string>();

        public string RegPathRcpCon = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Recipe Config Lami";
        public string RegPathRcpConGrid_Uper = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Recipe Config Lami\\Recipe Grid Uper";
        public string RegPathRcpConGrid_Down = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Recipe Config Lami\\Recipe Grid Down";
        public string RegPathRcpConInsp_Uper = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Recipe Config Lami\\Inspect Area Uper";
        public string RegPathRcpConInsp_Down = "Software\\ShinJin M Tec\\LNS System\\System Lamination\\Recipe Config Lami\\Inspect Area Down";

        public List<Rectangle> RectListRecipeBoxZone_Uper = new List<Rectangle>();
        public List<CvRect> RectListImageZone_Uper = new List<CvRect>();
        public List<Rectangle> RectListInspBoxZone_Uper = new List<Rectangle>();

        public List<Rectangle> RectListRecipeBoxZone_Down = new List<Rectangle>();
        public List<CvRect> RectListImageZone_Down = new List<CvRect>();
        public List<Rectangle> RectListInspBoxZone_Down = new List<Rectangle>();

        private float _floatSystemInspectZoomX_Uper = 0f;
        private float _floatSystemInspectZoomY_Uper = 0f;
        public float GetSet_System_Inspect_Zoom_X_Uper
        {
            get { return _floatSystemInspectZoomX_Uper; }
            set { _floatSystemInspectZoomX_Uper = value; }
        }

        public float GetSet_System_Inspect_Zoom_Y_Uper
        {
            get { return _floatSystemInspectZoomY_Uper; }
            set { _floatSystemInspectZoomY_Uper = value; }
        }

        private float _floatSystemInspectZoomX_Down = 0f;
        private float _floatSystemInspectZoomY_Down = 0f;
        public float GetSet_System_Inspect_Zoom_X_Down
        {
            get { return _floatSystemInspectZoomX_Down; }
            set { _floatSystemInspectZoomX_Down = value; }
        }

        public float GetSet_System_Inspect_Zoom_Y_Down
        {
            get { return _floatSystemInspectZoomY_Down; }
            set { _floatSystemInspectZoomY_Down = value; }
        }

        public List<string> StrLstRcpConInspData_Uper = new List<string>();
        public List<string> StrLstRcpConInspTitle_Uper = new List<string>();

        private float _floatSystemStatusZoomX_Uper = 0f;
        private float _floatSystemStatusZoomY_Uper = 0f;
        public float GetSet_System_Status_Zoom_X_Uper
        {
            get { return _floatSystemStatusZoomX_Uper; }
            set { _floatSystemStatusZoomX_Uper = value; }
        }

        public float GetSet_System_Status_Zoom_Y_Uper
        {
            get { return _floatSystemStatusZoomY_Uper; }
            set { _floatSystemStatusZoomY_Uper = value; }
        }

        public List<string> StrLstRcpConInspData_Down = new List<string>();
        public List<string> StrLstRcpConInspTitle_Down = new List<string>();

        private float _floatSystemStatusZoomX_Down = 0f;
        private float _floatSystemStatusZoomY_Down = 0f;
        public float GetSet_System_Status_Zoom_X_Down
        {
            get { return _floatSystemStatusZoomX_Down; }
            set { _floatSystemStatusZoomX_Down = value; }
        }

        public float GetSet_System_Status_Zoom_Y_Down
        {
            get { return _floatSystemStatusZoomY_Down; }
            set { _floatSystemStatusZoomY_Down = value; }
        }

        public string RegPathSystemStatus = "Software\\ShinJin M Tec\\LNS System\\System Lamination";

        private string _strNowAccount = string.Empty;
        private string _strNowModelName = string.Empty;
        private string _strNowModelNumber = string.Empty;
        private string _strNowUserId = string.Empty;
        private string _strNowUserPass = string.Empty;

        public string GetSet_Now_User_Account
        {
            get { return _strNowAccount; }
            set { _strNowAccount = value; }
        }

        public string GetSet_Now_Model_Name
        {
            get { return _strNowModelName; }
            set { _strNowModelName = value; }
        }

        public string GetSet_Now_Model_Number
        {
            get { return _strNowModelNumber; }
            set { _strNowModelNumber = value; }
        }
        public string GetSet_Now_User_ID
        {
            get { return _strNowUserId; }
            set { _strNowUserId = value; }
        }

        public string GetSet_Now_User_Pass
        {
            get { return _strNowUserPass; }
            set { _strNowUserPass = value; }
        }
        #endregion
    }
}
