using System.ComponentModel.DataAnnotations;

namespace apiUser.DAL.Model
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
    }
}
