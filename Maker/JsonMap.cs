﻿using System;
using System.Collections.Generic;

// C# 10 全新的 namespace 語法，不用再看到 namespace 的 { } 了！
namespace DocumentTemplateMaker.NET;

public class JsonMap
{
    public static void Maker(string tempFileName, string outputFileName, bool deleteAll = true)
    {
        // 讀檔案
        string text = System.IO.File.ReadAllText(tempFileName);
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
        System.IO.File.WriteAllText(outputFileName, newText);


        // 讀檔案
        string sqlText = System.IO.File.ReadAllText(".\\DocumentTemplate\\INSERT-WAGERS.sql");
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
        System.IO.File.WriteAllText(outputFileName, sqlText);
    }
}