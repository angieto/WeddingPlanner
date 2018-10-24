using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class RSVP
    {
        [Key]
        public int Id {get; set;}

        public int UserId {get; set;}
        [ForeignKey("UserId")]
        public User User {get; set;}

        public int WeddingId {get; set;}
        [ForeignKey("WeddingId")]
        public Wedding Wedding {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}