using System.IO;
using UnityEditor;

namespace VKBridgeSDK.Editor
{
    [InitializeOnLoad]
    public static class WebGLTemplateInstaller
    {
        static WebGLTemplateInstaller()
        {
            const string sourcePath = "Assets/VK2/WebGLTemplates";
            const string targetPath = "Assets/WebGLTemplates";

            if (Directory.Exists(sourcePath))
            {
                CopyTemplates(sourcePath, targetPath);
                AssetDatabase.Refresh();
            }
        }

        static void CopyTemplates(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var targetFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, targetFile, true);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                var targetSubDir = Path.Combine(targetDir, Path.GetFileName(directory));
                CopyTemplates(directory, targetSubDir);
            }
        }
    }
}