using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;



[Serializable]
[AddRandomizerMenu("NomadZ/R Goal Randomizer")]
public class RGoalRandomizer : Randomizer
{
    [Header("X Offset Range")]
    public float minOffsetx = -0.1f;
    public float maxOffsetx = 0.1f;

    public float minOffsety = -0.1f;
    public float maxOffsety = 0.1f;


    protected override void OnIterationStart()
    {
        // Generate once, shared by all tagged objects
        float offsetX = UnityEngine.Random.Range(minOffsetx, maxOffsetx);
        float offsetY = UnityEngine.Random.Range(minOffsety, maxOffsety);

        var tags = tagManager.Query<RGoalRandomizerTag>();
        foreach (var tag in tags)
        {
            var obj = tag.transform;
            Vector3 basePos = obj.position;
            obj.position = new Vector3(
                basePos.x + offsetX,
                basePos.y + offsetY,
                basePos.z
            );
        }
    }
}
