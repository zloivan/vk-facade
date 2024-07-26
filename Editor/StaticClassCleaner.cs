using UnityEditor;
using UnityEngine;
using VKBridgeSDK.Runtime;

namespace VKBridgeSDK.Editor
{

    [InitializeOnLoad]
    public class StaticClassCleaner
    {
        static StaticClassCleaner()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Debug.Log("Reset called");
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                VkBridgeFacade.Reset();
            }
        }
    }

}