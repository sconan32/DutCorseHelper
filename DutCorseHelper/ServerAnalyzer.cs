using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace DutCorseHelper
{
    class ServerAnalyzer
    {
        string uri;
        public List<string> ServerList { get; protected set; }
        public List<string> GetServerList()
        {
            if (ServerList != null && ServerList.Count > 0)
            {
                return ServerList;
            }
            else { ServerList = new List<string>(); }

            List<string> list = new List<string>();
            string result = null;
            try
            {

                DutAction act = new GetAction("http://teach.dlut.edu.cn", "/");
                result = act.DoAction(null);
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrEmpty(result))
                return null;
            Regex regex = new Regex("href=\"(http://.+)/.+\">本科生选课");
            MatchCollection matches = regex.Matches(result);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    list.Add(m.Groups[1].ToString());
                }
                ServerList = list;


            }
            ServerList.Insert(0, "http://202.118.65.20:8081");
            ServerList.Insert(0, "http://202.118.65.21:8089");
            ServerList.Insert(0, "http://202.118.65.20:8088");
            ServerList.Insert(0, "http://202.118.65.21:8088");
            return ServerList;

        }
        public bool TestServer(string server)
        {
            DutAction act;
            HttpWebResponse res = null;
            try
            {
                act = new GetAction(server, "/");
                res = (HttpWebResponse)act.DoAction(server, null);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }



            }
            catch
            {
                return false;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return false;
        }
        public string GetUsableServer()
        {
            DutAction act;
            HttpWebResponse res = null;

            foreach (string s in ServerList)
            {
                try
                {
                    act = new GetAction(s, "/");
                    res = (HttpWebResponse)act.DoAction(s, null);
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        return s;
                    }



                }
                catch { }
                finally
                {
                    if (res != null)
                    {
                        res.Close();
                    }
                }
            }
            return null;
        }
    }
}
