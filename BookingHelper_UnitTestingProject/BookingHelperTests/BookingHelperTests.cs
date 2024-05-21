using NUnit.Framework;
using Moq;
using BookingHelper_UnitTestingProject;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestingProject_UnitTests
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingsExist
    {
        [Test]
        public void BookingStartsAndFinishedBeforeAnExistingBooking_ReturnEmptyString()
        {
            // Arrange
            var repository = new Mock<IBookingRepository>();
            repository.Setup(r => r.GetActiveBooking(1)).Returns(new List<Booking>
            {
                new Booking
                {
                    Id= 2,
                    ArrivalDate = new System.DateTime(2017, 1, 15, 14, 0, 0),
                    DepartureDate = new System.DateTime(2017,1,20,10,0,0),
                    Reference = "Ref2"

                }
            }.AsQueryable());

            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = new System.DateTime(2017, 1, 10, 14, 0, 0),
                DepartureDate = new System.DateTime(2017, 1, 14, 10, 0, 0),
                Reference = "Ref1"
            }, repository.Object);

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}
