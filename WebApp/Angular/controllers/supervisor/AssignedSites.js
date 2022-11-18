(function () {
    'use strict';


    angular
        .module('app')
        .controller('supervisor_AssignedSites', supervisor_AssignedSites);

    supervisor_AssignedSites.$inject = ['$scope', 'com'];

    function supervisor_AssignedSites($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.assignedSites = {};

        $scope.searchModel = { projectName: '', siteName: '', assignedDate:'' };

        $scope.getAssignedSites = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,

                FirstName: $scope.searchModel.projectName, // First Name is used for Project Name
                LastName: $scope.searchModel.siteName,     // Last Name is used for Site Name
                AssignDate: $scope.searchModel.assignedDate == null ? null : $scope.searchModel.assignedDate
            };
            com.post("/Supervisor/AssignedSites", obj).then(function (result) {

                console.log(result);

                $scope.assignedSites = result.AssignedSites;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getAssignedSites();

        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getAssignedSites();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '', assignedDate: ''};
            $scope.getAssignedSites();
        };



        selectFirstOption('drpPage');
        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getAssignedSites();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getAssignedSites();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getAssignedSites();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getAssignedSites();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getAssignedSites();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getAssignedSites();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        //$scope.vehicleDetailsModal = function (Id) {
        //    //debugger

        //    $scope.VehiclesType = {};

        //    com.get('/Supervisor/GetVehiclesAgainstAssignedSites/' + Id).then(function (result) {

        //        $scope.VehiclesType = result.VehiclesAgainstAssignedSites;

        //        console.log($scope.VehiclesType);

        //        $("#vehicle-details-modal").modal('show');
        //    });

        //};

        $scope.vehicleDetailsModal = function (siteId) {
          //  //debugger;
            $scope.VehiclesType = {};
            com.get('/Installer/GetVehiclesAgainstAssignedSites/' + siteId).then(function (result) {
                $scope.VehiclesType = result.VehiclesAgainstAssignedSites;
                $("#vehicle-details-modal").modal('show');
            });
        };
    }
})();