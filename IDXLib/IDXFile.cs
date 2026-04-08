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
    }
}
