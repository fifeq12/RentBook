using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentBook.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole nazwa jest wymagane.")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość wynosi 50 znaków.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pole autor jest wymagane.")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość wynosi 50 znaków.")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Pole data wydania jest wymagane.")]
        public int ReleaseDate { get; set; }
        [MaxLength(150, ErrorMessage = "Maksymalna długość wynosi 150 znaków.")]
        public string Description { get; set; }
    }
}
