(function () {
    'use strict';

    angular
        .module('app')
        .controller('site_vehicle_type_list', site_vehicle_type_list);

    site_vehicle_type_list.$inject = ['$scope', 'com'];

    function site_vehicle_type_list($scope, com) {

        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.rightHide = false;
        $scope.searchModel = { vehicleType: '', specification:''};

        $scope.getSiteVehicle = function () {
            mApp.blockPage({ message: "Fetching..." });

            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                siteId: $('#siteId').val(),

                vehicleType: $scope.searchModel.vehicleType,
                specification: $scope.searchModel.specification
            };
            com.post("/SiteVehicleType/GetSiteVehicles", obj).then(

                function (result) {
                    debugger
                    $scope.siteVehicles = result.siteVehicles;
                    $scope.totalCount = result.TotalRecords;
                    $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                    $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                    $scope.rightHide = $scope.numOfPages > $scope.maxSize;
                });
        }

        $scope.getSiteVehicle();

        selectFirstOption('drpPage');


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getSiteVehicle();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { vehicleType: '', specification:''};
            $scope.getSiteVehicle();
        };

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getSiteVehicle();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getSiteVehicle();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getSiteVehicle();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getSiteVehicle();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getSiteVehicle();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getSiteVehicle();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.JobInQ = {};
        $scope.InQueueType = function (Id) {
            debugger
            com.get('/Site/JobInQueueDetailByJobId/' + Id).then(function (result) {
                $scope.JobInQ = result.InQueueType;
                $("#Job-In-Queue-Modal").modal('show');
            });
        };

        $scope.InProgress = {};
        $scope.InProgress = function (Id) {
            com.get('/Site/JobInProgressDetailByJobId/' + Id).then(function (result) {
                $scope.InProg = result.InProgressType;
                $("#Job-In-Progress-Modal").modal('show');
            });
        };

        $scope.Completed = {};
        $scope.Completed = function (Id) {
            com.get('/Site/CompletedDetailByJobId/' + Id).then(function (result) {
                $scope.Complete = result.CompletedType;
                $("#Job-Completed-Modal").modal('show');
            });
        };

        $scope.Vehicletypedetail = {};
        $scope.SiteVehicleTypeDetail = function (Id) {
            com.get('/Site/SiteVehicleTypeDetail/' + Id).then(function (result) {
                debugger
                $scope.Vehicletypedetail = result.Sitevehicletypedetail;
                $("#siteVehicleTypeDetail1").modal('show');
            });
           
        };
        $scope.DeleteSiteVehicleTypeDetail = {};
        $scope.DeleteSiteVehicleTypeDetail = function (Id) {
            debugger
            swal({
                title: "Delete?",
                text: `Are you sure to delete this Vehicle Details`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Delete"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/SiteVehicleType/DeleteSiteVehicleType/' + Id).then(function (result) {
                        debugger
                        $scope.getSiteVehicle();
                         });
                }
            });
           
        }

        //$scope.search = function () {
        //    $scope.pageIndex = 1;
        //    $scope.getSiteVehicle();
        //};

        //$scope.clearSearch = function () {
        //    $scope.pageIndex = 1;
        //    $scope.getSiteVehicle();
        //};
    }
})();
