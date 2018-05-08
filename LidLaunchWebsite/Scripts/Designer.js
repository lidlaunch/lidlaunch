var user = {
    "Id": "",
    "userId": "",
    "shopName": "",
    "paypalAddress": "",
    "street": "",
    "city": "",
    "state": "",
    "zip": "",
    "phone": ""
}

function saveDesigner() {
    var designerId = $('#lblDesignerId').text();
    var shopName = $('#txtShopName').val();
    var paypalAddress = $('#txtPaypalAddress').val();
    var street = $('#txtStreet').val();
    var city = $('#txtCity').val();
    var state = $('#txtState').val();
    var zip = $('#txtZip').val();
    var phone = $('#txtPhone').val();
   

    if (shopName == '' || paypalAddress == '' || street == '' || city == '' || state == '' || zip == '' || phone == '') {
        displayPopupNotification('You must enter all required fields.', 'error', false);
    } else {
        if (validateEmail(paypalAddress)) {
            if (validatePhone(phone)) {
                showLoading();
                if (designerId == '') {
                    $.ajax({
                        type: "POST",
                        url: '/Designer/CreateDesigner',
                        data: JSON.stringify({ "shopName": shopName, "paypalAddress": paypalAddress, "street": street, "city": city, "state": state, "zip": zip, "phone": phone }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            //TODO: navigate to designer profile page
                            window.location = "/Create/GetStarted";
                        },
                        error: function (err) {
                            displayPopupNotification('Error creating designer please try again.', 'error', false);
                        }
                    });
                } else {
                    $.ajax({
                        type: "POST",
                        url: '/Designer/UpdateDesigner',
                        data: JSON.stringify({ "shopName": shopName, "paypalAddress": paypalAddress, "street": street, "city": city, "state": state, "zip": zip, "phone": phone }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            window.location = "/Designer/Profile?id=" + data
                        },
                        error: function (err) {
                            displayPopupNotification('Error updating designer please try again.', 'error', false);
                        }
                    });
                }
            } else {
                displayPopupNotification('Please enter a valid phone number.', 'error', false);
            }
        } else {
            displayPopupNotification('Your Paypal address needs to be your valid Paypal email address.', 'error', false);
        }        
    }
}