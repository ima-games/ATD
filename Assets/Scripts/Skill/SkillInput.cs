using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能映射控制器
/// </summary>
public class SkillInput : MonoBehaviour
{
    public SkillSystem player;

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.ReleaseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.ReleaseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.ReleaseSkill(2);
        }
    }
}
