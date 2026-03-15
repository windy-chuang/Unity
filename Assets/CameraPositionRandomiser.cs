using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

/// <summary>
/// Randomizes the position and rotation of the Perception camera each iteration.
/// Camera is placed anywhere inside the field rectangle, mostly looking toward the center.
/// </summary>
[AddRandomizerMenu("Custom/Camera Randomiser")]
public class CameraPositionRandomiser : Randomizer
{
    [Header("Target Camera")]
    [Tooltip("If left empty, will use Camera.main")]
    public Camera targetCamera;

    [Header("Field Bounds")]
    [Tooltip("World-space center of the field")]
    public Vector3 fieldCenter = new Vector3(0f, 0f, 0f);

    [Tooltip("Half-extents of the allowed camera region (X = width, Z = depth)")]
    public Vector3 fieldHalfExtents = new Vector3(4.5f, 0f, 3f);

    [Header("Camera Height")]
    [Tooltip("Camera height above fieldCenter.Y")]
    public FloatParameter cameraHeight = new FloatParameter
    {
        value = new UniformSampler(0.3f, 2.5f)
    };

    [Header("Look-At Jitter")]
    [Tooltip("Random offset applied to the look-at point on X and Z")]
    public FloatParameter lookAtJitterXZ = new FloatParameter
    {
        value = new UniformSampler(-1f, 1f)
    };

    [Tooltip("Random offset applied to the look-at point on Y")]
    public FloatParameter lookAtJitterY = new FloatParameter
    {
        value = new UniformSampler(-0.3f, 0.8f)
    };

    [Header("Outward-Facing Shots")]
    [Tooltip("Probability [0-1] that the camera looks away from the field center")]
    [Range(0f, 1f)]
    public float outwardFacingProbability = 0.05f;

    [Tooltip("Yaw noise when facing outward (degrees)")]
    public FloatParameter outwardYawNoise = new FloatParameter
    {
        value = new UniformSampler(-20f, 20f)
    };

    protected override void OnIterationStart()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("[CameraRandomizer] No camera found. Assign one or ensure Camera.main exists.");
            return;
        }

        // --- 1. Sample position uniformly inside the field rectangle ---
        Vector3 camPos = new Vector3(
            UnityEngine.Random.Range(fieldCenter.x - fieldHalfExtents.x, fieldCenter.x + fieldHalfExtents.x),
            fieldCenter.y + cameraHeight.Sample(),
            UnityEngine.Random.Range(fieldCenter.z - fieldHalfExtents.z, fieldCenter.z + fieldHalfExtents.z)
        );
        cam.transform.position = camPos;

        // --- 2. Decide look-at behavior ---
        bool lookOutward = UnityEngine.Random.value < outwardFacingProbability;

        if (lookOutward)
        {
            // Face a random direction with some yaw noise
            float randomYaw = UnityEngine.Random.Range(0f, 360f);
            cam.transform.rotation = Quaternion.Euler(0f, randomYaw + outwardYawNoise.Sample(), 0f);
        }
        else
        {
            // Look toward field center with jitter
            Vector3 lookTarget = fieldCenter + new Vector3(
                lookAtJitterXZ.Sample(),
                lookAtJitterY.Sample(),
                lookAtJitterXZ.Sample()
            );

            // Avoid LookAt singularity if camera is exactly at the look target
            if ((lookTarget - camPos).sqrMagnitude > 0.001f)
                cam.transform.LookAt(lookTarget);
        }

        Debug.Log($"[CameraRandomizer] Iter {scenario.currentIteration} | pos={camPos} | outward={lookOutward}");
    }
}