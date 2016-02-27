using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemAlign
{
    class CNIAMConfig
    {
        //Axis 파라미터 개수 : Axis 카운트 계산
        double[] m_dArrayAxisMoveVelocity;
        double[] m_dArrayAxisJogVelocity;
        double[] m_dArrayAxisHomeVelocity;

        double[] m_dArrayAxisMoveAccelScale;
        double[] m_dArrayAxisJogAccelScale;
        double[] m_dArrayAxisHomeAccelScale;

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
        uint m_uDTPalletStopperUpSensMSec;
        uint m_uDTPalletCylinderUpSensMSec;
        uint m_uDTPalletCylinderDownSensMSec;
        uint m_uDTPalletClampSensMSec;
        uint m_uDTLightChangeMSec;
        uint m_uDTBacklightChangeMSec;
        uint m_uDTSideInspLightChangeMSec;
        uint m_uDTBarrelSensMSec;
        uint m_uDTBarrelCleanMSec;
        uint m_uDTMagazinePalletSensMSec;
        uint m_uDTTrayCylinderFwdMSec;
        uint m_uDTPalletHookUpMSec;
        uint m_uDTPalletHookDownMSec;
        uint m_uDTIndexPickerFwdMSec;
        uint m_uDTIndexPickerBwdMSec;
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
        uint m_uETCPalletBarCodeLength;
        uint m_uETCTrayBarCodeLength;
        string m_strETCLocalDataRecordDir;
        string m_strETCVisionSWPath;
        bool m_bETCWithWorkPalletClamp;
        string m_strETCAdminID;
        string m_strETCAdminPW;
        string m_strETCBarCodeReadSWPath;

        // Light : 개수 : NFLI_LIGHT_CTRL_CNT
        uint[] m_uArrayLightCtrlType;
        uint[] m_uArrayLightCtrlPort;
        uint[] m_uArrayLightCtrlNo;
        uint[] m_uArrayLightCtrlCh;


        public uint GetLightCtrlType(uint uLightCtrlIdx)
        {
            return this.m_uArrayLightCtrlType[uLightCtrlIdx];
        }

        public void SetLightCtrlType(uint uLightCtrlIdx, uint uType)
        {
            this.m_uArrayLightCtrlType[uLightCtrlIdx] = uType;
        }

        public uint GetLightCtrlPort(uint uLightCtrlIdx)
        {
            return this.m_uArrayLightCtrlPort[uLightCtrlIdx];
        }

        public void SetLightCtrlPort(uint uLightCtrlIdx, uint uPort)
        {
            this.m_uArrayLightCtrlPort[uLightCtrlIdx] = uPort;
        }

        public uint GetLightCtrlNo(uint uLightIdx)
        {
            return this.m_uArrayLightCtrlNo[uLightIdx];
        }

        public void SetLightCtrlNo(uint uLightIdx, uint uNo)
        {
            this.m_uArrayLightCtrlNo[uLightIdx] = uNo;
        }

        public uint GetLightCtrlChannel(uint uLightIdx)
        {
            return this.m_uArrayLightCtrlCh[uLightIdx];
        }

        public void SetCamResolution(uint uLightIdx, uint uCh)
        {
            this.m_uArrayLightCtrlCh[uLightIdx] = uCh;
        }

        // Cam : 개수 : NFLI_CAM_CNT        
        System.Drawing.Size[] m_szArrayCamSize;
        System.Drawing.Size[] m_szArrayCamResolution;


        public System.Drawing.Size GetCamResolution(uint uCamIdx)
        {
            return this.m_szArrayCamResolution[uCamIdx];
        }

        public void SetCamResolution(uint uCamIdx, System.Drawing.Size szCamResolution)
        {
            this.m_szArrayCamResolution[uCamIdx] = szCamResolution;
        }

        public System.Drawing.Size GetCamSize(uint uCamIdx)
        {
            return this.m_szArrayCamSize[uCamIdx];
        }

        public void SetCamSize(uint uCamIdx, System.Drawing.Size szCamSize)
        {
            this.m_szArrayCamSize[uCamIdx] = szCamSize;
        }

        uint[] m_uArrayCamWidth;
        uint[] m_uArrayCamHeight;

        double[] m_dArrayCamResolution;



        //Ezi Motion  : DWORD = int
        int m_iPortNo;
        int m_iBaudrate;

        

        public int ExiMotionBaudRate
        {
            get { return this.m_iBaudrate; }
            set { this.m_iBaudrate = value; }
        }

        public int EziMotionPortNo
        {
            get { return this.m_iPortNo; }
            set { this.m_iPortNo = value; }
        }
    }
}
