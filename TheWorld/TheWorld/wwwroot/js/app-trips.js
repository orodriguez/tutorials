(function () {
  "use strict";

  angular.module("app-trips", ["simpleControls", "ngRoute"])
    .config(function ($routeProvider, $locationProvider) {
      $routeProvider.when("/", {
        controller: "TripsCtrl",
        controllerAs: "vm",
        templateUrl: "/views/tripsView.html"
      });

      $routeProvider.when("/editor/:tripName", {
        controller: "TripEditorCtrl",
        controllerAs: "vm",
        templateUrl: "/views/tripEditorView.html "
      });

      $routeProvider.otherwise({
        redirectTo: "/"
      });

      $locationProvider.hashPrefix("");
    });
})();