using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Physics for the Player")]
    [SerializeField] [Tooltip("Particle Effect for the Speed.")] private GameObject speedEffect;
    [SerializeField] [Tooltip("The speed in which the player flies.")] private float flySpeed;
    [SerializeField] [Tooltip("The speed in which the player is able to turn.")] private float yAmount;
    [SerializeField] [Tooltip("The maximum amount of speed the player can have.")] private float maxSpeed = 30f;
    [SerializeField] [Tooltip("The amount of friction that the drag of the player generates.")] private float drag = 0.995f;
    [SerializeField] [Tooltip("The angle in which the player can steer.")] private float steerAngle = 20f;
    [SerializeField] [Tooltip("The amount of traction of the player.")] private float traction = 1f;

    [Header("Drifting Mechanic Variables")]
    [SerializeField] private GameObject driftEffect;
    [SerializeField] private float driftSteerMultiplier = 1.5f;
    [SerializeField] private float boostForce = 20f;

    [Header("Magical Trampoline Mechanic Variable")]
    [SerializeField] private Slider jumpCooldownBar;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpDuration = 0.6f;

    [Header("Pause Logic")]
    [SerializeField] private PauseManager pauseManager;

    private Vector3 moveForce; //The actual velocity-like force applied to the rigidbody
    private float yAxis;
    private Rigidbody rb;

    //Drifting Mechanic
    private bool isDrifting = false;
    private float driftTimer = 0f;
    private float driftScoreTimer;
    private float driftThreshold = 0.3f;
    private float driftScoredDuration;

    //Trampoline Mechanic
    private bool isJumping = false;
    private float nextJumpTime = 0f;
    private float jumpCooldown = 30f;
    private bool isCooldownAcitve = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        //Drifting Mechanic
        HandleDrift();

        // Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!isDrifting && !isJumping)
        {
            rb.velocity = transform.forward * flySpeed;

            yAxis += -horizontalInput * yAmount * Time.deltaTime;

            float xAxis = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput); // Pitch
            float zAxis = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput); // Roll

            Quaternion targetRotation = Quaternion.Euler(-xAxis, -yAxis, zAxis);
            rb.MoveRotation(targetRotation);
        }
        else if (isDrifting && !isJumping)
        {
            // Adds Forward Force/Movement
            moveForce += transform.forward * flySpeed * Time.fixedDeltaTime;

            // Apply the Drag and Limit Speed
            moveForce *= drag;

            // Apply the Steering
            float steerAmount = -horizontalInput * moveForce.magnitude * steerAngle * Time.fixedDeltaTime;
            if (isDrifting) steerAmount *= driftSteerMultiplier;
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
        }

        // Accelerate/Break
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Acceleration();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Breaking();
        }

    }

    private void Update()
    {
        // Magical Trampoline
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && Time.time >= nextJumpTime)
        {
            ScoreManager.instance.PointsForScore(1000);
            nextJumpTime = Time.time + jumpCooldown;
            isCooldownAcitve = true;
            jumpCooldownBar.gameObject.SetActive(true);
            StartCoroutine(PerformJump());
        }

        if (isCooldownAcitve)
        {
            float remaining = nextJumpTime - Time.time;
            jumpCooldownBar.value = 1f - (remaining / jumpCooldown);

            if (remaining <= 0f)
            {
                isCooldownAcitve = false;
                jumpCooldownBar.gameObject.SetActive(false);
            }
        }

        // ESC Screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseManager.PauseGame();
        }
    }

    private IEnumerator PerformJump()
    {
        isJumping = true;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Transform model = transform.GetChild(0);
        model.DOLocalRotate(model.localEulerAngles + new Vector3(0, 360, 0), jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(jumpDuration);

        isJumping = false;
    }

    private void HandleDrift()
    {
        bool isTurning = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
        bool ctrlHeld = Input.GetKey(KeyCode.LeftControl);

        // Start Drifting
        if (isTurning && ctrlHeld && !isDrifting)
        {
            isDrifting = true;
            driftTimer = 0f;
            driftScoreTimer = 0f;
            driftScoredDuration = 0f;
            driftEffect.SetActive(true);
        }

        // While Drifting
        if (isDrifting && ctrlHeld)
        {
            driftTimer += Time.fixedDeltaTime;
            driftScoreTimer += Time.fixedDeltaTime;

            //Every 0.1 seconds it gives 50 points with a limit
            if (driftTimer >= 0.1f && driftScoredDuration < 1.0f)
            {
                ScoreManager.instance.PointsForScore(50);
                driftScoreTimer -= 0.1f;
                driftScoredDuration += 0.1f;
            }
        }

        // Stop Drifting
        if (isDrifting && !ctrlHeld)
        {
            isDrifting = false;

            if (driftTimer >= driftThreshold)
            {
                rb.velocity += transform.forward * boostForce;

                if (driftEffect != null)
                {
                    StartCoroutine(DisableBoostEffect());
                }
            }

            driftTimer = 0f;
        }
    }

    //Function for gradual addition of speed, with limit at 50, plus speed effect
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

    //Function to slowly substracts speed at a limit of 20, plus quit speed effect
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

    private IEnumerator DisableBoostEffect()
    {
        yield return new WaitForSeconds(1f);
        driftEffect.SetActive(false);
    }
}
