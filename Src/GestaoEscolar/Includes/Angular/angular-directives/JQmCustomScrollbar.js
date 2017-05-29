'use strict';
(function (angular) {
    angular
		.module('app')
        .directive('jqMCustomScrollbar', jqMCustomScrollbar);

    function jqMCustomScrollbar() {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$apply(function () {
                    angular.element(document).ready(function () {
                        angular.element(element).mCustomScrollbar();
                    });
                });
            }
        };
    }
})(angular);