

function loadContacts(json, path) {
    if (json.hasOwnProperty('error')) {
        $('#contacts').html('<li class = "error">' + json.error + '</li>');
        return;
    }


    var contactsViewArray = [];
    var data = json.contact;
    for (var  i = 0; i <  data.length; i++) {
        var contact = data[i];
        var contactString = "<li><b>" + contact.Name + " " + contact.Surname + "</b> age: "
            + contact.Age + " location: " + contact.Address.Country + ", "
            + contact.Address.City + "</li>";
        contactsViewArray.push(contactString);
    }

    $('#contacts').html(contactsViewArray.join("\n"));
    $('.page-link').removeClass('bold nolink');
    $('#page-link-' + json.pageNumber).addClass('bold nolink');

    if (json.pageNumber > 1) {
        $('#prev-link').removeClass('hiden');
        $('#prev-link').addClass('show');
    } else {
        $('#prev-link').removeClass('show');
        $('#prev-link').addClass('hiden');
    }

    if (json.pageNumber < json.pagesCount) {
        $('#next-link').removeClass('hiden');
        $('#next-link').addClass('show');
    } else {
        $('#next-link').removeClass('show');
        $('#next-link').addClass('hiden');
    }
    $('#prev-link').attr('href',path+ '?page=' + (json.pageNumber - 1));
    $('#next-link').attr('href', path + '?page=' + +(json.pageNumber + 1));

}

function loadajax(url) {
    $.ajax({
        url: url,
        data: "username=test",
        type: "get",
        success: loadContacts
    });
}