using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float flySpeed = 20;
    public float fastFlySpeed = 30;
    public float cameraSensitivity = 1;
    private Vector2 currentRotation;
    private bool isCameraRotating;

    private void Start()
    {
        var rotation = transform.rotation;
        currentRotation = new Vector2(rotation.eulerAngles.x,rotation.eulerAngles.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            isCameraRotating = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.lockState = CursorLockMode.None;
            isCameraRotating = false;
        }
        
        if (!isCameraRotating) return;
        Vector3 moveInput = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal") + transform.up * Input.GetAxisRaw("Elevation");
        transform.position += moveInput * (Time.deltaTime * (Input.GetKey("left shift") ? fastFlySpeed : flySpeed));
        currentRotation.x += Input.GetAxis("Mouse X") * cameraSensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * cameraSensitivity;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        transform.rotation = Quaternion.Euler(currentRotation.y,currentRotation.x,0);

    }
}
