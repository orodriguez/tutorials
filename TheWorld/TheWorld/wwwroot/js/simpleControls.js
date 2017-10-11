(function() {
  "use strict";

  angular.module("simpleControls", [])
    .directive("waitCursor", function() {
      return {
        scope: {
          displayWhen: "=displayWhen"
        },
        restrict: "E",
        templateUrl: "/views/waitCursor.html"
      };
    });
})();