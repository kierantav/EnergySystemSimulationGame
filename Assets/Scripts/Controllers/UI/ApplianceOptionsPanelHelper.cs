using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceOptionsPanelHelper : MonoBehaviour
{
    //private Action<string> OnApplianceHandler;
    //GameController gameController;

    public GameObject applianceOptionBtnPrefab;
    public GameObject applianceOptionsPanel;
    public UIController uiController;
    public Image titleImage;
    public TextMeshProUGUI titleText;
    public Button closeApplianceOptionsBtn;

    private string objectName;

    private List<ApplianceBaseSO> applianceData;
    List<ApplianceBaseSO> applianceOptions = new List<ApplianceBaseSO>();

    public string ObjectName { get => objectName; set => objectName = value; }
    public List<ApplianceBaseSO> ApplianceData { get => applianceData; set => applianceData = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        closeApplianceOptionsBtn.onClick.AddListener(CloseApplianceOptionsPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateNamesInApplianceOptionsPanel(Transform panelTransform, string objectName)
    {
        ClearPanel();

        switch (objectName)
        {
            case "Air Conditioner":
                applianceOptions = new List<ApplianceBaseSO>(ApplianceData.FindAll(item => item.objectDescription == objectName));
                break;
            case "Washing Machine":
                applianceOptions = new List<ApplianceBaseSO>(ApplianceData.FindAll(item => item.objectDescription == objectName));
                break;
            case "Fridge":
                applianceOptions = new List<ApplianceBaseSO>(ApplianceData.FindAll(item => item.objectDescription == objectName));
                break;
            default:
                break;
        }
        if (applianceOptions.Count > panelTransform.childCount)
        {
            int quantityDifference = applianceOptions.Count - panelTransform.childCount;
            for (int i = 0; i < quantityDifference; i++)
            {
                Instantiate(applianceOptionBtnPrefab, panelTransform);
            }
        }
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();

            if (button != null)
            {
                ApplianceBaseSO objectData = applianceOptions[i];
                // normal
                button.GetComponentsInChildren<TextMeshProUGUI>()[0].text = objectData.objectName;
                button.GetComponentsInChildren<TextMeshProUGUI>()[1].text = objectData.emissionRate.ToString() + " kgco2/kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[2].text = objectData.powerNeededRate.ToString() + " kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[3].text = "$ " + objectData.purchaseCost.ToString();

                // highlighted
                button.GetComponentsInChildren<TextMeshProUGUI>()[4].text = objectData.objectName;
                button.GetComponentsInChildren<TextMeshProUGUI>()[5].text = objectData.emissionRate.ToString() + " kgco2/kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[6].text = objectData.powerNeededRate.ToString() + " kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[7].text = "$ " + objectData.purchaseCost.ToString();

                titleImage.sprite = objectData.objectIcon;
                titleText.text = objectData.objectDescription.ToString();

                button.onClick.AddListener(() => OnApplianceCallback(objectData.objectDescription.ToString(), objectData.objectName));
            }
        }
    }

    private void OnApplianceCallback(string applianceType, string applianceName)
    {
        switch (applianceType)
        {
            case "Air Conditioner":
                OnPurchaseAC(applianceType, applianceName);
                break;
            case "Washing Machine":

                break;
            case "Fridge":

                break;
            default:
                break;
        }
    }

    private void OnPurchaseAC(string applianceType, string applianceName)
    {
        //this.gameController.TransitionToState()
        uiController.OnShopCallback(applianceType, applianceName);
    }

    private void ClearPanel()
    {
        GameObject[] allChildren = new GameObject[applianceOptionsPanel.transform.childCount];
        for (int i = 0; i < applianceOptionsPanel.transform.childCount; i++)
        {
            allChildren[i] = applianceOptionsPanel.transform.GetChild(i).gameObject;
        }

        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void CloseApplianceOptionsPanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenApplianceOptionsPanel()
    {
        CreateNamesInApplianceOptionsPanel(applianceOptionsPanel.transform, ObjectName);
        gameObject.SetActive(true);
    }
}
