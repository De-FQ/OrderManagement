namespace Utility.Models.Base
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        //public string AniforgeryToken { get; set; }
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

       
    }
}
