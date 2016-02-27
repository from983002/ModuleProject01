using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PBasicCS
{
	public partial class PBasic : Form
	{
		private UInt32 m_dwDevice;
		private Int32 m_bDriverOpen;
		static public PBasic frmPbasic;

		public PBasic()
		{
			InitializeComponent();
		}

		private void PBasic_Load(object sender, EventArgs e)
		{
     		Int32 i, j;
     //       System.Boolean aaa;

            //aaa = true;
            System.Diagnostics.Debug.WriteLine(sizeof(System.Boolean).ToString());
			frmPbasic = this;
			progressBar.Hide();

			comboAxis.Items.Add("   #1");
			comboAxis.Items.Add("   #2");
			comboAxis.Items.Add("   #3");
			comboAxis.Items.Add("   #4");
            comboAxis.Items.Add("   #5");
            comboAxis.Items.Add("   #6");
            comboAxis.Items.Add("   #7");
            comboAxis.Items.Add("   #8");
			comboAxis.SelectedIndex = 0;

			listViewPosition.Columns.Add("축 정보");
			listViewPosition.Columns.Add("Command");
			listViewPosition.Columns.Add("Actual");
			listViewPosition.Columns.Add("Velocity");
			listViewPosition.Columns[0].Width = 60;
			listViewPosition.Columns[1].Width = 85;
			listViewPosition.Columns[2].Width = 85;
			listViewPosition.Columns[3].Width = 85;
			listViewPosition.Columns[0].TextAlign = HorizontalAlignment.Left;
			listViewPosition.Columns[1].TextAlign = HorizontalAlignment.Right;
			listViewPosition.Columns[2].TextAlign = HorizontalAlignment.Right;
			listViewPosition.Columns[3].TextAlign = HorizontalAlignment.Right;
			listViewPosition.Items.Add("   #1");
			listViewPosition.Items.Add("   #2");
			listViewPosition.Items.Add("   #3");
			listViewPosition.Items.Add("   #4");
            listViewPosition.Items.Add("   #5");
            listViewPosition.Items.Add("   #6");
            listViewPosition.Items.Add("   #7");
            listViewPosition.Items.Add("   #8");

			for (i = 0; i < 8; i++)
				for (j = 0; j < 3; j++)
					listViewPosition.Items[i].SubItems.Add("");

			listViewStatus.Columns.Add("축 정보");
			listViewStatus.Columns.Add("AMP");
			listViewStatus.Columns.Add("HFLG");
			listViewStatus.Columns.Add("PLIM");
			listViewStatus.Columns.Add("MLIM");
			listViewStatus.Columns.Add("FAUL");
			listViewStatus.Columns[0].Width = 60;
			listViewStatus.Columns[1].Width = 51;
			listViewStatus.Columns[2].Width = 51;
			listViewStatus.Columns[3].Width = 51;
			listViewStatus.Columns[4].Width = 51;
			listViewStatus.Columns[5].Width = 51;
			listViewStatus.Columns[0].TextAlign = HorizontalAlignment.Left;
			listViewStatus.Columns[1].TextAlign = HorizontalAlignment.Center;
			listViewStatus.Columns[2].TextAlign = HorizontalAlignment.Center;
			listViewStatus.Columns[3].TextAlign = HorizontalAlignment.Center;
			listViewStatus.Columns[4].TextAlign = HorizontalAlignment.Center;
			listViewStatus.Columns[5].TextAlign = HorizontalAlignment.Center;
			listViewStatus.Items.Add("   #1");
			listViewStatus.Items.Add("   #2");
			listViewStatus.Items.Add("   #3");
			listViewStatus.Items.Add("   #4");
            listViewStatus.Items.Add("   #5");
            listViewStatus.Items.Add("   #6");
            listViewStatus.Items.Add("   #7");
            listViewStatus.Items.Add("   #8");

			for (i = 0; i < 8; i++)
				for (j = 0; j < 5; j++)
					listViewStatus.Items[i].SubItems.Add("");

			m_dwDevice = PMAC.PmacSelect(0);

			if (m_dwDevice >= 0 && m_dwDevice < 255)
			{
				m_bDriverOpen = PMAC.OpenPmacDevice(m_dwDevice);
				timerStatus.Enabled = true;

                Byte[] byCommand;
                Byte[] byResponse;

                byCommand   = new Byte[255];
                byResponse  = new Byte[255];
                byCommand   = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("&1B11S");
                PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            }

			buttonMove01.Enabled = false;
			buttonMove02.Enabled = false;
			buttonMove03.Enabled = false;
			buttonMove04.Enabled = false;

			buttonDownload.Enabled = false;
			buttonRun.Enabled = false;
			buttonStop.Enabled = false;

		}

		private void PBasic_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (m_bDriverOpen == 1)
				PMAC.ClosePmacDevice(m_dwDevice);

			timerStatus.Enabled = false;
		}

		private void timerStatus_Tick(object sender, EventArgs e)
		{
            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            Byte[] byValue = new Byte[255];
			String[] strArray = new String[41];
			String strValue;
            String strResponse;

            byCommand   = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M0,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');

			checkOutput00.Checked = strArray[0].CompareTo("0") != 0 ? true : false;
			checkOutput01.Checked = strArray[1].CompareTo("0") != 0 ? true : false;
			checkOutput02.Checked = strArray[2].CompareTo("0") != 0 ? true : false;
			checkOutput03.Checked = strArray[3].CompareTo("0") != 0 ? true : false;
			checkOutput04.Checked = strArray[4].CompareTo("0") != 0 ? true : false;
			checkOutput05.Checked = strArray[5].CompareTo("0") != 0 ? true : false;
			checkOutput06.Checked = strArray[6].CompareTo("0") != 0 ? true : false;
			checkOutput07.Checked = strArray[7].CompareTo("0") != 0 ? true : false;

			checkInput00.Checked = strArray[10].CompareTo("0") != 0 ? true : false;
			checkInput01.Checked = strArray[11].CompareTo("0") != 0 ? true : false;
			checkInput02.Checked = strArray[12].CompareTo("0") != 0 ? true : false;
			checkInput03.Checked = strArray[13].CompareTo("0") != 0 ? true : false;
			checkInput04.Checked = strArray[14].CompareTo("0") != 0 ? true : false;
			checkInput05.Checked = strArray[15].CompareTo("0") != 0 ? true : false;
			checkInput06.Checked = strArray[16].CompareTo("0") != 0 ? true : false;
			checkInput07.Checked = strArray[17].CompareTo("0") != 0 ? true : false;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("P111,80");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
			strArray = strResponse.ToString().Split('\r');
            // #1 Position
			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[0]));
			if (strValue.CompareTo(listViewPosition.Items[0].SubItems[1].Text) != 0)
				listViewPosition.Items[0].SubItems[1].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[1]));
			if (strValue.CompareTo(listViewPosition.Items[0].SubItems[2].Text) != 0)
				listViewPosition.Items[0].SubItems[2].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[2]));
			if (strValue.CompareTo(listViewPosition.Items[0].SubItems[3].Text) != 0)
				listViewPosition.Items[0].SubItems[3].Text = strValue;
            // #2 Position
			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[10]));
			if (strValue.CompareTo(listViewPosition.Items[1].SubItems[1].Text) != 0)
				listViewPosition.Items[1].SubItems[1].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[11]));
			if (strValue.CompareTo(listViewPosition.Items[1].SubItems[2].Text) != 0)
				listViewPosition.Items[1].SubItems[2].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[12]));
			if (strValue.CompareTo(listViewPosition.Items[1].SubItems[3].Text) != 0)
				listViewPosition.Items[1].SubItems[3].Text = strValue;
            // #3 Position
			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[20]));
			if (strValue.CompareTo(listViewPosition.Items[2].SubItems[1].Text) != 0)
				listViewPosition.Items[2].SubItems[1].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[21]));
			if (strValue.CompareTo(listViewPosition.Items[2].SubItems[2].Text) != 0)
				listViewPosition.Items[2].SubItems[2].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[22]));
			if (strValue.CompareTo(listViewPosition.Items[2].SubItems[3].Text) != 0)
				listViewPosition.Items[2].SubItems[3].Text = strValue;
            // #4 Position
			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[30]));
			if (strValue.CompareTo(listViewPosition.Items[3].SubItems[1].Text) != 0)
				listViewPosition.Items[3].SubItems[1].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[31]));
			if (strValue.CompareTo(listViewPosition.Items[3].SubItems[2].Text) != 0)
				listViewPosition.Items[3].SubItems[2].Text = strValue;

			strValue = String.Format("{0:f}", Convert.ToDouble(strArray[32]));
			if (strValue.CompareTo(listViewPosition.Items[3].SubItems[3].Text) != 0)
				listViewPosition.Items[3].SubItems[3].Text = strValue;
            // #5 Position
            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[40]));
            if (strValue.CompareTo(listViewPosition.Items[4].SubItems[1].Text) != 0)
                listViewPosition.Items[4].SubItems[1].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[41]));
            if (strValue.CompareTo(listViewPosition.Items[4].SubItems[2].Text) != 0)
                listViewPosition.Items[4].SubItems[2].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[42]));
            if (strValue.CompareTo(listViewPosition.Items[4].SubItems[3].Text) != 0)
                listViewPosition.Items[4].SubItems[3].Text = strValue;
            // #6 Position
            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[50]));
            if (strValue.CompareTo(listViewPosition.Items[5].SubItems[1].Text) != 0)
                listViewPosition.Items[5].SubItems[1].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[51]));
            if (strValue.CompareTo(listViewPosition.Items[5].SubItems[2].Text) != 0)
                listViewPosition.Items[5].SubItems[2].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[52]));
            if (strValue.CompareTo(listViewPosition.Items[5].SubItems[3].Text) != 0)
                listViewPosition.Items[5].SubItems[3].Text = strValue;
            // #7 Position
            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[60]));
            if (strValue.CompareTo(listViewPosition.Items[6].SubItems[1].Text) != 0)
                listViewPosition.Items[6].SubItems[1].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[61]));
            if (strValue.CompareTo(listViewPosition.Items[6].SubItems[2].Text) != 0)
                listViewPosition.Items[6].SubItems[2].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[62]));
            if (strValue.CompareTo(listViewPosition.Items[6].SubItems[3].Text) != 0)
                listViewPosition.Items[6].SubItems[3].Text = strValue;
            // #8 Position
            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[70]));
            if (strValue.CompareTo(listViewPosition.Items[7].SubItems[1].Text) != 0)
                listViewPosition.Items[7].SubItems[1].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[71]));
            if (strValue.CompareTo(listViewPosition.Items[7].SubItems[2].Text) != 0)
                listViewPosition.Items[7].SubItems[2].Text = strValue;

            strValue = String.Format("{0:f}", Convert.ToDouble(strArray[72]));
            if (strValue.CompareTo(listViewPosition.Items[7].SubItems[3].Text) != 0)
                listViewPosition.Items[7].SubItems[3].Text = strValue;
            // #1
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M120,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
			strArray = strResponse.ToString().Split('\r');

			if (comboAxis.SelectedIndex == 0)
				checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
			if (strValue.CompareTo(listViewStatus.Items[0].SubItems[1].Text) != 0)
				listViewStatus.Items[0].SubItems[1].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
			if (strValue.CompareTo(listViewStatus.Items[0].SubItems[2].Text) != 0)
				listViewStatus.Items[0].SubItems[2].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
			if (strValue.CompareTo(listViewStatus.Items[0].SubItems[3].Text) != 0)
				listViewStatus.Items[0].SubItems[3].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
			if (strValue.CompareTo(listViewStatus.Items[0].SubItems[4].Text) != 0)
				listViewStatus.Items[0].SubItems[4].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
			if (strValue.CompareTo(listViewStatus.Items[0].SubItems[5].Text) != 0)
				listViewStatus.Items[0].SubItems[5].Text = strValue;
            // #2
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M220,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
			strArray = strResponse.ToString().Split('\r');

			if (comboAxis.SelectedIndex == 1)
				checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
			if (strValue.CompareTo(listViewStatus.Items[1].SubItems[1].Text) != 0)
				listViewStatus.Items[1].SubItems[1].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
			if (strValue.CompareTo(listViewStatus.Items[1].SubItems[2].Text) != 0)
				listViewStatus.Items[1].SubItems[2].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
			if (strValue.CompareTo(listViewStatus.Items[1].SubItems[3].Text) != 0)
				listViewStatus.Items[1].SubItems[3].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
			if (strValue.CompareTo(listViewStatus.Items[1].SubItems[4].Text) != 0)
				listViewStatus.Items[1].SubItems[4].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
			if (strValue.CompareTo(listViewStatus.Items[1].SubItems[5].Text) != 0)
				listViewStatus.Items[1].SubItems[5].Text = strValue;
            // #3
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M320,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
			strArray = strResponse.ToString().Split('\r');

			if (comboAxis.SelectedIndex == 2)
				checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
			if (strValue.CompareTo(listViewStatus.Items[2].SubItems[1].Text) != 0)
				listViewStatus.Items[2].SubItems[1].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
			if (strValue.CompareTo(listViewStatus.Items[2].SubItems[2].Text) != 0)
				listViewStatus.Items[2].SubItems[2].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
			if (strValue.CompareTo(listViewStatus.Items[2].SubItems[3].Text) != 0)
				listViewStatus.Items[2].SubItems[3].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
			if (strValue.CompareTo(listViewStatus.Items[2].SubItems[4].Text) != 0)
				listViewStatus.Items[2].SubItems[4].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
			if (strValue.CompareTo(listViewStatus.Items[2].SubItems[5].Text) != 0)
				listViewStatus.Items[2].SubItems[5].Text = strValue;
            // #4
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M420,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
			strArray = strResponse.ToString().Split('\r');

			if (comboAxis.SelectedIndex == 3)
				checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
			if (strValue.CompareTo(listViewStatus.Items[3].SubItems[1].Text) != 0)
				listViewStatus.Items[3].SubItems[1].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
			if (strValue.CompareTo(listViewStatus.Items[3].SubItems[2].Text) != 0)
				listViewStatus.Items[3].SubItems[2].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
			if (strValue.CompareTo(listViewStatus.Items[3].SubItems[3].Text) != 0)
				listViewStatus.Items[3].SubItems[3].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
			if (strValue.CompareTo(listViewStatus.Items[3].SubItems[4].Text) != 0)
				listViewStatus.Items[3].SubItems[4].Text = strValue;

			strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
			if (strValue.CompareTo(listViewStatus.Items[3].SubItems[5].Text) != 0)
				listViewStatus.Items[3].SubItems[5].Text = strValue;
            // #5
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M520,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');

            if (comboAxis.SelectedIndex == 4)
                checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
            if (strValue.CompareTo(listViewStatus.Items[4].SubItems[1].Text) != 0)
                listViewStatus.Items[4].SubItems[1].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
            if (strValue.CompareTo(listViewStatus.Items[4].SubItems[2].Text) != 0)
                listViewStatus.Items[4].SubItems[2].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
            if (strValue.CompareTo(listViewStatus.Items[4].SubItems[3].Text) != 0)
                listViewStatus.Items[4].SubItems[3].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
            if (strValue.CompareTo(listViewStatus.Items[4].SubItems[4].Text) != 0)
                listViewStatus.Items[4].SubItems[4].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
            if (strValue.CompareTo(listViewStatus.Items[4].SubItems[5].Text) != 0)
                listViewStatus.Items[4].SubItems[5].Text = strValue;
            // #6
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M620,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');

            if (comboAxis.SelectedIndex == 5)
                checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
            if (strValue.CompareTo(listViewStatus.Items[5].SubItems[1].Text) != 0)
                listViewStatus.Items[5].SubItems[1].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
            if (strValue.CompareTo(listViewStatus.Items[5].SubItems[2].Text) != 0)
                listViewStatus.Items[5].SubItems[2].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
            if (strValue.CompareTo(listViewStatus.Items[5].SubItems[3].Text) != 0)
                listViewStatus.Items[5].SubItems[3].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
            if (strValue.CompareTo(listViewStatus.Items[5].SubItems[4].Text) != 0)
                listViewStatus.Items[5].SubItems[4].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
            if (strValue.CompareTo(listViewStatus.Items[5].SubItems[5].Text) != 0)
                listViewStatus.Items[5].SubItems[5].Text = strValue;
            // #7
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M720,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');

            if (comboAxis.SelectedIndex == 6)
                checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
            if (strValue.CompareTo(listViewStatus.Items[6].SubItems[1].Text) != 0)
                listViewStatus.Items[6].SubItems[1].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
            if (strValue.CompareTo(listViewStatus.Items[6].SubItems[2].Text) != 0)
                listViewStatus.Items[6].SubItems[2].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
            if (strValue.CompareTo(listViewStatus.Items[6].SubItems[3].Text) != 0)
                listViewStatus.Items[6].SubItems[3].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
            if (strValue.CompareTo(listViewStatus.Items[6].SubItems[4].Text) != 0)
                listViewStatus.Items[6].SubItems[4].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
            if (strValue.CompareTo(listViewStatus.Items[6].SubItems[5].Text) != 0)
                listViewStatus.Items[6].SubItems[5].Text = strValue;
            // #8
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("M820,20");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
            strArray = strResponse.ToString().Split('\r');

            if (comboAxis.SelectedIndex == 7)
                checkAmplifier.Checked = Convert.ToInt32(strArray[19]) == 1 ? true : false;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[19]));
            if (strValue.CompareTo(listViewStatus.Items[7].SubItems[1].Text) != 0)
                listViewStatus.Items[7].SubItems[1].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[0]));
            if (strValue.CompareTo(listViewStatus.Items[7].SubItems[2].Text) != 0)
                listViewStatus.Items[7].SubItems[2].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[1]));
            if (strValue.CompareTo(listViewStatus.Items[7].SubItems[3].Text) != 0)
                listViewStatus.Items[7].SubItems[3].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[2]));
            if (strValue.CompareTo(listViewStatus.Items[7].SubItems[4].Text) != 0)
                listViewStatus.Items[7].SubItems[4].Text = strValue;

            strValue = String.Format("{0:d}", Convert.ToInt32(strArray[3]));
            if (strValue.CompareTo(listViewStatus.Items[7].SubItems[5].Text) != 0)
                listViewStatus.Items[7].SubItems[5].Text = strValue;

        /*  UInt32 offset = 0;
            UInt32 count = 10;

            IntPtr ptr1 = System.Runtime.InteropServices.Marshal.AllocHGlobal(10);
            IntPtr ptr2 = System.Runtime.InteropServices.Marshal.AllocHGlobal(10);

            Byte[] array1 = new Byte[10];
            Byte[] array2 = new Byte[10];
            UInt16[] array3 = new UInt16[5];

            array1[0] = 1;
            array1[1] = 2;
            array1[2] = 3;
            array1[3] = 4;
            array1[4] = 5;
            array1[5] = 6;
            array1[6] = 7;
            array1[7] = 8;
            array1[8] = 9;
            array1[9] = 10;

            array3[0] = 11;
            array3[1] = 12;
            array3[2] = 13;
            array3[3] = 14;
            array3[4] = 15;

            System.Runtime.InteropServices.Marshal.Copy(array1 0, ptr1, 10);

            PMAC.PmacDPRSetMem(m_dwDevice, offset, count, ptr1);
  
            PMAC.PmacDPRGetMem(m_dwDevice, offset, count, ptr2);

            System.Runtime.InteropServices.Marshal.Copy(ptr2, array2, 0, 10);

            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr1);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr2);
            */
		}

		private void timerProgress_Tick(object sender, EventArgs e)
		{
			timerProgress.Enabled = false;
			progressBar.Hide();
		}

		private void comboAxis_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void buttonHome_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

			Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("{0:d}HM", comboAxis.SelectedIndex + 1);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkAmplifier_CheckedChanged(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M{0:d}39={1:d}", comboAxis.SelectedIndex + 1, checkAmplifier.Checked == true ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonJogMinus00_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=100 #{1:d}J-", comboAxis.SelectedIndex + 1, comboAxis.SelectedIndex + 1);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonJogPlus00_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=100 #{1:d}J+", comboAxis.SelectedIndex + 1, comboAxis.SelectedIndex + 1);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonJogMinus01_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=200 #{1:d}J-", comboAxis.SelectedIndex + 1, comboAxis.SelectedIndex + 1);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonJogPlus01_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("I{0:d}22=200 #{1:d}J+", comboAxis.SelectedIndex + 1, comboAxis.SelectedIndex + 1);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonJog_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("#{0:d}J/", comboAxis.SelectedIndex + 1);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput00_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M0={0:d}", checkOutput00.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput01_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M1={0:d}", checkOutput01.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput02_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M2={0:d}", checkOutput02.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput03_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M3={0:d}", checkOutput03.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput04_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M4={0:d}", checkOutput04.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput05_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M5={0:d}", checkOutput05.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput06_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M6={0:d}", checkOutput06.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void checkOutput07_MouseClick(object sender, MouseEventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("M7={0:d}", checkOutput07.Checked ? 1 : 0);
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonMove01_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=21");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonMove02_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=22");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonMove03_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=23");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonMove04_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("MP2000=24");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
		}

		private void buttonFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog FileDlg = new OpenFileDialog();

			FileDlg.Filter = "PMAC Files (*.pmc)|*.pmc|All Files (*.*)|*.*||";
			FileDlg.FilterIndex = 1;
			DialogResult Result = FileDlg.ShowDialog();

			if (Result == DialogResult.OK)
			{
				textFile.Text = FileDlg.FileName;
				buttonDownload.Enabled = true;
			}
		}

        public static void DownloadMsgProc(String str, Int32 newlie)
		{
            System.Diagnostics.Debug.WriteLine(str);
		}

		public static void DownloadProgress(Int32 nPercent)
		{
			frmPbasic.progressBar.Value = nPercent;

			if (nPercent >= 100)
				frmPbasic.timerProgress.Enabled = true;
		}

		private void buttonDownload_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("&1A");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
			System.Threading.Thread.Sleep(10);

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(textFile.Text);
            if (PMAC.PmacDownloadA(m_dwDevice, new PMAC.DOWNLOADMSGPROC(DownloadMsgProc), null, new PMAC.DOWNLOADPROGRESS(DownloadProgress), byCommand, 1, 1, 1, 1) == 0)
			{
				MessageBox.Show("다운로드 에러입니다.", "에러");
				return;
			}

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("Enable PLC1,10,11");
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);
			buttonRun.Enabled = true;
			buttonStop.Enabled = false;

			progressBar.Show();
		}

		private void buttonRun_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("&1B11R");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);

			buttonHome.Enabled = false;
			buttonJogMinus00.Enabled = false;
			buttonJogMinus01.Enabled = false;
			buttonJogPlus00.Enabled = false;
			buttonJogPlus01.Enabled = false;

			buttonMove01.Enabled = true;
			buttonMove02.Enabled = true;
			buttonMove03.Enabled = true;
			buttonMove04.Enabled = true;

			buttonRun.Enabled = false;
			buttonStop.Enabled = true;
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			if (m_bDriverOpen == 0)
				return;

            Byte[] byCommand = new Byte[255];
            Byte[] byResponse = new Byte[255];
            String strValue;

            strValue = String.Format("&1B11S");
            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(strValue);
            PMAC.PmacGetResponseA(m_dwDevice, byResponse, Convert.ToUInt32(byResponse.Length - 1), byCommand);

			buttonHome.Enabled = true;
			buttonJogMinus00.Enabled = true;
			buttonJogMinus01.Enabled = true;
			buttonJogPlus00.Enabled = true;
			buttonJogPlus01.Enabled = true;

			buttonMove01.Enabled = false;
			buttonMove02.Enabled = false;
			buttonMove03.Enabled = false;
			buttonMove04.Enabled = false;

			buttonRun.Enabled = true;
			buttonStop.Enabled = false;
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
