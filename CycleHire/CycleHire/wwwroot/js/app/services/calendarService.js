var CalendarService = function () {

    var getNotAvailableDates = function (listingId, success, error) {

        $.ajax({
            url: "/api/Booking/NotAvailableDates/" + listingId,
            type: "GET",
            contentType: "JSON",
            success: success,
            error: error
        });
    };

    return {
        getNotAvailableDates: getNotAvailableDates
    };

}();