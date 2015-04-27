using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DutCorseHelper
{
    class ResetAction:DutAction
    {

        public override System.Net.WebResponse DoAction(string host, System.Net.CookieContainer cookie)
        {
            throw new NotImplementedException();
        }
        public override string DoAction(System.Net.CookieContainer cookies)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Host+ "/");
            
            return null;
        }
        public override System.Net.WebRequest MakeRequest()
        {
            throw new NotImplementedException();
        }
    }
}
