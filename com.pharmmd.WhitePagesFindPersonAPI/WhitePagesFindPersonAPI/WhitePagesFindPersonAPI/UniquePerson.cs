using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesFindPersonAPI
{
    public class UniquePerson
    {
        public string LastName;
        public string Name;
        public string Address;
        public string Address2;
        public string City;
        public string State;
        public string PostalCode;
        public List<string> PMDPatientIDs;
        public string PMDPatientID;
        public string PMDClientID;
        public string ContractNumber = "";

        public string PhoneNumber = "";
        public string PhoneNumber2 = "";
        public string PhoneNumber3 = "";
        public string LineType = "";
        public string LineType2 = "";
        public string LineType3 = "";
        public string DOB;
        public string JSON;
        public string Request;

        public UniquePerson()
        {

        }

        public UniquePerson(string _LastName, string _Name, string _Address, string _Address2, string _City, string _State, string _PostalCode, string _DOB, string _PMDPatientID, string _PMDClientID, string _ContractNumber)
        {
            LastName = _LastName;
            Name = _Name;
            Address = _Address;
            Address2 = _Address2;
            City = _City;
            State = _State;
            PostalCode = _PostalCode;
            DOB = _DOB;
            PMDPatientID = _PMDPatientID;
            PMDClientID = _PMDClientID;
            ContractNumber = _ContractNumber;
        }

        public bool Equals(UniquePerson other)
        {
            return this.Name == other.Name && this.PhoneNumber == other.PhoneNumber;
        }

    }
}
