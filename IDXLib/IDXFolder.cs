
namespace IDXLib
{
    public class IDXFolder
    {
        // Start index of files
        public ushort StartIndex;

        public List<IDXFile> Files;

        public IDXFolder(BinaryReader br)
        {
            StartIndex = br.ReadUInt16();
            var numFiles = br.ReadUInt16();

            Files = new List<IDXFile>();

            if (numFiles == 0)
                return;

            var ogPos = br.BaseStream.Position;

            br.BaseStream.Position = IDX.FileSection + (StartIndex * 48);

            for (int i = 0; i < numFiles; i++)
                Files.Add(new IDXFile(br));

            // Probably shouldn'tr be jumping around the place but oh well!
            br.BaseStream.Position = ogPos;
        }

        public static void Write(Stream s, ushort startIndex, ushort numFiles)
        {
            s.Write(BitConverter.GetBytes(startIndex));
            s.Write(BitConverter.GetBytes(numFiles));
        }


        internal const string FolderChars = "_`abcdefghijklmnopqrstuvwxyz";

        public static string GetName(int i)
        {
            return FolderChars[i].ToString();
        }

        public static int GetIndex(char c)
        {
            return FolderChars.IndexOf(c);
        }
    }
}
