using System.ComponentModel.DataAnnotations;

namespace PasswordsManager.Model
{
    public class Parameter
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
