enableLogging = false;
mergeInto(LibraryManager.library, {
    UnityVKBridge_SendMessage: function(methodNamePtr, paramsPtr) {
        var methodName = UTF8ToString(methodNamePtr);
        var params = UTF8ToString(paramsPtr);

        try {
            if (enableLogging) {console.log('Method name: ',methodName);}
            if (enableLogging) {console.log('Params before parse: ',params);}
            var parsedParams = params ? JSON.parse(params) : {};
            if (enableLogging) {console.log('Params after parse: ',parsedParams);}
            if (typeof vkBridge !== 'undefined') {
                vkBridge.send(methodName, parsedParams)
                    .then(function(data) {
                        if (enableLogging) {console.log('Got inside then: ',data);}
                        var dataStr = JSON.stringify({ method: methodName, data: data });
                        if (enableLogging) {console.log('Data after parse : ',dataStr);}
                        gameInstance.SendMessage('VKMessageReceiver', 'ReceivePromise', dataStr);
                        if (enableLogging) {console.log('Send message to unity...');}
                    })
                    .catch(function(error) {
                        if (enableLogging) {console.log('Got inside error: ',error);}
                        var errorStr = JSON.stringify({ method: methodName, error: error });
                        console.log('Error after parse: ',errorStr);
                        gameInstance.SendMessage('VKMessageReceiver', 'ReceiveError', errorStr);
                        if (enableLogging) {console.log('Send message to unity...');}
                    });
            } else {
                console.error('vkBridge is not defined. Make sure vkBridge is loaded.');
            }
        } catch (e) {
            console.error('JSON parse error:', e);
        }
    },

    UnityVKBridge_GetWindowLocationHref: function () {
        return allocate(intArrayFromString(window.location.href), 'i8', ALLOC_STACK);
    },
    
    UnityVKBridge_Subscribe: function() {
        if (typeof vkBridge !== 'undefined') {
            vkBridge.subscribe(function(event) {
                var eventStr = JSON.stringify(event);
                if (enableLogging) {console.log('SendMessage called... for ReceiveEvent');}
                gameInstance.SendMessage('VKMessageReceiver', 'ReceiveEvent', eventStr);
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
            gameInstance.SendMessage('VKMessageReceiver', 'OnFocus');
        });

        window.addEventListener('blur', function() {
            gameInstance.SendMessage('VKMessageReceiver', 'OnBlur');
        });

        document.addEventListener('visibilitychange', function() {
            if (document.hidden) {
                gameInstance.SendMessage('VKMessageReceiver', 'OnBlur');
            } else {
                gameInstance.SendMessage('VKMessageReceiver', 'OnFocus');
            }
        });
        
        if (typeof vkBridge !== 'undefined') {
            vkBridge.subscribe(function(event) {
                const { type } = event.detail;
                if (type === 'VKWebAppViewHide') {
                    gameInstance.SendMessage('VKMessageReceiver', 'OnBlur');
                } else if (type === 'VKWebAppViewRestore') {
                    gameInstance.SendMessage('VKMessageReceiver', 'OnFocus');
                }
            });
        }
    },

    UnityVKBridge_SetLogging: function (value) {
        enableLogging = value !== 0;
        console.log('Logging enabled: ' + enableLogging);
    },
});
