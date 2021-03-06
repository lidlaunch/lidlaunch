﻿$(document).ready(function () {

    //$("#chkSameAsShipping").change(function () {
    //    if (this.checked) {
    //        var address = $('#txtAddress').val();
    //        var city = $('#txtCity').val();
    //        var state = $('#txtState').val();
    //        var zip = $('#txtZip').val();

    //        $('#txtAddressBill').val(address);
    //        $('#txtCityBill').val(city);
    //        $('#txtStateBill').val(state);
    //        $('#txtZipBill').val(zip);
    //    } else {
    //        $('#txtAddressBill').val('');
    //        $('#txtCityBill').val('');
    //        $('#txtStateBill').val('');
    //        $('#txtZipBill').val('');
    //    }
    //});

});
function SubmitOrder() {
    var total = $('#lblTotal').text();
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

    var useShipAddressForBilling = $('#rdUseSameAsShipping').prop('checked');

    if (useShipAddressForBilling) {
        billFirstName = shipFirstName;
        billLastName = shipLastName;
        billAddress = shipAddress;
        billCity = shipCity;
        billState = shipState;
        billZip = shipZip;
        billPhone = shipPhone
    }

    showLoading();
    $.ajax({
        type: "POST",
        url: '/Cart/SubmitOrder',
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        data: JSON.stringify({
            "total": total, "firstName": shipFirstName, "lastName": shipLastName, "email": email, "phone": shipPhone, "address": shipAddress, "city": shipCity, "state": shipState, "zip": shipZip, "addressBill": billAddress, "cityBill": billCity, "stateBill": billState, "zipBill": billZip, "paymentGuid": ""
        }),
        //data: JSON.stringify({ "total": total, "firstName": firstName, "lastName": lastName, "email": email, "phone": phone}),
        success: function (result) {
            fbq('track', 'Purchase', {
                content_name: 'Web Hat Order',
                content_category: 'Web Hat Order',
                content_ids: '0',
                content_type: 'product',
                value: total,
                currency: 'USD'
            });
            setTimeout(function () {
                window.location = 'https://lidlaunch.com/cart/payment?PaymentCode=' + result;
            }, 1500);

        },
        error: function () {
            displayPopupNotification('There was an error creating your order please try again.', 'error', false);
        }
    });

}
function AddItemToCart(itemName, itemCategory, itemPrice) {
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
            "productId": productId, "qty": qty, "size": size, "typeId": typeId, "colorId": colorId
        }),
        success: function (result) {
            $('#cartTotal').text(result);
            displayPopupNotification('Item added to cart.', 'error', false);
            fbq('track', 'AddToCart', {
                content_name: itemName,
                content_category: itemCategory,
                content_ids: [productId],
                content_type: 'product',
                value: (itemPrice * qty),
                currency: 'USD'
            });
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
        data: JSON.stringify({ "cartGuid": cartGuid }),
        success: function (result) {
            location.reload();
        },
        error: function () {
            alert('error');
        }
    });
}
//function renderPaypalButtons(price, items, name, line1, city, zip, phone, state) {
function renderPaypalButtons(price, items, shippingCost, subtotal) {
    // Render the PayPal button

    paypal.Button.render({

        // Set your environment

        env: 'production', // sandbox | production

        // Specify the style of the button

        style: {
            size: 'medium', // small | medium | large | responsive
            shape: 'rect',  // pill | rect
            tagline: false
        },

        funding: {
            allowed: [paypal.FUNDING.CREDIT]
        },

        // PayPal Client IDs - replace with your own
        // Create a PayPal app: https://developer.paypal.com/developer/applications/create

        client: {
            sandbox: 'AUGWFUyOkNKvJMkpTl2mEoKxi3R3XQDeS7A2miRXir2epkYH3Xq2VP29C5KGGrGBgOugE7zCmZEw54C-',
            production: 'AdnLTOhpnQgW42pJFrli9Oer3A-oKrfj8aykjj-XqmhBcamkINRmhIR9J1n8rcxcWwDTUSYaLk4ipi0y'
        },

        // Wait for the PayPal button to be clicked

        payment: function (data, actions) {

            return actions.payment.create({
                payment: {
                    transactions: [
                        {
                            "amount": { "total": price, "currency": "USD", "details": { "shipping": shippingCost, "tax": 0, "subtotal": subtotal } },
                            "description": "LidLaunch Order",
                            "item_list": {
                                "items": JSON.parse(items)
                            }
                        }
                    ]
                }
            });

        },

        // Wait for the payment to be authorized by the customer

        onAuthorize: function (data, actions) {
            return actions.payment.execute().then(function () {
                SubmitOrder();
            });
        }

    }, '#paypal-button-container');
}
function showPaypalButtons() {
    $('#chckoutWizzard').hide();
    $('#paypalButtons').slideDown();
    var total = $('#lblTotal').text();
    var shippingCost = $('#shippingCost').text();
    var subTotal = total - shippingCost;
    var productList = $('#productList').text();
    renderPaypalButtons(total, productList, shippingCost, subTotal);
}
function trackCartPixel(totalItems, totalPrice) {
    fbq('track', 'InitiateCheckout', {
        content_category: 'Web Order Hats',
        content_ids: ['0'],
        content_type: 'product',
        num_items: totalItems,
        value: totalPrice,
        currency: 'USD'
    });
}

