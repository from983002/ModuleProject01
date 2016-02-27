///////////////////////////////////// INFO //////////////////////////////////
// by Ki Back Woo in Nexstart R&D center
// emergency : wkb@nexstar21.com
// MSN : 
/////////////////////////////////////////////////////////////////////////////

// NSIARMaster.cs: interface for the CNSIARMaster class.
//
//////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemAlign
{
    public class CNIAMMaster
    {
	// Motion
	double[] m_dArrayAxisMoveVelocity = new double[CNIAMAxisDefine.NFLI_AXIS_CNT];
	double[] m_dArrayAxisJogVelocity = new double[CNIAMAxisDefine.NFLI_AXIS_CNT];
	double[] m_dArrayAxisHomeVelocity = new double[CNIAMAxisDefine.NFLI_AXIS_CNT];

	double[] m_dArrayAxisMoveAccelScale = new double[CNIAMAxisDefine.NFLI_AXIS_CNT];
	double[] m_dArrayAxisJogAccelScale = new double[CNIAMAxisDefine.NFLI_AXIS_CNT];
	double[] m_dArrayAxisHomeAccelScale = new double[CNIAMAxisDefine.NFLI_AXIS_CNT];

	double m_dAxisETCPassMagazineStartPosZ;
	double m_dAxisETCPassMagazineSensorGapZ;
	double m_dAxisETCFailMagazineStartPosZ;
	double m_dAxisETCFailMagazineSensorGapZ;
	double m_dAxisETCPFMagazineIntervalZ;
	uint m_uAxisETCPFMagazineTrayCnt;

	bool m_bAxisETCSorterMagazineInterference;
	double m_dAxisETCSorterAvoidPosX;
	double m_dAxisETCSorterAvoidPosY;
	double m_dAxisETCPassMagazineAvoidPosZ;
	double m_dAxisETCFailMagazineAvoidPosZ;

	double m_dAxisETCLoaderMagazineStartPosZ;
	double m_dAxisETCLoaderMagazineSensorGapZ;
	double m_dAxisETCUnloaderMagazineStartPosZ;
	double m_dAxisETCUnloaderMagazineSensorGapZ;
	double m_dAxisETCInspMagazineIntervalZ;
	uint m_uAxisETCInspMagazinePalletCnt;

	double m_dAxisETCIndexIntervalT;
	uint m_uAxisETCIndexStepCnt;
	uint m_uAxisETCIndexUseHoleMask;

	// Delay Time
	uint m_uDTPickerDownSensMSec;
	uint m_uDTPickerVaccumMSec;
	uint m_uDTPickerBlowMSec;
	uint m_uDTPalletSensMSec;
	uint m_uDTUnloaderPalletSensMSec;
	uint m_uDTPalletStopperUpSensMSec;
	uint m_uDTPalletCylinderUpSensMSec;
	uint m_uDTPalletCylinderDownSensMSec;
	uint m_uDTPalletClampSensMSec;
	uint m_uDTLightChangeMSec;
	uint m_uDTBacklightChangeMSec;
	uint m_uDTBarrelSensMSec;
	uint m_uDTBarrelCleanMSec;
	uint m_uDTMagazinePalletSensMSec;
	uint m_uDTTrayCylinderFwdMSec;
	uint m_uDTPalletHookUpMSec;
	uint m_uDTPalletHookDownMSec;
	uint m_uDTIndexPickerFwdMSec;
	uint m_uDTIndexPickerBwdMSec;
	uint m_uDTCamZAxisMoveMSec;
	uint m_uDTIndexTAxisMoveMSec;

	// Limit Time
	uint m_uLTVisionProcSec;
	uint m_uLTVisionResponseMSec;
	uint m_uLTSeqStepSec;
	uint m_uLTPickerDownMSec;
	uint m_uLTPickerVaccumMSec;

	// Bar Code
	uint m_uBCComPortNo;
	uint m_uBCBaudRate;
	uint m_uBCDataBits;
	char m_cBCParity;
	uint m_uBCStopBits;
	uint m_uBCReadRepeatCnt;

	// Etc
	string m_strETCMachineID;
	uint m_uETCMagazineDataMaintCnt;
	uint m_uETCPalletDataMaintCnt;
	string m_strETCLocalDataRecordDir;
	string m_strETCVisionSWPath;
	bool m_bETCWithWorkPalletClamp;
	uint m_uETCPalletPickUpFailLimitCnt;
	string m_strETCAdminID;
	string m_strETCAdminPW;

	// Light
	uint[] m_uArrayLightCtrlType = new uint[CNIAMCommon.NFLI_LIGHT_CTRL_CNT];
	uint[] m_uArrayLightCtrlPort= new uint[CNIAMCommon.NFLI_LIGHT_CTRL_CNT];
	uint[] m_uArrayLightCtrlNonew =new uint[CNIAMCommon.NFLI_LIGHT_CTRL_CNT];
	uint[] m_uArrayLightCtrlChnew =new uint[CNIAMCommon.NFLI_LIGHT_CTRL_CNT];

	// Cam
	uint[] m_uArrayCamWidth =new uint[CNIAMCommon.NFLI_CAM_CNT];
	uint[] m_uArrayCamHeight =new uint[CNIAMCommon.NFLI_CAM_CNT];
    double[] m_dArrayCamResolution = new double[CNIAMCommon.NFLI_CAM_CNT];

	//Ezi Motion
	int m_iPortNo;
	uint m_dwBaudrate;

    }
}
