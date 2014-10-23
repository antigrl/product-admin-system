var app = angular.module('nprApp', ['tabs']);


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

angular.module( 'tabs', [])
 .directive( 'ngTabs', function() {
  return {
    scope: true,
    restrict: 'EAC',
    controller: function( $scope ) {
      $scope.tabs = {
        index: 0,
        count: 0
      };

      this.headIndex = 0;
      this.bodyIndex = 0;

      this.getTabHeadIndex = function() {
        return $scope.tabs.count = ++this.headIndex;
      };

      this.getTabBodyIndex = function() {
        return ++this.bodyIndex;
      };
    }
  };
})

 .directive( 'ngTabHead', function() {
  return {
    scope: false,
    restrict: 'EAC',
    require: '^ngTabs',
    link: function( scope, element, attributes, controller ) {
      var index = controller.getTabHeadIndex();
      var value = attributes.ngTabHead;
      var active = /[-*\/%^=!<>&|]/.test( value ) ? scope.$eval( value ) : !!value;

      scope.tabs.index = scope.tabs.index || ( active ? index : null );

      element.bind( 'click', function() {
        scope.tabs.index = index;
        scope.$$phase || scope.$apply();
      });

      scope.$watch( 'tabs.index', function() {
        element.toggleClass( 'active', scope.tabs.index === index );
      });
    }
  };
})

 .directive( 'ngTabBody', function() {
  return {
    scope: false,
    restrict: 'EAC',
    require: '^ngTabs',
    link: function( scope, element, attributes, controller ) {
      var index = controller.getTabBodyIndex();

      scope.$watch( 'tabs.index', function() {
        element.toggleClass( attributes.ngTabBody + ' ng-show', scope.tabs.index === index );
      });
    }
  };
});

// // Floating labels
// (function($){
//   function floatLabel(inputType){
//     $(inputType).each(function(){
//       var $this = $(this);
//       // on focus add cladd active to label
//       $this.focus(function(){
//         $this.next().addClass("active");
//       });
//       //on blur check field and remove class if needed
//       $this.blur(function(){
//         if($this.val() === '' || $this.val() === 'blank'){
//           $this.next().removeClass();
//         }
//       });
//     });
//   }
//   // just add a class of "floatLabel to the input field!"
//   floatLabel(".floatLabel");
// })(jQuery);