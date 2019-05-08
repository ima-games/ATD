using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能映射控制器
/// </summary>
public class SkillController : MonoBehaviour
{
    SkillSystem skillSystem;

    private void Awake()
    {
        skillSystem = GetComponent<SkillSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skillSystem.ReleaseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skillSystem.ReleaseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skillSystem.ReleaseSkill(2);
        }
    }
}
