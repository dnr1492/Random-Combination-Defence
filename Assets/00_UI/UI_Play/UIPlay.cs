using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    [SerializeField] CharacterGenerator characterGenerator;

    [Header("Wave")]
    [SerializeField] Text txtWave, txtWaveTimer, txtEnemyCount;
    private readonly string strWave = "Wave {0} / {1}";
    private readonly string strWaveTimer = "{0}";
    [SerializeField] Slider sliEnemyCount;
    private readonly string strEnemyCount = "{0} / {1}";
    private readonly int maxEnemyCount = 70;
    private int curEnemyCount = 0;

    [Header("Resources")]
    [SerializeField] Text txtGold, txtDiamond;
    //private int curGold = 5;
    //private int curDiamond = 0;
    //public int GetCurGold { get => curGold; private set => curGold = value; }
    //public int GetCurDiamond { get => curDiamond; private set => curDiamond = value; }

    [Header("GameOver")]
    [SerializeField] GameObject gameOverPrefab;

    [Header("Fast Forward")]
    [SerializeField] Button btnChangeFF;
    [SerializeField] CustomBackground cbChangeFF;
    private int curNextFFIndex = 0;

    private void Awake()
    {
        sliEnemyCount.maxValue = maxEnemyCount;

        SetUI_FastForward(curNextFFIndex);
        btnChangeFF.onClick.AddListener(() => {
            SetUI_FastForward(curNextFFIndex);
        });
    }

    //private void Start()
    //{
    //    SetUI_Gold(0);
    //    SetUI_Diamond(0);
    //}

    public void SetUI_Wave(int curWave, int maxWave)
    {
        if (curWave == 0) {
            txtWave.text = "시작";
            StartCoroutine(characterGenerator.DrawRandom(5, 0.25f));
        }
        else {
            txtWave.text = string.Format(strWave, curWave, maxWave);
            StartCoroutine(characterGenerator.DrawRandom(2, 0));
        }
    }

    public void SetUI_WaveTimer(float curWaveTimer) => txtWaveTimer.text = string.Format(strWaveTimer, curWaveTimer.ToString("F2"));

    public void SetUI_EnemyCount()
    {
        curEnemyCount = EnemyGenerator.ExistingEnemys.Count;
        txtEnemyCount.text = string.Format(strEnemyCount, curEnemyCount, maxEnemyCount);
        sliEnemyCount.value = curEnemyCount;
    }

    //public void SetUI_Gold(int gold)
    //{
    //    curGold += gold;
    //    txtGold.text = curGold.ToString();
    //}

    //public void SetUI_Diamond(int diamond)
    //{
    //    curDiamond += diamond;
    //    txtDiamond.text = curDiamond.ToString();
    //}

    public void SetUI_GameOver(UnityAction restartGameAction)
    {
        if (curEnemyCount >= maxEnemyCount) {
            Instantiate(gameOverPrefab).GetComponent<GameOver>().Init(restartGameAction, SetUI_FastForward);
        }
    }

    public void SetUI_FastForward(int index)
    {
        switch (index)
        {
            case (int)FastForward.X1:
                Time.timeScale = 1;
                break;
            case (int)FastForward.X2:
                Time.timeScale = 2;
                break;
            case (int)FastForward.X3:
                Time.timeScale = 3;
                break;
            default:
                break;
        }

        curNextFFIndex = (index + 1) % Enum.GetValues(typeof(FastForward)).Length;  //0 → 1 → 2순환 변경
        cbChangeFF.SetSelect(index);
    }
}