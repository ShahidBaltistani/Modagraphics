(function () {
    'use strict';

    angular
        .module('app')
        .controller('job_list', job_list);

    job_list.$inject = ['$scope', 'com'];

    function job_list($scope, com) {

        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.rightHide = false;
        $scope.searchModel = { VIN: '', LPN: '', UnitNo: 0, StatusString: '' };

        $('input').not('[type="hidden"]').first().focus();


        $scope.getJobs = function () {
            mApp.blockPage({ message: "Fetching..." });

            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                VIN: $scope.searchModel.VIN,
                LPN: $scope.searchModel.LPN,
                UnitNo: $scope.searchModel.UnitNo,
                StatusString: $scope.searchModel.StatusString,
                JobStatus: $scope.searchModel.StatusString,
                SiteVehicleTypeId: $('#siteVechcleId').val()
            };
           // debugger;
            com.post("/Job/GetJobs", obj).then(
                function (result) {
                    $scope.jobs = result.jobs;
                    $scope.totalCount = result.TotalRecords;
                    $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                    $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                    $scope.rightHide = $scope.numOfPages > $scope.maxSize;
                });
        }

        $scope.getJobs();

       

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getJobs();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getJobs();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getJobs();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getJobs();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getJobs();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getJobs();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getJobs();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { VIN: '', LPN: '', UnitNo: 0, StatusString: ''};
            $scope.getJobs();


            $scope.searchModel.StatusString = "";
            $("#StatusString").val("").change();
        };
        
    }
})();