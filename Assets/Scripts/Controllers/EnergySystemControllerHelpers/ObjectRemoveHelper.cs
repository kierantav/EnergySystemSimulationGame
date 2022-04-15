﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRemoveHelper: ObjectModificationHelper
{
    public ObjectRemoveHelper(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController) : base(grid, placementController, objectRepository, applianceRepository, resourceController)
    {
    }

    public override void PrepareObjectForModification(Vector3 inputPosition, string objectName, string type)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(gridPosition);
        //Debug.Log(type);
        // if the cell has been taken
        if (grid.IsCellTaken(positionList))
        {
            var obj = grid.GetObjectFromTheGrid(gridPosition);
            //Debug.Log(obj);
            List<Vector3> list = grid.GetObjectPositionListFromTheGrid(gridPosition);
            if (objectToBeModified.ContainsKey(list))
            {
                resourceController.AddMoney(resourceController.removeCost);
                StopObjectsFromBeingSelled(list, obj);
            }
            else if (resourceController.CanIBuyIt(resourceController.removeCost))
            {
                AddObjectsForSelling(list, obj);
                resourceController.SpendMoney(resourceController.removeCost);
            }
        }
        
    }

    public override void CancelModifications(string type)
    {
        foreach (var item in objectToBeModified)
        {
            resourceController.AddMoney(resourceController.removeCost);
        }
        //Debug.Log(objectToBeModified.Values);
        this.placementController.PlaceObjectsOnTheMap(objectToBeModified.Values);
        objectToBeModified.Clear();
    }

    public override void ConfirmModifications(string type)
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
        if (!objectToBeModified.ContainsKey(positionList)) {
            objectToBeModified.Add(positionList, obj);
            placementController.SetObjectForSale(obj);
        }
    }

    private void StopObjectsFromBeingSelled(List<Vector3> positionList, GameObject obj)
    {
        Debug.Log("stop objects");
        placementController.ResetObjectMaterial(obj);
        objectToBeModified.Remove(positionList);
    }

}
