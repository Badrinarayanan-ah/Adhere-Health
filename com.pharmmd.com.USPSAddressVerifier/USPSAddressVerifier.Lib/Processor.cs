using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using Microsoft.Win32;
using PharmMDFinal.Lib;
using System.Data;

namespace USPSAddressVerifier.Lib
{
    public class Processor
    {
        public bool haserrors = false;
        public int exactmatch = 0;
        public string usageValue = "";
        public string AddressCheckOutput = "";
        public bool usageValueError = false;
        public string FileName = "";
        public string ReturnFileName = "";
        public int filesize;
        public int prevalidatedcount = 0;

        public string FileBaseFolder = "";
        List<long> lstUsedPatients = new List<long>();

        public void ValidateParameters(string value, string[] array)
        {
            if (array.Length == 1)
            {
                value = array[0].Trim();
            }
            else if (array.Length == 2)
            {
                value = array[1].Trim();
            }
            else
            {
                value = "";
            }

            if (value == "")
            {
                AddressCheckOutput += (@"You entered an invalid mode - please enter a 1 for single address mode or a 2 for a file load mode or a 3 for a database query load mode or 4 for single pmdpatientid load mode. For the file load mode, your file must be called 'c:\addressfile.csv' or 'c:\usps\addressfile.csv'.");
                AddressCheckOutput += System.Environment.NewLine;

                usageValueError = true;
            }

            usageValue = value;
        }

        public void FileAddressMode()
        {
            string testaddressfile = @"c:\usps\testaddressfile.csv";
            FileLooper(testaddressfile, 2);

            string testaccountaddressfile = @"c:\usps\testaccountaddressfile.csv";
            FileLooper(testaccountaddressfile, 1);

        }



        public void FullFileAddressMode(string filename, int mode)
        {
            //mode 1 is a standard address file
            //mode 2 is an account based access file

            FileName = filename;

            WriteToRegistry();

            FileLooper(filename, mode);

        }

        public void SingleAddressMode()
        {
            USPSManager um = new USPSManager(USPSWebToolsClass.UserName, false);

            Address a = new Address();
            //good
            //a.Address1 = "2788 Aston Woods Lane";

            //good
            a.Address1 = "3367 Hallshire Drive";

            //bad
            //a.Address1 = "29343 Aston Woods Lane";
            a.City = "Memphis";
            a.State = "TN";


            //double PO BOX and Street
            a.Address1 = "P O BOX 578";
            a.Address2 = "973 OLD ROME PIKE";
            a.City = "LEBANON";
            a.State = "TN";


            //double PO BOX and Street
            a.Address1 = "POB 241";
            a.Address2 = "151 WABASH ST";
            a.City = "GRAND RIVERS";
            a.State = "KY";

            //double PO BOX and Street
            a.Address1 = "BAD";
            a.Address2 = "***BAD ADDRESS***";
            a.City = "GRAND RIVERS";
            a.State = "KY";

            //double PO BOX and Street
            a.Address1 = "PO BOX 900";
            a.Address2 = "USP LEE FEDERAL PRISON";
            a.City = "JONESVILLE";
            a.State = "VA";

            //double PO BOX and Street
            a.Address1 = "PO BOX 2352 ";
            a.Address2 = "APT D";
            //a.City = "HUNTERSVILLE";
            //a.City = "HUNTSVILLE";
            a.City = "HVILLE";
            a.State = "NC";
            a.Zip = "28070";
            a.ZipPlus4 = "2352";

            a = UnNullifyAddress(a);

            Address validatedAddress = um.ValidateAddress(a);

            POBoxAptLotSwitcher(validatedAddress);

            int goodaddresscount = 0;
            int badaddresscount = 0;
            int totaladdresscount = 1;
            int exactaddresscount = 0;

            DoTheVerifyAddressStuff(a, validatedAddress);

            exactaddresscount = exactmatch;

            if (a.Verified)
            {
                goodaddresscount = 1;
            }
            else
            {
                badaddresscount = 1;
            }

            /*
            if (validatedAddress.FirmName.ToUpper() == "BAD ADDRESS")
            {
                Console.Out.WriteLine("Address invalid according to USPS");
                Console.Out.WriteLine("Incoming address: " + a.Address1 + ", " + a.Address2 + ", " + a.City + ", " + a.State + " " + a.Zip + "-" + a.ZipPlus4);
                badaddresscount = 1;
            }
            else
            {
                Console.Out.WriteLine("GOOD ADDRESS");
                Console.Out.WriteLine(validatedAddress.Address1 + "," + validatedAddress.Address2 + ", " + validatedAddress.City + ", " + validatedAddress.State + " " + validatedAddress.Zip + "-" + validatedAddress.ZipPlus4);
                goodaddresscount = 1;
            }

            a.BuildFullAddressString();
            validatedAddress.BuildFullAddressString();

            if (a.FullAddressString.ToUpper().Trim() == validatedAddress.FullAddressString.ToUpper().Trim())
            {
                string s = a.FullAddressString;
                string s1 = validatedAddress.FullAddressString;

                exactaddresscount = 1;
            }
            */

            AddressCheckOutput += "";
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Good Address Count: " + goodaddresscount;
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Bad Address Count: " + badaddresscount;
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Exact Address Count: " + exactaddresscount;
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Total Address Count: " + totaladdresscount;
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "";
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "";
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "";
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "";
            AddressCheckOutput += System.Environment.NewLine;


            Console.Out.WriteLine("");
            Console.Out.WriteLine("Good Address Count: " + goodaddresscount);
            Console.Out.WriteLine("Bad Address Count: " + badaddresscount);
            Console.Out.WriteLine("Exact Address Count: " + exactaddresscount);
            Console.Out.WriteLine("Total Address Count: " + totaladdresscount);


            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
        }

