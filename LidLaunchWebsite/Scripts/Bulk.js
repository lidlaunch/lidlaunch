var currentBulkProductList = [];
var currentTotalCost = 0;
var currentShippingTotal = 0;
var currentGrandTotalCost = 0;
var currentOrderStep = 'intro';
var totalHats = 0;

function updateBulkTotals() {
    var productList = '[';
    totalHats = 0;
    currentTotalCost = 0;

    var currentFlexFit6277BasePrice = 15;
    var currentFlexFit6511BasePrice = 15;
    var currentFlexFit110BasePrice = 15;
    var currentFlexFitFlatBillBasePrice = 15;
    var currentYupoong6089BasePrice = 15;
    var currentYupoongDadCapBasePrice = 15;
    var currentYupoong6606BasePrice = 15;
    var currentYupoong6006BasePrice = 15;
    var currentShortBeanieBasePrice = 15;
    var currentCuffedBeanieBasePrice = 15;
    var currentRichardson112BasePrice = 15;
    
    currentShippingTotal = 5;

    var totalFlexFit6277Hats = 0;
    var totalFlexFit6511Hats = 0;
    var totalFlexFit110Hats = 0;
    var totalFlexFitFlatBillHats = 0;
    var totalYupoong6089Hats = 0;
    var totalYupoongDadCapHats = 0;
    var totalYupoong6606Hats = 0;
    var totalYupoong6006Hats = 0;
    var totalShortBeanies = 0;
    var totalCuffedBeanies = 0;
    var totalRichardson112Hats = 0;

    $('.hatStyleSetQty').find('.colorQty').each(function () {
        if (parseInt($(this).val()) > 0) {
            totalHats += parseInt($(this).val());
        }
    });
    //shipping price section
    if (totalHats >= 1000) {
        currentShippingTotal = 250;
    }
    else if (totalHats >= 750) {
        currentShippingTotal = 200;
    }
    else if (totalHats >= 500) {
        currentShippingTotal = 150;
    }
    else if (totalHats >= 300) {
        currentShippingTotal = 100;
    }
    else if (totalHats >= 120) {
        currentShippingTotal = 50;
    }
    else if (totalHats >= 96) {
        currentShippingTotal = 45;
    }
    else if (totalHats >= 72) {
        currentShippingTotal = 35;
    }
    else if (totalHats >= 48) {
        currentShippingTotal = 25;
    }
    else if (totalHats >= 24) {
        currentShippingTotal = 20;        
    }
    else if (totalHats >= 12) {
        currentShippingTotal = 15;
    }
    else if (totalHats >= 6) {
        currentShippingTotal = 10;
    }

    if (totalHats <= 5) {
        $('#chkBackStitching').prop("checked", false);
        $('#txtBackStitching').val('');
        $('#artworkStep4Header').text('Back Stitching - Available for orders 6 or more');
        $('#artworkStep4').hide();
    } else {
        $('#artworkStep4Header').text('Back Stitching');
        $('#artworkStep4').show();
    }

    


    $('#FlexFit6277 table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalFlexFit6277Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalFlexFit6277Hats >= 432) {
        currentFlexFit6277BasePrice = 10;
    }
    else if (totalFlexFit6277Hats >= 288) {
        currentFlexFit6277BasePrice = 11;
    }
    else if (totalFlexFit6277Hats >= 144) {
        currentFlexFit6277BasePrice = 12;
    }
    else if (totalFlexFit6277Hats >= 96) {
        currentFlexFit6277BasePrice = 13;
    }
    else if (totalFlexFit6277Hats >= 48) {
        currentFlexFit6277BasePrice = 14;
    }

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
                    //totalHats += parseInt($(this).val());
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

                    var hatPrice = currentFlexFit6277BasePrice;
                    if (hatName.includes('MULTICAM')) {
                        hatPrice = currentFlexFit6277BasePrice + 2;
                    }

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
                currentSizeIndex++;
            });
        });
    });

    $('#FlexFitTrucker table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalFlexFit6511Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalFlexFit6511Hats >= 432) {
        currentFlexFit6511BasePrice = 10;
    }
    else if (totalFlexFit6511Hats >= 288) {
        currentFlexFit6511BasePrice = 11;
    }
    else if (totalFlexFit6511Hats >= 144) {
        currentFlexFit6511BasePrice = 12;
    }
    else if (totalFlexFit6511Hats >= 96) {
        currentFlexFit6511BasePrice = 13;
    }
    else if (totalFlexFit6511Hats >= 48) {
        currentFlexFit6511BasePrice = 14;
    }

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
                    //totalHats += parseInt($(this).val());
                    hatName = 'FlexFit Trucker  - ' + hatColorText + ' - OSFA';
                    var hatPrice = currentFlexFit6511BasePrice;

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#FlexFit210 table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalFlexFitFlatBillHats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalFlexFitFlatBillHats >= 432) {
        currentFlexFitFlatBillBasePrice = 10;
    }
    else if (totalFlexFitFlatBillHats >= 288) {
        currentFlexFitFlatBillBasePrice = 11;
    }
    else if (totalFlexFitFlatBillHats >= 144) {
        currentFlexFitFlatBillBasePrice = 12;
    }
    else if (totalFlexFitFlatBillHats >= 96) {
        currentFlexFitFlatBillBasePrice = 13;
    }
    else if (totalFlexFitFlatBillHats >= 48) {
        currentFlexFitFlatBillBasePrice = 14;
    }

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
                    //totalHats += parseInt($(this).val());
                    var hatName = '';
                    if (currentSizeIndex == 0) {
                        hatName = 'FlexFit Flat Bill Fitted  - ' + hatColorText + ' - S/M';
                    }
                    if (currentSizeIndex == 1) {
                        hatName = 'FlexFit Flat Bill Fitted  - ' + hatColorText + ' - L/XL';
                    }
                    var hatPrice = currentFlexFitFlatBillBasePrice;

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
                currentSizeIndex++;
            });
        });
    });

    $('#FlexFit110 table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalFlexFit110Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalFlexFit110Hats >= 432) {
        currentFlexFit110BasePrice = 10;
    }
    else if (totalFlexFit110Hats >= 288) {
        currentFlexFit110BasePrice = 11;
    }
    else if (totalFlexFit110Hats >= 144) {
        currentFlexFit110BasePrice = 12;
    }
    else if (totalFlexFit110Hats >= 96) {
        currentFlexFit110BasePrice = 13;
    }
    else if (totalFlexFit110Hats >= 48) {
        currentFlexFit110BasePrice = 14;
    }

    $('#FlexFit110 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatName = 'FlexFit 110 Trucker Snapback  - ' + hatColorText + ' - OSFA';

                    var hatPrice = currentFlexFit110BasePrice;

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#6089FlatbillSnapback table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalYupoong6089Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalYupoong6089Hats >= 288) {
        currentYupoong6089BasePrice = 10;
    }
    else if (totalYupoong6089Hats >= 144) {
        currentYupoong6089BasePrice = 11;
    }
    else if (totalYupoong6089Hats >= 96) {
        currentYupoong6089BasePrice = 12;
    }
    else if (totalYupoong6089Hats >= 48) {
        currentYupoong6089BasePrice = 13;
    }
    else if (totalYupoong6089Hats >= 24) {
        currentYupoong6089BasePrice = 14;
    }

    $('#6089FlatbillSnapback table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatName = 'Yupoong Flat Bill Snapback  - ' + hatColorText + ' - OSFA';                                   

                    var hatPrice = currentYupoong6089BasePrice;     

                    if (hatName.includes('MULTICAM')) {
                        hatPrice = currentYupoong6089BasePrice + 2;
                    }

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#DadCap table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalYupoongDadCapHats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalYupoongDadCapHats >= 288) {
        currentYupoongDadCapBasePrice = 10;
    }
    else if (totalYupoongDadCapHats >= 144) {
        currentYupoongDadCapBasePrice = 11;
    }
    else if (totalYupoongDadCapHats >= 96) {
        currentYupoongDadCapBasePrice = 12;
    }
    else if (totalYupoongDadCapHats >= 48) {
        currentYupoongDadCapBasePrice = 13;
    }
    else if (totalYupoongDadCapHats >= 24) {
        currentYupoongDadCapBasePrice = 14;
    }

    $('#DadCap table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatPrice = currentYupoongDadCapBasePrice;
                    var hatName = 'Yupoong Dad Cap  - ' + hatColorText + ' - OSFA';
                    if (hatName.includes('MULTICAM')) {
                        hatPrice = currentYupoongDadCapBasePrice + 2;
                    }
                    
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#6606TruckerSnapback table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalYupoong6606Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalYupoong6606Hats >= 288) {
        currentYupoong6606BasePrice = 10;
    }
    else if (totalYupoong6606Hats >= 144) {
        currentYupoong6606BasePrice = 11;
    }
    else if (totalYupoong6606Hats >= 96) {
        currentYupoong6606BasePrice = 12;
    }
    else if (totalYupoong6606Hats >= 48) {
        currentYupoong6606BasePrice = 13;
    }
    else if (totalYupoong6606Hats >= 24) {
        currentYupoong6606BasePrice = 14;
    }

    $('#6606TruckerSnapback table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatPrice = currentYupoong6606BasePrice;
                    var hatName = 'Yupoong 6606 Trucker Snapback  - ' + hatColorText + ' - OSFA';
                    if (hatName.includes('MULTICAM')) {
                        hatPrice = currentYupoong6606BasePrice + 2;
                    }
                    
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#6006 table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalYupoong6006Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalYupoong6006Hats >= 288) {
        currentYupoong6006BasePrice = 10;
    }
    else if (totalYupoong6006Hats >= 144) {
        currentYupoong6006BasePrice = 11;
    }
    else if (totalYupoong6006Hats >= 96) {
        currentYupoong6006BasePrice = 12;
    }
    else if (totalYupoong6006Hats >= 48) {
        currentYupoong6006BasePrice = 13;
    }
    else if (totalYupoong6006Hats >= 24) {
        currentYupoong6006BasePrice = 14;
    }

    $('#6006 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatPrice = currentYupoong6006BasePrice;
                    var hatName = 'Yupoong 6006 Flat Bill Trucker Snapback  - ' + hatColorText + ' - OSFA';
                    if (hatName.includes('MULTICAM')) {
                        hatPrice = currentYupoong6006BasePrice + 2;
                    }
                    
                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#ShortBeanies table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalShortBeanies += parseInt($(this).val());
                }
            });
        });
    });

    if (totalShortBeanies >= 144) {
        currentShortBeanieBasePrice = 10;
    }
    else if (totalShortBeanies >= 96) {
        currentShortBeanieBasePrice = 11;
    }
    else if (totalShortBeanies >= 48) {
        currentShortBeanieBasePrice = 12;
    }
    else if (totalShortBeanies >= 24) {
        currentShortBeanieBasePrice = 13;
    }
    else if (totalShortBeanies >= 12) {
        currentShortBeanieBasePrice = 14;
    }

    $('#ShortBeanies table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatName = 'Yupoong Short Beanie  - ' + hatColorText + ' - OSFA';

                    var hatPrice = currentShortBeanieBasePrice;

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#CuffedBeanies table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalCuffedBeanies += parseInt($(this).val());
                }
            });
        });
    });

    if (totalCuffedBeanies >= 144) {
        currentCuffedBeanieBasePrice = 10;
    }
    else if (totalCuffedBeanies >= 96) {
        currentCuffedBeanieBasePrice = 11;
    }
    else if (totalCuffedBeanies >= 48) {
        currentCuffedBeanieBasePrice = 12;
    }
    else if (totalCuffedBeanies >= 24) {
        currentCuffedBeanieBasePrice = 13;
    }
    else if (totalCuffedBeanies >= 12) {
        currentCuffedBeanieBasePrice = 14;
    }

    $('#CuffedBeanies table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatName = 'Yupoong Cuffed Beanie  - ' + hatColorText + ' - OSFA';

                    var hatPrice = currentCuffedBeanieBasePrice;

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });

    $('#Richardson112 table').each(function () {
        $(this).find('tr').each(function () {
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    totalRichardson112Hats += parseInt($(this).val());
                }
            });
        });
    });

    if (totalRichardson112Hats >= 288) {
        currentRichardson112BasePrice = 10;
    }
    else if (totalRichardson112Hats >= 144) {
        currentRichardson112BasePrice = 11;
    }
    else if (totalRichardson112Hats >= 96) {
        currentRichardson112BasePrice = 12;
    }
    else if (totalRichardson112Hats >= 48) {
        currentRichardson112BasePrice = 13;
    }
    else if (totalRichardson112Hats >= 24) {
        currentRichardson112BasePrice = 14;
    }

    $('#Richardson112 table').each(function () {
        $(this).find('tr').each(function () {
            var hatColorText = $(this).find('.colorOption').text();
            $(this).find('.colorQty').each(function () {
                if (parseInt($(this).val()) > 0) {
                    //totalHats += parseInt($(this).val());
                    var hatName = 'Richardson 112  - ' + hatColorText + ' - OSFA';

                    var hatPrice = currentRichardson112BasePrice;

                    productList = productList + '{"name":"' + hatName + '","quantity":"' + $(this).val() + '","price":' + hatPrice + ',"currency":"USD"},';

                    currentTotalCost += parseInt($(this).val()) * hatPrice;
                }
            });
        });
    });
    



    if ($('#chkBackStitching').prop("checked")) {
        currentTotalCost += (5 * totalHats);
        productList = productList + '{"name":"Back Stitching","quantity":' + totalHats + ',"price":"5","currency":"USD"},';
    }

    if ($('#chkLeftSideStitching').prop("checked")) {
        currentTotalCost += (5 * totalHats);
        productList = productList + '{"name":"Left Side Stitching","quantity":' + totalHats + ',"price":"5","currency":"USD"},';
    }

    if ($('#chkRightSideStitching').prop("checked")) {
        currentTotalCost += (5 * totalHats);
        productList = productList + '{"name":"Right Side Stitching","quantity":' + totalHats + ',"price":"5","currency":"USD"},';
    }

    if (totalHats < 12 && artworkPreExisting == false) {
        currentTotalCost += 30;
        hasArtFee = true;
        productList = productList + '{"name":"Artwork Setup/Digitizing","quantity":"1","price":"30","currency":"USD"}';
        $('#artworkSetupBottom').show();
    } else {
        $('#artworkSetupBottom').hide();
        productList = productList.slice(0, -1);
    }

    productList = productList + ']';
    //productList = productList + '{"name":"Shipping","quantity":"1","price":"' + currentShippingTotal + '","currency":"USD"}]';


    currentBulkProductList = JSON.parse(productList);


    $('#bottomTotal').text('$' + currentTotalCost);
    $('#totalHatCount').text(totalHats);
    //$('#totalHatCost').text('$' + totalHats * currentPrice + ' @ ' + currentPrice + '/each');

    currentGrandTotalCost = currentTotalCost + currentShippingTotal;

    $('#lblTotal').text(currentGrandTotalCost);
    $('#lblMobileTotal').text('$' + currentGrandTotalCost);

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
                            "description": "LidLaunch Order",
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
                setTimeout(function () {
                    window.location = 'http://lidlaunch.com/bulk/payment?id=' + paymentCompleteGuid;
                }, 1500);

            });
        }

    }, '#paypal-button-container-bulk');
}
function verifyAndShowPaypal() {

    var files = $('#bulkArtwork')[0].files;

    var orderNotes = $('#txtDetails').val();
    var shippingCost = $('#shippingCost').text();

    var backStitching = $('#chkBackStitching').prop("checked");
    var leftSideStitching = $('#chkLeftSideStitching').prop("checked");
    var rightSideStitching = $('#chkRightSideStitching').prop("checked");
    var backStitchingComment = $('#txtBackStitching').val();
    var rightSideStitchingComment = $('#txtRightSideStitching').val();
    var leftSideStitchingComment = $('#txtLeftSideStitching').val();

    if (artworkPreExisting) {
        orderNotes = 'ARTWORK PRE-EXISTING : ' + orderNotes;
    }

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
        data.append("billToState", billState);
        data.append("billToAddress", billAddress);
        data.append("billToZip", billZip);
        data.append("billToPhone", billPhone);
        data.append("billToCity", billCity);
        data.append("billToName", billFirstName + ' ' + billLastName);
        data.append("shipToState", billState);
        data.append("shipToAddress", shipAddress);
        data.append("shipToZip", shipZip);
        data.append("shipToPhone", shipPhone);
        data.append("shipToCity", shipCity);
        data.append("shipToName", shipFirstName + ' ' + shipLastName);
        data.append("backStitching", backStitching);
        data.append("leftSideStitching", leftSideStitching);
        data.append("rightSideStitching", rightSideStitching);
        data.append("backStitchingComment", backStitchingComment);
        data.append("leftSideStitchingComment", leftSideStitchingComment);
        data.append("rightSideStitchingComment", rightSideStitchingComment);
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
                    displayPopupNotification('Sorry there was an error creating your order.', 'error', false);
                } else {
                    //set the url for the file link and show the link 
                    hideLoading();
                    $('#chckoutWizzard').hide();
                    $('#paypalButtons').slideDown();
                    $('#paypal-button-container-bulk').empty();
                    $('#checkoutPopoup').find('.close').hide();
                    renderBulkCartPaypalButtons(currentGrandTotalCost, currentBulkProductList, $('#paymentCompleteGuid').text(), shippingCost, (currentGrandTotalCost - currentShippingTotal));
                }
            },
            error: function (xhr, status, p3, p4) {
                displayPopupNotification('Sorry there was an error creating your order.', 'error', false);
            }
        });
    } else {
        displayPopupNotification('Use Google Chrome or Firefox!', 'error', false);
    }

}

