using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Services
{
    public interface IStripeService
    {
        Task<string> AddCustomerToStripeAsync(string customerToken, string customerEmail);
        Task<string> GetCustomerCardDetailsAsync(string customerId);
        Task<bool> ProcessPayment(string customerToken, int amount, string email, string description);
    }
}
