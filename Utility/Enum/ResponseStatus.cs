namespace Utility.Enum
{
    public enum ResponseStatus
    {
        /// <summary>
        /// Session Time out
        /// </summary>
        TokenExpired = 250,
        /// <summary>
        /// You are not authorized, Access right is denied
        /// </summary>
        NotAuthorized = 401,
        BadRequest = 400,
        NoToken = 0
    }
}
