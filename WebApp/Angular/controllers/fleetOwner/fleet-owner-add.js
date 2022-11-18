(function () {
    'use strict';

    angular
        .module('app')
        .controller('fleet_owner_add', fleet_owner_add);

    fleet_owner_add.$inject = ['$scope', 'com'];

    function fleet_owner_add($scope, com) {
        $scope.fleet_owner = {
            Id: 0,
            FirstName: '',
            LastName: '',
            EmailAddress: '',
            PhoneNo: '',
            Address: '',
            PZipCode: '',
            PCityId: '',
            Picture: 'null',
            CompanyName: '',
            BillToAddress: '',
            CZipCode: '',
            CCityId: '',
            FaxNo: '',
            CContactNo: '',
            BIEmbededCode: '',
            CorporateUserId: 0,
        };
        $scope.UniqueEmail = '';
        $('input').first().focus();

        $scope.save = function () {
            var corporateUserId = $('#drpCorporateUser').val();
            var pCityId = $('#pdrpCity').val();
            var cCityId = $('#cdrpCity').val();
            if (pCityId === '0' || cCityId == '0') {
                swal('Required', 'Please select city.', 'warning');
                mApp.unblockPage();
                return;
            }
            else if (corporateUserId === "0") {
                swal('Required', 'Please select corporate user.', 'warning');
                mApp.unblockPage();
                return;
            }
            else {
                $scope.fleet_owner.PCityId = parseInt(pCityId);
                $scope.fleet_owner.CCityId = parseInt(cCityId);
                $scope.fleet_owner.CorporateUserId = parseInt(corporateUserId);
            }

            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/FleetOwnerEmailCheck?id=' + 0 + '&email=' + $scope.fleet_owner.EmailAddress).then(function (result) {
                if (result) {
                    mApp.blockPage({ message: "Saving..." });
                    var imgStr = $scope.fleet_owner.Picture;
                    $scope.fleet_owner.Picture = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);
                    com.post('/FleetOwner/SaveFleetOwner', $scope.fleet_owner).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
                            .then(function (e) {
                                window.location = "/FleetOwner/FleetOwnerList";
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
            window.location = '/FleetOwner/FleetOwnerList';
        };



        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetCorporateUser').then(function (result) {
            var ctr = $('#drpCorporateUser');
            //ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                ctr.select2();
            });
        });

        mApp.blockPage({ message: "Loading..." });
        com.getCountries().then(function (result) {
            var cCtr = $('#cdrpCountry');
            var pctr = $('#pdrpCountry');
            cCtr.children().remove();
            pctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                cCtr.append(str);
                pctr.append(str);
            });
            cCtr.select2();
            pctr.select2();

        });
        $('#cdrpState').select2();
        $('#pdrpState').select2();
        $('#cdrpCity').select2();
        $('#pdrpCity').select2();


        $scope.EmailCheck = function () {
            if ($scope.fleet_owner.EmailAddress) {
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/FleetOwnerEmailCheck?id=' + 0 + '&email=' + $scope.fleet_owner.EmailAddress).then(function (result) {
                    if (result) { $scope.UniqueEmail = true; }
                    else { $scope.UniqueEmail = false; }

                });
            }
            else {
                $scope.UniqueEmail = null;
            }
        };


        $scope.sameAddresses = function (event) {
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