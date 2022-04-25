using System;
using System.IO;

// C# 10 全新的 namespace 語法，不用再看到 namespace 的 { } 了！
namespace DocumentTemplateMaker.NET;

/// <summary>
/// 輔助工具
/// </summary>
public class Helper
{
    /// <summary>
    /// 刪除目錄內的所有檔案
    /// </summary>
    /// <param name="filePath"></param>
    public static void DeleteAll(string filePath)
    {
        DirectoryInfo di = new(filePath);
        if (!di.Exists)
        {
            Console.WriteLine($"{filePath} 資料夾未建立");
            di.CreateSubdirectory(filePath);
            Console.WriteLine($"建立 {filePath} 資料夾");
            return;
        }

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    public static void DeleteFile(string filePath)
    {
        FileInfo file = new(filePath);
        if (!file.Exists)
        {
            Console.WriteLine($"{filePath} 檔案不存在");
            return;
        }
        file.Delete();
    }
}
