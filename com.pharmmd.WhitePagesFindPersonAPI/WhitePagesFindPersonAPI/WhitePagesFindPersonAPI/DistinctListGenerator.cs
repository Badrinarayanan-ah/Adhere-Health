using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitePagesFindPersonAPI
{
    public class DistinctListGenerator
    {
        public List<UniquePerson> ReturnUniqueList(List<Person> lstP)
        {
            List<UniquePerson> lstOutput = new List<UniquePerson>();

            foreach(Person p in lstP)
            {
                //if someone already exists with this name and info do not add them to the unique l ist
                //if(lstOutput.Any(x=>x.Name==p.Name && x.Address==p.Address1 && x.City==p.City && x.State==p.State && (x.PostalCode.Length >5 ? x.PostalCode.Substring(0,5) : x.PostalCode) == p.PostalCode))
                if (lstOutput.Any(x => x.LastName == p.LastName && x.Address == p.Address1 && x.City == p.City && x.State == p.State && (x.PostalCode.Length > 5 ? x.PostalCode.Substring(0, 5) : x.PostalCode) == p.PostalCode))
                {
                    var foundItems = lstOutput.Where(x => x.LastName == p.LastName && x.Address == p.Address1 && x.City == p.City && x.State == p.State && (x.PostalCode.Length > 5 ? x.PostalCode.Substring(0, 5) : x.PostalCode) == p.PostalCode);
                }
                else
                {
                    lstOutput.Add(new UniquePerson(p.LastName,p.Name, p.Address1, p.Address2, p.City, p.State, (p.PostalCode.Length > 5 ? p.PostalCode.Substring(0, 5) : p.PostalCode), p.DOB, p.PMDPatientID, p.PMDClientID, p.ContractNumber));
                }
            }

            return lstOutput;
        }
    }
}
