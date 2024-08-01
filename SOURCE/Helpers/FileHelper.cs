﻿using SOURCE.Models;
using System.Text;
using System.Text.Json;

namespace SOURCE.Helpers;

public class FileHelper
{
    public static async Task<List<Entity>> ReadJsonAsync()
    {
        var projectPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        var filePath = Path.Combine(projectPath, Constants.DATA_FILE_NAME);

        using var r = new StreamReader(filePath);

        var json = await r.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<List<Entity>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return data;
    }

    public static async Task<bool> CreateFileAsync(SourceFile sourceFile)
    {
        var projectPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName;
        var filePath = Path.Combine(projectPath, sourceFile.Path, sourceFile.Name);

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        }

        if (File.Exists(filePath) && sourceFile.SkipIfExists)
        {
            Console.WriteLine($"I found your file and skipped creating this. {filePath}");
            return true;
        }

        await using var fs = File.Create(filePath);

        var title = new UTF8Encoding(true).GetBytes(sourceFile.Text);
        await fs.WriteAsync(title);

        return true;
    }

    // public async void WriteJson(string destination)
    // {
    //     var jsonString = JsonSerializer.Serialize(destination, new JsonSerializerOptions { WriteIndented = true });
    //     using (var outputFile = new StreamWriter("dataReady.json"))
    //     {
    //         await outputFile.WriteLineAsync(jsonString);
    //     }
    // }

    public static string[] GetFileNames(string folderPath)
    {
        string rootPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName;
        folderPath = Path.Combine(rootPath, folderPath);
        var directoryInfo = new DirectoryInfo(folderPath);

        IEnumerable<FileInfo> files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

        return files.Select(s => Path.GetFileNameWithoutExtension(s.FullName)).ToArray()!;
    }
}