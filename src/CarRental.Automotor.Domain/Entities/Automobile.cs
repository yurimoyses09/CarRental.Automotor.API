using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Automotor.Domain.Entities
{
    [Table("tb_automobile")]
    public class Automobile : Entity
    {
        protected Automobile()
        {
            
        }

        public Automobile(string model, string yearManufacture, string plate, string fuelType, string color)
        {
            Model = model;
            YearManufacture = yearManufacture;
            Plate = plate;
            FuelType = fuelType;
            Color = color;
        }

        public Automobile(string model, string yearManufacture, string plate, string fuelType, string color, int id) : base()
        {
            Id = id;
            Model = model;
            YearManufacture = yearManufacture;
            Plate = plate;
            FuelType = fuelType;
            Color = color;
        }


        [Required]
        [Column("model")]
        public string Model { get; private set; }

        [Required]
        [Column("year_manufacture")]
        public string YearManufacture { get; private set; }

        [Required]
        [Column("plate")]
        public string Plate { get; private set; }

        [Required]
        [Column("fuel_type")]
        public string FuelType { get; private set; }

        [Required]
        [Column("color")]
        public string Color { get; private set; }
    }
}
