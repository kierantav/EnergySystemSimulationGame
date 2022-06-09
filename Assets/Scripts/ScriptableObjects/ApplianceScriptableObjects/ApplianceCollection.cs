using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Appliance Collection", menuName = "EnergySystemSimulation/ApplianceCollection")]
public class ApplianceCollection : ScriptableObject
{
    public ACSO smallACSO, mediumACSO, largeACSO;
    public WashingMachineSO smallWasherSO, largeWasherSO;
    //public LightSO lightSO;
    public FridgeSO smallFridgeSO, largeFridgeSO;
    //public FanSO fanSO;
    //public DryerSO dryerSO;

}
