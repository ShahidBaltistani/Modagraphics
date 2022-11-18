var timeout = 20;
var timer = 30;
var timeInterval;
var count = timer;
var innerTimeout;
var sessionTimeout;

function restartSession() {
    clearTimeout(sessionTimeout);
    sessionTimeout = setTimeout(() => {
        timeInterval = setInterval(() => $('#seconds').html(count--), 1000);
        $('#timeout-modal').modal('show');
        innerTimeout = setTimeout(() => {
            window.location = '/Login/Off';
        }, timer * 1000);
    }, timeout * 60000);
}

restartSession();

function keepMeAlive() {
    $.ajax({
        url: '/Login/Refresh',
        method: 'Get',
        success: function (e) {
            clearInterval(timeInterval);
            count = timer;
            clearTimeout(innerTimeout);
            restartSession();
            $('#timeout-modal').modal('hide');
        },
        error: function (e) {
            alert(e.responseText);
        }
    });
};


$.fn.updateSelect2 = function (source) {
    this.html(source);
    this.trigger('change.select2');
};

function GetCountries(success) {
    $.ajax({
        type: 'GET',
        url: '/DropDown/Countries',
        success: function (result) {
            var str = "<option value='0'>--Please Select--</option>";
            $.each(result, function myfunction(index, item) {
                str += "<option value='" + item.Id + "'>" + item.Text + "</option>";
            });
            success(str);
        },
        error: function (res) {
            swal("Error Occured", res.responseText);
        }
    });
}

function GetStates(id, success) {
    $.ajax({
        type: 'GET',
        url: '/DropDown/StatesOfCountry/' + id,
        success: function (result) {
            var str = "<option value='0'>--Select Country--</option>";
            $.each(result, function (index, item) {
                str += "<option value='" + item.Id + "'>" + item.Text + "</option>";
            });
            success(str);
        },
        error: function (res) {

        }
    });
}

function GetCities(id, success) {
    $.ajax({
        type: 'GET',
        url: '/Dropdown/CitiesOfState/' + id,
        success: function (result) {
            var str = "<option value='0'>--Select State--</option>";
            $.each(result, function (index, item) {
                str += "<option value='" + item.Id + "'>" + item.Text + "</option>";
            });
            success(str);
        },
        error: function (res) {

        }
    });
}
