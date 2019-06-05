using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model{
    public class City
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage="The length of the city must be less than 50")]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [ForeignKey("Country")]
        public int countryId { get; set; }
    }
}