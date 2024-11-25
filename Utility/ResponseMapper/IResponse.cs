namespace Utility.ResponseMapper
{
    public class IResponse<T> where T : new()
    {
        public IResponse()
        {
            Data = new T();
        }

        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } 
        public int MessageCode { get; set; }

    }
}