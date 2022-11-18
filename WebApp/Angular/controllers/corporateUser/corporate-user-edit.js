(function () {
    'use strict';

    angular
        .module('app')
        .controller('corporate_user_edit', corporate_user_edit);

    corporate_user_edit.$inject = ['$scope', 'com'];

    function corporate_user_edit($scope, com) {
        $scope.orignal = undefined;
        $scope.corporateUser = undefined;
        $scope.UniqueEmail = true;

        //Get all the counties
        $('#drpState').select2();
        $('#drpCity').select2();

        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/CorporateUser/GetUserCorporate/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.corporateUser = result;

                mApp.blockPage({ message: "Loading Countries..." });
                com.getCountries().then(function (result) {
                    var ctr = $('#drpCountry');
                    ctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                        ctr.select2();
                    });
                    $scope.getCitiesAndStates($scope.corporateUser.CityId);
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
                    drpcity.val(cityId).select2().trigger('change');
                    drpstate.val(data.stateId).select2().trigger('change');

                    $('#drpCountry').on('change', getStates);
                    drpstate.on('change', getCities);
                    mApp.unblockPage();
                }
            });
        };

        //Update record
        $scope.update = function () {
            
            var cityId = $('#drpCity').val();
            if (cityId === '0') {
                mApp.unblockPage();
                swal('Required', 'Please select city.', 'warning');
                return;
            } else {

                $scope.corporateUser.CityId = parseInt(cityId);
            }

           


            
            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/CorporateUserEmailCheck?id=' + $scope.corporateUser.Id + "&email=" + $scope.corporateUser.EmailAddress).then(function (result) {
                if (result) {
                    mApp.blockPage({ message: "Updating..." });
                    var imgStr = $scope.corporateUser.Picture;
                    $scope.corporateUser.Picture = imgStr === 'null' || imgStr === null  ? null : imgStr.substring(imgStr.indexOf(',') + 1);
                    com.post('/CorporateUser/UpdateUserCorporate', $scope.corporateUser).then(function (result) {
                        swal("Success", "Record has been updated.", "success")
                            .then(function (e) {
                                window.location = "/CorporateUser/CorporateUserList";
                            });

                    }); }
                else {
                    $("#EmailFocusOut").blur()
                    //swal('Error', 'Email already Exist.', 'warning');
                    mApp.unblockPage();

                    return; }

            });
        };

        $scope.Cancel = function () {
            $("#EmailFocusOut").blur();
            window.location = '/CorporateUser/CorporateUserList';
        };

        $('input').first().focus();

        $scope.EmailCheck = function () {
            if ($scope.corporateUser.EmailAddress) {

                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/CorporateUserEmailCheck?id=' + $scope.corporateUser.Id + "&email=" + $scope.corporateUser.EmailAddress).then(function (result) {
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