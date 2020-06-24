using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace CheckRunAdHocCampaigns
{
    class Program
    {
        static void Main(string[] args)
        {


            bool runInDebugMode = (ConfigurationManager.AppSettings["RunInDebugMode"].ToString().Trim() == "true") ? true : false;
            bool multipleExecutionsPerDayAllowed = (ConfigurationManager.AppSettings["MultipleExecutionsPerDayAllowed"].ToString().Trim() == "true") ? true : false;
            string excelOutputPathTemplate = ConfigurationManager.AppSettings["ExcelOutputPathTemplate"].ToString().Trim();
            string excelOutputPath = String.Format(excelOutputPathTemplate, DateTime.Now.ToString("yyyyMMdd"));
            bool hasalreadyRunToday = false;
            if ((Directory.Exists(excelOutputPath)) && (Directory.GetFiles(excelOutputPath, @"*.xlsx").Length > 0))
            {
                hasalreadyRunToday = true;
            }

            if (CheckRunAdHocCampaignsHelper.CheckCampaignsShouldRun())
            {
                if ((hasalreadyRunToday && multipleExecutionsPerDayAllowed) || (!hasalreadyRunToday))
                {
                    string pathToBatchFile = ConfigurationManager.AppSettings["CampaignRunnerBatchFilePath"].ToString().Trim();
                    pathToBatchFile = String.Format("\"{0}\"", pathToBatchFile);

                    var processInfo = new ProcessStartInfo("cmd.exe", "/c" + pathToBatchFile);  

                    Console.WriteLine(processInfo.FileName);
                    Console.WriteLine("    " + processInfo.Arguments);
                    Console.WriteLine();

                    if (runInDebugMode)
                        processInfo.CreateNoWindow = false;
                    else
                        processInfo.CreateNoWindow = true;


                    processInfo.UseShellExecute = false;

                    processInfo.RedirectStandardError = true;
                    processInfo.RedirectStandardOutput = true;

                    var process = Process.Start(processInfo);

                    process.Start();

                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (output != string.Empty)
                        Console.WriteLine(output);

                    if (error != string.Empty)
                        Console.WriteLine("ERROR: {0}", error);
                    else
                    {
                        DateTime newDate = CheckRunAdHocCampaignsHelper.GetLastReadyForCampaignRun();
                        CheckRunAdHocCampaignsHelper.SaveLastRun(newDate);
                    }
                }
                else
                {
                    Console.WriteLine("Something to do but campaigns have already processed for today and the application is not configured to run multiple times per day.");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Nothing to do!");
                Console.WriteLine();
            }
            if (runInDebugMode)
            {
                Console.Write("Press Enter to Exit...");
                Console.ReadLine();
            }

        }
    }
}
