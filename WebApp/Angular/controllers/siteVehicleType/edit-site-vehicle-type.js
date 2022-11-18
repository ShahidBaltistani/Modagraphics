

(function () {
    'use strict';

    angular
        .module('app')
        .controller('edit_site_vehicle_type', edit_site_vehicle_type );

    edit_site_vehicle_type.$inject = ['$scope', 'com'];

    function edit_site_vehicle_type($scope, com) {
        $scope.orignal = undefined;
        $scope.siteVehicle = undefined;


        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/SiteVehicleType/GetSiteVehicle/' + id).then(function (result) {

                $scope.orignal = result;
                $scope.siteVehicle = result;
                mApp.blockPage({ message: "Loading..." });
                com.get('/Dropdown/GetVehicleTypes').then(function (result) {
                    var ctr = $('#vehicleType');
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);

                    });
                    ctr.select2();
                    ctr.val($scope.siteVehicle.VehicleTypeId).select2().trigger('change');
                    ctr.on('change', getVehicleSpecification);
                });

                mApp.blockPage({ message: "Loading..." });
                com.get('/Dropdown/GetVehicleSpecification/' + $scope.siteVehicle.VehicleTypeId).then(function (result) {
                    var ctr = $('#vehicleSpecification');
                    if ($scope.siteVehicle.VehicleSpecificationId != 0) {
                        angular.forEach(result, function (value) {
                            var str = `<option value="${value.Id}">${value.Value}</option>`;
                            ctr.append(str);
                        });
                        ctr.val($scope.siteVehicle.VehicleSpecificationId).select2().trigger('change');
                    }
                    else {
                        ctr.children().remove();
                        var str = `<option value="0">--No Item Found--</option>`;
                        ctr.append(str);
                        ctr.select2();
                        ctr.val(0).select2().trigger('change');
                    }
                   
                    });

            });
        };

        $scope.getRecord();

        //Update record
        $scope.update = function () {
            mApp.blockPage({ message: "Updating..." });
            var vehicleType = $('#vehicleType').val();
            var vehicleSpecification = $('#vehicleSpecification').val();
            $scope.siteVehicle.VehicleSpecificationId = parseInt(vehicleSpecification);
            $scope.siteVehicle.VehicleTypeId = parseInt(vehicleType);
            $scope.siteVehicle.VehiclePrice = $('#vehiclePrice').val();
            com.post('/SiteVehicleType/UpdateSiteVehicleType', $scope.siteVehicle).then(function (result) {
                swal("Success", "Record has been updated.", "success")
                    .then(function (e) {
                        ////debugger;
                        window.location = "/SiteVehicleType/Types/" + $scope.siteVehicle.SiteId + "$" + $scope.siteVehicle.SiteName + "$"+false;
                    });

            });
        };


    }
})();

function getVehicleSpecification() {
    mApp.blockPage({ message: "Loading vehicle specification..." });
    var val = $('#vehicleType').val();
    $.ajax({
        url: '/Dropdown/GetVehicleSpecification/' + val,
        success: function (data) {
            var drp = $('#vehicleSpecification');
            if (data.length === 0) {
                drp.children().remove();
                var str = `<option value="0">--No Item Found--</option>`;
                drp.append(str);
                drp.select2();
                mApp.unblockPage();
                return;
            }
            drp.children().remove();
            $.each(data, function (index, item) {
                var str = `<option value="${item.Id}">${item.Value}</option>`;
                drp.append(str);
            });
            drp.select2();
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });

    mApp.blockPage({ message: "Loading vehicle price..." });
    $.ajax({
        url: '/SiteVehicleType/GetBasePrice/' + val,
        success: function (data) {
            $('#vehiclePrice').val(data);
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });
}