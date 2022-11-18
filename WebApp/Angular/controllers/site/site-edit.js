(function () {
    'use strict';

    angular
        .module('app')
        .controller('site_edit', site_edit);

    site_edit.$inject = ['$scope', 'com'];

    function site_edit($scope, com) {
        $scope.orignal = undefined;
        $scope.site = undefined;

        //Get all the counties
        $('#drpState').select2();
        $('#drpCity').select2();

        $scope.getRecord = function () {
            mApp.blockPage({ message: "Fetching..." });

            var id = $('#id').val();
            com.get('/Site/GetSite/' + id).then(function (result) {
                $scope.orignal = result;
                $scope.site = result;

                $scope.site.TakeDownStatus = $scope.site.TakeDownStatus.toString();

                //Fixing date format
                var date = new Date($scope.site.BidDueDate.match(/\d+/)[0] * 1);
                var month = ("0" + (date.getMonth() + 1)).slice(-2);
                var day = ("0" + date.getDate()).slice(-2); 
                $scope.site.BidDueDate = month + '/' + day + '/' + date.getFullYear();
                date = new Date($scope.site.SiteStartDate.match(/\d+/)[0] * 1);
                month = ("0" + (date.getMonth() + 1)).slice(-2);
                day = ("0" + date.getDate()).slice(-2); 
                $scope.site.SiteStartDate = month + '/' + day + '/' + date.getFullYear(); 
                date = new Date($scope.site.SiteEndDate.match(/\d+/)[0] * 1);
                month = ("0" + (date.getMonth() + 1)).slice(-2);
                day = ("0" + date.getDate()).slice(-2); 
                $scope.site.SiteEndDate = month + '/' + day + '/' + date.getFullYear();
                date = new Date($scope.site.VehicleAvailabilityDate.match(/\d+/)[0] * 1); 
                month = ("0" + (date.getMonth() + 1)).slice(-2);
                day = ("0" + date.getDate()).slice(-2); 
                $scope.site.VehicleAvailabilityDate = month + '/' + day + '/' + date.getFullYear();

                
                var workHours = $scope.site.WorkHoursAtFacility.split('-', 2);
                $("[name='WorkHoursForm']").val(workHours[0]);
                $("[name='workHoursTo']").val(workHours[1]);
                
                //workHourFrom = workHours[0];
                //workHourTo = workHours[1];
               
                mApp.blockPage({ message: "Loading Countries..." });
                com.getCountries().then(function (result) {
                    var ctr = $('#drpCountry');
                    ctr.children().remove();
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                    });
                    ctr.select2();
                    $scope.getCitiesAndStates($scope.site.CityId);
                });
                
                mApp.blockPage({ message: "Loading Fleet Owner..." });
                com.get('/Dropdown/GetFleetOwner').then(function (result) {
                    var ctr = $('#drpFleetOwner');
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                    });
                    ctr.select2();
                    ctr.val($scope.site.FleetOwnerId).select2().trigger('change');
                    ctr.on('change', getProjects);
                });
               
                mApp.blockPage({ message: "Loading Project..." });
                com.get('/Dropdown/GetProjects/' + $scope.site.FleetOwnerId).then(function (result) {
                    var ctr = $('#drpProject');
                    angular.forEach(result, function (value) {
                        var str = `<option value="${value.Id}">${value.Value}</option>`;
                        ctr.append(str);
                    });
                    ctr.select2();
                    ctr.val($scope.site.ProjectId).select2().trigger('change');
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
            
            var fleetOwnerId = $('#drpFleetOwner').val();
            var projectId = $('#drpProject').val();
            var cityId = $('#drpCity').val();
            if (fleetOwnerId === "0") {
                swal('Required', 'Please select fleet owner.', 'warning');
                mApp.unblockPage();
                return;
            }
            if (projectId === '0') {
                swal('Required', 'Please select project.', 'warning');
                mApp.unblockPage();
                return;
            }
            if (cityId === '0') {
                swal('Required', 'Please select city.', 'warning');
                mApp.unblockPage();
                return;
            }
            $scope.site.FleetOwnerId = parseInt(fleetOwnerId);
            $scope.site.ProjectId = parseInt(projectId);
            $scope.site.CityId = parseInt(cityId);

            var SDate = $scope.site.SiteStartDate;
            var EDate = $scope.site.SiteEndDate;
            var DDate = $scope.site.BidDueDate;
            if (new Date(DDate) >= new Date(SDate) || new Date(DDate) >= new Date(EDate) || new Date(SDate) >= new Date(EDate)) {
                var popUpMsg = ""
                if (new Date(DDate) >= new Date(SDate) || new Date(DDate) >= new Date(EDate)) {
                    popUpMsg = "Due date is greater then starting date or end date, Do you want to continue!"
                }
                else if (new Date(SDate) >= new Date(EDate)) {
                    popUpMsg = "Start date is greater then end date, Do you want to continue!"
                }
                swal({
                    title: "Are you sure?",
                    text: popUpMsg,
                    type: "warning",
                    showCancelButton: !0,
                    confirmButtonText: "Continue"
                })
                    .then(function (ok) {
                        debugger
                        if (ok.value) {
                            debugger
                            var workHourFrom = $("[name='WorkHoursForm']").val();
                            var workHourTo = $("[name='workHoursTo']").val();
                            $scope.site.WorkHoursAtFacility = workHourFrom + '-' + workHourTo;



                            com.get('/Site/CheckUserName/' + $scope.site.ProjectId + '/' + $scope.site.Id + '/' + $scope.site.Name).then(function (result) {
                                if (!result) {
                                    swal('Error', 'The site name with same project already exist.', 'warning');
                                    return;
                                }
                                else {
                                    mApp.blockPage({ message: "Updating..." });
                                    com.post('/Site/UpdateSite', $scope.site).then(function (result) {
                                        swal("Success", "Record has been updated.", "success")
                                            .then(function (e) {
                                                window.location = "/Site/SiteList";
                                            });

                                    });
                                }
                            });

                            mApp.unblockPage();
                            return;
                        } else {

                            mApp.unblockPage();
                            return;
                        }
                    });
                mApp.unblockPage();
                return;
            }
            else {
                $scope.site.FleetOwnerId = parseInt(fleetOwnerId);
                $scope.site.ProjectId = parseInt(projectId);
                $scope.site.CityId = parseInt(cityId);
            }

            var workHourFrom = $("[name='WorkHoursForm']").val();
            var workHourTo = $("[name='workHoursTo']").val();
            $scope.site.WorkHoursAtFacility = workHourFrom + '-' + workHourTo;



            com.get('/Site/CheckUserName/' + $scope.site.ProjectId + '/' + $scope.site.Id + '/' + $scope.site.Name).then(function (result) {
                if (!result) {
                    swal('Error', 'The site name with same project already exist.', 'warning');
                    return;
                }
                else {
                    mApp.blockPage({ message: "Updating..." });
                    com.post('/Site/UpdateSite', $scope.site).then(function (result) {
                        swal("Success", "Record has been updated.", "success")
                            .then(function (e) {
                                window.location = "/Site/SiteList";
                            });

                    });
                }
            });


            
        };

        $('input').first().focus();


        $scope.UncheckVinylMaterial = function () {
            if ($scope.site.IsVinylGraphics == false) {
                $scope.site.ArlonVinylFilmRemoval = false;
                $scope.site.ArlonVinylFilmApply = false;
                $scope.site.AveryVinylFilmRemoval = false;
                $scope.site.AveryVinylFilmApply = false;
                $scope.site.M3VinylFilmRemoval = false;
                $scope.site.M3VinylFilmApply = false;
                $scope.site.ArlonReflectiveFilmRemoval = false;
                $scope.site.ArlonReflectiveFilmApply = false;
                $scope.site.AveryReflectiveFilmRemoval = false;
                $scope.site.AveryReflectiveFilmApply = false;
                $scope.site.M3ReflectiveFilmRemoval = false;
                $scope.site.M3ReflectiveFilmApply = false;
                $scope.site.TotalSqFtRemoval = 0;
                $scope.site.TotalSqFtApply = 0;
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

function getProjects() {
    mApp.blockPage({ message: "Loading Projects..." });
    var val = $('#drpFleetOwner').val();
    $.ajax({
        url: '/Dropdown/GetProjects/' + val,
        success: function (data) {
            if (data.length === 0)
            {
                var drp = $('#drpProject');
                drp.children().remove();
                drp.append('<option  value="0">--No Project Found--</option>');
                //drp.select2('destroy');
                drp.select2();
                mApp.unblockPage();
                return;
            }
            var drp = $('#drpProject');
            drp.children().remove();
            $.each(data, function (index, item) {
                var str = `<option value="${item.Id}">${item.Value}</option>`;
                drp.append(str);
            });
            drp.select2();
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });
}