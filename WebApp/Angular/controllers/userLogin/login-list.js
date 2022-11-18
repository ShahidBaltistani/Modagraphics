(function () {
    'use strict';


    angular
        .module('app')
        .controller('login_list', login_list)

    login_list.$inject = ['$scope', 'com'];

    function login_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { loginName: '', userName: '', roleString: "", email: '', isActive: ''};
        $scope.userLogins = {};

        $scope.getUserLoginList = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                loginName: $scope.searchModel.loginName,
                userName: $scope.searchModel.userName,
                roleString: $scope.searchModel.roleString,
                email: $scope.searchModel.email,
                IsActiveString: $scope.searchModel.isActive
            };
            com.post("/UserLogin/GetUserLogins", obj).then(function (result) {
                $scope.userLogins = result.userLogins;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getUserLoginList();

        $scope.changeStatus = function (id) {
                mApp.blockPage({ message: "Updating..." });
                com.get('/UserLogin/ChangeUserStatus/' + id).then(function (result) {
                    
                    swal("Success", "Status has been Changed.", "success")
                        .then(function (e) {
                            window.location = "/UserLogin/LoginList";
                        });

                });
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getUserLoginList();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            ////debugger;
            $scope.searchModel = { loginName: '', userName: '', roleString: '', email: '', isActive: ''};
            $scope.getUserLoginList();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getUserLoginList();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getUserLoginList();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getUserLoginList();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getUserLoginList();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getUserLoginList();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getUserLoginList();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };
    }
})();