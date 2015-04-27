using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DutCorseHelper
{
    public class CorseItem
    {
        public string CorseId { get; set; }
        public string SeqNo { get; set; }
        public string CorseName { get; set; }
        public string Teacher { get; set; }
        public string Credits { get; set; }
        public string TestType { get; set; }
        public string Depart { get; set; }
        public int PeopleCnt { get; set; }
        public override string ToString()
        {
            StringBuilder sb=new StringBuilder ();
            sb.Append("(");
            sb.Append(CorseId);
            sb.Append("-");
            sb.Append(SeqNo);
            sb.Append(")");
            sb.Append(CorseName);
            sb.Append("[");
            sb.Append(Credits);
            sb.Append("分]");
            sb.Append(Environment.NewLine);
            sb.Append("\t");
            sb.Append(Depart);
            sb.Append("-");
            sb.Append(Teacher);
            sb.Append("-");
            sb.Append(TestType);
            return sb.ToString();
        }
    }
}
