/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 13/09/2021
 |  Description: To manager all controllers and assign event to them
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject placementControllerGameObject;
    private IPlacementController placementController;
    public GameObject resourceControllerGameObject;
    private IResourceController resourceController;
    private EnergySystemObjectController purchasingObjectController;
    private ApplianceObjectController purchasingApplianceController;
    //private string type;

    // Controllers
    public IInputController inputController;
    public UIController uiController;

 
    // Data Repository
    public ObjectRepository objectRepository;
    public ApplianceRepository applianceRepository;

    // Layer mask
    public LayerMask inputMask;

    // Map Information
    public int width, height, length;
    public int cellSize = 1;

    // States
    private PlayerState state;
    public PlayerSelectionState selectionState;
    public PlayerSellingObjectState sellingObjectState;
    //public PlayerSellingApplianceState sellingApplianceState;

    // Purchasing States
    public PlayerPurchasingSolarPanelState purchasingSolarPanelState;
    public PlayerPurchasingDieselGeneratorState purchasingDieselGeneratorState;
    public PlayerPurchasingBatteryState purchasingBatteryState;
    public PlayerPurchasingWindTurbineState purchasingWindTurbineState;
    public PlayerPurchasingInvertorState purchasingInvertorState;
    public PlayerPurchasingChargeControllerState purchasingChargeControllerState;

    public PlayerPurchasingACState purchasingACState;
    public PlayerPurchasingWashingMachineState purchasingWashingMachineState;

    public Vector3 DieselGeneratorPosition, WindTurbinePosition, BatteryPosition, InvertorPosition, ChargeControllerPosition, ACPosition, WashingMachinePosition;

    public CameraMovement cameraMovementController;

    // Exploit the state for play mode test
    // Todo: Fix play mode tests
    public PlayerState State { get => state; }

    private void Awake()
    {
        
        // Create platform dependent compilation for multiple version of the game
        #if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputController = gameObject.AddComponent<InputController>();
        #endif

        #if (UNITY_IOS || UNITY_ANDROID)
               // further usage
        #endif

    }

    void Start()
    {
        placementController = placementControllerGameObject.GetComponent<IPlacementController>();
        resourceController = resourceControllerGameObject.GetComponent<IResourceController>();
        PrepareStates();
        PrepareGameComponents();
        // Add Listener to event
        AssignInputListeners();
        AssignUIControllerListeners();
    }


    private void PrepareStates()
    {
        purchasingObjectController = new EnergySystemObjectController(cellSize, width, height, length, placementController, objectRepository, applianceRepository, resourceController);
        purchasingApplianceController = new ApplianceObjectController(cellSize, width, height, length, placementController, objectRepository, applianceRepository, resourceController);
        resourceController.PrepareResourceController(purchasingObjectController);

        selectionState = new PlayerSelectionState(this, purchasingObjectController);
        //sellingApplianceState = new PlayerSellingApplianceState(this, purchasingApplianceController);
        sellingObjectState = new PlayerSellingObjectState(this, purchasingObjectController, purchasingApplianceController);
        purchasingSolarPanelState = new PlayerPurchasingSolarPanelState(this, purchasingObjectController);
        purchasingBatteryState = new PlayerPurchasingBatteryState(this, purchasingObjectController, BatteryPosition);
        purchasingWindTurbineState = new PlayerPurchasingWindTurbineState(this, purchasingObjectController, WindTurbinePosition);
        purchasingDieselGeneratorState = new PlayerPurchasingDieselGeneratorState(this, purchasingObjectController, DieselGeneratorPosition);
        purchasingChargeControllerState = new PlayerPurchasingChargeControllerState(this, purchasingObjectController, ChargeControllerPosition);
        purchasingInvertorState = new PlayerPurchasingInvertorState(this, purchasingObjectController, InvertorPosition);

        purchasingACState = new PlayerPurchasingACState(this, purchasingApplianceController, ACPosition);
        purchasingWashingMachineState = new PlayerPurchasingWashingMachineState(this, purchasingApplianceController, WashingMachinePosition);

        // initialize state
        state = selectionState;
    }

    private void PrepareGameComponents()
    {
        inputController.MouseInputMask = inputMask; // Mouse Input Layer Mask
        this.uiController.CameraMovementController = this.cameraMovementController;
        this.uiController.CameraMovementController.SetCameraLimitation(-10f, width + 10f, 4f, height + 10f, -10f, length + 10f); // Camera Setup
    }

    private void AssignUIControllerListeners()
    {
        uiController.AddListenerOnPurchasingEvent((objectName) => state.OnPuchasingEnergySystem(objectName));  // transfer to purchase energy system state
        uiController.AddListenerOnPurchasingApplianceEvent((objectName) => state.OnPuchasingAppliance(objectName));
        uiController.AddListenerOnCancelEvent(() => state.OnCancel());
        uiController.AddListenerOnSellEvent(() => state.OnSellingObject());
        //uiController.AddListenerOnSellApplianceEvent(() => state.OnSellingAppliance());
        uiController.AddListenerOnConfirmEvent(() => state.OnConfirm());
    }

    private void AssignInputListeners()
    {
        inputController.AddListenerOnPointerDownEvent((position) => state.OnInputPointerDown(position));
        inputController.AddListenerOnPointerChangeEvent((position) => state.OnInputPointerChange(position));
    }

    public void TransitionToState(PlayerState newState, string objectVariable)
    {
        this.state = newState;
        this.state.EnterState(objectVariable);
    }

}

