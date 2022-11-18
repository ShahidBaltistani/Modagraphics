(function () {
    'use strict';
    angular
        .module('app')
        .controller('installer_assignedSite', installer_assignedSite);

    installer_assignedSite.$inject = ['$scope', 'com'];
    function installer_assignedSite($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.assignedSites = {};

        $scope.searchModel = { projectName: '', siteName: '', awardedDate:'' };

        $scope.getAssignedSites = function () {
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                FirstName: $scope.searchModel.projectName, // FirstName is used for ProjectName
                LastName: $scope.searchModel.siteName,     // LastName is used for SiteName
                //awardedDate: new Date($scope.searchModel.awardedDate)
                awardedDate: $scope.searchModel.awardedDate == null ? null : $scope.searchModel.awardedDate
            };

            com.post("/Installer/GetAssignedSites", obj).then(function (result) {
                debugger
                $scope.assignedSites = result.AssignedSite;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getAssignedSites();
        selectFirstOption('drpPage');

        $scope.CrewsDetailsModal = function (siteId) {
            debugger
            $scope.Items = {};
            com.get('/Installer/GetCrewsAgainstSites/' + siteId).then(function (result) {
                //debugger
                $scope.Items = result.CrewsAgainstSites;
                $("#Crews-details-modal").modal('show');
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
            $scope.getAssignedSites();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '', awardedDate: '' };
            $scope.getAssignedSites();
        };



        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getAssignedSites();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getAssignedSites();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getAssignedSites();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getAssignedSites();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getAssignedSites();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getAssignedSites();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

    }
})();