function processBulkPaymentShowPaypal(total, paymentCompleteGuid, shippingCost, orderSubTotal) {
    var items = JSON.parse($('#productList').text());

    renderBulkCartPaypalButtons((shippingCost + orderSubTotal), items, paymentCompleteGuid, shippingCost, orderSubTotal);
    $('#paypalPaymentButtonsPopup').show();
}

function saveBulkRework(bulkOrderBatchId, bulkOrderItemId, bulkOrderBlankName, parentBulkOrderId, parentBulkOrderBatchId, reworkId, shipping) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/CreateBulkRework',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderBatchId: bulkOrderBatchId, bulkOrderItemId: bulkOrderItemId, bulkOrderBlankName: bulkOrderBlankName, quantity: $('#bulkReworkQuantity').val(), note: $('#bulkReworkNote').val(), status: $('#selReworkStatus').children("option:selected").val(), reworkId: reworkId, bulkOrderId: parentBulkOrderId
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                if (bulkOrderBatchId != '' && bulkOrderBatchId != '0') {
                    alert('show the bulk order batch screen for batch id= ' + parentBulkOrderBatchId);
                } else {                    
                    if (shipping) {
                        showBulkOrderShip(parentBulkOrderId);
                    } else {
                        showBulkOrderDetailsPopup(parentBulkOrderId);
                    }
                    
                }
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}

