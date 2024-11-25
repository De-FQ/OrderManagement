using API.Resources;

namespace API.Helpers
{
    public static class ResourceHelper
    {
        public static string GetResource(string name, bool isEnglish)
        {
            string  value;

            if (isEnglish)
            {
                value = SharedResourceEn.ResourceManager.GetString(name);
            }
            else
            {
                value = SharedResourceAr.ResourceManager.GetString(name);
            }

            if (!string.IsNullOrEmpty(value))
            {
                 return value;
            } 
             

            return name;
        }
    }
}
