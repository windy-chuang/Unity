using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Wall Randomizer")]
public class WallRandomizer : Randomizer
{
    [Header("Floor Materials")]
    public Material[] wallMaterials;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<WallRandomizerTag>();

        if (wallMaterials == null || wallMaterials.Length == 0)
            return;

        // Pick ONE material for the entire scene
        Material chosenMaterial =
            wallMaterials[UnityEngine.Random.Range(0, wallMaterials.Length)];

        foreach (var tag in tags)
        {
            var renderers = tag.GetComponentsInChildren<Renderer>();

            foreach (var r in renderers)
            {
                if (r == null) continue;

                // apply same material to all floors
                r.sharedMaterial = chosenMaterial;
            }
        }
    }
}