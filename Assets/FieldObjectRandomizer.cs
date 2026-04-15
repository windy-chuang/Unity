using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("NomadZ/Field Object Randomizer")]
public class FieldObjectRandomizer : Randomizer
{
    [Header("Field Dimensions")]
    public float fieldWidth = 3.0f;        // Z axis (0 → width)
    public float fieldHalfLength = 4.5f;   // X axis (-L → +L)

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
            float x = Gaussian(0f, fieldHalfLength / 3f);
            float z = Gaussian(fieldWidth / 2f, fieldWidth / 6f);

            // Keep inside field
            x = Mathf.Clamp(x, -fieldHalfLength, fieldHalfLength);
            z = Mathf.Clamp(z, 0f, fieldWidth);

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