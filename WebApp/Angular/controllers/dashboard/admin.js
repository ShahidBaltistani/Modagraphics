(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', 'com'];
    function admin($scope, com) {
        $scope.active_project = 0;
        $scope.active_sites = 0;
        $scope.bids = 0;

        $scope.GetDashboardData = function () {
            com.get("/ListCounter/GetAdminDashboardData").then(function (result) {
                ////debugger;
                $scope.active_project = result[0].ActiveProjects;
                $scope.active_sites = result[0].ActiveSites;

            });
        }

        $scope.GetDashboardData();
    };

})();