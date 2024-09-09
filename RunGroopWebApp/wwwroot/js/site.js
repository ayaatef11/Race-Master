function readNotification(id, target) {
    $.ajax({
        url: "/Notification/ReadNotification",
        method: "GET",
        data: { notificationId: id },
        success: function (result) {
            getNotification();

            $(target).fade0ut('slow');

            error: function(error) {
                console.log(error);
            }
        })
}
                getNotification();

                let connection = new signalR.HubConnection("/signalServer");

                connection.on('displayNotification', () => {
                    getNotification();

                });

                connection.start();
            });