using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Terrain Tint Randomizer")]
public class TerrainTintRandomizer : Randomizer
{
    [Header("Tint Strength")]
    [Range(0f, 10f)]
    public float saturation = 0.3f;

    public float valueMin = 0.8f;
    public float valueMax = 1.0f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<TerrainTintRandomizerTag>();

        foreach (var tag in tags)
        {
            var renderer = tag.GetComponent<Renderer>();
            if (renderer == null) continue;

            Material mat = renderer.sharedMaterial;
            if (mat == null) continue;

            Color tint = UnityEngine.Random.ColorHSV(
                0f, 10f,           // hue
                0f, saturation,   // saturation
                valueMin, valueMax // brightness
            );

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", tint);
            else if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", tint);
        }
    }
}