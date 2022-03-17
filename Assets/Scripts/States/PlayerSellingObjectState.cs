using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSellingObjectState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    public PlayerSellingObjectState(GameController gameController, EnergySystemObjectController purchasingObjectController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
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
        OnCancel();
        base.OnPuchasingEnergySystem(objectName);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.purchasingObjectController.PrepareObjectForSellingAt(position);
    }

    public override void OnInputPointerUp()
    {
        return;
    }
    public override void EnterState(string objectVariable)
    {
        this.purchasingObjectController.PreparePurchasingObjectController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
    }


}
