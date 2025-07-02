using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] [Tooltip("The speed in which the player flies.")] private float flySpeed;
    [SerializeField] [Tooltip("The speed in which the player is able to turn.")] private float yAmount;

    private float yAxis;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * flySpeed * Time.deltaTime;

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

    private void Acceleration()
    {
        //Gradual addition of speed, with limit at 30, plus effect
        flySpeed += 5f;
    }

    private void Breaking()
    {
        //Makes sudden stop, slowly substracts speed at limit of 10, plus effect
        flySpeed -= 5f;
    }
}
