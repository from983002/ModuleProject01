using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;


//using Excel = Microsoft.Office.Interop.Excel;

namespace SystemAlign
{
    class CMIAMExcelControl
    {
        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public static int WM_QUIT = 0x12;
        //public Excel.Application xls;
        //public Excel.Worksheet xlsSheet;
        //public Excel.Workbook xlsBook;
        private object opt = Missing.Value;

        //public Excel.Worksheet ExcelSheet
        //{
        //    get { return this.xlsSheet; }
        //    set { this.xlsSheet = value; }
        //}

//         public bool ExcelFileOpen(string filePath)
//         {
//             string fileNamePath = filePath;
//             try
//             {
//                 this.xls = new Excel.Application();
//                 this.xls.Visible = false;
//                 this.xlsBook = (Excel.Workbook)this.xls.Workbooks.Open(fileNamePath, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt, opt);
//                 this.xlsSheet = (Excel.Worksheet)this.xlsBook.ActiveSheet;
//                 Debug.Write("ExcelControl 종료 : " + System.DateTime.Now.ToString() + System.Environment.NewLine);
//                 return true;
//             }
//             catch (System.Exception ex)
//             {
//                 return false;            	
//             }          
//         }
// 
//         public void ExclFileClose()
//         {
//             if (this.xlsSheet != null) Marshal.ReleaseComObject(this.xlsSheet);
//             if (this.xlsBook != null) this.xlsBook.Close(false, System.Type.Missing, System.Type.Missing);
// 
//             this.xls.UserControl = false;
//             IntPtr hwnd = new IntPtr(this.xls.Hwnd);
//             this.xls.Quit();
//             Marshal.ReleaseComObject(xls);
//             PostMessage(hwnd, WM_QUIT, 0, 0);
//         }
    }
}
