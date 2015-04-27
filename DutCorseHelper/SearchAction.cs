using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DutCorseHelper
{
    class SearchAction:PostAction
    {
        public string CourseSerial { get; set; }
        public string SequenceNo { get; set; }
        public SearchAction(string host,string corseNo,string seqNo)
            : base(host, "/xkAction.do")
        {
            CourseSerial = corseNo;
            SequenceNo = seqNo;
            ContentLength = 85 + CourseSerial.Length + SequenceNo.Length;
        }
        public override WebResponse DoAction(string host, CookieContainer cookie)
        {
            //kch=2703131&cxkxh=&kcm=&skjs=&kkxsjc=&skxq=&skjc=&pageNumber=-2&preActionType=2&actionType=5
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host + "/xkAction.do");
            req.CookieContainer = cookie;
            req.KeepAlive = true;
            req.ServicePoint.Expect100Continue = false;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";

            req.ContentLength = ContentLength;
            req.Method = HttpMethod;
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("kch="+CourseSerial+"&cxkxh="+SequenceNo+"&kcm=&skjs=&kkxsjc=&skxq=&skjc=&pageNumber=-2&preActionType=2&actionType=5");
            sw.Close();
            return req.GetResponse();
        }
        public override string DoAction(System.Net.CookieContainer cookies)
        {
            string result = null;
            HttpWebResponse res = (HttpWebResponse)DoAction(Host, cookies);

            if (res.StatusCode == HttpStatusCode.OK)
            {
                Stream ss = res.GetResponseStream();
                result = new StreamReader(ss, Encoding.Default).ReadToEnd();
            }
            res.Close();
            return result;
        }
    }
}
