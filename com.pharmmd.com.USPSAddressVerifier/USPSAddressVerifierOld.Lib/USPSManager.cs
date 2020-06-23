//////////////////////////////////////////////////////////////////////////
///This software is provided to you as-is and with not warranties!!!
///Use this software at your own risk.
///This software is Copyright by Scott Smith 2006
///You are free to use this software as you see fit.
//////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace USPSAddressVerifier.Lib
{
    public class USPSManager
    {
        #region Private Members
        private const string ProductionUrl = "http://production.shippingapis.com/ShippingAPI.dll";
        private const string TestingUrl = "http://testing.shippingapis.com/ShippingAPITest.dll";
        private WebClient web;
        private string _userid;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new USPS Manager instance
        /// </summary>
        /// <param name="USPSWebtoolUserID">The UserID required by the USPS Web Tools</param>
        public USPSManager(string USPSWebtoolUserID)
        {
            web = new WebClient();
            _userid = USPSWebtoolUserID;
            _TestMode = false;
            
        }
        /// <summary>
        /// Creates a new USPS Manager instance
        /// </summary>
        /// <param name="USPSWebtoolUserID">The UserID required by the USPS Web Tools</param>
        /// <param name="testmode">If True, then the USPS Test URL will be used.</param>
        public USPSManager(string USPSWebtoolUserID, bool testmode)
        {
            _TestMode = testmode;
            web = new WebClient();
            _userid = USPSWebtoolUserID;
        }

        #endregion

        #region Properties
        private bool _TestMode;
        /// <summary>
        /// Determines if the Calls to the USPS server is made to the Test or Production server.
        /// </summary>
        public bool TestMode
        {
            get { return _TestMode; }
            set { _TestMode = value; }
        }

        #endregion

        #region Address Methods
        /// <summary>
        /// Validate a single address
        /// </summary>
        /// <param name="address">Address object to be validated</param>
        /// <returns>Validated Address</returns>
        public Address ValidateAddress(Address address)
        {
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

                    url = String.Format(url,"","","","","","","","");

                    //return a blank address
                    return new Address(-1, "BAD ADDRESS", null, null, null, null, null, null, null, null,0);
                }

                return Address.FromXml(addressxml);
                
            }
            catch(WebException ex)
            {
                throw new USPSManagerException(ex);
            }
        }
        /// <summary>
        /// Get the zip code by providing an Address object with a city and state
        /// </summary>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        public Address GetZipcode(string city, string state)
        {
            Address a = new Address();
            a.City = city;
            a.State = state;
            return GetZipcode(a);
        }

        /// <summary>
        /// Get the zip code by providing an Address object with a city and state
        /// </summary>
        /// <param name="address">Address Object</param>
        /// <returns>Address Object</returns>
        public Address GetZipcode(Address address)
        {
            try
            {
                //The address must contain a city and state
                if (address.City == null || address.City.Length < 1 || address.State == null || address.State.Length < 1)
                    throw new USPSManagerException("You must supply a city and state for a zipcode lookup request.");

                string zipcodeurl = "?API=ZipCodeLookup&XML=<ZipCodeLookupRequest USERID=\"{0}\"><Address ID=\"{1}\"><Address1>{2}</Address1><Address2>{3}</Address2><City>{4}</City><State>{5}</State></Address></ZipCodeLookupRequest>";
                string url = GetURL() + zipcodeurl;
                url = String.Format(url, _userid, address.ID.ToString(), address.Address1, address.Address2, address.City, address.State, address.Zip, address.ZipPlus4);
                string addressxml = web.DownloadString(url);
                if (addressxml.Contains("<Error>"))
                {
                    int idx1 = addressxml.IndexOf("<Description>") + 13;
                    int idx2 = addressxml.IndexOf("</Description>");
                    int l = addressxml.Length;
                    string errDesc = addressxml.Substring(idx1, idx2 - idx1);
                    throw new USPSManagerException(errDesc);
                }

                return Address.FromXml(addressxml);
            }
            catch (WebException ex)
            {
                throw new USPSManagerException(ex);
            }
        }

        /// <summary>
        /// Get the city and state by proving the zip code.
        /// </summary>
        /// <param name="zipcode">Zipcode</param>
        public Address GetCityState(string zipcode)
        {
            Address a = new Address();
            a.Zip = zipcode;
            return GetCityState(a);
        }

        /// <summary>
        /// Get the city and state by proving the zip code.
        /// </summary>
        /// <param name="address">Address object</param>
        /// <returns>Address Object</returns>
        public Address GetCityState(Address address)
        {
            try
            {
                //The address must contain a city and state
                if (address.Zip == null || address.Zip.Length < 1)
                    throw new USPSManagerException("You must supply a zipcode for a city/state lookup request.");
                
                string citystateurl = "?API=CityStateLookup&XML=<CityStateLookupRequest USERID=\"{0}\"><ZipCode ID= \"{1}\"><Zip5>{2}</Zip5></ZipCode></CityStateLookupRequest>";
                string url = GetURL() + citystateurl;
                url = String.Format(url, _userid, address.ID.ToString(), address.Zip);
                string addressxml = web.DownloadString(url);
                if (addressxml.Contains("<Error>"))
                {
                    int idx1 = addressxml.IndexOf("<Description>") + 13;
                    int idx2 = addressxml.IndexOf("</Description>");
                    int l = addressxml.Length;
                    string errDesc = addressxml.Substring(idx1, idx2 - idx1);
                    throw new USPSManagerException(errDesc);
                }

                return Address.FromXml(addressxml);
            }
            catch (WebException ex)
            {
                throw new USPSManagerException(ex);
            }
        }

        #endregion


        #region TextConversions
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        #endregion

        #region Private methods
        private string GetURL()
        {
            string url = ProductionUrl;
            if (TestMode)
                url = TestingUrl;
            return url;
        }
        #endregion
    }
}
