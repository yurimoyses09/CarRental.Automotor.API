using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Automotor.Domain.Entities
{
    public class Entity
    {
        protected Entity()
        {
            
        }

        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [Column("delete_at")]
        public DateTimeOffset DeleteAt { get; set; }
    }
}
