(function () {
    'use strict';

    angular
        .module('app')
        .controller('edit_vehicle_type', edit_vehicle_type);

    edit_vehicle_type.$inject = ['$scope', 'com'];

    function edit_vehicle_type($scope, com) {
        $scope.orignal = undefined;
        $scope.vehicle = undefined;

        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/Vehicle/GetVehicle/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.vehicle = result;
                angular.forEach($scope.vehicle.Specifications, function (value) {
                    $scope.addPrevSpec(value.Specification, value.Value);
                });
            });
        };

        $scope.getRecord();

        $scope.update = function () {
            mApp.blockPage({ message: "Updating..." });
          
            $scope.vehicle.Specifications = [];
            var isError = false;
            var divs = $('#specsDiv').children();
            $.each(divs, function (index, item) {
                var inputs = $(item).find('input');
                var spec = inputs[0].value;
                var value = parseInt(inputs[1].value);
                if (spec !== "" && value) {
                    var specification = { Specification: spec, Value: value };
                    $scope.vehicle.Specifications.push(specification);
                }
                else {
                    $scope.vehicle.Specifications = [];
                    swal('Required', 'Please enter complete specification', 'warning');
                    mApp.unblockPage();
                    isError = true;
                }
            });
            
            if (isError) return;

            var imgStr = $scope.vehicle.ImageString;
            $scope.vehicle.ImageString = imgStr === 'null' ? null : imgStr.substring(imgStr.indexOf(',') + 1);

            com.post('/Vehicle/UpdateVehicleType', $scope.vehicle).then(function (result) {
                swal("Success", "Record has been updated.", "success")
                    .then(function (e) {
                        window.location = "/Vehicle/Types";
                    });
            }, function () {
                if ($scope.vehicle.ImageString !== null && $scope.vehicle.ImageString !== 'null') {
                    $scope.vehicle.ImageString = 'data:image/jpg;base64,' + $scope.vehicle.ImageString;
                } else if ($scope.vehicle.ImageString === null) {
                    $scope.vehicle.ImageString = 'null';
                }
            });
        };

        $scope.addSpec = function () {
            var ctrl = '<div class="row form-group m-form__group"><div class="col-5"><input class="form-control m-input" /></div>';
            ctrl += '<div class="col-5"><input type="number" class="form-control m-input" /></div>';
            ctrl += '<div class="col-2" style="padding:0"><button style="padding:.65rem 1rem" onclick="removeSpec(this)" ';
            ctrl += 'class="btn btn-danger m-btn m-btn--custom m-btn--icon m-btn--pill m-btn--air"><i class="fa fa-times"></i>';
            ctrl += '</button></div></div>';
            $('#specsDiv').append(ctrl);
        }

        $scope.addPrevSpec = function (spec, val) {
            var ctrl = `<div class="row form-group m-form__group"><div class="col-5"><input class="form-control m-input" value="${spec}" /></div>`;
            ctrl += `<div class="col-5"><input type="number" class="form-control m-input" value="${val}" /></div>`;
            ctrl += '<div class="col-2" style="padding:0"><button style="padding:.65rem 1rem" onclick="removeSpec(this)" ';
            ctrl += 'class="btn btn-danger m-btn m-btn--custom m-btn--icon m-btn--pill m-btn--air"><i class="fa fa-times"></i>';
            ctrl += '</button></div></div>';
            $('#specsDiv').append(ctrl);
        }

        $('input').first().focus();

    }
})();

function removeSpec(e) {
    $(e).parent().parent().remove();
};
