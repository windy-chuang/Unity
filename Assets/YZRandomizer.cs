using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;



[Serializable]
[AddRandomizerMenu("NomadZ/Y Z Randomizer")]
public class YZRandomizer : Randomizer
{
    [Header("X Offset Range")]
    public float minOffsety = -0.1f;
    public float maxOffsety = 0.1f;

    public float minOffsetz = -0.1f;
    public float maxOffsetz = 0.1f;


    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<YZRandomizerTag>();

        foreach (var tag in tags)
        {
            var obj = tag.transform;

            // Store original position safely per object
            Vector3 basePos = obj.position;

            float offsety = UnityEngine.Random.Range(minOffsety, maxOffsety);
            float offsetz = UnityEngine.Random.Range(minOffsetz, maxOffsetz);

            obj.position = new Vector3(
                basePos.x,
                basePos.y + offsety,
                basePos.z + offsetz
            );
        }
    }
}
