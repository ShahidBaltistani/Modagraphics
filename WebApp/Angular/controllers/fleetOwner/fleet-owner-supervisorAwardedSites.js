(function () {
    'use strict';


    angular
        .module('app')
        .controller('fleetOwner_awardedSites', fleetOwner_awardedSites);

    fleetOwner_awardedSites.$inject = ['$scope', 'com'];
    function fleetOwner_awardedSites($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;

        $scope.awardedSites = {};
        $scope.siteId = 0;
        $scope.selected = 1;

        $scope.searchModel = { projectName: '', siteName: '' };

        $scope.getAwardedSites = function () {
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,


                FirstName: $scope.searchModel.projectName,
                LastName: $scope.searchModel.siteName,
            }
            com.post("/FleetOwner/GetSupervisorAwardedSites", obj).then(function (result) {
               debugger
                $scope.awardedSites = result.AwardedSites;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });

        }
        $scope.getAwardedSites();

        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetSupervisorForSiteAssigning').then(function (result) {
            var ctr = $('#drpSupervisor');
            //ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                ctr.select2();
            });
        });

        //mApp.blockPage({ message: "Loading..." });
        //com.get('/Dropdown/GetSupervisor').then(function (result) {
        //    var ctr = $('#drpSupervisor');
        //    //ctr.children().remove();
        //    angular.forEach(result, function (value) {
        //        var str = `<option value="${value.Id}">${value.Value}</option>`;
        //        ctr.append(str);
        //        ctr.select2();
        //    });
        //});

        $scope.Assign = function (siteId, SiteName, SiteSupervisors) {
            $scope.siteId = siteId;
            $scope.selected = 1;
            debugger;
            $("#SupervisorModalTitle").text(SiteName);
            $("#drpSupervisor").val("");
            $.each(SiteSupervisors, function (i, e) {
                $("#drpSupervisor option[value='" + e + "']").prop("selected", true);
            });
            $("#drpSupervisor").trigger("change");
            $('#supervisor-modal').modal('show');
        };

        $scope.AssignSite = function () {
            var CrewId = $('#drpSupervisor').val();
            if (CrewId.length == 0) {
                $scope.selected = 0;
            } else {
                $('#supervisor-modal').modal('hide');
                var obj = { MainId: $scope.siteId, ChildIds: CrewId };
                mApp.blockPage({ message: "Assigning..." });
                com.post('/FleetOwner/AssignSite', obj).then(function (result) {
                    var ctr = $('#drpSupervisor');
                    $scope.getAwardedSites();
                });
            }
        };
        selectFirstOption('drpPage');
        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };



        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getAwardedSites();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '' };
            $scope.getAwardedSites();
        };


        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getAwardedSites();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getAwardedSites();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getAwardedSites();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getAwardedSites();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getAwardedSites();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getAwardedSites();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        //$scope.vehicleDetailsModal = function (Id) {
        //    //debugger
        //    $scope.VehiclesType = {};
        //    com.get('/FleetOwner/GetVehiclesAgainstAssignedSites/' + Id).then(function (result) {
        //        $scope.VehiclesType = result.VehiclesAgainstAssignedSites;
        //        $("#vehicle-details-modal").modal('show');
        //    });
        //};


        $scope.vehicleDetailsModal = function (Id) {
            //debugger
            $scope.VehiclesType = {};
            com.get('/Installer/GetVehiclesAgainstAssignedSites/' + Id).then(function (result) {
                $scope.VehiclesType = result.VehiclesAgainstAssignedSites;
                $("#vehicle-details-modal").modal('show');
            });
        };
    }
})();