using CycleHire.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CycleHire.UnitTests
{
    [TestClass]
    public class BookingTest
    {
        [TestMethod]
        public void CalculateDuration_ForDates1WeekApart_Returns8Days()
        {
            //Arrange
            var booking = new Booking()
            {
                From = new DateTime(2018, 4, 4),
                To = new DateTime(2018, 4, 11)
            };

            //Act
            booking.CalculateDuration();

            var days = booking.DurationInDays;

            //Assert
            //returns 8 days since when booking start day is included
            Assert.IsTrue(days == 8);
        }

        [TestMethod]
        public void CalculateSubTotal_ForListingOf5PerDayFor1Week_Returns40Pounds()
        {
            //Arrange
            var booking = new Booking() { PricePerDay = 5, DurationInDays = 8 };

            //Act
            booking.CalculateSubTotal();

            var subTotal = booking.SubTotal;

            //Assert
            Assert.IsTrue(subTotal == 40);
        }

        [TestMethod]
        public void CalculateOurServiceFee_ForListingOf5PerDayFor1Week_Returns6Pounds()
        {
            //Arrange
            var booking = new Booking() { PricePerDay = 5, DurationInDays = 8 };

            //Act
            booking.CalculateSubTotal();
            booking.CalculateOurServiceFees();

            var serviceFee = booking.OurServiceFees;

            //Assert
            Assert.IsTrue(serviceFee == 6);
        }

        [TestMethod]
        public void CalculateOurStripeTransactionFee_ForListingOf5PerDayFor1Week_Returns76p()
        {
            //Arrange
            var booking = new Booking() { PricePerDay = 5, DurationInDays = 8 };

            //Act
            booking.CalculateSubTotal();
            booking.CalculateStripeTransactionFees();

            var serviceFee = booking.StripeTransactionFees;

            //Assert
            Assert.IsTrue(serviceFee == 0.76M);
        }

        [TestMethod]
        public void CalculateTotalPrice_ForListingOf5PerDayFor1Week_Returns46Pounds76p()
        {
            //Arrange
            var booking = new Booking()
            {
                PricePerDay = 5,
                From = new DateTime(2018, 4, 4),
                To = new DateTime(2018, 4, 11)
            };

            //Act
            booking.CalculateTotalPrice();

            var totalCost = booking.TotalPrice;

            //Assert
            Assert.IsTrue(totalCost == 46.76M);
        }

        [TestMethod]
        public void Approve_WhenBookingIsCancelled_StatusIsNotChanged()
        {
            //Arrange
            var booking = new Booking()
            {
                PricePerDay = 5,
                From = new DateTime(2018, 4, 4),
                To = new DateTime(2018, 4, 11),
                Status = BookingStatus.CANCELLEDBYHOST
            };

            //Act
            booking.Approve();

            //Assert
            Assert.IsTrue(booking.Status == BookingStatus.CANCELLEDBYHOST);
        }

        [TestMethod]
        public void Decline_WhenBookingIsApproved_StatusIsNotChanged()
        {
            //Arrange
            var booking = new Booking()
            {
                PricePerDay = 5,
                From = new DateTime(2018, 4, 4),
                To = new DateTime(2018, 4, 11),
                Status = BookingStatus.ACCEPTED
            };

            //Act
            booking.Decline("Test");

            //Assert
            Assert.IsTrue(booking.Status == BookingStatus.ACCEPTED);
        }

        [TestMethod]
        public void CancelByTenant_WhenBookingIsApproved_StatusIsNotChanged()
        {
            //Arrange
            var booking = new Booking()
            {
                PricePerDay = 5,
                From = new DateTime(2018, 4, 4),
                To = new DateTime(2018, 4, 11),
                Status = BookingStatus.ACCEPTED
            };

            //Act
            booking.CancelByTenant();

            //Assert
            Assert.IsTrue(booking.Status == BookingStatus.ACCEPTED);
        }
    }
}
