﻿function proceedToPayment() {
    var shipAddress = $('#txtShippingAddress').val();
    var shipCity = $('#txtShippingCity').val();
    var shipState = $('#selShippingState').children("option:selected").val();
    var shipZip = $('#txtShippingZip').val();
    var email = $('#txtCustomerEmail').val();
    var firstName = $('#txtShippingFirstName').val();
    var lastName = $('#txtShippingLastName').val();

    $('#txtCustomerEmail').val(email.trim());
    email = email.trim();

    if (validateShippingInfo(firstName, lastName, shipAddress, shipCity, shipState, shipZip, email)) {
        $('#shipingSection').hide();
        setShipToSummary();
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
function setShipToSummary() {
    var shipAddress = $('#txtShippingAddress').val();
    var shipCity = $('#txtShippingCity').val();
    var shipState = $('#selShippingState').children("option:selected").val();
    var shipZip = $('#txtShippingZip').val();
    var email = $('#txtCustomerEmail').val();

    $('#spnContactPreview').text(email);

    $('#shipToSummary').text(shipAddress + ', ' + shipCity + ', ' + shipState + ' ' + shipZip);

    //if ($('#rdPaypal').prop('checked') == true) {
    //    $('#shipToSummary').text('PayPal shipping address');
    //    $('#shipToChange').hide(); 
    //} else {
    //    $('#shipToSummary').text(shipAddress + ', ' + shipCity + ', ' + shipState + ' ' + shipZip);        
    //    $('#shipToChange').show();
    //}
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
    var useSameAsShippingAddressForBilling = $('#rdUseSameAsShipping').prop('checked');

    if (!useSameAsShippingAddressForBilling) {
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

var attemptOrderWithoutArtwork = false;

function processPayment() {
    var isBulkOrder = false;
    var items = "";
    var data = new FormData();
    var continueToOrderSubmit = true;
    try {
        if ($('#isBulkOrder').text() == "true") {
            isBulkOrder = true;
        }



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

        var backStitching = $('#chkBackStitching').prop("checked");
        var leftSideStitching = $('#chkLeftSideStitching').prop("checked");
        var rightSideStitching = $('#chkRightSideStitching').prop("checked");
        var backStitchingComment = $('#txtBackStitching').val();
        var rightSideStitchingComment = $('#txtRightSideStitching').val();
        var leftSideStitchingComment = $('#txtLeftSideStitching').val();

        var shippingAddressJson = '{ "ShipToState" : "' + shipState + '", "ShipToStreet" : "' + shipAddress + '", "ShipToZip" : "' + shipZip + '", "ShipToCity" : "' + shipCity + '", "ShipToPhone" : "' + shipPhone + '", "ShipToFirstName" : "' + shipFirstName + '", "ShipToLastName" : "' + shipLastName + '" }';
        var billingAddressJson = "";
        if ($('#rdUseSameAsShipping').prop('checked') == true) {
            billingAddressJson = '{ "State" : "' + shipState + '", "Street" : "' + shipAddress + '", "Zip" : "' + shipZip + '", "City" : "' + shipCity + '", "PhoneNum" : "' + shipPhone + '" , "FirstName" : "' + shipFirstName + '", "LastName" : "' + shipLastName + '"}';
        } else {
            billingAddressJson = '{ "State" : "' + billState + '", "Street" : "' + billAddress + '", "Zip" : "' + billZip + '", "City" : "' + billCity + '", "PhoneNum" : "' + billPhone + '" , "FirstName" : "' + billFirstName + '", "LastName" : "' + billLastName + '"}';
        }

        var ccV = $('#txtSecurityCode').val();
        var ccExpMM = $('#selExpirationMonth').children("option:selected").val();
        var ccExpYY = $('#selExpirationYear').children("option:selected").val();
        var ccNumber = $('#txtCardNumber').val().replace(/\s/g, '');

        var files = [];
        var orderNotes = '';
        var artworkPlacement = '';
        var file = null;

        if (isBulkOrder) {
            if (!attemptOrderWithoutArtwork) {
                files = $('#bulkArtwork')[0].files;
                file = files[0];
            }
            orderNotes = $('#txtDetails').val();
            if (artworkPreExisting) {
                orderNotes = 'ARTWORK PRE-EXISTING : ' + orderNotes;
            }
            artworkPlacement = $('#artPlacement').text();
        }

        var totalPurchaseAmount = $('#lblTotal').text();


        data.append("file" + 0, file);
        data.append("cartItems", items);
        data.append("billingAddress", billingAddressJson);
        data.append("shippingAddress", shippingAddressJson);
        data.append("ccNumber", ccNumber);
        data.append("ccExpMM", ccExpMM);
        data.append("ccExpYY", ccExpYY);
        data.append("ccV", ccV);
        data.append("orderTotal", totalPurchaseAmount);
        data.append("email", email);
        data.append("isBulkOrder", isBulkOrder);
        data.append("orderNotes", orderNotes);
        data.append("artworkPlacement", artworkPlacement);
        data.append("shippingCost", shippingcost);
        data.append("backStitching", backStitching);
        data.append("leftSideStitching", leftSideStitching);
        data.append("rightSideStitching", rightSideStitching);
        data.append("backStitchingComment", backStitchingComment);
        data.append("leftSideStitchingComment", leftSideStitchingComment);
        data.append("rightSideStitchingComment", rightSideStitchingComment);
        showLoading();
    }
    catch (err) {
        continueToOrderSubmit = false;
    }
      


    if (continueToOrderSubmit) {
        $.ajax({
            type: "POST",
            url: '/Cart/PaymentWithCreditCard',
            contentType: false,
            processData: false,
            data: data,
            success: function (result) {
                if (result == "ccerror") {
                    hideLoading();
                    displayPopupNotification('Error processing payment, please check your credit card details and try again.', 'error', false);
                } else {
                    if (isBulkOrder) {
                        try {
                            fbq('track', 'Purchase', {
                                content_name: 'Bulk Hat Order',
                                content_category: 'Bulk Hat Order',
                                content_ids: '0',
                                content_type: 'product',
                                value: totalPurchaseAmount,
                                currency: 'USD'
                            });
                        }
                        catch (err) {
                            //error writing back to facebook..
                        }
                        // navigate to bulk order payment confirmation screen
                        setTimeout(function () {
                            window.location = 'https://lidlaunch.com/bulk/payment?id=' + result;
                        }, 1500);
                    } else {
                        try {
                            fbq('track', 'Purchase', {
                                content_name: 'Web Hat Order',
                                content_category: 'Web Hat Order',
                                content_ids: '0',
                                content_type: 'product',
                                value: totalPurchaseAmount,
                                currency: 'USD'
                            });
                        }
                        catch (err) {
                            //error writing back to facebook..
                        }
                        // navigate to the normal payment confirmation screen
                        setTimeout(function () {
                            window.location = 'https://lidlaunch.com/cart/payment?PaymentCode=' + result;
                        }, 1500);
                    }
                }
            },
            error: function (error) {
                if (!attemptOrderWithoutArtwork) {
                    attemptOrderWithoutArtwork = true;
                    processPayment();
                } else {
                    displayPopupNotification('Please Contact Us. There was an issue processing your order.', 'error', false);
                }
            }
        });
    } else {
        //submit order with payment issues???
        displayPopupNotification('Please Contact Us. There was an issue processing your order.', 'error', false);
    }    
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
    $(window).scrollTop(0);
}
var mobileCartShown = false;

function showHideMobileCart() {
    if (mobileCartShown) {
        mobileCartShown = false;
        $('#showHideSummary').text('Show order summary');
        $('.checkOutCartItems').detach().appendTo('#checkoutHolderRight');
    } else {
        mobileCartShown = true;
        $('#showHideSummary').text('Hide order summary');
        $('.checkOutCartItems').detach().appendTo('#clonedCart');
    }

}