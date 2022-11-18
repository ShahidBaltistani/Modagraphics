(function () {
    'use strict';
    angular
        .module('app')
        .controller('installer_awardedSites', installer_awardedSites);

    installer_awardedSites.$inject = ['$scope', 'com'];
    function installer_awardedSites($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.awardedSites = {};
        $scope.siteId = 0;
        $scope.selected = 1;

        $scope.searchModel = { projectName: '', siteName: '', awardedDate:''};

        $scope.getAwardedSites = function () {
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,


                FirstName: $scope.searchModel.projectName, // FirstName is used for ProjectName
                LastName: $scope.searchModel.siteName,     // LastName is used for SiteName
                //awardedDate: new Date($scope.searchModel.awardedDate)
                awardedDate: $scope.searchModel.awardedDate == null ? null : $scope.searchModel.awardedDate
            }

            com.post("/Installer/GetAwardedSites", obj).then(function (result) {
                
                $scope.awardedSites = result.AwardedSites;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });

        }
        $scope.getAwardedSites();

        $scope.vehicleDetailsModal = function (siteId) {
            //debugger
            $scope.VehiclesType = {};
            com.get('/Installer/GetVehiclesAgainstAwardedSites/' + siteId).then(function (result) {
                $scope.VehiclesType = result.VehiclesAgainstAssignedSites;
                $("#vehicle-details-modal").modal('show');
            });
        };

        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetCrewForAssigningSite').then(function (result) {
            var ctr = $('#drpCrew');
            //ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                ctr.select2();
            });
        });

        //mApp.blockPage({ message: "Loading..." });
        //com.get('/Dropdown/GetCrew').then(function (result) {
        //    var ctr = $('#drpCrew');
        //    //ctr.children().remove();
        //    angular.forEach(result, function (value) {
        //        var str = `<option value="${value.Id}">${value.Value}</option>`;
        //        ctr.append(str);
        //        ctr.select2();
        //    });
        //});

        $scope.Assign = function (siteId, SiteName) {
            $scope.siteId = siteId;
            $scope.selected = 1;
            $("#CrewModalTitle").text(SiteName);
            $("#drpCrew").val("");
            $("#drpCrew").trigger("change");
            $('#crew-modal').modal('show');
        };

        $scope.AssignSite = function () {
            var CrewId = $('#drpCrew').val();
            if (CrewId.length == 0) {
                $scope.selected = 0;
            } else {
                $('#crew-modal').modal('hide');
                var obj = { MainId: $scope.siteId, ChildIds: CrewId };
                mApp.blockPage({ message: "Assigning..." });
                com.post('/Installer/AssignSite', obj).then(function (result) {
                    var ctr = $('#drpCrew');
                     $scope.getAwardedSites();
                });
            }
            //alert(CrewId);
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
            $scope.searchModel = { projectName: '', siteName: '', awardedDate: '' };
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
            debugger
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getAwardedSites();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

    }

})();