using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMCBA.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1000, 9999, ErrorMessage = "CustomerID must be 4 digits")]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string Name { get; set; }

        [StringLength(11, MinimumLength = 11)]
        public string? TFN { get; set; }

        [StringLength(50, ErrorMessage = "Address must not exceed 50 characters")]
        public string? Address { get; set; }

        [StringLength(40, ErrorMessage = "City must not exceed 40 characters")]
        public string? City { get; set; }

        [StringLength(3, MinimumLength = 2, ErrorMessage = "State must be 2 or 3 characters")]
        public string? State { get; set; }

        [StringLength(4, MinimumLength = 4, ErrorMessage = "Postcode must be 4 digits")]
        public string? Postcode { get; set; }

        [StringLength(12, MinimumLength = 12, ErrorMessage = "Mobile must be 12 characters")]
        public string? Mobile { get; set; }

        public virtual List<Account> Accounts { get; set; }
        public virtual Login Login { get; set; }
    }
}
