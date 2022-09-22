using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordsManager.Model
{
    public class UserAccount
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Password { get; set; }

        [InverseProperty(nameof(PasswordUserAccount.User))]
        public List<PasswordUserAccount> PasswordsBelong { get; set; }

        [InverseProperty(nameof(Tag.User))]
        public List<Tag> TagsBelong { get; set; }
    }
}
