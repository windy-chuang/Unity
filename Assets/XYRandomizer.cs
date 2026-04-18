using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;



[Serializable]
[AddRandomizerMenu("NomadZ/X Y Randomizer")]
public class XYRandomizer : Randomizer
{
    [Header("X Offset Range")]
    public float minOffsetx = -0.1f;
    public float maxOffsetx = 0.1f;

    public float minOffsety = -0.1f;
    public float maxOffsety = 0.1f;


    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<XYRandomizerTag>();

        foreach (var tag in tags)
        {
            var obj = tag.transform;

            // Store original position safely per object
            Vector3 basePos = obj.position;

            float offsetx = UnityEngine.Random.Range(minOffsetx, maxOffsetx);
            float offsety = UnityEngine.Random.Range(minOffsety, maxOffsety);

            obj.position = new Vector3(
                basePos.x + offsetx,
                basePos.y + offsety,
                basePos.z
            );
        }
    }
}