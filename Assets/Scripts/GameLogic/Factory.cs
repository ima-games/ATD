using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// ID队列的最大容量
    /// </summary>
    [SerializeField]
    [Header("ID队列的最大容量，参考场景内总Individual数量设置")]
    private int _MAX_IDQUEUE_SIZE = 256;

    //消息系统
    private MessageSystem messageSystem;

    /// <summary>
    /// ID队列，ID分配容器
    /// </summary>
    private static Queue<int> _IDQueue;

    /// <summary>
    /// ID-Individual查找表
    /// </summary>
    private static Dictionary<int, Individual> _IDToIndividualDictionary;

    /// <summary>
    /// 存放被删除对象，用于lazy delete
    /// </summary>
    private static List<int> _IDToRemove;

    /// <summary>
    /// 特殊记录：玩家个体对象
    /// </summary>
    private static Individual player = null;

    /// <summary>
    /// 特殊记录：基地个体对象
    /// </summary>
    private static Individual baseIndividual = null;

    #endregion

    #region Properties

    public static Dictionary<int, Individual> IDToIndividualDictionary { get => _IDToIndividualDictionary;}

    public static Individual PlayerIndividual { get => player;}

    public static Individual BaseIndividual { get => baseIndividual;}

    #endregion

    #region Public Methods

    /// <summary>
    /// 在生成新的Individual时注册ID
    /// </summary>   
    /// <param name="ind">待注册的Individual</param>
    public static void RegisterIndividual(Individual ind, IndividualType individualType = IndividualType.Normal)
    {
        //根据个体类型，记录特别个体
        switch (individualType)
        {
            case IndividualType.Normal:
                break;
            case IndividualType.Player:
                if (player) { Logger.Log("Warning!PlayerIndividual already existed!!", LogType.Individual); }
                player = ind;
                break;
            case IndividualType.BaseIndividual:
                if (baseIndividual) { Logger.Log("Warning!baseIndividual already existed!!", LogType.Individual); }
                baseIndividual = ind;
                break;
        }

        int key = _IDQueue.Dequeue();
        ind.ID = key;
        IDToIndividualDictionary.Add(key, ind);

        Logger.Log($"Individual { ind.ID } has successfully registered.",LogType.Individual);
    }

    /// <summary>
    /// 在Individual死亡时注销Individual(lazy delete)
    /// </summary>
    /// <param name="individualID">死亡的Individual</param>
    public static void RemoveIndividual(int individualID)
    {
        _IDToRemove.Add(individualID);
    }

    /// <summary>
    /// 通过ID获取Individual
    /// </summary>
    /// <param name="ID">待查找的ID</param>
    public static Individual GetIndividual(int ID)
    {
        if (IDToIndividualDictionary.ContainsKey(ID))
        {
            return IDToIndividualDictionary[ID];
        }
        Logger.Log($"Individual { ID } is NOT found.", LogType.Individual);
        return null;
    }

    //NOTE：ADDED BY AERY
    /// <summary>
    /// 遍历个体列表（可带条件过滤）
    /// </summary>
    /// <param name="行为"></param>
    /// <param name="条件"></param>
    public static void TraversalIndividuals(Action<Individual> action, Func<Individual, bool> condition)
    {
        foreach (var pair in IDToIndividualDictionary)
        {
            Individual ind = pair.Value;
            if (condition(ind))
            {
                action(ind);
            }
        }
    }

    //NOTE：ADDED BY AERY
    /// <summary>
    /// 遍历个体列表（可带条件过滤）
    /// </summary>
    /// <param name="行为"></param>
    /// <param name="条件"></param>
    public static void TraversalIndividuals(Action<Individual> action)
    {
        foreach (var pair in IDToIndividualDictionary)
        {
           action(pair.Value);
        }
    }

    //NOTE：ADDED BY AERY
    /// <summary>
    /// 遍历个体列表（可带条件过滤）
    /// </summary>
    /// <param name="行为"></param>
    /// <param name="圆心"></param>
    /// <param name="半径"></param>
    /// <param name="条件"></param>
    public static void TraversalIndividualsInCircle(Action<Individual> action, Vector3 point, float radius, Func<Individual, bool> condition)
    {
        foreach (var pair in IDToIndividualDictionary)
        {
            Individual ind = pair.Value;
            if ((ind.transform.position - point).sqrMagnitude < radius * radius && condition(ind))
            {
                action(ind);
            }
        }
    }

    //NOTE：ADDED BY AERY
    /// <summary>
    /// 遍历个体列表（可带条件过滤）
    /// </summary>
    /// <param name="行为"></param>
    /// <param name="圆心"></param>
    /// <param name="半径"></param>
    /// <param name="条件"></param>
    public static void TraversalIndividualsInCircle(Action<Individual> action, Vector3 point, float radius)
    {
        foreach (var pair in IDToIndividualDictionary)
        {
            Individual ind = pair.Value;
            if ((ind.transform.position - point).sqrMagnitude < radius * radius)
            {
                action(ind);
            }
        }
    }

    /// <summary>
    /// 检测是否存在怪物
    /// </summary>
    /// <returns></returns>
    public static bool HasMonsterIndividual()
    {
        foreach (var ind in _IDToIndividualDictionary.Values)
        {
            if (ind.power == Individual.Power.Monster) return true;
        }

        return false;
    }


    public static bool IsPlayerDead()
    {
        Individual player = GetIndividual(0);
        if (player)
        {
            if (player.health <= 0)
            {
                return true;
            }
            return false;
        }
        else
        {
            //player不存在
            return true;
        }
    }

    public static bool IsBaseDestroyed()
    {
        Individual iBase = GetIndividual(1);
        if (iBase)
        {
            if (iBase.health <= 0)
            {
                return true;
            }
            return false;
        }
        else
        {
            //基地不存在
            return true;
        }
    }

    public static bool IsGameOver()
    {
        return IsPlayerDead() || IsBaseDestroyed();
    }

    #endregion


    #region Mono
    void Awake()
    {
        messageSystem = GetComponent<MessageSystem>();
        //初始化ID序号池
        _IDQueue = new Queue<int>(_MAX_IDQUEUE_SIZE);
        for (int id = 1; id < _MAX_IDQUEUE_SIZE; id++) _IDQueue.Enqueue(id);

        _IDToIndividualDictionary = new Dictionary<int, Individual>();

        _IDToRemove = new List<int>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        LazyRemoveIndividuals();
    }

    #endregion

    #region private

    private void LazyRemoveIndividuals()
    {
        for (int i = 0; i < _IDToRemove.Count; ++i)
        {
            if (IDToIndividualDictionary.Remove(_IDToRemove[i]))
            {
                _IDQueue.Enqueue(_IDToRemove[i]);
            }
        }
        _IDToRemove.Clear();
    }

    #endregion
}

// 个体类型，目前只需要特化玩家类型和基地类型，用于Factory直接查找特殊个体
public enum IndividualType {
    Normal, Player, BaseIndividual
};