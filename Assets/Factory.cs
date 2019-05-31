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
    /// 存活个体的ID列表字段
    /// </summary>
    private static List<Individual> _aliveIndividualList;

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
    /// 存活个体的ID列表
    /// </summary>
    public static List<Individual> AliveIndividualList { get { return _aliveIndividualList; } }

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
        _IDToIndividualDictionary.Add(key, ind);
        _aliveIndividualList.Add(ind);
        Logger.Log($"Individual { ind.ID } has successfully registered.",LogType.Individual);
    }

    /// <summary>
    /// 在生成英雄或者基地时注册ID
    /// </summary>   
    /// <param name="ind">待注册的Individual</param>
    /// <param name="ID">ID：英雄为0，基地为1</param>
    public static void RegisterIndividual(Individual ind, int ID)
    {
        _IDToIndividualDictionary.Add(ID, ind);
        _aliveIndividualList.Add(ind);
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

        if (_IDToIndividualDictionary.ContainsKey(ind.ID))
        {
            _IDToIndividualDictionary.Remove(ind.ID);
            _aliveIndividualList.Remove(ind);
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
        if (_IDToIndividualDictionary.ContainsKey(ID))
        {
            return _IDToIndividualDictionary[ID];
        }
        Logger.Log($"Individual { ID } is NOT found.", LogType.Individual);
        return null;
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
        _aliveIndividualList = new List<Individual>();
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
