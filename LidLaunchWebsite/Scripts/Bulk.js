var currentBulkProductList = [];
var currentTotalBulkHatsCount = 0;
var currentTotalCost = 0;
var currentShippingTotal = 0;
var currentGrandTotalCost = 0;
var currentOrderStep = 'intro';
var totalHats = 0;

function updateBulkTotals() {
    var productList = '[';
    totalHats = 0;
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

    if (currentTotalBulkHatsCount < 12 && $('#artworkPresetup').prop("checked") == false) {
        currentTotalCost += 30;
        hasArtFee = true;
        productList = productList + '{"name":"Artwork Setup/Digitizing","quantity":"1","price":"30","currency":"USD"}';
        $('#artworkSetupFee').show();
    } else {
        $('#artworkSetupFee').hide();
        productList = productList.slice(0, -1);
    }
    
    productList = productList + ']';
    //productList = productList + '{"name":"Shipping","quantity":"1","price":"' + currentShippingTotal + '","currency":"USD"}]';


    currentBulkProductList = JSON.parse(productList);

    var length = currentBulkProductList.length;

    if (hasArtFee) {
        length = currentBulkProductList.length - 1;
    }

    for (var i = 0; i < length; i++) {

        currentBulkProductList[i].price = currentPrice;
    }

    $('#bottomTotal').text('$' + currentTotalCost);
    $('#totalHatCount').text(currentTotalBulkHatsCount);
    $('#totalHatCost').text('$' + currentTotalBulkHatsCount * currentPrice + ' @ ' + currentPrice + '/each');

    currentGrandTotalCost = currentTotalCost + currentShippingTotal;

    $('#lblTotal').text(currentGrandTotalCost);
    $('#lblMobileTotal').text('$' + currentGrandTotalCost);

    console.log(currentGrandTotalCost);

}

function showBulkCart() {
    $(window).scrollTop(0);
    $('#bulkCartSubTotal').text('$' + currentTotalCost);
    $('#shippingCost').text(currentShippingTotal);
    $('#bulkCartTotal').text('$' + currentGrandTotalCost);
    $('#cartItems tbody').empty();
    for (var i = 0; i < currentBulkProductList.length; i++) {
        $('#cartItems tbody').append('<tr><td><span>' + currentBulkProductList[i].quantity + '</span> x <span>' + currentBulkProductList[i].name + '</span></td><td><span>' + currentBulkProductList[i].price + '</span></td></tr>');
    }
    $('.bulkCartPopup').show();    
}


function renderBulkCartPaypalButtons(price, items, paymentCompleteGuid, shippingCost, subtotal) {
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
                fbq('track', 'Purchase', {
                    content_name: 'Bulk Hat Order',
                    content_category: 'Bulk Hat Order',
                    content_ids: '0',
                    content_type: 'product',
                    value: price,
                    currency: 'USD'
                });
                window.location = 'http://lidlaunch.com/bulk/payment?id=' + paymentCompleteGuid;
            });
        }

    }, '#paypal-button-container-bulk');
}
function verifyAndShowPaypal() {    

    var files = $('#bulkArtwork')[0].files;

    var orderNotes = $('#txtDetails').val();
    var shippingCost = $('#shippingCost').text();

    if ($('#artworkPresetup').prop("checked")) {
        orderNotes = 'ARTWORK PRE-EXISTING : ' + orderNotes;
    }

    if (window.FormData !== undefined) {
        var data = new FormData();
        data.append("file" + 0, files[0]);
        data.append("name", $('#txtShippingFirstName').val() + ' ' + $('#txtShippingLastName').val());
        data.append("email", $('#txtCustomerEmail').val());
        data.append("phone", $('#txtPhone').val());
        data.append("artworkPlacement", $('#artPlacement').text());
        data.append("orderNotes", orderNotes);
        data.append("orderTotal", currentGrandTotalCost);
        data.append("items", JSON.stringify(currentBulkProductList));
        data.append("shippingCost", shippingCost);
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
                    $('#chckoutWizzard').hide();
                    $('#paypalButtons').slideDown();
                    $('#paypal-button-container-bulk').empty();
                    renderBulkCartPaypalButtons(currentGrandTotalCost, currentBulkProductList, $('#paymentCompleteGuid').text(), shippingCost, (currentGrandTotalCost - currentShippingTotal));
                }
            },
            error: function (xhr, status, p3, p4) {
                displayPopupNotification('Error.', 'error', false);
            }
        });
    } else {
        displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
    }
        
}

function processBulkPaymentShowPaypal(total, paymentCompleteGuid, shippingCost, orderSubTotal) {
    var items = JSON.parse($('#productList').text());    

    renderBulkCartPaypalButtons((shippingCost + orderSubTotal), items, paymentCompleteGuid, shippingCost, orderSubTotal);
    $('#paypalPaymentButtonsPopup').show();
}

