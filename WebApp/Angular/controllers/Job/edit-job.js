(function () {
    'use strict';

    angular
        .module('app')
        .controller('edit_job', edit_job);

    edit_job.$inject = ['$scope', 'com'];

    function edit_job($scope, com) {
        $scope.orignal = undefined;
        $scope.jobs = undefined;
       
        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/Job/GetJob/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.job = result;
            });
        };

        $scope.getRecord();


        //Update record
        $scope.update = function () {

            mApp.blockPage({ message: "Updating..." });
            com.post('/Job/UpdateJob', $scope.job).then(function (result) {
                swal("Success", "Record has been updated.", "success")
                    .then(function (e) {
                        var site = $('#site').val();
                        var siteVehicle = $('#siteVechcle').val();
                        window.location = "/Job/Jobs/" + $scope.job.SiteVehicleTypeId + "$" + site + "$" + $scope.job.SiteId + "$" + siteVehicle + "$" + false;
                    });

            });



        };

        $('input').not('[type="hidden"]').first().focus();

        $scope.VINError = '';
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
        $scope.Check = function () {
            if ($scope.job.VIN) {
                mApp.blockPage({ message: "Checking VIN..." });
                com.post('/Job/CheckVIN?id=' + $scope.job.Id + "&SiteId=" + $scope.job.SiteId + "&VIN=" + $scope.job.VIN).then(function (result) {
                    if (result) { $scope.VINError = true; }
                    else { $scope.VINError = false; $scope.job.VIN = ''; }
                });
            }else {
                $scope.VINError = null;
            }
        };
    }
})();