using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace TheWatcher
{
    public class Emailing
    {
        public void SendEmail(string fromemailaddress,string toemailaddress, string body, string subject)
        {
            SQLActions sa = new SQLActions();
            //sa.SendEmail(fromemailaddress, toemailaddress, body, subject);
            sa.SendEmail(fromemailaddress, toemailaddress, subject, body);
        }
    }
}
