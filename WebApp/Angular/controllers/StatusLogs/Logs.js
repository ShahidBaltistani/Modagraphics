(function () {
    'use strict';


    angular
        .module('app')
        .controller('Logs', Logs);

    Logs.$inject = ['$scope', 'com'];

    function Logs($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { projectName: '', fleetOwnerName: '', siteName: '', po: '', SitesStatus:'3' };
        //$scope.sitesStatus = {};
        $scope.SiteStatusName = 'Pendning';
        $('input').first().focus();


        $scope.getProjectList = function () {

            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                projectName: $scope.searchModel.projectName,
                fleetOwnerName: $scope.searchModel.fleetOwnerName,
                siteName: $scope.searchModel.siteName,
                Po: $scope.searchModel.po,
                SiteStatus: $scope.searchModel.SitesStatus,
            };
            mApp.blockPage({ message: "Fetching..." });
            com.post("/Logs/GetSiteStatusLog", obj).then(function (result) {
                debugger
                $scope.sitesStatus = result.sitesStatus;
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

        $scope.SiteLog = {};
        $scope.ViewLogs = function (Id, SiteName) {
            com.get('/Logs/ViewSiteLogDetail/' + Id).then(function (result) {
                debugger
                $scope.SiteLog = result.logs;
                $("#logTitle").text(SiteName);
                $("#View-Log-Modal").modal('show');
            });
        };


        $scope.JobDetail = {};
        $scope.ViewJobDetails = function (Id) {
            com.get('/Logs/JobDetail/' + Id).then(function (result) {
                debugger
                $scope.JobDetail = result.jobDetail;
                $("#View-JobDetail-Modal").modal('show');
            });
        };


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getProjectList();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', fleetOwnerName: '', siteName: '', po: '', SitesStatus: '3' };
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

        $scope.vehicleDetailsModal = function (siteId) {
            //debugger
            $scope.VehiclesType = {};
            com.get('/Crew/GetVehiclesAgainstAssignedSites/' + siteId).then(function (result) {
                debugger
                $scope.VehiclesType = result.VehiclesAgainstAssignedSites;
                $("#vehicle-details-modal").modal('show');
            });
        };
    }
})();

