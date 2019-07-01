using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelUI : MonoBehaviour
{
    public Image[] images;

    private SkillSystem skillSystem;

    // Start is called before the first frame update
    void Start()
    {
        //获取玩家的技能系统
        skillSystem = Factory.PlayerIndividual.GetComponent<SkillSystem>();


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skillSystem.HeroSkills.Count; ++i)
        {
            images[i].fillAmount = skillSystem.HeroSkills[i].GetColdTimePercent();
        }
    }
}
