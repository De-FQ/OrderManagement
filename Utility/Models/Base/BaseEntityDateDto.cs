using System.ComponentModel.DataAnnotations.Schema;

namespace Utility.Models.Base
{
    public partial class BaseEntityDateDto : BaseEntityIdDto
    {
        public long? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Deleted { get; set; } = false;

        #region Not mapped
        /// <summary>
        /// Display formatted text with createdBy and createdDate info
        /// </summary>
        [NotMapped]
        public string FormattedCreatedOn { get; set; }
        /// <summary>
        /// Display formatted text with modifiedBy and modifiedDate info
        /// </summary>
        [NotMapped]
        public string FormattedModifiedOn { get; set; }

        [NotMapped]
        public int UsersCount { get; set; }

        [NotMapped]
        public string Type { get; set; }
        #endregion
    }
}
