(function () {
    'use strict';

    angular
        .module('app')
        .controller('vehicle_type_list', vehicle_type_list);

    vehicle_type_list.$inject = ['$scope', 'com'];

    function vehicle_type_list($scope, com) {

        var step = 4;
        $scope.maxSize = 6;     
        $scope.totalCount = 0;  
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5; 
        $scope.numOfPages = 1;
        $scope.rightHide = false;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { vType: '', basePrice: 0, isActive: 'undefine' };
        $scope.specsModal = { title: '', specs: [{ Specification: '', Value: 0 }] };

        

        $scope.getVehicleTypes = function () {
            mApp.blockPage({ message: "Fetching..." });

            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                vType: $scope.searchModel.vType,
                basePrice: $scope.searchModel.basePrice,
                isActive: $scope.searchModel.isActive === "true" ? true : false,
                isAll: $scope.searchModel.isActive === 'undefine'
            };

            com.post("/Vehicle/GetVehicles", obj).then(
                function (result) {
                    $scope.vehicles = result.Vehicles;
                    $scope.totalCount = result.TotalRecords;
                    $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                    $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                    $scope.rightHide = $scope.numOfPages > $scope.maxSize;
                });
        }

        $scope.getVehicleTypes();

        $('input').first().focus();
        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getVehicleTypes();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getVehicleTypes();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getVehicleTypes();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getVehicleTypes();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getVehicleTypes();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getVehicleTypes();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            var vehicle = $scope.vehicles.filter(item => item.Id == id)[0];
            $scope.imgModal.title = vehicle.VehicleType;

            com.get('/Vehicle/GetImage/' + id).then(function (result) {
                $scope.imgModal.src = result;
                if (result === 'null') {
                    swal("", "No image saved for this vehicle type.", "warning");
                } else {
                    $("#image-modal").modal('show');
                }
            });
        };

        $scope.showSpecs = function (id) {
            var vehicle = $scope.vehicles.filter(item => item.Id == id)[0];
            $scope.specsModal.title = vehicle.VehicleType;
            $scope.specsModal.specs = vehicle.Specifications;

            $('#spec-modal').modal('show');
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getVehicleTypes();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { vType: '', basePrice: 0, isActive: 'undefine' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.getVehicleTypes();
        };
    }
})();
