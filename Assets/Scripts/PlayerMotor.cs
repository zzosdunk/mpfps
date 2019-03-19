using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    private Rigidbody rb;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    //Updates every physics frame
    void FixedUpdate()
    {
        PerfomMoving();
        PerformRotation();
    }

    //Perform rotation
    public void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            //Set our rotation and clapm it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply our rotation to the transform of camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    //Perform movement based on velocity variable
    void PerfomMoving()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    //Gets a rotational vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    //Gets a force vector for our thrusters
    public void ApplyThruster(Vector3 _thursterForce)
    {
        thrusterForce = _thursterForce;
    }
    public void CameraRotate(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }
}