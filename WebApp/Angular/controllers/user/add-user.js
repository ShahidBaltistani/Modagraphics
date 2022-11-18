(function () {
    'use strict';

    angular
        .module('app')
        .controller('add_user', add_user);

    add_user.$inject = ['$scope', 'com'];
    ////debugger;
    function add_user($scope, com) {
        ////debugger;
        $scope.user = {};
        $scope.user.UserType = -1;
        $scope.user.Image = 'null';
        $scope.UniqueEmail = '';
        $scope.save = function () {
            ////debugger;
            mApp.blockPage({ message: "Saving..." });

            var cityId = $('#drpCity').val();
            var userId = $('#userType').val();
            if (cityId === '0') {
                mApp.unblockPage();
                swal('Required', 'Please select city.', 'warning');
                return;
            }
            if (userId == -1) {
                mApp.unblockPage();
                swal('Required', 'Please select user type.', 'warning');
                return;
            }
            else {
                $scope.user.CityId = parseInt(cityId);
            }
            var imgStr = $scope.user.Image;
            $scope.user.Image = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);


            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/UserEmailCheck?id=' + 0 + "&email=" + $scope.user.Email).then(function (result) {
                if (result) {
                    com.post('/User/SaveUser', $scope.user).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
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



        $scope.removeDefaultOpt = function () {
            var drp = $('#userType');
            var val = drp.children().first().attr('value');
            if (val === '-1') {
                drp.children().first().remove();
                drp.selectpicker('refresh');
            }
        }

        mApp.blockPage({ message: "Loading..." });
        com.getCountries().then(function (result) {
            var ctr = $('#drpCountry');
            ////debugger;
            ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                ctr.select2();
            });
        });
        $('#drpState').select2();
        $('#drpCity').select2();

        selectFirstOption('userType');

        //$('#phoneNo').inputmask({ mask: "9", repeat: 15, greedy: !1 });

        $('input').first().focus();

        $scope.EmailCheck = function () {
            if ($scope.user.Email) {

                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/UserEmailCheck?id=' + 0 + "&email=" + $scope.user.Email).then(function (result) {
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
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });
}

function getStates() {
    ////debugger;
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
            getCities();
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });
}
