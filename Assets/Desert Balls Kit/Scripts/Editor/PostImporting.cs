using UnityEditor;
using System.IO;

public class PostImporting : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        string defines="";

        if (Directory.Exists("Assets/UnityAds")) // check if there is an advertising folder
        {
            defines += "; UNITY_ADS";
        }
        if (Directory.Exists("Assets/Plugins/UnityPurchasing")) // check if there is a folder with IAP
        {
            defines += "; UNITY_INAPPS";
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defines);
    }
}