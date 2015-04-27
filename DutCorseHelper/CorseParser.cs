using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace DutCorseHelper
{
    class CorseParser
    {
        public static CorseItem Parse(string webpart)
        {
            if (string.IsNullOrEmpty(webpart))
            {
                return null;
            }
            CorseItem c = new CorseItem();
            Regex regex = new Regex(@"</thead>[\s\S]*?<tr .+>[\s\S]*?((<td .+>[\s\S]*?</td>[\s\S]*?)+)</tr>", RegexOptions.Multiline);
            Regex r2 = new Regex(@"<td [^/]+>([\s\S]*?)</td>");
            MatchCollection matches = regex.Matches(webpart);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    string subpart = m.Groups[1].ToString();
                    MatchCollection mc2 = r2.Matches(subpart);
                    int cnt = mc2.Count;
                    if (cnt > 10)
                    {
                        c.Depart = mc2[1].Groups[1].ToString().Trim();
                        c.CorseId = mc2[2].Groups[1].ToString().Trim();
                        c.CorseName = mc2[3].Groups[1].ToString().Trim();
                        c.SeqNo = mc2[4].Groups[1].ToString().Trim();
                        c.Credits = mc2[5].Groups[1].ToString().Trim();
                        c.TestType = mc2[6].Groups[1].ToString().Trim();
                        c.Teacher = mc2[7].Groups[1].ToString().Trim();
                        string ts = mc2[8].Groups[1].ToString().Trim();
                        int d = 0;
                        int.TryParse(ts, out d);
                        c.PeopleCnt = d;
                        return c;
                    }

                }
            }

            return null;

        }
    }
}
