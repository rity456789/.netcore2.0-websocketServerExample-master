using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketDemo.Model
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Column(TypeName = "character varying")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Column(TypeName = "character varying")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
