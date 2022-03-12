using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

// Thank you Brackeys and Comment section
public class Player : MonoBehaviour
{
    public float horizontalAimingSpeed = 20f;
    public float verticalAimingSpeed = 20f;

    [Tooltip("This depends on your Free Look rigs setup, use to correct Y sensitivity,"
        + " about 1.5 - 2 results in good Y-X square responsiveness")]
    public float yCorrection = 2f;

    private float xAxisValue;
    private float yAxisValue;

    public CharacterController controller;
    public Transform cam;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public ParticleSystem dust;

    bool isFocused = false;

    void Start()
    {
        //Makes it invisable
        Cursor.visible = !isFocused;
        //Locks the mouse in place
        Cursor.lockState = CursorLockMode.Locked;
        // In unity press esc to unlock the mouse and unhide it
    }

    // Update is called once per frame
    void Update()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            if (!isGrounded)
            {
                dust.Stop();
            }
            else if (!dust.isPlaying)
            {
                dust.Play();
            }
        }
        else
        {
            dust.Stop();
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 2f * speed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speed / 2f;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }


    }
}