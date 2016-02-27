using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClosedXML.Excel;


using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Data;
using System.Drawing;

//ClosedXML을 사용하기 우해서는 다운로드 받은 dll파일을 참조 추가한 후
//DocumentFormat.OpenXml.dll도 같은 폴더에 위치시켜야 한다.
using Infragistics.Win.UltraWinTabControl;

namespace SystemAlign
{
    public class Control_Excel
    {
        //private XLWorkbook workbook;// = new XLWorkbook(strModelFileName);
        //private ClosedXML.Excel.IXLWorksheet worksheet;// = workbook.Worksheet(1);
        private string _strExcelFileName = string.Empty;

        public string GetSet_ExcelFileName
        {
            get { return _strExcelFileName; }
            set { _strExcelFileName = value; }
        }
        /*
         * public void CreateExcelFile(string strExcelName)
        {
            string[] columnNames = new string[] { "모델명", "트리거", "날자", "시간", "위치", "바이셀", "옵셋X", "옵셋Y", "Angle", "측정결과", "장축길이", "장축결과", "단축길이", "단축결과", "세파넓이", "세파결과", "전송시간", "쵤영간격" };

            XLWorkbook workbook = new XLWorkbook();
            ClosedXML.Excel.IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

            for (int i = 0; i < columnNames.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = columnNames[i];
            }
            workbook.SaveAs(strExcelName);
        }
        */

        public void Excel_Folder_Check_Or_Make(string checkFolder)
        {
            if (Directory.Exists(checkFolder) == false)
                Directory.CreateDirectory(checkFolder);
        }

