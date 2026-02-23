using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [SerializeField, Tooltip("How high/low the object moves between 0.45 and 0.55.")]
    private float amplitude = 0.075f;

    [SerializeField, Tooltip("Speed of vertical bobbing.")]
    private float frequency = 2.0f;

    [SerializeField, Tooltip("Rotation speed in degrees per second around Y.")]
    private float rotationSpeed = 35f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        // Bob between -amplitude and +amplitude
        float newY = startY + Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply smooth vertical motion
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotate slowly around Y
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
