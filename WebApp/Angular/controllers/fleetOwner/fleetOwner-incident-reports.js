(function () {
    'use strict';


    angular
        .module('app')
        .controller('fleetOwner_incidentReport', supervisor_incidentReport);

    supervisor_incidentReport.$inject = ['$scope', 'com'];

    function supervisor_incidentReport($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.incidentReports = {};

        $scope.searchModel = { projectName: '', siteName: '', LPN:'' ,VIN:'' ,Unit:'' };

        $scope.getIncidentReports = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,


                FirstName: $scope.searchModel.projectName,
                LastName: $scope.searchModel.siteName,
                LPN: $scope.searchModel.LPN,
                VIN: $scope.searchModel.VIN,
                Unit: $scope.searchModel.Unit
            };
            ////debugger;
            com.post("/FleetOwner/IncidentReportList", obj).then(function (result) {
                $scope.incidentReports = result.incidentReports;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getIncidentReports();

        $scope.Approve = function(id)
        {
            com.get('/FleetOwner/ApproveJob/' + id).then(function () {
                swal("Success", "Job has been Approved,", "success");
                $scope.getIncidentReports();

            });
        }

        $scope.SentForRepair = function (id)
        {
            com.get('/FleetOwner/SentForRepair/' + id).then(function () {
                swal("Success", "Job has been sent for repair.", "success");
                $scope.getIncidentReports();
            });
        }
        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getIncidentReports();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '', LPN: '', VIN: '', Unit: ''};
            $scope.getIncidentReports();
        };


        selectFirstOption('drpPage');
        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getIncidentReports();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getIncidentReports();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getIncidentReports();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getIncidentReports();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getIncidentReports();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getIncidentReports();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            com.get('/Supervisor/GetIncidentImage/' + id).then(function (result) {

                ////debugger

                $scope.imgModal.src = result;
                mApp.unblockPage();
                if (result === 'null') {
                    swal("", "No image saved for this user.", "warning");
                } else {
                    $("#image-modal").modal('show');
                }
            });
        };

    }
})();