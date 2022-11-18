(function () {
    'use strict';

    angular
        .module('app')
        .controller('change_password', change_password);

    change_password.$inject = ['$scope', 'com'];

    function change_password($scope, com) {
        $scope.orignal = undefined;
        $scope.login = undefined;
        
        
        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/UserLogin/GetUserLogin/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.login = result;
                $scope.login.Password = '';
            });
        };

        $scope.getRecord();

        $scope.update = function () {
            mApp.blockPage({ message: "Updating..." });
            
            com.post('/UserLogin/UpdateUserLogin', $scope.login).then(function (result) {
                swal("Success", "Record has been updated.", "success")
                    .then(function (e) {
                        window.location = "/UserLogin/LoginList";
                    });

            });
        };

        $('input').first().focus();
    }
})();