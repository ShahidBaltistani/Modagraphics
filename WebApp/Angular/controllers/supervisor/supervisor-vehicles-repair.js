(function () {
    'use strict';
    angular
        .module('app')
        .controller('supervisor_vehiclsToRepair', supervisor_vehiclsToRepair);

    supervisor_vehiclsToRepair.$inject = ['$scope', 'com'];

    function supervisor_vehiclsToRepair($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.vehicalsToReport = {};

        $scope.searchModel = { projectName: '', siteName: '', LPN:'', VIN:'', Unit:''};



        $scope.VehiclestoReports= function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,



                FirstName: $scope.searchModel.projectName,  // FirstName is used for ProjectName
                LastName: $scope.searchModel.siteName,      // LastName is used for SiteName
                LPN: $scope.searchModel.LPN,
                VIN: $scope.searchModel.VIN,
                Unit: $scope.searchModel.Unit
            };
            com.post("/Supervisor/GetVehiclesToRepair", obj).then(function (result) {
                $scope.vehicalsToReport = result.vehicalsToReport;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.VehiclestoReports();

        $scope.Repaired = function (id)
        {
            com.get('/Supervisor/Repaired/'+ id).then(function (result) {
                swal("Success", "Job has been Repaired,", "success");
                $scope.VehiclestoReports();
            });
        };
        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.VehiclestoReports();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '', LPN: '', VIN: '', Unit: '' };
            $scope.VehiclestoReports();
        };



        selectFirstOption('drpPage');
        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.VehiclestoReports();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.VehiclestoReports();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.VehiclestoReports();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.VehiclestoReports();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.VehiclestoReports();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.VehiclestoReports();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };
    }
})();