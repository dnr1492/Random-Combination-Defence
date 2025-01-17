using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    private Dictionary<int, PlayMapData> dicPlayMapDatas;
    
    [SerializeField] Text txtWave, txtWaveTimer, txtEnemyCount;
    [SerializeField] Text txtGold, txtDiamond, txtPopulation;
    [SerializeField] GameObject gameOverPrefab;

    private readonly string strWave = "Wave {0} / {1}";
    private readonly string strWaveTimer = "Next Wave : {0} Second";
    private readonly string strWaveSpawning = "적 생성 중";
    private readonly string strEnemyCount = "Enemy Count : {0} / {1}";
    private readonly string strPopulation = "{0} / {1}";

    private int curEnemyCount = 0;
    private int curGold = 5;
    private int curDiamond = 0;
    private int curPopulation = 0;

    public int GetCurMapId { get; private set; } = 10000;
    public int GetCurGold { get => curGold; private set => curGold = value; }
    public int GetCurDiamond { get => curDiamond; private set => curDiamond = value; }
    public int GetCurPopulation { get => curPopulation; private set => curPopulation = value; }

    private void Awake()
    {
        dicPlayMapDatas = DataManager.GetInstance().GetPlayMapData();
    }

    private void Start()
    {
        SetUI_Gold(0);
        SetUI_Diamond(0);
        SetUI_Population(true, true);
    }

    public void SetUI_Wave(int curWave, int maxWave)
    {
        if (curWave == 0) txtWave.text = "시작";
        else txtWave.text = string.Format(strWave, curWave, maxWave);
    }

    public void SetUI_WaveTimer(int curWaveTimer) => txtWaveTimer.text = string.Format(strWaveTimer, curWaveTimer);

    public void SetUI_WaveSpawning() => txtWaveTimer.text = strWaveSpawning;

    public void SetUI_EnemyCount(int enemyCount)
    {
        curEnemyCount += enemyCount;
        txtEnemyCount.text = string.Format(strEnemyCount, curEnemyCount, dicPlayMapDatas[GetCurMapId].maximumEnemyCount);
    }

    public void SetUI_Gold(int gold)
    {
        curGold += gold;
        txtGold.text = curGold.ToString();
    }

    public void SetUI_Diamond(int diamond)
    {
        curDiamond += diamond;
        txtDiamond.text = curDiamond.ToString();
    }

    public void SetUI_Population(bool isIncrease, bool isInit = false)
    {
        if (!isInit) {
            if (isIncrease) curPopulation++;
            else curPopulation--;
        }

        txtPopulation.text = string.Format(strPopulation, curPopulation, dicPlayMapDatas[GetCurMapId].maximumPopulation);
    }

    public void SetUI_GameOver(UnityAction restartGameAction)
    {
        if (curEnemyCount >= dicPlayMapDatas[GetCurMapId].maximumEnemyCount) {
            Instantiate(gameOverPrefab).GetComponent<GameOver>().Init(restartGameAction);
        }
    }
}