var Utilties = function () {

    var toDate = function toDate(date) {
        return new Date(date);
    };

    var difference = function (startDate, endDate) {
        return moment(endDate).diff(moment(startDate), "days");
    };

    var toGoogleMapLatLng = function(routes){

        var googleMapLatLng = []

        routes.map(function (route) {
            googleMapLatLng.push({ location: new google.maps.LatLng(route.latitude, route.longitude) });
        })

        return googleMapLatLng;
    }

    return {
        date: {
            toDate: toDate,
            difference: difference,
        },
        map: {
            toGoogleMapLatLng: toGoogleMapLatLng
        }
    };
}();