using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Diagnostics;
//using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;

using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;



namespace SystemAlign
{
    public partial class FormDlgModelReg : Form
    {
        public event ModelRegEvent1 ModelAdding;

        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public static int WM_QUIT = 0x12;

        public FormDlgModelReg()
        {
            InitializeComponent();
        }
        
        public static string FileName;
        private object opt = Missing.Value;
		private string AppPath;
		private bool visible;
        private CInspection_Lamination LamiSystem;

        public CInspection_Lamination GetSet_LamiSystem
        {
            get { return LamiSystem; }
            set { LamiSystem = value; }
        }

		public bool Visible
		{
			get { return visible; }
			set
			{
				visible = value;
				xls.Visible = value;
			}
		}

        private string _strModelName = null;
        private string _strModelNumber = null;

        public string GetSetModelName
        {
            get { return _strModelName; }
            set { _strModelName = value; }
        }

        public string GetSetModelNumber
        {
            get { return _strModelNumber; }
            set { _strModelNumber = value; }
        }
        public void DataSourceReflash()
        {
            int dataSourceCount = this.uDataSource.Rows.Count;

            for (int row = 0; row < dataSourceCount; row++) 
            {
                Debug.Write("Row " + row.ToString() + " 입니다." + System.Environment.NewLine);
                UltraDataRow rowItem = this.uDataSource.Rows[row];
                rowItem["NO"] = row+1;
            }		
        }

        private void Display_SaveFile(Excel.Worksheet xlsSheet)
		{
			try
			{
				this.uDataSource.Rows.Clear();
				string[] rowReadData = new string[2];
				UltraDataRow rowItem;

				for (int row = 2; row < 100; row++) //
				{ 
					Debug.Write("Row " + row.ToString() + " 입니다." + System.Environment.NewLine);
					if (((Excel.Range)xlsSheet.Cells[row, 1]).Value2 == null) // null 이면 종료
						break;

                    this.GridScrollCheck();

                    this.uDataSource.Rows.Add();
                    rowItem = this.uDataSource.Rows[row - 2];

                    for (int col = 1; col < uDataSource.Band.Columns.Count; col++)
                    {
                        
                        //rowReadData[col - 1] = ((Excel.Range)xlsSheet.Cells[row, col+1]).Value2.ToString().Trim();
                        string tempString = ((Excel.Range) xlsSheet.Cells[row, col + 1]).Value2.ToString();
                        tempString = tempString.TrimStart();
                        tempString = tempString.TrimEnd();
                        rowReadData[col - 1] = tempString;
                    }

                    rowItem["NO"] = this.uDataSource.Rows.Count;
					rowItem["Name"] =	rowReadData[0];
					rowItem["RecipeNo"] =	rowReadData[1];	
				}				
			}
			catch
			{
				MessageBox.Show("ERROR !! READ TO EXCEL FILE !!");
			}
			finally
			{
				
			}
		}

        private void UserRegist()
        {
            UltraDataRow rowRead;
            this.uDataSource.Rows.Add();
            rowRead = this.uDataSource.Rows[this.uDataSource.Rows.Count -1];
            rowRead["NO"] = this.uDataSource.Rows.Count;
            rowRead["Name"] = "123456"; //rowReadData[0];
            rowRead["RecipeNo"] = "qawsedrf";//rowReadData[1];
        }

      
        public Excel.Application xls;
        public Excel.Worksheet xlsSheet;
        public Excel.Workbook xlsBook;

