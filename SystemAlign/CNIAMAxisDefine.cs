///////////////////////////////////// INFO //////////////////////////////////
// by Ki Back Woo in Nexstart R&D center
// emergency : wkb@nexstar21.com
// MSN : 
/////////////////////////////////////////////////////////////////////////////

// NSIARAxisDefine.cs: interface for the CNSIARAxisDefine class.
//
//////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace SystemAlign
{
    static class CNIAMAxisDefine
    {
        public const int NFLI_AXIS_MODULE_CNT = 1;
        public const int NFLI_AXIS_MODULE_IDX1 = 0;
        public const int NFLI_AXIS_CNT_MODULE_IDX1 = 21;
        public const int NFLI_AXIS_CNT = 21;
        public const int NFLI_AXIS_IDX_LOADER_MAGAZINE_Z = 0;

        public const int NFLI_AXIS_IDX_UNLOADER_MAGAZINE_Z = 1;
        public const int NFLI_AXIS_IDX_PASS_MAGAZINE_Z = 2;
        public const int NFLI_AXIS_IDX_FAIL_MAGAZINE_Z = 3;
        public const int NFLI_AXIS_IDX_SORTER_X = 4;
        public const int NFLI_AXIS_IDX_SORTER_Y = 5;
        public const int NFLI_AXIS_IDX_PALLET_X = 6;
        public const int NFLI_AXIS_IDX_SUPPLY_Y = 7;
        public const int NFLI_AXIS_IDX_INDEX_T = 8;
        public const int NFLI_AXIS_IDX_OVER_CAM1_Z = 9;
        public const int NFLI_AXIS_IDX_OVER_CAM2_Z = 10;
        public const int NFLI_AXIS_IDX_OVER_CAM3_Z = 11;
        public const int NFLI_AXIS_IDX_UNDER_CAM1_Z = 12;
        public const int NFLI_AXIS_IDX_UNDER_CAM2_Z = 13;
        public const int NFLI_AXIS_IDX_UNDER_CAM3_Z = 14;
        public const int NFLI_AXIS_IDX_OVER_LIGHT1_Z = 15;
        public const int NFLI_AXIS_IDX_OVER_LIGHT2_Z = 16;
        public const int NFLI_AXIS_IDX_OVER_LIGHT3_Z = 17;
        public const int NFLI_AXIS_IDX_UNDER_LIGHT1_Z = 18;
        public const int NFLI_AXIS_IDX_UNDER_LIGHT2_Z = 19;
        public const int NFLI_AXIS_IDX_UNDER_LIGHT3_Z = 20;

        public const int NFLI_AXIS_EZI_MOTION_CNT = 6;
        public const int NFLI_AXIS_IDX_EZI_OVER_LIGHT1_Z = 0;
        public const int NFLI_AXIS_IDX_EZI_OVER_LIGHT2_Z = 1;
        public const int NFLI_AXIS_IDX_EZI_OVER_LIGHT3_Z = 2;
        public const int NFLI_AXIS_IDX_EZI_UNDER_LIGHT1_Z = 3;
        public const int NFLI_AXIS_IDX_EZI_UNDER_LIGHT2_Z = 4;
        public const int NFLI_AXIS_IDX_EZI_UNDER_LIGHT3_Z = 5;


        public const int NFLI_AXIS_SET_NONE = 0X00000000;

        public const int NFLI_AXIS_SET_LOADER_MAGAZINE_Z = 0X00000001;
        public const int NFLI_AXIS_SET_UNLOADER_MAGAZINE_Z = 0X00000002;
        public const int NFLI_AXIS_SET_PASS_MAGAZINE_Z = 0X00000004;
        public const int NFLI_AXIS_SET_FAIL_MAGAZINE_Z = 0X00000008;
        public const int NFLI_AXIS_SET_SORTER_X = 0X00000010;
        public const int NFLI_AXIS_SET_SORTER_Y = 0X00000020;
        public const int NFLI_AXIS_SET_PALLET_X = 0X00000040;
        public const int NFLI_AXIS_SET_SUPPLY_Y = 0X00000080;
        public const int NFLI_AXIS_SET_INDEX_T = 0X00000100;
        public const int NFLI_AXIS_SET_OVER_CAM1_Z = 0X00000200;
        public const int NFLI_AXIS_SET_OVER_CAM2_Z = 0X00000400;
        public const int NFLI_AXIS_SET_OVER_CAM3_Z = 0X00000800;
        public const int NFLI_AXIS_SET_UNDER_CAM1_Z = 0X00001000;
        public const int NFLI_AXIS_SET_UNDER_CAM2_Z = 0X00002000;
        public const int NFLI_AXIS_SET_UNDER_CAM3_Z = 0X00004000;
        public const int NFLI_AXIS_SET_OVER_LIGHT1_Z = 0X00008000;
        public const int NFLI_AXIS_SET_OVER_LIGHT2_Z = 0X00010000;
        public const int NFLI_AXIS_SET_OVER_LIGHT3_Z = 0X00020000;
        public const int NFLI_AXIS_SET_UNDER_LIGHT1_Z = 0X00040000;
        public const int NFLI_AXIS_SET_UNDER_LIGHT2_Z = 0X00080000;
        public const int NFLI_AXIS_SET_UNDER_LIGHT3_Z = 0X00100000;
        public const int NFLI_AXIS_SET_ALL = 0X001FFFFF;

        public const int NFLI_AXIS_IDX1 = NFLI_AXIS_IDX_LOADER_MAGAZINE_Z;
        public const int NFLI_AXIS_IDX2 = NFLI_AXIS_IDX_UNLOADER_MAGAZINE_Z;
        public const int NFLI_AXIS_IDX3 = NFLI_AXIS_IDX_PASS_MAGAZINE_Z;
        public const int NFLI_AXIS_IDX4 = NFLI_AXIS_IDX_FAIL_MAGAZINE_Z;
        public const int NFLI_AXIS_IDX5 = NFLI_AXIS_IDX_SORTER_X;
        public const int NFLI_AXIS_IDX6 = NFLI_AXIS_IDX_SORTER_Y;
        public const int NFLI_AXIS_IDX7 = NFLI_AXIS_IDX_PALLET_X;
        public const int NFLI_AXIS_IDX8 = NFLI_AXIS_IDX_SUPPLY_Y;
        public const int NFLI_AXIS_IDX9 = NFLI_AXIS_IDX_INDEX_T;
        public const int NFLI_AXIS_IDX10 = NFLI_AXIS_IDX_OVER_CAM1_Z;
        public const int NFLI_AXIS_IDX11 = NFLI_AXIS_IDX_OVER_CAM2_Z;
        public const int NFLI_AXIS_IDX12 = NFLI_AXIS_IDX_OVER_CAM3_Z;
        public const int NFLI_AXIS_IDX13 = NFLI_AXIS_IDX_UNDER_CAM1_Z;
        public const int NFLI_AXIS_IDX14 = NFLI_AXIS_IDX_UNDER_CAM2_Z;
        public const int NFLI_AXIS_IDX15 = NFLI_AXIS_IDX_UNDER_CAM3_Z;
        public const int NFLI_AXIS_IDX16 = NFLI_AXIS_IDX_OVER_LIGHT1_Z;
        public const int NFLI_AXIS_IDX17 = NFLI_AXIS_IDX_OVER_LIGHT2_Z;
        public const int NFLI_AXIS_IDX18 = NFLI_AXIS_IDX_OVER_LIGHT3_Z;
        public const int NFLI_AXIS_IDX19 = NFLI_AXIS_IDX_UNDER_LIGHT1_Z;
        public const int NFLI_AXIS_IDX20 = NFLI_AXIS_IDX_UNDER_LIGHT2_Z;
        public const int NFLI_AXIS_IDX21 = NFLI_AXIS_IDX_UNDER_LIGHT3_Z;

        struct AXISUNIT
        {
            int m_iModuleIdx;
	        int m_iAxisNo;
	        int m_iAxisIdx;
        }
        //tagAxisUnit AXISUNIT;
        //tagAxisUnit &PAXISUNIT;

        public const string NFLI_AXIS_DEF_AXIS_NO_SECTION = "NFLI_AXIS_DEF_AXIS_NO";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_NO_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_NO = 0;
        public const int NFLI_AXIS_DEF_AXIS2_NO = 1;
        public const int NFLI_AXIS_DEF_AXIS3_NO = 2;
        public const int NFLI_AXIS_DEF_AXIS4_NO = 3;
        public const int NFLI_AXIS_DEF_AXIS5_NO = 4;
        public const int NFLI_AXIS_DEF_AXIS6_NO = 5;
        public const int NFLI_AXIS_DEF_AXIS7_NO = 6;
        public const int NFLI_AXIS_DEF_AXIS8_NO = 7;
        public const int NFLI_AXIS_DEF_AXIS9_NO = 8;
        public const int NFLI_AXIS_DEF_AXIS10_NO = 9;
        public const int NFLI_AXIS_DEF_AXIS11_NO = 10;
        public const int NFLI_AXIS_DEF_AXIS12_NO = 11;
        public const int NFLI_AXIS_DEF_AXIS13_NO = 12;
        public const int NFLI_AXIS_DEF_AXIS14_NO = 13;
        public const int NFLI_AXIS_DEF_AXIS15_NO = 14;
        public const int NFLI_AXIS_DEF_AXIS16_NO = 15;
        public const int NFLI_AXIS_DEF_AXIS17_NO = 16;
        public const int NFLI_AXIS_DEF_AXIS18_NO = 17;
        public const int NFLI_AXIS_DEF_AXIS19_NO = 18;
        public const int NFLI_AXIS_DEF_AXIS20_NO = 19;
        public const int NFLI_AXIS_DEF_AXIS21_NO = 20;

        static string[] s_szArrayAxisDefEntryAxisNo = {    
        "AXIS1_NO",	    "AXIS2_NO",		"AXIS3_NO", 	"AXIS4_NO", 	"AXIS5_NO",
 	    "AXIS6_NO",		"AXIS7_NO",		"AXIS8_NO", 	"AXIS9_NO", 	"AXIS10_NO",
        "AXIS11_NO",	"AXIS12_NO",	"AXIS13_NO", 	"AXIS14_NO", 	"AXIS15_NO",
        "AXIS16_NO", 	"AXIS17_NO", 	"AXIS18_NO", 	"AXIS19_NO", 	"AXIS20_NO",
        "AXIS21_NO"     };

        public const string NFLI_AXIS_DEF_AXIS_CNT_PER_MM_SECTION = "NFLI_AXIS_DEF_AXIS_CNT_PER_MM";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_CNT_PER_MM_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_CNT_PER_MM = 0;
        public const int NFLI_AXIS_DEF_AXIS2_CNT_PER_MM = 1;
        public const int NFLI_AXIS_DEF_AXIS3_CNT_PER_MM = 2;
        public const int NFLI_AXIS_DEF_AXIS4_CNT_PER_MM = 3;
        public const int NFLI_AXIS_DEF_AXIS5_CNT_PER_MM = 4;
        public const int NFLI_AXIS_DEF_AXIS6_CNT_PER_MM = 5;
        public const int NFLI_AXIS_DEF_AXIS7_CNT_PER_MM = 6;
        public const int NFLI_AXIS_DEF_AXIS8_CNT_PER_MM = 7;
        public const int NFLI_AXIS_DEF_AXIS9_CNT_PER_MM = 8;
        public const int NFLI_AXIS_DEF_AXIS10_CNT_PER_MM = 9;
        public const int NFLI_AXIS_DEF_AXIS11_CNT_PER_MM = 10;
        public const int NFLI_AXIS_DEF_AXIS12_CNT_PER_MM = 11;
        public const int NFLI_AXIS_DEF_AXIS13_CNT_PER_MM = 12;
        public const int NFLI_AXIS_DEF_AXIS14_CNT_PER_MM = 13;
        public const int NFLI_AXIS_DEF_AXIS15_CNT_PER_MM = 14;
        public const int NFLI_AXIS_DEF_AXIS16_CNT_PER_MM = 15;
        public const int NFLI_AXIS_DEF_AXIS17_CNT_PER_MM = 16;
        public const int NFLI_AXIS_DEF_AXIS18_CNT_PER_MM = 17;
        public const int NFLI_AXIS_DEF_AXIS19_CNT_PER_MM = 18;
        public const int NFLI_AXIS_DEF_AXIS20_CNT_PER_MM = 19;
        public const int NFLI_AXIS_DEF_AXIS21_CNT_PER_MM = 20;

        static string[] s_szArrayAxisDefEntryAxisCntPerMM = {    
        "AXIS1_CNT_PER_MM",	"AXIS2_CNT_PER_MM", "AXIS3_CNT_PER_MM", "AXIS4_CNT_PER_MM", "AXIS5_CNT_PER_MM",
 	    "AXIS6_CNT_PER_MM", "AXIS7_CNT_PER_MM",	"AXIS8_CNT_PER_MM", "AXIS9_CNT_PER_MM", "AXIS10_CNT_PER_MM",
        "AXIS11_CNT_PER_MM","AXIS12_CNT_PER_MM","AXIS13_CNT_PER_MM","AXIS14_CNT_PER_MM","AXIS15_CNT_PER_MM",
        "AXIS16_CNT_PER_MM","AXIS17_CNT_PER_MM","AXIS18_CNT_PER_MM","AXIS19_CNT_PER_MM","AXIS20_CNT_PER_MM",
        "AXIS21_CNT_PER_MM" };

        public const string NFLI_AXIS_DEF_AXIS_MIN_POS_SECTION = "NFLI_AXIS_DEF_AXIS_MIN_POS";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_MIN_POS_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_MIN_POS = 0;
        public const int NFLI_AXIS_DEF_AXIS2_MIN_POS = 1;
        public const int NFLI_AXIS_DEF_AXIS3_MIN_POS = 2;
        public const int NFLI_AXIS_DEF_AXIS4_MIN_POS = 3;
        public const int NFLI_AXIS_DEF_AXIS5_MIN_POS = 4;
        public const int NFLI_AXIS_DEF_AXIS6_MIN_POS = 5;
        public const int NFLI_AXIS_DEF_AXIS7_MIN_POS = 6;
        public const int NFLI_AXIS_DEF_AXIS8_MIN_POS = 7;
        public const int NFLI_AXIS_DEF_AXIS9_MIN_POS = 8;
        public const int NFLI_AXIS_DEF_AXIS10_MIN_POS = 9;
        public const int NFLI_AXIS_DEF_AXIS11_MIN_POS = 10;
        public const int NFLI_AXIS_DEF_AXIS12_MIN_POS = 11;
        public const int NFLI_AXIS_DEF_AXIS13_MIN_POS = 12;
        public const int NFLI_AXIS_DEF_AXIS14_MIN_POS = 13;
        public const int NFLI_AXIS_DEF_AXIS15_MIN_POS = 14;
        public const int NFLI_AXIS_DEF_AXIS16_MIN_POS = 15;
        public const int NFLI_AXIS_DEF_AXIS17_MIN_POS = 16;
        public const int NFLI_AXIS_DEF_AXIS18_MIN_POS = 17;
        public const int NFLI_AXIS_DEF_AXIS19_MIN_POS = 18;
        public const int NFLI_AXIS_DEF_AXIS20_MIN_POS = 19;
        public const int NFLI_AXIS_DEF_AXIS21_MIN_POS = 20;

        static string[] s_szArrayAxisDefEntryAxisMinPos = {    
        "AXIS1_MIN_POS",	"AXIS2_MIN_POS", "AXIS3_MIN_POS", "AXIS4_MIN_POS",  "AXIS5_MIN_POS",
 	    "AXIS6_MIN_POS",	"AXIS7_MIN_POS", "AXIS8_MIN_POS", "AXIS9_MIN_POS",  "AXIS10_MIN_POS", 
        "AXIS11_MIN_POS","AXIS12_MIN_POS","AXIS13_MIN_POS","AXIS14_MIN_POS","AXIS15_MIN_POS",
        "AXIS16_MIN_POS","AXIS17_MIN_POS","AXIS18_MIN_POS","AXIS19_MIN_POS","AXIS20_MIN_POS",
        "AXIS21_MIN_POS" };

        public const string NFLI_AXIS_DEF_AXIS_MAX_POS_SECTION = "NFLI_AXIS_DEF_AXIS_MAX_POS";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_MAX_POS_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_MAX_POS = 0;
        public const int NFLI_AXIS_DEF_AXIS2_MAX_POS = 1;
        public const int NFLI_AXIS_DEF_AXIS3_MAX_POS = 2;
        public const int NFLI_AXIS_DEF_AXIS4_MAX_POS = 3;
        public const int NFLI_AXIS_DEF_AXIS5_MAX_POS = 4;
        public const int NFLI_AXIS_DEF_AXIS6_MAX_POS = 5;
        public const int NFLI_AXIS_DEF_AXIS7_MAX_POS = 6;
        public const int NFLI_AXIS_DEF_AXIS8_MAX_POS = 7;
        public const int NFLI_AXIS_DEF_AXIS9_MAX_POS = 8;
        public const int NFLI_AXIS_DEF_AXIS10_MAX_POS = 9;
        public const int NFLI_AXIS_DEF_AXIS11_MAX_POS = 10;
        public const int NFLI_AXIS_DEF_AXIS12_MAX_POS = 11;
        public const int NFLI_AXIS_DEF_AXIS13_MAX_POS = 12;
        public const int NFLI_AXIS_DEF_AXIS14_MAX_POS = 13;
        public const int NFLI_AXIS_DEF_AXIS15_MAX_POS = 14;
        public const int NFLI_AXIS_DEF_AXIS16_MAX_POS = 15;
        public const int NFLI_AXIS_DEF_AXIS17_MAX_POS = 16;
        public const int NFLI_AXIS_DEF_AXIS18_MAX_POS = 17;
        public const int NFLI_AXIS_DEF_AXIS19_MAX_POS = 18;
        public const int NFLI_AXIS_DEF_AXIS20_MAX_POS = 19;
        public const int NFLI_AXIS_DEF_AXIS21_MAX_POS = 20;

        static string[] s_szArrayAxisDefEntryAxisMaxPos = {    
        "AXIS1_MAX_POS",	"AXIS2_MAX_POS", "AXIS3_MAX_POS", "AXIS4_MAX_POS",  "AXIS5_MAX_POS",
 	    "AXIS6_MAX_POS",	"AXIS7_MAX_POS", "AXIS8_MAX_POS", "AXIS9_MAX_POS",  "AXIS10_MAX_POS", 
        "AXIS11_MAX_POS","AXIS12_MAX_POS","AXIS13_MAX_POS","AXIS14_MAX_POS","AXIS15_MAX_POS",
        "AXIS16_MAX_POS","AXIS17_MAX_POS","AXIS18_MAX_POS","AXIS19_MAX_POS","AXIS20_MAX_POS",
        "AXIS21_MAX_POS" };

        public const string NFLI_AXIS_DEF_AXIS_MV_STEP_SECTION = "NFLI_AXIS_DEF_AXIS_MV_STEP";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_MV_STEP_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_MV_STEP = 0;
        public const int NFLI_AXIS_DEF_AXIS2_MV_STEP = 1;
        public const int NFLI_AXIS_DEF_AXIS3_MV_STEP = 2;
        public const int NFLI_AXIS_DEF_AXIS4_MV_STEP = 3;
        public const int NFLI_AXIS_DEF_AXIS5_MV_STEP = 4;
        public const int NFLI_AXIS_DEF_AXIS6_MV_STEP = 5;
        public const int NFLI_AXIS_DEF_AXIS7_MV_STEP = 6;
        public const int NFLI_AXIS_DEF_AXIS8_MV_STEP = 7;
        public const int NFLI_AXIS_DEF_AXIS9_MV_STEP = 8;
        public const int NFLI_AXIS_DEF_AXIS10_MV_STEP = 9;
        public const int NFLI_AXIS_DEF_AXIS11_MV_STEP = 10;
        public const int NFLI_AXIS_DEF_AXIS12_MV_STEP = 11;
        public const int NFLI_AXIS_DEF_AXIS13_MV_STEP = 12;
        public const int NFLI_AXIS_DEF_AXIS14_MV_STEP = 13;
        public const int NFLI_AXIS_DEF_AXIS15_MV_STEP = 14;
        public const int NFLI_AXIS_DEF_AXIS16_MV_STEP = 15;
        public const int NFLI_AXIS_DEF_AXIS17_MV_STEP = 16;
        public const int NFLI_AXIS_DEF_AXIS18_MV_STEP = 17;
        public const int NFLI_AXIS_DEF_AXIS19_MV_STEP = 18;
        public const int NFLI_AXIS_DEF_AXIS20_MV_STEP = 19;
        public const int NFLI_AXIS_DEF_AXIS21_MV_STEP = 20;

        static string[] s_szArrayAxisDefEntryAxisMVStep = {    
        "AXIS1_MV_STEP",	"AXIS2_MV_STEP", "AXIS3_MV_STEP", "AXIS4_MV_STEP",  "AXIS5_MV_STEP",
 	    "AXIS6_MV_STEP",	"AXIS7_MV_STEP", "AXIS8_MV_STEP", "AXIS9_MV_STEP",  "AXIS10_MV_STEP", 
        "AXIS11_MV_STEP","AXIS12_MV_STEP","AXIS13_MV_STEP","AXIS14_MV_STEP","AXIS15_MV_STEP",
        "AXIS16_MV_STEP","AXIS17_MV_STEP","AXIS18_MV_STEP","AXIS19_MV_STEP","AXIS20_MV_STEP",
        "AXIS21_MV_STEP" };

        public const string NFLI_AXIS_DEF_AXIS_MV_MIN_SECTION = "NFLI_AXIS_DEF_AXIS_MV_MIN";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_MV_MIN_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_MV_MIN = 0;
        public const int NFLI_AXIS_DEF_AXIS2_MV_MIN = 1;
        public const int NFLI_AXIS_DEF_AXIS3_MV_MIN = 2;
        public const int NFLI_AXIS_DEF_AXIS4_MV_MIN = 3;
        public const int NFLI_AXIS_DEF_AXIS5_MV_MIN = 4;
        public const int NFLI_AXIS_DEF_AXIS6_MV_MIN = 5;
        public const int NFLI_AXIS_DEF_AXIS7_MV_MIN = 6;
        public const int NFLI_AXIS_DEF_AXIS8_MV_MIN = 7;
        public const int NFLI_AXIS_DEF_AXIS9_MV_MIN = 8;
        public const int NFLI_AXIS_DEF_AXIS10_MV_MIN = 9;
        public const int NFLI_AXIS_DEF_AXIS11_MV_MIN = 10;
        public const int NFLI_AXIS_DEF_AXIS12_MV_MIN = 11;
        public const int NFLI_AXIS_DEF_AXIS13_MV_MIN = 12;
        public const int NFLI_AXIS_DEF_AXIS14_MV_MIN = 13;
        public const int NFLI_AXIS_DEF_AXIS15_MV_MIN = 14;
        public const int NFLI_AXIS_DEF_AXIS16_MV_MIN = 15;
        public const int NFLI_AXIS_DEF_AXIS17_MV_MIN = 16;
        public const int NFLI_AXIS_DEF_AXIS18_MV_MIN = 17;
        public const int NFLI_AXIS_DEF_AXIS19_MV_MIN = 18;
        public const int NFLI_AXIS_DEF_AXIS20_MV_MIN = 19;
        public const int NFLI_AXIS_DEF_AXIS21_MV_MIN = 20;

        static string[] s_szArrayAxisDefEntryAxisMVMin = {    
        "AXIS1_MV_MIN",	"AXIS2_MV_MIN", "AXIS3_MV_MIN", "AXIS4_MV_MIN",  "AXIS5_MV_MIN",
 	    "AXIS6_MV_MIN",	"AXIS7_MV_MIN", "AXIS8_MV_MIN", "AXIS9_MV_MIN",  "AXIS10_MV_MIN", 
        "AXIS11_MV_MIN","AXIS12_MV_MIN","AXIS13_MV_MIN","AXIS14_MV_MIN","AXIS15_MV_MIN",
        "AXIS16_MV_MIN","AXIS17_MV_MIN","AXIS18_MV_MIN","AXIS19_MV_MIN","AXIS20_MV_MIN",
        "AXIS21_MV_MIN" };

        public const string NFLI_AXIS_DEF_AXIS_MV_MAX_SECTION = "NFLI_AXIS_DEF_AXIS_MV_MAX";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_MV_MAX_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_MV_MAX = 0;
        public const int NFLI_AXIS_DEF_AXIS2_MV_MAX = 1;
        public const int NFLI_AXIS_DEF_AXIS3_MV_MAX = 2;
        public const int NFLI_AXIS_DEF_AXIS4_MV_MAX = 3;
        public const int NFLI_AXIS_DEF_AXIS5_MV_MAX = 4;
        public const int NFLI_AXIS_DEF_AXIS6_MV_MAX = 5;
        public const int NFLI_AXIS_DEF_AXIS7_MV_MAX = 6;
        public const int NFLI_AXIS_DEF_AXIS8_MV_MAX = 7;
        public const int NFLI_AXIS_DEF_AXIS9_MV_MAX = 8;
        public const int NFLI_AXIS_DEF_AXIS10_MV_MAX = 9;
        public const int NFLI_AXIS_DEF_AXIS11_MV_MAX = 10;
        public const int NFLI_AXIS_DEF_AXIS12_MV_MAX = 11;
        public const int NFLI_AXIS_DEF_AXIS13_MV_MAX = 12;
        public const int NFLI_AXIS_DEF_AXIS14_MV_MAX = 13;
        public const int NFLI_AXIS_DEF_AXIS15_MV_MAX = 14;
        public const int NFLI_AXIS_DEF_AXIS16_MV_MAX = 15;
        public const int NFLI_AXIS_DEF_AXIS17_MV_MAX = 16;
        public const int NFLI_AXIS_DEF_AXIS18_MV_MAX = 17;
        public const int NFLI_AXIS_DEF_AXIS19_MV_MAX = 18;
        public const int NFLI_AXIS_DEF_AXIS20_MV_MAX = 19;
        public const int NFLI_AXIS_DEF_AXIS21_MV_MAX = 20;

        static string[] s_szArrayAxisDefEntryAxisMVMax = {    
        "AXIS1_MV_MAX",	"AXIS2_MV_MAX", "AXIS3_MV_MAX", "AXIS4_MV_MAX",  "AXIS5_MV_MAX",
 	    "AXIS6_MV_MAX",	"AXIS7_MV_MAX", "AXIS8_MV_MAX", "AXIS9_MV_MAX",  "AXIS10_MV_MAX", 
        "AXIS11_MV_MAX","AXIS12_MV_MAX","AXIS13_MV_MAX","AXIS14_MV_MAX","AXIS15_MV_MAX",
        "AXIS16_MV_MAX","AXIS17_MV_MAX","AXIS18_MV_MAX","AXIS19_MV_MAX","AXIS20_MV_MAX",
        "AXIS21_MV_MAX" };

        public const string NFLI_AXIS_DEF_AXIS_JV_STEP_SECTION = "NFLI_AXIS_DEF_AXIS_JV_STEP";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_JV_STEP_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_JV_STEP = 0;
        public const int NFLI_AXIS_DEF_AXIS2_JV_STEP = 1;
        public const int NFLI_AXIS_DEF_AXIS3_JV_STEP = 2;
        public const int NFLI_AXIS_DEF_AXIS4_JV_STEP = 3;
        public const int NFLI_AXIS_DEF_AXIS5_JV_STEP = 4;
        public const int NFLI_AXIS_DEF_AXIS6_JV_STEP = 5;
        public const int NFLI_AXIS_DEF_AXIS7_JV_STEP = 6;
        public const int NFLI_AXIS_DEF_AXIS8_JV_STEP = 7;
        public const int NFLI_AXIS_DEF_AXIS9_JV_STEP = 8;
        public const int NFLI_AXIS_DEF_AXIS10_JV_STEP = 9;
        public const int NFLI_AXIS_DEF_AXIS11_JV_STEP = 10;
        public const int NFLI_AXIS_DEF_AXIS12_JV_STEP = 11;
        public const int NFLI_AXIS_DEF_AXIS13_JV_STEP = 12;
        public const int NFLI_AXIS_DEF_AXIS14_JV_STEP = 13;
        public const int NFLI_AXIS_DEF_AXIS15_JV_STEP = 14;
        public const int NFLI_AXIS_DEF_AXIS16_JV_STEP = 15;
        public const int NFLI_AXIS_DEF_AXIS17_JV_STEP = 16;
        public const int NFLI_AXIS_DEF_AXIS18_JV_STEP = 17;
        public const int NFLI_AXIS_DEF_AXIS19_JV_STEP = 18;
        public const int NFLI_AXIS_DEF_AXIS20_JV_STEP = 19;
        public const int NFLI_AXIS_DEF_AXIS21_JV_STEP = 20;

        static string[] s_szArrayAxisDefEntryAxisJVStep = {    
        "AXIS1_JV_STEP",	"AXIS2_JV_STEP", "AXIS3_JV_STEP", "AXIS4_JV_STEP",  "AXIS5_JV_STEP",
 	    "AXIS6_JV_STEP",	"AXIS7_JV_STEP", "AXIS8_JV_STEP", "AXIS9_JV_STEP",  "AXIS10_JV_STEP", 
        "AXIS11_JV_STEP","AXIS12_JV_STEP","AXIS13_JV_STEP","AXIS14_JV_STEP","AXIS15_JV_STEP",
        "AXIS16_JV_STEP","AXIS17_JV_STEP","AXIS18_JV_STEP","AXIS19_JV_STEP","AXIS20_JV_STEP",
        "AXIS21_JV_STEP" };

        public const string NFLI_AXIS_DEF_AXIS_JV_MIN_SECTION = "NFLI_AXIS_DEF_AXIS_JV_MIN";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_JV_MIN_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_JV_MIN = 0;
        public const int NFLI_AXIS_DEF_AXIS2_JV_MIN = 1;
        public const int NFLI_AXIS_DEF_AXIS3_JV_MIN = 2;
        public const int NFLI_AXIS_DEF_AXIS4_JV_MIN = 3;
        public const int NFLI_AXIS_DEF_AXIS5_JV_MIN = 4;
        public const int NFLI_AXIS_DEF_AXIS6_JV_MIN = 5;
        public const int NFLI_AXIS_DEF_AXIS7_JV_MIN = 6;
        public const int NFLI_AXIS_DEF_AXIS8_JV_MIN = 7;
        public const int NFLI_AXIS_DEF_AXIS9_JV_MIN = 8;
        public const int NFLI_AXIS_DEF_AXIS10_JV_MIN = 9;
        public const int NFLI_AXIS_DEF_AXIS11_JV_MIN = 10;
        public const int NFLI_AXIS_DEF_AXIS12_JV_MIN = 11;
        public const int NFLI_AXIS_DEF_AXIS13_JV_MIN = 12;
        public const int NFLI_AXIS_DEF_AXIS14_JV_MIN = 13;
        public const int NFLI_AXIS_DEF_AXIS15_JV_MIN = 14;
        public const int NFLI_AXIS_DEF_AXIS16_JV_MIN = 15;
        public const int NFLI_AXIS_DEF_AXIS17_JV_MIN = 16;
        public const int NFLI_AXIS_DEF_AXIS18_JV_MIN = 17;
        public const int NFLI_AXIS_DEF_AXIS19_JV_MIN = 18;
        public const int NFLI_AXIS_DEF_AXIS20_JV_MIN = 19;
        public const int NFLI_AXIS_DEF_AXIS21_JV_MIN = 20;


        static string[] s_szArrayAxisDefEntryAxisJVMin = {    
        "AXIS1_JV_MIN",	"AXIS2_JV_MIN", "AXIS3_JV_MIN", "AXIS4_JV_MIN",  "AXIS5_JV_MIN",
 	    "AXIS6_JV_MIN",	"AXIS7_JV_MIN", "AXIS8_JV_MIN", "AXIS9_JV_MIN",  "AXIS10_JV_MIN", 
        "AXIS11_JV_MIN","AXIS12_JV_MIN","AXIS13_JV_MIN","AXIS14_JV_MIN","AXIS15_JV_MIN",
        "AXIS16_JV_MIN","AXIS17_JV_MIN","AXIS18_JV_MIN","AXIS19_JV_MIN","AXIS20_JV_MIN",
        "AXIS21_JV_MIN" };

        public const string NFLI_AXIS_DEF_AXIS_JV_MAX_SECTION = "NFLI_AXIS_DEF_AXIS_JV_MAX";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_JV_MAX_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_JV_MAX = 0;
        public const int NFLI_AXIS_DEF_AXIS2_JV_MAX = 1;
        public const int NFLI_AXIS_DEF_AXIS3_JV_MAX = 2;
        public const int NFLI_AXIS_DEF_AXIS4_JV_MAX = 3;
        public const int NFLI_AXIS_DEF_AXIS5_JV_MAX = 4;
        public const int NFLI_AXIS_DEF_AXIS6_JV_MAX = 5;
        public const int NFLI_AXIS_DEF_AXIS7_JV_MAX = 6;
        public const int NFLI_AXIS_DEF_AXIS8_JV_MAX = 7;
        public const int NFLI_AXIS_DEF_AXIS9_JV_MAX = 8;
        public const int NFLI_AXIS_DEF_AXIS10_JV_MAX = 9;
        public const int NFLI_AXIS_DEF_AXIS11_JV_MAX = 10;
        public const int NFLI_AXIS_DEF_AXIS12_JV_MAX = 11;
        public const int NFLI_AXIS_DEF_AXIS13_JV_MAX = 12;
        public const int NFLI_AXIS_DEF_AXIS14_JV_MAX = 13;
        public const int NFLI_AXIS_DEF_AXIS15_JV_MAX = 14;
        public const int NFLI_AXIS_DEF_AXIS16_JV_MAX = 15;
        public const int NFLI_AXIS_DEF_AXIS17_JV_MAX = 16;
        public const int NFLI_AXIS_DEF_AXIS18_JV_MAX = 17;
        public const int NFLI_AXIS_DEF_AXIS19_JV_MAX = 18;
        public const int NFLI_AXIS_DEF_AXIS20_JV_MAX = 19;
        public const int NFLI_AXIS_DEF_AXIS21_JV_MAX = 20;

        static string[] s_szArrayAxisDefEntryAxisJVMax = {    
        "AXIS1_JV_MAX",	"AXIS2_JV_MAX", "AXIS3_JV_MAX", "AXIS4_JV_MAX",  "AXIS5_JV_MAX",
 	    "AXIS6_JV_MAX",	"AXIS7_JV_MAX", "AXIS8_JV_MAX", "AXIS9_JV_MAX",  "AXIS10_JV_MAX", 
        "AXIS11_JV_MAX","AXIS12_JV_MAX","AXIS13_JV_MAX","AXIS14_JV_MAX","AXIS15_JV_MAX",
        "AXIS16_JV_MAX","AXIS17_JV_MAX","AXIS18_JV_MAX","AXIS19_JV_MAX","AXIS20_JV_MAX",
        "AXIS21_JV_MAX" };

        public const string NFLI_AXIS_DEF_AXIS_HV_STEP_SECTION = "NFLI_AXIS_DEF_AXIS_HV_STEP";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_HV_STEP_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_HV_STEP = 0;
        public const int NFLI_AXIS_DEF_AXIS2_HV_STEP = 1;
        public const int NFLI_AXIS_DEF_AXIS3_HV_STEP = 2;
        public const int NFLI_AXIS_DEF_AXIS4_HV_STEP = 3;
        public const int NFLI_AXIS_DEF_AXIS5_HV_STEP = 4;
        public const int NFLI_AXIS_DEF_AXIS6_HV_STEP = 5;
        public const int NFLI_AXIS_DEF_AXIS7_HV_STEP = 6;
        public const int NFLI_AXIS_DEF_AXIS8_HV_STEP = 7;
        public const int NFLI_AXIS_DEF_AXIS9_HV_STEP = 8;
        public const int NFLI_AXIS_DEF_AXIS10_HV_STEP = 9;
        public const int NFLI_AXIS_DEF_AXIS11_HV_STEP = 10;
        public const int NFLI_AXIS_DEF_AXIS12_HV_STEP = 11;
        public const int NFLI_AXIS_DEF_AXIS13_HV_STEP = 12;
        public const int NFLI_AXIS_DEF_AXIS14_HV_STEP = 13;
        public const int NFLI_AXIS_DEF_AXIS15_HV_STEP = 14;
        public const int NFLI_AXIS_DEF_AXIS16_HV_STEP = 15;
        public const int NFLI_AXIS_DEF_AXIS17_HV_STEP = 16;
        public const int NFLI_AXIS_DEF_AXIS18_HV_STEP = 17;
        public const int NFLI_AXIS_DEF_AXIS19_HV_STEP = 18;
        public const int NFLI_AXIS_DEF_AXIS20_HV_STEP = 19;
        public const int NFLI_AXIS_DEF_AXIS21_HV_STEP = 20;

        static string[] s_szArrayAxisDefEntryAxisHVStep = {    
        "AXIS1_HV_STEP",	"AXIS2_HV_STEP",    "AXIS3_HV_STEP", "AXIS4_HV_STEP",  "AXIS5_HV_STEP",
 	    "AXIS6_HV_STEP",	"AXIS7_HV_STEP",    "AXIS8_HV_STEP", "AXIS9_HV_STEP",  "AXIS10_HV_STEP", 
        "AXIS11_HV_STEP",   "AXIS12_HV_STEP",   "AXIS13_HV_STEP","AXIS14_HV_STEP","AXIS15_HV_STEP",
        "AXIS16_HV_STEP",   "AXIS17_HV_STEP",  "AXIS18_HV_STEP","AXIS19_HV_STEP","AXIS20_HV_STEP",
        "AXIS21_HV_STEP" };

        public const string NFLI_AXIS_DEF_AXIS_HV_MIN_SECTION = "NFLI_AXIS_DEF_AXIS_HV_MIN";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_HV_MIN_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_HV_MIN = 0;
        public const int NFLI_AXIS_DEF_AXIS2_HV_MIN = 1;
        public const int NFLI_AXIS_DEF_AXIS3_HV_MIN = 2;
        public const int NFLI_AXIS_DEF_AXIS4_HV_MIN = 3;
        public const int NFLI_AXIS_DEF_AXIS5_HV_MIN = 4;
        public const int NFLI_AXIS_DEF_AXIS6_HV_MIN = 5;
        public const int NFLI_AXIS_DEF_AXIS7_HV_MIN = 6;
        public const int NFLI_AXIS_DEF_AXIS8_HV_MIN = 7;
        public const int NFLI_AXIS_DEF_AXIS9_HV_MIN = 8;
        public const int NFLI_AXIS_DEF_AXIS10_HV_MIN = 9;
        public const int NFLI_AXIS_DEF_AXIS11_HV_MIN = 10;
        public const int NFLI_AXIS_DEF_AXIS12_HV_MIN = 11;
        public const int NFLI_AXIS_DEF_AXIS13_HV_MIN = 12;
        public const int NFLI_AXIS_DEF_AXIS14_HV_MIN = 13;
        public const int NFLI_AXIS_DEF_AXIS15_HV_MIN = 14;
        public const int NFLI_AXIS_DEF_AXIS16_HV_MIN = 15;
        public const int NFLI_AXIS_DEF_AXIS17_HV_MIN = 16;
        public const int NFLI_AXIS_DEF_AXIS18_HV_MIN = 17;
        public const int NFLI_AXIS_DEF_AXIS19_HV_MIN = 18;
        public const int NFLI_AXIS_DEF_AXIS20_HV_MIN = 19;
        public const int NFLI_AXIS_DEF_AXIS21_HV_MIN = 20;

        static string[] s_szArrayAxisDefEntryAxisHVMin = {    
        "AXIS1_HV_MIN",	"AXIS2_HV_MIN",    "AXIS3_HV_MIN", "AXIS4_HV_MIN",  "AXIS5_HV_MIN",
 	    "AXIS6_HV_MIN",	"AXIS7_HV_MIN",    "AXIS8_HV_MIN", "AXIS9_HV_MIN",  "AXIS10_HV_MIN", 
        "AXIS11_HV_MIN",   "AXIS12_HV_MIN",   "AXIS13_HV_MIN","AXIS14_HV_MIN","AXIS15_HV_MIN",
        "AXIS16_HV_MIN",   "AXIS17_HV_MIN",  "AXIS18_HV_MIN","AXIS19_HV_MIN","AXIS20_HV_MIN",
        "AXIS21_HV_MIN" };

        public const string NFLI_AXIS_DEF_AXIS_HV_MAX_SECTION = "NFLI_AXIS_DEF_AXIS_HV_MAX";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_HV_MAX_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_HV_MAX = 0;
        public const int NFLI_AXIS_DEF_AXIS2_HV_MAX = 1;
        public const int NFLI_AXIS_DEF_AXIS3_HV_MAX = 2;
        public const int NFLI_AXIS_DEF_AXIS4_HV_MAX = 3;
        public const int NFLI_AXIS_DEF_AXIS5_HV_MAX = 4;
        public const int NFLI_AXIS_DEF_AXIS6_HV_MAX = 5;
        public const int NFLI_AXIS_DEF_AXIS7_HV_MAX = 6;
        public const int NFLI_AXIS_DEF_AXIS8_HV_MAX = 7;
        public const int NFLI_AXIS_DEF_AXIS9_HV_MAX = 8;
        public const int NFLI_AXIS_DEF_AXIS10_HV_MAX = 9;
        public const int NFLI_AXIS_DEF_AXIS11_HV_MAX = 10;
        public const int NFLI_AXIS_DEF_AXIS12_HV_MAX = 11;
        public const int NFLI_AXIS_DEF_AXIS13_HV_MAX = 12;
        public const int NFLI_AXIS_DEF_AXIS14_HV_MAX = 13;
        public const int NFLI_AXIS_DEF_AXIS15_HV_MAX = 14;
        public const int NFLI_AXIS_DEF_AXIS16_HV_MAX = 15;
        public const int NFLI_AXIS_DEF_AXIS17_HV_MAX = 16;
        public const int NFLI_AXIS_DEF_AXIS18_HV_MAX = 17;
        public const int NFLI_AXIS_DEF_AXIS19_HV_MAX = 18;
        public const int NFLI_AXIS_DEF_AXIS20_HV_MAX = 19;
        public const int NFLI_AXIS_DEF_AXIS21_HV_MAX = 20;


        static string[] s_szArrayAxisDefEntryAxisHVMax = {    
        "AXIS1_HV_MAX",	"AXIS2_HV_MAX",    "AXIS3_HV_MAX", "AXIS4_HV_MAX",  "AXIS5_HV_MAX",
 	    "AXIS6_HV_MAX",	"AXIS7_HV_MAX",    "AXIS8_HV_MAX", "AXIS9_HV_MAX",  "AXIS10_HV_MAX", 
        "AXIS11_HV_MAX",   "AXIS12_HV_MAX",   "AXIS13_HV_MAX","AXIS14_HV_MAX","AXIS15_HV_MAX",
        "AXIS16_HV_MAX",   "AXIS17_HV_MAX",  "AXIS18_HV_MAX","AXIS19_HV_MAX","AXIS20_HV_MAX",
        "AXIS21_HV_MAX" };

        public const string NFLI_AXIS_DEF_AXIS_AS_MIN_SECTION = "NFLI_AXIS_DEF_AXIS_AS_MIN";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_AS_MIN_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_AS_MIN = 0;
        public const int NFLI_AXIS_DEF_AXIS2_AS_MIN = 1;
        public const int NFLI_AXIS_DEF_AXIS3_AS_MIN = 2;
        public const int NFLI_AXIS_DEF_AXIS4_AS_MIN = 3;
        public const int NFLI_AXIS_DEF_AXIS5_AS_MIN = 4;
        public const int NFLI_AXIS_DEF_AXIS6_AS_MIN = 5;
        public const int NFLI_AXIS_DEF_AXIS7_AS_MIN = 6;
        public const int NFLI_AXIS_DEF_AXIS8_AS_MIN = 7;
        public const int NFLI_AXIS_DEF_AXIS9_AS_MIN = 8;
        public const int NFLI_AXIS_DEF_AXIS10_AS_MIN = 9;
        public const int NFLI_AXIS_DEF_AXIS11_AS_MIN = 10;
        public const int NFLI_AXIS_DEF_AXIS12_AS_MIN = 11;
        public const int NFLI_AXIS_DEF_AXIS13_AS_MIN = 12;
        public const int NFLI_AXIS_DEF_AXIS14_AS_MIN = 13;
        public const int NFLI_AXIS_DEF_AXIS15_AS_MIN = 14;
        public const int NFLI_AXIS_DEF_AXIS16_AS_MIN = 15;
        public const int NFLI_AXIS_DEF_AXIS17_AS_MIN = 16;
        public const int NFLI_AXIS_DEF_AXIS18_AS_MIN = 17;
        public const int NFLI_AXIS_DEF_AXIS19_AS_MIN = 18;
        public const int NFLI_AXIS_DEF_AXIS20_AS_MIN = 19;
        public const int NFLI_AXIS_DEF_AXIS21_AS_MIN = 20;

        static string[] s_szArrayAxisDefEntryAxisASMin = {    
        "AXIS1_AS_MIN",	"AXIS2_AS_MIN",    "AXIS3_AS_MIN", "AXIS4_AS_MIN",  "AXIS5_AS_MIN",
 	    "AXIS6_AS_MIN",	"AXIS7_AS_MIN",    "AXIS8_AS_MIN", "AXIS9_AS_MIN",  "AXIS10_AS_MIN", 
        "AXIS11_AS_MIN",   "AXIS12_AS_MIN",   "AXIS13_AS_MIN","AXIS14_AS_MIN","AXIS15_AS_MIN",
        "AXIS16_AS_MIN",   "AXIS17_AS_MIN",  "AXIS18_AS_MIN","AXIS19_AS_MIN","AXIS20_AS_MIN",
        "AXIS21_AS_MIN" };

        public const string NFLI_AXIS_DEF_AXIS_AS_MAX_SECTION = "NFLI_AXIS_DEF_AXIS_AS_MAX";

        public const int NFLI_AXIS_DEF_ENTRY_AXIS_AS_MAX_CNT = 21;

        public const int NFLI_AXIS_DEF_AXIS1_AS_MAX = 0;
        public const int NFLI_AXIS_DEF_AXIS2_AS_MAX = 1;
        public const int NFLI_AXIS_DEF_AXIS3_AS_MAX = 2;
        public const int NFLI_AXIS_DEF_AXIS4_AS_MAX = 3;
        public const int NFLI_AXIS_DEF_AXIS5_AS_MAX = 4;
        public const int NFLI_AXIS_DEF_AXIS6_AS_MAX = 5;
        public const int NFLI_AXIS_DEF_AXIS7_AS_MAX = 6;
        public const int NFLI_AXIS_DEF_AXIS8_AS_MAX = 7;
        public const int NFLI_AXIS_DEF_AXIS9_AS_MAX = 8;
        public const int NFLI_AXIS_DEF_AXIS10_AS_MAX = 9;
        public const int NFLI_AXIS_DEF_AXIS11_AS_MAX = 10;
        public const int NFLI_AXIS_DEF_AXIS12_AS_MAX = 11;
        public const int NFLI_AXIS_DEF_AXIS13_AS_MAX = 12;
        public const int NFLI_AXIS_DEF_AXIS14_AS_MAX = 13;
        public const int NFLI_AXIS_DEF_AXIS15_AS_MAX = 14;
        public const int NFLI_AXIS_DEF_AXIS16_AS_MAX = 15;
        public const int NFLI_AXIS_DEF_AXIS17_AS_MAX = 16;
        public const int NFLI_AXIS_DEF_AXIS18_AS_MAX = 17;
        public const int NFLI_AXIS_DEF_AXIS19_AS_MAX = 18;
        public const int NFLI_AXIS_DEF_AXIS20_AS_MAX = 19;
        public const int NFLI_AXIS_DEF_AXIS21_AS_MAX = 20;


        static string[] s_szArrayAxisDefEntryAxisASMax = {    
        "AXIS1_AS_MAX",	"AXIS2_AS_MAX",    "AXIS3_AS_MAX", "AXIS4_AS_MAX",  "AXIS5_AS_MAX",
 	    "AXIS6_AS_MAX",	"AXIS7_AS_MAX",    "AXIS8_AS_MAX", "AXIS9_AS_MAX",  "AXIS10_AS_MAX", 
        "AXIS11_AS_MAX",   "AXIS12_AS_MAX",   "AXIS13_AS_MAX","AXIS14_AS_MAX","AXIS15_AS_MAX",
        "AXIS16_AS_MAX",   "AXIS17_AS_MAX",  "AXIS18_AS_MAX","AXIS19_AS_MAX","AXIS20_AS_MAX",
        "AXIS21_AS_MAX" };

        public const int NFLI_DEFAULT_AXIS_DEF_AXIS_NO = 0;
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_CNT_PER_MM = "100.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_MIN_POS = "-9999.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_MAX_POS = "9999.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_MV_STEP = "10.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_MV_MIN = "10.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_MV_MAX = "100.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_JV_STEP = "5.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_JV_MIN = "5.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_JV_MAX = "50.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_HV_STEP = "5.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_HV_MIN = "5.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_HV_MAX = "50.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_AS_MIN = "1.";
        public const string NFLI_DEFAULT_AXIS_DEF_AXIS_AS_MAX = "16.";

        static double[] s_dArrayAxisCntPerMM = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisMinPos = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisMaxPos = new double[NFLI_AXIS_CNT];

        static double[] s_dArrayAxisMoveVelocityStep = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisMoveVelocityMin = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisMoveVelocityMax = new double[NFLI_AXIS_CNT];

        static double[] s_dArrayAxisJogVelocityStep = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisJogVelocityMin = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisJogVelocityMax = new double[NFLI_AXIS_CNT];

        static double[] s_dArrayAxisHomeVelocityStep = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisHomeVelocityMin = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisHomeVelocityMax = new double[NFLI_AXIS_CNT];

        static double[] s_dArrayAxisAccelScaleMin = new double[NFLI_AXIS_CNT];
        static double[] s_dArrayAxisAccelScaleMax = new double[NFLI_AXIS_CNT];

	    static AXISUNIT[] s_arrayAxisUnit = new AXISUNIT[NFLI_AXIS_CNT];

       

        static public void setRe(string wdata, string wkey)
        {
            RegistryKey reg;
            reg = Registry.LocalMachine.CreateSubKey("Software\\Nexstar Technology\\NSIARMan\\NSIAR_CONFIG_ETC", RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(wdata, wkey, RegistryValueKind.String);
            reg.Close();
        }

        static public string getReg(string wdata)
        {
            RegistryKey reg = Registry.LocalMachine;
            reg = reg.OpenSubKey("Software\\Nexstar Technology\\NSIARMan\\NSIAR_CONFIG_ETC\\ET", true);
            if (reg == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(reg.GetValue(wdata));
            }
        }
    }
}


/*
    //C++에서 상수
    public const int pi 3.141592654
    public const int dolden_ratio 1.61803

    //C#에서 상수
    static class Constants
    {
        public const double Pi = 3.141592654;
        public const int SpeedOfLight = 300000; //km per sec.
    }

    class Program
    {
        static void Main()
        {
            double radius = 5.3;
            double area = Constants.Pi *(radius*radius);
            int secsFromSun = 149476000 / Constants.SpeedOfLight; //in km
        }
    }
*/