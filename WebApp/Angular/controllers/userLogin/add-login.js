(function () {
    'use strict';

    angular
        .module('app')
        .controller('add_login', add_login);

    add_login.$inject = ['$scope', 'com'];

    function add_login($scope, com) {
        $scope.login = {
            AssociatedId: 0,
            UserName:'',
            Password:'',
            IsActive: true,
            UserRole: '-1',
            ConfirmPassword:''
        };


        $scope.save = function () {
            var userType = $('#drpusertype').val();
            var userId = $('#drpusers').val();
            if (userType === "-1") {
                swal('Required', 'Please select User Type.', 'warning');
                mApp.unblockPage();
                return;
            }
            else {
                $scope.login.UserRole = userType;
                $scope.login.AssociatedId = userId;
            }
            mApp.blockPage({ message: "Checking User Name..." });
            com.get('/UserLogin/CheckUserName/0/' + $scope.login.UserName).then(function (result) {
                if (!result) {
                    swal('Error', 'The user with this name already exist.', 'warning');
                }
                else {
                    mApp.blockPage({ message: "Saving..." });
                    com.post('/UserLogin/SaveUserLogin', $scope.login).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
                            .then(function (e) {
                                window.location = "/UserLogin/LoginList";
                            });
                    });
                }
            });



           
        };

        //$scope.GetUsers = function () {
           
        //    switch ($scope.login.UserRole) {
        //        case '-1': {
        //            var ctr = $('#drpusers');
        //            ctr.children().remove();
        //            ctr.append('<option value="0">--Select User Type First--</option>');
        //            ctr.select2();
        //            break;
        //        }
        //        case '0': {
        //            $scope.FillUser('GetDataEntryOperator');
        //            break;
        //        }
        //        case '1': {
        //            $scope.FillUser('GetApprovalManager');
        //            break;
        //        }
        //        case '2': {
        //            $scope.FillUser('GetFinanceManager');
        //            break;
        //        }
        //        case '3': {
        //            $scope.FillUser('GetCorporateUser');
        //            break;
        //        }
        //        case '4': {
        //            $scope.FillUser('GetInstaller');
        //            break;
        //        }
        //        case '5': {
        //            $scope.FillUser('GetFleetOwner');
        //            break;
        //        }
        //        case '6': {
        //            $scope.FillUser('GetCrew');
        //            break;
        //        }

        //    }
        //};
        $scope.GetUsers = function () {

            switch ($scope.login.UserRole) {
                case '-1': {
                    var ctr = $('#drpusers');
                    ctr.children().remove();
                    ctr.append('<option value="0">--Select User Type First--</option>');
                    ctr.select2();
                    break;
                }
                case '0': {
                    $scope.FillUser('GetDataEntryOperatorForAddLogin');
                    break;
                }
                case '1': {
                    $scope.FillUser('GetApprovalManagerForAddLogin');
                    break;
                }
                case '2': {
                    $scope.FillUser('GetFinanceManagerForAddLogin');
                    break;
                }
                case '3': {
                    $scope.FillUser('GetCorporateUserForAddLogin');
                    break;
                }
                case '4': {
                    $scope.FillUser('GetInstallerForAddLogin');
                    break;
                }
                case '5': {
                    $scope.FillUser('GetFleetOwnerForAddLogin');
                    break;
                }
                //case '6': {
                //    $scope.FillUser('GetCrewForAddLogin');
                //    break;
                //}
            }
        };

        $scope.FillUser = function (user) {
            mApp.blockPage({ message: "Loading..." });
            com.get('/Dropdown/' + user).then(function (result) {
                var ctr = $('#drpusers');
                ctr.children().remove();
                if (result.length == 0) {
                    ctr.append('<option value="0">--No Record--</option>');
                    return;
                }
                
                angular.forEach(result, function (value) {
                    var str = `<option value="${value.Id}">${value.Value}</option>`;
                    ctr.append(str);
                    
                });
                ctr.select2();
            });
        };
    }
})();