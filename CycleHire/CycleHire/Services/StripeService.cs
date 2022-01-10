using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Services
{
    public class StripeService : IStripeService
    {
        public async Task<string> AddCustomerToStripeAsync(string customerToken, string customerEmail)
        {
            var customerService = new StripeCustomerService();

            var customer = await customerService.CreateAsync(new StripeCustomerCreateOptions
            {
                Email = customerEmail,
                SourceToken = customerToken
            });

            return customer.Id;
        }

        public async Task<string> GetCustomerCardDetailsAsync(string customerId)
        {
            var customerService = new StripeCustomerService();

            var customer = await customerService.GetAsync(customerId);

            return customer.Sources.Data[0].Card.Last4;
        }

        public async Task<bool> ProcessPayment(string customerToken, int amount, string email, string description)
        {
            var chargeService = new StripeChargeService();

            try
            {
                var charge = await chargeService.CreateAsync(new StripeChargeCreateOptions
                {
                    Amount = amount,
                    Description = description,
                    Currency = "GBP",
                    CustomerId = customerToken,
                    ReceiptEmail = email
                });
            }
            catch (StripeException e)
            {
                if (e.StripeError.ErrorType == "card_error")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
