namespace Utility.API
{
    public class DataTableResult<T> where T : new()
    {
        public string Draw { get; set; } = string.Empty;
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public T Data { get; set; } 
        public Exception Error { get; set; }
        public object AdditionalData { get; set; }
    }
}
