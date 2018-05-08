function displayPopupNotification(message, messagetype, reloadPage) {
    //types: success, error, warning
    var container;

    if (messagetype == 'error') {
        //error
        container = $('#errorNotifcation');
    }
    //else if (messagetype == 'warning') {
    //    //warning
    //    container = $('#dialog-notifications-warning');
    //}
    //else {
    //    //other
    //    container = $('#dialog-notifications-success');
    //}

    //we do not encode displayed message
    var htmlcode = '';
    if ((typeof message) == 'string') {
        htmlcode = '<p>' + message + '</p>';
    } else {
        for (var i = 0; i < message.length; i++) {
            htmlcode = htmlcode + '<p>' + message[i] + '</p>';
        }
    }

    htmlcode = htmlcode + '<p id="closeError" onclick="closeError();">Close</p>'
    container.find('#errorContent').html(htmlcode);
    hideLoading();
    $('#errorNotifcation').show();
}
function closeError() {
    $('#errorNotifcation').hide();
}
function errorFunc() {
    alert('error');
}
function showLoading() {
    $('#loadingPanel').show();
}
function hideLoading() {
    $('#loadingPanel').hide();
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function validateEmail(email) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email)) {
        return (true)
    }
    return (false)
}
function validatePhone(phone) {
    var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
    return phoneNumberPattern.test(phone);
}
var navShown = false;
function showNav() {
    if (navShown) {
        $('.mainNavigation').hide();
        navShown = false;
    } else {
        $('.mainNavigation').show();
        navShown = true;
    }
}
var profileLinksShown = false;
function showProfileLinks() {
    if (profileLinksShown) {
        $('#profileMenu').hide();
        profileLinksShown = false;
    } else {
        $('#profileMenu').show();
        profileLinksShown = true;
    }
}