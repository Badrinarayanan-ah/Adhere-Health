using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DOTSNCOALive;
using DOTSNCOALive.NCOAService;

namespace USPSAddressVerifier.Lib
{
    public class NCOAManager
    {
        #region Address Methods
        /// <summary>
        /// Validate a single address
        /// </summary>
        /// <param name="address">Address object to be validated</param>
        /// <returns>Validated Address</returns>
        public NCOAAddress ValidateAddress(Address address)
        {
            try
            {
                NCOAAddress a = new NCOAAddress();
                a.Address = "2788 Aston Woods Lane";
                a.City = "Thompsons Station";
                a.State = "TN";
                a.Zip = "37179";

                NCOAAddressResponse rtn = new NCOAAddressResponse();

                NCOAAddress[] lst = new NCOAAddress[] { a };

                rtn = DOTSNCOALive.DOTSNCOA.RunNCOALive(lst, NCOAWebToolsClass.JobId, NCOAWebToolsClass.LicenseKey, false);
                
                return a;
            }
            catch (WebException ex)
            {
                throw new NCOAManagerException(ex);
            }
        }
        /*
        try
        {
            string validateUrl = "?API=Verify&XML=<AddressValidateRequest USERID=\"{0}\"><Address ID=\"{1}\"><Address1>{2}</Address1><Address2>{3}</Address2><City>{4}</City><State>{5}</State><Zip5>{6}</Zip5><Zip4>{7}</Zip4></Address></AddressValidateRequest>";
            string url = GetURL() + validateUrl;
            url = String.Format(url, _userid, address.ID.ToString(), address.Address1, address.Address2, address.City, address.State, address.Zip, address.ZipPlus4);
            string addressxml = web.DownloadString(url);

            if (addressxml.Contains("<Error>"))
            {
                int idx1 = addressxml.IndexOf("<Description>") + 13;
                int idx2 = addressxml.IndexOf("</Description>");
                int l = addressxml.Length;
                string errDesc = addressxml.Substring(idx1, idx2 - idx1);
                //throw new USPSManagerException(errDesc);

                url = String.Format(url, "", "", "", "", "", "", "", "");

                //return a blank address
                return new Address(-1, "BAD ADDRESS", null, null, null, null, null, null, null, null, 0);
            }

            return Address.FromXml(addressxml);

        }
        catch (WebException ex)
        {
            throw new USPSManagerException(ex);
        }
        */
    }
    #endregion
}
