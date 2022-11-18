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
        $scope.searchModel = { firstName: '', lastName: '', email: '', companyName: '', phoneNumber: '' };
        $scope.fleetOwners = {};

        $scope.getFleetOwners = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                Company: $scope.searchModel.companyName,
                PhoneNo: $scope.searchModel.phoneNumber
            };

            com.post("/Approvals/GetFleetOwners", obj).then(function (result) {
                $scope.fleetOwners = result.FleetOwners;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        $scope.getFleetOwners();


        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getFleetOwners();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { firstName: '', lastName: '', email: '', companyName: '', phoneNumber: '' };
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

        $scope.approve = function (id) {
            var fleetOwner = $scope.fleetOwners.filter(item => item.Id == id)[0];
            swal({
                title: "Approve?",
                text: `Are you sure to approve ${fleetOwner.FirstName} ${fleetOwner.LastName}`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Approve"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/Approvals/ApproveFleetOwner/' + id).then(function (result) {
                        $scope.getFleetOwners();
                    });
                }
            });
        };

        $scope.reject = function (id) {
            var fleetOwner = $scope.fleetOwners.filter(item => item.Id == id)[0];
            swal({
                title: "Reject?",
                text: `Are you sure to reject ${fleetOwner.FirstName} ${fleetOwner.LastName}`,
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Reject"
            }).then(function (e) {
                if (e.value) {
                    mApp.blockPage({ message: "Updating..." });
                    com.get('/Approvals/RejectFleetOwner/' + id).then(function (result) {
                        $scope.getFleetOwners();
                    });
                }
            });
        };
    }
})();