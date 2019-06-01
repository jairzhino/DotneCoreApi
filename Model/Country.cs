
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Model{
    public class Country
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage="The country name can not be null")]
        [StringLength(50,ErrorMessage="The length of the country must be less than 50")]
        public string name { get; set; }

        public ICollection<City> cities { get; set; }
    }
}