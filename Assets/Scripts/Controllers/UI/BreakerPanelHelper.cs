using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakerPanelHelper : MonoBehaviour
{
    public UIController uiController;
    public ResourceController resourceController;

    public SwitchManager invertorSwitch, mainLoadSwitch, dieselGeneratorSwitch;
    public TextMeshProUGUI currentLoad;
    public GameObject noAppliancesText;
    public GameObject mainLoadPanel;
    public GameObject applianceSwitchPanel;
    public GameObject appliancePanelPrefab;
    public SwitchManager applianceSwitch1, applianceSwitch2, applianceSwitch3, applianceSwitch4;
    public Button aSwitch1Btn, aSwitch2Btn;
    public TMP_InputField loadValue;
    public Button saveBtn, closeLoadBtn, closeBreakerPanelBtn, openLoadPanelBtn;
    public NotificationManager saveddNotification;
    public NotificationManager failedNotification;

    private float load = 0;
    private bool isInterverSwitchOn = false;
    private bool isMainLoadSwitchOn = false;
    private bool isDGSwitchOn = false;

    public bool IsInterverSwitchOn { get => isInterverSwitchOn;  }
    public bool IsMainLoadSwitchOn { get => isMainLoadSwitchOn; }
    public bool IsDGSwitchOn { get => isDGSwitchOn; }
    public float Load { get => load; set => load = value; }

    private ApplianceBaseSO applianceData;
    private List<ApplianceBaseSO> applianceList;
    List<SwitchManager> switches = new List<SwitchManager>();

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(closeLoadBtn);
        gameObject.SetActive(false);
        HideLoadPanel();
        HideSwitches();
        noAppliancesText.gameObject.SetActive(false);


        switches.Add(applianceSwitch1);
        switches.Add(applianceSwitch2);
        switches.Add(applianceSwitch3);
        switches.Add(applianceSwitch4);


        UpdateLoadValueUI();
        //saveBtn.onClick.AddListener(Save);
        applianceData = ScriptableObject.CreateInstance<NullApplianceSO>();
        closeLoadBtn.onClick.AddListener(CloseLoadPanel);
        closeBreakerPanelBtn.onClick.AddListener(CloseBreakerPanel);
        openLoadPanelBtn.onClick.AddListener(ShowLoadPanel);
        aSwitch1Btn.onClick.AddListener(ToggleAppliance1Switch);
        aSwitch2Btn.onClick.AddListener(ToggleAppliance2Switch);
    }

    private void HideSwitches()
    {
        applianceSwitch1.gameObject.SetActive(false);
        applianceSwitch2.gameObject.SetActive(false);
        applianceSwitch3.gameObject.SetActive(false);
        applianceSwitch4.gameObject.SetActive(false);
    }

    private void ToggleAppliance1Switch()
    {
        if (applianceSwitch1.isOn)
        {
            applianceList[0].isTurnedOn = true;
            load += applianceList[0].powerNeededRate;
        }
        else
        {
            applianceList[0].isTurnedOn = false;
            load -= applianceList[0].powerNeededRate;
        }
        currentLoad.text = "Current Load: " + load + " kwh";
    }

    private void ToggleAppliance2Switch()
    {
        if (applianceSwitch2.isOn)
        {
            applianceList[1].isTurnedOn = true;
            load += applianceList[1].powerNeededRate;
        }
        else
        {
            applianceList[1].isTurnedOn = false;
            load -= applianceList[1].powerNeededRate;
        }
        currentLoad.text = "Current Load: " + load + " kwh";
    }

    private void Save()
    {
        GetLoadValue();
        //UpdateLoadValueUI();
    }

    public void UpdateLoadValueUI()
    {
        currentLoad.text = "Current Load : " + load + " kwh";
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(load);
        UpdateLoadValueUI();*/

        UpdateMainLoadUI();
        UpdateInvertorSwitchUI();
        UpdateDieselGeneratorSwitchUI();
    }

    private void UpdateDieselGeneratorSwitchUI()
    {
        if (dieselGeneratorSwitch.isOn)
        {
            isDGSwitchOn = true;
        }
        else
        {
            isDGSwitchOn = false;
        }
    }

    private void UpdateInvertorSwitchUI()
    {
        if (invertorSwitch.isOn)
        {
            isInterverSwitchOn = true;
        }
        else
        {
            isInterverSwitchOn = false;
        }
    }

    private void UpdateMainLoadUI()
    {
        if (mainLoadSwitch.isOn)
        {
            isMainLoadSwitchOn = true;
        }
        else
        {
            isMainLoadSwitchOn = false;
        }
    }

    private void GetLoadValue()
    {
        try
        {
            load = float.Parse(loadValue.text);
            saveddNotification.OpenNotification();
        }
        catch
        {
            failedNotification.OpenNotification();
        }
    }

    public void HideLoadPanel()
    {
        mainLoadPanel.SetActive(false);
    }

    public void ShowLoadPanel()
    {

        //Debug.Log(uiController.applianceObjectController);
        //Debug.Log(uiController.InstalledAppliances);
        CreateAppliancesInLoadPanel(applianceSwitchPanel.transform, uiController.InstalledAppliances);
        if (uiController.InstalledAppliances != null)
        {
            this.applianceList = uiController.InstalledAppliances;
        }
        mainLoadPanel.SetActive(true);
    }

    private void CreateAppliancesInLoadPanel(Transform panelTransform, List<ApplianceBaseSO> data)
    {
        if (data == null || data.Count == 0)
        {
            noAppliancesText.gameObject.SetActive(true);
            ClearAppliancesInLoadPanel(panelTransform);
        }
        else
        {
            noAppliancesText.gameObject.SetActive(false);
            UpdateAppliancesInLoadPanel(panelTransform, data);
        }
    }

    private void UpdateAppliancesInLoadPanel(Transform panelTransform, List<ApplianceBaseSO> data)
    {
        //Debug.Log(data.Count + "-" + panelTransform.childCount);
        if (data.Count > panelTransform.childCount)
        {
            int quantityDifference = data.Count - panelTransform.childCount;
            for (int index3 = 0; index3 < quantityDifference; index3++)
            {
                Instantiate(appliancePanelPrefab, panelTransform);
                if (switches[index3].gameObject.activeSelf)
                {
                    switches[index3 + 1].gameObject.SetActive(true);
                }
                else
                {
                    switches[index3].gameObject.SetActive(true);
                }
            }

            for (int index1 = 0; index1 < panelTransform.childCount; index1++)
            {
                var child = panelTransform.GetChild(index1);
                //Transform[] transforms = panelTransform.GetChild(index1).GetComponentsInChildren<Transform>();

                if (child != null)
                {
                    child.GetComponentsInChildren<TextMeshProUGUI>()[0].text = data[index1].objectName;
                    child.GetComponentsInChildren<Image>()[1].sprite = data[index1].objectIcon;
                    //this.applianceList[index1] = data[index1];
                }
            }
        }
        else if (data.Count < panelTransform.childCount)
        {
            var lastSwitch = switches[panelTransform.childCount - 1];
            lastSwitch.gameObject.SetActive(false);

            for (int index2 = 0; index2 < panelTransform.childCount; index2++)
            {
                
                var child = panelTransform.GetChild(index2); // air con
                foreach (var appliance in data)
                {
                    // if (air con != air con)
                    //Debug.Log(child.GetComponentsInChildren<TextMeshProUGUI>()[0].text + "!=" + appliance.objectName);
                    if (child.GetComponentsInChildren<TextMeshProUGUI>()[0].text.Equals(appliance.objectName) == false)
                    {
                        Destroy(child.gameObject);
                    }
                }

            }
        }
    }

    private void ClearAppliancesInLoadPanel(Transform panelTransform)
    {
        for (int count = 0; count < panelTransform.childCount; count++)
        {
            var child = panelTransform.GetChild(count);
            if (child != null)
            {
                Destroy(child.gameObject);
                
            }
        }
        HideSwitches();
    }

    private void CloseLoadPanel()
    {
        HideLoadPanel();
    }
    private void CloseBreakerPanel()
    {
        gameObject.SetActive(false);
    }

}
