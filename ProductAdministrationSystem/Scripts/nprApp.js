'use strict';

var app = angular.module('nprApp', ['datatables', 'textarea-fit', 'datatables.scroller', 'ngAnimate', 'ngTooltip']);

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

app.controller('WithScrollerCtrl', WithScrollerCtrl);

function WithScrollerCtrl(DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions()
  .withScroller()
  .withOption('deferRender', true)
  .withOption('scrollY', 200);
}

app.controller('NoScrollCtrl', NoScrollCtrl);

function NoScrollCtrl(DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions()
    .withOption('paging', false);
}

