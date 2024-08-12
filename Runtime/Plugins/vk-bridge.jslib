enableLogging = false;
mergeInto(LibraryManager.library, {
    UnityVKBridge_SendMessage: function(methodNamePtr, requestIdPtr, paramsPtr) {
        var methodName = UTF8ToString(methodNamePtr);
        var requestId = UTF8ToString(requestIdPtr);
        var params = UTF8ToString(paramsPtr);

        try {
            if (enableLogging) {console.log('JS:::Method name: ',methodName);}
            if (enableLogging) {console.log('JS:::Request ID: ', requestId);}
            if (enableLogging) {console.log('JS:::Params before parse: ',params);}
            var parsedParams = params ? JSON.parse(params) : {};
            if (enableLogging) {console.log('JS:::Params after parse: ',parsedParams);}
            if (typeof vkBridge !== 'undefined') {
                vkBridge.send(methodName, parsedParams)
                    .then(function(data) {
                        if (enableLogging) {console.log('JS:::Got inside then: ',data);}
                        var dataStr = JSON.stringify({ requestId: requestId, method: methodName, data: data });
                        if (enableLogging) {console.log('JS:::Data after parse : ',dataStr);}
                        gameInstance.SendMessage('VKMessageReceiver', 'ReceivePromise', dataStr);
                        if (enableLogging) {console.log('JS:::Send message to unity...');}
                    })
                    .catch(function(error) {
                        if (enableLogging) {console.log('JS:::Got inside error: ',error);}
                        var errorStr = JSON.stringify({ requestId: requestId, method: methodName, error: error });
                        if (enableLogging) {console.log('JS:::Error after parse: ',errorStr); }
                        gameInstance.SendMessage('VKMessageReceiver', 'ReceiveError', errorStr);
                        if (enableLogging) {console.log('JS:::JS:::Send message to unity...');}
                    });
            } else {
                console.error('JS:::vkBridge is not defined. Make sure vkBridge is loaded.');
            }
        } catch (e) {
            console.error('JS:::JSON parse error:', e);
        }
    },
    
    UnityVKBridge_GetWindowLocationHref: function () {
        var url = window.location.href;
        var lengthBytes = lengthBytesUTF8(url) + 1; // Determine the length of the string in bytes, including null terminator
        var stringOnStack = stackAlloc(lengthBytes); // Allocate memory on the stack
        stringToUTF8(url, stringOnStack, lengthBytes); // Write the string to the allocated memory
        return stringOnStack;
    },
    
    UnityVKBridge_Subscribe: function() {
        if (typeof vkBridge !== 'undefined') {
            vkBridge.subscribe(function(event) {
                var eventStr = JSON.stringify(event);
                if (enableLogging) {console.log('JS:::SendMessage called... for ReceiveEvent');}
                gameInstance.SendMessage('VKMessageReceiver', 'ReceiveEvent', eventStr);
            });
        } else {
            console.error('JS:::vkBridge is not defined. Make sure vkBridge is loaded.');
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
                if (event.detail.type === 'VKWebAppViewHide') {
                    gameInstance.SendMessage('VKMessageReceiver', 'OnBlur');
                }
            });

            vkBridge.subscribe(function(event) {
                if (event.detail.type === 'VKWebAppViewRestore') {
                    gameInstance.SendMessage('VKMessageReceiver', 'OnFocus');
                }
            });
        } else {
            console.error('JS:::vkBridge is not defined. Make sure vkBridge is loaded.');
        }
    },

    UnityVKBridge_SetLogging: function (value) {
        enableLogging = value !== 0;
        console.log('JS:::Logging enabled: ' + enableLogging);
    },
});
