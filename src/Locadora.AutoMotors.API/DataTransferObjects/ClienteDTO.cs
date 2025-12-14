using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Locadora.AutoMotors.API.DataTransferObjects
{
    public class ClienteDTO
    {
        [Required(ErrorMessage = "É necessário informar o nome do cliente.")]  
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [Required]
        [JsonPropertyName("idade")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Precisamos da informação do documento do cliente.")]
        [JsonPropertyName("documento")]
        public string Documento { get; set; }
    }
}
