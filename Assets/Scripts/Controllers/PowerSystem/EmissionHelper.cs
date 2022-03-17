using System;
using System.Collections.Generic;

public class EmissionHelper
{
    private float solarPanelEmissionAmount = 0f;
    private float dieselGeneratorEmissionAmout = 0f;
    private float windTurbineEmissionAmout = 0f;

    public EmissionHelper()
    {
    }


    public void CalculateEmissions(IEnumerable<EnergySystemGeneratorBaseSO> objects, float period)
    {
        foreach (var obj in objects)
        {
            //Debug.Log(obj.objectName);
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
            //todo
        }
    }

    private void CalculateWindTurbineEmissions(float period, EnergySystemGeneratorBaseSO obj)
    {
        windTurbineEmissionAmout += period * obj.emissionRate;
    }

    private void CalculateDieselGeneratorEmissions(float period, EnergySystemGeneratorBaseSO obj)
    {
        dieselGeneratorEmissionAmout += period * obj.emissionRate;
    }

    private void CalculateSolarPanelEmissions(float period, EnergySystemGeneratorBaseSO obj)
    {
        solarPanelEmissionAmount += period * obj.emissionRate;
    }
}
