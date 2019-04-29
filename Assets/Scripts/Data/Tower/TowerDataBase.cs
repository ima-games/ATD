using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataBase : MonoBehaviour
{
    public TextAsset towerJ;

    private List<TowerTrapData> towerDatas;
    private Dictionary<int, TowerTrapData> towerDataDictionary = new Dictionary<int, TowerTrapData>();
    private static TowerDataBase instance;

    //禁止外界通过new获取该类的实例
    private TowerDataBase() { }

    /// <summary>
    /// TowerDataBase的单例模式
    /// </summary>
    public static TowerDataBase Instance
    {
        get { return instance; }
        private set { }
    }

    void Awake()
    {
        instance = this;
        //用列表读取TowerData的数据
        towerDatas = JsonToObject.JsonToObject_ByJsonContent<TowerTrapData>(towerJ.text);
        //列表数据读入字典
        foreach (var a in towerDatas)
        {
            towerDataDictionary.Add(a.ID, a);
        }
    }
    /// <summary>
    /// 根据ID获取相应的TowerData对象的方法
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public TowerTrapData GetTowerData(int ID)
    {
        if (!towerDataDictionary.ContainsKey(ID))
        {
            Debug.Log("towerData中不存在" + ID);
            return null;
        }
        return towerDataDictionary[ID];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
