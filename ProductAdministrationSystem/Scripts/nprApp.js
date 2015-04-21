'use strict';

var app = angular.module('nprApp', ['datatables', 'textarea-fit', 'datatables.scroller', 'ngAnimate', 'ngTooltip', 'ngMaterial']);

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

$('.dataTables_filter > label > input').attr("placeholder", "Search");


var adjustment

$('.sortable-table').sortable({
  containerSelector: 'table',
  itemPath: '> tbody',
  itemSelector: 'tr',
  placeholder: '<tr class="placeholder"/>',
  pullPlaceholder: false,
  axis: "y",
  onDrop: function (item, targetContainer, _super) {
    var clonedItem = $('<tr/>').css({height: 0});
    item.before(clonedItem)
    clonedItem.animate({'height': item.height()})

    item.animate(clonedItem.position(), function  () {
      clonedItem.detach()
      _super(item)
    })
  },

  // set item relative to cursor position
  onDragStart: function ($item, container, _super) {
    var offset = $item.offset(),
    pointer = container.rootGroup.pointer

    adjustment = {
      left: pointer.left - offset.left,
      top: pointer.top - offset.top
    }

    _super($item, container)
  },
  onDrag: function ($item, position) {
    $item.css({
      left: position.left - adjustment.left,
      top: position.top - adjustment.top
    })
  }
});