        public void DoTheVerifyAddressStuff(Address a, Address validatedAddress)
        {
            //SWITCH PO Box and APT in Address 1 to Address2
            //BW 2016
            POBoxAptLotSwitcher(validatedAddress);

            if (validatedAddress.FirmName.ToUpper() == "BAD ADDRESS")
            {

                Console.Out.WriteLine("Address invalid according to USPS");

                AddressCheckOutput += "Address invalid according to USPS";
                AddressCheckOutput += System.Environment.NewLine;

                string address = a.BuildFullAddressString();
                a.OriginalFullAddressString = address;
                a.FullAddressString = "  ";
                Console.Out.WriteLine("Incoming address: " + address);
                Console.Out.WriteLine();

                AddressCheckOutput += "Incoming address: " + address;
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += System.Environment.NewLine;

                a.Verified = false;
            }
            else
            {
                Console.Out.WriteLine("GOOD ADDRESS");

                AddressCheckOutput += "GOOD ADDRESS";
                AddressCheckOutput += System.Environment.NewLine;

                string originaladdress = a.BuildFullAddressString();
                string address = validatedAddress.BuildFullAddressString();
                a.FullAddressString = address;
                a.OriginalFullAddressString = originaladdress;

                a.NewAddress1 = validatedAddress.Address1;
                a.NewAddress2 = validatedAddress.Address2;
                a.NewCity = validatedAddress.City;
                a.NewState = validatedAddress.State;
                a.NewZip = validatedAddress.Zip;

                if (originaladdress.ToUpper() == address.ToUpper())
                {
                    exactmatch++;
                    Console.Out.WriteLine("USPS Address Match: " + address);

                    AddressCheckOutput += "USPS Address Match: " + address;
                    AddressCheckOutput += System.Environment.NewLine;
                    AddressCheckOutput += System.Environment.NewLine;
                }
                else
                {
                    Console.Out.WriteLine("Original Address: " + originaladdress);
                    Console.Out.WriteLine("USPS Reformatted Address: " + address);

                    AddressCheckOutput += "Original Address: " + originaladdress;
                    AddressCheckOutput += System.Environment.NewLine;
                    AddressCheckOutput += "USPS Reformatted Address: " + address;
                    AddressCheckOutput += System.Environment.NewLine;
                    AddressCheckOutput += System.Environment.NewLine;
                    a.DoDifferenceChecks(a);

                }



                Console.Out.WriteLine();
                AddressCheckOutput += System.Environment.NewLine;
                a.Verified = true;
            }
        }


        public bool ValidateArguments(string filename, int? selectedindex)
        {
            bool returnValue = true;

            if (selectedindex == null || selectedindex == 0 || selectedindex == 1)
            {
                if (!(File.Exists(filename)))
                {
                    AddressCheckOutput += "The file (" + filename + ") does not exist or is inaccessible.";
                    AddressCheckOutput += System.Environment.NewLine;
                    AddressCheckOutput += System.Environment.NewLine;
                    returnValue = false;
                }
                else
                {
                    returnValue = true;
                }
            }
            else
            {
                returnValue = true;
            }

            return returnValue;
        }

