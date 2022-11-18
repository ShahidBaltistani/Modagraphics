(function () {
    'use strict';


    angular
        .module('app')
        .filter('jsonDate', function () {
            return function (input) {
                var date = new Date(input.match(/\d+/)[0] * 1);
                return (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
            };
        })
        .controller('project_list', project_list);

    project_list.$inject = ['$scope', 'com'];

    function project_list($scope, com) {
        var step = 4;
        $scope.maxSize = 6;
        $scope.totalCount = 0;
        $scope.pageIndex = 1;
        $scope.pageSizeSelected = 5;
        $scope.numOfPages = 1;
        $scope.imgModal = { title: '', src: '' };
        $scope.searchModel = { projectname: '', fleetOwnerName: '', projectAddress: '', projectContactName: '', projectContactPhone:'' }; //fromdate: '', todate: '',
        $scope.projectlist = {};

        $('input').first().focus();

        $scope.getProjectList = function () {
            
            mApp.blockPage({ message: "Fetching..." });
            //var fleetOwnerId = $('#drpFleetOwner').val();
            var obj = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSizeSelected,
                projectname: $scope.searchModel.projectname,
                //fromdate: $scope.searchModel.fromdate,
                //todate: $scope.searchModel.todate,
                fleetOwnerName: $scope.searchModel.fleetOwnerName,
                projectContactName: $scope.searchModel.projectContactName,
                projectContactPhone: $scope.searchModel.projectContactPhone,
                projectAddress: $scope.searchModel.projectAddress,
            };
            com.post("/Project/GetProjects", obj).then(function (result) {
                $scope.projectlist = result.ProjectList;
                $scope.totalCount = result.TotalRecords;
                $scope.numOfPages = Math.ceil($scope.totalCount / $scope.pageSizeSelected);
                $scope.maxSize = $scope.maxSize <= $scope.pageIndex ? $scope.pageIndex + 1 : $scope.maxSize;
                $scope.rightHide = $scope.numOfPages > $scope.maxSize;
            });
        }

        //Calling to load fleet owner list on first time load
        $scope.getProjectList();

        $scope.changeStatus = function () {
            console.log('status changed to '
                + result.ProjectList)
        };

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.getProjectList();
        };

        $scope.clearSearch = function () {
            $scope.pageIndex = 1;
            $scope.searchModel = { projectname: '', fromdate: '', todate: '' };
            //var ctr = $('#drpFleetOwner');
            //ctr.val(0).select2().trigger('change');
            $scope.getProjectList();
        };

        selectFirstOption('drpPage');

        $scope.pageChanged = function (index) {
            $scope.pageIndex = index;
            $scope.getProjectList();
        };

        $scope.previousPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex -= 1;
                $scope.getProjectList();
            }
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex += 1;
                $scope.getProjectList();
            }
        };

        $scope.firstPage = function () {
            if ($scope.pageIndex > 1) {
                $scope.pageIndex = 1;
                $scope.getProjectList();
            }
        };

        $scope.lastPage = function () {
            if ($scope.pageIndex < $scope.numOfPages) {
                $scope.pageIndex = $scope.numOfPages;
                $scope.getProjectList();
            }
        };

        $scope.changePageSize = function () {
            $scope.maxSize = 6;
            $scope.pageIndex = 1;
            $scope.getProjectList();
        };

        $scope.morePages = function () {
            $scope.maxSize += step;
            $scope.rightHide = $scope.numOfPages > $scope.maxSize;
        };

        //mApp.blockPage({ message: "Loading..." });
        //com.get('/Dropdown/GetFleetOwner').then(function (result) {
        //    var ctr = $('#drpFleetOwner');
        //    angular.forEach(result, function (value) {
        //        var str = `<option value="${value.Id}">${value.Value}</option>`;
        //        ctr.append(str);

        //    });
        //    ctr.select2();
        //});
    }
})();