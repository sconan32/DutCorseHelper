using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace DutCorseHelper
{
    abstract class DutAction
    {
        public string HttpMethod { get; set; }
        public string Host { get; set; }
      
        public abstract System.Net.WebRequest MakeRequest();
        public DutAction()
        {
         
        }
        public abstract System.Net.WebResponse DoAction(string host, CookieContainer cookie);
        public abstract string DoAction(CookieContainer cookies);
    }
}
