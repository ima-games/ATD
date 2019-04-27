using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDataBase : MonoBehaviour
{
    public TextAsset equipmentJ;

    private List<EquipmentData> equipmentDatas;
    private Dictionary<int, EquipmentData> equipmentDataDictionary = new Dictionary<int, EquipmentData>();
    private static EquipmentDataBase instance;

    //禁止外界通过new获取该类的实例
    private EquipmentDataBase() { }

    /// <summary>
    /// EquipmentDataBase的单例模式
    /// </summary>
    public static EquipmentDataBase Instance
    {
        get { return instance; }
        private set { }
    }

    void Awake()
    {
        instance = this;
        //用列表读取EquipmentData的数据
        equipmentDatas = JsonToObject.JsonToObject_ByJsonContent<EquipmentData>(equipmentJ.text);
        //列表数据读入字典
        foreach (var a in equipmentDatas)
        {
            equipmentDataDictionary.Add(a.ID, a);
        }
    }
    /// <summary>
    /// 根据ID获取相应的EquipmentData对象的方法
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public EquipmentData GetEquipmentData(int ID)
    {
        if (!equipmentDataDictionary.ContainsKey(ID))
        {
            Debug.Log("monsterData中不存在" + ID);
            return null;
        }
        return equipmentDataDictionary[ID];
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
