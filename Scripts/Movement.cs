using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{   
    public int zSpeed;
    public int rSpeed;
    GameObject[] cars = new GameObject[3];
    public GameObject shooterCar;
    public GameObject speederCar;
    public GameObject slowBoi;
    int cycler = 0;
    public float sampleDistance = 3f;
    public float tiltSpeed = 5f;
    private float currentYRotation = 0f;
    private Rigidbody rb;
    public followCamera cameraScript;
    
    
    void Start()
    {
        cars[0] = shooterCar;
        cars[1] = speederCar;
        cars[2] = slowBoi;
        shooterCar.SetActive(true);
        speederCar.SetActive(false);
        slowBoi.SetActive(false);
        cameraScript.target = shooterCar.transform;
        LoadCarStats(cars[cycler]);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            cars[cycler].SetActive(false);
            if(cycler == 2){
                cycler = 0;
            }
            else{
                cycler++;
            }
            cars[cycler].SetActive(true);
            cameraScript.target = cars[cycler].transform;
            LoadCarStats(cars[cycler]);
        }
        
        GameObject currentCar = cars[cycler];  // Get the active car
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
             currentCar.transform.Translate(0, 0, zSpeed * Time.deltaTime, Space.Self);
        }

        if(Input.GetKey(KeyCode.S))
        {
             currentCar.transform.Translate(0, 0, -zSpeed * Time.deltaTime, Space.Self);
        }
        
        // Sample terrain heights
        Vector3 centerPos = currentCar.transform.position;
        
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
        currentCar.transform.rotation = Quaternion.Slerp(currentCar.transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);

        // Keep at terrain height
        Vector3 currentPosition = currentCar.transform.position;
        currentPosition.y = centerHeight + 15;
        currentCar.transform.position = currentPosition;
    }
    void LoadCarStats(GameObject car){
        CarStats stats = car.GetComponent<CarStats>();
        if(stats != null){
            zSpeed = stats.zSpeed;
            rSpeed = stats.rSpeed;
        }
    }
}