using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;
namespace DutCorseHelper
{
    class SelectAction : PostAction
    {
        public string CourseSerial { get; set; }
        public string SequenceNo { get; set; }
        public SelectAction(string corseNo, string seqNo)
            : base("", "/xkAction.do")
        {
            CourseSerial = corseNo;
            SequenceNo = seqNo;
            //kcId=1520090_500&preActionType=2&actionType=9
            ContentLength = 35 + CourseSerial.Length + SequenceNo.Length;
        }
        public SelectAction(string host, string corseNo, string seqNo)
            : base(host, "/xkAction.do")
        {
            CourseSerial = corseNo;
            SequenceNo = seqNo;
            //kcId=1520090_500&preActionType=2&actionType=9
            ContentLength = 35 + CourseSerial.Length + SequenceNo.Length;
        }
        public override System.Net.WebRequest MakeRequest()
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("/xkAction.do");
            req.Method = HttpMethod;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = ContentLength;
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("kcId=" + CourseSerial + "_" + SequenceNo + "&preActionType=5&actionType=9");
            sw.Close();
            return req;
        }
        public override System.Net.WebResponse DoAction(string host, System.Net.CookieContainer cookie)
        {
            HttpWebRequest req =(HttpWebRequest) HttpWebRequest.Create(host + "/xkAction.do");
            // 设置一些公用的请求头  
            req.Timeout = 5 * 1000;
            req.KeepAlive = true;
            NameValueCollection collection = new NameValueCollection();
            collection.Add("Accept-Language", "zh-CN,zh;q=0.8");
            collection.Add("Accept-Encoding", "gzip,deflate,sdch");
            collection.Add("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            collection.Add("Cache-Control", "max-age=0");
            

            req.Headers.Add(collection);
            req.CookieContainer = cookie;
            req.Referer = host + "/xkAction.do" ;
            req.ServicePoint.Expect100Continue = false;

            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";

            req.ContentLength = ContentLength;
            req.Method = HttpMethod;
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("kcId=" + CourseSerial + "_" + SequenceNo + "&preActionType=5&actionType=9");
            sw.Close();
            return req.GetResponse();
        }
        public override string DoAction(CookieContainer cookies)
        {
            string result = null;
            HttpWebResponse res = (HttpWebResponse)DoAction(Host, cookies);

            if (res.StatusCode == HttpStatusCode.OK)
            {
                Stream ss = res.GetResponseStream();

                result = new StreamReader(ss, Encoding.Default).ReadToEnd();
                res.Close();
                int loc = result.IndexOf("<strong>") + 16 + 22;
                int loc2 = result.IndexOf("</strong>");
                if (loc > 0)
                {
                    switch (loc2 - loc)
                    {
                        case 9:
                            result = "选课成功!请复核课表！";
                            break;
                        case 22:
                            result = "选课失败[不能跨校区选课]!";
                            break;
                        case 27:
                            result = "选课失败[非选课阶段不能选课]!";
                            break;
                        case 32:
                            result = "选课失败[已经选择了本课程,不能重复选择]！";
                            break;
                        //case 51:
                        //    result = "你不符合修习本课程的条件";
                        //    break;
                        default:
                            result = "选课失败！";
                            break;
                    }
                }
            }
            return result;
        }
    }
}
