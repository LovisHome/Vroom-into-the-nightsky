using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] [Tooltip("The speed in which the player flies.")] private float flySpeed;
    [SerializeField] [Tooltip("The speed in which the player is able to turn.")] private float yAmount;
    [SerializeField] [Tooltip("Particle Effect for the Speed.")] private GameObject speedEffect;

    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float drag = 0.995f;
    [SerializeField] private float steerAngle = 20f;
    [SerializeField] private float traction = 1f;

    private Vector3 moveForce; //The actual velocity-like force applied to the rigidbody

    private float yAxis;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        // Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 1f);

        // Adds Forward Force
        moveForce += transform.forward * flySpeed * Time.fixedDeltaTime;

        // Apply the Drag and Limit Speed
        moveForce *= drag;

        // Apply the Steering
        float steerInput = horizontalInput;
        float steerAmount = -steerInput * moveForce.magnitude * steerAngle * Time.fixedDeltaTime;
        yAxis += steerAmount;

        // Calculates Pitch (xAxis) and Roll (zAxis)
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        // Final Rotation
        Quaternion targetRotation = Quaternion.Euler(-pitch, -yAxis, roll);
        rb.MoveRotation(targetRotation);

        // Apply the Traction
        moveForce = Vector3.Lerp(moveForce, transform.forward * moveForce.magnitude, traction * Time.fixedDeltaTime);
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);

        // Apply final velocity
        rb.velocity = moveForce;

        Debug.Log($"Final Velocity: {rb.velocity} | Magnitude: {rb.velocity.magnitude}");

        // Debuging Help
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
        Debug.DrawRay(transform.position, rb.velocity, Color.yellow);

        //Accelerate/Break
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Acceleration();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Breaking();
        }

    }

    //Function for gradual addition of speed, with limit at 30, plus speed effect
    private void Acceleration()
    {
        speedEffect.SetActive(true);
        if (flySpeed >= 50f)
        {
            flySpeed = 50f;
        }
        else
        {
            flySpeed += 5f * Time.deltaTime;
        }
    }

    //Function to slowly substracts speed at a limit of 10, plus quit speed effect
    private void Breaking()
    {
        speedEffect.SetActive(false);
        if (flySpeed <= 20f)
        {
            flySpeed = 20f;
        }
        else
        {
            flySpeed -= 7f * Time.deltaTime;
        }
    }
}
