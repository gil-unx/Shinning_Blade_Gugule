using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sb
{
    public class Text
    {
        private static string WR(string str, int size)
        {

            str = str.TrimStart();
            if (str.Length <= size) return str;
            var nextspace = str.LastIndexOf(' ', size);
            if (-1 == nextspace) nextspace = Math.Min(str.Length, size);
            return str.Substring(0, nextspace) + ((nextspace >= str.Length) ?
            "" : "\n" + WR(str.Substring(nextspace), size));
        }

        public static string Wrap(string s)
        {
            if (s == null) return "";
            if (s.Length < 8)
            {
                return s;
            }
            bool CR = false;
            s = s.Replace("\\r", "\r").Replace("\\n", "\n");
            if (s.Contains("\r"))
            {
                CR = true;
                s = s.Replace("\r\n", " ");

            }
            s = s.Replace("\n", " ");

            int size = 80;


            string n = WR(s, size);
            
            if (CR)
            {
                n = n.Replace("\n", "\r\n");
                return n;
            }
            return n;
        }
    }
}
