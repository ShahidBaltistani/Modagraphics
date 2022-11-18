(function () {
    'use strict';


    angular
        .module('app')
        .controller('crew_list', crew_list);

    crew_list.$inject = ['$scope', 'com'];

    function crew_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { CrewUserName: '', firstName: '', lastName: '', email: '', phoneNumber: ''};
        $scope.crews = {};
        //To manage login modal
        $scope.loginModel = { id: 0, username: '', password: '', associatedId: 0 };
        $scope.cnfrmPssword = '';
        $scope.currentCrewName = '';

        $('input').first().focus();

        $scope.Crew = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                PhoneNo: $scope.searchModel.phoneNumber,
                CrewUserName: $scope.searchModel.CrewUserName
            };
            com.post("/Crew/CrewList", obj).then(function (result) {
                debugger
                $scope.crews = result.crews;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.Crew();


        $scope.search = function () {
            
            $scope.pageIndex = 1;
            $scope.Crew();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { CrewUserName: '', firstName: '', lastName: '', email: '', phoneNumber: '' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.Crew();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.Crew();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.Crew();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.Crew();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.Crew();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.Crew();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.Crew();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            var crews = $scope.crews.filter(item => item.Id == id)[0];
            
            $scope.imgModal.title = crews.FirstName;

            com.get('/Crew/GetImage/' + id).then(function (result) {
                $scope.imgModal.src = result;
                mApp.unblockPage();
                if (result === 'null') {
                    swal("", "No image saved for this user.", "warning");
                } else {
                    $("#image-modal").modal('show');
                }
            });
        };

        $scope.showLoginModal = function (id) {
            var crew = $scope.crews.filter(item => item.Id == id)[0];
            $scope.currentCrewName = crew.FirstName + ' ' + crew.LastName;
            $scope.loginModel.associatedId = id;

            if (crew.IsLoginExists) {
                mApp.blockPage({ message: "Fetching Record..." });
                com.get('/Crew/GetLoginInfo/' + crew.Id).then(function (result) {
                    $scope.loginModel.id = result.Id;
                    $scope.loginModel.username = result.Username;
                    $scope.loginModel.password = result.Password;
                    $scope.cnfrmPssword = result.Password;
                    $('#login-modal').modal('show');
                });
            } else {
                $scope.loginModel.id = 0;
                $scope.loginModel.username = '';
                $scope.loginModel.password = '';
                $scope.cnfrmPssword = '';
                $('#login-modal').modal('show');
            }
        };

        $scope.saveLoginInfo = function () {
            if ($scope.loginModel.username == '' || $scope.loginModel.password == '' || $scope.cnfrmPssword == '') {
                swal('Required', 'Please fill all required fields.', 'warning');
            } else if($scope.loginModel.password != $scope.cnfrmPssword) {
                swal('Required', 'Password deasn\'t match with confirm password.', 'warning');
            } else {
                $('#login-modal').modal('hide');
                mApp.blockPage({ message: "Saving..." });
                com.post('/Crew/ManageLogin', $scope.loginModel).then(function (result) {
                    swal('Success', 'Record has been saved successfully.', 'success').then(function (e) {
                        $scope.Crew();
                    });
                });
            }
        };
    }
})();