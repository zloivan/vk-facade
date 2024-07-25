using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResponseManager
{
    private Dictionary<string, UniTaskCompletionSource<VKData>> _promiseTasks = new Dictionary<string, UniTaskCompletionSource<VKData>>();

    [DllImport("__Internal")]
    private static extern void UnityVKBridge_SendMessage(string methodName, string parameters);

    [DllImport("__Internal")]
    private static extern void UnityVKBridge_Subscribe();

    [DllImport("__Internal")]
    private static extern void UnityVKBridge_Alert(string message);

    [DllImport("__Internal")]
    private static extern void UnityVKBridge_SetupFocusHandlers();

    public ResponseManager()
    {
        UnityVKBridge_SetupFocusHandlers();
        UnityVKBridge_Subscribe();
    }

    public async UniTask<VKData> CallVkMethodAsync(string methodName, params object[] parameters)
    {
        var taskCompletionSource = new UniTaskCompletionSource<VKData>();
        _promiseTasks[methodName] = taskCompletionSource;

        var paramsString = parameters.Length > 0 ? JsonUtility.ToJson(parameters) : string.Empty;
        UnityVKBridge_SendMessage(methodName, paramsString);

        return await taskCompletionSource.Task;
    }

    public void HandlePromiseResponse(string jsonData)
    {
        var response = JsonUtility.FromJson<VKData>(jsonData);
        
        if (!_promiseTasks.TryGetValue(response.method, out var tcs)) 
            return;
        
        tcs.TrySetResult(response);
        _promiseTasks.Remove(response.method);
    }

    public void HandleErrorResponse(string jsonData)
    {
        var response = JsonUtility.FromJson<VKData>(jsonData);
        if (!_promiseTasks.TryGetValue(response.method, out var tcs)) 
            return;
        
        tcs.TrySetException(new Exception(response.error));
        _promiseTasks.Remove(response.method);
    }

    public void ShowAlert(string message)
    {
        UnityVKBridge_Alert(message);
    }
}