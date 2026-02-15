using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public Transform target; // The vehicle to follow
    public Vector3 offset = new Vector3(0, 20, -40); // Camera offset from vehicle (behind and above)
    public float smoothSpeed = 100f; // How smoothly the camera follows
    public float lookAtHeight = 2f; // Height offset for where camera looks at vehicle

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("FollowCamera: No target assigned!");
            return;
        }

        // Calculate the desired position based on the vehicle's position and rotation
        // This keeps the camera behind the vehicle as it rotates
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Look at a point slightly above the vehicle's position
        Vector3 lookAtTarget = target.position + Vector3.up * lookAtHeight;
        transform.LookAt(lookAtTarget);
    }
}