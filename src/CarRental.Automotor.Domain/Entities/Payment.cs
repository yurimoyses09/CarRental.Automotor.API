using CarRental.Automotor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Automotor.Domain.Entities
{
    [Table("tb_payment")]
    public class Payment : Entity
    {
        protected Payment() { }

        public Payment(int rentalId, decimal amount)
        {
            RentalId = rentalId;
            Amount = amount;
            Status = PaymentStatus.Pending;
        }

        [Required]
        [Column("rental_id")]
        public int RentalId { get; private set; }
        public Rental Rental { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; private set; }

        [Required]
        [Column("payment_status")]
        public PaymentStatus Status { get; private set; }

        [Column("paid_at")]
        public DateTimeOffset? PaidAt { get; private set; }

        public void Approve()
        {
            Status = PaymentStatus.Approved;
            PaidAt = DateTimeOffset.UtcNow;
        }
    }
}
