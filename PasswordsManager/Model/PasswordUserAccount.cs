using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordsManager.Model
{
    public class PasswordUserAccount
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //Foreign key
        public int UserId { get; set; }
        public int PasswordId { get; set; }
        //Foreign key



        

        [Required, ForeignKey(nameof(UserId))]
        public UserAccount User { get; set; }

        [Required,ForeignKey(nameof(PasswordId))]
        public Password Password { get; set; }

        
    }
}
