using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRepository : MonoBehaviour
{
    public ScriptableObjectCollection scriptableObjectCollection;

    public List<EnergySystemGeneratorBaseSO> GetEnergySystemGeneratorObjects()
    {
        List<EnergySystemGeneratorBaseSO> systemObjects = new List<EnergySystemGeneratorBaseSO>();
        systemObjects.Add(scriptableObjectCollection.dieselGeneratorSO);
        systemObjects.Add(scriptableObjectCollection.batterySO);
        systemObjects.Add(scriptableObjectCollection.solarPanelSO);
        systemObjects.Add(scriptableObjectCollection.windTurbineSO);
        systemObjects.Add(scriptableObjectCollection.invertorSO);
        systemObjects.Add(scriptableObjectCollection.hybirdChargeControllerSO);
        systemObjects.Add(scriptableObjectCollection.powerLinesSO);
        return systemObjects;
    }

    #region GetEnergySystemObjectPrafabByName
    public GameObject GetObjectPrefabByName(string objectName)
    {
        GameObject objectPrefabToReturn = null;
        switch (objectName)
        {
            case "Diesel Generator":
                objectPrefabToReturn = GetDieselGeneratorPrefab();
                break;
            case "Battery":
                objectPrefabToReturn = GetBatteryPrefab();
                break;
            case "Solar Panel":
                objectPrefabToReturn = GetSolarPanelPrefab();
                break;
            case "Wind Turbine":
                objectPrefabToReturn = GetWindTurbinePrefab();
                break;
            case "Invertor":
                objectPrefabToReturn = GetInvertorPrefab();
                break;
            case "Charge Controller":
                objectPrefabToReturn = GetHybirdChargeControllerPrefab();
                break;
            case "On-Grid Power":
                objectPrefabToReturn = GetPowerLinesPrefab();
                break;
            default:
                throw new Exception("No such energy system type." + objectName);
        }
        if(objectPrefabToReturn == null)
        {
            throw new Exception("No prefab for that name " + objectName);
        }
        return objectPrefabToReturn;
    }

    private GameObject GetHybirdChargeControllerPrefab()
    {
        return scriptableObjectCollection.hybirdChargeControllerSO.objectPrefab;
    }

    private GameObject GetInvertorPrefab()
    {
        return scriptableObjectCollection.invertorSO.objectPrefab;
    }

    private GameObject GetWindTurbinePrefab()
    {
        return scriptableObjectCollection.windTurbineSO.objectPrefab;
    }

    private GameObject GetSolarPanelPrefab()
    {
        return scriptableObjectCollection.solarPanelSO.objectPrefab;
    }

    private GameObject GetBatteryPrefab()
    {
        return scriptableObjectCollection.batterySO.objectPrefab;
    }

    private GameObject GetDieselGeneratorPrefab()
    {
        return scriptableObjectCollection.dieselGeneratorSO.objectPrefab;
    }
    private GameObject GetPowerLinesPrefab()
    {
        return scriptableObjectCollection.powerLinesSO.objectPrefab;
    }
    #endregion

    #region GetEnergySystemSizePrafabByName
    public List<int> GetObjectSize(string objectName)
    {
        List<int> objectSizeToReturn = null;
        switch (objectName)
        {
            case "Diesel Generator":
                objectSizeToReturn = GetDieselGeneratorSize();
                break;
            case "Battery":
                objectSizeToReturn = GetBatterySize();
                break;
            case "Solar Panel":
                objectSizeToReturn = GetSolarPanelSize();
                break;
            case "Wind Turbine":
                objectSizeToReturn = GetWindTurbineSize();
                break;
            case "Invertor":
                objectSizeToReturn = GetInvertorSize();
                break;
            case "Charge Controller":
                objectSizeToReturn = GetHybirdChargeControllerSize();
                break;
            case "On-Grid Power":
                objectSizeToReturn = GetPowerLinesSize();
                break;
            default:
                throw new Exception("No such energy system size." + objectName);

        }
        if (objectSizeToReturn == null)
        {
            throw new Exception("No size for that name " + objectName);
        }
        return objectSizeToReturn;
    }

    private List<int> GetHybirdChargeControllerSize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.hybirdChargeControllerSO.objectWidth);
        temp.Add(scriptableObjectCollection.hybirdChargeControllerSO.objectHeight);
        temp.Add(scriptableObjectCollection.hybirdChargeControllerSO.objectLength);
        return temp;
    }

    private List<int> GetInvertorSize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.invertorSO.objectWidth);
        temp.Add(scriptableObjectCollection.invertorSO.objectHeight);
        temp.Add(scriptableObjectCollection.invertorSO.objectLength);
        return temp;
    }

    public List<int> GetDieselGeneratorSize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.dieselGeneratorSO.objectWidth);
        temp.Add(scriptableObjectCollection.dieselGeneratorSO.objectHeight);
        temp.Add(scriptableObjectCollection.dieselGeneratorSO.objectLength);
        return temp;
    }
    public List<int> GetBatterySize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.batterySO.objectWidth);
        temp.Add(scriptableObjectCollection.batterySO.objectHeight);
        temp.Add(scriptableObjectCollection.batterySO.objectLength);
        return temp;
    }
    public List<int> GetSolarPanelSize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.solarPanelSO.objectWidth);
        temp.Add(scriptableObjectCollection.solarPanelSO.objectHeight);
        temp.Add(scriptableObjectCollection.solarPanelSO.objectLength);
        return temp;
    }
    public List<int> GetWindTurbineSize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.windTurbineSO.objectWidth);
        temp.Add(scriptableObjectCollection.windTurbineSO.objectHeight);
        temp.Add(scriptableObjectCollection.windTurbineSO.objectLength);
        return temp;
    }

    public List<int> GetPowerLinesSize()
    {
        List<int> temp = new List<int>();
        temp.Add(scriptableObjectCollection.powerLinesSO.objectWidth);
        temp.Add(scriptableObjectCollection.powerLinesSO.objectHeight);
        temp.Add(scriptableObjectCollection.powerLinesSO.objectLength);
        return temp;
    }

    #endregion

    // Todo: Editmode Test
    public EnergySystemGeneratorBaseSO GetEnergySystemData(string objectName)
    {
        switch (objectName)
        {
            case "Diesel Generator":
                return scriptableObjectCollection.dieselGeneratorSO;
            case "Battery":
                return scriptableObjectCollection.batterySO;
            case "Solar Panel":
                return scriptableObjectCollection.solarPanelSO;
            case "Wind Turbine":
                return scriptableObjectCollection.windTurbineSO;
            case "Invertor":
                return scriptableObjectCollection.invertorSO;
            case "Charge Controller":
                return scriptableObjectCollection.hybirdChargeControllerSO;
            case "On-Grid Power":
                return scriptableObjectCollection.powerLinesSO;
            default:
                return null;
        }
    }

}



