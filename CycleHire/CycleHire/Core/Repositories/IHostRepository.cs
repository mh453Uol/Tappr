using CycleHire.Core.Dtos;
using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public interface IHostRepository
    {
        Task<List<Booking>> GetPendingBookings(string hostId);
        Task<List<FullCalendarEventDto>> GetBookings(string hostId, DateTime start, DateTime end);

    }
}
