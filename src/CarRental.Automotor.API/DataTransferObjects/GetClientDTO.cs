using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class GetClientDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("document")]
        public string Document { get; set; }
    }
}
