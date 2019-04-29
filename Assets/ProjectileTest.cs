using UnityEngine;
using System.Collections;

public class ProjectileTest : MonoBehaviour
{
    public GameObject target;   //要到达的目标
    public GameObject point;
    public GameObject bullet;
    public float speed = 10;    //速度
    public int rate = 1;



    void Start()
    {
        //计算两者之间的距离
        
        StartCoroutine(StartShoot());
    }

    IEnumerator StartShoot()
    {
        GameObject bulletObj = Instantiate(bullet, point.transform.position, Quaternion.identity);
        float L = (point.transform.position - target.transform.position).x;
        float H = point.transform.position.y;
        float a = speed * speed * H * 2 / (L * L);
        Debug.Log("a:" + a);
        Debug.Log("L:" + L);
        Debug.Log("H:" + H);
        Debug.Log("T:" + L / speed);

        //bulletObj.transform.LookAt(target.transform);
        bulletObj.GetComponent<Rigidbody>().velocity = Vector3.left * speed;
        bulletObj.GetComponent<Rigidbody>().AddForce(Vector3.down * a);

        Debug.Log("velocity:" + bulletObj.GetComponent<Rigidbody>().velocity);
        Debug.Log("Force:" + Vector3.down * a);
        yield return new WaitForSeconds(rate);
    }

    void Update()
    {

    }

}
