using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Lighting Randomizer")]
public class LightingRandomizer : Randomizer
{
    [Header("Intensity (Lux)")]
    public float minIntensity = 50000f;
    public float maxIntensity = 130000f;

    [Header("Color Temperature (Kelvin)")]
    public float minTemperature = 4000f;
    public float maxTemperature = 7500f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<LightingRandomizerTag>();
        foreach (var tag in tags)
        {
            var light = tag.GetComponent<Light>();
            var hdLight = tag.GetComponent<HDAdditionalLightData>();

            if (hdLight == null || light == null)
            {
                Debug.LogWarning($"No HDR light found on {tag.name}");
                continue;
            }

            hdLight.SetIntensity(UnityEngine.Random.Range(minIntensity, maxIntensity));

            light.colorTemperature = UnityEngine.Random.Range(minTemperature, maxTemperature);
            light.useColorTemperature = true;
        }
    }
}