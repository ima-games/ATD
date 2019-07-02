using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class TowerSelect : MonoBehaviour {
	private Camera cam;
    private int towerBaseMask;
    private int UIMask;
    private bool selceting = false;
    private Outline outline;
    private TowerManager towerManager;
    //建造塔的UI面板
    public GameObject constructPanel;

    private void Awake()
    {
        cam = Camera.main;
        outline = cam.GetComponent<Outline>();
        towerBaseMask = (1 << LayerMask.NameToLayer("TowerBase"));
        UIMask = (1 << LayerMask.NameToLayer("UI"));

        towerManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TowerManager>();

        UnshowPanel();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        //没有选择一个塔基或者重新点击鼠标时
        if (!selceting || Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) //鼠标放到UI上
            {
                //do Notthing
            }
            else if (Physics.Raycast(ray, out hitInfo, 1000, towerBaseMask, new QueryTriggerInteraction())) //射线检测到塔基对象
            {
                //在scene视图中显示检测射线
                Debug.DrawLine(ray.origin, hitInfo.point, Color.green);

                var target = hitInfo.collider.gameObject;
                //更新towerManager
                towerManager.TargetTowerBase = target;
                // 描边
                outline.enabled = false;
                outline.targetObject = target;
                outline.enabled = true;

                if (Input.GetMouseButton(0))
                {
                    ShowPanel();
                }
            }
            else
            {
                //更新towerManager
                towerManager.TargetTowerBase = null;

                UnshowPanel();
            }
        }
    }

    public void ShowPanel ()
    {
        constructPanel.transform.position = Input.mousePosition;
        constructPanel.SetActive(true);
        selceting = true;
    }

    public void UnshowPanel()
    {
        constructPanel.SetActive(false);
        outline.enabled = false;
        outline.targetObject = null;
        selceting = false;
    }

    //禁用时，取消原先边缘显示
    public void OnDisable()
    {
        outline.enabled = false;
        outline.targetObject = null;
    }

}