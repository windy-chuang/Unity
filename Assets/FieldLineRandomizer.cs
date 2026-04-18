using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Field Line Randomizer")]
public class FieldLineRandomizer : Randomizer
{
    [Header("Fade Factor")]
    public float minFade = 0.5f;
    public float maxFade = 1.0f;

    [Header("Tiling (line width)")]
    public float minTiling = 0.8f;
    public float maxTiling = 1.2f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<FieldLineRandomizerTag>();
        foreach (var tag in tags)
        {
            var decal = tag.GetComponentInChildren<DecalProjector>();
            if (decal == null)
            {
                Debug.LogWarning($"No DecalProjector found on {tag.name}");
                continue;
            }

            // Fade
            decal.fadeFactor = UnityEngine.Random.Range(minFade, maxFade);

            // Tiling — scales the projected texture, affecting perceived line width
            float tiling = UnityEngine.Random.Range(minTiling, maxTiling);
            decal.uvScale = new Vector2(tiling, tiling);
        }
    }
}