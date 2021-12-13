using System.IO;

namespace DocumentTemplateMaker.NET
{
    public class Helper
    {
        public void DeleteAll(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