        public void POBoxAptLotSwitcher(Address a)
        {
            if ((a.Address1.ToUpper().StartsWith("PO BOX") || a.Address1.ToUpper().StartsWith("APT") || a.Address1.ToUpper().StartsWith("LOT") || a.Address1.ToUpper().StartsWith("BOX ") || a.Address1.ToUpper().StartsWith("UNIT ") || a.Address1.ToUpper().StartsWith("RT ") || a.Address1.ToUpper().StartsWith("C/O ") || a.Address1.ToUpper().StartsWith(@"C\O ")) && a.Address2.Trim().Length > 0)
            {
                String adr1a = a.Address1;
                string adr2a = a.Address2;

                a.Address2 = adr1a;
                a.Address1 = adr2a;
            }

            if (a.Address2.Trim().Length > 0 && a.Address1.Trim().Length == 0)
            {
                String adr1a = a.Address1;
                string adr2a = a.Address2;

                a.Address2 = adr1a;
                a.Address1 = adr2a;
            }
        }

        public void FileSizeGrabber(string filename)
        {
            filesize = 0;

            //get the file size
            if (File.Exists(filename))
            {
                using (StreamReader swFull = new StreamReader(filename))
                {
                    filesize = swFull.ReadToEnd().Split(new char[] { '\t', '|' }).Length;
                }
            }
        }

        public void DatabaseLooper(int? pmdclientid, int? pmdpatientid)
        {
            DataActions da = new DataActions();
            da.ClearReturnedUSPSAddresses();

            //lstUsedPatients=CurrentPatientSentList();

            string outputfile = @"c:\usps\dbaddressfile_" + DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString() + ".txt";

            if (Directory.Exists(@"c:\usps"))
            {
            }
            else
            {   
                Directory.CreateDirectory(@"c:\usps");
            }

            /*
            string basesql= "select distinct top 25 patient.pmd_patient_id, patient.cardholder_addr_1, patient.cardholder_addr_2, patient.cardholder_city, patient.cardholder_state, patient.cardholder_zip, null, client.client from patient(nolock) join client(nolock) on patient.pmd_client_id = client.pmdclientid";
            basesql += " where len(patient.cardholder_addr_1) > 0 and pmd_patient_id not in (select distinct a.patientid from USPSPatientAddressCheckList a where a.address1 = patient.cardholder_addr_1)";

            if(pmdclientid!=null)
            {
                basesql += " and patient.pmd_client_id=" + pmdclientid.ToString();
            }
            */

            //SQLETL - ETLCORE
            StreamWriter sw = new StreamWriter(outputfile);

            //basesql = AppendPatientList(basesql);
            //DataActions da = new DataActions();
            //DataTable dt = da.GetETLCoreAddresses(basesql);
            //DataTable dt = da.ReturnETLCoreUSPSAPIEligibleList(pmdclientid,5000);

            //DataTable dt = da.ReturnETLCoreUSPSAPIEligibleList(pmdclientid, 100, pmdpatientid);
            DataTable dt = da.ReturnETLCoreUSPSAPIEligibleList(pmdclientid, 500, pmdpatientid);
            //DataTable dt = da.ReturnETLCoreUSPSAPIEligibleList(pmdclientid, 100, pmdpatientid);

            sw.WriteLine("PMDPatientID|Address1|Address2|City|State}Zip|DOB|Client");

            foreach (DataRow dr in dt.Rows)
            {
                sw.WriteLine(dr["pmd_patient_id"].ToString() + "|" + dr["cardholder_addr_1"].ToString() + "|" + dr["cardholder_addr_2"].ToString() + "|" + dr["cardholder_city"].ToString() + "|" + dr["cardholder_state"].ToString() + "|" + dr["cardholder_zip"].ToString() + "|" + "" + "|" + dr["client"].ToString());
            }



            sw.Close();

            FileLooper(outputfile, 2);

            //MIRRORS ON SQLDW


            outputfile = @"c:\usps\dbaddressfile_" + DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString() + ".txt";

            sw = new StreamWriter(outputfile);

            //DataTable dt2 = da.ReturnMirrorsUSPSAPIEligibleList(pmdclientid, 5000);
            DataTable dt2 = da.ReturnMirrorsUSPSAPIEligibleList(pmdclientid, 5000, pmdpatientid);

            foreach (DataRow dr in dt2.Rows)
            {
                sw.WriteLine(dr["pmd_patient_id"].ToString() + "|" + dr["cardholder_addr_1"].ToString() + "|" + dr["cardholder_addr_2"].ToString() + "|" + dr["cardholder_city"].ToString() + "|" + dr["cardholder_state"].ToString() + "|" + dr["cardholder_zip"].ToString() + "|" + "" + "|" + dr["client"].ToString());
            }

            sw.Close();

            FileLooper(outputfile, 2);

            int datacounter1 = dt.Rows.Count;
            int datacounter2 = dt2.Rows.Count;

            outputfile = @"c:\usps\dbaddressfile_" + DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString() + ".txt";

            //wrong number guys
            /*
            sw = new StreamWriter(outputfile);

            DataTable dt3 = da.WrongNumberPatientsReportWithDemographics(pmdclientid, DateTime.Now.Year, DateTime.Today.AddDays(-90), DateTime.Today);

            foreach (DataRow dr in dt3.Rows)
            {
                sw.WriteLine(dr["pmdpatientid"].ToString() + "|" + dr["cardholder_addr_1"].ToString() + "|" + dr["cardholder_addr_2"].ToString() + "|" + dr["cardholder_city"].ToString() + "|" + dr["cardholder_state"].ToString() + "|" + dr["cardholder_zip"].ToString() + "|" + "" + "|" + dr["clientname"].ToString());
            }

            sw.Close();
            

            int datacounter3 = dt3.Rows.Count;
            */

        }

