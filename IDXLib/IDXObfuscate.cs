
namespace IDXLib
{
    public class IDXObfuscate
    {
        public enum ObfuscationType : long
        {
            None = 0,
            XOR = 2
        }
        
        private const byte _key = 0xAA;

        public static void Obfuscate(ref byte[] d, ObfuscationType type)
        {
            switch (type)
            {
                case ObfuscationType.XOR:
                    for (int i = 0; i < d.Length; i++)
                        d[i] = (byte)(d[i] ^ _key);

                    break;
            }
        }

        public static ObfuscationType GetObfuscation(string f)
        {
            if (f.EndsWith(".x") || f.EndsWith(".txt"))
                return ObfuscationType.XOR;
            
            return ObfuscationType.None;
        }
    }
}
