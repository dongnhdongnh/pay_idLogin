var countryCode = "";
var countryIndex = "";
var phoneNational = "";

function clearNewPhone() {
    countryCode = "";
    phoneNational = "";
    countryIndex = "";
}

function GenerateSMSCode(email, phoneNumber, typeGenerate) {
    if (typeGenerate === "ChangeTwoFactor") {
        $('#twoFa').modal('show');
    } else if (typeGenerate === "ChangePhoneOldPhone") {
        $('#verifyOldPhone').modal('show');
    }
    else if (typeGenerate === "ChangePhoneNewPhone") {
        if (phoneNumber !== null && phoneNumber.length > 3) {
            var phoneHide = phoneNumber.substr(0, 1);
            var lengthPhone = phoneNumber.length;
            for (var i = 1; i <= lengthPhone - 2; i++) {
                phoneHide += "x";
            }
            phoneHide += phoneNumber.substr(lengthPhone - 2, lengthPhone);
            $('#verifyPhoneHide').text('We just sent an SMS to ' + phoneHide + ' with your verification code. It may take several seconds to arrive.');
        }
        $('#addNewPhone').modal('hide');
        $('#verifyNewPhone').modal('show');

    }
    else if (typeGenerate === "ChangePassword") {
        $('#changePassword').modal('hide');
        $('#verifyChangePassword').modal('show');
    }
    else if (typeGenerate === "LockAccount") {
        $('#lockAccount').modal('show');
    }
    else if (typeGenerate === "AddLockScreen") {
        $('#lockScreen').modal('hide');
        $('#verifyLockScreen').modal('show');
    }
    var obj = {
        Email: email,
        PhoneNumber: phoneNumber,
        TypeGenerate: typeGenerate
    };
    $.ajax({
        url: '/Security/GenerateSms',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errorMessage) {
        }
    });
}


// change two fa
function VerifySmsEnableTwoFactor(email) {
    var code = $("#changeTwoFactorCode").val();
    if (code)
        var obj = {
            Email: email,
            Code: code
        };
    $.ajax({
        url: '/Security/VerifySmsEnableTwoFactor',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#twoFa').modal('hide');
                $('#twoFaEnableAuthenticator').modal('show');
            } else {
                $('#verifyTwoFaError').text(result.responseText);
            }
        },
        error: function () {
            $('#verifyTwoFaError').text("Enable Two Factor Authentication error. Please try again.");
        }
    });
}

function DisableTwoFactor(email) {
    var code = $("#changeTwoFactorCode").val();
    if (code)
        var obj = {
            Email: email,
            Code: code
        };
    $.ajax({
        url: '/Security/DisableTwoFactor',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#twoFa').modal('hide');
                $('#twoFaSuccess').modal('show');
            } else {
                $('#verifyTwoFaError').text(result.responseText);
            }
        },
        error: function () {
            $('#verifyTwoFaError').text("Disable Two Factor Authentication error. Please try again.");
        }
    });
}
function copyTextSecret() {
    var copyText = document.getElementById("inputShareKey");
    /* Select the text field */
    copyText.select();
    /* Copy the text inside the text field */
    document.execCommand("copy");
}

function EnableTwoFactor(email) {
    var code = $("#inputTwoAuthenticator").val();
    if (code)
        var obj = {
            Email: email,
            Code: code
        };
    $.ajax({
        url: '/Security/EnableTwoFactor',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#twoFaEnableAuthenticator').modal('hide');
                $('#twoFaSuccess').modal('show');
            } else {
                $('#twoFaEnableAuthenticatorError').text(result.responseText);
            }
        },
        error: function () {
            $('#twoFaEnableAuthenticatorError').text("Enable Two Factor Authentication error. Please try again.");
        }
    });
}


// Change phone number

function VerifyOldPhone(email, phoneNumber) {
    clearNewPhone();
    var code = $("#inputVerifyOldPhone").val();
    var obj = {
        Email: email,
        PhoneNumber: phoneNumber,
        Code: code,
        TypeGenerate: "ChangePhoneOldPhone"
    };
    $.ajax({
        url: '/Security/VerifyPhoneNumber',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#verifyOldPhone').modal('hide');
                $('#addNewPhone').modal('show');
            } else {
                $('#verifyOldPhoneError').text(result.responseText);
            }
        },
        error: function (errorMessage) {
            $('#verifyOldPhoneError').text("Verify code error. please try again.");
        }
    });
}

function AddNewPhone(email, isGoogleAuthenticator) {
    countryCode = $('#inputNewPhoneCode').text();
    countryIndex = $('#countryIndex').val();
    phoneNational = $('#inputPhone').val();
    var phoneNumber = countryCode + phoneNational;
    if (!phoneNumber.match("/^((\\+\\d{1,3}(-| )?\\(?\\d\\)?(-| )?\\d{1,5})|(\\(?\\d{2,6}\\)?))(-| )?(\\d{3,4})(-| )?(\\d{4})(( x| ext)\\d{1,5}){0,1}$/")) {
        $('#addNewPhone').text("Phone number incorrect.");
    }
    if (isGoogleAuthenticator) {
        $('#addNewPhone').modal('hide');
        $('#verifyNewPhone').modal('show');
    } else {
        GenerateSMSCode(email, phoneNumber, "ChangePhoneNewPhone");
    }
}

