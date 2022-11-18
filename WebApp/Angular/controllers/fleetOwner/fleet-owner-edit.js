(function () {
    'use strict';

    angular
        .module('app')
        .controller('fleet_owner_edit', fleet_owner_edit);

    fleet_owner_edit.$inject = ['$scope', 'com'];

    function fleet_owner_edit($scope, com) {
        //debugger
        $scope.orignal = undefined;
        $scope.fleet_owner = undefined;
        $scope.UniqueEmail = true;

        //Get all the counties
        $('#pdrpState').select2();
        $('#pdrpCity').select2();

        $('#cdrpState').select2();
        $('#cdrpCity').select2();

        $scope.getRecord = function () {
            //debugger
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/FleetOwner/GetFleetOwner/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.fleet_owner = result;
                var test;
                if (result.Address == result.BillToAddress) {
                    $('#chek').prop('checked', true);
                }
                
                mApp.blockPage({ message: "Loading..." });
                com.get('/Dropdown/GetCorporateUser').then(function (result) {
                    var ctr = $('#drpCorporateUser');
                    ctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                        ctr.select2();
                    });
                    ctr.val($scope.fleet_owner.CorporateUserId).select2().trigger('change');
                });

                mApp.blockPage({ message: "Loading Countries..." });
                com.getCountries().then(function (result) {
                    var pctr = $('#pdrpCountry');
                    var cctr = $('#cdrpCountry');
                    pctr.children().remove();
                    cctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        pctr.append(str);
                       
                        cctr.append(str);
                        
                    });
                    pctr.select2();
                    cctr.select2();
                    $scope.getCitiesAndStates($scope.fleet_owner.PCityId, $scope.fleet_owner.CCityId);
                });
            });
        };

        $scope.getRecord();

        //Getting state and city 
        $scope.getCitiesAndStates = function (pcityId, ccityId) {
            mApp.blockPage({ message: "Loading Cities and States..." });
            $.ajax({
                url: '/Dropdown/GetStatesAndCitiesFromCity/' + pcityId,
                success: function (data) {
                    var drpcity = $('#pdrpCity');
                    var drpstate = $('#pdrpState');

                    $('#pdrpCountry').val(data.countryId);
                    $('#pdrpCountry').select2().trigger('change');
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
                    drpcity.val(pcityId);
                    drpcity.select2().trigger('change');


                    $('#pdrpCountry').on('change', () => getStates('profile'));
                    drpstate.on('change', () => getCities('profile'));
                    mApp.unblockPage();
                }
            });
            $.ajax({
                url: '/Dropdown/GetStatesAndCitiesFromCity/' + ccityId,
                success: function (data) {
                    var drpcity = $('#cdrpCity');
                    var drpstate = $('#cdrpState');

                    $('#cdrpCountry').val(data.countryId);
                    $('#cdrpCountry').select2().trigger('change');

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
                    drpcity.val(ccityId).select2().trigger('change');
                    drpstate.val(data.stateId).select2().trigger('change');

                    $('#cdrpCountry').on('change', () => getStates('company'));
                    drpstate.on('change', () => getCities('company'));
                    mApp.unblockPage();
                }
            });
        };

        //Update record
        $scope.update = function () {
            //debugger
            var pcityId = $('#pdrpCity').val();
            var ccityId = $('#cdrpCity').val();
            var corporateUserId = $('#drpCorporateUser').val();
            if (pcityId === '0' || ccityId === '0') {
                swal('Required', 'Please select city.', 'warning');
                return;
            } else {
                $scope.fleet_owner.PCityId = parseInt(pcityId);
                $scope.fleet_owner.CCityId = parseInt(ccityId);
                $scope.fleet_owner.CorporateUserId = parseInt(corporateUserId);
            }

            var imgStr = $scope.fleet_owner.Picture;
            $scope.fleet_owner.Picture = imgStr === 'null' ? null : imgStr.substring(imgStr.indexOf(',') + 1);


            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/FleetOwnerEmailCheck?id=' + $scope.fleet_owner.Id + '&email=' + $scope.fleet_owner.EmailAddress).then(function (result) {
                if (result) {
                    //debugger
                    mApp.blockPage({ message: "Updating..." });
                    com.post('/FleetOwner/UpdateFleetOwner', $scope.fleet_owner).then(function (result) {
                        swal("Success", "Record has been updated.", "success")
                            .then(function (e) {
                                //debugger
                                if (result.Address == result.BillToAddress) {
                                    var t = true;
                                }
                                else {
                                    var t = false;
                                }
                                window.location = "/FleetOwner/FleetOwnerList";
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
            window.location = '/FleetOwner/FleetOwnerList';
        };


        $scope.EmailCheck = function () {
            if ($scope.fleet_owner.EmailAddress) {
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/FleetOwnerEmailCheck?id=' + $scope.fleet_owner.Id + '&email=' + $scope.fleet_owner.EmailAddress).then(function (result) {
                    if (result) { $scope.UniqueEmail = true; }
                    else { $scope.UniqueEmail = false; }

                });
            }
            else {
                $scope.UniqueEmail = null;
            }
        };

        $('input').first().focus();

        $scope.sameAddresses = function (event) {
            //debugger
            if (event.target.checked) {
                $scope.fleet_owner.BillToAddress = $scope.fleet_owner.Address;
                $('#billToAddress').prop('disabled', true);
            }
            else {
                $scope.fleet_owner.BillToAddress = '';
                $('#billToAddress').prop('disabled', false);
            }
        };
    }
})();

// For dorp down
function getCities(type) {
    mApp.blockPage({ message: "Loading Cities..." });

    if (type === 'profile') {
        var val = $('#pdrpState').val();
        $.ajax({
            url: '/Dropdown/GetCitiesOfState/' + val,
            success: function (data) {
                var drp = $('#pdrpCity');
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
        var val = $('#cdrpState').val();
        $.ajax({
            url: '/Dropdown/GetCitiesOfState/' + val,
            success: function (data) {
                var drp = $('#cdrpCity');
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

        var drpCity = $('#pdrpCity');
        drpCity.children().remove();
        drpCity.append('<option  value="0">--Select State First--</option>');
        drpCity.select2('destroy');
        drpCity.select2();

        var val = $('#pdrpCountry').val();

        $.ajax({
            url: '/Dropdown/GetStatesOfCountry/' + val,
            success: function (data) {
                if (data.length === 0) {
                    var drp = $('#pdrpState');
                    drp.children().remove();
                    drp.append('<option  value="0">--Select Country First--</option>');
                    drp.select2('destroy');
                    drp.select2();
                    mApp.unblockPage();
                    return;
                }
                var drp = $('#pdrpState');
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

        var drpCity = $('#cdrpCity');
        drpCity.children().remove();
        drpCity.append('<option  value="0">--Select State First--</option>');
        drpCity.select2('destroy');
        drpCity.select2();

        var val = $('#cdrpCountry').val();

        $.ajax({
            url: '/Dropdown/GetStatesOfCountry/' + val,
            success: function (data) {
                if (data.length === 0) {
                    var drp = $('#cdrpState');
                    drp.children().remove();
                    drp.append('<option  value="0">--Select Country First--</option>');
                    drp.select2('destroy');
                    drp.select2();
                    mApp.unblockPage();
                    return;
                }
                var drp = $('#cdrpState');
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