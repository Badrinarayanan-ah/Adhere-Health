using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileLastWriteTimeChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //loop for files in the folder matching a given pattern (*.* or ALL, *.txt for text files, etc).
            //we also get a count of files that are considered too small to be useful (5 bytes is a typical blank text file or a file with a size that we can't use for drug/npi/dea processing)
            //we also look for files last written to in a certain number of days - sometimes these files are weekly, sometimes monthly - the input arguments allow for control of these
            //days since last file defaults 10 
            try
            {
                int totalfilecount = 0;
                int recentfilescount = 0;
                int fileswithsmallsizecount = 0;
                double fileswithsmallsizepercentage = 0;
                int smallfilebytes = 5;

                Arguments a = new Arguments();
                a.ProcessArguments(args);
               
                //errors are checked for by the processarguments step - does the folder exist, are the arguments valid and/or valid types, etc
                if (!(a.HasError))
                {
                    //special check for refdrug
                    if(a.FileFolder.ToLower().Contains("refdrug"))
                    {
                        bool packagefilefound = false;

                        //if this file is blank FAIL the whole thing
                        foreach (FileInfo fi in new DirectoryInfo(a.FileFolder).GetFiles(a.FileFilter))
                        {
                            if(fi.Name.ToLower()== "package.txt")
                            {
                                packagefilefound = true;

                                if (fi.Length<=smallfilebytes)
                                {
                                    Console.Out.WriteLine("");
                                    Console.Out.WriteLine("");
                                    Console.Out.WriteLine("The Package file (" + fi.FullName + ") for RefDrug is too small. Please check with Infrastructure on pulling a new set of files or checking the license: " + a.FileFolder);

                                    System.Threading.Thread.Sleep(5000);
                                    System.Environment.Exit(1);
                                }
                            }
                        }

                        if (!packagefilefound)
                        {
                            Console.Out.WriteLine("");
                            Console.Out.WriteLine("");
                            Console.Out.WriteLine("The Package file for RefDrug could not be located. Please check with Infrastructure on pulling a new set of files or checking the license: " + a.FileFolder);

                            System.Threading.Thread.Sleep(5000);
                            System.Environment.Exit(1);
                        }
                    }


                    foreach (FileInfo fi in new DirectoryInfo(a.FileFolder).GetFiles(a.FileFilter))
                    {
                        totalfilecount++;

                        string lastwritetime = fi.LastWriteTime.ToString();
                        string lastaccesstime = fi.LastAccessTime.ToString();

                        //number of days since last write
                        //double dayssince= (fi.LastWriteTime - DateTime.Now).TotalDays;
                        double dayssince = (DateTime.Now - fi.LastWriteTime).TotalDays;

                        if (dayssince<=Convert.ToInt32(a.DaysSinceLastFile))
                        {
                            recentfilescount++;

                            if (fi.Length <= smallfilebytes)
                            {
                                fileswithsmallsizecount++;
                            }
                        }
                    }

                    Console.Out.WriteLine(totalfilecount.ToString() + " total file(s)/" + recentfilescount.ToString() + " recent file(s)/" + fileswithsmallsizecount.ToString() + " small file(s)");

                    if(recentfilescount > 0)
                    {
                        fileswithsmallsizepercentage = (float)fileswithsmallsizecount / (float)recentfilescount;
                    }

                    //throw error checking
                    if (totalfilecount == 0)
                    {
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("No files were found in the entered directory: " + a.FileFolder);

                        System.Threading.Thread.Sleep(5000);
                        System.Environment.Exit(1);
                    }
                    else if (recentfilescount == 0)
                    {
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("No recent files were found in the entered directory: " + a.FileFolder);

                        System.Threading.Thread.Sleep(5000);
                        System.Environment.Exit(1);
                    }
                    else if (fileswithsmallsizepercentage >= 0.25)
                    {
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("25% or more of the files found were under the 1 KB default file limit for the folder: " + a.FileFolder);

                        System.Threading.Thread.Sleep(5000);
                        System.Environment.Exit(1);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("The input arguments are not valid");

                    System.Threading.Thread.Sleep(5000);
                    System.Environment.Exit(1);
                }
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("An Unidentified Application Error: " + ex.Message + " - " + ex.StackTrace);

                System.Threading.Thread.Sleep(5000);
                System.Environment.Exit(1);
            }
        }
    }
}
