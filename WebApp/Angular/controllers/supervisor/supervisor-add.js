(function () {
    'use strict';

    angular
        .module('app')
        .controller('supervisor_add', supervisor_add);

    supervisor_add.$inject = ['$scope', 'com'];

    function supervisor_add($scope, com) {
        $scope.supervisor = {
            Id: 0,
            FirstName: '',
            LastName: '',
            EmailAddress: '',
            PhoneNo: '',
            Address: '',
            ZipCode: '',
            CityId: '',
            Picture: 'null',
            Status: '1',
            FleetOwnerId: 0,
        };
        $scope.UniqueEmail = '';
        $('input').first().focus();

        $scope.save = function () {
            //var fleetOwnerId = $('#drpFleetOwner').val();
            var CityId = $('#drpCity').val();
            if (CityId == '0') {
                swal('Required', 'Please select city.', 'warning');
                mApp.unblockPage();
                return;
            }
            //else if (fleetOwnerId === "0") {
            //    swal('Required', 'Please select Fleet Owner.', 'warning');
            //    mApp.unblockPage();
            //    return;
            //}
            else {
                $scope.supervisor.CityId = parseInt(CityId);
                //$scope.supervisor.FleetOwnerId = parseInt(fleetOwnerId);
            }

            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/SupervisorEmailCheck?id=' + 0 + '&email=' + $scope.supervisor.EmailAddress).then(function (result) {
                if (result) {
                    mApp.blockPage({ message: "Saving..." });
                    var imgStr = $scope.supervisor.Picture;
                    $scope.supervisor.Picture = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);
                    com.post('/Supervisor/SaveSupervisor', $scope.supervisor).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
                            .then(function (e) {
                                window.location = "/Supervisor/SupervisorList";
                            });
                    });
                }
                else {
                    //swal('Error', 'Email already Exist.', 'warning');
                    $("#EmailFocusOut").blur()
                    mApp.unblockPage();
                    return;

                }

            });
        }
        $scope.Cancel = function () {
            $("#EmailFocusOut").blur();
            window.location = '/Supervisor/SupervisorList';
        };

        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetFleetOwner').then(function (result) {
            var ctr = $('#drpFleetOwner');
            //ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                ctr.select2();
            });
        });

        mApp.blockPage({ message: "Loading..." });
        com.getCountries().then(function (result) {
            var Ctr = $('#drpCountry');
            Ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                Ctr.append(str);
            });
            Ctr.select2();

        });
        $('#drpState').select2();
        $('#drpCity').select2();


        $scope.EmailCheck = function () {
            if ($scope.supervisor.EmailAddress) {
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/SupervisorEmailCheck?id=' + 0 + '&email=' + $scope.supervisor.EmailAddress).then(function (result) {
                    if (result) { $scope.UniqueEmail = true; }
                    else { $scope.UniqueEmail = false; }

                });
            }
            else {
                $scope.UniqueEmail = null;
            }
        };
        
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

    if (type === 'profile') {

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
    else {

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
                getCities('company');
                mApp.unblockPage();
            }
        });
    }
}