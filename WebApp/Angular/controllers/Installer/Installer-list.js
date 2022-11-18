(function () {
    'use strict';


    angular
        .module('app')
        .controller('installer_list', installer_list);

    installer_list.$inject = ['$scope', 'com'];

    function installer_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { firstName: '', lastName: '', email: '', companyName: '' };
        $scope.installers = {};
        $('input').first().focus();


        $scope.getInstaller = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                Company: $scope.searchModel.companyName
            };
            com.post("/Installer/GetInstallers", obj).then(function (result) {
                $scope.installers = result.Installers;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.getInstaller();


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getInstaller();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { firstName: '', lastName: '', email: '', companyName: ''};
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.getInstaller();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getInstaller();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getInstaller();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getInstaller();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getInstaller();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getInstaller();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getInstaller();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            var installers = $scope.installers.filter(item => item.Id == id)[0];
            $scope.imgModal.title = installers.FirstName;

            com.get('/Installer/GetImage/' + id).then(function (result) {
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