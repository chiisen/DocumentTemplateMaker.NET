using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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

        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose() // 設定最低顯示層級   預設: Information
        .WriteTo.Console() // 輸出到 指令視窗
        .WriteTo.File("log-.log",
            rollingInterval: RollingInterval.Day, // 每天一個檔案
            outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u5}] {Message:lj}{NewLine}{Exception}"
        ) // 輸出到檔案 檔名範例: log-20211005.log
        .CreateLogger();

        /*
        Log.Verbose("Hello");
        Log.Debug("Hello");
        Log.Information("Hello");
        Log.Warning("Hello");
        Log.Error("Hello");
        Log.Fatal("Hello");
        */

        // 讀檔案
        string parameter_ = File.ReadAllText(@".\Parameter.json");
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
                JsonMap.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0], desc_);
                break;
            case "Replace":
                Replace.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0], desc_);
                break;
            case "ReplaceJsonMap":
                ReplaceJsonMap.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0], desc_);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("請記得更新 Parameter.json 裡的 KeyWords 與 ReplaceWords(這些有些資料是 ReplaceJsonMap.txt 上查不到的!)");
                break;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("程式結束，按任何鍵結束。");
        Console.ResetColor();
        Console.ReadKey();

        Log.CloseAndFlush();
    }
}
