using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMCBA.Models
{
    public class Login
    {
        [Key]
        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "LoginID must be 8 characters")]
        public string LoginID { get; set; }

        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
