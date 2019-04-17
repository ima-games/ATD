using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HatredSystem : MonoBehaviour
{
    private Individual individual;

    //key 是仇恨来源ID ，value 是仇恨值
    Dictionary<int, int> hatredList = new Dictionary<int, int>();

    void Awake()
    {
        individual = GetComponent<Individual>();
    }

    /// <summary>
    /// 增加仇恨值，若仇恨目标不在仇恨列表里，则先加入列表
    /// </summary>
    /// <param name="HateSource">仇恨目标</param>
    public void AddHateValue(int HateID)
    {
        Individual HateSource = LogicManager.GetIndividual(HateID);
        //如果目标的势力和自己相同则不列入仇恨列表
        if (HateSource.power == individual.power) 
        {
            Debug.Log("因为目标是友军，所以不对友军产生仇恨");
            return;
        }

        if (!hatredList.ContainsKey(HateSource.ID))
        {
            AddHatredList(HateSource);
        }
        else
        {
            hatredList[HateSource.ID] += HateSource.hatredValue;
        }

        Debug.Log(gameObject.name+"对ID为"+HateSource.gameObject.name+"的对象增加了"+hatredList[HateSource.ID]+"点仇恨值");
    }

    /// <summary>
    /// 获取当前仇恨值最高的目标
    /// </summary>
    /// <returns>返回仇恨值最高目标的Individual组件</returns>
    public Individual GetMostHatedTarget()
    {
        int maxValue = 0;
        int TargerID = 0;
        foreach(KeyValuePair<int, int> kvp in hatredList)
        {
            if (kvp.Value > maxValue)
            {
                maxValue = kvp.Value;
                TargerID = kvp.Key;
            }
        }

        return LogicManager.GetIndividual(TargerID);
    }

    //添加仇恨列表
    private void AddHatredList(Individual HateSource)
    {
        hatredList.Add(HateSource.ID, HateSource.hatredValue);
    }
}

