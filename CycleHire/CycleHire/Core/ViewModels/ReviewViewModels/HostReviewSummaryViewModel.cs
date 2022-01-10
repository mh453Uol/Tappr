using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ReviewViewModels
{
    public class HostReviewSummaryViewModel
    {
        public HostReviewSummaryViewModel(IEnumerable<HostReview> reviews)
        {
            Reviews = reviews;
            AggreatedReview = new HostReviewViewModel();
            Aggregate();
        }

        public HostReviewViewModel AggreatedReview { get; set; }
        public IEnumerable<HostReview> Reviews { get; set; }

        public void Aggregate()
        {
            double communication = 0;
            double accuracy = 0;
            double location = 0;
            double value = 0;
            var count = Reviews.Count();

            if (count > 0)
            {
                foreach (var review in Reviews)
                {
                    communication += review.ResponseAndCommunication;
                    accuracy += review.AccuracyOfListing;
                    location += review.Location;
                    value += review.Value;
                }

                communication = communication / count;
                accuracy = accuracy / count;
                location = location / count;
                value = value / count;
            }

            AggreatedReview.ResponseAndCommunication = (byte)Math.Round(communication);
            AggreatedReview.AccuracyOfListing = (byte)Math.Round(accuracy);
            AggreatedReview.Location = (byte)Math.Round(location);
            AggreatedReview.Value = (byte)Math.Round(value);
        }
    }
}
