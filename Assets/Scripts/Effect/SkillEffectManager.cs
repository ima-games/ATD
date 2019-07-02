using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能特效管理器
/// </summary>
public class SkillEffectManager : MonoBehaviour
{
    public GameObject[] effectPrefabs;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RemoveEffectAfterSeconds(float time,GameObject go)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

    internal void PlayEffect(Transform targetInd,int index)
    {
        if (index >= effectPrefabs.Length || !effectPrefabs[index])
        {
            return;
        }

        var go = Instantiate(effectPrefabs[index],targetInd);
        StartCoroutine(RemoveEffectAfterSeconds(2.0f,go));
    }
}
