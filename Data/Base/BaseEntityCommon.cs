namespace Data.Base
{
    public partial class BaseCommon : BaseDate
    {
        public long DisplayOrder { get; set; } = 0;
        public bool Active { get; set; } = false;
    }
}
