(function () {
    'use strict';

    angular
        .module('app')
        .controller('user_list', user_list);

    user_list.$inject = ['$scope', 'com'];

    function user_list($scope, com) {
        ////debugger;
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.rightHide = false;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { firstName: '', lastName: '', email: '', phone: '' };

        $scope.getUsersTypes = function () {
            ////debugger;
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                phone: $scope.searchModel.phone
            };

            com.post("/User/GetUsers", obj).then(function (result) {
                $scope.users = result.Users;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getUsersTypes();

        $('input').first().focus();

        selectFirstOption('drpPage');
        

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getUsersTypes();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getUsersTypes();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getUsersTypes();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getUsersTypes();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getUsersTypes();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getUsersTypes();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            var user = $scope.users.filter(item => item.Id == id)[0];
            $scope.imgModal.title = user.FirstName + ' ' + user.LastName;

            com.get('/User/GetImage/' + id).then(function (result) {
                $scope.imgModal.src = result;
                if (result === 'null') {
                    swal("", "No image saved for this user.", "warning");
                } else {
                    $("#image-modal").modal('show');
                }
            });
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getUsersTypes();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { firstName: '', lastName: '', email: '', phone: '' };
            $scope.getUsersTypes();
        };

        $scope.getEditLink = function (status, id) {
            ////debugger;
            return '';
        };
    }
})();
