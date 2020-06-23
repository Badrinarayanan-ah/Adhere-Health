using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

namespace ExcelToPDF
{
    public class MasterClass
    {
        object oMissing = System.Reflection.Missing.Value;

        public string ExcelFileName;
        public string OutputFileName;

        public void RunConverter()
        {
            string filename = ExcelFileName;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(filename, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            object outputFileName = filename.Replace(".xlsx", ".pdf").Replace(".xls",".pdf");

            OutputFileName = outputFileName.ToString();

            string newfilename = filename.Replace(".xlsx", ".pdf").Replace(".xls",".pdf");

            wb.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, outputFileName, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
            wb.Close(saveChanges, oMissing, oMissing);
            excel.Quit();
        }

    }
}
