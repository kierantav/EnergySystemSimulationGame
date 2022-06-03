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
        systemObjects.Add(applianceCollection.mediumWashingMachineSO);
        //systemObjects.Add(applianceCollection.lightSO);
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
                return applianceCollection.mediumWashingMachineSO;
            case "Fridge":
                return applianceCollection.largeFridgeSO;
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
            case "Large":
                return applianceCollection.smallACSO;
            default:
                return null;
        }
    }

    public List<int> GetObjectSize(string objectName, string applianceName)
    {
        List<int> objectSizeToReturn = null;
        switch (objectName)
        {
            case "Air Conditioner":
                objectSizeToReturn = GetACSize(applianceName);
                break;
            case "Washing Machine":
                objectSizeToReturn = GetWashingMachineSize();
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

    private List<int> GetWashingMachineSize()
    {
        List<int> temp = new List<int>();
        /*temp.Add(applianceCollection.washingMachineSO.objectWidth);
        temp.Add(applianceCollection.washingMachineSO.objectHeight);
        temp.Add(applianceCollection.washingMachineSO.objectLength);*/
        return temp;
    }

    private List<int> GetACSize(string objectName)
    {
        List<int> temp = new List<int>();
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

    private List<int> GetFridgeSize()
    {
        List<int> temp = new List<int>();
        /*temp.Add(applianceCollection.fridgeSO.objectWidth);
        temp.Add(applianceCollection.fridgeSO.objectHeight);
        temp.Add(applianceCollection.fridgeSO.objectLength);*/
        return temp;
    }
}


