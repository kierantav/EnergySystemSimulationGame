using System;
using System.Collections.Generic;

public class EmissionHelper
{
    private float solarPanelEmissionAmount = 0f;
    private float dieselGeneratorEmissionAmout = 0f;
    private float windTurbineEmissionAmout = 0f;
    private PowerHelper powerHelper;

    private bool isPowerLinesRunning, isDGRunning = false;

    public EmissionHelper(PowerHelper powerHelper)
    {
        this.powerHelper = powerHelper;
    }


    public void CalculateEmissions(IEnumerable<EnergySystemGeneratorBaseSO> objects, float period)
    {
        foreach (var obj in objects)
        {
            switch (obj.objectName)
            {
                case "On-Grid Power":
                    CalculateGridPowerEmissions(obj, period);
                    break;
                case "Diesel Generator":
                    CalculateDieselGeneratorEmissions(obj, period);
                    break;
                case "Solar Panel":
                    CalculateSolarPanelOffset(obj, period);
                    break;
                case "Wind Turbine":
                    break;
                default:
                    break;
            }
            /*//Debug.Log(obj.objectName);
            if (obj.objectName == "Solar Panel")
            {
                CalculateSolarPanelEmissions(period, obj);
            }
            if (obj.objectName == "Diesel Generator")
            {
                CalculateDieselGeneratorEmissions(period, obj);
            }
            if(obj.objectName == "Wind Turbine")
            {
                CalculateWindTurbineEmissions(period, obj);
            }
            //todo*/
        }
    }

    private void CalculateGridPowerEmissions(EnergySystemGeneratorBaseSO obj, float period)
    {
        if (obj.isRunning)
        {
            isPowerLinesRunning = true;
            obj.emissionGeneratedAmount += obj.emissionRate * period;
        } else
        {
            isPowerLinesRunning = false;
        }
    }
    private void CalculateDieselGeneratorEmissions(EnergySystemGeneratorBaseSO obj, float period)
    {
        if (obj.isRunning)
        {
            isDGRunning = true;
            obj.emissionGeneratedAmount += obj.emissionRate * period;
        }
        else
        {
            isDGRunning = false;
        }
    }

    private void CalculateSolarPanelOffset(EnergySystemGeneratorBaseSO obj, float period)
    {
        if (powerHelper.CanRenewableSystemHandleLoad && (!isPowerLinesRunning && !isDGRunning))
        {
            obj.emissionGeneratedAmount += obj.emissionRate * period;
        }
    }

    private void CalculateWindTurbineEmissions(float period, EnergySystemGeneratorBaseSO obj)
    {
        windTurbineEmissionAmout += period * obj.emissionRate;
    }

    

    
}
