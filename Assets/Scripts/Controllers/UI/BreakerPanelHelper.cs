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

    public SwitchManager invertorSwitch, mainLoadSwitch, dieselGeneratorSwitch, mainSwitch;
    public Button invertorBtn, dieselGeneratorBtn, mainBtn;
    public Button checkEnergySystemBtn;
    public TextMeshProUGUI currentLoad;
    public TextMeshProUGUI energySystemWarningText;
    public GameObject noAppliancesText;
    public GameObject mainLoadPanel;
    public GameObject applianceSwitchPanel;
    public GameObject appliancePanelPrefab;
    public SwitchManager applianceSwitch1, applianceSwitch2, applianceSwitch3, applianceSwitch4;
    public Button aSwitch1Btn, aSwitch2Btn, aSwitch3Btn, aSwitch4Btn;
    public TMP_InputField loadValue;
    public Button saveBtn, closeLoadBtn, closeBreakerPanelBtn, openLoadPanelBtn;
    public NotificationManager saveddNotification;
    public NotificationManager failedNotification;

    private float load = 0;
    private bool isInterverSwitchOn = false;
    private bool isMainLoadSwitchOn = false;
    private bool isDGSwitchOn = false;
    private bool isMainSwitchOn = false;

    public bool IsInterverSwitchOn { get => isInterverSwitchOn;  }
    public bool IsMainLoadSwitchOn { get => isMainLoadSwitchOn; }
    public bool IsDGSwitchOn { get => isDGSwitchOn; }
    public bool IsMainSwitchOn { get => isMainSwitchOn; }
    public float Load { get => load; set => load = value; }

    private List<ApplianceBaseSO> applianceList;
    List<SwitchManager> switches = new List<SwitchManager>();
    private List<Appliance> appliances;

    private List<EnergySystemGeneratorBaseSO> energySystemData;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        HideLoadPanel();
        HideSwitches();
        noAppliancesText.gameObject.SetActive(false);
        energySystemWarningText.gameObject.SetActive(false);
        AddSwitchesToList();

        UpdateLoadValueUI();
        closeLoadBtn.onClick.AddListener(CloseLoadPanel);
        closeBreakerPanelBtn.onClick.AddListener(CloseBreakerPanel);
        openLoadPanelBtn.onClick.AddListener(ShowLoadPanel);
        aSwitch1Btn.onClick.AddListener(ToggleAppliance1Switch);
        aSwitch2Btn.onClick.AddListener(ToggleAppliance2Switch);
        aSwitch3Btn.onClick.AddListener(ToggleAppliance3Switch);
        mainBtn.onClick.AddListener(ToggleMainSwitch);
        //aSwitch4Btn.onClick.AddListener(ToggleAppliance4Switch);

        //checkEnergySystemBtn.onClick.AddListener(isEnergySystemInstalledAndOn);
    }

    private void ToggleMainSwitch()
    {
        EnergySystemGeneratorBaseSO gridPower = GetGridPowerObject();
        if (mainSwitch.isOn && gridPower != null)
        {
            gridPower.isRunning = false;
            gridPower.isTurnedOn = false;
            Debug.Log(gridPower.isRunning);
        } else if (!mainSwitch.isOn && gridPower != null)
        {
            gridPower.isRunning = true;
            gridPower.isTurnedOn = true;
            Debug.Log(gridPower.isRunning);
        } else
        {
            Debug.Log("House must be connected to the grid!");
        }
        
    }

    private void isEnergySystemInstalledAndOn()
    {
        if (isInvertorOn())
        {
            checkEnergySystemBtn.gameObject.SetActive(false);
        } else
        {
            energySystemWarningText.gameObject.SetActive(true);
        }
    }

    private EnergySystemGeneratorBaseSO GetGridPowerObject()
    {
        this.energySystemData = uiController.InstalledEnergySystems;
        if (energySystemData != null)
        {
            foreach (var obj in energySystemData)
            {
                if (obj.objectName.Equals("On-Grid Power"))
                {
                    return obj;
                }
            }
        }
        return null;
    }

    private void AddSwitchesToList()
    {
        switches.Add(applianceSwitch1);
        switches.Add(applianceSwitch2);
        switches.Add(applianceSwitch3);
        switches.Add(applianceSwitch4);
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
        UpdateLoadValueUI();
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
        UpdateLoadValueUI();
    }

    private void ToggleAppliance3Switch()
    {
        //Debug.Log(applianceList[2].objectName);
        if (!applianceSwitch3.isOn)
        {
            applianceList[2].isTurnedOn = true;
            load += applianceList[2].powerNeededRate;
        }
        else
        {
            applianceList[2].isTurnedOn = false;
            load -= applianceList[2].powerNeededRate;
        }
        UpdateLoadValueUI();
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
        UpdateMainSwitch();
    }

    private void UpdateMainSwitch()
    {
        if (mainSwitch.isOn)
        {
            isMainSwitchOn = true;
        }
        else
        {
            isMainSwitchOn = false;
        }
    }

    /*private void toggleEnergySystem(string objectName)
    {
        if (uiController.InstalledEnergySystems != null || uiController.InstalledEnergySystems.Count == 0)
        {
            foreach (var energySystem in uiController.InstalledEnergySystems)
            {
                if (energySystem.objectName.Equals(objectName))
                {
                    if (!energySystem.isTurnedOn && !energySystem.isRunning)
                    {
                        energySystem.isTurnedOn = true;
                        energySystem.isRunning = true;
                    } else
                    {
                        energySystem.isTurnedOn = false;
                        energySystem.isRunning = false;
                    }
                    Debug.Log(energySystem.isRunning);
                }
            }
        }
    }*/

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

    private bool isInvertorOn()
    {
        this.energySystemData = uiController.InstalledEnergySystems;
        if (energySystemData != null)
        {
            foreach (var obj in energySystemData)
            {
                if (obj.objectName.Equals("Invertor") && obj.isTurnedOn)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool isDieselGeneratorOn()
    {
        this.energySystemData = uiController.InstalledEnergySystems;
        if (energySystemData != null)
        {
            foreach (var obj in energySystemData)
            {
                if (obj.objectName.Equals("Diesel Generator") && obj.isTurnedOn)
                {
                    return true;
                }
            }
        }
        return false;
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
        ClearApplianceSwitches();
        AddApplianceSwitches();
        //Debug.Log(data.Count + "-" + panelTransform.childCount);
        if (data.Count > panelTransform.childCount)
        {
            int quantityDifference = data.Count - panelTransform.childCount;
            for (int index3 = 0; index3 < quantityDifference; index3++)
            {
                Instantiate(appliancePanelPrefab, panelTransform);
            }

            for (int index1 = 0; index1 < panelTransform.childCount; index1++)
            {
                var child = panelTransform.GetChild(index1);

                if (child != null)
                {
                    child.GetComponentsInChildren<TextMeshProUGUI>()[0].text = data[index1].objectName;
                    child.GetComponentsInChildren<Image>()[1].sprite = data[index1].objectIcon;
                }
            }
        }
        else if (data.Count < panelTransform.childCount)
        {
            for (int index2 = 0; index2 < panelTransform.childCount; index2++)
            {
                var child = panelTransform.GetChild(index2);
                foreach (var appliance in data)
                {
                    if (child.GetComponentsInChildren<TextMeshProUGUI>()[0].text.Equals(appliance.objectName) == false)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }

    private void AddApplianceSwitches()
    {
        for (int i = 0; i < uiController.InstalledAppliances.Count; i++)
        {
            if (!switches[i].gameObject.activeSelf)
            {
                switches[i].gameObject.SetActive(true);
            }
        }
    }

    private void ClearApplianceSwitches()
    {
        foreach (var appSwitch in switches)
        {
            appSwitch.gameObject.SetActive(false);
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
        energySystemWarningText.gameObject.SetActive(false);
        checkEnergySystemBtn.gameObject.SetActive(true);
    }

}

struct Appliance
{
    public ApplianceBaseSO appliance;
    public SwitchManager applianceSwitch;

    public Appliance(ApplianceBaseSO appliance, SwitchManager applianceSwitch)
    {
        this.appliance = appliance;
        this.applianceSwitch = applianceSwitch;
    }
}
