using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string nursinghomedirectoryname = args[0];
                string productionfoldername = args[1];

                DirectoryInfo subDI = new DirectoryInfo(nursinghomedirectoryname);

                string nursinghomename = string.Empty;
                string filename1 = string.Empty;
                string filename2 = string.Empty;
                string filename3 = string.Empty;

                int filecount = 0;

                foreach (FileInfo fi in subDI.GetFiles())
                {
                    string tempnursinghomename = subDI.Name;
                    nursinghomename = tempnursinghomename;

                    if (fi.Extension.ToLower() == ".pdf")
                    {
                        if (fi.Name.ToLower().Contains("cover letter"))
                        {
                            filename1 = fi.FullName;
                            filecount++;
                        }

                        else if (fi.Name.ToLower().Contains("pharmmd awareness"))
                        {
                            filename2 = fi.FullName;
                            filecount++;
                        }
                        else
                        {
                            filename3 = fi.FullName;
                            filecount++;
                        }
                    }
                }

                string outputpath = productionfoldername + "\\" + nursinghomename + " - MERGED.pdf";

                if (filecount == 3)
                {
                    MergePDFs(outputpath, new string[3] { filename1, filename2, filename3 });

                    System.Environment.Exit(0);
                }
                else
                {
                    Console.Out.WriteLine("All 3 necessary files for LTC Output were not found");
                    System.Environment.Exit(1);
            }
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine(ex.Message + " -" + ex.StackTrace);
                System.Environment.Exit(1);
            }
        
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

