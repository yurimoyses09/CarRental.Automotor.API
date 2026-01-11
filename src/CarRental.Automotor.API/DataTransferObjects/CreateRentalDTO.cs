using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class CreateRentalDTO
    {
        [Required(ErrorMessage = "The client ID is required.")]
        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "The automobile ID is required.")]
        [JsonPropertyName("automobile_id")]
        public int AutomobileId { get; set; }

        [Required(ErrorMessage = "The rental start date is required.")]
        [JsonPropertyName("start_date")]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessage = "The expected return date is required.")]
        [JsonPropertyName("expected_end_date")]
        public DateTimeOffset ExpectedEndDate { get; set; }

        [Required(ErrorMessage = "The daily rate is required.")]
        [JsonPropertyName("daily_rate")]
        public decimal DailyRate { get; set; }

        [Required(ErrorMessage = "The total rental amount is required.")]
        [JsonPropertyName("total_amount")]
        public decimal? TotalAmount { get; set; }

        [Required(ErrorMessage = "The rental status is required.")]
        [JsonPropertyName("status")]
        public RentalStatus Status { get; set; }
    }
}
