namespace IDXLib
{
    public class IDX
    {
        Stream stream;
     
        public List<IDXFile> files = new List<IDXFile>();
        public IDX(Stream stream)
        {
            this.stream = stream;
            ReadHeader();
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
        public byte[] GetFileData(IDXFile file)
        {
            byte[] buffer = new byte[file.Size];
            stream.Position = file.Location;
            stream.Read(buffer);
            return buffer;
        }

        private void ReadHeader()
        {
            stream.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(stream);
            ReadFileInfo(br);
        }
        private void ReadFileInfo(BinaryReader br)
        {
            files.Clear();

            br.BaseStream.Position = 1056;

            var firstFile = new IDXFile(br, files);

            while (br.BaseStream.Position < firstFile.Location)
                new IDXFile(br, files);
        }

        public void Dispose()
        {
            stream.Dispose();
            stream.Close();
            files.Clear();
        }
    }
}
