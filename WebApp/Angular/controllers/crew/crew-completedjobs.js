﻿(function () {
    'use strict';


    angular
        .module('app')
        .controller('crew_completedjobs', crew_completedjobs);

    crew_completedjobs.$inject = ['$scope', 'com'];

    function crew_completedjobs($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { Project: '', Site: '', VehicleType: '', VIN: '', LPN: '', UnitNo:''};
        $scope.crews = {};

        $('input').first().focus();

        $scope.Crew = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                ProjectName: $scope.searchModel.Project,
                SiteName: $scope.searchModel.Site,
                VehicleTypeName: $scope.searchModel.VehicleType,
                VIN: $scope.searchModel.VIN,
                LPN: $scope.searchModel.LPN,


                UnitNo: $scope.searchModel.UnitNo
            };
            com.post("/Crew/CompletedJobs", obj).then(function (result) {
                $scope.crews = result.crews;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.Crew();

        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };


        $scope.search = function () {
            
            $scope.pageIndex = 1;
            $scope.Crew();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { Project: '', Site: '', VehicleType: '', VIN: '', LPN: '', UnitNo: '' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.Crew();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.Crew();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.Crew();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.Crew();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.Crew();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.Crew();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.Crew();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

    }
})();