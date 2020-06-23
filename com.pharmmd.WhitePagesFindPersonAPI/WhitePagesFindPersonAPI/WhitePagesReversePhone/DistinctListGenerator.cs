using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesReversePhoneAPI
{
    public class DistinctListGenerator
    {
        public List<UniquePhones> ReturnUniqueList(List<Phones> lstP)
        {
            List<UniquePhones> lstOutput = new List<UniquePhones>();

            foreach (Phones p in lstP)
            {
                //if someone already exists with this name and info do not add them to the unique l ist
                //if(lstOutput.Any(x=>x.Name==p.Name && x.Address==p.Address1 && x.City==p.City && x.State==p.State && (x.PostalCode.Length >5 ? x.PostalCode.Substring(0,5) : x.PostalCode) == p.PostalCode))
                if (lstOutput.Any(x => x.PhoneNumber == p.PhoneNumber && x.InputName==p.Name))
                {
                    var foundItems = lstOutput.Where(x => x.PhoneNumber == p.PhoneNumber && x.InputName==p.Name);
                }
                else
                {
                    lstOutput.Add(new UniquePhones(p.PhoneNumber,p.Name));
                }
            }

            return lstOutput;
        }
    }
}
