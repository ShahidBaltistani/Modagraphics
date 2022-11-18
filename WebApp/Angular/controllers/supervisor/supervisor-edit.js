(function () {
    'use strict';

    angular
        .module('app')
        .controller('supervisor_edit', supervisor_edit);

    supervisor_edit.$inject = ['$scope', 'com'];

    function supervisor_edit($scope, com) {
        debugger
        $scope.orignal = undefined;
        $scope.supervisor = undefined;
        $scope.UniqueEmail = true;

        //Get all the counties
        $('#drpState').select2();
        $('#drpCity').select2();

        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/Supervisor/GetSupervisor/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.supervisor = result;

                
                mApp.blockPage({ message: "Loading..." });
                com.get('/Dropdown/GetFleetOwner').then(function (result) {
                    var ctr = $('#drpFleetOwner');
                    ctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                        ctr.select2();
                    });
                    ctr.val($scope.supervisor.FleetOwnerId).select2().trigger('change');
                });

                mApp.blockPage({ message: "Loading Countries..." });
                com.getCountries().then(function (result) {
                    var ctr = $('#drpCountry');
                    ctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                    });
                    ctr.select2();
                    $scope.getCitiesAndStates($scope.supervisor.CityId);
                });
            });
        };

        $scope.getRecord();

        //Getting state and city 
        $scope.getCitiesAndStates = function (cityId) {
            mApp.blockPage({ message: "Loading Cities and States..." });
            $.ajax({
                url: '/Dropdown/GetStatesAndCitiesFromCity/' + cityId,
                success: function (data) {
                    var drpcity = $('#drpCity');
                    var drpstate = $('#drpState');

                    $('#drpCountry').val(data.countryId);
                    $('#drpCountry').select2().trigger('change');
                    drpcity.children().remove();
                    if (data.cities.length === 0) {
                        drpcity.append('<option  value="0">-No City Found-</option>');
                    } else {
                        $.each(data.cities, function (index, item) {
                            var str = `<option value="${item.Id}">${item.Value}</option>`;
                            drpcity.append(str);
                        });
                    }

                    drpstate.children().remove();
                    $.each(data.states, function (index, item) {
                        var str = `<option value="${item.Id}">${item.Value}</option>`;
                        drpstate.append(str);
                    });
                    drpcity.select2('destroy');
                    drpcity.select2();
                    drpstate.select2('destroy');
                    drpstate.select2();

                    drpstate.val(data.stateId);
                    drpstate.select2().trigger('change');
                    drpcity.val(cityId);
                    drpcity.select2().trigger('change');


                    $('#drpCountry').on('change', () => getStates('profile'));
                    drpstate.on('change', () => getCities('profile'));
                    mApp.unblockPage();
                }
            });
        };

        //Update record
        $scope.update = function () {
            debugger
            //var fleetOwnerId = $('#drpFleetOwner').val();
            var cityId = $('#drpCity').val();
            if (cityId === '0') {
                swal('Required', 'Please select city.', 'warning');
                return;
            } else {
                $scope.supervisor.CityId = parseInt(cityId);
                //$scope.supervisor.FleetOwnerId = parseInt(fleetOwnerId);
            }

            var imgStr = $scope.supervisor.Picture;

            $scope.supervisor.Picture = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);

            //$scope.supervisor.Picture = imgStr === 'null' ? null : imgStr.substring(imgStr.indexOf(',') + 1);


            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/SupervisorEmailCheck?id=' + $scope.supervisor.Id + '&email=' + $scope.supervisor.EmailAddress).then(function (result) {
                if (result) {

                    mApp.blockPage({ message: "Updating..." });
                    com.post('/Supervisor/UpdateSupervisor', $scope.supervisor).then(function (result) {
                        swal("Success", "Record has been updated.", "success")
                            .then(function (e) {
                                window.location = "/Supervisor/SupervisorList";
                            });

                    }); }
                else {
                    //swal('Error', 'Email already Exist.', 'warning');
                    $("#EmailFocusOut").blur()
                    mApp.unblockPage();
                    return;
                }

            });


           
        };
        $scope.Cancel = function () {
            $("#EmailFocusOut").blur();
            window.location = '/Supervisor/SupervisorList';
        };

        $scope.EmailCheck = function () {
            if ($scope.supervisor.EmailAddress) {
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/SupervisorEmailCheck?id=' + $scope.supervisor.Id + '&email=' + $scope.supervisor.EmailAddress).then(function (result) {
                    if (result) { $scope.UniqueEmail = true; }
                    else { $scope.UniqueEmail = false; }

                });
            }
            else {
                $scope.UniqueEmail = null;
            }
        };

        $('input').first().focus();
    }
})();

// For dorp down
function getCities(type) {
    mApp.blockPage({ message: "Loading Cities..." });

    if (type === 'profile') {
        var val = $('#drpState').val();
        $.ajax({
            url: '/Dropdown/GetCitiesOfState/' + val,
            success: function (data) {
                var drp = $('#drpCity');
                drp.children().remove();
                if (data.length === 0) {
                    drp.append('<option  value="0">-No City Found-</option>');
                } else {
                    $.each(data, function (index, item) {
                        var str = `<option value="${item.Id}">${item.Value}</option>`;
                        drp.append(str);
                    });
                }

                drp.select2('destroy');
                drp.select2();
                mApp.unblockPage();
            }
        });
    }
    else {
        var val = $('#drpState').val();
        $.ajax({
            url: '/Dropdown/GetCitiesOfState/' + val,
            success: function (data) {
                var drp = $('#drpCity');
                drp.children().remove();
                if (data.length === 0) {
                    drp.append('<option  value="0">-No City Found-</option>');
                } else {
                    $.each(data, function (index, item) {
                        var str = `<option value="${item.Id}">${item.Value}</option>`;
                        drp.append(str);
                    });
                }

                drp.select2('destroy');
                drp.select2();
                mApp.unblockPage();
            }
        });
    }
}

function getStates(type) {
    mApp.blockPage({ message: "Loading States..." });

        var drpCity = $('#drpCity');
        drpCity.children().remove();
        drpCity.append('<option  value="0">--Select State First--</option>');
        drpCity.select2('destroy');
        drpCity.select2();

        var val = $('#drpCountry').val();

        $.ajax({
            url: '/Dropdown/GetStatesOfCountry/' + val,
            success: function (data) {
                if (data.length === 0) {
                    var drp = $('#drpState');
                    drp.children().remove();
                    drp.append('<option  value="0">--Select Country First--</option>');
                    drp.select2('destroy');
                    drp.select2();
                    mApp.unblockPage();
                    return;
                }
                var drp = $('#drpState');
                drp.children().remove();
                $.each(data, function (index, item) {
                    var str = `<option value="${item.Id}">${item.Value}</option>`;
                    drp.append(str);
                });
                drp.select2('destroy');
                drp.select2();
                getCities('profile');
                mApp.unblockPage();
            }
        });
}