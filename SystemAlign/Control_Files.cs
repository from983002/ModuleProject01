using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SystemAlign
{
    public class IsAccess
    {
        static public bool IsAccessAble(String path)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(path, FileMode.Open,FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //에러가 발생한 이유는 이미 다른 프로세서에서 점유중이거나.
                //혹은 파일이 존재하지 않기 때문이다.
                return false;
            }

            finally
            {
                if (fs != null)
                {
                    //만약에 파일이 정상적으로 열렸다면 점유중이 아니다.
                    //다시 파일을 닫아줘야 한다.
                    fs.Close();
                }
            }
            return true;
        }
    }
    public class Control_Files
    {
        private string fileNameLogin = string.Empty;
        private string fileNameInspect = string.Empty;
        private string fileNameExcel = string.Empty;

        public string GetSet_FileNameLogin
        {
            get { return fileNameLogin; }
            set { fileNameLogin = value; }
        }

        public string GetSet_FileNameInspect
        {
            get { return fileNameInspect; }
            set { fileNameInspect = value; }
        }

        public string GetSet_FileNameExcel
        {
            get { return fileNameExcel; }
            set { fileNameExcel = value; }
        }

        public void File_IO_Folder_Check_Or_Make(string checkFolder)
        {
            if (Directory.Exists(checkFolder) == false)
                Directory.CreateDirectory(checkFolder);
        }

        private StreamWriter streamWriterLogin;
        private StreamWriter streamWriterInspect;

        List<string> strLstmodelName = new List<string>();
        List<string> strLstrecipeNO = new List<string>(); 
        public List<string> GetFilesName_InFolder(string folderPath)
        {
            if (Directory.Exists(folderPath) == false) return null;
            strLstmodelName.Clear();
            string[] files = Directory.GetFiles(folderPath);
            foreach (var afile in files)
            {
                if (afile.IndexOf(".rcp") > -1)
                {
                    int nameIndex = afile.LastIndexOf("\\");
                    string fileName = afile.Substring(nameIndex + 1, afile.Length - nameIndex - 1);

                    int frontIndex = fileName.IndexOf("-");
                    int rearIndex = fileName.IndexOf(".rcp");
                    string modelName = fileName.Substring(0, frontIndex);
                    string recipeNo = fileName.Substring(frontIndex+1, rearIndex - frontIndex-1);
                    strLstmodelName.Add(modelName);
                    strLstmodelName.Add(recipeNo);
                }
            }
            return strLstmodelName;
        }
        /*
        StreamWriter strLoginFile = new StreamWriter(fileName, true, Encoding.Default);
            strLoginFile.Flush();

            string logData = String.Format("[{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}] ",
                _dtInspLogFileTime.Year, _dtInspLogFileTime.Month, _dtInspLogFileTime.Day, _dtInspLogFileTime.Hour,
                _dtInspLogFileTime.Minute, _dtInspLogFileTime.Second, _dtInspLogFileTime.Millisecond);

            logData += FormDlgInsp_Inspection_LogData_Message_Make(msgNo);
            strLoginFile.WriteLine(logData);

            strLoginFile.Close();
        */
        public void File_IO_Text_File_Check_Or_Make(string checkFile,int typeNo)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                if (typeNo == 1)
                {
                    streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterLogin.Flush();
                    streamWriterLogin.Close();
                }
                else if (typeNo == 2)
                {
                    streamWriterInspect = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterInspect.Flush();
                    streamWriterInspect.Close();
                }
            }
            else
            {
                if (streamWriterLogin == null && typeNo == 1)
                {
                    streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterLogin.Flush();
                    streamWriterLogin.Close();
                }
                else if (streamWriterInspect == null && typeNo == 2)
                {
                    streamWriterInspect = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterInspect.Flush();
                    streamWriterInspect.Close();
                }
                    
            }
        }

        public void File_IO_Text_File_Check_Or_Make_Login(string checkFile)
        {
//              var logFileInfo = new FileInfo(checkFile);
//              if (logFileInfo.Exists == false || streamWriterLogin != null)
//             {
//                 streamWriterLogin.Close();
//                 //streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
//                 streamWriterLogin = logFileInfo.CreateText();
//             }
//              else if(streamWriterLogin == null)
//                  //streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
//                  streamWriterLogin = logFileInfo.CreateText();
        }

        public void File_IO_Text_File_Check_Or_Make_Inspect(string checkFile)
        {
//             var logFileInfo = new FileInfo(checkFile);
//             if (logFileInfo.Exists == false)
//             {
//                 streamWriterInspect = new StreamWriter(checkFile, true, Encoding.Default);
//                 streamWriterInspect.Close();
//             }
        }

        IsAccess AccessIs = new IsAccess();
        public void File_IO_Text_File_Write_Login(string checkFile, string writeData)
        {
            streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
            streamWriterLogin.WriteLine(writeData);
            streamWriterLogin.Close();
            streamWriterLogin.Dispose();
        }

        public void File_IO_Text_File_Write_Inspect(string checkFile, string writeData)
        {
                streamWriterInspect = new StreamWriter(checkFile, true, Encoding.Default);
                streamWriterInspect.WriteLine(writeData);
                streamWriterInspect.Close();
                streamWriterInspect.Dispose();
        }

        /*
        public void File_IO_Text_File_Check_Or_Make(string checkFile, int typeNo)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                if (typeNo == 1)
                {
                    streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterLogin.Flush();
                    streamWriterLogin.Close();
                }
                else if (typeNo == 2)
                {
                    streamWriterInspect = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterInspect.Flush();
                    streamWriterInspect.Close();
                }
            }
            else
            {
                if (streamWriterLogin == null && typeNo == 1)
                {
                    streamWriterLogin = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterLogin.Flush();
                    streamWriterLogin.Close();
                }
                else if (streamWriterInspect == null && typeNo == 2)
                {
                    streamWriterInspect = new StreamWriter(checkFile, true, Encoding.Default);
                    streamWriterInspect.Flush();
                    streamWriterInspect.Close();
                }

            }
        }
        */

        private string testtmp = string.Empty;
        public void File_IO_Text_File_Write(string writeData, int typeNo)
        {
            if (typeNo == 1)
                streamWriterLogin.WriteLine(writeData);
            else if (typeNo == 2)
                streamWriterInspect.WriteLine(writeData);
        }

        public void File_IO_Text_File_Write(string filePath, string writeData, int typeNo)
        {
            System.IO.File.WriteAllText(filePath, writeData);
        }

        public void File_IO_Text_File_Close_Inspect()
        {
            if (streamWriterInspect != null)
            {
                streamWriterInspect.Close();
                streamWriterInspect.Dispose();
            }
        }

        public void File_IO_Text_File_Close_Login()
        {
            if (streamWriterLogin != null)
            {
                streamWriterLogin.Close();
                streamWriterLogin.Dispose();
            }
        }

        /*
        public void File_IO_Text_File_Check_Or_Make(string checkFile, int fileType)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                var fs = new FileStream(checkFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //fs.Close();
            }
            else
            {
                
            }

        }


       
        public bool File_IO_Excel_File_Check_Or_Make(string checkFile, int fileType)
        {
            var logFileInfo = new FileInfo(checkFile);
            if (logFileInfo.Exists == false)
            {
                var excelFile = new Control_Excel();
                excelFile.CreateExcelFile(checkFile);
                File_IO_File_Maked_File_Named(checkFile, fileType);
            }

        }
        */

        private void File_IO_File_Maked_File_Named(string fileName, int typeNo)
        {
            switch (typeNo)
            {
                case 1:
                    GetSet_FileNameLogin = fileName;
                    break;
                case 2:
                    GetSet_FileNameInspect = fileName;
                    break;
                case 3:
                    GetSet_FileNameExcel = fileName;
                    break;
            }
        }


//         public void File_IO_Excel_File_Data_Write(string fileName, string strExcelRow, List<string> writeData)
//         {
//             int excelRow = int.Parse(strExcelRow);
//             var excelFile = new Control_Excel();
//             excelFile.ExcelFileClose();
//             excelFile.WriteExcelFile(fileName, excelRow, writeData);
//         }

    }
}
