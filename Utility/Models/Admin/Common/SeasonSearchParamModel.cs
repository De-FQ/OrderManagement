namespace Utility.Models.Admin.Common
{
    public class SeasonSearchParamModel
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
    }
}
