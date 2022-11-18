(function () {
    'use strict';


    angular
        .module('app')
        .controller('fleet_owner_list', fleet_owner_list);

    fleet_owner_list.$inject = ['$scope', 'com'];

    function fleet_owner_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { corporateUser: '',firstName: '', lastName: '', email: '', companyName: '', phoneNumber: '' };
        $scope.fleetOwners = {};

        $('input').first().focus();

        $scope.getFleetOwners = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                Company: $scope.searchModel.companyName,
                PhoneNo: $scope.searchModel.phoneNumber,
                CorporateUserName: $scope.searchModel.corporateUser
            };
            com.post("/FleetOwner/FleetOwnerList", obj).then(function (result) {
                //debugger
                $scope.fleetOwners = result.fleetOwners;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.getFleetOwners();


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getFleetOwners();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { corporateUser: '',firstName: '', lastName: '', email: '', companyName: '', phoneNumber: '' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.getFleetOwners();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getFleetOwners();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getFleetOwners();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getFleetOwners();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getFleetOwners();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getFleetOwners();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getFleetOwners();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            var fleetOwners = $scope.fleetOwners.filter(item => item.Id == id)[0];
            $scope.imgModal.title = fleetOwners.FirstName;

            com.get('/FleetOwner/GetImage/' + id).then(function (result) {
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