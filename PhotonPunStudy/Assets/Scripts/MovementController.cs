using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float m_speed = 8;
    [SerializeField] private float m_lookSensitivity = 3f;
    [SerializeField] private Transform m_fpsCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float m_cameraUpDownRotation = 0f;
    private float m_curCameraUpAndDownRotation = 0;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Vector3 inputAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 movimentHorizontal = transform.right * xMovement;
        Vector3 movimentVertical = transform.forward * zMovement;

        Vector3 movementVelocity = (movimentHorizontal + movimentVertical).normalized * m_speed;

        Move(movementVelocity);

        //=====================================

        float yRotation = Input.GetAxis("Mouse X");
        Vector3 rotationVector = new Vector3(0, yRotation, 0) * m_lookSensitivity;

        Rotate(rotationVector);

        //=====================================

        float cameraUpDownRotation = Input.GetAxis("Mouse Y") * m_lookSensitivity;

        RotateCamera(cameraUpDownRotation);

    }

    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (m_fpsCamera != null)
        {
            m_curCameraUpAndDownRotation -= m_cameraUpDownRotation;
            m_curCameraUpAndDownRotation = Mathf.Clamp(m_curCameraUpAndDownRotation, -85, 85);

            m_fpsCamera.transform.localEulerAngles = new Vector3(m_curCameraUpAndDownRotation, 0, 0);
        }
    }

    private void Move(Vector3 movementVelocity)
    {
        velocity = movementVelocity;
    }

    private void Rotate(Vector3 rotationVector) 
    {
        rotation = rotationVector;
    }

    private void RotateCamera(float cameraUPDownRotation) 
    {
        m_cameraUpDownRotation = cameraUPDownRotation;
    }
}
