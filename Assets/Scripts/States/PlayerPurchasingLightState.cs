using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingLightState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;

    public PlayerPurchasingLightState(GameController gameController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        if (purchasingApplianceController.FanExists(applianceName))
        {
            Debug.Log("true");
        }
        // Get AC position
        Vector3 lightPosition = GetAppliancePosition(applianceName);

        // Get AC camera position & rotation
        Vector3 cameraPosition = new Vector3(0f, 0f, 0f);
        Quaternion cameraRotation = Quaternion.Euler(0f, 0f, 0f);
        GetCameraOptions(applianceName, ref cameraPosition, ref cameraRotation);

        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

        // Prepare AC purchase
        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        this.objectName = objectName;
        if (!lightPosition.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(lightPosition, this.objectName, applianceName);
        }
    }

    private void GetCameraOptions(string applianceName, ref Vector3 cameraPosition, ref Quaternion cameraRotation)
    {
        switch (applianceName)
        {
            case "Light Bedroom":
                cameraPosition = new Vector3(67.36755f, 11f, 54.09552f);
                cameraRotation = Quaternion.Euler(0f, -66.578f, 0f);
                break;
            case "Light Kitchen":
                cameraPosition = new Vector3(70.50482f, 11f, 52.69537f);
                cameraRotation = Quaternion.Euler(0f, -111.623f, 0f);
                break;
            case "Light Laundry":
                cameraPosition = new Vector3(66.29301f, 12f, 53.62359f);
                cameraRotation = Quaternion.Euler(0f, 14.569f, 0f);
                break;
            case "Light Living Room":
                cameraPosition = new Vector3(70.65f, 10f, 54.84f);
                cameraRotation = Quaternion.Euler(0f, 111.38f, 0f);
                break;
            default:
                break;
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
        Debug.Log(objectName);
        if (objectName != "Light")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}
