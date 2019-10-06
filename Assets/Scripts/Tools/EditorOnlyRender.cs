using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnlyRender : MonoBehaviour
{
    private Renderer renderer = null;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        if (!renderer) return;
        renderer.enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
