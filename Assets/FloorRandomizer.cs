using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Floor Randomizer")]
public class FloorRandomizer : Randomizer
{
    [Header("Floor Materials")]
    public Material[] floorMaterials;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<FloorRandomizerTag>();

        if (floorMaterials == null || floorMaterials.Length == 0)
            return;

        // Pick ONE material for the entire scene
        Material chosenMaterial =
            floorMaterials[UnityEngine.Random.Range(0, floorMaterials.Length)];

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