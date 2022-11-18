(function () {
    'use strict';

    angular
        .module('app')
        .factory('com', com);

    com.$inject = ['$http', '$q'];

    function com($http, $q) {
        var service = {
            get: get,
            post: post,
            getCountries: getCountries,
            getStatesOfCountry: getStatesOfCountry,
            getCitiesOfState: getCitiesOfState
        };

        return service;

        function get(url) {
            var deferred = $q.defer();
            $http.get(url).then(function (result) {
                mApp.unblockPage();
                restartSession();
                deferred.resolve(result.data);
            }, function (error) {
                showError(error);
            });
            return deferred.promise;
        };

        function post(url, data) {
            var deferred = $q.defer();
            $http.post(url, data).then(function (result) {
                mApp.unblockPage();
                restartSession();
                deferred.resolve(result.data);
            }, function (error) {
                showError(error);
                deferred.reject();
            });
            return deferred.promise;
        };

        function getCountries() {
            var deferred = $q.defer();
            get('/Dropdown/GetCountries').then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        function getStatesOfCountry(countryId) {
            var deferred = $q.defer();
            get('/Dropdown/GetStatesOfCountry/' + countryId).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        function getCitiesOfState(stateId) {
            var deferred = $q.defer();
            get('/Dropdown/GetCitiesOfState/' + stateId).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        function showError(error) {
            mApp.unblockPage();
            switch (error.status) {
                case 401:
                    swal("", "Your session has been expired. Please login again.", "warning").then(function (e) {
                        $.ajax({
                            url: '/Login/Off',
                            success: function (r) {
                                window.location = '/';
                            },
                            error: function (e) {
                                window.location = '/';
                            }
                        });
                    });
                    break;
                case 520:
                    swal(error.data.Caption, error.data.Message, "error");
                    break;
                case 404:
                    swal(error.data.Caption, error.data.Message, "error");
                    break;
                default:
                    swal("Error occured", error.data, "error");
            }
        };
    }
})();