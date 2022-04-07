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
        systemObjects.Add(applianceCollection.acSO);
        systemObjects.Add(applianceCollection.washingMachineSO);
        //systemObjects.Add(applianceCollection.lightSO);
        //systemObjects.Add(applianceCollection.fridgeSO);
        //systemObjects.Add(applianceCollection.fanSO);
        //systemObjects.Add(applianceCollection.dryerSO);
        return systemObjects;
    }

    public ApplianceBaseSO GetApplianceData(string objectName)
    {
        switch (objectName)
        {
            case "Air Conditioner":
                return applianceCollection.acSO;
            case "Washing Machine":
                return applianceCollection.washingMachineSO;
            default:
                return null;
        }
    }

    public List<int> GetObjectSize(string objectName)
    {
        List<int> objectSizeToReturn = null;
        switch (objectName)
        {
            case "Air Conditioner":
                objectSizeToReturn = GetACSize();
                break;
            case "Washing Machine":
                objectSizeToReturn = GetWashingMachineSize();
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
        temp.Add(applianceCollection.washingMachineSO.objectWidth);
        temp.Add(applianceCollection.washingMachineSO.objectHeight);
        temp.Add(applianceCollection.washingMachineSO.objectLength);
        return temp;
    }

    private List<int> GetACSize()
    {
        List<int> temp = new List<int>();
        temp.Add(applianceCollection.acSO.objectWidth);
        temp.Add(applianceCollection.acSO.objectHeight);
        temp.Add(applianceCollection.acSO.objectLength);
        return temp;
    }
}


