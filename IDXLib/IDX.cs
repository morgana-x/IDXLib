namespace IDXLib
{
    public class IDX
    {
        // "IDX2"
        static byte[] Header = new byte[] { 0x49, 0x44, 0x58, 0x32 };

        // "ELEMENTAL SOFT"
        static byte[] Author = new byte[] { 0x45, 0x4C, 0x45, 0x4D, 0x45, 0x4E, 0x54, 0x41, 0x4C, 0x20, 0x53, 0x4F, 0x46, 0x54 };

        Stream _stream;
        BinaryReader _br;

        public List<IDXFile> Files;

        public short Version;
        public IDX(Stream stream)
        {
            Files = new List<IDXFile>();

            _stream = stream;
            _br = new BinaryReader(_stream);

            // Skip identifier
            _br.BaseStream.Position = 4;

            // This is actually 2 bytes for Major / Minor version, but since it doesn't change, who cares!
            Version = _br.ReadInt16();

            ushort numFiles = _br.ReadUInt16();

            uint fileSize = _br.ReadUInt32();

            // All file data pointers are absolute... todo: figure out why this exists
            uint dataSection = _br.ReadUInt32();

            // File metadata location
            _br.BaseStream.Position = 1056;
        
            for (int i = 0; i < numFiles && _br.BaseStream.Position < dataSection; i++)
                Files.Add(new IDXFile(_br));
        }

        public void ExtractFile(IDXFile file, string outFolder)
        {
            File.WriteAllBytes(Path.Combine(outFolder, file.Name), GetFileData(file));
        }

        public void ExtractAll(string outFolder)
        {
            foreach (IDXFile file in Files)
                ExtractFile(file, outFolder);
        }

        public byte[] GetFileData(IDXFile file)
        {
            _br.BaseStream.Position = file.Location;
            return _br.ReadBytes((int)file.Size);
        }

        public void Dispose()
        {
            _stream.Dispose();
            _stream.Close();
            Files.Clear();
        }
    }
}
