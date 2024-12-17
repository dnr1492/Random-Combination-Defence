using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class BuildSettingsSync
{
    private const string SourcePath = "ProjectSettings/EditorUserBuildSettings.asset";
    private const string TargetPath = "Library/EditorUserBuildSettings.asset";

    static BuildSettingsSync()
    {
        if (File.Exists(SourcePath)) File.Copy(SourcePath, TargetPath, true);
    }
}