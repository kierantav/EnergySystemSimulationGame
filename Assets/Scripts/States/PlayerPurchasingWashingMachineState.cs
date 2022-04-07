using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingWashingMachineState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    string objectName;
    Vector3 position;
    public PlayerPurchasingWashingMachineState(GameController gameController, ApplianceObjectController purchasingApplianceController, Vector3 position) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.position = position;
    }

    public override void EnterState(string objectName)
    {
        //Debug.Log(objectName);
        purchasingApplianceController.PreparePurchasingApplianceController(GetType());

        //purchasingApplianceController.UpdateSystemAttributesToApplianceData();
        this.objectName = objectName;
        if (!this.position.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(this.position, this.objectName);
        }
    }

    public override void OnCancel()
    {
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnConfirm()
    {
        this.purchasingApplianceController.ConfirmModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnPuchasingAppliance(string objectName)
    {
        //Debug.Log(objectName);
        if (objectName != "Washing Machine")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName);
    }
}
