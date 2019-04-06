using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public static class JsonToObject
{
    /// <summary>
    /// 把一个Json文本文件，转成一个对象(Object)
    /// </summary>
    /// <typeparam name="T">对象的类型</typeparam>
    /// <param name="filePath">Json文本文件的地址(需要加上文件名和后缀名)</param>
    /// <returns></returns>
    public static List<T> JsonToObject_ByJsonFile<T>(string filePath)
    {
        /*直接解析成对象*/
        //读取Json文本中的内容
        string json = File.ReadAllText(filePath);
        //解析Json文本中的内容 -(解析成数组或者List列表都可以)
        List<T> datas = JsonToObject_ByJsonContent<T>(json);
        return datas;
    }

    /// <summary>
    /// 把一个Json格式的文本，转成一个对象(Object)
    /// </summary>
    /// <typeparam name="T">对象的类型</typeparam>
    /// <param name="filePath">Json文本中的内容</param>
    /// <returns></returns>
    public static List<T> JsonToObject_ByJsonContent<T>(string conntent)
    {
        /*直接解析成对象*/
        //解析Json文本中的内容 -(解析成数组或者List列表都可以)
        T[] datas = JsonMapper.ToObject<T[]>(conntent);

        //把数组封装成List列表
        List<T> dataList = new List<T>();
        for (int i = 0; i < datas.Length; i++)
        {
            dataList.Add(datas[i]);
        }
        return dataList;
    }
}
