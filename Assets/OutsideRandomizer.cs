using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Outside Randomizer")]
public class OutsideRandomizer : Randomizer
{
    [Header("Field Dimensions")]
    public float zstart = -5.0f;       
    public float zend = 8.0f;
    public float xstart = -13.0f;   
    public float xend = 0.0f;

    [Header("Roll")]
    public float rollSigma = 5f;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<FieldObjectRandomizerTag>();

        foreach (var tag in tags)
        {
            var obj = tag.transform;

            // -------------------------
            // Position (Gaussian)
            // -------------------------
            float x = UnityEngine.Random.Range(xstart, xend);
            float z = UnityEngine.Random.Range(zstart, zend);

            float y = obj.position.y; // keep ground height

            obj.position = new Vector3(x, y, z);

            // -------------------------
            // Roll only (Z axis)
            // -------------------------
            float roll = SampleGaussianClamped(0f, rollSigma, -15f, 15f);

            Vector3 euler = obj.eulerAngles;
            obj.rotation = Quaternion.Euler(euler.x, euler.y, roll);
        }
    }

    // -------------------------
    // Gaussian helpers
    // -------------------------

    float Gaussian(float mean, float stdDev)
    {
        float u1 = 1f - UnityEngine.Random.value;
        float u2 = 1f - UnityEngine.Random.value;

        float randStdNormal =
            Mathf.Sqrt(-2f * Mathf.Log(u1)) *
            Mathf.Cos(2f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }

    float SampleGaussianClamped(float mean, float sigma, float min, float max)
    {
        float sample = mean + sigma * Gaussian(0f, 1f);
        return Mathf.Clamp(sample, min, max);
    }
}