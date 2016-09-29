namespace BaseCms.Common.Binders
{
    public class JDataTableSettings
    {
        public string Echo { get; set; }
        public string ColumnsStr { get; set; }
        public int DisplayStart { get; set; }
        public int DisplayLength { get; set; }
        public int[] DataProperties { get; set; }
        public string Search { get; set; }
        public bool Regex { get; set; }
        public string[] ColumnSearch { get; set; }
        public bool[] ColumnSearchable { get; set; }
        public bool[] ColumnRegex { get; set; }
        public int[] SortIndexes { get; set; }
        public string[] SortDirections { get; set; }
        public bool[] ColumnSortable { get; set; }

        public JDataTableSettings()
        {
            DataProperties = new int[] { };
            ColumnSearch = new string[] { };
            ColumnSearchable = new bool[] { };
            ColumnRegex = new bool[] { };
            SortIndexes = new int[] { };
            SortDirections = new string[] { };
            ColumnSortable = new bool[] { };
        }
    }
}
