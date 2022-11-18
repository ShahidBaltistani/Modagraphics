(function () {
    'use strict';

    angular
        .module('app')
        .controller('site_add', site_add);

    site_add.$inject = ['$scope', 'com'];
  
    function site_add($scope, com) {
        $scope.site = {
            FleetOwnerId: 0,
            ProjectId: 0,
            Name: '',
            CSR: '',
            SalesMan: '',
            BidDueDate: '',
            SiteStartDate: '',
            SiteEndDate: '',
            MaximumBidAmount: 0,
            SalePrice: 0,
            ScopeOfWork: '',
            SpecialInstruction: '',
            Address: '',
            ZipCode: 0,
            CityId: 0,
            PONo: '',
            EPMSJobNo: '',
            TakeDownStatus: '2',
            SiteStatus: '0',
            IndoorFacility: false,
            BusinessHours: false,
            WeekendHours: false,
            IsSpottingTractor: false,
            StoreEquipmentOnSite: false,
            SpotVehicle: false,
            RemoveExistingGraphics: false,
            YearsOnVehicle: 0,
            CleaningAndPrepVehicle: false,
            PowerWash: false,
            IsVinylGraphics: false,
            SealOfDecals: false,
            ArlonVinylFilmRemoval: false,
            ArlonVinylFilmApply: false,
            AveryVinylFilmRemoval: false,
            AveryVinylFilmApply: false,
            M3VinylFilmRemoval: false,
            M3VinylFilmApply: false,
            ArlonReflectiveFilmRemoval: false,
            ArlonReflectiveFilmApply: false,
            AveryReflectiveFilmRemoval: false,
            AveryReflectiveFilmApply: false,
            M3ReflectiveFilmRemoval: false,
            M3ReflectiveFilmApply: false,
            TotalSqFtRemoval: 0,
            TotalSqFtApply: 0,
            OtherQuestionsComments: '',
            CustomerAvailabilityOnInstallationDay: false,
            InsideFacilities: false,
            IsDecalsReceived: false,
            VehicleCleanessBeforeArrival: false,
            IsInformed6FeetArea: false,
            VehicleAvailabilityDate: '',
            WorkHoursAtFacility: '',
            AdditionalContacts: '',
        };
        $scope.UniqueName = null;

        setTimeout(function () { $('input').first().focus() });
        
       
        $scope.save = function () {
         

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
            debugger
            var SDate = $scope.site.SiteStartDate;
            var EDate = $scope.site.SiteEndDate;
            var DDate = $scope.site.BidDueDate;
            debugger
            if (new Date(DDate) >= new Date(SDate) || new Date(DDate) >= new Date(EDate) || new Date(SDate) >= new Date(EDate)) {
                var popUpMsg = ""
                if (new Date(DDate) >= new Date(SDate) || new Date(DDate) >= new Date(EDate)) {
                    popUpMsg ="Due date is greater then starting date or end date, Do you want to continue!"
                }
                else if (new Date(SDate) >= new Date(EDate)) {
                    popUpMsg ="Start date is greater then end date, Do you want to continue!"
                }
                swal({
                    title: "Are you sure?",
                    text: popUpMsg,
                    type: "warning",
                    showCancelButton: !0,
                    confirmButtonText: "Continue"
                })
                    .then(function(ok) {
                        debugger
                        if (ok.value) {
                            debugger
                            var workHourFrom = $("[name='WorkHoursForm']").val();
                            var workHourTo = $("[name='workHoursTo']").val();
                            $scope.site.WorkHoursAtFacility = workHourFrom + '-' + workHourTo;

                            mApp.blockPage({ message: "Checking User Name..." });
                            com.get('/Site/CheckUserName/' + $scope.site.ProjectId + '/' + 0 + '/' + $scope.site.Name).then(function (result) {
                                if (!result) {
                                    swal('Error', 'The site name with same project already exist.', 'warning');
                                    return;
                                }
                                else {
                                    mApp.blockPage({ message: "Saving..." });
                                    com.post('/site/SaveProject', $scope.site).then(function (result) {
                                        swal("Success", "Record has been saved .", "success")
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

           
            //debugger
            var workHourFrom = $("[name='WorkHoursForm']").val();
            var workHourTo = $("[name='workHoursTo']").val();
            $scope.site.WorkHoursAtFacility = workHourFrom + '-' + workHourTo;
            mApp.blockPage({ message: "Checking User Name..." });
            com.get('/Site/CheckUserName/' + $scope.site.ProjectId + '/' + 0 + '/' + $scope.site.Name).then(function (result) {
                if (!result) {
                    swal('Error', 'The site name with same project already exist.', 'warning');
                    return;
                }
                else {
                    mApp.blockPage({ message: "Saving..." });
                    com.post('/site/SaveProject', $scope.site).then(function (result) {
                        swal("Success", "Record has been saved .", "success")
                            .then(function (e) {
                                window.location = "/Site/SiteList";
                            });
                    });
                }
            });

        };
















        mApp.blockPage({ message: "Loading..." });
        com.get('/Dropdown/GetFleetOwner').then(function (result) {
            var ctr = $('#drpFleetOwner');
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
                
            });
            ctr.select2();
        });

        mApp.blockPage({ message: "Loading..." });
        com.getCountries().then(function (result) {
            var ctr = $('#drpCountry');
            ctr.children().remove();
            angular.forEach(result, function (value) {
                var str = `<option value="${value.Id}">${value.Value}</option>`;
                ctr.append(str);
               
            });
            ctr.select2();
        });
        $('#drpState').select2();
        $('#drpCity').select2();

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

function getProjects() {
    mApp.blockPage({ message: "Loading Projects..." });
    var val = $('#drpFleetOwner').val();
    if (val === '0') {
        var drp = $('#drpProject');
        drp.children().remove();
        drp.append('<option  value="0">--Select Fleet Owner--</option>');
        drp.select2('destroy');
        drp.select2();
        mApp.unblockPage();
        return;
    }
    $.ajax({
        url: '/Dropdown/GetProjects/' + val,
        success: function (data) {
            if (data.length === 0) {
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
            //drp.select2('destroy');
            drp.select2();
            mApp.unblockPage();
        },
        error: function (error) {
            mApp.unblockPage();
            swal("Error", error.responseText, "error");
        }
    });
}
