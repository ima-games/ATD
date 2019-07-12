using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个类的实现应该写在个体组件里面
public class TestHurt : MonoBehaviour
{
    [SerializeField]
    private float HealthPoint;
    private void Start()
    {
        HealthPoint = 100;
    }
    public void ReduceHP(float damage)
    {
        HealthPoint -= damage;
    }
    private void Update()
    {
        if (HealthPoint <= 0) gameObject.SetActive(false);
    }
}
