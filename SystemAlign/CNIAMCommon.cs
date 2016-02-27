///////////////////////////////////// INFO //////////////////////////////////
// by Ki Back Woo in Nexstart R&D center
// emergency : wkb@nexstar21.com
// MSN : 
/////////////////////////////////////////////////////////////////////////////

// NSIARCommon.cs: interface for the CNSIARCommon class.
//
//////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemAlign
{
    class CNIAMCommon
    {
        public const int WM_APP = 0x8000;

        public const int NFLI_TIMER_UPDATE_STATUS = 100;
        public const int NFLI_QUEST_FINISH = WM_APP + 0X00003000;
        // w: unused, l: unused
        public const int NFLI_QUEST_FINISHED = WM_APP + 0X00003001;
        // w: unused, l: unused
        public const int NFLI_UPDATE_SEQ_LOG = WM_APP + 0X00003002;
        // w: unused, l: unused
        public const int NFLI_UPDATE_STATUS = WM_APP + 0X00003003;
        // w: unused, l: unused
        public const int NFLI_END_INPUT_YES_DLG = WM_APP + 0X00003004;
        //w: wnd *, l: unused
        public const int NFLI_END_INPUT_YES_NO_DLG = WM_APP + 0X00003005;
        //w: wnd *, l: IDYES or IDNO
        public const int NFLI_END_CONFIRM_LAST_PALLET_DLG = WM_APP + 0X00003006;
        //w: wnd *, l: unused
        public const int NFLI_END_CONFIRM_LAST_PASS_TRAY_DLG = WM_APP + 0X00003007;
        //w: wnd *, l: unused
        public const int NFLI_END_CONFIRM_LAST_FAIL_TRAY_DLG = WM_APP + 0X00003008;
        //w: wnd *, l: unused
        public const int NFLI_PASS_MAGAZINE_INFO_LBDW = WM_APP + 0X00003009;
        //w: row idx, l: col idx
        public const int NFLI_PASS_TRAY_INFO_LBDW = WM_APP + 0X0000300A;
        //w: row idx, l: col idx
        public const int NFLI_FAIL_MAGAZINE_INFO_LBDW = WM_APP + 0X0000300B;
        //w: row idx, l: col idx
        public const int NFLI_FAIL_TRAY_INFO_LBDW = WM_APP + 0X0000300C;
        //w: row idx, l: col idx
        public const int NFLI_PALLET_INFO_LBDW = WM_APP + 0X0000300D;
        //w: row idx, l: col idx
        public const int NFLI_LENS_INFO_LBDW = WM_APP + 0X0000300E;
        //w: row idx, l: col idx
        public const int NFLI_DEFECT_INFO_LBDW = WM_APP + 0X0000300F;
        //w: row idx, l: col idx
        public const int NFLI_FRAMEWORK_STARTED = WM_APP + 0X00003010;
        //w: unused, l: unused
        public const int NFLI_FRAMEWORK_FINISHED = WM_APP + 0X00003011;
        //w: unused, l: unused

        public const int NFLI_FPS_CHECK_BOX_SET = WM_APP + 0X00003070;
        //w: fps address, l: lo word=row; hi word=col;
        public const int NFLI_FPS_CHECK_BOX_RESET = WM_APP + 0X00003071;
        //w: fps address, l: lo word=row; hi word=col;
        public const int NFLI_PENDANT_IO_REQUEST = WM_APP + 0X00003072;
        //w: pendant io request, l: unused

        public const int NFLI_DATA_ARRANGE = WM_APP + 0X00003090;
        //w: seed, l: unused

        public const int SEQ_INFO_START_RUN = WM_APP + 0X00003100;
        //w: time, l: clock
        public const int SEQ_INFO_READY_PASS_MAGAZINE = WM_APP + 0X00003101;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_END_PASS_MAGAZINE = WM_APP + 0X00003102;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_READY_FAIL_MAGAZINE = WM_APP + 0X00003103;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_END_FAIL_MAGAZINE = WM_APP + 0X00003104;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_READY_LOADER_MAGAZINE = WM_APP + 0X00003105;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_END_LOADER_MAGAZINE = WM_APP + 0X00003106;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_READY_UNLOADER_MAGAZINE = WM_APP + 0X00003107;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_END_UNLOADER_MAGAZINE = WM_APP + 0X00003108;
        //w: LO=magazine idx;, l: unused
        public const int SEQ_INFO_READY_PASS_TRAY = WM_APP + 0X00003109;
        //w: LO=magazine idx;, HI=tray idx;, l: unused
        public const int SEQ_INFO_READY_FAIL_TRAY = WM_APP + 0X0000310A;
        //w: LO=magazine idx;, HI=tray idx;, l: unused
        public const int SEQ_INFO_END_PASS_TRAY_UNLOAD = WM_APP + 0X0000310B;
        //w: LO=magazine idx;, HI=tray idx;, l: unused
        public const int SEQ_INFO_END_FAIL_TRAY_UNLOAD = WM_APP + 0X0000310C;
        //w: LO=magazine idx;, HI=tray idx;, l: unused
        public const int SEQ_INFO_END_SORT_PASS_LENS = WM_APP + 0X0000310D;
        //w: LO=magazine idx;, HI=tray idx;, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_INFO_END_SORT_FAIL_LENS = WM_APP + 0X0000310E;
        //w: LO=magazine idx;, HI=tray idx;, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_INFO_READY_PALLET = WM_APP + 0X0000310F;
        //w: LO=pallet idx;, l: unused
        public const int SEQ_INFO_END_PALLET_SORT = WM_APP + 0X00003110;
        //w: LO=pallet idx;, l: unused
        public const int SEQ_INFO_END_PALLET_UNLOAD = WM_APP + 0X00003111;
        //w: LO=pallet idx;, l: unused
        public const int SEQ_INFO_END_LENS_SCAN = WM_APP + 0X00003112;
        //w: LO=pallet idx;, HI=scan idx;, l: LO=hole idx x;, HI=hole idxY;
        public const int SEQ_INFO_END_LENS_INSP = WM_APP + 0X00003113;
        //w: LO=pallet idx;, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_INFO_END_UPDATE_SRC = WM_APP + 0X00003114;
        //w: scan idx, l: unused


        public const int SEQ_DYNAMIC_RUN_LOADER_MAGAZINE = WM_APP + 0X00003200;
        public const int SEQ_DYNAMIC_RUN_UNLOADER_MAGAZINE = WM_APP + 0X00003201;
        public const int SEQ_DYNAMIC_RUN_PALLET_LOADER = WM_APP + 0X00003202;
        public const int SEQ_DYNAMIC_RUN_PALLET_PICKER = WM_APP + 0X00003203;
        public const int SEQ_DYNAMIC_RUN_CLEANER = WM_APP + 0X00003204;
        public const int SEQ_DYNAMIC_RUN_INDEX = WM_APP + 0X00003205;
        public const int SEQ_DYNAMIC_RUN_INSPECT_OVER1 = WM_APP + 0X00003206;
        public const int SEQ_DYNAMIC_RUN_INSPECT_OVER2 = WM_APP + 0X00003207;
        public const int SEQ_DYNAMIC_RUN_INSPECT_OVER3 = WM_APP + 0X00003208;
        public const int SEQ_DYNAMIC_RUN_INSPECT_UNDER1 = WM_APP + 0X00003209;
        public const int SEQ_DYNAMIC_RUN_INSPECT_UNDER2 = WM_APP + 0X0000320A;
        public const int SEQ_DYNAMIC_RUN_INSPECT_UNDER3 = WM_APP + 0X0000320B;
        public const int SEQ_DYNAMIC_RUN_SORTER = WM_APP + 0X0000320C;
        public const int SEQ_DYNAMIC_RUN_PASS_MAGAZINE = WM_APP + 0X0000320D;
        public const int SEQ_DYNAMIC_RUN_FAIL_MAGAZINE = WM_APP + 0X0000320E;

        /***
        public const int  SEQ_PROC_BUF_TRAY											=WM_APP + 0X00003300;
        w: seed, l: unused
        ***/

        public const int SEQ_CALL_CLEANER_INSERT = WM_APP + 0X00003400;
        //w: seed, l: pallet idx
        public const int SEQ_CALL_INDEX_INSERT = WM_APP + 0X00003401;
        //w: seed, l: pallet idx
        public const int SEQ_CALL_PASS_TRAY_BAR_CODE = WM_APP + 0X00003402;
        //w: seed, l: LO=magazine idx;, HI=tray idx;
        public const int SEQ_CALL_FAIL_TRAY_BAR_CODE = WM_APP + 0X00003403;
        //w: seed, l: LO=magazine idx;, HI=tray idx;

        /***
        public const int  SEQ_AUX_RUN_STOP											=WM_APP + 0X00003400;
        ***/

        public const int SEQ_INSP_SCAN = WM_APP + 0X00003450;
        //w: LO=pallet idx;, HI=scan idx;, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_INSP_SCAN_WAIT = WM_APP + 0X00003451;
        //w: LO=pallet idx;, HI=scan idx;, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_INSP_STOP = WM_APP + 0X00003452;
        //w: unused, l: unused

        public const int SEQ_GRAB_SCAN = WM_APP + 0X00003453;
        //w: LO=pallet idx;, HI=scan idx;, l: LO=hole idx x;, HI=hole idx y;

        public const int SEQ_LIGHT_BRIGHT = WM_APP + 0X00003460;
        //w: light idx, l: brightness
        public const int SEQ_LIGHT_WAIT = WM_APP + 0X00003461;
        //w: light idx, l: unused
        public const int SEQ_LIGHT_STOP = WM_APP + 0X00003462;
        //w: unused, l: unused

        public const int SEQ_SET_STEP_TIME_OVER = WM_APP + 0X00003500;
        //w: status idx, l: seed
        public const int SEQ_SET_TRAY_FAIL_RATE_OVER = WM_APP + 0X00003501;
        // w: magazine idx, l: LO=tray idx;
        public const int SEQ_SET_FAIL_PALLET_BAR_CODE_READ = WM_APP + 0X00003502;
        //w: unused, l: unused
        public const int SEQ_SET_FAIL_PASS_TRAY_BAR_CODE_READ = WM_APP + 0X00003503;
        //w: unused, l: unused
        public const int SEQ_SET_FAIL_FAIL_TRAY_BAR_CODE_READ = WM_APP + 0X00003504;
        //w: unused, l: unused
        public const int SEQ_SET_DIO_OUT_CHANGED = WM_APP + 0X00003505;
        //w: unit idx, l: unused
        public const int SEQ_SET_DETECT_DOOR_OPEN = WM_APP + 0X00003506;
        //w: '0' door1, '1' door2, '2' door3, '3' door4, '4' closed l: unused
        public const int SEQ_SET_CHANGE_PASS_MAGAZINE = WM_APP + 0X00003507;
        //w: unused, l: '0' change, '1' check
        public const int SEQ_SET_CHANGE_FAIL_MAGAZINE = WM_APP + 0X00003508;
        //w: unused, l: '0' change, '1' check
        public const int SEQ_SET_CHANGE_LOADER_MAGAZINE = WM_APP + 0X00003509;
        //w: unused, l: '0' change, '1' check
        public const int SEQ_SET_CHANGE_UNLOADER_MAGAZINE = WM_APP + 0X0000350A;
        //w: unused, l: '0' change, '1' check
        public const int SEQ_SET_REMOVE_INSERT_POS_LENS = WM_APP + 0X0000350B;
        //w: unused, l: unused
        public const int SEQ_SET_FAIL_PICK_UP_PALLET_LENS = WM_APP + 0X0000350C;
        //w: unused, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_SET_FAIL_PICK_UP_CLEANER_LENS = WM_APP + 0X0000350D;
        //w: unused, l: LO=hole idx x;, HI=hole idx y;
        public const int SEQ_SET_FAIL_IMPORT_PALLET_FROM_DB = WM_APP + 0X0000350E;
        //w: LO=pallet idx;, l: unused
        public const int SEQ_SET_CONFIRM_LAST_PALLET_INFO = WM_APP + 0X0000350F;
        //w: unused, l: unused
        public const int SEQ_SET_CONFIRM_LAST_PASS_TRAY_INFO = WM_APP + 0X00003510;
        //w: unused, l: unused
        public const int SEQ_SET_CONFIRM_LAST_FAIL_TRAY_INFO = WM_APP + 0X00003511;
        //w: unused, l: unused

        public const int SEQ_RESET_STEP_TIME_OVER = WM_APP + 0X00003600;
        //w: unused, l: unused
        public const int SEQ_RESET_TRAY_FAIL_RATE_OVER = WM_APP + 0X00003601;
        //w: '0' goto, '1' skip solder, l: unused
        public const int SEQ_RESET_FAIL_PALLET_BAR_CODE_READ = WM_APP + 0X00003602;
        //w: unused, l: unused
        public const int SEQ_RESET_FAIL_PASS_TRAY_BAR_CODE_READ = WM_APP + 0X00003603;
        //w: unused, l: unused
        public const int SEQ_RESET_FAIL_FAIL_TRAY_BAR_CODE_READ = WM_APP + 0X00003604;
        //w: unused, l: unused
        public const int SEQ_RESET_DIO_OUT_CHANGED = WM_APP + 0X00003605;
        //w: unused, l: unused
        public const int SEQ_RESET_DETECT_DOOR_OPEN = WM_APP + 0X00003606;
        //w: unused, l: unused
        public const int SEQ_RESET_CHANGE_PASS_MAGAZINE = WM_APP + 0X00003607;
        //w: unused, l: unused
        public const int SEQ_RESET_CHANGE_FAIL_MAGAZINE = WM_APP + 0X00003608;
        //w: unused, l: unused
        public const int SEQ_RESET_CHANGE_LOADER_MAGAZINE = WM_APP + 0X00003609;
        //w: unused, l: unused
        public const int SEQ_RESET_CHANGE_UNLOADER_MAGAZINE = WM_APP + 0X0000360A;
        //w: unused, l: unused
        public const int SEQ_RESET_REMOVE_INSERT_POS_LENS = WM_APP + 0X0000360B;
        //w: unused, l: unused
        public const int SEQ_RESET_FAIL_PICK_UP_PALLET_LENS = WM_APP + 0X0000360C;
        //w: '0' retry, '1' skip lens, l: unused
        public const int SEQ_RESET_FAIL_PICK_UP_CLEANER_LENS = WM_APP + 0X0000360D;
        //w: '0' retry, '1' skip lens, l: unused
        public const int SEQ_RESET_FAIL_IMPORT_PALLET_FROM_DB = WM_APP + 0X0000360E;
        //w: LO=pallet idx;, l: unused
        public const int SEQ_RESET_CONFIRM_LAST_PALLET_INFO = WM_APP + 0X0000360F;
        //w: unused, l: unused
        public const int SEQ_RESET_CONFIRM_LAST_PASS_TRAY_INFO = WM_APP + 0X00003610;
        //w: unused, l: unused
        public const int SEQ_RESET_CONFIRM_LAST_FAIL_TRAY_INFO = WM_APP + 0X00003611;
        //w: unused, l: unused

        public const int NFLI_INIT_VISION_SERVER = WM_APP + 0X00003700;
        public const int NFLI_SEND_TO_CLIENT = WM_APP + 0X00003701;
        public const int NFLI_SHOW_SERVER = WM_APP + 0X00003702;
        public const int NFLI_SERVER_ASYNC = WM_APP + 0X00003703;
        public const int NFLI_ON_SEND_MESSAGE = WM_APP + 0X00003704;
        public const int NFLI_ON_RECV_MESSAGE = WM_APP + 0X00003705;

        public const int NFLI_DSC_SEND_MACHINE_MODE = WM_APP + 0X00003800;
        //w: unused, l: unused
        public const int NFLI_DSC_SEND_MACHINE_STATUS = WM_APP + 0X00003801;
        //w: unused, l: unused
        public const int NFLI_DSC_SEND_PALLET_REPORT = WM_APP + 0X00003802;
        //w: unused, l: pallet idx
        public const int NFLI_DSC_SEND_PALLET_IMAGE = WM_APP + 0X00003803;
        //w: unused, l: pallet idx

        public const int NFLI_DSC_SEND_RETRY_PALLET_REPORT = WM_APP + 0X00003810;
        //w: unused, l: pallet idx
        public const int NFLI_DSC_SEND_RETRY_PALLET_IMAGE = WM_APP + 0X00003811;
        //w: unused, l: pallet idx


        public const int NFLI_LAMD_IMPORT_PALLET = WM_APP + 0X00003850;
        //w: unused, l: pallet idx
        public const int NFLI_LAMD_DELETE_PALLET = WM_APP + 0X00003851;
        //w: unused, l: pallet idx

        public const int NFLI_MAX_INDEX_HOLE_CNT = 30;

        public const int SEQ_STATUS_CNT = 17;
        public const int SEQ_STATUS_LOADER_MAGAZINE = 0;
        public const int SEQ_STATUS_PALLET_LOADER = 1;
        public const int SEQ_STATUS_PALLET_PICKER = 2;
        public const int SEQ_STATUS_CLEANER_INSERT = 3;
        public const int SEQ_STATUS_CLEANER = 4;
        public const int SEQ_STATUS_INDEX_INSERT = 5;
        public const int SEQ_STATUS_INDEX = 6;
        public const int SEQ_STATUS_INSPECT_OVER1 = 7;
        public const int SEQ_STATUS_INSPECT_OVER2 = 8;
        public const int SEQ_STATUS_INSPECT_OVER3 = 9;
        public const int SEQ_STATUS_INSPECT_UNDER1 = 10;
        public const int SEQ_STATUS_INSPECT_UNDER2 = 11;
        public const int SEQ_STATUS_INSPECT_UNDER3 = 12;
        public const int SEQ_STATUS_SORTER = 13;
        public const int SEQ_STATUS_PASS_MAGAZINE = 14;
        public const int SEQ_STATUS_FAIL_MAGAZINE = 15;
        public const int SEQ_STATUS_UNLOADER_MAGAZINE = 16;

        public const int NFLI_MAX_LENS_TYPE_CNT = 4;
        public const int NFLI_LENS_TYPE_IDX1 = 0;
        public const int NFLI_LENS_TYPE_IDX2 = 1;
        public const int NFLI_LENS_TYPE_IDX3 = 2;
        public const int NFLI_LENS_TYPE_IDX4 = 3;

        public const int NFLI_LENS_TYPE_P1 = NFLI_LENS_TYPE_IDX1;
        public const int NFLI_LENS_TYPE_P2 = NFLI_LENS_TYPE_IDX2;
        public const int NFLI_LENS_TYPE_P3 = NFLI_LENS_TYPE_IDX3;
        public const int NFLI_LENS_TYPE_P4 = NFLI_LENS_TYPE_IDX4;

        public const int NFLI_MAX_HOLE_CNT_X = 20;
        public const int NFLI_MAX_HOLE_CNT_Y = 30;

        public const int NFLI_IPS_TYPE_DIO_IN = 0;
        public const int NFLI_IPS_TYPE_DIO_OUT = 1;

        public const int NFLI_ERR_CNT = 7;

        public const int NFLI_ERR_EMG = 0;
        public const int NFLI_ERR_UNKNOWN = 1;
        public const int NFLI_ERR_MOTION_STATUS = 2;
        public const int NFLI_ERR_IO_STATUS = 3;
        public const int NFLI_ERR_ETC_STATUS = 4;
        public const int NFLI_ERR_CREATE_REPORT = 5;
        public const int NFLI_ERR_MAIN_AIR_ABNORMAL = 6;



        static string[] s_szErrorCodeEntry = {    
                         "EMG PUSH",	    "UNKNOWN",		"MOTION STATUS", 	"I/O STATUS", 	"ETC STATUS",
                  	    "Fail 'create report file",		"Main Air Abnormal"};

        public const int NFLI_TOWER_LAMP_CNT = 3;
        public const int NFLI_TOWER_LAMP_RED = 0;
        public const int NFLI_TOWER_LAMP_YELLOW = 1;
        public const int NFLI_TOWER_LAMP_GREEN = 2;

        public const int NFLI_STATUS_LAMP_CNT = 3;
        public const int NFLI_STATUS_LAMP1 = 0;
        public const int NFLI_STATUS_LAMP2 = 1;
        public const int NFLI_STATUS_LAMP3 = 2;

        public const int NFLI_START_SWITCH_LAMP = NFLI_STATUS_LAMP1;
        public const int NFLI_STOP_SWITCH_LAMP = NFLI_STATUS_LAMP2;
        public const int NFLI_RESET_SWITCH_LAMP = NFLI_STATUS_LAMP3;

        public const int NFLI_HOLE_CNT_MAX = 100;

        public const uint NFLI_HOLE_TYPE_INVALID = 0XFFFFFFFF;
        public const int NFLI_HOLE_TYPE_EMPTY = 0;
        public const int NFLI_HOLE_TYPE_PASS = 1;
        public const int NFLI_HOLE_TYPE_FAIL = 2;
        public const int NFLI_HOLE_TYPE_BARREL = 3;

        public const int NFLI_UI_HOLE_TYPE_INVALID = 0;
        public const int NFLI_UI_HOLE_TYPE_EMPTY = 1;
        public const int NFLI_UI_HOLE_TYPE_PASS = 2;
        public const int NFLI_UI_HOLE_TYPE_FAIL = 3;
        public const int NFLI_UI_HOLE_TYPE_DEFECT = 4;
        public const int NFLI_UI_HOLE_TYPE_PRE_FAIL = 5;
        public const int NFLI_UI_HOLE_TYPE_PRE_PASS = 6;

        public const int NFLI_MAX_SCAN_TARGET_TYPE_CNT = 4;
        public const uint NFLI_SCAN_TARGET_TYPE_INVALID = 0XFFFFFFFF;
        public const int NFLI_SCAN_TARGET_TYPE_P1 = 0;
        public const int NFLI_SCAN_TARGET_TYPE_P2 = 1;
        public const int NFLI_SCAN_TARGET_TYPE_P3 = 2;
        public const int NFLI_SCAN_TARGET_TYPE_P4 = 3;

        public const int NFLI_MAX_SCAN_CNT = 12;
        public const int NFLI_SCAN_IDX1 = 0;
        public const int NFLI_SCAN_IDX2 = 1;
        public const int NFLI_SCAN_IDX3 = 2;
        public const int NFLI_SCAN_IDX4 = 3;
        public const int NFLI_SCAN_IDX5 = 4;
        public const int NFLI_SCAN_IDX6 = 5;
        public const int NFLI_SCAN_IDX7 = 6;
        public const int NFLI_SCAN_IDX8 = 7;
        public const int NFLI_SCAN_IDX9 = 8;
        public const int NFLI_SCAN_IDX10 = 9;
        public const int NFLI_SCAN_IDX11 = 10;
        public const int NFLI_SCAN_IDX12 = 11;

        public const int NFLI_SCAN_CAM_OVER1_1 = NFLI_SCAN_IDX1;
        public const int NFLI_SCAN_CAM_OVER1_2 = NFLI_SCAN_IDX2;
        public const int NFLI_SCAN_CAM_OVER2_1 = NFLI_SCAN_IDX3;
        public const int NFLI_SCAN_CAM_OVER2_2 = NFLI_SCAN_IDX4;
        public const int NFLI_SCAN_CAM_OVER3_1 = NFLI_SCAN_IDX5;
        public const int NFLI_SCAN_CAM_OVER3_2 = NFLI_SCAN_IDX6;
        public const int NFLI_SCAN_CAM_UNDER1_1 = NFLI_SCAN_IDX7;
        public const int NFLI_SCAN_CAM_UNDER1_2 = NFLI_SCAN_IDX8;
        public const int NFLI_SCAN_CAM_UNDER2_1 = NFLI_SCAN_IDX9;
        public const int NFLI_SCAN_CAM_UNDER2_2 = NFLI_SCAN_IDX10;
        public const int NFLI_SCAN_CAM_UNDER3_1 = NFLI_SCAN_IDX11;
        public const int NFLI_SCAN_CAM_UNDER3_2 = NFLI_SCAN_IDX12;

        public const int NFLI_ZONE1_MAX_SCAN_CNT = 2;
        public const int NFLI_ZONE2_MAX_SCAN_CNT = 2;
        public const int NFLI_ZONE3_MAX_SCAN_CNT = 2;
        public const int NFLI_ZONE4_MAX_SCAN_CNT = 2;
        public const int NFLI_ZONE5_MAX_SCAN_CNT = 2;
        public const int NFLI_ZONE6_MAX_SCAN_CNT = 2;

        public const int NFLI_ZONE_CNT = 6;
        public const int NFLI_ZONE_IDX1 = 0;
        public const int NFLI_ZONE_IDX2 = 1;
        public const int NFLI_ZONE_IDX3 = 2;
        public const int NFLI_ZONE_IDX4 = 3;
        public const int NFLI_ZONE_IDX5 = 4;
        public const int NFLI_ZONE_IDX6 = 5;

        public const int NFLI_ZONE_OVER1 = NFLI_ZONE_IDX1;
        public const int NFLI_ZONE_OVER2 = NFLI_ZONE_IDX2;
        public const int NFLI_ZONE_OVER3 = NFLI_ZONE_IDX3;
        public const int NFLI_ZONE_UNDER1 = NFLI_ZONE_IDX4;
        public const int NFLI_ZONE_UNDER2 = NFLI_ZONE_IDX5;
        public const int NFLI_ZONE_UNDER3 = NFLI_ZONE_IDX6;

        public const int NFLI_CAM_CNT = 6;
        public const int NFLI_CAM_IDX1 = 0;
        public const int NFLI_CAM_IDX2 = 1;
        public const int NFLI_CAM_IDX3 = 2;
        public const int NFLI_CAM_IDX4 = 3;
        public const int NFLI_CAM_IDX5 = 4;
        public const int NFLI_CAM_IDX6 = 5;

        public const int NFLI_CAM_OVER1 = NFLI_CAM_IDX1;
        public const int NFLI_CAM_OVER2 = NFLI_CAM_IDX2;
        public const int NFLI_CAM_OVER3 = NFLI_CAM_IDX3;
        public const int NFLI_CAM_UNDER1 = NFLI_CAM_IDX4;
        public const int NFLI_CAM_UNDER2 = NFLI_CAM_IDX5;
        public const int NFLI_CAM_UNDER3 = NFLI_CAM_IDX6;

        public const int NFLI_LIGHT_CTRL_CNT = 7;
        public const int NFLI_LIGHT_CTRL_IDX1 = 0;
        public const int NFLI_LIGHT_CTRL_IDX2 = 1;
        public const int NFLI_LIGHT_CTRL_IDX3 = 2;
        public const int NFLI_LIGHT_CTRL_IDX4 = 3;
        public const int NFLI_LIGHT_CTRL_IDX5 = 4;
        public const int NFLI_LIGHT_CTRL_IDX6 = 5;
        public const int NFLI_LIGHT_CTRL_IDX7 = 6;
        public const int NFLI_LIGHT_CTRL_IDX_NONE = 7;

        public const int NFLI_LIGHT_CTRL_TYPE_CNT = 3;
        public const int NFLI_LIGHT_CTRL_TYPE_PLUS_TECH = 0;
        public const int NFLI_LIGHT_CTRL_TYPE_JFLLS = 1;
        public const int NFLI_LIGHT_CTRL_TYPE_LVSPN = 2;
        public const int NFLI_LIGHT_CTRL_TYPE_NONE = 3;

        public const int NFLI_LIGHT_CTRL_PORT_CNT = 18;
        public const int NFLI_LIGHT_CTRL_PORT_IDX1 = 0;
        public const int NFLI_LIGHT_CTRL_PORT_IDX2 = 1;
        public const int NFLI_LIGHT_CTRL_PORT_IDX3 = 2;
        public const int NFLI_LIGHT_CTRL_PORT_IDX4 = 3;
        public const int NFLI_LIGHT_CTRL_PORT_IDX5 = 4;
        public const int NFLI_LIGHT_CTRL_PORT_IDX6 = 5;
        public const int NFLI_LIGHT_CTRL_PORT_IDX7 = 6;
        public const int NFLI_LIGHT_CTRL_PORT_IDX8 = 7;
        public const int NFLI_LIGHT_CTRL_PORT_IDX9 = 8;
        public const int NFLI_LIGHT_CTRL_PORT_IDX10 = 9;
        public const int NFLI_LIGHT_CTRL_PORT_IDX11 = 10;
        public const int NFLI_LIGHT_CTRL_PORT_IDX12 = 11;
        public const int NFLI_LIGHT_CTRL_PORT_IDX13 = 12;
        public const int NFLI_LIGHT_CTRL_PORT_IDX14 = 13;
        public const int NFLI_LIGHT_CTRL_PORT_IDX15 = 14;
        public const int NFLI_LIGHT_CTRL_PORT_IDX16 = 15;
        public const int NFLI_LIGHT_CTRL_PORT_IDX17 = 16;
        public const int NFLI_LIGHT_CTRL_PORT_IDX18 = 17;

        public const int NFLI_LIGHT_CTRL_NO_CNT = 8;
        public const int NFLI_LIGHT_CTRL_NO_IDX1 = 0;
        public const int NFLI_LIGHT_CTRL_NO_IDX2 = 1;
        public const int NFLI_LIGHT_CTRL_NO_IDX3 = 2;
        public const int NFLI_LIGHT_CTRL_NO_IDX4 = 3;
        public const int NFLI_LIGHT_CTRL_NO_IDX5 = 4;
        public const int NFLI_LIGHT_CTRL_NO_IDX6 = 5;
        public const int NFLI_LIGHT_CTRL_NO_IDX7 = 6;
        public const int NFLI_LIGHT_CTRL_NO_NONE = 7;

        public const int NFLI_LIGHT_CTRL_CH_CNT = 4;
        public const int NFLI_LIGHT_CTRL_CH_IDX1 = 0;
        public const int NFLI_LIGHT_CTRL_CH_IDX2 = 1;
        public const int NFLI_LIGHT_CTRL_CH_IDX3 = 2;
        public const int NFLI_LIGHT_CTRL_CH_IDX4 = 3;

        public const int NFLI_LIGHT_CNT = 12;
        public const int NFLI_LIGHT_IDX1 = 0;
        public const int NFLI_LIGHT_IDX2 = 1;
        public const int NFLI_LIGHT_IDX3 = 2;
        public const int NFLI_LIGHT_IDX4 = 3;
        public const int NFLI_LIGHT_IDX5 = 4;
        public const int NFLI_LIGHT_IDX6 = 5;
        public const int NFLI_LIGHT_IDX7 = 6;
        public const int NFLI_LIGHT_IDX8 = 7;
        public const int NFLI_LIGHT_IDX9 = 8;
        public const int NFLI_LIGHT_IDX10 = 9;
        public const int NFLI_LIGHT_IDX11 = 10;
        public const int NFLI_LIGHT_IDX12 = 11;

        public const int NFLI_LIGHT_OVER1_FRONT = NFLI_LIGHT_IDX1;
        public const int NFLI_LIGHT_OVER1_BACK = NFLI_LIGHT_IDX2;
        public const int NFLI_LIGHT_OVER2_FRONT = NFLI_LIGHT_IDX3;
        public const int NFLI_LIGHT_OVER2_BACK = NFLI_LIGHT_IDX4;
        public const int NFLI_LIGHT_OVER3_FRONT = NFLI_LIGHT_IDX5;
        public const int NFLI_LIGHT_OVER3_BACK = NFLI_LIGHT_IDX6;
        public const int NFLI_LIGHT_UNDER1_FRONT = NFLI_LIGHT_IDX7;
        public const int NFLI_LIGHT_UNDER1_BACK = NFLI_LIGHT_IDX8;
        public const int NFLI_LIGHT_UNDER2_FRONT = NFLI_LIGHT_IDX9;
        public const int NFLI_LIGHT_UNDER2_BACK = NFLI_LIGHT_IDX10;
        public const int NFLI_LIGHT_UNDER3_FRONT = NFLI_LIGHT_IDX11;
        public const int NFLI_LIGHT_UNDER3_BACK = NFLI_LIGHT_IDX12;

        public const int NFLI_DOOR_CNT = 4;
        public const int NFLI_DOOR_IDX1 = 0;
        public const int NFLI_DOOR_IDX2 = 1;
        public const int NFLI_DOOR_IDX3 = 2;
        public const int NFLI_DOOR_IDX4 = 3;

        public const int NFLI_MACHINE_MODE_AUTO = 0;
        public const int NFLI_MACHINE_MODE_MANUAL = 1;

        public const int NFLI_MACHINE_STATUS_RUN = 0;
        public const int NFLI_MACHINE_STATUS_ALARM = 1;
        public const int NFLI_MACHINE_STATUS_PM = 2;
        public const int NFLI_MACHINE_STATUS_SETUP = 3;
        public const int NFLI_MACHINE_STATUS_IDLE = 4;
        public const int NFLI_MACHINE_STATUS_DISCONNECT = 5;

        public const int NFLI_BAR_CODE_CNT = 3;
        public const int NFLI_BAR_CODE_IDX1 = 0;
        public const int NFLI_BAR_CODE_IDX2 = 1;
        public const int NFLI_BAR_CODE_IDX3 = 2;

        public const int NFLI_BAR_CODE_PALLET = NFLI_BAR_CODE_IDX1;
        public const int NFLI_BAR_CODE_PASS_TRAY = NFLI_BAR_CODE_IDX2;
        public const int NFLI_BAR_CODE_FAIL_TRAY = NFLI_BAR_CODE_IDX3;

        public const int NFLI_BAR_CODE_MODULE_CNT = 2;
        public const int NFLI_BAR_CODE_MODULE_IDX1 = 0;
        public const int NFLI_BAR_CODE_MODULE_IDX2 = 1;

        public const string NFLI_SYS_RCP_BACKUP_DIR = "NFLIRB";

        public const string NFLI_FILE_INVALID_CHAR = "\\/:*?\"<>|";
        public const string NFLI_DIR_INVALID_CHAR = "\\/:*?\"<>|";
        public const string NFLI_DIR_PATH_TRIM = " \n\t\r\b";
        public const string NFLI_RCP_TXT_SEPARATOR = "=/";
        public const string NFLI_RCP_TXT_TRIM = " \n\t\r\b";
        public const string NFLI_DINF_TXT_SEPARATOR = "/";
        public const string NFLI_TYPE_RCP_TXT_SEPARATOR = ":\n\t\r\b";
        public const string NFLI_TYPE_RCP_TXT_SEPARATOR_II = ";";

        public const string NFLI_TXT_ERROR = "ERROR";
        public const string NFLI_TXT_E_NFLI_ALREADY_RUN = "NFLIMAN is already running";
        public const string NFLI_TXT_E_CANNOT_CREATE_LOG_DIR = "NFLIMAN can't create system log directory";
        public const string NFLI_TXT_E_ENABLE_AXIS = "Fail! 'enable axis'";
        public const string NFLI_TXT_E_JOG_PLUS = "Fail! 'jog +'";
        public const string NFLI_TXT_E_JOG_MINUS = "Fail! 'jog -'";
        public const string NFLI_TXT_E_JOG_STOP = "Fail! 'jog stop'";
        public const string NFLI_TXT_E_AXIS_MOVE = "Fail! 'axis move'";
        public const string NFLI_TXT_E_AXIS_HOME = "Fail! 'axis homing'";
        public const string NFLI_TXT_E_AXIS_STOP = "Fail! 'axis stop'";
        public const string NFLI_TXT_E_DIO_OUT_BIT = "Fail! 'dio out bit'";
        public const string NFLI_TXT_E_READ_BAR_CODE = "Fail! 'bar code read'";
        public const string NFLI_TXT_E_FAIL_QUEST = "Fail! quest";
        public const string NFLI_TXT_E_CANNOT_INIT_SERVER = "NFLIMan can't initialize TCP/IP server";
        public const string NFLI_TXT_E_CANNOT_LOAD_WS2_32 = "NFLIMan can't load WS2_32.DLL";
        public const string NFLI_TXT_E_AXIS_INIT = "NFLIMan can't initialize '%s'";
        public const string NFLI_TXT_E_MON_THREAD_INIT = "NFLIMan can't initialize 'Monitor Thread'";
        public const string NFLI_TXT_E_SEQ_THREAD_INIT = "NFLIMan can't initialize 'Sequence Thread'";
        public const string NFLI_TXT_E_DATA_ARRANGE_THREAD_INIT = "NFLIMan can't initialize 'Data Arrange Thread'";
        public const string NFLI_TXT_E_MON_THREAD_RUN = "NFLIMan can't run 'Monitor Thread'";
        public const string NFLI_TXT_E_SEQ_THREAD_RUN = "NFLIMan can't run 'Sequence Thread'";
        public const string NFLI_TXT_E_DATA_ARRANGE_THREAD_RUN = "NFLIMan can't run 'Data Arrange Thread'";
        public const string NFLI_TXT_E_CANNOT_CREATE_LOCAL_DATA_RECORD_DIR = "NFLIMan can't create 'local data record directory'";
        public const string NFLI_TXT_E_CAM_INIT = "카메라 설정 초기화를 실패하였습니다.";
        public const string NFLI_TXT_E_VISION_INIT = "NFLIMan can't initialize 'Vision S/W'";
        public const string NFLI_TXT_E_LIGHT_INIT = "NFLIMan can't initialize 'Light Controller'";
        public const string NFLI_TXT_E_BAR_CODE_READER_IF_INIT = "NFLIMan can't initialize 'Bar Code Reader Interface'";
        public const string NFLI_TXT_E_BAR_CODE_READER_SW_CREATE = "NFLIMan can't create 'Bar Code Reader SW'";
        public const string NFLI_TXT_E_DATA_SERVER_TRANS_IF_INIT = "NFLIMan can't initialize 'Data Server Transfer Interface'";
        public const string NFLI_TXT_E_DATA_SERVER_TRANS_IF_CREATE = "NFLIMan can't create 'Data Server Transfer Interface'";
        public const string NFLI_TXT_E_LAMD_INIT = "NFLIMan can't initialize 'Lens Assy Machine DB'";
        public const string NFLI_TXT_E_RCP_CALC_PALLET_POS = "NFLIMan can't calc pallet pos in recipe";
        public const string NFLI_TXT_E_RCP_CALC_TRAY_POS = "NFLIMan can't calc tray pos in recipe";
        public const string NFLI_TXT_E_PALLET_BAR_CODE_LENGTH_INVALID = "Pallet barcode characters length is invalid!\nRetry to key-in";
        public const string NFLI_TXT_E_TRAY_BAR_CODE_LENGTH_INVALID = "Tray barcode characters length is invalid!\nRetry to key-in";

        public const string NFLI_TXT_E_EZI_AXIS_INIT = "NFLIMan can't initialize Ezi Motion";

        public const string NFLI_TXT_QUESTION = "QUESTION";
        public const string NFLI_TXT_Q_APPLY_CONFIG = "Apply config?";
        public const string NFLI_TXT_Q_REFRESH_MOTION_DEFINE = "Refresh motion defines?";
        public const string NFLI_TXT_Q_REFRESH_IO_DEFINE = "Refresh io defines?";
        public const string NFLI_TXT_Q_DO_YOU_READY = "Do you ready?";
        public const string NFLI_TXT_Q_NOW_STOPPING_DO_YOU_FORCE_STOP = "Stopping now, do you force the stop?";
        public const string NFLI_TXT_Q_DO_YOU_STOP = "Do you stop?";
        public const string NFLI_TXT_Q_DO_YOU_RUN = "Click 'YES', if you want to run.";
        public const string NFLI_TXT_Q_DO_YOU_PAUSE = "Do you pause?";
        public const string NFLI_TXT_Q_DO_YOU_UNLOCK_PAUSE = "Do you end a pause?";
        public const string NFLI_TXT_Q_DO_YOU_QUIT = "Do you quit?";
        public const string NFLI_TXT_Q_DO_YOU_SOLDER = "Do you solder?";
        public const string NFLI_TXT_Q_DO_YOU_SOLDER_CLEAN = "Do you clean solder?";
        public const string NFLI_TXT_Q_CONFIRM_USE_INPUT_BARCODE = "Click 'YES', if bar code is correct.\n'%s'";
        public const string NFLI_TXT_Q_APPLY_RECIPE = "Caution! Now Running.\nClick 'YES', if apply modified recipe.";
        public const string NFLI_TXT_Q_DO_YOU_CREATE_NEW_FAIL_RECIPE = "Do you create new fail recipe?";
        public const string NFLI_TXT_Q_DO_YOU_CREATE_NEW_TYPE_RECIPE = "Do you create new type recipe?";
        public const string NFLI_TXT_Q_DO_YOU_LOG_OUT = "Log-Off 하시겠습니까?";

        public const string NFLI_TXT_NOTIFY = "NOTIFY";
        public const string NFLI_TXT_N_MOTION_DEFINE_REFRESH_COMPLETE = "'MOTION DEFINE REFRESH' complete!";
        public const string NFLI_TXT_N_IO_DEFINE_REFRESH_COMPLETE = "'IO DEFINE REFRESH' complete!";
        public const string NFLI_TXT_N_TRAY_LOAD_CYLINDER_BACKWARD_FIRST = "Tray load cylinder backward first!";
        public const string NFLI_TXT_N_TRAY_UNLOAD_CYLINDER_BACKWARD_FIRST = "Tray unload cylinder backward first!";
        public const string NFLI_TXT_N_PASS_MAGAZINE_Z_AXIS_HOME_FIRST = "'양품 메거진 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_FAIL_MAGAZINE_Z_AXIS_HOME_FIRST = "'불량 메거진 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_LOADER_MAGAZINE_Z_AXIS_HOME_FIRST = "'로더부 메거진 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNLOADER_MAGAZINE_Z_AXIS_HOME_FIRST = "'언로더부 메거진 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_PALLET_X_AXIS_HOME_FIRST = "'파레트 X축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_SUPPLY_Y_AXIS_HOME_FIRST = "'공급 Y축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_SORTER_X_AXIS_HOME_FIRST = "'소터 X축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_SORTER_Y_AXIS_HOME_FIRST = "'소터 Y축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_OVER_CAM1_Z_AXIS_HOME_FIRST = "'상부카메라1 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_OVER_CAM2_Z_AXIS_HOME_FIRST = "'상부카메라2 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_OVER_CAM3_Z_AXIS_HOME_FIRST = "'상부카메라3 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_OVER_LIGHT1_Z_AXIS_HOME_FIRST = "'상부조명1 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_OVER_LIGHT2_Z_AXIS_HOME_FIRST = "'상부조명2 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_OVER_LIGHT3_Z_AXIS_HOME_FIRST = "'상부조명3 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNDER_CAM1_Z_AXIS_HOME_FIRST = "'하부카메라1 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNDER_CAM2_Z_AXIS_HOME_FIRST = "'하부카메라2 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNDER_CAM3_Z_AXIS_HOME_FIRST = "'하부카메라3 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNDER_LIGHT1_Z_AXIS_HOME_FIRST = "'하부조명1 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNDER_LIGHT2_Z_AXIS_HOME_FIRST = "'하부조명2 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_UNDER_LIGHT3_Z_AXIS_HOME_FIRST = "'하부조명3 Z축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_INDEX_T_AXIS_HOME_FIRST = "'인덱스 T축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_AXIS_HOME_FIRST = "'%s축'의 Homing 진행을 먼저 하세요!";
        public const string NFLI_TXT_N_SORTER_X_AXIS_MOVE_SAFE_POS_FIRST = "'소터 X축'을 안전위치로 먼저 이동시키세요!";
        public const string NFLI_TXT_N_SORTER_Y_AXIS_MOVE_SAFE_POS_FIRST = "'소터 Y축'을 안전위치로 먼저 이동시키세요!";
        public const string NFLI_TXT_N_PASS_MAGAZINE_Z_AXIS_MOVE_SAFE_POS_FIRST = "'양품 메거진 Z축'을 안전위치로 먼저 이동시키세요!";
        public const string NFLI_TXT_N_FAIL_MAGAZINE_Z_AXIS_MOVE_SAFE_POS_FIRST = "'불량 메거진 Z축'을 안전위치로 먼저 이동시키세요!";
        public const string NFLI_TXT_N_LOAD_MAGAZINE_CYLINDER_BACKWARD_FIRST = "Backward 'load magazine cylinder' first!";
        public const string NFLI_TXT_N_UNLOAD_MAGAZINE_CYLINDER_BACKWARD_FIRST = "Backward 'unload magazine cylinder' first!";
        public const string NFLI_TXT_N_CAM_Z_AXIS_UP_FIRST = "Camera z axis up to '%.3f ~ %.3f' first!";
        public const string NFLI_TXT_N_LIGHT_Z_AXIS_UP_FIRST = "Light z axis up to '%.3f ~ %.3f' first!";
        public const string NFLI_TXT_N_CANNOT_BACK_LIGHT_BLOW_OFF_IN_RUN = "NFLIMAN can't turn off back light blow in running";
        public const string NFLI_TXT_N_CANNOT_IONIZER_OFF_IN_RUN = "NFLIMAN can't turn off ionizer in running";
        public const string NFLI_TXT_N_CANNOT_MAIN_POWER_OFF_IN_RUN = "NFLIMAN can't turn off main power in running";
        public const string NFLI_TXT_N_CANNOT_LIGHT_REMOTE_OFF_IN_RUN = "NFLIMAN can't turn off light remote in running";
        public const string NFLI_TXT_N_MOVE_POS_INVALID_A3 = "Move range of '%s' is %.3f ~ %.3f";
        public const string NFLI_TXT_N_AXIS_HOME_COMPLETE = "Axis homing complete!";
        public const string NFLI_TXT_N_AXIS_MOVE_COMPLETE = "Axis move complete!";
        public const string NFLI_TXT_N_AXIS_STOP_COMPLETE = "Axis stop complete!";
        public const string NFLI_TXT_N_READ_BAR_CODE_COMPLETE = "Read bar code complete!";
        public const string NFLI_TXT_N_STOP_RUNNING_FIRST = "Stop Running first!";
        public const string NFLI_TXT_N_NOW_RUNNING = "Runing now!";
        public const string NFLI_TXT_N_SELECT_TRY_OR_ALL = "Click 'Yes' for selected tray, 'No' for all hole";
        public const string NFLI_TXT_N_SELECT_TRAY = "Select tray";
        public const string NFLI_TXT_N_CANNOT_OPEN_RECIPE = "NFLIMan can't open '%s' recipe file";
        public const string NFLI_TXT_N_CANNOT_SAVE_RECIPE = "NFLIMan can't save '%s' recipe file";
        public const string NFLI_TXT_N_CANNOT_OPEN_TYPE_RECIPE = "NFLIMan can't open '%s' type recipe file";
        public const string NFLI_TXT_N_CANNOT_SAVE_TYPE_RECIPE = "NFLIMan can't save '%s' type recipe file";
        public const string NFLI_TXT_N_CANNOT_OPEN_FAIL_RECIPE = "NFLIMan can't open '%s' fail recipe file";
        public const string NFLI_TXT_N_CANNOT_SAVE_FAIL_RECIPE = "NFLIMan can't save '%s' fail recipe file";
        public const string NFLI_TXT_N_INVALID_TYPE_RECIPE = "디펙구분 레시피가 유효하지 않습니다.\n'%s'";
        public const string NFLI_TXT_N_INVALID_FAIL_RECIPE = "불량판정 레시피가 유효하지 않습니다.\n'%s'";
        public const string NFLI_TXT_N_STEP_TIME_OVER = "%s 에서 '스텝 제한 시간 초과'가 발생되었습니다.\r\n해당 모듈을 확인 한 후, '확인'버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_FAIL_PALLET_BAR_CODE_READ = "작업파레트 바코드 리딩 실패가 발생되었습니다.\n바코드를 입력 한 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_FAIL_PASS_TRAY_BAR_CODE_READ = "양품트레이 바코드 리딩 실패가 발생되었습니다.\n바코드를 입력 한 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_FAIL_FAIL_TRAY_BAR_CODE_READ = "불량트레이 바코드 리딩 실패가 발생되었습니다.\n바코드를 입력 한 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_MOVE_AXIS_SAFE_POS_FIRST = "Move '%s' in '%.3f ~ %.3f' first!";
        public const string NFLI_TXT_N_DIO_OUT_CHANGED = "I/O출력 '%d'번의 상태가, '일시정지'시점의 상태와 동일하지 않습니다.\r\n해당 I/O를 복원한 후 '일시정지해제'를 재시도 하세요.";
        public const string NFLI_TXT_N_DETECT_DOOR_OPEN = "'%s' 도어 열림이 감지되었습니다. 도어를 닫은 후 '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHANGE_PASS_MAGAZINE = "양품트레이 메거진 교체가 필요합니다.\n교체 후 '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHANGE_FAIL_MAGAZINE = "불량트레이 메거진 교체가 필요합니다.\n교체 후 '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHANGE_LOADER_MAGAZINE = "로더부 메거진 교체가 필요합니다.\n메거진 교체 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHANGE_UNLOADER_MAGAZINE = "언로더부 메거진 교체가 필요합니다.\n메거진 교체 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHECK_PASS_MAGAZINE = "양품트레이 메거진이 감지되지 않습니다.\n메거진을 점검한 후 '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHECK_FAIL_MAGAZINE = "불량트레이 메거진이 감지되지 않습니다.\n메거진을 점검한 후 '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHECK_LOADER_MAGAZINE = "로더부 메거진이 감지되지 않습니다.\n메거진 확인 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_CHECK_UNLOADER_MAGAZINE = "언로더부 메거진이 감지되지 않습니다.\n메거진 확인 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_REMOVE_INSERT_POS_LENS = "인덱스 투입부에 제품이 감지되고 있습니다.\n 제품 제거 후, '확인' 버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_FAIL_PICK_UP_PALLET_LENS = "'작업부 파레트에서 제품 픽업 실패가 발생되었습니다.\n재시도 하려면 '확인'버튼을, 스킵하려면 '취소'버튼을 눌러 주세요.";
        public const string NFLI_TXT_N_FAIL_PICK_UP_CLEANER_LENS = "클리너에서 바렐 픽업을 실패하였습니다.\n피커에 진공이 감지되지 않거나, 클리너에 바렐이 감지되고 있습니다.\r\n재시도 하려면 '확인' 버튼을, 무시하려면 제품을 제거한 후 '취소' 버튼을 눌러주세요.";
        public const string NFLI_TXT_N_FAIL_IMPORT_PALLET_FROM_DB = "DB에서 파레트 정보를 가져올 수 없습니다.\n재시도 하려면 '확인' 버튼을, 무시하려면 '취소' 버튼을 눌러 주세요.";

        public const string NFLI_TXT_L_LOG_ON = "[관리자 Log-On]";
        public const string NFLI_TXT_L_LOG_OUT = "[관리자 Log-Off]";
        public const string NFLI_TXT_L_CHANGE_CONFIG = "[CONFIG CHANGED]";
        public const string NFLI_TXT_L_EXIT_PROGRAM = "[EXIT PROGRAM]";
        public const string NFLI_TXT_L_PROGRAM_START = "[PROGRAM START]";
        public const string NFLI_TXT_L_MODIFY_RECIPE = "[MODIFY RECIPE]";
        public const string NFLI_TXT_L_USER_INPUT_PALLET_BAR_CODE = "[USER INPUT PALLET BARCODE] %s";
        public const string NFLI_TXT_L_USER_INPUT_PASS_TRAY_BAR_CODE = "[USER INPUT PASS TRAY BARCODE] %s";
        public const string NFLI_TXT_L_USER_INPUT_FAIL_TRAY_BAR_CODE = "[USER INPUT FAIL TRAY BARCODE] %s";
        public const string NFLI_TXT_L_FAIL_PAUSE_PREPROCESS = "[PAUSE] Preprocess fail";
        public const string NFLI_TXT_L_FAIL_PAUSE_END_PREPROCESS = "[PAUSE END] Preprocess fail";
        public const string NFLI_TXT_L_RUN_FINISH_INFO = "[RUN FINISH INFO]";
        public const string NFLI_TXT_L_RUN_OPTION = "[가동 옵션]";
        public const string NFLI_TXT_L_USE_BAR_CODE_KEY_IN = "#바코드 Key-In 사용:";
        public const string NFLI_TXT_L_USE_TRANSFER_DATA = "#데이타 전송 사용: ";
        public const string NFLI_TXT_L_USE_BAR_CODE_READER = "#바코드 리더 사용: ";
        public const string NFLI_TXT_L_USE_MAGAZINE = "#메거진 사용: ";
        public const string NFLI_TXT_L_USE_TRAY = "#트레이 사용: ";
        public const string NFLI_TXT_L_USE_PALLET = "#파레트 사용: ";
        public const string NFLI_TXT_L_USE_BARREL = "#바렐 사용: ";
        public const string NFLI_TXT_L_USE_VISION = "#비젼 사용: ";
        public const string NFLI_TXT_L_USE_SAVE_IMG = "#원본 이미지 저장: ";
        public const string NFLI_TXT_L_USE_BUZZER = "#경보음 사용: ";
        public const string NFLI_TXT_L_USE_OVER_FAIL_ALARM = "#불량률 초과 알람 사용: ";
        public const string NFLI_TXT_L_USE_DOOR_CLOSE = "#도어 열림 감지 사용: ";
        public const string NFLI_TXT_L_MODEL = "#모델명: ";

        public const string NFLI_TXT_SW_TITLE = "Nexstar Lens Inspector SYSTEM SW";
        public const string NFLI_TXT_MOTION = "MOTION";
        public const string NFLI_TXT_DELAY_TIME = "DELAY TIME";
        public const string NFLI_TXT_LIMIT_TIME = "LIMIT TIME";
        public const string NFLI_TXT_SOLDER_IRON = "SOLDER IRON";
        public const string NFLI_TXT_BAR_CODE = "BAR CODE";
        public const string NFLI_TXT_ETC = "ETC";
        public const string NFLI_TXT_CAM = "CAMERA";
        public const string NFLI_TXT_LIGHT = "LIGHT";
        public const string NFLI_TXT_AXIS_ETC = "AXIS ETC";
        public const string NFLI_TXT_MOTION_DEFINE_REFRESH = "MOTION DEFINE REFRESH";
        public const string NFLI_TXT_IO_DEFINE_REFRESH = "IO DEFINE REFRESH";
        public const string NFLI_TXT_AXIS_MOVE_VELOCITY = "Move velocity=mm/sec;";
        public const string NFLI_TXT_AXIS_JOG_VELOCITY = "Jog velocity=mm/sec;";
        public const string NFLI_TXT_AXIS_HOME_VELOCITY = "Homing velocity=mm/sec;";
        public const string NFLI_TXT_AXIS_MOVE_ACCEL_SCALE = "Move accel scale";
        public const string NFLI_TXT_AXIS_JOG_ACCEL_SCALE = "Jog accel scale";
        public const string NFLI_TXT_AXIS_HOME_ACCEL_SCALE = "Homing accel scale";
        public const string NFLI_TXT_AXIS_ETC_SORTER_MAGAZINE_INTERFERENCE = "양/불 메거진 간섭 장비";
        public const string NFLI_TXT_AXIS_ETC_SORTER_AVOID_POS_X = "소터 피커 X축 회피 위치=mm;";
        public const string NFLI_TXT_AXIS_ETC_SORTER_AVOID_POS_Y = "소터 피커 Y축 회피 위치=mm;";
        public const string NFLI_TXT_AXIS_ETC_PASS_MAGAZINE_AVOID_POS_Z = "양품메거진 Z축 회피 위치=mm;";
        public const string NFLI_TXT_AXIS_ETC_FAIL_MAGAZINE_AVOID_POS_Z = "불량메거진 Z축 회피 위치=mm;";
        public const string NFLI_TXT_AXIS_ETC_PASS_MAGAZINE_START_POS_Z = "Pass Magazine Z Axis Start Pos.=mm;";
        public const string NFLI_TXT_AXIS_ETC_PASS_MAGAZINE_SENSOR_GAP_Z = "Pass Magazine Z Axis Sensor Gap=mm;";
        public const string NFLI_TXT_AXIS_ETC_FAIL_MAGAZINE_START_POS_Z = "Fail Magazine Z Axis Start Pos.=mm";
        public const string NFLI_TXT_AXIS_ETC_FAIL_MAGAZINE_SENSOR_GAP_Z = "Fail Magazine Z Axis Sensor Gap=mm;";
        public const string NFLI_TXT_AXIS_ETC_PF_MAGAZINE_INTERVAL_Z = "PF Magazine Z Axis Interval=mm;";
        public const string NFLI_TXT_AXIS_ETC_PF_MAGAZINE_TRAY_CNT = "Tray Count In PF Magazine";
        public const string NFLI_TXT_AXIS_ETC_LOADER_MAGAZINE_START_POS_Z = "Loader Magazine Z Axis Start Pos.=mm;";
        public const string NFLI_TXT_AXIS_ETC_LOADER_MAGAZINE_SENSOR_GAP_Z = "Loader Magazine Z Axis Sensor Gap=mm;";
        public const string NFLI_TXT_AXIS_ETC_UNLOADER_MAGAZINE_START_POS_Z = "Unloader Magazine Z Axis Start Pos.=mm";
        public const string NFLI_TXT_AXIS_ETC_UNLOADER_MAGAZINE_SENSOR_GAP_Z = "Unloader Magazine Z Axis Sensor Gap=mm;";
        public const string NFLI_TXT_AXIS_ETC_INSP_MAGAZINE_INTERVAL_Z = "Insp. Magazine Z Axis Interval=mm;";
        public const string NFLI_TXT_AXIS_ETC_INSP_MAGAZINE_PALLET_CNT = "Pallet Count In Insp. Magazine";
        public const string NFLI_TXT_AXIS_ETC_PASS_TRAY_BAR_CODE_POS_X = "Pass Tray Bar Code Reading Pos. X=mm;";
        public const string NFLI_TXT_AXIS_ETC_PASS_TRAY_BAR_CODE_POS_Y = "Pass Tray Bar Code Reading Pos. Y=mm;";
        public const string NFLI_TXT_AXIS_ETC_FAIL_TRAY_BAR_CODE_POS_X = "Fail Tray Bar Code Reading Pos. X=mm;";
        public const string NFLI_TXT_AXIS_ETC_FAIL_TRAY_BAR_CODE_POS_Y = "Fail Tray Bar Code Reading Pos. Y=mm;";
        public const string NFLI_TXT_AXIS_ETC_INDEX_INTERVAL_T = "Index T Axis Interval=mm;";
        public const string NFLI_TXT_AXIS_ETC_INDEX_STEP_CNT = "Hole Count In Index";
        public const string NFLI_TXT_AXIS_ETC_INDEX_RESET_STEP_CNT = "Step Count For Index Pos. Reset";
        public const string NFLI_TXT_AXIS_ETC_INDEX_USE_HOLE_MASK = "Index Hole Bit Mask For Use=use:1, unuse: 0, First Hole: left;";
        public const string NFLI_TXT_DT_PICKER_DOWN_SENS_MSEC = "피커 하강=msec;";
        public const string NFLI_TXT_DT_PICKER_VACCUM_MSEC = "피커 진공=msec;";
        public const string NFLI_TXT_DT_PICKER_BLOW_MSEC = "피커 파기=msec;";
        public const string NFLI_TXT_DT_PALLET_SENS_MSEC = "파레트 감지=msec; : 버퍼부 && 작업부";
        public const string NFLI_TXT_DT_UNLOADER_PALLET_SENS_MSEC = "파레트 감지=msec; : 언로더부";
        public const string NFLI_TXT_DT_PALLET_STOPPER_UP_SENS_MSEC = "파레트 스토퍼 상승=msec;";
        public const string NFLI_TXT_DT_PALLET_CYLINDER_UP_SENS_MSEC = "파레트 실린더 상승=msec;";
        public const string NFLI_TXT_DT_PALLET_CYLINDER_DOWN_SENS_MSEC = "파레트 실린더 하강=msec;";
        public const string NFLI_TXT_DT_PALLET_CLAMP_SENS_MSEC = "파레트 클램프 클램핑=msec;";
        public const string NFLI_TXT_DT_LIGHT_CHANGE_MSEC = "정면 조명 밝기 변경=msec;";
        public const string NFLI_TXT_DT_BACKLIGHT_CHANGE_MSEC = "후면 조명 밝기 변경=msec;";
        public const string NFLI_TXT_DT_BARREL_SENS_MSEC = "바렐 감지=msec;";
        public const string NFLI_TXT_DT_BARREL_CLEAN_MSEC = "바렐 에어 세척=msec;";
        public const string NFLI_TXT_DT_MAGAZINE_PALLET_SENS_MSEC = "메거진 파레트=트레이; 감지=msec;";
        public const string NFLI_TXT_DT_TRAY_CYLINDER_FWD_MSEC = "트레이 실린더 전진=msec;";
        public const string NFLI_TXT_DT_PALLET_HOOK_UP_MSEC = "파레트 후크 상승=msec;";
        public const string NFLI_TXT_DT_PALLET_HOOK_DOWN_MSEC = "파레트 후크 하강=msec;";
        public const string NFLI_TXT_DT_INDEX_PICKER_FWD_MSEC = "인덱스 피커 전진=msec;";
        public const string NFLI_TXT_DT_INDEX_PICKER_BWD_MSEC = "인덱스 피커 후진=msec;";
        public const string NFLI_TXT_DT_CAM_Z_AXIS_MOVE_MSEC = "카메라 Z축 이동=msec;";
        public const string NFLI_TXT_DT_INDEX_T_AXIS_MOVE_MSEC = "인덱스 T축 이동=msec;";
        public const string NFLI_TXT_LT_VISION_PROC_SEC = "Vision Process=sec;";
        public const string NFLI_TXT_LT_VISION_RESPONSE_MSEC = "Vision Response=msec;";
        public const string NFLI_TXT_LT_SEQ_STEP_SEC = "Sequence Step=sec;";
        public const string NFLI_TXT_LT_PICKER_DOWN_MSEC = "피커 하강 감지=msec;";
        public const string NFLI_TXT_LT_PICKER_VACCUM_MSEC = "피커 진공 감지=msec;";
        public const string NFLI_TXT_SI_CLEAN_CYCLE_MSEC = "Cleaning Cycle=msec;";
        public const string NFLI_TXT_SI_CLEAN_REPEAT_CNT = "Repeat CNT. at Cleaning";
        public const string NFLI_TXT_SI_CLEAN_POS_X = "Cleaning POS. X=mm;";
        public const string NFLI_TXT_SI_CLEAN_POS_Y = "Cleaning POS. Y=mm;";
        public const string NFLI_TXT_SI_CLEAN_POS_Z = "Cleaning POS. Z=mm;";
        public const string NFLI_TXT_SI_CLEAN_READY_POS_Z = "Cleaning Ready POS. Z=mm;";
        public const string NFLI_TXT_SI_CAM_OFFSET_X = "Offset X From CAM. Center=mm;";
        public const string NFLI_TXT_SI_CAM_OFFSET_Y = "Offset Y From CAM. Center=mm;";
        public const string NFLI_TXT_SI_CLEAN_LENS_CNT = "Cleaning Per Lens CNT.";
        public const string NFLI_TXT_SI_Z_AXIS_VELOCITY = "Z Axis Velocity At Soldering=mm/sec;";
        public const string NFLI_TXT_SI_LIGHT_Z_AXIS_WAIT_POS = "Light Wait POS. Z=mm;";
        public const string NFLI_TXT_BC_COM_PORT_NO = "COM Port";
        public const string NFLI_TXT_BC_BAUD_RATE = "Baud Rate";
        public const string NFLI_TXT_BC_DATA_BITS = "Data Bits";
        public const string NFLI_TXT_BC_PARITY = "Parity";
        public const string NFLI_TXT_BC_STOP_BITS = "Stop Bits";
        public const string NFLI_TXT_BC_READ_REPEAT_CNT = "Retry CNT. Of Bar Code Reading";
        public const string NFLI_TXT_FS_SERVER_ADD = "Server Address";
        public const string NFLI_TXT_FS_ID = "ID.";
        public const string NFLI_TXT_FS_PW = "PW.";
        public const string NFLI_TXT_FS_RESULT_DATA_DIR = "Result Data DIR.";
        public const string NFLI_TXT_FS_STATUS_DATA_DIR = "Status Data DIR.";
        public const string NFLI_TXT_ETC_MACHINE_STAND_ALONE = "Machine Stand Alone";
        public const string NFLI_TXT_ETC_MACHINE_ID = "Machine ID";
        public const string NFLI_TXT_ETC_VISION_SERVER_PORT = "Vision Server TCP/IP Port";
        public const string NFLI_TXT_ETC_MAGAZINE_DATA_MAINT_CNT = "Magazine Data Mastring. Count";
        public const string NFLI_TXT_ETC_PALLET_DATA_MAINT_CNT = "Pallet Data Mastring. Count";
        public const string NFLI_TXT_ETC_USE_INDEX_SENSOR = "Use Index Sensor";
        public const string NFLI_TXT_ETC_PALLET_BAR_CODE_LENGTH = "Pallet Bar Code Character Length";
        public const string NFLI_TXT_ETC_TRAY_BAR_CODE_LENGTH = "Tray Bar Code Character Length";
        public const string NFLI_TXT_ETC_OVER_INSP_LIGHT_Z_AXIS_HOMING_LENS_CNT = "Lens Cnt For Over Insp. Light Z Axis Homing";
        public const string NFLI_TXT_ETC_UNDER_INSP_LIGHT_Z_AXIS_HOMING_LENS_CNT = "Lens Cnt For Under Insp. Light Z Axis Homing";
        public const string NFLI_TXT_ETC_LOCAL_DATA_RECORD_DIR = "Local Data Record Directory";
        public const string NFLI_TXT_ETC_BAR_CODE_READ_SW_PATH = "Bar Code Read S/W Path";
        public const string NFLI_TXT_ETC_VISION_SW_PATH = "Vision S/W Path";
        public const string NFLI_TXT_ETC_WITH_WORK_PALLET_CLAMP = "작업부 파레트 클램프 포함 장비";
        public const string NFLI_TXT_ETC_PALLET_PICK_UP_FAIL_LIMIT_CNT = "파레트 언로딩 픽업 연속 실패 횟수";
        public const string NFLI_TXT_CAM_MIL_SYS_NAME = "그랩보드 모델";
        public const string NFLI_TXT_CAM_MIL_BOARD_IDX = "그랩보드 번호";
        public const string NFLI_TXT_CAM_MIL_BOARD_NO = "그랩 보드";
        public const string NFLI_TXT_CAM_MIL_DIGT_IDX = "디지타이져";
        public const string NFLI_TXT_CAM_WIDTH_HEIGHT = "해상도 X / Y=pixel;";
        public const string NFLI_TXT_CAM_BAYER = "BAYER 타입";
        public const string NFLI_TXT_CAM_RESOLUTION = "분해능=um;";
        public const string NFLI_TXT_CAM_MIL_DCF = "DCF 파일";
        public const string NFLI_TXT_CMD_FAIL = "Command Fail!";
        public const string NFLI_TXT_BUZZER_OFF_DLG = "Push 'OK' for Buzzer Off";
        public const string NFLI_TXT_LOADER_MAGAZINE_MODULE = "'로더부 메거진 모듈'";
        public const string NFLI_TXT_PALLET_LOADER_MODULE = "'파레트 로더 모듈'";
        public const string NFLI_TXT_PALLET_PICKER_MODULE = "'파레트 피커 모듈'";
        public const string NFLI_TXT_CLEANER_INSERT_MODULE = "'클리너 투입 모듈'";
        public const string NFLI_TXT_CLEANER_MODULE = "'클리너 모듈'";
        public const string NFLI_TXT_INDEX_INSERT_MODULE = "'인덱스 투입 모듈'";
        public const string NFLI_TXT_INDEX_MODULE = "'인덱스 모듈'";
        public const string NFLI_TXT_INSPECT_OVER1_MODULE = "'상부검사부1 모듈'";
        public const string NFLI_TXT_INSPECT_OVER2_MODULE = "'상부검사부2 모듈'";
        public const string NFLI_TXT_INSPECT_OVER3_MODULE = "'상부검사부3 모듈'";
        public const string NFLI_TXT_INSPECT_UNDER1_MODULE = "'하부검사부1 모듈'";
        public const string NFLI_TXT_INSPECT_UNDER2_MODULE = "'하부검사부2 모듈'";
        public const string NFLI_TXT_INSPECT_UNDER3_MODULE = "'하부검사부3 모듈'";
        public const string NFLI_TXT_SORTER_MODULE = "'소터 모듈'";
        public const string NFLI_TXT_PASS_MAGAZINE_MODULE = "'양품 메거진 모듈'";
        public const string NFLI_TXT_FAIL_MAGAZINE_MODULE = "'불량 메거진 모듈'";
        public const string NFLI_TXT_UNLOADER_MAGAZINE_MODULE = "'언로더부 메거진 모듈'";
        //public const string NFLI_TXT_BAR_CODE = "BAR CODE";
        public const string NFLI_TXT_ALL_MOUNT = "ALL MOUNT";
        public const string NFLI_TXT_USE = "사용";
        public const string NFLI_TXT_UNUSED = "사용안함";
        public const string NFLI_TXT_ENABLE_AXIS_FAIL = "%s ENABLE AXIS FAIL!";
        public const string NFLI_TXT_DISABLE_AXIS_FAIL = "%s DISABLE AXIS FAIL!";
        public const string NFLI_TXT_READY_AXIS_FAIL = "%s READY AXIS FAIL!";
        public const string NFLI_TXT_JOG_PLUS_FAIL = "%s JOG + FAIL!";
        public const string NFLI_TXT_JOG_MINUS_FAIL = "%s JOG - FAIL!";
        public const string NFLI_TXT_STOP_AXIS_FAIL = "%s STOP AXIS FAIL!";
        public const string NFLI_TXT_DIO_UNIT_IDX_NONE = "'%d' is none in I/O unit index range!";
        public const string NFLI_TXT_DIO_OUT_BIT_FAIL = "DIO OUTPUT BIT FAIL!";
    }
    
}
