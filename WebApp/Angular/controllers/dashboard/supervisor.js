(function () {
    'use strict';

    angular
        .module('app')
        .controller('supervisor', supervisor);

    supervisor.$inject = ['$scope', 'com'];
    function supervisor($scope, com)
    {
        $scope.TotalSites = 0;
        $scope.Approvals = 0;
        $scope.TotalIncidentReports = 0;

        $scope.GetDashboardData = function () {
            com.get("/ListCounter/GetSupervisorDashboardData").then(function (result) {
                $scope.TotalSites = result[0].TotalSites;
                $scope.Approvals = result[0].Approvals;
                $scope.TotalIncidentReports = result[0].TotalIncidentReports ;
            });
        }
        $scope.GetDashboardData();
    }
})();