using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class CreateAutomobileDTO
    {
        [Required(ErrorMessage = "The car model is required.")]
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "The car manufacturing year is required.")]
        [JsonPropertyName("year_manufacture")]
        [MaxLength(4, ErrorMessage = "The manufacturing year must contain 4 digits.")]
        public string YearManufacture { get; set; }

        [Required(ErrorMessage = "The car plate number is required.")]
        [JsonPropertyName("plate")]
        public string Plate { get; set; }

        [Required(ErrorMessage = "The fuel type is required.")]
        [JsonPropertyName("fuel_type")]
        public string FuelType { get; set; }

        [Required(ErrorMessage = "The car color is required.")]
        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}
