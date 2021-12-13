using System;
using System.Collections.Generic;

namespace DocumentTemplateMaker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            TempMaker(@".\DocumentTemplate-1.sql", @".\output\alter#1#-#2#-1.sql");
            TempMaker(@".\DocumentTemplate-2.sql", @".\output\alter#1#-#2#-2.sql");

            Console.WriteLine("程式結束");
            Console.ReadKey();

        }

        static void TempMaker(string tempFileName, string outputFileName)
        {
            // 讀檔案
            string text = System.IO.File.ReadAllText(tempFileName);

            var dates = new List<DateTime>();
            DateTime start = new DateTime(2021, 09, 01, 00, 00, 00);
            DateTime end = new DateTime(2021, 09, 03, 00, 00, 00);

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);

                Console.WriteLine(dt);

                string newText = (string)text.Clone();
                newText = newText.Replace("#1#", dt.Month.ToString("00"));
                newText = newText.Replace("#2#", dt.Day.ToString("00"));

                // 寫檔案
                string fileName = outputFileName;
                fileName = fileName.Replace("#1#", dt.Month.ToString("00"));
                fileName = fileName.Replace("#2#", dt.Day.ToString("00"));
                System.IO.File.WriteAllText(fileName, newText);
            }
        }
    }
}
