using System.ComponentModel.DataAnnotations;

namespace Luval.Security.Model.Views
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public string ConfirUserPassword { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
