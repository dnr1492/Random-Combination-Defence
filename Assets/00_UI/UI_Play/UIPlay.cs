using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    private Dictionary<int, PlayMapData> dicPlayMapDatas;
    
    [SerializeField] Text txtWave, txtWaveTimer, txtEnemyCount;
    [SerializeField] Text txtGold, txtDarkGold, txtPopulation;
    [SerializeField] GameObject gameOverPrefab;

    private readonly string strWave = "{0} Wave";
    private readonly string strWaveTimer = "Next Wave : {0} Second";
    private readonly string strWaveSpawning = "적 생성 중";
    private readonly string strEnemyCount = "{0} <- 패배 \n Enemy Count : {1}";
    private readonly string strPopulation = "{0}/{1}";

    private int curEnemyCount = 0;
    private int curGold = 5;
    private int curDarkGold = 0;
    private int curPopulation = 0;

    public int GetCurMapId { get; private set; } = 10000;
    public int GetCurGold { get => curGold; private set => curGold = value; }
    public int GetCurDarkGold { get => curDarkGold; private set => curDarkGold = value; }
    public int GetCurPopulation { get => curPopulation; private set => curPopulation = value; }

    private void Awake()
    {
        dicPlayMapDatas = DataManager.instance.GetPlayMapData();
    }

    private void Start()
    {
        SetUI_Gold(0);
        SetUI_DarkGold(0);
        SetUI_Population(0);
    }

    public void SetUI_Wave(int curWave)
    {
        if (curWave == 0) txtWave.text = "시작";
        else txtWave.text = string.Format(strWave, curWave);
    }

    public void SetUI_WaveTimer(int curWaveTimer) => txtWaveTimer.text = string.Format(strWaveTimer, curWaveTimer);

    public void SetUI_WaveSpawning() => txtWaveTimer.text = strWaveSpawning;

    public void SetUI_EnemyCount(int enemyCount)
    {
        curEnemyCount += enemyCount;
        txtEnemyCount.text = string.Format(strEnemyCount, dicPlayMapDatas[GetCurMapId].maximum_enemy_count, curEnemyCount);

        UI_GameOver();
    }

    public void SetUI_Gold(int gold)
    {
        curGold += gold;
        txtGold.text = curGold.ToString();
    }

    public void SetUI_DarkGold(int darkGold)
    {
        curDarkGold += darkGold;
        txtDarkGold.text = curDarkGold.ToString();
    }

    public void SetUI_Population(int population)
    {
        curPopulation += population;
        txtPopulation.text = string.Format(strPopulation, curPopulation, dicPlayMapDatas[GetCurMapId].maximum_population);
    }

    // ==================== 게임 오버 로직 - 게임 오버, 이어 하기 (광고), 메인 메뉴로 돌아가기 ========================= //
    // ==================== 게임 오버 로직 - 게임 오버, 이어 하기 (광고), 메인 메뉴로 돌아가기 ========================= //
    // ==================== 게임 오버 로직 - 게임 오버, 이어 하기 (광고), 메인 메뉴로 돌아가기 ========================= //

    private void UI_GameOver()
    {
        if (curEnemyCount >= dicPlayMapDatas[GetCurMapId].maximum_enemy_count)
        {
            Time.timeScale = 0;
            Instantiate(gameOverPrefab).GetComponent<GameOver>().Init(this);
        }
    }
}