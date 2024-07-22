using NUnit.Framework;
using Moq;
using BookingHelper_UnitTestingProject;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UnitTestingProject_UnitTests
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingsExist
    {
        private Booking _booking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _booking = new Booking
            {
                Id = 2,
                ArrivalDate = ArrivedOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20),
                Reference = "Ref2"

            };
             _repository = new Mock<IBookingRepository>();
            _repository.Setup(r => r.GetActiveBooking(1)).Returns(new List<Booking>
            {
                _booking
            }.AsQueryable());
        }

        [Test]
        public void BookingStartsAndFinishedBeforeAnExistingBooking_ReturnEmptyString()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = Before (_booking.ArrivalDate, days: 2),
                DepartureDate = Before (_booking.ArrivalDate),
                Reference = "Ref1"
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsBeforeAndFinishesInTheMiddleExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = Before(_booking.ArrivalDate, days: 2),
                DepartureDate = After(_booking.ArrivalDate),
                Reference = "Ref1"
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterAnExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = Before(_booking.ArrivalDate, days: 2),
                DepartureDate = After(_booking.DepartureDate),
                Reference = "Ref1"
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(_booking.ArrivalDate),
                DepartureDate = Before(_booking.DepartureDate),
                Reference = "Ref1"
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }

        [Test]
        public void BookingStartsInTheMiddleOfAnExistingBookingButFinishesAfter_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(_booking.ArrivalDate),
                DepartureDate = After(_booking.DepartureDate),
                Reference = "Ref1"
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesAfterAnExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(_booking.DepartureDate),
                DepartureDate = After(_booking.DepartureDate, days:2),
                Reference = "Ref1"
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingOverlapButNewBookingIsCancelled_ReturnEmptyString()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(_booking.ArrivalDate),
                DepartureDate = After(_booking.DepartureDate),
                Reference = "Ref1",
                Status = "cancelled",
                
            }, _repository.Object);

            // Assert
            Assert.That(result, Is.Empty);
        }


        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }
        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }

        private DateTime ArrivedOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}
