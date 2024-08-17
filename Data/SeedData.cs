using Microsoft.EntityFrameworkCore;
using MvcMCBA.Data;
using Newtonsoft.Json;

namespace MvcMCBA.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMCBAContext(
                serviceProvider.GetRequiredService<DbContextOptions<MvcMCBAContext>>()))
            {
                if (context.Customer.Any())
                {
                    return;   
                }

                List<Customer> customerList = await LoadWebData<List<Customer>>("http://ec2-54-153-142-25.ap-southeast-2.compute.amazonaws.com/services/customers/");

                foreach (var customer in customerList)
                {
                    var newCustomer = new Customer
                    {
                        CustomerID = customer.CustomerID,
                        Name = customer.Name,
                        Address = customer.Address,
                        City = customer.City,
                        Postcode = customer.Postcode,
                        Accounts = new List<Account>()
                    };

                    context.Customer.Add(newCustomer);
                }

                await context.SaveChangesAsync();

                foreach (var customer in customerList)
                {
                    var newLogin = new Login
                    {
                        LoginID = customer.Login.LoginID,
                        PasswordHash = customer.Login.PasswordHash,
                        CustomerID = customer.CustomerID 
                    };

                    context.Login.Add(newLogin);
                }

                await context.SaveChangesAsync();

                foreach (var customer in customerList)
                {
                    foreach (var account in customer.Accounts)
                    {
                        var newAccount = new Account
                        {
                            AccountNumber = account.AccountNumber, 
                            AccountType = account.AccountType,
                            CustomerID = customer.CustomerID, 
                            Transactions = new List<Transaction>()
                        };

                        context.Account.Add(newAccount);

                        foreach (var transaction in account.Transactions)
                        {
                            var newTransaction = new Transaction
                            {
                                TransactionType = "D",
                                Amount = transaction.Amount,
                                TransactionTimeUtc = transaction.TransactionTimeUtc,
                                AccountNumber = account.AccountNumber 
                            };

                            context.Transaction.Add(newTransaction);
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        // Get Json data and deserialize using generics
        public static async Task<T> LoadWebData<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    T result = JsonConvert.DeserializeObject<T>(jsonResponse);

                    return result;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return default;
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"JSON error: {e.Message}");
                    return default;
                }
            }
        }
    }
}
