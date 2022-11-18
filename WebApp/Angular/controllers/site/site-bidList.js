(function () {
    'use strict';


    angular
        .module('app')
        .controller('site_bidList', site_bidList);

    site_bidList.$inject = ['$scope', 'com'];

    function site_bidList($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;

        $scope.sites = {};


        $scope.searchModel = { installer: '', bidDate: '', bidPrice: ''};

        $('input').first().focus();

        $scope.getProjectList = function () {
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                SiteId: $('#siteId').val(),

                installer: $scope.searchModel.installer,
                bidDate: $scope.searchModel.bidDate == null ? null : $scope.searchModel.bidDate,
                bidPrice: $scope.searchModel.bidPrice
            };
            mApp.blockPage({ message: "Fetching..." });

            com.post("/Site/GetBidList", obj).then(function (result) {
                $scope.sites = result.sites;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }
        $scope.getProjectList();
        selectFirstOption('drpPage');

        //$scope.awardSite = function (id) {
        //        var info = id.split('-');
        //        //alert('site awarded and Bid ID is ' + " " + info[0] + " " + info[1]);
        //        var obj = {
        //            Id: info[0],
        //            SiteId: info[1]
        //        };
        //        com.post("/Site/AwardSite", obj).then(function () {
        //            swal('Succeeded','Site awarded ', 'success');
        //            $scope.getProjectList();
        //        });
        //};
        $scope.awardSite = function (id) {
            var info = id.split('-');
            var obj = {
                Id: info[0],
                SiteId: info[1]
            };
            // For Checking Supervisor Association
            com.post('/Site/AssociationVerification', obj).then(function (result) {
                if (!result) {
                    swal('Error', 'Supervisor not Added for this Site.', 'warning');
                    return false;
                } else {
                    com.post("/Site/AwardSite", obj).then(function () {

                        swal('Succeeded', 'Site awarded ', 'success');
                        $scope.getProjectList();

                    });
                }
            });
            // End
        };

        $scope.rejectSite = function (id) {
                var info = id.split('-');
                var obj = {
                    Id: info[0],
                    SiteId: info[1]
            };
            com.post("/Site/RejectSite", obj).then(function () {
                swal('Succeeded','Site Rejected', 'success');
               
                });
            };


            $scope.search = function () {
                $scope.pageIndex = 1;
                $scope.getProjectList();
            };

            $scope.clearSearch = function () {
                $scope.pageIndex = 1;
                $scope.searchModel = { installer: '', bidDate: '', bidPrice: '' };
                $scope.getProjectList();
            };



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
    }) ();

