using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonDataBase : MonoBehaviour
{
    public TextAsset personJ;

    private List<PersonData> personDatas;
    private Dictionary<int, PersonData> personDataDictionary = new Dictionary<int, PersonData>();
    private static PersonDataBase instance;

    //禁止外界通过new获取该类的实例
    private PersonDataBase() { }

    /// <summary>
    /// PersonDataBase的单例模式
    /// </summary>
    public static PersonDataBase Instance
    {
        get { return instance; }
        private set { }
    }

    void Awake()
    {
        instance = this;
        //用列表读取PersonData的数据
        personDatas = JsonToObject.JsonToObject_ByJsonContent<PersonData>(personJ.text);
        //列表数据读入字典
        foreach (var a in personDatas)
        {
            personDataDictionary.Add(a.ID, a);
        }
    }
    /// <summary>
    /// 根据ID获取相应的PersonData对象的方法
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public PersonData GetPersonData(int ID)
    {
        if (!personDataDictionary.ContainsKey(ID))
        {
            Debug.Log("personData中不存在" + ID);
            return null;
        }
        return personDataDictionary[ID];
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
