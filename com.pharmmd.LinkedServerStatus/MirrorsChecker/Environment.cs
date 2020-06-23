using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MirrorsChecker
{
    public class Environment
    {
        public int mirror_id;
        public string dst_db_name_final;
        public string medproconnection;
        public string linkservername;

        public Environment()
        {

        }

        public Environment(int _mirror_id, string _dst_db_name_final, string _medproconnection, string _linkedservername)
        {
            mirror_id = _mirror_id;
            dst_db_name_final = _dst_db_name_final;
            medproconnection = _medproconnection;
            linkservername = _linkedservername;
        }
    }
}
