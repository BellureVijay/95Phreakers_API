using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _95PhrEAKer.Persistence.DbModals
{
    [Table("Posts")]
    public class Posts
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "text")] // Use 'text' to avoid longtext unless you expect large messages
        public string Message { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users Users { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
