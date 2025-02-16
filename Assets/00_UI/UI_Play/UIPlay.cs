using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    [SerializeField] CharacterGenerator characterGenerator;
    [SerializeField] Text txtWave, txtWaveTimer, txtEnemyCount;
    [SerializeField] Text txtGold, txtDiamond;
    [SerializeField] Slider sliEnemyCount;
    [SerializeField] GameObject gameOverPrefab;

    private readonly string strWave = "Wave {0} / {1}";
    private readonly string strWaveTimer = "{0}";
    private readonly string strEnemyCount = "{0} / {1}";
    private readonly int maxEnemyCount = 70;

    private int curEnemyCount = 0;
    //private int curGold = 5;
    //private int curDiamond = 0;

    //public int GetCurGold { get => curGold; private set => curGold = value; }
    //public int GetCurDiamond { get => curDiamond; private set => curDiamond = value; }

    private void Awake()
    {
        sliEnemyCount.maxValue = maxEnemyCount;
    }

    //private void Start()
    //{
    //    SetUI_Gold(0);
    //    SetUI_Diamond(0);
    //}

    public void SetUI_Wave(int curWave, int maxWave)
    {
        if (curWave == 0) {
            txtWave.text = "½ÃÀÛ";
            characterGenerator.DrawRandom(5);
        }
        else {
            txtWave.text = string.Format(strWave, curWave, maxWave);
            characterGenerator.DrawRandom(2);
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
            Instantiate(gameOverPrefab).GetComponent<GameOver>().Init(restartGameAction);
        }
    }
}