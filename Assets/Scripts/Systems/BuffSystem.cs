using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{

    List<int> BuffList = new List<int>();                    //存目前身上存在的buffID
    Dictionary<int, int> BuffCount = new Dictionary<int, int>();       //存目前身上buff叠加的数量

    public void StickBuff(int buffID)
    {
        //收到buffID之后如何把buff体现，并修改实体组件的值？

        //把buffID对应的buff加到buff表里
        //BuffsList.Add(Buff)

        //启动一个计时器后删除这个buff
        //1.把buffID加到表里，count+1       ----AddBuff
        //1.5、把buff数据同步到实体组件     ----BuffSync
        //2.启动计时                        ----BuffCountTime
        //3.时间到，把buff去掉，count-1     ----DestroyBuff
        //4.buff同步到实体组件

        AddBuff(buffID);
        //启动一个计时器，时间到把buff移除
        //StartCoroutine("BuffCountTime");


    }


    //1.把buffID加到表里，count+1       ----AddBuff
    //
    private void AddBuff(int buffID)
    {
        //如果已经有这个buff，则把count里的计数+1
        if(BuffList.Contains(buffID))
        {
            BuffCount[buffID]++;
            
        }
        //如果没有这个buff，则把这个buff加进List和Count中，并把count里的值置为1，属性同步
        else
        {
            BuffList.Add(buffID);
            BuffCount.Add(buffID, 1);
            BuffSync();
        }
    }

    //1.5、把buff数据同步到实体组件     ----BuffSync

    private void BuffSync()
    {
        //哇这个好烦
        //遍历BuffList，同步数据
        //----------------------以下等待一个接口，通过ID获取到buffdata--------------------

        //----------------------以上
    }



    //2.启动计时                        ----BuffCountTime
    //计时，时间结束调用DestroyBuff方法
    IEnumerator BuffCountTime(float timer,int buffID)
    {
        yield return new WaitForSeconds(timer);
        DestroyBuff(buffID);
    }



    //3.时间到，把buff去掉，count-1     ----DestroyBuff
    //buff持续时间到，destroy一个buff，如果有叠加的buff，则把计数的count-1，否则把List和count里的remove掉
    private void DestroyBuff(int buffID)
    {
        
        if (BuffCount[buffID] > 1)
        {
            BuffCount[buffID]--;
            return;
        }


        if (!BuffList.Contains(buffID))
        {
            throw new Exception(string.Format("物体{0}身上并没有名为{1}的Buff", gameObject.name, buffID));
        } 
        else
        {
            //buff移除，属性同步
            BuffList.Remove(buffID);
            BuffCount.Remove(buffID);
            BuffSync();
        }
    }

}
