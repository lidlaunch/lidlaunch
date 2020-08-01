var currentBulkProductList = [];
var currentTotalBulkHatsCount = 0;
var currentTotalCost = 0;
var currentShippingTotal = 0;
var currentGrandTotalCost = 0;

function updateBulkTotals() {
    var productList = '[';
    var totalHats = 0;
    $('#FlexFit6277 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            var currentSizeIndex = 0;
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //if (parseInt($(this).val()) % 12 == 0) {
                    //    //do nothing its okay
                    //} else {
                    //    displayPopupNotification('FlexFit/Yupoong hats must be ordered in quantities of 12.', 'error', false);
                    //    var val = parseInt($(this).val()) + (12 - (parseInt($(this).val()) % 12));
                    //    $(this).val(val);
                    //}  
                    totalHats += parseInt($(this).val());
                    var hatName = '';
                    if (currentSizeIndex == 0) {
                        hatName = 'FlexFit 6277 - ' + hatColorText + ' - S/M';
                    }
                    if (currentSizeIndex == 1) {
                        hatName = 'FlexFit 6277 - ' + hatColorText + ' - L/XL';
                    }
                    if (currentSizeIndex == 2) {
                        hatName = 'FlexFit 6277 - ' + hatColorText + ' - XL/XXL';
                    }
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + 15.00 + ',"currency":"USD"},';
                }
                currentSizeIndex++;
            });
        });
    });
    $('#FlexFitTrucker table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //if (parseInt($(this).val()) % 12 == 0) {
                    //    //do nothing its okay
                    //} else {
                    //    displayPopupNotification('FlexFit/Yupoong hats must be ordered in quantities of 12.', 'error', false);
                    //    var val = parseInt($(this).val()) + (12 - (parseInt($(this).val()) % 12));
                    //    $(this).val(val);
                    //}
                    totalHats += parseInt($(this).val());
                    hatName = 'FlexFit Trucker  - ' + hatColorText + ' - OSFA';
                   
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + 15.00 + ',"currency":"USD"},';
                }
            });
        });
    });
    $('#FlexFit210 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            var currentSizeIndex = 0;
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //if (parseInt($(this).val()) % 12 == 0) {
                    //    //do nothing its okay
                    //} else {
                    //    displayPopupNotification('FlexFit/Yupoong hats must be ordered in quantities of 12.', 'error', false);
                    //    var val = parseInt($(this).val()) + (12 - (parseInt($(this).val()) % 12));
                    //    $(this).val(val);
                    //}
                    totalHats += parseInt($(this).val());
                    var hatName = '';
                    if (currentSizeIndex == 0) {
                        hatName = 'FlexFit Premium 210  - ' + hatColorText + ' - S/M';
                    }
                    if (currentSizeIndex == 1) {
                        hatName = 'FlexFit Premium 210  - ' + hatColorText + ' - L/XL';
                    }
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + 15.00 + ',"currency":"USD"},';
                }
                currentSizeIndex++;
            });
        });
    });
    $('#FlexFit110 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalHats += parseInt($(this).val());
                    var hatName = 'FlexFit 110 Trucker Snapback  - ' + hatColorText + ' - OSFA';
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + 15.00 + ',"currency":"USD"},';
                }
            });
        });
    });
    $('#6089FlatbillSnapback table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalHats += parseInt($(this).val());
                    var hatName = 'Yupoong Flat Bill Snapback  - ' + hatColorText + ' - OSFA';
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + 15.00 + ',"currency":"USD"},';
                }
            });
        });
    });
    $('#Richardson112 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalHats += parseInt($(this).val());
                    var hatName = 'Richardson 112  - ' + hatColorText + ' - OSFA';                    
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + 15.00 + ',"currency":"USD"},';
                }
            });
        });
    });
    var shippingPrice = 15;    

    currentTotalBulkHatsCount = totalHats;    

    var currentPrice = 15;
    currentShippingTotal = 5;

    if (currentTotalBulkHatsCount >= 120) {
        currentPrice = 10;
        currentShippingTotal = 50;
    }
    else if (currentTotalBulkHatsCount >= 96) {
        currentPrice = 11;
        currentShippingTotal = 35;
    }
    else if (currentTotalBulkHatsCount >= 72) {
        currentPrice = 12;
        currentShippingTotal = 25;
    }
    else if (currentTotalBulkHatsCount >= 48) {
        currentPrice = 13;
        currentShippingTotal = 20;
    }
    else if (currentTotalBulkHatsCount >= 24) {
        currentShippingTotal = 15;
        currentPrice = 14;
    }
    else if (currentTotalBulkHatsCount >= 12) {
        currentShippingTotal = 10;
    }
    else if (currentTotalBulkHatsCount >= 6) {
        currentShippingTotal = 5;
    }
    
    currentTotalCost = currentTotalBulkHatsCount * currentPrice;

    var hasArtFee = false; 

    if (currentTotalBulkHatsCount < 12) {
        currentTotalCost += 30;
        hasArtFee = true;
        productList = productList + '{"name":"Artwork Setup/Digitizing","quantity":"1","price":"30","currency":"USD"},';
        $('#artworkSetupFee').show();
    } else {
        $('#artworkSetupFee').hide();
    }

    productList = productList + '{"name":"Shipping","quantity":"1","price":"' + currentShippingTotal + '","currency":"USD"}]';


    currentBulkProductList = JSON.parse(productList);

    var length = currentBulkProductList.length - 1;

    if (hasArtFee) {
        length = currentBulkProductList.length - 2;
    }

    for (var i = 0; i < length; i++) {

        currentBulkProductList[i].price = currentPrice
    }

    $('#bottomTotal').text('$' + currentTotalCost);
    $('#totalHatCount').text(currentTotalBulkHatsCount);
    $('#totalHatCost').text('$' + currentTotalBulkHatsCount * currentPrice + ' @ ' + currentPrice + '/each');

    currentGrandTotalCost = currentTotalCost + currentShippingTotal;

    console.log(currentTotalCost);

    console.log(currentBulkProductList);


}

