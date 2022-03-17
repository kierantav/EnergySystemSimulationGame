using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectModificationFactory
{
    private readonly ObjectModificationHelper objectPlacementHelper;
    private readonly ObjectModificationHelper objectRemoveHelper;

    public ObjectModificationFactory(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, IResourceController resourceController)
    {
        objectPlacementHelper = new ObjectPlacementHelper(grid, placementController, objectRepository, resourceController);
        objectRemoveHelper = new ObjectRemoveHelper(grid, placementController, objectRepository, resourceController);
    }

    public ObjectModificationHelper GetHelper(Type classType)
    {
        if (classType == typeof(PlayerSellingObjectState))
        {
            return objectRemoveHelper;
        }
        else
        {
            return objectPlacementHelper;
        }
    }
}
