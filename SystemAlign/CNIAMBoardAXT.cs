using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Threading;
using System.Runtime.InteropServices;

public enum AXT_INTERRUPT_CLASS : uint
{
    KIND_MESSAGE,
    KIND_CALLBACK,
    KIND_EVENT
}

enum AX_PORT_WID 
{ 
    AXW_EMPTY, 
    AXW_8, 
    AXW_16, 
    AXW_32 
};

enum INP_EVENT 
{ 
    IE_DISABLE, 
    IE_CHANGE, 
    IE_RISING, 
    IE_FALLING 
};

namespace SystemAlign
{
    class CNIAMBoardAXT
    {
        CNIAMMessageBoxView msgBox = new CNIAMMessageBoxView();
        private uint hInterrupt = 0;
        private Thread EventThread = null;
        private bool bThread = false;

        public readonly static uint INFINITE = 0xFFFFFFFF;
        public readonly static uint STATUS_WAIT_0 = 0x00000000;
        public readonly static uint WAIT_OBJECT_0 = ((STATUS_WAIT_0) + 0);
        [DllImport("kernel32", EntryPoint = "WaitForSingleObject", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern uint WaitForSingleObject(uint hHandle, uint dwMilliseconds);

        [DllImport("KERNEL32", EntryPoint = "SetEvent", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SetEvent(long hEvent);

        //AX_PORT_WID axPortWidth
        bool AxlOpened = false;
        int nModuleCount = 0;
        int nOutCount = 0;
        int nInpCount = 0;
        int[] nOutModMap;
        int[] nOutBitMap;
        int[] nInpModMap;
        int[] nInpBitMap;

        //EVT_DATA		CAxDio::m_OutTgtEvent[ AX_MAX_INP_PORT ];
        //EVT_DATA		CAxDio::m_InpTgtEvent[ AX_MAX_INP_PORT ];
        AX_PORT_WID[] mOutModWidth;
        AX_PORT_WID[] mInpModWidth;
        uint[] uOutMask;
        uint[] uInpMask;

        int[] nInpBitCount;
        int[] nOutBitCount;
        int[] nInpIdxNo;
        int[] nOutIdxNo;
        
        
        
        int[] nOutData;
        int[] nInpData;

        uint[] uOutEventMask;
        uint[] uInpEventMask;

        int[] nModuleID;
        int[] nModulePos;
        int[] nBoardNo;

        //Atx모듈이 오픈 상태를 가지는 변수.
        bool AxlOpenResult = false;
        int AxlIRQNo = 7;

        uint duRetCode,duState1,duState2;
                
        public System.Windows.Forms.Timer m_tmIOTick;
        public CNIAMBoardAXT()
        {

        }

        public int m_lAxisCounts = 0;                            // 제어 가능한 축갯수 선언 및 초기화
        private int m_lAxisNo = 0;                            // 제어할 축 번호 선언 및 초기화
        public uint m_uModuleID = 0;                            // 제어할 축의 모듈 I/O 선언 및 초기화 
        public int m_lBoardNo = 0, m_lModulePos = 0;


        public System.Collections.ArrayList MotionList
        {
            get { return this.m_alDioModuleList; }
        }

        public void initLibrary()
        {
            try
            {

                if ((CAXL.AxlIsOpened() == (int)AXT_FUNC_RESULT.AXT_RT_OPEN_ALREADY)) return;
                if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {

                    msgBox.ShowMessage("아젠보드 오류", "아젠 보드 초기화 오류", "현재 아젠 보드가 시스템에 장착되어 있지 않거나 정상적으로 인스톨되어 있지 않습니다.\r\n\r\n제어판에서 확인하십시요!");//("Intialize Fail..!!");
                    return;
                }                
                
                if (this.DigitalIO_Open() == false)
                {
                    msgBox.ShowMessage("AT 보드 오류", "AT 보드 DIO 오류",
                        "현재 DIO 보드가 시스템에 장착되어 있지 않거나 정상적으로 인스톨되어 있지 않습니다.\r\n\r\n제어판에서 확인하십시요!");
                }

                int uAxisCount = 0;
                
                uint upStatus = 0;

                upStatus = CAXM.AxmInfoGetAxisCount(ref uAxisCount);

                if (CAXM.AxmInfoIsMotionModule(ref upStatus) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    if (upStatus != (uint)AXT_EXISTENCE.STATUS_EXIST)
                    {
                        msgBox.ShowMessage("모션 모듈", "모션 모듈 에러.", "현재 모션 모듈을 찾지 못했습니다. 시스템을 확인해 주십시요!");
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                msgBox.ShowMessage("AT Main Control Board", "AT Main Control Board Initializing Error", "System에서 AT Main Control Board를 찾지 못했습니다. System의 내용을 \r\n\r\n제어판에서 확인하십시요!");
            }  
        }

        System.Collections.ArrayList m_alDioModuleList = new System.Collections.ArrayList();
        private bool DigitalIO_Open()
        {
            uint uStatus = 0;
            if (CAXD.AxdInfoIsDIOModule(ref uStatus) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if ((AXT_EXISTENCE)uStatus == AXT_EXISTENCE.STATUS_EXIST)
                {
                    int nModuleCount = 0;
                    if (CAXD.AxdInfoGetModuleCount(ref nModuleCount) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        short i = 0;
                        int nBoardNo = 0;
                        int nModulePos = 0;
                        uint uModuleID = 0;
                        string strData = "";
                        for (i = 0; i < nModuleCount; i++)
                        {
                            if (CAXD.AxdInfoGetModule(i, ref nBoardNo, ref nModulePos, ref uModuleID) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                            {
                                switch ((AXT_MODULE)uModuleID)
                                {
                                    case AXT_MODULE.AXT_SIO_DI32:
                                        strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32", nBoardNo, i);
                                        break;

                                    case AXT_MODULE.AXT_SIO_DO32P:
                                        strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32P", nBoardNo, i);
                                        break;

                                    case AXT_MODULE.AXT_SIO_DB32P:
                                        strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32P", nBoardNo, i);
                                        break;

                                    case AXT_MODULE.AXT_SIO_DO32T:
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T", nBoardNo, i);
                                            break;

                                    case AXT_MODULE.AXT_SIO_DB32T:
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32T", nBoardNo, i);
                                            break;

                                    case AXT_MODULE.AXT_SIO_RDI32:
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO_RDI32", nBoardNo, i);
                                            break;

                                    case AXT_MODULE.AXT_SIO_RDO32:
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO_RDO32", nBoardNo, i);
                                            break;
                                    case AXT_MODULE.AXT_SIO_RDB128MLII:
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO_RDO32", nBoardNo, i);
                                            break;
                                }
                                m_alDioModuleList.Add(strData);
                            }
                        }
                    }
                }
            }
            else
            {
                msgBox.ShowMessage("아젠 보드", "아젠 보드 DIO 모듈", "Module not exist.");
                return false;
            }
            return true;
        }





        private void ThreadProc()
        {
            while (bThread)
            {
                if (WaitForSingleObject(hInterrupt, INFINITE) == WAIT_OBJECT_0)
                {
                    int nModuleNo = 0;
                    uint uFlag = 0;
                    CAXD.AxdiInterruptRead(ref nModuleNo, ref uFlag);
                    this.InterruptProc(AXT_INTERRUPT_CLASS.KIND_EVENT, nModuleNo, uFlag);
                }
            }
            EventThread = null;
        }


        private bool InterruptProc(AXT_INTERRUPT_CLASS uClass, int nModuleNo, uint uFlag)
        {
            int i = 0;
            int j = 0;
            uint uValue = 0;
            string strClass = "";
            string strInt = "";

            switch (uClass)
            {
                case AXT_INTERRUPT_CLASS.KIND_MESSAGE:
                    strClass = "Message";
                    break;

                case AXT_INTERRUPT_CLASS.KIND_CALLBACK:
                    strClass = "Callback";
                    break;

                case AXT_INTERRUPT_CLASS.KIND_EVENT:
                    strClass = "Event";
                    break;
            }

            string textInterrupt = null;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if ((((uFlag >> (i * 8)) >> j) & 0x01) == 0x01)
                    {
                        CAXD.AxdiReadInportBit(nModuleNo, ((i * 8) + j), ref uValue);

                        if (uValue == 0x01)
                            strInt = String.Format("{0:s} : Rising Int Set Bit {1:X2}", strClass, (i * 8) + j);
                        else
                            strInt = String.Format("{0:s} : Falling Int Set Bit {1:X2}", strClass, (i * 8) + j);


                        if (textInterrupt.Length == 0)
                            textInterrupt += strInt;
                        else
                            textInterrupt += "\r\n" + strInt;


                        //textInterrupt.SelectionStart = textInterrupt.TextLength;
                        //textInterrupt.ScrollToCaret();

                    }
                }
            }

            return true;
        }


        

    }
}

