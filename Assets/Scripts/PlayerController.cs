using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] [Tooltip("The speed in which the player flies.")] private float flySpeed;
    [SerializeField] [Tooltip("The speed in which the player is able to turn.")] private float yAmount;
    [SerializeField] private GameObject speedEffect;

    private float yAxis;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * flySpeed;

        //Rotation on Y Axis to get TurnMovement or Left and Right Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        yAxis += horizontalInput * yAmount * Time.deltaTime;
        float xAxis = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float zAxis = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        rb.rotation = Quaternion.Euler(Vector3.up * yAxis + Vector3.left * xAxis + Vector3.forward * zAxis);

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
        if (flySpeed >= 30f)
        {
            flySpeed = 30f;
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
        if (flySpeed <= 10f)
        {
            flySpeed = 10f;
        }
        else
        {
            flySpeed -= 7f * Time.deltaTime;
        }
    }
}
