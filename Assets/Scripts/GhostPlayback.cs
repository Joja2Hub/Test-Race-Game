using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayback : MonoBehaviour
{
    private List<Vector3> positions;
    private List<Quaternion> rotations;
    private List<float> timeStamps;
    private int index = 0;
    private float playbackStartTime;

    public void Initialize(List<Vector3> pos, List<Quaternion> rot, List<float> time)
    {
        positions = pos;
        rotations = rot;
        timeStamps = time;
        playbackStartTime = Time.time;
    }

    void Update()
    {
        if (positions == null || rotations == null || timeStamps == null || positions.Count == 0)
        {
            return; 
        }

        if (index < positions.Count - 1)
        {
            float elapsedTime = Time.time - playbackStartTime;
            if (elapsedTime >= timeStamps[index + 1])
            {
                index++;
            }

            if (index < positions.Count - 1)
            {
                float lerpFactor = (elapsedTime - timeStamps[index]) / (timeStamps[index + 1] - timeStamps[index]);
                transform.position = Vector3.Lerp(positions[index], positions[index + 1], lerpFactor);
                transform.rotation = Quaternion.Slerp(rotations[index], rotations[index + 1], lerpFactor);
            }
        }
    }
}
