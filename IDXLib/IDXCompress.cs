using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDXLib
{
    public class IDXCompress
    {
        public static Stream Decompress(Stream inStream)
        {
            Stream outStream = new MemoryStream();
            return outStream;
        }

        public static bool ShouldBeCompressed(string f)
        {
            return f.EndsWith(".x") || f.EndsWith(".txt");
        }
    }
}
