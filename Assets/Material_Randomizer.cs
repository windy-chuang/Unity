using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Parameters;

public class MaterialRandomizer : Randomizer
{
    public Material[] materials;

    [System.Obsolete]
    protected override void OnIterationStart()
    {
        var renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            if (materials.Length == 0) return;

            int randomIndex = Random.Range(0, materials.Length);
            renderer.sharedMaterial = materials[randomIndex];
        }
    }
}