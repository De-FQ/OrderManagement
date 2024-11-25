namespace Data.Base
{
    public class JsonResultViewModel<T>  where T : class
    {
        public Int64 Id { get; set; }
        public string Message { get; set; }
        public string CurrentURL { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
    }
}
