(function () {
    'use strict';

    angular
        .module('app')
        .controller('crew_add', crew_add);

    crew_add.$inject = ['$scope', 'com'];

    function crew_add($scope, com) {
        $scope.crew = {
            Id: 0,
            FirstName: '',
            LastName: '',
            EmailAddress: '',
            PhoneNo: '',
            Address: '',
            ZipCode: '',
            CityId: '',
            Picture: 'null',
            Status: '0',
            InstallerId: '0',
        };
        $scope.UniqueEmail = '';
        $('input').first().focus();

        $scope.save = function () {
            //var installerId = $('#drpInstaller').val();
            var CityId = $('#drpCity').val();
            if (CityId == '0') {
                swal('Required', 'Please select city.', 'warning');
                mApp.unblockPage();
                return;
            }
            //else if (installerId === "0") {
            //    swal('Required', 'Please select Fleet Owner.', 'warning');
            //    mApp.unblockPage();
            //    return;
            //}
            else {
                $scope.crew.CityId = parseInt(CityId);
                //$scope.crew.InstallerId = parseInt(installerId);
            }

            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/CrewEmailCheck?id=' + 0 + '&email=' + $scope.crew.EmailAddress).then(function (result) {
                if (result) {
                    mApp.blockPage({ message: "Saving..." });
                    var imgStr = $scope.crew.Picture;
                    $scope.crew.Picture = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);
                    com.post('/Crew/SaveCrew', $scope.crew).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
                            .then(function (e) {
                                window.location = "/Crew/CrewList";
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
            window.location = '/Crew/CrewList';
        };

        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetInstaller').then(function (result) {
            var ctr = $('#drpInstaller');
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
            if ($scope.crew.EmailAddress) {
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/CrewEmailCheck?id=' + 0 + '&email=' + $scope.crew.EmailAddress).then(function (result) {
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