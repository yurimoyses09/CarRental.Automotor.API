using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class GetAutomobileDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("ano_fabricacao")]
        public string YearManufacture { get; set; }

        [JsonPropertyName("plate")]
        public string Plate { get; set; }

        [JsonPropertyName("fuel_type")]
        public string FuelType { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}
