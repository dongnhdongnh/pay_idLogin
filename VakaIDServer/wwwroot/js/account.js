function resendEmail(email, typeGenerate) {
    var obj = {
        Email: email,
        TypeGenerate: typeGenerate
    };
    $.ajax({
        url: '/Account/ResendEmail',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            ReloadPage();
        },
        error: function (errorMessage) {
        }
    });
}

function ResendSMS(email, typeGenerate) {
    var obj = {
        Email: email,
        TypeGenerate: typeGenerate
    };
    $.ajax({
        url: '/Account/ResendSMS',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            ReloadPage();
        },
        error: function (errorMessage) {
        }
    });

}

function ReloadPage() {
    location.reload()
}