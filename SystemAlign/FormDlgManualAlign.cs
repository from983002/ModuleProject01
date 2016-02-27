using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SystemAlign
{
    public partial class FormDlgManualAlign : Form
    {
        public FormDlgManualAlign()
        {
            InitializeComponent();
        }

        int _iGripNo = 0;
        int _iCellNo = 0;
        int _iCellType = 0;
        int _iModelCellCount = 0;

        public int ModelCellCount
        {
            get { return _iModelCellCount; }
            set { _iModelCellCount = value; }
        }

        public int GripNo
        {
            get { return _iGripNo; }
            set { _iGripNo = value; }
        }
                
        public int CellType
        {
            get { return _iCellType; }
            set { _iCellType = value; }
        }

        public int CellNo
        {
            get { return _iCellNo; }
            set { _iCellNo = value; }
        }

        private void ubtnLogin_Click(object sender, EventArgs e)
        {
            GripNo = Manual_uCombo_01.SelectedIndex + 1;
            CellType = Manual_uCombo_02.SelectedIndex + 1;
            CellNo = Manual_uCombo_03.SelectedIndex;
        }

        private void FormDlgManualAlign_Load(object sender, EventArgs e)
        {
            Manual_CellNo_ListMake();
            Manual_uCombo_01.SelectedIndex = _iGripNo;
            Manual_uCombo_02.SelectedIndex = 0;
            Manual_uCombo_03.SelectedIndex = 0;
        }

        private void Manual_CellNo_ListMake()
        {
            this.Manual_uCombo_03.Items.Clear();

            Infragistics.Win.ValueListItem CellNoComboList0 = new Infragistics.Win.ValueListItem();
            CellNoComboList0 = new Infragistics.Win.ValueListItem();
            CellNoComboList0.DataValue = new decimal(new int[] { 0, 0, 0, 0 });
            CellNoComboList0.DisplayText = "NO 0";
            this.Manual_uCombo_03.Items.Add(CellNoComboList0);

            Infragistics.Win.ValueListItem[] CellNoComboList = new Infragistics.Win.ValueListItem[ModelCellCount];
            for (int i = 0; i < ModelCellCount; i++)
            {
                CellNoComboList[i] = new Infragistics.Win.ValueListItem();
                CellNoComboList[i].DataValue = new decimal(new int[] { i + 1, 0, 0, 0 });
                CellNoComboList[i].DisplayText = "NO " + (i + 1).ToString();
            }
            this.Manual_uCombo_03.Items.AddRange(CellNoComboList);
        }
    }
}
