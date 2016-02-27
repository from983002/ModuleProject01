using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SystemAlign
{
    public class Interface_LVS
    {
        private SerialPort comPort;// = new SerialPort();

        public Interface_LVS(SerialPort port)
        {
            comPort = port;
        }

        public string[] Port_GetNames()
        {
            string[] portNames = SerialPort.GetPortNames();
            return portNames;
        }

        public void Port_Initialize(string portNo)
        {
            comPort.BaudRate = 19200;
            comPort.DataBits = 8;
            comPort.StopBits = StopBits.One;
            comPort.Parity = Parity.None;
            comPort.PortName = "COM" + portNo;
        }

        public void Port_Open()
        {
            if(comPort.IsOpen == false)
                comPort.Open();
        }

        public void Port_Close()
        {
            if (comPort.IsOpen == true)
                comPort.Close();
        }

        public void Port_Write(byte[] writeData)
        {
            comPort.Write(writeData, 0, writeData.Length);
        }

        public byte[] tmpBytes = new byte[7];
        public void Dute_Set(int setValue)
        {
            byte[] sendData = new byte[7];
            sendData[0] = byte.Parse("L");
            sendData[1] = 0x31;
            sendData[2] = 0x31;
            sendData[3] = 0x32;
            sendData[4] = 0x30;
            sendData[5] = 0x0D;
            sendData[6] = 0x0A;

            tmpBytes = sendData;
        }
    }
}
