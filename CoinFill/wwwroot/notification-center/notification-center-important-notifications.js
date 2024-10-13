var importantCount;
var unreadCount;

function GetImportantNotifications() {
    $.ajax({
        url: url + "GetImportant",
        method: "GET",
        success: function (result) {
            importantCount = result.importantCount;
            unreadCount = result.unreadCount;

            SetUnreadNotificationCounter(unreadCount);

            if (importantCount === 0) {
                $('#notification-center-content').append('<div class="card"><div class="card-body"><span id="no-important-notification"><center class="text-secondary"><i class="far fa-star-shooting text-warning mr-2"></i>No important notification</center></span></div></div>');
            } else {
                $('#no-important-notification').remove();

                var notifications = result.userNotification;
                $('#notifications-holder').nextAll('notificationsContainer').remove();
                notifications.forEach(element => {
                    CreateNotificationDropdownElement(element);
                });
            }
        },
        error: function () {
            HandleError('An error occurred', 'Please refresh the page or contact the customer support.', 'error');
        }
    });
}

function CreateNotificationDropdownElement(element) {
    let notificationsHolder = $('#notifications-holder').clone();
    notificationsHolder.attr('id', 'notifications-holder-' + element.id);

    let notificationListMain = notificationsHolder.find('#notification-list-main');
    notificationListMain.attr('id', 'notification-list-' + element.id);

    let checkIcon = notificationsHolder.find('#check-icon-main');
    checkIcon.attr('id', 'check-icon-' + element.id);

    let paragraphMarkRead = notificationsHolder.find('#paragraph-mark-read-main');
    paragraphMarkRead.attr('id', 'paragraph-mark-read-' + element.id);

    let importantStar = notificationsHolder.find('#important-main');
    importantStar.attr('id', 'important-' + element.id);

    if (element.important) {
        importantStar.removeClass('fal');
        importantStar.addClass('fas');
        importantStar.addClass('important-star--true');
    }
    else {
        importantStar.removeClass('fas');
        importantStar.addClass('fal');
        importantStar.removeClass('important-star--true');
    }

    importantStar.click(function () {
        importantStar.toggleClass('fas');
        importantStar.toggleClass('fal');
        importantStar.toggleClass('important-star--true')

        if (element.important === true) {
            $.ajax({
                url: url + "SetUnimportant",
                method: "POST",
                data: { notificationId: element.id },
                success: function () {
                    toastr['success']('', 'Notification is set to unimportant');
                    element.important = false;
                },
                error: function () {
                    HandleError('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                }
            });
        } else {
            $.ajax({
                url: url + "SetImportant",
                method: "POST",
                data: { notificationId: element.id },
                success: function () {
                    toastr['success']('', 'Notification is set to important');
                    element.important = true;
                },
                error: function () {
                    HandleError('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                }
            });
        }
    })

    let notificationIconMain = notificationsHolder.find('#notification-icon-main');
    notificationIconMain.attr('id', 'notification-icon-' + element.id);
    notificationIconMain.addClass('text-' + element.severity);
    notificationIconMain.addClass(element.icon);

    let notificationText = notificationsHolder.find('#notification-text-main');
    notificationText.attr('id', 'notification-text-' + element.id);
    notificationText.text(element.title);

    notificationsHolder.show();
    notificationsHolder.appendTo('#notification-center-content');
}

function SetUnreadNotificationCounter(count) {
    var el = $('#unread-notifications');
    el.hide();

    if (unreadCount > 0) {
        el.fadeIn('fast');
        el.html(unreadCount);
    }
}

function HandleError(title, body, severity) {
    toastr[severity](body, title);
}

GetImportantNotifications();

let connection = new signalR.HubConnectionBuilder().withUrl(notificationHubUrl).build();

connection.on('displayNotification', () => {
    GetImportantNotifications();
});

connection.start();