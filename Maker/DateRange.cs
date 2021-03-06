using System;
using System.IO;

// C# 10 全新的 namespace 語法，不用再看到 namespace 的 { } 了！
namespace DocumentTemplateMaker.NET;

/// <summary>
/// 依據指定間格的時間範圍產生範型文件
/// </summary>
public class DateRange
{
    public static DateTime AddOffset(DateTime dt, int offSet, string offSetUnit)
    {
        return offSetUnit switch
        {
            "Day" => dt.AddDays(offSet),
            "Month" => dt.AddMonths(offSet),
            _ => dt.AddDays(offSet),
        };
    }

    public static DateTime AddOffsetDay(DateTime dt, int offSet, string offSetUnit, DateTime end)
    {
        switch (offSetUnit)
        {
            case "Day":
                DateTime newDate = dt.AddDays(offSet - 1);
                if(newDate > end)// 超過最後一天，改成最後一天
                {
                    newDate = end;
                }
                return newDate;
            case "Month":
                DateTime dt_ = dt.AddMonths(offSet);
                return dt_.AddDays(-1);
            default:
                return dt.AddDays(offSet - 1);
        }
    }

    /// <summary>
    /// 取代指定的時間資料
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="newText"></param>
    /// <param name="dt"></param>
    /// <param name="offsetDt"></param>
    public static void ReplaceByMonth(string fileName, string newText, DateTime dt, DateTime offsetDt)
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

        File.WriteAllText(fileName, newText);
    }

    public static void ReplaceByDay(string fileName, string newText, DateTime dt, int offSet, DateTime offsetDt)
    {
        if (offSet > 1)
        {
            // 特例->日期往後一天
            DateTime newDate_ = offsetDt.AddDays(1);
            newText = newText.Replace("#AfterEndYear#", newDate_.Year.ToString("0000"));
            newText = newText.Replace("#AfterEndMonth#", newDate_.Month.ToString("00"));
            newText = newText.Replace("#AfterEndDay#", newDate_.Day.ToString("00"));


            newText = newText.Replace("#StartYear#", dt.Year.ToString("0000"));
            newText = newText.Replace("#StartMonth#", dt.Month.ToString("00"));
            newText = newText.Replace("#StartDay#", dt.Day.ToString("00"));

            newText = newText.Replace("#EndYear#", offsetDt.Year.ToString("0000"));
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

        File.WriteAllText(fileName, newText);
    }

    public static void ReplaceData(string fileName, string newText, DateTime dt, int offSet, DateTime offsetDt, string offSetUnit)
    {
        switch (offSetUnit)
        {
            case "Month":
                ReplaceByMonth(fileName, newText, dt, offsetDt);
                break;
            case "Day":
                ReplaceByDay(fileName, newText, dt, offSet, offsetDt);
                break;
        }
    }

    /// <summary>
    /// //每月最後是31號改成31日
    /// </summary>
    /// <param name="offSetUnit">只判斷 "Day"</param>
    /// <param name="offsetDt">區段內的最後一天</param>
    /// <returns></returns>
    private static bool CheckAddDayAndFixIt(string offSetUnit, ref DateTime offsetDt)
    {
        if (offSetUnit == "Day" && offsetDt.Day == 30)
        {
            DateTime newDt_ = offsetDt.AddDays(1);
            if (newDt_.Month == offsetDt.Month)
            {
                offsetDt = newDt_;
                return true;
            }
        }
        return false;
    }

    public static void Maker(string tempFileName, string outputFileName, string startDate, string endDate, int offSet, string offSetUnit)
    {
        // 讀檔案
        string text = File.ReadAllText(tempFileName);

        DateTime start = Convert.ToDateTime(startDate);
        DateTime end = Convert.ToDateTime(endDate);

        for (var dt = start; dt <= end; dt = AddOffset(dt, offSet, offSetUnit))
        {
            DateTime offsetDt_ = AddOffsetDay(dt, offSet, offSetUnit, end);

            bool isAddDays = CheckAddDayAndFixIt(offSetUnit, ref offsetDt_);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(dt);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" ~ ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(offsetDt_);
            Console.Write('\n');
            Console.ResetColor();

            ReplaceData(outputFileName, (string)text.Clone(), dt, offSet, offsetDt_, offSetUnit);

            if(isAddDays)
            {
                dt = dt.AddDays(1);
            }
        }
    }
}
