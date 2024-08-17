using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMCBA.Models
{
    public class Payee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int PayeeID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Address must not exceed 50 characters")]
        public string Address { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "City must not exceed 40 characters")]
        public string City { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "State must be 2 or 3 letters")]
        public string State { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Postcode must be 4 digits")]
        public string Postcode { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 14)]
        public string Phone { get; set; }

        public virtual List<BillPay> BillPays { get; set; }
    }
}
