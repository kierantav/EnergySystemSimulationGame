using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceObjectController : MonoBehaviour
{
    public GameObject applianceTempObject;

    public void PurchasingApplianceObject(string objectName)
    {
        applianceTempObject.name = objectName;
        Instantiate(applianceTempObject, Vector3.zero, Quaternion.identity);
    }

}
