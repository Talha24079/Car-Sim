using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManController : MonoBehaviour
{
    public float moveSpeed = 2f;         // Movement speed
    public float rotationSpeed = 150f;  // Rotation speed
    public float jumpHeight = 0.11f;       // Jump height
    public float gravity = -9.81f;      // Gravity value

    private CharacterController controller;
    private Vector3 velocity;           // For storing velocity, including gravity
    private bool isGrounded;

    void Start()
    {
        // Get the CharacterController component attached to the model
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset velocity when on the ground
        }

        // Movement input
        float horizontal = Input.GetAxis("Horizontal"); // Left/Right movement
        float vertical = Input.GetAxis("Vertical");     // Forward/Backward movement

        // Combine inputs for movement direction
        Vector3 move = transform.forward * vertical + transform.right * horizontal;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Rotation input (rotate with "A" and "D" keys or left/right arrows)
        if (horizontal != 0)
        {
            transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        }

        // Jump input (space key)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate jump velocity
        }

        // Apply gravity to the velocity
        velocity.y += gravity * Time.deltaTime;

        // Move the character based on gravity
        controller.Move(velocity * Time.deltaTime);
    }
}
