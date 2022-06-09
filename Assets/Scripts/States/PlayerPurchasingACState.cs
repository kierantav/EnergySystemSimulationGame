using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingACState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;

    public PlayerPurchasingACState(GameController gameController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        // Get AC position
        Vector3 ACPosition = new Vector3(0, 0, 0);
        ACPosition = GetACPosition(applianceName, ACPosition);

        // Get AC camera position & rotation
        Vector3 cameraPosition = new Vector3(0f, 0f, 0f);
        Quaternion cameraRotation = Quaternion.Euler(0f, 0f, 0f);
        GetCameraOptions(applianceName, ref cameraPosition, ref cameraRotation);

        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

        // Prepare AC purchase
        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        this.objectName = objectName;
        if (!ACPosition.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(ACPosition, this.objectName, applianceName);
        }
    }

    private void GetCameraOptions(string applianceName, ref Vector3 cameraPosition, ref Quaternion cameraRotation)
    {
        switch (applianceName)
        {
            case "Small AC":
                cameraPosition = new Vector3(61.22f, 12f, 57.24f);
                cameraRotation = Quaternion.Euler(0f, -74.871f, 0f);
                break;
            case "Medium AC":
                cameraPosition = new Vector3(69.46967f, 11f, 42.3863f);
                cameraRotation = Quaternion.Euler(0f, 27.215f, 0f);
                break;
            case "Large AC":
                cameraPosition = new Vector3(70.65f, 10f, 54.84f);
                cameraRotation = Quaternion.Euler(0f, 111.38f, 0f);
                break;
            default:
                break;
        }
    }

    private Vector3 GetACPosition(string applianceName, Vector3 ACPosition)
    {
        switch (applianceName)
        {
            case "Small AC":
                ACPosition = new Vector3(45.51f, 15f, 56.41f);
                break;
            case "Medium AC":
                ACPosition = new Vector3(77f, 14f, 61f);
                break;
            case "Large AC":
                ACPosition = new Vector3(96f, 14f, 38f);
                break;
            default:
                break;
        }

        return ACPosition;
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
        if (objectName != "Air Conditioner")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}
