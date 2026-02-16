using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShake2 : MonoBehaviour
{
    public float shakeIntensity = 0.05f; // Small amount for vibration
    private Vector3 originalPosition;


    void Update()
    {
   

        // Check if ANY keyboard or mouse input is NOT present
        if (!Input.anyKey)
        {
            originalPosition = transform.position;

            // Apply a small random quiver
            transform.position = originalPosition + (Random.insideUnitSphere * shakeIntensity);
        }

       

    }

}