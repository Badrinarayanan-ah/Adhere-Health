using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.Lib
{
    public class MultipleAddressClass
    {
        public string OriginalAddress;
        public string ReturnedAddress;

        public MultipleAddressClass()
        {
        }

        public MultipleAddressClass(string _originaladdress, string _returnedaddress)
        {
            OriginalAddress=_originaladdress;
            ReturnedAddress = _returnedaddress;
        }
    }
}