        public void FileLooper(string filename, int mode)
        {
            Addresses masterAddressList = new Addresses();

            //here we can check the address before we do something with it
            //Processor p = new Processor();
            List<Address> lstA = new List<Address>();

            FileBaseFolder = new FileInfo(filename).DirectoryName;

            bool hasaccount = false;
            exactmatch = 0;

            //read the file one line at a time
            if (File.Exists(filename))
            {
                /*
                StreamReader sw = new StreamReader(filename);
                */

                int linecount = 0;

                var lines = File.ReadLines(filename);

                /*while ((line = sw.ReadLine()) != null)*/
                foreach (string line in lines)
                {
                    USPSManager um = new USPSManager(USPSWebToolsClass.UserName, false);

                    if (linecount == 0)
                    {
                        //string[] headerLine = line.Split(new char[] { ',' });
                        //string[] headerLine = line.Split(new char[] { ',','\t'  });

                        string[] headerLine = line.Split(new char[] { '\t', '|' });

                        int tryparseint;

                        if (headerLine[0].ToString().ToUpper() == "ACCOUNTID" || headerLine[0].ToString().ToUpper() == "PATIENTID" || headerLine[0].ToString().ToUpper().EndsWith("ACCOUNTID") || headerLine[0].ToString().ToUpper().EndsWith("PATIENTID"))
                        {
                            hasaccount = true;
                        }

                        if (Int32.TryParse(headerLine[0], out tryparseint))
                        {
                            hasaccount = true;
                        }
                    }

                    if (linecount > 0)
                    {
                        //string[] splitLine = line.Split(new char[] { ',' });
                        //string[] splitLine = line.Split(new char[] { ',', '\t' });

                        if (line == "")
                        {
                        }
                        else if (line.ToLower().Contains("rows affected") || line.ToLower().Contains("row affected"))
                        {
                        }
                        else if (line.Substring(0, 5) == "-----")
                        {
                        }
                        else
                        {
                            string[] splitLine = line.Split(new char[] { '\t', '|' });

                            if (line.Trim() != "")
                            {
                                Address a = new Address();

                                bool doContinue = false;

                                if (hasaccount)
                                {
                                    if (splitLine[1].ToString().ToUpper() == "NULL")
                                    {
                                    }
                                    else
                                    {
                                        doContinue = true;
                                    }
                                }
                                else
                                {
                                    if (splitLine[0].ToString().ToUpper() == "NULL")
                                    {

                                    }
                                    else
                                    {
                                        doContinue = true;
                                    }
                                }

                                if (doContinue)
                                {
                                    if (mode == 2)
                                    {
                                        long myint;

                                        if (Int64.TryParse(splitLine[0].Trim(), out myint))
                                        {
                                            a.AccountID = Convert.ToInt64(splitLine[0].Trim());
                                        }
                                        else
                                        {
                                            a.AccountCode = splitLine[0].Trim();
                                        }

                                        if (splitLine[1].Trim().Length > 38)
                                        {
                                            a.Address1 = splitLine[1].Trim().Substring(0, 38);
                                        }
                                        else
                                        {
                                            a.Address1 = splitLine[1].Trim();
                                        }

                                        if (splitLine[2].Trim().Length > 38)
                                        {
                                            a.Address2 = splitLine[2].Trim().Substring(0, 38);
                                        }
                                        else
                                        {
                                            a.Address2 = splitLine[2].Trim();
                                        }


                                        if (splitLine[3].Trim().Length > 15)
                                        {
                                            a.City = splitLine[3].Trim().PadRight(15).Substring(0, 15);
                                        }
                                        else
                                        {
                                            a.City = splitLine[3].Trim();
                                        }


                                        if (splitLine[4].Trim().Length > 2)
                                        {
                                            a.State = splitLine[4].Trim().Substring(0, 2);
                                        }
                                        else
                                        {
                                            a.State = splitLine[4].Trim();
                                        }

                                        if (splitLine[5].Trim().Length > 5)
                                        {
                                            a.Zip = splitLine[5].Trim().Substring(0, 5);
                                        }
                                        else
                                        {
                                            a.Zip = splitLine[5].Trim();
                                        }

                                        if (splitLine.Length > 6)
                                        {
                                            if (splitLine[6].Length > 4)
                                            {
                                                a.ZipPlus4 = splitLine[6].Trim().Substring(0, 4);
                                            }
                                            else
                                            {
                                                a.ZipPlus4 = splitLine[6].Trim();
                                            }
                                        }

                                        if (mode == 2)
                                        {
                                            try
                                            {
                                                a.ClientCode = splitLine[7].Trim();
                                            }
                                            catch { }
                                        }

                                        a = UnNullifyAddress(a);
                                    }
                                    else if (mode == 1)
                                    {
                                        if (splitLine[0].Trim().Length > 38)
                                        {
                                            a.Address1 = splitLine[0].Trim().Substring(0, 38);
                                        }
                                        else
                                        {
                                            a.Address1 = splitLine[0].Trim();
                                        }

                                        if (splitLine[1].Trim().Length > 38)
                                        {
                                            a.Address2 = splitLine[1].Trim().Substring(0, 38);
                                        }
                                        else
                                        {
                                            a.Address2 = splitLine[1].Trim();
                                        }

                                        if (splitLine[2].Trim().Length > 15)
                                        {
                                            a.City = splitLine[2].Trim().Substring(0, 15);
                                        }
                                        else
                                        {
                                            a.City = splitLine[2].Trim();
                                        }

                                        if (splitLine[3].Trim().Length > 2)
                                        {
                                            a.State = splitLine[3].Trim().Substring(0, 2);
                                        }
                                        else
                                        {
                                            a.State = splitLine[3].Trim();
                                        }

                                        if (splitLine[4].Trim().Length > 5)
                                        {
                                            a.Zip = splitLine[4].Trim().Substring(0, 5);
                                        }
                                        else
                                        {
                                            a.Zip = splitLine[4].Trim();
                                        }

                                        if (splitLine[5].Trim().Length > 4)
                                        {
                                            a.ZipPlus4 = splitLine[5].Trim().Substring(0, 4);
                                        }
                                        else
                                        {
                                            a.ZipPlus4 = splitLine[5].Trim();
                                        }

                                        a = UnNullifyAddress(a);
                                    }

                                    //change a 0 zip to a blank
                                    /*
                                    if (a.Zip == "0")
                                    {
                                        a.Zip = "";
                                    }
                                    */


                                    a.BuildFullAddressString();

                                    if (mode == 2)
                                    {
                                        //if (masterAddressList.lstAddresses.Any(x => x.addressstring == a.OriginalFullAddressString)==true)
                                        if (masterAddressList.lstAddresses.Any(x => x.addressstring == a.FullAddressString) == true)
                                        {

                                        }
                                        else
                                        {
                                            //masterAddressList.lstAddresses.Add(new Addresses(a.OriginalFullAddressString, Convert.ToInt32(a.AccountID)));
                                            masterAddressList.lstAddresses.Add(new Addresses(a.FullAddressString, Convert.ToInt32(a.AccountID)));
                                        }
                                    }


                                    /*
                                    if (RemoveAddressFromListIfAlreadyValidated(a.AccountID))
                                    {
                                        prevalidatedcount++;
                                    }
                                    else
                                    */
                                    {

                                        Address validatedAddress = new Address();

                                        POBoxAptLotSwitcher(a);

                                        validatedAddress = um.ValidateAddress(a);
                                        DoTheVerifyAddressStuff(a, validatedAddress);

                                        lstA.Add(a);

                                        if (mode == 2)
                                        {
                                            if (a.AccountID != 0)
                                            {
                                                DataActions da = new DataActions();
                                                da.InsertSentPatient(a.AccountID, a.Address1);
                                            }
                                        }
                                        validatedAddress.AccountID = a.AccountID;
                                    }
                                }
                            }

                            if (linecount % 1000 == 0)
                            {
                                Console.Out.WriteLine("(" + linecount + " lines read)");

                            }
                        }
                    }

                    um = null;
                    linecount++;

#if DEBUG
                    /*
                    if (linecount == 500)
                    {
                        break;
                    }
                     */
#endif

                }

                /*sw.Close();*/

                //Console.Out.Write(lstA.ToString());

                string filenamestart = new FileInfo(filename).Name;
                filenamestart = filenamestart.Substring(0, filenamestart.IndexOf("."));

                string returnedFilename = "";

                returnedFilename = @"" + FileBaseFolder + "/" + filenamestart + "_returned_" + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Year.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString() + ".txt";

                try
                {
                    StreamWriter swReturned = new StreamWriter(returnedFilename);
                    ReturnFileName = returnedFilename;

                    string thedelimiter = "||";

                    if (hasaccount)
                    {
                        swReturned.WriteLine("Stats " + thedelimiter + "Total Count:" + lstA.Count.ToString() + thedelimiter + "Verified Count:" + lstA.Count(x => x.Verified).ToString() + thedelimiter + "Unique Address Count:" + thedelimiter + masterAddressList.lstAddresses.Count.ToString() + thedelimiter);

                        if (mode == 1)
                        {
                            swReturned.WriteLine("Account ID" + thedelimiter + "Original Address" + thedelimiter + "Returned Address" + thedelimiter + "Verified" + thedelimiter + "Match?" + thedelimiter + "SQL" + thedelimiter + "Differences" + thedelimiter + "Soundex");
                        }
                        else
                        {
                            swReturned.WriteLine("Account Code" + thedelimiter + "Original Address" + thedelimiter + "Returned Address" + thedelimiter + "Verified" + thedelimiter + "Match?" + thedelimiter + "SQL" + thedelimiter + "Differences" + thedelimiter + "Soundex");
                        }
                    }
                    else
                    {
                        swReturned.WriteLine("Stats " + thedelimiter + "Total Count:" + lstA.Count.ToString() + thedelimiter + "Verified Count:" + lstA.Count(x => x.Verified).ToString() + thedelimiter + thedelimiter + thedelimiter);
                        swReturned.WriteLine("Original Address" + thedelimiter + "Returned Address" + thedelimiter + "Verified" + thedelimiter + "Match?" + thedelimiter + "Differences" + thedelimiter + "Soundex");
                    }

                    int adrcount = 0;

                    DataActions da = new DataActions();

                    int addresscount = lstA.Count;

                    foreach (Address a in lstA)
                    {
                        adrcount++;



                        if (hasaccount)
                        {
                            if (a.Verified)
                            {
                                if ((a.FullAddressString.ToUpper().Substring(0, a.FullAddressString.Length - 5) == a.OriginalFullAddressString.ToUpper()) && (a.FullAddressString.ToUpper().Substring(a.FullAddressString.Length - 5, 5).Contains("-")))
                                {
                                    swReturned.WriteLine(a.AccountID == 0 ? a.AccountCode : a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "True" + thedelimiter + @"""""" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                    da.InsertUSPSReturnVerification(a.AccountID, a.OriginalFullAddressString, a.FullAddressString, a.Verified, true, a.Differences, a.SoundexValue, a.Address1, a.Address2, a.City, a.State, a.Zip, a.NewAddress1, a.NewAddress2, a.NewCity, a.NewState, a.NewZip);

                                    /*
                                    if (mode == 1)
                                    {
                                        swReturned.WriteLine(a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "True" + thedelimiter + """" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                    }
                                    else
                                    {
                                        swReturned.WriteLine(a.AccountCode.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "True" + thedelimiter + """" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                    }
                                    */
                                }
                                else if (a.FullAddressString.ToUpper() == a.OriginalFullAddressString.ToUpper())
                                {
                                    swReturned.WriteLine(a.AccountID == 0 ? a.AccountCode : a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "True" + thedelimiter + @"""""" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                    da.InsertUSPSReturnVerification(a.AccountID, a.OriginalFullAddressString, a.FullAddressString, a.Verified, true, a.Differences, a.SoundexValue, a.Address1, a.Address2, a.City, a.State, a.Zip, a.NewAddress1, a.NewAddress2, a.NewCity, a.NewState, a.NewZip);

                                    /*
                                    if (mode == 1)
                                    {
                                        swReturned.WriteLine(a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "True" + thedelimiter + """" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                    }
                                    else
                                    {
                                        swReturned.WriteLine(a.AccountCode.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "True" + thedelimiter + """" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                    }
                                    */
                                }
                                else
                                {
                                    if (a.NewAddress2 != "" && a.NewAddress1 == "")
                                    {
                                        if ((a.FullAddressString.ToUpper().Substring(0, a.FullAddressString.Length - 5) == a.OriginalFullAddressString.ToUpper()) && (a.FullAddressString.ToUpper().Substring(a.FullAddressString.Length - 5, 5).Contains("-")))
                                        {
                                            swReturned.WriteLine(a.AccountID == 0 ? a.AccountCode : a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + "update statement here" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                            da.InsertUSPSReturnVerification(a.AccountID, a.OriginalFullAddressString, a.FullAddressString, a.Verified, false, a.Differences, a.SoundexValue, a.Address1, a.Address2, a.City, a.State, a.Zip, a.NewAddress1, a.NewAddress2, a.NewCity, a.NewState, a.NewZip);

                                            /*
                                            if (mode == 1)
                                            {
                                                swReturned.WriteLine(a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + "update statement here" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                            }
                                            else
                                            {
                                                swReturned.WriteLine(a.AccountCode.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + "update statement here" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                            }
                                            */
                                        }
                                        else
                                        {
                                            int dosomethingelse = 14;
                                        }
                                    }
                                    else
                                    {
                                        swReturned.WriteLine(a.AccountID == 0 ? a.AccountCode : a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + "update statement here" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);



                                        da.InsertUSPSReturnVerification(a.AccountID, a.OriginalFullAddressString, a.FullAddressString, a.Verified, false, a.Differences, a.SoundexValue, a.Address1, a.Address2, a.City, a.State, a.Zip, a.NewAddress1, a.NewAddress2, a.NewCity, a.NewState, a.NewZip);


                                        /*
                                        if (mode == 1)
                                        {
                                            swReturned.WriteLine(a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + "update statement here" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                        }
                                        else
                                        {
                                            swReturned.WriteLine(a.AccountCode.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + "update statement here" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                        }
                                        */
                                    }
                                }
                            }
                            else
                            {
                                swReturned.WriteLine(a.AccountID == 0 ? a.AccountCode : a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + @"""""" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                da.InsertUSPSReturnVerification(a.AccountID, a.OriginalFullAddressString, a.FullAddressString, a.Verified, false, a.Differences, a.SoundexValue, a.Address1, a.Address2, a.City, a.State, a.Zip, a.NewAddress1, a.NewAddress2, a.NewCity, a.NewState, a.NewZip);

                                /*
                                if (mode == 1)
                                {
                                    swReturned.WriteLine(a.AccountID.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + """" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                }
                                else
                                {
                                    swReturned.WriteLine(a.AccountCode.ToString() + thedelimiter + a.OriginalFullAddressString + thedelimiter + a.FullAddressString + thedelimiter + a.Verified + thedelimiter + "False" + thedelimiter + """" + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                                }
                                */
                            }
                        }
                        else
                        {
                            swReturned.WriteLine(a.OriginalFullAddressString + thedelimiter + a.FullAddressString + " " + thedelimiter + a.Verified + " " + thedelimiter + "False" + thedelimiter + " " + thedelimiter + thedelimiter + a.Differences + thedelimiter + a.SoundexValue);
                            int stepper = 14;
                            //da.InsertUSPSReturnVerification(a.AccountID, a.OriginalFullAddressString, a.FullAddressString, a.Verified, false, a.Differences, a.SoundexValue, a.Address1, a.Address2, a.City, a.State, a.Zip, a.NewAddress1, a.NewAddress2, a.NewCity, a.NewState, a.NewZip);
                        }

                        if (adrcount % 1000 == 0)
                        {
                            swReturned.Flush();

                        }
                    }

                    swReturned.Close();
                }
                catch (Exception ex)
                {
                    haserrors = true;

                    Console.Out.WriteLine("Output File Write Failure" + ex.Message + "-" + ex.StackTrace);
                    Console.Out.WriteLine("");

                    AddressCheckOutput += "Output File Write Failure" + ex.Message + "-" + ex.StackTrace;
                    AddressCheckOutput += System.Environment.NewLine;

                }

                Console.Out.WriteLine("File: " + filename);
                Console.Out.WriteLine(lstA.Count.ToString() + " total lines");
                Console.Out.WriteLine(lstA.Count(x => x.Verified).ToString() + " verified lines");
                Console.Out.WriteLine(masterAddressList.lstAddresses.Count.ToString() + " unique address lines");

                Console.Out.WriteLine("");

                AddressCheckOutput += "File: " + filename;
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += lstA.Count.ToString() + " total lines";
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += lstA.Count(x => x.Verified).ToString() + " verified lines";
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += masterAddressList.lstAddresses.Count.ToString() + " unique address lines";

                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += System.Environment.NewLine;

            }
            else
            {
                Console.Out.WriteLine("File " + filename + " does not exist");
                Console.Out.WriteLine("Also, the file you use must have one of the following formatting of headers and rows: ");
                Console.Out.WriteLine("Address1, Address2, City, State, Zip, Zip4 OR");
                Console.Out.WriteLine("AccountID, Address1, Address2, City, State, Zip, Zip4");

                Console.Out.WriteLine("");

                AddressCheckOutput += "File " + filename + " does not exist";
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += "Also, the file you use must have one of the following formatting of headers and rows: ";
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += "Address1, Address2, City, State, Zip, Zip4 OR";
                AddressCheckOutput += System.Environment.NewLine;
                AddressCheckOutput += "AccountID, Address1, Address2, City, State, Zip, Zip4";
                AddressCheckOutput += System.Environment.NewLine;

                AddressCheckOutput += System.Environment.NewLine;

            }

            Console.Out.WriteLine("Good Address Count: " + lstA.Count(x => x.Verified == true));
            Console.Out.WriteLine("Bad Address Count: " + lstA.Count(x => x.Verified == false));
            Console.Out.WriteLine("Exact Address Count: " + exactmatch.ToString());
            Console.Out.WriteLine("Total Address Count: " + lstA.Count.ToString());
            Console.Out.WriteLine("------------------------------------------------------");

            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");

            AddressCheckOutput += "Good Address Count: " + lstA.Count(x => x.Verified == true);
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Bad Address Count: " + lstA.Count(x => x.Verified == false);
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Exact Address Count: " + exactmatch.ToString();
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "Total Address Count: " + lstA.Count.ToString();
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += "------------------------------------------------------";
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += System.Environment.NewLine;
            AddressCheckOutput += System.Environment.NewLine;

        }

        internal void WriteToRegistry()
        {
            try
            {
                string subkeyname = "Software";
                string appname = "USPSAddressVerifier";

                RegistryKey key = Registry.LocalMachine.OpenSubKey(subkeyname, true);
                RegistryKey tfmKey = key.OpenSubKey(appname, true);

                if (tfmKey != null)
                {
                }
                else
                {
                    key.CreateSubKey(appname, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    tfmKey = key.OpenSubKey(appname, true);
                }



                tfmKey.SetValue("FileName", FileName);

                tfmKey.Close();
                key.Close();
            }
            catch { }
        }

        internal Address UnNullifyAddress(Address a)
        {
            if (a.Zip.ToUpper() == "NULL")
            {
                a.Zip = "";
            }

            if (a.ZipPlus4.ToUpper() == "NULL")
            {
                a.ZipPlus4 = "";
            }


            if (a.City.ToUpper() == "NULL")
            {
                a.City = "";
            }


            if (a.State.ToUpper() == "NULL")
            {
                a.State = "";
            }


            if (a.Address1.ToUpper() == "NULL")
            {
                a.Address1 = "";
            }


            if (a.Address2.ToUpper() == "NULL")
            {
                a.Address2 = "";
            }

            if (a.AccountCode.ToUpper() == "NULL")
            {
                a.AccountCode = "";
            }

            /*
            if (a.AccountID.ToUpper() == "NULL")
            {
                a.AccountID = "";
            }
            */

            return a;
        }

        /*
        internal List<long> CurrentPatientSentList()
        {
            PharmMDFinal.Lib.Processor p = new PharmMDFinal.Lib.Processor();

            DataActions da = new DataActions();
            return p.ValidatedPatientList();

        }

        internal bool RemoveAddressFromListIfAlreadyValidated(long accountid)
        {
            bool returnValue = false;

            if (lstUsedPatients.Contains(accountid))
            {
                returnValue = true;
            }

            return returnValue;
        }

        public string AppendPatientList(string input)
        {
            string output = input;

            int j = lstUsedPatients.Count;

            for (int i = 0; i <= j; i++)
            {
                if (i == 0)
                {
                }
                else if (i == 1)
                {
                    output += " where pmd_patient_id not in(" + lstUsedPatients[i].ToString() + ")";
                }
                else if (i >= 2)
                {
                    int looper = 0;

                    for (looper = 0; looper < lstUsedPatients.Count; looper++)
                    {
                        if (looper == 0)
                        {
                            output += " where pmd_patient_id not in(";
                        }

                        output += lstUsedPatients[i].ToString() + ",";
                    }

                    output = output.TrimEnd(new char[] { ',' });

                    output += ")";
                }
            }

            return output;

        }
        */
    }

    public class Addresses
    {
        public Addresses()
        { }

        public Addresses(string _addressstring, List<int> _i)
        {
            addressstring = _addressstring;
            lstPatients = _i;
        }

        public Addresses(string _addressstring, int patientid)
        {
            addressstring = _addressstring;
            lstPatients.Add(patientid);
        }

        public List<int> lstPatients = new List<int>();
        public string addressstring = "";

        public List<Addresses> lstAddresses = new List<Addresses>();
    }



}

