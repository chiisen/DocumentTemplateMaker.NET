using System;
using Newtonsoft.Json;

namespace DocumentTemplateMaker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            // 讀檔案
            string parameter_ = System.IO.File.ReadAllText(@".\Parameter.json");

            Parameter desc_ = JsonConvert.DeserializeObject<Parameter>(parameter_);
            switch(desc_.TempType)
            {
                case "DateRange":
                    DateRange dateRange_ = new DateRange();
                    for (int i = 0; i < desc_.SrcTempPath.Length; ++i)
                    {
                        dateRange_.Maker(desc_.SrcTempPath[i], desc_.OutputPath[i], desc_.StartDate, desc_.EndDate);
                    }
                    break;
            }            

            Console.WriteLine("程式結束");
            Console.ReadKey();

        }
    }
}
