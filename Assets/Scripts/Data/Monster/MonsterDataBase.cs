using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataBase : MonoBehaviour
{
    //读取excel插件生成的json文件
    public TextAsset MonsterDataJ;


    private List<MonsterData> monsterDatas;
    private Dictionary<int, MonsterData> monsterDataDictionary = new Dictionary<int, MonsterData>();
    private static MonsterDataBase instance;


    //禁止外界通过new获取该类的实例
    private MonsterDataBase() { }

    /// <summary>
    /// MonsterDataBased的单例模式
    /// </summary>
    public static MonsterDataBase Instance
    {
        get { return instance; }
        private set { }
    }

    void Awake()
    {
        instance = this;
        //用列表读取MonsterData的数据
        monsterDatas = JsonToObject.JsonToObject_ByJsonContent<MonsterData>(MonsterDataJ.text);
        //列表数据读入字典
        foreach (var a in monsterDatas)
        {
            monsterDataDictionary.Add(a.ID, a);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// 根据ID获取相应的MonsterData对象的方法
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public MonsterData GetMonsterData(int ID)
    {
        if (!monsterDataDictionary.ContainsKey(ID))
        {
            Debug.Log("monsterData中不存在" + ID);
            return null;
        }
        return monsterDataDictionary[ID];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
//该脚本必须挂在一个GameObject上
//用例：MonsterData a = MonsterDataBase.Instance.GetMonsterData(0);
