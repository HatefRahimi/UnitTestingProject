using System.Collections.Generic;
using System.Linq;

namespace BookingHelper_UnitTestingProject
{
    public class UnitOfWork
    {
        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }
    }
}
