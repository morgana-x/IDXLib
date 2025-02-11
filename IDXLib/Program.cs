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
        IDX idx = new IDX(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        string folder = filePath.Replace(".idx", "");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        Console.WriteLine($"Extracting to {folder}...");
        idx.ExtractAll(folder);
        Console.WriteLine($"Finished extracting {idx.files.Count} files!");
    }
}
