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


// var adjustment = $(this);


// $('.sortable-table').sortable({
//   containerSelector: 'table',
//   itemPath: '> tbody',
//   itemSelector: 'tr',
//   placeholder: '<tr class="placeholder"/>',
//   pullPlaceholder: false,
//   onDrop: function (item, targetContainer, _super) {
//     var clonedItem = $('<tr/>').css({height: 0});
//     item.before(clonedItem)
//     clonedItem.animate({'height': item.height()})

//     item.animate(clonedItem.position(), function  () {
//       clonedItem.detach()
//       _super(item)
//     })
//   },

//   // set item relative to cursor position
//   onDragStart: function ($item, container, _super) {
//     var offset = $item.offset(),
//     pointer = container.rootGroup.pointer

//     adjustment = {
//       left: pointer.left - offset.left,
//       top: pointer.top - offset.top
//     }

//     _super($item, container)
//   },
//   onDrag: function ($item, position) {
//     $item.css({
//       left: position.left - adjustment.left,
//       top: position.top - adjustment.top
//     })
//   }
// });

// $('.category-checkboxes').sortable({
//   containerSelector: 'dl',
//   itemPath: '> dd',
//   itemSelector: 'div.sortable-category',
//   placeholder: '<div class="placeholder"/>',
//   pullPlaceholder: false,
//   onDrop: function (item, targetContainer, _super) {
//     var clonedItem = $('<tr/>').css({height: 0});
//     item.before(clonedItem)
//     clonedItem.animate({'height': item.height()})

//     item.animate(clonedItem.position(), function  () {
//       clonedItem.detach()
//       _super(item)
//     })
//   },

//   // set item relative to cursor position
//   onDragStart: function ($item, container, _super) {
//     var offset = $item.offset(),
//     pointer = container.rootGroup.pointer

//     adjustment = {
//       left: pointer.left - offset.left,
//       top: pointer.top - offset.top
//     }

//     _super($item, container)
//   },
//   onDrag: function ($item, position) {
//     $item.css({
//       left: position.left - adjustment.left,
//       top: position.top - adjustment.top
//     })
//   }
// });

$(".sortable-table ul").on('click', 'li', function (e) {
    if (e.ctrlKey || e.metaKey) {
        $(this).toggleClass("selected");
    } else {
        $(this).addClass("selected").siblings().removeClass('selected');
    }
}).sortable({
    connectWith: "ul",
    delay: 150, //Needed to prevent accidental drag when trying to select
    revert: 0,
    helper: function (e, item) {
        //Basically, if you grab an unhighlighted item to drag, it will deselect (unhighlight) everything else
        if (!item.hasClass('selected')) {
            item.addClass('selected').siblings().removeClass('selected');
        }

        //////////////////////////////////////////////////////////////////////
        //HERE'S HOW TO PASS THE SELECTED ITEMS TO THE `stop()` FUNCTION:

        //Clone the selected items into an array
        var elements = item.parent().children('.selected').clone();

        //Add a property to `item` called 'multidrag` that contains the
        //  selected items, then remove the selected items from the source list
        item.data('multidrag', elements).siblings('.selected').remove();

        //Now the selected items exist in memory, attached to the `item`,
        //  so we can access them later when we get to the `stop()` callback

        //Create the helper
        var helper = $('<li/>');
        return helper.append(elements);
    },
    stop: function (e, ui) {
        //Now we access those items that we stored in `item`s data!
        var elements = ui.item.data('multidrag');

        //`elements` now contains the originally selected items from the source list (the dragged items)!!

        //Finally I insert the selected items after the `item`, then remove the `item`, since
        //  item is a duplicate of one of the selected items.
        ui.item.after(elements).remove();
    }

});