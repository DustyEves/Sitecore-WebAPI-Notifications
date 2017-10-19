jQuery(document).ready(function () {

    jQuery("#cmdSubmit").click(function () {
        console.log('Sending notification request');
        jQuery.ajax({
            url: "/",
            type: "POST",
            data: {
                scAction: "SendSubscriptionMessage",
                scController: "PushAPI"
            },
            success: function () { console.log('Subscription data pushed to server'); },
            error: function () { console.log('error persisting subscription data to server'); }
        });
    });

    if ('serviceWorker' in navigator) {
        console.log('I have a service worker')
    }
    if ('PushManager' in window) {
        console.log('and a push manager')
    }
    

});

function askPermission() {
    return new Promise(function (resolve, reject) {
        const permissionResult = Notification.requestPermission(function (result) {
            resolve(result);
        });

        if (permissionResult) {
            permissionResult.then(resolve, reject);
        }
    })
    .then(function (permissionResult) {
        if (permissionResult !== 'granted') {
            throw new Error('We weren\'t granted permission.');
        }
        else {
            console.log("permission Granted");
            subscribeUserToPush();
        }
    });
}


function subscribeUserToPush() {

    var publicKey = jQuery('#PushAPIPublicKey').val();
    return track()
    .then(function (registration) {
        const subscribeOptions = {
            userVisibleOnly: true,
            applicationServerKey: urlBase64ToUint8Array(publicKey)
        };

        return registration.pushManager.subscribe(subscribeOptions);
    })
    .then(function (pushSubscription) {
        console.log('Received PushSubscription: ', JSON.stringify(pushSubscription));
        recordSubscription(pushSubscription);
        return pushSubscription;
    });
}


function recordSubscription(_pushSubscription) {
    jQuery.ajax({
        url: "/",
        type: "POST",
        data: {
            scAction: "RecordSubscription",
            scController: "PushAPI",
            _pushAuthorization: JSON.stringify(_pushSubscription),
            _engagementPlan: jQuery('#EngagementPlanId').val(),
            _engagementState: jQuery('#EngagementPlanState').val(),
            _goal: jQuery('#GoalTriggerId').val()
        },
        success: function () { console.log('Subscription data pushed to server'); },
        error: function () { console.log('error persisting subscription data to server'); }
    });
}

function track() {
    return navigator.serviceWorker.register('/Scripts/service-worker.js');
}


function urlBase64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
      .replace(/\-/g, '+')
      .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

