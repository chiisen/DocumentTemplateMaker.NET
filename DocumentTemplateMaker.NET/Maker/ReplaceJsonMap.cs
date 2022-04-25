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

        int count = 1;
        foreach (string line_ in lines_)
        {
            // 寫檔案
            string tempInput_ = outputFileName + "input" + count.ToString() + ".txt";
            System.IO.File.WriteAllText(tempInput_, line_);

            string tempOutput_ = outputFileName + "output" + count.ToString() + ".txt";            
            JsonMap.Maker(tempInput_, tempOutput_, false);

            string tempInoutSql_ = tempOutput_;
            string tempOutputSql_ = tempOutput_;
            Replace.Maker(tempInoutSql_, tempOutputSql_, para);
            count++;
        }


    }
}
