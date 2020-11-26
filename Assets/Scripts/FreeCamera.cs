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
    currentRotation = new Vector2(rotation.eulerAngles.x, rotation.eulerAngles.y);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.M))
    {
      Cursor.lockState = isCameraRotating ? CursorLockMode.None : CursorLockMode.Locked;
      isCameraRotating = !isCameraRotating;
    }

    if (!isCameraRotating) return;
    var transform1 = transform;
    var forward = transform1.forward;
    var right = transform1.right;
    Vector3 moveInput = new Vector3(forward.x,0,forward.z) * Input.GetAxisRaw("Vertical") + new Vector3(right.x,0,right.z) * Input.GetAxisRaw("Horizontal") + Vector3.up * Input.GetAxisRaw("Elevation");
    transform.position += moveInput * (Time.deltaTime * (Input.GetKey("left shift") ? fastFlySpeed : flySpeed));
    currentRotation.x += Input.GetAxis("Mouse X") * cameraSensitivity;
    currentRotation.y -= Input.GetAxis("Mouse Y") * cameraSensitivity;
    currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
    transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
  }
}