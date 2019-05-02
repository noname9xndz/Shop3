

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/Shop3Hub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {

   // console.log(message);
    var template = $('#announcement-template').html();
    var html = Mustache.render(template, {
        Content: message.content,
        Id: message.id,
        Title: message.title,
        FullName: message.fullName,
        Avatar: message.avatar  /*todo join aspnetuser get avatar*/
    });
    $('#annoncementList').prepend(html);

    var totalAnnounce = parseInt($('#totalAnnouncement').text()) + 1;

    $('#totalAnnouncement').text(totalAnnounce);
});