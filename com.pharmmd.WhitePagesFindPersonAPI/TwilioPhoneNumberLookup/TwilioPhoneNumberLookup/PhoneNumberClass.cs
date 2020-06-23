using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioPhoneNumberLookup
{
    public class PhoneNumberClass
    {
        public string Number { get; set; }
        public bool IsValid { get; set; }
        public bool? IsMobile { get; set; }
        public string Name { get; set; }

        public PhoneNumberClass()
        { }

        public PhoneNumberClass(string _number, bool _isvalid, bool? _ismobile)
        {
            Number = _number;
            IsValid = _isvalid;
            IsMobile = _ismobile;
        }

        public PhoneNumberClass(string _number, bool _isvalid, bool? _ismobile, string _name)
        {
            Number = _number;
            IsValid = _isvalid;
            IsMobile = _ismobile;
            Name = _name;
        }
    }
}
