(function () {
    'use strict';

    angular
        .module('app')
        .controller('crew', crew);

    crew.$inject = ['$scope', 'com'];
    function crew($scope, com)
    {
        $scope.AssignedSites = 0;
        $scope.AssignedJobs = 0;
        $scope.JobsUnderReview = 0;

        $scope.GetDashboardData = function () {
            com.get("/ListCounter/GetCrewDashboardData").then(function (result) {
                ////debugger;
                $scope.AssignedSites = result[0].AssignedSites;
                $scope.AssignedJobs = result[0].AssignedJobs;
                $scope.JobsUnderReview = result[0].JobsUnderReview;
            });
        }
        $scope.GetDashboardData();
    }
})();