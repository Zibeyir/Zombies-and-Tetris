using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float ZPosition;

    [Header("Speed Control")]
    [SerializeField] private float xSpeedFactor = 1f; 

    private Rigidbody rb;
    private Vector3 lastVelocity;

    private void Start()
    {
        ZPosition = transform.position.y;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
       // rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        LaunchBall();
    }

    private void LaunchBall()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            ZPosition,
            Random.Range(-1f, 1f)
        ).normalized;

        rb.velocity = randomDirection * initialSpeed * GridSelector.TimeSpeed;
    }

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity * GridSelector.TimeSpeed;

        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, ZPosition, transform.position.z);
        }

       

       
        if (rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Rotation
        transform.Rotate(Vector3.right * rotationSpeed * Time.fixedDeltaTime * rb.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 reflectDir = Vector3.Reflect(lastVelocity.normalized, contact.normal);

        rb.velocity = reflectDir * Mathf.Max(lastVelocity.magnitude, minSpeed);

        rb.AddForce(new Vector3(
            Random.Range(-0.5f, 0.5f),
            0,
            Random.Range(-0.5f, 0.5f)
        ) * bounceForce, ForceMode.Impulse);
    }
}
