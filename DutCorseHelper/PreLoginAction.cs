using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DutCorseHelper
{
    class PreLoginAction:PostAction
    {
        public PreLoginAction(string host)
            : base(host, "/logout.do")
        {
            ContentLength = 23;
        }
        
        public override System.Net.WebResponse DoAction(string host, System.Net.CookieContainer cookie)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host + "/logout.do");
            req.Timeout = 5 * 1000;
            req.Method = HttpMethod;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            req.KeepAlive = true;
            req.ContentLength = ContentLength;
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("loginType=platformLogin");
            sw.Close();
            req.CookieContainer = cookie;
            //req.Headers.Add("Cookie",""); //使用已经保存的cookies 方法二
            try
            {
                return req.GetResponse();
            }
            catch
            {
                throw;
            }
            finally
            {
                req.Abort();
            }
        }
    }
}