function addLog(bulkOrderId) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/AddOrderLogEntry',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderId: bulkOrderId, logText: $('#logText').val()
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                showBulkOrderDetailsPopup(bulkOrderId);
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}

function saveNote(noteType, idVal, parentBulkOrderId, customerAdded, shipping) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/CreateNote',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            noteType: noteType, idVal: idVal, parentBulkOrderId: parentBulkOrderId, text: $('#noteText').val(), attachment: '', customerAdded: customerAdded
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                if (shipping) {
                    showBulkOrderShip(parentBulkOrderId);
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

function InternallyApproveBulkOrder(id, approve) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/InternallyApproveBulkOrder',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            id: id,
            approve: approve
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                showBulkOrderDetailsPopup(id);
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}


function internallyApproveDigitizing(id, bulkOrderId) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/InternallyApproveDigitizing',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            id: id,
            bulkOrderId: bulkOrderId
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

function approveDigitizing(id, bulkOrderId) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/ApproveDigitizing',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            id: id,
            bulkOrderId: bulkOrderId
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

function revisionCancelClick(that) {
    var parent = $(that).closest('.designApprovalGroup');
    $(parent).find('.approveDigitizing').show();
    $(parent).find('.revisionText').hide();
    $(parent).find('.revisionStartButton').show();
}

