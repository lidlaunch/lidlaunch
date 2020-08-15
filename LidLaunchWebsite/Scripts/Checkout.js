function proceedToPayment() {
    $('#shipingSection').hide();
    setShipToSummary();
    $('#paymentStep').fadeIn();
}
function setShipToSummary() {
    var shipAddress = $('#txtShippingAddress').val();
    var shipCity = $('#txtShippingCity').val();
    var shipState = $('#selShippingState').children("option:selected").val();
    var shipZip = $('#txtShippingZip').val();

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
    var expMonth = $('#txtExpirationMonth').val();
    var expYear = $('#txtExpirationYear').val();
    var ccNumber = $('#txtCardNumber').val();
    var cType = $('#txt').val();    

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
            if (result == "error") {
                alert("error processing payment - please try again");   
            } else {
                if (isBulkOrder) {
                    // navigate to bulk order payment confirmation screen
                    window.location = 'http://lidlaunch.com/bulk/payment?id=' + result;
                } else {
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

}