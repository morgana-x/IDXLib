using System.Text;

namespace IDXLib
{
    public class IDXFile
    {
        public uint Location;

        public uint Size;

        public string Name;

        public IDXFile(BinaryReader br)
        {
            Location = br.ReadUInt32();
            Size = br.ReadUInt32();

            br.BaseStream.Position += 8;

            Name = Encoding.UTF8.GetString(br.ReadBytes(32)).TrimEnd().Replace("\0", "");
        }

        public static void Write(Stream s, string Name, uint Location, uint Size)
        {
            s.Write(BitConverter.GetBytes(Location));
            s.Write(BitConverter.GetBytes(Size));

            s.Position += 8;

            var bytes = Encoding.UTF8.GetBytes(Name);
            s.Write(bytes);

            s.Position += 32 - bytes.Length;
        }
    }
}
