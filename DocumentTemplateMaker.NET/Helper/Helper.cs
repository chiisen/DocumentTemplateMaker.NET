using System;
using System.IO;

namespace DocumentTemplateMaker.NET
{
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
    }
}
