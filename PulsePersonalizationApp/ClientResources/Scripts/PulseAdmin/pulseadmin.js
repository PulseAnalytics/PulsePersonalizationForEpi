$(document).ready(function () {
    $("#tabs").tabs();

    $("#registrationForm").validate({
        rules: {
            firstName: {
                required: true,
                minlength: 2
            },
            lastName: {
                required: true,
                minlength: 2
            },
            companyName: {
                required: true,
                minlength: 2
            },
            email: {
                required: true,
                email: true
            }
        },
        submitHandler: function () {
            submitForm();
        }
    });

    $("#saveApiKey").click(function () {
        disableForm(true);
        $.ajax({
            method: "POST",
            url: "PulseAdmin/SaveApiKey",
            data: { apiKey: $("#apiKey").val() },
            success: function () {
                disableForm(false);
                alert('API key saved successfully! Addon activated! Congratulation!');
            },
            error: function () {
                alert('Error while saving API key!');
                disableForm(false);
            }
        });
    });

    $("#editApi").click(function () {
        $("#apiKey").prop('disabled', !$("#apiKey").prop('disabled'));
    });

    $("#deleteVGbtn").click(function () {
        disableForm(true);
        if ($("#deleteVG").val() == "DELETE") {
            $.ajax({
                method: "GET",
                url: "PulseAdmin/RemoveAllVisitorGroups",
                success: function () {
                    $("#deleteVG").val('');
                    alert('All Visitor Groups successfully deleted!');
                    disableForm(false);
                },
                error: function () {
                    $("#deleteVG").val('');
                    alert('Error while deleting Visitor Groups!');
                    disableForm(false);
                }
            });
        }
    });

    $("#deleteVG").on("change paste keyup", function () {
        if ($("#deleteVG").val() == "DELETE") {
            $("#deleteVGbtn").prop('disabled', false);
        } else {
            $("#deleteVGbtn").prop('disabled', true);
        }
    });

    $("#runDiag").click(function () {
        disableForm(true);
        $("#diagText").val('');
        $.ajax({
            method: "GET",
            url: "PulseAdmin/RunDiagnostics",
            success: function (result) {
                $("#diagText").val(result)
                disableForm(false);
            },
            error: function () {
                alert('Error while running diagnostics!');
                disableForm(false);
            }
        });
    });

    $("#clearDiag").click(function () {
        $("#diagText").val('');
    });

    function submitForm() {
        disableForm(true);

        $.ajax({
            method: "POST",
            url: "https://getnewsubscriptionforpulseepiconnector.azurewebsites.net/api/Register?code=AxBUjq/6mV/wpnjQ8HT6dbeOGsW0GrnDpvK6V6/ip6fICpWh//qX0Q==",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(getFormData()),
            success: function (result) {
                console.log(result);           
                saveCredentials(result.subkey);
            },
            error: function (xhr) {
                var response = JSON.parse(xhr.responseText)
                console.log(response);
                alert('Error while acquiring new API key: ' + response.error);
                disableForm(false);
            }
        });
    };

    function disableForm(disabled) {
        $("#registrationForm input, #pulseSubmit, #apiKey, #saveApiKey").prop('disabled', disabled);
        $("#apiKey").prop('disabled', true);

        if (disabled) {
            $("body").addClass('loading');
        } else {
            $("body").removeClass('loading');
        }
    }

    function getFormData() {
        return {
            firstname: $("#firstName").val(),
            lastname: $("#lastName").val(),
            email: $("#email").val(),
            companyname: $("#companyName").val()
        };
    }

    function saveCredentials(apiKey) {
        var formData = getFormData();
        formData.pulseApiKey = apiKey;

        $.ajax({
            method: "POST",
            url: "PulseAdmin/SaveCredentials",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function () {
                $("#apiKey").val(apiKey);
                alert('API key saved successfully! Addon activated! Congratulation!');
                disableForm(false);
            },
            error: function () {
                alert('Error while saving credentials!');
                disableForm(false);
            }
        });
    }
});