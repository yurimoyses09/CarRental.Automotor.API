using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class ClientDTO
    {
        [Required(ErrorMessage = "The client's name is required.")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The client's age is required.")]
        [JsonPropertyName("age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "The client's document information is required.")]
        [JsonPropertyName("document")]
        public string Document { get; set; }
    }
}
