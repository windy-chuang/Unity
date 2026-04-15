using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;



[Serializable]
[AddRandomizerMenu("NomadZ/X Offset Randomizer")]
public class XOffsetRandomizer : Randomizer
{
    [Header("X Offset Range")]
    public float minOffset = -0.1f;
    public float maxOffset = 0.1f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<XOffsetRandomizerTag>();

        foreach (var tag in tags)
        {
            var obj = tag.transform;

            // Store original position safely per object
            Vector3 basePos = obj.position;

            float offset = UnityEngine.Random.Range(minOffset, maxOffset);

            obj.position = new Vector3(
                basePos.x + offset,
                basePos.y,
                basePos.z
            );
        }
    }
}