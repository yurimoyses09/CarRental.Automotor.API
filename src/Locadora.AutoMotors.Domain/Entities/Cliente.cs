using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locadora.AutoMotors.Domain.Entities
{
    [Table("tb_cliente")]
    public class Cliente : Entity
    {
        protected Cliente()
        {
            
        }

        public Cliente(string nome, int idade, string documento)
        {
            Nome = nome;
            Idade = idade;
            Documento = documento;
        }

        [Required]
        [Column("nome")]
        public string Nome { get; private set; }

        [Required]
        [Column("idade")]
        public int Idade { get; private set; }

        [Required]
        [Column("documento")]
        public string Documento { get; private set; }
    }
}
