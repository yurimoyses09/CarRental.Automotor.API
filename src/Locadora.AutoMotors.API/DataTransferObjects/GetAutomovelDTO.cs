using System.Text.Json.Serialization;

namespace Locadora.AutoMotors.API.DataTransferObjects
{
    public class GetAutomovelDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("modelo")]
        public string Modelo { get; set; }

        [JsonPropertyName("ano_fabricacao")]
        public string AnoFabricacao { get; set; }

        [JsonPropertyName("placa")]
        public string Placa { get; set; }

        [JsonPropertyName("combustivel")]
        public string Combustivel { get; set; }

        [JsonPropertyName("cor")]
        public string Cor { get; set; }
    }
}
