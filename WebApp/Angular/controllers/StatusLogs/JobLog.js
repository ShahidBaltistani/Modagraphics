(function () {
    'use strict';


    angular
        .module('app')
        .controller('JobLog', JobLog);

    JobLog.$inject = ['$scope', 'com'];

    function JobLog($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { ProjectName: '', SiteName: '', JobStatus: '2' };
        //$scope.jobsStatus = {};
        $scope.SiteStatusName = 'Pendning';
        $('input').first().focus();


        $scope.getProjectList = function () {
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                VehicleTypeName: $scope.searchModel.VehicleTypeName,
                VIN: $scope.searchModel.VIN,
                LPN: $scope.searchModel.LPN,
                SiteId: $scope.SiteId,
                JobStatus: $scope.searchModel.JobStatus,
                ProjectName: $scope.searchModel.ProjectName,
                SiteName: $scope.searchModel.SiteName,
            };
            mApp.blockPage({ message: "Fetching..." });
            com.post("/Logs/GetJobDetail", obj).then(function (result) {
                debugger
                $scope.crews = result.crews;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });

        }

        //Calling to load fleet owner list on first time load
        $scope.getProjectList();



        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };

        $scope.JobLog = {};
        $scope.ViewLogs = function (Id) {
            debugger
            com.get('/Logs/ViewJobLogDetail/' + Id).then(function (result) {
                debugger
                $scope.JobLog = result.JobLogs;
                $("#View-JobLog-Modal").modal('show');
            });
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getProjectList();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', fleetOwnerName: '', siteName: '', po: '', JobStatus: '2' };
            $scope.getProjectList();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getProjectList();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getProjectList();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getProjectList();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getProjectList();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getProjectList();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getProjectList();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };



    }
})();

