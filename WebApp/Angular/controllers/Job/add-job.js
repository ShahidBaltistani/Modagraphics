(function () {
    'use strict';

    angular
        .module('app')
        .controller('add_job', add_job);

    add_job.$inject = ['$scope', 'com'];

    function add_job($scope, com) {
        $scope.job = {
            SiteId: 0,
            VIN:'',
            LPN:'',
            UnitNo:0,
            SiteVehicleTypeId: 0,
        };

        $('input').not('[type="hidden"]').first().focus();

        setTimeout(function () { });
       
        $scope.job.SiteVehicleTypeId = $('#siteVechcleId').val();
      //  //debugger;
        var siteId = $('#siteId').val().split("$");
        $scope.job.SiteId = siteId[2];
        $scope.save = function () {
         //  //debugger;
            mApp.blockPage({ message: "Saving..." });
            com.post('/Job/SaveJob', $scope.job).then(function (result) {
                swal("Success", "Record has been saved.", "success")
                    .then(function (e) {
                        window.location = "/Job/Jobs/" + $('#siteVechcleId').val() + $('#siteId').val() + "$" + false;
                    });
            });
        };

        
        //$scope.VINError='';
        //$scope.Check = function () {
        //    if ($scope.job.VIN == '') {
        //        return false;
        //    }
        //    com.post('/Job/CheckVIN', $scope.job).then(function (result) {
        //        if (!result) {
        //            $scope.job.VIN = null;
        //            $scope.VINError = "VIN with this name already exist.";
        //        } else {
        //            $scope.VINError = null;
        //        }
        //    });
        //};
        $scope.VINError = '';
        $scope.Check = function () {
            if ($scope.job.VIN) {
                mApp.blockPage({ message: "Checking VIN..." });
                com.post('/Job/CheckVIN?id=' + 0 + "&SiteId=" + $scope.job.SiteId + "&VIN=" + $scope.job.VIN).then(function (result) {
                    if (result)
                    {
                        $scope.VINError = true;
                    }else
                    {
                        $scope.VINError = false;
                        $scope.job.VIN = '';
                    }
                });
            } else {
                $scope.VINError = null;
            }
        };
    }
})();