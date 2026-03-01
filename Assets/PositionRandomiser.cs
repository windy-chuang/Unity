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
        x = new NormalSampler(-0.8f, 11.77f, 5f, 1f),
        y = new UniformSampler(0.92f, 0.92f),
        z = new NormalSampler(-0.38f, 9.3f, 4.5f, 1f)
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

