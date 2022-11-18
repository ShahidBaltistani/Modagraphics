(function () {
    'use strict';

    angular
        .module('app')
        .controller('approvalmanager', approvalmanager);

    approvalmanager.$inject = ['$scope', 'com'];
    function approvalmanager($scope, com)
    {
        $scope.UsersApprovals = 0;
        $scope.JobApprovals = 0;

        $scope.GetDashboardData = function () {
            com.get("/ListCounter/GetApprovalManagerDashboardData").then(function (result) {
                ////debugger;
                $scope.UsersApprovals = result[0].UserApprovals;
                $scope.JobApprovals = result[0].JobApprovals;
            });
        }
        $scope.GetDashboardData();
    }
})();