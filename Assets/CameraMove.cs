using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float rotateSpeed = 200f;
    Vector3 direction = new Vector3(1f, 0f, -0.1f).normalized;



    void Update()
    {
        // Move forward
        //transform.position +=  moveSpeed * Time.deltaTime * direction;

        // Rotate around Y-axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
