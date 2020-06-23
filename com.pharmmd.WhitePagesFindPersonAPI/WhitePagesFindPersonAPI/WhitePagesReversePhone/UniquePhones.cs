using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesReversePhoneAPI
{
    public class UniquePhones
    {
        public string PhoneNumber;
        public string InputName;
        public string LineType;
        public bool? IsCommercial=null;
        public bool? IsValid=null;

        public int? AltPhonesCount = null;
        public int? CurrentAddressesCount = null;

        public string JSON;
        public string Request;

        public List<string> Names = new List<string>();


        public UniquePhones()
        {

        }

        public UniquePhones(string _PhoneNumber, string _Name)
        {
            PhoneNumber = _PhoneNumber;
            InputName = _Name;

        }

        public bool Equals(UniquePhones other)
        {
            return this.PhoneNumber == other.PhoneNumber && this.InputName == InputName;
        }
    }
}
