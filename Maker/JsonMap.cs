using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

// C# 10 全新的 namespace 語法，不用再看到 namespace 的 { } 了！
namespace DocumentTemplateMaker.NET;

public class JsonMap
{
    public static void Maker(string tempFileName, string outputFileName, bool deleteAll = true)
    {
        // 讀檔案
        string text = File.ReadAllText(tempFileName);
        //text = text.Replace(" ", "");
        text = text.Replace("\r\n", "");

        // 砍擋
        if (deleteAll)
        {
            Helper.DeleteAll(".\\output\\");
        }

        string[] lines_ = text.Split(',');
        Dictionary<string, string> map_ = new();
        foreach (string line_ in lines_)
        {
            if (line_.Contains(':'))
            {
                string newLine_ = line_;
                int indexStart_ = newLine_.IndexOf('{');
                if (indexStart_ != -1)
                {
                    newLine_ = newLine_.Remove(0, indexStart_ + 1);
                }
                int indexEnd_ = newLine_.IndexOf('}');
                if (indexEnd_ != -1)
                {
                    string[] spliteLine_ = newLine_.Split('}');
                    newLine_ = spliteLine_[0];
                }

                string[] mapLine_ = null;
                if (newLine_.Contains("IP:::"))
                {
                    if (map_.ContainsKey("IP"))
                    {
                        Console.WriteLine("IP 重複了");
                    }
                    else
                    {
                        string ip_ = newLine_.Remove(0, 3);
                        map_.Add("IP", ip_);
                    }
                }
                else if (newLine_.Contains("AddDate:"))
                {
                    if (map_.ContainsKey("AddDate"))
                    {
                        Console.WriteLine("AddDate 重複了");
                    }
                    else
                    {
                        string date_ = newLine_.Remove(0, 8);
                        map_.Add("AddDate", date_);
                    }
                }
                else
                {
                    mapLine_ = newLine_.Split(':');
                    if (map_.ContainsKey(mapLine_[0]))
                    {
                        Console.WriteLine(mapLine_[0] + " 重複了");
                    }
                    else
                    {
                        map_.Add(mapLine_[0], mapLine_[1]);
                    }
                }                
            }
        }

        string newText = "";
        foreach (KeyValuePair<string, string> item in map_)
        {
            string newLine = "";
            if (int.TryParse(item.Value, out int n)
                || float.TryParse(item.Value, out float f)
                || item.Value.ToLower() == "false"
                || item.Value.ToLower() == "true")
            {
                newLine = string.Format("\"{0}\" : {1}\r\n", item.Key, item.Value);
                newText += newLine;
            }
            else
            {
                newLine = string.Format("\"{0}\" : \"{1}\"\r\n", item.Key, item.Value);
                newText += newLine;
            }
            Console.WriteLine(newLine);
        }

        // 寫檔案
        File.WriteAllText(outputFileName, newText);


        // 分析log得到的
        LogToMap(map_);


        // 顯示沒讀到的欄位
        Dictionary<string, bool> totalFields = TotalLostFields(map_);


        string totalLostFields = "\n";
        foreach (KeyValuePair<string, bool> item in totalFields)
        {
            if (!item.Value)
            {
                totalLostFields += item.Key + "\n";
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("目前沒有的欄位:" + totalLostFields);
        Console.ForegroundColor = ConsoleColor.White;


        // 讀檔案
        string sqlText = File.ReadAllText(".\\DocumentTemplate\\INSERT-WAGERS.sql");
        foreach (KeyValuePair<string, string> item in map_)
        {
            string keyword_ = string.Format("#{0}#", item.Key);
            

            if (int.TryParse(item.Value, out int n)
                || float.TryParse(item.Value, out float f)
                || item.Value.ToLower() == "false"
                || item.Value.ToLower() == "true")
            {
                sqlText = sqlText.Replace(keyword_, item.Value);
            }
            else
            {
                sqlText = sqlText.Replace(keyword_, '"' + item.Value + '"');
            }
        }

        // 寫檔案
        File.WriteAllText(outputFileName, sqlText);
    }

    /// <summary>
    /// 分析log得到的
    /// </summary>
    private static void LogToMap(Dictionary<string, string> map)
    {
        // Wid, Cid, UserName, UpId, HallId, Rid, ShoeNo, PlayNo, GGId, GameId, GameTypeId, Result, BetGold, BetPoint, WinGold, WinPoint, RealBetPoint, RealBetGold, JPPoint, JPGold, JPConGold, JPConGoldOriginal, JPConPoint, JPConPointOriginal, OldQuota, NewQuota, Currency, ExCurrency, CryDef, IsDemo, IsSingleWallet, IsFreeGame, JPTxnId, IsBonusGame, IsJP, JPType, AddDate, IP, DBId, ClientType, Repair, roundID, JPPoolId, Denom, PlatformWid, CycleId, IsValid
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(@"./output/debug.xlsx"))
        {
            var sheet = package.Workbook.Worksheets.Add("分析log得到的");
            int count = 1;
            foreach (KeyValuePair<string, string> item in map)
            {
                string cellsKey = "A" + count.ToString();
                string cellsValue = "B" + count.ToString();

                sheet.Cells[cellsKey].Value = item.Key;
                sheet.Cells[cellsValue].Value = item.Value;

                count++;
            }

            // Save to file
            package.Save();
        }
    }

    /// <summary>
    /// 判斷沒讀到的欄位
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    private static Dictionary<string, bool> TotalLostFields(Dictionary<string, string> map)
    {
        Dictionary<string, bool> totalFields = new Dictionary<string, bool>() {
        {"Wid",false},
        {"Cid",false},
        {"UserName",false},
        {"UpId",false},
        {"HallId",false},
        {"Rid",false},
        {"ShoeNo",false},
        {"PlayNo",false},
        {"GGId",false},
        {"GameId",false},
        {"GameTypeId",false},
        {"Result",false},
        {"BetGold",false},
        {"BetPoint",false},
        {"WinGold",false},
        {"WinPoint",false},
        {"RealBetPoint",false},
        {"RealBetGold",false},
        {"JPPoint",false},
        {"JPGold",false},
        {"JPConGold",false},
        {"JPConGoldOriginal",false},
        {"JPConPoint",false},
        {"JPConPointOriginal",false},
        {"OldQuota",false},
        {"NewQuota",false},
        {"Currency",false},
        {"ExCurrency",false},
        {"CryDef",false},
        {"IsDemo",false},
        {"IsSingleWallet",false},
        {"IsFreeGame",false},
        {"JPTxnId",false},
        {"IsBonusGame",false},
        {"IsJP",false},
        {"JPType",false},
        {"AddDate",false},
        {"IP",false},
        {"DBId",false},
        {"ClientType",false},
        {"Wid_Parent",false},
        {"Repair",false},
        {"roundID",false},
        {"CreateTime",false},
        {"JPPoolId",false},
        {"Denom",false},
        {"PlatformWid",false},
        {"CycleId",false},
        {"IsValid",false},
        };

        foreach (KeyValuePair<string, string> item in map)
        {
            if (totalFields.ContainsKey(item.Key))
            {
                totalFields[item.Key] = true;
            }
        }


        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(@"./output/debug.xlsx"))
        {
            var sheet = package.Workbook.Worksheets.Add("目前沒有的欄位");
            int count = 1;
            foreach (KeyValuePair<string, bool> item in totalFields)
            {
                string cellsKey = "A" + count.ToString();
                string cellsValue = "B" + count.ToString();

                sheet.Cells[cellsKey].Value = item.Key;
                sheet.Cells[cellsValue].Value = item.Value;

                sheet.Cells[cellsKey].Style.Fill.PatternType = ExcelFillStyle.Solid; // 設定背景填色方法，沒有這一行就上背景色會報錯
                                                                                       // Solid = 填滿；另外還有斜線、交叉線、條紋等

                sheet.Cells[cellsValue].Style.Fill.PatternType = ExcelFillStyle.Solid; // 設定背景填色方法，沒有這一行就上背景色會報錯
                                                                                       // Solid = 填滿；另外還有斜線、交叉線、條紋等

                if (!item.Value)
                {
                    sheet.Cells[cellsKey].Style.Fill.BackgroundColor.SetColor(Color.DarkGray); // 儲存格顏色
                    sheet.Cells[cellsValue].Style.Fill.BackgroundColor.SetColor(Color.DarkGray); // 儲存格顏色
                }
                else
                {
                    sheet.Cells[cellsKey].Style.Fill.BackgroundColor.SetColor(Color.Green); // 儲存格顏色
                    sheet.Cells[cellsValue].Style.Fill.BackgroundColor.SetColor(Color.Green); // 儲存格顏色
                }

                count++;
            }

            // Save to file
            package.Save();
        }

        return totalFields;
    }
}
