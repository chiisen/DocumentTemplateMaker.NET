using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentTemplateMaker.NET
{
    public class Replace
    {
        public void Maker(string tempFileName, string outputFileName, Parameter para)
        {
            // 讀檔案
            string text = System.IO.File.ReadAllText(tempFileName);

            // 砍擋
            Helper helper_ = new Helper();
            helper_.DeleteAll(".\\output\\");

            if (para.KeyWords.Length != para.ReplaceWords.Length)
            {
                Console.WriteLine("[ERROR] 參數數量不相等!");
                return;
            }

            for(int i = 0; i < para.KeyWords.Length; ++i)
            {
                text = text.Replace(para.KeyWords[i], para.ReplaceWords[i]);
            }

            // 寫檔案
            System.IO.File.WriteAllText(outputFileName, text);
        }
    }
}
