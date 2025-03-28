using UnityEditor;
using UnityEngine;
using System.IO;

public class LocalStorageEditor : EditorWindow
{
    private string infoName;
    private string infoFileName;

    [MenuItem("정재욱/Info 설정")]
    public static void ShowWindow()
    {
        LocalStorageEditor window = GetWindow<LocalStorageEditor>("Info 설정");
        window.LoadPrefs();
    }

    private void LoadPrefs()
    {
        infoName = EditorPrefs.GetString("InfoName", "");
        infoFileName = EditorPrefs.GetString("InfoFileName", "");
    }

    private void OnGUI()
    {
        GUILayout.Label("Info 설정", EditorStyles.boldLabel);

        infoName = EditorGUILayout.TextField("Info Name", infoName);
        infoFileName = EditorGUILayout.TextField("Info File Name", infoFileName);

        if (GUILayout.Button("저장")) Set();
        if (GUILayout.Button("삭제")) Delete();
    }

    private void Set()
    {
        EditorPrefs.SetString("InfoName", infoName);
        EditorPrefs.SetString("InfoFileName", infoFileName);
        Debug.Log($"Info 설정이 {infoName}와 {infoFileName}로 저장되었습니다.");
    }

    private void Delete()
    {
        string infoName = EditorPrefs.GetString("InfoName", "TestInfo");
        string infoFileName = EditorPrefs.GetString("InfoFileName", "test_info.json");
        string infoFileNamePath = Application.persistentDataPath + "/" + infoFileName;

        //PlayerPrefs
        if (PlayerPrefs.HasKey(infoName)) {
            PlayerPrefs.DeleteKey(infoName);
            Debug.LogFormat("PlayerPrefs에서 '{0}' Key가 삭제되었습니다.", infoName);
        }
        else Debug.LogFormat("PlayerPrefs에 '{0}' Key가 존재하지 않습니다.", infoName);

        //영구 데이터 경로 (Application.persistentDataPath)
        if (File.Exists(infoFileNamePath)) {
            File.Delete(infoFileNamePath);
            Debug.LogFormat("영구 데이터 경로에 있는 '{0}'를 삭제했습니다.", infoName);
            ShowExplorer(Application.persistentDataPath);
        }
        else {
            Debug.LogFormat("영구 데이터 경로에 있는 '{0}'를 찾을 수 없습니다.", infoName);
            ShowExplorer(Application.persistentDataPath);
        }

        //EditorPrefs에서 삭제
        EditorPrefs.DeleteKey("InfoName");
        EditorPrefs.DeleteKey("InfoFileName");
        Debug.Log("InfoName 및 InfoFileName 설정이 삭제되었습니다.");
    }

    private void ShowExplorer(string path)
    {
        path = path.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
}
