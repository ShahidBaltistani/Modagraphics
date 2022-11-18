(function () {
    'use strict';

    angular
        .module('app')
        .controller('user_list', user_list);

    user_list.$inject = ['$scope', 'com'];

    function user_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.rightHide = false;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { firstName: '', lastName: '', email: '', phone: '' };

        $scope.getUsers = function () {
            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                phone: $scope.searchModel.phone
            };

            com.post("/Approvals/GetUsers", obj).then(function (result) {
                $scope.users = result.Users;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
                mApp.unblockPage();
            });
        }

        $scope.getUsers();

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getUsers();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getUsers();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getUsers();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getUsers();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getUsers();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getUsers();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getUsers();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { firstName: '', lastName: '', email: '', phone: '' };
            $scope.getUsers();
        };

        $scope.approve = function (id) {
            var user = $scope.users.filter(item => item.Id == id)[0];
            swal({
                title: "Approve?",
                text: `Are you sure to approve ${user.FirstName} ${user.LastName}`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Approve"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/Approvals/ApproveUser/' + id).then(function (result) {
                        $scope.getUsers();
                    });
                }
            });
        };

        $scope.reject = function (id) {
            var user = $scope.users.filter(item => item.Id == id)[0];
            swal({
                title: "Reject?",
                text: `Are you sure to reject ${user.FirstName} ${user.LastName}`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Reject"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/Approvals/RejectUser/' + id).then(function (result) {
                        $scope.getUsers();
                    });
                }
            });
        };
    }
})();