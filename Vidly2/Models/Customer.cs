using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vidly2.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]  //makes it not nullable in the db
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public MembershipType MembershipType { get; set; }  //navigation prop: allows us to nav from one type to another

        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; } //entity framework recognizes this convention and treats this as foreign key
        
        //[Display(Name="Date of Birth")]
        public DateTime? Birthdate { get; set; }
    }
}