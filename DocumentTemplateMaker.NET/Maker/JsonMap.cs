using System;
using System.Collections.Generic;

namespace DocumentTemplateMaker.NET
{
    public class JsonMap
    {
        public static void Maker(string tempFileName, string outputFileName)
        {
            // 讀檔案
            string text = System.IO.File.ReadAllText(tempFileName);
            text = text.Replace(" ", "");
            text = text.Replace("\r\n", "");

            // 砍擋
            Helper.DeleteAll(".\\output\\");

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

                    string[] mapLine_ = newLine_.Split(':');
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
            string sqlText = System.IO.File.ReadAllText(".\\DocumentTemplate\\INSERT.sql");
            foreach (KeyValuePair<string, string> item in map_)
            {
                string keyword_ = string.Format("#{0}#", item.Key);
                sqlText = sqlText.Replace(keyword_, item.Value);
            }

            // 寫檔案
            System.IO.File.WriteAllText(".\\output\\INSERT-1.sql", sqlText);
        }
    }
}
