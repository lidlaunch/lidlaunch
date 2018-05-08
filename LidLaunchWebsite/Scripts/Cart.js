$(document).ready(function () {

    $("#chkSameAsShipping").change(function () {
        if (this.checked) {
            var address = $('#txtAddress').val();
            var city = $('#txtCity').val();
            var state = $('#txtState').val();
            var zip = $('#txtZip').val();

            $('#txtAddressBill').val(address);
            $('#txtCityBill').val(city);
            $('#txtStateBill').val(state);
            $('#txtZipBill').val(zip);
        } else {
            $('#txtAddressBill').val('');
            $('#txtCityBill').val('');
            $('#txtStateBill').val('');
            $('#txtZipBill').val('');
        }
    });

});
function SubmitOrder() {
    var total = $('#lblTotal').text();
    $('#OrderAmmount').val(total);
    //$('#paypalButton').click();
    var firstName = $('#txtFirstName').val();
    var lastName = $('#txtLastName').val();
    var email = $('#txtEmail').val();
    var phone = $('#txtPhone').val();
    var address = $('#txtAddress').val();
    var city = $('#txtCity').val();
    var state = $('#txtState').val();
    var zip = $('#txtZip').val();
    var addressBill = $('#txtAddressBill').val();
    var cityBill = $('#txtCityBill').val();
    var stateBill = $('#txtStateBill').val();
    var zipBill = $('#txtZipBill').val();   
    if (firstName == '' || lastName == '' || email == '' || phone == '' || address == '' || city == '' || state == '' || zip == '' || addressBill == '' || cityBill == '' || stateBill == '' || zipBill == '')
    {
        displayPopupNotification('You must enter in all required fields to continue.', 'error', false);
    } else {
        if (validateEmail(email)){
            if (validatePhone(phone)) {
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Cart/SubmitOrder',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ "total": total, "firstName": firstName, "lastName": lastName, "email": email, "phone": phone, "address": address, "city": city, "state": state, "zip": zip, "addressBill": addressBill, "cityBill": cityBill, "stateBill": stateBill, "zipBill": zipBill }),
                    success: function (result) {
                        //$('#ReturnURL').val('http://lidlaunch.com/cart/payment?PaymentCode=' + result[0]);
                        //$('#orderInformation').val('Order ID: ' + result[1]);
                        //$('#paypalButton').click();
                        window.location = 'http://lidlaunch.com/cart/payment?PaymentCode=' + result[0];
                    },
                    error: function () {
                        displayPopupNotification('There was an error creating your order please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Please enter a valid phone.', 'error', false);
            }
        } else {
            displayPopupNotification('Please enter a valid email.', 'error', false);
        }
    }    
}
function AddItemToCart() {
    var productId = $('#lblProductId').text();
    var qty = $('#txtQty').val();
    var size = $('#selectSize').val();
    var typeId = $('#typeId').text();
    var colorId = $('#colorId').text();

    $.ajax({
        type: "POST",
        url: '/Cart/AddItemToCart',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            "productId": productId, "qty": qty, "size": size, "typeId": typeId, "colorId": colorId }),
        success: function (result) {
            $('#cartTotal').text(result);
            displayPopupNotification('Item added to cart.', 'error', false);
        },
        error: function () {
            alert('error');
        }
    });
}

function RemoveItemFromCart(that) {
    var cartGuid = $(that).closest('.productDetails').find('.cartGuid').text();

    $.ajax({
        type: "POST",
        url: '/Cart/RemoveItemFromCart',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "cartGuid": cartGuid}),
        success: function (result) {
            location.reload();
        },
        error: function () {
            alert('error');
        }
    });
}

function renderPaypalButtons(price, items, name, line1, city, zip, phone, state) {
    // Render the PayPal button

     paypal.Button.render({

        // Set your environment

         env: 'production', // sandbox | production

        // Specify the style of the button

        style: {
            size:  'medium', // small | medium | large | responsive
            shape: 'rect',  // pill | rect
            tagline: false
        },

        funding: {
            allowed: [ paypal.FUNDING.CREDIT ]
        },

        // PayPal Client IDs - replace with your own
        // Create a PayPal app: https://developer.paypal.com/developer/applications/create

        client: {
            sandbox:    'AUGWFUyOkNKvJMkpTl2mEoKxi3R3XQDeS7A2miRXir2epkYH3Xq2VP29C5KGGrGBgOugE7zCmZEw54C-',
            production: 'AdnLTOhpnQgW42pJFrli9Oer3A-oKrfj8aykjj-XqmhBcamkINRmhIR9J1n8rcxcWwDTUSYaLk4ipi0y'
        },

        // Wait for the PayPal button to be clicked

        payment: function(data, actions) {
            
            return actions.payment.create({
                payment: {
                    transactions: [
                        {
                            "amount": { "total": price, "currency": "USD" },
                            "description": "Lid Launch Order",
                            "item_list": {
                                "items": JSON.parse(items)                                                         
                            }
                        }
                    ]
                }
            });                   
                    
        },

        // Wait for the payment to be authorized by the customer

        onAuthorize: function(data, actions) {
            return actions.payment.execute().then(function() {
                SubmitOrder();
            });
        }

    }, '#paypal-button-container');
}
function showPaypalButtons() {
    var productList = $('#productList').text();
    var total = $('#lblTotal').text();
    $('#OrderAmmount').val(total);
    var firstName = $('#txtFirstName').val();
    var lastName = $('#txtLastName').val();
    var email = $('#txtEmail').val();
    var phone = $('#txtPhone').val();
    var address = $('#txtAddress').val();
    var city = $('#txtCity').val();
    var state = $('#txtState').val();
    var zip = $('#txtZip').val();
    var addressBill = $('#txtAddressBill').val();
    var cityBill = $('#txtCityBill').val();
    var stateBill = $('#txtStateBill').val();
    var zipBill = $('#txtZipBill').val();
    if (firstName == '' || lastName == '' || email == '' || phone == '' || address == '' || city == '' || state == '' || zip == '' || addressBill == '' || cityBill == '' || stateBill == '' || zipBill == '') {
        displayPopupNotification('You must enter in all required fields to continue.', 'error', false);
    } else {
        if (validateEmail(email)) {
            if (validatePhone(phone)) {
                $('#customerInfo').hide();
                $('#paypalButtons').show();
                renderPaypalButtons(total, productList, firstName + ' ' + lastName, address, city, zip, phone, state);
            } else {
                displayPopupNotification('Please enter a valid phone.', 'error', false);
            }
        } else {
            displayPopupNotification('Please enter a valid email.', 'error', false);
        }
    }
}

