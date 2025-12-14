using System.Text.Json.Serialization;

namespace Locadora.AutoMotors.API.DataTransferObjects
{
    public class GetClienteDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("idade")]
        public int Idade { get; set; }

        [JsonPropertyName("documento")]
        public string Documento { get; set; }
    }
}
