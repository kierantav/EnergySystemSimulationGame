using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPlacementHelper: ObjectModificationHelper
{
    public ObjectPlacementHelper(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, IResourceController resourceController) : base(grid, placementController, objectRepository, resourceController)
    {
    }

    public override void PrepareObjectForModification(Vector3 inputPosition, string objectName)
    {
        base.PrepareObjectForModification(inputPosition, objectName);
        GameObject objectPrefab = energySystemData.objectPrefab; // Get object prefab
        List<Vector3> positionList = GetPositionListByName(inputPosition, objectName); // Get and update object positions List
        if (!grid.IsCellTaken(positionList)) // If the cells are not taken 
        {
            List<Vector3> currentPositionList = CheckExisting(positionList);
            if (currentPositionList != null)
            {
                if (currentPositionList.Contains(positionList[0]))
                {
                    resourceController.AddMoney(energySystemData.purchaseCost);
                    RevokeObjectFromBeingPlaced(currentPositionList);
                }
                else 
                {
                    Debug.Log("Cell has been taken by the existing ghost object");
                }
            }
            else if (resourceController.CanIBuyIt(energySystemData.purchaseCost))
            {
                AddObjectForPlacement(objectPrefab, positionList);
                resourceController.SpendMoney(energySystemData.purchaseCost);
            }
        }
        else
        {
            //Todo: Create a notification here (Only one item can be generated)
            //foreach(Vector3 p in positionList)
            //{
            //    //Debug.Log(p);
            //}
            Debug.Log("Cell has been taken!!!");
        }
    }


    private void AddObjectForPlacement(GameObject objectPrefab, List<Vector3> positionList)
    {
        objectToBeModified.Add(positionList, placementController.CreateGhostObject(positionList, objectPrefab));
    }

    private void RevokeObjectFromBeingPlaced(List<Vector3> positionList)
    {
        var obj = objectToBeModified[positionList];
        placementController.DestroySingleObject(obj);
        objectToBeModified.Remove(positionList);
    }

    // return positionlist
    private List<Vector3> GetPositionListByName(Vector3 inputPosition, string objectName)
    {
        List<Vector3> positionList = new List<Vector3>();
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition); // Convert mouse position into grid position
        Vector3 p; // temp position
        List<int> objectSize = this.objectRepository.GetObjectSize(objectName);
        // Adding current mouse/grid position with object volume
        for (int x = (int)gridPosition.x; x < gridPosition.x + objectSize[0]; x++)
        {
            for (int y = (int)gridPosition.y; y < gridPosition.y + objectSize[1]; y++)
            {
                for (int z = (int)gridPosition.z; z < gridPosition.z + objectSize[2]; z++)
                {
                    p.x = x;
                    p.y = y;
                    p.z = z;
                    positionList.Add(p);
                    // ------ Debug ------//
                    //Debug.Log(p);
                }
            }
        }
        return positionList;
    }

    private List<Vector3> CheckExisting(List<Vector3> positionList)
    {
        foreach (var list in objectToBeModified.Keys)
        {
            foreach (var position in positionList)
            {
                if (list.Contains(position))
                    return list;
            }
        }
        return null;
    }

    public override void CancelModifications()
    {
        resourceController.AddMoney(objectToBeModified.Count * energySystemData.purchaseCost);
        base.CancelModifications();
    }
    public override void ConfirmModifications()
    {
        resourceController.AddExperience(objectToBeModified.Count * energySystemData.purchaseExperience);
        base.ConfirmModifications();   
    }
}
