using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class BuildSettingsSync
{
    private const string SourcePath = "Library/EditorUserBuildSettings.asset";
    private const string TargetPath = "ProjectSettings/EditorUserBuildSettings.asset";

    static BuildSettingsSync()
    {
        if (File.Exists(SourcePath)) File.Copy(SourcePath, TargetPath, true);
    }
}