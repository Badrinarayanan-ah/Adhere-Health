using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Lib
{
    public class AddressClass
    {
        public int AddressID { get; set; }
        public string OriginalAddress { get; set; }
        public string ReturnedAddress { get; set; }
        public bool Verified { get; set; }

        public AddressClass(int _addressid, string _originaladdress, string _returnedaddress, bool _verified)
        {
            AddressID=_addressid;
            OriginalAddress=_originaladdress;
            ReturnedAddress=_returnedaddress;
            Verified=_verified;
        }
    }
}
