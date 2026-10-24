using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class CreatePaymentDTO
    {
        [Required(ErrorMessage = "The rental ID is required.")]
        [JsonPropertyName("rental_id")]
        public int RentalId { get; set; }

        [Required(ErrorMessage = "The payment amount is required.")]
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
