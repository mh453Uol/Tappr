var CalendarController = function (calendarService, utilities) {

    var view = {
        listingId: null,
        selectFromDate: null,
        selectedToDate: null,
        duration: null,
        totalPrice: null,
        pricePerDay: null,
        totalDurationMessage: "",
        totalPriceMessage: "",
        errorMessage: ""
    };

    var model = {
        startDate: null,
        endDate: null,
        bookedDates: null,
        notRentableDays: null
    };

    var initialize = function (container) {

        view.listingId = $(container).attr("data-listing-id");
        view.pricePerDay = parseInt($(container).find("#js-total-price").attr("data-price"));
        calendarService.getNotAvailableDates(view.listingId, setModel, null);
    };

    var setModel = function (data) {
        model.startDate = utilities.date.toDate(data.startDate);
        model.endDate = utilities.date.toDate(data.endDate);
        model.bookedDates = data.bookedDates.map(utilities.date.toDate);
        model.notRentableDays = data.notRentableDays;

        initializePickADateCalendar();
        console.log(data, view);
    };

    var initializePickADateCalendar = function () {

        $(".datepicker").pickadate({
            min: model.startDate,
            max: model.endDate,
            disable: getAllDisabledDates(),
            onSet: function (context) {

                if (this.$node.is("#js-from")) {
                    setSelectedFrom(this.get());
                }

                if (this.$node.is("#js-to")) {
                    setSelectedTo(this.get());
                }

                if (bothFromAndToSelected()) {

                    view.duration = utilities.date.difference(view.selectFromDate, view.selectedToDate) + 1;
                    view.totalPrice = view.pricePerDay * view.duration;

                    if (isBookable()) {
                        disableRentButton(false);
                        render(isError = false);
                    } else {
                        disableRentButton(true);
                        render(isError = true);
                    }
                }
            }
        });
    };

    function setSelectedFrom(date) {
        if (date) {
            view.selectFromDate = utilities.date.toDate(date);
        } else {
            view.selectFromDate = null;
        }
    }

    function setSelectedTo(date) {
        if (date) {
            view.selectedToDate = utilities.date.toDate(date);
        } else {
            view.selectedToDate = null;
        }
    }

    function isBookable() {
        // Javascript passes objects by reference.
        // You can only book consecutive dates so cant have booked days in the middle.
        var start = new Date(view.selectFromDate.toString());
        var end = new Date(view.selectedToDate.toString());

        if (start > end) {
            return false;
        }

        while (start <= end) {
            // javascript dates are by reference (memory addresses not value) so when we do include 
            // in date object it checks if the memory address is included in the array we need to
            // change array to numbers which represent dates
            // I do start.getDay() + 1 since in JavaScript Sunday is 0, Monday is 1, and so on in my 
            // model sunday is 1
            if (model.notRentableDays.includes(start.getDay() + 1) || model.bookedDates.map(Number).includes(+start)) {
                return false;
            }
            var newDate = start.setDate(start.getDate() + 1);
            start = new Date(newDate);
        }
        return true;
    }

    function getAllDisabledDates() {
        return model.notRentableDays.concat(model.bookedDates);
    }

    function bothFromAndToSelected() {
        return view.selectedToDate && view.selectFromDate;
    }

    function setDurationMessage() {
        if (view.duration > 0) {
            view.totalDurationMessage = "Booking for " + view.duration + " days";
        } else {
            view.totalDurationMessage = "";
        }
    }

    function setTotalPriceMessage() {
        if (view.duration > 0) {
            view.totalPriceMessage = "Total cost is £" + view.totalPrice;
        } else {
            view.totalPriceMessage = "";
        }
    }

    function setErrorMessage() {
        if (view.duration <= 0) {
            view.errorMessage = "Start date must be earlier than end date";
        } else {
            view.errorMessage = "You can only book in consecutive dates";
        }
    }

    function render(isError = false) {
        setDurationMessage();
        setTotalPriceMessage();
        setErrorMessage();

        if (isError) {
            $(".js-calendar").find("#js-error").text(view.errorMessage);
            $(".js-calendar").find("#js-error").show();
            $(".js-calendar").find("#js-success-info").hide();

        } else {
            $(".js-calendar").find("#js-total-days").text(view.totalDurationMessage);
            $(".js-calendar").find("#js-total-price").text(view.totalPriceMessage);
            $(".js-calendar").find("#js-success-info").show();
            $(".js-calendar").find("#js-error").hide();
        }

    }

    function disableRentButton(enable) {
        $(".js-calendar").find("#js-button").prop("disabled", enable);
    }

    return {
        initialize: initialize
    };

}(CalendarService, Utilties);