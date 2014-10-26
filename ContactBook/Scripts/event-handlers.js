function showDialog(dialogID, urlForDialogContent) {
    $.get(urlForDialogContent, function (data) {
        $(dialogID).html(data);
        $(dialogID).modal('show');
    });
}