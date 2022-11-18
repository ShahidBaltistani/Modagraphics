(function () {
    'use strict';

    angular
        .module('app')
        .controller('project_edit', project_edit);

    project_edit.$inject = ['$scope', 'com'];

    function project_edit($scope, com) {
        $scope.orignal = undefined;
        $scope.project = undefined;


        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/Project/GetProject/' + id).then(function (result) {

                $scope.orignal = result;
                $scope.project = result;

                mApp.blockPage({ message: "Loading..." });
                com.get('/Dropdown/GetFleetOwner').then(function (result) {
                    var ctr = $('#drpFleetOwner');
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                        
                    });
                    ctr.select2();
                    ctr.val($scope.project.FleetOwnerId).select2().trigger('change');
                });
                
            });
        };

        $scope.getRecord();

        //Update record
        $scope.update = function () {
            mApp.blockPage({ message: "Updating..." });
            var fleetOwnerId = $('#drpFleetOwner').val();
            $scope.project.FleetOwnerId = parseInt(fleetOwnerId);
            com.post('/Project/UpdateProject', $scope.project).then(function (result) {
                swal("Success", "Record has been updated.", "success")
                    .then(function (e) {
                        window.location = "/Project/ProjectList";
                    });

            });
        };

        
    }
})();