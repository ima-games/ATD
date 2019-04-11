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
    // Start is called before the first frame update
    void Start()
    {
        List<PersonData> perDatas = JsonToObject.JsonToObject_ByJsonContent<PersonData>(PersonJ.text);
        List<TowerTrapData> ttDatas = JsonToObject.JsonToObject_ByJsonContent<TowerTrapData>(Tow_TrapJ.text);
        List<EquipmentData> equipmentDatas = JsonToObject.JsonToObject_ByJsonContent<EquipmentData>(EquipmentJ.text);
        List<MonsterData> monsterDatas = JsonToObject.JsonToObject_ByJsonContent<MonsterData>(MonsterJ.text);
        List<HatredData> hatredDatas = JsonToObject.JsonToObject_ByJsonContent<HatredData>(HatredJ.text);
        
        /*Debug.Log("ID "+perDatas[1].ID);
        Debug.Log("Name "+perDatas[1].Name);
        Debug.Log("HP "+perDatas[1].HP);
        Debug.Log("HpReturn "+perDatas[1].HpReturn);
        Debug.Log("Attack "+perDatas[1].Attack);
        Debug.Log("AttSpeed "+perDatas[1].AttSpeed);
        Debug.Log("Speed "+perDatas[1].Speed);*/
        //Debug.Log("Name " + hatredDatas[0].State);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
