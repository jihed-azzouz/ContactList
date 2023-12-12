using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactList.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string EmailAddress { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Select category")]
        public int CategoryId { get; set; }


        [NotMapped]
        public Cat CatName { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string Dash => $"{FirstName}-{LastName}".ToLower().Replace(" ", "-");
        
    }
}
