using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class StatsPanelHelper : MonoBehaviour
{
    public UIController uiController;
    public GameObject energySystemsPanel;
    public GameObject renewablesPanel;
    public GameObject statsPanelPrefab;
    private TextMeshProUGUI dieselCo2ProducedText;
    private TextMeshProUGUI gridPowerCo2ProducedText;
    private string objectName;
    private int delayAmount = 1;
    private double co2 = 0;

    private int dieselIndex;
    private int gridPowerIndex;

    List<EnergySystemGeneratorBaseSO> energySystemData;
    List<EnergySystemGeneratorBaseSO> renewablesData;

    //private float nextActionTime = 0.0f;
    //public float period = 0.1f;

    protected float Timer;

    // Start is called before the first frame update
    void Start()
    {
        //newData = UpdateData(uiController.InstalledEnergySystems);
        InvokeRepeating("UpdateAmountProduced", 0.01f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= delayAmount)
        {
            Timer = 0f;
            co2 += 0.22;
            //Debug.Log(co2);
            //co2ProducedText.text = co2.ToString() + " kg";
        }
    }

    private void UpdateAmountProduced()
    {
        if (energySystemData != null)
        {
            if (dieselCo2ProducedText != null)
            {
                dieselCo2ProducedText.text = Math.Round(energySystemData[dieselIndex].emissionGeneratedAmount, 2) + " kg";
            }
            if (gridPowerCo2ProducedText != null)
            {
                gridPowerCo2ProducedText.text = Math.Round(energySystemData[gridPowerIndex].emissionGeneratedAmount, 2) + " kg";
            }
        }
    }

    public void CreateEnergySystemsInStatsMenu(List<EnergySystemGeneratorBaseSO> data)
    {
        List<EnergySystemGeneratorBaseSO> newData = data;
        if (newData == null || newData.Count == 0)
        {
            Debug.Log("No energy systems installed!");
            //noAppliancesText.gameObject.SetActive(true);
            //ClearAppliancesInLoadPanel(panelTransform);
        }
        else
        {
            energySystemData = UpdateData(newData);
            //noAppliancesText.gameObject.SetActive(false);
            UpdateEnergySystemsInStatsPanel(energySystemsPanel.transform, newData);
            //List<EnergySystemGeneratorBaseSO> renewablesData = UpdateRenewablesData(newData);
            //UpdateEnergySystemsInStatsPanel(renewablesPanel.transform, data);
        }
    }

    public void CreateRenewablesInStatsMenu(List<EnergySystemGeneratorBaseSO> data)
    {
        List<EnergySystemGeneratorBaseSO> newData = data;
        if (newData == null || newData.Count == 0)
        {
            Debug.Log("No renewables installed!");
            //noAppliancesText.gameObject.SetActive(true);
            //ClearAppliancesInLoadPanel(panelTransform);
        }
        else
        {
            renewablesData = UpdateRenewablesData(newData);
            //noAppliancesText.gameObject.SetActive(false);
            UpdateEnergySystemsInStatsPanel(renewablesPanel.transform, newData);
            //List<EnergySystemGeneratorBaseSO> renewablesData = UpdateRenewablesData(newData);
            //UpdateEnergySystemsInStatsPanel(renewablesPanel.transform, data);
        }
    }

    private void UpdateEnergySystemsInStatsPanel(Transform panelTransform, List<EnergySystemGeneratorBaseSO> data)
    {
        //ClearApplianceSwitches();
        //AddApplianceSwitches();
        //Debug.Log(data.Count + "-" + panelTransform.childCount);
        List<EnergySystemGeneratorBaseSO> newData = UpdateData(data);
        if (newData.Count > panelTransform.childCount)
        {
            int quantityDifference = newData.Count - panelTransform.childCount;
            for (int index3 = 0; index3 < quantityDifference; index3++)
            {
                Instantiate(statsPanelPrefab, panelTransform);
            }

            for (int index1 = 0; index1 < panelTransform.childCount; index1++)
            {
                var child = panelTransform.GetChild(index1);

                if (child != null)
                {
                    child.GetComponentsInChildren<TextMeshProUGUI>()[0].text = newData[index1].objectName;
                    child.GetComponentsInChildren<Image>()[1].sprite = newData[index1].objectIcon;
                    if (newData[index1].objectName.Equals("Diesel Generator"))
                    {
                        dieselCo2ProducedText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                        dieselIndex = index1;
                    }
                    else
                    {
                        gridPowerIndex = index1;
                        gridPowerCo2ProducedText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                    }

                    //objectName = newData[index1].objectName;
                    //InvokeRepeating("UpdateCo2Text", 1.0f, 1.0f);
                }
            }
        }
        else if (newData.Count < panelTransform.childCount)
        {
            for (int index2 = 0; index2 < panelTransform.childCount; index2++)
            {
                var child = panelTransform.GetChild(index2);
                foreach (var energySystem in newData)
                {
                    if (child.GetComponentsInChildren<TextMeshProUGUI>()[0].text.Equals(energySystem.objectName) == false)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }

    void UpdateCo2Text()
    {
        dieselCo2ProducedText.text = GetCo2Produced(objectName);
    }

    private string GetCo2Produced(string objectName)
    {
        foreach (var energySystem in uiController.InstalledEnergySystems)
        {
            if (energySystem.objectName.Equals(objectName))
            {
                return co2.ToString() + " kg";
            }
        }
        return "";
    }

    private List<EnergySystemGeneratorBaseSO> UpdateData(List<EnergySystemGeneratorBaseSO> data)
    {
        List<EnergySystemGeneratorBaseSO> newData = data;
        if (newData != null || newData.Count > 0)
        {
            for (int i = 0; i < newData.Count; i++)
            {
                if (!newData[i].objectName.Equals("Diesel Generator") && !newData[i].objectName.Equals("On-Grid Power"))
                {
                    newData.Remove(newData[i]);
                }
            }
        }
        return newData;
    }

    private List<EnergySystemGeneratorBaseSO> UpdateRenewablesData(List<EnergySystemGeneratorBaseSO> data)
    {
        List<EnergySystemGeneratorBaseSO> newData = data;
        if (newData != null || newData.Count > 0)
        {
            for (int i = 0; i < newData.Count; i++)
            {
                if (!newData[i].objectName.Equals("Solar Panel") && !newData[i].objectName.Equals("Wind Turbine"))
                {
                    newData.Remove(newData[i]);
                }
            }
        }
        return newData;
    }
}