using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using vk_facade.Runtime;

namespace vk_facade.Editor
{
  
    public static class DebugEnableInEditor
    {
        [MenuItem("Tools/VKBridge/Show Debug Menu")]
        public static void EnableDebugMenu()
        {
            if (Application.isPlaying)
            {
                VkBridgeFacade.ShowDebug();
            }
        }

        [MenuItem("Tools/VKBridge/Init")]
        public static void InitVk()
        {
            VkBridgeFacade.Initialize();
            VkBridgeFacade.VkBridgeInit().Forget();
        }
        
    }
}