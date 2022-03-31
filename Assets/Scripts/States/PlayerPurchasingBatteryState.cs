using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingBatteryState : PlayerState
{
    // Inherited from abstract class
    EnergySystemObjectController purchasingObjectController;

    // New field
    string objectName;
    Vector3 position;
   
 

    public PlayerPurchasingBatteryState(GameController gameController, EnergySystemObjectController purchasingObjectController, Vector3 position) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.position = position;

    }

    public override void EnterState(string objectName)
    {
        purchasingObjectController.PreparePurchasingObjectController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.objectName = objectName;
        if (!this.position.Equals(Vector3.zero))
        {
            this.purchasingObjectController.PrepareObjectForModification(this.position, this.objectName, "Energy");
        }
    }

    public override void OnCancel()
    {
        this.purchasingObjectController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnConfirm()
    {
        this.purchasingObjectController.ConfirmModification();
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }
    public override void OnPuchasingEnergySystem(string objectName)
    {
        if(objectName!="Battery")
        {
            OnCancel();
        }
        base.OnPuchasingEnergySystem(objectName);
    }
}
