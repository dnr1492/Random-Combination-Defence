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
        btnGameStart.onClick.AddListener(() => {
            //StartCoroutine(LoadScene("PlayScene_Iso"));
            StartCoroutine(LoadingManager.LoadScene("PlayScene_Test"));
        });
    }
}