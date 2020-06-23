using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioPhoneNumberLookup
{
    public class DistinctListGenerator
    {
        public List<UniquePhoneNumberClass> ReturnUniqueOutputList(List<PhoneNumberClass> lst)
        {
            List<UniquePhoneNumberClass> lstOutput = new List<UniquePhoneNumberClass>();

            foreach(PhoneNumberClass p in lst)
            {
                if (lstOutput.Any(x => x.PhoneNumber == p.Number))
                {

                }
                else
                {
                    lstOutput.Add(new UniquePhoneNumberClass(p.Number));
                }
            }
            

            return lstOutput;
        }
    }
}
