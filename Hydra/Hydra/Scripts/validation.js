$('form').on('submit', function () {

    var request,
        $page = $('body'),
        $form = $(this),
        $response = $('.response'),
        data = $form.serialize();

    request = $.ajax({
        type: 'post',
        data: data
    });

    request.fail(function (xhr) {
        var response, i;

        $form.find('p.invalid').removeClass('invalid');

        if (xhr.status === 400) {
            response = JSON.parse(xhr.responseText);
            var fields = Object.keys(response);
            $response.text(vkbeautify.json(xhr.responseText, 2));

            fields.forEach(function (field) {
                if (response[field].Errors && response[field].Errors.length > 0) {
                    var theDiv = $('#' + field).closest('div.form-group');
                    theDiv.addClass('invalid');
                    
                    // the title of a div serves as tooltip. appending the error messages
                    // will thus give you nice info what exactly is the problem with the field.
                    theDiv[0].title = '';
                    response[field].Errors.forEach(function (error) {
                        theDiv[0].title = theDiv[0].title + error.ErrorMessage + '\n';
                    });
                }
            });
        }

        $page.removeClass('successful').addClass('invalid');
    });

    request.success(function (r) {
        $form.find('div.invalid').removeClass('invalid').attr('title', '');
        $page.removeClass('invalid').addClass('successful');
        $response.text('(No response body)');

        if (r != null && r.redirect !== undefined && r.redirect != null && r.redirect != '') {
            window.location.replace(r.redirect);
        }
    });

    return false;

});