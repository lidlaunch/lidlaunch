$(document).ready(function () {
    $('#searchTop').blur(function () {
        window.location = "/shop?search=" + $('#searchTop').val();
    });
    $('#searchTop').on('keypress', function (e) {
        if (e.which === 13) {
            window.location = "/shop?search=" + $('#searchTop').val();
        }
    });
    $('#search').blur(function () {
        window.location = "/shop?search=" + $('#search').val();
    });
    $('#search').on('keypress', function (e) {
        if (e.which === 13) {
            window.location = "/shop?search=" + $('#search').val();
        }
    });
    $('#searchButton').click(function () {
        if ($('#search').val() != '') {
            window.location = "/shop?search=" + $('#search').val();
        }
    });
});
function ApproveProduct(id) {
    $.ajax({
        type: "POST",
        url: '/Dashboard/ApproveProduct',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "id": id }),
        dataType: "json",
        success: function (result) {
            if (result) {
                location.reload();
            } else {
                displayPopupNotification('Error approving product.', 'error', false);
            }
        },
        error: function () {
            displayPopupNotification('Error approving product.', 'error', false);
        }
    });
}

function DenyProduct(id, that) {
    var denyReason = $(that).closest('tr').find("#txtDenyReason").val();
    $.ajax({
        type: "POST",
        url: '/Dashboard/DenyProduct',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "id": id, "denyReason": denyReason}),
        dataType: "json",
        success: function (result) {
            if (result) {
                location.reload();
            } else {
                displayPopupNotification('Error approving product.', 'error', false);
            }
        },
        error: function () {
            displayPopupNotification('Error approving product.', 'error', false);
        }
    });
}
function changeHatTypeProduct(typeId, that) {
    $('#typeId').text(typeId);

    $('.hatDisplayImage').each(function () {
        $(this).addClass('hidden');
    });

    $('.hatDisplayImage.type' + typeId).first().removeClass('hidden');
    $('.hatStyleColors.' + typeId).first().find(' .selectedHatColor').prop('checked', true);
    $('.hatStyleColors.' + typeId).first().find('.color').addClass('selected');

    if (typeId == 2) {
        $('#oneSizeFitsAll').hide();
        $('#selectSize').show();
    } else {        
        $('#selectSize').hide();
        $('#oneSizeFitsAll').show();
    } 

    $('.hatStyleColors').each(function () {
        $(this).addClass('hidden');
    });

    $('.hatStyleColors.' + typeId).removeClass('hidden');

    $('.selectedHatType').each(function () {
        $(this).prop('checked', false);
        $(this).closest('.hatStyle').find('.hatCheck').attr('src', '/Images/CheckGray.png');
    });
    if ($(that).find('.selectedHatType').prop('checked') == true) {
        $(that).find('.hatCheck').attr('src', '/Images/CheckGray.png');
        $(that).find('.selectedHatType').prop('checked', false);
    } else {
        $(that).find('.hatCheck').attr('src', '/Images/Check.png');
        $(that).find('.selectedHatType').prop('checked', true);
    } 
    $('#selectedHatTypeInfo').text($(that).closest('.hatStyle').find('.hatStyleName').text());
    $('#selectedHatTypeImg').attr('src', $(that).closest('.hatStyle').find('.hatStyleImg').attr('src'));
    $('#productHatStyleSelect').hide();
}

function changeHatColor(colorId, that) {
    var typeId = $('#typeId').text();
    $('#colorId').text(colorId);

    $('.selectedHatColor').each(function () {
        $(this).prop('checked', false);
    });

    $('#' + colorId + '.selectedHatColor').prop('checked', true);

    $('.color').each(function () {
        $(this).removeClass('selected');
    });

    $('.hatStyleColors.' + typeId + ' .color.' + colorId).addClass('selected');

    $('.hatDisplayImage').each(function () {
        $(this).addClass('hidden');
    });

    $('.hatDisplayImage.type' + typeId + '.color' + colorId).removeClass('hidden');    
}