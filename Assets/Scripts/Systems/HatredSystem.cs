using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HatredSystem : MonoBehaviour
{
    
    //key 是仇恨来源ID ，value 是仇恨值
    Dictionary<int, int> hatredList = new Dictionary<int, int>();
    //Dictionary<int, KeyValuePair<int, float>> HatredList = new Dictionary<int, KeyValuePair<int, float>>();
    private Individual individual;
    // Start is called before the first frame update
    void Start()
    {
        individual = GetComponent<Individual>();

    }
    /// <summary>
    /// 增加仇恨值，若仇恨目标不在仇恨列表里，则先加入列表
    /// </summary>
    /// <param name="HateSource">仇恨目标</param>
    public void AddHateValue(Individual HateSource)
    {
        if (!hatredList.ContainsKey(HateSource.ID))
        {
            AddHatredList(HateSource);
        }
        else
        {
            hatredList[HateSource.ID] += HateSource.hatredValue;
        }

        Debug.Log(gameObject.name+"对ID为"+HateSource.ID+"的对象增加了"+hatredList[HateSource.ID]+"点仇恨值");
    }

    private void AddHatredList(Individual HateSource)
    {
        hatredList.Add(HateSource.ID, HateSource.hatredValue);
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
}