        public void Excel_File_Check_Or_Make(string checkFile)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile(checkFile);
            }
        }

        public void Excel_File_Check_Or_Make_Gap(string checkFile)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile_Gap(checkFile);
            }
        }

        public void Excel_File_Check_Or_Make_Uper(string checkFile, List<string> itemNames )
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile_Uper(checkFile, itemNames);
            }
        }

       

        public void Excel_File_Check_Or_Make_Down(string checkFile, List<string> itemNames)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile_Down(checkFile, itemNames);
            }
        }


        public void Excel_File_Check_Or_Make_Gap_Total(string checkFile)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile_Uper(checkFile);
            }
        }

       
        public void CreateExcelFile_Gap(string strExcelName)
        {
            string[] columnNames = new string[]
            {
                "시간", "갭번호", "측정결과", "좌측간격", "좌측결과", "우측간격", "우축결과", "중심값", "최대값","최소값"
            };

            string writeData = string.Empty;

            for (int i = 0; i < columnNames.Length; i++)
            {
                writeData += columnNames[i] + ",";
            }
            streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
            streamWriterCSV_Uper.WriteLine(writeData);
            streamWriterCSV_Uper.Close();
            streamWriterCSV_Uper.Dispose();
        }

        public void CreateExcelFile_Uper(string strExcelName, List<string>itemNames )
        {
            string writeData = string.Empty;
            string[] columnNames = new string[itemNames.Count + 4];

            writeData = "모델명,트리거,시간,판정,";

            for (int i = 0; i < itemNames.Count; i++)
            {
                writeData += itemNames[i] + ",";
            }
            
            streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
            streamWriterCSV_Uper.WriteLine(writeData);
            streamWriterCSV_Uper.Close();
            streamWriterCSV_Uper.Dispose();
        }

        public void CreateExcelFile_Down(string strExcelName, List<string> itemNames)
        {
            string writeData = string.Empty;
            string[] columnNames = new string[itemNames.Count + 4];

            writeData = "모델명,트리거,시간,판정,";

            for (int i = 0; i < itemNames.Count; i++)
            {
                writeData += itemNames[i] + ",";
            }

            streamWriterCSV_Down = new StreamWriter(strExcelName, true, Encoding.Default);
            streamWriterCSV_Down.WriteLine(writeData);
            streamWriterCSV_Down.Close();
            streamWriterCSV_Down.Dispose();
        }

        public void CreateExcelFile_Uper(string strExcelName)
        {
            string[] columnNames = new string[]
            {
                "모델명", "날자", "시간", "판정", "Gap #1 R","Gap #2 L", "Gap #2 R","Gap #3 L", "Gap #3 R","Gap #4 L", "Gap #4 R",
                "Gap #5 L", "Gap #5 R","Gap #6 L", "Gap #6 R","Gap #7 L", "Gap #7 R","Gap #8 L", "Gap #8 R","Gap #9 L", "Gap #9 R",
                "Gap #10 L", "Gap #10 R", "판정"
            };

            string writeData = string.Empty;

            for (int i = 0; i < columnNames.Length; i++)
            {
                writeData += columnNames[i] + ",";
            }
            streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
            streamWriterCSV_Uper.WriteLine(writeData);
            streamWriterCSV_Uper.Close();
            streamWriterCSV_Uper.Dispose();
        }

        public void Excel_File_Check_Or_Make_BiCell(string checkFile)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile_BiCell(checkFile);
            }
        }

        public void CreateExcelFile_BiCell(string strExcelName)
        {
            string[] columnNames = new string[]
            {
                "날자", "시간", "트리거", "이동거리", "기준 옵셋", "측정값 mm", "측정값 Pix", "해상도", "측정위치", "기준위치"
            };

            string writeData = string.Empty;

            for (int i = 0; i < columnNames.Length; i++)
            {
                writeData += columnNames[i] + ",";
            }
            streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
            streamWriterCSV_Uper.WriteLine(writeData);
            streamWriterCSV_Uper.Close();
            streamWriterCSV_Uper.Dispose();
        }

        public void CreateExcelFile(string strExcelName)
        {
            string[] columnNames = new string[]
            {
                "모델명", "트리거", "날자", "시간", "위치", "바이셀", "옵셋X", "옵셋Y", "Angle", "측정결과", "장축길이", "장축결과", "단축길이", "단축결과",
                "세파넓이", "세파결과", "전송시간", "쵤영간격"
            };

            string writeData = string.Empty;

            for (int i = 0; i < columnNames.Length; i++)
            {
                writeData += columnNames[i] + ",";
            }
            streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
            streamWriterCSV_Uper.WriteLine(writeData);
            streamWriterCSV_Uper.Close();
            streamWriterCSV_Uper.Dispose();
        }

        /*
        public bool Excel_File_Check_Or_Make(string checkFile)
        {
            NowExcelName = checkFile;
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                CreateExcelFile(checkFile);
                return true;
            }
            else
            {
                if (workbook == null)
                {
                    workbook = new XLWorkbook(checkFile);
                    worksheet = workbook.Worksheet(1);
                }
            }

            return false;
        }
        */


        private XLWorkbook workbook;// = new XLWorkbook();
        private ClosedXML.Excel.IXLWorksheet worksheet;// = workbook.Worksheets.Add("Sheet1");
        private string NowExcelName = string.Empty;

        /*
        public void CreateExcelFile(string strExcelName)
        {
            

            ExcelFileClose();
            string[] columnNames = new string[]
            {
                "모델명", "트리거", "날자", "시간", "위치", "바이셀", "옵셋X", "옵셋Y", "Angle", "측정결과", "장축길이", "장축결과", "단축길이", "단축결과",
                "세파넓이", "세파결과", "전송시간", "쵤영간격"
            };

            workbook = new XLWorkbook();
            worksheet = workbook.Worksheets.Add("Sheet1");

            for (int i = 0; i < columnNames.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = columnNames[i];
            }
            workbook.SaveAs(strExcelName);
            worksheet.Dispose();
            workbook.Dispose();

            workbook = new XLWorkbook(strExcelName);
            worksheet = workbook.Worksheet(1);
        }

        */

        public void CloseExcelFile(string strExcelName)
        {
            workbook.Save();
            worksheet.Dispose();
            workbook.Dispose();
        }

        public void CloseExcelFile()
        {
            try
            {
                if (workbook != null) workbook.Save();
                if (worksheet != null) worksheet.Dispose();
                if (workbook != null) workbook.Dispose();
            }
            catch (Exception)
            {
                Thread.Sleep(200);
                if (workbook != null) workbook.Save();
                if (worksheet != null) worksheet.Dispose();
                if (workbook != null) workbook.Dispose();
            }
            
        }
        
        /*
        //검출 에러시에는 나타나지 않던 에러가 발생한다.
        //이유는 검출 에러는 정상적인 작업보다 싸이클 시간이 짧아서 이전의 세이브 프로세스가
        //끝나기 전에 다시 진행 되기 때문에 나타나고 있다.
        //아래와 같이 실패시 200을 대기 후 재 시작하면 검출 에러시에도 잘 자동한다.
        public void WriteExcelFile(int rowNo, List<string> strExcelData, int cellNo)
        {
            //Trace.WriteLine((traceNo).ToString("00000") + " : Method Name : " + MethodBase.GetCurrentMethod().Name);
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                for (int i = 0; i < strExcelData.Count; i++)
                {
                    worksheet.Cell(rowNo, i + 1).Value = strExcelData[i];
                }
                workbook.Save();
            }
            catch (Exception e)
            {
                Thread.Sleep(200);
            }
            finally
            {
                workbook.Save();
            }
        }
       

        public void WriteExcelFile(int rowNo, List<string> strExcelData, int cellNo)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            
            for (int i = 0; i < strExcelData.Count; i++)
            {
                worksheet.Cell(rowNo, i + 1).Value = strExcelData[i];
            }
            
            while (IsAccess.IsAccessAble(NowExcelName) == false)
            {
                Thread.Sleep(50);
            }

            workbook.Save();
        }
 */
        private StreamWriter streamWriterCSV_Uper;
        private StreamWriter streamWriterCSV_Down;

        /*
        public void WriteExcelFile(string strExcelName, string strExcelData)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV.Write(strExcelData);
                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV.Write(strExcelData);
                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            finally
            {
                
            }
        }
        */

        public void WriteExcelFile(string strExcelName, string strExcelData1, string strExcelData2)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV_Uper.Write(strExcelData1 + "," + strExcelData2 + ",");
                streamWriterCSV_Uper.Close();
                streamWriterCSV_Uper.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV_Uper.Write(strExcelData1 + "," + strExcelData2 + ",");
                streamWriterCSV_Uper.Close();
                streamWriterCSV_Uper.Dispose();
            }
            finally
            {

            }
        }

        public void WriteExcelFile(string strExcelName, string strExcelData1, string strExcelData2, string strExcelData3)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV_Uper.Write(strExcelData1 + strExcelData2 + "," + strExcelData3 + ",");
                streamWriterCSV_Uper.Close();
                streamWriterCSV_Uper.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV_Uper.Write(strExcelData1 + strExcelData2 + "," + strExcelData3 + ",");
                streamWriterCSV_Uper.Close();
                streamWriterCSV_Uper.Dispose();
            }
            finally
            {

            }
        }

        /*
        public void WriteExcelFile(string strExcelName, string[] strExcelData)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV.Write(strExcelData[0] + "," + strExcelData[1] + ",");
                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV.Write(strExcelData[0] + "," + strExcelData[1] + ",");
                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            finally
            {

            }
        }
         * 
         * */

        /*
        public void WriteExcelFile(string strExcelName, StringBuilder strExcelData)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV.WriteLine(strExcelData.ToString());
                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV.WriteLine(strExcelData.ToString());
                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
        }
        */

        /*
        public void WriteExcelFile(string strExcelName, string strExcelData, bool crFalg, string[] resultData, int Cells)
        {
            string resultstr = string.Empty;
            try
            {
                for (int i = 0; i < Cells; i++)
                {
                    if (resultData[i] == "NG")
                    {
                        resultstr = "NG";
                        break;
                    }
                    else
                    {
                        resultstr = "OK";
                    }
                }

                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);

                if (crFalg == true)
                {
                    //streamWriterCSV.Write(strExcelData + ",");
                    streamWriterCSV.WriteLine(resultstr);
                    streamWriterCSV.Write(strExcelData + ",");
                }
                else streamWriterCSV.Write(strExcelData + ",");

                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);

                if (crFalg == true) streamWriterCSV.WriteLine(strExcelData);
                else streamWriterCSV.Write(strExcelData);

                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            finally
            {
               
            }
        }
        */
        /*
        public void WriteExcelFile(string strExcelName, string strExcelData, bool crFalg)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);

                if (crFalg == true)
                {
                    streamWriterCSV.WriteLine("");
                    streamWriterCSV.Write(strExcelData + ",");
                }
                else streamWriterCSV.Write(strExcelData + ",");

                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();
                Thread.Sleep(100);
                streamWriterCSV = new StreamWriter(strExcelName, true, Encoding.Default);

                if (crFalg == true) streamWriterCSV.WriteLine(strExcelData);
                else streamWriterCSV.Write(strExcelData);

                streamWriterCSV.Close();
                streamWriterCSV.Dispose();
            }
            finally
            {

            }
        }
        */

        public void WriteExcelFile(string strExcelName, List<string> strExcelData)
        {
            Trace.WriteLine("Method Name : " + MethodBase.GetCurrentMethod().Name);
            try
            {
                string strWriteData = string.Empty;
                for (int i = 0; i < strExcelData.Count; i++)
                {
                    strWriteData += strExcelData[i] + ",";
                }
                streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV_Uper.WriteLine(strWriteData);
                streamWriterCSV_Uper.Close();
                streamWriterCSV_Uper.Dispose();
            }
            catch (Exception e)
            {
                ExcelFileClose();

                Thread.Sleep(100);
                string strWriteData = string.Empty;
                for (int i = 0; i < strExcelData.Count; i++)
                {
                    strWriteData += strExcelData[i] + ",";
                }
                streamWriterCSV_Uper = new StreamWriter(strExcelName, true, Encoding.Default);
                streamWriterCSV_Uper.WriteLine(strWriteData);
                streamWriterCSV_Uper.Close();
                streamWriterCSV_Uper.Dispose();
            }
            finally
            {
               
            }
        }

        public void ExcelFileClose()
        {
            Process[] processList = Process.GetProcessesByName("Excel");
            if (processList.Length > 0)
            {
                foreach (var process in processList)
                {
                    process.Kill();
                }
            }
        }
    }
}
