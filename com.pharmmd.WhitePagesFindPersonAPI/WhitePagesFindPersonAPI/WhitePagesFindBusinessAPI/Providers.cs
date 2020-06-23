using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesFindBusinessAPI
{
    public class Providers
    {
        public string Name;
        public string Address1;
        public string Address2;
        public string City;
        public string PostalCode;
        public string State;
        public string CountryCode;
        public bool MetroSearch;
        public bool HistoricalSearch;
        public string DOB;
        public string NPI;


        public Providers()
        {

        }

        public Providers(string _name, string _address1, string _address2, string _city, string _postalcode, string _state, string _countrycode, bool _metrosearch, bool _historicalsearch, string _dob, string _npi)
        {

            Name = _name;
            Address1 = _address1;
            Address2 = _address2;
            City = _city;
            PostalCode = _postalcode;
            State = _state;
            CountryCode = _countrycode;
            MetroSearch = _metrosearch;
            HistoricalSearch = _historicalsearch;

            DOB = _dob;
            NPI = _npi;
        }
        


    }
}
