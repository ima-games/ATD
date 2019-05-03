using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ProjectileTest : MonoBehaviour
{
    public GameObject target;   //要到达的目标
    public GameObject point;
    public GameObject bullet;
    [Range(5, 9)]
    public float angle = 5.0f;
    [Range(0,100)]
    public int len;

    public int count = 5;
    
    private List<Vector3> n;
    void Start()
    {
        n = new List<Vector3>();
        for (int i = count; i > 0; i--)
        {
            Vector3 newVec = Quaternion.Euler(0, -1 * angle * i, 0) * transform.forward * len;
            n.Add(newVec);
        }
        for (int i = 0; i < count; i++)
        {
            Vector3 newVec = Quaternion.Euler(0, angle * i, 0) * transform.forward * len;
            n.Add(newVec);
            
        }
    }

    

    void Update()
    {
        Dictionary<string, RaycastHit> h = new Dictionary<string, RaycastHit>();
        RaycastHit[] hits;
        
        Vector3 dir = transform.position;

        hits = Physics.RaycastAll(dir, transform.forward, len);


        //for (int i = 0; i < n.Count; i++)
        //{
        //    hits = Physics.RaycastAll(dir, n[i], len);
        foreach (var j in hits)
        {
            Debug.Log(j.transform.name);
        }
        //    //foreach (RaycastHit j in hits)
        //    //{
        //    //    if (!h.ContainsKey(j.transform.name))
        //    //    {
        //    //        h.Add(j.transform.name, j);
        //    //    }
        //    //}
        //    Debug.DrawRay(dir, n[i], Color.red);
        //}

        //foreach (var i in h.Keys)
        //{
        //    Debug.Log(i);
        //}
    }

}
