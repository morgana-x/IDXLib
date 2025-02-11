using System.Text;

namespace IDXLib
{
    public class IDX
    {
        Stream stream;
        static byte[] header = new byte[30] { 0x49, 0x44, 0x58, 0x32, 0x01, 0x02, 0xB3, 0x11, 0x58, 0x23, 0x81, 0x0C, 0xB0, 0x55, 0x03, 0x00, 0x45, 0x4C, 0x45, 0x4D, 0x45, 0x4E, 0x54, 0x41, 0x4C, 0x20, 0x53, 0x4F, 0x46, 0x54 };
        public List<IDXFile> files = new List<IDXFile>();
        public IDX(Stream stream)
        {
            this.stream = stream;
            ReadHeader();
        }
        public byte[] GetFileData(IDXFile file)
        {
            byte[] buffer = new byte[file.Size];
            stream.Position = file.Location;
            stream.Read(buffer);
            return buffer;
        }
        public void ExtractFile(IDXFile file, string outFolder)
        {
            File.WriteAllBytes(Path.Combine(outFolder, file.Name), GetFileData(file));
        }
        public void ExtractAll(string outFolder)
        {
            foreach (IDXFile file in files)
                ExtractFile(file, outFolder);
        }
        private IDXFile ReadFile(BinaryReader br)
        {
            int dataLocation = br.ReadInt32();
            ulong dataSize = br.ReadUInt32();

            br.BaseStream.Position += 8;
            byte[] textBuffer = new byte[32];
            br.Read(textBuffer);
            string fileName = Encoding.UTF8.GetString(textBuffer).TrimEnd().Replace("\0", "");

            var fileEnt = new IDXFile(fileName, dataLocation, dataSize);
            files.Add(fileEnt);
            return fileEnt;
        }

        private void ReadHeader()
        {
            stream.Seek(0, SeekOrigin.Begin);

            BinaryReader br = new BinaryReader(stream);

            stream.Position += header.Length + 392; // Some unknown data here

            br.BaseStream.Position = 1056;

            var firstFile = ReadFile(br);

            while (br.BaseStream.Position < firstFile.Location)
                ReadFile(br);
        }
    }
}
