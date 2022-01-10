var ListingController = function () {
    var autocomplete;

    var initialize = function (container) {
        var input = $(container).find("#js-location-prediction");
        autocomplete = new google.maps.places.Autocomplete(input[0]);
        autocomplete.addListener('place_changed', onPlaceChanged);
    };

    function onPlaceChanged() {
        var lat = autocomplete.getPlace().geometry.location.lat();
        var lng = autocomplete.getPlace().geometry.location.lng();

        $("#js-latitude").val(lat);
        $("#js-longitude").val(lng);
    }

    return {
        initialize: initialize
    };

}();