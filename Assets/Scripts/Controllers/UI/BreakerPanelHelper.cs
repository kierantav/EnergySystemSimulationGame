using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakerPanelHelper : MonoBehaviour
{
    public SwitchManager invertorSwitch, mainLoadSwitch, dieselGeneratorSwitch;
    public TextMeshProUGUI currentLoad;
    public GameObject mainLoadPanel;
    public TMP_InputField loadValue;
    public Button saveBtn, closeLoadBtn, closeBreakerPanelBtn, openLoadPanelBtn;
    public NotificationManager saveddNotification;
    public NotificationManager failedNotification;

    private float load = 0;
    private bool isInterverSwitchOn = false;
    private bool isMainLoadSwitchOn = false;
    private bool isDGSwitchOn = false;

    public bool IsInterverSwitchOn { get => isInterverSwitchOn;  }
    public bool IsMainLoadSwitchOn { get => isMainLoadSwitchOn; }
    public bool IsDGSwitchOn { get => isDGSwitchOn; }
    public float Load { get => load; set => load = value; }


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        HideLoadPanel();
        UpdateLoadValueUI();
        saveBtn.onClick.AddListener(Save);
        closeLoadBtn.onClick.AddListener(CloseLoadPanel);
        closeBreakerPanelBtn.onClick.AddListener(CloseBreakerPanel);
        openLoadPanelBtn.onClick.AddListener(ShowLoadPanel);
    }


    private void Save()
    {
        GetLoadValue();
        UpdateLoadValueUI();
    }

    public void UpdateLoadValueUI()
    {
        currentLoad.text = "Your Property's Current Load is: " + load + " kwh.";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMainLoadUI();
        UpdateInvertorSwitchUI();
        UpdateDieselGeneratorSwitchUI();

    }

    private void UpdateDieselGeneratorSwitchUI()
    {
        if (dieselGeneratorSwitch.isOn)
        {
            isDGSwitchOn = true;
        }
        else
        {
            isDGSwitchOn = false;
        }
    }

    private void UpdateInvertorSwitchUI()
    {
        if (invertorSwitch.isOn)
        {
            isInterverSwitchOn = true;
        }
        else
        {
            isInterverSwitchOn = false;
        }
    }

    private void UpdateMainLoadUI()
    {
        if (mainLoadSwitch.isOn)
        {
            isMainLoadSwitchOn = true;
        }
        else
        {
            isMainLoadSwitchOn = false;
        }
    }

    private void GetLoadValue()
    {
        try
        {
            load = float.Parse(loadValue.text);
            saveddNotification.OpenNotification();
        }
        catch
        {
            failedNotification.OpenNotification();
        }
    }

    private void HideLoadPanel()
    {
        mainLoadPanel.SetActive(false);
    }

    private void ShowLoadPanel()
    {
        mainLoadPanel.SetActive(true);
    }
    private void CloseLoadPanel()
    {
        HideLoadPanel();
    }
    private void CloseBreakerPanel()
    {
        gameObject.SetActive(false);
    }

}
