using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _95PhrEAKer.Persistence.DbModals
{
    public partial class Notifications
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")] // or "text" if you want long text
        public string UserId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Message { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Type { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
