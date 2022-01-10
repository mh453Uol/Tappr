using System;
using System.ComponentModel.DataAnnotations;

namespace CycleHire.Core.Models
{
    public class Booking : Audit
    {
        public Booking()
        {
            Status = BookingStatus.PENDING;
        }

        public Guid Id { get; set; }

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public HostReview HostReview { get; set; }
        public TenantReview TenantReview { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        public int DurationInDays { get; set; }

        public decimal PricePerDay { get; set; }

        //SubTotal holds PricePerDay * Duration
        public decimal SubTotal { get; set; }
        public decimal StripeTransactionFees { get; set; }
        public decimal OurServiceFees { get; set; }

        //Total Price includes SubTotal + StripeTransactionFees + OurServiceFees
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(3000), MinLength(100)]
        public string UserMessage { get; set; }

        [MaxLength(3000)]
        public string OwnerDeclinedMessage { get; set; }

        [Required]
        [MaxLength(32)]
        public string UserMobileNumber { get; set; }
        public BookingStatus Status { get; set; }
        //Stripe Information
        public String StripeCustomerIdToken { get; set; }

        public void CalculateSubTotal()
        {
            SubTotal = DurationInDays * PricePerDay;
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
            CalculateDuration();
            CalculateSubTotal();
            CalculateStripeTransactionFees();
            CalculateOurServiceFees();

            TotalPrice = SubTotal + StripeTransactionFees + OurServiceFees;
        }
        public void CalculateDuration()
        {
            DurationInDays = (To - From).Days + 1;
        }

        //Host Approves
        public void Approve()
        {
            if (Status == BookingStatus.PENDING)
            {
                Status = BookingStatus.ACCEPTED;
            }
        }

        //Host Declines
        public void Decline(string message)
        {
            if (Status == BookingStatus.PENDING)
            {
                Status = BookingStatus.DECLINED;
            }

            if (!String.IsNullOrWhiteSpace(message))
            {
                OwnerDeclinedMessage = message;
            }

        }

        //Tenant Cancels
        public void CancelByTenant()
        {
            if (Status == BookingStatus.PENDING)
            {
                Status = BookingStatus.CANCELLEDBYHOST;
            }
        }

        public bool IsOwnerOfBooking(string ownerId)
        {
            return OwnerId == ownerId;
        }

        public bool IsNotExpired()
        {
            return From >= DateTime.Now.Date;
        }
    }
}
