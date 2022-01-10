using CycleHire.Core.ViewModels.ListingViewModels;
using CycleHire.Utilites.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace CycleHire.Core.ViewModels.BookingViewModels
{
    public class BookingFormViewModel
    {
        public ListingDetailsViewModel Listing { get; set; }

        [Required]
        public String MessageToOwner { get; set; }

        public int DurationInDays { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PricePerDay { get; set; }

        //SubTotal holds PricePerDay * Duration
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal SubTotal { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal StripeTransactionFees { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal OurServiceFees { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(32)]
        public string UserMobileNumber { get; set; }

        public string UserEmailAddress { get; set; }

        [Required(ErrorMessage = "Your card details are invalid")]
        public String StripeToken { get; set; }

        public void CalculateSubTotal()
        {
            GetDuration();
            SubTotal = PricePerDay * DurationInDays;
        }

        public void CalculateStripeTransactionFees()
        {
            //1.4%
            decimal StripeEuropeTransactionFeePercentage = 1.4M;
            //0.20p
            decimal StripeOneOffCost = 0.20M;

            StripeTransactionFees = ((SubTotal / 100M) * StripeEuropeTransactionFeePercentage) + StripeOneOffCost;
        }

        public void CalculateOurServiceFees()
        {
            //15%
            decimal ServiceFeePercentage = 15M;
            OurServiceFees = (SubTotal / 100) * ServiceFeePercentage;
        }

        public void CalculateTotalPrice()
        {
            GetDuration();
            CalculateSubTotal();
            CalculateStripeTransactionFees();
            CalculateOurServiceFees();

            TotalPrice = SubTotal + StripeTransactionFees + OurServiceFees;
        }

        public void GetDuration()
        {
            DurationInDays = (Listing.To.Value - Listing.From.Value).Days + 1;
        }

    }
}
