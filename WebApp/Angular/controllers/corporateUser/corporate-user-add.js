(function () {
    'use strict';

    angular
        .module('app')
        .controller('corporate_user_add', corporate_user_add);

    corporate_user_add.$inject = ['$scope', 'com'];

    function corporate_user_add($scope, com) {
        $scope.corporateUser = {
            Id: 0,
            FirstName: '',
            LastName: '',
            EmailAddress: '',
            PhoneNo: '',
            Address: '',
            ZipCode: '',
            CityId: '',
            Picture: 'null',
            UniqueEmail: '',
        };
        $('input').first().focus();

        $scope.save = function () {
            

            var cityId = $('#drpCity').val();
            if (cityId === '0') {
                swal('Required', 'Please select city.', 'warning');
                return;
            } else {
                $scope.corporateUser.CityId = parseInt(cityId);
            }


            mApp.blockPage({ message: "Checking Email..." });
            com.get('/EmailCheck/CorporateUserEmailCheck?id=' + 0 + "&email=" + $scope.corporateUser.EmailAddress).then(function (result) {
                if (result) {
                    var imgStr = $scope.corporateUser.Picture;
                    $scope.corporateUser.Picture = imgStr === 'null' || imgStr === null ? null : imgStr.substring(imgStr.indexOf(',') + 1);
                    mApp.blockPage({ message: "Saving..." });
                    com.post('/CorporateUser/SaveUserCorporate', $scope.corporateUser).then(function (result) {
                        swal("Success", "Record has been saved.", "success")
                            .then(function (e) {
                                window.location = "/CorporateUser/CorporateUserList";
                            });
                    }
                    );
                }
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
            window.location = '/CorporateUser/CorporateUserList';
        };

        $scope.EmailCheck = function () {
            if ($scope.corporateUser.EmailAddress) {
                
                mApp.blockPage({ message: "Checking Email..." });
                com.get('/EmailCheck/CorporateUserEmailCheck?id=' + 0 + "&email=" + $scope.corporateUser.EmailAddress).then(function (result) {
                    if (result) { $scope.corporateUser.UniqueEmail = true; }
                    else { $scope.corporateUser.UniqueEmail = false;}

                });
            }
            else {
                $scope.corporateUser.UniqueEmail = null;
            }
        };

        mApp.blockPage({ message: "Loading..." });
        com.getCountries().then(function (result) {
            var ctr = $('#drpCountry');
            ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                ctr.select2();
            });
        });
        $('#drpState').select2();
        $('#drpCity').select2();
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
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
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
            getCities();
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });
}