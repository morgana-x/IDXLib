namespace IDXLib
{
    public class IDX
    {
        Stream _stream;
        BinaryReader _br;

        public List<IDXFile> Files;

        public IDX(Stream stream)
        {
            Files = new List<IDXFile>();

            _stream = stream;
            _br = new BinaryReader(_stream);

            // File metadata location
            _br.BaseStream.Position = 1056;
        
            Files.Add(new IDXFile(_br));

            while (_br.BaseStream.Position < Files.First().Location)
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