function showBulkCart() {
    $(window).scrollTop(0);
    $('#bulkCartSubTotal').text('$' + currentTotalCost);
    $('#bulkCartShipping').text('$' + currentShippingTotal);
    $('#bulkCartTotal').text('$' + currentGrandTotalCost);
    $('#cartListItems').empty();
    for (var i = 0; i < currentBulkProductList.length - 1; i++) {
        $('#cartListItems').append('<div class="bulkItemCartListItem"><div class="bulkItemCartListItemDetials">' + currentBulkProductList[i].name + '</div><div class="bulkItemCartListItemQuantity">' + currentBulkProductList[i].quantity + '</div><div class="bulkItemCartListItemPrice">' + currentBulkProductList[i].price + '</div></div>');
    }
    $('.bulkCartPopup').show();
    $('#paypal-button-container-bulk').empty();
    renderBulkCartPaypalButtons(currentGrandTotalCost, currentBulkProductList, $('#paymentCompleteGuid').text());
}


function renderBulkCartPaypalButtons(price, items, paymentCompleteGuid) {
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
                            "amount": { "total": price, "currency": "USD" },
                            "description": "Lid Launch Order",
                            "item_list": {
                                "items": items
                            }
                        }
                    ]
                }
            });

        },

        // Wait for the payment to be authorized by the customer

        onAuthorize: function (data, actions) {
            return actions.payment.execute().then(function () {
                window.location = 'http://lidlaunch.com/bulk/payment?id=' + paymentCompleteGuid;
            });
        }

    }, '#paypal-button-container-bulk');
}
function verifyAndShowPaypal() {

    console.log(JSON.stringify(currentBulkProductList));
    if ($('#txtFirstName').val() != '' || $('#txtEmail').val() != '' || $('#txtPhone').val() != '') {

        var that = this;
        var files = $('#bulkArtwork')[0].files;
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                data.append("file" + 0, files[0]);
                data.append("name", $('#txtFirstName').val());
                data.append("email", $('#txtEmail').val());
                data.append("phone", $('#txtPhone').val());
                data.append("artworkPlacement", $('#artPlacement').text());
                data.append("orderNotes", $('#txtDetails').val());
                data.append("orderTotal", currentGrandTotalCost);
                data.append("items", JSON.stringify(currentBulkProductList));
                data.append("paymentCompleteGuid", $('#paymentCompleteGuid').text());
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Bulk/CreateBulkOrder',
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('error.', 'error', false);
                        } else {
                            //set the url for the file link and show the link 
                            hideLoading();
                            $('#paypalPaymentButtonsPopup').show();
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Please upload your artwork on the main screen.', 'error', false);
        }
        
    } else {
        displayPopupNotification('Please enter all contact information.', 'error', false);
    }
}

function processBulkPaymentShowPaypal(total, paymentCompleteGuid) {
    var items = JSON.parse($('#productList').text());    

    renderBulkCartPaypalButtons(total, items, paymentCompleteGuid);
    $('#paypalPaymentButtonsPopup').show();
}

function saveNote(noteType, idVal, parentBulkOrderId) {
    $.ajax({
        type: "POST",
        url: '/Dashboard/CreateNote',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            noteType: noteType, idVal: idVal, parentBulkOrderId: parentBulkOrderId, text: $('#noteText').val(), attachment: ''}),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window              
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}


function approveDigitizing(bulkOrderId) {
    $.ajax({
        type: "POST",
        url: '/Bulk/ApproveDigitizing',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            id: bulkOrderId
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                window.location.reload();
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}


function createBulkOrderBatch() {
    $.ajax({
        type: "POST",
        url: '/Bulk/CreateBulkOrderBatch',
        contentType: false,
        processData: false,
        data: '',
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                window.location.reload();
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}