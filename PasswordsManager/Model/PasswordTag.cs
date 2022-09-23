using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordsManager.Model
{
    public class PasswordTag
    {


        [ForeignKey(nameof(Password))]
        public int PasswordId { get; set; }
        public Password Password { get; set; }

        [ForeignKey(nameof(Tag))]
        public string TagLabel { get; set; }
        public Tag Tag { get; set; }
    }
}
