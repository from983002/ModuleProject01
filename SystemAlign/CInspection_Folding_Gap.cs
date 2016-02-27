using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace SystemAlign
{
    public class CInspection_Folding_Gap
    {
        #region 클래스 멤버

        public string DCF_File_Path_Gap = string.Empty;
        public string DCF_File_Path_BiCell = string.Empty;

        private bool Grab_Auto_Falg_Gap = false;

        public bool GetSet_Grab_Auto_Flag_Gap
        {
            get { return Grab_Auto_Falg_Gap; }
            set { Grab_Auto_Falg_Gap = value; }
        }

        private bool Grab_Auto_Falg_BiCell = false;

        public bool GetSet_Grab_Auto_Flag_BiCell
        {
            get { return Grab_Auto_Falg_BiCell; }
            set { Grab_Auto_Falg_BiCell = value; }
        }

        /// <summary>
        /// Calibration Data
        /// </summary>

        public List<string> StrLstGapTotalTitle = new List<string>();
        public List<string> StrLstGapTotalData = new List<string>();
        public string RegPathGapTotal = "Software\\ShinJin M Tec\\LNS System\\System Gap\\GapTotalData";

        public List<string> StrLstCalTitle = new List<string>();
        public List<string> StrLstCalData = new List<string>();
        public string RegPathCal = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Calibration";

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
        public string RegPathEquip = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Equip Config";

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
        public string RegPathGapStatus = "Software\\ShinJin M Tec\\LNS System\\System Gap";
        public List<string> StrLstGapStatusTitle = new List<string>();
        public List<string> StrLstGapStatusData = new List<string>();
       
        /// <summary>
        /// 시스템 사용 멤버스
        /// </summary>
        public List<string> StrListSysConData = new List<string>();
        public List<string> StrListSysConTitle = new List<string>();
        public List<string> StrListSysConName = new List<string>();
        public string RegPathSysCon = "Software\\ShinJin M Tec\\LNS System\\System Gap\\System Config";

        /// <summary>
        /// 비전부 사용 멤버스
        /// </summary>
        public List<string> StrListVisConData_Gap = new List<string>();
        public List<string> StrListVisConTitle_Gap = new List<string>();
        public List<string> StrListVisConName_Gap = new List<string>();
        public List<string> StrListVisConGridData_Uper = new List<string>();
        public List<string> StrListVisConGridTitle_Gap = new List<string>();
        public string RegPathVisCon_Lami = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Vision Config Lami";
        public string RegPathVisConGrid_Uper = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Vision Config Lami\\Vision Grid Uper";

        /// <summary>
        /// 비전부 사용 멤버스
        /// </summary>
        public List<string> StrListVisConData_BiCell = new List<string>();
        public List<string> StrListVisConTitle_BiCell = new List<string>();
        public List<string> StrListVisConName_BiCell = new List<string>();
        public List<string> StrListVisConGridData_Down = new List<string>();
        public List<string> StrListVisConGridTitle_Down = new List<string>();
        public string RegPathVisCon_BiCell = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Vision Config BiCell";
        public string RegPathVisConGrid_BiCell = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Vision Config BiCell\\Vision Grid BiCell";

        /// <summary>
        /// 레시피 사용 멤버스  _vListMeasPolarity
        /// </summary>

        public Infragistics.Win.ValueList _vListMeasMethod_Gap = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasDivid_Gap = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _imgSideList_Gap = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasPola_Gap = new Infragistics.Win.ValueList();

        public List<string> StrListRcpConData_Gap = new List<string>();
        public List<string> StrListRcpConTitle_Gap = new List<string>();
        public List<string> StrLstRcpConName_Gap = new List<string>();
        public List<string> StrLstRcpConGridData_Gap = new List<string>();
        public List<string> StrLstRcpConGridTitle_Gap = new List<string>();

        public string RegPathRcpCon_Gap = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Recipe Config Gap";
        public string RegPathRcpConGrid_Gap = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Recipe Config Gap\\Recipe Grid Gap";
        public string RegPathRcpConInsp_Gap = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Recipe Config Gap\\Inspect Area Gap";

        public List<Rectangle> RectListRecipeBoxZone_Gap = new List<Rectangle>();
        public List<CvRect> RectListImageZone_Gap = new List<CvRect>();
        public List<Rectangle> RectListInspBoxZone_Gap = new List<Rectangle>();

        /// <summary>
        /// 레시피 바이셀 사용멤버들
        /// 레시피 사용 멤버스  _vListMeasPolarity
        /// </summary>

        public Infragistics.Win.ValueList _vListMeasMethod_BiCell = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasDivid_BiCell = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _imgSideList_BiCell = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasPola_BiCell = new Infragistics.Win.ValueList();

        public List<string> StrListRcpConData_BiCell = new List<string>();
        public List<string> StrListRcpConTitle_BiCell = new List<string>();
        public List<string> StrLstRcpConName_BiCell = new List<string>();
        public List<string> StrLstRcpConGridData_BiCell = new List<string>();
        public List<string> StrLstRcpConGridTitle_BiCell = new List<string>();

        public string RegPathRcpCon_BiCell = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Recipe Config BiCell";
        public string RegPathRcpConGrid_BiCell = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Recipe Config BiCell\\Recipe Grid BiCell";
        public string RegPathRcpConInsp_BiCell = "Software\\ShinJin M Tec\\LNS System\\System Gap\\Recipe Config BiCell\\Inspect Area BiCell";

        public List<Rectangle> RectListRecipeBoxZone_BiCell = new List<Rectangle>();
        public List<CvRect> RectListImageZone_BiCell = new List<CvRect>();
        public List<Rectangle> RectListInspBoxZone_BiCell = new List<Rectangle>();

        private float _floatSystemInspectZoomX_Gap = 0f;
        private float _floatSystemInspectZoomY_Gap = 0f;
        public float GetSet_System_Inspect_Zoom_X_Gap
        {
            get { return _floatSystemInspectZoomX_Gap; }
            set { _floatSystemInspectZoomX_Gap = value; }
        }

        public float GetSet_System_Inspect_Zoom_Y_Gap
        {
            get { return _floatSystemInspectZoomY_Gap; }
            set { _floatSystemInspectZoomY_Gap = value; }
        }

        private float _floatSystemInspectZoomX_BiCell = 0f;
        private float _floatSystemInspectZoomY_BiCell = 0f;
        public float GetSet_System_Inspect_Zoom_X_BiCell
        {
            get { return _floatSystemInspectZoomX_BiCell; }
            set { _floatSystemInspectZoomX_BiCell = value; }
        }

        public float GetSet_System_Inspect_Zoom_Y_BiCell
        {
            get { return _floatSystemInspectZoomY_BiCell; }
            set { _floatSystemInspectZoomY_BiCell = value; }
        }

        public List<string> StrLstRcpConInspData_Gap = new List<string>();
        public List<string> StrLstRcpConInspTitle_Gap = new List<string>();

        private float _floatSystemStatusZoomX_Gap = 0f;
        private float _floatSystemStatusZoomY_Gap = 0f;
        public float GetSet_System_Status_Zoom_X_Gap
        {
            get { return _floatSystemStatusZoomX_Gap; }
            set { _floatSystemStatusZoomX_Gap = value; }
        }

        public float GetSet_System_Status_Zoom_Y_Gap
        {
            get { return _floatSystemStatusZoomY_Gap; }
            set { _floatSystemStatusZoomY_Gap = value; }
        }

        public List<string> StrLstRcpConInspData_BiCell = new List<string>();
        public List<string> StrLstRcpConInspTitle_BiCell = new List<string>();

        private float _floatSystemStatusZoomX_BiCell = 0f;
        private float _floatSystemStatusZoomY_BiCell = 0f;
        public float GetSet_System_Status_Zoom_X_BiCell
        {
            get { return _floatSystemStatusZoomX_BiCell; }
            set { _floatSystemStatusZoomX_BiCell = value; }
        }

        public float GetSet_System_Status_Zoom_Y_BiCell
        {
            get { return _floatSystemStatusZoomY_BiCell; }
            set { _floatSystemStatusZoomY_BiCell = value; }
        }

        public string RegPathSystemStatus = "Software\\ShinJin M Tec\\LNS System\\System Gap";

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
