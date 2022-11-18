(function () {
    'use strict';


    angular
        .module('app')
        .controller('supervisor_list', supervisor_list);

    supervisor_list.$inject = ['$scope', 'com'];

    function supervisor_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { SupervisorUserName: '', firstName: '', lastName: '', email: '', phoneNumber: '', Status: '' };
        $scope.supervisors = {};
        $scope.currentSupervisorName = "";
        $scope.cnfrmPssword = '';
        $scope.loginModel = { id: 0, username: '', password: '', associatedId: 0 };

        $('input').first().focus();

        $scope.Supervisor = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                firstName: $scope.searchModel.firstName,
                lastName: $scope.searchModel.lastName,
                email: $scope.searchModel.email,
                Company: $scope.searchModel.companyName,
                phoneNumber: $scope.searchModel.phoneNumber,
                SupervisorUserName: $scope.searchModel.SupervisorUserName
            };
            com.post("/supervisor/SupervisorList", obj).then(function (result) {
                //debugger
                $scope.supervisors = result.supervisors;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.Supervisor();
        ////

      

        $scope.search = function () {
            
            $scope.pageIndex = 1;
            $scope.Supervisor();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { SupervisorUserName: '', firstName: '', lastName: '', email: '', phoneNumber: '', Status: '' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.Supervisor();
        };
        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.Supervisor();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.Supervisor();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.Supervisor();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.Supervisor();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.Supervisor();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.Supervisor();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        $scope.showImage = function (id) {
            mApp.blockPage({ message: "Loading..." });
            var supervisors = $scope.supervisors.filter(item => item.Id == id)[0];
            $scope.imgModal.title = supervisors.FirstName;

            com.get('/Supervisor/GetImage/' + id).then(function (result) {
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
          
            var supervisor = $scope.supervisors.filter(item => item.Id == id)[0];
            $scope.currentSupervisorName = supervisor.FirstName + ' ' + supervisor.LastName;
            $scope.loginModel.associatedId = id;
            debugger
            if (supervisor.IsLoginExists) {
                mApp.blockPage({ message: "Fetching Record..." });
                com.get('/Supervisor/GetLoginInfo/' + supervisor.Id).then(function (result) {
                   
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
            debugger
            if ($scope.loginModel.username == '' || $scope.loginModel.password == '' || $scope.cnfrmPssword == '') {
                swal('Required', 'Please fill all required fields.', 'warning');
            } else if ($scope.loginModel.password != $scope.cnfrmPssword) {
                swal('Required', 'Password deasn\'t match with confirm password.', 'warning');
            } else {
                $('#login-modal').modal('hide');
                mApp.blockPage({ message: "Saving..." });
                com.post('/Supervisor/ManageLogin', $scope.loginModel).then(function (result) {
                    swal('Success', 'Record has been saved successfully.', 'success').then(function (e) {
                        $scope.Supervisor();
                    });
                });
            }
        };


        $scope.SiteDetailModel = function (Id) {
           
            //debugger
            $scope.Sites = {};
            com.get('/supervisor/SiteDetails/' + Id).then(function (result) {
                //debugger
                $scope.Sites = result.SiteDetail;
                $("#Site-details-modal").modal('show');
            });
        };
    }
})();