using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Poster Randomizer")]
public class PosterRandomizer : Randomizer
{
    [Header("Poster Materials")]
    public Material[] posterMaterials;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<PosterRandomizerTag>();

        if (posterMaterials == null || posterMaterials.Length == 0)
            return;

        foreach (var tag in tags)
        {
            var renderers = tag.GetComponentsInChildren<Renderer>();
            if (renderers == null) continue;

            // each poster gets its OWN independent material
            Material chosenMaterial =
                posterMaterials[UnityEngine.Random.Range(0, posterMaterials.Length)];

            foreach (var r in renderers)
            {
                if (r == null) continue;

                r.sharedMaterial = chosenMaterial;
            }
        }
    }
}