function startRevisionClick(that) {
    var parent = $(that).closest('.designApprovalGroup');
    $(parent).find('.approveDigitizing').hide();
    $(parent).find('.revisionText').show();
    $(parent).find('.revisionStartButton').hide();
}


function requestDigitizingRevision(id, revisionText, bulkOrderId, customerAdded) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/RequestDigitizingRevision',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            id: id,
            text: revisionText,
            bulkOrderId: bulkOrderId,
            customerAdded: customerAdded
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
    showLoading();
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

function toggleBackStitching() {
    if ($('#chkBackStitching').prop("checked")) {
        $('#chkBackStitching').prop("checked", false);
        //$('#bulkArtwork').show();
        updateBulkTotals();
    } else {
        $('#chkBackStitching').prop("checked", true);
        //$('#bulkArtwork').hide();
        updateBulkTotals();
    }
}

function toggleRightSideStitching() {
    if ($('#chkRightSideStitching').prop("checked")) {
        $('#chkRightSideStitching').prop("checked", false);
        //$('#bulkArtwork').show();
        updateBulkTotals();
    } else {
        $('#chkRightSideStitching').prop("checked", true);
        //$('#bulkArtwork').hide();
        updateBulkTotals();
    }
}

function toggleLeftSideStitching() {
    if ($('#chkLeftSideStitching').prop("checked")) {
        $('#chkLeftSideStitching').prop("checked", false);
        //$('#bulkArtwork').show();
        updateBulkTotals();
    } else {
        $('#chkLeftSideStitching').prop("checked", true);
        //$('#bulkArtwork').hide();
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
    if (totalHats > 0) {
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

    if (artworkPreExisting) {
        orderNotes = 'ARTWORK PRE-EXISTING : ' + orderNotes;
    }

    //if ((files.length > 0 || artworkPreExisting) && artPlacement != "") {
    if (artPlacement != "") {
        return true;
    } else {
        displayPopupNotification('Select a placement location by clicking on the placement you would like.', 'error', false);
        return false;
    }

}

function showBulkOrderDetailsPopup(bulkOrderId) {
    showLoading();
    $('#popUpHolder').load(url, { bulkOrderId: bulkOrderId });
}


function setBulkOrderAsShipped(bulkOrderId, shipping) {
    var noEmail = $('#chkNoEmail').prop("checked");
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/SetBulkOrderAsShipped',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderId: bulkOrderId,
            trackingNumber: $('#txtTrackingNumber').val(),
            noEmail: noEmail
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                if (shipping) {
                    showBulkOrderShip(bulkOrderId);
                } else {
                    showBulkOrderDetailsPopup(bulkOrderId);
                }                
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}

function searchBulkDesigns() {
    showLoading();
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
                hideLoading();
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
    showLoading();
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

function SaveAdminReview(bulkOrderId, fromBulkPopup) {
    var comment = $('#txtAdminReviewComment').val();
    var designerReview = $('#chkDesignerReview').prop("checked");
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/SaveAdminReview',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderId: bulkOrderId,
            comment: comment,
            designerReview: designerReview
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                if (fromBulkPopup) {
                    showBulkOrderDetailsPopup(bulkOrderId);
                } else {
                    window.location.reload();
                }
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}

function UpdateAdminReviewFinished(bulkOrderId, fromBulkPopup) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Bulk/UpdateAdminReviewFinished',
        contentType: false,
        processData: false,
        data: JSON.stringify({
            bulkOrderId: bulkOrderId
        }),
        contentType: "application/json",
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('error.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                //reload bulk order window   
                if (fromBulkPopup) {
                    showBulkOrderDetailsPopup(bulkOrderId);
                } 
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error.', 'error', false);
        }
    });
}