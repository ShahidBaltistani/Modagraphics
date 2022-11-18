(function () {
    'use strict';


    angular
        .module('app')
        .controller('approve_job', approve_job);

    approve_job.$inject = ['$scope', 'com'];

    function approve_job($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        //$scope.imgModal = { title: '', src: '' };
        $scope.imgModal = {};
        $scope.jobs = {};
        $scope.jobID = 0;
        $scope.comments = '';

        $scope.searchModel = { installer: '', siteName: '', LPN: '', VIN: '', assignDate: '' };

        $scope.getJobs = function () {
            ////debugger;
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,


                FirstName: $scope.searchModel.installer,
                LastName: $scope.searchModel.siteName,
                LPN: $scope.searchModel.LPN,
                VIN: $scope.searchModel.VIN,
                assignDate: $scope.searchModel.assignDate == null ? null : $scope.searchModel.assignDate
            };

            com.post("/FleetOwner/GetJobsWaitingForApproval", obj).then(function (result) {
                $scope.jobs = result.Jobs;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getJobs();

        $scope.showImage = function (id) {
            ////debugger;
            mApp.blockPage({ message: "Loading..." });
            $scope.jobID = id;
            com.get('/FleetOwner/GetApprovalImages/' + id).then(function (result) {
                $scope.imgModal = result;
                mApp.unblockPage();
                if (result === 'null') {
                    swal("", "No image found . .", "warning");
                } else {
                    $("#image-modal").modal('show');
                }
            });
        };

        $scope.ApproveJob = function (id) {
            if (!id) {
                id = $scope.jobID;
            }
          //  //debugger;
            mApp.blockPage({ message: "Loading..." });
            com.get('/Approvals/ApproveJobWaiting/' + id).then(function (result) {
                swal("", "Job Approved !!", "success");
                $scope.getJobs();
                $scope.jobID = 0;
                $("#image-modal").modal('hide');
            });
        };

        $scope.rejectModel = { Id: 0, comments: '' };
        $scope.RejectJob = function (id) {
            $("#reject-modal").modal('show');
            $scope.rejectModel.Id = id;
            $scope.rejectModel.comments = '';
            ////debugger;
            //var temp_comment = $scope.comments;
        };


        $scope.Reject = function () {
            $("#reject-modal").modal('hide');
            ////debugger;
            com.post('/Approvals/RejectJob', $scope.rejectModel).then(function (result) {
                ////debugger;
                swal('', 'Job Rejected', 'success');
                $scope.rejectModel.Id = 0;
                $scope.rejectModel.comments = '';
                $scope.getJobs();
            });
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getJobs();
        };
        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { installer: '', siteName: '', LPN: '', VIN: '', assignDate: ''  };
            $scope.getJobs();
        };
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
    }
})();