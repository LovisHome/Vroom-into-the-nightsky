using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] [Tooltip("The speed in which the player flies.")] private float flySpeed;
    [SerializeField] [Tooltip("The speed in which the player is able to turn.")] private float yAmount;
    public CinemachineVirtualCamera virtualCamera;

    private float yAxis;

    private void Update()
    {
        transform.position += transform.forward * flySpeed * Time.deltaTime;

        //Rotation on Y Axis to get TurnMovement or Left and Right Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        yAxis += horizontalInput * yAmount * Time.deltaTime;
        float xAxis = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float zAxis = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        transform.localRotation = Quaternion.Euler(Vector3.up * yAxis + Vector3.left * xAxis + Vector3.forward * zAxis);

    }
}
