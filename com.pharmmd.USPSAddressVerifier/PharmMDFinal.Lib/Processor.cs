using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PharmMDFinal.Lib
{
    public class Processor
    {
        DataActions DA = new DataActions();

        public List<long> ValidatedPatientList()
        {
            List<long> lst = new List<long>();

            DataTable dt = DA.SelectUsedPatientRecord();

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(Convert.ToInt32(dr["patientid"]));
            }

            return lst;
        }
        
    }
}
