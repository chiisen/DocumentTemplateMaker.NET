using System;
using System.Collections.Generic;

namespace DocumentTemplateMaker.NET
{
    public class DateRange
    {
        public DateRange()
        {
            // 砍擋
            Helper helper_ = new Helper();
            helper_.DeleteAll(".\\output\\");
        }

        public void Maker(string tempFileName, string outputFileName, string startDate, string endDate)
        {
            // 讀檔案
            string text = System.IO.File.ReadAllText(tempFileName);

            var dates = new List<DateTime>();

            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);

                Console.WriteLine(dt);

                string newText = (string)text.Clone();
                newText = newText.Replace("#Month#", dt.Month.ToString("00"));
                newText = newText.Replace("#Day#", dt.Day.ToString("00"));

                // 寫檔案
                string fileName = outputFileName;
                fileName = fileName.Replace("#Month#", dt.Month.ToString("00"));
                fileName = fileName.Replace("#Day#", dt.Day.ToString("00"));
                System.IO.File.WriteAllText(fileName, newText);
            }
        }
    }
}
