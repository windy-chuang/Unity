using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Samplers;


[Serializable]
[AddRandomizerMenu("Perception/Position Randomizer")]
public class PositionRandomizer : Randomizer
{
    // The area in which objects can be spawned
    public Vector3Parameter spawnArea = new Vector3Parameter
    {
        x = new UniformSampler(-2f, 6f),
        y = new UniformSampler(0.05f, 0.05f),
        z = new UniformSampler(-2f, 5f)
    };

    protected override void OnIterationStart()
    {
        foreach (var tagged in tagManager.Query<PositionRandomizerTag>())
        {
            Vector3 randomOffset = spawnArea.Sample();
            tagged.transform.localPosition = randomOffset;
        }
    }
}

