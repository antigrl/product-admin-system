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
$('.category-checkboxes dd').sortable({
  placeholder: '<div class="placeholder"/>',
  onDrop: function (item, targetContainer, _super) {
    var clonedItem = $('<div/>').css({height: 0});
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

$(".sortable-table").on('click', 'ul', function (e) {
    if (e.ctrlKey || e.metaKey) {
        $(this).toggleClass("selected");
    } else {
        $(this).addClass("selected").siblings().removeClass('selected');
    }
}).sortable({
    delay: 150, //Needed to prevent accidental drag when trying to select
    revert: 0,
    placeholder: false,
    helper: function (e, item) {

        if (!item.hasClass('selected')) {
            item.addClass('selected').siblings().removeClass('selected');
        }
        var elements = item.parent().children('.selected').clone();

        item.data('multidrag', elements).siblings('.selected').remove();

        var helper = $('<ul/>');
        return helper.append(elements);
    },
    stop: function (e, ui) {

        var elements = ui.item.data('multidrag');

        ui.item.after(elements).remove();
    }

});

app.controller('AppCtrl', function($scope, $mdToast, $animate) {
  // Save toast alert
  $scope.toastPosition = {
    bottom: true,
    top: false,
    left: true
  };
  $scope.getToastPosition = function() {
    return Object.keys($scope.toastPosition)
      .filter(function(pos) { return $scope.toastPosition[pos]; })
      .join(' ');
  };
  $scope.showSimpleToast = function() {
    $mdToast.show(
      $mdToast.simple()
        .content('Changes saved!')
        .position($scope.getToastPosition())
        .hideDelay(3000)
    );
  };
})
.controller('ToastCtrl', function($scope, $mdToast) {
  $scope.closeToast = function() {
    $mdToast.hide();
  };
});