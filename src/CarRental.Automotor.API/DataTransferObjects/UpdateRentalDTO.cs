using CarRental.Automotor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class UpdateRentalDTO
    {
        [Required(ErrorMessage = "The client ID is required.")]
        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "The automobile ID is required.")]
        [JsonPropertyName("automobile_id")]
        public int AutomobileId { get; set; }

        [JsonPropertyName("start_date")]
        public DateTimeOffset StartDate { get; set; }

        [JsonPropertyName("expected_end_date")]
        public DateTimeOffset ExpectedEndDate { get; set; }

        [JsonPropertyName("daily_rate")]
        public decimal DailyRate { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal? TotalAmount { get; set; }

        [JsonPropertyName("status")]
        public RentalStatus Status { get; set; }
    }
}
