using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTETHERLib;


namespace SystemAlign
{
    public class Melsec
    {
        //32비트에서 정상
        //public ACTETHERLib.ActQJ71E71UDP m_MelsecUDP = new ACTETHERLib.ActQJ71E71UDP();
        public ACTETHERLib.ActQJ71E71TCP m_MelsecTCP = new ACTETHERLib.ActQJ71E71TCP();
        
        //64비트에서 정상
        //public AxACTETHERLib.AxActQJ71E71TCP m_MelsecTCP;

        //public Melsec(AxACTETHERLib.AxActQJ71E71TCP m_Melsec)
        //{
        //    m_MelsecTCP = m_Melsec;
        //}
        
        public void PLC_Initialize()
        {
            //PC쪽 설정 내용 "192.168.0.25";
            this.m_MelsecTCP.ActSourceNetworkNumber = 4;
            this.m_MelsecTCP.ActSourceStationNumber = 2;

            //PLC쪽 설정 내용
            //this.m_MelsecUDP.ActCpuType = 146; //Q06UDEHCPU=34, Q06HCPU=35;(폴딩)
            //this.m_MelsecUDP.ActHostAddress = "192.168.172.233";
            this.m_MelsecTCP.ActCpuType = 34;
            this.m_MelsecTCP.ActHostAddress = "100.100.100.2";
            this.m_MelsecTCP.ActNetworkNumber = 4;
            this.m_MelsecTCP.ActStationNumber = 1;
            
            this.m_MelsecTCP.ActTimeOut = 5000;
        }

        public int PLC_Connection()
        {
            int IsOpen = this.m_MelsecTCP.Open();
            return IsOpen;
        }

        public int PLC_GetDevice(string szDevice, out int lData)
        {
            //int iData = 0;
            int iRet = this.m_MelsecTCP.GetDevice(szDevice, out lData);
            return iRet;
        }

        public int PLC_ReadData(string szDivece, int dwSize, int[] lpdwData)
        {
            int iRet = this.m_MelsecTCP.ReadDeviceBlock(szDivece, dwSize, out lpdwData[0]);
            return iRet;
        }

        public int PLC_WriteData(string szDivece, int dwSize, int[] lpdwData)
        {
            int iRet = this.m_MelsecTCP.WriteDeviceBlock(szDivece, dwSize, ref lpdwData[0]);
            return iRet;
        }

        public int PLC_Close()
        {
            int IsClose = this.m_MelsecTCP.Close();
            return IsClose;
        }

        /*
        public void PLC_Initialize()
        {
            //PC쪽 설정 내용 "192.168.0.25";
            this.m_MelsecUDP.ActSourceNetworkNumber = 4;
            this.m_MelsecUDP.ActSourceStationNumber = 2;

            //PLC쪽 설정 내용
            //this.m_MelsecUDP.ActCpuType = 146; //Q06UDEHCPU=34, Q06HCPU=35;(폴딩)
            //this.m_MelsecUDP.ActHostAddress = "192.168.172.233";
            this.m_MelsecUDP.ActCpuType = 34;
            this.m_MelsecUDP.ActHostAddress = "100.100.100.2";
            this.m_MelsecUDP.ActNetworkNumber = 4;
            this.m_MelsecUDP.ActStationNumber = 1;

            this.m_MelsecUDP.ActTimeOut = 5000;
        }

        public int PLC_Connection()
        {
            int IsOpen = this.m_MelsecUDP.Open();
            return IsOpen;
        }

        public int PLC_GetDevice(string szDevice, out int lData)
        {
            //int iData = 0;
            int iRet = this.m_MelsecUDP.GetDevice(szDevice, out lData);
            return iRet;
        }

        public int PLC_ReadData(string szDivece, int dwSize, int[] lpdwData)
        {
            int iRet = this.m_MelsecUDP.ReadDeviceBlock(szDivece, dwSize, out lpdwData[0]);
            return iRet;
        }

        public int PLC_WriteData(string szDivece, int dwSize, int[] lpdwData)
        {
            int iRet = this.m_MelsecUDP.WriteDeviceBlock(szDivece, dwSize, ref lpdwData[0]);
            return iRet;
        }

        public int PLC_Close()
        {
            int IsClose = this.m_MelsecUDP.Close();
            return IsClose;
        }
        */
    }
}
