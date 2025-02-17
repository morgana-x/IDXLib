using IDXLib;
public partial class Program
{
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
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File \"{filePath}\" doesn't exist!");
            return;
        }

        IDX idx = new IDX(new FileStream(filePath, FileMode.Open, FileAccess.Read));

        string folder = filePath.Replace(".idx", "");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        Console.WriteLine($"Extracting to {folder}...");

        idx.ExtractAll(folder);
        idx.Dispose();

        Console.WriteLine($"Finished extracting {idx.files.Count} files!");
    }
}
