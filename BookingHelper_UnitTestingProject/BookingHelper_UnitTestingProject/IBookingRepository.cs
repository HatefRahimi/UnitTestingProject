using System.Linq;

namespace BookingHelper_UnitTestingProject
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBooking(int? excludedBookingId = null);
    }
}