using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Base
{
    public partial class BaseDate : BaseId
    {
        public long? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        #region Not mapped

        [NotMapped]
        public string FormattedCreatedOn { get; set; }

        [NotMapped]
        public string FormattedModifiedOn { get; set; }

        [NotMapped]
        public string CreatedByName { get; set; }

        [NotMapped]
        public string ModifiedByName { get; set; }
        #endregion
    }
}
