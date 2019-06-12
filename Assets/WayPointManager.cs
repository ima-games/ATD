using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    //先在面板通过roadsToInit获取n条路径
    //一个road对象应该顺序含有若干个空对象，每个对象代表一个路径点
    //然后根据每个road初始化每条路径对应的点坐标，并删除road对象（无用）

    public List<GameObject> roadsToInit;
    private List<List<Transform>> roads = new List<List<Transform>>();

    private void Awake()
    {
        for(int i = 0; i < roadsToInit.Count; ++i)
        {
            var childrenTransforms = roadsToInit[i].GetComponentsInChildren<Transform>();
            var road = new List<Transform>();
            for(int j=0; j < childrenTransforms.Length; ++j)
            {
                road.Add(childrenTransforms[j]);
            }
            roads.Add(road);
        }
    }

    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<Transform> GetRoad(int index)
    {
        if(index >= roads.Count)
        {
            Logger.Log("错误的路径index！", LogType.AI);
            return null;
        }

        return roads[index];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
