using Microsoft.EntityFrameworkCore;
using MvcMCBA.Models;

namespace MvcMCBA.Data
{
    public class MvcMCBAContext : DbContext
    {
        public MvcMCBAContext(DbContextOptions<MvcMCBAContext> options) : base(options)
        { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<Payee> Payee { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasKey(c => c.CustomerID);

            builder.Entity<Account>()
                .HasKey(a => a.AccountNumber);

            builder.Entity<Transaction>()
                .HasKey(t => t.TransactionID);

            builder.Entity<Login>()
                .HasKey(l => l.LoginID);

            builder.Entity<BillPay>()
                .HasKey(b => b.BillPayID);

            builder.Entity<Payee>()
                .HasKey(p => p.PayeeID);

            builder.Entity<Account>(entity =>
            {
                entity.HasMany(a => a.Transactions)
                    .WithOne(t => t.Account)
                    .HasForeignKey(t => t.AccountNumber)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.BillPays)
                    .WithOne(b => b.Account)
                    .HasForeignKey(b => b.AccountNumber)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Customer>(entity =>
            {
                entity.HasMany(c => c.Accounts)
                    .WithOne(a => a.Customer)
                    .HasForeignKey(a => a.CustomerID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Login)
                    .WithOne(l => l.Customer)
                    .HasForeignKey<Login>(l => l.CustomerID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BillPay>(entity =>
            {
                entity.HasOne(b => b.Payee)
                    .WithMany(p => p.BillPays)
                    .HasForeignKey(b => b.PayeeID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Account)
                    .WithMany(a => a.BillPays)
                    .HasForeignKey(b => b.AccountNumber)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(b => b.Amount)
                    .HasColumnType("decimal(18,2)");
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.Property(t => t.Amount)
                    .HasColumnType("decimal(18,2)");
            });
        }
    }
}
