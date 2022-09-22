using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordsManager.Model
{
    public class Tag
    {
        [Key]
        public string Label { get; set; }
        public int UserId { get; set; }
        [InverseProperty(nameof(PasswordTag.Tag))]
        public List<PasswordTag> PasswordsBelong { get; set; }

        [Required,ForeignKey(nameof(UserId))]
        public UserAccount User { get; set; }
        
    }
}
