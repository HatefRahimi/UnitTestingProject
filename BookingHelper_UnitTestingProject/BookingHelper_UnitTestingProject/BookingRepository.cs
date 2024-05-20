using System.Linq;

namespace BookingHelper_UnitTestingProject
{
    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> GetActiveBooking(int? excludedBookingId = null)
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Status != "Cancelled");
            if (excludedBookingId.HasValue)
                bookings.Where(b => b.Id != excludedBookingId.Value);

            return bookings;
        }
    }
}
