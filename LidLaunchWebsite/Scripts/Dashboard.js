$(document).ready(function () {

    bindDigitizingFilesInputs();
    bindHatPreviewInputs();
    bindBatchDropdown();
});
function bindBatchDropdown() {
    $('.batchSelect').on('change', function (e) {
        var bulkOrderId = $(this).closest('tr').find(".bulkOrderId").text();
        var batchId = $(this).children("option:selected").val();
        if (batchId === 0) {
            //do nothing
        } else {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/Dashboard/UpdateBulkOrderBatchId?bulkOrderId=' + bulkOrderId + '&batchId=' + batchId,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "") {
                        //do nothing
                        displayPopupNotification('Error setting batch Id, please try again.', 'error', false);
                    } else {
                        //set the url for the file link and show the link 
                        hideLoading();
                    }
                },
                error: function (xhr, status, p3, p4) {
                    displayPopupNotification('Error setting batch Id, please try again.', 'error', false);
                }
            });
        }        
            
    });
}

function sendArtworkEmail(bulkOrderId, customerEmail) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/SendArtworkEmail?bulkOrderId=' + bulkOrderId + '&customerEmail=' + customerEmail,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('Error Sending Artwork Email.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                hideLoading();
                showBulkOrderDetailsPopup(bulkOrderId);
                
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error Sending Artwork Email, please try again.', 'error', false);
        }
    });
}

function releaseToDigitizer(bulkOrderId) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/ReleaseToDigitizer?bulkOrderId=' + bulkOrderId,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('Error Releasing To Digitizer.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                hideLoading();
                showBulkOrderDetailsPopup(bulkOrderId);

            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Error Releasing To Digitizer.', 'error', false);
        }
    });
}



