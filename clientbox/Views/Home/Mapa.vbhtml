@Code
    ViewData("Title") = "Mapa"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="map"></div>

<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBDpASx1MGfU4-yc1RFyvylYoIR7cLiwJA&libraries=geometry&sensor=false"></script>

<script>
    var map,
        car = "M17.402,0H5.643C2.526,0,0,3.467,0,6.584v34.804c0,3.116,2.526,5.644,5.643,5.644h11.759c3.116,0,5.644-2.527,5.644-5.644 V6.584C23.044,3.467,20.518,0,17.402,0z M22.057,14.188v11.665l-2.729,0.351v-4.806L22.057,14.188z M20.625,10.773 c-1.016,3.9-2.219,8.51-2.219,8.51H4.638l-2.222-8.51C2.417,10.773,11.3,7.755,20.625,10.773z M3.748,21.713v4.492l-2.73-0.349 V14.502L3.748,21.713z M1.018,37.938V27.579l2.73,0.343v8.196L1.018,37.938z M2.575,40.882l2.218-3.336h13.771l2.219,3.336H2.575z M19.328,35.805v-7.872l2.729-0.355v10.048L19.328,35.805z",
        gps_central = null,
        users_marker = [],
        colors = ["#007bff", "#6c757d", "#28a745", "#17a2b8", "#28a745", "#ffc107", "#dc3545", "#343a40"];

    $(function () {
        initMap()
        var viewModel = kendo.observable({
            panelsVisible: false
        });
        kendo.bind(document.body, viewModel);

        setInterval(function () {
            if (usersActivity.length > 0) {
                $.each(usersActivity, function (a, b) {
                    if (b.Lat > 0 && b.Lng > 0) {
                        userPosition(b.IDUser, b.Name, b.Lat, b.Lng)
                    }
                })
            }
        }, 1000)
    });

    function initMap(callback) {
        gps_central = new google.maps.LatLng(50.150937, 14.120382)
        map = new google.maps.Map(document.getElementById("map"), {
            zoom: 17,
            center: gps_central,
            mapTypeControl: false,
            styles: null,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });

        new CustomMarker(gps_central, map, { content: "<span style='font-size:14px;'>Centrála</span><br>AGILO.CZ s.r.o. - Huťská 366, Kladno, PSČ 27201" });
    };

    function userPosition(idu, name, lat, lng) {
        if (map) {
                var found = $.map(users_marker, function (val) {
                    if (val.idu == idu) {
                        return val
                    }
                });


            if (found.length > 0) {               
                var marker = found[0].marker;
                var overlay = found[0].overlay;
                var line = found[0].line;
                var icon = marker.icon;
                var newpos = new google.maps.LatLng(lat, lng);
                var lastPosn = marker.getPosition();
                var heading = google.maps.geometry.spherical.computeHeading(lastPosn, newpos);
                icon.rotation = heading;
                marker.setIcon(icon);
                marker.setPosition(newpos);
                overlay.setPosition(newpos);
                line.setPath([gps_central, newpos]);
            } else {
                var rand_color = colors[Math.floor(Math.random() * colors.length)];
                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(lat, lng),
                    icon: {
                        path: car,
                        scale: .7,
                        strokeColor: 'black',
                        strokeWeight: .5,
                        fillOpacity: 1,
                        fillColor: rand_color,
                        offset: '5%',
                        anchor: new google.maps.Point(10, 25)
                    },
                    map: map
                })
                var overlay = new CustomMarker(new google.maps.LatLng(lat, lng), map, { content: name, background: rand_color, id: idu });
                var line = new google.maps.Polyline({
                    path: [
                        gps_central,
                        new google.maps.LatLng(lat, lng)
                    ],
                    strokeColor: rand_color,
                    strokeOpacity: 1.0,
                    strokeWeight: 1,
                    map: map
                });
                users_marker.push({ idu: idu, marker: marker, overlay: overlay, line: line });
            }            
        };
    };

        function CustomMarker(latlng, map, args) {
            this.latlng = latlng;
            this.args = args;
            this.setMap(map);
    };

        CustomMarker.prototype = new google.maps.OverlayView();

        CustomMarker.prototype.draw = function () {

            var self = this;

            var div = this.div;

            if (!div) {

                div = this.div = document.createElement('div');

                div.className = 'marker_label';

                if (typeof (self.args.id) !== 'undefined') {
                    div.dataset.iduser = self.args.id;
                }

                if (typeof (self.args.background) !== 'undefined') {
                    div.style.background = self.args.background;
                }

                if (typeof (self.args.content) !== 'undefined') {
                    div.innerHTML = "<div class='arrow' " + (self.args.background ? "style='border-color: transparent " + self.args.background + " transparent transparent;'" : "") + "></div>" + self.args.content;
                }

                if (typeof (self.args.marker_id) !== 'undefined') {
                    div.dataset.marker_id = self.args.marker_id;
                }

                google.maps.event.addDomListener(div, "click", function (event) {
                    console.log(this)
                    google.maps.event.trigger(self, "click");
                });

                var panes = this.getPanes();
                panes.overlayImage.appendChild(div);
            }

            var point = this.getProjection().fromLatLngToDivPixel(this.latlng);
            if (point) {
                div.style.left = (point.x + 20) + 'px';
                div.style.top = (point.y - 20) + 'px';
            }
        };

        CustomMarker.prototype.remove = function () {
            if (this.div) {
                this.div.parentNode.removeChild(this.div);
                this.div = null;
            }
        };

        CustomMarker.prototype.getPosition = function () {
            return this.latlng;
        };

        CustomMarker.prototype.setPosition = function (latlng) {
            try {
                var point = this.getProjection().fromLatLngToDivPixel(latlng);
                if (point) {
                    this.div.style.left = (point.x + 20) + 'px';
                    this.div.style.top = (point.y - 20) + 'px';
                }
            } catch (ex) { }
        };
</script>


