using System.Linq;

namespace BookingHelper_UnitTestingProject
{
    public static class BookingHelper
    {
    
        public static string OverlappingBookingsExist(Booking booking, IBookingRepository bookingRepository)
        {
            if (booking.Status == "Cancelled")
                return string.Empty;

           var bookings = bookingRepository.GetActiveBooking(booking.Id);

            var overlappingBooking =
                bookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate < b.ArrivalDate
                        && b.ArrivalDate < booking.DepartureDate);

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }
}
