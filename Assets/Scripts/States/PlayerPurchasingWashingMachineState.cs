using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingWashingMachineState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;
    Vector3 position;

    Vector3 cameraPosition = new Vector3(71.93f, 8f, 46.89f);
    Quaternion cameraRotation = Quaternion.Euler(0f, -8.76f, 0f);

    public PlayerPurchasingWashingMachineState(GameController gameController, ApplianceObjectController purchasingApplianceController, Vector3 position, UIController uiController) : base(gameController)
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
        if (objectName != "Washing Machine")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}
