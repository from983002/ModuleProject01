using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SystemAlign
{

    public class Control_LS1224
    {
        private Interface_LVS LS1224;
        public string PortSetName = "1";

        public Control_LS1224(SerialPort port)
        {
            LS1224 = new Interface_LVS(port);
        }

        public void LVS_Connect()
        {
            LS1224.Port_Initialize(PortSetName);
            LS1224.Port_Open();
        }

        public void LVS_Set_InSide_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //Encoding.ASCII.GetString(ascii);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x34;      //채널>> 4
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            LS1224.Port_Write(setZero);
        }

        public void LVS_Set_OutSide_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x33;      //채널>> 3
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            LS1224.Port_Write(setZero);
        }


        public string[] LVS_GetPort()
        {
            return LS1224.Port_GetNames();
        }

        public void LVS_Close()
        {
            LS1224.Port_Close();
        }
    }

    public class Control_PN2212
    {
        private Interface_LVS PN2212;
        private SerialPort comPort;

        public Control_PN2212(SerialPort port)
        {
            comPort = port;
            PN2212 = new Interface_LVS(port);
        }

        public void LVS_Connect()
        {
            PN2212.Port_Initialize("1");
            PN2212.Port_Open();
        }

        public void LVS_Duty_Set()
        {
            byte[] sendData = new byte[7];
            sendData[0] = 0x4C;//L
            sendData[1] = 0x31;
            sendData[2] = 0x30;
            sendData[3] = 0x32;
            sendData[4] = 0x30;
            sendData[5] = 0x0D;
            sendData[6] = 0x0A;
            PN2212.Port_Write(sendData);
        }

        public void LVS_OFF_Set()
        {
            byte[] sendData = new byte[4];
            sendData[0] = 0x45;//E
            sendData[1] = 0x31;
            sendData[2] = 0x0D;
            sendData[3] = 0x0A;
            PN2212.Port_Write(sendData);
        }

        public void LVS_ON_Set()
        {
            byte[] sendData = new byte[4];
            sendData[0] = 0x53;//S
            sendData[1] = 0x31;
            sendData[2] = 0x0D;
            sendData[3] = 0x0A;
            PN2212.Port_Write(sendData);
        }


        public void LVS_Now_Set()
        {
            byte[] sendData = new byte[4];
            sendData[0] = 0x52;//R
            sendData[1] = 0x31;
            sendData[2] = 0x0D;
            sendData[3] = 0x0A;
            PN2212.Port_Write(sendData);
        }

        public string[] LVS_GetPort()
        {
            return PN2212.Port_GetNames();
        }

        public void LVS_Close()
        {
            PN2212.Port_Close();
        }


        public void LVS_Set_InSide_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //Encoding.ASCII.GetString(ascii);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x34;      //채널>> 4
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            PN2212.Port_Write(setZero);
        }

        public void LVS_Set_OutSide_BackLight(string setValue)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(setValue);
            //byte[] setZero = new byte[9];
            //setZero[6] = 0x30;      //값4 >> 0
            //setZero[7] = 0x0D;      //종료>> CR
            //setZero[8] = 0x0A;      //종료>> LF

            byte[] setZero = new byte[8];
            setZero[6] = 0x0D;      //종료>> CR
            setZero[7] = 0x0A;      //종료>> LF

            setZero[0] = 0x3A;      //시작>> :
            setZero[1] = 0x31;      //국번>> 1
            setZero[2] = 0x33;      //채널>> 3
            setZero[3] = ascii[0];      //값1 >> 0
            setZero[4] = ascii[1];      //값2 >> 0
            setZero[5] = ascii[2];      //값3 >> 0

            PN2212.Port_Write(setZero);
        }

    }
}
