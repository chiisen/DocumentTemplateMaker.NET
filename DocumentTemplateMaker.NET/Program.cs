using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentTemplateMaker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            // 讀檔案
            string parameter_ = System.IO.File.ReadAllText(@".\Parameter.json");
            List<Parameter> descArray_ = JsonConvert.DeserializeObject<List<Parameter>>(parameter_);
            Dictionary<string, Parameter> dic_ = descArray_.ToDictionary(x => x.TempType, x => x);
            Parameter desc_ = dic_[args[0]];
            switch (desc_.TempType)
            {
                case "DateRange":
                    DateRange dateRange_ = new DateRange();
                    for (int i = 0; i < desc_.SrcTempPath.Length; ++i)
                    {
                        dateRange_.Maker(desc_.SrcTempPath[i], desc_.OutputPath[i], desc_.StartDate, desc_.EndDate);
                    }
                    break;
                case "JsonMap":
                    JsonMap jsonMap_ = new JsonMap();
                    jsonMap_.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0]);
                    break;
                case "Replace":
                    Replace replace_ = new Replace();
                    replace_.Maker(desc_.SrcTempPath[0], desc_.OutputPath[0], desc_);
                    break;
            }            

            Console.WriteLine("程式結束");
            Console.ReadKey();

        }
    }
}
