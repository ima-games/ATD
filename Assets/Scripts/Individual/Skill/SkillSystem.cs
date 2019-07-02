using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{    
    //英雄技能对象列表
    List<ISkill> heroSkills = new List<ISkill>();
    public List<ISkill> HeroSkills { get => heroSkills; set => heroSkills = value; }

    //TODO
    public SkillEffectManager skillEffectManager;
    
    ////装备技能对象列表
    //List<ISkill> EquipmentSkill = new List<ISkill>();

    private Individual individual;


    private void Awake()
    {
        individual = GetComponent<Individual>();
        //TODO
        //目前硬编码给玩家赋予3个技能
        HeroSkills.Add(new BuffSkill(6, true, true, 5.0f));   //主动技能：嘲讽Buff
        HeroSkills.Add(new BuffSkill(0, true, false));        //主动技能：回血buff
        HeroSkills.Add(new BuffSkill(14, true, false));       //主动技能：攻速戒指buff
    }


    void Start()
    {
        foreach (ISkill skill in HeroSkills)
        {
            skill.InitSkill(individual);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ISkill skill in HeroSkills)
        {
            skill.UpdateSkill(individual);
        }
    } 

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="index"></param>
    public void ReleaseSkill(int index)
    {
        Logger.Log("Release Skill " + index , LogType.Skill);

        if(index >= HeroSkills.Count){ return; }

        HeroSkills[index].ReleaseSkill(individual);
    }

    public void ReceiveMessage(Individual attacker,float damage)
    {
        //为方便调试，暂时什么也不做
        //foreach (var skill in HeroSkill)
        //{
        //    skill.DealAttackMessage(attacker, damage);
        //}
        //foreach (var skill in EquipmentSkill)
        //{
        //    skill.DealAttackMessage(attacker, damage);
        //}
    }
}
