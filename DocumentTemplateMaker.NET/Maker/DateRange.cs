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

        public void Maker(string tempFileName, string outputFileName, string startDate, string endDate, int offSet)
        {
            // 讀檔案
            string text = System.IO.File.ReadAllText(tempFileName);

            var dates = new List<DateTime>();

            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);

            for (var dt = start; dt <= end; dt = dt.AddDays(offSet))
            {
                dates.Add(dt);

                Console.WriteLine(dt);

                string newText = (string)text.Clone();
                DateTime offset_ = dt.AddDays(offSet - 1);
                string fileName = outputFileName;
                if (offSet > 1)
                {
                    newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
                    newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

                    newText = newText.Replace("#EndMonth#", offset_.Month.ToString("00"));
                    newText = newText.Replace("#EndDay#", offset_.Day.ToString("00"));

                    // 寫檔案
                    fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                    fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                    fileName = fileName.Replace("#EndMonth#", offset_.Month.ToString("00"));
                    fileName = fileName.Replace("#EndDay#", offset_.Day.ToString("00"));
                }
                else
                {
                    newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
                    newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

                    newText = newText.Replace("#EndMonth#", dt.Month.ToString("00"));
                    newText = newText.Replace("#EndDay#", dt.Day.ToString("00"));

                    // 寫檔案
                    fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                    fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                    fileName = fileName.Replace("#EndMonth#", "");
                    fileName = fileName.Replace("#EndDay#", "");
                }                

                System.IO.File.WriteAllText(fileName, newText);
            }
        }
    }
}
