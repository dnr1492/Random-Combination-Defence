using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    // ===== 추후에 엑셀로 게임 플레이 데이터 (총 웨이브, 웨이브 간 타이머, 획득 골드 등...) 정리하기 ===== //
    //private readonly int maximumWave = 4;  //최대 웨이브
    //private readonly float waveTimer = 10;  //다음 웨이브 대기 시간
    //private readonly int waveEnemyCount = 10;  //웨이브 적 수
    //private readonly int gainGold = 1;  //획득 골드량
    private readonly int gainGoldPerKill = 1;  //킬당 골드 획득 조건
    //private readonly int gainDarkGold = 1;  //획득 어둠의 골드량
    private readonly int gainDarkGoldPerKill = 10;  //킬당 어둠의 골드 획득 조건
    //private readonly int maximumPopulation = 50;  //최대 인구수
    // ==================================================================================================== //

    private Dictionary<int, PlayMapData> dicPlayMapDatas;
    
    [SerializeField] Text txtWave, txtWaveTimer;
    [SerializeField] Text txtGold, txtDarkGold, txtPopulation;

    private readonly string strWave = "{0} Wave";
    private readonly string strWaveTimer = "Next Wave : {0} Second";
    private readonly string strWaveSpawning = "적 생성 중";
    private readonly string strPopulation = "{0}/{1}";

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
}