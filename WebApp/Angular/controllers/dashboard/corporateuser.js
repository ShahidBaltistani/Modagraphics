(function () {
    'use strict';

    angular
        .module('app')
        .controller('fleetowner', fleetowner);

    fleetowner.$inject = ['$scope', 'com'];
    function fleetowner($scope, com) {
        $scope.TotalFleetOwner = 0;
        $scope.TotalVehicals = 0;
        $scope.CompletedJobs = 0;
        $scope.CompletedSites = 0;
        $scope.TotalSites = 0;
        $scope.TotalInProgressSites = 0;
        $scope.JobApprovals = 0;
        $scope.GetDashboardData = function () {
            com.get("/ListCounter/GetCorporateUserDashboardData").then(function (result) {
                $scope.TotalFleetOwner = result[0].TotalFleetOwner;
                $scope.TotalVehicals = result[0].TotalVehicals;
                $scope.CompletedSites = result[0].CompletedSites;
                $scope.CompletedJobs = result[0].CompletedJobs;
                $scope.TotalSites = result[0].TotalSites;
                $scope.TotalInProgressSites = result[0].TotalInProgressSites;
                $scope.JobApprovals = result[0].JobApprovals;
            });
        }
        $scope.GetDashboardData();
    }
})();