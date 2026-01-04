using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Locadora.AutoMotors.API.DataTransferObjects
{
    public class CreateAutomovelDTO
    {
        [Required(ErrorMessage = "É necessário informar o modelo do carro.")]
        [JsonPropertyName("modelo")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "É necessário informar o ano de fabricação do carro.")]
        [JsonPropertyName("ano_fabricacao")]
        [MaxLength(4)]
        public string AnoFabricacao { get; set; }

        [Required(ErrorMessage = "É necessário informar a placa do carro.")]
        [JsonPropertyName("placa")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "É necessário informar o tipo de combustivél.")]
        [JsonPropertyName("combustivel")]
        public string Combustivel { get; set; }

        [Required(ErrorMessage = "É necessário informar a cor do carro.")]
        [JsonPropertyName("cor")]
        public string Cor { get; set; }
    }
}
