using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceOptionsPanelHelper : MonoBehaviour
{
    public Button closeApplianceOptionsBtn;
    public Button smallApplianceOptionBtn, mediumApplianceOptionBtn, largeApplianceOptionBtn;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        closeApplianceOptionsBtn.onClick.AddListener(CloseApplianceOptionsPanel);
        smallApplianceOptionBtn.onClick.AddListener(PurchaseSmallApplianceHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PurchaseSmallApplianceHandler()
    {
        throw new NotImplementedException();
    }

    private void CloseApplianceOptionsPanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenApplianceOptionsPanel()
    {
        gameObject.SetActive(true);
    }
}
