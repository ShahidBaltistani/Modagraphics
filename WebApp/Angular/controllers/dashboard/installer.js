(function () {
    'use strict';

    angular
        .module('app')
        .controller('installer', installer);

    installer.$inject = ['$scope', 'com'];
    function installer($scope, com) {
        $scope.TotalCrews = 0;
        $scope.OpenSites = 0;
        $scope.AssginedSite = 0;
        $scope.TotalBids = 0;
        $scope.TotalSites = 0;

        $scope.GetDashboardData = function () {
            com.get("/ListCounter/GetInstallerDashboardData").then(function (result) {
                $scope.TotalCrews = result[0].TotalCrews;
                $scope.OpenSites = result[0].OpenSites;
                $scope.AssginedSite = result[0].AssignedSites;
                $scope.TotalBids = result[0].TotalBids;
                $scope.TotalSites = result[0].TotalSites;
            });
        }


        $scope.GetDashboardData();
    }


})();