using UnityEditor;
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
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                VkBridgeFacade.Reset();
            }
        }
    }
}