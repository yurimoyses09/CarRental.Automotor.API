using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locadora.AutoMotors.Domain.Entities
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
        [Column("dt_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Column("dt_alteracao")]
        public DateTime DataAlteracao { get; set; }
    }
}
