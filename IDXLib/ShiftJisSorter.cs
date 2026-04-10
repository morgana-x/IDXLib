using System.Text;

namespace IDXLib
{

    public class ShiftJis
    {
        static Encoding? sjis;

        public static Encoding GetShiftJis()
        {
            if (sjis == null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                sjis = Encoding.GetEncoding(932);
            }

            return sjis;
        }
    }

    public class ShiftJisSorter : StringComparer
    {
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


            var sjis = ShiftJis.GetShiftJis();
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
