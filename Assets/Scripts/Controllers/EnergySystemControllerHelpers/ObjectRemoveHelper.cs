using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRemoveHelper: ObjectModificationHelper
{
    public ObjectRemoveHelper(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, IResourceController resourceController) : base(grid, placementController, objectRepository, resourceController)
    {
    }

    public override void PrepareObjectForModification(Vector3 inputPosition, string objectName)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(gridPosition);
        // if the cell has been taken
        if (grid.IsCellTaken(positionList))
        {
            var obj = grid.GetObjectFromTheGrid(gridPosition);
            List<Vector3> list = grid.GetObjectPositionListFromTheGrid(gridPosition);
            if (objectToBeModified.ContainsKey(list))
            {
                resourceController.AddMoney(resourceController.removeCost);
                StopObjectsFromBeingSelled(list, obj);
            }
            else if(resourceController.CanIBuyIt(resourceController.removeCost))
            {
                AddObjectsForSelling(list, obj);
                resourceController.SpendMoney(resourceController.removeCost);
            }
        }
    }

    public override void CancelModifications()
    {
        foreach (var item in objectToBeModified)
        {
            resourceController.AddMoney(resourceController.removeCost);
        }
        this.placementController.PlaceObjectsOnTheMap(objectToBeModified.Values);
        objectToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        foreach (var gridPosition in objectToBeModified.Keys)
        {
            grid.RemoveObjectFromTheGrid(gridPosition);
        }
        this.placementController.DestroyObjects(objectToBeModified.Values);
        objectToBeModified.Clear();
    }


    private void AddObjectsForSelling(List<Vector3> positionList, GameObject obj)
    {
        // Remove an object on this cell
        objectToBeModified.Add(positionList, obj);
        placementController.SetObjectForSale(obj);
    }

    private void StopObjectsFromBeingSelled(List<Vector3> positionList, GameObject obj)
    {
        placementController.ResetObjectMaterial(obj);
        objectToBeModified.Remove(positionList);
    }

}
