(function () {
    'use strict';

    angular
        .module('app')
        .controller('vehicle_type', vehicle_type);

    vehicle_type.$inject = ['$scope', 'com'];

    function vehicle_type($scope, com) {

        //$scope.min = 1;
        //$scope.max = 15;


        $('input').first().focus();

        $scope.vehicle = {
            Id: 0,
            VehicleType: '',
            BasePrice: 0,
            IsActive: true,
            Specifications: [],
            ImageString: 'null'
        };

        

        $scope.save = function () {

            if ($scope.vehicle.BasePrice < 1) {
                swal('BasePrice', 'BasePrice must not be Zero', 'warning');
                return false;
            }


            mApp.blockPage({ message: "Saving..." });
            var isError = false;
            
            $scope.vehicle.Specifications = [];
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


            com.post('/Vehicle/SaveVehicleType', $scope.vehicle).then(function (result) {
                swal("Success", "Record has been saved.", "success")
                    .then(function (e) {
                        window.location = "/Vehicle/Types";
                    });
            }, function() {
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

        

    }
})();

function removeSpec(e) {
    $(e).parent().parent().remove();
};
