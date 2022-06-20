using System;
using System.IO;

// C# 10 全新的 namespace 語法，不用再看到 namespace 的 { } 了！
namespace DocumentTemplateMaker.NET;

public class Replace
{
    public static void Maker(string tempFileName, string outputFileName, Parameter para, bool deleteAll = true)
    {
        // 讀檔案
        string text = File.ReadAllText(tempFileName);

        // 砍擋
        if (deleteAll)
        {
            Helper.DeleteAll(".\\output\\");
        }

        if (para.KeyWords.Length != para.ReplaceWords.Length)
        {
            Console.WriteLine("[ERROR] 參數數量不相等!");
            return;
        }

        string textSetting = "";
        for (int i = 0; i < para.KeyWords.Length; ++i)
        {
            if (int.TryParse(para.KeyWords[i], out _)
                || float.TryParse(para.KeyWords[i], out float f)
                || para.KeyWords[i].ToLower() == "false"
                || para.KeyWords[i].ToLower() == "true")
            {
                text = text.Replace(para.KeyWords[i], para.ReplaceWords[i]);

                textSetting += para.KeyWords[i] + " " + para.ReplaceWords[i] + "\n";
            }
            else
            {
                text = text.Replace(para.KeyWords[i], '"' + para.ReplaceWords[i] + '"');

                textSetting += para.KeyWords[i] + " " + para.ReplaceWords[i] + "\n";
            }
            textSetting += "\n";
        }


        // 寫檔案
        File.WriteAllText(outputFileName, text);

        File.WriteAllText(".\\output\\map.txt", textSetting);
    }
}
