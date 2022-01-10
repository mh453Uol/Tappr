using CycleHire.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CycleHire.UnitTests
{
    [TestClass]
    public class ListingAvailabilityTest
    {
        [TestMethod]
        public void GetDaysNotAvailable_WhenNotAvailableIsMondayOnly_ReturnsAllDaysExceptMonday()
        {
            //Arrange
            var availability = new Availability()
            {
                Monday = true
            };

            //Act
            var daysAvailable = availability.GetDaysNotAvailable();

            //1 is Sunday, 2 is Monday ..
            var availableDaysExpected = new int[] { 1, 3, 4, 5, 6, 7 };
            //Assert
            Assert.IsTrue(daysAvailable.SequenceEqual(availableDaysExpected));
        }

        [TestMethod]
        public void GetDaysNotAvailable_WhenNotAvailableIsMondayToFriday_ReturnsSaturdayAndSunday()
        {
            //Arrange
            var availability = new Availability()
            {
                Monday = true,
                Tuesday = true,
                Wenesday = true,
                Thursday = true,
                Friday = true
            };

            //Act
            var daysAvailable = availability.GetDaysNotAvailable();

            //1 is Sunday, 2 is Monday ..
            var availableDaysExpected = new int[] { 1, 7 };
            //Assert
            Assert.IsTrue(daysAvailable.SequenceEqual(availableDaysExpected));
        }
    }
}
