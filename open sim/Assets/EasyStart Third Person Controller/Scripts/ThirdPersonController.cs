
using UnityEditor.VersionControl;
using UnityEngine;
public class ThirdPersonController : MonoBehaviour
{
    [Tooltip("Speed at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 0.5f;  // Decreased speed
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 1.2f;  // Decreased sprint speed boost
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 1.5f;  // Decreased jump force
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;

    // Player states
    bool isJumping = false;
    bool isSprinting = false;
    bool isCrouching = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    Animator animator;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning("Hey buddy, you don't have the Animator component in your player. Without it, the animations won't work.");
    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        if (inputCrouch)
            isCrouching = !isCrouching;

        if (cc.isGrounded && animator != null)
        {
            animator.SetBool("crouch", isCrouching);
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);

            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            animator.SetBool("sprint", isSprinting);
        }

        if (animator != null)
            animator.SetBool("air", !cc.isGrounded);

        if (inputJump && cc.isGrounded)
        {
            isJumping = true;
        }

        HeadHittingDetect();
    }

    private void FixedUpdate()
    {
        // Sprinting velocity boost or crouching decelerate
        float velocityAdittion = 0;
        if (isSprinting)
            velocityAdittion = sprintAdittion;
        if (isCrouching)
            velocityAdittion = -(velocity * 0.50f);

        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        // Jump handler
        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        // Add gravity to Y axis
        directionY -= gravity * Time.deltaTime;

        // Rotate the character based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * 5f, 0);

        // Move the character based on keyboard input
        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = transform.forward * directionZ + transform.right * directionX;

        Vector3 movement = verticalDirection + horizontalDirection;
        cc.Move(movement);
    }

    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }
}

