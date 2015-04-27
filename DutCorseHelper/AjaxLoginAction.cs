using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DutCorseHelper
{
    class AjaxLoginAction:PostAction
    {
        /*
         * POST /dwr/call/plaincall/ajaxtool.getMessage.dwr HTTP/1.1
Accept: * / *
Accept-Language: zh-cn
Referer: http://202.118.65.20:8084/menu/s_main.jsp
Content-Type: text/plain
Accept-Encoding: gzip, deflate
User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Zune 4.7)
Host: 202.118.65.20:8084
Content-Length: 235
Connection: Keep-Alive
Cache-Control: no-cache
Cookie: JSESSIONID=dfeZ0q26FSV9t9piKxtEt
Lv}`_ER@^YvAG
R=GP@)$rcallCount=1
page=/menu/s_main.jsp
httpSessionId=dfeZ0q26FSV9t9piKxtEt
scriptSessionId=BE349D683FA75D2994DDC7EFD3F1070C639
c0-scriptName=ajaxtool
c0-methodName=getMessage
c0-id=0
c0-param0=string:201192255
c0-param1=string:01
batchId=0
    */
        public AjaxLoginAction(string host)
            : base(host, "/dwr/call/plaincall/ajaxtool.getMessage.dwr")
        {
            ContentLength = 235;
        }
        public override System.Net.WebResponse DoAction(string host, System.Net.CookieContainer cookie)
        {
            string sessstr = "";
            foreach (Cookie cook in cookie.GetCookies(new Uri(host + "/")))
            {
                if (cook.Name == "JSESSIONID")
                {
                    sessstr = cook.Value;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host + WebPath);
            req.ServicePoint.Expect100Continue = false;
            req.Timeout = 5 * 1000;
            req.Method = HttpMethod;
            req.ContentType = "text/plain";
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            req.KeepAlive = true;
            req.ContentLength = ContentLength;
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("callCount=1\x0A");
            sw.Write("page=/menu/s_main.jsp\x0A");
            sw.Write("httpSessionId="+sessstr+"\x0A");
            sw.Write("scriptSessionId=BE349D683FA75D2994DDC7EFD3F1070C639\x0A");
            sw.Write("c0-scriptName=ajaxtool\x0A");
            sw.Write("c0-methodName=getMessage\x0A");
            sw.Write("c0-id=0\x0A");
            sw.Write("c0-param0=string:201192255\x0A");
            sw.Write("c0-param1=string:01\x0A");
            sw.Write("batchId=0\x0A");
            sw.Close();
            req.CookieContainer = cookie;
            //req.Headers.Add("Cookie",""); //使用已经保存的cookies 方法二
            return req.GetResponse();
        }
        public override string DoAction(CookieContainer cookies)
        {
            HttpWebResponse res =(HttpWebResponse) DoAction(Host, cookies);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                return  new StreamReader(res.GetResponseStream(), Encoding.Default).ReadToEnd();
            }
            return null;
        }
    }
}
