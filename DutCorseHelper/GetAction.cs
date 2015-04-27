using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
namespace DutCorseHelper
{
    class GetAction : DutAction
    {
        public string WebPath { get; set; }
        public GetAction(string path)
            : base()
        {
            HttpMethod = "GET";
            WebPath = path;
        }
        public GetAction(string host, string path)
            : base()
        {

            HttpMethod = "GET";
            WebPath = path;
            Host = host;
        }
        public override System.Net.WebRequest MakeRequest()
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("/");
            return req;
        }
        public override System.Net.WebResponse DoAction(string host, CookieContainer cookie)
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host + WebPath);

            req.ServicePoint.Expect100Continue = false;
            req.Timeout = 10 * 1000;
            req.Method = "GET";
            req.ContentLength = 0;
            req.CookieContainer = cookie;
            //req.KeepAlive = true;
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            //req.KeepAlive = true;

            return req.GetResponse();


        }
        public override string DoAction(CookieContainer cookies)
        {
            HttpWebResponse res = null;
            try
            {
                res = (HttpWebResponse)DoAction(Host, cookies);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    return new StreamReader(res.GetResponseStream(), Encoding.Default).ReadToEnd();
                }

                res.Close();
            }
            catch
            {
            }
            finally
            {
                if (res != null)
                    res.Close();
            }
            return null;
        }
    }
}
