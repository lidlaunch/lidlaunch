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
function changeHatTypeProduct(typeId, that) {
    $('#typeId').text(typeId);
    if (typeId == 2) {
        $('.type2').show();
        $('.type3').hide();
        $('.type4').hide();
        $('.type5').hide();
        $('.type6').hide();
        $('#oneSizeFitsAll').hide();
        $('#selectSize').show();
    } else if (typeId == 3) {
        $('.type2').hide();
        $('.type3').show();
        $('.type4').hide();
        $('.type5').hide();
        $('.type6').hide();
        $('#selectSize').hide();
        $('#oneSizeFitsAll').show();
    } else if (typeId == 4) {
        $('.type2').hide();
        $('.type3').hide();
        $('.type4').show();
        $('.type5').hide();
        $('.type6').hide();
        $('#selectSize').hide();
        $('#oneSizeFitsAll').show();
    } else if (typeId == 5) {
        $('.type2').hide();
        $('.type3').hide();
        $('.type4').hide();
        $('.type5').show();
        $('.type6').hide();
        $('#selectSize').hide();
        $('#oneSizeFitsAll').show();
    } else if (typeId == 6) {
        $('.type2').hide();
        $('.type3').hide();
        $('.type4').hide();
        $('.type5').hide();
        $('.type6').show();
        $('#selectSize').hide();
        $('#oneSizeFitsAll').show();
    }
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