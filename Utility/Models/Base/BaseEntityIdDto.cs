using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utility.Models.Base
{
    public partial class BaseEntityIdDto
    { 
        public virtual long Id { get; set; }
        public Guid? Guid { get; set; }
        /// <summary>
        /// for protection of the guid, when the data is sent to admin for list and edit
        /// this id will contain "protected Guid" of guid, when its reached to backed for update
        /// we have "unprotectGuid"
        /// </summary>
        public string EncryptedGuid { get; set; } 
    }
}
