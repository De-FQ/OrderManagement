namespace Utility.Models.Base
{
    public partial class BaseEntityCommonDto : BaseEntityDateDto
    {
        public long DisplayOrder { get; set; } = 0;
        public bool Active { get; set; } = false;
    }
}
