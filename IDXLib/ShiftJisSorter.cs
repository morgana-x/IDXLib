using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDXLib
{
    public class ShiftJisSorter : StringComparer
    {
        static Encoding sjis;
        int Compare(byte[] a, byte[] b)
        {
            var length = Math.Min(a.Length, b.Length);
            for (int i = 0; i < length; i++)
            {
                if (a[i] < b[i])
                    return -1;

                if (a[i] > b[i])
                    return 1;
            }

            return 0;
        }
        public override int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (sjis == null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                sjis = Encoding.GetEncoding(932);
            }

            return Compare(sjis.GetBytes(x), sjis.GetBytes(y));
        }

        public override bool Equals(string? x, string? y)
        {
            return x == y;
        }

        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
