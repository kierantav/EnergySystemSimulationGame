/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 21/08/2021
 |  Latest Modified Date: 21/08/2021
 |  Description: 
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    Vector3? previousPosition;

    public PlayerSelectionState(GameController gameController, EnergySystemObjectController objectController) : base(gameController)
    {
        this.purchasingObjectController = objectController;
    }
    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        EnergySystemGeneratorBaseSO data = purchasingObjectController.GetEnergySystemDataFromPosition(position);
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        if (data)
        {
            this.gameController.uiController.DisplaySystemInfo(data);
            previousPosition = position;
        }
        else
        {
            this.gameController.uiController.HideSystemInfo();
            data = null;
            previousPosition = null;
        }
        return;
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        return;
    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        
        switch (objectName)
        {
            case "Diesel Generator":
                this.gameController.TransitionToState(this.gameController.purchasingDieselGeneratorState, objectName);
                break;
            case "Battery":
                this.gameController.TransitionToState(this.gameController.purchasingBatteryState, objectName);
                break;
            case "Solar Panel":
                this.gameController.TransitionToState(this.gameController.purchasingSolarPanelState, objectName);
                break;
            case "Wind Turbine":
                this.gameController.TransitionToState(this.gameController.purchasingWindTurbineState, objectName);
                break;
            case "Invertor":
                this.gameController.TransitionToState(this.gameController.purchasingInvertorState, objectName);
                break;
            case "Charge Controller":
                this.gameController.TransitionToState(this.gameController.purchasingChargeControllerState, objectName);
                break;
            default:
                throw new Exception("No such energy system type." + objectName);
        }
    }

    public override void EnterState(string objectVariable)
    {
        if (this.gameController.uiController.GetSystemInfoVisibility())
        {
            EnergySystemGeneratorBaseSO data = purchasingObjectController.GetEnergySystemDataFromPosition(previousPosition.Value);
            
            if (data)
            {
                this.gameController.uiController.DisplaySystemInfo(data);
            }
            else
            {
                this.gameController.uiController.HideSystemInfo();
                previousPosition = null;
            }
        }
    }
}
