using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class UpdateAutomobileDTO
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("year_manufacture")]
        public string YearManufacture { get; set; }

        [JsonPropertyName("plate")]
        public string Plate { get; set; }

        [JsonPropertyName("fuel_type")]
        public string FuelType { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}
