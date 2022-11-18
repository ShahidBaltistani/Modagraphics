(function () {
    'use strict';

    angular.module('app', ['ngRoute', 'ui.bootstrap']);

    angular.module("app").directive("ngImage", function () {
        return {
            require: "ngModel",
            link: function postLink(scope, elem, attrs, ngModel) {
                elem.on("change", function (e) {
                    var f = elem[0].files;
                    if (f.length > 0) {
                        mApp.blockPage();
                        var file = elem[0].files[0];
                        var reader = new FileReader();

                        var options = {
                            checkOrientation: true,
                            maxWidth: 980,
                            maxHeight: 980,
                            minWidth: 0,
                            minHeight: 0,
                            width: undefined,
                            height: undefined,
                            quality: 0.7,
                            mimeType: '',
                            convertSize: 5000000,
                            success: function (compressedFile) {
                                reader.readAsDataURL(compressedFile);
                                reader.onload = function (e) {
                                    ngModel.$setViewValue(e.target.result);
                                    mApp.unblockPage();
                                }
                            },
                            error: function (e) {
                                mApp.unblockPage();
                                window.alert(e.message);
                            },
                        };

                        new ImageCompressor(file, options);
                    } else {
                        ngModel.$setViewValue('null');
                    }
                })
            }
        }
    });

    angular.module("app").directive("ngNum", function () {
        return {
            restrict: 'A',
            link: function postLink(scope, elem, attrs) {
                elem.inputmask({ mask: "9", repeat: attrs.ngNum, greedy: !1 });
            }
        }
    });

    angular.module("app").directive("ngZip", function () {
        return {
            link: function postLink(scope, elem, attrs) {
                elem.inputmask({ mask: "9", repeat: 5, greedy: !1 });
            }
        }
    });

    angular.module("app").directive("ngPhone", function () {
        return {
            link: function postLink(scope, elem, attrs) {
                elem.inputmask({ mask: "9", repeat: 15, greedy: !1 });
            }
        }
    });
    angular.module("app").directive('nameregex', function () {
         function link(scope, elem, attrs, ngModel) {
             ngModel.$parsers.push(function (viewValue) {
                 var reg = /^[^`~!#$%/\^&*+={}|[\]\\:';"<>.?]*$/;
                 // if view values matches regexp, update model value
                 if (viewValue.match(reg)) {
                     return viewValue;
                 }
                 // keep the model value as it is
                 var transformedValue = ngModel.$modelValue;
                 ngModel.$setViewValue(transformedValue);
                 ngModel.$render();
                 return transformedValue;
             });
         }

         return {
             restrict: 'A',
             require: 'ngModel',
             link: link
         };
     });

    angular.module("app").filter('range', function () {
        return function (input, total) {
            total = parseInt(total);
            for (var i = 1; i <= total; i++)
                input.push(i);
            return input;
        };
    });

    //angular.module("app").filter('mydate', function () {
    //    return function (input, format) {
    //        //debugger;
    //        input = input.replace('/Date(', '').replace(')/', '');
    //        var date = new Date(parseInt(input));

    //        var str = date.toLocaleDateString();
    //    };
    //});

    angular.module('app').filter('dateFormat', function ($filter) {
        var angularDateFilter = $filter('date');
        return function (input, filter) {
            if (input === null) {
                return 'N/A';
            } else {
                input = input.replace('/Date(', '').replace(')/', '');
                if (filter) {
                    return angularDateFilter(input, filter);
                } else {
                    return angularDateFilter(input, 'dd/MM/yyyy');
                }
            }
        }
    });

})();

function selectFirstOption(selectId) {
    setTimeout(function () {
        $('#' + selectId).children().first().remove();
        $('#' + selectId).children().first().attr('selected', 'selected');
    });
};