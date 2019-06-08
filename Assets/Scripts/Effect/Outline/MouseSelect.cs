using UnityEngine;

[ExecuteInEditMode]
public class MouseSelect : MonoBehaviour {
	private Camera cam;
    private int mask;

    private void Awake()
    {
        cam = Camera.main;
        mask = (1 << LayerMask.NameToLayer("TowerBase"));
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // 射线检测
        if (Physics.Raycast(ray,out hitInfo,10000, mask, new QueryTriggerInteraction()))
        {
            //在scene视图中显示检测射线
            Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
            var target = hitInfo.collider.gameObject;
            // 描边
            GetComponent<Outline>().enabled = false;
            GetComponent<Outline>().targetObject = target;
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
            GetComponent<Outline>().targetObject = null;
        }
    }

    //禁用时，取消原先边缘显示
    public void OnDisable()
    {
        GetComponent<Outline>().enabled = false;
        GetComponent<Outline>().targetObject = null;
    }
}