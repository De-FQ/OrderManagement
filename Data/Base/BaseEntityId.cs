using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Data.Base
{
    public interface ISoftDelete
    {
        public bool Deleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public void Undo()
        {
            Deleted = false;
            DeletedAt = null;
        }
    }
    public partial class BaseId : ISoftDelete
    {
        [Key]
        public virtual int Id { get; set; }
        public Guid? Guid { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        [NotMapped]
        public string EncryptedGuid { get; set; }
    }
}
