using UnityEditor;
using UnityEngine;
using vk_facade.Runtime;

namespace vk_facade.Editor
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
                VkBridgeFacade.ResetStaticFields();
            }
        }
    }

}