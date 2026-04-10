using IDXLib;
public partial class Program
{

    static void Process(string filePath)
    {
        if (File.Exists(filePath))
        {
            IDX idx = new IDX(new FileStream(filePath, FileMode.Open, FileAccess.Read));

            string folder = filePath.Replace(".idx", "");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            Console.WriteLine($"Extracting to {folder}...");

            idx.ExtractAll(folder);

            Console.WriteLine($"Finished extracting {idx.NumFiles} files!");

            idx.Dispose();
            return;
        }

        if (!Directory.Exists(filePath) && !File.Exists(filePath))
        {
            Console.WriteLine($"Couldn't find file or folder \"{filePath}\"");
            return;
        }

        Console.WriteLine($"Repacking {filePath}...");

        IDX.Repack(filePath);

        Console.WriteLine("Repacked!");
    }

    public static void Main (string[] args)
    {
        string filePath = "";

        if (args.Length > 0 )
            filePath = args[0];
        else
        {
            Console.WriteLine("Drag and drop IDX file to extract!");
            filePath = Console.ReadLine().Replace("\"","");
        }

        Process(filePath);
    }
}
