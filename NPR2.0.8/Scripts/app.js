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