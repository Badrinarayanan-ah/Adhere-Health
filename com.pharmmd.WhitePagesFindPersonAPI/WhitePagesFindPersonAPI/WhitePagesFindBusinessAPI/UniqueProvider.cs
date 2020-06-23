using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesFindBusinessAPI
{
    public class UniqueProvider
    {
        public string Name;
        public string Address;
        public string Address2;
        public string City;
        public string State;
        public string PostalCode;

        public List<string> PhoneNumbers = new List<string>();
        public List<string> LineTypes = new List<string>();

        public string DOB;
        public string JSON;
        public string Request;

        public string NPI;
        
        public UniqueProvider()
        {

        }

        public UniqueProvider(string _Name, string _Address, string _Address2, string _City, string _State, string _PostalCode, string _DOB, string _NPI)
        {
            Name = _Name;
            Address = _Address;
            Address2 = _Address2;
            City = _City;
            State = _State;
            PostalCode = _PostalCode;

            DOB = _DOB;
            NPI = _NPI;
        }

        public bool Equals(UniqueProvider other)
        {
            return this.Name == other.Name && this.Address == other.Address;
        }

    }
}
