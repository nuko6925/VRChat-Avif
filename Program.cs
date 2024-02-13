using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VRChat_Avif;

[SuppressMessage("Interoperability", "CA1416:プラットフォームの互換性を検証")]
internal abstract class Program
{
    public static void Main(string[] args)
    {
        if (!Directory.Exists($@"C:\Users\{Environment.UserName}\Pictures\VRChat\AVIF"))
        {
            Directory.CreateDirectory($@"C:\Users\{Environment.UserName}\Pictures\VRChat\AVIF");
        }
        var watcher = new FileSystemWatcher();
        watcher.Path = $@"C:\Users\{Environment.UserName}\Pictures\VRChat";
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        watcher.Filter = "*.png";
        watcher.IncludeSubdirectories = true;
        watcher.Created += file_Created;
        watcher.EnableRaisingEvents = true;
        Console.WriteLine("このソフトはcavifを使用しています。");
        Console.WriteLine("https://github.com/link-u/cavif");
        Console.WriteLine($@"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] Listening on 'C:\Users\{Environment.UserName}\Pictures\VRChat'");
        Console.ReadKey();
    }

    private static void file_Created(object obj, FileSystemEventArgs e)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] File {e.Name} has created.");
        if (!$"{e.Name}".Contains("png")) return;
        var outputPath = $@"C:\Users\{Environment.UserName}\Pictures\VRChat\AVIF\{e.Name!.Replace("png", "avif").Split('\\')[1]}";
        var outputDir = $@"C:\Users\{Environment.UserName}\Pictures\VRChat\AVIF";
        var pInfo = new ProcessStartInfo
        {
            FileName = "lib\\cavif",
            Arguments = $"\"{e.FullPath}\" -o {outputDir} --quiet"
        };
        var process = Process.Start(pInfo);
        process!.WaitForExit();
        Console.WriteLine(File.Exists(outputPath) ? $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] Saved to '{outputPath}'" : $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] 多分エラー");
        //Converter.ToAvif(Image.Load(e.FullPath), e.Name!);
        /*var image = new Bitmap(e.FullPath);
        var outputPath = $@"C:\Users\{Environment.UserName}\Pictures\VRChat\AVIF\{e.Name!.Replace("png", "avif").Split('\\')[1]}";
        var codec = GetEncoderInfo("image/avif");
        var encoderParameters = new EncoderParameters(1);
        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
        image.Save(outputPath, codec, encoderParameters);*/
    }

    /*private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        var codecs = ImageCodecInfo.GetImageEncoders();
        foreach (var codec in codecs)
        {
            Console.WriteLine(codec.MimeType);
        }
        return codecs.FirstOrDefault(codec => codec.MimeType == mimeType)!;
    }*/
}