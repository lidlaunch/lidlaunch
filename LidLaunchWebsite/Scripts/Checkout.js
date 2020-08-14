function proceedToPayment() {
    $('#shipingSection').hide();
    $('#paymentStep').fadeIn();
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
}
function usePaypal() {
    $('#creditCardEntry').hide();
    $('#paypalEntry').slideDown();
    $('#rdCreditCard').prop('checked', false);
    $('#rdPaypal').prop('checked', true);
    $('#btnPayNow').hide();
    $('#btnCompleteOrder').show();
    $('#billingAddres').hide();
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

    var isBulkOrder = $('#isBulkOrder').text();

    var items = $('#productList').text();
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

    var creditCardJson = '{ "billing_address" : ' + billingAddressJson + ', "cvv2" : "' + cvv + '", "expire_month" : "' + expMonth + '", "expire_year" : "' + expYear + '", "first_name" : "' + billFirstName + '", "last_name" : "' + billLastName + '", "number" : "' + ccNumber + '" }';

    $.ajax({
        type: "POST",
        url: '/Cart/PaymentWithCreditCard',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            "creditCard": creditCardJson, "cartItems": items, "billingAddress": billingAddressJson, "shippingAddress": shippingAddressJson, "shippingRecipient": shippingRecipient, "shippingPrice": shippingcost, "email": email, "isBulkOrder": isBulkOrder
        }),
        success: function () {
            if (result == "error") {
                alert("error processing payment - please try again");   
            } else {
                if (isBulkOrder == "true") {
                    // navigate to bulk order payment confirmation screen
                } else {
                    // navigate to the normal paymetn confirmation screen
                }
            }            
        },
        error: function () {
            alert('error');
        }
    });



}