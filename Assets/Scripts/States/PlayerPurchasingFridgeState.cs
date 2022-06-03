using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingFridgeState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;
    Vector3 position;

    Vector3 cameraPosition = new Vector3(80f, 10f, 54.84f);
    Quaternion cameraRotation = Quaternion.Euler(0f, -100f, 0f);

    public PlayerPurchasingFridgeState(GameController gameController, ApplianceObjectController purchasingApplianceController, Vector3 position, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.position = position;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        //purchasingApplianceController.UpdateSystemAttributesToApplianceData();
        this.objectName = objectName;
        if (!this.position.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(this.position, this.objectName, applianceName);
        }
    }

    public override void OnCancel()
    {
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnConfirm()
    {
        this.purchasingApplianceController.ConfirmModification();
        uiController.InstalledAppliances = purchasingApplianceController.GetListOfAllAppliances();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnPuchasingAppliance(string objectName, string applianceName)
    {
        //Debug.Log(objectName);
        if (objectName != "Fridge")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}
