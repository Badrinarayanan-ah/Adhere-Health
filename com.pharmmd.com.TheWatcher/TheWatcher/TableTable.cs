using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheWatcher
{
    public class TableTable
    {
        public string DaysToSkip { get; set; }

        public string SQL { get; set; }

        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string SchemaName { get; set; }

        public int? DateInterval { get; set; }

        public int RowID { get; set; }
        public bool Active { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? DaysSinceLastVerified { get; set; }

        public DateTime? LastChecked { get; set; }

        //mode 1 is the default and uses a date field
        //mode 2 uses the existence of any data in the table
        public int ModeID { get; set; }
    }
}
