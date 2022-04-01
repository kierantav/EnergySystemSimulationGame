using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceObjectController
{

    GridStructure grid;
    IPlacementController placementController;
    ObjectRepository objectRepository;
    ApplianceRepository applianceRepository;
    ObjectModificationFactory objectModificationFactory;
    ObjectModificationHelper objectModificationHelper;
    ObjectUpdateHelper objectUpdateHelper;

    public ApplianceObjectController(int cellSize, int width, int height, int length, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController)
    {
        grid = new GridStructure(cellSize, width, height, length);
        this.objectRepository = objectRepository;
        this.placementController = placementController;
        objectModificationFactory = new ObjectModificationFactory(grid, placementController, objectRepository, applianceRepository, resourceController);
        objectUpdateHelper = new ObjectUpdateHelper();
    }

    /*public void PurchasingApplianceObject(string objectName)
    {
        applianceTempObject.name = objectName;
        Instantiate(applianceTempObject, Vector3.zero, Quaternion.identity);
    }*/

    public void CancelModification()
    {
        if (objectModificationHelper != null)
            objectModificationHelper.CancelModifications("Appliance");
    }

    public void PreparePurchasingApplianceController(Type classType)
    {
        Debug.Log(classType);
        objectModificationHelper = objectModificationFactory.GetHelper(classType);
    }

    public void UpdateSystemAttributesToApplianceData()
    {
        objectUpdateHelper.GetSystemData(grid.GetListOfAllObjects(), grid);
        objectUpdateHelper.UpdateSystemObjectAttributes();
    }

    public void ConfirmModification()
    {
        objectModificationHelper.ConfirmModifications();
    }

    public void PrepareApplianceForModification(Vector3 inputPosition, string objectName, string type)
    {
        try
        {
            objectModificationHelper.PrepareObjectForModification(inputPosition, objectName, type);
        }
        catch
        {
            throw new Exception("No such appliance type." + objectName+ ','+type);

        }
    }
}
