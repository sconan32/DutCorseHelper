using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DutCorseHelper
{
    class WebController
    {
        public string Host { get; set; }
        DutAction act;
        CookieContainer cookies = new CookieContainer();
        public event EventHandler StatusChanged;
        public event EventHandler MessageRecieved;
        public string JSessionCookie { get; protected set; }
        public string result { get; set; }


        public string UserName { get; set; }
        public string Password { get; set; }
        public List<CorseItem> Corses { get; set; }
        public List<string> Courses { get; set; }
        public List<string> SeqNos { get; set; }

        public bool BeginRequest()
        {
            HttpWebResponse res = null;
            try
            {
                OnStatusChange(1);
                OnMessageRecieved("正在连接选课服务器...");
                act = new PreLoginAction(Host);
                 res = (HttpWebResponse)act.DoAction(Host, cookies);

                if (res.Cookies.Count <= 0)
                {
                    res.Close();
                    act = new PreLoginAction(Host);
                    res = (HttpWebResponse)act.DoAction(Host, cookies);

                }
                if (res.Cookies.Count <= 0)
                    throw new Exception("无法获取连接信息,请稍后再试");
                foreach (Cookie cook in res.Cookies)
                {
                    cookies.Add(cook);
                    //if (cook.Name == "JSESSIONID")
                    //    JSessionCookie = cook.Value;
                }
                res.Close();
                OnStatusChange(2);
                OnMessageRecieved("正在登录(" + UserName + ")");

                act = new LoginAction(Host,UserName, Password);
                result = act.DoAction(cookies);
                OnMessageRecieved(result);
                OnStatusChange(3);
                OnMessageRecieved("正在获得课程信息...");
                act = new GetAction("/menu/mainFrame.jsp");
                res = (HttpWebResponse)act.DoAction(Host, cookies);
                res.Close();
                //if (res.StatusCode == HttpStatusCode.OK)
                //{
                //    //result = new StreamReader(res.GetResponseStream(), Encoding.Default).ReadToEnd();
                //}
                OnStatusChange(4);
                act = new GetAction("/xkAction.do?actionType=17");
                res = (HttpWebResponse)act.DoAction(Host, cookies);
                res.Close();
                
                //act = new GetAction("/xkAction.do?actionType=2&pageNumber=-1&oper1=ori");
                //res = (HttpWebResponse)act.DoAction(Host, cookies);

               
                res.Close();
                for(int i=0;i<Corses.Count;i++)
                {
                    string c1 = Corses[i].CorseId;
                    string c2 = Corses[i].SeqNo;
                    OnMessageRecieved("正在选课(" + c1 + "-" + c2 + ")");
                    act = new SearchAction(Host, c1, c2);
                    result = act.DoAction(cookies);
                    CorseItem ci=CorseParser.Parse(result);
                    if (ci == null)
                    {
                        throw new NullReferenceException("无法得到课程信息，请稍后再试");
                    }
                    if (ci.CorseId == Corses[i].CorseId &&
                        ci.SeqNo == Corses[i].SeqNo)
                    {
                        Corses[i] = ci;
                        OnMessageRecieved("本课的基本信息："+Environment.NewLine+ ci.ToString());
                        if (ci.PeopleCnt > 0)
                        {
                            act = new SelectAction(Host, c1, c2);
                            result = act.DoAction(cookies);
                           
                        }
                        else
                        {
                            result="选课失败[课程(" + c1 + "-" + c2 + ")的课余量为0]!";
                        }
                        OnMessageRecieved(result);
                    }
                   
                  
                }

                OnMessageRecieved("正在注销登录...");
                act = new GetAction("/logout.do");
                res = (HttpWebResponse)act.DoAction(Host, cookies);
                res.Close();
                return true;
            }
            catch (Exception ex)
            {
                OnStatusChange(-3);
                OnMessageRecieved(ex.Message);
                return false;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
                OnMessageRecieved("完成");
            }
           // return false;
        }
        protected void OnStatusChange(int status)
        {
            if (StatusChanged != null)
            {
                StatusChanged(status, EventArgs.Empty);
            }
        }
        protected void OnMessageRecieved(string message)
        {
            if (MessageRecieved != null)
            {
                MessageRecieved(message, EventArgs.Empty);
            }
        }
    }
}
