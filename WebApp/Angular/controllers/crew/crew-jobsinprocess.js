(function () {
    'use strict';


    angular
        .module('app')
        .controller('crew_jobsinprocess', crew_jobsinprocess);

    crew_jobsinprocess.$inject = ['$scope', 'com'];

    function crew_jobsinprocess($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { ProjectName: '', SiteName: '', VehicleTypeName: '', VIN: '', LPN: '', Unit: '', StartedDate:''};
        $scope.crews = {};
        $scope.SiteId = $('#id').val();


        $scope.jobimages = {
            Id: 0,
            JobId: '',
            SiteId: '',
            CrewId: '',
            Picture: 'null',
            Remarks: '',
            Status: 0, 
            jobImagesStatus: 0,
            ImagesDeleteStatus: 0
        };
        $scope.imageTypes = [
            { Id: "1", name: "Starting" },
            { Id: "2", name: "Ending" }
        ];

        $('input').first().focus();

        $scope.Crew = function () {

            mApp.blockPage({ message: "Fetching..." });
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                ProjectName: $scope.searchModel.ProjectName,
                SiteName: $scope.searchModel.SiteName,
                VehicleTypeName: $scope.searchModel.VehicleTypeName,
                VIN: $scope.searchModel.VIN,
                LPN: $scope.searchModel.LPN,
                SiteId: $scope.SiteId,

                UnitNo: $scope.searchModel.Unit,
                //StartedDate: new Date($scope.searchModel.StartedDate)
                StartedDate: $scope.searchModel.StartedDate == null ? null : $scope.searchModel.StartedDate
            };
            com.post("/Crew/JobsInProcess", obj).then(function (result) {
                $scope.crews = result.crews;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
            //needs verify
            //$scope.clearData();
        }

        //Calling to load fleet owner list on first time load
        $scope.Crew();
        
        $scope.cancelmodal = () => {
           // $("#drpImageType").val("0");
            //$("#drpImageType").val("Select Image Type");
            $(":checkbox").prop('checked', false);
            $("#customFile").html("");
            $("#drpImageType").prop('selectedIndex', 0); 
        }

        $scope.uploadImageModal = (crew) => {
            debugger
            $("#drpImageType").prop('selectedIndex', 0); 
            var drpImageValue = $("#drpImageType").val();
            $scope.jobimages.Remarks = "";
            if (drpImageValue === "0") {
                $("#chkboxEnding").hide();
                $("#chkboxStarting").hide();
            }
            $("#customFile").val('');
            $(".custom-file-label").text('Select File')

            $("#upload-image-modal").modal('show');
            $scope.jobimages.JobId = crew.JobId;
            $scope.jobimages.SiteId = crew.SiteId;
            $scope.jobimages.CrewId = crew.CrewId;
        };

        $scope.incidentRemarksModal = (crew) => {
            //Incident Reporting
            $("#customFile2").val('');
            $(".custom-file-label").text('Select File');
            $scope.jobimages.Remarks = '';
            var remarks = $("#incidentRemarks").val();
            $("#drpImageType").prop('selectedIndex', 0); 
            if (remarks == "") {
                $("#incident-modal").modal('show');
                $scope.jobimages.JobId = crew.JobId;
                $scope.jobimages.SiteId = crew.SiteId;
                $scope.jobimages.CrewId = crew.CrewId;
                $scope.jobimages.Status = 5;
                $scope.jobimages.jobImagesStatus = 3;
            }
        };

        //$scope.save = function () {
        //    debugger
        //    var test = $scope.jobimages.Remarks;
        //    var drpImageValue = $("#drpImageType").val();
        //    var JobComplet = $("#chkJobCompleted").prop('checked');
        //    var DeleteImg = $("#chkDeleteImages").prop('checked');
        //    var image = $('#customFile').val();
        //    if (drpImageValue === "0" || image === "") {
        //        swal('Required', 'Please fill all required fields .', 'warning');
        //        mApp.unblockPage();
        //        return;

        //    }
        //    debugger
        //    if (DeleteImg) {
        //        $scope.jobimages.ImagesDeleteStatus = 2;
        //    }
        //    if (drpImageValue != 0 && $scope.jobimages.Remarks == "") {
        //        if (drpImageValue == 1) {
        //            //StartingPicture
        //            $scope.jobimages.Status = 2;
        //            $scope.jobimages.jobImagesStatus = 0;
        //        }
        //        else if (drpImageValue == 2) {
        //            //EndingPicture
        //            if (JobComplet) {
        //                $scope.jobimages.Status = 4;
        //                $scope.jobimages.jobImagesStatus = 1;
        //            }
        //            else {
        //                $scope.jobimages.Status = 2;
        //                $scope.jobimages.jobImagesStatus = 1;
        //            }
        //        }
        //        $('#upload-image-modal').modal('hide');

        //        mApp.blockPage({ message: "Adding image..." });
        //        com.post('/crew/savejobimage', $scope.jobimages).then(function (result) {
        //            $scope.Crew();
        //        });
        //    }
        //    else if ($scope.jobimages.Remarks != "") {
        //        $('#incident-modal').modal('hide');

        //        mApp.blockPage({ message: "Adding Incident Remarks..." });
        //        com.post('/crew/savejobimage', $scope.jobimages).then(function (result) {
        //            $scope.Crew();
        //        });

        //    }
        //};
        $scope.showImage = function (id) {
            $scope.imgModal = {};
            com.get('/Job/GetImage/' + id).then(function (result) {
                $scope.imgModal = result;

                console.log($scope.imgModal);


                if (result === 'null') {
                    swal("", "No image", "warning");
                } else {
                    $("#image-modal").modal('show');
                }
            });
        };

        $scope.Projects = {};
        $scope.ProjectAgainstSite = function (Id) {
            com.get('/Site/ProjectAgainstSite/' + Id).then(function (result) {
                $scope.Projects = result.ProjectDetail;
                $("#ProjectDetail").modal('show');
            });
        };

        $scope.search = function () {
            
            $scope.pageIndex = 1;
            $scope.Crew();
        };

        $scope.clearData = function () {
            $scope.jobimages = {
                Id: 0,
                JobId: '',
                SiteId: '',
                CrewId: '',
                Picture: 'null',
                Remarks: '',
                Status: 0,
                jobImagesStatus: 0,
                ImagesDeleteStatus: 0
            };
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { ProjectName: '', SiteName: '', VehicleTypeName: '', VIN: '', LPN: '', Unit: '', StartedDate: '' };
            $("#drpActive").val('undefine').selectpicker("refresh");
            $scope.Crew();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.Crew();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.Crew();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.Crew();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.Crew();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.Crew();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.Crew();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };


        $scope.save = function () {
            //debugger
            //var t = $("#drpImageType option:selected").text();
            var drpImageValue = $("#drpImageType").val();
            var JobComplet = $("#chkJobCompleted").prop('checked');
            var DeleteImg = $("#chkDeleteImages").prop('checked');
            var image = $('#customFile').val();
            if (drpImageValue === "0" || image === "") {
                swal('Required', 'Please fill all required fields .', 'warning');
                mApp.unblockPage();
                return;

            }
           // debugger
            if (DeleteImg) {
                $scope.jobimages.ImagesDeleteStatus = 2;
            }
            if (drpImageValue != 0 && $scope.jobimages.Remarks == "") {



                if (drpImageValue == 1) {
                    //StartingPicture
                    $scope.jobimages.Status = 2;
                    $scope.jobimages.jobImagesStatus = 0;
                }
                else if (drpImageValue == 2) {
                    //EndingPicture
                    if (JobComplet) {
                        $scope.jobimages.Status = 4;
                        $scope.jobimages.jobImagesStatus = 1;
                    }
                    else {
                        $scope.jobimages.Status = 2;
                        $scope.jobimages.jobImagesStatus = 1;
                    }
                }
               // debugger
                $('#upload-image-modal').modal('hide');
                debugger
                mApp.blockPage({ message: "Adding image..." });
                com.post('/crew/savejobimage', $scope.jobimages).then(function (result) {
                    $scope.Crew();
                });
            }
        };

        $scope.saveModel2 = function () {
            //debugger
            var drpImageValue = $("#incidentRemarks").val();
            var image2 = $('#customFile2').val();

            if (drpImageValue.length == "" || image2 == "") {
                swal('Required', 'Please fill all required fields .', 'warning');
                mApp.unblockPage();
                return;

            }
            else {
                $('#incident-modal').modal('hide');
                mApp.blockPage({ message: "Adding Incident Remarks..." });
                com.post('/crew/savejobimage', $scope.jobimages).then(function (result) {
                    $scope.Crew();
                });

            }
        };
    }
})();