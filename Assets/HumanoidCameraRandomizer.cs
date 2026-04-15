using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

/// <summary>
/// Randomizes a camera to simulate a humanoid robot's head-mounted camera
/// on a RoboCup soccer field. The camera is positioned at roughly head height,
/// and rotation is biased toward horizontal and downward-looking angles —
/// mimicking how a NAO/humanoid actually scans the field during a game.
///
/// Setup:
///   1. Add this Randomizer to your FixedLengthScenario via Add Randomizer.
///   2. Assign the cameraTransform to your scene Camera's Transform in the Inspector.
///      Leave null to auto-find Camera.main.
/// </summary>
[Serializable]
[AddRandomizerMenu("NomadZ/Humanoid Camera Randomizer")]

public class HumanoidCameraRandomizer : Randomizer
{
    // -------------------------------------------------------------------------
    // Robot position on field
    // -------------------------------------------------------------------------

    [Header("Robot Position on Field")]
    [Tooltip("Half-width of the RoboCup field (x-axis). Standard SPL field is 9m long, 6m wide.")]
    public float fieldWidth = 3.0f;

    [Tooltip("Half-length of the RoboCup field (z-axis).")]
    public float fieldHalfLength = 4.5f;

    [Tooltip("Camera height above ground. NAO upper camera is ~0.53m.")]
    public float cameraHeightMin = 0.50f;
    public float cameraHeightMax = 0.58f;

    // -------------------------------------------------------------------------
    // Pitch (up/down tilt)
    // NAO upper camera default: ~0 deg, lower camera: ~-38 deg
    // -------------------------------------------------------------------------

    [Header("Pitch (Vertical Tilt)")]
    [Tooltip("Range for the Gaussian mode. 0 = horizontal, negative = look down.")]
    public float pitchModeMin = -5f;
    public float pitchModeMax = -25f;

    [Tooltip("Hard clamp on pitch. Negative = downward.")]
    public float pitchMin = -60f;
    public float pitchMax = 15f;

    [Tooltip("Spread (sigma) of the Gaussian around the sampled pitch mode.")]
    public float pitchSigma = 10f;

    // -------------------------------------------------------------------------
    // Yaw (left/right pan)
    // -------------------------------------------------------------------------

    [Header("Yaw (Horizontal Pan)")]
    [Tooltip("Full yaw range in degrees. Robot mostly faces forward but scans sides.")]
    public float yawRange = 60f;  // ±30° from forward

    // -------------------------------------------------------------------------
    // Roll — small wobble only
    // -------------------------------------------------------------------------

    [Header("Roll")]
    [Tooltip("Sigma of roll in degrees. Simulates slight lean from walking phase.")]
    public float rollSigma = 3f;

    // -------------------------------------------------------------------------
    // Camera reference
    // -------------------------------------------------------------------------

    [Header("Camera Transform")]
    [Tooltip("Transform to randomize. Leave null to auto-find Camera.main.")]
    public Transform cameraTransform;

    // -------------------------------------------------------------------------
    // Randomizer lifecycle
    // -------------------------------------------------------------------------

    protected override void OnAwake()
    {
        if (cameraTransform == null)
        {
            var cam = Camera.main;
            if (cam != null)
                cameraTransform = cam.transform;
            else
                Debug.LogWarning("[HumanoidCameraRandomizer] No cameraTransform assigned and Camera.main not found.");
        }
    }

    protected override void OnIterationStart()
    {
        if (cameraTransform == null) return;

        RandomizePosition();
        RandomizeRotation();
    }

    // -------------------------------------------------------------------------
    // Position
    // -------------------------------------------------------------------------

    float Gaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - UnityEngine.Random.value; // (0,1]
        float u2 = 1.0f - UnityEngine.Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }
    private void RandomizePosition()
    {
        float z = Gaussian(fieldWidth / 2f, fieldWidth / 6f);
        float x = Gaussian(0f, fieldHalfLength / 3f);
        float y = UnityEngine.Random.Range(cameraHeightMin, cameraHeightMax);
        cameraTransform.position = new Vector3(x, y, z);
    }

    // -------------------------------------------------------------------------
    // Rotation
    // -------------------------------------------------------------------------

    private void RandomizeRotation()
    {
        // Yaw: uniform within ±yawRange/2 from world forward (+Z)
        float yaw = UnityEngine.Random.Range(-yawRange * 0.5f, yawRange * 0.5f);

        // Pitch: pick a mode in [pitchModeMin, pitchModeMax], then draw from
        // a Gaussian around it — views cluster near horizontal/downward
        
        float pitch = UnityEngine.Random.Range(pitchModeMin, pitchModeMax);

        // Roll: small Gaussian wobble around 0
        float roll = SampleGaussianClamped(0f, rollSigma, -10f, 10f);

        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, roll);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    /// <summary>
    /// Samples a Gaussian N(mean, sigma) clamped to [min, max].
    /// </summary>
    private float SampleGaussianClamped(float mean, float sigma, float min, float max)
    {
        float sample = mean + sigma * BoxMullerSample();
        return Mathf.Clamp(sample, min, max);
    }

    /// <summary>
    /// Box-Muller transform — returns a standard normal N(0,1) sample.
    /// </summary>
    private float BoxMullerSample()
    {
        float u1 = 1f - UnityEngine.Random.value;  // avoid log(0)
        float u2 = 1f - UnityEngine.Random.value;
        return Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2);
    }
}