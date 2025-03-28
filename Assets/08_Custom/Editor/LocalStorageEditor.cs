using UnityEditor;
using UnityEngine;
using System.IO;

public class LocalStorageEditor : EditorWindow
{
    private string infoName;
    private string infoFileName;

    [MenuItem("�����/Info ����")]
    public static void ShowWindow()
    {
        LocalStorageEditor window = GetWindow<LocalStorageEditor>("Info ����");
        window.LoadPrefs();
    }

    private void LoadPrefs()
    {
        infoName = EditorPrefs.GetString("InfoName", "");
        infoFileName = EditorPrefs.GetString("InfoFileName", "");
    }

    private void OnGUI()
    {
        GUILayout.Label("Info ����", EditorStyles.boldLabel);

        infoName = EditorGUILayout.TextField("Info Name", infoName);
        infoFileName = EditorGUILayout.TextField("Info File Name", infoFileName);

        if (GUILayout.Button("����")) Set();
        if (GUILayout.Button("����")) Delete();
    }

    private void Set()
    {
        EditorPrefs.SetString("InfoName", infoName);
        EditorPrefs.SetString("InfoFileName", infoFileName);
        Debug.Log($"Info ������ {infoName}�� {infoFileName}�� ����Ǿ����ϴ�.");
    }

    private void Delete()
    {
        string infoName = EditorPrefs.GetString("InfoName", "TestInfo");
        string infoFileName = EditorPrefs.GetString("InfoFileName", "test_info.json");
        string infoFileNamePath = Application.persistentDataPath + "/" + infoFileName;

        //PlayerPrefs
        if (PlayerPrefs.HasKey(infoName)) {
            PlayerPrefs.DeleteKey(infoName);
            Debug.LogFormat("PlayerPrefs���� '{0}' Key�� �����Ǿ����ϴ�.", infoName);
        }
        else Debug.LogFormat("PlayerPrefs�� '{0}' Key�� �������� �ʽ��ϴ�.", infoName);

        //���� ������ ��� (Application.persistentDataPath)
        if (File.Exists(infoFileNamePath)) {
            File.Delete(infoFileNamePath);
            Debug.LogFormat("���� ������ ��ο� �ִ� '{0}'�� �����߽��ϴ�.", infoName);
            ShowExplorer(Application.persistentDataPath);
        }
        else {
            Debug.LogFormat("���� ������ ��ο� �ִ� '{0}'�� ã�� �� �����ϴ�.", infoName);
            ShowExplorer(Application.persistentDataPath);
        }

        //EditorPrefs���� ����
        EditorPrefs.DeleteKey("InfoName");
        EditorPrefs.DeleteKey("InfoFileName");
        Debug.Log("InfoName �� InfoFileName ������ �����Ǿ����ϴ�.");
    }

    private void ShowExplorer(string path)
    {
        path = path.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
}
