using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
namespace DutCorseHelper
{
    class LoginAction:PostAction
    {
        public string UserName { get; set; }
        public string Password { get; set; }
       
        public LoginAction(string host,string username,string pwd)
            : base(host,"/loginAction.do")
        {
            UserName = username;
            Password = pwd;
            //zjh=.....&mm=......
            ContentLength = 8 + UserName.Length + Password.Length;
        }
        public override System.Net.WebRequest MakeRequest()
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("/loginAction.do");
            req.Method = HttpMethod;
            req.ContentType="application/x-www-form-urlencoded";
            req.ContentLength = ContentLength;
            
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("zjh=" + UserName + "&mm=" + Password);
            sw.Close();
            return req;
        }
        public override System.Net.WebResponse DoAction(string host, System.Net.CookieContainer cookie)
        {
            HttpWebRequest req =(HttpWebRequest) System.Net.WebRequest.Create(host+"/loginAction.do");
            req.ServicePoint.Expect100Continue = false;
            req.CookieContainer = cookie;
            req.KeepAlive = true;
            req.ContentLength = ContentLength;
            req.ContentType = "application/x-www-form-urlencoded";
            
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            
            req.Method = HttpMethod;
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write("zjh=" + UserName + "&mm=" + Password);
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
                if (result.IndexOf("提供全新教务管理方案") >= 0)
                {
                    throw new Exception("登录失败!请检查用户名和密码！");
                }
                return "登录成功";
               
            }
            return result;
        }
        
    }
}
