using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDataBase : MonoBehaviour
{
    public TextAsset PersonJ;
    public TextAsset Tow_TrapJ;
    public TextAsset EquipmentJ;
    public TextAsset MonsterJ;
    public TextAsset HatredJ;
    public TextAsset BuffDataJ;

    private List<BuffData> buffDatas;
    private List<PersonData> personDatas;
    private List<PersonData> perDatas;
    private List<TowerTrapData> towerTrapDatas;
    private List<EquipmentData> equipmentDatas;
    private List<MonsterData> monsterDatas;
    private List<HatredData> hatredDatas;

    private static BuffDataBase instance;
    private Dictionary<int, BuffData> buffDataDictionary = new Dictionary<int, BuffData>();

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
        perDatas = JsonToObject.JsonToObject_ByJsonContent<PersonData>(PersonJ.text);
        towerTrapDatas = JsonToObject.JsonToObject_ByJsonContent<TowerTrapData>(Tow_TrapJ.text);
        equipmentDatas = JsonToObject.JsonToObject_ByJsonContent<EquipmentData>(EquipmentJ.text);
        monsterDatas = JsonToObject.JsonToObject_ByJsonContent<MonsterData>(MonsterJ.text);
        hatredDatas = JsonToObject.JsonToObject_ByJsonContent<HatredData>(HatredJ.text);
        buffDatas = JsonToObject.JsonToObject_ByJsonContent<BuffData>(BuffDataJ.text);

        foreach (var a in buffDatas)
        {
            buffDataDictionary.Add(a.ID, a);
        }


    }
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
