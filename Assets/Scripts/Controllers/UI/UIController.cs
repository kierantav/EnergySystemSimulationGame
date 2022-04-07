/*======================================================*
|  Author: Yifan Song
|  Creation Date: 19/08/2021
|  Latest Modified Date: 19/08/2021
|  Description: To manager UI elements and assign event to them
|  Bugs: N/A
*=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Use delegate to inform the game manager about buttons clicked
    private Action<string> OnShopHandler;
    private Action<string> OnApplianceHandler;
    private Action OnCancelHandler;
    private Action OnConfirmHandler;
    private Action OnSellActionHandler;
    //private Action OnSellApplianceActionHandler;

    // modify - cancel & confirm to place or remove objects
    [Header("Constructure Modified Panel Property")]
    public GameObject modifyPanel;
    public Button cancelBtn;
    public Button confirmBtn;

    public Button openShopMenuBtn;
    public Button closeShopMenuBtn;

    // shop - install or sell energy systems and appliances
    [Header("Shop Panel Property")]
    public GameObject shopMenuPanel;


    public Button sellBtn;
    public Button openEnergySystemWindowBtn;
    public Button openApplianceWindowBtn;
    public GameObject energySystemPurchasePanel;
    public GameObject appliancePurchasePanel;
    public GameObject itemBtnPrefab; // to list all selections
    public TooltipManager itemTooltip;
    //public GameObject applianceTempObject; // to create empty gameobject based on appliance type

    [Header("Main Menu Panel Property")]
    public GameObject mainMenuPanel;
    public Button openStatsMenuBtn;
    public Button closeStatsMenuBtn;
    // stats
    public GameObject stateMenuPanel;
    public Button openWeatherWindowBtn;
    public Button openPropertyWindowBtn;
    public GameObject weatherWindowPanel;
    public GameObject propertyWindowPanel;

    [Header("Repository")]
    public ApplianceObjectController applianceObjectController;
    public ObjectRepository objectRepository;
    public ApplianceRepository applianceRepository;

    [Header("Camera")]
    private CameraMovement cameraMovementController;

    [Header("Money Property")]
    public TextMeshProUGUI moneyValue;
    [Header("Level&Experience Property")]
    public TextMeshProUGUI levelValue;
    public ProgressBar experienceBar;
    public NotificationManager levelUpNotificationManager;

    [Header("Weather Data Property")]
    public TextMeshProUGUI weatherValue;
    public TextMeshProUGUI temperatureValue;
    public TextMeshProUGUI solarRadiationValue;
    public TextMeshProUGUI windSpeedValue;

    [Header("Date&Time Property")]
    public TextMeshProUGUI timeValue;
    public TextMeshProUGUI dateValue;

    [Header("Game Speed Property")]
    public Button increaseGameSpeedButton;
    public Button decreaseGameSpeedButton;
    public Button pauseGameButton;
    public Button resumeGameButton;
    public GameObject pausePlane;

    [Header("System Info")]
    public SystemInfoPanelHelper systemInfoPanelHelper;
    public BreakerPanelHelper breakerPanelHelper;





    public CameraMovement CameraMovementController { get => cameraMovementController; set => cameraMovementController = value; } // exploit the cameraMovementController to GameController

    void Start()
    {
        itemBtnPrefab.GetComponent<TooltipContent>().tooltipRect = itemTooltip.tooltipObject;
        itemBtnPrefab.GetComponent<TooltipContent>().descriptionText = itemTooltip.tooltipContent.GetComponentInChildren<TextMeshProUGUI>();

        mainMenuPanel.SetActive(true);
        shopMenuPanel.SetActive(false); // Hide shopMenuPanel until the player clicks on shop button
        modifyPanel.SetActive(false);   // Hide modifyPanel until the player is in shopMode
        stateMenuPanel.SetActive(false);

        cancelBtn.onClick.AddListener(OnCancelCallback);
        confirmBtn.onClick.AddListener(OnConfirmCallback);

        openShopMenuBtn.onClick.AddListener(OnShopMenu);
        sellBtn.onClick.AddListener(OnSellHandler);
        //sellBtn.onClick.AddListener(OnSellApplianceHandler);
        closeShopMenuBtn.onClick.AddListener(OnCloseShopMenuHandler);

        openStatsMenuBtn.onClick.AddListener(OnStatsMenu);
        closeStatsMenuBtn.onClick.AddListener(OnCloseStatsMenuHandler);
    }

    private void PurchaseFuel()
    {
        throw new NotImplementedException();
    }

    public void DisplaySystemInfo(EnergySystemGeneratorBaseSO data)
    {
        
        switch (data.objectName)
        {
            case ("Solar Panel"):
                systemInfoPanelHelper.DisplaySolarPanelInfo(data);
                break;
            case ("Diesel Generator"):
                systemInfoPanelHelper.DisplayDieselGeneratorInfo(data);
                break;
            case ("Invertor"):
                systemInfoPanelHelper.DisplayInvertorInfo(data);
                break;
            case ("Charge Controller"):
                systemInfoPanelHelper.DisplayChargeControllerInfo(data);
                break;
            case ("Battery"):
                systemInfoPanelHelper.DisplayBatteryInfo(data);
                break;
            case ("Wind Turbine"):
                systemInfoPanelHelper.DisplayWindTurbineInfo(data);
                break;
        }  
    }

    public void HideSystemInfo()
    {
        systemInfoPanelHelper.Hide();
    }

    
    public void DisplayApplianceInfo(ApplianceBaseSO applianceData)
    {
        switch (applianceData.objectName)
        {
            case ("Air Conditioner"):
                systemInfoPanelHelper.DisplayACInfo(applianceData);
                break;
            case ("Washing Machine"):
                //systemInfoPanelHelper.DisplayDieselGeneratorInfo(data);
                break;
        }
    }

    public bool GetSystemInfoVisibility()
    {
        return systemInfoPanelHelper.gameObject.activeSelf;
    }

    public void SetMoneyValue(int money)
    {
        moneyValue.text = money + " ";
    }

    public void SetExperienceValue(int level, int experience, int experienceToNextLevel)
    {
        experienceBar.restart = true;
        experienceBar.invert = false;
        levelValue.text = level + " ";
        float percentage = (float)experience /  experienceToNextLevel * 100f;
        experienceBar.speed = 45;
        if (experienceBar.currentPercent <= percentage)
        {
            experienceBar.targetPercent = percentage;
        }
        else
        {
            experienceBar.currentPercent = 0;
            experienceBar.targetPercent = percentage;
        }
    }

    public void SetWeatherValue( string weather, float temperature, float solar, float wind)
    {
        weatherValue.text = weather + " ";
        temperatureValue.text = temperature + " °C";
        solarRadiationValue.text = solar + " w/m2";
        windSpeedValue.text = wind + " m/s";
    }



    #region ModifyPanelButtonsCallback
    private void OnConfirmCallback()
    {
        modifyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        OnConfirmHandler?.Invoke();
        EnableCameraMovement(true);
    }

    private void OnCancelCallback()
    {
        modifyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        OnCancelHandler?.Invoke();
    }
    #endregion


    #region ShopMenuPanelCallback
    private void OnStatsMenu()
    {
        stateMenuPanel.SetActive(true);
        EnableCameraMovement(false);
    }

    private void OnCloseStatsMenuHandler()
    {
        stateMenuPanel.SetActive(false);
        EnableCameraMovement(true);
    }
    #endregion


    #region ShopMenuPanelCallback
    private void OnShopMenu()
    {
        shopMenuPanel.SetActive(true);
        EnableCameraMovement(false);
        PreparePurchaseMenu();
    }

    private void PreparePurchaseMenu()
    {
        CreateButtonsInEnergyPanel(energySystemPurchasePanel.transform, objectRepository.GetEnergySystemGeneratorObjects());
        CreateButtonsInAppliancePanel(appliancePurchasePanel.transform, applianceRepository.GetApplianceObjects());
    }

    private void CreateButtonsInEnergyPanel(Transform panelTransform, List<EnergySystemGeneratorBaseSO> data)
    {
        if (data.Count > panelTransform.childCount)
        {
            int quantityDifference = data.Count - panelTransform.childCount;
            for (int i = 0; i < quantityDifference; i++)
            {
                Instantiate(itemBtnPrefab, panelTransform);
            }
        }
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();

            if (button != null)
            {
                EnergySystemGeneratorBaseSO objectData = data[i];
                //normal
                button.GetComponentsInChildren<TextMeshProUGUI>()[0].text = objectData.objectName;
                button.GetComponentsInChildren<TextMeshProUGUI>()[1].text = objectData.emissionRate+" kgCO2/kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[2].text = objectData.powerGeneratedRate + "kw";
                button.GetComponentsInChildren<TextMeshProUGUI>()[3].text = "$" + objectData.purchaseCost;

                //highlight
                button.GetComponentsInChildren<TextMeshProUGUI>()[4].text = objectData.objectName;
                button.GetComponentsInChildren<TextMeshProUGUI>()[5].text = objectData.emissionRate + " kgCO2/kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[6].text = objectData.powerGeneratedRate + "kw";
                button.GetComponentsInChildren<TextMeshProUGUI>()[7].text = "$" + objectData.purchaseCost;

                //tooltip
                button.GetComponent<TooltipContent>().description = objectData.objectDescription.ToString();

                button.GetComponentsInChildren<Image>()[0].sprite = objectData.objectIcon;
                button.GetComponentsInChildren<Image>()[2].sprite = objectData.objectIcon;
                button.onClick.AddListener(() => OnShopCallback(button.GetComponentsInChildren<TextMeshProUGUI>()[0].text));
            }
        }
    }

    private void OnShopCallback(string objectName)
    {
        modifyPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        OnCloseShopMenuHandler();
        List<EnergySystemGeneratorBaseSO> energyObjList = objectRepository.GetEnergySystemGeneratorObjects();
        var foundObj = energyObjList.Find(obj => obj.objectName == objectName);
        if (foundObj != null)
        {
            OnShopHandler?.Invoke(objectName);// ? to check if it is null, will invoke the listener
        }
        else
        {
            OnApplianceHandler?.Invoke(objectName);
        }
    }

    private void CreateButtonsInAppliancePanel(Transform panelTransform, List<ApplianceBaseSO> data)
    {
        if (data.Count > panelTransform.childCount)
        {
            int quantityDifference = data.Count - panelTransform.childCount;
            for (int i = 0; i < quantityDifference; i++)
            {
                Instantiate(itemBtnPrefab, panelTransform);
            }
        }
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();

            if (button != null)
            {
                ApplianceBaseSO objectData = data[i];
                button.GetComponentsInChildren<TextMeshProUGUI>()[0].text = objectData.objectName;
                button.GetComponentsInChildren<TextMeshProUGUI>()[1].text = objectData.emissionRate+" kgCO2/kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[2].text = objectData.powerNeededRate+ "kw";
                button.GetComponentsInChildren<TextMeshProUGUI>()[3].text = "$ "+objectData.purchaseCost;
                button.GetComponentsInChildren<TextMeshProUGUI>()[4].text = objectData.objectName;
                button.GetComponentsInChildren<TextMeshProUGUI>()[5].text = objectData.emissionRate + " kgCO2/kwh";
                button.GetComponentsInChildren<TextMeshProUGUI>()[6].text = objectData.powerNeededRate + "kw";
                button.GetComponentsInChildren<TextMeshProUGUI>()[7].text = "$ " + objectData.purchaseCost;
                //button.GetComponentsInChildren<TextMeshProUGUI>()[8].text = objectData.objectDescription.ToString();
                button.GetComponentsInChildren<Image>()[0].sprite = objectData.objectIcon;
                button.GetComponentsInChildren<Image>()[2].sprite = objectData.objectIcon;
                button.onClick.AddListener(() => OnShopCallback(button.GetComponentsInChildren<TextMeshProUGUI>()[0].text));
            }
        }
    }

    //public void PurchasingApplianceObject(string objectName)
    //{
    //    applianceTempObject.name = objectName;
    //    Instantiate(applianceTempObject, Vector3.zero, Quaternion.identity);
    //}

    private void OnSellHandler()
    {
        OnSellActionHandler?.Invoke();
        OnCloseShopMenuHandler();
        modifyPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    /*private void OnSellApplianceHandler()
    {
        OnSellApplianceActionHandler?.Invoke();
        OnCloseShopMenuHandler();
        modifyPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }*/

    private void OnCloseShopMenuHandler()
    {
        shopMenuPanel.SetActive(false);
        EnableCameraMovement(true);
    }
    #endregion

    #region Listeners
    // Assign listener and remove listener
    public void AddListenerOnPurchasingEvent(Action<string> listener)
    {
        OnShopHandler += listener;
    }

    public void RemoveListenerOnShopEvent(Action<string> listener)
    {
        OnShopHandler -= listener;
    }

    public void AddListenerOnPurchasingApplianceEvent(Action<string> listener)
    {
        OnApplianceHandler += listener;
    }

    public void RemoveListenerOnShopApplianceEvent(Action<string> listener)
    {
        OnApplianceHandler -= listener;
    }

    public void AddListenerOnCancelEvent(Action listener)
    {
        OnCancelHandler += listener;
    }

    public void RemoveListenerOnCancelEvent(Action listener)
    {
        OnCancelHandler -= listener;
    }

    public void AddListenerOnConfirmEvent(Action listener)
    {
        OnConfirmHandler += listener;
    }

    public void RemoveListenerOnConfirmEvent(Action listener)
    {
        OnConfirmHandler -= listener;
    }

    public void AddListenerOnSellEvent(Action listener)
    {
        OnSellActionHandler += listener;
    }

    public void RemoveListenerOnSellEvent(Action listener)
    {
        OnSellActionHandler -= listener;
    }

    /*public void AddListenerOnSellApplianceEvent(Action listener)
    {
        OnSellApplianceActionHandler += listener;
    }

    public void RemoveListenerOnSellApplianceEvent(Action listener)
    {
        OnSellApplianceActionHandler -= listener;
    }*/
    #endregion

    private void EnableCameraMovement(bool setting)
    {
        // camera can't be moved when opening the shop menu
        if (setting)
        {
            this.cameraMovementController.isAvailable = true;
        }
        else
        {
            this.cameraMovementController.isAvailable = false;
        }
    }
}
