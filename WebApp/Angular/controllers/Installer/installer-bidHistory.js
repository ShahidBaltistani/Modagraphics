(function () {
    'use strict';


    angular
        .module('app')
        .controller('installer_bidHistory', installer_bidHistory);

    installer_bidHistory.$inject = ['$scope', 'com'];

    function installer_bidHistory($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;

        $scope.searchModel = { projectName: '', siteName: '', price: '', bidDate:'' };

        $scope.bidHistories = {};
        $('input').first().focus();

        $scope.getBidHistory = function () {
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                //firstName: "", Commented By Kashif Shahzad
                //lastName: "",  Commented By Kashif Shahzad
                //email: "",
                Company: "",

                FirstName: $scope.searchModel.projectName, // FirstName is used for projectname
                LastName: $scope.searchModel.siteName,     // LastName is used for sitename
                Price: $scope.searchModel.price, // Email is used for Price
                //AwardedDate: new Date($scope.searchModel.bidDate) //AwardedDate is used for biddate
                AwardedDate: $scope.searchModel.bidDate == null ? null : $scope.searchModel.bidDate

            };
            com.post("/Installer/GetBidHistory", obj).then(function (result) {
                $scope.bidHistories = result.BidHistories;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }
        $scope.getBidHistory();


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getBidHistory();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectName: '', siteName: '', price: '', bidDate: ''};
            $scope.getBidHistory();
        };

        selectFirstOption('drpPage');
        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getBidHistory();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getBidHistory();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getBidHistory();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getBidHistory();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getBidHistory();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getBidHistory();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

    }

})();