using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Board Randomizer")]
public class BoardRandomizer : Randomizer
{
    [Header("Materials")]
    [Tooltip("Main material used 70% of the time")]
    public Material primaryMaterial;

    [Tooltip("Other materials used 30% of the time")]
    public Material[] otherMaterials;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<BoardRandomizerTag>();

        if (tags == null)
            return;

        // Pick ONE material for the entire iteration
        Material chosenMaterial = ChooseMaterial();

        foreach (var tag in tags)
        {
            var renderers = tag.GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                if (renderer == null)
                    continue;

                // sharedMaterial ensures consistency across objects
                renderer.sharedMaterial = chosenMaterial;
            }
        }
    }

    /// <summary>
    /// 70% chance primary material, 30% split across others.
    /// </summary>
    private Material ChooseMaterial()
    {
        float r = UnityEngine.Random.value;

        // 70% primary material
        if (r < 0.7f || otherMaterials == null || otherMaterials.Length == 0)
        {
            return primaryMaterial;
        }

        // 30% distributed among other materials
        int index = UnityEngine.Random.Range(0, otherMaterials.Length);
        return otherMaterials[index];
    }
}