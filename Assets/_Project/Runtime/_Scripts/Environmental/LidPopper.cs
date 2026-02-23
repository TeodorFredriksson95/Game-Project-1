using UnityEngine;

public class PopFlyer : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField, Tooltip("Base upward force at start. Randomized slightly each time.")]
    float upForce = 6.5f;

    [SerializeField, Tooltip("Horizontal randomness of launch direction.")]
    float lateralVariance = 0.35f;

    [Header("Rotation Settings")]
    [SerializeField, Tooltip("Initial rotational strength.")]
    float torqueStrength = 15f;

    [SerializeField, Tooltip("How quickly the spin slows down over time.")]
    float angularDamping = 2f;

    [Header("Lifetime Settings")]
    [SerializeField] float lifetime = 2.0f;
    [SerializeField] bool removeAllColliders = true;
    [SerializeField] bool gravity = true;

    Rigidbody rb;
    float timer;

    void Awake()
    {
        if (removeAllColliders)
        {
            foreach (var col in GetComponentsInChildren<Collider>(includeInactive: true))
                Destroy(col);
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = gravity;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Start()
    {
        // Random direction slightly off-center
        Vector3 randomDir = (Vector3.up * (1f + Random.Range(0.1f, 0.3f))) +
                            (Random.insideUnitSphere * lateralVariance);

        // Add stronger upward impulse
        rb.AddForce(randomDir.normalized * upForce, ForceMode.VelocityChange);

        // Add random spin
        rb.AddTorque(Random.onUnitSphere * torqueStrength, ForceMode.VelocityChange);

        // Schedule destruction
        if (lifetime > 0f)
            Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        // Gradually slow down rotation over time
        if (rb != null)
        {
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, angularDamping * Time.fixedDeltaTime);
        }
    }

    public void Configure(float upForce, float torqueStrength, float lifetime, bool removeAllColliders, bool gravity)
    {
        this.upForce = upForce;
        this.torqueStrength = torqueStrength;
        this.lifetime = lifetime;
        this.removeAllColliders = removeAllColliders;
        this.gravity = gravity;
    }
}
