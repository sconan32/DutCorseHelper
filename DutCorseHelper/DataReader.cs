using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DutCorseHelper
{
    class DataReader
    {
        private const string filename = "data.txt";

        public List<CorseItem> Corses { get; protected set; }
        public List<string> Courses { get; protected set; }
        public List<string> SeqNos { get; protected set; }

        
        public DataReader()
        {
            Corses = new List<CorseItem>();
            FileStream fs = File.OpenRead(filename);
           
            StreamReader sr = new StreamReader(fs);
           // StuNum = sr.ReadLine().Trim();
           // Password = sr.ReadLine().Trim();
            CorseItem c = new CorseItem();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Length > 1)
                {
                    string[] arr = line.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    c.CorseId = arr[0];
                    c.SeqNo = arr[1];
                    Corses.Add(c);
                }
            }
        }
    }
}
