(function () {
    'use strict';

    angular
        .module('app')
        .controller('add_site_vehicle_type', add_site_vehicle_type);

    add_site_vehicle_type.$inject = ['$scope', 'com'];

    function add_site_vehicle_type($scope, com) {
        ////debugger;
        $scope.siteVehicle = {
            VehicleTypeId: 0,
            SiteId: 0,
            SiteName : "",
            VehiclePrice : 0,
            VehicleSpecificationId: 0,
            SideHeight: 0,
            SideWidth: 0,
            FrontHeight: 0,
            FrontWidth:0,
            RearHeight:0,
            RearWidth: 0,
        };


        $scope.siteVehicle.SiteId = $('#siteId').val();
        $scope.save = function () {
            ////debugger;
            var VehicleId = $('#vehicleType').val();
            var SpecificationId = $('#vehicleSpecification').val();
            
            if (VehicleId == 0) {
                swal('Required', 'Please select Vehicle Type .', 'warning');
                return;
            }
            $scope.siteVehicle.VehicleSpecificationId = parseInt(SpecificationId);
            $scope.siteVehicle.VehicleTypeId = parseInt(VehicleId);
            $scope.siteVehicle.VehiclePrice = $('#vehiclePrice').val();

            mApp.blockPage({ message: "Saving..." });
            com.post('/SiteVehicleType/SaveSiteVehicleType', $scope.siteVehicle).then(function (result) {
                swal("Success", "Record has been saved.", "success")
                    .then(function (e) {

                        $scope.siteVehicle.SiteName = $('#siteName').val();
                        window.location = "/SiteVehicleType/Types/" + $scope.siteVehicle.SiteId + "$" + $scope.siteVehicle.SiteName + '$' + false;//"/Site/SiteList";
                    });
            });
        };

        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetVehicleTypes').then(function (result) {
            debugger
            var ctr = $('#vehicleType');
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);

            });
            ctr.select2();
        });

    }
})();


function getVehicleSpecification() {
    debugger
    mApp.blockPage({ message: "Loading vehicle specification..." });
    var val = $('#vehicleType').val();
    var drp = $('#vehicleSpecification');
    if (val === '0') {
        drp.children().remove();
        var str = `<option value="0">--First Select Vehicle Type--</option>`;
        drp.append(str);
        drp.select2();
        mApp.unblockPage();
        return;
    }
    $.ajax({
        url: '/Dropdown/GetVehicleSpecification/' + val,
        success: function (data) {
            debugger
            drp = $('#vehicleSpecification');
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
        success: function(data) {
            debugger
            $('#vehiclePrice').val(data);
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });

    //Type
    
}