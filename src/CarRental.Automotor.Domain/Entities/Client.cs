using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Automotor.Domain.Entities
{
    [Table("tb_client")]
    public class Client : Entity
    {
        protected Client()
        {
            
        }

        public Client(string name, int age, string document)
        {
            Name = name;
            Age = age;
            Document = document;
        }

        public Client(string name, int age, string document, int id) : base()
        {
            Id = id;
            Name = name;
            Age = age;
            Document = document;
        }

        [Required]
        [Column("name")]
        public string Name { get; private set; }

        [Required]
        [Column("age")]
        public int Age { get; private set; }

        [Required]
        [Column("document")]
        public string Document { get; private set; }
    }
}
