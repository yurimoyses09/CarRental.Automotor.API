using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class UpdateAutomobileDTO
    {
        [JsonPropertyName("model")]
        public string Model { get; private set; }

        [JsonPropertyName("year_manufacture")]
        public string YearManufacture { get; private set; }

        [JsonPropertyName("plate")]
        public string Plate { get; private set; }

        [JsonPropertyName("fuel_type")]
        public string FuelType { get; private set; }

        [JsonPropertyName("color")]
        public string Color { get; private set; }
    }
}
