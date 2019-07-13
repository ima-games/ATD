#if UNITY_EDITOR
#define DEBUG
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    #region Fields
    /// <summary>
    /// 游戏中的金币数量字段
    /// </summary>
    [SerializeField]
    [Header("金钱数：")]
    private int _cash;

	/// <summary>
	/// 游戏中的金币数量
	/// </summary>
	public int Cash { get { return _cash; } }

    //消息系统
    private MessageSystem messageSystem;

    #endregion

    #region Public Methods
    /// <summary>
    /// 增加游戏中的金币数量
    /// </summary>
    /// <param name="cash">增加的值</param>
    public void AddCash(int cash) {
		_cash += cash;
	}

	/// <summary>
	/// 减少游戏中的金币数量
	/// </summary>
	/// <param name="cash">减少的值</param>
	/// <returns>减少值大于现有量时返回false且金币不会发生变化；否则金币数量减少并返回true</returns>
	public bool ReduceCash(int cash) {
		if (_cash - cash < 0) {
            Logger.Log($"Poor Guy.",LogType.Default);
			return false;
		}
		_cash -= cash;
		return true;
	}
	#endregion

	#region Private Methods
	/// <summary>
    /// 通过击杀个体，获得金币
    /// </summary>
    /// <param name="individual"></param>
    public void AddMoneyByKillIndividual(int individualID)
    {
        //只有怪物势力个体增加金币
        if (Factory.GetIndividual(individualID).power != Individual.Power.Monster) return;

        //TODO:暂定每个怪物金币 12
        AddCash(12);
    }

	#endregion

	#region Mono
	void Awake() {
        messageSystem = GetComponent<MessageSystem>();
    }

	void Start() {
        //注册死亡监听事件
        messageSystem.registerDieEvent(AddMoneyByKillIndividual);
    }

	void Update() {

	}

    private void LateUpdate()
    {
    }

    #endregion
}
