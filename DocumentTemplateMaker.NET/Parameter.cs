namespace DocumentTemplateMaker.NET
{
    /// <summary>
    /// 儲存參數內容的類別
    /// </summary>
    public class Parameter
    {
        public string TempType;
        public string[] SrcTempPath;
        public string[] OutputPath;
        public string StartDate;
        public string EndDate;
        public string[] KeyWords;
        public string[] ReplaceWords;
        public int Offset;
        public string OffsetUnit;
    }

}
