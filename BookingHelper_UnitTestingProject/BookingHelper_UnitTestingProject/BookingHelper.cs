using System.Linq;

namespace BookingHelper_UnitTestingProject
{
    public static class BookingHelper
    {
    
        public static string OverlappingBookingsExist(Booking booking, IBookingRepository bookingRepository)
        {
            if (booking.Status == "Cancelled".ToLower())
                return string.Empty;

           var bookings = bookingRepository.GetActiveBooking(booking.Id);

            var overlappingBooking =
                bookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate < b.DepartureDate
                        && b.ArrivalDate < booking.DepartureDate);

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }
}
