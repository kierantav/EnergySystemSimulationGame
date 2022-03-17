using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergySystemObjectController
{
    GridStructure grid;
    IPlacementController placementController;
    ObjectRepository objectRepository;
    ObjectModificationFactory objectModificationFactory;
    ObjectModificationHelper objectModificationHelper;
    ObjectUpdateHelper objectUpdateHelper;

    public EnergySystemObjectController(int cellSize, int width, int height, int length, IPlacementController placementController, ObjectRepository objectRepository, IResourceController resourceController)
    {
        grid = new GridStructure(cellSize, width, height, length);
        this.objectRepository = objectRepository;
        this.placementController = placementController;
        objectModificationFactory = new ObjectModificationFactory(grid, placementController, objectRepository, resourceController);
        objectUpdateHelper = new ObjectUpdateHelper();

    }

    public IEnumerable<EnergySystemGeneratorBaseSO> GetAllObjects()
    {
        return grid.GetAllObjects();
    }

    public void PreparePurchasingObjectController(Type classType)
    {
        objectModificationHelper = objectModificationFactory.GetHelper(classType);
    }

    #region PlacementAction
    public void PrepareObjectForModification(Vector3 inputPosition, string objectName)
    {
        try
        {
            objectModificationHelper.PrepareObjectForModification(inputPosition, objectName);
        }
        catch
        {
            throw new Exception("No such energy system type." + objectName);
            
        }
    }



    public void ConfirmModification()
    {
        objectModificationHelper.ConfirmModifications();
    }

    public void CancelModification()
    {
        objectModificationHelper.CancelModifications();
    }
    #endregion

    #region RemoveAction
    public void PrepareObjectForSellingAt(Vector3 inputPosition)
    {
        objectModificationHelper.PrepareObjectForModification(inputPosition,"");
    }

    #endregion


    #region ExploitDataForPlayModeTests
    public GameObject CheckForStructureInGrid(Vector3 inputPosition)
    {
        List<Vector3> PositionList = new List<Vector3>();
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        PositionList.Add(gridPosition);
        if (grid.IsCellTaken(PositionList))
        {
            return grid.GetObjectFromTheGrid(PositionList[0]);
        }
        return null;
    }

    public GameObject CheckForOnjectToModifyDictionary(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        GameObject structureToReturn = null;
        structureToReturn = objectModificationHelper.AccessStructureInDictionary(gridPosition);
        if (structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = objectModificationHelper.AccessStructureInDictionary(gridPosition);
        return structureToReturn;
    }
    #endregion

    public EnergySystemGeneratorBaseSO GetEnergySystemDataFromPosition(Vector3 inputPosition)
    {
        List<Vector3> PositionList = new List<Vector3>();
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        PositionList.Add(gridPosition);
        if (grid.IsCellTaken(PositionList))
        {
            return grid.GetEnergySystemDataFromTheGrid(PositionList[0]);
        }
        return null;
    }

    public void UpdateSystemAttributesToEnergySystemData()
    {
        objectUpdateHelper.GetSystemData(grid.GetListOfAllObjects(), grid);
        objectUpdateHelper.UpdateSystemObjectAttributes();

    }

  
}