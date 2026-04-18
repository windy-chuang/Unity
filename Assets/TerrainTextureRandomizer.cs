using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;



[Serializable]
[AddRandomizerMenu("NomadZ/Terrain Texture Randomizer")]
public class TerrainTextureRandomizer : Randomizer
{
    [Header("Grass Textures (Diffuse/BaseMap)")]
    public Texture2D[] grassTextures;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<TerrainTextureRandomizerTag>();

        if (grassTextures == null || grassTextures.Length == 0)
            return;

        // Pick ONE texture for this iteration
        Texture2D chosenTexture =
            grassTextures[UnityEngine.Random.Range(0, grassTextures.Length)];

        foreach (var tag in tags)
        {
            var terrain = tag.GetComponent<Terrain>();
            if (terrain == null) continue;

            var terrainData = terrain.terrainData;
            var layers = terrainData.terrainLayers;

            if (layers == null || layers.Length == 0)
                continue;

            // Clone first layer (assumes grass is layer 0)
            TerrainLayer originalLayer = layers[0];
            TerrainLayer newLayer = new TerrainLayer();

            newLayer.diffuseTexture = chosenTexture;
            newLayer.normalMapTexture = originalLayer.normalMapTexture;

            // Copy important properties
            newLayer.tileSize = originalLayer.tileSize;
            newLayer.tileOffset = originalLayer.tileOffset;
            newLayer.metallic = originalLayer.metallic;
            newLayer.smoothness = originalLayer.smoothness;

            // Replace layer
            layers[0] = newLayer;
            terrainData.terrainLayers = layers;
        }
    }
}