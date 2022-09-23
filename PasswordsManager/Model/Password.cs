using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordsManager.Model
{
    public class Password
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Login { get; set; }

        [Required]
        public string Pass { get; set; }

        [InverseProperty(nameof(PasswordTag.Password))]
        public List<PasswordTag> PasswordTagsBelong { get; set; }

        [InverseProperty(nameof(PasswordUserAccount.Password))]
        public PasswordUserAccount UsersBelong { get; set; }

    }
}
