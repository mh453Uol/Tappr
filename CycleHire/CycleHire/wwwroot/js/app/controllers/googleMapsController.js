var GoogleMapsController = function (utilities) {

    var directionsService;
    var directionsDisplay;

    function addMap(container, lat, long, zoomlevel) {
        var latLng = { lat: lat, lng: long };

        var map = new google.maps.Map(document.getElementById(container), { zoom: zoomlevel, center: latLng });

        var marker = new google.maps.Marker({
            position: latLng,
            map: map
        });
    }

    function addAutoComplete(container, onEnter) {
        var input = $(container).find("#js-location-prediction");
        var autocomplete = new google.maps.places.Autocomplete(input[0]);

        autocomplete.addListener('place_changed', function () {
            var lat = autocomplete.getPlace().geometry.location.lat();
            var lng = autocomplete.getPlace().geometry.location.lng();
            onEnter(lat, lng);
        });
    }

    function addMapWithManyMarkers(container, lat, long, markers) {
        var latLng = { lat: lat, lng: long };
        var map = new google.maps.Map(document.getElementById(container), { zoom: 15, center: latLng });

        var bounds = new google.maps.LatLngBounds();
        var loc;

        for (var i = 0; i < markers.length; i++) {
            var listing = markers[i];
            console.log(listing);

            var contentString = '<div id="content">' +
                '<h3>' + listing.title + '</h3>' +
                '<p>£' + listing.price + '</p>' +
                '</div>';

            m = addMarker(listing.latitude, listing.longitude, listing.title, map, contentString);

            loc = new google.maps.LatLng(m.position.lat(), m.position.lng());
            bounds.extend(loc);

            map.fitBounds(bounds);
            map.panToBounds(bounds);
        }
    }

    function addMarker(lat, long, title, map, windowContent) {

        var infowindow = new google.maps.InfoWindow({
            content: windowContent
        });

        var marker = new google.maps.Marker({
            position: { lat: lat, lng: long },
            map: map,
            title: title
        });

        marker.addListener('click', function () {
            infowindow.open(map, marker);
        });

        return marker;
    }

    function setLocation(map) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var coordinates = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                map.setCenter(coordinates);
            }, function () {
                $("#js-location-blocked").show();
            });
        }
    }

    function addMapWithDraggableDirections(container, direction_panel, startInput, endInput, onDirectionChange) {

        //Set the default coordinates of the map to Uol
        map = new google.maps.Map(document.getElementById(container), {
            center: { lat: 52.6369, lng: 1.1398 },
            zoom: 15
        });

        //Create new instance of directionServices
        directionsService = new google.maps.DirectionsService;

        //Set up the map so users are able to drag the polyline
        //and specify the panel where written directions will be 
        //render.
        directionsDisplay = new google.maps.DirectionsRenderer({
            draggable: true,
            map: map,
            panel: document.getElementById(direction_panel)
        });

        //Get user locations and set it to the center of the map
        setLocation(map);

        //When user drags markers or add waypoints listen to event.
        directionsDisplay.addListener('directions_changed', function () {

            //using directions api get directions journey legs
            var directions = directionsDisplay.directions.routes[0].legs[0];

            //create a route object which has a origin which contains lat and long
            //and a destination.
            var route = {
                origin: {
                    latitude: directions.start_location.lat(),
                    longitude: directions.start_location.lng()
                },
                destination: {
                    latitude: directions.end_location.lat(),
                    longitude: directions.end_location.lng()
                },
                waypoints: [],
                polyline: directionsDisplay.directions.routes[0].overview_polyline
            };

            //iterate through waypoints returned by the api using map function 
            //and add them to the waypoints array.
            directions.via_waypoint.map(function (direction) {
                route.waypoints.push({
                    latitude: direction.location.lat(),
                    longitude: direction.location.lng()
                });
            });

            //update the GUI
            onDirectionChange(route);
        });

        new AutocompleteDirections(map, startInput, endInput);
    }

    function addMapWithExistingRoute(container,route) {
        map = new google.maps.Map(document.getElementById(container), {
            //Default Leicester Coordinates
            center: { lat: 52.6369, lng: 1.1398 },
            zoom: 15
        });

        directionsService = new google.maps.DirectionsService;
        directionsDisplay = new google.maps.DirectionsRenderer({
            map: map,
        });

        var path = google.maps.geometry.encoding.decodePath(route.polyline);

        var polyline = new google.maps.Polyline({
            path: path,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 5,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
            map: map
        });

        var bounds = new google.maps.LatLngBounds();

        for (var i = 0; i < path.length; i++) {
            bounds.extend(path[i]);
        }

        polyline.setMap(map);
        map.fitBounds(bounds);

        var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        var labelIndex = 0;

        //add markers manually 
        var origin = new google.maps.Marker({
            position: { lat: route.origin.latitude, lng: route.origin.longitude },
            map: map,
            title: 'Start',
            label: labels[labelIndex++ % labels.length],
        });

        route.waypoints.map(function (waypoint,index) {
            new google.maps.Marker({
                position: { lat: waypoint.latitude, lng: waypoint.longitude },
                map: map,
                title: 'Waypoint',
                label: labels[labelIndex++ % labels.length],
            });
        })

        var destination = new google.maps.Marker({
            position: { lat: route.destination.latitude, lng: route.destination.longitude },
            map: map,
            title: 'End',
            label: labels[labelIndex++ % labels.length],
        });

    }


    function AutocompleteDirections(map, startInput, endInput) {
        this.map = map;
        this.startPlaceId = null;
        this.endPlaceId = null;
        this.travelMode = 'BICYCLING';
        var startInput = $(startInput)[0];
        var endInput = $(endInput)[0];

        var startAutocomplete = new google.maps.places.Autocomplete(startInput, { placeIdOnly: true });
        var endAutocomplete = new google.maps.places.Autocomplete(endInput, { placeIdOnly: true });

        this.setupPlaceChangedListener(startAutocomplete, 'ORIG');
        this.setupPlaceChangedListener(endAutocomplete, 'DEST');
    }

    AutocompleteDirections.prototype.setupPlaceChangedListener = function (autocomplete, mode) {
        var me = this;
        autocomplete.bindTo('bounds', this.map);
        autocomplete.addListener('place_changed', function () {
            var place = autocomplete.getPlace();
            if (!place.place_id) {
                alert("Select a place from the dropdown");
                return;
            }
            if (mode === 'ORIG') {
                me.startPlaceId = place.place_id;
            } else {
                me.endPlaceId = place.place_id;
            }
            me.route();
        });

    };

    AutocompleteDirections.prototype.route = function () {
        if (!this.startPlaceId || !this.endPlaceId) {
            return;
        }
        var me = this;

        directionsService.route({
            origin: { 'placeId': this.startPlaceId },
            destination: { 'placeId': this.endPlaceId },
            travelMode: this.travelMode
        }, function (response, status) {
            if (status === 'OK') {
                directionsDisplay.setDirections(response);
            } else {
                alert('Directions request failed due to ' + status);
            }
        });
    };


    return {
        addMapWithMarker: addMap,
        addMapWithDraggableDirections: addMapWithDraggableDirections,
        addMapWithExistingRoute: addMapWithExistingRoute,
        //addAutoComplete: addAutoComplete,
        addMapWithMultipleMarkers: addMapWithManyMarkers,
        getLocation: setLocation
    };

}(Utilties);