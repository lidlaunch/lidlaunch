$(document).ready(function () {

    $('#fileClick').on('change', function (e) {
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
                    url: '/Create/UploadArtwork',
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result == "") {
                            //do nothing
                            displayPopupNotification('Error uploading artwork please try again.', 'error', false);
                        } else {
                            if (result == '"PNG"') {
                                displayPopupNotification('Your artwork MUST be in PNG format.', 'error', false);
                            }
                            else if (result == '"SIZE"') {
                                displayPopupNotification('Your artwork is too small. Download our template for a good starting point.', 'error', false);
                            }
                            else if (result == '"COLORS"') {
                                displayPopupNotification('Your artwork contains more than 5 colors. Remember this is for embroidery. Designs need to be simple with no gradients.', 'error', false);
                            }
                            else {
                                location.reload();
                            }                                 
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        displayPopupNotification('Error uploading artwork please try again.', 'error', false);
                    }
                });
            } else {
                displayPopupNotification('Use Google Chrome browser or Firefox!', 'error', false);
            }
        } else {
            displayPopupNotification('Only one file can be used per design.', 'error', false);
        }
    });
    $('#previewDesign').load(function () {
        $("#dragHelper").draggable({ containment: "#artworkAreaContainer" });
        $('#previewDesign').resizable({ aspectRatio: true });
    });

    if ($('#previewDesign').attr('src') == '/Images/DesignImages/Temp/') {
        $('#previewDesign').hide();
    }
    
});

function CreateDesign() {
    var typeId = $('#lblTypeId').text();
    var colorId = $('#lblColorId').text();
    var x = $('#dragHelper').position().left;
    var y = $('#dragHelper').position().top;
    var width = $('#dragHelper').width();
    var height = $('#dragHelper').height();
    if ($('#designImageLabel').text().length == 0){
        displayPopupNotification('You must upload a design to continue.', 'error', false);
    } else {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Create/CreateDesign',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "x": x, "y": y, "width": width, "height": height, "typeId": typeId, "colorId": colorId }),
            success: function (result) {
                if (result == "") {
                    displayPopupNotification('Error creating your design please try again.', 'error', false);
                } else {
                    hideLoading();
                    $('#renderedPreview').attr('src', '/Images/DesignImages/Temp/' + result);
                    $('#previewPopup').show();  
                }
                                         
            },
            error: function () {
                displayPopupNotification('Error creating your design please try again.', 'error', false);
            }
        });
    }
    
}
function AcceptDesign() {
    if ($('#agreeToTerms').prop('checked') == true) {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Create/AcceptDesign',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != "") {
                    window.location = "/Create/Complete";
                } else {
                    displayPopupNotification('Error creating your design please try again.', 'error', false);
                }
            },
            error: function () {
                displayPopupNotification('Error creating your design please try again.', 'error', false);
            }
        });
    } else {
        displayPopupNotification('You must agree to the Terms & Conditions before you can continue.', 'error', false);
    }
    
}

