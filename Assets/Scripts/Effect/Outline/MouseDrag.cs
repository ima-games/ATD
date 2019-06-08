using UnityEngine;

[ExecuteInEditMode]
public class MouseDrag : MonoBehaviour {
	private Camera cam;

	void Start() {
		cam = Camera.main;
	}

    void Update()
    {
        //TODO
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        int mask = (1 << 11);
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
}