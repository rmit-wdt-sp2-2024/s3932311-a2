using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMCBA.Models
{
    public class BillPay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int BillPayID { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual Account Account { get; set; }

        [Required]
        public int PayeeID { get; set; }

        [ForeignKey("PayeeID")]
        public virtual Payee Payee { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be in format >0.00")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ScheduleTimeUtc { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Period { get; set; }
    }
}
