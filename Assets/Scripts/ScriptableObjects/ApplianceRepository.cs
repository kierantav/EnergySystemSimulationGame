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
}


