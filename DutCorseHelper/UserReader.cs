using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DutCorseHelper
{
    class UserReader
    {

        private const string userfile = "user.txt";
        public string StuNum { get; protected set; }
        public string Password { get; protected set; }

        public UserReader()
        {
            using (FileStream fs = File.OpenRead(userfile))
            {

                StreamReader sr = new StreamReader(fs);
                StuNum = sr.ReadLine().Trim();
                Password = sr.ReadLine().Trim();
            }
        }
    }
}
