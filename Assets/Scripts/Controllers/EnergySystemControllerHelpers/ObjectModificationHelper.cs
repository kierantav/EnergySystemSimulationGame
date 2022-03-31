using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectModificationHelper 
{
    protected readonly GridStructure grid;
    protected readonly IPlacementController placementController;
    protected readonly ObjectRepository objectRepository;
    protected readonly ApplianceRepository applianceRepository;
    protected Dictionary<List<Vector3>, GameObject> objectToBeModified = new Dictionary<List<Vector3>, GameObject>();
    protected EnergySystemGeneratorBaseSO energySystemData;
    protected ApplianceBaseSO applianceData;
    protected IResourceController resourceController;

    public ObjectModificationHelper(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController)
    {
        this.grid = grid;
        this.placementController = placementController;
        this.objectRepository = objectRepository;
        this.applianceRepository = applianceRepository;
        this.resourceController = resourceController;
        energySystemData = ScriptableObject.CreateInstance<NullObjectSO>();
        applianceData = ScriptableObject.CreateInstance<NullApplianceSO>();
    }

    public GameObject AccessStructureInDictionary(Vector3 gridPosition)
    {
        foreach (var list in objectToBeModified.Keys)
        {
            if (list.Contains(gridPosition))
            {
                return objectToBeModified[list];
            }
        }
        return null;
    }

    public virtual void CancelModifications(string type)
    {
        if (type == "Energy")
        {
            placementController.DestroyObjects(objectToBeModified.Values);
            ResetHelperData();
        } else {
            //Debug.Log("Appliance");
        }
    }

    public virtual void ConfirmModifications()
    {
        placementController.PlaceObjectsOnTheMap(objectToBeModified.Values);
        foreach (var keyValuePair in objectToBeModified)
        {
            grid.PlaceObjectOnTheGrid(keyValuePair.Value, keyValuePair.Key, GameObject.Instantiate(energySystemData));
            //grid.PlaceObjectOnTheGrid(keyValuePair.Value, keyValuePair.Key, energySystemData);
        }
        ResetHelperData();
    }

    public virtual void PrepareObjectForModification(Vector3 inputPosition, string objectName, string type)
    {
        if (type == "Energy")
        {
            if (energySystemData.GetType() == typeof(NullObjectSO))
            {
                energySystemData = this.objectRepository.GetEnergySystemData(objectName);
            }
        } else {
            if (applianceData.GetType() == typeof(NullApplianceSO))
            {
                //applianceData = this.applianceRepository.GetApplianceData(objectName);
            }
        }
        
    }

    private void ResetHelperData()
    {
        objectToBeModified.Clear();
        energySystemData = ScriptableObject.CreateInstance<NullObjectSO>();
    }
}
