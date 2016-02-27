using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infragistics.Win.Misc;
using System.Windows.Forms;


namespace SystemAlign
{
    class Control_Winsock : Form
    {
        private bool isRunning = true;
        private int lineCount = 1;
        private bool m_fTCPThreadStop;
        private NetworkStream ns;
        private TcpClient server;
        private DateTime nowDateTime;
        private void LogData_Write(string sWriteString)
        {

            //string sDirPath1 = "C:\\Viston IK\\SaveDataFiles\\images\\";
            string sDirPath1 = "C:\\VisteonIK\\SaveDataFiles\\";
            string saveFileName = nowDateTime.Hour.ToString("00") + nowDateTime.Minute.ToString("00") +
                                 nowDateTime.Second.ToString("00");
            string Logfilename = sDirPath1 + saveFileName + ".txt";
            var file = new StreamWriter(Logfilename);

            file.Write(sWriteString);

            file.Close();
        }

        public void ColorSet_ConnectStatus(UltraLabel colorLbl, int colorNo)
        {
            UltraLabel colorLabel = null; // this.uGrd_Status1;

            colorLabel = colorLbl;

            if (colorNo == 1)
            {
                colorLabel.Appearance.BackColor = Color.Chartreuse; // GreenYellow;//Chartreuse;
                colorLabel.Appearance.BackColor2 = Color.LawnGreen;
                // YellowGreen;//Chartreuse;//ForestGreen;//LimeGreen;
            }
            else if (colorNo == 2)
            {
                colorLabel.Appearance.BackColor = Color.Red;
                colorLabel.Appearance.BackColor2 = Color.Firebrick;
            }
            else if (colorNo == 3)
            {
                colorLabel.Appearance.BackColor = Color.FromArgb(((((224)))), ((((224)))), ((((224)))));
                colorLabel.Appearance.BackColor2 = Color.DarkGray;
            }
        }

        public int TCP_Client_Open()
        {
            try
            {
                server = new TcpClient("192.168.0.24", 3);
                //server = new TcpClient("192.168.0.25", 5000);
                ns = server.GetStream();
                var recvThread = new Thread(TCP_RecvThread);
                recvThread.Start();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("용접기와 연결에 실패함");
                return -1;
            }
            return 0;
        }


        private void TCP_Data_Read(string msg)
        {
            //TCP_Data_Compute(msg);
            TCP_Data_Compute1(msg);
        }

        int m_iGraphDataCount = 0;
        private string[] m_sReadGraphDataArray = new string[200];
        string csTmp = "", sLongTime = "", sHornCount = "", sEnvilCount = "", csDate = "", csTime = "", sEneray = "", sPower = "", sAlram = "", sRight = "", sData = "";

        private int TCP_Data_Compute1(string sReadData)
        {
            if (sReadData == null) return 0;
            LogData_Write(sReadData);
            int nIndex = sReadData.IndexOf("Versagraphix");
            if (nIndex >= 0)
            {
                //연결 녹색 표시 : 위의 문자열은 용접기가 보내주는 데이터의 항목을 나열해서 보내주는
                //나열해서 보내주는 것이다.
                //ColorSet_ConnectStatus(uLbl_ConnectWelder, 1);
                return 0;
            }
            else
            {
                //비연결 적색 표시
                //ColorSet_ConnectStatus(uLbl_ConnectWelder, 2);
            }

            m_sReadGraphDataArray = new string[200];
            //if (debugFlag == true) this.textBox1.Text += "\r\nsReadData.Count : " + sReadData.Length.ToString("0000") + "\r\n";

            m_iGraphDataCount = 0;
            for (int i = 0; i < 24; i++)
            {
                nIndex = sReadData.IndexOf("\t");
                if (nIndex < 0) return 0;
                csTmp = sReadData.Substring(0, nIndex);

                //if (debugFlag == true) this.textBox1.Text += i.ToString("00 ") + sReadData.Substring(0, nIndex) + " ";

                if (i == 0)
                    sHornCount = sReadData.Substring(0, nIndex);
                ;
                if (i == 1)
                    csDate = sReadData.Substring(0, nIndex);
                ;
                if (i == 2) csTime = sReadData.Substring(0, nIndex);
                ;
                if (i == 5) sEneray = sReadData.Substring(0, nIndex);
                ;
                if (i == 19)
                    sLongTime = sReadData.Substring(0, nIndex);
                ;

                if (i == 20) sPower = sReadData.Substring(0, nIndex);
                ;
                if (i == 23) sAlram = sReadData.Substring(0, nIndex);
                ;

                sReadData = sReadData.Substring(nIndex + 1);
            }

            //int nAxisCount = this.ChartAxisXCountCompute();
            int nAxisCount=0;// = this.ChartAxisXCountCompute();

            for (int i = 0; i < 200; i++)
            {
                m_sReadGraphDataArray[i] = "-1";
            }



            for (int i = 0; i < nAxisCount + 2; i++)
            {

                nIndex = sReadData.IndexOf("\t");
                if (nIndex < 0)
                {
                    m_sReadGraphDataArray[i] = "0";
                }
                else
                {
                    m_sReadGraphDataArray[i] = sReadData.Substring(0, nIndex);
                    m_iGraphDataCount++;
                    sReadData = sReadData.Substring(nIndex + 1);
                }

            }

            //if (debugFlag == true) this.textBox1.Text += "\r\nm_iGraphDataCount : " + m_iGraphDataCount.ToString("00") + "\r\n";
            return 0;
        }

        private void TCP_Client_Close()
        {
            isRunning = false;
            if (ns != null)
            {
                ns.Close();
            }
            if (server != null)
            {
                server.Close();
            }
        }

        public void TCP_RecvThread()
        {
            var buffer = new byte[1024];
            string msg = "";
            while (isRunning)
            {
                try
                {
                    buffer.Initialize();
                    ns.Read(buffer, 0, buffer.Length);
                    msg = Encoding.ASCII.GetString(buffer);
                    Invoke(new LogToForm(TCP_Data_Read), new object[] { msg });
                }
                catch (Exception ex)
                {
                    m_fTCPThreadStop = true;
                }
            }
        }

        private delegate void LogToForm(string msg);

    }
}
