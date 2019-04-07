using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Individual : MonoBehaviour
{
    public int ID = 0;
    public int health = 100;           //生命
    public int attack = 10;            //攻击力
    public float attackSpeed = 1.0f;   //攻击速度
    public float speed = 1.0f;         //速度
    public float attackDistance = 1.0f;//攻击距离
    public float recoverRate = 0.0f;   //回复速率

    public int hatredValue = 10;       //攻击时造成的仇恨值,先默认设定为10

    public enum Power {Monster, Human, Neutral}
    public Power power = Power.Neutral; //所属势力

    public bool movable = true;         //是否可移动
    public bool attackable = true;      //是否可攻击
    public bool controllable = true;    //是否可自由控制

    public bool tauntable = true;       //是否可被嘲讽
    public bool charmable = true;       //是否可被魅惑
    public bool restrictable = true;    //是否可被束缚
    public bool speedDownAble = true;   //是否可被减速
    public bool dizzyAble = true;       //是否可被麻痹

    public int reviveCount = 0;         //复活次数
    public int maxReviveCount = 0;      //最大复活次数

    void Start() {
		LogicManager.Instance.Register(this);
		//Do something
	}
}
