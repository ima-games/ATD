using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour {
	/// <summary>
	/// 单例全局逻辑管理器的实例
	/// </summary>
	public static LogicManager Instance { get; private set; }

	/// <summary>
	/// ID-Individual查找表
	/// </summary>
	private Dictionary<int, Individual> individualList;

	/// <summary>
	/// 在生成新的Individual时注册ID
	/// </summary>
	/// <param name="ind">待注册的Individual</param>
	/// <returns>成功注册返回true；否则为false</returns>
	public bool Register(Individual ind) {
		if (individualList.ContainsKey(ind.ID)) {
			Debug.Log("Individual " + ind.ID + " has existed.");
			return false;
		}
		individualList.Add(ind.ID, ind);
		Debug.LogWarning("Individual" + ind.ID + "has successfully registered.");
		return true;
	}

	/// <summary>
	/// 通过ID获取Individual
	/// </summary>
	/// <param name="ID">待查找的ID</param>
	/// <returns>找到则返回该ID对应的Individual；否则返回null</returns>
	public Individual GetIndividual(int ID) {
		if (individualList.ContainsKey(ID)) {
			Debug.Log("Individual " + ID + " is found.");
			return individualList[ID];
		}
		Debug.LogWarning("Individual " + ID + " is NOT found.");
		return null;
	}

	bool IsPlayerDead() {
		Individual player = GetIndividual(0);
		if (player) {
			if (player.health <= 0) {
				return true;
			}
			return false;
		} else {
			//player不存在
			return true;
		}
	}

	bool IsBaseDestroyed() {
		Individual iBase = GetIndividual(1);
		if (iBase) {
			if (iBase.health <= 0) {
				return true;
			}
			return false;
		} else {
			//基地不存在
			return true;
		}
	}

	bool IsGameOver() {
		return IsPlayerDead() || IsBaseDestroyed();
	}

	void Awake() {
		if (Instance == null) {
			Instance = this;
			individualList = new Dictionary<int, Individual>();
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {

	}

	void Update() {
		if (IsGameOver()) {
			//player ID: 0
			//基地 ID: 1
			Debug.Log("GAME OVER");
		}
	}
}
