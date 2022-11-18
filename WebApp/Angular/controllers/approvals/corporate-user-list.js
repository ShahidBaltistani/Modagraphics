(function () {
    'use strict';

    angular
        .module('app')
        .controller('corporate_user_list', corporate_user_list);

    corporate_user_list.$inject = ['$scope', 'com'];

    function corporate_user_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.rightHide = false;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { firstName: '', lastName: '', email: '', phoneNumber: '' };

        $('input').first().focus();

        $scope.getCorporateUsers = function () {
            mApp.blockPage({ message: "Fetching..." });

            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                phoneNumber: $scope.searchModel.phoneNumber
            };

            com.post("/Approvals/GetCorporateUsers", obj).then(function (result) {
                $scope.corporateUsers = result.CorporateUsers;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        };

        $scope.getCorporateUsers();

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getCorporateUsers();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getCorporateUsers();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getCorporateUsers();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getCorporateUsers();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getCorporateUsers();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getCorporateUsers();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getCorporateUsers();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { firstName: '', lastName: '', email: '', phoneNumber: '' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.getCorporateUsers();
        };

        $scope.approve = function (id) {
            var user = $scope.corporateUsers.filter(item => item.Id == id)[0];
            swal({
                title: "Approve?",
                text: `Are you sure to approve ${user.FirstName} ${user.LastName}`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Approve"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/Approvals/ApproveCorporateUser/' + id).then(function (result) {
                        $scope.getCorporateUsers();
                    });
                }
            });
        };

        $scope.reject = function (id) {
            var user = $scope.corporateUsers.filter(item => item.Id == id)[0];
            swal({
                title: "Reject?",
                text: `Are you sure to reject ${user.FirstName} ${user.LastName}`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Reject"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/Approvals/RejectCorporateUser/' + id).then(function (result) {
                        $scope.getCorporateUsers();
                    });
                }
            });
        };
    }
})();