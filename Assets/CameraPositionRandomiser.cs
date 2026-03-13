using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

/// <summary>
/// Randomizes the position and rotation of the Perception camera each iteration.
/// Attach this Randomizer to your PerceptionCamera's ScenarioBase.
/// </summary>
[AddRandomizerMenu("Custom/Camera Randomiser")]
public class CameraPositionRandomiser : Randomizer
{
    [Header("Target Camera")]
    [Tooltip("If left empty, will use Camera.main")]
    public Camera targetCamera;

    [Header("Position Randomization")]
    public bool randomizePosition = true;

    [Tooltip("Center point around which position is randomized")]
    public Vector3 positionCenter = new Vector3(0f, 1.5f, -3f);

    public FloatParameter xOffset = new FloatParameter
    {
        value = new UniformSampler(-1f, 1f)
    };

    public FloatParameter yOffset = new FloatParameter
    {
        value = new UniformSampler(-0.5f, 0.5f)
    };

    public FloatParameter zOffset = new FloatParameter
    {
        value = new UniformSampler(-0.5f, 0.5f)
    };

    [Header("Rotation Randomization")]
    public bool randomizeRotation = true;

    [Tooltip("Base rotation before applying random offsets")]
    public Vector3 rotationCenter = new Vector3(0f, 0f, 0f);

    [Tooltip("Random pitch offset (X axis), in degrees")]
    public FloatParameter pitchOffset = new FloatParameter
    {
        value = new UniformSampler(-15f, 15f)
    };

    [Tooltip("Random yaw offset (Y axis), in degrees")]
    public FloatParameter yawOffset = new FloatParameter
    {
        value = new UniformSampler(-30f, 30f)
    };

    [Tooltip("Random roll offset (Z axis), in degrees")]
    public FloatParameter rollOffset = new FloatParameter
    {
        value = new UniformSampler(-5f, 5f)
    };

    [Header("Look-At (optional)")]
    [Tooltip("If set, camera will look at this transform after position randomization (overrides rotation randomization)")]
    public Transform lookAtTarget;

    [Tooltip("Add small random noise on top of the look-at direction")]
    public bool addLookAtNoise = false;

    public FloatParameter lookAtNoise = new FloatParameter
    {
        value = new UniformSampler(-3f, 3f)
    };

    protected override void OnIterationStart()
    {
        Debug.Log($"Iteration {scenario.currentIteration}");
    // ... rest of code

        // Resolve camera reference
        Camera cam = targetCamera != null ? targetCamera : Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("[CameraRandomizer] No camera found. Assign one or ensure Camera.main exists.");
            return;
        }

        // --- Position ---
        if (randomizePosition)
        {
            Vector3 newPos = positionCenter + new Vector3(
                xOffset.Sample(),
                yOffset.Sample(),
                zOffset.Sample()
            );
            cam.transform.position = newPos;
        }

        // --- Rotation ---
        if (lookAtTarget != null)
        {
            // Look at target with optional noise
            cam.transform.LookAt(lookAtTarget);

            if (addLookAtNoise)
            {
                Vector3 noiseEuler = cam.transform.eulerAngles + new Vector3(
                    lookAtNoise.Sample(),
                    lookAtNoise.Sample(),
                    0f
                );
                cam.transform.eulerAngles = noiseEuler;
            }
        }
        else if (randomizeRotation)
        {
            Vector3 newRotation = rotationCenter + new Vector3(
                pitchOffset.Sample(),
                yawOffset.Sample(),
                rollOffset.Sample()
            );
            cam.transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}
