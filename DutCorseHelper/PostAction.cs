using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DutCorseHelper
{
    class PostAction:DutAction
    {
        public string WebPath { get; set; }
        public int ContentLength { get; protected set; }
        public string Content { get; set; }
        public PostAction(string host,string path):
            base()
        {
            HttpMethod = "POST";
            Host = host;
            WebPath = path;
        }
        public override System.Net.WebRequest MakeRequest()
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("/xkAction.do");
           
            return req;
        }
        public override System.Net.WebResponse DoAction(string host, System.Net.CookieContainer cookie)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host + "/xkAction.do");
            req.ServicePoint.Expect100Continue = false;
            req.Timeout = 5 * 1000;
            req.Method = HttpMethod;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            req.KeepAlive = true;
            req.ContentLength = ContentLength;
            
            req.CookieContainer = cookie;
            //req.Headers.Add("Cookie",""); //使用已经保存的cookies 方法二
            return req.GetResponse();
        }
        public override string DoAction(CookieContainer cookies)
        {
            throw new NotImplementedException();
        }
    }
}
