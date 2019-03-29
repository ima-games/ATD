using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHurt : MonoBehaviour
{
    [SerializeField]
    private float HealthPoint;
    private void Start()
    {
        HealthPoint = 100;
    }
    
    public float SetHP
    {
        get { return HealthPoint; }
        set { HealthPoint = value; }
    }

    public float AddHP
    {
        set { HealthPoint += value; }
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
