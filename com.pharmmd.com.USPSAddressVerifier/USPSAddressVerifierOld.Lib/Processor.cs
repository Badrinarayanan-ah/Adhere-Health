using System;
using System.Collections.Generic;
using System.Text;

namespace USPSAddressVerifier.Lib
{    
    public class Processor
    {
        public bool haserrors = false;

        public string AddressCheckOutput;

        public void SingleAddressMode()
        {
            USPSManager um = new USPSManager(WebToolsClass.UserName, false);

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
        public void ValidateArguments()
        {

        }
    }
}

