(function () {
    'use strict';

    angular
        .module('app')
        .controller('fleetowner', fleetowner);

    fleetowner.$inject = ['$scope', 'com'];
    function fleetowner($scope, com)
    {
        $scope.TotalSupervisors = 0;
        $scope.TotalVehicals = 0;
        $scope.Approvals = 0;
        $scope.IncidentReports = 0;
        $scope.CompletedJobs = 0;

        $scope.GetDashboardData = function ()
        {
            com.get("/ListCounter/GetFleetownerDashboardData").then(function (result) {
                $scope.TotalSupervisors = result[0].TotalSupervisors;
                $scope.TotalVehicals = result[0].TotalVehicals;
                $scope.Approvals = result[0].Approvals;
                $scope.IncidentReports = result[0].IncidentReports;
                $scope.CompletedJobs = result[0].CompletedJobs;
            });
        }
        $scope.GetDashboardData();
    }
})();