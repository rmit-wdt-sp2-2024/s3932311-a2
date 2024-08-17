using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMCBA.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int TransactionID { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string TransactionType { get; set; }

        [Required]
        [Range(1000, 9999)]
        public int AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual Account Account { get; set; }

        public int? DestinationAccountNumber { get; set; }

        [ForeignKey("DestinationAccountNumber")]
        public virtual Account DestinationAccount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be in format >0.00")]
        public decimal Amount { get; set; }

        [StringLength(30)]
        public string? Comment { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TransactionTimeUtc { get; set; }
    }
}
