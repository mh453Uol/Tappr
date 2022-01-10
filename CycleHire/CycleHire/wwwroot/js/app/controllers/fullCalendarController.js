var FullCalendarController = function () {

    var initialize = function (container) {
        $(container).fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,listWeek'
            },
            navLinks: true, // can click day/week names to navigate views
            editable: false,
            eventLimit: true, // allow "more" link when too many events
            events: '/api/Booking/Host'
        });
    };

    return {
        initialize: initialize
    };

}();