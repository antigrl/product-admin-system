'use strict';

var app = angular.module('nprApp', ['datatables']);


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

// Toggle Edit Campaign field
app.controller('toggleController', function($scope) {
  $scope.campaignToggle = false;
  $scope.campaignToggle = false;
});

app.run(function (DTDefaultOptions) {
  DTDefaultOptions.setDisplayLength(20);
});