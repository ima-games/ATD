using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Buff特效管理器
/// </summary>
public class BuffEffectManager : MonoBehaviour
{
    public GameObject[] effectPrefabs;

    private Dictionary<Transform,List<KeyValuePair<int,GameObject>>> effects = new Dictionary<Transform, List<KeyValuePair<int, GameObject>>>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void PlayEffect(Transform targetInd, int buffID)
    {
        if (!effects.ContainsKey(targetInd))
        {
            effects.Add(targetInd, new List<KeyValuePair<int, GameObject>>());
        }

        var l = effects[targetInd];

        foreach(var itr in l)
        {
            if (itr.Key == buffID) return;
        }

        var go = Instantiate(effectPrefabs[buffID],targetInd);

        l.Add(new KeyValuePair<int, GameObject>(buffID,go));
    }

    internal void StopEffect(Transform targetInd, int buffID)
    {
        if (!effects.ContainsKey(targetInd))
        {
            return;
        }

        var l = effects[targetInd];

        for(int i = 0; i < l.Count;++i)
        {
            if (l[i].Key == buffID)
            {
                Destroy(l[i].Value);
                l.RemoveAt(i);
                return;
            }
        }
    }
}
