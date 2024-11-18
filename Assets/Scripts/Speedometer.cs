using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public RectTransform needle;
    public float maxSpeed = 200f;
    public float maxNeedleRotation = -130f; 
    public float minNeedleRotation = 130f;
    public float smoothing = 5f;

    private float currentNeedleRotation;

    void Update()
    {
        float speed = carRigidbody.velocity.magnitude * 3.6f;
        float speedPercentage = Mathf.Clamp01(speed / maxSpeed);
        float targetNeedleRotation = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, speedPercentage);
        currentNeedleRotation = Mathf.Lerp(currentNeedleRotation, targetNeedleRotation, Time.deltaTime * smoothing);
        needle.localRotation = Quaternion.Euler(0, 0, currentNeedleRotation);
    }
}
