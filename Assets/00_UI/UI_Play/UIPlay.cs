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

    [SerializeField] Text txtWave, txtWaveTimer;
    [SerializeField] Text txtGold, txtDarkGold, txtPopulation;

    private readonly string strWave = "{0} Wave";
    private readonly string strWaveTimer = "Next Wave : {0} Second";
    private readonly string strWaveSpawning = "적 생성 중";
    private readonly string strPopulation = "{0}/{1}";

    //private int curGold = 0;
    //private int curDarkGold = 0;
    //private int curPopulation = 0;
    //private int curKillCount = 0;

    //// ===== 적 제거에 맞게 획득 로직 구현 ===== //
    //private void GainGold()
    //{
    //    curGold += gainGold;
    //    Set_UI_Gold();
    //}

    //// ===== 적 제거에 맞게 획득 로직 구현 ===== //
    //private void GainDarkGold()
    //{
    //    curDarkGold += gainDarkGold;
    //    Set_UI_DarkGold();
    //}

    //// ===== 적 제거에 맞게 획득 로직 구현 ===== //
    //private void GainPopulation()
    //{
    //    curPopulation += 1;
    //    Set_UI_Population();
    //}

    //// ===== 추후 데이터 값을 매개변수로 전달하는 로직으로 변경 요망 ===== //
    //private void Set_UI_Gold() => txtGold.text = curGold.ToString();

    //private void Set_UI_DarkGold() => txtDarkGold.text = curDarkGold.ToString();

    //private void Set_UI_Population() => txtPopulation.text = string.Format(strPopulation, curPopulation, maximumPopulation);

    public void SetUI_Wave(int curWave = 1) => txtWave.text = string.Format(strWave, curWave + 1);

    public void SetUI_WaveTimer(int curWaveTimer) => txtWaveTimer.text = string.Format(strWaveTimer, curWaveTimer);

    public void SetUI_WaveSpawning() => txtWaveTimer.text = strWaveSpawning;
}