function bindDigitizingFilesInputs() {
    $('.EmbroideryFile').on('change', function (e) {
        var that = this;
        var designId = $(this).closest('tr').find(".designId").text();
        var bulkOrderId = $(this).closest('tr').find(".bulkOrderId").text();
        var files = e.target.files;
        //var myID = 3; //uncomment this to make sure the ajax URL works
        if (files.length > 0 & files.length < 2) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/UploadDigitizedFile?designId=' + designId + '&bulkOrderId=' + bulkOrderId,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                        } else {
                            //set the url for the file link and show the link   
                            hideLoading();
                            $(that).closest('tr').find(".EmbroideryFileName").text(result);
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be uploaded.', 'error', false);
        }
    });
    $('.TransparentPreview').on('change', function (e) {
        var that = this;
        var designId = $(this).closest('tr').find(".designId").text();
        var bulkOrderId = $(this).closest('tr').find(".bulkOrderId").text();
        var files = e.target.files;
        //var myID = 3; //uncomment this to make sure the ajax URL works
        if (files.length > 0 & files.length < 2) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/UpdateDesignDigitizedPreview?designId=' + designId + '&bulkOrderId=' + bulkOrderId,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                        } else {
                            //set the url for the file link and show the link 
                            hideLoading();
                            $(that).closest('tr').find(".TransparentPreviewName").text(result);
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be uploaded.', 'error', false);
        }
    });
    $('.InfoFile').on('change', function (e) {
        var that = this;
        var designId = $(this).closest('tr').find(".designId").text();
        var bulkOrderId = $(this).closest('tr').find(".bulkOrderId").text();
        var files = e.target.files;
        //var myID = 3; //uncomment this to make sure the ajax URL works
        if (files.length > 0 & files.length < 2) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/UpdateDesignDigitizedProductionSheet?designId=' + designId + '&bulkOrderId=' + bulkOrderId,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                        } else {
                            //set the url for the file link and show the link  
                            hideLoading();
                            $(that).closest('tr').find(".InfoFileName").text(result);
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be uploaded.', 'error', false);
        }
    });
    $('.EMBFile').on('change', function (e) {
        var that = this;
        var designId = $(this).closest('tr').find(".designId").text();
        var bulkOrderId = $(this).closest('tr').find(".bulkOrderId").text();
        var files = e.target.files;
        //var myID = 3; //uncomment this to make sure the ajax URL works
        if (files.length > 0 & files.length < 2) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/UpdateDesignEMBFile?designId=' + designId + '&bulkOrderId=' + bulkOrderId,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                        } else {
                            //set the url for the file link and show the link 
                            hideLoading();
                            $(that).closest('tr').find(".EMBFileName").text(result);
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading embroidery file please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be uploaded.', 'error', false);
        }
    });
    $('.hatTypePreviewUpload').on('change', function (e) {
        var that = this;
        var files = e.target.files;
        if (files.length > 0 & files.length < 2) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/UploadHatTypePreviewImage',
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading hat type preview image please try again.', 'error', false);
                        } else {
                            //set the url for the file link and show the link 
                            hideLoading();
                            var filename = result.replace(new RegExp('"', 'g'), "");
                            $(".hatTypePreview").val(filename);
                            $(".hatTypePreviewImg").attr('src', '/Images/HatAssets/' + filename + "?test");
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading hat type preview image please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be uploaded.', 'error', false);
        }
    });
}
function bindHatPreviewInputs() {
    $('.creationImageUpload').on('change', function (e) {
        var that = this;
        var files = e.target.files;
        if (files.length > 0 & files.length < 2) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/UploadHatCreationImage',
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading hat creation image please try again.', 'error', false);
                        } else {
                            //set the url for the file link and show the link 
                            hideLoading();
                            var filename = result.replace(new RegExp('"', 'g'), "");
                            $(that).closest('.hatTypeEditRow').find(".creationImage").val(filename);
                            $(that).closest('.hatTypeEditRow').find(".hatStyleImg").attr('src', '/Images/HatAssets/' + filename + "?test");
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading hat creation image please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be uploaded.', 'error', false);
        }
    });
    
}
function RequestPayout() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/RequestPayout',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                displayPopupNotification('Your payout request has been successfully submitted. Please allow up to 24-48 hours for us to process and send your payment.', 'error', false);                
            } else {
                displayPopupNotification('Payout request failed please try again.', 'error', false);
            }
        },
        error: function (err) {
            displayPopupNotification('Payout request failed please try again.', 'error', false);
        }
        //error: displayPopupNotification('User create failed please try again.', 'error', false)
    });
}
function CreateBatch() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Dashboard/CreateBatchOrder',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                window.location = "/Dashboard/BatchOrder?batchId=" + data;
            } else {
                displayPopupNotification('Create batch failed please try again.', 'error', false);
            }
        },
        error: function (err) {
            displayPopupNotification('Create batch failed please try again.', 'error', false);
        }
        //error: displayPopupNotification('User create failed please try again.', 'error', false)
    });
}
function saveTracking(that) {
    var orderProductId = $(that).closest('tr').find('.orderProductId').text();
    var trackingNumber = $(that).closest('tr').find('.txtTrackingNumber').val();
    var customerEmail = $(that).closest('tr').find('.customerEmail').text();
    if (trackingNumber == "" || orderProductId == "" || customerEmail == "") {
        displayPopupNotification('One of the fields didnt work!?!?', 'error', false);
    } else {
        $.ajax({
            type: "POST",
            url: '/Dashboard/UpdateTracking',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "orderProductId": orderProductId, "trackingNumber": trackingNumber, "customerEmail": customerEmail}),
            dataType: "json",
            success: function (data) {
                //it worked
            },
            error: function (err) {
                displayPopupNotification('Error updating tracking info.', 'error', false);
            }
            //error: displayPopupNotification('User create failed please try again.', 'error', false)
        });
    }
}

function updatePreviewImage(image, that) {
    $(that).closest('tr').find('.hatStyleImg').attr("src", image);
}

function addNewHatTypeColorRow() {    
    $('#hatTypeColors').append($('.hatTypeEditRowTemplate').html());
    bindHatPreviewInputs();
}

