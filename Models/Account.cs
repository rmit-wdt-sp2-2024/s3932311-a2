using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMCBA.Models
{
    public class Account
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1000, 9999, ErrorMessage = "AccountNumber must be 4 digits")]
        public int AccountNumber { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string AccountType { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<BillPay> BillPays { get; set; }
    }
}
