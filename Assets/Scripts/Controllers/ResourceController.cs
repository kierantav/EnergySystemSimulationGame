using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class ResourceController : MonoBehaviour, IResourceController
{
    [SerializeField]
    private int startMoneyAmount = 5000;
    [SerializeField]
    private float moneyCalculationInterval = 2; // every 2 second the player income will be calculated
    [SerializeField]
    private int removeCost = 20; // cost of removing an object
    MoneyHelper moneyHelper;

    [SerializeField]
    private int startLevel = 0;
    [SerializeField]
    private int experience = 0;
    [SerializeField]
    private int experienceToNextLevel = 100;
    LevelHelper levelHelper;

    private EnergySystemObjectController purchasingObjectController;
    public UIController uIController;
    public TimeController timeController;
    public int StartMoneyAmount { get => startMoneyAmount; }
    public float MoneyCalculationInterval { get => moneyCalculationInterval;}
    int IResourceController.removeCost { get => removeCost; }

    private string startTime;
    private float currentPoA;
    private float previousPoA; 

    private PowerHelper powerHelper;
    public MeterHelper meterHelper;

    //public NotificationManager popupNotification;
    //bool isPopNotiDone = false;

    void Awake()
    {
        timeController.PrepareTimeController();
        startTime = GetTime();
        currentPoA = GetPoA();

    }
        
    void Start()
    {
        moneyHelper = new MoneyHelper(startMoneyAmount);
        levelHelper = new LevelHelper(startLevel,experience, experienceToNextLevel);
        powerHelper = new PowerHelper();
        UpdateMoneyValueUI();
        UpdateExperienceValueUI();
        levelHelper.OnExperienceChanged += LevelHelper_OnExperienceChanged;
        levelHelper.OnLevelChanged += LevelHelper_OnLevelChanged;
        uIController.systemInfoPanelHelper.fuelPurchaseBtn.onClick.AddListener(PurchaseFuel);
        meterHelper.TargetLoadRate = uIController.breakerPanelHelper.Load;
    }


    public void PrepareResourceController(EnergySystemObjectController purchasingObjectController)
    {
        this.purchasingObjectController = purchasingObjectController;
      
        InvokeRepeating("TimePeriod", 0, 1);
        //InvokeRepeating("CalculatePropertyIncome", 0, moneyCalculationInterval);
    }


    // Update is called once per frame
    private void Update()
    {
        timeController.UpdateTimeDateString();
        powerHelper.GetBreakerSwitchesValue(uIController.breakerPanelHelper.IsInterverSwitchOn, uIController.breakerPanelHelper.IsMainLoadSwitchOn, uIController.breakerPanelHelper.IsDGSwitchOn, uIController.breakerPanelHelper.Load);
        if (powerHelper.LoadDiff != 0f)
        {
            uIController.breakerPanelHelper.loadValue.text = "0";
            uIController.breakerPanelHelper.Load = 0f;
            uIController.breakerPanelHelper.UpdateLoadValueUI();
            powerHelper.LoadDiff = 0f;
            
        }

        meterHelper.TargetLoadRate = uIController.breakerPanelHelper.Load;
        meterHelper.TargetPowerRate = powerHelper.TotalOutputRate;



        //if (!isPopNotiDone)
        //{
        //    if (powerHelper.pop)
        //    {
        //        popupNotification.OpenNotification();
        //        isPopNotiDone = true;

        //    }
        //}
    }



    public void PurchaseFuel()
    {
        foreach (var item in purchasingObjectController.GetAllObjects())
        {
            if (item.GetType() == typeof(DieselGeneratorSO))
            {
                float costOfFuelNeed = (60f - item.fuelAmount)*2;

                if (SpendMoney((int)costOfFuelNeed))
                {
                    item.fuelAmount = 60f;
                }
                else
                {
                    Debug.Log("You don't have enough money to buy fuel.");
                }

                
            }
        }
    }




    public string GetTime()
    {
        return timeController.GetTime();
    }

    public float GetPoA()
    {
        return timeController.SolarRadiation;
    }

    public void TimePeriod()
    {
        currentPoA = GetPoA();
        string endTime = GetTime();

        if(startTime!=endTime)
            CalculateTimePeriod(startTime, endTime);
        startTime = endTime;
        previousPoA = currentPoA;

    }

    public void CalculateTimePeriod(string start, string end)
    {
        string[] startResult = start.Split(char.Parse("_"));
        string[] endResult = end.Split(char.Parse("_"));

        float startHr = int.Parse(startResult[0]);
        float startMin = int.Parse(startResult[1]);
        float endHr = int.Parse(endResult[0]);
        float endMin = int.Parse(endResult[1]);

        if (startHr != endHr)
        {
            float previousHr = (60f - startMin)/60f;
            powerHelper.CalculatePowerOutput(purchasingObjectController.GetAllObjects(), previousHr, previousPoA);
            powerHelper.CalculatePowerOutput(purchasingObjectController.GetAllObjects(), endMin / 60f, previousPoA);
        }
        else
        {
            
            float period = (endMin-startMin)/60;
            powerHelper.CalculatePowerOutput(purchasingObjectController.GetAllObjects(), period, currentPoA);
        }
        //powerHelper.CalculateSolaPanelToMainLoadOutputRate(purchasingObjectController.GetAllObjects());
    }




    #region Level
    private void LevelHelper_OnLevelChanged(object sender, EventArgs e)
    {
        UpdateExperienceValueUI();
        uIController.levelUpNotificationManager.OpenNotification();
    }

    private void LevelHelper_OnExperienceChanged(object sender, EventArgs e)
    {
        UpdateExperienceValueUI();
    }

    private void UpdateExperienceValueUI()
    {
        uIController.SetExperienceValue(levelHelper.Level, levelHelper.Experience, levelHelper.ExperienceToNextLevel);
    }
    public void AddExperience(int amount)
    {
        levelHelper.AddExperience(amount);
    }
    #endregion

    #region Money
    public void AddMoney(int amount)
    {
        moneyHelper.AddMoney(amount);
        UpdateMoneyValueUI();
    }



    private void UpdateMoneyValueUI()
    {
        uIController.SetMoneyValue(moneyHelper.Money);

    }

    public void CalculatePropertyIncome()
    {
        try
        {
            moneyHelper.CalculateMoney(purchasingObjectController.GetAllObjects());
            UpdateMoneyValueUI();
        }
        catch (MoneyException)
        {
            ReloadGame();
        }
    }



    public bool CanIBuyIt(int amount)
    {
        if (moneyHelper.Money >= amount)
        {
            return true;
        }
        return false;
    }

    public bool SpendMoney(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                moneyHelper.ReduceMoney(amount);
                UpdateMoneyValueUI();
                return true;
            }
            catch (MoneyException)
            {
                ReloadGame();
            }
        }
        return false;
    }
    #endregion

    private void ReloadGame()
    {
        Debug.Log("End the game");
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}