function VerifyNewPhone(email) {
    var code = $("#inputVerifyNewPhone").val();
    var phoneNumber = countryCode + phoneNational;
    var obj = {
        Email: email,
        PhoneNumber: phoneNumber,
        TypeGenerate: "ChangePhoneNewPhone",
        Code: code,
        PhoneNational: phoneNational,
        CountryCode: countryCode,
        CountryIndex: countryIndex
    };
    $.ajax({
        url: '/Security/VerifyPhoneNumber',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#verifyNewPhone').modal('hide');
                $('#changePhoneSuccess').modal('show');
            } else {
                $('#verifyNewPhoneError').text(result.responseText);
            }
        },
        error: function (errorMessage) {
            $('#verifyNewPhoneError').text("Verify new PhoneNumber error. Please try again.");
        }
    });
}

function ChangePassword(email, phoneNumber, typeGenerate, isGoogleAuthenticator) {
    clearChangePasswordError();
    var oldPassword = $('#oldPassword').val();
    var newPassword = $('#newPassword').val();
    var confirmPassword = $('#confirmPassword').val();
    if (!newPassword.match("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,100}$")) {
        $('#oldPasswordError').text("Old Password incorrect.");
        return;
    }
    if (!newPassword.match("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,100}$")) {
        $('#newPasswordError').text("Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)");
        return;
    }
    if (oldPassword === newPassword) {
        $('#newPasswordError').text("New passwords and old passwords must be different.");
        return;
    }
    if (confirmPassword !== newPassword) {
        $('#confirmPassWordError').text("The password and confirmation password do not match.");
        return;
    }

    //Check password
    var obj = {
        Email: email,
        OldPassword: oldPassword
    };
    $.ajax({
        url: '/Security/CheckPassword',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                if (isGoogleAuthenticator) {
                    $('#changePassword').modal('hide');
                    $('#verifyChangePassword').modal('show');
                } else {
                    GenerateSMSCode(email, phoneNumber, typeGenerate);
                }
            } else {
                $('#oldPasswordError').text(result.responseText);
            }
        },
        error: function (errorMessage) {
            $('#oldPasswordError').text("Old Password incorrect.");
        }
    });
}

function VerifyChangePassword(email) {
    var oldPassword = $('#oldPassword').val();
    var newPassword = $('#newPassword').val();
    var changePasswordCode = $('#changePasswordCode').val();
    // if (changePasswordCode.match(/^-{0,1}\d+$/)) {
    //     $('#changePasswordCodeError').text("Code must be a number.");
    //     return;
    // }
    var obj = {
        Email: email,
        OldPassword: oldPassword,
        NewPassword: newPassword,
        Code: changePasswordCode
    };
    $.ajax({
        url: '/Security/VerifyChangePassword',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#verifyChangePassword').modal('hide');
                $('#changePasswordSuccess').modal('show');
            } else {
                $('#verifyChangePasswordError').text(result.responseText);
            }
        },
        error: function (errorMessage) {
            $('#verifyChangePasswordError').text("Change Password error. Please try again.");
        }
    });
}

function SetLockScreen(email, typeGenerate, isGoogleAuthenticator) {
    var password = $('#lockScreenPassword').val();
    var confirmPassword = $('#lockScreenPasswordConfirm').val();
    if (!password.match("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,100}$")) {
        $('#lockScreenError').text("Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)");
        return;
    }
    if (confirmPassword !== password) {
        $('#confirmPassWordError').text("The password and confirmation password do not match.");
        return;
    }

    if (isGoogleAuthenticator) {
        $('#lockScreen').modal('hide');
        $('#verifyLockScreen').modal('show');
    } else {
        //GenerateSMSCode
        GenerateSMSCode(email, "", typeGenerate);
    }
}

function VerifyLockScreen(email) {
    var password = $('#lockScreenPassword').val();
    var lockScreenCode = $('#inputVerifyLockScreen').val();
    var obj = {
        Email: email,
        Password: password,
        Code: lockScreenCode
    };
    $.ajax({
        url: '/Security/VerifyLockScreen',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result && result.success) {
                $('#verifyLockScreen').modal('hide');
                $('#lockScreenSuccess').modal('show');
            } else {
                $('#verifyLockScreenError').text(result.responseText);
            }
        },
        error: function (errorMessage) {
            $('#verifyLockScreenError').text("Set Password Lock Screen error. Please try again.");
        }
    });

}

function ResendSMS(email, phoneNumber, typeGenerate) {
    if (typeGenerate === "ChangePhoneNewPhone") {
        phoneNumber = countryCode + phoneNational;
    }
    var obj = {
        Email: email,
        PhoneNumber: phoneNumber,
        TypeGenerate: typeGenerate
    };
    $.ajax({
        url: '/Security/GenerateSms',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errorMessage) {
        }
    });
}

function LockAccount(email) {
    var code = $('#inputLockAccount').val();
    var obj = {
        Email: email,
        Code: code
    };
    $.ajax({
        url: '/Security/LockAccount',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.success) {
                $('#lockAccount').modal('hide');
                ReloadPage();
            } else {
                $('#lockAccountError').text(result.responseText);
            }
        },
        error: function (errorMessage) {
            $('#lockAccountError').text(result.responseText);
        }
    });

}

function LockOut() {
    var obj = {};
    $.ajax({
        url: '/Account/LockAccount',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.success) {

            } else {
            }
        },
        error: function (errorMessage) {
        }
    });

}

function clearChangePasswordError() {
    $('#newPasswordError').text('');
    $('#confirmPassWordError').text('');
    $('#changePasswordCodeError').text('');
    $('#oldPasswordError').text('');
    $('#verifyChangePasswordError').text('');
    $('#changePasswordError').text('');
}

function ReloadPage() {
    location.reload()
}