using UnityEngine;

[ExecuteInEditMode]
public class MouseDrag : MonoBehaviour {
	private Camera cam;

	void Start() {
		cam = Camera.main;
	}

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // 射线检测
        if (Physics.Raycast(ray, out hitInfo))
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