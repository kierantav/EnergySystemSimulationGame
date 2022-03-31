using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingACState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    Vector3 position;
    public PlayerPurchasingACState(GameController gameController, ApplianceObjectController purchasingApplianceController, Vector3 position) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.position = position;
    }

    public override void EnterState(string objectName)
    {
        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        //purchasingApplianceController.UpdateSystemAttributesToApplianceData();
        if (!this.position.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(this.position, objectName, "Appliance");
        }
    }

    public override void OnCancel()
    {
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }
}
