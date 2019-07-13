using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using BehaviorDesigner.Runtime;

public class MonsterSpawner : MonoBehaviour
{
    /*
    怪物生成器
    1.获取当前波数的json文件数据  ----完成
    2.遍历数据，加入到队列里，启动协程计数，时间到生成怪物
    具体：
    先获取每关的时间队列，获取每波的怪物列表队列                  ----
    游戏开始，启动时间队列里的第一个时间，开始生成怪物            ----
    生成怪物前访问列表队列第一个表，把目标ID的怪物数量压入生成队列，计时生成
    每次生成队列为空，把列表队列的第一个弹出等时间结束，继续下一个
    */


    /*
    ----------------------json数据包括以下内容----------------------
    包括攻击波数、每波持续时间
    攻击波数包括，怪物+个数，生成间隔，出生点
    每波持续时间，时间到下一波
    -----------json接受对象模型-----------
    波数信息
    */

    //波数信息
    private class WaveInformation
    {
        public List<MonsterList> monsterLists;
        public float time;
    }

    //怪物列表类
    private class MonsterList
    {
        public int monsterID;
        public int count;
        public int wayID;
        public float rate;
    }

    //关卡(波)列表队列
    private Queue<WaveInformation> waveQueue = new Queue<WaveInformation>();
    //怪物预制体列表
    public GameObject[] monsterPrefabs;
    //怪物列表父对象
    public Transform Monsters;
    //出生点
    public Transform spawnPoint;
    //是否已经生成完一波的所有怪物
    public bool alreadySpawnOneWave = true;

    private WayPointManager wayPointManager;


    //获取json文件，传入json文件路径
    private void GetJsonToLevelData(string path)
    {
        //如果文件不存在，则跳出该方法
        if(File.Exists(path)==false)
        {
            Logger.Log(path + "  文件不存在", LogType.Data);
            return;
        }

        //获取文件数据流读入
        StreamReader streamReader = new StreamReader(path);
        
        //获取json文件的string格式，可优化
        string jsonToString = streamReader.ReadToEnd();

        //获得json中的数据
        Dictionary<string, JsonData> levelData = JsonMapper.ToObject<Dictionary<string, JsonData>>(jsonToString);

        if (levelData==null)
        {
            Logger.Log("没有获取到 关卡json 文件", LogType.Data);
        }
        else
        {
            Logger.Log("已获取到关卡json数据", LogType.Data);
            //获取关卡队列
            var missions = levelData["mission"];
            for(int i = 0 ;i < missions.Count ;++i)
            {
                //获取某个关卡的怪物列表
                JsonData monsterLists = missions[i]["monsterList"];
                List<MonsterList> onelist = new List<MonsterList>();
                for (int j = 0; j< monsterLists.Count; ++j)
                {
                    MonsterList m = new MonsterList();
                    m.monsterID = (int)monsterLists[j]["monsterID"];
                    m.rate = (float)monsterLists[j]["rate"];
                    m.wayID = (int)monsterLists[j]["wayID"];
                    m.count = (int)monsterLists[j]["count"];

                    onelist.Add(m);
                }
                //生成某个关卡的数据
                WaveInformation waveInformation = new WaveInformation();
                waveInformation.time = (int)missions[i]["time"];
                waveInformation.monsterLists = onelist;

                waveQueue.Enqueue(waveInformation);
            }

            Logger.Log("成功获取关卡队列:" + waveQueue.Count, LogType.Monster);
        }
    }

    //生成一波怪物
    public void StartOneWave()
    {
        alreadySpawnOneWave = false;
        // 开启生成
        StartCoroutine(CreateMonster());
    }

    IEnumerator CreateMonster()
    {
        if (waveQueue.Count <= 0) yield break;

        WaveInformation waveInformation = waveQueue.Dequeue();
        //每一波生成前的等待时间
        yield return new WaitForSeconds(waveInformation.time);
        //每一个怪物列表
        foreach (var monsterList in waveInformation.monsterLists)
        {
            for (int j = 0; j < monsterList.count; ++j)
            {
                //生成怪物
                var monster = Instantiate(monsterPrefabs[monsterList.monsterID - 1], spawnPoint.position, Quaternion.identity, Monsters);
                //设置路径
                monster.GetComponent<BehaviorTree>().SetVariableValue("Road", wayPointManager.GetRoad(monsterList.wayID - 1));
                //生成间隔
                float rate = monsterList.rate;
                yield return new WaitForSeconds(rate);
            }
        }

        alreadySpawnOneWave = true;
    }

    //用于查询是否还有关卡
    public bool HasWave()
    {
        return waveQueue.Count > 0;
    }

    //用于查询某一关卡出现的怪物种类
    public List<int> GetMonsterIDsInOneWave()
    {
        List<int> IDList = new List<int>();
        WaveInformation waveInformation = waveQueue.Peek();
        //每一个怪物列表
        foreach (var monsterList in waveInformation.monsterLists)
        {
            int ID = monsterList.monsterID;

            if (!IDList.Contains(ID))
                IDList.Add(ID);
        }
        return IDList;
    }


    private void Awake()
    {
        wayPointManager = GetComponent<WayPointManager>();

        GetJsonToLevelData("Mission.json");
    }

    private void Start()
    {

    }

}
