using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesReversePhoneAPI
{
    public class Phones
    {
        public string PhoneNumber;
        public string Name;

        public Phones()
        {

        }

        public Phones(string _phonenumber, string _name)
        {
            PhoneNumber = _phonenumber;
            Name = _name;
        }
    }
}
