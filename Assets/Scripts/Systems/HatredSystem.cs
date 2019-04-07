using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HatredSystem : MonoBehaviour
{
    

    Dictionary<int, int> hatredList = new Dictionary<int, int>();

    private Individual individual;
    // Start is called before the first frame update
    void Start()
    {
        individual = GetComponent<Individual>();

    }
    /// <summary>
    /// 将HateSource添加到仇恨表中
    /// </summary>
    /// <param name="HateSource">仇恨目标</param>
    public void AddHatredList(Individual HateSource)
    {
        hatredList.Add(HateSource.ID, HateSource.hatredValue);
    }

    public Individual GetTarget()
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
        //知道了Individual的ID，怎么通过ID找到他的Individual
        //方法一，广播
        //方法二，建个全局的表来存目前存在的ID和Individual所属对象的名称和相关信息
        //等lf的接口
        Individual target=null;
        return target;
    }
}

