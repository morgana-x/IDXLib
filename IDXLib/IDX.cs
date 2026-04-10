namespace IDXLib
{
    public class IDX
    {
        // Constants

        const int NumFolders = 27;

        internal const int FolderSection = 412;

        internal const int FileSection = 1056;


        Stream _stream;
        BinaryReader _br;


        public byte[] Version = [2];

        public IDXFolder[] Folders = new IDXFolder[NumFolders];

        public ushort NumFiles;


        public IDX(Stream stream)
        {

            _stream = stream;
            _br = new BinaryReader(_stream);

            // Skip identifier
            _br.BaseStream.Position = 4;

            // Major, Minor
            Version = _br.ReadBytes(2);

            NumFiles = _br.ReadUInt16();

            uint fileSize = _br.ReadUInt32();

            uint dataSection = _br.ReadUInt32();

            // Folder entries location
            _br.BaseStream.Position = FolderSection;

            for (int i = 0; i < NumFolders; i++)
                Folders[i] = new IDXFolder(_br);
        }
        public byte[] GetFileData(IDXFile file)
        {
            _br.BaseStream.Position = file.Location;
            return _br.ReadBytes((int)file.Size);
        }

        public void ExtractFile(IDXFile file, string outFolder)
        {
            var parent = Directory.GetParent(outFolder).FullName;
            if (!Directory.Exists(parent))
                Directory.CreateDirectory(parent);

            File.WriteAllBytes(Path.Combine(outFolder, file.Name), GetFileData(file));
        }

        public void ExtractAll(string outFolder)
        {
            for (int i = 0; i < Folders.Length; i++)
                foreach (var file in Folders[i].Files)
                    ExtractFile(file, $"{outFolder}/{IDXFolder.GetName(i)}/");
        }


        public void Dispose()
        {
            _stream.Dispose();
            _stream.Close();
        }

        static void generateFileInfo(string folder, uint dataSection, out Stream directoryStream, out Stream fileStream)
        {
            directoryStream = new MemoryStream();

            fileStream = new MemoryStream();

            Stream dataStream = new MemoryStream();

            int fileIndex = 0;

            for (int i = 0; i < IDXFolder.FolderChars.Length; i++)
            {
                string path = folder + "/" + IDXFolder.FolderChars[i];

                var files = Directory.Exists(path) ? Directory.GetFiles(path).ToList() : new List<string>();
                files.Sort(new ShiftJisSorter());

                IDXFolder.Write(directoryStream, (ushort)(files.Count > 0 ? fileIndex : 0), (ushort)files.Count);

                fileIndex += files.Count;

                foreach(var f in files)
                {
                    var fs = new FileStream(f, FileMode.Open, FileAccess.Read);

                    IDXFile.Write(fileStream, Path.GetFileName(f), (uint)(dataSection + dataStream.Position), (uint)fs.Length, IDXCompress.ShouldBeCompressed(f));

                    fs.CopyTo(dataStream);
                    fs.Dispose();
                    fs.Close();
                }
            }

            dataStream.Position = 0;
            dataStream.CopyTo(fileStream);
            dataStream.Dispose();
            dataStream.Close();

            return;
        }

        // "IDX2"
        static byte[] Identifier = new byte[] { 0x49, 0x44, 0x58, 0x32 };

        // "ELEMENTAL SOFT"
        static byte[] Author = new byte[] { 0x45, 0x4C, 0x45, 0x4D, 0x45, 0x4E, 0x54, 0x41, 0x4C, 0x20, 0x53, 0x4F, 0x46, 0x54 };

        public static void Repack(string folder, string outPath="")
        {
            if (outPath == "")
                outPath = folder + ".idx";

            var numFiles = new DirectoryInfo(folder).GetFiles("*", SearchOption.AllDirectories).Length;

            uint dataSection = (uint)(FileSection + (numFiles * 48));

            Stream idxStream = new FileStream(outPath, FileMode.Create, FileAccess.Write);

            generateFileInfo(folder, dataSection, out Stream folderDataStream, out Stream fileDataStream);

            BinaryWriter bw = new BinaryWriter(idxStream);

            // IDX2
            bw.Write(Identifier);

            // Version 1.2
            bw.Write(new byte[2] { 1, 2 });

            // Number of files
            bw.Write((ushort)numFiles);

            // Archive Size
            bw.Write((uint)(FileSection + fileDataStream.Length));

            // Pointer to data section
            bw.Write(dataSection);

            // ELEMENTAL SOFT
            bw.Write(Author);

            // Write directory info
            copyStream(idxStream, folderDataStream, FolderSection);

            // Copy File list + data
            copyStream(idxStream, fileDataStream, FileSection);


            bw.Dispose();
            bw.Close();
        }

        private static void copyStream(Stream dest, Stream src, long pos)
        {
            dest.Position = pos;

            src.Position = 0;
            src.CopyTo(dest);
            src.Dispose();
            src.Close();
        }
    }
}
