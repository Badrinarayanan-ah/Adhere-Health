using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHocCampaignRunner
{
    public class CampaignInfo
    {
        public string Name;
        public string DialerTable;
        public string DialerTableHx;
        public string ExcelPathTemplate;

        public CampaignInfo(string _name, string _dialertable, string _dialertablehx, string _excelpathtemplate)
        {
            Name = _name;
            DialerTable = _dialertable;
            DialerTableHx = _dialertablehx;
            ExcelPathTemplate = _excelpathtemplate;
        }
    }
}


