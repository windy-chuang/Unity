using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Outside Randomizer")]
public class OutsideRandomizer : Randomizer
{
    [Header("Field Center (mean)")]
    public float centerX = 0f;
    public float centerZ = 3.5f;

    [Header("Spread (sigma)")]
    public float sigmaX = 2f;
    public float sigmaZ = 2f;

    [Header("Field Bounds (clamp)")]
    public float xstart = -6.0f;
    public float xend = 6.0f;
    public float zstart = 0f;
    public float zend = 7f;

    [Header("Roll (degrees)")]
    public float rollMean = 0f;
    public float rollSigma = 5f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<OutsideRandomizerTag>();
        foreach (var tag in tags)
        {
            var obj = tag.transform;

            // X and Z sampled independently (iid)
            float x = UnityEngine.Random.Range(xstart, xend);
            float z = Mathf.Clamp(SampleGaussian(centerZ, sigmaZ), zstart, zend);
            float y = obj.localPosition.y;

            obj.localPosition = new Vector3(x, y, z);

            // Rotate around world Y axis regardless of object's local axis orientation
            // float roll = SampleGaussian(rollMean, rollSigma);
            // obj.rotation = Quaternion.AngleAxis(roll, Vector3.up);

            Debug.Log($"{obj.name} spawned at world pos {obj.position}, local pos {obj.localPosition}");
        }
    }

    private float SampleGaussian(float mean, float sigma)
    {
        float u1 = 1f - UnityEngine.Random.value;
        float u2 = 1f - UnityEngine.Random.value;
        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        return mean + sigma * z;
    }
}