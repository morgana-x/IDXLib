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
    }
}
