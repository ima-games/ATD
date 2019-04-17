using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDataBase : MonoBehaviour
{
    //读取excel插件生成的json文件
    public TextAsset BuffDataJ;

    
    private List<BuffData> buffDatas;
    private Dictionary<int, BuffData> buffDataDictionary = new Dictionary<int, BuffData>();
    private static BuffDataBase instance;
    

    //禁止外界通过new获取该类的实例
    private BuffDataBase() { }

    /// <summary>
    /// BuffDataBased的单例模式
    /// </summary>
    public static BuffDataBase Instance
    {
        get { return instance; }
        private set { }
    }
    
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //用列表读取BuffData的数据
        buffDatas = JsonToObject.JsonToObject_ByJsonContent<BuffData>(BuffDataJ.text);
        //列表数据读入字典
        foreach (var a in buffDatas)
        {
            buffDataDictionary.Add(a.ID, a);
        }
    }
    /// <summary>
    /// 根据ID获取相应的BuffData对象的方法
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public BuffData GetBuffData(int ID)
    {
        if (!buffDataDictionary.ContainsKey(ID))
        {
            Debug.Log("buffData中不存在" + ID);
            return null;
        }
        return buffDataDictionary[ID];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
//该脚本必须挂在一个GameObject上
//用例：BuffData a = BuffDataBase.Instance.GetBuffData(0);
