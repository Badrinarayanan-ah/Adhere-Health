using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Word;

namespace WordToPDF
{
    public class MasterClass
    {
        object oMissing = System.Reflection.Missing.Value;

        public string WordFileName;
        public string OutputFileName;

        public void RunConverter()
        {
            string filename = WordFileName;

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document oWordDoc = new Microsoft.Office.Interop.Word.Document();
            //word.Visible = true;
            word.Visible = false;
            Object oTemplatePath = filename;

            Document doc = word.Documents.Open(ref oTemplatePath, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            doc.Activate();

            object outputFileName = filename.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
            object fileFormat = WdSaveFormat.wdFormatPDF;

            OutputFileName = outputFileName.ToString();

            // Save document into PDF Format
            doc.SaveAs(ref outputFileName,
                ref fileFormat, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Close the Word document, but leave the Word application open.
            // doc has to be cast to type _Document so that it will find the
            // correct Close method.                
            object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
            ((_Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);

            //doc.Close(ref saveChanges, ref oMissing, ref oMissing);
            word.Quit(ref saveChanges, ref oMissing, ref oMissing);

            //doc = null;
        }
    }
}
