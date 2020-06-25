//////////////////////////////////////////////////////////////////////////
///This software is provided to you as-is and with not warranties!!!
///Use this software at your own risk.
///This software is Copyright by Scott Smith 2006
///You are free to use this software as you see fit.
//////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace USPSAddressVerifier.Lib
{
    public class NCOAManagerException : ApplicationException
    {
        public NCOAManagerException(string ErrorMessage)
        {
            _Exception = new Exception(ErrorMessage);
        }

        public NCOAManagerException(string ErrorMessage, Exception ex)
        {
            _Exception = new Exception(ErrorMessage, ex);
        }

        public NCOAManagerException(Exception ex)
        {
            _Exception = ex;
        }

        private Exception _Exception;

        public override string Message
        {
            get
            {
                return _Exception.Message;
            }
        }

        public override string Source
        {
            get
            {
                return _Exception.Source;
            }
            set
            {
                _Exception.Source = value;
            }
        }

        public override string StackTrace
        {
            get
            {
                return _Exception.StackTrace;
            }
        }

        public override System.Collections.IDictionary Data
        {
            get
            {
                return _Exception.Data;
            }
        }

        public override string HelpLink
        {
            get
            {
                return _Exception.HelpLink;
            }
            set
            {
                _Exception.HelpLink = value;
            }
        }



    }
}
