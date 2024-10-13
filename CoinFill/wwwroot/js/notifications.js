var notificationCount;

function GetNotifications() {
    $.ajax({
        url: url + "GetUnread",
        method: "GET",
        success: function (result) {
            notificationCount = result.unreadCount;

            if (notificationCount === 0) {
                $('#notification-section-dropdown-content').append('<span id="no-new-notification"><br /><center class="text-secondary">No unread notification</center><br /></span>');
            }
            else {
                $('#no-new-notification').remove();

                BellNotificationAnimation(notificationCount);

                var notifications = result.userNotification;
                $('#anchor-main').nextAll('notificationsContainer').remove();
                notifications.forEach(element => {
                    CreateNotificationDropdownElement(element);
                });
            }
        },
        error: function () {
            HandleError('And error occurred', 'Please refresh the page or contact the customer support.', 'error');
        }
    });
}

function CreateNotificationDropdownElement(element) {
    let anchorClone = $('#anchor-main').clone();
    anchorClone.attr('id', 'anchor-' + element.id);

    let notificationListMain = anchorClone.find('#notification-list-main');
    notificationListMain.attr('id', 'notification-list-' + element.id);

    let checkIcon = anchorClone.find('#check-icon-main');
    checkIcon.attr('id', 'check-icon-' + element.id);

    let read = anchorClone.find('#read-main');
    read.attr('id', 'read-' + element.id);


    let paragraphMarkRead = anchorClone.find('#paragraph-mark-read-main');
    paragraphMarkRead.attr('id', 'paragraph-mark-read-' + element.id);

    if (element.read === true) {
        checkIcon.addClass('text-primary far fa-check-double');
        read.text(' Read');

        paragraphMarkRead.remove();
    } else {
        notificationListMain.addClass('notification-list--unread');
        checkIcon.addClass('far fa-check');
        read.text(' Unread');

        let smallMarkRead = anchorClone.find('#small-mark-read-main');
        smallMarkRead.attr('id', 'small-mark-read-' + element.id);

        let markRead = anchorClone.find('#mark-read-main');
        markRead.attr('id', 'mark-read-' + element.id);

        markRead.click(function () {
            smallMarkRead.addClass('no-tick');
            const width = paragraphMarkRead.width();
            const height = paragraphMarkRead.height()
            smallMarkRead.fadeIn('fast', function () {
                $(this).html('<i class="fas fa-spinner fa-spin text-primary no-tick"></i>');
                paragraphMarkRead.width(width);
                paragraphMarkRead.height(height);
            })
            

            $.ajax({
                url: url + "SetRead",
                method: "POST",
                data: { notificationId: element.id },
                success: setTimeout(function (result) {
                    //GetNotifications();
                    smallMarkRead.remove();
                    checkIcon.removeClass('fa-check');
                    checkIcon.addClass('fa-check-double');
                    checkIcon.addClass('text-primary');
                    read.text(' Read');
                    anchorClone.fadeOut('slow');

                    var counter = $('#main-notif');

                    notificationCount -= 1;

                    counter.attr('data-count', notificationCount);

                    if (Number(counter.attr('data-count')) === 0) {
                        anchorClone.promise().done(function () {
                            $('#notification-section-dropdown-content')
                                .append('<span id="no-new-notification"><br /><center class="text-secondary">No unread notification</center><br /></span>')
                                .fadeIn('slow');

                            counter.removeClass('notify');
                            counter.removeClass('show-count');
                        })
                    }
                }, 1000),
                error: function () {
                    HandleError('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                }
            })
        })
    }

    let notificationIconMain = anchorClone.find('#notification-icon-main');
    notificationIconMain.attr('id', 'notification-icon-' + element.id);
    notificationIconMain.addClass('text-' + element.severity);
    notificationIconMain.addClass(element.icon);

    let notificationText = anchorClone.find('#notification-text-main');
    notificationText.attr('id', 'notification-text-' + element.id);
    notificationText.text(element.title);

    anchorClone.show();
    anchorClone.appendTo('#notification-section-dropdown-content');
}

function BellNotificationAnimation(count) {
    var el = document.getElementById('main-notif');
    el.setAttribute('data-count', count);
    el.classList.remove('notify');
    el.offsetWidth = el.offsetWidth;

    if (notificationCount > 0) {
        el.classList.add('notify');
        el.classList.add('show-count');
    }
}

$('#notif-navbarDropdown').click(function () {
    BellNotificationAnimation(notificationCount);

    let dropdown = $('#notif-dropdown-menu');
    if (dropdown.is(':visible'))
        dropdown.fadeOut('fast');
    else if (dropdown.is(':hidden'))
        dropdown.fadeIn('fast');
});

function HandleError(title, body, severity) {
    toastr[severity](body, title);
}

GetNotifications();

let connection = new signalR.HubConnectionBuilder().withUrl(notificationHubUrl).build();

connection.on('displayNotification', () => {
    GetNotifications();
});

connection.start();


