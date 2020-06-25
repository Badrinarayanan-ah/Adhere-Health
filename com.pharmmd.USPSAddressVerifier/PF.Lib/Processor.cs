using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Lib
{
    public class Processor
    {
        PFDataActions pfDA = new PFDataActions();

        public List<string> UnvalidatedAddressList()
        {
            List<string> lst = new List<String>();

            DataTable dt=pfDA.SelectSentAddressesUnVerified();

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(dr["originaladdress"].ToString());
            }

            return lst;
        }

        public List<MultipleAddressClass> TrueFullAddressList()
        {
            List<MultipleAddressClass> lst = new List<MultipleAddressClass>();

            DataTable dt = pfDA.SelectSentAddresses();

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new MultipleAddressClass(dr["originaladdress"].ToString(), dr["returnedaddress"].ToString()));
            }


            return lst;
        }

        public List<string> FullAddressList()
        {
            List<string> lst = new List<String>();

            DataTable dt = pfDA.SelectSentAddresses();

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(dr["originaladdress"].ToString());
            }

            return lst;
        }

        public List<string> ValidatedAddressList()
        {
            List<string> lst = new List<String>();

            DataTable dt = pfDA.SelectSentAddressesVerified();

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(dr["originaladdress"].ToString());
            }

            return lst;
        }

        public void SaveNewAddress(string address, string address1, string address2, string city, string state, string zip, string returnedaddress, bool verified, string radr1, string radr2, string rcity, string rstate, string rzip, long accountid)
        {
            pfDA.InsertAddress(address, address1, address2, city, state, zip, returnedaddress, verified, radr1, radr2, rcity, rstate, rzip, accountid);
        }
    }
}
