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

        public DateTime AddOffset(DateTime dt, int offSet, string offSetUnit)
        {
            switch(offSetUnit)
            {
                case "Day":
                    return dt.AddDays(offSet);
                case "Month":
                    return dt.AddMonths(offSet);
                default:
                    return dt.AddDays(offSet);
            }
        }

        public DateTime AddOffsetDay(DateTime dt, int offSet, string offSetUnit)
        {
            switch (offSetUnit)
            {
                case "Day":
                    return dt.AddDays(offSet - 1);
                case "Month":
                    DateTime dt_ = dt.AddMonths(offSet);
                    return dt_.AddDays(-1);
                default:
                    return dt.AddDays(offSet - 1);
            }
        }

        public void DataFormatByMonth(string fileName, string newText, DateTime dt, DateTime offsetDt)
        {
            newText = newText.Replace("#StartYear#", dt.Year.ToString("0000"));
            newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
            newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

            newText = newText.Replace("#EndYear#", dt.Year.ToString("0000"));
            newText = newText.Replace("#EndMonth#", offsetDt.Month.ToString("00"));
            newText = newText.Replace("#EndDay#", offsetDt.Day.ToString("00"));

            // 寫檔案: startTime-endTime
            fileName = fileName.Replace("#StartYear#", dt.Year.ToString("0000"));
            fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
            fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

            fileName = fileName.Replace("#EndYear#", offsetDt.Year.ToString("0000"));
            fileName = fileName.Replace("#EndMonth#", offsetDt.Month.ToString("00"));
            fileName = fileName.Replace("#EndDay#", offsetDt.Day.ToString("00"));

            fileName = fileName.Replace("#-#", "-");

            System.IO.File.WriteAllText(fileName, newText);
        }

        public void DataFormatByDay(string fileName, string newText, DateTime dt, int offSet, DateTime offsetDt)
        {
            if (offSet > 1)
            {
                newText = newText.Replace("#StartYear#", dt.Year.ToString("0000"));
                newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
                newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

                newText = newText.Replace("#EndYear#", dt.Year.ToString("0000"));
                newText = newText.Replace("#EndMonth#", offsetDt.Month.ToString("00"));
                newText = newText.Replace("#EndDay#", offsetDt.Day.ToString("00"));

                // 寫檔案格式: startTime-endTime
                fileName = fileName.Replace("#StartYear#", dt.Year.ToString("0000"));
                fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                fileName = fileName.Replace("#EndYear#", offsetDt.Year.ToString("0000"));
                fileName = fileName.Replace("#EndMonth#", offsetDt.Month.ToString("00"));
                fileName = fileName.Replace("#EndDay#", offsetDt.Day.ToString("00"));

                fileName = fileName.Replace("#-#", "-");
            }
            else
            {
                newText = newText.Replace("#StartYear#", dt.Year.ToString("0000"));
                newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
                newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

                newText = newText.Replace("#EndYear#", dt.Year.ToString("0000"));
                newText = newText.Replace("#EndMonth#", dt.Month.ToString("00"));
                newText = newText.Replace("#EndDay#", dt.Day.ToString("00"));

                // 寫檔案: startTime
                fileName = fileName.Replace("#StartYear#", dt.Year.ToString("0000"));
                fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                fileName = fileName.Replace("#EndYear#", "");
                fileName = fileName.Replace("#EndMonth#", "");
                fileName = fileName.Replace("#EndDay#", "");

                fileName = fileName.Replace("#-#", "");
            }

            System.IO.File.WriteAllText(fileName, newText);
        }

        public void DataFormat(string fileName, string newText, DateTime dt, int offSet, DateTime offsetDt, string offSetUnit)
        {
            switch (offSetUnit)
            {
                case "Month":
                    DataFormatByMonth(fileName, newText, dt, offsetDt);
                    break;
                case "Day":
                    DataFormatByDay(fileName, newText, dt, offSet, offsetDt);
                    break;
            }
        }

        public void Maker(string tempFileName, string outputFileName, string startDate, string endDate, int offSet, string offSetUnit)
        {
            // 讀檔案
            string text = System.IO.File.ReadAllText(tempFileName);

            var dates = new List<DateTime>();

            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);

            for (var dt = start; dt <= end; dt = AddOffset(dt, offSet, offSetUnit))
            {
                dates.Add(dt);

                DateTime offsetDt_ = AddOffsetDay(dt, offSet, offSetUnit);

                Console.WriteLine(dt + " ~ " + offsetDt_);

                DataFormat(outputFileName, (string)text.Clone(), dt, offSet, offsetDt_, offSetUnit);
            }
        }
    }
}
