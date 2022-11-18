(function () {
    'use strict';

    angular
        .module('app')
        .controller('edit_user', edit_user);

    edit_user.$inject = ['$scope', 'com'];

    function edit_user($scope, com) {
        $scope.user = undefined;
        $scope.UniqueEmail = true;
        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/User/GetUser/' + id).then(function (result) {
                $scope.user = result;

                $("#userType").val(result.UserType).selectpicker('refresh');
                
                mApp.blockPage({ message: "Loading Countries..." });
                com.getCountries().then(function (result) {
                    var ctr = $('#drpCountry');
                    ctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                        ctr.select2();
                    });
                    $scope.getCitiesAndStates($scope.user.CityId);
                });
            });
        };

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
                    drpcity.val(cityId).select2().trigger('change');
                    drpstate.val(data.stateId).select2().trigger('change');

                    $('#drpCountry').on('change', getStates);
                    drpstate.on('change', getCities);
                    mApp.unblockPage();
                }
            });
        };

        $scope.getRecord();

        $scope.update = function () {

            var cityId = $('#drpCity').val();
            if (cityId === '0') {
                swal('Required', 'Please select city.', 'warning');
                return;
            } else {
                $scope.user.CityId = parseInt(cityId);
            }

            
           

            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/UserEmailCheck?id=' + $scope.user.Id + "&email=" + $scope.user.Email).then(function (result) {
                if (result) {
                    var imgStr = $scope.user.Image;
                    $scope.user.Image = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);
                    mApp.blockPage({ message: "Updating..." });
                    com.post('/User/UpdateUser', $scope.user).then(function (result) {
                        swal("Success", "Record has been updated.", "success")
                            .then(function (e) {
                                window.location = "/User/UserList";
                            });
                    }, function () {
                        if ($scope.user.Image !== null && $scope.user.Image !== 'null') {
                            $scope.user.Image = 'data:image/jpg;base64,' + $scope.user.Image;
                        } else if ($scope.user.Image === null) {
                            $scope.user.Image = 'null';
                        }
                    }); }
                else {
                    $("#EmailFocusOut").blur()
                    //swal('Error', 'Email already Exist.', 'warning');
                    mApp.unblockPage();

                    return;
                }

            });


            
        };

        $scope.Cancel = function () {
            $("#EmailFocusOut").blur();
            window.location = '/User/UserList';
        };

        $scope.EmailCheck = function () {
            if ($scope.user.Email) {

                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/UserEmailCheck?id=' + $scope.user.Id + "&email=" + $scope.user.Email).then(function (result) {
                    if (result) { $scope.UniqueEmail = true; }
                    else
                    {
                        $scope.UniqueEmail = false;
                    }

                });
            }
            else {
                $scope.UniqueEmail = null;
            }
        };

        $scope.removeDefaultOpt = function () {
            $('#userType').selectpicker('refresh');
        }

        $('#drpState').select2();
        $('#drpCity').select2();
        $('input').first().focus();
    }
})();

function getCities() {
    mApp.blockPage({ message: "Loading Cities..." });
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

function getStates() {
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
                var drpCity = $('#drpState');
                drpCity.children().remove();
                drpCity.append('<option  value="0">--Select Country First--</option>');
                drpCity.select2('destroy');
                drpCity.select2();
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
            mApp.unblockPage();
            getCities();
        }
    });
}