function saveHatType() {
    var hatTypeColors = [];
    var hatTypeId = 0;
    var hatTypeName = '';
    var hatTypePreview = '';

    $('#hatTypeColors .hatTypeEditRow').each(function () {
        var hatTypeColor = {
            'colorId': $(this).find('.colorId').text(),
            'color': $(this).find('.colorName').val(),
            'availableToCreate': $(this).find('.availableToCreate').is(':checked'),
            'colorCode': $(this).find('.colorCode').val(),
            'creationImage': $(this).find('.creationImage').val()
        };
        hatTypeColors.push(hatTypeColor);
    });
    hatTypeId = $('#hatTypeId').text();
    hatTypeName = $('#hatTypeName').val();
    hatTypePreview = $('.hatTypePreview').val();

    console.log(hatTypeColors);
    console.log(hatTypeId + ' ' + hatTypeName + ' ' + hatTypePreview);


    if ('' == 'this') {
        displayPopupNotification('One of the fields didnt work!?!?', 'error', false);
    } else {
        $.ajax({
            type: "POST",
            url: '/Dashboard/SaveHatType',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "hatTypeColors": hatTypeColors, "hatTypeId": hatTypeId, "hatTypeName": hatTypeName, "hatTypePreview": hatTypePreview
            }),
            dataType: "json",
            success: function (data) {
                //it worked
                window.location = "/Dashboard/HatManager";
            },
            error: function (err) {
                displayPopupNotification('Error creating/updating hat type.', 'error', false);
            }
            //error: displayPopupNotification('User create failed please try again.', 'error', false)
        });
    }

}


function updateBulkOrderPaid(bulkOrderId, orderPaid, that) {
    $.ajax({
        type: "POST",
        url: '/Dashboard/UpdateBulkOrderPaid',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            "bulkOrderId": bulkOrderId, "orderPaid": orderPaid
        }),
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function (err) {
            displayPopupNotification('Error updating order paid.', 'error', false);
        }
    });
}


function addBulkOrderItemEdit() {
    var clonedTemplate = $('#bulkOrderItemEditTemplate').clone();
    $('#bulkOrderItemsEdit tbody').append(clonedTemplate);    
}
function changePlacement(that) {
    $('.editPlacementSelected').each(function () {
        $(this).removeClass('placementSelected');
    });
    $(that).addClass('placementSelected');
}
function saveBulkOrderEdit(bulkOrderId) {
    showLoading();
    var customerEmail = $('#txtEditCustomerEmail').val();
    var artworkPosition = $('.placementSelected').attr('id');
    var artSource = $('#editArtworkUpload')[0].files;
    var orderTotal = $('#txtOrderTotal').val();
    var bulkOrderItems = '[';
    var itemLength = $('#bulkOrderItemsEdit').find('.bulkOrderItemRow').length;
    for (var i = 0; i < itemLength; i++) {
        var tableRow = $('#bulkOrderItemsEdit').find('.bulkOrderItemRow')[i];
        var id = $(tableRow).find('.bulkOrderItemId').text();
        var name = $(tableRow).find('.txtBulkOrderItemName').val();
        var qty = $(tableRow).find('.txtBulkOrderItemQuantity').val();
        var cost = $(tableRow).find('.txtBulkOrderItemCost').val();
        if (i == itemLength - 1) {
            bulkOrderItems += '{"Id":"' + id + '","ItemName":"' + name + '","ItemQuantity":"' + qty + '","ItemCost":"' + cost + '"}'
        } else {
            bulkOrderItems += '{"Id":"' + id + '","ItemName":"' + name + '","ItemQuantity":"' + qty + '","ItemCost":"' + cost + '"},'
        }        
    }
    bulkOrderItems += ']';
    console.log(bulkOrderItems);

    var data = new FormData();
    data.append("file" + 0, artSource[0]);
    data.append("items", bulkOrderItems);
    data.append("customerEmail", customerEmail);
    data.append("artworkPosition", artworkPosition);
    data.append("bulkOrderId", bulkOrderId);
    data.append("orderTotal", orderTotal);


    $.ajax({
        type: "POST",
        url: '/Dashboard/UpdateBulkOrder',
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            if (result == "") {
                //do nothing
                displayPopupNotification('Sorry there was an error updating the bulk order.', 'error', false);
            } else {
                //set the url for the file link and show the link 
                hideLoading();
                showBulkOrderDetailsPopup(bulkOrderId);
            }
        },
        error: function (xhr, status, p3, p4) {
            displayPopupNotification('Sorry there was an error creating your order.', 'error', false);
        }
    });
}


