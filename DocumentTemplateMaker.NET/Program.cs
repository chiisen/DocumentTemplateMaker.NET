using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// C# 10 全新的 namespace 語法，不用再看到 namespace 的 { } 了！
namespace DocumentTemplateMaker.NET;

/// <summary>
/// 主程式
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        if (args == null)
        {
            Console.WriteLine($"沒有傳入參數");
            Console.ReadKey();
            return;
        }
        if (args.Length == 0)
        {
            Console.WriteLine($"沒有傳入參數 args.Length == {args.Length}");
            Console.ReadKey();
            return;
        }

        // 讀檔案
        string parameter_ = System.IO.File.ReadAllText(@".\Parameter.json");
        List<Parameter> descArray_ = JsonConvert.DeserializeObject<List<Parameter>>(parameter_);
        Dictionary<string, Parameter> dic_ = descArray_.ToDictionary(x => x.TempType, x => x);
        Parameter desc_ = dic_[args[0]];
        switch (desc_.TempType)
        {
            case "DateRange":
                // 砍擋
                Helper.DeleteAll(".\\output\\");

                for (int i = 0; i < desc_.SrcTempPath.Length; ++i)
                {
                    DateRange.Maker(desc_.SrcTempPath[i], desc_.OutputPath[i], desc_.StartDate, desc_.EndDate, desc_.Offset, desc_.OffsetUnit);
                }
                break;
            case "JsonMap":
                JsonMap.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0]);
                break;
            case "Replace":
                Replace.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0], desc_);
                break;
        }

        Console.WriteLine("程式結束");
        Console.ReadKey();

    }
}
