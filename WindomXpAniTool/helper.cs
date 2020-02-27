using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WindomXpAniTool
{
    class helper
    {
        public static string replaceUnsupportedChar(string str)
        {
            return str.Replace(":", " ").Replace("\\", " ").Replace("/", " ").Replace("\"", " ").Replace("|", " ").Replace("*", " ").Replace("?", " ");
        }

        public static void log(string text)
        {
            StreamWriter debug = File.AppendText("Debug.txt");
            debug.WriteLine(text);
            debug.Close();
        }
    }
}
