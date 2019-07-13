using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HatredSystem : MonoBehaviour
{

    //public int hateValueDecrement = 5;         //每秒仇恨值减少量

    //public int hateDecrementTime = 1;         //仇恨值减少间隔

    private Individual individual;

    //仇恨列表可视化
    [SerializeField] private List<string> hatredListShow = new List<string>();

    //key 是仇恨来源个体 ，value 是仇恨值
    Dictionary<Individual, int> hatredMap = new Dictionary<Individual, int>();

    private BehaviorTree behaviorTree;

    private MessageSystem messageSystem;

    private void Awake()
    {
        messageSystem = GetComponent<MessageSystem>();
        individual = GetComponent<Individual>();
        behaviorTree = GetComponent<BehaviorTree>();
    }

    private void Start()
    {
        ////实例化时调用
        //StartCoroutine(HateDecrementTimer());

        //订阅消息
        messageSystem.registerAttackEvent((int attackerID, float damage) => { AddHateValue(attackerID); });
        messageSystem.registerBuffEvent((int targetID, int buffID) => { if (BuffDataBase.Instance.GetBuffData(buffID).isTaunt) { AddHateValue(targetID); } });
        messageSystem.registerDieEvent((int sender) => { if (sender == individual.ID) { this.enabled = false; } });
    }

    private void Update()
    {

    }



    /// <summary>
    /// 增加仇恨值，若仇恨目标不在仇恨列表里，则先加入列表
    /// </summary>
    /// <param name="HateSourceID">仇恨目标</param>
    public void AddHateValue(int HateSourceID)
    {
        Individual HateSource = Factory.GetIndividual(HateSourceID);
        if (HateSource == null)
        {
            Logger.Log("HateSource is null", LogType.Hatred);
            return;
        }
        //如果目标的势力和自己相同则不列入仇恨列表
        //if (HateSource.power == individual.power) 
        //{
        //    Debug.Log("因为目标是友军，所以不对友军产生仇恨");
        //    return;
        //}

        if (!hatredMap.ContainsKey(HateSource))
        {
            AddHatredList(HateSource);
            //把仇恨目标的名字加入到仇恨列表可视化
            hatredListShow.Add(HateSource.name);
        }
        else
        {
            hatredMap[HateSource] += HateSource.hatredValue;
        }

        //此处更新行为树的最新目标
        SharedTransform sf = GetMostHatedTarget();
        behaviorTree.SetVariable("MostHatredTarget", sf);

        Logger.Log(gameObject.name+"对ID为"+HateSource.gameObject.name+"的对象增加了"+hatredMap[HateSource]+"点仇恨值",LogType.Hatred);
    }

    /// <summary>
    /// 获取当前仇恨值最高的目标
    /// </summary>
    /// <returns>返回仇恨值最高目标的Individual组件</returns>
    public Transform GetMostHatedTarget()
    {
        int maxValue = 0;
        Individual targetInd = null;
        foreach (KeyValuePair<Individual, int> kvp in hatredMap)
        {
            if (kvp.Value > maxValue)
            {
                maxValue = kvp.Value;
                targetInd = kvp.Key;
            }
        }

        //若找不到则返还null
        if (!targetInd)
            return null;

        if (!targetInd.enabled)
        {
            hatredMap.Remove(targetInd);
            hatredListShow.Remove(targetInd.name);
            return null;
        }

        return targetInd.transform;
    }

    //添加仇恨列表
    private void AddHatredList(Individual HateSource)
    {
        hatredMap.Add(HateSource, HateSource.hatredValue); 
    }

    ////随时间流逝仇恨减少
    //private void HateDecrement()
    //{
    //    Dictionary<int, int>.KeyCollection HLkeys = hatredList.Keys;
    //    if(HLkeys.Count==0) 
    //    {
    //        StartCoroutine(HateDecrementTimer());
    //        return;
    //    }
    //    int[] keyArray = new int[HLkeys.Count];

    //    int index = 0;
    //    foreach (int key in HLkeys)
    //    {
    //        keyArray[index] = key;
    //    }


    //    for(int i=0;i<HLkeys.Count;i++)
    //    {
    //        //对基地的仇恨不用减少
    //        //if (pair.Key == 0) continue;

    //        //减少固定
    //        hatredList[keyArray[i]] -= hateValueDecrement;
    //        Debug.Log("减少了对" + LogicManager.GetIndividual(keyArray[i]).gameObject.name + "的" + hateValueDecrement + "点仇恨值，当前仇恨值是："+ hatredList[keyArray[i]]);

    //        //仇恨值小于0移除出仇恨表
    //        if (hatredList[keyArray[i]] <= 0)
    //        {
    //            hatredList.RemoveIndividual(keyArray[i]);
    //        }
    //    }

    //    StartCoroutine(HateDecrementTimer());
    //}

    ////仇恨值每秒减少固定值，死循环
    //IEnumerator HateDecrementTimer()
    //{
    //    yield return new WaitForSeconds(hateDecrementTime);
    //    HateDecrement();
    //}
}

