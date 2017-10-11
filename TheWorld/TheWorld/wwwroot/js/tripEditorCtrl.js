(function () {
  "use strict";

  angular.module("app-trips")
    .controller("TripEditorCtrl", function ($routeParams, $http) {
      var vm = this;

      vm.tripName = $routeParams.tripName;
      vm.stops = [];
      vm.errorMessage = "";
      vm.isBusy = true;
      vm.newStop = {};

      var url = "/api/trips/" + vm.tripName + "/stops";

      $http.get(url)
        .then(function (reps) {
          angular.copy(reps.data, vm.stops);
          showMap(vm.stops);
        }, function (err) {
          vm.errorMessage = "Failet to load stops";
        })
        .finally(function () {
          vm.isBusy = false;
        });

      vm.addStop = function() {
        vm.isBusy = true;

        $http.post(url, vm.newStop)
          .then(function (reps) {
            vm.stops.push(reps.data);
            showMap(vm.stops);
            vm.newStop = {};
          }, function (err) {
            vm.errorMessage = "Failet to load stops";
          })
          .finally(function () {
            vm.isBusy = false;
          });;
      };
    });

  function showMap(stops) {
    if (stops && stops.length > 0) {
      var mapsStops = _.map(stops, function(stop) {
        return {
          lat: stop.latitude,
          long: stop.longitude,
          info: stop.name 
        };
      });
      
      travelMap.createMap({
        stops: mapsStops,
        selector: "#map",
        currentStop: 1,
        initialZoom: 3
      });
    }
  }
})();