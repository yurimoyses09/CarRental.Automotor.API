using CarRental.Automotor.Domain.Enums;

namespace CarRental.Automotor.API.DataTransferObjects
{
    public class GetRentalDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int AutomobileId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset ExpectedEndDate { get; set; }

        public DateTimeOffset? ActualEndDate { get; set; }

        public decimal DailyRate { get; set; }

        public decimal? TotalAmount { get; set; }

        public RentalStatus Status { get; set; }
    }
}
