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

    public ApplianceBaseSO GetApplianceDataFromPosition(Vector3 inputPosition)
    {
        List<Vector3> PositionList = new List<Vector3>();
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        PositionList.Add(gridPosition);
        if (grid.IsCellTaken(PositionList))
        {
            return grid.GetApplianceDataFromTheGrid(PositionList[0]);
        }
        return null;
    }
    public IEnumerable<ApplianceBaseSO> GetAllAppliances()
    {
        return grid.GetAllAppliances();
    }

    public void CancelModification()
    {
        if (objectModificationHelper != null)
            objectModificationHelper.CancelModifications("Appliance");
    }

    public void PreparePurchasingApplianceController(Type classType)
    {
        //Debug.Log(classType);
        objectModificationHelper = objectModificationFactory.GetHelper(classType);
    }

    public void PrepareApplianceForSellingAt(Vector3 inputPosition)
    {
        //Debug.Log(objectModificationHelper);
        objectModificationHelper.PrepareObjectForModification(inputPosition, "", "Appliance");
    }

    public void UpdateSystemAttributesToApplianceData()
    {
        objectUpdateHelper.GetSystemData(grid.GetListOfAllObjects(), grid.GetListOfAllAppliances(), grid);
        objectUpdateHelper.UpdateSystemObjectAttributes();
    }

    public void ConfirmModification()
    {
        objectModificationHelper.ConfirmModifications("Appliance");
    }

    public void PrepareApplianceForModification(Vector3 inputPosition, string objectName)
    {
        try
        {
            //Debug.Log("1");
            objectModificationHelper.PrepareObjectForModification(inputPosition, objectName, "Appliance");
        }
        catch
        {
            throw new Exception("No such appliance type." + objectName);

        }
    }
}
