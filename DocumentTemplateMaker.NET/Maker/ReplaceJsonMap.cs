using System;
using System.Collections.Generic;

namespace DocumentTemplateMaker.NET;

internal class ReplaceJsonMap
{
    public static void Maker(string tempFileName, string outputFileName, Parameter para)
    {
        // 讀檔案
        string text = System.IO.File.ReadAllText(tempFileName);

        // 砍擋
        Helper.DeleteAll(".\\output\\");

        string[] lines_ = text.Split("\r\n");

        string all_ = "";

        int count = 1;
        foreach (string line_ in lines_)
        {
            // 寫檔案
            string tempInputTxt_ = outputFileName + "input" + count.ToString() + ".txt";
            System.IO.File.WriteAllText(tempInputTxt_, line_);

            string tempOutputTxt_ = outputFileName + "output" + count.ToString() + ".txt";            
            JsonMap.Maker(tempInputTxt_, tempOutputTxt_, false);

            // 砍擋
            Helper.DeleteFile(tempInputTxt_);

            string tempInoutSql_ = tempOutputTxt_;
            string tempOutputSql_ = tempOutputTxt_;
            tempOutputSql_ = tempOutputSql_.Replace(".txt", ".sql");
            Replace.Maker(tempInoutSql_, tempOutputSql_, para, false);

            // 砍擋
            Helper.DeleteFile(tempInoutSql_);

            // 讀檔案
            string txtAll = System.IO.File.ReadAllText(tempOutputSql_);
            all_ += txtAll;

            // 砍擋
            Helper.DeleteFile(tempOutputSql_);

            count++;
        }

        // 寫檔案
        System.IO.File.WriteAllText(outputFileName + "alter.sql", all_);
    }
}
