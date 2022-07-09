using System.IO;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DocumentTemplateMaker.NET;

internal class ReplaceJsonMap
{
    public static void Maker(string tempFileName, string outputFileName, Parameter para)
    {
        // 讀檔案
        string text = File.ReadAllText(tempFileName);

        // 砍擋
        Helper.DeleteAll(".\\output\\");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(@"./output/debug.xlsx"))
        {
            var sheet = package.Workbook.Worksheets.Add("分析的log內容ReplaceJsonMap.txt");

            sheet.Cells["A1"].Value = text;

            // Save to file
            package.Save();
        }

        string[] lines_ = text.Split("\r\n");

        string all_ = "";

        int count = 1;
        foreach (string line_ in lines_)
        {
            // 寫檔案
            string tempInputTxt_ = outputFileName + "input" + count.ToString() + ".txt";
            File.WriteAllText(tempInputTxt_, line_);

            string tempOutputTxt_ = outputFileName + "output" + count.ToString() + ".txt";            
            JsonMap.Maker(tempInputTxt_, tempOutputTxt_, para, false);

            // 砍擋
            Helper.DeleteFile(tempInputTxt_);

            string tempInoutSql_ = tempOutputTxt_;
            string tempOutputSql_ = tempOutputTxt_;
            tempOutputSql_ = tempOutputSql_.Replace(".txt", ".sql");
            Replace.Maker(tempInoutSql_, tempOutputSql_, para, false);

            // 砍擋
            Helper.DeleteFile(tempInoutSql_);

            // 讀檔案
            string txtAll = File.ReadAllText(tempOutputSql_);
            all_ += txtAll;

            // 砍擋
            Helper.DeleteFile(tempOutputSql_);

            count++;
        }

        // 寫檔案
        File.WriteAllText(outputFileName + "alter.sql", all_);
    }
}