function saveBulkRework(bulkOrderBatchId, bulkOrderItemId, bulkOrderBlankName, parentBulkOrderId, parentBulkOrderBatchId, reworkId) {
    $.ajax({
        type: "POST",
        url: '/Dashboard/CreateBulkRework',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderBatchId: bulkOrderBatchId, bulkOrderItemId: bulkOrderItemId, bulkOrderBlankName: bulkOrderBlankName, quantity: $('#bulkReworkQuantity').val(), note: $('#bulkReworkNote').val(), status: $('#selReworkStatus').children("option:selected").val(), reworkId: reworkId}),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                if (bulkOrderBatchId != '' && bulkOrderBatchId != '0') {
                    alert('show the bulk order batch screen for batch id= ' + parentBulkOrderBatchId);
                } else {
                    showBulkOrderDetailsPopup(parentBulkOrderId);
                }                
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
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
                showBulkOrderDetailsPopup(parentBulkOrderId);
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

function togglePresetArtwork() {
    if ($('#artworkPresetup').prop("checked")) {
        $('#artworkPresetup').prop("checked", false);
        $('#bulkArtwork').show();
        updateBulkTotals();
    } else {
        $('#artworkPresetup').prop("checked", true);
        $('#bulkArtwork').hide();
        updateBulkTotals();
    }
}


function changeBulkOrderSection(section) {
    if (section == 'intro') {
        $('#bulkIntro').show();
        $('#bulkHatTypeSelect').hide();        
        $('#bulkArtworkStep').hide();

        $('#introStepButton').removeClass('selected');
        $('#hatsStepButton').removeClass('selected');
        $('#artStepButton').removeClass('selected');

        $('.bulkProgressBarContinuteButton').text('Start');

        $('#introStepButton').addClass('selected');
        currentOrderStep = 'intro';
        document.getElementById("header").scrollIntoView();
    }
    if (section == 'hats') {
        $('#bulkIntro').hide();
        $('#bulkHatTypeSelect').show();
        $('#bulkArtworkStep').hide();

        $('#introStepButton').removeClass('selected');
        $('#hatsStepButton').removeClass('selected');
        $('#artStepButton').removeClass('selected');

        $('.bulkProgressBarContinuteButton').text('Next Step');

        $('#hatsStepButton').addClass('selected');
        currentOrderStep = 'hats';
        document.getElementById("header").scrollIntoView();

        fbq('track', 'ViewContent', {
            content_name: 'Bulk Hat Order',
            content_category: 'Bulk Hat Order',
            content_ids: '0',
            content_type: 'product',
            value: '15',
            currency: 'USD'
        });
    }
    if (section == 'art') {
        if (validateHats()) {
            $('#bulkIntro').hide();
            $('#bulkHatTypeSelect').hide();
            $('#bulkArtworkStep').show();

            $('#introStepButton').removeClass('selected');
            $('#hatsStepButton').removeClass('selected');
            $('#artStepButton').removeClass('selected');

            $('.bulkProgressBarContinuteButton').text('Checkout');

            $('#artStepButton').addClass('selected');
            currentOrderStep = 'art';
            document.getElementById("header").scrollIntoView();

            var currentTotal = $('#lblTotal').text();

            fbq('track', 'AddToCart', {
                content_name: 'Bulk Order Hats',
                content_category: 'Bulk Order Hats',
                content_ids: '0',
                content_type: 'product',
                value: currentTotal,
                currency: 'USD'
            });
        }
    }
    if (section == 'checkout') {
        if (validateHats()) {
            if (validateArt()) {
                fbq('track', 'InitiateCheckout', {                    
                    content_category: 'Bulk Order Hats',
                    content_ids: ['0'],
                    content_type: 'product',
                    num_items: totalHats,
                    value: currentGrandTotalCost,
                    currency: 'USD'
                });
                showBulkCart();
            }     
        }
           
    }
}
function goToNextStep(currentStep) {
    if (currentStep == 'intro') {
        changeBulkOrderSection('hats');
    }
    if (currentStep == 'hats') {        
        changeBulkOrderSection('art');
    }
    if (currentStep == 'art') {        
        changeBulkOrderSection('checkout');
    }
}
function validateHats() {
    if (currentBulkProductList.length > 0) {
        return true;
    } else {
        displayPopupNotification('You need to select some hats from the expandable regions before continuing to the next step.', 'error', false);
        return false;
    }
}
function validateArt() {
    var files = $('#bulkArtwork')[0].files;

    var orderNotes = $('#txtDetails').val();
    var artPlacement = $('#artPlacement').text();

    if ($('#artworkPresetup').prop("checked")) {
        orderNotes = 'ARTWORK PRE-EXISTING : ' + orderNotes;
    }

    if ((files.length > 0 || $('#artworkPresetup').prop("checked")) && artPlacement != "") {
        return true;
    } else {
        displayPopupNotification('Please upload your artwork if it is not already setup and select a placement location.', 'error', false);
        return false;
    }

}

function showBulkOrderDetailsPopup(bulkOrderId) {
    $('#popUpHolder').load(url, { bulkOrderId: bulkOrderId});
}


function setBulkOrderAsShipped(bulkOrderId) {
    $.ajax({
        type: "POST",
        url: '/Bulk/SetBulkOrderAsShipped',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderId: bulkOrderId,
            trackingNumber: $('#txtTrackingNumber').val()
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                showBulkOrderDetailsPopup(bulkOrderId);
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}

function searchBulkDesigns() {
    $.ajax({
        type: "POST",
        url: '/Bulk/SearchBulkDesigns',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            email: $('#txtBulkDesignEmail').val()
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //loop through the results and show them all to chose from
                //showBulkOrderDetailsPopup(bulkOrderId);
                var designs = JSON.parse(result);
                for (i = 0; i < designs.length; i++) {
                    $('#designImageHolder').append('<tr><td><img src="https://lidlaunch.com/Images/DesignImages/Digitizing/Preview/' + designs[i].DigitizedPreview + '" widht="200" height="200" onclick="setBulkDesign(' + designs[i].Id + ')"/></td></tr>');
                }
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}

function setBulkDesign(designId) {
    var bulkOrderId = $('#BulkOrderId').text();
    $.ajax({
        type: "POST",
        url: '/Dashboard/SetBulkOrderDesign',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderId: bulkOrderId,
            designId: designId
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                showBulkOrderDetailsPopup(bulkOrderId);
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}