using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodHUD : MonoBehaviour
{
    public GameObject addBloodTextPrefab;
    public GameObject reduceBloodTextPrefab;

    private void Awake()
    {
        //将事件类型和函数绑定
        EventCenter.AddListener<int, int, int, object>(EventType.Message, SolveMessage);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// 处理消息的函数，对其中的攻击消息进行血量变化的HUD显示
    /// </summary>
    /// <param name="messageID"></param>
    /// <param name="ob"></param>
    /// <param name="senderID"></param>
    /// <param name="receverID"></param>
    public void SolveMessage(int messageID, int senderID, int receverID, object ob)
    {
        //todo
        //只要处理攻击消息
        if(messageID != 1) {
            return;
        }

        var ind = Factory.GetIndividual(receverID);
        if(ind == null)
        {
            return;
        }

        int bloodchange = -(int)((float)ob);

        //血量减少HUD显示
        if(bloodchange < 0.0f)
        {
            var go = Instantiate(reduceBloodTextPrefab, Vector3.zero, Quaternion.identity, transform);
            go.transform.position = Camera.main.WorldToScreenPoint(ind.transform.position);
            go.transform.localScale = Vector3.one / (Camera.main.transform.position - ind.transform.position).magnitude * 7;
            go.transform.GetChild(0).GetComponent<Text>().text = bloodchange.ToString();
            go.transform.GetChild(0).GetComponent<Animation>().Play();
            StartCoroutine(DeleteBloodChangeTextLater(go));
        }
    }

    //TODO
    public void ShowBlood(int health,Transform target)
    {
        int bloodchange = health;
        GameObject go;
        //血量减少HUD显示
        if (bloodchange < 0.0f)
        {
            go = Instantiate(reduceBloodTextPrefab, Vector3.zero, Quaternion.identity, transform);
        }
        else
        {
            go = Instantiate(addBloodTextPrefab, Vector3.zero, Quaternion.identity, transform);
        }
        go.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        go.transform.localScale = Vector3.one / (Camera.main.transform.position - target.transform.position).magnitude * 7;
        go.transform.GetChild(0).GetComponent<Text>().text = bloodchange.ToString();
        go.transform.GetChild(0).GetComponent<Animation>().Play();
        StartCoroutine(DeleteBloodChangeTextLater(go));
    }

    IEnumerator DeleteBloodChangeTextLater(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.7f);
        GameObject.Destroy(gameObject);
        yield return 0;
    }
}
