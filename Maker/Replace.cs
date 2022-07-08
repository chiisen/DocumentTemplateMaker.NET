using System;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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

        for (int i = 0; i < para.KeyWords.Length; ++i)
        {
            if (int.TryParse(para.KeyWords[i], out _)
                || float.TryParse(para.KeyWords[i], out float f)
                || para.KeyWords[i].ToLower() == "false"
                || para.KeyWords[i].ToLower() == "true")
            {
                text = text.Replace(para.KeyWords[i], para.ReplaceWords[i]);
            }
            else
            {
                text = text.Replace(para.KeyWords[i], '"' + para.ReplaceWords[i] + '"');
            }
        }


        // 寫檔案
        File.WriteAllText(outputFileName, text);

        // 取代內容Parameter.json
        ReplaceWords(para);
    }

    /// <summary>
    /// 取代內容Parameter.json
    /// </summary>
    private static void ReplaceWords(Parameter para)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(@"./output/debug.xlsx"))
        {
            var sheet = package.Workbook.Worksheets.Add("取代內容Parameter.json");

            sheet.Cells["A1"].Value = "KeyWords";
            sheet.Cells["B1"].Value = "ReplaceWords";
            sheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid; // 設定背景填色方法，沒有這一行就上背景色會報錯
                                                                             // Solid = 填滿；另外還有斜線、交叉線、條紋等
            sheet.Cells["B1"].Style.Fill.PatternType = ExcelFillStyle.Solid; // 設定背景填色方法，沒有這一行就上背景色會報錯
                                                                             // Solid = 填滿；另外還有斜線、交叉線、條紋等
            sheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray); // 儲存格顏色
            sheet.Cells["B1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray); // 儲存格顏色

            int count = 2;
            for (int i = 0; i < para.KeyWords.Length; ++i)
            {
                string cellsKeyWords = "A" + count.ToString();
                string cellsReplaceWord = "B" + count.ToString();

                sheet.Cells[cellsKeyWords].Value = para.KeyWords[i];
                sheet.Cells[cellsReplaceWord].Value = para.ReplaceWords[i];

                count++;
            }

            // Save to file
            package.Save();
        }
    }
}
