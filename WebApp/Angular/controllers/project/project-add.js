(function () {
    'use strict';

    angular
        .module('app')
        .controller('project_add', project_add);

    project_add.$inject = ['$scope', 'com'];

    function project_add($scope, com) {
        $scope.project = {
            name: '',
            isPoolVehicle: false,
            fleetOwnerId: 0,
            address: '',
            contactname: '',
            contactphone :''
        };

        $('input').first().focus();

        $scope.save = function () {
            var fleetOwnerId = $('#drpFleetOwner').val();
            if (fleetOwnerId === "0") {
                swal('Required', 'Please select fleet owner.', 'warning');
                mApp.unblockPage();
                return;
            }
            else {
                $scope.project.fleetOwnerId = parseInt(fleetOwnerId);
            }
            mApp.blockPage({ message: "Saving..." });
            com.post('/Project/SaveProject', $scope.project).then(function (result) {
                swal("Success", "Record has been saved.", "success")
                    .then(function (e) {
                        window.location = "/Project/ProjectList";
                    });
            });
        };

        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetFleetOwner').then(function (result) {
            var ctr = $('#drpFleetOwner');
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                
            });
            ctr.select2();
        });
    }
})();