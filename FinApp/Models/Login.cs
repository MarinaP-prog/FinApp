using System.ComponentModel.DataAnnotations;

namespace FinApp.Models {
    public class Login {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public bool Remember { get; set; }
    }
}
