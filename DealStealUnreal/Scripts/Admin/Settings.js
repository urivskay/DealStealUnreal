// class site settings
function Settings() { }

// methods

Settings.prototype.SaveMessageForMail = function () {
    var text = tinyMCE.activeEditor.getContent();

    $.ajax({
        url: "/Settings/SaveMailMessage/",
        type: 'POST',
        data: { message: text },
        dataType: "text",
        success:
            function (result)
            {
                if (result == "success")
                    $("#div_MailMessage").html("<p>Changes were successfully saved!</p>");
                else 
                    $("#div_MailMessage").prepend("<p style='color:red;'>Error! Changes not saved!</p>");
            }
    });
}

var settings = new Settings();