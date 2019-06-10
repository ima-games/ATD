using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔的逻辑管理器
/// </summary>
public class TowerManager : MonoBehaviour
{
    //塔个体对象prefab
    public List<GameObject> _towerPrefabs;

    //游戏场景里的塔基
    private GameObject[] _towerBases;

    //塔的数据库
    private TowerDataBase _towerDataBase;

    //金钱
    private MoneyManager _logicManager;

    //塔基对应是否有塔
    private Dictionary<GameObject, bool> _towerBaseHasTower = new Dictionary<GameObject, bool>();

    [HideInInspector]
    private GameObject targetTowerBase = null;

    public GameObject TargetTowerBase { get => targetTowerBase; set => targetTowerBase = value; }

    private void Awake()
    {
        _towerBases = GameObject.FindGameObjectsWithTag("TowerBase");

        //初始化默认都没有塔
        for (int i = 0; i < _towerBases.Length; ++i)
        {
            _towerBaseHasTower.Add(_towerBases[i], false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //检测是否可以造塔
    public bool IfCanBuildTower()
    {
        if (_towerBaseHasTower.ContainsKey(TargetTowerBase))
        {
            return !_towerBaseHasTower[TargetTowerBase];
        }
        Logger.Log("TowerBase Doesn't exist!", LogType.Tower);
        return false;
    }

    //检测是否可以造某种塔(取决塔的造价)
    public bool IfCanBuildTower(int towerIndex)
    {
        if (!IfCanBuildTower())
        {
            return false;
        }

        //目前而言都是免费造塔
        //TODO:检测塔 钱

        return true;
    }

    //建造塔
    public void BuildTower(int towerIndex)
    {
        if (!IfCanBuildTower(towerIndex))
        {
            Logger.Log("Can't Build Tower!", LogType.Tower);
            return;
        }

        _towerBaseHasTower[TargetTowerBase] = true;
        Instantiate(_towerPrefabs[towerIndex], TargetTowerBase.transform);
        Logger.Log("Succeessfully Build A Tower.", LogType.Tower);
    }
}
