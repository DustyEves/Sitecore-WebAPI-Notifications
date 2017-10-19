function handlePushEvent(event) {
    const DEFAULT_TAG = 'web-push-book-example-site'
    return Promise.resolve()
    .then(() => {
        return event.data.json();
    })
    .then((data) => {

        const options = {
            body: data.body,
            icon: data.icon,
            badge: data.badge,
            tag: DEFAULT_TAG
        };
        return registration.showNotification(data.title, options);

    })
    .catch((err) => {
        console.error('Push event caused an error: ', err);

        const title = 'Message Received (TEXT)';
        console.log(event.data.text());
        var obj = JSON.parse(event.data.text());

        const options = {
            body: event.data.text(),
            tag: DEFAULT_TAG
        };
        return registration.showNotification(title, options);
    });
}

self.addEventListener('push', function (event) {
    event.waitUntil(handlePushEvent(event));
});

const doSomething = () => {
    return Promise.resolve();
};

// This is here just to highlight the simple version of notification click.
// Normally you would only have one notification click listener.
/**** START simpleNotification ****/
self.addEventListener('notificationclick', function (event) {
    const clickedNotification = event.notification;
    clickedNotification.close();

    // Do something as the result of the notification click
    const promiseChain = doSomething();
    event.waitUntil(promiseChain);
});
/**** END simpleNotification ****/