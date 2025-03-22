using UnityEngine;
using UnityEditor;
using System.IO;

public class MergeTextureEditor : EditorWindow
{
    private Texture2D baseTexture;
    private Texture2D overlayTexture;
    private string saveFileName;

    [MenuItem("정재욱/Merge Texture")]
    public static void ShowWindow()
    {
        GetWindow<MergeTextureEditor>("Merge Texture");
    }

    private void OnGUI()
    {
        GUILayout.Label("Merge Two Textures", EditorStyles.boldLabel);

        baseTexture = (Texture2D)EditorGUILayout.ObjectField("Base Texture", baseTexture, typeof(Texture2D), false);
        overlayTexture = (Texture2D)EditorGUILayout.ObjectField("Overlay Texture", overlayTexture, typeof(Texture2D), false);
        saveFileName = EditorGUILayout.TextField("Save File Name", saveFileName);

        if (GUILayout.Button("Merge & Save"))
        {
            if (baseTexture != null && overlayTexture != null)
            {
                MergeAndSave();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Please assign both textures!");
            }
        }
    }

    private void MergeAndSave()
    {
        string basePath = AssetDatabase.GetAssetPath(baseTexture);
        string overlayPath = AssetDatabase.GetAssetPath(overlayTexture);

        bool baseWasReadable = SetTextureReadable(basePath, true);
        bool overlayWasReadable = SetTextureReadable(overlayPath, true);

        Texture2D merged = MergeTextures(baseTexture, overlayTexture);
        if (merged != null)
        {
            SaveTextureAsPNG(merged, saveFileName);
        }

        //원래 설정으로 복원
        SetTextureReadable(basePath, baseWasReadable);
        SetTextureReadable(overlayPath, overlayWasReadable);
    }

    private bool SetTextureReadable(string assetPath, bool enable)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            bool wasReadable = importer.isReadable;
            if (wasReadable != enable)
            {
                importer.isReadable = enable;
                importer.SaveAndReimport();
            }
            return wasReadable;
        }
        return false;
    }

    private Texture2D MergeTextures(Texture2D baseTex, Texture2D overlayTex)
    {
        if (baseTex.width != overlayTex.width || baseTex.height != overlayTex.height)
        {
            Debug.LogError("Textures must be the same size!");
            return null;
        }

        Texture2D result = new Texture2D(baseTex.width, baseTex.height);
        for (int y = 0; y < baseTex.height; y++)
        {
            for (int x = 0; x < baseTex.width; x++)
            {
                Color baseColor = baseTex.GetPixel(x, y);
                Color overlayColor = overlayTex.GetPixel(x, y);
                Color finalColor = Color.Lerp(baseColor, overlayColor, overlayColor.a);
                result.SetPixel(x, y, finalColor);
            }
        }

        result.Apply();
        return result;
    }

    private void SaveTextureAsPNG(Texture2D texture, string filename)
    {
        byte[] bytes = texture.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, filename);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Texture saved at: " + path);
    }
}