        private void ubtnReg_Click(object sender, EventArgs e)
        {
            FormDlgModelEdit.CaptionString = "모델 등록";
            var userAdd = new FormDlgModelEdit();

            if (userAdd.ShowDialog() == DialogResult.OK)
            {
                string[] AddedData = userAdd.InputData;
                if (Model_Reg_DoubleCheck(AddedData) == false)
                {

                    var messageBox = new Control_UltraMessageBox();

                    if (messageBox.MessageBox_Show("모델 변경", "적용 모델 변경",
                        "다른 모델 정보와 중복되는 모델 정보입니다.<br/><br/>확인 후에 다시 작업하여 주십시요!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information) == DialogResult.OK) return;
                }

                UltraDataRow rowRead;
                this.uDataSource.Rows.Add();
                rowRead = this.uDataSource.Rows[this.uDataSource.Rows.Count - 1];
                rowRead["NO"] = this.uDataSource.Rows.Count;
                //rowRead["Name"] = AddedData[0];
                //rowRead["RecipeNo"] = AddedData[1];

                rowRead["Name"] = userAdd.GetSet_Model_Name;
                rowRead["RecipeNo"] = userAdd.GetSet_Model_Number;

                this.GridScrollCheck();
                //this.uGridExcelExporter.Export(this.Model_uGrid1, FileName);

                _strModelName = userAdd.GetSet_Model_Name;
                _strModelNumber = userAdd.GetSet_Model_Number;

                bool modelAdd = ModelAdding(_strModelName, _strModelNumber);
            }
        }

        /*
        private void ubtnReg_Click(object sender, EventArgs e)
        {
            FormDlgModelEdit.CaptionString = "모델 등록";
            var userAdd = new FormDlgModelEdit();

            if (userAdd.ShowDialog() == DialogResult.OK)
            {                
                string[] AddedData = userAdd.InputData;
                if (Model_Reg_DoubleCheck(AddedData) == false)
                {

                    var messageBox = new Control_UltraMessageBox();

                    if (messageBox.MessageBox_Show("모델 변경", "적용 모델 변경",
                        "다른 모델 정보와 중복되는 모델 정보입니다.<br/><br/>확인 후에 다시 작업하여 주십시요!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information) == DialogResult.OK) return;
                }

                UltraDataRow rowRead;
                this.uDataSource.Rows.Add();
                rowRead = this.uDataSource.Rows[this.uDataSource.Rows.Count - 1];
                rowRead["NO"] = this.uDataSource.Rows.Count;
                //rowRead["Name"] = AddedData[0];
                //rowRead["RecipeNo"] = AddedData[1];

                rowRead["Name"] = userAdd.GetSet_Model_Name;
                rowRead["RecipeNo"] = userAdd.GetSet_Model_Number;

                this.GridScrollCheck();
                //this.uGridExcelExporter.Export(this.Model_uGrid1, FileName);

                _strModelName = userAdd.GetSet_Model_Name;
                _strModelNumber = userAdd.GetSet_Model_Number;

                bool modelAdd = ModelAdding(_strModelName, _strModelNumber);
            }
        }
        */

        private bool Model_Reg_DoubleCheck(string[] AddingData)
        {
            string[] strGridData = new string[Model_uGrid1.DisplayLayout.Rows.Count];
            for (int i = 0; i < Model_uGrid1.DisplayLayout.Rows.Count; i++)
            {
                strGridData[i] = Model_uGrid1.DisplayLayout.Rows[i].Cells[1].Value.ToString() + Model_uGrid1.DisplayLayout.Rows[i].Cells[2].Value.ToString();
                Trace.WriteLine("GridData : " + strGridData[i] + "   AddData : " + AddingData[0] + AddingData[1]);
                if (strGridData[i] == AddingData[0] + AddingData[1])
                {
                    return false;
                }
            }
            return true;
        }

        private ResourceManager rm = SystemAlign.Properties.Resources.ResourceManager;
        public void ExcelFileOpen()
        {
            FileName = Environment.CurrentDirectory + "\\Data\\ModelData\\ModelList.xls";
            xls = new Excel.Application();
            xls.Visible = false;
            xlsBook = (Excel.Workbook)xls.Workbooks.Open(FileName, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt);
            xlsSheet = (Excel.Worksheet)xlsBook.ActiveSheet;
            Debug.Write("ExcelControl 종료 : " + System.DateTime.Now.ToString() + System.Environment.NewLine);
            Display_SaveFile(xlsSheet);
            ExclFileClose();
            GridScrollCheck();
        }

        private void ubtnOK_Click(object sender, EventArgs e)
        {
            var messageBox = new Control_UltraMessageBox();
            if (Model_Select_uText01.Text == "")
            {
                if (messageBox.MessageBox_Show("모델 변경", "적용 모델 변경",
                "시스템에 적용할 모델이 선택되지 않았습니다..<br/><br/>시스템에 적용할 모델을 선택하여 주십시요!", MessageBoxButtons.OK,
                MessageBoxIcon.Question) == DialogResult.OK) 
                return;
            }

            if (messageBox.MessageBox_Show("모델 변경", "적용 모델 변경",
                "확인을 선택하면 선택한 모델이 시스템에 적용됩니다.<br/><br/>선택한 모델을 시스템에 적용 하시겠습니까?", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.Cancel) return;

            _strModelName = Model_Select_uText01.Text;
            _strModelNumber = Model_Select_uText02.Text;

            SetReg(LamiSystem.RegPathSystemStatus, "모델 이름", Model_Select_uText01.Text);
            SetReg(LamiSystem.RegPathSystemStatus, "모델 번호", Model_Select_uText02.Text);

            //this.uGridExcelExporter.Export(this.Model_uGrid1, FileName);
            this.DialogResult = DialogResult.OK;
        }

        public void SetReg(string strNodePath, string strName, string strData)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(strNodePath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            reg.SetValue(strName, strData, RegistryValueKind.String);
            reg.Close();
        }

        private void ExclFileClose()
        {
            if (xlsSheet != null) Marshal.ReleaseComObject(xlsSheet); 	
            if(xlsBook != null) xlsBook.Close(false, System.Type.Missing, System.Type.Missing);

            xls.UserControl = false;
            IntPtr hwnd = new IntPtr(xls.Hwnd);
            xls.Quit();
            Marshal.ReleaseComObject(xls);
            PostMessage(hwnd, WM_QUIT, 0, 0);
        }

        private void ubtnEdit_Click(object sender, EventArgs e)
        {
            if (Model_Select_uText01.Text == "")
            {
                return;
            }

            var messageBox = new Control_UltraMessageBox();

            if (Model_Current_uText01.Text == Model_Select_uText01.Text &&
                Model_Current_uText02.Text == Model_Select_uText02.Text)
            {
                messageBox.MessageBox_Show("모델 수정", "등록 모델 정보 수정",
                    "모델 수정 오류 발생<br/><br/>현재 적용되어 있는 모델은 삭제할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int SelectRow = this.Model_uGrid1.ActiveRow.Index;
            Infragistics.Win.UltraWinGrid.CellsCollection CuurrentRow = this.Model_uGrid1.Rows[SelectRow].Cells;
            //string[] EditValue = {CuurrentRow[1].Value.ToString(), CuurrentRow[2].Value.ToString(),CuurrentRow[3].Value.ToString()};
            string[] EditValue = { CuurrentRow[1].Value.ToString(), CuurrentRow[2].Value.ToString()};
            FormDlgModelEdit.CaptionString = "모델 수정";
            var userAdd = new FormDlgModelEdit();
            userAdd.InputData = EditValue;
            userAdd.EditDataDisplay();

            if (userAdd.ShowDialog() == DialogResult.OK)
            {
                string[] EditedData = userAdd.InputData;

                string modelNameOld = EditValue[0] + "-" + EditValue[1] + ".rcp";
                string oldModelFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelNameOld;
                //string oldModelFileName = Environment.CurrentDirectory + "\\ModelData\\" + modelNameOld;

                string modelNameNew = EditedData[0] + "-" + EditedData[1] + ".rcp";
                string newModelFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelNameNew;
                //string newModelFileName = Environment.CurrentDirectory + "\\ModelData\\" + modelNameNew;

                System.IO.File.Move(oldModelFileName, newModelFileName);

                //FileInfo fileRename = new FileInfo(newModelFileName);
                //if (fileRename.Exists == true)
                //{
                    
                    //MessageBox.Show("이미 파일이 존재합니다. 확인하여 주십시요 !");
                    //fileRename.Delete();
                    //return;
                    //System.IO.File.Move(oldname, newname);
                //}

                UltraDataRow rowRead = this.uDataSource.Rows[SelectRow];
                //string oldName = this.Model_uGrid1.Rows[SelectRow].Cells[1].Value.ToString();
                //string oldNo = this.Model_uGrid1.Rows[SelectRow].Cells[2].Value.ToString();
                
                rowRead["NO"] = SelectRow + 1;
                rowRead["Name"] = EditedData[0];
                rowRead["RecipeNo"] = EditedData[1];

                //ModelFile_Name_Change(oldName, oldNo, EditedData[0], EditedData[1]);

                this.GridScrollCheck();
                //this.uGridExcelExporter.Export(this.Model_uGrid1, FileName);
            }
            Model_uGrid1_NowRowDisplay();
        }

        private void ModelFile_Name_Change(string oldName, string oldNo, string newName, string newNo)
        {

            //bool modelAdd = ModelAdding(_strModelName, _strModelNumber);
            string modelNameOld = oldName + "-" + oldNo + ".rcp";
            string modelNameNew = newName + "-" + newNo + ".rcp";

            string oldModelFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelNameOld;
            string newModelFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelNameNew;

            FileInfo fileRename = new FileInfo(oldModelFileName);
            if (fileRename.Exists)
            {
                fileRename.MoveTo(newModelFileName); //이미있으면 에러
            }
        }

        private void ubtnDelete_Click(object sender, EventArgs e)
        {
            if (Model_Select_uText01.Text == "")
            {
                return;
            }

            var messageBox = new Control_UltraMessageBox();

            if (Model_Current_uText01.Text == Model_Select_uText01.Text &&
                Model_Current_uText02.Text == Model_Select_uText02.Text)
            {
                messageBox.MessageBox_Show("모델 삭제", "등록 모델 정보 삭제",
                    "모델 삭제 오류 발생<br/><br/>현재 적용되어 있는 모델은 삭제할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            if (messageBox.MessageBox_Show("모델 변경", "등록 모델 정보 삭제",
                "확인을 선택하면 선택한 모델의 등록 정보가 삭제됩니다.<br/><br/>선택한 모델을 삭제 하시겠습니까?", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.Cancel) return;

            var SelectRow = this.Model_uGrid1.ActiveRow.Index;
            var rowRead = this.uDataSource.Rows[SelectRow];
            this.uDataSource.Rows.Remove(rowRead);

            this.DataSourceReflash();


            string modelName = String_Trim_BothSide(Model_Select_uText01.Text) + "-" + String_Trim_BothSide(Model_Select_uText02.Text) + ".rcp";
            string deleteFileName = Environment.CurrentDirectory + "\\Data\\ModelData\\" + modelName;
            //string deleteFileName = Environment.CurrentDirectory + "\\ModelData\\" + modelName;

            System.IO.File.Delete(deleteFileName);

            this.GridScrollCheck();
            //this.uGridExcelExporter.Export(this.Model_uGrid1, FileName);
            this.GridScrollCheck();
            Model_uGrid1_NowRowDisplay();
        }


        public string String_Trim_BothSide(string sourceString)
        {
            string targetString = sourceString.TrimStart();
            targetString = targetString.TrimEnd();
            return targetString;
        }

        private int _intGridScrollWidth = 100;
        private void GridScrollCheck()
        {
            if (uDataSource.Rows.Count > 9)
                this.Model_uGrid1.DisplayLayout.Bands["Band 0"].Columns["RecipeNo"].Width = _intGridScrollWidth - 17;
            else
                this.Model_uGrid1.DisplayLayout.Bands["Band 0"].Columns["RecipeNo"].Width = _intGridScrollWidth;
        }

        private void Model_uGrid1_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            Model_uGrid1_NowRowDisplay();
        }

        private void Model_uGrid1_NowRowDisplay()
        {
            if (this.Model_uGrid1.ActiveRow == null)
            {
                Model_Select_uText01.Text = "";
                Model_Select_uText02.Text = "";
                return;
            }

            var SelectRow = this.Model_uGrid1.ActiveRow.Index;
            var CuurrentRow = this.Model_uGrid1.Rows[SelectRow].Cells;
            Model_Select_uText01.Text = CuurrentRow[1].Value.ToString();
            Model_Select_uText02.Text = CuurrentRow[2].Value.ToString();
        }

        private List<string> fileNameList;// = new List<string>(); 
        private void FormDlgModelReg_Load(object sender, EventArgs e)
        {
            Model_Current_uText01.Text = GetSetModelName;
            Model_Current_uText02.Text = GetSetModelNumber;
            // Scroll이 최하단으로 내려갔을때 빈공간이 없도록 설정
            Model_uGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;

            string folderPath = Environment.CurrentDirectory + "\\Data\\ModelData";
            Control_Files fileSytem = new Control_Files();
            fileNameList = fileSytem.GetFilesName_InFolder(folderPath);
            Display_SaveFile(fileNameList);
        }

        private void Display_SaveFile(List<string> fileNames)
        {
            List<string> SortList = new List<string>();

            for (int i = 1; i < 1000; i++)
            {
                for (int j = 0; j < fileNames.Count / 2; j++)
                {
                    string strValueData = fileNames[j * 2 + 1];
                    int iValueData = int.Parse(strValueData);

                    if (i == iValueData)
                    {
                        SortList.Add(fileNames[j * 2]);
                        SortList.Add(fileNames[j * 2 + 1]);
                    }
                }
            }

            for (int i = 0; i < SortList.Count / 2; i++)
            {
                //UltraDataRow rowRead;
                this.uDataSource.Rows.Add();
                UltraDataRow rowRead = this.uDataSource.Rows[this.uDataSource.Rows.Count - 1];
                rowRead["NO"] = (i + 1).ToString();
                rowRead["Name"] = SortList[i * 2];
                rowRead["RecipeNo"] = SortList[i * 2 + 1];
            }
        }
        private void Model_Cancel_uBtn05_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void Model_Current_uText02_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Model_Select_uText02_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
        
    }
}
