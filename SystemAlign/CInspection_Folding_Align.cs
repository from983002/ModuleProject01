using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace SystemAlign
{
    // Bitmap to IplImage
	//IplImage iplOriginal = (OpenCvSharp.IplImage)BitmapConverter.ToIplImage(beforeBitmap);

	// IplImage to Bitmap
	//Bitmap afterBitmap = BitmapConverter.ToBitmap(iplOriginal);

    //Bitmap img1 = new Bitmap(fname);
    //pictureBoxIpl1.ImageIpl = (OpenCvSharp.IplImage) BitmapConverter.ToIplImage(img1);
    //Bitmap img2 = BitmapConverter.ToBitmap(pictureBoxIpl1.ImageIpl);
    //this.panel1.BackgroundImage = img2;
    
    public class CInspection_Folding_Align
    {
        #region 클래스 멤버

        public int GrabImageSizeGaro = 2352;
        public int GrabImageSizeSero = 1728;

        public string[] SystemEdgeParam = new string[3];
        public string[] SystemLineParam = new string[3];

        public string[] SystemTimeoutStep = new string[2];
        public string[] SystemTimeoutImaging = new string[2];

        public bool[] SystemStatusFlag = {false, false, false, false};
        
        public bool IsConnect_UMAC = false;
        public bool IsConnect_LVS = false;
        public bool IsConnect_PLC = false;
        /// <summary>
        /// Equipment Config Data
        /// </summary>
        public List<string> strLstEquipTitle = new List<string>();
        public List<string> strLstEquipName = new List<string>();
        public List<string> strLstEquipData = new List<string>();
        public string RegPathEquip = "Software\\ShinJin M Tec\\LNS System\\System Align\\Equip Config";

        public static readonly string Account_Operator = "OPERATOR";
        public static readonly string Account_Engineer = "ENGINEER";
        public static readonly string Account_Maker = "MAKER";

//         public static readonly string Account_Operator = "OP";
//         public static readonly string Account_Engineer = "ENG";
//         public static readonly string Account_Maker = "MK";
        public static readonly string Account_Password = "PASSWORD EDIT";

        /// <summary>
        /// Calibration Data
        /// </summary>
        public string RegPathAlignStatus = "Software\\ShinJin M Tec\\LNS System\\System Align";
        public List<string> strLstAlignStatusTitle = new List<string>();
        public List<string> strLstAlignStatusData = new List<string>();
        /// <summary>
        /// Calibration Data
        /// </summary>
        public List<string> StrLstCalTitle = new List<string>();
        public List<string> StrLstCalData = new List<string>();
        public string RegPathCal = "Software\\ShinJin M Tec\\LNS System\\System Align\\Calibration";

        /// <summary>
        /// 시스템 사용 멤버스
        /// </summary>
        public List<string> StrListSysConData = new List<string>();
        public List<string> StrListSysConTitle = new List<string>();
        public List<string> StrListSysConName = new List<string>();
        public  string RegPathSysCon = "Software\\ShinJin M Tec\\LNS System\\System Align\\System Config";

        /// <summary>
        /// 비전부 사용 멤버스
        /// </summary>
        public List<string> StrListVisConData = new List<string>();
        public List<string> StrListVisConTitle = new List<string>();
        public List<string> StrListVisConName = new List<string>();
        public List<string> StrListVisConGridData = new List<string>();
        public List<string> StrListVisConGridTitle = new List<string>();
        public  string RegPathVisCon = "Software\\ShinJin M Tec\\LNS System\\System Align\\Vision Config";
        public  string RegPathVisConGrid = "Software\\ShinJin M Tec\\LNS System\\System Align\\Vision Config\\Vision Grid";


        /// <summary>
        /// 레시피 사용 멤버스  _vListMeasPolarity
        /// </summary>

        public Infragistics.Win.ValueList _vListMeasMethod = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasDivid = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _imgSideList = new Infragistics.Win.ValueList();
        public Infragistics.Win.ValueList _vListMeasPola = new Infragistics.Win.ValueList();
        
        public List<string> StrListRcpConData = new List<string>();
        public List<string> StrListRcpConTitle = new List<string>();
        public List<string> StrLstRcpConName = new List<string>();
        public List<string> StrLstRcpConGridData = new List<string>();
        public List<string> StrLstRcpConGridTitle = new List<string>();
        
        public string RegPathRcpCon = "Software\\ShinJin M Tec\\LNS System\\System Align\\Recipe Config";
        public string RegPathRcpConGrid = "Software\\ShinJin M Tec\\LNS System\\System Align\\Recipe Config\\Recipe Grid";
        public string RegPathRcpConInsp = "Software\\ShinJin M Tec\\LNS System\\System Align\\Recipe Config\\Inspect Area";

        public List<Rectangle> RectListRecipeBoxZone = new List<Rectangle>();
        public List<CvRect> RectListImageZone = new List<CvRect>();
        public List<Rectangle> RectListInspBoxZone = new List<Rectangle>();

        private float _floatSystemInspectZoomX = 0f;
        private float _floatSystemInspectZoomY = 0f;
        public float GetSet_System_Inspect_Zoom_X
        {
            get { return _floatSystemInspectZoomX; }
            set { _floatSystemInspectZoomX = value; }
        }

        public float GetSet_System_Inspect_Zoom_Y
        {
            get { return _floatSystemInspectZoomY; }
            set { _floatSystemInspectZoomY = value; }
        }
  
        public List<string> StrLstRcpConInspData = new List<string>();
        public List<string> StrLstRcpConInspTitle = new List<string>();

        private float _floatSystemStatusZoomX = 0f;
        private float _floatSystemStatusZoomY = 0f;
        public float GetSet_System_Status_Zoom_X
        {
            get { return _floatSystemStatusZoomX; }
            set { _floatSystemStatusZoomX = value; }
        }

        public float GetSet_System_Status_Zoom_Y
        {
            get { return _floatSystemStatusZoomY; }
            set { _floatSystemStatusZoomY = value; }
        }

        private DateTime _dateUserLoginTime = new DateTime();

        public DateTime GetSet_Now_Login_Time
        {
            get { return _dateUserLoginTime; }
            set { _dateUserLoginTime = value; }
        }

        
        public string RegPathSystemStatus = "Software\\ShinJin M Tec\\LNS System\\System Align";

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
