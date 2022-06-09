using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ApplianceRepository : MonoBehaviour
{
    public ApplianceCollection applianceCollection;

    public List<ApplianceBaseSO> GetApplianceObjects()
    {
        List<ApplianceBaseSO> systemObjects = new List<ApplianceBaseSO>();
        systemObjects.Add(applianceCollection.smallACSO);
        systemObjects.Add(applianceCollection.mediumACSO);
        systemObjects.Add(applianceCollection.largeACSO);
        systemObjects.Add(applianceCollection.smallWasherSO);
        systemObjects.Add(applianceCollection.largeWasherSO);
        //systemObjects.Add(applianceCollection.lightSO);
        systemObjects.Add(applianceCollection.smallFridgeSO);
        systemObjects.Add(applianceCollection.largeFridgeSO);
        //systemObjects.Add(applianceCollection.fanSO);
        //systemObjects.Add(applianceCollection.dryerSO);
        return systemObjects;
    }

    public ApplianceBaseSO GetApplianceData(string objectName, string applianceName)
    {
        switch (objectName)
        {
            case "Air Conditioner":
                return GetACData(applianceName);
            case "Washing Machine":
                return GetWasherData(applianceName);
            case "Fridge":
                return GetFridgeData(applianceName);
            default:
                return null;
        }
    }

    private ApplianceBaseSO GetACData(string objectName)
    {
        switch (objectName)
        {
            case "Small AC":
                return applianceCollection.smallACSO;
            case "Medium AC":
                return applianceCollection.mediumACSO;
            case "Large AC":
                return applianceCollection.largeACSO;
            default:
                return null;
        }
    }

    private ApplianceBaseSO GetFridgeData(string objectName)
    {
        switch (objectName)
        {
            case "Small Fridge":
                return applianceCollection.smallFridgeSO;
            case "Large Fridge":
                return applianceCollection.largeFridgeSO;
            default:
                return null;
        }
    }

    private ApplianceBaseSO GetWasherData(string objectName)
    {
        switch (objectName)
        {
            case "7kg Washer":
                return applianceCollection.smallWasherSO;
            case "10kg Washer":
                return applianceCollection.largeWasherSO;
            default:
                return null;
        }
    }

    public List<float> GetObjectSize(string objectName, string applianceName)
    {
        List<float> objectSizeToReturn = null;
        switch (objectName)
        {
            case "Air Conditioner":
                objectSizeToReturn = GetACSize(applianceName);
                break;
            case "Washing Machine":
                objectSizeToReturn = GetWashingMachineSize(applianceName);
                break;
            case "Fridge":
                objectSizeToReturn = GetFridgeSize();
                break;
            default:
                throw new Exception("No such appliance size." + objectName);

        }
        if (objectSizeToReturn == null)
        {
            throw new Exception("No size for that name " + objectName);
        }
        return objectSizeToReturn;
    }

    private List<float> GetWashingMachineSize(string objectName)
    {
        List<float> temp = new List<float>();
        switch (objectName)
        {
            case "7kg Washer":
                temp.Add(applianceCollection.smallWasherSO.objectWidth);
                temp.Add(applianceCollection.smallWasherSO.objectHeight);
                temp.Add(applianceCollection.smallWasherSO.objectLength);
                break;
            case "10kg Washer":
                temp.Add(applianceCollection.largeWasherSO.objectWidth);
                temp.Add(applianceCollection.largeWasherSO.objectHeight);
                temp.Add(applianceCollection.largeWasherSO.objectLength);
                break;
            default:
                break;
        }
        return temp;
    }

    private List<float> GetACSize(string objectName)
    {
        List<float> temp = new List<float>();
        switch (objectName)
        {
            case "Small AC":
                temp.Add(applianceCollection.smallACSO.objectWidth);
                temp.Add(applianceCollection.smallACSO.objectHeight);
                temp.Add(applianceCollection.smallACSO.objectLength);
                break;
            case "Medium AC":
                temp.Add(applianceCollection.mediumACSO.objectWidth);
                temp.Add(applianceCollection.mediumACSO.objectHeight);
                temp.Add(applianceCollection.mediumACSO.objectLength);
                break;
            case "Large AC":
                temp.Add(applianceCollection.largeACSO.objectWidth);
                temp.Add(applianceCollection.largeACSO.objectHeight);
                temp.Add(applianceCollection.largeACSO.objectLength);
                break;
            default:
                break;
        }
        return temp;
    }

    private List<float> GetFridgeSize()
    {
        List<float> temp = new List<float>();
        temp.Add(applianceCollection.smallFridgeSO.objectWidth);
        temp.Add(applianceCollection.smallFridgeSO.objectHeight);
        temp.Add(applianceCollection.smallFridgeSO.objectLength);
        return temp;
    }
}


