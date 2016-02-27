using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;



namespace SystemAlign
{
    class AXTDigitalIO
    {
        private CheckBox[] checkHigh = new CheckBox[16];
        private CheckBox[] checkLow = new CheckBox[16];
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


        public AXTDigitalIO()
        {
            if (OpenDevice())
			{
				//radioMessage.Checked	= true;
				//timerSensor.Enabled		= true;
				//frmDigitalIO			= this;
			}
            //CheckForIllegalCrossThreadCalls = false;
        }

        private bool OpenDevice()
        {
            //++
            // Initialize library 
            if (CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
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

                                    //comboModule.Items.Add(strData);
                                }
                            }

                            //comboModule.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Module not exist.");

                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Open Error!");
            }

            return true;
        }

        private bool SelectModule()
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;
                short nIndex = 0;
                uint uDataHigh = 0;
                uint uDataLow = 0;
                uint uFlagHigh = 0;
                uint uFlagLow = 0;
                uint uUse = 0;

                CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DI32:
                    case AXT_MODULE.AXT_SIO_RDI32:
                        groupHigh.Text = "INPUT  0bit ~ 15Bit";
                        groupLow.Text = "INPUT 16bit ~ 31Bit";

                        if (((AXT_MODULE)uModuleID) == AXT_MODULE.AXT_SIO_RDI32)
                        {
                            checkInterrupt.Checked = false;
                            radioCallback.Enabled = false;
                            radioMessage.Enabled = false;
                            radioEvent.Enabled = false;
                            checkRigingEdge.Checked = false;
                            checkFallingEdge.Checked = false;
                        }
                        else
                        {
                            checkInterrupt.Checked = true;
                            radioCallback.Enabled = true;
                            radioMessage.Enabled = true;
                            radioEvent.Enabled = true;
                            checkRigingEdge.Checked = true;
                            checkFallingEdge.Checked = true;

                            CAXD.AxdiInterruptGetModuleEnable(comboModule.SelectedIndex, ref uUse);
                            if (uUse == (uint)AXT_USE.ENABLE)
                            {
                                checkInterrupt.Checked = true;
                                SelectMessage();
                            }
                            else
                                checkInterrupt.Checked = false;

                            CAXD.AxdiInterruptEdgeGetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataHigh);
                            CAXD.AxdiInterruptEdgeGetWord(comboModule.SelectedIndex, 1, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataLow);
                            if (uDataHigh == 0xFFFF && uDataLow == 0xFFFF)
                                checkRigingEdge.Checked = true;
                            else
                                checkRigingEdge.Checked = false;

                            CAXD.AxdiInterruptEdgeGetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataHigh);
                            CAXD.AxdiInterruptEdgeGetWord(comboModule.SelectedIndex, 1, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataLow);
                            if (uDataHigh == 0xFFFF && uDataLow == 0xFFFF)
                                checkFallingEdge.Checked = true;
                            else
                                checkFallingEdge.Checked = false;
                        }

                        for (nIndex = 0; nIndex < 16; nIndex++)
                        {
                            checkHigh[nIndex].Text = String.Format("{0:D2}", nIndex);
                            checkLow[nIndex].Text = String.Format("{0:D2}", nIndex + 16);
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                        groupHigh.Text = "OUTPUT  0bit ~ 15Bit";
                        groupLow.Text = "OUTPUT 16bit ~ 31Bit";

                        checkInterrupt.Checked = false;
                        radioCallback.Enabled = false;
                        radioMessage.Enabled = false;
                        radioEvent.Enabled = false;
                        checkRigingEdge.Checked = false;
                        checkFallingEdge.Checked = false;

                        //++
                        // Read outputting signal in WORD
                        CAXD.AxdoReadOutportWord(comboModule.SelectedIndex, 0, ref uDataHigh);
                        CAXD.AxdoReadOutportWord(comboModule.SelectedIndex, 1, ref uDataLow);

                        for (nIndex = 0; nIndex < 16; nIndex++)
                        {
                            // Verify the last bit value of data read
                            uFlagHigh = uDataHigh & 0x0001;
                            uFlagLow = uDataLow & 0x0001;

                            // Shift rightward by bit by bit
                            uDataHigh = uDataHigh >> 1;
                            uDataLow = uDataLow >> 1;

                            // Updat bit value in control
                            if (uFlagHigh == 1)
                                checkHigh[nIndex].Checked = true;
                            else
                                checkHigh[nIndex].Checked = false;

                            if (uFlagLow == 1)
                                checkLow[nIndex].Checked = true;
                            else
                                checkLow[nIndex].Checked = false;

                            checkHigh[nIndex].Text = String.Format("{0:D2}", nIndex);
                            checkLow[nIndex].Text = String.Format("{0:D2}", nIndex + 16);
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                    case AXT_MODULE.AXT_SIO_RDB128MLII:
                        groupHigh.Text = "INPUT  0bit ~ 15Bit";
                        groupLow.Text = "OUTPUT  0bit ~ 15Bit";

                        // Only Digital Input was used
                        checkInterrupt.Enabled = true;
                        checkRigingEdge.Enabled = true;
                        checkFallingEdge.Enabled = true;

                        CAXD.AxdiInterruptGetModuleEnable(comboModule.SelectedIndex, ref uUse);
                        if (uUse == (uint)AXT_USE.ENABLE)
                        {
                            checkInterrupt.Checked = true;
                            SelectMessage();
                        }
                        else
                            checkInterrupt.Checked = false;

                        CAXD.AxdiInterruptEdgeGetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataHigh);
                        if (uDataHigh == 0xFFFF)
                            checkRigingEdge.Checked = true;
                        else
                            checkRigingEdge.Checked = false;

                        CAXD.AxdiInterruptEdgeGetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataHigh);
                        if (uDataHigh == 0xFFFF)
                            checkFallingEdge.Checked = true;
                        else
                            checkFallingEdge.Checked = false;

                        //++
                        // Read outputting signal in WORD
                        CAXD.AxdoReadOutportWord(comboModule.SelectedIndex, 0, ref uDataLow);

                        for (nIndex = 0; nIndex < 16; nIndex++)
                        {
                            // Verify the last bit value of data read
                            uFlagLow = uDataLow & 0x0001;

                            // Shift rightward by bit by bit
                            uDataLow = uDataLow >> 1;

                            // Updat bit value in control
                            if (uFlagLow == 1)
                                checkLow[nIndex].Checked = true;
                            else
                                checkLow[nIndex].Checked = false;

                            checkHigh[nIndex].Text = String.Format("{0:D2}", nIndex);
                            checkLow[nIndex].Text = String.Format("{0:D2}", nIndex);
                        }
                        break;
                }
            }

            return true;
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


                        if (textInterrupt.TextLength == 0)
                            textInterrupt.Text += strInt;
                        else
                            textInterrupt.Text += "\r\n" + strInt;


                        textInterrupt.SelectionStart = textInterrupt.TextLength;
                        textInterrupt.ScrollToCaret();

                    }
                }
            }

            return true;
        }

       
        private bool SelectMessage()
        {
            uint pEvent = 0;

            if (EventThread != null)
            {
                bThread = false;
                SetEvent(hInterrupt);
                //EventThread.Abort();
                //EventThread	= null;
            }

            CAXD.AxdiInterruptSetModule(comboModule.SelectedIndex, this.Handle, (uint)AXT_EVENT.WM_AXL_INTERRUPT, null, ref pEvent);

            return true;
        }

        public static void InterruptCallback(int nModuleNo, uint uFlag)
        {
            frmDigitalIO.InterruptProc(AXT_INTERRUPT_CLASS.KIND_CALLBACK, nModuleNo, uFlag);
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
                    frmDigitalIO.InterruptProc(AXT_INTERRUPT_CLASS.KIND_EVENT, nModuleNo, uFlag);
                }
            }
            EventThread = null;
        }

        private bool SelectCallback()
        {
            uint pEvent = 0;

            if (EventThread != null)
            {
                EventThread.Abort();
                EventThread = null;
                bThread = false;
            }

            CAXD.AxdiInterruptSetModule(comboModule.SelectedIndex, (IntPtr)null, 0, new CAXHS.AXT_INTERRUPT_PROC(InterruptCallback), ref pEvent);

            return true;
        }


        private bool SelectEvent()
        {
            CAXD.AxdiInterruptSetModule(comboModule.SelectedIndex, (IntPtr)null, 0, null, ref hInterrupt);

            if (EventThread != null)
            {
                EventThread.Abort();
                EventThread = null;
                bThread = false;
            }

            if (!bThread)
            {
                bThread = true;
                EventThread = new Thread(new ThreadStart(this.ThreadProc));
                EventThread.Start();
            }

            return true;
        }

        private bool SelectHighIndex(int nIndex, uint uValue)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                        CAXD.AxdoWriteOutportBit(comboModule.SelectedIndex, nIndex, uValue);
                        break;

                    default:
                        return false;
                }
            }

            return true;
        }

        private bool SelectLowIndex(int nIndex, uint uValue)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                        CAXD.AxdoWriteOutportBit(comboModule.SelectedIndex, nIndex + 16, uValue);
                        break;
                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                    case AXT_MODULE.AXT_SIO_RDB128MLII:
                        CAXD.AxdoWriteOutportBit(comboModule.SelectedIndex, nIndex, uValue);
                        break;

                    default:
                        return false;
                }
            }

            return true;
        }

        private void CloseWindow()
        {
            if (EventThread != null)
            {
                bThread = false;
                SetEvent(hInterrupt);
                EventThread.Abort();
                EventThread = null;

            }

            CAXL.AxlClose();
        }










        private void Threading_Timer()
        {
            short nIndex = 0;
            uint uDataHigh = 0;
            uint uDataLow = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;
            int nBoardNo = 0;
            int nModulePos = 0;
            uint uModuleID = 0;

            CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

            switch ((AXT_MODULE)uModuleID)
            {
                case AXT_MODULE.AXT_SIO_DI32:
                case AXT_MODULE.AXT_SIO_RDI32:
                    //++
                    // Read inputting signal in WORD
                    CAXD.AxdiReadInportWord(comboModule.SelectedIndex, 0, ref uDataHigh);
                    CAXD.AxdiReadInportWord(comboModule.SelectedIndex, 1, ref uDataLow);

                    for (nIndex = 0; nIndex < 16; nIndex++)
                    {
                        // Verify the last bit value of data read
                        uFlagHigh = uDataHigh & 0x0001;
                        uFlagLow = uDataLow & 0x0001;

                        // Shift rightward by bit by bit
                        uDataHigh = uDataHigh >> 1;
                        uDataLow = uDataLow >> 1;

                        // Updat bit value in control
                        if (uFlagHigh == 1)
                            checkHigh[nIndex].Checked = true;
                        else
                            checkHigh[nIndex].Checked = false;

                        if (uFlagLow == 1)
                            checkLow[nIndex].Checked = true;
                        else
                            checkLow[nIndex].Checked = false;
                    }
                    break;

                case AXT_MODULE.AXT_SIO_DB32P:
                case AXT_MODULE.AXT_SIO_DB32T:
                    //++
                    // Read inputting signal in WORD
                    CAXD.AxdiReadInportWord(comboModule.SelectedIndex, 0, ref uDataHigh);

                    for (nIndex = 0; nIndex < 16; nIndex++)
                    {
                        // Verify the last bit value of data read
                        uFlagHigh = uDataHigh & 0x0001;

                        // Shift rightward by bit by bit
                        uDataHigh = uDataHigh >> 1;

                        // Updat bit value in control
                        if (uFlagHigh == 1)
                            checkHigh[nIndex].Checked = true;
                        else
                            checkHigh[nIndex].Checked = false;
                    }
                    break;
            }
        }

        private void ModuleSelectedChanged()
        {
            SelectModule();
        }



        private void checkInterrupt_CheckedChanged(object sender, System.EventArgs e)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DI32:
                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                        if (checkInterrupt.Checked)
                        {
                            CAXL.AxlInterruptEnable();
                            CAXD.AxdiInterruptSetModuleEnable(comboModule.SelectedIndex, (uint)AXT_USE.ENABLE);
                        }
                        else
                        {
                            IntPtr pEvent = (IntPtr)0;


                            CAXD.AxdiInterruptSetModuleEnable(comboModule.SelectedIndex, (uint)AXT_USE.DISABLE);
                            CAXL.AxlInterruptDisable();
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDB128MLII:

                        checkInterrupt.Checked = false;
                        break;
                }
            }
        }

        private void checkRigingEdge_CheckedChanged(object sender, System.EventArgs e)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DI32:
                        if (checkRigingEdge.Checked)
                        {
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.UP_EDGE, 0xFFFF);
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 1, (uint)AXT_DIO_EDGE.UP_EDGE, 0xFFFF);
                        }
                        else
                        {
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.UP_EDGE, 0x0000);
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 1, (uint)AXT_DIO_EDGE.UP_EDGE, 0x0000);
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                        if (checkRigingEdge.Checked)
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.UP_EDGE, 0xFFFF);
                        else
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.UP_EDGE, 0x0000);
                        break;
                }
            }
        }

        private void checkFallingEdge_CheckedChanged(object sender, System.EventArgs e)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(comboModule.SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DI32:
                        if (checkFallingEdge.Checked)
                        {
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, 0xFFFF);
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 1, (uint)AXT_DIO_EDGE.DOWN_EDGE, 0xFFFF);
                        }
                        else
                        {
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, 0x0000);
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 1, (uint)AXT_DIO_EDGE.DOWN_EDGE, 0x0000);
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                        if (checkFallingEdge.Checked)
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, 0xFFFF);
                        else
                            CAXD.AxdiInterruptEdgeSetWord(comboModule.SelectedIndex, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, 0x0000);
                        break;
                }
            }
        }

        private void SelectMessageChanged()
        {
                SelectMessage();
        }

        private void SelectCallbackChanged()
        {
                SelectCallback();
        }

        private void SelectEventChanged()
        {
                SelectEvent();
        }

        private void checkHigh00_CheckedChanged()
        {
            SelectHighIndex(0, (uint)checkHigh[00].CheckState);
        }

        private void InterrupClear()
        {
            //textInterrupt.ResetText();
        }
    }
}
