using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingACState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;
    Vector3 position;

    Vector3 cameraPosition = new Vector3(70.65f, 10f, 54.84f);
    Quaternion cameraRotation = Quaternion.Euler(0f, 111.38f, 0f);

    public PlayerPurchasingACState(GameController gameController, ApplianceObjectController purchasingApplianceController, Vector3 position, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.position = position;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName)
    {
        //uiController.applianceOptionsPanelHelper.OpenApplianceOptionsPanel();
        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

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
        uiController.InstalledAppliances = purchasingApplianceController.GetListOfAllAppliances();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnPuchasingAppliance(string objectName)
    {
        Debug.Log(objectName);
        if (objectName != "Air Conditioner")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName);
    }
}
