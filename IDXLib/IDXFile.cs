using System.Text;

namespace IDXLib
{
    public class IDXFile
    {
        public long Location;
        public ulong Size;
        public string Name;
        public IDXFile(string Name, long Location, ulong Size)
        {
            this.Name = Name;
            this.Location = Location;
            this.Size = Size;
        }
        public IDXFile(BinaryReader br, List<IDXFile> listToAddTo = null)
        {
            Location = br.ReadInt32();
            Size = br.ReadUInt32();

            br.BaseStream.Position += 8;
            byte[] textBuffer = new byte[32];
            br.Read(textBuffer);
            Name = Encoding.UTF8.GetString(textBuffer).TrimEnd().Replace("\0", "");

            if (listToAddTo != null)
                listToAddTo.Add(this);
        }
    }
}
