function onCreateContactButtonClick(url) {
    try {
        showDialog('#ModalDialog', url);
    } catch (e) {
        alert(e.Message);
    }
}

function onEditContactButtonClick(url) {
    try {
        showDialog('#ModalDialog', url);
    } catch (e) {
        alert(e.Message);
    }
}

function onRemoveContactButtonClick(url) {
    try {
        if (confirm("Вы действительно хотите удалить контакт?")) {
            window.location = url;
        }
    } catch (e) {
        alert(e.Message);
    }
}

function showDialog(dialogID, urlForDialogContent) {
    $.get(urlForDialogContent, function (data) {
        $(dialogID).html(data);
        $(dialogID).modal('show');
    });
}