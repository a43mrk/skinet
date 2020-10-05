using System.ComponentModel.DataAnnotations;

// 163-2
namespace Core.Entities.Identity
{
    public class Address
    {
        public int Id { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        // strings can be nullable at database to avoid being nullable use [Required]
        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { set; get; }
    }
}