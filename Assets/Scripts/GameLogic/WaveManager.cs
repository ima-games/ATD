using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 关卡管理
/// </summary>
public class WaveManager : MonoBehaviour
{
    //关卡开始UI提示
    public Animation waveStartPanel;
    public Text waveStartText;
    //关卡结束UI提示
    public Animation waveEndPanel;
    public Text waveEndText;
    //怪物生成器
    public MonsterSpawner monsterSpawner;

    public Animation endPanel;
    public Animation losePanel;

    //金钱管理器
    private MoneyManager moneyManager;
    //是否已经开始一波
    private bool alreadyStart = false;

    private int waveIndex = 1;

    private void StartOneWave()
    {
        alreadyStart = true;
        var monsterIDList = monsterSpawner.GetMonsterIDsInOneWave();
        monsterSpawner.StartOneWave();
        waveStartText.text = "第"+ waveIndex + "波";
        waveStartPanel.Play();
    }

    private void EndOneWave()
    {
        alreadyStart = false;
        //关卡奖励
        moneyManager.AddCash(100);
        waveEndPanel.Play();
        waveEndText.text = "第" + waveIndex + "波";

        //没有关卡则退出
        if (!monsterSpawner.HasWave())
        {
            StartCoroutine(EndAllWaves());
            return;
        }


        waveIndex++;
        StartCoroutine(StartOneWaveAfterSeconds());
    }

    IEnumerator StartOneWaveAfterSeconds()
    {
        yield return new WaitForSeconds(8.0f);
        StartOneWave();
    }

    //所有波都防守成功后，提示胜利
    IEnumerator EndAllWaves()
    {
        yield return new WaitForSeconds(5.0f);
        endPanel.Play();
    }

    private void Awake()
    {
        moneyManager = GetComponent<MoneyManager>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartOneWave();
    }

    //TODO FLAG
    bool alreadyLose = false;
    // Update is called once per frame
    void Update()
    {
        //若玩家基地死了，则提示失败
        if ((!Factory.PlayerIndividual.enabled || !Factory.BaseIndividual || !Factory.BaseIndividual.enabled) && !alreadyLose)
        {
            losePanel.Play();
            alreadyLose = true;
        }
        //若无怪物，则证明完成一波
        if (alreadyStart && monsterSpawner.alreadySpawnOneWave && !Factory.HasMonsterIndividual())
        {
            EndOneWave();
        }
    }
}
