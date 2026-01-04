using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locadora.AutoMotors.Domain.Entities
{
    [Table("tb_automovel")]
    public class Automovel : Entity
    {
        protected Automovel()
        {
            
        }

        public Automovel(string modelo, string anoFabricacao, string placa, string combustivel, string cor)
        {
            Modelo = modelo;
            AnoFabricacao = anoFabricacao;
            Placa = placa;
            Combustivel = combustivel;
            Cor = cor;
        }

        [Required]
        [Column("modelo")]
        public string Modelo { get; private set; }

        [Required]
        [Column("ano_fabricacao")]
        public string AnoFabricacao { get; private set; }

        [Required]
        [Column("placa")]
        public string Placa { get; private set; }

        [Required]
        [Column("combustivel")]
        public string Combustivel { get; private set; }

        [Required]
        [Column("cor")]
        public string Cor { get; private set; }
    }
}
