using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Info Board Randomizer")]
public class InfoBoardRandomizer : Randomizer
{
    [Header("Field Dimensions")]
    public float xstart = -6.0f;        
    public float xend = 6.0f;
    public float zstart = 0f; 

    public float zend = 7f;
    [Header("Roll")]
    public float rollSigma = 5f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<InfoBoardRandomizerTag>();

        foreach (var tag in tags)
        {
            var obj = tag.transform;

            // -------------------------
            // Position (Gaussian)
            // -------------------------
            
            // Keep inside field
            float x = UnityEngine.Random.Range(xstart, xend);
            float z = UnityEngine.Random.Range(zstart, zend);

            float y = obj.localPosition.y; // keep ground height

            obj.localPosition = new Vector3(x, y, z);

            // -------------------------
            // Roll only (Z axis)
            // -------------------------
            float roll = UnityEngine.Random.Range(0, 7);

            Vector3 euler = obj.eulerAngles;
            obj.rotation = Quaternion.Euler(euler.x, euler.y, roll);
            Debug.Log($"{obj.name} spawned at world pos {obj.position}, local pos {obj.localPosition}");
        }
    }

}