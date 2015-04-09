'use strict';

var app = angular.module('nprApp', ['datatables', 'textarea-fit', 'datatables.scroller']);

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
app.controller('toggleController', function() {
  this.tab = 1;

  this.selectTab = function(setTab) {
    this.tab = setTab;
  };

  this.isSelected = function(checkTab) {
    return this.tab === checkTab;
  };
});

app.controller('WithScrollerCtrl', WithScrollerCtrl);

function WithScrollerCtrl(DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions()
  .withOption('paging', false)
  .withScroller()
  .withOption('deferRender', true)
  .withOption('scrollY', 200);
}