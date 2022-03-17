using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplianceRepository : MonoBehaviour
{
    public ApplianceCollection applianceCollection;

    public List<ApplianceBaseSO> GetApplianceObjects()
    {
        List<ApplianceBaseSO> systemObjects = new List<ApplianceBaseSO>();
        systemObjects.Add(applianceCollection.acSO);
        //systemObjects.Add(applianceCollection.lightSO);
        //systemObjects.Add(applianceCollection.fridgeSO);
        //systemObjects.Add(applianceCollection.fanSO);
        //systemObjects.Add(applianceCollection.washingMachineSO);
        //systemObjects.Add(applianceCollection.dryerSO);
        return systemObjects;
    }
}


