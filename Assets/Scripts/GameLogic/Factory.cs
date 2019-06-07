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
    private int _MAX_IDQUEUE_SIZE = 100;

    /// <summary>
    /// ID队列，ID分配容器
    /// </summary>
    private static Queue<int> _IDQueue;

    /// <summary>
    /// ID-Individual查找表
    /// </summary>
    private static Dictionary<int, Individual> _IDToIndividualDictionary;

    //NOTE：ADDED BY AERY
    /// <summary>
    /// 被标记死亡的个体对象（每帧检查这个列表，并对列表内的游戏对象进行Destory）
    /// </summary>
    private static List<Individual> _IndividualsToDelete;


    #endregion

    #region Properties
    /// <summary>
    /// 个体列表
    /// </summary>
    public static Dictionary<int, Individual> IDToIndividualDictionary { get => _IDToIndividualDictionary;}
    #endregion

    #region Public Methods

    /// <summary>
    /// 在生成新的Individual时注册ID
    /// </summary>   
    /// <param name="ind">待注册的Individual</param>
    public static void RegisterIndividual(Individual ind)
    {
        int key = _IDQueue.Dequeue();
        ind.ID = key;
        IDToIndividualDictionary.Add(key, ind);
        Logger.Log($"Individual { ind.ID } has successfully registered.",LogType.Individual);
    }

    /// <summary>
    /// 在生成英雄或者基地时注册ID
    /// </summary>   
    /// <param name="ind">待注册的Individual</param>
    /// <param name="ID">ID：英雄为0，基地为1</param>
    public static void RegisterIndividual(Individual ind, int ID)
    {
        IDToIndividualDictionary.Add(ID, ind);
        Logger.Log($"Individual { ind.ID } has successfully registered.", LogType.Individual);
    }

    /// <summary>
    /// 在Individual死亡时注销Individual
    /// </summary>
    /// <param name="ind">死亡的Individual</param>
    public static void RemoveIndividual(Individual ind)
    {
        //带删除列表增加该对象
        _IndividualsToDelete.Add(ind);

        if (IDToIndividualDictionary.ContainsKey(ind.ID))
        {
            IDToIndividualDictionary.Remove(ind.ID);
            if (ind.ID != 0 && ind.ID != 1)
            {
                _IDQueue.Enqueue(ind.ID);
            }
        }
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

    #endregion

    #region Private Methods
    private bool IsPlayerDead()
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

    private bool IsBaseDestroyed()
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

    private bool IsGameOver()
    {
        return IsPlayerDead() || IsBaseDestroyed();
    }
    #endregion

    #region Mono
    void Awake()
    {
        _IDQueue = new Queue<int>(_MAX_IDQUEUE_SIZE);
        _IDToIndividualDictionary = new Dictionary<int, Individual>();
        _IndividualsToDelete = new List<Individual>();

        for (int id = 2; id < _MAX_IDQUEUE_SIZE; id++)
        {
            _IDQueue.Enqueue(id);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        //游戏循环的最后阶段,检查带删除列表，删除标记死亡的对象
        foreach (var ind in _IndividualsToDelete)
        {
            GameObject.Destroy(ind.gameObject);
        }
        _IndividualsToDelete.Clear();
    }

    #endregion
}
