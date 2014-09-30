var app = angular.module('nprApp', []);


app.controller('clientController', ['$scope', function ($scope) {
  $scope.clients = [
    { name: 'Google', ID: 10, contactName: 'Bryan', contactEmail: 'bryan@google.com', contactPhone: '555-555-5555', margin: '25%', acctManager: 'Jenna', mentor: 'Susan' },
    { name: 'Cadillac', ID: 47 },
    { name: 'Mercedes', ID: 3 }
  ];
}]);

app.controller('campaignController', ['$scope', function ($scope) {
  $scope.campaigns = [];
}]);


// Side nav expansion
$(function () {
  $("nav.main header a, nav.main a.open").click(function (e) {
    e.preventDefault();
    $("nav, main, footer").toggleClass("collapsed");

    setTimeout(function () {
      $(window).trigger("throttledresize", true);
    }, 410);
  });
});

app.directive('name', [function () {
  return {
    restrict: 'EAC',
    link: function (scope, elm, attrs) {
      scope.$watch(attrs.)
    }
  };
}])