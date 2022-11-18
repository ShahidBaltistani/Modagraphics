(function () {
    'use strict';


    angular
        .module('app')
        .controller('site_open', site_open);

    site_open.$inject = ['$scope', 'com'];

    function site_open($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { projectName: '', siteName: '', openingDate: '' };
        $scope.sites = {};

        $('input').first().focus();

        $scope.getProjectList = function () {

            //alert($scope.searchModel.openingDate);

            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                projectName: $scope.searchModel.projectName,
                siteName: $scope.searchModel.siteName,
                //OpeningDate: new Date($scope.searchModel.openingDate),
                openingDate: $scope.searchModel.openingDate == null ? null : $scope.searchModel.openingDate
            };
            //debugger
            mApp.blockPage({ message: "Fetching..." });
            com.post("/Site/GetSitesOpenForBid", obj).then(function (result) {
                $scope.sites = result.sites;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.getProjectList();

        $scope.changeBidStatus = function (id) {
            mApp.blockPage({ message: "Updating..." });
            com.get('/Site/ChangeBidStatus/' + id).then(function (result) {

                swal("Success", "Bidding Status Changed.", "success")
                    .then(function (e) {
                        $scope.getProjectList();
                        //window.location = "/Site/SiteList";
                    });

            });
        };

        $scope.bidModel = { siteId: 0, price: 0.0, comments: '' };

        $scope.bid = function (siteId) {
            ////debugger;
            $scope.bidModel.siteId = siteId;
            $scope.bidModel.price = 0.0;
            $scope.bidModel.comments = '';
            $('#bid-modal').modal('show');
        };

        $scope.saveBid = function () {
            var model = $scope.bidModel;
            $('#bid-modal').modal('hide');
            mApp.blockPage({ message: "Saving Bid..." });
            com.post("/Bid/SaveBid", model).then(function (result) {
                if (result == true) {
                    swal("Success", "Bid has been saved successfully.", "success")
                        .then((e) => $scope.getProjectList());
                }
            });
        };


        $scope.vehicleDetailsModal = function (siteId) {
            //debugger
            $scope.VehiclesType = {};
            com.get('/Installer/GetVehiclesAgainstAssignedSites/' + siteId).then(function (result) {
                $scope.VehiclesType = result.VehiclesAgainstAssignedSites;
                $("#vehicle-details-modal").modal('show');
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
            $scope.getProjectList();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '', openingDate: ''};
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