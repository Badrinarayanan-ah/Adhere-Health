using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

    class ProgramOriginal
    {

        static void Main2(string[] args)
        {
            int counter = 0;

            string directory = @"C:\Users\brian.williams.PHARMMD\Desktop\ABC\staging\ABINGTON MANOR";
            string newdirectory = directory + @"\NEW";

            string file3 = directory + @"\ABINGTON MANOR.pdf";
            string file2 = directory + @"\PharmMD Awareness Letter for LTCs.pdf";
            string file1 = directory + @"\Cover Letter.pdf";

            if(System.IO.Directory.Exists(newdirectory))
            {

            }
            else
            {
            System.IO.Directory.CreateDirectory(newdirectory);
            }

            MergePDFs(newdirectory + @"\Merged.pdf", new string[3] { file1, file2, file3 });

            //PdfSharp.Pdf.PdfDocument doc = PdfSharp.Pdf.open()
         }

        public static void MergePDFs(string targetPath, params string[] pdfs)
        {
            using (PdfDocument targetDoc = new PdfDocument())
            {
                foreach (string pdf in pdfs)
                {
                    using (PdfDocument pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                    {
                        for (int i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }
                targetDoc.Save(targetPath);
            }
        }
}

