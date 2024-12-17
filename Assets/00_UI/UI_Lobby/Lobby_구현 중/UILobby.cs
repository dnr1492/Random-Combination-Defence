using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] Button btnGameStart;

    private void Awake()
    {
        DataManager.GetInstance().LoadCharacterCardLevelData();
        DataManager.GetInstance().LoadCharacterCardLevelInfoData();
        DataManager.GetInstance().LoadCharacterData();
        DataManager.GetInstance().LoadCharacterSkillData();
        DataManager.GetInstance().LoadCharacterRecipeData();
        DataManager.GetInstance().LoadPlayWaveData();
        DataManager.GetInstance().LoadPlayMapData();
        DataManager.GetInstance().LoadPlayEnemyData();
        PlayFabManager.instance.InitUserData("Characters");

        btnGameStart.onClick.AddListener(() => {
            //StartCoroutine(LoadScene("PlayScene"));
            StartCoroutine(LoadScene("PlayScene_Iso"));
        });
    }

    #region ¾À ·Îµå - µ¿±â
    private IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            asyncOperation.allowSceneActivation = true;
            yield return Time.deltaTime;
        }
    }
    #endregion
}