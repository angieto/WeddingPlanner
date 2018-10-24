using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Wedding 
    {
        [Key]
        public int WeddingId {get; set;}

        [Required(ErrorMessage = "Wedder One can't be blanked")]
        [MinLength(5, ErrorMessage = "Full name is required")]
        public string Wedder1 {get; set;}
        
        [Required(ErrorMessage = "Wedder Two can't be blanked")]
        [MinLength(5, ErrorMessage = "Full name is required")]
        public string Wedder2 {get; set;}

        [Required(ErrorMessage = "Missing wedding date!")]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date {get; set;}

        [Required(ErrorMessage = "Missing wedding address!")]
        [MinLength(5, ErrorMessage = "Invalid address")]
        public string Address {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        // RSVP Confirmation
        public List<RSVP> RSVPs {get; set;}

        public int UserId {get; set;}
        [ForeignKey("UserId")]
        public User Creator {get; set;}
    }
}