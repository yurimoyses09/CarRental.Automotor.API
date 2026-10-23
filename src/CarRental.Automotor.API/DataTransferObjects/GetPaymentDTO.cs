using CarRental.Automotor.Domain.Enums;
using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class GetPaymentDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("rental_id")]
        public int RentalId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("status")]
        public PaymentStatus Status { get; set; }

        [JsonPropertyName("paid_at")]
        public DateTimeOffset? PaidAt { get; set; }
    }
}
