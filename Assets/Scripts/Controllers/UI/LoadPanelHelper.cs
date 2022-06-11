using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelHelper : MonoBehaviour
{
    public UIController uiController;

    public GameObject applianceLoadPanel;
    public GameObject appliancePanelPrefab;
    public Button closeLoadPanelBtn;
    public TextMeshProUGUI currentLoadText, noAppliancesText;

    private int loadPanelChildCount = 0;
    private float currentLoad;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        closeLoadPanelBtn.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLoadPanel();
        UpdateCurrentLoadText();
    }

    private void UpdateCurrentLoadText()
    {
        currentLoadText.text = Math.Round(currentLoad, 2).ToString();
    }

    private void UpdateLoadPanel()
    {
        List<ApplianceBaseSO> applianceData = GetApplianceData();
        if (loadPanelChildCount != applianceData.Count)
        {
            ClearPanel();
            UpdateAppliancesInLoadPanel(applianceLoadPanel.transform);
            loadPanelChildCount = applianceData.Count;
        }
        UpdateNoAppliancesText(applianceData);
    }

    private List<ApplianceBaseSO> GetApplianceData() 
    {
        if (uiController.InstalledAppliances != null)
        {
            return new List<ApplianceBaseSO>(uiController.InstalledAppliances);
        }
        return new List<ApplianceBaseSO>();
    }
    private void UpdateNoAppliancesText(List<ApplianceBaseSO> applianceData)
    {
        if (applianceData.Count > 0)
        {
            noAppliancesText.gameObject.SetActive(false);
        }
        else
        {
            noAppliancesText.gameObject.SetActive(true);
        }
    }

    private void ClearPanel()
    {
        GameObject[] allChildren = new GameObject[applianceLoadPanel.transform.childCount];
        for (int i = 0; i < applianceLoadPanel.transform.childCount; i++)
        {
            allChildren[i] = applianceLoadPanel.transform.GetChild(i).gameObject;
        }

        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void UpdateAppliancesInLoadPanel(Transform panelTransform)
    {
        List<ApplianceBaseSO> applianceData = new List<ApplianceBaseSO>(uiController.InstalledAppliances);
        if (applianceData.Count > panelTransform.childCount)
        {
            for (int i = 0; i < applianceData.Count; i++)
            {
                Instantiate(appliancePanelPrefab, panelTransform);
            }

            for (int index1 = 0; index1 < applianceData.Count; index1++)
            {
                var child = panelTransform.GetChild(index1);

                if (child != null)
                {
                    child.GetComponentsInChildren<Image>()[1].sprite = applianceData[index1].objectIcon;
                    child.GetComponentsInChildren<TextMeshProUGUI>()[0].text = applianceData[index1].objectName;
                    AddListenerToApplianceSwitch(applianceData[index1], child);
                }
            }
        }
    }

    private void AddListenerToApplianceSwitch(ApplianceBaseSO appliance, Transform child)
    {
        // Update switch status if appliance is turned on
        SwitchManager applianceSwitch = child.GetChild(2).GetChild(0).GetComponent<SwitchManager>();
        if (appliance.isTurnedOn)
        {
            applianceSwitch.isOn = true;
        }

        // Add listener to switch btn
        Button applianceSwitchBtn = child.GetChild(2).GetChild(0).GetComponent<Button>();
        if (applianceSwitchBtn.onClick.GetPersistentEventCount() < 1)
        {
            applianceSwitchBtn.onClick.AddListener(() => ToggleSwitch(appliance));
        }
    }

    private void ToggleSwitch(ApplianceBaseSO appliance)
    {
        List<ApplianceBaseSO> applianceData = new List<ApplianceBaseSO>(uiController.InstalledAppliances);
        if (applianceData != null)
        {
            foreach (var obj in applianceData)
            {
                if (obj.objectName.Equals(appliance.objectName) && !obj.isTurnedOn)
                {
                    currentLoad += obj.powerNeededRate;
                    obj.isTurnedOn = true;
                }
                else if (obj.objectName.Equals(appliance.objectName))
                {
                    currentLoad -= obj.powerNeededRate;
                    obj.isTurnedOn = false;
                    obj.powerNeededAmount = 0f;
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(0)) return;

        uiController.isUIClicked(true);
    }
}