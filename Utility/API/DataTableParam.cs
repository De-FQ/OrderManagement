namespace Utility.API
{
    public class DataTableParam 
    {   
        public string Draw { get; set; } = string.Empty;
        public bool IsEnglish { get; set; }
        public string SearchValue { get; set; } = string.Empty;
        public string SortColumn { get; set; } = string.Empty;
        public string SortColumnDirection { get; set; } = string.Empty;        
        public int Skip { get; set; }
        public int PageSize { get; set; }

    }
}
