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
    [SerializeField] Slider sliEnemyCount;
    [SerializeField] GameObject gameOverPrefab;

    private readonly string strWave = "Wave {0} / {1}";
    private readonly string strWaveTimer = "{0}";
    private readonly string strWaveSpawning = "생성 중";
    private readonly string strEnemyCount = "{0} / {1}";
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

        sliEnemyCount.maxValue = dicPlayMapDatas[GetCurMapId].maximumEnemyCount;
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

    public void SetUI_WaveTimer(float curWaveTimer) => txtWaveTimer.text = string.Format(strWaveTimer, curWaveTimer.ToString("F2"));

    public void SetUI_WaveSpawning() => txtWaveTimer.text = strWaveSpawning;

    public void SetUI_EnemyCount()
    {
        curEnemyCount = EnemyGenerator.ExistingEnemys.Count;
        txtEnemyCount.text = string.Format(strEnemyCount, curEnemyCount, dicPlayMapDatas[GetCurMapId].maximumEnemyCount);
        sliEnemyCount.value = curEnemyCount;
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