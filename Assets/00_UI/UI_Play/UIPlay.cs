using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    // ===== ���Ŀ� ������ ���� �÷��� ������ (�� ���̺�, ���̺� �� Ÿ�̸�, ȹ�� ��� ��...) �����ϱ� ===== //
    //private readonly int maximumWave = 4;  //�ִ� ���̺�
    //private readonly float waveTimer = 10;  //���� ���̺� ��� �ð�
    //private readonly int waveEnemyCount = 10;  //���̺� �� ��
    //private readonly int gainGold = 1;  //ȹ�� ��差
    private readonly int gainGoldPerKill = 1;  //ų�� ��� ȹ�� ����
    //private readonly int gainDarkGold = 1;  //ȹ�� ����� ��差
    private readonly int gainDarkGoldPerKill = 10;  //ų�� ����� ��� ȹ�� ����
    //private readonly int maximumPopulation = 50;  //�ִ� �α���
    // ==================================================================================================== //

    [SerializeField] Text txtWave, txtWaveTimer;
    [SerializeField] Text txtGold, txtDarkGold, txtPopulation;

    private readonly string strWave = "{0} Wave";
    private readonly string strWaveTimer = "Next Wave : {0} Second";
    private readonly string strWaveSpawning = "�� ���� ��";
    private readonly string strPopulation = "{0}/{1}";

    //private int curGold = 0;
    //private int curDarkGold = 0;
    //private int curPopulation = 0;
    //private int curKillCount = 0;

    //// ===== �� ���ſ� �°� ȹ�� ���� ���� ===== //
    //private void GainGold()
    //{
    //    curGold += gainGold;
    //    Set_UI_Gold();
    //}

    //// ===== �� ���ſ� �°� ȹ�� ���� ���� ===== //
    //private void GainDarkGold()
    //{
    //    curDarkGold += gainDarkGold;
    //    Set_UI_DarkGold();
    //}

    //// ===== �� ���ſ� �°� ȹ�� ���� ���� ===== //
    //private void GainPopulation()
    //{
    //    curPopulation += 1;
    //    Set_UI_Population();
    //}

    //// ===== ���� ������ ���� �Ű������� �����ϴ� �������� ���� ��� ===== //
    //private void Set_UI_Gold() => txtGold.text = curGold.ToString();

    //private void Set_UI_DarkGold() => txtDarkGold.text = curDarkGold.ToString();

    //private void Set_UI_Population() => txtPopulation.text = string.Format(strPopulation, curPopulation, maximumPopulation);

    public void SetUI_Wave(int curWave = 1) => txtWave.text = string.Format(strWave, curWave + 1);

    public void SetUI_WaveTimer(int curWaveTimer) => txtWaveTimer.text = string.Format(strWaveTimer, curWaveTimer);

    public void SetUI_WaveSpawning() => txtWaveTimer.text = strWaveSpawning;
}