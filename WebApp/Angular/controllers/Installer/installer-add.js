(function () {
    'use strict';

    angular
        .module('app')
        .controller('installer_add', installer_add);

    installer_add.$inject = ['$scope', 'com'];

    function installer_add($scope, com) {
        $scope.installer = {
            Id: 0,
            firstName: "",
            lastName: "",
            emailAddress: "",
            phoneNo: "",
            address: "",
            pzipCode: "",
            pcity: "",
            picture: "",
            travelInMiles: "",
            isPreferred: "",
            companyName: "",
            billToAddress: "",
            czipCode: "",
            ccity: "",
            faxNo: "",
            cContactNo: "",
            companyEmail: "",
            fEINumber: "",
            certificationTraining: "",
            isInstallersEmployee: "0",
            isEmployeesBackgroundCheck: "0",
            isDrugTest: "0",
            equipmentOwned: "",
            installLocations: "",
            installProjectCompleted: "",
            yearInBusiness: "",
            NoOfEmployees: 0,
        };
        $scope.UniqueEmail = null;
        $('input').first().focus();

        $scope.save = function () {
            var pCityId = $('#pdrpCity').val();
            var cCityId = $('#cdrpCity').val();
            if (pCityId === '0' || cCityId == '0') {
                swal('Required', 'Please select city.', 'warning');
                mApp.unblockPage();
                return;
            }
            else if ($scope.installer.isEmployeesBackgroundCheck === '0' || $scope.installer.isDrugTest === '0' || $scope.installer.isInstallersEmployee === '0') {
                swal('Required', 'Please select yes or no.', 'warning');
                mApp.unblockPage()
                return;
            }
            else {
                $scope.installer.pcity = parseInt(pCityId);
                $scope.installer.ccity = parseInt(cCityId);
            }
            


            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/InstallerEmailCheck?id=' + 0 + '&email=' + $scope.installer.emailAddress).then(function (result) {
                if (result) {
                    mApp.blockPage({ message: "Saving..." });
                    var imgStr = $scope.installer.picture;
                    $scope.installer.picture = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);

                    com.post('/Installer/SaveInstaller', $scope.installer).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
                            .then(function (e) {
                                window.location = "/Installer/InstallerList";
                            });
                    }); }
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
            window.location = '/Installer/InstallerList';
        };


        $scope.EmailCheck = function () {
            if ($scope.installer.emailAddress) {
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/InstallerEmailCheck?id=' + 0 + '&email=' + $scope.installer.emailAddress).then(function (result) {
                    if (result) { $scope.UniqueEmail = true; }
                    else { $scope.UniqueEmail = false; }

                });
            }
            else {
                $scope.UniqueEmail = null;
            }
        };


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

        $scope.sameAddresses = function (event) {
            if (event.target.checked) {
                $scope.installer.billToAddress = $scope.installer.address;
                $('#billToAddress').prop('disabled', true);
            }
            else {
                $scope.installer.billToAddress = '';
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
                    var drpCity = $('#pdrpState');
                    drpCity.children().remove();
                    drpCity.append('<option  value="0">--Select Country First--</option>');
                    drpCity.select2('destroy');
                    drpCity.select2();
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
                    var drpCity = $('#cdrpState');
                    drpCity.children().remove();
                    drpCity.append('<option  value="0">--Select Country First--</option>');
                    drpCity.select2('destroy');
                    drpCity.select2();
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