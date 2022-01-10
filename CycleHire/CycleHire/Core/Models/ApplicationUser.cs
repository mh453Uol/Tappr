using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CycleHire.Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Listings = new List<Listing>();
            HostReviews = new List<HostReview>();
            TenantReviews = new List<TenantReview>();
            Routes = new List<Node>();
        }

        [Required]
        [MaxLength(255)]
        [MinLength(2)]
        public String Firstname { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(2)]
        public String Surname { get; set; }

        [MaxLength(1000)]
        public String AboutMe { get; set; }

        public String StripeUserId { get; set; }
        public String StripePublishableKeys { get; set; }
        public String StripeAccessToken { get; set; }
        public String StripeRefreshToken { get; set; }

        [MaxLength(450)]
        public String ProfileImageId { get; set; }

        public ICollection<Listing> Listings { get; set; }
        public ICollection<HostReview> HostReviews { get; set; }
        public ICollection<TenantReview> TenantReviews { get; set; }

        public ICollection<Node> Routes { get; set; }


        public void ConnectHostToStripe(String userId, String publishableKeys,
            String accessToken, String refreshToken)
        {

            this.StripeUserId = userId;
            this.StripePublishableKeys = publishableKeys;
            this.StripeAccessToken = accessToken;
            this.StripeRefreshToken = refreshToken;
        }

        public bool IsHostStripeConnected()
        {
            return !String.IsNullOrEmpty(StripeUserId) &
                !String.IsNullOrEmpty(StripePublishableKeys) &&
                !String.IsNullOrEmpty(StripeAccessToken) &&
                !String.IsNullOrEmpty(StripeRefreshToken);
        }

        public string FullName()
        {
            return String.Format("{0} {1}", Firstname, Surname);
        }

    }
}
