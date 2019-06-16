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

    private class WaveInformation
    {
        public List<MonsterList> monsterList;
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

    //关卡数据
    private Dictionary<string, List<WaveInformation>> levelData = new Dictionary<string, List<WaveInformation>>();
    //关卡时间队列
    private Queue<float> timeQueue = new Queue<float>();
    //怪物列表队列
    private Queue<MonsterList> monsterListQueue = new Queue<MonsterList>();
    //怪物生成队列
    private Queue<GameObject> monsterQueue = new Queue<GameObject>();
    

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
        levelData = JsonMapper.ToObject<Dictionary<string, List<WaveInformation>>>(jsonToString);
        
        if(levelData==null)
        {
            Logger.Log("没有获取到 关卡json 文件", LogType.Data);
        }
        else
        {
            Logger.Log("已获取到关卡json数据", LogType.Data);
        }
    }

    //遍历关卡数据，获取时间队列
    private void GetTimeQueue()
    {
        foreach(WaveInformation t in levelData["misson"])
        {
            timeQueue.Enqueue(t.time);
        }
        Logger.Log("成功获取时间队列", LogType.Monster);
    }

    //获取怪物列表队列
    private void GetMonsterListQueue()
    {
        foreach (WaveInformation t in levelData["misson"])
        {
            foreach (MonsterList k in t.monsterList)
            {
                monsterListQueue.Enqueue(k);
            }
        }

        Logger.Log("成功获取怪物列表队列", LogType.Monster);
    }

    //访问队列第一个列表，压入生成队列
    private void GetMonsterListToMonsterQueue()
    {
        if(monsterListQueue.Count==0)
        {
            Debug.Log("怪物列表队列已为空");
            return;
        }

        MonsterList nowMonsterList = monsterListQueue.Peek();
        Logger.Log("即将生成ID为" + nowMonsterList.monsterID + "的怪物，数量为" + nowMonsterList.count + "，出生点为" + nowMonsterList.wayID + "生成间隔" + nowMonsterList.rate + " s", LogType.Monster);

        for(int i=0;i<nowMonsterList.count;i++)
        {
            //需要一个通过ID获取怪物gameObject的方法
            //monsterQueue.Enqueue(?????(nowMonsterList.monsterID));
        }

    }

    //通过生成队列依次生成怪物
    private void CreateMonster()
    {
        float rate = monsterListQueue.Peek().rate;
        //生成队列未空一直生成，生成完了倒计时
        while(monsterQueue.Count>0)
        {
            Instantiate(monsterQueue.Dequeue());
            new WaitForSeconds(rate);
        }
    }


    //TODO
    float timer = 1.0f;
    public GameObject[] monsterPrefabs;
    public Transform Monsters;
    public Transform spawnPoint;
    private WayPointManager wayPointManager;

    private void Awake()
    {
        wayPointManager = GetComponent<WayPointManager>();
    }

    private void Start()
    {
        //test
        GetJsonToLevelData("Assets/Scripts/TestScripts_Zhidai/Mission.json");
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            Test();
            timer = 3.0f;
        }

    }



    private void Test()
    {
       var monster = Instantiate(monsterPrefabs[Random.Range(0,monsterPrefabs.Length)],spawnPoint.position,Quaternion.identity,Monsters);
        monster.GetComponent<BehaviorTree>().SetVariableValue("Road",wayPointManager.GetRoad(Random.Range(0,2)));
    }
}
