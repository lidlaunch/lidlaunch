var user = {
    "Id": "",
    "firstName": "",
    "lastName": "",
    "middleInitial": "",
    "email": "",
    "password": ""
}

function closeDesignerPopup() {
    window.location = "/Home";
}

function saveUser() {
    var firstName = $('#txtFirstName').val();
    var lastName = $('#txtLastName').val();
    var middleInitial = $('#txtMiddleInitial').val();
    var email = $('#txtEmail').val();
    var password = $('#txtPassword').val();
    var verifyPassword = $('#txtVerifyPassword').val();
    var userId = $('#lblUserId').text();

    if (firstName == '' || lastName == '' || email == '' || password == '' || verifyPassword == '') {
        displayPopupNotification('You must enter in all required fields.', 'error', false);
    } else {
        if (validateEmail(email)) {
            if (password == verifyPassword) {
                showLoading();
                if (userId == '') {
                    $.ajax({
                        type: "POST",
                        url: '/User/CreateUser',
                        data: JSON.stringify({ "firstName": firstName, "lastName": lastName, "middleInitial": middleInitial, "email": email, "password": password }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            hideLoading();                            
                            $('#createDesignerPopup').show();
                        },
                        error: function (err) {
                            displayPopupNotification('User create failed please try again.', 'error', false);
                        }  
                        //error: displayPopupNotification('User create failed please try again.', 'error', false)
                    });
                } else {
                    $.ajax({
                        type: "POST",
                        url: '/User/UpdateUser',
                        data: JSON.stringify({ "firstName": firstName, "lastName": lastName, "middleInitial": middleInitial, "email": email, "password": password, "userId": userId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            window.location = "/Home";
                        },
                        error: function (err) {
                            displayPopupNotification('User update failed please try again.', 'error', false);
                        } 
                    });
                }
            } else {
                displayPopupNotification('Both passwords must match.', 'error', false);
            }
        } else {
            displayPopupNotification('Please enter a valid email.', 'error', false);
        }
        
    }

}

function login() {
    var email = $('#txtEmail').val();
    var password = $('#txtPassword').val();

    if (email == '' || password == '') {
        displayPopupNotification('You must enter all required fields.', 'error', false);
    } else {  
        showLoading();
        $.ajax({
            type: "POST",
            url: '/User/LoginUser',
            data: JSON.stringify({"email":email,"password": password}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data, status) {
                if (data.Id > 0) {
                    window.location = "/Home";
                } else {
                    displayPopupNotification('Login failed. Username or Password is wrong.', 'error', false);
                }
            },
            error: function (err) {
                displayPopupNotification('Login failed please try again.', 'error', false);
            }
        });
        
    }

}

function SendForgotPasswordEmail() {
    var email = $('#txtEmail').val();

    if (email == '') {
        displayPopupNotification('Please enter your email.', 'error', false);
    } else {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/User/SendPasswordResetEmail',
            data: JSON.stringify({ "email": email }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data, status) {
                if (data) {
                    displayPopupNotification('Please check your email for instructions on  how to reset your password.', 'error', false);
                } else {
                    displayPopupNotification('Failed sending email please try again.', 'error', false);
                }
            },
            error: function (err) {
                displayPopupNotification('Failed sending email please try again.', 'error', false);
            }
        });

    }

}
function UpdatePassword() {
    var email = $('#txtEmail').val();
    var password = $('#txtPassword').val();
    var verifyPassword = $('#txtConfirmPassword').val();
    if (email == '') {
        displayPopupNotification('Please enter all fields.', 'error', false);
    } else {
        if (password == verifyPassword) {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/User/ResetPasswordProcess',
                data: JSON.stringify({ "email": email, "password":password }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, status) {
                    window.location = "/User/Login";
                },
                error: function (err) {
                    displayPopupNotification('Login failed please try again.', 'error', false);
                }
            });
        } else {
            displayPopupNotification('Passwords do not match.', 'error', false);
        }       

    }

}