function UpdateProduct() {
    var name = $('#txtName').val();
    var description = $('#txtDescription').val();
    var categoryId = $('#category').val();
    var parentProductId = $('#parentProducts').val();
    var privateProduct = $('#privateProduct').prop('checked');
    var hatTypes = [];
    $('.selectedHatType').each(function() {
        if($(this).prop('checked') == true){
            hatTypes.push($(this).closest('.hatType').find('.hatTypeId').text());
        }
    });

    if (name == '' || description == '') {
        displayPopupNotification('You must enter all required fields.', 'error', false);
    } else {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Create/UpdateProduct',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                "name": name,
                "description": description,
                "categoryId": categoryId,
                "privateProduct": privateProduct,
                "hatTypes": hatTypes,
                "parentProductId": parentProductId
            }),
            success: function (result) {
                if (result == "") {
                    displayPopupNotification('Error updating product try again.', 'error', false);
                } else {
                    window.location = "/Product?id=" + result + "&showApproval=1";                                      
                }                
            },
            error: function () {
                displayPopupNotification('Error updating product try again.', 'error', false);
            }
        });
    }
    
}
function UpdateProductExisting() {
    var name = $('#txtName').val();
    var description = $('#txtDescription').val();
    var categoryId = $('#category').val();
    var privateProduct = $('#privateProduct').prop('checked');
    var parentProductId = $('#parentProducts').val();
    var removeProduct = $('#removeProduct').prop('checked');    
    
    if (name == '' || description == '') {
        displayPopupNotification('You must enter all required fields.', 'error', false);
    } else {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Create/UpdateProductExisting',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                "name": name,
                "description": description,
                "categoryId": categoryId,
                "privateProduct": privateProduct,
                "remove": removeProduct,
                "parentProductId": parentProductId
            }),
            success: function (result) {
                if (result == "") {
                    displayPopupNotification('Error updating product try again.', 'error', false);
                } else {

                    window.location = "/Product?id=" + result;

                }
            },
            error: function () {
                displayPopupNotification('Error updating product try again.', 'error', false);
            }
        });
    }

}

function changeHatType(typeId) {
    showLoading();
    var colorId = $('#lblColorId').text();
    if (typeId == 2) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/HatFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/HatFrontWhite.png');
        }
    } else if (typeId == 3) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/TruckFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/TruckFrontWhite.png');
        }
    } else if (typeId == 4) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/DadCapFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/DadCapFrontWhite.png');
        }
    } else if (typeId == 5) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/CurveSnapFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/CurveSnapFrontWhite.png');
        }
    } else if (typeId == 6) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/FlatSnapFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/FlatSnapFrontWhite.png');
        }
    }
    $('#lblTypeId').text(typeId);
    saveHatTypeChange(typeId);    
}
function changePreviewColor(colorId, that) {
    var typeId = $('#lblTypeId').text();
    showLoading();
    if (typeId == 2) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/HatFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/HatFrontWhite.png');
        }        
    } else if (typeId == 3) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/TruckFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/TruckFrontWhite.png');
        }  
    } else if (typeId == 4) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/DadCapFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/DadCapFrontWhite.png');
        }  
    } else if (typeId == 5) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/CurveSnapFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/CurveSnapFrontWhite.png');
        }  
    } else if (typeId == 6) {
        if (colorId == 1) {
            $('#hat').attr('src', '/Images/HatAssets/FlatSnapFrontBlack.png');
        } else if (colorId == 2) {
            $('#hat').attr('src', '/Images/HatAssets/FlatSnapFrontWhite.png');
        }  
    }
    $('.colorPicker').find('.color').each(function () {
        $(this).removeClass('selected');
    });
    $(that).addClass('selected');
    saveColorChange(colorId);
}
function saveHatTypeChange(typeId) {
    $.ajax({
        type: "POST",
        url: '/Create/UpdateTypeId',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            "typeId": typeId
        }),
        success: function (result) {
            hideLoading();
        },
        error: function () {
            displayPopupNotification('Error updating hat type, please try again.', 'error', false);
        }
    });
}
function saveColorChange(colorId) {
    $.ajax({
        type: "POST",
        url: '/Create/UpdateColor',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            "colorId": colorId
        }),
        success: function (result) {
            hideLoading();
        },
        error: function () {
            displayPopupNotification('Error updating color, please try again.', 'error', false);
        }
    });  
}
function selectHatType(typeId, that, currentHatTypeId) {
    if (typeId == currentHatTypeId) {
        //do nothing
    } else {
        if ($(that).find('.selectedHatType').prop('checked') == true) {
            $(that).find('.hatCheck').attr('src', '/Images/CheckGray.png');
            $(that).find('.selectedHatType').prop('checked', false);
        } else {
            $(that).find('.hatCheck').attr('src', '/Images/Check.png');
            $(that).find('.selectedHatType').prop('checked', true);
        }
        
    }    
}