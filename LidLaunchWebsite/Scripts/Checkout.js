function proceedToPayment() {
    var shipAddress = $('#txtShippingAddress').val();
    var shipCity = $('#txtShippingCity').val();
    var shipState = $('#selShippingState').children("option:selected").val();
    var shipZip = $('#txtShippingZip').val();
    var email = $('#txtCustomerEmail').val();
    var firstName = $('#txtShippingFirstName').val();
    var lastName = $('#txtShippingLastName').val();

    if (validateShippingInfo(firstName, lastName, shipAddress, shipCity, shipState, shipZip, email)) {
        $('#shipingSection').hide();
        setShipToSummary(shipAddress, shipCity, shipState, shipZip);
        $('#paymentStep').fadeIn();
    }    
}
function validateShippingInfo(firstName, lastName, address, city, state, zip, email) {
    if (validateEmail(email) == false) {
        displayPopupNotification('Please enter a valid email address.', 'error', false);
        return false;
    }

    if (lastName == '' || firstName == '' || address == '' || city == '' || zip == '') {
        displayPopupNotification('You must enter in all required fields.', 'error', false);
        return false;
    }

    return true;
}
function cc_format(value) {
    var v = value.replace(/\s+/g, '').replace(/[^0-9]/gi, '')
    var matches = v.match(/\d{4,16}/g);
    var match = matches && matches[0] || ''
    var parts = []

    for (i = 0, len = match.length; i < len; i += 4) {
        parts.push(match.substring(i, i + 4))
    }

    if (parts.length) {
        return parts.join(' ')
    } else {
        return value
    }
}
function formatPhoneNumber(input) {
    // Strip all characters from the input except digits
    input = input.replace(/\D/g, '');

    // Trim the remaining input to ten characters, to preserve phone number format
    input = input.substring(0, 10);

    // Based upon the length of the string, we add formatting as necessary
    var size = input.length;
    if (size == 0) {
        input = input;
    } else if (size < 4) {
        input = '(' + input;
    } else if (size < 7) {
        input = '(' + input.substring(0, 3) + ') ' + input.substring(3, 6);
    } else {
        input = '(' + input.substring(0, 3) + ') ' + input.substring(3, 6) + '-' + input.substring(6, 10);
    }
    return input;
}
function setShipToSummary(shipAddress, shipCity, shipState, shipZip) {
   

    if ($('#rdPaypal').prop('checked') == true) {
        $('#shipToSummary').text('PayPal shipping address');
        $('#shipToChange').hide(); 
    } else {
        $('#shipToSummary').text(shipAddress + ', ' + shipCity + ', ' + shipState + ' ' + shipZip);        
        $('#shipToChange').show();
    }
}
function goBackToShipping() {
    $('#shipingSection').fadeIn();
    $('#paymentStep').hide();
}
function useCreditCard() {
    $('#paypalEntry').hide();
    $('#creditCardEntry').slideDown();
    $('#rdPaypal').prop('checked', false);
    $('#rdCreditCard').prop('checked', true);
    $('#btnCompleteOrder').hide();
    $('#btnPayNow').show();
    $('#billingAddres').show();
    setShipToSummary();
}
function usePaypal() {
    $('#creditCardEntry').hide();
    $('#paypalEntry').slideDown();
    $('#rdCreditCard').prop('checked', false);
    $('#rdPaypal').prop('checked', true);
    $('#btnPayNow').hide();
    $('#btnCompleteOrder').show();
    $('#billingAddres').hide();
    setShipToSummary();
}
function useDifferentBillingAddress() {
    $('#differntBillingAddress').slideDown();    
    $('#rdUseSameAsShipping').prop('checked', false);
    $('#rdUsedifferentAddress').prop('checked', true);
}
function sameAsShipping() {
    $('#differntBillingAddress').hide();    
    $('#rdUseSameAsShipping').prop('checked', true);
    $('#rdUsedifferentAddress').prop('checked', false);

}
function validateCreditCardAndProcessPayment() {
    if (validateCreditCardFields()) {
        processPayment();
    }

}
function validateCreditCardFields() {
    var cvv = $('#txtSecurityCode').val();
    var expMonth = $('#selExpirationMonth').children("option:selected").val();
    var expYear = $('#selExpirationYear').children("option:selected").val();
    var ccNumber = $('#txtCardNumber').val();
    var alternateBilling = $('#rdUseSameAsShipping').prop('checked');

    if (!alternateBilling) {
        var shipAddress = $('#txtBillingAddress').val();
        var shipCity = $('#txtBillingCity').val();
        var shipState = $('#selBillingState').children("option:selected").val();
        var shipZip = $('#txtBillingZip').val();
        if (shipAddress == '' || shipCity == '' || shipState == '' || shipZip == '') {
            displayPopupNotification('Please enter your billing address information.', 'error', false);
            return false;
        }
    }

    if (cvv == '' || expMonth == '0' || expYear == '0' || ccNumber == '') {
        displayPopupNotification('Please check your Credit Card info and try again.', 'error', false);
        return false
    }

    fbq('track', 'AddPaymentInfo');

    return true;
    
}
function processPayment() {
    var isBulkOrder = false;

    if ($('#isBulkOrder').text() == "true") {
        isBulkOrder = true;
    }

    var items = "";

    if (isBulkOrder) {
        items = JSON.stringify(currentBulkProductList);
    } else {
        items = $('#productList').text();
    }
    
    var shippingcost = $('#shippingCost').text();
    var email = $('#txtCustomerEmail').val();
    //shipping info
    var shipFirstName = $('#txtShippingFirstName').val();
    var shipLastName = $('#txtShippingLastName').val();
    var shippingRecipient = shipFirstName + " " + shipLastName;

    var shipAddress = $('#txtShippingAddress').val();
    var shipCity = $('#txtShippingCity').val();
    var shipState = $('#selShippingState').children("option:selected").val();
    var shipZip = $('#txtShippingZip').val();
    var shipPhone = $('#txtPhone').val();

    var billFirstName = $('#txtBillingFirstName').val();
    var billLastName = $('#txtBillingLastName').val();

    var billAddress = $('#txtBillingAddress').val();
    var billCity = $('#txtBillingCity').val();
    var billState = $('#selBillingState').children("option:selected").val();
    var billZip = $('#txtBillingZip').val();
    var billPhone = $('#txtBillingPhone').val();

    var shippingAddressJson = '{ "state" : "' + shipState + '", "line1" : "' + shipAddress + '", "postal_code" : "' + shipZip + '", "city" : "' + shipCity + '", "phone" : "' + shipPhone + '" }';
    var billingAddressJson = "";
    if ($('#rdUseSameAsShipping').prop('checked') == true) {
        billingAddressJson = shippingAddressJson;
        billFirstName = shipFirstName;
        billLastName = shipLastName;
    } else {
        billingAddressJson = '{ "state" : "' + billState + '", "line1" : "' + billAddress + '", "postal_code" : "' + billZip + '", "city" : "' + billCity + '", "phone" : "' + billPhone + '" }';
    }

    var cvv = $('#txtSecurityCode').val();
    var expMonth = $('#selExpirationMonth').children("option:selected").val();
    var expYear = $('#selExpirationYear').children("option:selected").val();
    var ccNumber = $('#txtCardNumber').val();   

    var creditCardJson = '{ "billing_address" : ' + billingAddressJson + ', "cvv2" : "' + cvv + '", "expire_month" : "' + expMonth + '", "expire_year" : "' + expYear + '", "first_name" : "' + billFirstName + '", "last_name" : "' + billLastName + '", "number" : "' + ccNumber + '"}';

    var files = [];
    var orderNotes = '';
    var artworkPlacement = '';
    var file = null;

    if (isBulkOrder) {
        files = $('#bulkArtwork')[0].files;
        file = files[0];
        orderNotes = $('#txtDetails').val();
        if ($('#artworkPresetup').prop("checked")) {
            orderNotes = 'ARTWORK PRE-EXISTING : ' + orderNotes;
        }
        artworkPlacement = $('#artPlacement').text();
    }


    var data = new FormData();
    data.append("file" + 0, file);
    data.append("creditCard", creditCardJson);
    data.append("cartItems", items);
    data.append("billingAddress", billingAddressJson);
    data.append("shippingAddress", shippingAddressJson);
    data.append("shippingRecipient", shippingRecipient);
    data.append("shippingPrice", shippingcost);
    data.append("email", email);
    data.append("isBulkOrder", isBulkOrder);
    data.append("orderNotes", orderNotes);
    data.append("artworkPlacement", artworkPlacement);
    showLoading();

    var totalPurchaseAmount = $('#lblTotal').text();

    $.ajax({
        type: "POST",
        url: '/Cart/PaymentWithCreditCard',
        contentType: false,
        processData: false,
        data: data,
        //data: JSON.stringify({
        //    "creditCard": creditCardJson, "cartItems": items, "billingAddress": billingAddressJson, "shippingAddress": shippingAddressJson, "shippingRecipient": shippingRecipient, "shippingPrice": shippingcost, "email": email, "isBulkOrder": isBulkOrder
        //}),
        success: function (result) {
            if (result == "ccerror") {
                hideLoading();
                displayPopupNotification('Error processing payment, please check your credit card details and try again.', 'error', false);                 
            } else {
                if (isBulkOrder) {
                    fbq('track', 'Purchase', {
                        content_name: 'Bulk Hat Order',
                        content_category: 'Bulk Hat Order',
                        content_ids: '0',
                        content_type: 'product',
                        value: totalPurchaseAmount,
                        currency: 'USD'
                    }); 
                    // navigate to bulk order payment confirmation screen
                    window.location = 'http://lidlaunch.com/bulk/payment?id=' + result;
                } else {
                    fbq('track', 'Purchase', {
                        content_name: 'Web Hat Order',
                        content_category: 'Web Hat Order',
                        content_ids: '0',
                        content_type: 'product',
                        value: totalPurchaseAmount,
                        currency: 'USD'
                    }); 
                    // navigate to the normal paymetn confirmation screen
                    window.location = 'http://lidlaunch.com/cart/payment?PaymentCode=' + result;
                }
            }            
        },
        error: function () {
            alert('error');
        }
    });

}
function proceedToPayWithPaypal() {
    var isBulkOrder = false;

    if ($('#isBulkOrder').text() == "true") {
        isBulkOrder = true;
    }
    if (isBulkOrder) {        
        verifyAndShowPaypal();
    } else {
        showPaypalButtons();
    }
    fbq('track', 'AddPaymentInfo');

}