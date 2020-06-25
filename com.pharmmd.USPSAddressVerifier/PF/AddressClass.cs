using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF
{
    public class AddressClass
    {
        public int AddressID { get; set; }
        public string OriginalAddress { get; set; }
        public string ReturnedAddress { get; set; }
        public bool Verified { get; set; }
    }
}
