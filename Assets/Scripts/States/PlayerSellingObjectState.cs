using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSellingObjectState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    ApplianceObjectController purchasingApplianceController;
    public PlayerSellingObjectState(GameController gameController, EnergySystemObjectController purchasingObjectController, ApplianceObjectController purchasingApplianceController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.purchasingApplianceController = purchasingApplianceController;
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
        /*if (objectType.Equals("Energy"))
        {*/
            this.purchasingObjectController.PrepareObjectForSellingAt(position);
        /*} 
        else
        {*/
            this.purchasingApplianceController.PrepareApplianceForSellingAt(position);
        //}
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
