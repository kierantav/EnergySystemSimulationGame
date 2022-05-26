using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSellingObjectState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    public PlayerSellingObjectState(GameController gameController, EnergySystemObjectController purchasingObjectController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void OnCancel()
    {
        this.purchasingObjectController.CancelModification();
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnConfirm()
    {
        this.purchasingObjectController.ConfirmModification();
        this.purchasingApplianceController.ConfirmModification();
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.uiController.InstalledAppliances = purchasingApplianceController.GetListOfAllAppliances();
        this.uiController.InstalledEnergySystems = purchasingObjectController.GetListOfAllObjects();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        OnCancel();
        base.OnPuchasingEnergySystem(objectName);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        EnergySystemGeneratorBaseSO obj = this.purchasingObjectController.GetEnergySystemDataFromPosition(position);
        if (obj != null && obj.objectType.Equals("Energy"))
        {
            this.purchasingObjectController.PrepareObjectForSellingAt(position);
        } else
        {
            this.purchasingApplianceController.PrepareApplianceForSellingAt(position);
        }
    }

    public override void OnInputPointerUp()
    {
        return;
    }
    public override void EnterState(string objectVariable)
    {
        this.purchasingObjectController.PreparePurchasingObjectController(GetType());
        this.purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
    }


}
