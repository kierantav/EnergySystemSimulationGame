using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerController : MonoBehaviour
{
    public BreakerPanelHelper breakerPanelHelper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        
    }

    public void OnMouseDown()
    {
        breakerPanelHelper.gameObject.SetActive(true);
    }
}
