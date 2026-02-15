using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{   
    int zSpeed = 100;
    int rSpeed = 100;

    public float sampleDistance = 3f;
    public float tiltSpeed = 5f;
    private float currentYRotation = 0f;
    private Rigidbody rb;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        Terrain terrain = Terrain.activeTerrain;

        // Original movement code
        

        if(Input.GetKey(KeyCode.A))
        {
            currentYRotation -= rSpeed * Time.deltaTime;
        }
        
        else if(Input.GetKey(KeyCode.D))
        {
            currentYRotation += rSpeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, zSpeed * Time.deltaTime, Space.Self);
        }

        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -zSpeed * Time.deltaTime, Space.Self);
        }
        
        // Sample terrain heights
        Vector3 centerPos = transform.position;
        
        Vector3 forward = Quaternion.Euler(0, currentYRotation, 0) * Vector3.forward;
        Vector3 right = Quaternion.Euler(0, currentYRotation, 0) * Vector3.right;
        
        Vector3 frontPos = centerPos + forward * sampleDistance;
        Vector3 backPos = centerPos - forward * sampleDistance;
        Vector3 leftPos = centerPos - right * sampleDistance;
        Vector3 rightPos = centerPos + right * sampleDistance;

        float centerHeight = terrain.SampleHeight(centerPos);
        float frontHeight = terrain.SampleHeight(frontPos);
        float backHeight = terrain.SampleHeight(backPos);
        float leftHeight = terrain.SampleHeight(leftPos);
        float rightHeight = terrain.SampleHeight(rightPos);

        // Calculate pitch and roll
        float pitchAngle = Mathf.Atan2(backHeight - frontHeight, sampleDistance * 2) * Mathf.Rad2Deg;
        float rollAngle = Mathf.Atan2(rightHeight - leftHeight, sampleDistance * 2) * Mathf.Rad2Deg;

        // Apply rotation
        Quaternion targetRotation = Quaternion.Euler(pitchAngle, currentYRotation, rollAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);

        // Keep at terrain height
        Vector3 currentPosition = transform.position;
        currentPosition.y = centerHeight + 15;
        transform.position = currentPosition;
    }
}