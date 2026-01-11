using CarRental.Automotor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Automotor.Domain.Entities
{
    [Table("tb_rental")]
    public class Rental : Entity
    {
        protected Rental() { }

        public Rental(int clientId, int automobileId, DateTimeOffset startDate, DateTimeOffset expectedEndDate, DateTimeOffset? actualEndDate, decimal dailyRate, decimal? totalAmount, RentalStatus status)
        {
            ClientId = clientId;
            AutomobileId = automobileId;
            StartDate = startDate;
            ExpectedEndDate = expectedEndDate;
            ActualEndDate = actualEndDate;
            DailyRate = dailyRate;
            TotalAmount = totalAmount;
            Status = status;
        }

        public Rental(int clientId, int automobileId, DateTimeOffset startDate, DateTimeOffset expectedEndDate, DateTimeOffset? actualEndDate, decimal dailyRate, decimal? totalAmount, RentalStatus status, int id) : base()
        {
            Id = id;
            ClientId = clientId;
            AutomobileId = automobileId;
            StartDate = startDate;
            ExpectedEndDate = expectedEndDate;
            ActualEndDate = actualEndDate;
            DailyRate = dailyRate;
            TotalAmount = totalAmount;
            Status = status;
        }

        [Column("client_id")]
        [Required]
        public int ClientId { get; private set; }
        public Client Client { get; set; }

        [Column("automobile_id")]
        [Required]
        public int AutomobileId { get; private set; }
        public Automobile Automobile { get; set; }

        [Required]
        [Column("start_date")]
        public DateTimeOffset StartDate { get; private set; }

        [Required]
        [Column("expected_end_date")]
        public DateTimeOffset ExpectedEndDate { get; private set; }

        [Column("actual_end_date")]
        public DateTimeOffset? ActualEndDate { get; private set; }

        [Required]
        [Column("daily_rate")]
        public decimal DailyRate { get; private set; }

        [Required]
        [Column("total_amount")]
        public decimal? TotalAmount { get; private set; }

        [Required]
        [Column("rental_status")]
        public RentalStatus Status { get; private set; }
    }
}
