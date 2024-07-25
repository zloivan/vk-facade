mergeInto(LibraryManager.library, {
    UnityVKBridge_SendMessage: function(methodNamePtr, paramsPtr) {
        var methodName = UTF8ToString(methodNamePtr);
        var params = UTF8ToString(paramsPtr);

        if (typeof vkBridge !== 'undefined') {
            vkBridge.send(methodName, JSON.parse(params))
                .then(function(data) {
                    var dataStr = JSON.stringify({ method: methodName, data: data });
                    unityInstance.SendMessage('MessageReceiver', 'ReceivePromise', dataStr);
                })
                .catch(function(error) {
                    var errorStr = JSON.stringify({ method: methodName, error: error });
                    unityInstance.SendMessage('MessageReceiver', 'ReceiveError', errorStr);
                });
        } else {
            console.error('vkBridge is not defined. Make sure vkBridge is loaded.');
        }
    },

    UnityVKBridge_Subscribe: function() {
        if (typeof vkBridge !== 'undefined') {
            vkBridge.subscribe(function(event) {
                var eventStr = JSON.stringify(event);
                unityInstance.SendMessage('MessageReceiver', 'ReceiveEvent', eventStr);
            });
        } else {
            console.error('vkBridge is not defined. Make sure vkBridge is loaded.');
        }
    },

    UnityVKBridge_Alert: function(messagePtr) {
        var message = UTF8ToString(messagePtr);
        alert(message);
    },

    UnityVKBridge_SetupFocusHandlers: function() {
        window.addEventListener('focus', function() {
            unityInstance.SendMessage('MessageReceiver', 'OnFocus');
        });

        window.addEventListener('blur', function() {
            unityInstance.SendMessage('MessageReceiver', 'OnBlur');
        });
    }
});
