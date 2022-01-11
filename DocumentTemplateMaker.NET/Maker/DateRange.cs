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

                string newText = (string)text.Clone();
                DateTime offset_ = AddOffsetDay(dt, offSet, offSetUnit);

                Console.WriteLine(dt + " ~ " + offset_);

                string fileName = outputFileName;
                switch(offSetUnit)
                {
                    case "Month":
                        newText = newText.Replace("#StartYear#", dt.Year.ToString("0000"));
                        newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
                        newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

                        newText = newText.Replace("#EndYear#", dt.Year.ToString("0000"));
                        newText = newText.Replace("#EndMonth#", offset_.Month.ToString("00"));
                        newText = newText.Replace("#EndDay#", offset_.Day.ToString("00"));

                        // 寫檔案
                        fileName = fileName.Replace("#StartYear#", dt.Year.ToString("0000"));
                        fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                        fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                        fileName = fileName.Replace("#EndYear#", offset_.Year.ToString("0000"));
                        fileName = fileName.Replace("#EndMonth#", offset_.Month.ToString("00"));
                        fileName = fileName.Replace("#EndDay#", offset_.Day.ToString("00"));

                        fileName = fileName.Replace("#-#", "-");
                        break;
                    case "Day":
                        if (offSet > 1)
                        {
                            newText = newText.Replace("#StartYear#", dt.Year.ToString("0000"));
                            newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
                            newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

                            newText = newText.Replace("#EndYear#", dt.Year.ToString("0000"));
                            newText = newText.Replace("#EndMonth#", offset_.Month.ToString("00"));
                            newText = newText.Replace("#EndDay#", offset_.Day.ToString("00"));

                            // 寫檔案
                            fileName = fileName.Replace("#StartYear#", dt.Year.ToString("0000"));
                            fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                            fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                            fileName = fileName.Replace("#EndYear#", offset_.Year.ToString("0000"));
                            fileName = fileName.Replace("#EndMonth#", offset_.Month.ToString("00"));
                            fileName = fileName.Replace("#EndDay#", offset_.Day.ToString("00"));

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

                            // 寫檔案
                            fileName = fileName.Replace("#StartYear#", dt.Year.ToString("0000"));
                            fileName = fileName.Replace("#StartMonth#", dt.Month.ToString("00"));
                            fileName = fileName.Replace("#StartDay#", dt.Day.ToString("00"));

                            fileName = fileName.Replace("#EndYear#", offset_.Year.ToString("0000"));
                            fileName = fileName.Replace("#EndMonth#", "");
                            fileName = fileName.Replace("#EndDay#", "");

                            fileName = fileName.Replace("#-#", "");
                        }
                        break;
                }
                               

                System.IO.File.WriteAllText(fileName, newText);
            }
        }
    }
}
