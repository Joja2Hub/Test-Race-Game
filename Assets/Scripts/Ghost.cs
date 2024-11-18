using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ghost : MonoBehaviour
{
    public GameObject car;
    public GameObject ghostPrefab; // Префаб призрака
    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();
    private List<float> timeStamps = new List<float>();
    private bool isRecording = false;
    private float startTime;

    // Метод для начала записи
    public void StartRecording()
    {
        Debug.Log("StartRecord");
        positions.Clear();
        rotations.Clear();
        timeStamps.Clear();
        isRecording = true;
        startTime = Time.time;
    }

    // Метод для остановки записи
    public void StopRecording()
    {
        isRecording = false;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            positions.Add(car.transform.position);
            rotations.Add(car.transform.rotation);
            timeStamps.Add(Time.time - startTime);
        }
    }

    public void StartGhost()
    {
        Debug.Log("StartGhost");
        GameObject ghost = Instantiate(ghostPrefab, positions[0], rotations[0]);
        ghost.AddComponent<GhostPlayback>().Initialize(positions, rotations, timeStamps);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartLine"))
        {
            StartRecording(); 
        }

        if (other.CompareTag("FinishLine"))
        {
            StopRecording();
            StartGhost();
        }
    }


}
