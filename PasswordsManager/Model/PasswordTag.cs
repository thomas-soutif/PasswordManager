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
        public int PasswordId { get; set; }

        public string TagLabel { get; set; }

        [ForeignKey(nameof(PasswordId))]
        public Password Password { get; set; }

        [ForeignKey(nameof(TagLabel))]
        public Tag Tag { get; set; }
    }
}
