(function () {
    'use strict';


    angular
        .module('app')
        .controller('site_list', site_list);

    site_list.$inject = ['$scope', 'com'];

    function site_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { projectName: '', fleetOwnerName: '', siteName: '', po: '' };
        $scope.sites = {};
        $scope.SiteStatusName = 'Pendning';
        $('input').first().focus();
       

        $scope.getProjectList = function () {

            //mApp.blockPage({ message: "Loading..." });
            //com.get('/Dropdown/GetFleetOwner').then(function (result) {
            //    var ctr = $('#drpFleetOwner');
            //    angular.forEach(result, function (value) {
            //        var str = `<option value="${value.Id}">${value.Value}</option>`;
            //        ctr.append(str);

            //    });
            //    ctr.select2();
            //});

          
            //var fleetOwnerId = $('#drpFleetOwner').val();
            //var projectId = $('#drpProject').val();
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                projectName: $scope.searchModel.projectName,
                fleetOwnerName: $scope.searchModel.fleetOwnerName,
                siteName: $scope.searchModel.siteName,
                Po: $scope.searchModel.po,
            };
            mApp.blockPage({ message: "Fetching..." });
            com.post("/Site/GetSites", obj).then(function (result) {
                $scope.sites = result.sites;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.getProjectList();

        //$scope.changeBidStatus = function (id) {
        //    mApp.blockPage({ message: "Updating..." });
        //    com.get('/Site/ChangeBidStatus/' + id).then(function (result) {

        //        swal("Success", "Bidding Status Changed.", "success")
        //            .then(function (e) {
        //                window.location = "/Site/SiteList";
        //            });

        //    });
        //};


        //$scope.changeBidStatus = function (id) {
        //    mApp.blockPage({ message: "Updating..." });
        //    com.get('/Site/ChangeBidStatus/' + id).then(function (result) {

        //        swal("Success", "Bidding Status Changed.", "success")
        //            .then(function (e) {
        //                window.location = "/Site/SiteList";
        //            });

        //    });
        //};

        $scope.IsModificationDisabled  = function (statusID) {
            if (statusID == 0 ) {
                return false;
            }
            else
                return true;
        };
        $scope.StuatsOpenForBid = function (id) {
            mApp.blockPage({ message: "Updating..." });
            com.get('/Site/StuatsOpenForBid/' + id).then(function (result) {

                swal("Success", "Bid Opened for this site.", "success")
                    .then(function (e) {
                        $scope.getProjectList();
                    });

            });
        };

        $scope.StuatsClosedForBid = function (id) {
            mApp.blockPage({ message: "Updating..." });
            com.get('/Site/StuatsClosedForBid/' + id).then(function (result) {

                swal("Success", "Bid Cloesed for this site.", "success")
                    .then(function (e) {
                        $scope.getProjectList();
                    });

            });
        };
        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };

        $scope.Site = {};
        $scope.SiteAgainstProject = function (Id) {
            com.get('/Site/SiteAgainstProject/' + Id).then(function (result) {
                $scope.Site = result.SiteInfo;
                $("#Siteinfo").modal('show');
            });
        };


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getProjectList();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', fleetOwnerName: '', siteName: '', po: '' };
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

        $scope.InQueue = {};
        $scope.InQueue = function (Id) {
            debugger
            com.get('/Site/JobInQueueDetail/' + Id).then(function (result) {
                $scope.JobInQ = result.InQueue;
                $("#Job-In-Queue-Modal").modal('show');
            });
        };

        $scope.InProgress = {};
        $scope.InProgress = function (Id) {
            com.get('/Site/JobInProgressDetail/' + Id).then(function (result) {
                $scope.InProg = result.InProgress;
                $("#Job-In-Progress-Modal").modal('show');
            });
        };

        $scope.Completed = {};
        $scope.Completed = function (Id) {
            com.get('/Site/CompletedJobDetail/' + Id).then(function (result) {
                $scope.Complete = result.Completed;
                $("#Job-Completed-Modal").modal('show');
            });
        };

        $scope.UnderReview = {};
        $scope.UnderReview = function (Id) {
            com.get('/Site/JobUnderReviewDetail/' + Id).then(function (result) {
                $scope.UnderReviews = result.UnderReview;
                $("#Job-Under-Review-Modal").modal('show');
            });
        };

    }
})();

//function getProjects() {
//    mApp.blockPage({ message: "Loading Projects..." });
//    var val = $('#drpFleetOwner').val();
//    $.ajax({
//        url: '/Dropdown/GetProjects/' + val,
//        success: function (data) {
//            var drp = $('#drpProject');
//            //debugger;
//            if (data.length === 0) {
//                var ctr = $('#drpProject');
//                ctr.children().remove();
//                var str = `<option value="0">--Select Fleet Ower First--</option>`;
//                ctr.append(str);
//                ctr.val(0).select2().trigger('change');
//                mApp.unblockPage();
//                return;

//            }
           
//            drp.children().remove();
//            $.each(data, function (index, item) {
//                var str = `<option value="${item.Id}">${item.Value}</option>`;
//                drp.append(str);
//            });
//            //drp.select2('destroy');
//            drp.select2();
//            mApp.unblockPage();
//        },
//        error: function (error) {
//            mApp.unblockPage();
//            swal("Error", error.responseText, "error");
//        }
//    });
//}