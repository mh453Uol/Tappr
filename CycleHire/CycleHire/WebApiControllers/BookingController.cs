using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CycleHire.Core.Dtos;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CycleHire.WebApiControllers
{
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        // GET: api/NotAvailableDates/id
        [HttpGet("NotAvailableDates/{id}")]
        public async Task<CalendarAvailablityDto> GetNotAvailableDatesAsync(Guid id)
        {
            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddMonths(12);

            var bookedDays = await _unitOfWork.Bookings.GetBookedDays(id, startDate, endDate);

            var availabilty = await _unitOfWork.Availability.GetAvailabilityByIdAsync(id);

            var dto = new CalendarAvailablityDto()
            {
                BookedDates = bookedDays,
                NotRentableDays = availabilty.GetDaysNotAvailable(),
                StartDate = startDate,
                EndDate = endDate
            };

            return dto;
        }

        // GET api/host?start=2013-12-01&end=2014-12-01
        [HttpGet("Host")]
        [Authorize(Roles = "Host")]
        public async Task<List<FullCalendarEventDto>> Get(DateTime start, DateTime end)
        {
            var userId = _userManager.GetUserId(User);
            var bookings = await _unitOfWork.Host.GetBookings(userId, start, end);
            return bookings;
        }
    }
}
